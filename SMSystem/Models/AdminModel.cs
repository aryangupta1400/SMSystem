using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CompareAttribute = System.ComponentModel.DataAnnotations.CompareAttribute;

namespace SMSystem.Models
{
    public class AdminModel
    {
        public int AdminId { get; set; }

        [Display(Name = "Name")]
        [Required(ErrorMessage = "Requried")]
        public string AdminName { get; set; }

        [Display(Name = "Email")]
        [Required(ErrorMessage = "Requried")]
        [EmailAddress]
        [Remote("doesAdminExist", "Admin", ErrorMessage = "Admin already exist. Try to Login.")]        
        public string AdminEmail { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = "Requried")]
        [MinLength(6)]
        [DataType(DataType.Password)]
        [PasswordPropertyText]
        public string Password { get; set; }

        [Display(Name = "Confirm Password")]
        [Required(ErrorMessage = "Requried")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password does not match.")]
        public string ConfirmPassword { get; set; }

    }
}
