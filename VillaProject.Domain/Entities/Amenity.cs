using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VillaProject.Domain.Entities
{
    public class Amenity
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        //public string? Description { get; set; }

        //Amenities list
        //Kitchen && Cooking
        public bool Microwave { get; set; }
        
        public bool Freezer { get; set; }
        public bool Kitchen { get; set; }
        public bool Restaurant { get; set; }
        public bool DeliveryService { get; set; }
        public bool Breakfast { get; set; }

        //Media
        public bool Wifi { get; set; }
        public bool HDTV { get; set; }

        //Territory
        public bool Parking { get; set; }
        public bool Pool { get; set; }
        public bool GardenView { get; set; }
        public bool MountainView { get; set; }
        public bool OceanView { get; set; }


        //Room
        public bool KingBed { get; set; }
        public bool Balcony { get; set; }
        public bool AC { get; set; }
        public bool Safe { get; set; }   
        public bool Heating { get; set; }
 
        public bool SelfCheckIn { get; set; }



        //т е VillaId будет внешним ключем для Villa,  и он будет соответствовать главному ключу таблицы Villas в БД
        [ForeignKey("Villa")] //foreign key relation to the table - обозначаем внешний ключ (тут указывается главный ключ от другой таблицы)
        public int VillaId { get; set; }
        [ValidateNever] //убираем валидацию виллы при создании нового VillaNumber, т к нам некуда вписать значение виллы и оно будет не валидно. Если открыть дебаг
        public Villa Villa { get; set; }

    }
}
