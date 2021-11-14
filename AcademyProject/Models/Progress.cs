using System;
using System.Collections.Generic;

#nullable disable

namespace AcademyProject.Models
{
    public partial class Progress
    {
        public int StepId { get; set; }
        public int UserId { get; set; }
        public DateTime StartedAt { get; set; }

        public virtual Step Step { get; set; }
        public virtual User User { get; set; }
    }
}
