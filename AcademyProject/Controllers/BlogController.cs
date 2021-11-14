using AcademyProject.DTOs;
using AcademyProject.Models;
using AcademyProject.Services;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IGenericService<Blog> blogService;
        public BlogController(IMapper mapper, IGenericService<Blog> blogService)
        {
            this.mapper = mapper;
            this.blogService = blogService;
        }

        [HttpGet]
        public async Task<ActionResult<List<BlogDTO>>> Get()
        {
            var list = await blogService.GetAll();
            var listBlog = list.Select(x => mapper.Map<BlogDTO>(x)).ToList();
            return Ok(listBlog);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BlogDTO>> Get(int id)
        {
            var blog = await blogService.GetById(id);
            if (blog==null)
            {
                return NotFound();
            }
            var blogDTO = mapper.Map<BlogDTO>(blog);
            return Ok(blogDTO);
        }

        [HttpPost]
        public async Task<ActionResult<BlogDTO>> Post([FromBody] BlogDTO blogDTO)
        {
            string userId = User.Claims.Where(x => x.Type == "Id").FirstOrDefault()?.Value;
            if (userId == null)
            {
                return Unauthorized();
            }
            blogDTO.UserId = Convert.ToInt32(userId);
            var blog = mapper.Map<Blog>(blogDTO);
            blog = await blogService.Insert(blog);
            blogDTO = mapper.Map<BlogDTO>(blog);
            return Ok(blogDTO);
        }

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
            blog.CreatedAt = blogDTO.CreatedAt;
            blog.IsDeleted = blogDTO.IsDeleted;
            return Ok(blogDTO);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await blogService.Delete(id);
            return Ok();
        }
    }
}
