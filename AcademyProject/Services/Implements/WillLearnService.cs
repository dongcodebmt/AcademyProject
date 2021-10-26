using AcademyProject.Models;
using AcademyProject.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyProject.Services.Implements
{
    public class WillLearnService : GenericService<WillLearn>, IWillLearnService
    {
        private IWillLearnRepository willLearnRepository;
        public WillLearnService(IWillLearnRepository willLearnRepository) : base(willLearnRepository)
        {
            this.willLearnRepository = willLearnRepository;
        }
    }
}
