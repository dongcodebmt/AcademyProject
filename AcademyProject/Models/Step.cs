using System;
using System.Collections.Generic;

#nullable disable

namespace AcademyProject.Models
{
    public partial class Step
    {
        public Step()
        {
            Progresses = new HashSet<Progress>();
        }

        public int Id { get; set; }
        public int TrackId { get; set; }
        public string Title { get; set; }
        public int Duration { get; set; }
        public string Content { get; set; }
        public string EmbedLink { get; set; }
        public bool IsDeleted { get; set; }

        public virtual Track Track { get; set; }
        public virtual ICollection<Progress> Progresses { get; set; }
    }
}
