using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyProject.DTOs
{
    public class TrackStepTextDTO
    {
        public int Id { get; set; }
        public int TrackId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
    }
}
