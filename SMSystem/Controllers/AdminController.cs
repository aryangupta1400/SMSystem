using SMSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

                administrator.Password = EncryptPassword(newAdministrator.Password);

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
                            .Where(a => a.AdminEmail.Equals(user.AdminEmail)).FirstOrDefault();

                if(admin != null && (DecryptPassword(admin.Password) == user.Password))
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

        private string EncryptPassword(string password)
        {
            StringBuilder encryptedPassword = new StringBuilder(password);
            for (int i = 0; i < password.Length; i++)
            {
                if (i % 2 == 0)
                {
                    encryptedPassword[i] = (char)(((int)password[i]) + 1);
                }
                else
                {
                    encryptedPassword[i] = (char)(((int)password[i]) - 1);
                }
            }
            return encryptedPassword.ToString();
        }

        private string DecryptPassword(string password)
        {
            StringBuilder encryptedPassword = new StringBuilder(password);
            for (int i = 0; i < password.Length; i++)
            {
                if (i % 2 != 0)
                {
                    encryptedPassword[i] = (char)(((int)password[i]) + 1);
                }
                else
                {
                    encryptedPassword[i] = (char)(((int)password[i]) - 1);
                }
            }
            return encryptedPassword.ToString();
        }

    }
}

