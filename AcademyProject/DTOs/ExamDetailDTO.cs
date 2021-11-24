using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyProject.DTOs
{
    public class ExamDetailDTO
    {
        public int Id { get; set; }
        public int ExamUserId { get; set; }
        public int QuestionId { get; set; }
        public int? OptionId { get; set; }
    }

    public class ExamDetailWithContentDTO
    {
        public int QuestionId { get; set; }
        public string QuestionContent { get; set; }
        public int? OptionId { get; set; }
        public string OptionContent { get; set; }
        public bool IsRight { get; set; }
    }
}
