using System;
using System.Collections.Generic;

#nullable disable

namespace AcademyProject.Models
{
    public partial class Exam
    {
        public Exam()
        {
            ExamQuestions = new HashSet<ExamQuestion>();
            ExamUsers = new HashSet<ExamUser>();
        }

        public int Id { get; set; }
        public int CourseId { get; set; }
        public string Title { get; set; }
        public int ExamDuration { get; set; }
        public bool IsDeleted { get; set; }

        public virtual Course Course { get; set; }
        public virtual ICollection<ExamQuestion> ExamQuestions { get; set; }
        public virtual ICollection<ExamUser> ExamUsers { get; set; }
    }
}
