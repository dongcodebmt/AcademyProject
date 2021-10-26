using AcademyProject.Models;
using AcademyProject.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyProject.Services.Implements
{
    public class TrackService : GenericService<Track>, ITrackService
    {
        private ITrackRepository trackRepository;
        public TrackService(ITrackRepository trackRepository) : base(trackRepository)
        {
            this.trackRepository = trackRepository;
        }
    }
}
