using AcademyProject.Models;

namespace AcademyProject.Repositories.Implements
{
    public class ExamOptionRepository : GenericRepository<ExamOption>, IExamOptionRepository
    {
        public ExamOptionRepository(AcademyProjectContext context) : base(context)
        {
        }
    }
}
