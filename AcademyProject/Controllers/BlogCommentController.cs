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
    public class BlogCommentController : ControllerBase
    {
        private readonly IGenericService<BlogComment> blogCommentService;
        private readonly IMapper mapper;
        public BlogCommentController(IGenericService<BlogComment> blogCommentService, IMapper mapper)
        {
            this.blogCommentService = blogCommentService;
            this.mapper = mapper;
        }
        // GET: api/<BlogCommentController>
        [HttpGet]
        public async Task<ActionResult<BlogCommentDTO>> Get()
        {
            var list = await blogCommentService.GetAll();
            var listBlogComment = list.Select(x => mapper.Map<BlogCommentDTO>(x)).ToList();
            return Ok(new { listBlogComment });
        }

        // GET api/<BlogCommentController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BlogCommentDTO>> Get(int id)
        {
            var blogComment = await blogCommentService.GetById(id);
            if (blogComment == null)
            {
                return NotFound();
            }
            var blogCommentDTO = mapper.Map<BlogCommentDTO>(blogComment);
            return Ok(new { blogCommentDTO });
        }

        // POST api/<BlogCommentController>
        [HttpPost]
        public async Task<ActionResult<BlogCommentDTO>> Post([FromBody] BlogCommentDTO blogCommentDTO)
        {
            var blogComment = mapper.Map<BlogComment>(blogCommentDTO);
            blogComment = await blogCommentService.Insert(blogComment);
            blogCommentDTO = mapper.Map<BlogCommentDTO>(blogComment);
            return Ok(new { blogComment });
        }

        // PUT api/<BlogCommentController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult<BlogCommentDTO>> Put(int id, [FromBody] BlogCommentDTO blogCommentDTO)
        {
            var blogComment = await blogCommentService.GetById(id);
            if (blogComment == null)
            {
                return NotFound();
            }
            blogComment.BlogId = blogCommentDTO.BlogId;
            blogComment.UserId = blogCommentDTO.UserId;
            blogComment.Content = blogCommentDTO.Content;
            blogComment.CreateAt = blogCommentDTO.CreateAt;

            blogCommentDTO = mapper.Map<BlogCommentDTO>(blogComment);

            return Ok(new { blogCommentDTO });
        }

        // DELETE api/<BlogCommentController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await blogCommentService.Delete(id);
            return Ok();
        }
    }
}
