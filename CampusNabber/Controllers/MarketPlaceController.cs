using CampusNabber.Helpers.SchoolClasses;
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
        private static CampusNabberEntities db = new CampusNabberEntities();

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
            School school = db.Schools.Where(d => d.object_id == user.school_id).First();
            market.setSchoolToken(school.school_name);
            market.mainSchoolColor = school.main_hex_color;
            market.school_name = school.school_name;
          //  Session["Color"] = school.main_hex_color;
            market.setList();
            return View(market);
        }

        [HttpPost]
        public ActionResult SearchSite(String Search)
        {
            if(Search.Length <1)
              return RedirectToAction("MainMarketView");
            else
                return RedirectToAction("MainMarketView");
        }

        public ActionResult CategoryView(MarketPlace market)
        {
            ApplicationUser user = UserManager.FindById(User.Identity.GetUserId());
            market.setCategoryNames();
            market.chosenCategory = market.CategoryNames[(int)market.categoryToDisplay];
            School school = db.Schools.Where(d => d.object_id == user.school_id).First();
            market.mainSchoolColor = school.main_hex_color;
            return View(market);
        }



        public JsonResult GetPostItemData(jQueryDataTableParamModel param, string Category, string Search="", string FromPrice="0", string ToPrice="1000000000")
        {
            using (var context = new CampusNabberEntities())
            {
                IQueryable<PostItem> postItems = null;
                if (!string.IsNullOrEmpty(FromPrice) || !string.IsNullOrEmpty(ToPrice))
                {
                    if (FromPrice.Equals(""))
                        FromPrice = "0";
                    if (ToPrice.Equals(""))
                        ToPrice = "1000000000";
                    int fromPrice = Int32.Parse(FromPrice);
                    int toPrice = Int32.Parse(ToPrice);
                    postItems = context.PostItems.Where(d =>
                                                      d.category == Category &&
                                                      d.price >= fromPrice &&
                                                      d.price <= toPrice);
                }
                else
                {
                    postItems = context.PostItems.Where(d =>
                                                    d.category == Category);
                }
                foreach (PostItem postItem in postItems)
                {
                    if(true)
                    {
                    }

                }

                // Count
                var count = postItems.Count();
                var iDisplayRecords = count;
                var totalRecords = count;
                
                // Search
                if (!string.IsNullOrEmpty(Search))
                {
                    postItems = postItems.Where(s =>
                                            //SqlFunctions.StringConvert((double)s.price).Contains(param.sSearch) ||
                                            s.title.Contains(Search) ||
                                            s.description.Contains(Search)
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
