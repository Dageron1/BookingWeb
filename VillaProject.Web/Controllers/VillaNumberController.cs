using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VillaProject.Application.Common.Interfaces;
using VillaProject.Domain.Entities;
using VillaProject.Infrastructure.Data;
using VillaProject.Web.Models.ViewModels;

namespace VillaProject.Web.Controllers
{
    public class VillaNumberController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public VillaNumberController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            //подгружаем навигационные пропертичи через Include().
            //includeProp = "Villa" property name of IVillaRepository inside the IUnitOfWork interface.
            var villaNumbers = _unitOfWork.VillaNumber.GetAll(includeProperties: "Villa");
            return View(villaNumbers);
        }
        public IActionResult Create()
        {
            VillaNumberVM villaNumberVM = new()
            {
                VillaList = _unitOfWork.Villa.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name, //key
                    Value = u.Id.ToString()
                })
            };
            /*ViewData["VillaList"] = list;*/ //позволяет передавать данные в View как Dictionary (ket value pair) KEY will be the string
            //ViewBag.VillaList = list;
            return View(villaNumberVM);
        }                    
        [HttpPost]
        public IActionResult Create(VillaNumberVM obj)
        {
            bool roomNumberExists = _unitOfWork.VillaNumber.Any(u =>u.Villa_Number == obj.VillaNumber.Villa_Number);

            //ModelState.Remove("Villa"); убираем валидацию виллы, т к при вводе данных на сайте мы не заполняем эту инфу(о вилле) и модель не валидна.
            if (ModelState.IsValid && !roomNumberExists)
            {
                _unitOfWork.VillaNumber.Add(obj.VillaNumber);
                _unitOfWork.Save();
                TempData["success"] = "The villa number has been created successfully.";
                return RedirectToAction("Index");
            }
            if (roomNumberExists)
            {
                TempData["error"] = "The villa Number has already exist.";
            }

            obj.VillaList = _unitOfWork.Villa.GetAll().Select(u => new SelectListItem
            {
                Text = u.Name, //key
                Value = u.Id.ToString()
            });

            return View(obj);
        }
        public IActionResult Update(int villaNumberId)
        {

            VillaNumberVM villaNumberVM = new()
            {
                VillaList = _unitOfWork.Villa.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name, //key
                    Value = u.Id.ToString()
                }),
                VillaNumber = _unitOfWork.VillaNumber.Get(u => u.Villa_Number == villaNumberId)
            };
            if (villaNumberVM.VillaNumber == null)
            {
                return RedirectToAction("Error","Home");
            }
            return View(villaNumberVM);
        }
        [HttpPost]
        public IActionResult Update(VillaNumberVM villaNumberVM)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.VillaNumber.Update(villaNumberVM.VillaNumber);
                _unitOfWork.Save();
                TempData["success"] = "The villa number has been created successfully.";
                return RedirectToAction(nameof(Index));
            }
            villaNumberVM.VillaList = _unitOfWork.Villa.GetAll().Select(u => new SelectListItem
            {
                Text = u.Name, //key
                Value = u.Id.ToString()
            });

            return View(villaNumberVM);
        }
        public IActionResult Delete(int villaNumberId)
        {

            VillaNumberVM villaNumberVM = new()
            {
                VillaList = _unitOfWork.Villa.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name, //key
                    Value = u.Id.ToString()
                }),
                VillaNumber = _unitOfWork.VillaNumber.Get(u => u.Villa_Number == villaNumberId)
            };
            if (villaNumberVM.VillaNumber == null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(villaNumberVM);
        }
        [HttpPost]
        public IActionResult Delete(VillaNumberVM villaNumberVM)
        {
            VillaNumber? objFromDb = _unitOfWork.VillaNumber.Get(u=>u.Villa_Number == villaNumberVM.VillaNumber.Villa_Number);
            if (objFromDb is not null)
            {
                _unitOfWork.VillaNumber.Remove(objFromDb);
                _unitOfWork.Save();
                TempData["success"] = "The villa number has been deleted successfully.";
                return RedirectToAction(nameof(Index));
            }
            TempData["error"] = "The villa number could not be deleted.";
            return View();
        }
    }
}
