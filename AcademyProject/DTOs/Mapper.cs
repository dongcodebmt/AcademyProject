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

            CreateMap<UserRoleDTO, UserRole>();
            CreateMap<UserRole, UserRoleDTO>();

            CreateMap<RoleDTO, Role>();
            CreateMap<Role, RoleDTO>();

            CreateMap<CourseDTO, Course>();
            CreateMap<Course, CourseDTO>();

            CreateMap<AnswerDTO, Answer>();
            CreateMap<Answer, AnswerDTO>();

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

            CreateMap<TrackStepDTO, TrackStep>();
            CreateMap<TrackStep, TrackStepDTO>();

            CreateMap<WillLearnDTO, WillLearn>();
            CreateMap<WillLearn, WillLearnDTO>();


        }
    }
}
