using SMSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SMSystem.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin

        StudentInformationDBEntities studentInformationDBEntities = new StudentInformationDBEntities();

        public ActionResult AdminRegistration()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AdminRegistration(AdminModel newAdministrator)
        {
            if (ModelState.IsValid)
            {
                Administrator administrator = new Administrator();

                administrator.AdminName = newAdministrator.AdminName;
                administrator.AdminEmail = newAdministrator.AdminEmail;
                administrator.Password = newAdministrator.Password;

                studentInformationDBEntities.Administrators.Add(administrator);

                studentInformationDBEntities.SaveChanges();

                ModelState.Clear();

                return RedirectToAction("AdminLogin");
            }
            return View();            
        }

        /*public ActionResult AdminLogin()
        {
            Session.Clear();
            return View();
        }*/

        public ActionResult AdminLogin(LoginModel user)
        {
            Session.Clear();

            if (ModelState.IsValid)
            {
                var admin = studentInformationDBEntities.Administrators.ToList()
                            .Where(a => a.AdminEmail.Equals(user.AdminEmail) && a.Password.Equals(user.Password)).FirstOrDefault();

                if(admin != null && (admin.Password == user.Password))
                {
                    Session["AdminId"] = admin.AdminId.ToString();
                    Session["AdminName"] = admin.AdminName.ToString();

                    return RedirectToAction("StudentList", "Student");
                }
                else
                {
                    ViewBag.Message = "Invalid user details.! \n Try Again!";
                }
            }
            return View(user);
        }


        public ActionResult Logout()
        {
            Session["AdminId"] = null;
            return RedirectToAction("AdminLogin");
        }


        public JsonResult doesAdminExist(string AdminEmail)
        {
            return Json(!studentInformationDBEntities.Administrators.Any(x => x.AdminEmail == AdminEmail), JsonRequestBehavior.AllowGet);
        }
    }
}

