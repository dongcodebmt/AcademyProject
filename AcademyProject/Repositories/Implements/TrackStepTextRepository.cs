using AcademyProject.Models;

namespace AcademyProject.Repositories.Implements
{
    public class TrackStepTextRepository : GenericRepository<TrackStepText>, ITrackStepTextRepository
    {
        public TrackStepTextRepository(AcademyProjectContext context) : base(context)
        {
        }
    }
}
