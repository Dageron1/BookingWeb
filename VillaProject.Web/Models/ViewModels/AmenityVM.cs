using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using VillaProject.Domain.Entities;

namespace VillaProject.Web.Models.ViewModels
{
    public class AmenityVM
    {
        public Facility? Facility { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> FacilityList { get; set; }
    }
}
