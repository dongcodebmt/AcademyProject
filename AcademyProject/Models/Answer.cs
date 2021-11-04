using System;
using System.Collections.Generic;

#nullable disable

namespace AcademyProject.Models
{
    public partial class Answer
    {
        public int Id { get; set; }
        public int QuestionId { get; set; }
        public int UserId { get; set; }
        public string Content { get; set; }
        public DateTime CreateAt { get; set; }

        public virtual Question Question { get; set; }
        public virtual User User { get; set; }
    }
}
