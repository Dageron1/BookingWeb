using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
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


        public void CreateVilla(Villa villa, List<IFormFile> files)
        {
            if (files != null)
            {
                foreach (IFormFile file in files)
                {
                    //we can retrieve the extension of the image that was uploaded, так же можно на этом этапе добавить валидацию изображения.
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string villaPath = @"images\villas\villa-" + villa.Id;
                    string finalPath = Path.Combine(_webHostEnvironment.WebRootPath, villaPath);

                    var fileFull = Path.Combine(finalPath, fileName);

                    if (!Directory.Exists(finalPath))
                        Directory.CreateDirectory(finalPath);

                    using (var fileStream = new FileStream(fileFull, FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }

                    VillaImages villaImages = new()
                    {
                        ImageUrl = @"\" + villaPath + @"\" + fileName,
                        VillaId = villa.Id,
                    };

                    if (villa.VillaImages == null)
                        //избегаем исключения, если картинка не выбрана. Инициализируя
                        villa.VillaImages = new List<VillaImages>();

                    villa.VillaImages.Add(villaImages);
                }
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
                    string villaPath = @"images\villas\villa-" + id;
                    string finalPath = Path.Combine(_webHostEnvironment.WebRootPath, villaPath);

                    if (Directory.Exists(finalPath))
                    {
                        string[] filePaths = Directory.GetFiles(finalPath);
                        foreach (string filePath in filePaths)
                        {
                            System.IO.File.Delete(filePath);
                        }

                        Directory.Delete(finalPath);
                    }

                    _unitOfWork.Villa.Remove(objFromDb);
                    _unitOfWork.Save();
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        public bool DeleteVillaImage(int imageId, int villaId)
        {
            try
            {
                var villaImages = _unitOfWork.Villa.Get(x => x.Id == villaId, includeProperties: "VillaImages").VillaImages;
                var imageToDelete = villaImages.FirstOrDefault(x => x.Id == imageId);

                if (imageToDelete != null)
                {
                    var oldImagePath =
                                   Path.Combine(_webHostEnvironment.WebRootPath,
                                   imageToDelete.ImageUrl.TrimStart('\\'));

                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }

                    _unitOfWork.VillaImage.Remove(imageToDelete);
                    _unitOfWork.Save();
                }
                return true;    
            }
            catch (Exception)
            {
                return false;
            }
        }
            
        

        public IEnumerable<Villa> GetAllVillas()
        {
            return _unitOfWork.Villa.GetAll(includeProperties: "VillaAmenity,VillaImages,Facilities");
        }

        public Villa GetVillaById(int id)
        {
            return _unitOfWork.Villa.Get(u => u.Id == id, includeProperties: "VillaAmenity,VillaImages,Facilities");
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

            var villaList = _unitOfWork.Villa.GetAll(includeProperties: "Facilities,VillaImages");
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

        public void UpdateVilla(Villa villa, List<IFormFile> files, int[] selectFacilitiesIds)
        {
            var villaFromDb = _unitOfWork.Villa.Get(x => x.Id == villa.Id, includeProperties: "Facilities", tracked: true);

            var facilitiesFromDb = _unitOfWork.Facility.GetAll();

            villaFromDb.Facilities.RemoveAll(x => !selectFacilitiesIds.Contains(x.Id)); //Delete choosen!!! facility if facilitiesIds not contains any villaFromDb.Facilities.Id

            //compares the first collection with the second and removes from the first array the data that is in the second array
            //then take all facilities based on facilities id.
            var newFacilities = selectFacilitiesIds.Except(villaFromDb.Facilities.Select(x => x.Id)).Select(u => facilitiesFromDb.FirstOrDefault(x => x.Id == u));

            villaFromDb.Facilities.AddRange(newFacilities);

            villaFromDb.Description = villa.Description;
            villaFromDb.Name = villa.Name;
            villaFromDb.Occupancy = villa.Occupancy;
            villaFromDb.Sqft = villa.Sqft;
            villaFromDb.Price = villa.Price;

            if (files != null)
            {
                foreach (IFormFile file in files)
                {
                    //we can retrieve the extension of the image that was uploaded, так же можно на этом этапе добавить валидацию изображения.
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string villaPath = @"images\villas\villa-" + villa.Id;
                    string finalPath = Path.Combine(_webHostEnvironment.WebRootPath, villaPath);

                    var fileFull = Path.Combine(finalPath, fileName);

                    if (!Directory.Exists(finalPath))
                        Directory.CreateDirectory(finalPath);

                    using (var fileStream = new FileStream(fileFull, FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }

                    VillaImages villaImages = new()
                    {
                        ImageUrl = @"\" + villaPath + @"\" + fileName,
                        VillaId = villa.Id,
                    };

                    if (villa.VillaImages == null)
                        //избегаем исключения, если картинка не выбрана. Инициализируя
                        villaFromDb.VillaImages = new List<VillaImages>();


                    villaFromDb.VillaImages.Add(villaImages);
                   
                }
            }
            _unitOfWork.Villa.Update(villaFromDb);
            _unitOfWork.Save();
        }

    }
}
