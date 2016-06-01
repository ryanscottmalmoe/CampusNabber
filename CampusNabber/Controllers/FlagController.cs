using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CampusNabber;
using CampusNabber.Models;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Threading.Tasks;

namespace CampusNabber.Controllers
{
    [Authorize]
    public class FlagController : Controller
    {
        private CampusNabberEntities db = new CampusNabberEntities();
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
        //
        // GET: /Flag/
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Create(Guid postId, string username)
        {
            IEnumerable<FlagPost> previousFlags = db.FlagPosts.Where(flag => flag.username_of_flagger.Equals(User.Identity.Name) && flag.flagged_postitem_id.Equals(postId));
            if (previousFlags.Count() > 0)
            {
                return View("AlreadyFlagged");
            }
            FlagPost newFlag = new FlagPost { username_of_post = username, flagged_postitem_id = postId, flag_date = DateTime.Now, username_of_flagger = User.Identity.Name };
            return View(newFlag);
        }


        [HttpPost]
        public async Task<ActionResult> Create(FlagPost newFlag)
        {
            if (ModelState.IsValid)
            {
                newFlag.flag_date = DateTime.Now;
                newFlag.username_of_flagger = User.Identity.Name;
                newFlag.object_id = Guid.NewGuid();
                db.FlagPosts.Add(newFlag);
                UrlHelper u = new UrlHelper(this.ControllerContext.RequestContext);
                string url = u.Action("Details", "PostItems", new { id = newFlag.flagged_postitem_id });
                try
                {
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                await UserManager.SendEmailAsync("df60a819-7b79-49ee-9b18-7c4f32726115", "Post Flagged as Inappropriate", "<h2>The following post has been flagged as inappropriate.</h2><h4>Please address appropriately.</h4><a href=\"" + url + "\">Link to the Post</a>");
                return View("SuccessfulCreation");
            }
            return View();
        }

        [Authorize(Roles = "Admin")]
        public ActionResult RemoveFlags(PostXFlagViewModel model)
        {
            IEnumerable<FlagPost> flags = db.FlagPosts.Where(flag => flag.flagged_postitem_id == model.PostId);
            db.FlagPosts.RemoveRange(flags);
            db.SaveChanges();
            return View("~/Views/PostXFlagViewModel/Details.cshtml", model);
        }

    }
}