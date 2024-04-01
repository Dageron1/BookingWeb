using Microsoft.AspNetCore.Mvc;
using Syncfusion.EJ2.Calendars;
using Syncfusion.Presentation;
using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Numerics;
using VillaProject.Application.Common.Interfaces;
using VillaProject.Application.Common.Utility;
using VillaProject.Application.Services.Interface;
using VillaProject.Domain.Entities;
using VillaProject.Web.Models;
using VillaProject.Web.Models.ViewModels;

namespace VillaProject.Web.Controllers
{
    public class DateRange
    {
        [Required(ErrorMessage = "Please enter the value")]
        public DateOnly[] value { get; set; }

    }
    public class HomeController : Controller
    {
        private readonly IVillaService _villaService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public HomeController(IVillaService villaService, IWebHostEnvironment webHostEnvironment)
        {
            _villaService = villaService;
            _webHostEnvironment = webHostEnvironment;
        }
        DateRange DateRangeValue = new DateRange();

        public IActionResult Index()
        {
            var min = DateOnly.FromDateTime(DateTime.Now);
            var max = min.AddMonths(1);

            DateRangeValue.value = new DateOnly[] { min, max };
            HomeVM homeVM = new()
            {
                VillaList = _villaService.GetAllVillas(),
                Nights = 1,
                CheckInDate = DateOnly.FromDateTime(DateTime.Now),
                dateRange = null,
            };

            return View(homeVM);
        }
        [HttpPost]
        public IActionResult GetVillasByDate(DateRange model) //here I want to somehow get data from POST Ajax
        {
            if (model.value.Length != 2)
            {
                DateRangeValue.value = model.value;

                var stratDate = DateOnly.FromDateTime(DateTime.Now);
                var endDate = DateOnly.FromDateTime(DateTime.Now).AddDays(1);
                var nightsCalc = endDate.DayNumber - stratDate.DayNumber + 1;
                HomeVM homeVM = new()
                {
                    CheckInDate = stratDate,
                    VillaList = _villaService.GetVillasAvailabilityByDate(nightsCalc, stratDate),
                    Nights = nightsCalc,
                };
                return PartialView("_VillaList", homeVM);
            }
            else
            {
                DateRangeValue.value = model.value;

                var stratDate = model.value[0];
                var endDate = model.value[1];
                var nightsCalc = endDate.DayNumber - stratDate.DayNumber + 1;
                HomeVM homeVM = new()
                {
                    CheckInDate = stratDate,
                    VillaList = _villaService.GetVillasAvailabilityByDate(nightsCalc, stratDate),
                    Nights = nightsCalc,
                };
                return PartialView("_VillaList", homeVM);
            }
            

            
           
        }

        [HttpPost]
        public IActionResult GeneratePPTExprot(int id)
        {
            var villa = _villaService.GetVillaById(id);
            if (villa is null)
            {
                return RedirectToAction(nameof(Error));
            }
            string basePath = _webHostEnvironment.WebRootPath;
            string filePath = basePath + @"/Exports/ExportVillaDetails.pptx";

            using IPresentation presentation = Presentation.Open(filePath);
            ISlide slide = presentation.Slides[0];
            //shape это все элементы в Powerpoint со своими личными id | потом конвертируем в Ishape и записываем в переменную
            IShape? shape = slide.Shapes.FirstOrDefault(u => u.ShapeName == "txtVillaName") as IShape;
            if (shape is not null)
            {
                shape.TextBody.Text = villa.Name;
            }
            shape = slide.Shapes.FirstOrDefault(u => u.ShapeName == "txtVillaDescription") as IShape;
            if (shape is not null)
            {
                shape.TextBody.Text = villa.Description;
            }
            shape = slide.Shapes.FirstOrDefault(u => u.ShapeName == "txtOccupancy") as IShape;
            if (shape is not null)
            {
                shape.TextBody.Text = string.Format("Max Occupancy: {0} adults", villa.Occupancy);
            }
            shape = slide.Shapes.FirstOrDefault(u => u.ShapeName == "txtVillaSize") as IShape;
            if (shape is not null)
            {
                shape.TextBody.Text = string.Format("Villa Size: {0} sqft", villa.Sqft);
            }
            shape = slide.Shapes.FirstOrDefault(u => u.ShapeName == "txtPricePerNight") as IShape;
            if (shape is not null)
            {
                shape.TextBody.Text = string.Format("USD {0}/night", villa.Price.ToString("c"));
            }

            shape = slide.Shapes.FirstOrDefault(u => u.ShapeName == "txtVillaAmenitiesHeading") as IShape;
            if (shape is not null)
            {
                List<string> listItems = villa.VillaAmenity.Select(x => x.Name).ToList();

                shape.TextBody.Text = "";

                foreach (var item in listItems)
                {
                    IParagraph paragraph = shape.TextBody.AddParagraph();
                    ITextPart textPart = paragraph.AddTextPart(item);

                    paragraph.ListFormat.Type = ListType.Bulleted;
                    paragraph.ListFormat.BulletCharacter = '\u2022';
                    textPart.Font.FontName = "system-ui";
                    textPart.Font.FontSize = 18;
                    textPart.Font.Color = ColorObject.FromArgb(144, 148, 152);
                }
            }

            shape = slide.Shapes.FirstOrDefault(u => u.ShapeName == "imgVilla") as IShape;
            if (shape is not null)
            {
                byte[] imageData;
                string imageUrl;
                try
                {
                    imageUrl = string.Format("{0}{1}", basePath, villa.VillaImages.FirstOrDefault());
                    imageData = System.IO.File.ReadAllBytes(imageUrl);
                }
                catch (Exception)
                {
                    imageUrl = string.Format("{0}{1}", basePath, "/images/placeholder.png");
                    imageData = System.IO.File.ReadAllBytes(imageUrl);
                }
                slide.Shapes.Remove(shape); //удал€ем старую картинку
                using MemoryStream imageStream = new(imageData);//записываем данные в пам€ть
                IPicture newPicture = slide.Pictures.AddPicture(imageStream, 60, 120, 300, 200);
            }

            MemoryStream memoryStream = new();
            presentation.Save(memoryStream);
            //чтобы сбросить в начало, а не продолжать с того момента где было записано прошлое число в €чейках пам€ти.
            memoryStream.Position = 0; //по дефолту тоже 0 стоит
            return File(memoryStream, "application/pptx", "villa.pptx");
        }

        public IActionResult Privacy()
        {
            return View();
        }


        public IActionResult Error()
        {
            return View();
        }
    }
}
