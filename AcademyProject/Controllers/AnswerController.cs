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
    public class AnswerController : ControllerBase
    {

        private readonly IMapper mapper;
        private readonly SharedComponent component;
        private readonly IGenericService<Answer> answerService;
        private readonly IGenericService<User> userService;

        public AnswerController(
            IMapper mapper,
            SharedComponent component,
            IGenericService<Answer> answerService,
            IGenericService<User> userService
        )
        {
            this.mapper = mapper;
            this.component = component;
            this.answerService = answerService;
            this.userService = userService;
        }

        protected int GetCurrentUserId()
        {
            string userId = User.Claims.Where(x => x.Type == "Id").FirstOrDefault()?.Value;
            int id = Convert.ToInt32(userId);
            return id;
        }

        // POST: api/<AnswerController>
        [HttpPost]
        public async Task<ActionResult<AnswerDTO>> Post([FromBody] AnswerDTO answerDTO)
        {
            int userId = GetCurrentUserId();

            Answer comment = new Answer();
            comment.QuestionId = answerDTO.QuestionId;
            comment.UserId = userId;
            comment.Content = answerDTO.Content;
            comment = await answerService.Insert(comment);
            answerDTO = mapper.Map<AnswerDTO>(comment);

            // Get comment user
            var user = await userService.GetById(answerDTO.UserId);
            answerDTO.User = mapper.Map<UserCommentDTO>(user);
            answerDTO.User.Picture = await component.GetImageAsync(answerDTO.User.PictureId);
            return Ok(answerDTO);
        }

        // PUT: api/<AnswerController>/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult<AnswerDTO>> Put(int id, [FromBody] AnswerDTO answerDTO)
        {
            var answer = await answerService.GetById(id);
            if (answer == null)
            {
                return NotFound();
            }

            var userId = GetCurrentUserId();
            if (userId != answer.UserId && !User.IsInRole("Administrators") && !User.IsInRole("Moderators"))
            {
                return Unauthorized();
            }

            answer.Content = answerDTO.Content;
            answer.UpdatedAt = DateTime.UtcNow;
            await answerService.Update(answer);

            answerDTO.UpdatedAt = answer.UpdatedAt;
            return Ok(answerDTO);
        }

        // DELETE: api/<AnswerController>/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var answer = await answerService.GetById(id);
            if (answer == null)
            {
                return NotFound();
            }
            int userId = GetCurrentUserId();
            if (userId != answer.UserId && !User.IsInRole("Administrators") && !User.IsInRole("Moderators"))
            {
                return Unauthorized();
            }

            await answerService.Delete(id);
            return Ok();
        }
    }
}
