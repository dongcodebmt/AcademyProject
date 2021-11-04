using System;
using System.Collections.Generic;

#nullable disable

namespace AcademyProject.Models
{
    public partial class TrackStepVideo
    {
        public int Id { get; set; }
        public int TrackId { get; set; }
        public string Type { get; set; }
        public string Data { get; set; }

        public virtual Track Track { get; set; }
    }
}
