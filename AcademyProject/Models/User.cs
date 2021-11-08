﻿using System;
using System.Collections.Generic;

#nullable disable

namespace AcademyProject.Models
{
    public partial class User
    {
        public User()
        {
            Answers = new HashSet<Answer>();
            Attendances = new HashSet<Attendance>();
            BlogComments = new HashSet<BlogComment>();
            Blogs = new HashSet<Blog>();
            Courses = new HashSet<Course>();
            ExamUsers = new HashSet<ExamUser>();
            Questions = new HashSet<Question>();
            UserRoles = new HashSet<UserRole>();
        }

        public int Id { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int? PictureId { get; set; }

        public virtual Picture Picture { get; set; }
        public virtual ICollection<Answer> Answers { get; set; }
        public virtual ICollection<Attendance> Attendances { get; set; }
        public virtual ICollection<BlogComment> BlogComments { get; set; }
        public virtual ICollection<Blog> Blogs { get; set; }
        public virtual ICollection<Course> Courses { get; set; }
        public virtual ICollection<ExamUser> ExamUsers { get; set; }
        public virtual ICollection<Question> Questions { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}
