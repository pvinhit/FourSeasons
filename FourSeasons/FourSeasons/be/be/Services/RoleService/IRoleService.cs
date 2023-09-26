using be.Models;

namespace be.Services
{
    public interface IRoleService
    {
        Role GetRole(int roleId);
        void AddRole(Role role);
        void UpdateRole(Role role);
        void DeleteRole(int roleId);
        IList<Role> GetAllRoles();
    }
}
