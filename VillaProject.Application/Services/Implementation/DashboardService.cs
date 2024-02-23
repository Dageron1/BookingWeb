using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VillaProject.Application.Common.Interfaces;
using VillaProject.Application.Common.Utility;
using VillaProject.Application.Services.Interface;
using VillaProject.Web.Models.ViewModels;

namespace VillaProject.Application.Services.Implementation
{
    public class DashboardService : IDashboardService
    {
        private readonly IUnitOfWork _unitOfWork;
        static int previousMonth = DateTime.Now.Month == 1 ? 12 : DateTime.Now.Month - 1;
        readonly DateTime currentMonthStartDate = new(DateTime.Now.Year, DateTime.Now.Month, 1);

        readonly DateTime previousMonthStartDate =
            DateTime.Now.Month == 1 ? new(DateTime.Now.Year - 1, previousMonth, 1) : new(DateTime.Now.Year, previousMonth, 1);


        public DashboardService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<PieChartDTO> GetBookingPieChartData()
        {
            var totalBookings = _unitOfWork.Booking.GetAll(u => u.BookingDate >= DateTime.Now.AddDays(-30) &&
            (u.Status != SD.StatusPending || u.Status == SD.StatusCancelled));
            //(u.Status != SD.StatusPending && u.Status != SD.StatusCancelled)); если не нужны отмененные букинги

            //получаем только одну строку с юзером, у которого только 1 букинг в таблице. Key = id юзера по которому группировали
            var customerWithOneBooking = totalBookings.GroupBy(b => b.UserId).Where(x => x.Count() == 1).Select(x => x.Key).ToList();

            int bookingsByNewCustomer = customerWithOneBooking.Count();

            int bookingsByReturningCustomer = totalBookings.Count() - bookingsByNewCustomer;

            PieChartDTO PieChartDTO = new()
            {
                Labels = new string[] { "New Customer Bookings", "Returning Customer Bookings" },
                Series = new decimal[] { bookingsByNewCustomer, bookingsByReturningCustomer }
            };

            return PieChartDTO;
        }

        public async Task<LineChartDTO> GetMemberAndBookingLineChartData()
        {
            var bookingData = _unitOfWork.Booking.GetAll(u => u.BookingDate >= DateTime.Now.AddDays(-30) &&
            u.BookingDate.Date <= DateTime.Now).GroupBy(b => b.BookingDate.Date) //группируем именно по ДНЮ. Чтобы считало именно букинги за этот день
            .Select(u => new
            {
                DateTime = u.Key,
                NewBookingCount = u.Count(),
            });

            var customerData = _unitOfWork.User.GetAll(u => u.CreatedAt >= DateTime.Now.AddDays(-30) &&
            u.CreatedAt.Date <= DateTime.Now).GroupBy(b => b.CreatedAt.Date)//тут создался объект key(дата) - (1,2,3 объект) //группируем именно по ДНЮ. Чтобы считало именно букинги за этот день
            .Select(u => new
            {
                DateTime = u.Key,
                NewCustomerCount = u.Count(),
            });
            //Левое — соединение обычно возвращает все записи из левой таблицы и соответствующие записи правой таблицы; если совпадений нет,
            //в столбцах будет возвращено значение NULL. 
            //booking=>booking.DateTime - на основании чего происходит присоединение
            var leftJoin = bookingData.GroupJoin(customerData, booking => booking.DateTime, customer => customer.DateTime,
                (booking, customer) => new
                {
                    booking.DateTime,
                    booking.NewBookingCount,
                    NewCustomerCount = customer.Select(x => x.NewCustomerCount).FirstOrDefault() //для возвращения 0, если нет совпадения
                });

            var rightJoin = customerData.GroupJoin(bookingData, customer => customer.DateTime, booking => booking.DateTime,
                (customer, booking) => new
                {
                    customer.DateTime,
                    NewBookingCount = booking.Select(x => x.NewBookingCount).FirstOrDefault(),
                    customer.NewCustomerCount
                });
            //соединяем две таблицы в одну т к у их одинаковые колонки
            var mergedData = leftJoin.Union(rightJoin).OrderBy(x => x.DateTime).ToList();

            var newBookingData = mergedData.Select(x => x.NewBookingCount).ToArray();
            var newCustomerData = mergedData.Select(x => x.NewCustomerCount).ToArray();
            var categories = mergedData.Select(x => x.DateTime.ToString("MM/dd/yyyy")).ToArray();

            List<ChartData> chartDataList = new()
            {
                new ChartData
                {
                    Name = "New Bookings",
                    Data = newBookingData
                },
                new ChartData
                {
                    Name = "New Members",
                    Data = newCustomerData
                },
            };

            LineChartDTO LineChartDTO = new()
            {
                Categories = categories,
                Series = chartDataList
            };

            return LineChartDTO;
        }

        public async Task<RadialBarChartDTO> GetRegisteredUserChartData()
        {
            var totalUsers = _unitOfWork.User.GetAll();

            var countByCurrentMonth = totalUsers.Count(u => u.CreatedAt >= currentMonthStartDate && u.CreatedAt <= DateTime.Now);

            var countByPreviousMonth = totalUsers.Count(u => u.CreatedAt >= previousMonthStartDate && u.CreatedAt <= currentMonthStartDate);



            return SD.GetRadialChartDataModel(totalUsers.Count(), countByCurrentMonth, countByPreviousMonth);
        }

        public async Task<RadialBarChartDTO> GetRevenueChartData()
        {
            var totalBookings = _unitOfWork.Booking.GetAll(u => u.Status != SD.StatusPending || u.Status == SD.StatusCancelled);

            var totalRevenue = Convert.ToInt32(totalBookings.Sum(u => u.TotalCost));

            var countByCurrentMonth = totalBookings.Where(u => u.BookingDate >= currentMonthStartDate && u.BookingDate <= DateTime.Now)
                .Sum(u => u.TotalCost);

            var countByPreviousMonth = totalBookings.Where(u => u.BookingDate >= previousMonthStartDate && u.BookingDate <= currentMonthStartDate)
                .Sum(u => u.TotalCost);

            return SD.GetRadialChartDataModel(totalRevenue, countByCurrentMonth, countByPreviousMonth);
        }

        public async Task<RadialBarChartDTO> GetTotalBookingRadialChartData()
        {
            var totalBookings = _unitOfWork.Booking.GetAll(u => u.Status != SD.StatusPending || u.Status == SD.StatusCancelled);

            var countByCurrentMonth = totalBookings.Count(u => u.BookingDate >= currentMonthStartDate && u.BookingDate <= DateTime.Now);

            var countByPreviousMonth = totalBookings.Count(u => u.BookingDate >= previousMonthStartDate && u.BookingDate <= currentMonthStartDate);

            return SD.GetRadialChartDataModel(totalBookings.Count(), countByCurrentMonth, countByPreviousMonth);
        }


    }
}
