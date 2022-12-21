using NLog;
using PagedList;
using PagedList.Mvc;
using SMSystem.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace SMSystem.Controllers
{
    public class StudentController : Controller
    {
        // GET: Student

        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        StudentInformationDBEntities studentInformationDBEntities = new StudentInformationDBEntities();

        public ActionResult StudentRegistration()
        {
            if (Session["AdminId"] != null)
            {
                SelectListItem[] courseName = GetCourseList();

                SelectListItem[] status = GetStatusList();

                return View();
                                
            }

            logger.Error("Login Error --> Trying to access functional page without Login.");

            return RedirectToAction("LoginError", "Home");

        }

        [HttpPost]
        public async Task<ActionResult> StudentRegistration(Student newStudent)
        {

            if (Session["AdminId"] != null)
            {
                SelectListItem[] courseName = GetCourseList();

                SelectListItem[] status = GetStatusList();

                

                if (ModelState.IsValid)
                {
                    Student student = new Student();

                    /*student.StudentName = newStudent.StudentName;
                    student.StudentEmail = newStudent.StudentEmail;
                    student.StudentMobile = newStudent.StudentMobile;
                    student.Gender = newStudent.Gender;
                    student.StudentDoB = newStudent.StudentDoB;
                    student.Address = newStudent.Address;
                    student.JoiningDate = newStudent.JoiningDate;
                    student.CourseId = newStudent.CourseId;
                    student.StatusCode = newStudent.StatusCode;*/

                    student.StudentAge = CalculateAge((DateTime)newStudent.StudentDoB);

                    /*if (student.StudentAge >= 16)
                    {
                        studentInformationDBEntities.Students.Add(newStudent);

                        await studentInformationDBEntities.SaveChangesAsync();

                        ModelState.Clear();
                    }
                    else
                    {
                        ViewBag.ageError = "Age of the student must be atleast 16.";

                        return View();
                    }*/

                    studentInformationDBEntities.Students.Add(newStudent);

                    await studentInformationDBEntities.SaveChangesAsync();

                    ModelState.Clear();

                    return RedirectToAction("StudentList");
                }

                return View();
            }

            logger.Error("Login Error --> Trying to access functional page without Login.");

            return RedirectToAction("LoginError", "Home");

        }

        public ActionResult StudentList(int? page)
        {
            if (Session["AdminId"] != null)
            {
                var students = studentInformationDBEntities.Students.OrderByDescending(l => l.StudentId).ToList().ToPagedList(page??1, 3);
                                
                return View(students);
            }

            logger.Error("Login Error --> Trying to access functional page without Login.");

            return RedirectToAction("LoginError", "Home");
        }
                
        public int CalculateAge(DateTime dob)
        {
            int age = 0;
            TimeSpan span = DateTime.Now - dob;
            var diff = span.TotalDays;
            age = (int) diff/365;

            return age;
        }

        public JsonResult CheckAge(DateTime StudentDoB)
        {
            int age = CalculateAge(StudentDoB);
            return Json(age >= 16 ? true : false, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditStudent(int? id)
        {
            if (Session["AdminId"] != null)
            {
                SelectListItem[] courseName = GetCourseList();

                SelectListItem[] status = GetStatusList();

                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                Student student = studentInformationDBEntities.Students.Find(id);

                if (student == null)
                {
                    return HttpNotFound();
                }

                return View(student);

                /*if (Session["StudentId"] != null)
                {

                }*/

                //return View();

            }

            logger.Error("Login Error --> Trying to access functional page without Login.");

            return RedirectToAction("LoginError", "Home");

            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditStudent([Bind(Include = "StudentId, StudentName, StudentEmail, StudentMobile, Gender, StudentDoB, StudentAge, Address, CourseId, JoiningDate, StatusCode, ParentId")] Student student)
        {
            if (Session["AdminId"] != null)
            {
                if (ModelState.IsValid)
                { 
                    student.StudentAge = CalculateAge((DateTime)student.StudentDoB);

                    studentInformationDBEntities.Entry(student).State = EntityState.Modified;

                    studentInformationDBEntities.SaveChanges();

                    return RedirectToAction("StudentList");
                }

                return View();
            }

            logger.Error("Login Error --> Trying to access functional page without Login.");

            return RedirectToAction("LoginError", "Home");                                    
        }

        public ActionResult StudentSearch(string searchBy, string search, int? page)
        {
            if (Session["AdminId"] != null)
            {
                if (search == "" || search == null)
                {
                    return RedirectToAction("StudentList");
                }

                if (searchBy == "StudentId" && search != null)
                {
                    try
                    {
                        int? id = Convert.ToInt32(search);
                        var model = studentInformationDBEntities.Students.Where(s => s.StudentId == id || id == null).ToList().ToPagedList(page ?? 1, 3); ;
                        return View(model);
                    }
                    catch (Exception ex)
                    {
                        ViewBag.Message = "Please enter numeric values only..!";
                        var model = studentInformationDBEntities.Students.Where(s => s.StudentId == 0).ToList().ToPagedList(page ?? 1, 3); ;

                        logger.Error(ex, "Alphabets entered insed of numeric value.");

                        return View(model);
                    }
                }
                else if (searchBy == "StudentName" && search != null)
                {

                    try
                    {
                        var model = studentInformationDBEntities.Students.Where(s => s.StudentName.StartsWith(search) || search == null).ToList().ToPagedList(page ?? 1, 3); ;
                        return View(model);
                    }
                    catch (Exception ex)
                    {
                        ViewBag.Message = "Please enter alphabets only..!";
                        var model = studentInformationDBEntities.Students.Where(s => s.StudentId == 0).ToList().ToPagedList(page ?? 1, 3); ;

                        logger.Error(ex, "Numeric value entered insed of alphabets.");

                        return View(model);
                    }

                }
                else if (searchBy == "StudentEmail" && search != null)
                {
                    try
                    {
                        var model = studentInformationDBEntities.Students.Where(s => s.StudentEmail == search.ToLower() || search.ToLower() == null).ToList().ToPagedList(page ?? 1, 3); ;
                        return View(model);
                    }
                    catch (Exception ex)
                    {
                        ViewBag.Message = "Please enter alphabets only..!";
                        var model = studentInformationDBEntities.Students.Where(s => s.StudentId == 0).ToList().ToPagedList(page ?? 1, 3); ;

                        logger.Error(ex, "Numeric value entered insed of alphabets.");

                        return View(model);
                    }

                }
                else if (searchBy == "Gender" && search != null)
                {
                    try
                    {
                        var model = studentInformationDBEntities.Students.Where(s => s.Gender.ToLower() == search.ToLower() || search == null).ToList().ToPagedList(page ?? 1, 3); ;
                        return View(model);
                    }
                    catch (Exception ex)
                    {
                        ViewBag.Message = "Please enter alphabets only..!";
                        var model = studentInformationDBEntities.Students.Where(s => s.StudentId == 0).ToList().ToPagedList(page ?? 1, 3); ;

                        logger.Error(ex, "Numeric value entered insed of alphabets.");

                        return View(model);
                    }

                }
                else if (searchBy == "CourseId" && search != null)
                {
                    try
                    {
                        int? id = GetCourseId(search);
                        if (id == 0)
                        {
                            ViewBag.Message = "Please enter a valid course name..!";
                        }
                        var model = studentInformationDBEntities.Students.Where(s => s.CourseId == id || id == null).ToList().ToPagedList(page ?? 1, 3); ;
                        return View(model);
                    }
                    catch (Exception ex)
                    {
                        ViewBag.Message = "Please enter alphabets only..!";
                        var model = studentInformationDBEntities.Students.Where(s => s.StudentId == 0).ToList().ToPagedList(page ?? 1, 3); ;

                        logger.Error(ex, "Invalid course name entered.");

                        return View(model);
                    }

                }
                else if (searchBy == "StatusCode" && search != null)
                {
                    try
                    {
                        int? id = GetStatusCode(search);
                        if (id == 0)
                        {
                            ViewBag.Message = "Please enter a valid status..!";
                        }
                        var model = studentInformationDBEntities.Students.Where(s => s.StatusCode == id || id == null).ToList().ToPagedList(page ?? 1, 3); ;
                        return View(model);
                    }
                    catch (Exception ex)
                    {
                        ViewBag.Message = "Please enter alphabets only..!";
                        var model = studentInformationDBEntities.Students.Where(s => s.StudentId == 0).ToList().ToPagedList(page ?? 1, 3); ;

                        logger.Error(ex, "Invalid status entered.");

                        return View(model);
                    }

                }

                return RedirectToAction("StudentList");
            }

            logger.Error("Login Error --> Trying to access functional page without Login.");

            return RedirectToAction("LoginError", "Home");
            
        }

        public ActionResult StudentProfile(int? id)
        {
            if (Session["AdminId"] != null)
            {
                Student student = studentInformationDBEntities.Students.Find(id);

                return View(student);
            }

            logger.Error("Login Error --> Trying to access functional page without Login.");

            return RedirectToAction("LoginError", "Home");
            
        }

        protected int GetCourseId(string course)
        {
            int courseId = 0;

            var courses = studentInformationDBEntities.Courses.ToList().Where(x => x.CourseName == course).FirstOrDefault();
            if (courses != null)
            {
                courseId = courses.CourseId;
            }
            else
            {
                courseId = 0;
            }

            return courseId;
        }

        protected int GetStatusCode(string status)
        {
            int statusCode = 0;

            var statuses = studentInformationDBEntities.Status.ToList().Where(x => x.StatusDescription == status).FirstOrDefault();
            if (statuses != null)
            {
                statusCode = statuses.StatusCode;
            }
            else
            {
                statusCode = 0;
            }

            return statusCode;
        }

        /*protected int GetStatusCode()
        {
            int a = 0;

            return a;
        }*/

        protected SelectListItem[] GetCourseList()
        {
            var names = studentInformationDBEntities.Courses.Where(c => c.IsValid == true).Select(n => new SelectListItem()
            {
                Text = n.CourseName,
                Value = n.CourseId.ToString()
            }).ToList();

            SelectListItem[] courseName = names.ToArray();


            return ViewBag.cname = courseName.Select(n => new SelectListItem()
            {
                Text = n.Text,
                Value = n.Value.ToString()
            }).ToArray(); ; // this will carry data to the view page

        }

        protected SelectListItem[] GetStatusList()
        {
            var code = studentInformationDBEntities.Status.Select(n => new SelectListItem()
            {
                Text = n.StatusDescription,
                Value = n.StatusCode.ToString()
            }).ToList();

            SelectListItem[] status = code.ToArray();

            return ViewBag.sdesc = status.Select(n => new SelectListItem()
            {
                Text = n.Text,
                Value = n.Value.ToString()
            }).ToArray(); ; // this will carry data to the view page
        }
    }

    public class AgeValidator : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                StudentController studentController = new StudentController();
                int age = studentController.CalculateAge((DateTime)value);

                if (age >= 16)
                {
                    return ValidationResult.Success;
                }
                else
                {
                    return new ValidationResult("Age of the student must be atleast 16.");
                }
            }
            else
            {
                return new ValidationResult("" + validationContext.DisplayName + " is required");
            }
        }
    }
}

