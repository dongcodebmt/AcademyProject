using System;
using System.Collections.Generic;

#nullable disable

namespace AcademyProject.Models
{
    public partial class ExamQuestion
    {
        public ExamQuestion()
        {
            ExamDetails = new HashSet<ExamDetail>();
            ExamOptions = new HashSet<ExamOption>();
        }

        public int Id { get; set; }
        public int ExamId { get; set; }
        public string Content { get; set; }
        public bool IsDeleted { get; set; }

        public virtual Exam Exam { get; set; }
        public virtual ExamRightOption ExamRightOption { get; set; }
        public virtual ICollection<ExamDetail> ExamDetails { get; set; }
        public virtual ICollection<ExamOption> ExamOptions { get; set; }
    }
}
