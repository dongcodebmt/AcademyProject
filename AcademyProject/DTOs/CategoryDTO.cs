using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace AcademyProject.DTOs
{
    public class CategoryDTO
    {
        [SwaggerSchema(ReadOnly = true)]
        public int Id { get; set; }

        [Required(ErrorMessage = "The name is required")]
        public string Name { get; set; }
    }
}
