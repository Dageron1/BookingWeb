using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VillaProject.Domain.Entities;
using VillaProject.Web.Models.ViewModels;

namespace VillaProject.Application.Services.Interface
{
    public interface IFacilityService
    {
        IEnumerable<Facility> GetAll();
        IEnumerable<Category> GetAllCategories();
        bool DeleteFacilities(int villaId);
        bool AddNewFacility(Facility facility);
        void DeleteFacilityInDb(Facility obj);
        void UpdateFacility(Facility obj);
        bool AddCategory(Category obj);
        void DeleteCategory(Category obj);
    }
}
