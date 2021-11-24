using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyProject.DTOs
{
    public class ExamUserDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ExamId { get; set; }
        public int NoOfQuestion { get; set; }
        public DateTime StartedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public int? NoOfRightOption { get; set; }
        public double Mark { get; set; }
        [SwaggerSchema(ReadOnly = true)]
        public List<ExamDetailWithContentDTO> Details { get; set; }
        [SwaggerSchema(ReadOnly = true)]
        public string Title { get; set; }
    }
}
