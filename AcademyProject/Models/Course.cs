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
            Certifications = new HashSet<Certification>();
            Exams = new HashSet<Exam>();
            Requirements = new HashSet<Requirement>();
            Tracks = new HashSet<Track>();
            WillLearns = new HashSet<WillLearn>();
        }

        public int Id { get; set; }
        public int LecturerId { get; set; }
        public int CategoryId { get; set; }
        public int? PictureId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Credits { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }

        public virtual Category Category { get; set; }
        public virtual User Lecturer { get; set; }
        public virtual Picture Picture { get; set; }
        public virtual ICollection<Attendance> Attendances { get; set; }
        public virtual ICollection<Certification> Certifications { get; set; }
        public virtual ICollection<Exam> Exams { get; set; }
        public virtual ICollection<Requirement> Requirements { get; set; }
        public virtual ICollection<Track> Tracks { get; set; }
        public virtual ICollection<WillLearn> WillLearns { get; set; }
    }
}
