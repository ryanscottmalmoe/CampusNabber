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
using System.Data.Entity.Validation;
using System.Diagnostics;
using CampusNabber.Utility;
using CampusNabber.Helpers.SchoolClasses;
using DatabaseCode.FactoryFiles;
using System.Linq.Dynamic;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using System.Configuration;
using System.IO;
using System.Collections;
using Newtonsoft.Json;

namespace CampusNabber.Controllers
{
    [Authorize]
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


        // GET: PostItems/Details
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PostItem postItemTemp = db.PostItems.Find(id);
            PostItemModel postItem = PostItemModel.bindToModel(postItemTemp);
            if (postItem == null)
            {
                return HttpNotFound();
            }


            //Builds the school class for create page.
            School school = db.Schools.Where(d => d.school_name == postItem.school_name).First();
            ViewBag.main_color = school.main_hex_color;
            ViewBag.secondary_color = school.secondary_hex_color;
            ApplicationUser user = UserManager.FindById(User.Identity.GetUserId());
            if(postItem.photo_path_id.HasValue)
            {
                PostItemPhotos postItemPhotos = db.PostItemPhotos.Where(d => d.object_id == postItem.photo_path_id).First();
                //*******AWS Portion *********************
                List<string> photoList = PostItemService.GetS3Photos(postItem);
                if (photoList != null && photoList.Count > 0)
                {
                    ViewBag.RESULTS = photoList;
                    ViewBag.FIRSTPHOTO = photoList[0];
                    ViewBag.HASPHOTO = true;
                }
                //******************************************
            }
            else
            {
                ViewBag.HASPHOTO = false;
            }
           

