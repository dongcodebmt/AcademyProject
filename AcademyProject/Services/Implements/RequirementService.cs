using AcademyProject.Models;
using AcademyProject.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyProject.Services.Implements
{
    public class RequirementService : GenericService<Requirement>, IRequirementService
    {
        private IRequirementRepository requirementRepository;
        public RequirementService(IRequirementRepository requirementRepository) : base(requirementRepository)
        {
            this.requirementRepository = requirementRepository;
        }
    }
}
