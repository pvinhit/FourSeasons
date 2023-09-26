using be.Models;
using be.Repositories.BookingRepository;
using Microsoft.EntityFrameworkCore;
using System.Dynamic;
using System.Globalization;

namespace be.Services
{
    public class BookingService : IBookingService
    {
        public IBookingRepository _bookingRepo;

        public BookingService(IBookingRepository bookingRepository)
        {
            _bookingRepo = bookingRepository;
        }

        public IList<Booking> GetAllBookings()
        {
            return _bookingRepo.GetAllBookings();
        }

        public Booking GetBooking(int bookingId)
        {
            return _bookingRepo.GetBooking(bookingId);
        }

        public void UpdateBooking(Booking booking)
        {
            _bookingRepo.UpdateBooking(booking);
        }

        #region KHUYENHTB - VIEW BOOKING HISTORIES
        public void DeleteBooking(int bookingId)
        {
            _bookingRepo.DeleteBooking(bookingId);
        }

        public IEnumerable<dynamic> GetBookingByConditions(int userId, string checkin, string checkout, string bookingDate)
        {
            return _bookingRepo.GetBookingByConditions(userId, checkin, checkout, bookingDate);
        }

        /// <summary>
        /// Get all booking history of user
        /// </summary>
        /// <param name="userId">user id</param>
        /// <returns>list of all booking history of user</returns>
        public dynamic GetBookingHistories(int userId)
        {
            return _bookingRepo.GetBookingHistories(userId);
        }

        public List<Room> GetRoomsByBookingId(int bookingId)
        {
            return _bookingRepo.GetRoomsByBookingId(bookingId);
        }
        #endregion

        #region HUYNG5 - MANAGE BOOKING BY MANAGER/RECEPTIONIST
        public IList<BookingDetail> GetAllBookingDetail()
        {
            return _bookingRepo.GetAllBookingDetail();
        }

        public void ChangeStatus(int id, string status)
        {
            _bookingRepo.ChangeStatus(id, status);
        }

        public void UpdateBooking(int id, DateTime checkIn, DateTime checkOut, string status)
        {
            _bookingRepo.UpdateBooking(id, checkIn, checkOut, status);
        }
        #endregion

        #region HIEUVM15 - ADD BOOKING
        public object AddBooking(Data data)
        {
            return _bookingRepo.AddBooking(data);
        }

        public bool isBooking(Data data)
        {
            return _bookingRepo.isBooking(data);
        }
        #endregion
    }
}
