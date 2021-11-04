using AcademyProject.Models;

namespace AcademyProject.Repositories.Implements
{
    public class TrackStepVideoRepository : GenericRepository<TrackStepVideo>, ITrackStepVideoRepository
    {
        public TrackStepVideoRepository(AcademyProjectContext context) : base(context)
        {
        }
    }
}
