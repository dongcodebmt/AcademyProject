using AcademyProject.Models;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyProject.DTOs
{
    public class Mapper : Profile
    {
        public Mapper()
        {
            CreateMap<UserDTO, User>();
            CreateMap<User, UserDTO>();

            CreateMap<User2DTO, User>();
            CreateMap<User, User2DTO>();

            CreateMap<User, UserCommentDTO>();

            CreateMap<RoleDTO, Role>();
            CreateMap<Role, RoleDTO>();

            CreateMap<CourseDTO, Course>();
            CreateMap<Course, CourseDTO>();

            CreateMap<AnswerDTO, Answer>();
            CreateMap<Answer, AnswerDTO>();

            CreateMap<BlogDTO, Blog>();
            CreateMap<Blog, BlogDTO>();

            CreateMap<BlogCommentDTO, BlogComment>();
            CreateMap<BlogComment, BlogCommentDTO>();

            CreateMap<CategoryDTO, Category>();
            CreateMap<Category, CategoryDTO>();

            CreateMap<PictureDTO, Picture>();
            CreateMap<Picture, PictureDTO>();

            CreateMap<QuestionDTO, Question>();
            CreateMap<Question, QuestionDTO>();

            CreateMap<RequirementDTO, Requirement>();
            CreateMap<Requirement, RequirementDTO>();

            CreateMap<TrackDTO, Track>();
            CreateMap<Track, TrackDTO>();

            CreateMap<StepDTO, Step>();
            CreateMap<Step, StepDTO>();

            CreateMap<WillLearnDTO, WillLearn>();
            CreateMap<WillLearn, WillLearnDTO>();

            CreateMap<ExamDTO, Exam>();
            CreateMap<Exam, ExamDTO>();

            CreateMap<ExamQuestionDTO, ExamQuestion>();
            CreateMap<ExamQuestion, ExamQuestionDTO>();

            CreateMap<ExamOptionDTO, ExamOption>();
            CreateMap<ExamOption, ExamOptionDTO>();

            CreateMap<ExamRightOptionDTO, ExamRightOption>();
            CreateMap<ExamRightOption, ExamRightOptionDTO>();

            CreateMap<ExamUserDTO, ExamUser>();
            CreateMap<ExamUser, ExamUserDTO>();

            CreateMap<ExamDetailDTO, ExamDetail>();
            CreateMap<ExamDetail, ExamDetailDTO>();

            CreateMap<CertificationDTO, Certification>();
            CreateMap<Certification, CertificationDTO>();

            //Track steps
            CreateMap<Track, TrackStepDTO>();
            CreateMap<Step, StepWithoutContentDTO>();
        }
    }
}
