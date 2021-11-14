using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AcademyProject.DTOs
{
    public class QuestionFullDTO
    {
        [Required(ErrorMessage = "The question is required")]
        public ExamQuestionDTO Question { get; set; }
        [Required(ErrorMessage = "The options is required")]
        public List<ExamOptionDTO> Options { get; set; }
        [Required(ErrorMessage = "The right option is required")]
        public ExamRightOptionDTO RightOption { get; set; }
    }
}
