using AcademyProject.Models;
using AcademyProject.Repositories;

namespace AcademyProject.Services.Implements
{
    public class ExamQuestionService : GenericService<ExamQuestion>, IExamQuestionService
    {
        private IExamQuestionRepository examQuestionRepository;
        public ExamQuestionService(IExamQuestionRepository examQuestionRepository) : base(examQuestionRepository)
        {
            this.examQuestionRepository = examQuestionRepository;
        }
    }
}
