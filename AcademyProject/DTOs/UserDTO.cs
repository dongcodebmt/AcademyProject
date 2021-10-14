using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyProject.DTOs
{
    public class UserDTO
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "The Email is required")]
        public string Email { get; set; }
        [Required(ErrorMessage = "The PasswordHash is required")]
        public string PasswordHash { get; set; }
        [Required(ErrorMessage = "The FirstName is required")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "The LastName is required")]
        public string LastName { get; set; }
    }
}
