using System;
using System.Collections.Generic;

#nullable disable

namespace AcademyProject.Models
{
    public partial class ExamDetail
    {
        public int Id { get; set; }
        public int ExamUserId { get; set; }
        public int QuestionId { get; set; }
        public int? OptionId { get; set; }

        public virtual ExamUser ExamUser { get; set; }
        public virtual ExamOption Option { get; set; }
        public virtual ExamQuestion Question { get; set; }
    }
}
