using DatabaseCode.CNObjectFolder;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;

namespace CampusNabber.Models
{
    public class FlagPostModel
    {
        public System.Guid object_id { get; set; }
        public System.Guid flagged_postitem_id { get; set; }
        public string username_of_post { get; set; }
        public string flag_reason { get; set; }
        public string username_of_flagger { get; set; }
        public System.DateTime flag_date { get; set; }

    }
}