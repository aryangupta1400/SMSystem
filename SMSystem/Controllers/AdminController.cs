using NLog;
using SMSystem.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace SMSystem.Controllers
{
    //AdminController for basic admin related methods
    public class AdminController : Controller
    {
        // logger object to log any anomalyies in logFile
        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        //database object
        StudentInformationDBEntities studentInformationDBEntities = new StudentInformationDBEntities();

        // method to register a new admin
        public ActionResult AdminRegistration()
        {
            return View();
        }

        // method to register a new admin and store details in DB
        [HttpPost]
        public ActionResult AdminRegistration(AdminModel newAdministrator)
        {
            if (ModelState.IsValid)
            {
                #region Storing admin details to DB

                Administrator administrator = new Administrator();

                administrator.AdminName = newAdministrator.AdminName;
                administrator.AdminEmail = newAdministrator.AdminEmail;


                administrator.Password = EncryptPassword(newAdministrator.Password);

                studentInformationDBEntities.Administrators.Add(administrator);

                studentInformationDBEntities.SaveChanges();

                #endregion

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

        

        // method for admin to Login and access the portal.
        public ActionResult AdminLogin(LoginModel user)
        {
            Session.Clear();

            if (ModelState.IsValid)
            {
                // variable to store valid admin details
                var admin = studentInformationDBEntities.Administrators.ToList()
                            .Where(a => a.AdminEmail.Equals(user.AdminEmail)).FirstOrDefault();

                #region Authenticating the admin
                if (admin != null && (DecryptPassword(admin.Password) == user.Password))
                {
                    Session["AdminId"] = admin.AdminId.ToString();
                    Session["AdminName"] = admin.AdminName.ToString();

                    if(admin.IsActive == false)
                    {
                        admin.IsActive = true;

                        TempData["adminId"] = admin.AdminId;

                        studentInformationDBEntities.SaveChanges();
                    }
                    #endregion
                    //studentInformationDBEntities.Administrators.AddOrUpdate()

                    return RedirectToAction("WelcomeScreen", "Home");
                }
                else
                {
                    logger.Error("Access Denied to invalid user.");
                    
                    ViewBag.Message = "Invalid user details..! \n Please Try Again..!";
                }
            }
            return View(user);
        }

        // method which will allow theu ser to logout.
        public ActionResult Logout()
        {
            var admin = studentInformationDBEntities.Administrators.ToList()
                            .Where(a => a.AdminId.Equals(TempData["adminId"])).FirstOrDefault();

            #region clearing the session after user logs-out
            if (TempData["adminId"] != null)
            {
                admin.IsActive = false;
                                
                studentInformationDBEntities.SaveChanges(); 
            } 

            Session["AdminId"] = null;
            #endregion
            return RedirectToAction("Dashboard", "Home");
        }

        // jason result used as validation to validate only unique admins
        public JsonResult doesAdminExist(string AdminEmail)
        {
            return Json(!studentInformationDBEntities.Administrators.Any(x => x.AdminEmail == AdminEmail), JsonRequestBehavior.AllowGet);
        }

        // method to encrypt password.
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

        // method to decrypt password
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
