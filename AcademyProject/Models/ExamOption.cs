using System;
using System.Collections.Generic;

#nullable disable

namespace AcademyProject.Models
{
    public partial class ExamOption
    {
        public ExamOption()
        {
            ExamDetails = new HashSet<ExamDetail>();
            ExamRightOptions = new HashSet<ExamRightOption>();
        }

        public int Id { get; set; }
        public int QuestionId { get; set; }
        public string Content { get; set; }

        public virtual ExamQuestion Question { get; set; }
        public virtual ICollection<ExamDetail> ExamDetails { get; set; }
        public virtual ICollection<ExamRightOption> ExamRightOptions { get; set; }
    }
}
