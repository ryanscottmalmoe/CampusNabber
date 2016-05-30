using DatabaseCode.CNObjectFolder;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;

namespace CampusNabber.Models
{
    public class PostItemModel
    {
        private static CampusNabberEntities db = new CampusNabberEntities();


        public System.Guid object_id { get; set; }
        public string username { get; set; }
        public string school_name { get; set; }
        public System.DateTime post_date { get; set; }
        public short price { get; set; }
        [Required(ErrorMessage = "Title is required")]
        public string title { get; set; }
        [Required(ErrorMessage = "Description is required")]
        public string description { get; set; }
        public System.Guid? photo_path_id { get; set; }
        [Required(ErrorMessage = "Category is required")]
        public string category { get; set; }
        [Required(ErrorMessage = "Subcategory is required")]
        public string subCategory { get; set; }
        [Required]
        public bool social_flag { get; set; }
        public List<string> photoPaths { get; set; }

        public PostItem bindToPostItem()
        {
            PostItem postItem = new PostItem();
            postItem.object_id = this.object_id;
            postItem.username = this.username;
            postItem.post_date = this.post_date;
            postItem.price = this.price;
            postItem.title = this.title;
            postItem.description = this.description;
            if (this.photo_path_id.HasValue)
                postItem.photo_path_id = this.photo_path_id.Value;
            else
                postItem.photo_path_id = Guid.Empty;
           // postItem.category = this.category;
            //postItem.category = this.subCategory;
            String category = this.category;
            String subCategory = this.subCategory;
            Category cat = db.Categories.Where(d => d.category_name == category && d.sub_category_name == subCategory).First();
            postItem.category = cat.object_id.ToString();
            postItem.school_id = db.Schools.Where(d => d.school_name == this.school_name).First().object_id;
            postItem.social_flag = this.social_flag;
            return postItem;
        }

        public static PostItemModel bindToModel(PostItem postItem)
        {
            PostItemModel postItemModel = new PostItemModel();
            postItemModel.object_id = postItem.object_id;
            postItemModel.username = postItem.username;
            postItemModel.post_date = postItem.post_date;
            postItemModel.price = postItem.price;
            postItemModel.title = postItem.title;
            postItemModel.description = postItem.description;
            postItemModel.photo_path_id = postItem.photo_path_id;
            
            postItemModel.school_name = db.Schools.Where(d => d.object_id == postItem.school_id).First().school_name;
            Guid guid = new Guid(postItem.category);
            Category cat = db.Categories.Where(d => d.object_id == new Guid(postItem.category)).First();

            postItemModel.category = cat.category_name;
            postItemModel.subCategory = cat.sub_category_name;
            postItemModel.social_flag = postItem.social_flag;
            return postItemModel;
        }

    }
}