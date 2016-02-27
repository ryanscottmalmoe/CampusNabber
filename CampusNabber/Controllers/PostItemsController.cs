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
using CampusNabber.Utility;

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
        
        public ActionResult Create()
        {
            
               var postItem = new PostItem { username = User.Identity.GetUserName() };
          

            return View(postItem);
        }

        // POST: PostItems/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
     //   [ValidateAntiForgeryToken]
     // Christian Change
        //So that the system supplies the school name based off the current user & find a way to have the model do the data binding
        public ActionResult Create([Bind(Include = "object_id,username,school_name,post_date,price,title,description,photo_path,category")] PostItem postItem)
        {
            if (ModelState.IsValid)
            {
                var service = new PostItemService();
                service.setMissingFields(postItem, UserManager);                
                postItem.createEntity();                
                return RedirectToAction("MainMarketView");
            }
            //I added this
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
                db.Entry(postItem).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
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
