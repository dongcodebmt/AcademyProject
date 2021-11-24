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

    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {

        private readonly IMapper mapper;
        private readonly SharedComponent component;
        private readonly IGenericService<User> userService;
        private readonly IGenericService<Course> courseService;
        private readonly IGenericService<Blog> blogService;
        private readonly IGenericService<Question> questionService;
        public SearchController(
            IMapper mapper,
            SharedComponent component,
            IGenericService<User> userService,
            IGenericService<Course> courseService,
            IGenericService<Blog> blogService,
            IGenericService<Question> questionService
        )
        {
            this.mapper = mapper;
            this.component = component;
            this.userService = userService;
            this.courseService = courseService;
            this.blogService = blogService;
            this.questionService = questionService;
        }

        // GET: api/<SearchController>/[Action]/{query}/
        [AllowAnonymous]
        [HttpGet("{query}")]
        public async Task<ActionResult<List<Search>>> Get(string query, int page = 0)
        {
            List<Search> search = new List<Search>();
            if (query.Length < 5)
            {
                return Ok(search);
            }
            int skip = page * 5;
            int take = 5;
            var courses = await courseService.GetList(
                x => x.IsDeleted == false && (x.Title.Contains(query) || x.Description.Contains(query)),
                o => o.OrderByDescending(x => x.Id),
                "",
                skip,
                take
            );
            var questions = await questionService.GetList(
                x => x.IsDeleted == false && (x.Title.Contains(query) || x.Content.Contains(query)),
                o => o.OrderByDescending(x => x.Id),
                "",
                skip,
                take
            );
            var blogs = await blogService.GetList(
                x => x.IsDeleted == false && (x.Title.Contains(query) || x.Content.Contains(query)),
                o => o.OrderByDescending(x => x.Id),
                "",
                skip,
                take
            );

            foreach (var item in courses)
            {
                Search s = new Search();
                s.Id = item.Id;
                s.Type = "course";
                s.Title = item.Title;
                s.Description = component.Truncate(component.StripHTML(item.Description), 100);
                s.PicturePath = await component.GetImageAsync(item.PictureId);
                search.Add(s);
            }
            foreach (var item in questions)
            {
                Search s = new Search();
                s.Id = item.Id;
                s.Type = "question";
                s.Title = item.Title;
                s.Description = component.Truncate(component.StripHTML(item.Content), 100);
                s.PicturePath = await component.GetImageAsync(item.PictureId);
                search.Add(s);
            }
            foreach (var item in blogs)
            {
                Search s = new Search();
                s.Id = item.Id;
                s.Type = "blog";
                s.Title = item.Title;
                s.Description = component.Truncate(component.StripHTML(item.Content), 100);
                s.PicturePath = await component.GetImageAsync(item.PictureId);
                search.Add(s);
            }
            return Ok(search);
        }
    }

    public class Search
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string PicturePath { get; set; }
    }
}
