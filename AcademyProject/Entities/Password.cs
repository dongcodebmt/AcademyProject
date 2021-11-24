using System.ComponentModel.DataAnnotations;

namespace AcademyProject.Entities
{
    public class Password
    {
        public string OldPassword { get; set; }
        [Required(ErrorMessage = "The new password is required")]
        public string NewPassword { get; set; }
    }
}
