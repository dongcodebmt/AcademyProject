using AcademyProject.DTOs;
using AcademyProject.Models;
using AcademyProject.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyProject.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PictureController : ControllerBase
    {
        private readonly IWebHostEnvironment environment;
        private readonly IGenericService<User> userService;
        private readonly IGenericService<Picture> pictureService;

        public PictureController(IWebHostEnvironment environment, IGenericService<Picture> pictureService, IGenericService<User> userService)
        {
            this.environment = environment;
            this.pictureService = pictureService;
            this.userService = userService;
        }

        // POST: api/<PictureController>/[Action]
        // Upload the picture, save the path to the database and return
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<PictureDTO>> Upload([FromForm] IFormFile file)
        {
            string filePath = FileWriter(file);
            if (filePath == null)
            {
                return BadRequest();
            }
            //Create object image then insert to DB
            Picture picture = new Picture();
            picture.PicturePath = filePath;
            picture = await pictureService.Insert(picture);
            return Ok(picture);
        }

        // POST: api/<PictureController>/[Action]
        // Upload profile picture and update user
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Profile([FromForm] IFormFile file)
        {
            string userId = User.Claims.Where(x => x.Type == "Id").FirstOrDefault()?.Value;
            if (userId == null)
            {
                return Unauthorized();
            }

            string filePath = FileWriter(file);
            if (filePath == null)
            {
                return BadRequest();
            }
            //Create object image then insert to DB
            Picture picture = new Picture();
            picture.PicturePath = filePath;
            picture = await pictureService.Insert(picture);

            //Update user picture
            int id = Convert.ToInt32(userId);
            var users = await userService.GetAll();
            var user = users.FirstOrDefault(u => u.Id == id);
            user.PictureId = picture.Id;
            user.UpdatedAt = DateTime.Now;
            user = await userService.Update(user);
            return Ok();
        }

        private string FileWriter(IFormFile file)
        {
            if (file == null || file.Length < 0)
            {
                return null;
            }
            string path = environment.WebRootPath + "\\images\\";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string fileExtension = Path.GetExtension(file.FileName).ToLower();
            if (fileExtension != ".jpg" && fileExtension != ".gif" && fileExtension != ".png")
            {
                return null;
            }
            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            using (FileStream fileStream = System.IO.File.Create(path + fileName))
            {
                file.CopyTo(fileStream);
                fileStream.Flush();
            }
            return "/images/" + fileName;
        }
    }
}
