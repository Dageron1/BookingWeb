using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VillaProject.Domain.Entities
{
    public class Villa
    {
        public int Id { get; set; }
        [MaxLength(50)]
        public required string Name { get; set; }
        public string? Description { get; set; }
        [Display(Name = "Price per night")]
        [Range(10,10000)]
        public double Price { get; set; }

        public int Sqft { get; set; }
        [Range(1,10)]
        public int Occupancy { get; set; }

        public DateTime? Created_Date { get; set; }
        public DateTime? Updated_Data { get; set; }

        [ValidateNever]
        public IEnumerable<Amenity> VillaAmenity { get; set; } //navigation property

        [NotMapped] // чтобы не добавлять в БД
        public bool IsAvaliable { get; set; } = true;

        [ValidateNever]
        public List<VillaImages> VillaImages { get; set; }

        public List<Facility> Facilities { get; set; } = [];
    }
}
