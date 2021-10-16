using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyProject.Entities
{
    public class JWT
    {
        [Required(ErrorMessage = "The token is required")]
        public string Token { get; set; }
        public DateTime Expires { get; set; }
        public JWT()
        {

        }
        public JWT(string Token, DateTime Expires)
        {
            this.Token = Token;
            this.Expires = Expires;
        }
    }
}
