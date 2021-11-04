using AcademyProject.Models;

namespace AcademyProject.Repositories.Implements
{
    public class ExamDetailRepository : GenericRepository<ExamDetail>, IExamDetailRepository
    {
        public ExamDetailRepository(AcademyProjectContext context) : base(context)
        {
        }
    }
}
