using AcademyProject.Models;
using AcademyProject.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyProject.Services.Implements
{
    public class BlogCommentService : GenericService<BlogComment>, IBlogCommentService
    {
        private IBlogCommentRepository blogCommentRepository;
        public BlogCommentService(IBlogCommentRepository blogCommentRepository) : base(blogCommentRepository)
        {
            this.blogCommentRepository = blogCommentRepository;
        }
    }
}
