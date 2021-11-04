using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyProject.DTOs
{
    public class TrackStepVideoDTO
    {
        public int Id { get; set; }
        public int TrackId { get; set; }
        public string Type { get; set; }
        public string Data { get; set; }
    }
}
