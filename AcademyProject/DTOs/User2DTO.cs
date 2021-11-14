using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AcademyProject.DTOs
{
    public class User2DTO
    {
        [SwaggerSchema(ReadOnly = true)]
        public int Id { get; set; }
        [Required(ErrorMessage = "The email is required")]
        public string Email { get; set; }
        [Required(ErrorMessage = "The first name is required")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "The last name is required")]
        public string LastName { get; set; }
        public int? PictureId { get; set; }
        [SwaggerSchema(ReadOnly = true)]
        public string Picture { get; set; }
        [SwaggerSchema(ReadOnly = true)]
        public List<string> Scope { get; set; }
    }
}
