using Newtonsoft.Json;
using NLog;
using SMSystem.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net;
using System.Net.Mail;
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

        [AllowAnonymous]
        public ActionResult AdminLogin(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }


        // method for admin to Login and access the portal.
        [AllowAnonymous]
        [HttpPost]
        public ActionResult AdminLogin(LoginModel user, string returnUrl)
        {
            Session.Clear();

            bool isCapthcaValid = ValidateCaptcha(Request["g-recaptcha-response"]);

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

        [AllowAnonymous]
        public bool ValidateCaptcha(string response)
        {
            //  Setting _Setting = repositorySetting.GetSetting;



            //secret that was generated in key value pair  
            string secret = ConfigurationManager.AppSettings["GoogleSecretkey"]; //WebConfigurationManager.AppSettings["recaptchaPrivateKey"];



            var client = new WebClient();
            var reply = client.DownloadString(string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", secret, response));



            var captchaResponse = JsonConvert.DeserializeObject<CaptchaResponse>(reply);



            return Convert.ToBoolean(captchaResponse.Success);



        }

        // method to access forget password view
        public ActionResult ForgetPassword()
        {            
            return View();
        }

        // method to verify user and send email
        [HttpPost]
        public ActionResult ForgetPassword(int AdminId, string AdminEmail)
        {
            var admin = studentInformationDBEntities.Administrators.Where(a => a.AdminEmail == AdminEmail).FirstOrDefault();

            #region Authenticating user
            if (admin != null && admin.AdminId == AdminId)
            {
                // function to send email if valid user
                SendEmail(AdminEmail);
                ViewBag.Message1 = "Your password has been sent to your Email-Id.";

                ModelState.Clear();
            }
            else
            {
                ViewBag.Message1 = "Details mismatch.";
                ViewBag.Message2 = "Please enter valid details.";
                ViewBag.Message3 = "Or register to access the portal.";
                return View();
            }
            #endregion

            return View();
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

            #region logic to Encrypt Password
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
            #endregion
            return encryptedPassword.ToString();
        }

        // method to decrypt password
        private string DecryptPassword(string password)
        {
            StringBuilder encryptedPassword = new StringBuilder(password);

            #region logic to Decrypt Password
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
            #endregion
            return encryptedPassword.ToString();
        }

        // method to send email to an admin
        public void SendEmail(string emailId)
        {
            var admin = studentInformationDBEntities.Administrators.Where(a => a.AdminEmail == emailId).FirstOrDefault();
                              
            var fromEmail = new MailAddress("autodidact.project4@gmail.com", "SMS Customer Care");
            var toEmail = new MailAddress(emailId);

            var fromEmailPassword = "yqefeoxzvdwvmsxd";

            string subject = "Password Recovery";
            
            string body = "<br/> Hello " + admin.AdminName +
                            "<br/><br/> We have successfuly processed your request for forget password." +
                            "<br/><br/> Your <br/> Admin-Id : " + admin.AdminId + 
                            "<br/>      Password : " + DecryptPassword(admin.Password) + 
                            "<br/><br/> We were happy to help you..! :)" + 
                            "<br/><br/> Regards" +
                            "<br/> SMS Customer Care";

            var smtpRequest = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword)
            };

            using (var message = new MailMessage(fromEmail, toEmail)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            }) smtpRequest.Send(message);
        }

    }
}
