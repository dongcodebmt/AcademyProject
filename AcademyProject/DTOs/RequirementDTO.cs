using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyProject.DTOs
{
    public class RequirementDTO
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public string Content { get; set; }
    }
}
