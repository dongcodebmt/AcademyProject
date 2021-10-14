using System;
using System.Collections.Generic;

#nullable disable

namespace AcademyProject.Models
{
    public partial class TrackStep
    {
        public int Id { get; set; }
        public int TrackId { get; set; }
        public string Title { get; set; }
        public int Duration { get; set; }

        public virtual Track Track { get; set; }
    }
}
