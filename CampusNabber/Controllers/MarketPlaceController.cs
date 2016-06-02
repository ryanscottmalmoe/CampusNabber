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
using System.Linq.Dynamic;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

namespace CampusNabber.Controllers
{
    [Authorize]
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
            market.SchoolToken = school.school_tag;
            market.mainSchoolColor = school.main_hex_color;
            //market.school_names.Add(school.school_name);
            if(Session["Color"] == null)
                Session["Color"] = school.main_hex_color;
            market.setList();
            Session["addSchoolView"] = "MainMarket";
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

        

        [HttpPost]
        public ActionResult AddAdditionalSchools(MarketPlace market)
        {
            var newMarket = new MarketPlace(UserManager.FindById(User.Identity.GetUserId()));
            newMarket.setCategoryNames();
            string fromView = (string)Session["addSchoolView"];
            if (fromView.Equals("Category"))
            {
                if (market.subCategoryToDisplay == null)
                {
                    if (market.categoryToDisplay != null)
                    {
                        newMarket.chosenCategory = newMarket.CategoryNames[(int)market.categoryToDisplay];
                    }
                    else
                        newMarket.chosenCategory = "All Categories";
                    }
                else
                {
                    newMarket.chosenCategory = newMarket.CategoryNames[(int)market.categoryToDisplay];
                    newMarket.chosenSubCategory = newMarket.SubCategoryNames[(int)market.categoryToDisplay].ElementAt((int)market.subCategoryToDisplay);
                }
                string searchCondition = (string)Session["searchString"];
                if (searchCondition != null)
                {
                    newMarket.searchString = searchCondition;
                    //Session.Remove("searchString");
                }
            }
            for (int i = 0; i < market.selectSchool.Count(); i++)
            {
                if (market.selectSchool[i])
                {
                    newMarket.selectSchool[i] = true;
                    newMarket.school_names.Add(newMarket.otherSchools[i]);
                    //  newMarket.otherSchools.Remove(newMarket.otherSchools[i]);
                }
            }
            //bool GU = Convert.ToBoolean(Request.Form["GU"]);
            //bool WU = Convert.ToBoolean(Request.Form["WU"]);
            
            newMarket.setList();
            if (fromView.Equals("Category"))
            {


                return View("CategoryView", newMarket);
            }
            return View("MainMarketView", newMarket);
        }

        [HttpPost]
        public ActionResult CategoryView(string Search)
        {
            MarketPlace market = new MarketPlace(UserManager.FindById(User.Identity.GetUserId()));
            ApplicationUser user = UserManager.FindById(User.Identity.GetUserId());
            market.setCategoryNames();
            market.chosenCategory = "All Categories";
            School school = db.Schools.Where(d => d.object_id == user.school_id).First();
            market.mainSchoolColor = school.main_hex_color;
            market.searchString = Search;
            Session["addSchoolView"] = "Category";
            Session["searchString"] = Search;
            return View(market);
        }

        public ActionResult CategoryView(MarketPlace market)
        {
            String[] schools = market.schools.Split(',');
            market.school_names = schools.ToList();
            market.setOtherSchools();
            market.setCategoryNames();
            String str = market.otherSchools.ElementAt(0);
            if (market.subCategoryToDisplay == null)
            {
                market.chosenCategory = market.CategoryNames[(int)market.categoryToDisplay];
            }
            else
            {
                market.chosenCategory = market.CategoryNames[(int)market.categoryToDisplay];
                market.chosenSubCategory = market.SubCategoryNames[(int)market.categoryToDisplay].ElementAt((int)market.subCategoryToDisplay);
            }
            ApplicationUser user = UserManager.FindById(User.Identity.GetUserId());
            Session["addSchoolView"] = "Category";

           // market.chosenCategory = market.CategoryNames[(int)market.categoryToDisplay];
            School school = db.Schools.Where(d => d.object_id == user.school_id).First();
            if (Session["Color"] == null)
                Session["Color"] = school.main_hex_color;
            market.mainSchoolColor = school.main_hex_color;
            market.searchString = "";
            return View(market);
        }



