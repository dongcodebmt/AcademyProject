using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyProject.Entities
{
    public class Login
    {
        [Required(ErrorMessage = "The Email is required")]
        public string Email { get; set; }
        [Required(ErrorMessage = "The Password is required")]
        public string Password { get; set; }
        public Login()
        {

        }
        public Login(string Email, string Password)
        {
            this.Email = Email;
            this.Password = Password;
        }
    }
}
