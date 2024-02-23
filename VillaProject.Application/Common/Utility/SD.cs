using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VillaProject.Domain.Entities;
using VillaProject.Web.Models.ViewModels;

namespace VillaProject.Application.Common.Utility
{
    public static class SD
    {
        public const string Role_Customer = "Customer";
        public const string Role_Admin = "Admin";

        public const string StatusPending = "Pending";
        public const string StatusApproved = "Approved";
        public const string StatusCheckedIn = "CheckedIn";
        public const string StatusCompleted = "Completed";
        public const string StatusCancelled = "Cancelled";
        public const string StatusRefunded = "Refunded";
        public const string StatusAll = "All";

        public static int VillaRoomsAvaliable_Count(int villaId, List<VillaNumber> villaNumberList, DateOnly checkInDate, int nights, List<Booking> bookings)
        {
            // сколько вилл уже занято на основании времени, кол-ве ночей и прочего
            List<int> bookingInDate = new();
            int finalAvaliableRoomForAllNights = int.MaxValue;

            //how many room numbers are there for that villa
            var roomsInVilla = villaNumberList.Where(x => x.VillaId == villaId).Count();

            for(int i = 0; i < nights; i++)
            {
                //получаем кол-во занятых вилл
                var villasBooked = bookings.Where(u => u.CheckInDate <= checkInDate.AddDays(i) 
                && u.CheckOutDate > checkInDate.AddDays(i) && u.VillaId == villaId);

                //перебираем в занятых комнатах
                foreach(var booking in villasBooked)
                {
                    if (!bookingInDate.Contains(booking.Id))
                    {
                        bookingInDate.Add(booking.Id);
                    }
                }
                //в totalAvaliableRooms записывается число свободных комнат при каждой итерации
                var totalAvaliableRooms = roomsInVilla - bookingInDate.Count(); //Допустим при трех итерациях получили результат: 3, 1, 5
                if(totalAvaliableRooms == 0)
                {
                    return 0;
                }
                else
                {
                    //получаем самое наименьшее число в каждой итерации и если нет числа еще меньше то возвращаем самое наименьшее число.
                    if(finalAvaliableRoomForAllNights > totalAvaliableRooms)
                    {
                        finalAvaliableRoomForAllNights = totalAvaliableRooms; //первый раз будет равно 3, на второй итерации 3 > 1 = true, на третей 1 > 5 = true
                    }
                }            
            }
            return finalAvaliableRoomForAllNights;
        }

        public static RadialBarChartDTO GetRadialChartDataModel(int totalCount, double currentMonthCount, double previousMonthCount)
        {
            RadialBarChartDTO RadialBarChartDTO = new();

            int increaseDecreaseRatio = 100;

            if (previousMonthCount != 0)
            {
                //на сколько % от прошлого месяца был подъем или снижение
                increaseDecreaseRatio = Convert.ToInt32((currentMonthCount - previousMonthCount) / previousMonthCount * 100);
            }

            RadialBarChartDTO.TotalCount = totalCount;
            RadialBarChartDTO.CountInCurrentMonth = Convert.ToInt32(currentMonthCount);
            RadialBarChartDTO.HasRatioIncreased = currentMonthCount > previousMonthCount;
            RadialBarChartDTO.Series = new int[] { increaseDecreaseRatio };

            return RadialBarChartDTO;
        }
    }
}
