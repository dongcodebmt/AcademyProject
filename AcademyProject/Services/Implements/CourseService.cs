using AcademyProject.Models;
using AcademyProject.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyProject.Services.Implements
{
    public class CourseService : GenericService<Course>, ICourseService
    {
        private ICourseRepository courseRepository;
        public CourseService(ICourseRepository courseRepository) : base(courseRepository)
        {
            this.courseRepository = courseRepository;
        }
    }
}
