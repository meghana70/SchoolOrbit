//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SchoolOrbit.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class sys_user
    {
        public sys_user()
        {
            this.sch_student = new HashSet<sch_student>();
        }
    
        public int Id { get; set; }
        public string first_name { get; set; }
        public string middle_name { get; set; }
        public string last_name { get; set; }
        public string gender { get; set; }
        public System.DateTime DOB { get; set; }
        public string screen_name { get; set; }
        public string photo_url { get; set; }
        public string permanent_address { get; set; }
        public string permanent_city { get; set; }
        public string permanent_state { get; set; }
        public string permanent_country { get; set; }
        public Nullable<int> permanent_zip_code { get; set; }
        public string present_address { get; set; }
        public string present_city { get; set; }
        public string present_state { get; set; }
        public string present_country { get; set; }
        public Nullable<int> present_zip_code { get; set; }
        public string paediatrician_details { get; set; }
        public string allergic { get; set; }
        public string blood_group { get; set; }
        public bool active { get; set; }
        public string username { get; set; }
        public string aadhaar_number { get; set; }
        public string contact_number { get; set; }
        public string contact_number_alternate { get; set; }
    
        public virtual sch_parent sch_parent { get; set; }
        public virtual ICollection<sch_student> sch_student { get; set; }
    }
}