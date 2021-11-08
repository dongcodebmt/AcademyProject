using System;
using System.Collections.Generic;

#nullable disable

namespace AcademyProject.Models
{
    public partial class Track
    {
        public Track()
        {
            Steps = new HashSet<Step>();
        }

        public int Id { get; set; }
        public int CourseId { get; set; }
        public string Title { get; set; }
        public bool IsDeleted { get; set; }

        public virtual Course Course { get; set; }
        public virtual ICollection<Step> Steps { get; set; }
    }
}
