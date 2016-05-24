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
        private static readonly string _bucketName = "adphotos";

        private static CampusNabberEntities db = new CampusNabberEntities();
        public static void getAWSCreds()
        {
            string desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            try
            {   // Open the text file using a stream reader.
                using (StreamReader sr = new StreamReader(desktop + "\\awscreds.txt"))
                {
                    // Read the stream to a string, and write the string to the console.
                    String line = sr.ReadToEnd();
                    String[] results = line.Split(new String[]{"\r\n"}, StringSplitOptions.None);
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
        public static string GetFirstPhotoPath(Ad ad)
        {
            getAWSCreds();
            try
            {
                return "https://s3.amazonaws.com/adphotos/" + ad.photo_path_160x600;
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
            photosList.Add("https://s3.amazonaws.com/adphotos/" + ad.photo_path_160x600);
            photosList.Add("https://s3.amazonaws.com/adphotos/" + ad.photo_path_468x60);
            photosList.Add("https://s3.amazonaws.com/adphotos/" + ad.photo_path_728x90);
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
                ad.photo_path_160x600 = "https://campusnabberphotos.amazonaws.com/adphotos/" + ad.object_id.ToString() + "/160x600";
                ad.photo_path_468x60 = "https://campusnabberphotos.amazonaws.com/adphotos/" + ad.object_id.ToString() + "/468x60";
                ad.photo_path_728x90 = "https://campusnabberphotos.amazonaws.com/adphotos/" + ad.object_id.ToString() + "/728x90";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public static void TestAmazonImageBucket()
        {
            getAWSCreds();
            IAmazonS3 client;
            client = new AmazonS3Client(Amazon.RegionEndpoint.USWest2);
            try
            {
                ListObjectsRequest request = new ListObjectsRequest
                {
                    BucketName = _bucketName,
                    MaxKeys = 2
                };
                do
                {
                    ListObjectsResponse response = client.ListObjects(request);

                    // Process response.
                    foreach (S3Object entry in response.S3Objects)
                    {
                        Console.WriteLine("key = {0} size = {1}",
                            entry.Key, entry.Size);
                    }

                    // If response is truncated, set the marker to get the next 
                    // set of keys.
                    if (response.IsTruncated)
                    {
                        request.Marker = response.NextMarker;
                    }
                    else
                    {
                        request = null;
                    }
                } while (request != null);
            }
            catch (AmazonS3Exception amazonS3Exception)
            {
                if (amazonS3Exception.ErrorCode != null &&
                    (amazonS3Exception.ErrorCode.Equals("InvalidAccessKeyId")
                    ||
                    amazonS3Exception.ErrorCode.Equals("InvalidSecurity")))
                {
                    Console.WriteLine("Check the provided AWS Credentials.");
                    Console.WriteLine(
                    "To sign up for service, go to http://aws.amazon.com/s3");
                }
                else
                {
                    Console.WriteLine(
                     "Error occurred. Message:'{0}' when listing objects",
                     amazonS3Exception.Message);
                }
            }
        }
    }
}
