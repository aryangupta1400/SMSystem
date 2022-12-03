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
            return View();
        }

        [HttpPost]
        public ActionResult StudentRegistration(StudentModel newStudent)
        {
            return View();
        }

        public ActionResult StudentList()
        {
            var students = studentInformationDBEntities.Students.ToList();

            return View(students);
        }
    }
}

