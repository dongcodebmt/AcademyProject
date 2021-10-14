using AcademyProject.Models;
using AcademyProject.Repositories;

namespace AcademyProject.Services.Implements
{
    public class UserRoleService : GenericService<UserRole>, IUserRoleService
    {
        private IUserRoleRepository userRoleRepository;
        public UserRoleService(IUserRoleRepository userRoleRepository) : base(userRoleRepository)
        {
            this.userRoleRepository = userRoleRepository;
        }
    }
}
