using SMSystem.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net;
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

            if (TempData["errorMessage"] != null)
            {
                ViewBag.Message = TempData["errorMessage"];
            }

            return View(courses);
        }

        public ActionResult AddCourse()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddCourse(Cours newCourse)
        {
            if (ModelState.IsValid)
            {
                studentInformationDBEntities.Courses.Add(newCourse);
                studentInformationDBEntities.SaveChanges();

                ModelState.Clear();

                return RedirectToAction("CourseList");
            }

            return View();
        }

        public ActionResult DeleteCourse(int id)
        {           
            try
            {
                Cours course = studentInformationDBEntities.Courses.Find(id);

                studentInformationDBEntities.Courses.Remove(course);

                studentInformationDBEntities.SaveChanges();

                //ViewBag.Message = "Students are already enrolled in the course.";

            }
            catch (Exception ex)
            {
                //ViewBag.Message = "Students are already enrolled in the course.";

                TempData["errorMessage"] = "Students are already enrolled in the course.";

                return RedirectToAction("CourseList");
            }

            return RedirectToAction("CourseList");
        }

        /*public ActionResult DeleteCourse(Cours course)
        {
            try
            {
                studentInformationDBEntities.Courses.Remove(course);
                studentInformationDBEntities.SaveChanges();
            }
            catch
            {
                ViewBag.Message = "Students are already enrolled in the course.";
            }
            
            return View();
        }*/

        public ActionResult EditCourse(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Cours course = studentInformationDBEntities.Courses.Find(id);

            if (course == null)
            {
                return HttpNotFound();
            }

            return View(course);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditCourse(Cours course)
        {
            if (ModelState.IsValid)
            {
                studentInformationDBEntities.Courses.AddOrUpdate(course);

                studentInformationDBEntities.SaveChanges();

                return RedirectToAction("CourseList");
            }

            return View();
        }

    }
}