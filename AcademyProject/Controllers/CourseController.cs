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
using System.Threading.Tasks;

namespace AcademyProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        public IConfiguration configuration;
        private readonly IMapper mapper;
        private readonly IGenericService<Course> courseService;
        private readonly IGenericService<Picture> pictureService;
        private readonly IGenericService<WillLearn> willLearnService;
        private readonly IGenericService<Requirement> requirementService;
        private readonly IGenericService<Track> trackService;
        private readonly IGenericService<Step> stepService;
        private readonly IGenericService<Attendance> attendanceService;
        private readonly IGenericService<Progress> progressService;
        private readonly IGenericService<Exam> examService;
        public CourseController(IMapper mapper, IConfiguration configuration, IGenericService<Course> courseService, IGenericService<Picture> pictureService,
            IGenericService<WillLearn> willLearnService, IGenericService<Requirement> requirementService, IGenericService<Track> trackService,
            IGenericService<Step> stepService, IGenericService<Attendance> attendanceService, IGenericService<Exam> examService,
            IGenericService<Progress> progressService)
        {
            this.configuration = configuration;
            this.mapper = mapper;
            this.courseService = courseService;
            this.pictureService = pictureService;
            this.willLearnService = willLearnService;
            this.requirementService = requirementService;
            this.trackService = trackService;
            this.stepService = stepService;
            this.attendanceService = attendanceService;
            this.examService = examService;
            this.progressService = progressService;
        }

        protected int GetCurrentUserId()
        {
            string userId = User.Claims.Where(x => x.Type == "Id").FirstOrDefault()?.Value;
            int id = Convert.ToInt32(userId);
            return id;
        }

        protected async Task<bool> IsCourseRegistedAsync(int userId, int courseId)
        {
            var attendance = await attendanceService.Get(x => x.UserId == userId && x.CourseId == courseId);
            if (attendance != null)
            {
                return true;
            }
            return false;
        }

        [HttpGet]
        public async Task<ActionResult<List<CourseDTO>>> Get()
        {
            var list = await courseService.GetAll();
            var courses = list.Select(x => mapper.Map<CourseDTO>(x)).ToList();
            foreach (var item in courses)
            {
                var picture = await pictureService.GetById((int)item.PictureId);
                if (picture.PicturePath != "/" && picture.PicturePath.Substring(0, 1) == "/")
                {
                    item.PicturePath = configuration["ServerHostName"] + picture.PicturePath;
                } else
                {
                    item.PicturePath = picture.PicturePath;
                }
            }
            return Ok(courses);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CourseDTO>> Get(int id)
        {
            var course = await courseService.GetById(id);
            if (course == null)
            {
                return NotFound();
            }
            var courseDTO = mapper.Map<CourseDTO>(course);
            var picture = await pictureService.GetById((int)courseDTO.PictureId);
            if (picture.PicturePath != "/" && picture.PicturePath.Substring(0, 1) == "/")
            {
                courseDTO.PicturePath = configuration["ServerHostName"] + picture.PicturePath;
            }
            else
            {
                courseDTO.PicturePath = picture.PicturePath;
            }
            return Ok(courseDTO);
        }

        [HttpGet("{id}/[action]")]
        public async Task<ActionResult<List<WillLearnDTO>>> WillLearns(int id)
        {
            var list = await willLearnService.GetList(x => x.CourseId == id);
            if (list == null)
            {
                return NotFound();
            }
            var willLearns = list.Select(x => mapper.Map<WillLearnDTO>(x)).ToList();
            return Ok(willLearns);
        }

        [HttpGet("{id}/[action]")]
        public async Task<ActionResult<List<RequirementDTO>>> Requirements(int id)
        {
            var list = await requirementService.GetList(x => x.CourseId == id);
            if (list == null)
            {
                return NotFound();
            }
            var requirements = list.Select(x => mapper.Map<RequirementDTO>(x)).ToList();
            return Ok(requirements);
        }

        [HttpGet("{id}/[action]")]
        public async Task<ActionResult<List<ExamDTO>>> Exams(int id)
        {
            var list = await examService.GetList(x => x.CourseId == id && x.IsDeleted == false);
            if (list == null)
            {
                return NotFound();
            }
            var exams = list.Select(x => mapper.Map<ExamDTO>(x)).ToList();
            return Ok(exams);
        }


        [HttpGet("{id}/[action]")]
        public async Task<ActionResult<List<TrackDTO>>> Tracks(int id)
        {
            var list = await trackService.GetList(x => x.CourseId == id && x.IsDeleted == false);
            if (list == null)
            {
                return NotFound();
            }
            var tracks = list.Select(x => mapper.Map<TrackDTO>(x)).ToList();
            return Ok(tracks);
        }

        //[HttpGet("{id}/[action]")]
        //public async Task<ActionResult<List<ExamDTO>>> Exam(int id)
        //{
        //    var list = await examQuestionService.GetList(x => x.ExamId == id && x.IsDeleted == false);
        //    if (list == null)
        //    {
        //        return NotFound();
        //    }
        //    List<ExamDTO> exams = new List<ExamDTO>();
        //    foreach (var item in list)
        //    {
        //        ExamDTO exam = new ExamDTO();
        //        exam.Question = mapper.Map<ExamQuestionDTO>(item);
        //        var options = await examOptionService.GetList(x => x.QuestionId == item.Id);
        //        exam.Options = options.Select(x => mapper.Map<ExamOptionDTO>(x)).ToList();
        //        exams.Add(exam);
        //    }
        //    return Ok(exams);
        //}

        [HttpGet("{id}/[action]")]
        public async Task<ActionResult<List<TrackStepDTO>>> TrackSteps(int id)
        {
            var list = await trackService.GetList(x => x.CourseId == id && x.IsDeleted == false);
            if (list == null)
            {
                return NotFound();
            }
            var trackSteps = list.Select(x => mapper.Map<TrackStepDTO>(x)).ToList();
            foreach(var item in trackSteps)
            {
                var steps = await stepService.GetList(x => x.TrackId == item.Id && x.IsDeleted == false);
                item.Steps = steps.Select(x => mapper.Map<StepWithoutContentDTO>(x)).ToList();
                int userId = GetCurrentUserId();
                if (userId != 0)
                {
                    foreach (var i in item.Steps)
                    {
                        var progress = await progressService.Get(x => x.UserId == userId && x.StepId == i.Id);
                        i.Completed = progress == null ? false : true;
                    }
                }
            }
            return Ok(trackSteps);
        }

        [Authorize]
        [HttpPost("{id}/[action]")]
        public async Task<IActionResult> Register(int id)
        {
            int userId = GetCurrentUserId();
            var course = await courseService.GetById(id);
            if (course == null)
            {
                return NotFound();
            }
            Attendance attendance = new Attendance();
            attendance.UserId = Convert.ToInt32(userId);
            attendance.CourseId = course.Id;
            await attendanceService.Insert(attendance);

            return Ok();
        }

        [Authorize]
        [HttpGet("{id}/[action]")]
        public async Task<ActionResult<bool>> IsRegisted(int id)
        {
            int userId = GetCurrentUserId();
            bool isValid = await IsCourseRegistedAsync(userId, id);
            if (!isValid)
            {
                return Ok(false);
            }

            return Ok(true);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<CourseDTO>> Post([FromBody] CourseDTO courseDTO)
        {
            int userId = GetCurrentUserId();
            courseDTO.LecturerId = Convert.ToInt32(userId);
            var course = mapper.Map<Course>(courseDTO);
            course = await courseService.Insert(course);
            courseDTO = mapper.Map<CourseDTO>(course);
            return Ok(courseDTO);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<CourseDTO>> Put(int id, [FromBody] CourseDTO courseDTO)
        {
            var course = await courseService.GetById(id);
            if (course == null)
            {
                return NotFound();
            }
            course.LecturerId = courseDTO.LecturerId;
            course.CategoryId = courseDTO.CategoryId;
            course.PictureId = (courseDTO.PictureId != null) ? courseDTO.PictureId : course.PictureId;
            course.Title = courseDTO.Title;
            course.Description = courseDTO.Description;
            course = await courseService.Update(course);

            courseDTO = mapper.Map<CourseDTO>(course);

            return Ok(courseDTO);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var course = await courseService.GetById(id);
            if (course == null)
            {
                return NotFound();
            }
            course.IsDeleted = true;
            await courseService.Update(course);
            return Ok();
        }
    }
}
