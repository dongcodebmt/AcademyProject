using System;
using System.Collections.Generic;

#nullable disable

namespace AcademyProject.Models
{
    public partial class Course
    {
        public Course()
        {
            Attendances = new HashSet<Attendance>();
            ExamQuestions = new HashSet<ExamQuestion>();
            Requirements = new HashSet<Requirement>();
            Tracks = new HashSet<Track>();
            WillLearns = new HashSet<WillLearn>();
        }

        public int Id { get; set; }
        public int LecturerId { get; set; }
        public int CategoryId { get; set; }
        public int? PictureId { get; set; }
        public DateTime CreateAt { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsDeleted { get; set; }

        public virtual Category Category { get; set; }
        public virtual User Lecturer { get; set; }
        public virtual Picture Picture { get; set; }
        public virtual ICollection<Attendance> Attendances { get; set; }
        public virtual ICollection<ExamQuestion> ExamQuestions { get; set; }
        public virtual ICollection<Requirement> Requirements { get; set; }
        public virtual ICollection<Track> Tracks { get; set; }
        public virtual ICollection<WillLearn> WillLearns { get; set; }
    }
}
