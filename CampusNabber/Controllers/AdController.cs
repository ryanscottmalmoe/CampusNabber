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
        private CampusNabberEntities db = new CampusNabberEntities();

        public AdController(ApplicationUserManager _userManager)
        {
            UserManager = _userManager;
        }
        public AdController()
        {

        }

        public ActionResult Index()
        {
            db.Ads.ToList();
            List<AdModel> adModels = new List<AdModel>();
            foreach(Ad ad in db.Ads.ToList())
            {
                adModels.Add(AdModel.BindToAdModel(ad));
            }
            return View(adModels);
        }

        // Get: /PostItems/Create
        public ActionResult Create()
        {
            AdModel ad = new AdModel();
            return View(ad);
        }

        public ActionResult Details(AdModel adModel)
        {
            ViewBag.firstPhotoPath = AdService.GetFirstPhotoPath(adModel.BindAd());
            return View(adModel);
        }

        //Post /PostItems/Create
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create([Bind(Include = "object_id,company_name,photo_link")] AdModel adModel, HttpPostedFileBase image160x600, HttpPostedFileBase image468x60, HttpPostedFileBase image728x90)
        {
            Ad ad = null;
            if (ModelState.IsValid)
            {
                adModel.object_id = Guid.NewGuid();
                adModel.photo_path_160x600 = adModel.object_id.ToString() + "/160x600";
                adModel.photo_path_468x60 = adModel.object_id.ToString() + "/468x60";
                adModel.photo_path_728x90 = adModel.object_id.ToString() + "/728x90";
                ad = adModel.BindAd();

                //****AWS Portion**************
                AdService.StoreS3Photos(image160x600, image468x60, image728x90, ad);
                //******************************

                db.Ads.Add(ad);
                db.SaveChanges();

                return RedirectToAction("AdminTools", "Admin");
            }
            return View(ad);
        }

    }
}
