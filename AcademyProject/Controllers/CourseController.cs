using AcademyProject.DTOs;
using AcademyProject.Models;
using AcademyProject.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        public IConfiguration configuration;
        private readonly IMapper mapper;
        private readonly IGenericService<Course> courseService;
        private readonly IGenericService<Picture> pictureService;
        public CourseController(IMapper mapper, IConfiguration configuration, IGenericService<Course> courseService, IGenericService<Picture> pictureService)
        {
            this.configuration = configuration;
            this.mapper = mapper;
            this.courseService = courseService;
            this.pictureService = pictureService;
        }

        // GET: api/<CourseController>
        [HttpGet]
        public async Task<ActionResult<CourseDTO>> Get()
        {
            var list = await courseService.GetAll();
            var courses = list.Select(x => mapper.Map<CourseDTO>(x)).ToList();
            foreach (var item in courses)
            {
                var picture = await pictureService.GetById((int)item.PictureId);
                if (picture.PicturePath.Substring(0, 1) == "/")
                {
                    item.PicturePath = configuration["ServerHostName"] + picture.PicturePath;
                } else
                {
                    item.PicturePath = picture.PicturePath;
                }
            }
            return Ok(new { courses });
        }

        // GET api/<CourseController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CourseDTO>> Get(int id)
        {
            var course = await courseService.GetById(id);
            if (course == null)
            {
                return NotFound();
            }
            var courseDTO = mapper.Map<CourseDTO>(course);
            var picture = await pictureService.GetById((int)courseDTO.PictureId);
            if (picture.PicturePath.Substring(0, 1) == "/")
            {
                courseDTO.PicturePath = configuration["ServerHostName"] + picture.PicturePath;
            }
            else
            {
                courseDTO.PicturePath = picture.PicturePath;
            }
            return Ok(new { courseDTO });
        }

        // POST api/<CourseController>
        [HttpPost]
        public async Task<ActionResult<CourseDTO>> Post([FromBody] CourseDTO courseDTO)
        {
            var course = mapper.Map<Course>(courseDTO);
            course = await courseService.Insert(course);
            courseDTO = mapper.Map<CourseDTO>(course);
            return Ok(new { courseDTO });
        }

        // PUT api/<CourseController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult<CourseDTO>> Put(int id, [FromBody] CourseDTO courseDTO)
        {
            var course = await courseService.GetById(id);
            if (course == null)
            {
                return NotFound();
            }
            course.LecturerId = courseDTO.LecturerId;
            course.CategoryId = courseDTO.CategoryId;
            course.Title = courseDTO.Title;
            course = await courseService.Update(course);

            courseDTO = mapper.Map<CourseDTO>(course);

            return Ok(new { courseDTO });
        }

        // DELETE api/<CourseController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await courseService.Delete(id);
            return Ok();
        }
    }
}
