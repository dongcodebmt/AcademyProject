using AcademyProject.Models;
using AcademyProject.Repositories;

namespace AcademyProject.Services.Implements
{
    public class ExamRightOptionService : GenericService<ExamRightOption>, IExamRightOptionService
    {
        private IExamRightOptionRepository examRightOptionRepository;
        public ExamRightOptionService(IExamRightOptionRepository examRightOptionRepository) : base(examRightOptionRepository)
        {
            this.examRightOptionRepository = examRightOptionRepository;
        }
    }
}
