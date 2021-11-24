using System;
using System.Collections.Generic;

#nullable disable

namespace AcademyProject.Models
{
    public partial class Certification
    {
        public int UserId { get; set; }
        public int CourseId { get; set; }
        public double Mark { get; set; }
        public DateTime CreatedAt { get; set; }

        public virtual Course Course { get; set; }
        public virtual User User { get; set; }
    }
}
