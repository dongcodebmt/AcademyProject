using AcademyProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyProject.Repositories.Implements
{
    public class RequirementRepository : GenericRepository<Requirement>, IRequirementRepository
    {
        public RequirementRepository(AcademyProjectContext context) : base(context)
        {

        }
    }
}
