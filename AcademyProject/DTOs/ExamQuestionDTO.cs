using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyProject.DTOs
{
    public class ExamQuestionDTO
    {
        public int Id { get; set; }
        public int TrackStepId { get; set; }
        public string Content { get; set; }
        public bool IsDeleted { get; set; }
    }
}