        public JsonResult GetPostItemData(jQueryDataTableParamModel param, String school_names, String user_name, string Category,
            string SubCategory, string Search = "", string FromPrice = "0", string ToPrice = "1000000000", bool HasImage = false)
       {
            using (var context = new CampusNabberEntities())
            {
                String[] schools = school_names.Split(',');
                Guid[] schoolIDS = new Guid[schools.Count()];
                String[] schoolTokens = new String[schools.Count()];
                School school = null;
                Guid schoolID;
                String schoolName;
                
                
                for (int i = 0; i < schools.Count(); i++)
                                    {
                    schoolName = schools[i];
                                        while (schoolName.ElementAt(0) == ' ')
                                           {
                         schoolName = schoolName.Substring(1);
                                            }
                    IQueryable < School > schoo = db.Schools.Where(d => d.school_name == schoolName);
                    school = schoo.First();
                    schoolIDS[i] = school.object_id;
                    schoolTokens[i] = school.school_tag;
                                    }
                
                                 IQueryable < PostItem > postItems = null;
                //IQueryable < PostItem > temp = null;
                var totalRecords = 0;
                var result = new List<PostItemTableModel>();
                var iDisplayRecords = 0;
                for (int i = 0; i < schoolIDS.Count(); i++)
                {
                    schoolID = schoolIDS[i];
                    //  IQueryable<PostItem> postItems = null;


                    if (!string.IsNullOrEmpty(FromPrice) || !string.IsNullOrEmpty(ToPrice))
                    {
                        if (FromPrice.Equals(""))
                            FromPrice = "0";
                        if (ToPrice.Equals(""))
                            ToPrice = "1000000000";
                        int fromPrice = Int32.Parse(FromPrice);
                        int toPrice = Int32.Parse(ToPrice);
                        if (Category.Equals("All Categories"))
                        {
                            postItems = context.PostItems.Where(d =>
                                                          d.price >= fromPrice &&
                                                          d.price <= toPrice &&
                                                          d.username != user_name &&
                                                          d.school_id == schoolID);
                        }
                        else
                        {
                            postItems = context.PostItems.Where(d =>
                                                          d.price >= fromPrice &&
                                                          d.price <= toPrice &&
                                                          d.price <= toPrice &&
                                                          d.username != user_name &&
                                                          d.school_id == schoolID);
                        }

                    }
                    else
                    {
                        if (Category.Equals("All Categories"))
                            postItems = context.PostItems.Where(d =>
                                                         d.username != user_name &&
                                                         d.school_id == schoolID);
                        else
                            postItems = context.PostItems.Where(d =>
                                                         d.username != user_name &&
                                                         d.school_id == schoolID);
                    }
                    if (HasImage)
                    {
                        postItems = postItems.Where(d => d.photo_path_id.HasValue);
                    }

                    

                    

                    // Search
                    if (!string.IsNullOrEmpty(Search))
                    {
                        postItems = postItems.Where(s =>
                                                s.title.Contains(Search) ||
                                                s.description.Contains(Search)
                            );
                        
                    }
                    if(Search.Equals(""))
                        Session["searchString"] = Search;
                    List<PostItemModel> models = new List<PostItemModel>();
                    foreach (PostItem post in postItems)
                    {
                        models.Add(PostItemModel.bindToModel(post));
                    }
                    //only search based off of our category
                    if (!Category.Equals("All Categories")) {
                        if (SubCategory.Length < 1)
                        {
                            models = models.Where(d => d.category == Category).ToList();

                        }
                        //search off category & subcategory
                        else
                        {
                            models = models.Where(d => d.category == Category && d.subCategory == SubCategory).ToList();
                        }
                    }
                    postItems = null;
                    List < PostItem > pi = new List<PostItem>();
                    foreach(PostItemModel post in models)
                    {
                        if(schoolIDS.Count() > 1)
                        {
                            post.title += " [" + schoolTokens[i]+"]";
                        }
                        pi.Add(post.bindToPostItem());
                    }
                    postItems = pi.AsQueryable();

                    // Count
                    var count = postItems.Count();
                    iDisplayRecords += count;
                    totalRecords += count;

                    // Skip and take
                    if (param.iDisplayLength != null && param.iDisplayStart != null)
                    {
                        postItems = postItems
                                    .Skip(param.iDisplayStart)
                                    .Take(param.iDisplayLength);
                    }

                    // Order
                    var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
                    Expression<Func<PostItem, int>> intOrdering = (sortInteger => sortInteger.price);
                    Expression<Func<PostItem, DateTime>> dateOrdering = (sortDate => sortDate.post_date);


                    var sortDirection = Request["sSortDir_0"]; // asc or desc
                    if (sortColumnIndex == 3)
                    {
                        if (sortDirection == "asc")
                        {
                            postItems = postItems.OrderBy(intOrdering);
                        }
                        else
                        {
                            postItems = postItems.OrderByDescending(intOrdering);
                        }
                    }
                    else
                    {
                        postItems = postItems.OrderBy(dateOrdering);
                    }




                    // Project
                    result.AddRange(postItems
                                        .ToList()
                                        .Select(d => new PostItemTableModel
                                        {
                                            PostItemID = d.object_id,
                                            PhotoPath = PostItemService.GetFirstPhotoPath(d),
                                            Title = d.title.ToString(),
                                            Price = d.price.ToString(),
                                            Username = d.username,
                                        })
                                        .ToList()
                                    );
                }
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
            Session["addSchoolView"] = "MainMarket";
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
