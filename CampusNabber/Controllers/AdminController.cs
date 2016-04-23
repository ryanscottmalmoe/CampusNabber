using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using CampusNabber.Models;

namespace CampusNabber.Controllers
{
    public class AdminController : Controller
    {
        ApplicationUserManager _userManager;
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
        private CampusNabberEntities db = new CampusNabberEntities();
        //
        // GET: /Admin/
        [Authorize(Roles ="Admin")]
        public ActionResult AdminTools()
        {
            return View();
        }

        [Authorize(Roles ="Admin")]
        public ActionResult BlockUser()
        {
            return View();
        }

        [Authorize(Roles ="Admin")]
        [HttpPost]
        public ActionResult FindUserToBlock(string userEmail)
        {
            ApplicationUser result = UserManager.FindByEmail(userEmail);
            if(result != null)
            {
                ProfileModel user = new ProfileModel();
                user.getProfilePosts(result);
                user.user = result;
                School school = db.Schools.Where(d => d.object_id == result.school_id).First();
                user.school_name = school.school_name;
                return View(user);
            }
            else
            {
                return View("UserNotFound");
            }
        }
        [Authorize(Roles="Admin")]
        public ActionResult LockoutUser(ProfileModel user)
        {
            if(user != null)
            {
                user.user.LockoutEnabled = true;
                user.user.LockoutEndDateUtc = System.DateTime.MaxValue;
                ViewBag.Username = user.user.UserName;
                return View("LockoutSuccess");
            }
            else
            {
                return View("UserNotFound");
            }
        }

        [Authorize(Roles="Admin")]
        public ActionResult UnblockUser()
        {
            return View();
        }

        [Authorize(Roles="Admin")]
        public ActionResult FindUserToUnblock(string userEmail)
        {
            ApplicationUser result = UserManager.FindByEmail(userEmail);
            if (result != null)
            {
                ProfileModel user = new ProfileModel();
                user.getProfilePosts(result);
                user.user = result;
                School school = db.Schools.Where(d => d.object_id == result.school_id).First();
                user.school_name = school.school_name;
                return View(user);
            }
            else
            {
                return View("UserNotFound");
            }
        }

        [Authorize(Roles="Admin")]
        public ActionResult UnlockUser(ProfileModel user)
        {
            if (user != null)
            {
                user.user.LockoutEndDateUtc = null;
                ViewBag.Username = user.user.UserName;
                return View("UnlockSuccess");
            }
            else
            {
                return View("UserNotFound");
            }
        }
	}
}