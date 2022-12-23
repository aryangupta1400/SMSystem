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
        // index method [NOT PART OF PROJECT]
        public ActionResult Index()
        {
            return View();
        }

        // About page [NOT PART OF PROJECT]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        // contact page [NOT PART OF PROJECT]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        // object to access DB
        StudentInformationDBEntities studentInformationDBEntities = new StudentInformationDBEntities();

        // Dashboard page 
        public ActionResult Dashboard()
        {
            var activeUser = studentInformationDBEntities.Administrators.Where(a => a.IsActive == true).ToList();

            if(activeUser.Count == 0)
            {
                return View();
            }
            else
            {
                #region admin status update
                for (int i = 0; i < activeUser.Count; i++)
                {
                    Administrator administrator = studentInformationDBEntities.Administrators.Find(activeUser[i].AdminId);

                    administrator.IsActive = false;

                    studentInformationDBEntities.SaveChanges();
                }
                #endregion
            }

            return View();
        }
        
        // welcome method 
        public ActionResult WelcomeScreen()
        {
            return View();
        }

        // login error 
        public ActionResult LoginError()
        {
            return View();
        }
    }
}