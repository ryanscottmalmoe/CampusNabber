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
    using System;
    using System.Collections.Generic;
    
    public partial class AdPostItem
    {
        public System.Guid object_id { get; set; }
        public string company_name { get; set; }
        public Nullable<System.DateTime> post_date { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string category { get; set; }
        public string sub_category { get; set; }
        public Nullable<System.Guid> school_id { get; set; }
    }
}
