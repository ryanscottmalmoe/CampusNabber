using CampusNabber.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CampusNabber.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationUserManager _userManager;

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        

        public ActionResult Index()
        {
            using (var context = new CampusNabberEntities())
            {
                if (User.Identity.IsAuthenticated && Session["Color"] == null)
                {
                    ApplicationUser user = UserManager.FindById(User.Identity.GetUserId());
                    School school = context.Schools.Where(d => d.object_id == user.school_id).First();
                    Session["Color"] = school.main_hex_color;

                }
                List<string> schools = context.Schools.Select(d => d.school_name).ToList();
                schools.Sort();
                string[] schoolsArr = schools.ToArray();
                ViewData["schools"] = schoolsArr;


                return View();
            }
        }

        public ActionResult About()
        {
            ViewBag.Message = "Our Mission";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }


    }
}