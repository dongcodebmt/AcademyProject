using AcademyProject.Models;
using AcademyProject.Repositories.Implements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyProject.Services.Implements
{
    public class BlogService : GenericService<Blog>, IBlogService
    {
        private IBlogRepository blogRepository;
        public BlogService(IBlogRepository blogRepository) : base(blogRepository)
        {
            this.blogRepository = blogRepository;
        }
    }
}
