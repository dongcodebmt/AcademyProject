using AcademyProject.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyProject.Entities
{
    public class UserRank
    {
        public int Top { get; set; }
        public int NoOfCourse { get; set; }
        public UserCommentDTO User { get; set; }
    }

    public class MarkRank
    {
        public int Top { get; set; }
        public double Mark { get; set; }
        public UserCommentDTO User { get; set; }
    }
}
