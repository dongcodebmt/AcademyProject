using Swashbuckle.AspNetCore.Annotations;
using System;
using System.ComponentModel.DataAnnotations;

namespace AcademyProject.DTOs
{
    public class CourseDTO
    {
        [SwaggerSchema(ReadOnly = true)]
        public int Id { get; set; }
        [Required(ErrorMessage = "The lecturer id is required")]
        public int LecturerId { get; set; }
        [Required(ErrorMessage = "The category id is required")]
        public int CategoryId { get; set; }
        public int? PictureId { get; set; }
        public DateTime CreateAt { get; set; }
        [Required(ErrorMessage = "The title is required")]
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsDeleted { get; set; }
        [SwaggerSchema(ReadOnly = true)]
        public string PicturePath { get; set; }
    }
}
