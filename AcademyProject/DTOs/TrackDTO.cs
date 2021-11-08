using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace AcademyProject.DTOs
{
    public class TrackDTO
    {
        [SwaggerSchema(ReadOnly = true)]
        public int Id { get; set; }
        [Required(ErrorMessage = "The course id is required")]
        public int CourseId { get; set; }
        [Required(ErrorMessage = "The title is required")]
        public string Title { get; set; }
        [SwaggerSchema(ReadOnly = true)]
        public bool IsDeleted { get; set; }
    }
}
