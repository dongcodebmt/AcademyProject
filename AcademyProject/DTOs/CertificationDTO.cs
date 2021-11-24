using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyProject.DTOs
{
    public class CertificationDTO
    {
        public int UserId { get; set; }
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public double Mark { get; set; }
    }
}
