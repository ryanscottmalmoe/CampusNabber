using Amazon.S3;
using Amazon.S3.Model;
using CampusNabber.Models;
using DatabaseCode.CNQueryFolder;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using System;
using System.Collections;
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
        private static readonly string _awsAccessKey = "AKIAJ4CAE6M72TYTV2KA";
        private static readonly string _awsSecretKey = "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";
        private static readonly string _bucketName = "campusnabberphotos";

        private static CampusNabberEntities db = new CampusNabberEntities();


        public static void DeleteS3Photos(PostItemModel postItem)
        {
            List<string> photoStrings = GetS3Photos(postItem);
            foreach (string photo in photoStrings)
            {
                IAmazonS3 client;
                client = new AmazonS3Client(Amazon.RegionEndpoint.USWest2);

                DeleteObjectRequest deleteObjectRequest =
                    new DeleteObjectRequest
                    {
                        BucketName = _bucketName,
                        Key = photo
                    };

                using (client = Amazon.AWSClientFactory.CreateAmazonS3Client(
                     _awsAccessKey, _awsSecretKey))
                {
                    client.DeleteObject(deleteObjectRequest);
                }
            }
        }

        /// <summary>
        /// This is a GET request for all of the images in a certain folder
        /// </summary>
        public static string GetFirstPhotoPath(PostItem postItem)
        {
            if (!postItem.photo_path_id.HasValue)
                return "/Content/images/Lloyd_Gibson_aka_pedo.jpg";
            try
            {
                PostItemPhotos photos = db.PostItemPhotos.Find(postItem.photo_path_id);
                return "https://s3-us-west-2.amazonaws.com/campusnabberphotos/" + postItem.photo_path_id.ToString() + "/" + "1";
            }
            catch
            {
                return "";
            }


        }

        /// <summary>
        /// This is a GET request for all of the images in a certain folder
        /// </summary>
        public static List<string> GetS3Photos(PostItemModel postItem)
        {
            PostItemPhotos photos = db.PostItemPhotos.Find(postItem.photo_path_id);
            List<string> photosList = new List<string>();
            if (photos.num_photos == 0)
                return null;
            for (int i = 0, counter = 1; i < photos.num_photos; i++, counter++)
                photosList.Add("https://s3-us-west-2.amazonaws.com/campusnabberphotos/" + postItem.photo_path_id.ToString() + "/" + counter.ToString());
            return photosList;
        }

        /// <summary>
        /// This method stores all of the currently uploaded images to AWS S3
        /// </summary>
        /// <param name="images"> Images of the file upload</param>
        /// <param name="awsFolderName"> Username of the account</param>
        /// <param name="postItemID"> This associates this posting to the current user</param>
        public static void StoreS3Photos(HttpPostedFileBase[] images, PostItem postItem)
        {
            PostItemPhotos itemPhotos = new PostItemPhotos();
            itemPhotos.object_id = (Guid)postItem.photo_path_id;
            int imageCounter = 1;
            try
            {
                IAmazonS3 client;
                using (client = new AmazonS3Client(_awsAccessKey, _awsSecretKey, Amazon.RegionEndpoint.USWest2))
                {
                    foreach (var image in images)
                    {
                        if (image != null)
                        {
                            //Will need to save url to AWS S3 here....
                            var request = new PutObjectRequest()
                            {
                                BucketName = _bucketName,
                                CannedACL = S3CannedACL.PublicRead,//PERMISSION TO FILE PUBLIC ACCESIBLE
                                Key = itemPhotos.object_id.ToString() + "/" + imageCounter.ToString(),
                                InputStream = image.InputStream//SEND THE FILE STREAM
                            };
                            client.PutObject(request);
                            imageCounter++;
                        }
                    }
                }
                itemPhotos.num_photos = (short)(imageCounter - 1);
                db.PostItemPhotos.Add(itemPhotos);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static void deleteAllPostsByUsername(string username)
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
            return db.PostItems.Where(p => p.username == user.UserName).ToList<PostItem>();
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
                    item.school_id = user.school_id;
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

        /*
        public static void setMissingFields(PostItem postItem, ApplicationUserManager userManager)
        {
            if(postItem.username == null)
            {
                throw new Exception();
            }
            postItem.post_date = System.DateTime.Today;
            var user = userManager.FindByName(postItem.username).school_name;
            postItem.school_id = userManager.FindByName(postItem.username).school_name;
            postItem.object_id = Guid.NewGuid();
            postItem.photo_path_id = Guid.NewGuid();
        }
        */
    }
}