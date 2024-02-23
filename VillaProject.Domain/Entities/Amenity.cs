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
        public required string Name { get; set; }
        public string? Description { get; set; }

        //т е VillaId будет внешним ключем для Villa,  и он будет соответствовать главному ключу таблицы Villas в БД
        [ForeignKey("Villa")] //foreign key relation to the table - обозначаем внешний ключ (тут указывается главный ключ от другой таблицы)
        public int VillaId { get; set; }
        [ValidateNever] //убираем валидацию виллы при создании нового VillaNumber, т к нам некуда вписать значение виллы и оно будет не валидно. Если открыть дебаг
        public Villa Villa { get; set; }

    }
}
