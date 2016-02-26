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
    public class ProfileController : Controller
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

        // GET: Profile
        public ActionResult Index()
        {
            return View();
        }

        public ProfileController()
        {

        }

        public ProfileController(ApplicationUserManager manager)
        {
            UserManager = manager;
        }

        public ActionResult ProfileView()
        {

            if (_userManager == null)
                _userManager = UserManager;
            ViewBag.userName = User.Identity.GetUserName();
      
            var profile = new ProfileModel();
            profile.getProfilePosts(_userManager.FindById(User.Identity.GetUserId()));
            profile.user = (_userManager.FindById(User.Identity.GetUserId()));
            return View(profile);
        }

    }
}