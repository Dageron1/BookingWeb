using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using VillaProject.Domain.Entities;

namespace VillaProject.Web.Models.ViewModels
{
    public class CategoryVM
    {
        public Category? Category { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> CategoryList { get; set; }
    }
}
