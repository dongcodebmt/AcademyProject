using AcademyProject.Models;
using AcademyProject.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyProject.Services.Implements
{
    public class QuestionService : GenericService<Question>, IQuestionService
    {
        private IQuestionRepository questionRepository;
        public QuestionService(IQuestionRepository questionRepository) : base(questionRepository)
        {
            this.questionRepository = questionRepository;
        }
    }
}
