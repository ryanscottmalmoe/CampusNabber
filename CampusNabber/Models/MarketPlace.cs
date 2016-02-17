using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CampusNabber.Models
{
    public class MarketPlace
    {
        public int Id { get; set; }

        public Guid StudentGuid { get; set; }

    
        public ApplicationUser CurrentUser { get; set; }

         public virtual ICollection<PostItem> Posts { get; set; }

        
    }
}