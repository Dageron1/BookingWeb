using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VillaProject.Domain.Entities
{
    public class FacilityVilla
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Facility")]
        public int FacilityId { get; set; }
        public Facility Facility { get; set; }


        [ForeignKey("Villa")]
        public int VillaId { get; set; }
        public Villa Villa { get; set; }
    }
}
