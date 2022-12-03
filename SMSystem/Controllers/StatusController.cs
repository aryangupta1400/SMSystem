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
        // GET: Status

        StudentInformationDBEntities studentInformationDBEntities = new StudentInformationDBEntities();

        public ActionResult StatusList()
        {
            var status = studentInformationDBEntities.Status.ToList();

            return View(status);
        }
    }
}