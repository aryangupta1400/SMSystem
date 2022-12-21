using PagedList;
using SMSystem.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace SMSystem.Controllers
{
    public class CourseController : Controller
    {
        // GET: Course

        StudentInformationDBEntities studentInformationDBEntities = new StudentInformationDBEntities();

        public ActionResult CourseList(int? page)
        {
            if (Session["AdminId"] != null)
            {
                var courses = studentInformationDBEntities.Courses.ToList().ToPagedList(page ?? 1, 3);

                if (TempData["errorMessage"] != null)
                {
                    ViewBag.Message = TempData["errorMessage"];
                }

                return View(courses);

            }

            return RedirectToAction("LoginError", "Home");
            
        }

        public ActionResult AddCourse()
        {
            if (Session["AdminId"] != null)
            {
                return View();
            }

            return RedirectToAction("LoginError", "Home");
            
        }

        [HttpPost]
        public ActionResult AddCourse(Cours newCourse)
        {
            if (Session["AdminId"] != null)
            {
                if (ModelState.IsValid)
                {
                    Cours cours = new Cours();

                    cours.CourseId = newCourse.CourseId;
                    cours.CourseName = newCourse.CourseName;
                    cours.Duration = newCourse.Duration;
                    cours.Description = newCourse.Description;
                    cours.IsValid = true;

                    //var temp = studentInformationDBEntities.Courses.Add(cours);

                    studentInformationDBEntities.Courses.Add(cours);
                    studentInformationDBEntities.SaveChanges();

                    ModelState.Clear();

                    return RedirectToAction("CourseList");
                }

                return View();
            }

            return RedirectToAction("LoginError", "Home");
            
        }

        public ActionResult DeleteCourse(int id)
        {
            if (Session["AdminId"] != null)
            {
                try
                {
                    Cours course = studentInformationDBEntities.Courses.Find(id);

                    var student = studentInformationDBEntities.Students.Where(s => s.CourseId == course.CourseId).ToList();

                    if (student.Count == 0)
                    {
                        course.IsValid = false;

                        studentInformationDBEntities.SaveChanges();
                    }
                    else
                    {
                        TempData["errorMessage"] = "Students are already enrolled in this course.";
                    }

                    return RedirectToAction("CourseList");

                    //ViewBag.Message = "Students are already enrolled in the course.";

                }
                catch (Exception ex)
                {
                    //ViewBag.Message = "Students are already enrolled in the course.";

                    TempData["errorMessage"] = "Students are already enrolled in this course.";

                    return RedirectToAction("CourseList");
                }

                return RedirectToAction("CourseList");
            }

            return RedirectToAction("LoginError", "Home");            
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
            if (Session["AdminId"] != null)
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

            return RedirectToAction("LoginError", "Home");            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditCourse(Cours course)
        {
            if (Session["AdminId"] != null)
            {
                if (ModelState.IsValid)
                {
                    studentInformationDBEntities.Courses.AddOrUpdate(course);

                    studentInformationDBEntities.SaveChanges();

                    return RedirectToAction("CourseList");
                }
                return View();
            }

            return RedirectToAction("LoginError", "Home");            
        }
    }
}