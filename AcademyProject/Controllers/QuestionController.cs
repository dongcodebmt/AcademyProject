using AcademyProject.DTOs;
using AcademyProject.Models;
using AcademyProject.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
    [Authorize(Roles = "Administrators, Moderators, Lecturers, Students")]
    public class QuestionController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly SharedComponent component;
        private readonly IGenericService<Question> questionService;
        private readonly IGenericService<Answer> answerService;
        private readonly IGenericService<User> userService;
        public QuestionController(
            IMapper mapper,
            SharedComponent component,
            IGenericService<Question> questionService,
            IGenericService<Answer> answerService,
            IGenericService<User> userService
        )
        {
            this.mapper = mapper;
            this.component = component;
            this.questionService = questionService;
            this.answerService = answerService;
            this.userService = userService;
        }

        protected int GetCurrentUserId()
        {
            string userId = User.Claims.Where(x => x.Type == "Id").FirstOrDefault()?.Value;
            int id = Convert.ToInt32(userId);
            return id;
        }

        // GET: api/<QuestionController>
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<List<QuestionDTO>>> Get(int skip = 0, int take = 10, int categoryId = 0)
        {
            List<Question> list;
            if (categoryId > 0)
            {
                list = (List<Question>)await questionService.GetList(x => x.IsDeleted == false && x.CategoryId == categoryId, o => o.OrderByDescending(x => x.UpdatedAt), "", skip, take);
            }
            else
            {
                list = (List<Question>)await questionService.GetList(x => x.IsDeleted == false, o => o.OrderByDescending(x => x.UpdatedAt), "", skip, take);
            }
            var questionDTOs = list.Select(x => mapper.Map<QuestionDTO>(x)).OrderByDescending(x => x.UpdatedAt).ToList();
            foreach (var item in questionDTOs)
            {
                item.Content = component.Truncate(component.StripHTML(item.Content), 128);
                item.PicturePath = await component.GetImageAsync(item.PictureId);
            }
            return Ok(questionDTOs);
        }

        // GET: api/<QuestionController>/{id}
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<QuestionDTO>> Get(int id)
        {
            var question = await questionService.GetById(id);
            if (question == null)
            {
                return NotFound();
            }
            var questionDTO = mapper.Map<QuestionDTO>(question);
            questionDTO.PicturePath = await component.GetImageAsync(questionDTO.PictureId);
            return Ok(questionDTO);
        }

        // GET: api/<QuestionController>/{id}/[Action]
        [HttpGet("{id}/[action]")]
        [AllowAnonymous]
        public async Task<ActionResult<List<AnswerDTO>>> Comments(int id, int skip = 0, int take = 10)
        {
            var comments = await answerService.GetList(x => x.QuestionId == id, o => o.OrderByDescending(x => x.CreatedAt), "", skip, take);
            var commentDTOs = comments.Select(x => mapper.Map<AnswerDTO>(x)).ToList();
            foreach (var item in commentDTOs)
            {
                var user = await userService.GetById(item.UserId);
                item.User = mapper.Map<UserCommentDTO>(user);
                item.User.Picture = await component.GetImageAsync(item.User.PictureId);
            }
            return Ok(commentDTOs);
        }

        // POST: api/<QuestionController>
        [HttpPost]
        public async Task<ActionResult<QuestionDTO>> Post([FromBody] QuestionDTO questionDTO)
        {
            int userId = GetCurrentUserId();
            questionDTO.UserId = userId;
            var question = mapper.Map<Question>(questionDTO);
            question = await questionService.Insert(question);
            questionDTO = mapper.Map<QuestionDTO>(question);
            return Ok(questionDTO);
        }

        // PUT: api/<QuestionController>/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult<QuestionDTO>> Put(int id, [FromBody] QuestionDTO questionDTO)
        {
            var question = await questionService.GetById(id);
            if (question == null)
            {
                return NotFound();
            }

            int userId = GetCurrentUserId();
            if (userId != question.UserId && !User.IsInRole("Administrators") && !User.IsInRole("Moderators"))
            {
                return Unauthorized();
            }

            question.CategoryId = questionDTO.CategoryId;
            question.Title = questionDTO.Title;
            question.Content = questionDTO.Content;
            question.UpdatedAt = DateTime.Now;
            question.PictureId = questionDTO.PictureId;
            await questionService.Update(question);

            questionDTO.UpdatedAt = question.UpdatedAt;
            return Ok(questionDTO);
        }

        // DELETE: api/<QuestionController>/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var question = await questionService.GetById(id);
            if (question == null)
            {
                return NotFound();
            }

            int userId = GetCurrentUserId();
            if (userId != question.UserId && !User.IsInRole("Administrators") && !User.IsInRole("Moderators"))
            {
                return Unauthorized();
            }

            question.IsDeleted = true;
            await questionService.Update(question);
            return Ok();
        }
    }
}
