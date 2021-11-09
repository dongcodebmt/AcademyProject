using AcademyProject.DTOs;
using AcademyProject.Models;
using AcademyProject.Services;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
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
        private readonly IGenericService<ExamQuestion> examQuestionService;
        public CourseController(IMapper mapper, IConfiguration configuration, IGenericService<Course> courseService, IGenericService<Picture> pictureService,
            IGenericService<WillLearn> willLearnService, IGenericService<Requirement> requirementService, IGenericService<Track> trackService,
            IGenericService<ExamQuestion> examQuestionService, IGenericService<Step> stepService)
        {
            this.configuration = configuration;
            this.mapper = mapper;
            this.courseService = courseService;
            this.pictureService = pictureService;
            this.willLearnService = willLearnService;
            this.requirementService = requirementService;
            this.trackService = trackService;
            this.examQuestionService = examQuestionService;
            this.stepService = stepService;
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

        [HttpGet("{id}/WillLearns")]
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

        [HttpGet("{id}/Requirements")]
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

        [HttpGet("{id}/Tracks")]
        public async Task<ActionResult<List<TrackDTO>>> Tracks(int id)
        {
            var list = await trackService.GetList(x => x.CourseId == id);
            if (list == null)
            {
                return NotFound();
            }
            var tracks = list.Select(x => mapper.Map<TrackDTO>(x)).ToList();
            return Ok(tracks);
        }

        [HttpGet("{id}/ExamQuestions")]
        public async Task<ActionResult<List<ExamQuestionDTO>>> ExamQuestions(int id)
        {
            var list = await examQuestionService.GetList(x => x.CourseId == id && x.IsDeleted == false); 
            if (list == null)
            {
                return NotFound();
            }
            var examQuestions = list.Select(x => mapper.Map<ExamQuestionDTO>(x)).ToList();
            return Ok(examQuestions);
        }

        [HttpGet("{id}/TrackSteps")]
        public async Task<ActionResult<List<TrackStepDTO>>> TrackSteps(int id)
        {
            var list = await trackService.GetList(x => x.CourseId == id);
            if (list == null)
            {
                return NotFound();
            }
            var trackSteps = list.Select(x => mapper.Map<TrackStepDTO>(x)).ToList();
            foreach(var item in trackSteps)
            {
                var steps = await stepService.GetList(x => x.TrackId == item.Id);
                item.Steps = steps.Select(x => mapper.Map<StepWithoutContentDTO>(x)).ToList();
            }
            return Ok(trackSteps);
        }


        [HttpPost]
        public async Task<ActionResult<CourseDTO>> Post([FromBody] CourseDTO courseDTO)
        {
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
