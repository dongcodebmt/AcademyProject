﻿using AcademyProject.DTOs;
using AcademyProject.Models;
using AcademyProject.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CourseController : ControllerBase
    {
        private readonly ICourseService courseService;
        private readonly IMapper mapper;
        public CourseController(ICourseService courseService, IMapper mapper)
        {
            this.courseService = courseService;
            this.mapper = mapper;
        }

        // GET: api/<CourseController>
        [HttpGet]
        public async Task<ActionResult<CourseDTO>> Get()
        {
            var list = await courseService.GetAll();
            var listCourse = list.Select(x => mapper.Map<CourseDTO>(x)).ToList();
            return Ok(new { listCourse });
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
            return Ok(new { courseDTO });
        }

        // POST api/<CourseController>
        [HttpPost]
        public async Task<ActionResult<CourseDTO>> Post([FromBody] CourseDTO courseDTO)
        {
            var course = mapper.Map<Course>(courseDTO);
            course = await courseService.Insert(course);
            courseDTO = mapper.Map<CourseDTO>(course);
            return Ok(new { course });
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
