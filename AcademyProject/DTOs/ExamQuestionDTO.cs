using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace AcademyProject.DTOs
{
    public class ExamQuestionDTO
    {
        [SwaggerSchema(ReadOnly = true)]
        public int? Id { get; set; }
        [Required(ErrorMessage = "The exam id is required")]
        public int ExamId { get; set; }
        [Required(ErrorMessage = "The content is required")]
        public string Content { get; set; }
        [SwaggerSchema(ReadOnly = true)]
        public bool IsDeleted { get; set; }
    }
}
