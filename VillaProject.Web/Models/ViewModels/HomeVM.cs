using Syncfusion.EJ2.Calendars;
using VillaProject.Domain.Entities;
using VillaProject.Web.Controllers;

namespace VillaProject.Web.Models.ViewModels
{
    public class HomeVM
    {
        public IEnumerable<Villa>? VillaList { get; set; }
        public DateOnly CheckInDate { get; set; }
        public DateOnly? CheckOutDate { get; set; }
        public int Nights { get; set; }
        public DateRange dateRange {  get; set; }
    }
}
