using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SMSystem.Models
{
    public class CourseModel
    {
        public int CourseId { get; set; }

        [Display(Name = "Course Name")]
        [Required]
        public string CourseName { get; set; }

        [Display(Name = "Duration")]
        [Required]
        public int Duration { get; set; }

        [Display(Name = "Description")]
        [Required]
        public string Description { get; set; }

        public bool IsValid { get; set; }
    }
}