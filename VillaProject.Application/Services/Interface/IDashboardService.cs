﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VillaProject.Web.Models.ViewModels;

namespace VillaProject.Application.Services.Interface
{
    public interface IDashboardService
    {
        Task<RadialBarChartDTO> GetTotalBookingRadialChartData();
        Task<RadialBarChartDTO> GetRegisteredUserChartData();
        Task<RadialBarChartDTO> GetRevenueChartData();
        Task<PieChartDTO> GetBookingPieChartData();
        Task<LineChartDTO> GetMemberAndBookingLineChartData();
    }
}
