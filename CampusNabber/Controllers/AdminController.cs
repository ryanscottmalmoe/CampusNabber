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
        public ActionResult FindUser(string userEmail)
        {
            ApplicationUser result = UserManager.FindByEmail(userEmail);
            return View(result);
        }
	}
}