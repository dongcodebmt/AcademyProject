using AcademyProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyProject.Repositories.Implements
{
    public class ImageRepository : GenericRepository<Image>, IImageRepository
    {
        public ImageRepository(AcademyProjectContext context) : base(context)
        {

        }
    }
}
