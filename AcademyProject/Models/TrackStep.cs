using System;
using System.Collections.Generic;

#nullable disable

namespace AcademyProject.Models
{
    public partial class TrackStep
    {
        public TrackStep()
        {
            ExamQuestions = new HashSet<ExamQuestion>();
        }

        public int Id { get; set; }
        public int TrackId { get; set; }
        public string Title { get; set; }
        public int Duration { get; set; }
        public int Type { get; set; }
        public bool IsDeleted { get; set; }

        public virtual Track Track { get; set; }
        public virtual ICollection<ExamQuestion> ExamQuestions { get; set; }
    }
}
