using AcademyProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyProject.Repositories.Implements
{
    public class BlogCommentRepository : GenericRepository<BlogComment>, IBlogCommentRepository
    {
        public BlogCommentRepository(AcademyProjectContext context) : base(context)
        {

        }
    }
}
