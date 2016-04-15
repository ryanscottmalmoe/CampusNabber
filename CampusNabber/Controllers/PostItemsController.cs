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
using DatabaseCode.FactoryFiles;
using System.Linq.Dynamic;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using System.Configuration;
using System.IO;
using System.Collections;

namespace CampusNabber.Controllers
{
    public class PostItemsController : Controller
    {

        private static readonly string _awsAccessKey = "AKIAJ4CAE6M72TYTV2KA";
        private static readonly string _awsSecretKey = "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";
        private static readonly string _bucketName = "campusnabberphotos";



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
            PostItem postItem = db.PostItems.Find(id);
            if (postItem == null)
            {
                return HttpNotFound();
            }


            //Builds the school class for create page.
            School school = SchoolFactory.BuildSchool(postItem.school_name);
            ViewBag.main_color = school.main_hex_color;
            ViewBag.secondary_color = school.secondary_hex_color;
           
            ArrayList photoList = GetS3Photos(postItem);
            if(photoList.Count != 0)
            {
                string s3Url = photoList[0].ToString();
                ViewBag.AWS_URL = s3Url;
                ViewBag.NUMPHOTOS = photoList.Count;
                int photoNum = 0;
                foreach (var photo in photoList)
                {
                    string photoName = "photos" + photoNum;
                    Session[photoName] = photo;
                    photoNum++;
                }
            }


            return View(postItem);
        }
        
        // Get: /PostItems/Create
        public ActionResult Create(String userId)
        {
            PostItem postItem = null;
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

        //Post /PostItems/Create
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create([Bind(Include = "object_id,username,school_name,post_date,price,title,description,photo_path_id,tags,category")] PostItem postItem, HttpPostedFileBase[] images)
        {
            if (ModelState.IsValid)
            {
                //Sets the school_name here
                ApplicationUser user = UserManager.FindByName(postItem.username);
                postItem.school_name = user.school_name;
                postItem.post_date = System.DateTime.Now;
                postItem.object_id = Guid.NewGuid();
                postItem.photo_path_id = Guid.NewGuid();
                if (postItem.tags == null)
                    postItem.tags = "default";

                StoreS3Photos(images, postItem);
                
                postItem.createEntity();

                return RedirectToAction("Index");
            }
            return View(postItem);
        }

        /// <summary>
        /// This is a GET request for all of the images in a certain folder
        /// </summary>
        public ArrayList GetS3Photos(PostItem postItem)
        {
            PostItemPhotos photos = db.PostItemPhotos.Find(postItem.photo_path_id);
            ArrayList photosList = new ArrayList();
            for (int i = 0, counter = 1; i < photos.num_photos; i++, counter++)
                photosList.Add("https://s3-us-west-2.amazonaws.com/campusnabberphotos/" + postItem.username + "/" + postItem.photo_path_id.ToString() + "/" + counter.ToString());
            return photosList;


            /*
            IAmazonS3 client;
            using (client = new AmazonS3Client(_awsAccessKey, _awsSecretKey, Amazon.RegionEndpoint.USWest2))
            {
                GetObjectRequest request = new GetObjectRequest
                {
                    BucketName = _bucketName,
                    Key = postItem.username + "/" + postItem.photo_path_id.ToString() + "/" + imageCounter.ToString()
                };
            }
            */
        }

        
         /// <summary>
         /// This method stores all of the currently uploaded images to AWS S3
         /// </summary>
         /// <param name="images"> Images of the file upload</param>
         /// <param name="awsFolderName"> Username of the account</param>
         /// <param name="postItemID"> This associates this posting to the current user</param>
        public void StoreS3Photos(HttpPostedFileBase[] images, PostItem postItem)
        {   
            PostItemPhotos itemPhotos = new PostItemPhotos();
            itemPhotos.object_id = (Guid)postItem.photo_path_id;
            int imageCounter = 1;
            try
            {
                IAmazonS3 client;
                using (client = new AmazonS3Client(_awsAccessKey, _awsSecretKey, Amazon.RegionEndpoint.USWest2))
                {
                    foreach (var image in images)
                    {
                        if (image != null)
                        {
                            //Will need to save url to AWS S3 here....
                            var request = new PutObjectRequest()
                            {
                                BucketName = _bucketName,
                                CannedACL = S3CannedACL.PublicRead,//PERMISSION TO FILE PUBLIC ACCESIBLE
                                Key = postItem.username + "/" + itemPhotos.object_id.ToString() + "/" + imageCounter.ToString(),
                                InputStream = image.InputStream//SEND THE FILE STREAM
                            };
                            client.PutObject(request);
                            imageCounter++;
                        }
                    }
                }
                itemPhotos.num_photos = (short)(imageCounter - 1);
                itemPhotos.createEntity();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        


        // GET: PostItems/Edit
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

        // POST: PostItems/Edit
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [ValidateAntiForgeryToken]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit([Bind(Include = "object_id,username,school_name,post_date,price,title,description,photo_path_id,tags,category")] PostItem postItem, HttpPostedFileBase[] images)
        {
            if (ModelState.IsValid)
            {
                //Here we need to delete old photos. And upload new ones! 
                //      Will take care of this after break ~Ryan
                postItem.photo_path_id = Guid.NewGuid();

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

        // POST: PostItems/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            PostItem postItem = db.PostItems.Find(id);
            //Remove associated flags from the database
            var flags = db.FlagPosts.Where(flag => flag.flagged_postitem_id == id).ToList();
            db.FlagPosts.RemoveRange(flags);
            db.PostItems.Remove(postItem);
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
            return View("Index");
        }

        [Authorize(Roles = "Admin")]
        public ActionResult PostFlagDetails(PostXFlagViewModel model)
        {
            model.Flags = QueryFlags(model.PostId);
            return View("~/Views/PostXFlagViewModel/Details.cshtml", model);
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
