using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using VillaProject.Application.Common.Interfaces;
using VillaProject.Application.Services.Implementation;
using VillaProject.Application.Services.Interface;
using VillaProject.Domain.Entities;
using VillaProject.Infrastructure.Data;
using VillaProject.Infrastructure.Repository;
using VillaProject.Web.Models.ViewModels;

namespace VillaProject.Web.Controllers
{
    [Authorize]
    public class VillaController : Controller
    {
        private readonly IVillaService _villaService;
        private readonly IAmenityService _amenityService;
        private readonly IFacilityService _facilityService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public VillaController(IVillaService villaService, IWebHostEnvironment webHostEnvironment, IAmenityService amenityService, IFacilityService facilityService)
        {
            _villaService = villaService;
            _webHostEnvironment = webHostEnvironment;
            _amenityService = amenityService;
            _facilityService = facilityService;

        }
        public IActionResult Index()
        {
            var villas = _villaService.GetAllVillas();
            return View(villas);

        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Villa obj, List<IFormFile> files)
        {
            if (obj.Name == obj.Description)
            {
                ModelState.AddModelError("name", "The description can't exactly match the Name.");
            }
            if (ModelState.IsValid)
            {
                _villaService.CreateVilla(obj, files);
                TempData["success"] = "The villa has been created successfully.";
                return RedirectToAction(nameof(Index));
            }
            return View(obj);
        }
        public IActionResult Update(int villaId)
        {
            //Вилла с её fasilities
            Villa? obj = _villaService.GetVillaById(villaId);  

            var allFacilities = _facilityService.GetAll().Select(x => new FacilityVM
            {
                Id = x.Id,
                Name = x.Name,
                Logo = x.Logo,
                Category = x.Category,
            }).ToList();

            foreach(var oneFacility in allFacilities)
            {
                foreach (var oneFacilityFromObj in obj.Facilities)
                {
                    if(oneFacility.Name == oneFacilityFromObj.Name)
                    {
                        oneFacility.Selected = true;
                    }
                }
            }

            VillaVM villaVM = new()
            {
                Villa = obj,
                //Amenity = _amenityService.GetAllAmenities().Where(x => x.VillaId == villaId).FirstOrDefault(),
                Facilities = allFacilities
            };
            
            if (obj == null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(villaVM);
        }
        [HttpPost]
        public IActionResult Update(VillaVM obj, List<IFormFile> files)
        {
            if (ModelState.IsValid && obj.Villa.Id > 0)
            {
                var facilities = obj.Facilities.Where(x => x.Selected).Select(x => x.Id).ToArray();
               
                _villaService.UpdateVilla(obj.Villa, files, facilities);
                //_amenityService.UpdateAmenity(obj.Amenity);
                TempData["success"] = "The villa has been updated successfully.";
                return RedirectToAction(nameof(Update), new { villaId = obj.Villa.Id });
            }
            return View(obj);
        }
        public IActionResult Delete(int villaId)
        {
            Villa? obj = _villaService.GetVillaById(villaId);
            if (obj is null) //потому что == может быть overloaded, which is not common to see ia a code
            {
                return RedirectToAction("Error", "Home");
            }
            return View(obj);
        }
        [HttpPost]
        public IActionResult Delete(Villa obj)
        {
            bool deleted = _villaService.DeleteVilla(obj.Id);
            if (deleted)
            {
                TempData["success"] = "The villa has been deleted successfully.";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["error"] = "The villa could not be deleted.";
            }
            return View();
        }

        public IActionResult DeleteImage(int imageId, int villaId)
        {
            _villaService.DeleteVillaImage(imageId, villaId);

            TempData["success"] = "Deleted successfully";

            return RedirectToAction(nameof(Update), new { villaId = villaId });
        }
    }
}
