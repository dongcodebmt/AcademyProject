using System;
using System.Collections.Generic;

#nullable disable

namespace AcademyProject.Models
{
    public partial class ExamUser
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime DateOfExam { get; set; }

        public virtual User User { get; set; }
        public virtual ExamDetail ExamDetail { get; set; }
    }
}
