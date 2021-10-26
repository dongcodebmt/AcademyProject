using AcademyProject.Models;
using AcademyProject.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyProject.Services.Implements
{
    public class AnswerService : GenericService<Answer>, IAnswerService
    {
        private IAnswerRepository answerRepository;
        public AnswerService(IAnswerRepository answerRepository) : base(answerRepository)
        {
            this.answerRepository = answerRepository;
        }
    }
}
