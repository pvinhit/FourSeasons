using be.Models;
using be.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace be.Controllers
{
    [Route("api/booking")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        public IBookingService _bookingService;

        public BookingController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        [HttpPost("order")]
        public async Task<ActionResult> AddBooking([FromBody] Data data)
        {
            try
            {
                var result = _bookingService.AddBooking(data);
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }
    }
    
}
