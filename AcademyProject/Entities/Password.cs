using System.ComponentModel.DataAnnotations;

namespace AcademyProject.Entities
{
    public class Password
    {
        [Required(ErrorMessage = "The old password is required")]
        public string OldPassword { get; set; }
        [Required(ErrorMessage = "The new password is required")]
        public string NewPassword { get; set; }
    }
}
