using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyProject.DTOs
{
    public class QuestionDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int CategoryId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreateAt { get; set; }
        public bool IsDeleted { get; set; }
    }
}
