using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyProject.DTOs
{
    public class RequirementDTO
    {
        [SwaggerSchema(ReadOnly = true)]
        public int Id { get; set; }
        [Required(ErrorMessage = "The course id is required")]
        public int CourseId { get; set; }
        [Required(ErrorMessage = "The content is required")]
        public string Content { get; set; }
    }
}
