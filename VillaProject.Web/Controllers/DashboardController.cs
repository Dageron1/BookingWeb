using Microsoft.AspNetCore.Mvc;
using System.Linq;
using VillaProject.Application.Common.Interfaces;
using VillaProject.Application.Services.Implementation;
using VillaProject.Application.Common.Utility;
using VillaProject.Application.Services.Interface;
using VillaProject.Web.Models.ViewModels;

namespace VillaProject.Web.Controllers
{
    public class DashboardController : Controller
    {
        //на этом этапе программа пытается создать этот объект
        //если в program.cs в сервисах указана реализация. То программа знает, что при обращении к объекту этого интерфейса нужно создать объект реализации
        //далее происходит передача в контроллер
        private readonly IDashboardService _dashboardService; 

        public DashboardController(IDashboardService dashboardService)
        { 
            _dashboardService = dashboardService;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GetTotalBookingRadialChartData()
        {
            return Json(await _dashboardService.GetTotalBookingRadialChartData());    
        }

        public async Task<IActionResult> GetRegisteredUserChartData()
        {
            return Json(await _dashboardService.GetRegisteredUserChartData());
        }

        public async Task<IActionResult> GetRevenueChartData()
        {
            return Json(await _dashboardService.GetRevenueChartData());
        }

        public async Task<IActionResult> GetBookingPieChartData()
        {
            return Json(await _dashboardService.GetBookingPieChartData());
            //return Json(PieChartDTO); //it's not a real VM, its DTOs data transfer object
        }

        public async Task<IActionResult> GetMemberAndBookingLineChartData()
        {
            return Json(await _dashboardService.GetMemberAndBookingLineChartData());
        }

        
    }
}
