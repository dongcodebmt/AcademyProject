using Swashbuckle.AspNetCore.Annotations;
using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace AcademyProject.DTOs
{
    public class BlogCommentDTO
    {
        [SwaggerSchema(ReadOnly = true)]
        public int Id { get; set; }
        [Required(ErrorMessage = "The blog id is required")]
        public int BlogId { get; set; }
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

    public class UserCommentDTO
    {
        [SwaggerSchema(ReadOnly = true)]
        public int Id { get; set; }
        [SwaggerSchema(ReadOnly = true)]
        public string FirstName { get; set; }
        [SwaggerSchema(ReadOnly = true)]
        public string LastName { get; set; }
        [SwaggerSchema(ReadOnly = true)]
        public int? PictureId { get; set; }
        [SwaggerSchema(ReadOnly = true)]
        public string Picture { get; set; }
    }
}
