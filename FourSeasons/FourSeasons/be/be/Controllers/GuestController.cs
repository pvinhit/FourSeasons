using be.Models;
using be.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Dynamic;
using System.Text.Json;

namespace be.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GuestController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IBookingService _bookingService;
        private readonly ICategoryService _categoryService;
        private readonly IRoomService _roomService;

        public GuestController(IUserService userService, IBookingService bookingService, ICategoryService categoryService, IRoomService roomService)
        {
            _bookingService = bookingService;
            _userService = userService;
            _categoryService = categoryService; 
            _roomService = roomService;
        }

        #region KHOALHN - Get Category/Room
        [HttpGet("GetAllCategories")]
        public ActionResult GetAllCategories()
        {
            var result = _categoryService.GetEnableCategories();

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpGet("GetAllRoomCategories/{id}")]
        public ActionResult GetAllRoomCategories(int id)
        {
            var result = _roomService.GetAllRoomCategories(id);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }
        [HttpGet("GetAllRoom")]
        public ActionResult GetAllRoom()
        {
            var result = _roomService.GetAllRoom();

            if (result == null)
                return NotFound();
            return Ok(result);
        }
        #endregion
        
        #region KHUYENHTB - USER-INFORMATION/VIEW BOOKING HISTORY/CANCEL BOOKING       
        [HttpGet("GetUserBookingHistory")]
        public ActionResult GetUserBookingHistory(int userId)
        {
            try
            {
                var bookingHistories = _bookingService.GetBookingHistories(userId);
                return Ok(bookingHistories);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("SearchBookingHistory")]
        public ActionResult SearchBookingHistory(int userId, string? checkin, string? checkout, string? bookingDate)
        {
            try
            {
                var bookingHistories = _bookingService.GetBookingByConditions(userId, checkin, checkout, bookingDate);
                return Ok(bookingHistories);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("CancelBooking")]
        public ActionResult CancelBooking([FromBody] CancelBookingData data)
        {
            try
            {
                _bookingService.DeleteBooking(data.bookingId);
                var bookingHistories = _bookingService.GetBookingHistories(data.userId);
                return Ok(bookingHistories);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region NAMTT23 - CATEGORY/ROOM 
        [HttpGet("getAllRooms")]
        public ActionResult GetAllRooms()
        {
            dynamic rooms = _roomService.GetEnableRooms();
            return Ok(rooms);
        }

        [HttpGet("getRoomTypes")]
        public ActionResult GetRoomTypes()
        {
            dynamic roomtypes = _categoryService.GetEnableCategories();
            return Ok(roomtypes);
        }

        [HttpGet("getRoomsByRoomType")]
        public ActionResult GetRoomsByRoomType(string roomType, DateTime sd, DateTime ed)
        {
            dynamic rooms = _roomService.GetAvailableRoomsByType(roomType, sd, ed);
            return Ok(rooms);
        }

        [HttpPost("getBookingRooms")]
        public ActionResult GetBookingRooms([FromBody] int[] roomIds)
        {
            dynamic rooms = _roomService.GetRooms(roomIds);
            return Ok(rooms);
        }
        #endregion
    }

    #region DTO MODELS
    public class CancelBookingData
    {
        public int bookingId { get; set; }
        public int userId { get; set; }
    }

    public class UserUpdateInfor
    {
        public string password { get; set; }
        public string? fullName { get; set; }
        public string? address { get; set; }
        public string phone { get; set; }
        public string idcard { get; set; }
        public string? birthday { get; set; }
        public string? avatar { get; set; }
        public int userId { get; set; }
    }
    #endregion    
}
