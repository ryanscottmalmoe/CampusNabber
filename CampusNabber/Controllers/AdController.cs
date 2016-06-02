using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.Entity;
using System.Net;
using CampusNabber.Models;
using CampusNabber.Utility;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Web;
using System.Web.Mvc;

namespace CampusNabber.Controllers
{
    [Authorize(Roles ="Admin")]
    public class AdController : Controller
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

        public AdController(ApplicationUserManager _userManager)
        {
            UserManager = _userManager;
        }
        public AdController()
        {

        }

        public ActionResult Index()
        {
            using (var context = new CampusNabberEntities())
            {
                context.Ads.ToList();
                List<AdModel> adModels = new List<AdModel>();
                foreach (Ad ad in context.Ads.ToList())
                {
                    adModels.Add(AdModel.BindToAdModel(ad));
                }
                return View(adModels);
            }
        }

        // Get: /PostItems/Create
        public ActionResult Create()
        {
            AdModel ad = new AdModel();
            return View(ad);
        }

        public ActionResult Details(AdModel adModel)
        {
            List<string> imageURLs = AdService.GetS3Photos(adModel);
            if(imageURLs.Count >= 3)
            {
                ViewBag.firstPhotoPath = imageURLs.ElementAt(0);
                ViewBag.secondPhotoPath = imageURLs.ElementAt(1);
                ViewBag.thirdPhotoPath = imageURLs.ElementAt(2);
            } 
            return View(adModel);
        }

        //Post /PostItems/Create
        [AcceptVerbs(HttpVerbs.Post)]
        [HttpPost]
        public ActionResult Create([Bind(Include = "object_id,company_name,photo_link")] AdModel adModel, HttpPostedFileBase image1, 
            HttpPostedFileBase image2, HttpPostedFileBase image3)
        {
            using (var context = new CampusNabberEntities())
            {
                Ad ad = null;
                if (ModelState.IsValid)
                {
                    if (image1 == null || image2 == null || image3 == null)
                    {
                        ViewBag.errorTitle = "File Not Found";
                        ViewBag.errorMsg = "One or more of the submitted photos were not uploaded properly.";
                        return View("~/Views/Admin/AdminError.cshtml");
                    }
                    adModel.object_id = Guid.NewGuid();
                    adModel.photo_path_160x600 = adModel.object_id.ToString() + "/160x600";
                    adModel.photo_path_468x60 = adModel.object_id.ToString() + "/468x60";
                    adModel.photo_path_728x90 = adModel.object_id.ToString() + "/728x90";
                    ad = adModel.BindAd();

                    //****AWS Portion**************
                    AdService.StoreS3Photos(image1, image2, image3, ad);
                    //******************************

                    context.Ads.Add(ad);
                    context.SaveChanges();

                    return RedirectToAction("AdminTools", "Admin");
                }
                return View(ad);
            }
        }

        // POST: PostItems/Delete
        public ActionResult Delete(Guid? id)
        {
            using (var context = new CampusNabberEntities())
            {
                Ad adTemp = context.Ads.Find(id);
                AdModel ad = AdModel.BindToAdModel(adTemp);

                context.Ads.Remove(adTemp);

                //Delete AWS Photos    
                AdService.DeleteS3Photos(ad);

                context.SaveChanges();

                return RedirectToAction("Index");
            }
        }


    }
}
