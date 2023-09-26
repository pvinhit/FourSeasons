using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using be.Models;
using be.Services;


namespace be.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManageGuestController : Controller
    {

        private readonly IUserService _userService;

        public ManageGuestController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerator<User>>> GetGuestList()
        {
            if (_userService.GetGuestList() == null)
            {
                return NotFound();
            }
            var guestList = _userService.GetGuestList();
            var result = from guest in guestList
                         select new
                         {
                             guest.UserId,
                             guest.Avatar,
                             guest.FullName,
                             guest.Email,
                             guest.Phone,
                             guest.Idcard,
                             guest.Status,
                             guest.Password
                         };
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetGuestByID(int id)
        {
            var guest = _userService.GetUser(id);
            if (guest == null)
            {
                return NotFound();
            }
            return Ok(guest);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<User>> EditGuest(int id, User user)
        {
            if (id != user.UserId)
            {
                return BadRequest();
            }
            _userService.UpdateUserByManagerOrReception(user);
            var aftetUpdate = _userService.GetUser(user.UserId);
            return Ok(aftetUpdate);
        }

        [HttpPost("{id}")]
        public async Task<ActionResult<User>> ChangeStatus(int id, User user)
        {
            if (id != user.UserId)
            {
                return BadRequest();
            }
            _userService.ChangeStatus(user);
            return Ok(_userService.GetUser(id));
        }

    }
}
