using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Stripe.Checkout;
using Syncfusion.DocIO;
using Syncfusion.DocIO.DLS;
using Syncfusion.DocIORenderer;
using Syncfusion.Drawing;
using Syncfusion.Pdf;
using System.Security.Claims;
using VillaProject.Application.Common.Interfaces;
using VillaProject.Application.Common.Utility;
using VillaProject.Application.Services.Interface;
using VillaProject.Domain.Entities;

namespace VillaProject.Web.Controllers
{
    public class BookingController : Controller
    {
        private readonly IBookingService _bookingService;
        private readonly IVillaService _villaService;
        private readonly IVillaNubmberService _villaNubmberService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly UserManager<ApplicationUser> _userManager;

        public BookingController(IBookingService bookingService, IVillaService villaService, 
            IVillaNubmberService villaNubmberService,IWebHostEnvironment webHostEnvironment, UserManager<ApplicationUser> userManager)
        {
            _bookingService = bookingService;
            _villaService = villaService;
            _villaNubmberService = villaNubmberService;
            _webHostEnvironment = webHostEnvironment;
            _userManager = userManager;
        }
        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult FinalizeBooking(int villaId, DateOnly checkInDate, int nights)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            ApplicationUser user = _userManager.FindByIdAsync(userId).GetAwaiter().GetResult(); //Обращение к AspNetUsers в таблице sql

            Booking booking = new()
            {
                Villa = _villaService.GetVillaById(villaId),
                VillaId = villaId,
                CheckInDate = checkInDate,
                Nights = nights,
                CheckOutDate = checkInDate.AddDays(nights),
                UserId = userId,
                Phone = user.PhoneNumber,
                Email = user.Email,
                Name = user.UserName
            };
            booking.TotalCost = booking.Villa.Price * nights;

            return View(booking);
        }
        [Authorize]
        [HttpPost]
        public IActionResult FinalizeBooking(Booking booking)
        {
            var villa = _villaService.GetVillaById(booking.VillaId);
            booking.TotalCost = villa.Price * booking.Nights;

            booking.Status = SD.StatusPending;
            booking.BookingDate = DateTime.Now;


           
            
            if(!_villaService.IsVillaAvailableByDate(villa.Id, booking.Nights, booking.CheckInDate))
            {
                TempData["error"] = "Room has been sold out!";
                //no rooms avaliable
                return RedirectToAction(nameof(FinalizeBooking), new
                {
                    villaId = booking.VillaId,
                    checkInDate = booking.CheckInDate,
                    nights = booking.Nights
                });
            }
          
            _bookingService.CreateBooking(booking);

            var domain = Request.Scheme+"://"+Request.Host.Value+"/";
            var options = new SessionCreateOptions
            {
                LineItems = new List<SessionLineItemOptions>(), //set = empty
                Mode = "payment",
                SuccessUrl = domain + $"booking/BookingConfirmation?bookingId={booking.Id}", //booking.Id берется когда он сохраняется в бд, сразу же получаем id
                CancelUrl = domain + $"booking/FinalizeBooking?villaId={booking.VillaId}&checkInDate={booking.CheckInDate}&nights={booking.Nights}",
            };

            options.LineItems.Add(new SessionLineItemOptions
            {
                PriceData = new SessionLineItemPriceDataOptions
                {
                    //UnitAmount in cents/ decimal not allowed here/ умножаем на 100 и конвертируем в лонг.
                    UnitAmount = (long)(booking.TotalCost * 100),
                    Currency = "usd",
                    ProductData = new SessionLineItemPriceDataProductDataOptions
                    {
                        Name = villa.Name,
                        Description = villa.Description
                        //url will not be accessible from Stripe because the image is in localhost
                        //Images = new List<string> { domain + villa.ImageUrl},
                    },
                },
                Quantity = 1,
            });
               //на этом этапе делаем запрос на создание сессии с выбранными настройками.
            var service = new SessionService();
            //сессия создается и помещается в переменную session
            //это Nuget class from stripe
            Session session = service.Create(options);
            

            _bookingService.UpdateStripePaymentID(booking.Id, session.Id, session.PaymentIntentId);

            //перенаправляем пользователя по ссылке на оплату
            Response.Headers.Add("Location", session.Url);
            return new StatusCodeResult(303);


            //booking.Id получает свой Id сразу же как объект сохраняется в БД и дальше мы его передаем дальше.
            //return RedirectToAction(nameof(BookingConfirmation), new { bookingId = booking.Id });
        }

        [Authorize]
        public IActionResult BookingConfirmation(int bookingId)
        {
            Booking bookingFromDb = _bookingService.GetBookingById(bookingId);

            if(bookingFromDb.Status == SD.StatusPending)
            {
                //this is a pending order, we need to confirm if payment was successful

                var service = new SessionService();
                //делаем запрос передавая ид совершенной сессии и получаем PaymentIntentId который нужен если нужно будет сделать refund т е возврат денег.
                Session session = service.Get(bookingFromDb.StripeSessionId);

                if(session.PaymentStatus == "paid")
                {
                    _bookingService.UpdateStatus(bookingFromDb.Id, SD.StatusApproved, 0);
                    _bookingService.UpdateStripePaymentID(bookingFromDb.Id, session.Id, session.PaymentIntentId);
                }
            }

            return View(bookingId);
        }

