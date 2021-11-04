using System;
using System.Collections.Generic;

#nullable disable

namespace AcademyProject.Models
{
    public partial class Track
    {
        public Track()
        {
            TrackStepTexts = new HashSet<TrackStepText>();
            TrackStepVideos = new HashSet<TrackStepVideo>();
            TrackSteps = new HashSet<TrackStep>();
        }

        public int Id { get; set; }
        public int CourseId { get; set; }
        public string Title { get; set; }
        public bool IsDeleted { get; set; }

        public virtual Course Course { get; set; }
        public virtual ICollection<TrackStepText> TrackStepTexts { get; set; }
        public virtual ICollection<TrackStepVideo> TrackStepVideos { get; set; }
        public virtual ICollection<TrackStep> TrackSteps { get; set; }
    }
}
