using be.Models;
using System.Data.Entity.Infrastructure;

namespace be.Repositories.ManagerRepository
{
    public class ManagerRepository : IManagerRepository
    {
        private readonly DbFourSeasonHotelContext _context;

        public ManagerRepository()
        {
            _context = new DbFourSeasonHotelContext();
        }

        public void AddManager(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public void DeleteManager(int userId)
        {
            var item = _context.Users.Find(userId);

            _context.Users.Remove(item);
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public IList<User> GetAllManagers()
        {
            return _context.Users.Where(user => user.RoleId == 2).ToList();
        }

        public User GetManager(int userId)
        {
            return _context.Users.Find(userId);
        }

        public void UpdateManager(User user)
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

        public void UpdateStatusManager(UserStatus userStatus)
        {
            User manager = _context.Users.Find(userStatus.UserId);
            if (manager != null)
            {
                manager.Status = userStatus.Status;
                _context.SaveChanges();
            }
        }
        public string GenerateEmail(string fullName)
        {
            string email = fullName.ToLower().Replace(" ", "") + "@fourseasons.com";
            int count = 0;
            IList<User> listUser = GetAllManagersWithEmail(email);
            count = listUser.Count;
            while (count > 0)
            {
                count++;
                email = fullName.ToLower().Replace(" ", "") + count + "@fourseasons.com";
                listUser = GetAllManagersWithEmail(email);
                count = listUser.Count;
            }


            return email;
        }
        public IList<User> GetAllManagersWithEmail(string email)
        {
            return _context.Users.Where(user => user.Email == email).ToList();
        }

        public IList<string> GetManagerIdCard()
        {
            return _context.Users.Where(user => user.Idcard != null)
                .Select(user => user.Idcard)
                .ToList();
        }
    }
}
