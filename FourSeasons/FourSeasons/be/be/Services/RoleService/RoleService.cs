using be.Models;
using be.Repositories.RoleRepository;

namespace be.Services
{
    public class RoleService : IRoleService
    {
        public IRoleRepository _roleRepo;

        public RoleService(IRoleRepository roleRepository)
        {
            _roleRepo = roleRepository;
        }

        public void AddRole(Role role)
        {
            _roleRepo.AddRole(role);
        }

        public void DeleteRole(int roleId)
        {
            _roleRepo.DeleteRole(roleId);
        }

        public IList<Role> GetAllRoles()
        {
            return _roleRepo.GetAllRoles();
        }

        public Role GetRole(int roleId)
        {
            return _roleRepo.GetRole(roleId);
        }

        public void UpdateRole(Role role)
        {
            _roleRepo.UpdateRole(role);
        }
    }
}
