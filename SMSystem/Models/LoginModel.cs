using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;

namespace SMSystem.Models
{
    public class LoginModel
    {
        [Display(Name = "Email")]
        [Required(ErrorMessage = "Requried")]
        [EmailAddress]
        public string AdminEmail { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = "Requried")]
        [DataType(DataType.Password)]
        [PasswordPropertyText]
        public string Password { get; set; }
    }
}

