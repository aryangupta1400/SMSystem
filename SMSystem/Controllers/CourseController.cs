using SMSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SMSystem.Controllers
{
    public class CourseController : Controller
    {
        // GET: Course

        StudentInformationDBEntities studentInformationDBEntities = new StudentInformationDBEntities();

        public ActionResult CourseList()
        {
            var courses = studentInformationDBEntities.Courses.ToList();

            return View(courses);
        }
    }
}