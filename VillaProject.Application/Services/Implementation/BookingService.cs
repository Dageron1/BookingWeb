﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VillaProject.Application.Common.Interfaces;
using VillaProject.Application.Common.Utility;
using VillaProject.Application.Services.Interface;
using VillaProject.Domain.Entities;

namespace VillaProject.Application.Services.Implementation
{
    public class BookingService : IBookingService
    {
        private readonly IUnitOfWork _unitOfWork;

        public BookingService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public void CreateBooking(Booking booking)
        {
            _unitOfWork.Booking.Add(booking);
            _unitOfWork.Save();
        }

        public IEnumerable<Booking> GetAllBookings(string? statusFilterList = "", string userId = "")
        {
            IEnumerable<string> statusList = statusFilterList.ToLower().Split(',');
            if(!string.IsNullOrEmpty(statusFilterList) && !string.IsNullOrEmpty(userId))
            {         
                    return _unitOfWork.Booking.GetAll(u => statusList.Contains(u.Status.ToLower()) &&
                    u.UserId == userId, includeProperties: "User,Villa");
            }
            else
            {
                if (!string.IsNullOrEmpty(statusFilterList))
                {
                    if (statusFilterList != SD.StatusAll)
                    {
                        return _unitOfWork.Booking.GetAll(u => statusList.Contains(u.Status.ToLower()), includeProperties: "User,Villa");
                    }   
                }
                if (!string.IsNullOrEmpty(userId))
                {
                    return _unitOfWork.Booking.GetAll(u => u.UserId == userId, includeProperties: "User,Villa");
                }
            }
            return _unitOfWork.Booking.GetAll(includeProperties: "User,Villa");
            //else //можно убрать, но я еще думаю насчет логики.
            //{
            //    if(!string.IsNullOrEmpty(statusFilterList))
            //    {
            //        return _unitOfWork.Booking.GetAll(u => statusList.Contains(u.Status.ToLower()), includeProperties: "User,Villa");
            //    }
            //    if (!string.IsNullOrEmpty(userId))
            //    {
            //        return _unitOfWork.Booking.GetAll(u => u.UserId == userId, includeProperties: "User,Villa");
            //    }
            //}
            //return _unitOfWork.Booking.GetAll(includeProperties: "User,Villa");
        }

        public Booking GetBookingById(int bookingId)
        {
            return _unitOfWork.Booking.Get(u => u.Id == bookingId, includeProperties: "User,Villa");
        }

        public IEnumerable<int> GetCheckedInVillaNumbers(int villaId)
        {
            return _unitOfWork.Booking.GetAll(u => u.VillaId == villaId && u.Status == SD.StatusCheckedIn).Select(u => u.VillaNumber);
        }

        public void UpdateStatus(int bookingId, string bookingStatus, int villaNumber = 0)
        {
            var bookingFromDb = _unitOfWork.Booking.Get(m => m.Id == bookingId, tracked: true); //автоматом трекает эту переменную
            if (bookingFromDb != null)
            {
                bookingFromDb.Status = bookingStatus;
                if (bookingStatus == SD.StatusCheckedIn)
                {
                    bookingFromDb.VillaNumber = villaNumber;
                    bookingFromDb.ActualCheckInDate = DateTime.Now;
                }
                if (bookingStatus == SD.StatusCompleted)
                {
                    bookingFromDb.ActualCheckInDate = DateTime.Now;
                }
            }
            _unitOfWork.Save();
        }

        public void UpdateStripePaymentID(int bookingId, string sessionId, string paymentIntentId)
        {
            var bookingFromDb = _unitOfWork.Booking.Get(m => m.Id == bookingId, tracked: true); //до этого был FirstOrDefault и не нужно было трекать.
            if (bookingFromDb != null)
            {
                if (!string.IsNullOrEmpty(sessionId))
                {
                    bookingFromDb.StripeSessionId = sessionId;
                }
                if (!string.IsNullOrEmpty(paymentIntentId))
                {
                    bookingFromDb.StripePaymentIntentId = paymentIntentId;
                    bookingFromDb.PaymentDate = DateTime.Now;
                    bookingFromDb.IsPaymentSuccessful = true;
                }
            }
            _unitOfWork.Save();
        }
    }
}