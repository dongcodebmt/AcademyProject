using AcademyProject.DTOs;
using AcademyProject.Models;
using AcademyProject.Services;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AcademyProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly IImageService imageService;
        private readonly IMapper mapper;
        public ImageController(IImageService imageService, IMapper mapper)
        {
            this.imageService = imageService;
            this.mapper = mapper;
        }
        // GET: api/<ImageController>
        [HttpGet]
        public async Task<ActionResult<ImageDTO>> Get()
        {
            var list = await imageService.GetAll();
            var listImage = list.Select(x => mapper.Map<ImageDTO>(x)).ToList();
            return Ok(new { listImage });
        }

        // GET api/<ImageController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ImageDTO>> Get(int id)
        {
            var image = await imageService.GetById(id);
            if (image == null)
            {
                return NotFound();
            }
            var imageDTO = mapper.Map<ImageDTO>(image);
            return Ok(new { imageDTO });
        }

        // POST api/<ImageController>
        [HttpPost]
        public async Task<ActionResult<ImageDTO>> Post([FromBody] ImageDTO imageDTO)
        {
            var image = mapper.Map<Image>(imageDTO);
            image = await imageService.Insert(image);
            imageDTO = mapper.Map<ImageDTO>(image);
            return Ok(new { image });
        }

        // PUT api/<ImageController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult<ImageDTO>> Put(int id, [FromBody] ImageDTO imageDTO)
        {
            var image = await imageService.GetById(id);
            if (image == null)
            {
                return NotFound();
            }
            image.UserId = imageDTO.UserId;
            image.ImagePath = imageDTO.ImagePath;
            
            image = await imageService.Update(image);

            imageDTO = mapper.Map<ImageDTO>(image);

            return Ok(new { imageDTO });
        }

        // DELETE api/<ImageController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await imageService.Delete(id);
            return Ok();
        }
    }
}
