using System;
using System.ComponentModel.DataAnnotations;

namespace AcademyProject.DTOs
{
    public class CourseDTO
    {
        public int Id { get; set; }
        public int LecturerId { get; set; }
        public int CategoryId { get; set; }
        public DateTime CreateAt { get; set; }
        [Required(ErrorMessage = "The title is required")]
        public string Title { get; set; }
    }
}
