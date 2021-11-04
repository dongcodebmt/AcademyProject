using System;
using System.Collections.Generic;

#nullable disable

namespace AcademyProject.Models
{
    public partial class ExamRightOption
    {
        public int QuestionId { get; set; }
        public int OptionId { get; set; }

        public virtual ExamOption Option { get; set; }
        public virtual ExamQuestion Question { get; set; }
    }
}
