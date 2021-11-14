using AcademyProject.DTOs;
using AcademyProject.Entities;
using AcademyProject.Models;
using AcademyProject.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
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
        private readonly IGenericService<Exam> examService;
        private readonly IGenericService<ExamQuestion> examQuestionService;
        private readonly IGenericService<ExamOption> examOptionService;
        private readonly IGenericService<ExamRightOption> EROService;
        private readonly IGenericService<ExamUser> examUserService;
        private readonly IGenericService<ExamDetail> examDetailService;

        public ExamController(IMapper mapper, IGenericService<Exam> examService, IGenericService<ExamQuestion> examQuestionService, IGenericService<ExamOption> examOptionService,
            IGenericService<ExamRightOption> EROService, IGenericService<ExamUser> examUserService, IGenericService<ExamDetail> examDetailService)
        {
            this.mapper = mapper;
            this.examService = examService;
            this.examQuestionService = examQuestionService;
            this.examOptionService = examOptionService;
            this.EROService = EROService;
            this.examUserService = examUserService;
            this.examDetailService = examDetailService;
        }
        protected int GetCurrentUserId()
        {
            string userId = User.Claims.Where(x => x.Type == "Id").FirstOrDefault()?.Value;
            int id = Convert.ToInt32(userId);
            return id;
        }

        [HttpGet]
        public async Task<ActionResult<List<ExamDTO>>> Get()
        {
            var list = await examService.GetList(x => x.IsDeleted == false);
            var exams = list.Select(x => mapper.Map<ExamDTO>(x)).ToList();
            return Ok(exams);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ExamDTO>> Get(int id)
        {
            var exam = await examService.GetById(id);
            if (exam == null)
            {
                return NotFound();
            }
            ExamDTO examDTO = mapper.Map<ExamDTO>(exam);
            return Ok(examDTO);
        }


        [HttpGet("{id}/[action]")]
        public async Task<ActionResult<List<ExamQuestionDTO>>> ExamQuestions(int id)
        {
            var list = await examQuestionService.GetList(x => x.ExamId == id && x.IsDeleted == false);
            if (list == null)
            {
                return NotFound();
            }
            var examQuestions = list.Select(x => mapper.Map<ExamQuestionDTO>(x)).ToList();
            return Ok(examQuestions);
        }

        [HttpGet("{id}/[action]")]
        public async Task<ActionResult<List<QuestionFullDTO>>> Questions(int id)
        {
            var examQuestions = await examQuestionService.GetList(x => x.ExamId == id && x.IsDeleted == false);
            if (examQuestions == null)
            {
                return NotFound();
            }
            List<QuestionFullDTO> list = new List<QuestionFullDTO>();
            foreach(var item in examQuestions)
            {
                QuestionFullDTO q = new QuestionFullDTO();
                q.Question = mapper.Map<ExamQuestionDTO>(item);
                var options = await examOptionService.GetList(x => x.QuestionId == item.Id);
                q.Options = options.Select(x => mapper.Map<ExamOptionDTO>(x)).ToList();
                list.Add(q);
            }
            return Ok(list);
        }

        [HttpGet("{questionId}/[action]")]
        public async Task<ActionResult<QuestionFullDTO>> QuestionFull(int questionId)
        {
            var examQuestion = await examQuestionService.GetById(questionId);
            if (examQuestion == null)
            {
                return NotFound();
            }
            QuestionFullDTO questionFullDTO = new QuestionFullDTO();
            questionFullDTO.Question = mapper.Map<ExamQuestionDTO>(examQuestion);
            var examOptions = await examOptionService.GetList(x => x.QuestionId == examQuestion.Id);
            questionFullDTO.Options = examOptions.Select(x => mapper.Map<ExamOptionDTO>(x)).ToList();
            var examRightOption = await EROService.Get(x => x.QuestionId == questionId);
            questionFullDTO.RightOption = examRightOption != null ? mapper.Map<ExamRightOptionDTO>(examRightOption) : null;
            return Ok(questionFullDTO);
        }

        [HttpPost]
        public async Task<ActionResult<ExamDTO>> Post([FromBody] ExamDTO examDTO)
        {
            var exam = mapper.Map<Exam>(examDTO);
            exam = await examService.Insert(exam);
            examDTO = mapper.Map<ExamDTO>(exam);
            return Ok(examDTO);
        }

        [HttpPost("{id}/[action]")]
        public async Task<ActionResult<QuestionFullDTO>> Question([FromBody] QuestionFullDTO questionFullDTO)
        {
            questionFullDTO = await InsertQuestionAsync(questionFullDTO);
            return Ok(questionFullDTO);
        }

        [Authorize]
        [HttpGet("{id}/[action]")]
        public async Task<ActionResult<ExamUserDTO>> Test(int id)
        {
            var exam = await examService.Get(x => x.Id == id && x.IsDeleted == false);
            if (exam == null)
            {
                return NotFound();
            }
            int userId = GetCurrentUserId();
            ExamUser e = new ExamUser();
            e.UserId = userId;
            e.ExamId = exam.Id;
            e.NoOfQuestions = await examQuestionService.Count(x => x.ExamId == exam.Id && x.IsDeleted == false);
            e = await examUserService.Insert(e);

            //var questions = await examQuestionService.GetList(x => x.ExamId == exam.Id && x.IsDeleted == false);
            //foreach(var item in questions)
            //{
            //    ExamDetail examDetail = new ExamDetail();
            //    examDetail.ExamUserId = e.Id;
            //    examDetail.QuestionId = item.Id;
            //    await examDetailService.Insert(examDetail);
            //}

            ExamUserDTO examUserDTO = mapper.Map<ExamUserDTO>(e);
            return Ok(examUserDTO);
        }

        [Authorize]
        [HttpPost("{examUserId}/[action]")]
        public async Task<ActionResult<ExamResult>> Answers(int examUserId, [FromBody] List<QuestionOptions> questionOptions)
        {
            var examUser = await examUserService.GetById(examUserId);
            examUser.CompletedAt = DateTime.Now;
            await examUserService.Update(examUser);
            foreach (var item in questionOptions)
            {
                ExamDetail e = new ExamDetail();
                e.ExamUserId = examUserId;
                e.QuestionId = item.questionId;
                e.OptionId = item.optionId;
                await examDetailService.Insert(e);
            }

            ExamResult examResult = new ExamResult();
            examResult.Id = examUser.Id;
            examResult.NoOfQuestion = examUser.NoOfQuestions;
            examResult.NoOfRightOption = 0;
            foreach (var item in questionOptions)
            {
                var ERO = await EROService.Get(x => x.QuestionId == item.questionId && x.OptionId == item.optionId);
                if (ERO != null)
                {
                    examResult.NoOfRightOption += 1;
                }
            }
            return Ok(examResult);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ExamDTO>> Put(int id, [FromBody] ExamDTO examDTO)
        {
            var exam = await examService.GetById(id);
            if (exam == null)
            {
                return NotFound();
            }
            exam.Title = examDTO.Title;
            exam.ExamDuration = examDTO.ExamDuration;
            await examService.Update(exam);
            return Ok(examDTO);
        }

        [HttpPut("{id}/[action]")]
        public async Task<ActionResult<QuestionFullDTO>> Question(int id, [FromBody] QuestionFullDTO questionFullDTO)
        {
            var examQuestion = await examQuestionService.GetById(id);
            if (examQuestion == null)
            {
                return NotFound();
            }
            examQuestion.IsDeleted = true;
            await examQuestionService.Update(examQuestion);
            questionFullDTO = await InsertQuestionAsync(questionFullDTO);
            return Ok(questionFullDTO);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var exam = await examService.GetById(id);
            if (exam == null)
            {
                return NotFound();
            }
            exam.IsDeleted = true;
            await examService.Update(exam);
            return Ok();
        }

        [HttpDelete("{questionId}/[action]")]
        public async Task<IActionResult> Question(int questionId)
        {
            var examQuestion = await examQuestionService.GetById(questionId);
            if (examQuestion == null)
            {
                return NotFound();
            }
            examQuestion.IsDeleted = true;
            await examQuestionService.Update(examQuestion);
            return Ok();
        }

        private async Task<QuestionFullDTO> InsertQuestionAsync(QuestionFullDTO questionFullDTO)
        {
            //Insert exam question
            ExamQuestion examQuestion = new ExamQuestion();
            examQuestion.ExamId = questionFullDTO.Question.ExamId;
            examQuestion.Content = questionFullDTO.Question.Content;
            examQuestion = await examQuestionService.Insert(examQuestion);
            //Insert exam options and right option
            ExamRightOption examRightOption = new ExamRightOption();
            for (int i = 0; i < questionFullDTO.Options.Count; i++)
            {
                ExamOption examOption = new ExamOption();
                examOption.QuestionId = examQuestion.Id;
                examOption.Content = questionFullDTO.Options[i].Content;
                examOption = await examOptionService.Insert(examOption);
                if (i == questionFullDTO.RightOption.Index)
                {
                    examRightOption.QuestionId = examQuestion.Id;
                    examRightOption.OptionId = examOption.Id;
                    await EROService.Insert(examRightOption);
                }
            }

            //Return new data
            questionFullDTO.Question = mapper.Map<ExamQuestionDTO>(examQuestion);
            var examOptions = await examOptionService.GetList(x => x.QuestionId == examQuestion.Id);
            questionFullDTO.Options = examOptions.Select(x => mapper.Map<ExamOptionDTO>(x)).ToList();
            questionFullDTO.RightOption = mapper.Map<ExamRightOptionDTO>(examRightOption);
            return questionFullDTO;
        }
    }
}
