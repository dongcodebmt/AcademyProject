using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace AcademyProject.DTOs
{
    public class ExamRightOptionDTO
    {
        [SwaggerSchema(ReadOnly = true)]
        public int QuestionId { get; set; }
        [SwaggerSchema(ReadOnly = true)]
        public int OptionId { get; set; }
        [Required(ErrorMessage = "The index is required")]
        public int Index { get; set; }
    }
}
