using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VillaProject.Application.Common.Interfaces;
using VillaProject.Application.Common.Utility;
using VillaProject.Application.Services.Interface;
using VillaProject.Domain.Entities;


namespace VillaProject.Application.Services.Implementation
{
    public class VillaService : IVillaService
    {
        //private readonly ApplicationDbContext _db;
        //private readonly IVillaRepository _villaRepo; больше не используем т к нет связанное реализации интерфейса и класса в program.cs
        //используем теперь pattern UnitOfWork
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public VillaService(/*ApplicationDbContext db*/ /*IVillaRepository villaRepo,*/ IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            //_villaRepo = villaRepo;
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }


        public void CreateVilla(Villa villa)
        {
            if (villa.Image != null)
            {
                //we can retrieve the extension of the image that was uploaded, так же можно на этом этапе добавить валидацию изображения.
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(villa.Image.FileName);
                string imagePath = Path.Combine(_webHostEnvironment.WebRootPath, @"images\VillaImage");

                using (var fileStream = new FileStream(Path.Combine(imagePath, fileName), FileMode.Create)) //на этом этапе оно создает "пустую белую картинку"
                {
                    villa.Image.CopyTo(fileStream); //на этом этапе уже заполняет картинку информацией
                }
                villa.ImageUrl = @"\images\VillaImage\" + fileName;
            }
            else
            {
                villa.ImageUrl = "https://placeholder.co/600x400";
            }


            _unitOfWork.Villa.Add(villa);
            _unitOfWork.Save();
        }

        public bool DeleteVilla(int id)
        {
            try
            {
                Villa? objFromDb = _unitOfWork.Villa.Get(u => u.Id == id);
                if (objFromDb is not null)
                {
                    if (!string.IsNullOrEmpty(objFromDb.ImageUrl))
                    {
                        var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, objFromDb.ImageUrl.TrimStart('\\'));

                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }
                    _unitOfWork.Villa.Remove(objFromDb);
                    _unitOfWork.Save();
                }
                return true;
            }
            catch(Exception)
            {
                return false;
            } 
        }

        public IEnumerable<Villa> GetAllVillas()
        {
            return _unitOfWork.Villa.GetAll(includeProperties: "VillaAmenity");
        }

        public Villa GetVillaById(int id)
        {
            return _unitOfWork.Villa.Get(u =>u.Id == id, includeProperties: "VillaAmenity");
        }

        public IEnumerable<Villa> GetVillasAvailabilityByDate(int nights, DateOnly checkInDate)
        {
            //var checkOutDate = checkInDate.AddDays(nights);

            //var bookings = _unitOfWork.Booking.GetAll(x => x.Status == SD.StatusApproved || x.Status == SD.StatusCheckedIn);
            //var rooms = _unitOfWork.VillaNumber.GetAll();
            //var query =
            //    from room in rooms
            //    join booking in bookings //занятые букинги
            //        on room.VillaId equals booking.VillaId into g
            //    from booking in g.DefaultIfEmpty()
            //    where booking == null
            //    //получаем свободные букинги
            //        || booking.CheckInDate >= checkOutDate
            //        || booking.CheckOutDate <= checkInDate
            //    select room.VillaId;

            //var villas = _unitOfWork.Villa.GetAll(includeProperties: "VillaAmenity").ToList();

            //var availableVillaIds = query.Distinct().ToArray(); //убираем повторяющиеся
            //villas.ForEach(x => x.IsAvaliable = availableVillaIds.Contains(x.Id));

            //var homeVM = new HomeVM()
            //{
            //    CheckInDate = checkInDate,
            //    VillaList = villas,
            //    Nights = nights,
            //};

            var villaList = _unitOfWork.Villa.GetAll(includeProperties: "VillaAmenity");
            var villaNumbersList = _unitOfWork.VillaNumber.GetAll().ToList();
            var bookedVillas = _unitOfWork.Booking.GetAll(u => u.Status == SD.StatusApproved || u.Status == SD.StatusCheckedIn).ToList();



            foreach (var villa in villaList)
            {
                int roomAvaliable = SD.VillaRoomsAvaliable_Count(villa.Id, villaNumbersList, checkInDate, nights, bookedVillas);
                villa.IsAvaliable = roomAvaliable > 0 ? true : false;
            }

            return villaList;
        }

        public bool IsVillaAvailableByDate(int villaId, int nights, DateOnly checkInDate)
        {
            var villaNumbersList = _unitOfWork.VillaNumber.GetAll().ToList();
            var bookedVillas = _unitOfWork.Booking.GetAll(u => u.Status == SD.StatusApproved || u.Status == SD.StatusCheckedIn).ToList();

            int roomsAvaliable = SD.VillaRoomsAvaliable_Count(villaId, villaNumbersList, checkInDate, nights, bookedVillas);

            return roomsAvaliable > 0;
        }

        public void UpdateVilla(Villa villa)
        {
            if (villa.Image != null)
            {
                //we can retrieve the extension of the image that was uploaded, так же можно на этом этапе добавить валидацию изображения.
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(villa.Image.FileName);
                string imagePath = Path.Combine(_webHostEnvironment.WebRootPath, @"images\VillaImage");

                if (!string.IsNullOrEmpty(villa.ImageUrl))
                {
                    var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, villa.ImageUrl.TrimStart('\\'));

                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                using (var fileStream = new FileStream(Path.Combine(imagePath, fileName), FileMode.Create)) //на этом этапе оно создает "пустую белую картинку"
                {
                    villa.Image.CopyTo(fileStream); //на этом этапе уже заполняет картинку информацией
                }
                villa.ImageUrl = @"\images\VillaImage\" + fileName;
            }
            _unitOfWork.Villa.Update(villa);
            _unitOfWork.Save();
        }
    }
}
