using AcademyProject.Models;
using AcademyProject.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyProject.Services.Implements
{
    public class ImageService : GenericService<Image>, IImageService
    {
        private IImageRepository imageRepository;
        public ImageService(IImageRepository imageRepository) : base(imageRepository)
        {
            this.imageRepository = imageRepository;
        }
    }
}
