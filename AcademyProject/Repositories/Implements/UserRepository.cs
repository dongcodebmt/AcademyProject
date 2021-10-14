using AcademyProject.Models;

namespace AcademyProject.Repositories.Implements
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(AcademyProjectContext context) : base (context)
        {

        }
    }
}
