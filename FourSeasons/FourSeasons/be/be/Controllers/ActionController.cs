using BCrypt.Net;
using be.Helpers;
using be.Models;
using be.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Xml.Linq;

namespace be.Controllers
{
    [ApiController]
    [Route("api")]

    public class ActionController : ControllerBase
    {
        private readonly DbFourSeasonHotelContext _db;
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;

        public ActionController(DbFourSeasonHotelContext db, IConfiguration configuration, IUserService userService)
        {
            this._db = db;
            this._configuration = configuration;
            this._userService = userService;
        }
        [HttpPost("login")]
        public ActionResult Login([FromBody] Login login)
        {
            try
            {
                var result = _userService.Login(login.email, login.password, _configuration);
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody]User user)
        {
            try
            {
                var result = await _userService.Register(user);
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpGet("confirm")]
        public async Task<ActionResult> ConfirmAccount(int id)
        {
            try
            {
                var _user = await _db.Users.FindAsync(id);
                if (_user == null)
                {
                    return NotFound();
                }
                _user.Status = "1";
                _db.Entry(await _db.Users.FirstOrDefaultAsync(x => x.UserId == id)).CurrentValues.SetValues(_user);
                await _db.SaveChangesAsync();
                return Ok(new
                {
                    status = 200,
                    message = "Confirm success"
                });
            }
            catch
            {
                return BadRequest();
            }
        }
    }

    public class Login
    {
        public string email { get; set; }
        public string password { get; set; }
    }
}
