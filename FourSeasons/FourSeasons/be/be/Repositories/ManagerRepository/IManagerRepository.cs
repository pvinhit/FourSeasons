using be.Models;

namespace be.Repositories.ManagerRepository
{
    public interface IManagerRepository
    {
        User GetManager(int userId);
        void AddManager(User user);
        void UpdateManager(User user);
        void DeleteManager(int userId);
        IList<User> GetAllManagers();
        void UpdateStatusManager(UserStatus userStatus);
        public string GenerateEmail(string fullName);
        public IList<User> GetAllManagersWithEmail(string email);
        public IList<string> GetManagerIdCard();
    }
}
