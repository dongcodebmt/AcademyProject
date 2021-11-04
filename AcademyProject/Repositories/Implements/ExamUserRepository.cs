using AcademyProject.Models;

namespace AcademyProject.Repositories.Implements
{
    public class ExamUserRepository : GenericRepository<ExamUser>, IExamUserRepository
    {
        public ExamUserRepository(AcademyProjectContext context) : base(context)
        {
        }
    }
}
