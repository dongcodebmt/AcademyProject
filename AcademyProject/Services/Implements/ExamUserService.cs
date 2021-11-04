using AcademyProject.Models;
using AcademyProject.Repositories;

namespace AcademyProject.Services.Implements
{
    public class ExamUserService : GenericService<ExamUser>, IExamUserService
    {
        private IExamUserRepository examUserRepository;
        public ExamUserService(IExamUserRepository examUserRepository) : base(examUserRepository)
        {
            this.examUserRepository = examUserRepository;
        }
    }
}
