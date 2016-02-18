using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Mail;

namespace CampusNabber.Models
{
    public class GMailerModel
    {
        public static String GMailUsername { get; set; }
        public static String GMailPassword { get; set; }
        public static String GMailHost {get; set;}
        public static int GMailPort { get; set; }
        public static bool GMailSSL { get; set; }

        public string ToEmail { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public bool IsHtml { get; set; }

        static GMailerModel()
        {
            GMailHost = "smtp.gmail.com";
            GMailPort = 587;
            GMailSSL = true;
        }
        public void Send()
        {
            SmtpClient client = new SmtpClient();
            client.Host = GMailHost;
            client.Port = GMailPort;
            client.EnableSsl = GMailSSL;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential(GMailUsername, GMailPassword);

            using(MailMessage message = new MailMessage(GMailUsername, ToEmail))
            {
                message.Subject = Subject;
                message.Body = Body;
                message.IsBodyHtml = IsHtml;
                client.Send(message);
            }
        }
    }
}