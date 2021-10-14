using AcademyProject.Models;

namespace AcademyProject.Repositories.Implements
{
    public class UserRoleRepository : GenericRepository<UserRole>, IUserRoleRepository
    {
        public UserRoleRepository(AcademyProjectContext context) : base(context)
        {

        }
    }
}
