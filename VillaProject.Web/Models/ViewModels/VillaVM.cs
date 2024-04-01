using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using VillaProject.Domain.Entities;

namespace VillaProject.Web.Models.ViewModels
{
    public class VillaVM
    {
        //public Amenity? Amenity { get; set; }
        [ValidateNever]
        public Villa? Villa { get; set; }
        [BindProperty]
        [ValidateNever]
        public List<FacilityVM> Facilities { get; set; }
    }
}
