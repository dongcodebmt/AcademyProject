using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyProject.DTOs
{
    public class AnswerDTO
    {
        [SwaggerSchema(ReadOnly = true)]
        public int Id { get; set; }
        [Required(ErrorMessage = "The question id is required")]
        public int QuestionId { get; set; }
        [SwaggerSchema(ReadOnly = true)]
        public int UserId { get; set; }
        [Required(ErrorMessage = "The content is required")]
        public string Content { get; set; }
        [SwaggerSchema(ReadOnly = true)]
        public DateTime CreatedAt { get; set; }
        [SwaggerSchema(ReadOnly = true)]
        public DateTime UpdatedAt { get; set; }
        [SwaggerSchema(ReadOnly = true)]
        public UserCommentDTO User { get; set; }
    }
}
