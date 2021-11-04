using System;
using System.Collections.Generic;

#nullable disable

namespace AcademyProject.Models
{
    public partial class BlogComment
    {
        public int Id { get; set; }
        public int BlogId { get; set; }
        public int UserId { get; set; }
        public string Content { get; set; }
        public DateTime CreateAt { get; set; }

        public virtual Blog Blog { get; set; }
        public virtual User User { get; set; }
    }
}
