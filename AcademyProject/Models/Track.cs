using System;
using System.Collections.Generic;

#nullable disable

namespace AcademyProject.Models
{
    public partial class Track
    {
        public Track()
        {
            TrackSteps = new HashSet<TrackStep>();
        }

        public int Id { get; set; }
        public int CourseId { get; set; }
        public string Title { get; set; }

        public virtual Course Course { get; set; }
        public virtual ICollection<TrackStep> TrackSteps { get; set; }
    }
}
