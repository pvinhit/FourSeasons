using be.Controllers;
using be.Models;
using be.Repositories.ReceptionRepository;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace be.Services
{
    public class ReceptionService : IReceptionService
    {
        public IReceptionRepository _receptionRepo;

        public ReceptionService(IReceptionRepository receptionRepository)
        {
            _receptionRepo = receptionRepository;
        }

        public void AddReception(User user)
        {
            _receptionRepo.AddReception(user);
        }

        public void DeleteReception(int userId)
        {
            _receptionRepo.DeleteReception(userId);
        }

        public string GenerateEmail(string fullName)
        {
            return _receptionRepo.GenerateEmail(fullName);
        }

        public IList<User> GetAllReceptions()
        {
            return _receptionRepo.GetAllReceptions();
        }

        public IList<User> GetAllReceptionsWithEmail(string email)
        {
            return _receptionRepo.GetAllReceptionsWithEmail(email);
        }

        public User GetReception(int userId)
        {
            return _receptionRepo.GetReception(userId);
        }

        public void UpdateReception(User user)
        {
            _receptionRepo.UpdateReception(user);
        }

        public void UpdateStatusReception(UserStatus userStatus)
        {
            _receptionRepo.UpdateStatusReception(userStatus);
        }
    }
}
