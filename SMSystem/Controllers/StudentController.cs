using SMSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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
            var names = studentInformationDBEntities.Courses.Select(n => new SelectListItem()
            {
                Text = n.CourseName,
                Value = n.CourseId.ToString()
            }).ToList();

            SelectListItem[] courseName = names.ToArray();

            if (names != null)
            {
                ViewBag.cname = courseName.Select(n => new SelectListItem()
                {
                    Text = n.Text,
                    Value = n.Value.ToString()
                }).ToArray(); ; // this will carry data to the view page

            }


            var code = studentInformationDBEntities.Status.Select(n => new SelectListItem()
            {
                Text = n.StatusDescription,
                Value = n.StatusCode.ToString()
            }).ToList();

            SelectListItem[] status = code.ToArray();

            if (code != null)
            {
                ViewBag.sdesc = status.Select(n => new SelectListItem()
                {
                    Text = n.Text,
                    Value = n.Value.ToString()
                }).ToArray(); ; // this will carry data to the view page

                return View();
            }

            return View();
        }

        [HttpPost]
        public ActionResult AddStudent(StudentModel newStudent)
        {
            
            return View();
        }

        public ActionResult StudentList()
        {
            var students = studentInformationDBEntities.Students.ToList();

            return View(students);
        }

        public ActionResult EditStudent()
        {

            return View();
        }

        public ActionResult DeleteStudent()
        {

            return View();
        }

        public ActionResult StudentProfile()
        {

            return View();
        }
    }
}

