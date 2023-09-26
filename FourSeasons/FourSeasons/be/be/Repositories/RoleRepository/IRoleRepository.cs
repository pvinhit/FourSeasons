using be.Models;

namespace be.Repositories.RoleRepository
{
    public interface IRoleRepository
    {
        Role GetRole(int roleId);
        void AddRole(Role role);
        void UpdateRole(Role role);
        void DeleteRole(int roleId);
        IList<Role> GetAllRoles();
    }
}
