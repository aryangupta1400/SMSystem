using SMSystem.Controllers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.Mvc;

namespace SMSystem.Models
{
    public class StudentModel
    {
        [Display(Name = "Student Id")]
        public int StudentId { get; set; }

        [Display(Name = "Student Name")]
        [Required]
        public string StudentName { get; set; }

        [Display(Name = "Student Email")]
        [Required]
        [EmailAddress]
        public string StudentEmail { get; set; }

        [Display(Name = "Student Phone no.")]
        [MinLength(10)]
        [MaxLength(10)]
        public string StudentMobile { get; set; }

        [Display(Name = "Gender")]
        [Required]
        public string Gender { get; set; }

        [Display(Name = "Date of birth")]
        [Required]
        [Remote("CheckAge", "Student", ErrorMessage = "Age must be atlest 16 years.")]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]        
        public DateTime StudentDoB { get; set; }
        public Nullable<int> StudentAge { get; set; }

        [Display(Name = "Address")]
        [Required]
        public string Address { get; set; }

        [Display(Name = "Course")]
        [Required]
        public int CourseId { get; set; }

        [Display(Name = "Joining Date")]
        [Required]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> JoiningDate { get; set; }

        [Display(Name = "Current Status")]
        [Required]
        public Nullable<int> StatusCode { get; set; }

/*
        [Display(Name = "Father Name")]
        [Required]
        public string FatherName { get; set; }

        [Display(Name = "Father Email")]
        public string FatherEmail { get; set; }

        [Display(Name = "Father Phone No.")]
        [Required]
        *//*[MinLength(10)]
        [MaxLength(10)]*//*
        public string FatherMobile { get; set; }

        [Display(Name = "Mother Name")]
        [Required]
        public string MotherName { get; set; }

        [Display(Name = "Mother Email")]
        public string MotherEmail { get; set; }

        [Display(Name = "Mother Phone No.")]
        [Required]
        *//*[MinLength(10)]
        [MaxLength(10)]*//*
        public string MotherMobile { get; set; }

        [Display(Name = "Mother Occupation")]
        [Required]
        public string MotherOccupation { get; set; }

        [Display(Name = "Father Occupation")]
        [Required]
        public string FatherOccupation { get; set; }*/
    }
}