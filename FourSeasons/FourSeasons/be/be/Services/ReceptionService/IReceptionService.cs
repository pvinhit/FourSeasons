using be.Controllers;
using be.Models;

namespace be.Services
{
    public interface IReceptionService
    {
        User GetReception(int userId);
        void AddReception(User user);
        void UpdateReception(User user);
        void DeleteReception(int userId);
        IList<User> GetAllReceptions();
        void UpdateStatusReception(UserStatus userStatus);
        public string GenerateEmail(string fullName);
        public IList<User> GetAllReceptionsWithEmail(string email);
    }
}
