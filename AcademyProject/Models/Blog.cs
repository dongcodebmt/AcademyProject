using System;
using System.Collections.Generic;

#nullable disable

namespace AcademyProject.Models
{
    public partial class Blog
    {
        public Blog()
        {
            BlogComments = new HashSet<BlogComment>();
        }

        public int Id { get; set; }
        public int UserId { get; set; }
        public int CategoryId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreateAt { get; set; }
        public bool IsDeleted { get; set; }
        public int? PictureId { get; set; }

        public virtual Category Category { get; set; }
        public virtual Picture Picture { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<BlogComment> BlogComments { get; set; }
    }
}
