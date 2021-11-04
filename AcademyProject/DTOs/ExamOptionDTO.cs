using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyProject.DTOs
{
    public class ExamOptionDTO
    {
        public int Id { get; set; }
        public int QuestionId { get; set; }
        public string Content { get; set; }
    }
}
