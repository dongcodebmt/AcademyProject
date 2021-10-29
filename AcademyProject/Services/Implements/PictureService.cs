using AcademyProject.Models;
using AcademyProject.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyProject.Services.Implements
{
    public class PictureService : GenericService<Picture>, IPictureService
    {
        private IPictureRepository pictureRepository;
        public PictureService(IPictureRepository pictureRepository) : base(pictureRepository)
        {
            this.pictureRepository = pictureRepository;
        }
    }
}
