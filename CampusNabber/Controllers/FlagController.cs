using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CampusNabber;
using CampusNabber.Models;

namespace CampusNabber.Controllers
{
    public class FlagController : Controller
    {
        private CampusNabberEntities db = new CampusNabberEntities();
        //
        // GET: /Flag/
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Create(Guid postId, string username)
        {
            IEnumerable<FlagPost> previousFlags = db.FlagPosts.Where(flag => flag.username_of_flagger.Equals(User.Identity.Name) && flag.flagged_postitem_id.Equals(postId));
            if(previousFlags.Count() > 0)
            {
                return View("AlreadyFlagged");
            }
            FlagPost newFlag = new FlagPost { username_of_post = username, flagged_postitem_id = postId, flag_date = DateTime.Now, username_of_flagger = User.Identity.Name };
            return View(newFlag);
        }

        [HttpPost]
        public ActionResult Create(FlagPost newFlag)
        {
            if(ModelState.IsValid)
            {
                newFlag.flag_date = DateTime.Now;
                newFlag.username_of_flagger = User.Identity.Name;
                newFlag.object_id = Guid.NewGuid();
                db.FlagPosts.Add(newFlag);
                try
                {
                    db.SaveChanges();
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                //probably insert "flag created" view
                return View("SuccessfulCreation");
            }
            return View();
        }
        [Authorize(Roles ="Admin")]
        public ActionResult RemoveFlags(PostXFlagViewModel model)
        {
            IEnumerable<FlagPost> flags = db.FlagPosts.Where(flag => flag.flagged_postitem_id == model.PostId);
            db.FlagPosts.RemoveRange(flags);
            db.SaveChanges();
            return View("~/Views/PostXFlagViewModel/Details.cshtml", model);
        }
	}
}