using AcademyProject.Models;

namespace AcademyProject.Repositories.Implements
{
    public class ExamRightOptionRepository : GenericRepository<ExamRightOption>, IExamRightOptionRepository
    {
        public ExamRightOptionRepository(AcademyProjectContext context) : base(context)
        {
        }
    }
}
