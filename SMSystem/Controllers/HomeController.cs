using SMSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SMSystem.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        StudentInformationDBEntities studentInformationDBEntities = new StudentInformationDBEntities();

        public ActionResult Dashboard()
        {
            var activeUser = studentInformationDBEntities.Administrators.Where(a => a.IsActive == true).ToList();

            if(activeUser.Count == 0)
            {
                return View();
            }
            else
            {
                for(int i = 0; i < activeUser.Count; i++)
                {
                    Administrator administrator = studentInformationDBEntities.Administrators.Find(activeUser[i].AdminId);

                    administrator.IsActive = false;

                    studentInformationDBEntities.SaveChanges();
                }                
            }

            return View();
        }

        public ActionResult WelcomeScreen()
        {
            return View();
        }

        public ActionResult LoginError()
        {
            return View();
        }
    }
}