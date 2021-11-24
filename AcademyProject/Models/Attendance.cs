using System;
using System.Collections.Generic;

#nullable disable

namespace AcademyProject.Models
{
    public partial class Attendance
    {
        public int CourseId { get; set; }
        public int UserId { get; set; }
        public int Credits { get; set; }
        public DateTime CreatedAt { get; set; }

        public virtual Course Course { get; set; }
        public virtual User User { get; set; }
    }
}
