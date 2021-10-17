using Microsoft.AspNetCore.Mvc;

namespace AcademyProject.Entities
{
    [Route("api/[controller]")]
    [ApiController]
    public class GoogleUser : ControllerBase
    {
        public string name { get; set; }
        public string given_name { get; set; }
        public string family_name { get; set; }
        public string email { get; set; }
        public string picture { get; set; }
    }
}
