using AcademyProject.Models;
using AcademyProject.Repositories;

namespace AcademyProject.Services.Implements
{
    public class ExamOptionService : GenericService<ExamOption>, IExamOptionService
    {
        private IExamOptionRepository examOptionRepository;
        public ExamOptionService(IExamOptionRepository examOptionRepository) : base(examOptionRepository)
        {
            this.examOptionRepository = examOptionRepository;
        }
    }
}
