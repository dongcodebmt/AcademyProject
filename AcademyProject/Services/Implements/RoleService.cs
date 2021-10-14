using AcademyProject.Models;
using AcademyProject.Repositories;

namespace AcademyProject.Services.Implements
{
    public class RoleService : GenericService<Role>, IRoleService
    {
        private IRoleRepository roleRepository;
        public RoleService(IRoleRepository roleRepository) : base(roleRepository)
        {
            this.roleRepository = roleRepository;
        }
    }
}
