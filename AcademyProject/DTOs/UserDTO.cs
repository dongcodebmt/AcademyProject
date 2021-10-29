using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace AcademyProject.DTOs
{
    public class UserDTO
    {
        [SwaggerSchema(ReadOnly = true)]
        public int Id { get; set; }
        [Required(ErrorMessage = "The email is required")]
        public string Email { get; set; }
        [Required(ErrorMessage = "The password is required")]
        public string Password { get; set; }
        [SwaggerSchema(ReadOnly = true)]
        public string PasswordHash { get; set; }
        [Required(ErrorMessage = "The first name is required")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "The last name is required")]
        public string LastName { get; set; }
        [SwaggerSchema(ReadOnly = true)]
        public int? PictureId { get; set; }
    }
}
