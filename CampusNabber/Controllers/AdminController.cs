using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using CampusNabber.Models;

namespace CampusNabber.Controllers
{
    [Authorize(Roles ="Admin")]
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
        public ActionResult AdminTools()
        {
            return View();
        }

        public ActionResult BlockUser()
        {
            return View();
        }
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

        public ActionResult LockoutUser(string userEmail)
        {
            ApplicationUser user = UserManager.FindByEmail(userEmail);
            if(user != null)
            {
                ViewBag.Username = user.UserName;
                UserManager.SetLockoutEnabled(user.Id, true);
                UserManager.SetLockoutEndDate(user.Id, DateTime.MaxValue.ToUniversalTime());
                UserManager.UpdateSecurityStamp(user.Id);
                return View("LockoutSuccess");
            }
            else
            {
                return View("UserNotFound");
            }
        }

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

        public ActionResult UnlockUser(string userEmail)
        {
            ApplicationUser user = UserManager.FindByEmail(userEmail);
            if (user != null)
            {
                UserManager.SetLockoutEnabled(user.Id, true);
                UserManager.SetLockoutEndDate(user.Id, DateTime.UtcNow.Subtract(new TimeSpan(1, 0, 0, 0)));
                ViewBag.Username = user.UserName;
                return View("UnlockSuccess");
            }
            else
            {
                return View("UserNotFound");
            }
        }




        public ActionResult ManageSchools()
        {
            var schools = db.Schools.AsEnumerable();
            List<SchoolModel> schoolModels = new List<SchoolModel>();
            foreach(School s in schools)
            {
                schoolModels.Add(SchoolModel.bindToSchoolModel(s));
            }
            return View(schoolModels.AsEnumerable());
        }

        public ActionResult ManageAdmins()
        {
            return View();
        }
	}
}