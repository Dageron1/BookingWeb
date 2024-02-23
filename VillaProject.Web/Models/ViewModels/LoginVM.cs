using System.ComponentModel.DataAnnotations;

namespace VillaProject.Web.Models.ViewModels
{
    public class LoginVM
    {
        [Required]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)] //automatically hide whatever is being typed in that field
        public string Password { get; set; }

        public bool RememberMe { get; set; }

        public string? RedirectUrl { get; set; }
    }
}
