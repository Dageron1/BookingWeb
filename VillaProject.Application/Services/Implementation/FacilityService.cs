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
using VillaProject.Web.Models.ViewModels;


namespace VillaProject.Application.Services.Implementation
{
    public class FacilityService : IFacilityService
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public FacilityService(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            //_villaRepo = villaRepo;
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }

        public bool AddNewFacility(Facility facility)
        {
            var facilityExist = _unitOfWork.Facility.Any(x => x.Name == facility.Name && x.Category == facility.Category);

            if (!facilityExist)
            {
                _unitOfWork.Facility.Add(facility);
                _unitOfWork.Save();

                return true;
            }
            else
                return false;

        }

        public bool AddCategory(Category obj)
        {
            try
            {
                var existOrNot = _unitOfWork.Category.Get(x => x.Name == obj.Name);
                if (existOrNot is null)
                {
                    _unitOfWork.Category.Add(obj);
                    _unitOfWork.Save();
                    return true;
                }
                return false;

            }
            catch (Exception)
            {
                return false;
            }

        }


        public void UpdateFacility(Facility obj)
        {
            _unitOfWork.Facility.Update(obj);
            _unitOfWork.Save();

        }

        public void DeleteFacilityInDb(Facility obj)
        {
            _unitOfWork.Facility.Remove(obj);
            _unitOfWork.Save();

        }

        public void DeleteCategory(Category obj)
        {
            _unitOfWork.Category.Remove(obj);
            _unitOfWork.Save();

        }
        //Поменть название или удалить вообще
        public bool DeleteFacilities(int villaId)
        {
            try
            {
                //Special Villa with all facilities
                var villaById = _unitOfWork.Villa.Get(x => x.Id == villaId, includeProperties: "Facilities");
                //all facilities с их виллами
                var facilities = _unitOfWork.Facility.GetAll(includeProperties: "Villas");
                //
                villaById.Facilities.Clear();

                _unitOfWork.Save();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }


        public IEnumerable<Category> GetAllCategories()
        {
            return _unitOfWork.Category.GetAll();
        }

        public IEnumerable<Facility> GetAll()
        {
            return _unitOfWork.Facility.GetAll(includeProperties: "Villas");
        }


    }
}
