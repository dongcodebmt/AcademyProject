using AcademyProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyProject.Repositories.Implements
{
    public class TrackRepository : GenericRepository<Track>, ITrackRepository
    {
        public TrackRepository(AcademyProjectContext context) : base(context)
        {

        }
    }
}
