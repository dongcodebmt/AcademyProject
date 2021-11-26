using AcademyProject.DTOs;
using AcademyProject.Models;
using AcademyProject.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Administrators, Moderators, Lecturers, Students")]
    public class BlogCommentController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly SharedComponent component;
        private readonly IGenericService<BlogComment> blogCommentService;
        private readonly IGenericService<User> userService;

        public BlogCommentController(
            IMapper mapper,
            SharedComponent component,
            IGenericService<BlogComment> blogCommentService,
            IGenericService<User> userService
        )
        {
            this.mapper = mapper;
            this.component = component;
            this.blogCommentService = blogCommentService;
            this.userService = userService;
        }

        protected int GetCurrentUserId()
        {
            string userId = User.Claims.Where(x => x.Type == "Id").FirstOrDefault()?.Value;
            int id = Convert.ToInt32(userId);
            return id;
        }

        // GET: api/<BlogCommentController>
        //[HttpGet]
        //public async Task<ActionResult<BlogCommentDTO>> Get()
        //{
        //    var list = await blogCommentService.GetAll();
        //    var listBlogComment = list.Select(x => mapper.Map<BlogCommentDTO>(x)).ToList();
        //    return Ok(listBlogComment);
        //}

        // GET: api/<BlogCommentController>/{id}
        //[HttpGet("{id}")]
        //public async Task<ActionResult<BlogCommentDTO>> Get(int id)
        //{
        //    var blogComment = await blogCommentService.GetById(id);
        //    if (blogComment == null)
        //    {
        //        return NotFound();
        //    }
        //    var blogCommentDTO = mapper.Map<BlogCommentDTO>(blogComment);
        //    return Ok(blogCommentDTO);
        //}

        // POST: api/<BlogCommentController>
        [HttpPost]
        public async Task<ActionResult<BlogCommentDTO>> Post([FromBody] BlogCommentDTO blogCommentDTO)
        {
            int userId = GetCurrentUserId();

            BlogComment comment = new BlogComment();
            comment.BlogId = blogCommentDTO.BlogId;
            comment.UserId = userId;
            comment.Content = blogCommentDTO.Content;
            comment = await blogCommentService.Insert(comment);
            blogCommentDTO = mapper.Map<BlogCommentDTO>(comment);

            // Get comment user
            var user = await userService.GetById(blogCommentDTO.UserId);
            blogCommentDTO.User = mapper.Map<UserCommentDTO>(user);
            blogCommentDTO.User.Picture = await component.GetImageAsync(blogCommentDTO.User.PictureId);
            return Ok(blogCommentDTO);
        }

        // PUT: api/<BlogCommentController>/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult<BlogCommentDTO>> Put(int id, [FromBody] BlogCommentDTO blogCommentDTO)
        {
            var blogComment = await blogCommentService.GetById(id);
            if (blogComment == null)
            {
                return NotFound();
            }

            int userId = GetCurrentUserId();
            if (userId != blogComment.UserId && !User.IsInRole("Administrators") && !User.IsInRole("Moderators"))
            {
                return Unauthorized();
            }

            blogComment.Content = blogCommentDTO.Content;
            blogComment.UpdatedAt = DateTime.UtcNow;
            await blogCommentService.Update(blogComment);

            blogCommentDTO.UpdatedAt = blogComment.UpdatedAt;
            return Ok(blogCommentDTO);
        }

        // DELETE: api/<BlogCommentController>/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var blogComment = await blogCommentService.GetById(id);
            if (blogComment == null)
            {
                return NotFound();
            }
            int userId = GetCurrentUserId();
            if (userId != blogComment.UserId && !User.IsInRole("Administrators") && !User.IsInRole("Moderators"))
            {
                return Unauthorized();
            }

            await blogCommentService.Delete(id);
            return Ok();
        }
    }
}
