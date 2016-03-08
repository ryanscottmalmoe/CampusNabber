using CampusNabber.Models;
using CampusNabber.Utility;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
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

        public ActionResult ProfileView(string failedPost)
        {

            if (_userManager == null)
                _userManager = UserManager;
            ViewBag.userName = User.Identity.GetUserName();

            //Creates profile model and assigns current users posts to the model
            var profile = new ProfileModel();
            profile.getProfilePosts(_userManager.FindById(User.Identity.GetUserId()));
            profile.user = (_userManager.FindById(User.Identity.GetUserId()));

            //Creates school drop down menu
            SelectList selectCategory = PostItemService.generateSchoolsList();
            ViewBag.selectCategory = selectCategory;
            TempData["FailedPost"] = failedPost;
            return View(profile);
        }
        

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UpdateUser(ProfileModel profileModel)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser Model = UserManager.FindById(User.Identity.GetUserId());
                string oldUserName = Model.UserName;
                Model.Email = profileModel.user.Email;
                Model.UserName = profileModel.user.UserName;
                Model.school_name = profileModel.user.school_name;
                IdentityResult result = await UserManager.UpdateAsync(Model);
                if(result.Succeeded)
                {
                    if (!oldUserName.Equals(profileModel.user.UserName))
                    {
                        PostItemService.updateAllPostItemsInfo(Model, oldUserName);
                    }
                    return RedirectToAction("LogOffWithoutPost", "Account");
                }
            } 
            return RedirectToAction("ProfileView", "Profile", new { failedPost = "true" });
        }

    }
}