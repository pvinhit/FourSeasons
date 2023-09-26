using be.Controllers;
using be.Models;
using be.Repositories.ManagerRepository;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace be.Services
{
    public class ManagerService : IManagerService
    {
        public IManagerRepository _managerRepo;

        public ManagerService(IManagerRepository managerRepository)
        {
            _managerRepo = managerRepository;
        }

        public void AddManager(User user)
        {
            _managerRepo.AddManager(user);
        }

        public void DeleteManager(int userId)
        {
            _managerRepo.DeleteManager(userId);
        }

        public IList<User> GetAllManagers()
        {
            return _managerRepo.GetAllManagers();
        }

        public User GetManager(int userId)
        {
            return _managerRepo.GetManager(userId); 
        }

        public void UpdateManager(User user)
        {
            _managerRepo.UpdateManager(user);
        }

        public void UpdateStatusManager(UserStatus userStatus)
        {
            _managerRepo.UpdateStatusManager(userStatus);
        }

        public string GenerateEmail(string fullName)
        {
            return _managerRepo.GenerateEmail(fullName);
        }

        public IList<User> GetAllManagersWithEmail(string email)
        {
            return _managerRepo.GetAllManagersWithEmail(email);
        }

        public IList<string> GetManagerIdCard()
        {
            return _managerRepo.GetManagerIdCard();
        }
    }
}
