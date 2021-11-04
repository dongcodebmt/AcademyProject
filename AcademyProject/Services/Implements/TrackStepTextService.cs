using AcademyProject.Models;
using AcademyProject.Repositories;

namespace AcademyProject.Services.Implements
{
    public class TrackStepTextService : GenericService<TrackStepText>, ITrackStepTextService
    {
        private ITrackStepTextRepository trackStepTextRepository;
        public TrackStepTextService(ITrackStepTextRepository trackStepTextRepository) : base(trackStepTextRepository)
        {
            this.trackStepTextRepository = trackStepTextRepository;
        }
    }
}
