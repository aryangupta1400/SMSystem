using SMSystem.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SMSystem.Controllers
{
    public class StudentController : Controller
    {
        // GET: Student

        StudentInformationDBEntities studentInformationDBEntities = new StudentInformationDBEntities();

        public ActionResult StudentRegistration()
        {
            if (Session["AdminId"] != null)
            {
                SelectListItem[] courseName = GetCourseList();

                SelectListItem[] status = GetStatusList();
            }
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> StudentRegistration(Student newStudent)
        {

            SelectListItem[] courseName = GetCourseList();

            SelectListItem[] status = GetStatusList();
            //Student student = new Student();

            if (ModelState.IsValid) 
            {
                /*student.StudentName = newStudent.StudentName;
                student.StudentEmail = newStudent.StudentEmail;
                student.StudentMobile = newStudent.StudentMobile;
                student.Gender = newStudent.Gender;
                student.StudentDoB = newStudent.StudentDoB;
                student.Address = newStudent.Address;
                student.JoiningDate = newStudent.JoiningDate;
                student.CourseId = newStudent.CourseId;
                student.StatusCode = newStudent.StatusCode;*/

                studentInformationDBEntities.Students.Add(newStudent);

                await studentInformationDBEntities.SaveChangesAsync();

                ModelState.Clear();

                return RedirectToAction("StudentList");
            }

            return View();
        }

        public ActionResult StudentList()
        {
            if (Session["AdminId"] != null)
            {
                var students = studentInformationDBEntities.Students.ToList();

                return View(students);
            }

            return RedirectToAction("Dashboard", "Home");
        }

        public ActionResult EditStudent(int? id)
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditStudent([Bind(Include = "StudentId, StudentName, StudentEmail, StudentMobile, Gender, StudentDoB, StudentAge, Address, CourseId, JoiningDate, StatusCode, ParentId")] Student student)
        {
            if (ModelState.IsValid)
            {
                studentInformationDBEntities.Entry(student).State = EntityState.Modified;

                studentInformationDBEntities.SaveChanges();

                return RedirectToAction("StudentList");
            }

            return View();
        }

        public ActionResult StudentSearch(string searchBy, string search)
        {
            if(search == "" || search == null)
            {
                return RedirectToAction("StudentList");
            }
            
            if (searchBy == "StudentId" && search != null)
            {
                try
                {
                    int? id = Convert.ToInt32(search);
                    var model = studentInformationDBEntities.Students.Where(s => s.StudentId == id || id == null).ToList();
                    return View(model);
                }
                catch(Exception ex)
                {
                    ViewBag.Message = "Please enter numeric values only..!";
                    var model = studentInformationDBEntities.Students.Where(s => s.StudentId == 0 ).ToList();
                    return View(model);
                }                
            }
            else if (searchBy == "StudentName" && search != null)
            {
                
                try
                {
                    var model = studentInformationDBEntities.Students.Where(s => s.StudentName.Contains(search) || search == null).ToList();
                    return View(model);
                }
                catch (Exception ex)
                {
                    ViewBag.Message = "Please enter alphabets only..!";
                    var model = studentInformationDBEntities.Students.Where(s => s.StudentId == 0).ToList();
                    return View(model);
                }
                
            }
            else if (searchBy == "StudentEmail" && search != null)
            {
                try
                {
                    var model = studentInformationDBEntities.Students.Where(s => s.StudentEmail == search.ToLower() || search.ToLower() == null).ToList();
                    return View(model);
                }
                catch (Exception ex)
                {
                    ViewBag.Message = "Please enter alphabets only..!";
                    var model = studentInformationDBEntities.Students.Where(s => s.StudentId == 0).ToList();
                    return View(model);
                }
                
            }
            else if (searchBy == "Gender" && search != null)
            {
                try
                {
                    var model = studentInformationDBEntities.Students.Where(s => s.Gender.ToLower() == search.ToLower() || search == null).ToList();
                    return View(model);
                }
                catch (Exception ex)
                {
                    ViewBag.Message = "Please enter alphabets only..!";
                    var model = studentInformationDBEntities.Students.Where(s => s.StudentId == 0).ToList();
                    return View(model);
                }
                
            }
            else if (searchBy == "CourseId" && search != null)
            {
                try
                {
                    int? id = GetCourseId(search);
                    if(id == 0)
                    {
                        ViewBag.Message = "Please enter a valid course name..!";
                    }
                    var model = studentInformationDBEntities.Students.Where(s => s.CourseId == id || id == null).ToList();
                    return View(model);
                }
                catch (Exception ex)
                {
                    ViewBag.Message = "Please enter alphabets only..!";
                    var model = studentInformationDBEntities.Students.Where(s => s.StudentId == 0).ToList();
                    return View(model);
                }
                
            }
            else if (searchBy == "StatusCode" && search != null)
            {
                try
                {
                    int? id = GetStatusCode(search);
                    if(id == 0)
                    {
                        ViewBag.Message = "Please enter a valid status..!";
                    }
                    var model = studentInformationDBEntities.Students.Where(s => s.StatusCode == id || id == null).ToList();
                    return View(model);
                }
                catch (Exception ex)
                {
                    ViewBag.Message = "Please enter alphabets only..!";
                    var model = studentInformationDBEntities.Students.Where(s => s.StudentId == 0).ToList();
                    return View(model);
                }
                
            }
            
            return RedirectToAction("StudentList");
        }

        public ActionResult StudentProfile(int? id)
        {
            Student student = studentInformationDBEntities.Students.Find(id);

            return View(student);
        }

        protected int GetCourseId(string course)
        {
            int courseId = 0;                       

            var courses = studentInformationDBEntities.Courses.ToList().Where(x => x.CourseName == course).FirstOrDefault();
            if(courses != null)
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

        protected SelectListItem[] GetCourseList()
        {
            var names = studentInformationDBEntities.Courses.Select(n => new SelectListItem()
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
}

