using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyProject.DTOs
{
    public class StatsDTO
    {
    }

    public class MarkStats
    {
        public double AverageMark { get; set; }
        public double LowestMark { get; set; }
        public double HighestMark { get; set; }
        public double StandardDeviation { get; set; }
        public int AverageTime { get; set; }
        public List<MarkChart> Charts { get; set; }
    }

    public class MarkChart
    {
        public int Mark { get; set; }
        public int Count { get; set; }
    }

    public class TopCourse
    {
        public int CourseId { get; set; }
        public string Title { get; set; }
        public int Count { get; set; }
    }

    public class Overview
    {
        public string Label { get; set; }
        public string Icon { get; set; }
        public int Total { get; set; }
        public int ThisTime { get; set; }
        public int LastTime { get; set; }
    }
}
