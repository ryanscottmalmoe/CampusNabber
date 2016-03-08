//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CampusNabber.Models
{
    using DatabaseCode.CNObjectFolder;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Validation;
    using System.Diagnostics;
    using System.Linq;
    using System.Web.Mvc;

    public partial class PostItem
    {


        public System.Guid object_id { get; set; }
        public string username { get; set; }
        public string school_name { get; set; }
        public System.DateTime post_date { get; set; }
        public short price { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string photo_path { get; set; }
        public string category { get; set; }


        public void deleteEntity()
        {
            //Creates new context and deletes local variable to server
            using (var context = new CampusNabberEntities())
            {
                context.PostItems.Remove(this);
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
                postItem.photo_path = photo_path;
                postItem.post_date = post_date;
                try
                {

                    context.PostItems.Attach(postItem);
                    var entry = context.Entry(postItem);

                    entry.Property(e => e.price).IsModified = true;
                    entry.Property(e => e.description).IsModified = true;
                    entry.Property(e => e.title).IsModified = true;
                    entry.Property(e => e.photo_path).IsModified = true;
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

        public void createEntity()
        {
            //Creates new context and saves local variable to server
            using (var context = new CampusNabberEntities())
            {
                context.PostItems.Add(this);
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
