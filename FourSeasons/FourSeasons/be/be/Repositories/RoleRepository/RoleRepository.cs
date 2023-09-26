using be.Models;

namespace be.Repositories.RoleRepository
{
    public class RoleRepository : IRoleRepository
    {
        private readonly DbFourSeasonHotelContext _context;

        public RoleRepository()
        {
            _context = new DbFourSeasonHotelContext();
        }

        public void AddRole(Role role)
        {
            _context.Roles.Add(role);
            _context.SaveChanges();
        }

        public void DeleteRole(int roleId)
        {
            var item = _context.Roles.Find(roleId);
            _context.Roles.Remove(item);
            _context.SaveChanges();
        }

        public IList<Role> GetAllRoles()
        {
            return _context.Roles.ToList();
        }

        public Role GetRole(int roleId)
        {
            return _context.Roles.Find(roleId);
        }

        public void UpdateRole(Role role)
        {
            Role updateRole = _context.Roles.Find(role.RoleId);
            updateRole = role;
            _context.SaveChanges();
        }
    }
}
