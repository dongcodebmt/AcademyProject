using AcademyProject.Models;
using AcademyProject.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyProject.Services.Implements
{
    public class TrackStepService : GenericService<TrackStep>, ITrackStepService
    {
        private ITrackStepRepository trackStepRepository;
        public TrackStepService(ITrackStepRepository trackStepRepository) : base(trackStepRepository)
        {
            this.trackStepRepository = trackStepRepository;
        }
    }
}
