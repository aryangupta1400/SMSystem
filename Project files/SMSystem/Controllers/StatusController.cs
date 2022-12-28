using SMSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SMSystem.Controllers
{
    public class StatusController : Controller
    {
        // object to access DB
        StudentInformationDBEntities studentInformationDBEntities = new StudentInformationDBEntities();

        // Display the status list
        public ActionResult StatusList()
        {
            if (Session["AdminId"] != null)
            {
                var status = studentInformationDBEntities.Status.ToList();

                return View(status);
            }
            
            return RedirectToAction("LoginError", "Home");

        }
    }
}