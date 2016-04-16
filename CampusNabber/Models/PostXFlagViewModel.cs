/*The purpose of this ViewModel is to combine and display data about posts and
 their corresponding flags.*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CampusNabber.Models
{
    public class PostXFlagViewModel
    {
        public Guid PostId { get; set; }
        public string Title { get; set; }
        public DateTime DatePosted { get; set; }
        public IEnumerable<FlagPost> Flags { get; set; }
        public PostXFlagViewModel(Guid newGuid, string newTitle, DateTime newDatePosted)
        {
            PostId = newGuid;
            Title = newTitle;
            DatePosted = newDatePosted;
        }
        public PostXFlagViewModel()
        { }
    }
}