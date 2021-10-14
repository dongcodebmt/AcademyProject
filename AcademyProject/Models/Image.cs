using System;
using System.Collections.Generic;

#nullable disable

namespace AcademyProject.Models
{
    public partial class Image
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string ImagePath { get; set; }

        public virtual User User { get; set; }
    }
}
