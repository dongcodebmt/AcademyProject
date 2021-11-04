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
    public class BlogController : ControllerBase
    {
        private readonly IGenericService<Blog> blogService;
        private readonly IMapper mapper;
        public BlogController(IGenericService<Blog> blogService, IMapper mapper)
        {
            this.blogService = blogService;
            this.mapper = mapper;
        }
        // GET: api/<BlogController>
        [HttpGet]
        public async Task<ActionResult<BlogDTO>> Get()
        {
            var list = await blogService.GetAll();
            var listBlog = list.Select(x => mapper.Map<BlogDTO>(x)).ToList();
            return Ok(new { listBlog });
        }

        // GET api/<BlogController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BlogDTO>> Get(int id)
        {
            var blog = await blogService.GetById(id);
            if (blog==null)
            {
                return NotFound();
            }
            var blogDTO = mapper.Map<BlogDTO>(blog);
            return Ok(new { blogDTO });
        }

        // POST api/<BlogController>
        [HttpPost]
        public async Task<ActionResult<BlogDTO>> Post([FromBody] BlogDTO blogDTO)
        {
            var blog = mapper.Map<Blog>(blogDTO);
            blog = await blogService.Insert(blog);
            blogDTO = mapper.Map<BlogDTO>(blog);
            return Ok(new { blogDTO });
        }

        // PUT api/<BlogController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult<BlogDTO>> Put(int id, [FromBody] BlogDTO blogDTO)
        {
            var blog = await blogService.GetById(id);
            if (blog==null)
            {
                return NotFound();
            }
            blog.UserId = blogDTO.UserId;
            blog.CategoryId = blogDTO.CategoryId;
            blog.Title = blogDTO.Title;
            blog.Content = blogDTO.Content;
            blog.CreateAt = blogDTO.CreateAt;
            blog.IsDeleted = blogDTO.IsDeleted;
            return Ok(new { blogDTO });
        }

        // DELETE api/<BlogController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await blogService.Delete(id);
            return Ok();
        }
    }
}
