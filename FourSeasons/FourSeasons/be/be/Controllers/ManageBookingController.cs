using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using be.Models;
using be.Services;
using System.Collections;
using Microsoft.EntityFrameworkCore;

namespace be.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManageBookingController : Controller
    {
        private readonly IUserService _userService;
        private readonly IBookingService _bookingService;
        private readonly IRoomService _roomService;
        private readonly DbFourSeasonHotelContext _context = new DbFourSeasonHotelContext();

        public ManageBookingController(IUserService userService, IBookingService bookingService, IRoomService roomService)
        {
            _bookingService = bookingService;
            _userService = userService;
            _roomService = roomService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerator<Booking>>> GetBookingList()
        {
            if (_bookingService.GetAllBookings() == null)
            {
                return NotFound();
            }
            var bookingList = _bookingService.GetAllBookings();
            var guestList = _userService.GetGuestList();
            var bookingOfGuest = from booking in bookingList
                                 join guest in guestList on booking.UserId equals guest.UserId
                                 join bd in _context.BookingDetails on booking.BookingId equals bd.BookingId
                                 join r in _context.Rooms on bd.RoomId equals r.RoomId
                                 group bd by new { booking.CreateDate, booking.CheckIn, booking.CheckOut } into k
                                 select new
                                 {
                                     BookingId = (from bb in _context.Bookings
                                                  where bb.CreateDate == k.Key.CreateDate && bb.CheckIn == k.Key.CheckIn && bb.CheckOut == k.Key.CheckOut
                                                  select bb.BookingId).FirstOrDefault(),
                                     FullName = (from bb in _context.Bookings
                                                 join u in _context.Users on bb.UserId equals u.UserId
                                                 where bb.CreateDate == k.Key.CreateDate && bb.CheckIn == k.Key.CheckIn && bb.CheckOut == k.Key.CheckOut
                                                 select u.FullName).FirstOrDefault(),
                                     Email = (from bb in _context.Bookings
                                              join u in _context.Users on bb.UserId equals u.UserId
                                              where bb.CreateDate == k.Key.CreateDate && bb.CheckIn == k.Key.CheckIn && bb.CheckOut == k.Key.CheckOut
                                              select u.Email).FirstOrDefault(),
                                     Phone = (from bb in _context.Bookings
                                              join u in _context.Users on bb.UserId equals u.UserId
                                              where bb.CreateDate == k.Key.CreateDate && bb.CheckIn == k.Key.CheckIn && bb.CheckOut == k.Key.CheckOut
                                              select u.Phone).FirstOrDefault(),
                                     createDate = k.Key.CreateDate.ToString("yyyy-MM-dd"),
                                     checkIn = k.Key.CheckIn.ToString("yyyy-MM-dd"),
                                     checkOut = k.Key.CheckOut.ToString("yyyy-MM-dd"),
                                     Status = (from bb in _context.Bookings
                                               where bb.CreateDate == k.Key.CreateDate && bb.CheckIn == k.Key.CheckIn && bb.CheckOut == k.Key.CheckOut
                                               select bb.Status).FirstOrDefault(),
                                     totalPrice = CurrencyFormat((from bb in _context.Bookings
                                                                  where bb.CreateDate == k.Key.CreateDate && bb.CheckIn == k.Key.CheckIn && bb.CheckOut == k.Key.CheckOut
                                                                  select bb.TotalPrice).FirstOrDefault()),
                                     RoomName = (from rr in _context.Rooms
                                                 join bdd in _context.BookingDetails on rr.RoomId equals bdd.RoomId
                                                 join bb in _context.Bookings on bdd.BookingId equals bb.BookingId
                                                 where bb.CreateDate == k.Key.CreateDate && bb.CheckIn == k.Key.CheckIn && bb.CheckOut == k.Key.CheckOut
                                                 select rr.RoomName),
                                     Style = (k.Key.CheckOut < DateTime.Now) ? "none" : "",
                                     Disable = (k.Key.CheckIn < DateTime.Now && k.Key.CheckOut > DateTime.Now) ? true : false,
                                 };
            return Ok(bookingOfGuest);
        }

        public static string CurrencyFormat(decimal money)
        {
            return String.Format("{0:0,0}", money);
        }

        [HttpPost("{id}")]
        public async Task<ActionResult<Booking>> ChangeStatus(int id, string status)
        {
            if (_bookingService.GetBooking(id) == null)
            {
                return BadRequest();
            }
            _bookingService.ChangeStatus(id, status);
            return Ok(_bookingService.GetBooking(id));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateBooking(int id, DateTime checkIn, DateTime checkOut, string status)
        {
            if (_bookingService.GetBooking(id) == null)
            {
                return BadRequest();
            }
            _bookingService.UpdateBooking(id, checkIn, checkOut, status);
            return Ok();
        }
    }
}
