using be.Models;
using System.Data.Entity.Infrastructure;

namespace be.Repositories.ReceptionRepository
{
    public class ReceptionRepository : IReceptionRepository
    {
        private readonly DbFourSeasonHotelContext _context;

        public ReceptionRepository()
        {
            _context = new DbFourSeasonHotelContext();
        }

        public void AddReception(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public void DeleteReception(int userId)
        {
            var item = _context.Users.Find(userId);

            _context.Users.Remove(item);
            _context.SaveChanges();
        }

        public string GenerateEmail(string fullName)
        {
            string email = fullName.ToLower().Replace(" ", "") + "@fourseasons.com";
            int count = 0;
            IList<User> listUser = GetAllReceptionsWithEmail(email);
            count = listUser.Count;
            while (count > 0)
            {
                count++;
                email = fullName.ToLower().Replace(" ", "") + count + "@fourseasons.com";
                listUser = GetAllReceptionsWithEmail(email);
                count = listUser.Count;
            }


            return email;
        }

        public IList<User> GetAllReceptions()
        {
            return _context.Users.Where(user => user.RoleId == 3).ToList();
        }

        public IList<User> GetAllReceptionsWithEmail(string email)
        {
            return _context.Users.Where(user => user.Email == email).ToList();
        }

        public User GetReception(int userId)
        {
            return _context.Users.Find(userId);
        }

        public void UpdateReception(User user)
        {
            User update = _context.Users.Where(x => x.UserId == user.UserId).FirstOrDefault();

            update.Idcard = user.Idcard;
            update.Address = user.Address;
            update.Phone = user.Phone;
            update.Email = user.Email;
            update.BirthDay = user.BirthDay;
            update.FullName = user.FullName;

            try
            {
                _context.SaveChanges();

            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw;
            }
        }

        public void UpdateStatusReception(UserStatus userStatus)
        {
            User manager = _context.Users.Find(userStatus.UserId);
            if (manager != null)
            {
                manager.Status = userStatus.Status;
                _context.SaveChanges();
            }
        }
    }
}
