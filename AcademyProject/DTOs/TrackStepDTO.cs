using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyProject.DTOs
{
    public class TrackStepDTO
    {
        [SwaggerSchema(ReadOnly = true)]
        public int Id { get; set; }
        [Required(ErrorMessage = "The course id is required")]
        public int CourseId { get; set; }
        [Required(ErrorMessage = "The title is required")]
        public string Title { get; set; }
        public List<StepWithoutContentDTO> Steps { get; set; }
    }

    public class StepWithoutContentDTO
    {
        public int Id { get; set; }
        public int TrackId { get; set; }
        public string Title { get; set; }
        public int Duration { get; set; }
        public bool Completed { get; set; }
    }
}
