using AcademyProject.Models;

namespace AcademyProject.Repositories.Implements
{
    public class CourseRepository : GenericRepository<Course>, ICourseRepository
    {
        public CourseRepository(AcademyProjectContext context) : base(context)
        {

        }
    }
}
