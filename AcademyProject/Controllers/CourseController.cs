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
    [ApiController]
    [Route("api/[controller]")]
    public class CourseController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly SharedComponent component;
        private readonly IGenericService<Course> courseService;
        private readonly IGenericService<WillLearn> willLearnService;
        private readonly IGenericService<Requirement> requirementService;
        private readonly IGenericService<Track> trackService;
        private readonly IGenericService<Step> stepService;
        private readonly IGenericService<Attendance> attendanceService;
        private readonly IGenericService<Progress> progressService;
        private readonly IGenericService<Exam> examService;
        private readonly IGenericService<ExamUser> examUserService;
        private readonly IGenericService<Certification> certificationService;
        public CourseController(
            IMapper mapper,
            SharedComponent component,
            IGenericService<Course> courseService,
            IGenericService<WillLearn> willLearnService,
            IGenericService<Requirement> requirementService,
            IGenericService<Track> trackService,
            IGenericService<Step> stepService,
            IGenericService<Attendance> attendanceService,
            IGenericService<Exam> examService,
            IGenericService<Progress> progressService,
            IGenericService<ExamUser> examUserService,
            IGenericService<Certification> certificationService
        )
        {
            this.mapper = mapper;
            this.component = component;
            this.courseService = courseService;
            this.willLearnService = willLearnService;
            this.requirementService = requirementService;
            this.trackService = trackService;
            this.stepService = stepService;
            this.attendanceService = attendanceService;
            this.examService = examService;
            this.progressService = progressService;
            this.examUserService = examUserService;
            this.certificationService = certificationService;
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

        // GET: api/<CourseController>
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<List<CourseDTO>>> Get(int skip = 0, int take = 10)
        {
            var list = await courseService.GetList(x => x.IsDeleted == false, o => o.OrderByDescending(x => x.UpdatedAt), "", skip, take);
            var courses = list.Select(x => mapper.Map<CourseDTO>(x)).ToList();
            foreach (var item in courses)
            {
                item.Description = null;
                item.PicturePath = await component.GetImageAsync(item.PictureId);
            }
            return Ok(courses);
        }

        // GET: api/<CourseController>/{id}
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<CourseDTO>> Get(int id)
        {
            var course = await courseService.GetById(id);
            if (course == null)
            {
                return NotFound();
            }
            var courseDTO = mapper.Map<CourseDTO>(course); 
            courseDTO.PicturePath = await component.GetImageAsync(courseDTO.PictureId);
            return Ok(courseDTO);
        }

        // GET: api/<CourseController>/{id}/[Action]
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

        // GET: api/<CourseController>/[Action]/{userId}
        [AllowAnonymous]
        [HttpGet("[action]/{userId}")]
        public async Task<ActionResult<List<CertificationDTO>>> Certifications(int userId)
        {
            var list = await certificationService.GetList(x => x.UserId == userId);
            var certs = list.Select(x => mapper.Map<CertificationDTO>(x)).ToList();
            foreach (var item in certs)
            {
                var course = await courseService.GetById(item.CourseId);
                item.CourseName = course.Title;
            }
            return Ok(certs);
        }

        // GET: api/<CourseController>/[Action]/{userId}
        [AllowAnonymous]
        [HttpGet("[action]/{userId}")]
        public async Task<ActionResult<List<CourseDTO>>> RegistedCourses(int userId)
        {
            var list = await attendanceService.GetList(x => x.UserId == userId);
            List<CourseDTO> courseDTOs = new List<CourseDTO>();
            foreach (var item in list)
            {
                var cert = await certificationService.Get(x => x.CourseId == item.CourseId && x.UserId == userId);
                if (cert == null)
                {
                    var course = await courseService.GetById(item.CourseId);
                    var courseDTO = mapper.Map<CourseDTO>(course);
                    courseDTO.Description = null;

                    //Calc progress
                    int totalStep = 0;
                    int completedStep = 0;
                    var tracks = await trackService.GetList(x => x.CourseId == courseDTO.Id && x.IsDeleted == false);
                    foreach (var track in tracks)
                    {
                        var steps = await stepService.GetList(x => x.TrackId == track.Id && x.IsDeleted == false);
                        foreach (var step in track.Steps)
                        {
                            totalStep++;
                            var progress = await progressService.Get(x => x.UserId == userId && x.StepId == step.Id);
                            if (progress != null)
                            {
                                completedStep++;
                            }
                        }
                    }
                    
                    if (completedStep > 0 && totalStep > 0)
                    {
                        courseDTO.Progress = (double)completedStep / (double)totalStep * 100.0;
                    } 
                    else
                    {
                        courseDTO.Progress = 0;
                    }
                    courseDTOs.Add(courseDTO);
                }
            }
            return Ok(courseDTOs);
        }

        // GET: api/<CourseController>/{id}/[Action]
        [AllowAnonymous]
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

        // GET: api/<CourseController>/{id}/[Action]
        [AllowAnonymous]
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

        // GET: api/<CourseController>/{id}/[Action]
        [AllowAnonymous]
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

        // GET: api/<CourseController>/{id}/[Action]
        [AllowAnonymous]
        [HttpGet("{id}/[action]")]
        public async Task<ActionResult<List<TrackStepDTO>>> TrackSteps(int id)
        {
            var list = await trackService.GetList(x => x.CourseId == id && x.IsDeleted == false);
            if (list == null)
            {
                return NotFound();
            }
            var trackSteps = list.Select(x => mapper.Map<TrackStepDTO>(x)).ToList();
            foreach (var item in trackSteps)
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

        // GET: api/<CourseController>/{id}/[Action]
        [Authorize]
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

        // GET: api/<CourseController>/{id}/[Action]
        [Authorize]
        [HttpGet("{id}/[action]")]
        public async Task<ActionResult<List<ExamUserDTO>>> ExamUsers(int id, int userId = 0)
        {
            if (userId == 0)
            {
                userId = GetCurrentUserId();
            }
            var exams = await examService.GetList(x => x.CourseId == id);
            if (exams == null)
            {
                return NotFound();
            }
            List<ExamUserDTO> examUserDTOs = new List<ExamUserDTO>();
            foreach (var item in exams)
            {
                var examUsers = await examUserService.GetList(x => x.ExamId == item.Id && x.UserId == userId);
                foreach (var i in examUsers)
                {
                    ExamUserDTO examUserDTO = mapper.Map<ExamUserDTO>(i);
                    examUserDTO.Title = item.Title;
                    examUserDTOs.Add(examUserDTO);
                }
            }
            return Ok(examUserDTOs);
        }

        // POST: api/<CourseController>
        [HttpPost]
        [Authorize(Roles = "Administrators, Lecturers")]
        public async Task<ActionResult<CourseDTO>> Post([FromBody] CourseDTO courseDTO)
        {
            int userId = GetCurrentUserId();
            courseDTO.LecturerId = Convert.ToInt32(userId);
            var course = mapper.Map<Course>(courseDTO);
            course = await courseService.Insert(course);
            courseDTO = mapper.Map<CourseDTO>(course);
            return Ok(courseDTO);
        }

        // POST: api/<CourseController>/{id}/[Action]
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

        // POST: api/<CourseController>/{id}/[Action]
        [Authorize]
        [HttpPost("{id}/[action]")]
        public async Task<ActionResult<bool>> Certify(int id)
        {
            var userId = GetCurrentUserId();
            var exams = await examService.GetList(x => x.CourseId == id && x.IsDeleted == false);
            if (exams == null)
            {
                return NotFound();
            }
            double total = 0;
            int i = 0;
            foreach (var item in exams)
            {
                var examUsers = await examUserService.GetList(x => x.ExamId == item.Id && x.UserId == userId, o => o.OrderByDescending(x => x.Mark), "", 0, 1);
                var examUser = examUsers.FirstOrDefault();
                if (examUser != null && examUser.Mark >= 4)
                {
                    total += (double)examUser.Mark;
                    i++;
                }
            }
            double mark = total / (double)i;
            if (i == exams.Count() && mark >= 4)
            {
                Certification cert = new Certification();
                cert.CourseId = id;
                cert.UserId = userId;
                cert.Mark = mark;
                await certificationService.Insert(cert);
                return Ok(true);
            }
            return Ok(false);
        }

        // PUT: api/<CourseController>/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "Administrators, Lecturers")]
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

        // DELETE: api/<CourseController>/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrators, Lecturers")]
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
