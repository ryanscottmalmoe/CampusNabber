using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CampusNabber.Models
{
    public class MarketPlace
    {
        //public ApplicationUser CurrentUser { get; set; }
        public int numPosts;
        public String[] CategoryNames { get; set; }
        public List<PostItem>[] Categories { get; set; }
        public List<PostItemModel>[] CategoriesToDisplay { get; set; }
        public List<PostItem> Posts { get; set; }
        public string school_name { get; set; }
        public Guid school_id { get; set; }
        public List<Guid> secondary_schools_to_display { get; set; }
        public string user_name { get; set; }
        public int rangeFrom { get; set; }
        public int displayRange { get; set; }
        public int rangeTo { get; set; }
        public int? categoryToDisplay { get; set; }
        public string chosenCategory { get; set; }
        public string mainSchoolColor { get; set; }
        public string userId { get; set; }
        public String SchoolToken { get; set; }
        public string searchString { get; set; }

        private static CampusNabberEntities db = new CampusNabberEntities();


        public MarketPlace(ApplicationUser user)
        {

            Categories = new List<PostItem>[4];
            secondary_schools_to_display = new List<Guid>();
            Posts = new List<PostItem>();
            CategoryNames = new String[] { "Automotive", "Books", "Housing", "Other" };
            numPosts = 0;
            displayRange = 2;
            rangeFrom = 0;
            rangeTo = displayRange;
            //might want to change this to a school object in the future //Christian
            //School school = db.Schools.Where(d => d.object_id == user.school_id).First();
            school_id = user.school_id;
            user_name = user.UserName;
            userId = user.Id;
   //         SchoolToken = setSchoolToken();
        }



        public MarketPlace()
        {
           
        }

        public void  setCategoryNames()
        {
            CategoryNames = new String[] { "Automotive", "Books", "Housing", "Other" };
        }

        public void setSchoolToken(String school_name)
        {
            String token = "";
                        if (school_name.Equals("Eastern Washington University"))
                            token += "[EWU]";
                        else if (school_name.Equals("Washington State University"))
                            token += "[WSU]";
                        else if (school_name.Equals("Gonzaga"))
                            token += "[GU]";
                        else if (school_name.Equals("Whitworth"))
                            token += "[WU]";
                        SchoolToken = token;
        }

        public void setList()
        {
            CategoriesToDisplay = new List<PostItemModel>[4];
            Categories = new List<PostItem>[4];
            Posts = new List<PostItem>();
            CategoryNames = new String[] { "Automotive", "Books", "Housing", "Other" };
            Categories = new List<PostItem>[CategoryNames.Length];
            for (int i = 0; i < CategoryNames.Length; i++)
            {
                string categoryNameTemp = CategoryNames[i];
                IQueryable<PostItem> postItem = null;
                postItem = db.PostItems.Where(d => d.school_id == school_id &&
                                                  d.username != user_name &&
                                                  d.category == categoryNameTemp);
                Categories[i] = postItem.ToList<PostItem>();
                CategoriesToDisplay[i] = new List<PostItemModel>();
                for(int j = 0; j < Categories[i].Count(); j++)
                {
                    CategoriesToDisplay[i].Add(PostItemModel.bindToModel(Categories[i].ElementAt(j)));
                }
                
            }
        }

        //increment the values by a higher factor to display more records per page
        public PostItem getPost(int index)
        {
            if (index < 0 || index > Posts.Count)
            {
                throw new Exception("This index is out of bounds");
            }
            else
            return Posts.ElementAt(index);
        }

        public void incrimentRange()
        {
            rangeTo += displayRange;
            rangeFrom += displayRange;
        }

        public void decrimentRange()
        {
            rangeTo -= displayRange;
            rangeFrom -= displayRange;
        }
    }

    

   
}