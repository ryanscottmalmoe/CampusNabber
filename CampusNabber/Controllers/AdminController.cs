using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using CampusNabber.Models;
using System.Linq.Expressions;
using CampusNabber.Utility;

namespace CampusNabber.Controllers
{
    [Authorize(Roles ="Admin")]
    public class AdminController : Controller
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
        //
        // GET: /Admin/
        public ActionResult AdminTools()
        {
            return View();
        }

        public ActionResult BlockUser()
        {
            return View();
        }
        [HttpPost]
        public ActionResult FindUserToBlock(string userEmail)
        {
            ApplicationUser result = UserManager.FindByEmail(userEmail);
            if(result != null)
            {
                ProfileModel user = new ProfileModel();
                user.getProfilePosts(result);
                user.user = result;
                School school = db.Schools.Where(d => d.object_id == result.school_id).First();
                user.school_name = school.school_name;
                return View(user);
            }
            else
            {
                return View("UserNotFound");
            }
        }

        public ActionResult LockoutUser(string userEmail)
        {
            ApplicationUser user = UserManager.FindByEmail(userEmail);
            if(user != null)
            {
                ViewBag.Username = user.UserName;
                UserManager.SetLockoutEnabled(user.Id, true);
                UserManager.SetLockoutEndDate(user.Id, DateTime.MaxValue.ToUniversalTime());
                UserManager.UpdateSecurityStamp(user.Id);
                return View("LockoutSuccess");
            }
            else
            {
                return View("UserNotFound");
            }
        }

        public ActionResult UnblockUser()
        {
            return View();
        }

        public ActionResult ManageAdPosts()
        {
            AdPostItemViewModel adPostItemViewModel = new AdPostItemViewModel();
            SelectList selectCategory = PostItemService.generateCategoryList();
            ViewBag.selectCategory = selectCategory;
            String[] categories = db.Categories.Select(d => d.category_name).Distinct().ToArray();
            ViewBag.categories = categories;
            adPostItemViewModel.generateSchoolsList();

            return View(adPostItemViewModel);
        }

        public JsonResult GetAdPosts(jQueryDataTableParamModel param)
        {
            using (var context = new CampusNabberEntities())
            {
                IQueryable<AdPostItem> adPostItems = db.AdPostItems;

                var totalRecords = 0;
                var result = new List<AdPostItemViewModel>();
                var iDisplayRecords = 0;


                    // Count
                    var count = adPostItems.Count();
                    iDisplayRecords += count;
                    totalRecords += count;

                // Search
                /*
                if (!string.IsNullOrEmpty(Search))
                    {
                        adPostItems = adPostItems.Where(s =>
                                                s.title.Contains(Search) ||
                                                s.description.Contains(Search)
                            );
                    }
                   */

                // Order
                var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
                Expression<Func<AdPostItem, DateTime?>> dateOrdering = (sortDate => sortDate.post_date);

                adPostItems = adPostItems.OrderBy(dateOrdering);

                // Project
                result.AddRange(adPostItems
                                    .ToList()
                                    .Select(d => new AdPostItemViewModel
                                    {
                                        company_name = d.company_name,
                                        category = d.category,
                                        sub_category = d.sub_category,
                                        title = d.title,
                                        school_name = db.Schools.Where(s => s.object_id == d.school_id).First().school_name,
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

        [Authorize(Roles="Admin")]
        public ActionResult FindUserToUnblock(string userEmail)
        {
            ApplicationUser result = UserManager.FindByEmail(userEmail);
            if (result != null)
            {
                ProfileModel user = new ProfileModel();
                user.getProfilePosts(result);
                user.user = result;
                School school = db.Schools.Where(d => d.object_id == result.school_id).First();
                user.school_name = school.school_name;
                return View(user);
            }
            else
            {
                return View("UserNotFound");
            }
        }

        public ActionResult UnlockUser(string userEmail)
        {
            ApplicationUser user = UserManager.FindByEmail(userEmail);
            if (user != null)
            {
                UserManager.SetLockoutEnabled(user.Id, true);
                UserManager.SetLockoutEndDate(user.Id, DateTime.UtcNow.Subtract(new TimeSpan(1, 0, 0, 0)));
                ViewBag.Username = user.UserName;
                return View("UnlockSuccess");
            }
            else
            {
                return View("UserNotFound");
            }
        }




        public ActionResult ManageSchools()
        {
            var schools = db.Schools.AsEnumerable();
            List<SchoolModel> schoolModels = new List<SchoolModel>();
            foreach(School s in schools)
            {
                schoolModels.Add(SchoolModel.bindToSchoolModel(s));
            }
            return View(schoolModels.AsEnumerable());
        }

        public ActionResult AddSchool()
        {
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CreateSchool([Bind(Include = "object_id,school_name,address,school_tag,main_hex_color,secondary_hex_color")] SchoolModel schoolModel)
        {
            if (ModelState.IsValid)
            {
                IEnumerable<School> existingSchools = db.Schools.Where(school => school.school_name == schoolModel.school_name).AsEnumerable();
                if(existingSchools.Count() > 0)
                {
                    ViewBag.school_name = schoolModel.school_name;
                    return View("SchoolExists");
                }
                School newSchool = new School();
                newSchool.address = "TempAddress";
                newSchool.school_name = schoolModel.school_name;
                newSchool.school_tag = schoolModel.school_tag;
                newSchool.main_hex_color = schoolModel.main_hex_color;
                newSchool.secondary_hex_color = schoolModel.secondary_hex_color;
                newSchool.object_id = Guid.NewGuid();
                db.Schools.Add(newSchool);
                db.SaveChanges();
                return View();
            }
            else
            {
                return View("AdminTools");
            }
        }

        public ActionResult ManageAdmins()
        {
            return View();
        }
	}
}