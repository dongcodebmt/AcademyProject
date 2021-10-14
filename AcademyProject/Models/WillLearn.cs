using System;
using System.Collections.Generic;

#nullable disable

namespace AcademyProject.Models
{
    public partial class WillLearn
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public string Content { get; set; }

        public virtual Course Course { get; set; }
    }
}
