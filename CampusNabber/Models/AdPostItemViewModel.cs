using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CampusNabber.Models
{
    public class AdPostItemViewModel
    {
        private static CampusNabberEntities db = new CampusNabberEntities();


        public System.Guid object_id { get; set; }
        public string company_name { get; set; }
        public System.DateTime? post_date { get; set; }
        [Required(ErrorMessage = "Title is required")]
        public string title { get; set; }
        [Required(ErrorMessage = "Description is required")]
        public string description { get; set; }
        [Required(ErrorMessage = "Category is required")]
        public string category { get; set; }
        [Required(ErrorMessage = "Subcategory is required")]
        public string sub_category { get; set; }
        public Guid school_id { get; set; }      
        public string school_name { get; set; }  
        public SelectList schools { get; set; }
        

        public AdPostItem bindToAdPostItem()
        {
            AdPostItem adPostItem = new AdPostItem();
            adPostItem.object_id = this.object_id;
            adPostItem.company_name = this.company_name;
            adPostItem.post_date = this.post_date;
            adPostItem.title = this.title;
            adPostItem.description = this.description;
            adPostItem.category = this.category;
            adPostItem.sub_category = this.sub_category;
            adPostItem.school_id = db.Schools.Where(d => d.school_name == this.school_name).First().object_id;

            return adPostItem;
        }

        public void generateSchoolsList()
        {
            List<SelectListItem> list = new List<SelectListItem>();

            var genSchools = db.Schools.Select ( d =>
                    new SelectListItem { Text = d.school_name, Value = d.school_name}
                );
            this.schools = new SelectList(genSchools.ToList(), "Text", "Value", 1);

            /*
            list.Add(new SelectListItem { Text = "Eastern Washington University", Value = "Eastern Washington University", Selected = true });
            list.Add(new SelectListItem { Text = "Washington State University", Value = "Washington State University" });
            list.Add(new SelectListItem { Text = "Gonzaga", Value = "Gonzaga" });
            list.Add(new SelectListItem { Text = "Whitworth", Value = "Whitworth" });

            this.schools = new SelectList(list, "Text", "Value", 1);
            */
        }

        public static AdPostItemViewModel bindToModel(AdPostItem adPostItem)
        {
            AdPostItemViewModel adPostItemModel = new AdPostItemViewModel();
            adPostItemModel.object_id = adPostItem.object_id;
            adPostItemModel.company_name = adPostItem.company_name;
            adPostItemModel.post_date = adPostItem.post_date;
            adPostItemModel.title = adPostItem.title;
            adPostItemModel.description = adPostItem.description;
            adPostItemModel.category = adPostItem.category;
            adPostItemModel.sub_category = adPostItem.sub_category;
            adPostItemModel.school_name = db.Schools.Where(d => d.object_id == adPostItem.school_id).First().school_name;
            Guid guid = new Guid(adPostItem.category);
            Category cat = db.Categories.Where(d => d.object_id == new Guid(adPostItem.category)).First();

            adPostItemModel.category = cat.category_name;
            adPostItemModel.sub_category = cat.sub_category_name;
            return adPostItemModel;
        }

    }
}