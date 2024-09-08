using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VillaProject.Domain.Entities
{
    public class ApplicationUser : IdentityUser //когда мы добавили наследование. ApplicationUser получил все дефолтные проперти от IdentityUser
    {
        public string? Name { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
