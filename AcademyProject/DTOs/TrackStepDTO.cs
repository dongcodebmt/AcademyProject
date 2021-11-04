using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyProject.DTOs
{
    public class TrackStepDTO
    {
        public int Id { get; set; }
        public int TrackId { get; set; }
        public string Title { get; set; }
        public int Duration { get; set; }
        public int Type { get; set; }
        public bool IsDeleted { get; set; }
    }
}
