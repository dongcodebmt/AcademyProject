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
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AcademyProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Administrators, Moderators, Lecturers, Students")]
    public class BlogController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly SharedComponent component;
        private readonly IGenericService<Blog> blogService;
        private readonly IGenericService<BlogComment> blogCommentService;
        private readonly IGenericService<User> userService;
        public BlogController(
            IMapper mapper,
            SharedComponent component,
            IGenericService<Blog> blogService,
            IGenericService<BlogComment> blogCommentService,
            IGenericService<User> userService
        )
        {
            this.mapper = mapper;
            this.component = component;
            this.blogService = blogService;
            this.blogCommentService = blogCommentService;
            this.userService = userService;
        }

        protected int GetCurrentUserId()
        {
            string userId = User.Claims.Where(x => x.Type == "Id").FirstOrDefault()?.Value;
            int id = Convert.ToInt32(userId);
            return id;
        }

        // GET: api/<BlogController>
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<List<BlogDTO>>> Get(int skip = 0, int take = 10, int categoryId = 0)
        {
            List<Blog> list;
            if (categoryId > 0)
            {
                list = (List<Blog>)await blogService.GetList(x => x.IsDeleted == false && x.CategoryId == categoryId, o => o.OrderByDescending(x => x.UpdatedAt), "", skip, take);
            } else
            {
                list = (List<Blog>)await blogService.GetList(x => x.IsDeleted == false, o => o.OrderByDescending(x => x.UpdatedAt), "", skip, take);
            }
            var listBlog = list.Select(x => mapper.Map<BlogDTO>(x)).ToList();
            foreach (var item in listBlog)
            {
                item.Content = component.Truncate(component.StripHTML(item.Content), 95);
                item.PicturePath = await component.GetImageAsync(item.PictureId);
            }
            return Ok(listBlog);
        }

        // GET: api/<BlogController>/{id}
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<BlogDTO>> Get(int id)
        {
            var blog = await blogService.GetById(id);
            if (blog == null)
            {
                return NotFound();
            }
            var blogDTO = mapper.Map<BlogDTO>(blog);
            blogDTO.PicturePath = await component.GetImageAsync(blogDTO.PictureId);
            return Ok(blogDTO);
        }

        // GET: api/<BlogController>/{id}/[Action]
        [HttpGet("{id}/[action]")]
        [AllowAnonymous]
        public async Task<ActionResult<List<BlogCommentDTO>>> Comments(int id, int skip = 0, int take = 10)
        {
            var comments = await blogCommentService.GetList(x => x.BlogId == id, o => o.OrderByDescending(x => x.CreatedAt), "", skip, take);
            var commentDTOs = comments.Select(x => mapper.Map<BlogCommentDTO>(x)).ToList();
            foreach (var item in commentDTOs)
            {
                var user = await userService.GetById(item.UserId);
                item.User = mapper.Map<UserCommentDTO>(user);
                item.User.Picture = await component.GetImageAsync(item.User.PictureId);
            }
            return Ok(commentDTOs);
        }

        // POST: api/<BlogController>
        [HttpPost]
        public async Task<ActionResult<BlogDTO>> Post([FromBody] BlogDTO blogDTO)
        {
            int userId = GetCurrentUserId();
            blogDTO.UserId = userId;
            var blog = mapper.Map<Blog>(blogDTO);
            blog = await blogService.Insert(blog);
            blogDTO = mapper.Map<BlogDTO>(blog);
            return Ok(blogDTO);
        }

        // PUT: api/<BlogController>/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult<BlogDTO>> Put(int id, [FromBody] BlogDTO blogDTO)
        {
            var blog = await blogService.GetById(id);
            if (blog == null)
            {
                return NotFound();
            }

            int userId = GetCurrentUserId();
            if (userId != blog.UserId && !User.IsInRole("Administrators") && !User.IsInRole("Moderators"))
            {
                return Unauthorized();
            }

            blog.CategoryId = blogDTO.CategoryId;
            blog.Title = blogDTO.Title;
            blog.Content = blogDTO.Content;
            blog.UpdatedAt = DateTime.UtcNow;
            blog.PictureId = blogDTO.PictureId;
            await blogService.Update(blog);

            blogDTO.UpdatedAt = blog.UpdatedAt;
            return Ok(blogDTO);
        }

        // DELETE: api/<BlogController>/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var blog = await blogService.GetById(id);
            if (blog == null)
            {
                return NotFound();
            }
            int userId = GetCurrentUserId();
            if (userId != blog.UserId && !User.IsInRole("Administrators") && !User.IsInRole("Moderators"))
            {
                return Unauthorized();
            }

            blog.IsDeleted = true;
            await blogService.Update(blog);
            return Ok();
        }
    }
}
