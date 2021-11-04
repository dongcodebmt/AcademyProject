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
    public class AnswerController : ControllerBase
    {
        private readonly IGenericService<Answer> answerService;
        private readonly IMapper mapper;
        public AnswerController(IGenericService<Answer> answerService, IMapper mapper)
        {
            this.answerService = answerService;
            this.mapper = mapper;
        }
        // GET: api/<AnswerController>
        [HttpGet]
        public async Task<ActionResult<AnswerDTO>> Get()
        {
            var list = await answerService.GetAll();
            var listAnswer = list.Select(x => mapper.Map<AnswerDTO>(x)).ToList();
            return Ok(new { listAnswer });
        }

        // GET api/<AnswerController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AnswerDTO>> Get(int id)
        {
            var answer = await answerService.GetById(id);
            if (answer == null)
            {
                return NotFound();
            }
            var answerDTO = mapper.Map<AnswerDTO>(answer);
            return Ok(new { answerDTO });
        }

        // POST api/<AnswerController>
        [HttpPost]
        public async Task<ActionResult<AnswerDTO>> Post([FromBody] AnswerDTO answerDTO)
        {
            var answer = mapper.Map<Answer>(answerDTO);
            answer = await answerService.Insert(answer);
            answerDTO = mapper.Map<AnswerDTO>(answer);
            return Ok(new { answer });
        }

        // PUT api/<AnswerController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult<AnswerDTO>> Put(int id, [FromBody] AnswerDTO answerDTO)
        {
            var answer = await answerService.GetById(id);
            if (answer == null)
            {
                return NotFound();
            }
            answer.QuestionId = answerDTO.QuestionId;
            answer.UserId = answerDTO.UserId;
            answer.Content = answerDTO.Content;
            answer.CreateAt = answerDTO.CreateAt;
            answer = await answerService.Update(answer);

            answerDTO = mapper.Map<AnswerDTO>(answer);

            return Ok(new { answerDTO });
        }

        // DELETE api/<AnswerController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await answerService.Delete(id);
            return Ok();
        }
    }
}
