using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Util;
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
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace CampusNabber.Utility
{
    abstract class AdService
    {
        private static string _awsAccessKey = "";
        private static string _awsSecretKey = "";
        private static readonly string _bucketName = "campusnabberadphotos";

        private static CampusNabberEntities db = new CampusNabberEntities();
        public static void getAWSCreds()
        {
            string desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            try
            {   // Open the text file using a stream reader.
                using (StreamReader sr = new StreamReader(desktop + "\\awscreds.txt"))
                {
                    // Read the stream to a string, and write the string to the console.
                    string line = sr.ReadToEnd();
                    string[] results = line.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                    _awsAccessKey = results[0];
                    _awsSecretKey = results[1];
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }
        }

        public static void DeleteS3Photos(AdModel ad)
        {
            getAWSCreds();
            List<string> photoStrings = GetS3Photos(ad);
            foreach (string photo in photoStrings)
            {
                IAmazonS3 client;
                client = new AmazonS3Client(_awsAccessKey, _awsSecretKey, Amazon.RegionEndpoint.USWest2);

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
        public static string GetFirstPhotoPath(Ad ad)
        {
            getAWSCreds();
            try
            {
                return "https://s3-us-west-2.amazonaws.com/campusnabberadphotos/" + ad.photo_path_160x600;
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// This is a GET request for all of the images in a certain folder
        /// </summary>
        public static List<string> GetS3Photos(AdModel ad)
        {
            getAWSCreds();
            List<string> photosList = new List<string>();
            photosList.Add("https://s3-us-west-2.amazonaws.com/campusnabberadphotos/" + ad.photo_path_160x600);
            photosList.Add("https://s3-us-west-2.amazonaws.com/campusnabberadphotos/" + ad.photo_path_468x60);
            photosList.Add("https://s3-us-west-2.amazonaws.com/campusnabberadphotos/" + ad.photo_path_728x90);
            return photosList;
        }


        /// <summary>
        /// This method stores all of the currently uploaded images to AWS S3
        /// </summary>
        /// <param name="images"> Images of the file upload</param>
        /// <param name="awsFolderName"> Username of the account</param>
        /// <param name="postItemID"> This associates this posting to the current user</param>
        public static void StoreS3Photos(HttpPostedFileBase photo_160x600, HttpPostedFileBase photo_468x60, HttpPostedFileBase photo_728x90, Ad ad)
        {
            try
            {
                getAWSCreds();
                IAmazonS3 client;
                using (client = new AmazonS3Client(_awsAccessKey, _awsSecretKey, Amazon.RegionEndpoint.USWest2))
                {
                    //Will need to save url to AWS S3 here....
                    var request = new PutObjectRequest()
                    {
                        BucketName = _bucketName,
                        CannedACL = S3CannedACL.PublicRead,//PERMISSION TO FILE PUBLIC ACCESIBLE
                        Key = ad.object_id.ToString() + "/160x600",
                        InputStream = photo_160x600.InputStream//SEND THE FILE STREAM
                    };
                    client.PutObject(request);
                    request = new PutObjectRequest()
                    {
                        BucketName = _bucketName,
                        CannedACL = S3CannedACL.PublicRead,
                        Key = ad.object_id.ToString() + "/468x60",
                        InputStream = photo_468x60.InputStream
                    };
                    client.PutObject(request);
                    request = new PutObjectRequest()
                    {
                        BucketName = _bucketName,
                        CannedACL = S3CannedACL.PublicRead,
                        Key = ad.object_id.ToString() + "/728x90",
                        InputStream = photo_728x90.InputStream
                    };
                    client.PutObject(request);
                }
                ad.photo_path_160x600 = ad.object_id.ToString() + "/160x600";
                ad.photo_path_468x60 = ad.object_id.ToString() + "/468x60";
                ad.photo_path_728x90 = ad.object_id.ToString() + "/728x90";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
