using be.Controllers;
using be.Helpers;
using be.Models;
using be.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace be.Repositories.UserRepository
{
    public class UserRepository : IUserRepository
    {
        private readonly DbFourSeasonHotelContext _context;

        public UserRepository()
        {
            _context = new DbFourSeasonHotelContext();
        }

        public void AddUser(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public void DeleteUser(int userId)
        {
            var item = _context.Users.Find(userId);
            _context.Users.Remove(item);
            _context.SaveChanges();
        }

        public IList<User> GetAllUsers()
        {
            return _context.Users.ToList();
        }

        public User GetUser(int userId)
        {
            return _context.Users.Find(userId);
        }

        #region KHUYENHTB - USER-INFORMATION/VIEW BOOKING HISTORY/CANCEL BOOKING
        public dynamic GetUserInformation(int userId)
        {
            //get user infor
            var userInfo = (from user in _context.Users
                            join booking in _context.Bookings
                            on user.UserId equals booking.UserId into bookingOfUser
                            from b in bookingOfUser.DefaultIfEmpty()
                            where user.UserId == userId
                            group new { user, b } by new
                            {
                                user.Email,
                                user.Password,
                                user.FullName,
                                user.Address,
                                user.Phone,
                                user.Idcard,
                                user.BirthDay,
                                user.RoleId,
                                user.Avatar
                            } into ubg
                            select new
                            {
                                email = ubg.Key.Email,
                                password = Helper.DecodeFrom64(ubg.Key.Password),
                                fullName = ubg.Key.FullName,
                                address = ubg.Key.Address,
                                phone = ubg.Key.Phone,
                                idcard = ubg.Key.Idcard,
                                birthday = ubg.Key.BirthDay.HasValue ? ubg.Key.BirthDay.Value.ToString("yyyy-MM-dd") : string.Empty,
                                roleId = ubg.Key.RoleId,
                                avatar = ubg.Key.Avatar,
                                bookingTimes = ubg.Where(ub => ub.b.UserId != null).Count(),
                                moneySpend = CurrencyFormat(ubg.Where(ub => ub.b.UserId != null).Select(ub => ub.b.TotalPrice).Sum()).ToString(),
                                cancelBookings = ubg.Where(ub => ub.b.UserId != null && ub.b.Status == "Canceled").Count(),
                            }).FirstOrDefault();
            return userInfo;
        }

        public User UpdateUser(UserUpdateInfor user)
        {
            User updateUser = _context.Users.Find(user.userId);
            updateUser.Password = Helper.EncodePasswordToBase64(user.password);
            updateUser.FullName = user.fullName;
            updateUser.Address = user.address;
            updateUser.Phone = user.phone;
            updateUser.Idcard = user.idcard;
            updateUser.BirthDay = (!string.IsNullOrEmpty(user.birthday)) ? DateTime.Parse(user.birthday) : null;
            updateUser.Avatar = user.avatar;
            _context.SaveChanges();

            return updateUser;
        }

        /// <summary>
        /// Format currency with format like 100,000,000
        /// </summary>
        /// <param name="money"> number you want to format</param>
        /// <returns>currency string after format</returns>
        public static string CurrencyFormat(decimal money)
        {
            return String.Format("{0:0,0}", money);
        }
        #endregion

        #region HIEUVMH15 - LOGIN/REGISTER/CREATE TOKEN
        public string CreateToken(string email, int id, IConfiguration config)
        {
            string role = _context.Roles.Find(id).RoleName;
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Role, role)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                config.GetSection("AppSettings:Token").Value!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds);
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return "bearer " + jwt;
        }
        public object Login(string email, string password, IConfiguration config)
        {
            string token = "";
            var user = _context.Users.FirstOrDefault(x => x.Email == email);
            if (user == null)
            {
                return new
                {
                    status = 404,
                    message = "The account is not found"
                };
            }
            if (Helper.DecodeFrom64(user.Password) != password)
            {
                return new
                {
                    message = "Password is wrong",
                    status = 400
                };
            }
            if (user.Status == "2")
            {
                return new
                {
                    message = "Please check your email to confirm your account",
                    status = 400
                };
            }
            if (user.Status == "0")
            {
                return new
                {
                    message = "Your account is blocked. Please contact admin!",
                    status = 400
                };
            }
            token = CreateToken(user.Email, (int)user.RoleId, config);
            return new
            {
                message = "Login success",
                status = 200,
                data = user,
                token
            };
        }
        public async Task<object> Register(User user)
        {
            if ((_context.Users.Where(x => x.Email == user.Email)).FirstOrDefault() != null)
            {
                return new
                {
                    message = "Email was already registered!",
                    status = 400
                };
            }
            if ((_context.Users.Where(x => x.Idcard == user.Idcard)).FirstOrDefault() != null)
            {
                return new
                {
                    message = "IDCard was already registered!",
                    status = 400
                };
            }
            user.Password = Helper.EncodePasswordToBase64(user.Password);
            if (user.RoleId == null)
            {
                var role = await _context.Roles.Where(x => x.RoleName == "Guest").FirstOrDefaultAsync();
                if (role != null)
                {
                    user.RoleId = role.RoleId;
                }
            }
            try
            {
                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            var id = (user.UserId).ToString();
            if (user.Status != "1")
            {
                try
                {
                    await EmailService.Instance.SendMail(user.Email, 2, id);

                }
                catch
                {
                    return new
                    {
                        message = "Send mail failed!",
                        status = 400
                    };
                }
            }
            return new
            {
                message = "Add account success!",
                status = 200,
            };
        }
        public async Task<object> ForgotPassword(string email)
        {
            var user = await _context.Users.Where(x => x.Email == email).FirstOrDefaultAsync();
            if (user == null)
            {
                return new
                {
                    status = 404,
                    message = "Email is not found"
                };
            }
            if (user.Status == "0")
            {
                return new
                {
                    status = 400,
                    message = "The account is unverified, please check email is verify."
                };
            }
            string _pass = RandomString.Instance.CreateString();
            string pass = Helper.EncodePasswordToBase64(_pass);
            user.Password = pass;
            await _context.SaveChangesAsync();
            var isCheck = await EmailService.Instance.SendMail(user.Email, 1, _pass);
            if (isCheck == false)
            {
                return new
                {
                    message = "Send a new password error!",
                    status = 400,
                };
            }
            return new
            {
                message = "Send a new password success!",
                status = 200,
            };

        }
        #endregion

        #region HUYNG5 - MANAGE USERS BY MANAGER/RECEPTIONIST
        public async void UpdateUserByManagerOrReception(User user)
        {
            var updateGuest = _context.Users.SingleOrDefault(x => x.UserId == user.UserId);
            updateGuest.FullName = user.FullName;
            updateGuest.Phone = user.Phone;
            updateGuest.Idcard = user.Idcard;
            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
        }

        public IList<User> GetGuestList()
        {
            var guestList = _context.Users.Where(x => x.RoleId == 4);
            var count = guestList.Count();
            return guestList.ToList();
        }

        public void ChangeStatus(User user)
        {
            var guest = _context.Users.SingleOrDefault(x => x.UserId == user.UserId);
            guest.Status = user.Status;
            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
        }
        #endregion
    }
}
