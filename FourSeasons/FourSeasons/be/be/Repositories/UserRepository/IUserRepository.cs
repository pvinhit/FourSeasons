using be.Controllers;
using be.Models;

namespace be.Repositories.UserRepository
{
    public interface IUserRepository
    {
        void AddUser(User user);
        void DeleteUser(int userId);
        IList<User> GetAllUsers();
        User GetUser(int userId);
        dynamic GetUserInformation(int userId);
        User UpdateUser(UserUpdateInfor user);
        object Login(string email, string password, IConfiguration config);
        Task<object> Register(User user);
        Task<object> ForgotPassword(string email);
        void UpdateUserByManagerOrReception(User user);
        IList<User> GetGuestList();
        void ChangeStatus(User user);
    }
}
