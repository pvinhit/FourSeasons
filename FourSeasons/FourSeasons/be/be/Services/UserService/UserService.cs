using be.Controllers;
using be.Helpers;
using be.Models;
using be.Repositories.UserRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace be.Services
{
    public class UserService : IUserService
    {
        public IUserRepository _userRepo;

        public UserService(IUserRepository userRepository)
        {
            _userRepo = userRepository;
        }

        public void AddUser(User user)
        {
            _userRepo.AddUser(user);
        }

        public void DeleteUser(int userId)
        {
            _userRepo.DeleteUser(userId);
        }

        public IList<User> GetAllUsers()
        {
            return _userRepo.GetAllUsers();
        }

        public User GetUser(int userId)
        {
            return _userRepo.GetUser(userId);
        }

        #region KHUYENHTB - USER-INFORMATION/VIEW BOOKING HISTORY/CANCEL BOOKING
        public dynamic GetUserInformation(int userId)
        {
            return _userRepo.GetUserInformation(userId);
        }

        public User UpdateUser(UserUpdateInfor user)
        {
            return _userRepo.UpdateUser(user);
        }
        #endregion

        #region HIEUVMH15 - LOGIN/REGISTER/CREATE TOKEN
        public object Login(string email, string password, IConfiguration config)
        {
            return _userRepo.Login(email, password, config);
        }
        public async Task<object> Register(User user)
        {
            return await _userRepo.Register(user);
        }
        public async Task<object> ForgotPassword(string email)
        {
            return await _userRepo.ForgotPassword(email);
        }
        #endregion

        #region HUYNG5 - MANAGE USERS BY MANAGER/RECEPTIONIST
        public async void UpdateUserByManagerOrReception(User user)
        {
            _userRepo.UpdateUserByManagerOrReception(user);
        }

        public IList<User> GetGuestList()
        {
            return _userRepo.GetGuestList();
        }

        public void ChangeStatus(User user)
        {
            _userRepo.ChangeStatus(user);
        }
        #endregion
    }
}
