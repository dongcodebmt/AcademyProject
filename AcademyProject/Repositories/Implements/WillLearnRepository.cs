using AcademyProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyProject.Repositories.Implements
{
    public class WillLearnRepository : GenericRepository<WillLearn>, IWillLearnRepository
    {
        public WillLearnRepository(AcademyProjectContext context) : base(context)
        {

        }
    }
}
