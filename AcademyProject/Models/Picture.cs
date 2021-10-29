using System;
using System.Collections.Generic;

#nullable disable

namespace AcademyProject.Models
{
    public partial class Picture
    {
        public Picture()
        {
            Blogs = new HashSet<Blog>();
            Courses = new HashSet<Course>();
            Users = new HashSet<User>();
        }

        public int Id { get; set; }
        public string PicturePath { get; set; }

        public virtual ICollection<Blog> Blogs { get; set; }
        public virtual ICollection<Course> Courses { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}
