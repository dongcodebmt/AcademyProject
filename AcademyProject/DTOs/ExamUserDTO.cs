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
        public DateTime DateOfExam { get; set; }
    }
}
