using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyProject.Entities
{
    public class JWT
    {
        public string AccessToken { get; set; }
        public DateTime AccessTokenExpires { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpires { get; set; }
        public JWT()
        {

        }
        public JWT(string AccessToken, DateTime AccessTokenExpires, string? RefreshToken, DateTime? RefreshTokenExpires)
        {
            this.AccessToken = AccessToken;
            this.AccessTokenExpires = AccessTokenExpires;
            this.RefreshToken = RefreshToken;
            this.RefreshTokenExpires = RefreshTokenExpires;
        }
    }
}
