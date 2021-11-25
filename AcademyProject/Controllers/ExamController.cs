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
        private readonly SharedComponent component;
        private readonly IGenericService<Exam> examService;
        private readonly IGenericService<ExamQuestion> examQuestionService;
        private readonly IGenericService<ExamOption> examOptionService;
        private readonly IGenericService<ExamRightOption> EROService;
        private readonly IGenericService<ExamUser> examUserService;
        private readonly IGenericService<ExamDetail> examDetailService;
        private readonly IGenericService<Certification> certificationService;

        public ExamController(
            IMapper mapper,
            SharedComponent component,
            IGenericService<Exam> examService, 
            IGenericService<ExamQuestion> examQuestionService, 
            IGenericService<ExamOption> examOptionService,
            IGenericService<ExamRightOption> EROService, 
            IGenericService<ExamUser> examUserService, 
            IGenericService<ExamDetail> examDetailService,
            IGenericService<Certification> certificationService
        )
        {
            this.mapper = mapper;
            this.component = component;
            this.examService = examService;
            this.examQuestionService = examQuestionService;
            this.examOptionService = examOptionService;
            this.EROService = EROService;
            this.examUserService = examUserService;
            this.examDetailService = examDetailService;
            this.certificationService = certificationService;
        }
        protected int GetCurrentUserId()
        {
            string userId = User.Claims.Where(x => x.Type == "Id").FirstOrDefault()?.Value;
            int id = Convert.ToInt32(userId);
            return id;
        }
        
        // GET: api/<ExamController>
        //[HttpGet]
        //public async Task<ActionResult<List<ExamDTO>>> Get()
        //{
        //    var list = await examService.GetList(x => x.IsDeleted == false);
        //    var exams = list.Select(x => mapper.Map<ExamDTO>(x)).ToList();
        //    return Ok(exams);
        //}

        // GET: api/<ExamController>/{id}
        [HttpGet("{id}")]
        [Authorize]
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

        // GET: api/<ExamController>/{id}/[Action]
        // Get only list questions
        [HttpGet("{id}/[action]")]
        [Authorize(Roles = "Administrators, Lecturers")]
        public async Task<ActionResult<List<ExamQuestionDTO>>> Questions(int id)
        {
            var list = await examQuestionService.GetList(x => x.ExamId == id && x.IsDeleted == false);
            if (list == null)
            {
                return NotFound();
            }
            var examQuestions = list.Select(x => mapper.Map<ExamQuestionDTO>(x)).ToList();
            foreach (var item in examQuestions)
            {
                item.Content = component.Truncate(component.StripHTML(item.Content), 70);
            }
            return Ok(examQuestions);
        }

        // GET: api/<ExamController>/{id}/[Action]
        // Get question including question, options, right option
        [HttpGet("{questionId}/[action]")]
        [Authorize(Roles = "Administrators, Lecturers")]
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

        // GET: api/<ExamController>/{id}/[Action]
        // Check if the course has been completed or not
        // The course will be completed once the user is certified
        [Authorize]
        [HttpGet("{id}/[action]")]
        public async Task<ActionResult<bool>> IsFinished(int id)
        {
            int userId = GetCurrentUserId();
            var exam = await examService.GetById(id);
            if (exam == null)
            {
                return NotFound();
            }
            var cert = await certificationService.Get(x => x.UserId == userId && x.CourseId == exam.CourseId);
            if (cert != null)
            {
                return Ok(true);
            }
            var examUsers = await examUserService.GetList(x => x.ExamId == exam.Id && x.UserId == userId, o => o.OrderByDescending(x => x.Mark), "", 0, 1);
            var examUser = examUsers.FirstOrDefault();
            if (examUser != null && examUser.Mark >= 4)
            {
                return Ok(true);
            }
            return Ok(false);
        }

        // GET: api/<ExamController>/{id}/[Action]
        // Request to take the test and start test
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
            e.NoOfQuestion = await examQuestionService.Count(x => x.ExamId == exam.Id && x.IsDeleted == false);
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

        // GET: api/<ExamController>/{id}/[Action]
        // Get list questions including question, options
        [Authorize]
        [HttpGet("{id}/[action]")]
        public async Task<ActionResult<List<QuestionFullDTO>>> ExamQuestions(int id)
        {
            var examQuestions = await examQuestionService.GetList(x => x.ExamId == id && x.IsDeleted == false);
            if (examQuestions == null)
            {
                return NotFound();
            }
            List<QuestionFullDTO> list = new List<QuestionFullDTO>();
            foreach (var item in examQuestions)
            {
                QuestionFullDTO q = new QuestionFullDTO();
                q.Question = mapper.Map<ExamQuestionDTO>(item);
                var options = await examOptionService.GetList(x => x.QuestionId == item.Id);
                q.Options = options.Select(x => mapper.Map<ExamOptionDTO>(x)).ToList();
                list.Add(q);
            }
            return Ok(list);
        }

        // GET: api/<ExamController>/{examUserId}/[Action]
        [Authorize]
        [HttpGet("{examUserId}/[action]")]
        public async Task<ActionResult<ExamUserDTO>> Result(int examUserId)
        {
            var examUser = await examUserService.GetById(examUserId);
            ExamUserDTO examUserDTO = mapper.Map<ExamUserDTO>(examUser);
            examUserDTO.Details = new List<ExamDetailWithContentDTO>();
            var exam = await examService.GetById(examUser.ExamId);
            examUserDTO.Title = exam.Title;
            // Get exams details
            // Get list questions of exam
            var examQuestions = await examQuestionService.GetList(x => x.ExamId == examUser.ExamId);
            // Get user answers
            var userOptions = await examDetailService.GetList(x => x.ExamUserId == examUserId);
            foreach (var item in examQuestions)
            {
                ExamDetailWithContentDTO detail = new ExamDetailWithContentDTO();
                detail.QuestionId = item.Id;
                detail.QuestionContent = item.Content;

                // Check option is right
                var userOption = userOptions.Where(x => x.QuestionId == item.Id).FirstOrDefault();
                if (userOption != null)
                {
                    var ERO = await EROService.Get(x => x.QuestionId == item.Id && x.OptionId == userOption.OptionId);
                    if (ERO != null)
                    {
                        detail.IsRight = true;
                    } 
                    else
                    {
                        detail.IsRight = false;
                    }

                    // Get option of user answer
                    detail.OptionId = userOption.OptionId;
                    var option = await examOptionService.Get(x => x.Id == detail.OptionId);
                    detail.OptionContent = option.Content;

                    examUserDTO.Details.Add(detail);
                }
            }

            return Ok(examUserDTO);
        }

        // POST: api/<ExamController>
        [HttpPost]
        [Authorize(Roles = "Administrators, Lecturers")]
        public async Task<ActionResult<ExamDTO>> Post([FromBody] ExamDTO examDTO)
        {
            var exam = mapper.Map<Exam>(examDTO);
            exam = await examService.Insert(exam);
            examDTO = mapper.Map<ExamDTO>(exam);
            return Ok(examDTO);
        }

        // POST: api/<ExamController>/{id}/[Action]
        [HttpPost("{id}/[action]")]
        [Authorize(Roles = "Administrators, Lecturers")]
        public async Task<ActionResult<QuestionFullDTO>> Question([FromBody] QuestionFullDTO questionFullDTO)
        {
            if (questionFullDTO.Options.Count < 2)
            {
                return BadRequest("Vui lòng nhập ít nhất 2 câu hỏi!");
            }
            questionFullDTO = await InsertQuestionAsync(questionFullDTO);
            return Ok(questionFullDTO);
        }

        // POST: api/<ExamController>/{examUserId}/[Action]
        // User submits the test including a list of question ids and option ids
        [Authorize]
        [HttpPost("{examUserId}/[action]")]
        public async Task<ActionResult<ExamUserDTO>> Answers(int examUserId, [FromBody] List<QuestionOptions> questionOptions)
        {
            var examUser = await examUserService.GetById(examUserId);
            foreach (var item in questionOptions)
            {
                ExamDetail e = new ExamDetail();
                e.ExamUserId = examUserId;
                e.QuestionId = item.QuestionId;
                e.OptionId = item.OptionId;
                await examDetailService.Insert(e);
            }

            int rightOption = 0;
            //Get exams details
            foreach (var item in questionOptions)
            {
                var ERO = await EROService.Get(x => x.QuestionId == item.QuestionId && x.OptionId == item.OptionId);
                if (ERO != null)
                {
                    rightOption++;
                }
            }

            examUser.NoOfRightOption = rightOption;
            examUser.Mark = (double)rightOption / (double)examUser.NoOfQuestion * 10.0;
            examUser.CompletedAt = DateTime.Now;
            examUser = await examUserService.Update(examUser);

            ExamUserDTO examUserDTO = mapper.Map<ExamUserDTO>(examUser);

            return Ok(examUserDTO);
        }

        // PUT: api/<ExamController>/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "Administrators, Lecturers")]
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

        // PUT: api/<ExamController>/{id}/[Action]
        [HttpPut("{id}/[action]")]
        [Authorize(Roles = "Administrators, Lecturers")]
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

        // DELETE: api/<ExamController>/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrators, Lecturers")]
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

        // DELETE: api/<ExamController>/{questionId}/[Action]
        [HttpDelete("{questionId}/[action]")]
        [Authorize(Roles = "Administrators, Lecturers")]
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

        //Insert full question including question, options, right option
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
