using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyProject.Entities
{
    public class Token
    {
        public string AccessToken { get; set; }
        public DateTime Expires { get; set; }
        public Token()
        {

        }
        public Token(string AccessToken, DateTime Expires)
        {
            this.AccessToken = AccessToken;
            this.Expires = Expires;
        }
    }
}
