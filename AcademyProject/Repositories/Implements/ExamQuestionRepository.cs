using AcademyProject.Models;

namespace AcademyProject.Repositories.Implements
{
    public class ExamQuestionRepository : GenericRepository<ExamQuestion>, IExamQuestionRepository
    {
        public ExamQuestionRepository(AcademyProjectContext context) : base(context)
        {
        }
    }
}
