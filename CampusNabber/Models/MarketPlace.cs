using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CampusNabber.Models
{
    public class MarketPlace
    {
        public int Id { get; set; }

        public Guid StudentGuid { get; set; }

        // public virtual ICollection<Post> Posts { get; set; }

        //public virtual ApplicationUser User { get; set; }

        //public string ApplicationUserId { get; set; }
    }
}