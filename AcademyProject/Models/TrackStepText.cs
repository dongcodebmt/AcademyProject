using System;
using System.Collections.Generic;

#nullable disable

namespace AcademyProject.Models
{
    public partial class TrackStepText
    {
        public int Id { get; set; }
        public int TrackId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

        public virtual Track Track { get; set; }
    }
}
