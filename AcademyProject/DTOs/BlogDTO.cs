using AcademyProject.Models;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyProject.DTOs
{
    public class BlogDTO
    {
        [SwaggerSchema(ReadOnly = true)]
        public int Id { get; set; }
        [Required(ErrorMessage = "The UserId is required")]
        public int UserId { get; set; }
        public int CategoryId { get; set; }
        [Required(ErrorMessage = "The Title is required")]
        public string Title { get; set; }
        [Required(ErrorMessage = "The Content is required")]
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
        public int? PictureId { get; set; }
        [SwaggerSchema(ReadOnly = true)]
        public string PicturePath { get; set; }
    }
}
