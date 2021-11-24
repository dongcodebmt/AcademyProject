using AcademyProject.DTOs;
using AcademyProject.Entities;
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
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize(Roles = "Administrators")]
    public class StatController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly SharedComponent component;
        private readonly IGenericService<User> userService;
        private readonly IGenericService<Course> courseService;
        private readonly IGenericService<Exam> examService;
        private readonly IGenericService<Blog> blogService;
        private readonly IGenericService<Question> questionService;
        private readonly IGenericService<Attendance> attendanceService;
        private readonly IGenericService<ExamUser> examUserService;
        private readonly IGenericService<Certification> certificationUserService;
        public StatController(
            IMapper mapper,
            SharedComponent component,
            IGenericService<User> userService,
            IGenericService<Course> courseService,
            IGenericService<Exam> examService,
            IGenericService<Blog> blogService,
            IGenericService<Question> questionService,
            IGenericService<Attendance> attendanceService,
            IGenericService<ExamUser> examUserService,
            IGenericService<Certification> certificationUserService
        )
        {
            this.mapper = mapper;
            this.component = component;
            this.userService = userService;
            this.courseService = courseService;
            this.examService = examService;
            this.blogService = blogService;
            this.questionService = questionService;
            this.attendanceService = attendanceService;
            this.examUserService = examUserService;
            this.certificationUserService = certificationUserService;
        }

        // GET: api/<StatController>/[Action]/{start:datetime}/{end:datetime}
        [HttpGet("{start:datetime}/{end:datetime}")]
        public async Task<ActionResult<List<Overview>>> Overview(DateTime start, DateTime end)
        {
            if (start > end)
            {
                return BadRequest("Invalid Date Range");
            }

            TimeSpan diff = end - start;

            List<Overview> overviews = new List<Overview>();
            Overview user = new Overview();
            user.Label = "Người dùng";
            user.Icon = "users";
            user.Total = await userService.Count();
            user.ThisTime = await userService.Count(x => x.CreatedAt >= start && x.CreatedAt <= end);
            user.LastTime = await userService.Count(x => x.CreatedAt >= start.AddDays(-diff.TotalDays * 2) && x.CreatedAt <= end.AddDays(-diff.TotalDays));
            overviews.Add(user);

            Overview course = new Overview();
            course.Label = "Khóa học";
            course.Icon = "book";
            course.Total = await courseService.Count(x => x.IsDeleted == false);
            course.ThisTime = await courseService.Count(x => x.CreatedAt >= start && x.CreatedAt <= end && x.IsDeleted == false);
            course.LastTime = await courseService.Count(x => x.CreatedAt >= start.AddDays(-diff.TotalDays * 2) && x.CreatedAt <= end.AddDays(-diff.TotalDays) && x.IsDeleted == false);
            overviews.Add(course);

            Overview exam = new Overview();
            exam.Label = "Bài kiểm tra";
            exam.Icon = "diagnoses";
            exam.Total = await examService.Count(x => x.IsDeleted == false);
            exam.ThisTime = await examService.Count(x => x.CreatedAt >= start && x.CreatedAt <= end && x.IsDeleted == false);
            exam.LastTime = await examService.Count(x => x.CreatedAt >= start.AddDays(-diff.TotalDays * 2) && x.CreatedAt <= end.AddDays(-diff.TotalDays) && x.IsDeleted == false);
            overviews.Add(exam);

            Overview blog = new Overview();
            blog.Label = "Blog";
            blog.Icon = "blog";
            blog.Total = await blogService.Count(x => x.IsDeleted == false);
            blog.ThisTime = await blogService.Count(x => x.CreatedAt >= start && x.CreatedAt <= end && x.IsDeleted == false);
            blog.LastTime = await blogService.Count(x => x.CreatedAt >= start.AddDays(-diff.TotalDays * 2) && x.CreatedAt <= end.AddDays(-diff.TotalDays) && x.IsDeleted == false);
            overviews.Add(blog);

            Overview question = new Overview();
            question.Label = "Hỏi đáp";
            question.Icon = "question";
            question.Total = await questionService.Count(x => x.IsDeleted == false);
            question.ThisTime = await questionService.Count(x => x.CreatedAt >= start && x.CreatedAt <= end && x.IsDeleted == false);
            question.LastTime = await questionService.Count(x => x.CreatedAt >= start.AddDays(-diff.TotalDays * 2) && x.CreatedAt <= end.AddDays(-diff.TotalDays) && x.IsDeleted == false);
            overviews.Add(question);


            Overview attendance = new Overview();
            attendance.Label = "Đăng ký khóa học";
            attendance.Icon = "registered";
            attendance.Total = await attendanceService.Count();
            attendance.ThisTime = await attendanceService.Count(x => x.CreatedAt >= start && x.CreatedAt <= end);
            attendance.LastTime = await attendanceService.Count(x => x.CreatedAt >= start.AddDays(-diff.TotalDays * 2) && x.CreatedAt <= end.AddDays(-diff.TotalDays));
            overviews.Add(attendance);

            return Ok(overviews);
        }

        // GET: api/<StatController>/[Action]/{start:datetime}/{end:datetime}
        [HttpGet("{start:datetime}/{end:datetime}")]
        [AllowAnonymous]
        public async Task<ActionResult<List<TopCourse>>> TopCourses(DateTime start, DateTime end)
        {
            if (start > end)
            {
                return BadRequest("Invalid Date Range");
            }

            var list = await attendanceService.GetList(x => x.CreatedAt >= start && x.CreatedAt <= end);
            var res = list.GroupBy(g => g.CourseId)
                .Select(s => new { CourseId = s.Key, Count = s.Count() })
                .OrderByDescending(o => o.Count)
                .Take(10)
                .ToList();
            List<TopCourse> top = new List<TopCourse>();
            foreach (var item in res)
            {
                TopCourse tc = new TopCourse();
                tc.CourseId = item.CourseId;
                tc.Count = item.Count;
                var course = await courseService.GetById(item.CourseId);
                tc.Title = course.Title;
                top.Add(tc);
            }
            return Ok(top);
        }

        // GET: api/<StatController>/[Action]/{start:datetime}/{end:datetime}
        [HttpGet("{start:datetime}/{end:datetime}")]
        public async Task<ActionResult<MarkStats>> Mark(DateTime start, DateTime end)
        {
            if (start > end)
            {
                return BadRequest("Invalid Date Range");
            }

            MarkStats mark = new MarkStats();
            var list = await examUserService.GetList(x => x.StartedAt >= start && x.StartedAt <= end);

            double total = 0.0;
            int totalTimes = 0;
            List<double> sd = new List<double>();
            foreach (var item in list)
            {
                sd.Add(item.Mark);
                total += item.Mark;
                if (item.CompletedAt != null)
                {
                    DateTime completed = (DateTime)item.CompletedAt;
                    double diff = (completed - item.StartedAt).TotalSeconds;
                    totalTimes += (int)Math.Round(diff);
                }
            }
            int length = sd.Count;
            mark.AverageMark = length == 0 ? 0.0 : total / sd.Count;
            mark.StandardDeviation = calculateSD(sd);
            mark.AverageTime = length == 0 ? 0 : totalTimes / sd.Count;

            var highest = list.OrderByDescending(x => x.Mark).FirstOrDefault();
            mark.HighestMark = highest == null ? 0.0 : highest.Mark;

            var lowest = list.OrderBy(x => x.Mark).FirstOrDefault();
            mark.LowestMark = lowest == null ? 0.0 : lowest.Mark;

            var charts = list.GroupBy(g => Math.Round(g.Mark))
                .Select(s => new MarkChart { Mark = (int)Math.Round(s.Key), Count = s.Count() })
                .OrderBy(o => o.Mark)
                .ToList();
            mark.Charts = charts;

            return Ok(mark);
        }

        // GET: api/<StatController>/[Action]/{userId}
        [HttpGet("{userId}")]
        public async Task<ActionResult<MarkStats>> Mark(int userId)
        {
            var user = await userService.GetById(userId);
            MarkStats mark = new MarkStats();
            var list = await examUserService.GetList(x => x.UserId == userId);

            double total = 0.0;
            int totalTimes = 0;
            List<double> sd = new List<double>();
            foreach (var item in list)
            {
                sd.Add(item.Mark);
                total += item.Mark;
                if (item.CompletedAt != null)
                {
                    DateTime completed = (DateTime)item.CompletedAt;
                    double diff = (completed - item.StartedAt).TotalSeconds;
                    totalTimes += (int)Math.Round(diff);
                }
            }
            int length = sd.Count;
            mark.AverageMark = length == 0 ? 0.0 : total / sd.Count;
            mark.StandardDeviation = calculateSD(sd);
            mark.AverageTime = length == 0 ? 0 : totalTimes / sd.Count;

            var highest = list.OrderByDescending(x => x.Mark).FirstOrDefault();
            mark.HighestMark = highest == null ? 0.0 : highest.Mark;

            var lowest = list.OrderBy(x => x.Mark).FirstOrDefault();
            mark.LowestMark = lowest == null ? 0.0 : lowest.Mark;

            var charts = list.GroupBy(g => Math.Round(g.Mark))
                .Select(s => new MarkChart { Mark = (int)Math.Round(s.Key), Count = s.Count() })
                .OrderBy(o => o.Mark)
                .ToList();
            mark.Charts = charts;

            return Ok(mark);
        }

        // GET: api/<StatController>/[Action]/{start:datetime}/{end:datetime}
        [AllowAnonymous]
        [HttpGet("{start:datetime}/{end:datetime}")]
        public async Task<ActionResult<UserRank>> Rank(DateTime start, DateTime end)
        {
            if (start > end)
            {
                return BadRequest("Invalid Date Range");
            }

            var list = await certificationUserService.GetList(x => x.CreatedAt >= start && x.CreatedAt <= end);
            var res = list.GroupBy(g => g.UserId)
                .Select(s => new { UserId = s.Key, Count = s.Count() })
                .OrderByDescending(o => o.Count)
                .Take(10)
                .ToList();
            List<UserRank> ranks = new List<UserRank>();
            int top = 1;
            foreach (var item in res)
            {
                UserRank rank = new UserRank();
                var user = await userService.GetById(item.UserId);
                rank.Top = top;
                rank.NoOfCourse = item.Count;
                rank.User = mapper.Map<UserCommentDTO>(user);
                rank.User.Picture = await component.GetImageAsync(user.PictureId);
                top++;
                ranks.Add(rank);
            }
            return Ok(ranks);
        }

        // GET: api/<StatController>/[Action]/{courseId}/{start:datetime}/{end:datetime}
        [AllowAnonymous]
        [HttpGet("{courseId}/{start:datetime}/{end:datetime}")]
        public async Task<ActionResult<MarkRank>> Rank(int courseId, DateTime start, DateTime end)
        {
            if (start > end)
            {
                return BadRequest("Invalid Date Range");
            }

            var list = await certificationUserService.GetList(x => x.CourseId == courseId && x.CreatedAt >= start && x.CreatedAt <= end);
            var res = list.GroupBy(g => g.UserId, g => g.Mark)
                .Select(s => new { UserId = s.Key, Mark = s.FirstOrDefault() })
                .OrderByDescending(o => o.Mark)
                .Take(10)
                .ToList();
            List<MarkRank> ranks = new List<MarkRank>();
            int top = 1;
            foreach (var item in res)
            {
                MarkRank rank = new MarkRank();
                var user = await userService.GetById(item.UserId);
                rank.Top = top;
                rank.Mark = item.Mark;
                rank.User = mapper.Map<UserCommentDTO>(user);
                rank.User.Picture = await component.GetImageAsync(user.PictureId);
                top++;
                ranks.Add(rank);
            }
            return Ok(ranks);
        }

        //Calculate Standard Deviation
        private double calculateSD(List<double> list)
        {
            if (list.Count <= 0)
            {
                return 0.0;
            }

            double sum = 0.0, standardDeviation = 0.0;
            int length = list.Count;

            foreach (double num in list)
            {
                sum += num;
            }

            double mean = sum / length;

            foreach (double num in list)
            {
                standardDeviation += Math.Pow(num - mean, 2);
            }

            return Math.Sqrt(standardDeviation / length);
        }
    }
}
