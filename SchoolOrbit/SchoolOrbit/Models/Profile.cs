using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SchoolOrbit.Models
{
    public class Profile
    {
        public int Id { get; set; }
        [Display(Name = "First Name")]
        public string first_name { get; set; }
        public string middle_name { get; set; }
        [Display(Name = "Last Name")]
        public string last_name { get; set; }
         [Display(Name = "Gender")]
        public string gender { get; set; }
        [Display(Name = "Date of Birth")]
         [DataType(DataType.Date)]
         [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public System.DateTime DOB { get; set; }
        [Display(Name = "Display Name")]
        public string screen_name { get; set; }
        [Display(Name = "Photo")]
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
        [Display(Name = "Email")]
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string UserName { get; set; }
    }
}