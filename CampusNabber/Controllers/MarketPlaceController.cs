﻿using CampusNabber.Helpers.SchoolClasses;
using CampusNabber.Models;
using CampusNabber.Utility;
using DatabaseCode.CNQueryFolder;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
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
            var market = new MarketPlace(UserManager.FindById(User.Identity.GetUserId()));

            ApplicationUser user = UserManager.FindById(User.Identity.GetUserId());
            School school = SchoolFactory.BuildSchool(user.school_name);
            ViewBag.main_color = school.main_hex_color;
            market.setList();
            return View(market);
        }

        public ActionResult CategoryView(MarketPlace market)
        {
            market.setCategoryNames();
            market.chosenCategory = market.CategoryNames[(int)market.categoryToDisplay];
            return View(market);
        }



        public JsonResult GetPostItemData(jQueryDataTableParamModel param, string Category)
        {
            using (var context = new CampusNabberEntities())
            {
                IQueryable<PostItem> postItems = null;
                postItems = context.PostItems.Where(d =>
                                                      d.category == Category);
                                                       /*
                                                      d.PO_Order.OrderDate >= fromDate &&
                                                      d.PO_Order.OrderDate <= toDate &&
                                                      d.ItemID == itemID);
                                                      */
                


                // Count
                var count = postItems.Count();
                var iDisplayRecords = count;
                var totalRecords = count;
                
                // Search
                if (!string.IsNullOrEmpty(param.sSearch))
                {
                    postItems = postItems.Where(s =>
                                            //SqlFunctions.StringConvert((double)s.price).Contains(param.sSearch) ||
                                            s.title.Contains(param.sSearch) ||
                                            s.description.Contains(param.sSearch)
                        );
                }

                /*

                // Order
                var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
                //Expression<Func<PO_Order, int>> intOrdering = (sortInteger => sortInteger.ID);
                Expression<Func<PO_OrderDetail1, string>> stringOrdering = (sortString => sortColumnIndex == 1 ? sortString.PO_Order.PurchaseFrom : sortColumnIndex == 2 ? sortString.PPL_Account.Title : sortColumnIndex == 5 ? sortString.TypeID : sortString.StatusID);
                Expression<Func<PO_Order, DateTime>> dateOrdering = (sortDate => sortColumnIndex == 1 ? (sortDate.OrderDate == null ? sortDate.OrderDate : new DateTime()));

                var sortDirection = Request["sSortDir_0"]; // asc or desc
                if (sortColumnIndex == 0 || sortColumnIndex == 2 || sortColumnIndex == 3 || sortColumnIndex == 4 || sortColumnIndex == 5)
                {
                    if (sortDirection == "asc")
                    {
                        lines = lines.OrderBy(stringOrdering);
                    }
                    else
                    {
                        lines = lines.OrderByDescending(dateOrdering);
                    }
                }
                else if (sortColumnIndex == 1 || sortColumnIndex == 2 || sortColumnIndex == 5 || sortColumnIndex == 6)
                {
                    if (sortDirection == "asc")
                    {
                        orders = orders.OrderBy(stringOrdering);
                    }
                    else
                    {
                        orders = orders.OrderByDescending(stringOrdering);
                    }
                }
                else if (sortColumnIndex == 3 || sortColumnIndex == 4)
                {
                    if (sortDirection == "asc")
                    {
                        orders = orders.OrderBy(dateOrdering);
                    }
                    else
                    {
                        orders = orders.OrderByDescending(dateOrdering);
                    }
                }
                else
                {
                    orders = orders.OrderByDescending(intOrdering);
                }

                // Skip and take
                if (param.iDisplayLength != null && param.iDisplayStart != null)
                {
                    lines = lines.Skip(param.iDisplayStart).Take(param.iDisplayLength);
                }
                */
                var result = new List<PostItemTableModel>();


                // Project
                result.AddRange(postItems
                                    .ToList()
                                    .Select(d => new PostItemTableModel
                                    {
                                        PhotoPath = PostItemService.GetFirstPhotoPath(d.photo_path_id),
                                        Title = d.title.ToString(),
                                        Price = d.price.ToString(),
                                        Username = d.username,
                                    })
                                    .ToList()
                                );
                /*
                foreach (PostItemTableModel item in result)
                {
                    PostItemService.GetFirstPhotoPath(item);
                }
                */

                return Json(new
                {
                    sEcho = param.sEcho,
                    iTotalRecords = totalRecords,
                    iTotalDisplayRecords = iDisplayRecords,
                    aaData = result
                }, JsonRequestBehavior.AllowGet);
            
            }
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
