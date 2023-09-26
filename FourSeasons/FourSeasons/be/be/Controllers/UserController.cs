using be.Helpers;
using be.Models;
using be.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace be.Controllers
{
    [ApiController]
    [Route("api/user")]

    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly DbFourSeasonHotelContext _db;
        public UserController(DbFourSeasonHotelContext db, IUserService userService)
        {
            this._db = db;
            this._userService = userService;
        }

        #region HIEUVMH15 - LOGIN/REGISTER/CREATE TOKEN
        [HttpGet("info")]
        public async Task<ActionResult> GetInfo(string token)
        {
            string _token = token.Split(' ')[1];
            if (_token == null)
            {
                return Ok(new
                {
                    message = "Token is wrong!",
                    status = 400
                });
            }
            var handle = new JwtSecurityTokenHandler();
            string email = handle.ReadJwtToken(_token).Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress")?.Value;
            var user = _db.Users.Where(x => x.Email == email).FirstOrDefault();
            if (user == null)
            {
                return Ok(new
                {
                    message = "User is not found!",
                    status = 404
                });
            }
            return Ok(new
            {
                message = "Get information success!",
                status = 200,
                data = user
            });
        }
        [HttpGet("search")]
        public async Task<ActionResult> GetUserByEmail(string email)
        {
            var user = await _db.Users.Where(x => x.Email.Contains(email)).FirstOrDefaultAsync();
            if (user == null)
            {
                return Ok(new
                {
                    status = 404,
                    message = "Email is not found"
                });
            }
            return Ok(new
            {
                status = 200,
                message = "Email is found"
            });
        }
        [HttpPost("forgot")]
        public async Task<ActionResult> ForgotPassword([FromBody] string email)
        {
            try
            {
                var result = await _userService.ForgotPassword(email);
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }
        #endregion

        #region KHUYENHTB - USER-INFORMATION/VIEW BOOKING HISTORY/CANCEL BOOKING
        [HttpGet("GetUserInformation")]
        public ActionResult GetUserInformation(int userId)
        {
            try
            {
                var user = _userService.GetUserInformation(userId);
                return Ok(user);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("UpdateUser")]
        public ActionResult UpdateUser([FromBody] UserUpdateInfor user)
        {
            try
            {
                var updateData = _userService.UpdateUser(user);
                return Ok(updateData);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion
    }
}
