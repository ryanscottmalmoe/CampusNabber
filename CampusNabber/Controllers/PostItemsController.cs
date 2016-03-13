using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CampusNabber.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using DatabaseCode.CNQueryFolder;
using System.Data.Entity.Validation;
using System.Diagnostics;
using CampusNabber.Utility;
using CampusNabber.Helpers.SchoolClasses;

namespace CampusNabber.Controllers
{
    public class PostItemsController : Controller
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

        public PostItemsController(ApplicationUserManager _userManager)
        {
            UserManager = _userManager;
        }

        // GET: PostItems
        public ActionResult Index()
        {
           return View(db.PostItems.ToList());
        }

        public PostItemsController()
        {

        }


        // GET: PostItems/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PostItem postItem = db.PostItems.Find(id);
            if (postItem == null)
            {
                return HttpNotFound();
            }
            //Builds the school class for create page.
            School school = SchoolFactory.BuildSchool(postItem.school_name);
            ViewBag.main_color = school.main_hex_color;
            ViewBag.secondary_color = school.secondary_hex_color;

            return View(postItem);
        }

        // GET: PostItems/Create
        /*
        public ActionResult Create()
        { 
            var user = UserManager.FindByIdAsync(User.Identity.GetUserId());
            return View(user);
        }
        */
        
        public ActionResult Create(String userId)
        {
            PostItem postItem = null;
            //var user = UserManager.FindByName(userId);
            if (userId == null)
                postItem = new PostItem { username = User.Identity.GetUserName() };
            else
                postItem = new PostItem { username = userId };

            ViewBag.username = userId;

            SelectList selectCategory = PostItemService.generateCategoryList();
            ViewBag.selectCategory = selectCategory;

            //Builds the school class for create page.
            ApplicationUser user = UserManager.FindByName(postItem.username);
            School school = SchoolFactory.BuildSchool(user.school_name);
            ViewBag.main_color = school.main_hex_color;
            ViewBag.secondary_color = school.secondary_hex_color;

            return View(postItem);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create(PostItem postItem, HttpPostedFileBase[] images)
        {

            if (ModelState.IsValid)
            {
                //Sets the school_name here
                ApplicationUser user = UserManager.FindByName(postItem.username);
                postItem.school_name = user.school_name;
                postItem.post_date = System.DateTime.Today;
                postItem.object_id = Guid.NewGuid();
                postItem.photo_path_id = "";
                foreach (var image in images) //reset photo_path here if there is a photo
                {
                    //set the path here to url.
                    string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                    path = path + "\\_2" + image.FileName;
                    image.SaveAs(path);
                }
                postItem.createEntity();

                return RedirectToAction("Index");
            }
            return View(postItem);
        }

        // GET: PostItems/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PostItem postItem = db.PostItems.Find(id);
            if (postItem == null)
            {
                return HttpNotFound();
            }
            SelectList selectCategory = PostItemService.generateCategoryList();
            ViewBag.selectCategory = selectCategory;

            //Builds the school class for edit page.
            School school = SchoolFactory.BuildSchool(postItem.school_name);
            ViewBag.main_color = school.main_hex_color;
            ViewBag.secondary_color = school.secondary_hex_color;

            return View(postItem);
        }

        // POST: PostItems/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "object_id,username,school_name,post_date,price,title,description,photo_path,category")] PostItem postItem)
        {
            if (ModelState.IsValid)
            {
                postItem.updateEntity();
                //Instead of taking you back to the index page, the user is now taken back to the Details page of that particular post. - ahenry
                return RedirectToAction("Details", new { id = postItem.object_id });
            }
            return View(postItem);
        }

        // GET: PostItems/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PostItem postItem = db.PostItems.Find(id);
            if (postItem == null)
            {
                return HttpNotFound();
            }
            return View(postItem);
        }

        // POST: PostItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            PostItem postItem = db.PostItems.Find(id);
            db.PostItems.Remove(postItem);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

       
        

    }
}
