using System;
using System.Collections.Generic;

#nullable disable

namespace AcademyProject.Models
{
    public partial class ExamUser
    {
        public ExamUser()
        {
            ExamDetails = new HashSet<ExamDetail>();
        }

        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime DateOfExam { get; set; }

        public virtual User User { get; set; }
        public virtual ICollection<ExamDetail> ExamDetails { get; set; }
    }
}
