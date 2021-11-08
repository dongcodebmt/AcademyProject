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
    public class ExamController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IGenericService<ExamQuestion> examQuestionService;
        private readonly IGenericService<ExamOption> examOptionService;
        private readonly IGenericService<ExamRightOption> EROService;

        public ExamController(IMapper mapper, IGenericService<ExamQuestion> examQuestionService, IGenericService<ExamOption> examOptionService,
            IGenericService<ExamRightOption> EROService)
        {
            this.mapper = mapper;
            this.examQuestionService = examQuestionService;
            this.examOptionService = examOptionService;
            this.EROService = EROService;
        }

        [HttpGet]
        public async Task<ActionResult<List<ExamQuestionDTO>>> Get()
        {
            var list = await examQuestionService.GetList(x => x.IsDeleted == false);
            var EQs = list.Select(x => mapper.Map<ExamQuestionDTO>(x)).ToList();
            return Ok(EQs);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ExamDTO>> Get(int id)
        {
            var examQuestion = await examQuestionService.GetById(id);
            if (examQuestion == null)
            {
                return NotFound();
            }
            var examOptions = await examOptionService.GetList(x => x.QuestionId == id);
            var ERO = await EROService.GetById(id);
            ExamDTO examDTO = new ExamDTO();
            examDTO.Question = mapper.Map<ExamQuestionDTO>(examQuestion);
            examDTO.Options = examOptions.Select(x => mapper.Map<ExamOptionDTO>(x)).ToList();
            examDTO.RightOption = (ERO != null) ? mapper.Map<ExamRightOptionDTO>(ERO) : new ExamRightOptionDTO();
            return Ok(examDTO);
        }

        [HttpPost]
        public async Task<ActionResult<ExamDTO>> Post([FromBody] ExamDTO examDTO)
        {
            examDTO = await InsertExam(examDTO);
            return Ok(examDTO);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ExamDTO>> Put(int id, [FromBody] ExamDTO examDTO)
        {
            var examQuestion = await examQuestionService.GetById(id);
            if (examQuestion == null)
            {
                return NotFound();
            }
            examQuestion.IsDeleted = true;
            await examQuestionService.Update(examQuestion);
            examDTO = await InsertExam(examDTO);
            return Ok(examDTO);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var examQuestion = await examQuestionService.GetById(id);
            if (examQuestion == null)
            {
                return NotFound();
            }
            examQuestion.IsDeleted = true;
            await examQuestionService.Update(examQuestion);
            return Ok();
        }

        private async Task<ExamDTO> InsertExam(ExamDTO examDTO)
        {
            //Insert exam question
            ExamQuestion examQuestion = new ExamQuestion();
            examQuestion.CourseId = examDTO.Question.CourseId;
            examQuestion.Content = examDTO.Question.Content;
            examQuestion = await examQuestionService.Insert(examQuestion);
            //Insert exam options and right option
            ExamRightOption examRightOption = new ExamRightOption();
            for (int i = 0; i < examDTO.Options.Count; i++)
            {
                ExamOption examOption = new ExamOption();
                examOption.QuestionId = examQuestion.Id;
                examOption.Content = examDTO.Options[i].Content;
                examOption = await examOptionService.Insert(examOption);
                if (i == examDTO.RightOption.Index)
                {
                    examRightOption.QuestionId = examQuestion.Id;
                    examRightOption.OptionId = examOption.Id;
                    await EROService.Insert(examRightOption);
                }
            }

            //Return new data
            examDTO.Question = mapper.Map<ExamQuestionDTO>(examQuestion);
            var examOptions = await examOptionService.GetList(x => x.QuestionId == examQuestion.Id);
            examDTO.Options = examOptions.Select(x => mapper.Map<ExamOptionDTO>(x)).ToList();
            examDTO.RightOption = mapper.Map<ExamRightOptionDTO>(examRightOption);
            return examDTO;
        }
    }
}
