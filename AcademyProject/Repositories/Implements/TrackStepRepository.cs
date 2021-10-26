using AcademyProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyProject.Repositories.Implements
{
    public class TrackStepRepository : GenericRepository<TrackStep>, ITrackStepRepository
    {
        public TrackStepRepository(AcademyProjectContext context) : base(context)
        {

        }
    }
}
