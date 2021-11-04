using AcademyProject.Models;
using AcademyProject.Repositories;

namespace AcademyProject.Services.Implements
{
    public class ExamDetailService : GenericService<ExamDetail>, IExamDetailService
    {
        private IExamDetailRepository examDetailRepository;
        public ExamDetailService(IExamDetailRepository examDetailRepository) : base(examDetailRepository)
        {
            this.examDetailRepository = examDetailRepository;
        }
    }
}
