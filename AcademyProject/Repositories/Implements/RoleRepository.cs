using AcademyProject.Models;

namespace AcademyProject.Repositories.Implements
{
    public class RoleRepository : GenericRepository<Role>, IRoleRepository
    {
        public RoleRepository(AcademyProjectContext context) : base(context)
        {

        }
    }
}
