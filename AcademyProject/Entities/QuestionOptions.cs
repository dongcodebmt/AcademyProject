using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyProject.Entities
{
    public class QuestionOptions
    {
        [Required(ErrorMessage = "The question id is required")]
        public int QuestionId { get; set; }
        [Required(ErrorMessage = "The option id is required")]
        public int OptionId { get; set; }
    }
}
