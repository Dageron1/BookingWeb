using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VillaProject.Domain.Entities
{
    public class Booking
    {
        [Key]//не обязательно
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }

        [Required]
        public int VillaId { get; set; }
        [ForeignKey("VillaId")]
        public Villa Villa { get; set; }

        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        public string? Phone { get; set; }

        [Required]
        public double TotalCost { get; set; }
        public int Nights { get; set; }
        public string? Status { get; set; }

        [Required]
        public DateTime BookingDate { get; set; }
        [Required]
        public DateOnly CheckInDate { get; set; }
        [Required]
        public DateOnly CheckOutDate { get; set; }

        public bool IsPaymentSuccessful { get; set; } = false;
        public DateTime PaymentDate { get; set; }

        //когда отправляем запрос на Stripe API и создаем Checkout Session и Stripe API возвращает SessionId,
        //вместе с этим Id создается специальная URL by stripe, и мы должны переадресовать покупателя на этот URL(Stripe Checkout) 
        //далее пользователь оказывается на странице Stripe Checkout где вводит данные кредитной карты
        //после нажатия оплаты и подтверждения Stripe Checkout перенаправляет пользователя обратно на основной сайт.
        // и Stripe Checkout так же при возврате выдает PaymentIntentId - уникальный идентификатор показывающий завершение платежа
        //по этому уникальному номеру мы отслеживает состояние платежа, если успешно завершен указываем - (Status Approved)
        public string? StripeSessionId { get; set; }
        public string? StripePaymentIntentId { get; set; }

        public DateTime ActualCheckInDate { get; set; }
        public DateTime ActualCheckOutDate { get;set; }

        //отвечает занята вила или нет, привязывается к выбранному букингу
        public int VillaNumber { get; set; }

        [NotMapped]
        public List<VillaNumber> VillaNumbers { get; set; }
    }
}