            ViewBag.EMAIL = UserManager.FindByName(postItem.username).Email;
            return View(postItem);
        }

        public JsonResult GetSubCategory(string category)
        {
            String[] categories = db.Categories.Select(d => d.category_name).Distinct().ToArray();
            SelectList categoriesDropdown = null;
            switch (category)
            {
                case "Eats and Drinks":
                    categoriesDropdown = PostItemService.generateSubCategoryList("Eats and Drinks");
                    break;
                case "For Sale":
                    categoriesDropdown = PostItemService.generateSubCategoryList("For Sale");
                    break;
                case "Housing":
                    categoriesDropdown = PostItemService.generateSubCategoryList("Housing");
                    break;
                case "Jobs":
                    categoriesDropdown = PostItemService.generateSubCategoryList("Jobs");
                    break;
                case "On Campus":
                    categoriesDropdown = PostItemService.generateSubCategoryList("On Campus");
                    break;
            }

            return Json(new SelectList(categoriesDropdown, "Value", "Text"));
        }


        // Get: /PostItems/Create
        public ActionResult Create(String userId)
        {
            PostItemModel postItem = null;
            if (userId == null)
                postItem = new PostItemModel { username = User.Identity.GetUserName() };
            else
                postItem = new PostItemModel { username = userId };

            ViewBag.username = userId;

            SelectList selectCategory = PostItemService.generateCategoryList();
            ViewBag.selectCategory = selectCategory;

            //Builds the school class for create page.
            ApplicationUser user = UserManager.FindByName(postItem.username);
            School school = db.Schools.Where(d => d.object_id == user.school_id).First();
            postItem.school_name = school.school_name;
            ViewBag.main_color = school.main_hex_color;
            ViewBag.secondary_color = school.secondary_hex_color;
            String[] categories = db.Categories.Select(d => d.category_name).Distinct().ToArray();
            ViewBag.categories = categories;

            return View(postItem);
        }

        //Post /PostItems/Create
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create([Bind(Include = 
        "object_id,username,school_name,post_date,price,title,description,photo_path_id,category,subCategory,social_flag")]
        PostItemModel postItemModel, HttpPostedFileBase[] images)
        {
            PostItem postItem = null;
            if (ModelState.IsValid)
            {
                School school = db.Schools.Where(d => d.school_name == postItemModel.school_name).First();


                //Sets the school_name here
                ApplicationUser user = UserManager.FindByName(postItemModel.username);
                postItemModel.school_name = school.school_name;
                postItemModel.post_date = System.DateTime.Now;
                postItemModel.object_id = Guid.NewGuid();
                if (images.Count() > 0)
                    postItemModel.photo_path_id = Guid.NewGuid();
                else
                    postItemModel.photo_path_id = Guid.Empty;
                postItem = postItemModel.bindToPostItem();

                //****AWS Portion**************
                if(images.Count() > 0)
                    PostItemService.StoreS3Photos(images, postItem);
                //******************************

                db.PostItems.Add(postItem);
                db.SaveChanges();

                return RedirectToAction("MainMarketView", "MarketPlace");
            }
            return View(postItem);
        }


        // GET: PostItems/Edit
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PostItem postItemTemp = db.PostItems.Find(id);
            PostItemModel postItem = PostItemModel.bindToModel(postItemTemp);

            if (postItem == null)
            {
                return HttpNotFound();
            }
            SelectList selectCategory = PostItemService.generateCategoryList();
            ViewBag.selectCategory = selectCategory;

            //Builds the school class for edit page.
            School school = db.Schools.Where(d => d.school_name == postItem.school_name).First();
            ViewBag.main_color = school.main_hex_color;
            ViewBag.secondary_color = school.secondary_hex_color;

            if(postItem.photo_path_id.HasValue)
            {
                //*******AWS Portion *********************
                List<string> photoList = PostItemService.GetS3Photos(postItem);
                if (photoList != null && photoList.Count > 0)
                {
                    List<dynamic> photoPaths = new List<dynamic>();
                    foreach (var photo in photoList)
                        photoPaths.Add(new { image = photo.ToString() });
                    var results = JsonConvert.SerializeObject(photoPaths);
                    ViewBag.RESULTS = results;
                }
                //******************************************
            }

            return View(postItem);
        }

        // POST: PostItems/Edit
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [ValidateAntiForgeryToken]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit([Bind(Include = "object_id,username,school_name,post_date,price,title,description,photo_path_id,tags,category")] PostItem postItem, HttpPostedFileBase[] images)
        {
            if (ModelState.IsValid)
            {
                postItem.photo_path_id = Guid.NewGuid();
                db.Set<PostItem>().Attach(postItem);
                db.Entry(postItem).State = EntityState.Modified;
                db.SaveChanges();

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

        // POST: PostItems/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            PostItem postItemTemp = db.PostItems.Find(id);
            PostItemModel postItem = PostItemModel.bindToModel(postItemTemp);
            PostItemPhotos postItemPhotos = db.PostItemPhotos.Find(postItem.photo_path_id);

            //Remove associated flags from the database
            var flags = db.FlagPosts.Where(flag => flag.flagged_postitem_id == id).ToList();
            db.FlagPosts.RemoveRange(flags);

            db.PostItems.Remove(postItemTemp);
            PostItemPhotos postItemPhoto = db.PostItemPhotos.Find(postItem.photo_path_id);
            db.PostItemPhotos.Remove(postItemPhoto);

            //Delete AWS Photos    
            PostItemService.DeleteS3Photos(postItem);
            db.PostItemPhotos.Remove(postItemPhotos);

            db.SaveChanges();

            return RedirectToAction("MainMarketView", "MarketPlace");
        }

        [Authorize(Roles ="Admin")]
        public ActionResult ViewFlaggedPosts()
        {
            ContextFactory cf = new ContextFactory();
            
            using (var context = new CampusNabberEntities())
            {
                List<PostItem> posts = cf.PostItems.AsEnumerable().Cast<PostItem>().ToList();
                List<FlagPost> flags = cf.FlagPosts.AsEnumerable().Cast<FlagPost>().ToList();
                //var joinedTables = posts.Join(flags, postitem => postitem.object_id, flagpost => flagpost.flagged_postitem_id, (postitem, flagpost) => new { Title = postitem.title, Id = postitem.object_id, FlagReason = flagpost.flag_reason, PostDate = postitem.post_date, FlagId = flagpost.object_id });
                var flaggedPosts = posts.Join(flags, postitem => postitem.object_id, flagpost => flagpost.flagged_postitem_id, (postitem, flagpost) => new { Title = postitem.title, Id = postitem.object_id, PostDate = postitem.post_date}).Distinct();
                //var results = from item in joinedTables group item by item.Title into grp select new { Title = grp.Key };
                //var results = from item in joinedTables orderby item.Title select item;
                if (flaggedPosts.Count() > 0)
                {
                    List<PostXFlagViewModel> resultList = new List<PostXFlagViewModel>();
                    Guid currentGuid = Guid.Empty;
                    for (int i = 0; i < flaggedPosts.Count(); i++)
                    {
                        resultList.Add(new PostXFlagViewModel(flaggedPosts.ElementAt(i).Id, flaggedPosts.ElementAt(i).Title, flaggedPosts.ElementAt(i).PostDate));
                        resultList.Last().Flags = QueryFlags(resultList.Last().PostId);
                    }
                    return View("~/Views/PostXFlagViewModel/Index.cshtml", resultList);
                }
                
            }
            return View("~/Views/PostXFlagViewModel/Index.cshtml");
        }

        [Authorize(Roles = "Admin")]
        public ActionResult PostFlagDetails(PostXFlagViewModel model)
        {
            model.Flags = QueryFlags(model.PostId);
            return View("~/Views/PostXFlagViewModel/Details.cshtml", model);
        }

        [Authorize(Roles ="Admin")]
        public ActionResult PostFlagDetailsGuid(Guid post_id)
        {
            PostItem tempPost = db.PostItems.Where(post => post.object_id == post_id).First();
            PostXFlagViewModel model = new PostXFlagViewModel(tempPost.object_id, tempPost.title, tempPost.post_date);
            model.Flags = QueryFlags(tempPost.object_id);
            return View("~/Views/PostXFlagViewModel/Details.cshtml", model);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult RemoveFlags(Guid[] flag_ids)
        {
            if (flag_ids != null && flag_ids.Count() > 0)
            {
                Guid firstFlagId = flag_ids[0];
                Guid postId = db.FlagPosts.Where(flag => flag.object_id == firstFlagId).Select(flag => flag.flagged_postitem_id).AsEnumerable().First();
                foreach (Guid id in flag_ids)
                {
                    db.FlagPosts.RemoveRange(db.FlagPosts.Where(flag => flag.object_id == id));
                }
                db.SaveChanges();
                return (PostFlagDetailsGuid(postId));
            }
            return View("~/Views/PostXFlagViewModel/Index.cshtml");
        }

        protected IEnumerable<FlagPost> QueryFlags(Guid queryGuid)
        {
            ContextFactory cf = new ContextFactory();
            return cf.FlagPosts.Where(flag => flag.flagged_postitem_id == queryGuid).AsEnumerable();
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
