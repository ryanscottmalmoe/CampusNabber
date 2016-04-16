using CampusNabber.Helpers.SchoolClasses;
using CampusNabber.Models;
using DatabaseCode.CNQueryFolder;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CampusNabber.Controllers
{
    public class MarketPlaceController : Controller
    {
        private ApplicationUserManager _userManager;
        private CNQuery query;

        // GET: MarketPlace
        public ActionResult Index()
        {
           
            
            return View();
        }


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

        public MarketPlaceController()
        {
           
        }


        public MarketPlaceController(ApplicationUserManager manager)
        {
            UserManager = manager;
        }

        
        public ActionResult MainMarketView()
        {
            var market = new MarketPlace { };
            market.setList(UserManager.FindById(User.Identity.GetUserId()));

            ApplicationUser user = UserManager.FindById(User.Identity.GetUserId());
            School school = SchoolFactory.BuildSchool(user.school_name);
            ViewBag.main_color = school.main_hex_color;

            return MainMarketView(market);
            var market = new MarketPlace (UserManager.FindById(User.Identity.GetUserId()));
            market.setList();
            //Session["Market"] = market;
            return View(market);
        }

        public ActionResult CategoryView(MarketPlace market)
        {
            /*
            if (category != null)
                Session["Category"] = category;
            else
                category = (int?)Session["Category"];
            MarketPlace market = (MarketPlace)Session["Market"];
            Session.Clear();
            */
         //   MarketPlace market = new MarketPlace(UserManager.FindById(User.Identity.GetUserId()));
            market.setList();
            if(market.CategoryNames.Length < 4)
            {
                market.setCategoryNames();
            }
            market.Posts = market.Categories[(int)market.categoryToDisplay];
            market.numPosts = market.Posts.Count;
            //market.Posts = market.Categories[(int)category];
            //market.categoryToDisplay = category;
          //  market.numPosts = market.Posts.Count;
            return View(market);
        }


       [HttpPost]
        public ActionResult MainMarketView(MarketPlace market)
        {
            ModelState.Clear();
            return View(market);
        }

      

        // GET: MarketPlace/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: MarketPlace/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: MarketPlace/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: MarketPlace/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: MarketPlace/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: MarketPlace/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: MarketPlace/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
