using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VillaProject.Domain.Entities
{
    public class FacilityItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Selected { get; set; }
        public string Logo { get; set; }
    }
}
