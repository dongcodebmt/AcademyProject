using AcademyProject.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyProject.DTOs
{
    public class BlogDTO
    {
        public BlogDTO()
        {
            BlogComments = new HashSet<BlogCommentDTO>();
        }
        public int Id { get; set; }
        [Required(ErrorMessage = "The UserId is required")]
        public int UserId { get; set; }
        public int CategoryId { get; set; }
        [Required(ErrorMessage = "The Title is required")]
        public string Title { get; set; }
        [Required(ErrorMessage = "The Content is required")]
        public string Content { get; set; }
        public DateTime CreateAt { get; set; }
        public bool IsDeleted { get; set; }
        public HashSet<BlogCommentDTO> BlogComments { get; }
    }
}
