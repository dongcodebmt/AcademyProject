using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace AcademyProject.DTOs
{
    public class ExamOptionDTO
    {
        [SwaggerSchema(ReadOnly = true)]
        public int? Id { get; set; }
        [SwaggerSchema(ReadOnly = true)]
        public int? QuestionId { get; set; }
        [Required(ErrorMessage = "The content is required")]
        public string Content { get; set; }
    }
}
