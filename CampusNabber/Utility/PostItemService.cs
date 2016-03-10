using CampusNabber.Models;
using DatabaseCode.CNQueryFolder;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CampusNabber.Utility
{
    abstract class PostItemService
    {

        public static void deleteALlPostsByUsername(string username)
        {
            //Creates new context and deletes local variable to server
            using (var context = new CampusNabberEntities())
            {
                var postItems = (from o in context.PostItems
                                 where o.username.Equals(username)
                                 select o);
                foreach (var item in postItems)
                    context.PostItems.Remove(item);
                try
                {
                    context.SaveChanges();
                }
                catch (DbEntityValidationException dbEx)
                {
                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            Trace.TraceInformation("Property: {0} Error: {1}",
                                                    validationError.PropertyName,
                                                    validationError.ErrorMessage);
                        }
                    }
                }
                catch (DbUpdateException dbEx)
                {
                    Console.WriteLine(dbEx.Message);
                    foreach (var entries in dbEx.Entries)
                    {
                        Console.WriteLine(entries.Entity);
                        Console.WriteLine(dbEx.InnerException);
                    }
                }
            }

        }

        public static List<PostItem> getProfilePosts(ApplicationUser user)
        {
            CNQuery query = new CNQuery("PostItem");
            query.setQueryWhereKeyEqualToCondition("username", user.UserName);
            return query.select().Cast<PostItem>().ToList();
        }

        public static SelectList generateSchoolsList()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem { Text = "Eastern Washington University", Value = "Eastern Washington University", Selected = true });
            list.Add(new SelectListItem { Text = "Washington State University", Value = "Washington State University" });
            list.Add(new SelectListItem { Text = "Gonzaga", Value = "Gonzaga" });
            list.Add(new SelectListItem { Text = "Whitworth", Value = "Whitworth" });

            return new SelectList(list, "Text", "Value", 1);
        }
        public static SelectList generateCategoryList()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem { Text = "Automotive", Value = "Automotive", Selected = true });
            list.Add(new SelectListItem { Text = "Books", Value = "Books" });
            list.Add(new SelectListItem { Text = "Housing", Value = "Housing" });
            list.Add(new SelectListItem { Text = "Other", Value = "Other" });

            return new SelectList(list, "Text", "Value", 1);
        }

        public static void updateAllPostItemsInfo(ApplicationUser user, string oldUsername)
        {
            //Creates new context and saves local variable to server
            using (var context = new CampusNabberEntities())
            {
                List<PostItem> postItems = (from o in context.PostItems
                                            where o.username.Equals(oldUsername)
                                            select o).Cast<PostItem>().ToList();
                foreach (PostItem item in postItems)
                {
                    item.username = user.UserName;
                    item.school_name = user.school_name;
                    try
                    {
                        context.PostItems.Attach(item);
                        DbEntityEntry<PostItem> entry = context.Entry(item);
                        entry.State = System.Data.Entity.EntityState.Modified;
                        context.SaveChanges();
                    }
                    catch (DbEntityValidationException dbEx)
                    {
                        foreach (var validationErrors in dbEx.EntityValidationErrors)
                        {
                            foreach (var validationError in validationErrors.ValidationErrors)
                            {
                                Trace.TraceInformation("Property: {0} Error: {1}",
                                                        validationError.PropertyName,
                                                        validationError.ErrorMessage);
                            }
                        }
                    }
                    catch (DbUpdateException dbEx)
                    {
                        Console.WriteLine(dbEx.Message);
                        foreach (var entries in dbEx.Entries)
                        {
                            Console.WriteLine(entries.Entity);
                            Console.WriteLine(dbEx.InnerException);
                        }
                    }
                }
               
            }
        }

        public static void setMissingFields(PostItem postItem, ApplicationUserManager userManager)
        {
            if(postItem.username == null)
            {
                throw new Exception();
            }
            postItem.post_date = System.DateTime.Today;
            var user = userManager.FindByName(postItem.username).school_name;
            postItem.school_name = userManager.FindByName(postItem.username).school_name;
            postItem.object_id = Guid.NewGuid();
            postItem.photo_path = "";
        }
    }
}