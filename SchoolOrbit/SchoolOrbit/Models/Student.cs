using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SchoolOrbit.Models
{
    public class Student
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "First Name")]
        public string first_name { get; set; }
        [Display(Name = "Middle Name")]
        public string middle_name { get; set; }
        [Required]
        [Display(Name = "Last Name")]
        public string last_name { get; set; }
        [Required]
        [Display(Name = "Gender")]
        public string gender { get; set; }
        [Display(Name = "Date of Birth")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]      
        public System.DateTime DOB { get; set; }
        [Display(Name = "Display Name")]
        public string screen_name { get; set; }
        [Display(Name = "Photo")]
        public string photo_url { get; set; }        
        [Required]
        [Display(Name="Father/Guardian Phone")]
        public string father_guardian_phone_no{ get; set;}
        [Required]
        [Display(Name = "Father/Guardian First Name")]
        public string father_guardian_first_name { get; set;}
        [Required]
        [Display(Name = "Father/Guardian Last Name")]
        public string father_guardian_last_name { get; set; }
        [Display(Name="Father/Guardian Email")]
        public string father_guardian_email {get;set;}
        [Display(Name="Father/Guardian Occupation")]
	    public string father_guardian_occupation{get;set;}
        [Display(Name="Mother Name")]
	    public string mother_name{get;set;}
        [Display(Name="Mother Phone")]
	    public string mother_phone_no{get;set;}
        [Display(Name="Mother Email")]
	    public string mother_email{get;set;}
        [Display(Name = "Mother Occupation")]
        public string mother_occupation { get; set; }
        [Display(Name = "HNO / Street ")]
        [Required]
        public string permanent_address { get; set; }
        [Display(Name = "City ")]
        [Required]
        public string permanent_city { get; set; }
        [Display(Name = "State ")]
        [Required]
        public string permanent_state { get; set; }
        [Display(Name = "Country ")]
        [Required]
        public string permanent_country { get; set; }
        [Display(Name = "Zip Code ")]
        [Required]
        public Nullable<int> permanent_zip_code { get; set; }
        [Display(Name = "HNO / Street ")]
        [Required]
        public string present_address { get; set; }
        [Display(Name = "City")]
        [Required]
        public string present_city { get; set; }
         [Required]
        [Display(Name = "State")]
        public string present_state { get; set; }
        [Display(Name = "Country")]
        [Required]
        public string present_country { get; set; }
        [Display(Name = "Zip Code")]
        [Required]
        public Nullable<int> present_zip_code { get; set; }
        [Display(Name = "Paediatrician Details")]
        public string paediatrician_details { get; set; }
        [Display(Name="Allergic")]
        public string allergic { get; set; }
        [Display(Name = "Blood Group")]
        public string blood_group { get; set; }
        public bool active { get; set; }
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }
        [Display(Name = "User Name")]
        public string UserName { get; set; }
        public int idSchool { get; set; }
        [Display(Name = "Admission Number")]
        [Required]
        public int admission_number { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]     
        [Display(Name = "Admission Date")]
        [Required]
        public Nullable<System.DateTime> admission_date { get; set; }
       [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> transfer_date { get; set; }       
     

    }

}
