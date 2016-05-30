using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Diagnostics;


namespace CampusNabber.Models
{
    public class AdModel
    {
        public System.Guid object_id { get; set; }
        public string company_name { get; set; }
        public string photo_path_160x600 { get; set; }
        public string photo_path_468x60 { get; set; }
        public string photo_path_728x90 { get; set; }
        public string photo_link { get; set; }

        public AdModel()
        {

        }

        public Ad BindAd()
        {
            Ad ad = new Ad();
            ad.object_id = this.object_id;
            ad.company_name = this.company_name;
            ad.photo_path_160x600 = this.photo_path_160x600;
            ad.photo_path_468x60 = this.photo_path_468x60;
            ad.photo_path_728x90 = this.photo_path_728x90;
            ad.photo_link = this.photo_link;
            return ad;
        }

        public static AdModel BindToAdModel(Ad ad)
        {
            return new AdModel
            {
                object_id = ad.object_id,
                company_name = ad.company_name,
                photo_path_160x600 = ad.photo_path_160x600,
                photo_path_468x60 = ad.photo_path_468x60,
                photo_path_728x90 = ad.photo_path_728x90,
                photo_link = ad.photo_link
            };
        }

        public AdModel(string company_name, string photo_path_160x600, string photo_path_468x60, string photo_path_728x90, string photo_link)
        {
            this.object_id = Guid.NewGuid();
            this.company_name = company_name;
            this.photo_path_160x600 = photo_path_160x600;
            this.photo_path_468x60 = photo_path_468x60;
            this.photo_path_728x90 = photo_path_728x90;
            this.photo_link = photo_link;
        }

        public void DeleteEntity(Ad ad)
        {
            //Creates new context and deletes local variable to server
            using (var context = new CampusNabberEntities())
            {
                context.Ads.Remove(ad);
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

        public void UpdateEntity()
        {
            //Creates new context and saves local variable to server
            using (var context = new CampusNabberEntities())
            {
                Ad ad = (from o in context.Ads
                                 where o.object_id.Equals(object_id)
                                 select o).First();

                ad.photo_link = photo_link;
                ad.photo_path_160x600 = photo_path_160x600;
                ad.photo_path_468x60 = photo_path_468x60;
                ad.photo_path_728x90 = photo_path_728x90;

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

        public void CreateEntity(Ad ad)
        {
            //Creates new context and saves local variable to server
            using (var context = new CampusNabberEntities())
            {
                context.Ads.Add(ad);
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