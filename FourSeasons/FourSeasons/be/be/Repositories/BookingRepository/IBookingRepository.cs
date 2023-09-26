using be.Models;

namespace be.Repositories.BookingRepository
{
    public interface IBookingRepository
    {
        IList<Booking> GetAllBookings();
        Booking GetBooking(int bookingId);
        void UpdateBooking(Booking booking);
        void DeleteBooking(int bookingId);
        IEnumerable<dynamic> GetBookingByConditions(int userId, string checkin, string checkout, string bookingDate);
        dynamic GetBookingHistories(int userId);
        List<Room> GetRoomsByBookingId(int bookingId);
        IList<BookingDetail> GetAllBookingDetail();
        void ChangeStatus(int id, string status);
        void UpdateBooking(int id, DateTime checkIn, DateTime checkOut, string status);
        object AddBooking(Data data);
        bool isBooking(Data data);

    }
}
