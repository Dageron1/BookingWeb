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
using VillaProject.Infrastructure.Migrations;
using VillaProject.Infrastructure.Repository;
using VillaProject.Web.Models.ViewModels;
using Facility = VillaProject.Domain.Entities.Facility;

namespace VillaProject.Web.Controllers
{
    [Authorize(Roles = SD.Role_Admin)]
    public class AmenityController : Controller
    {
        private readonly IAmenityService _amenityService;
        private readonly IVillaService _villaService;
        private readonly IFacilityService _facilityService;

        public AmenityController(IAmenityService amenityService, IVillaService villaService, IFacilityService facilityService)
        {
            _amenityService = amenityService;
            _villaService = villaService;
            _facilityService = facilityService;
        }
        public IActionResult Index()
        {
            //var amenities = _amenityService.GetAllAmenities();
            var facilities = _facilityService.GetAll();
            return View(facilities);
        }
        public IActionResult Create()
        {
            AmenityVM amenityVM = new()
            {
                FacilityList = _facilityService.GetAllCategories().Select(x => x.Name).Distinct().Select(u => new SelectListItem
                {
                    Text = u
                })
            };
            //Facility facility = new();
            return View(amenityVM);
        }
        [HttpPost]
        public IActionResult Create(AmenityVM obj)
        {

            var addedOrNot = _facilityService.AddNewFacility(obj.Facility);
            if (addedOrNot)
            {
                TempData["success"] = "The facility has been created successfully.";
                return RedirectToAction("Index");
            }

            TempData["error"] = "This facility has already exist.";

            obj.FacilityList = _facilityService.GetAll().Select(x => x.Category).Distinct().Select(u => new SelectListItem
            {
                Text = u,
            });

            return View(obj);

        }
        public IActionResult Update(int facilityId)
        {
            if (facilityId != 0)
            {
                AmenityVM amenityVM = new()
                {
                    FacilityList = _facilityService.GetAllCategories().Select(x => x.Name).Distinct().Select(u => new SelectListItem
                    {
                        Text = u
                    }),
                    Facility = _facilityService.GetAll().Where(x => x.Id == facilityId).FirstOrDefault(),
                    
                };


                if (amenityVM == null)
                {
                    return RedirectToAction("Error", "Home");
                }
                return View(amenityVM);
            }

            return RedirectToAction("Error", "Home");
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
                _facilityService.UpdateFacility(amenityVM.Facility);
                TempData["success"] = "The amenity has been created successfully.";
                return RedirectToAction(nameof(Index));
            }

            amenityVM.FacilityList = _facilityService.GetAll().Select(x => x.Category).Distinct().Select(u => new SelectListItem
            {
                Text = u
            });

            return View(amenityVM);
        }

        public IActionResult Category()
        {
            CategoryVM categoryVM = new()
            {
                CategoryList = _facilityService.GetAllCategories().Select(x => new SelectListItem
                {
                    Text = x.Name
                })
            };
            

            return View(categoryVM);
        }
        [HttpPost]
        public IActionResult Category(CategoryVM categoryVM)
        {
            var tryAddCategory = _facilityService.AddCategory(categoryVM.Category);
            if (tryAddCategory)
            {
                TempData["success"] = "The category has been created successfully.";
                return RedirectToAction(nameof(Index));
            }
            TempData["error"] = "The category could not be deleted.";
            return View(categoryVM);
        }
        public IActionResult Delete(int facilityId)
        {

            Facility facilityToDelete = _facilityService.GetAll().Where(x => x.Id == facilityId).FirstOrDefault();

            if (facilityToDelete == null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(facilityToDelete);
        }
        [HttpPost]
        public IActionResult Delete(Facility obj)
        {
            var facilityFromDb = _facilityService.GetAll().Where(x => x.Id == obj.Id).FirstOrDefault();
            if (facilityFromDb is not null)
            {
                _facilityService.DeleteFacilityInDb(facilityFromDb);
                TempData["success"] = "The amenity has been deleted successfully.";
                return RedirectToAction(nameof(Index));
            }
            TempData["error"] = "The amenity could not be deleted.";
            return View();
        }

        public IActionResult CategoryList()
        {
            var categoryToDelete = _facilityService.GetAllCategories();
            if (categoryToDelete is null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(categoryToDelete);
        }
        public IActionResult DeleteCategory(int categoryId)
        {
            var categoryFromDb = _facilityService.GetAllCategories().Where(x => x.Id == categoryId).FirstOrDefault();
            if (categoryFromDb is not null)
            {
                return View(categoryFromDb);
            }
            return RedirectToAction("Error", "Home");
        }

        [HttpPost]
        public IActionResult DeleteCategory(Category obj)
        {
            Category categoryToDelete = _facilityService.GetAllCategories().Where(x => x.Id == obj.Id).FirstOrDefault();
            if (categoryToDelete is not null)
            {
                _facilityService.DeleteCategory(categoryToDelete);
                TempData["success"] = "The amenity has been deleted successfully.";
                return RedirectToAction(nameof(Index));
            }
            return View();
        }
    }
}
