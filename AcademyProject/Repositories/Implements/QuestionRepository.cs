using AcademyProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyProject.Repositories.Implements
{
    public class QuestionRepository : GenericRepository<Question>, IQuestionRepository
    {
        public QuestionRepository(AcademyProjectContext context) : base(context)
        {

        }
    }
}
