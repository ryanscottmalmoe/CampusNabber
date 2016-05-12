using DatabaseCode.CNObjectFolder;
using System;
using System.Collections.Generic;
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
        public string title { get; set; }
        public string description { get; set; }
        public System.Guid? photo_path_id { get; set; }
        public string category { get; set; }

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
            postItem.category = this.category;
            postItem.school_id = db.Schools.Where(d => d.school_name == this.school_name).First().object_id;
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
            postItemModel.category = postItem.category;
            postItemModel.school_name = db.Schools.Where(d => d.object_id == postItem.school_id).First().school_name;
            return postItemModel;
        }

    }
}