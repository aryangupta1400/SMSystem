using NLog;
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
    // controller to declare asic course related methods
    public class CourseController : Controller
    {
        // logger object to log any anomalyies in logFile
        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        //database object
        StudentInformationDBEntities studentInformationDBEntities = new StudentInformationDBEntities();

        // method to access and display course list to active admin
        public ActionResult CourseList(int? page)
        {
            if (Session["AdminId"] != null)
            {
                // storing the values from DB to a paged list
                var courses = studentInformationDBEntities.Courses.OrderByDescending(c => c.CourseId).ToList().ToPagedList(page ?? 1, 3);

                if (TempData["errorMessage"] != null)
                {
                    ViewBag.Message = TempData["errorMessage"];
                }

                return View(courses);

            }

            logger.Error("Login Error --> Trying to access functional page without Login.");

            return RedirectToAction("LoginError", "Home");            
        }

        // method to access the view add a new course
        public ActionResult AddCourse()
        {
            if (Session["AdminId"] != null)
            {
                return View();
            }

            logger.Error("Login Error --> Trying to access functional page without Login.");

            return RedirectToAction("LoginError", "Home");
            
        }

        // method for storeing new course details to DB
        [HttpPost]
        public ActionResult AddCourse(Cours newCourse)
        {
            if (Session["AdminId"] != null)
            {
                if (ModelState.IsValid)
                {
                    #region stroing course details to DB
                    Cours cours = new Cours();

                    cours.CourseId = newCourse.CourseId;
                    cours.CourseName = newCourse.CourseName;
                    cours.Duration = newCourse.Duration;
                    cours.Description = newCourse.Description;
                    cours.IsValid = true;

                    //var temp = studentInformationDBEntities.Courses.Add(cours);

                    studentInformationDBEntities.Courses.Add(cours);
                    studentInformationDBEntities.SaveChanges();

                    #endregion

                    ModelState.Clear();

                    return RedirectToAction("CourseList");
                }

                return View();
            }

            logger.Error("Login Error --> Trying to access functional page without Login.");

            return RedirectToAction("LoginError", "Home");
            
        }

        // method to delete a course
        public ActionResult DeleteCourse(int id)
        {
            if (Session["AdminId"] != null)
            {
                try
                {
                    #region soft-deleting a course (changing its status to inactive)
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
                    #endregion
                    return RedirectToAction("CourseList");

                    //ViewBag.Message = "Students are already enrolled in the course.";

                }
                catch (Exception ex)
                {
                    //ViewBag.Message = "Students are already enrolled in the course.";

                    logger.Error(ex, "Course with students enrolled in it attempted to be deleted. FAILED!");

                    TempData["errorMessage"] = "Students are already enrolled in this course.";

                    return RedirectToAction("CourseList");
                }
            }

            logger.Error("Login Error --> Trying to access functional page without Login.");

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

        // method to edit a course details
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

            logger.Error("Login Error --> Trying to access functional page without Login.");

            return RedirectToAction("LoginError", "Home");            
        }

        // updating the edited values to DB
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditCourse(Cours course)
        {
            if (Session["AdminId"] != null)
            {
                #region accessing DB and updating values
                if (ModelState.IsValid)
                {
                    studentInformationDBEntities.Courses.AddOrUpdate(course);

                    studentInformationDBEntities.SaveChanges();

                    return RedirectToAction("CourseList");
                }
                #endregion
                return View();
            }

            logger.Error("Login Error --> Trying to access functional page without Login.");

            return RedirectToAction("LoginError", "Home");            
        }
    }
}