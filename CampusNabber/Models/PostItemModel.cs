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
        public System.Guid object_id { get; set; }
        public string username { get; set; }
        public string school_name { get; set; }
        public System.DateTime post_date { get; set; }
        public short price { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public Nullable<System.Guid> photo_path_id { get; set; }
        public string category { get; set; }
        public string tags { get; set; }

        public void deleteEntity(PostItem postItem)
        {
            //Creates new context and deletes local variable to server
            using (var context = new CampusNabberEntities())
            {
                context.PostItems.Remove(postItem);
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

        public void updateEntity()
        {
            //Creates new context and saves local variable to server
            using (var context = new CampusNabberEntities())
            {
                PostItem postItem = (from o in context.PostItems
                                     where o.object_id.Equals(object_id)
                                     select o).First();
                postItem.username = username;
                postItem.school_name = school_name;
                postItem.price = price;
                postItem.title = title;
                postItem.description = description;
                postItem.category = category;
                postItem.photo_path_id = photo_path_id;
                postItem.post_date = post_date;
                try
                {

                    context.PostItems.Attach(postItem);
                    var entry = context.Entry(postItem);

                    entry.Property(e => e.price).IsModified = true;
                    entry.Property(e => e.description).IsModified = true;
                    entry.Property(e => e.title).IsModified = true;
                    entry.Property(e => e.photo_path_id).IsModified = true;
                    entry.Property(e => e.category).IsModified = true;

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

        public void createEntity(PostItem postItem)
        {
            //Creates new context and saves local variable to server
            using (var context = new CampusNabberEntities())
            {
                context.PostItems.Add(postItem);
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

    }
}