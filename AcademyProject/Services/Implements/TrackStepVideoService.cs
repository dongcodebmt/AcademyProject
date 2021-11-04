using AcademyProject.Models;
using AcademyProject.Repositories;

namespace AcademyProject.Services.Implements
{
    public class TrackStepVideoService : GenericService<TrackStepVideo>, ITrackStepVideoService
    {
        private ITrackStepVideoRepository trackStepVideoRepository;
        public TrackStepVideoService(ITrackStepVideoRepository trackStepVideoRepository) : base(trackStepVideoRepository)
        {
            this.trackStepVideoRepository = trackStepVideoRepository;
        }
    }
}
