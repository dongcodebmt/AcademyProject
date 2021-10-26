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
    public class QuestionController : ControllerBase
    {
        private readonly IQuestionService questionService;
        private readonly IMapper mapper;
        public QuestionController(IQuestionService questionService, IMapper mapper)
        {
            this.questionService = questionService;
            this.mapper = mapper;
        }
        // GET: api/<QuestionController>
        [HttpGet]
        public async Task<ActionResult<QuestionDTO>> Get()
        {
            var list = await questionService.GetAll();
            var listQuestion = list.Select(x => mapper.Map<QuestionDTO>(x)).ToList();
            return Ok(new { listQuestion });
        }

        // GET api/<QuestionController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<QuestionDTO>> Get(int id)
        {
            var question = await questionService.GetById(id);
            if (question == null)
            {
                return NotFound();
            }
            var questionDTO = mapper.Map<QuestionDTO>(question);
            return Ok(new { questionDTO });
        }

        // POST api/<QuestionController>
        [HttpPost]
        public async Task<ActionResult<QuestionDTO>> Post([FromBody] QuestionDTO questionDTO)
        {
            var question = mapper.Map<Question>(questionDTO);
            question = await questionService.Insert(question);
            questionDTO = mapper.Map<QuestionDTO>(question);
            return Ok(new { question });
        }

        // PUT api/<QuestionController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult<QuestionDTO>> Put(int id, [FromBody] QuestionDTO questionDTO)
        {
            var question = await questionService.GetById(id);
            if (question == null)
            {
                return NotFound();
            }
            question.CategoryId = questionDTO.CategoryId;
            question.UserId = questionDTO.UserId;
            question.Title = questionDTO.Title;
            question.Content = questionDTO.Content;
            question.CreateAt = questionDTO.CreateAt;
            question.IsDeleted = questionDTO.IsDeleted;
            question = await questionService.Update(question);

            questionDTO = mapper.Map<QuestionDTO>(question);

            return Ok(new { questionDTO });
        }

        // DELETE api/<QuestionController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await questionService.Delete(id);
            return Ok();
        }
    }
}
