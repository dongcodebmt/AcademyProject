using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyProject.DTOs
{
    public class RoleDTO
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "The Name is required")]
        public string Name { get; set; }
    }
}