        [Authorize]
        public IActionResult BookingDetails(int bookingId)
        {
            Booking bookingFromDb = _bookingService.GetBookingById(bookingId);

            //checkIn logic
            if(bookingFromDb.VillaNumber == 0 && bookingFromDb.Status == SD.StatusApproved)
            {
                var availableVillaNumber = AssignAvaliableVillaNumberByVilla(bookingFromDb.VillaId);

                bookingFromDb.VillaNumbers = _villaNubmberService.GetAllVillaNumbers().Where(u => u.VillaId == bookingFromDb.VillaId
                && availableVillaNumber.Any(x => x == u.Villa_Number)).ToList();
            }

            return View(bookingFromDb);
        }

        [HttpPost]
        [Authorize]
        public IActionResult GenerateInvoice(int id, string downloadType)
        {
            string basePath = _webHostEnvironment.WebRootPath;

            WordDocument document = new WordDocument();
            //load the template
            string dataPath = basePath + @"/exports/BookingDetails.docx";
            //The FileShare has nothing to do with drives shared over a network - it indicates how other processes can access the file.
            //FileShare — перечисление, определяющее способ совместного использования файла:
            //ReadWrite Разрешает последующее открытие файла для чтения или записи.
            using FileStream fileStream = new(dataPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            document.Open(fileStream, FormatType.Automatic);

            //Update Template
            Booking bookingFromDb = _bookingService.GetBookingById(id);

            TextSelection textSelection = document.Find("xx_customer_name", false, true);
            WTextRange textRange = textSelection.GetAsOneRange();
            textRange.Text = bookingFromDb.Name;

            textSelection = document.Find("xx_customer_phone", false, true);
            textRange = textSelection.GetAsOneRange();
            textRange.Text = bookingFromDb.Phone;

            textSelection = document.Find("xx_customer_email", false, true);
            textRange = textSelection.GetAsOneRange();
            textRange.Text = bookingFromDb.Email;

            textSelection = document.Find("XX_BOOKING_NUMBER", false, true);
            textRange = textSelection.GetAsOneRange();
            textRange.Text = "BOOKING ID - " + bookingFromDb.Id;

            textSelection = document.Find("XX_BOOKING_DATE", false, true);
            textRange = textSelection.GetAsOneRange();
            textRange.Text = "BOOKING DATE - " + bookingFromDb.BookingDate.ToShortDateString();

            textSelection = document.Find("xx_payment_date", false, true);
            textRange = textSelection.GetAsOneRange();
            textRange.Text = bookingFromDb.PaymentDate.ToShortDateString();

            textSelection = document.Find("xx_checkin_date", false, true);
            textRange = textSelection.GetAsOneRange();
            textRange.Text = bookingFromDb.CheckInDate.ToShortDateString();

            textSelection = document.Find("xx_checkout_date", false, true);
            textRange = textSelection.GetAsOneRange();
            textRange.Text = bookingFromDb.CheckOutDate.ToShortDateString();

            textSelection = document.Find("xx_booking_total", false, true);
            textRange = textSelection.GetAsOneRange();
            textRange.Text = bookingFromDb.TotalCost.ToString("c");

            WTable table = new(document);

            table.TableFormat.Borders.LineWidth = 1f;
            table.TableFormat.Borders.Color = Color.Black;
            table.TableFormat.Paddings.Top = 7f;
            table.TableFormat.Paddings.Bottom = 7f;
            table.TableFormat.Borders.Horizontal.LineWidth = 1f;

            int rows = bookingFromDb.VillaNumber > 0 ? 3 : 2;
            table.ResetCells(rows, 4); //2 строки и 4 колонки

            WTableRow row0 = table.Rows[0];
            row0.Cells[0].AddParagraph().AppendText("NIGHTS");
            row0.Cells[0].Width = 80;
            row0.Cells[1].AddParagraph().AppendText("VILLA");
            row0.Cells[1].Width = 220;
            row0.Cells[2].AddParagraph().AppendText("PRICE PER NIGHT");
            row0.Cells[3].AddParagraph().AppendText("TOTAL");
            row0.Cells[3].Width = 80;

            WTableRow row1 = table.Rows[1];
            row1.Cells[0].AddParagraph().AppendText(bookingFromDb.Nights.ToString());
            row1.Cells[0].Width = 80;
            row1.Cells[1].AddParagraph().AppendText(bookingFromDb.Villa.Name);
            row1.Cells[1].Width = 220;
            row1.Cells[2].AddParagraph().AppendText((bookingFromDb.TotalCost/bookingFromDb.Nights).ToString("c"));
            row1.Cells[3].AddParagraph().AppendText(bookingFromDb.TotalCost.ToString("c"));
            row1.Cells[3].Width = 80;

            if(bookingFromDb.VillaNumber > 0)
            {
                WTableRow row2 = table.Rows[2];
                row2.Cells[0].Width = 80;
                row2.Cells[1].AddParagraph().AppendText("Villa Number - " + bookingFromDb.VillaNumber.ToString());
                row2.Cells[1].Width = 220;
                row2.Cells[3].Width = 80;
            }

            WTableStyle tableStyle = document.AddTableStyle("CustomStyle") as WTableStyle;
            tableStyle.TableProperties.RowStripe = 1;
            tableStyle.TableProperties.ColumnStripe = 2;
            tableStyle.TableProperties.Paddings.Top = 2;
            tableStyle.TableProperties.Paddings.Bottom = 1;
            tableStyle.TableProperties.Paddings.Left = 5.4f;
            tableStyle.TableProperties.Paddings.Right = 5.4f;

            //конкретно для FirstRow стайл делаем
            ConditionalFormattingStyle firstRowStyle = tableStyle.ConditionalFormattingStyles.Add(ConditionalFormattingType.FirstRow);
            firstRowStyle.CharacterFormat.Bold = true;
            firstRowStyle.CharacterFormat.TextColor = Color.FromArgb(255, 255, 255, 255);
            firstRowStyle.CellProperties.BackColor = Color.Gray;

            table.ApplyStyle("CustomStyle");


            TextBodyPart bodyPart = new(document); //указываем что будем использовать часть документа
            bodyPart.BodyItems.Add(table); //добавляем таблицу в документ
            document.Replace("<ADDTABLEHERE>", bodyPart, false, false);

            using DocIORenderer renderer = new();
            MemoryStream stream = new();
            if (downloadType == "word")
            {
                document.Save(stream, FormatType.Docx);
                stream.Position = 0;

                return File(stream, "application/docx", "BookingDetails.docx");
            }
            else
            {
                PdfDocument pdfDocument = renderer.ConvertToPDF(document);

                pdfDocument.Save(stream);
                //Connection id "0HN16CVEH029R", Request id "0HN16CVEH029R:00000045": An unhandled exception was thrown by the application.
                //System.InvalidOperationException: Response Content-Length mismatch: too few bytes written(0 of 12721).
                //если не указать позицию, то чтение будет с позиции где остановилась запись файла в байтах.
                //Допустим остановилось на 37 из 100. Если не сбросить будет чтение с 37 и далее а не с 0
                stream.Position = 0; 

                return File(stream, "application/pdf", "BookingDetails.pdf");
            }
           
        }

        [HttpPost]
        [Authorize(Roles = SD.Role_Admin)]
        public IActionResult CheckIn(Booking booking)
        {
            _bookingService.UpdateStatus(booking.Id, SD.StatusCheckedIn, booking.VillaNumber);

            TempData["Success"] = "Booking Updated Successfully.";
            return RedirectToAction(nameof(BookingDetails), new {bookingId = booking.Id});
        }

        [HttpPost]
        [Authorize(Roles = SD.Role_Admin)]
        public IActionResult CheckOut(Booking booking)
        {
            _bookingService.UpdateStatus(booking.Id, SD.StatusCompleted, booking.VillaNumber);

            TempData["Success"] = "Booking Completed Successfully.";
            return RedirectToAction(nameof(BookingDetails), new { bookingId = booking.Id });
        }

        [HttpPost]
        [Authorize(Roles = SD.Role_Admin)]
        public IActionResult CancelBooking(Booking booking)
        {
            _bookingService.UpdateStatus(booking.Id, SD.StatusCancelled, 0);

            TempData["Success"] = "Booking Cancelled Successfully.";
            return RedirectToAction(nameof(BookingDetails), new { bookingId = booking.Id });
        }

        private List<int> AssignAvaliableVillaNumberByVilla(int villaId)
        {
            List<int> availableVillaNumbers = new();

            var villaNumbers = _villaNubmberService.GetAllVillaNumbers().Where(u => u.VillaId == villaId);

            var checkedInVilla = _bookingService.GetCheckedInVillaNumbers(villaId);

            foreach(var villaNumber in villaNumbers) //переборка комнат
            {
                if(!checkedInVilla.Contains(villaNumber.Villa_Number))
                {
                    availableVillaNumbers.Add(villaNumber.Villa_Number);
                }
            }
            return availableVillaNumbers;
        }

        #region APi Calls
        [HttpGet]
        [Authorize]
        public IActionResult GetAll(string status)
        {
            IEnumerable<Booking> objBookings;
            //благодаря идентити можно легко использовать эту логику в приложении
            if (User.IsInRole(SD.Role_Admin))
            {
                objBookings = _bookingService.GetAllBookings(status);
            }
            else
            {
                //получаем в этих двх строках Id юзера
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

                objBookings = _bookingService.GetAllBookings(status, userId);
            }
            
            return Json(new {data = objBookings});
            
        }

        #endregion
    }
}
