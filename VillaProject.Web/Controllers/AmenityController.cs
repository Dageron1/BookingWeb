using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VillaProject.Application.Common.Interfaces;
using VillaProject.Application.Common.Utility;
using VillaProject.Application.Services.Implementation;
using VillaProject.Application.Services.Interface;
using VillaProject.Domain.Entities;
using VillaProject.Infrastructure.Data;
using VillaProject.Web.Models.ViewModels;

namespace VillaProject.Web.Controllers
{
    [Authorize(Roles = SD.Role_Admin)]
    public class AmenityController : Controller
    {
        private readonly IAmenityService _amenityService;
        private readonly IVillaService _villaService;

        public AmenityController(IAmenityService amenityService, IVillaService villaService)
        {
            _amenityService = amenityService;
            _villaService = villaService;
        }
        public IActionResult Index()
        {
            var amenities = _amenityService.GetAllAmenities();
            return View(amenities);
        }
        public IActionResult Create()
        {
            AmenityVM amenityVM = new()
            {
                VillaList = _villaService.GetAllVillas().Select(u => new SelectListItem
                {
                    Text = u.Name, //key
                    Value = u.Id.ToString()
                })
            };
            return View(amenityVM);
        }                    
        [HttpPost]
        public IActionResult Create(AmenityVM obj)
        {   

            if (ModelState.IsValid)
            {
                _amenityService.CreateAmenity(obj.Amenity);
                TempData["success"] = "The amenity has been created successfully.";
                return RedirectToAction("Index");
            }
           
            obj.VillaList = _villaService.GetAllVillas().Select(u => new SelectListItem
            {
                Text = u.Name, //key
                Value = u.Id.ToString()
            });

            return View(obj);
        }
        public IActionResult Update(int amenityId)
        {

            AmenityVM amenityVM = new()
            {
                VillaList = _villaService.GetAllVillas().Select(u => new SelectListItem
                {
                    Text = u.Name, //key
                    Value = u.Id.ToString()
                }),
                Amenity = _amenityService.GetAmenityById(amenityId)
            };
            if (amenityVM.Amenity == null)
            {
                return RedirectToAction("Error","Home");
            }
            return View(amenityVM);
        }
        [HttpPost]
        public IActionResult Update(AmenityVM amenityVM)
        {
            if (ModelState.IsValid)
            {
                //специально указал на ошибку. ЕФ начнет отслеживать 2 объекта с одним id  и будет ошибка
                //ef не может трекать 2 объекта с одинаково key-value pair
                //Amenity amenity = _unitOfWork.Amenity.Get(u => u.Id == amenityVM.Amenity.Id);
                //if (amenity.Name.ToLower().Contains("special"))
                //{
                //    amenityVM.Amenity.Description = amenity.Description;
                //}
                //else
                //{
                //    _unitOfWork.Amenity.Update(amenityVM.Amenity);
                //}
                _amenityService.UpdateAmenity(amenityVM.Amenity);
                TempData["success"] = "The amenity has been created successfully.";
                return RedirectToAction(nameof(Index));
            }
            amenityVM.VillaList = _villaService.GetAllVillas().Select(u => new SelectListItem
            {
                Text = u.Name, //key
                Value = u.Id.ToString()
            });

            return View(amenityVM);
        }
        public IActionResult Delete(int amenityId)
        {

            AmenityVM amenityVM = new()
            {
                VillaList = _villaService.GetAllVillas().Select(u => new SelectListItem
                {
                    Text = u.Name, //key
                    Value = u.Id.ToString()
                }),
                Amenity = _amenityService.GetAmenityById(amenityId)
            };
            if (amenityVM.Amenity == null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(amenityVM);
        }
        [HttpPost]
        public IActionResult Delete(AmenityVM amenityVM)
        {
            Amenity? objFromDb = _amenityService.GetAmenityById(amenityVM.Amenity.Id);
            if (objFromDb is not null)
            {
                _amenityService.DeleteAmenity(objFromDb.Id);
                TempData["success"] = "The amenity has been deleted successfully.";
                return RedirectToAction(nameof(Index));
            }
            TempData["error"] = "The amenity could not be deleted.";
            return View();
        }
    }
}
