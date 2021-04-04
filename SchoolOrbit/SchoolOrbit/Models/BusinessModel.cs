
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SchoolOrbit.Models
{


    public  class UserDetails
    {
        public  int Iduser { get; set; }
        public  string screen_name { get; set; }
        public string role_name { get; set; }
        public  string photo_url { get; set; }
        public  int SchoolId { get; set; }
        public static UserDetails Current
        {
            get
            {
                UserDetails session =
                  (UserDetails)HttpContext.Current.Session["userDetails"];
                if (session == null)
                {
                   // session = new UserDetails();
                    //HttpContext.Current.Session["userDetails"] = session;
                }
                return session;
            }
        }
    }

    public class Employee
    {
        public int Id { get; set; }
        [Display(Name = "First Name")]
        [Required]
        public string first_name { get; set; }
        [Display(Name = "Middle Name")]
        public string middle_name { get; set; }
        [Display(Name = "Last Name")]
        public string last_name { get; set; }
        [Display(Name = "Gender")]
        [Required]
        public string gender { get; set; }
        [Display(Name = "Date of Birth")]
        [Required]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public System.DateTime DOB { get; set; }
        [Display(Name = "Display Name")]
        public string screen_name { get; set; }
        [Display(Name = "Photo")]
        public string photo_url { get; set; }
        [Required]
        [Display(Name = "HNO / Street ")]
        public string permanent_address { get; set; }        
        [Required]
        [Display(Name = "City ")]
        public string permanent_city { get; set; }        
        [Required]
        [Display(Name = "State ")]
        public string permanent_state { get; set; }        
        [Required]
        [Display(Name = "Country ")]
        public string permanent_country { get; set; }        
        [Required]
        [Display(Name = "Zip Code ")]
        public Nullable<int> permanent_zip_code { get; set; }
        [Required]
        [Display(Name = "HNO / Street ")]        
        public string present_address { get; set; }        
        [Required]
        [Display(Name = "City")]
        public string present_city { get; set; }
        [Required]
        [Display(Name = "State")]
        public string present_state { get; set; }        
        [Required]
        [Display(Name = "Country")]
        public string present_country { get; set; }        
        [Required]
        [Display(Name = "Zip Code")]
        public Nullable<int> present_zip_code { get; set; }
        [Display(Name = "Paediatrician Details")]
        public string paediatrician_details { get; set; }
        [Display(Name = "Allergic")]
        public string allergic { get; set; }
        [Display(Name = "Blood Group")]
        public string blood_group { get; set; }
        public bool active { get; set; }       
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Display(Name = "User Name")]
        public string UserName { get; set; }
        public int idSchool { get; set; }
        public int idUser { get; set; }
        [Required]
        [Display(Name = "Employee ID")]
        public int employee_number { get; set; }
        public int idDepartment { get; set; }

        [Display(Name = "Department Name")]
        public string department_name { get; set; }

        [Required]
        [Display(Name = "Designation")]
        public string designation { get; set; }
        [Required]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Date of Joining")]
        public Nullable<System.DateTime> WorkStartDate { get; set; }
        [Display(Name = "Date of Resignation")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> WorkEndDate { get; set; }
        [Required]
        public string Qualification { get; set; }        
        public Nullable<System.DateTime> PassingYear { get; set; }         
        public string University { get; set; }
        [Required]
        [Display(Name = "Bank Account Number")]
        public string BankAccountNo { get; set; }
        [Required]
        [Display(Name = "Bank Name")]
        public string BankName { get; set; }
        [Display(Name = "Bank IFSC Code")]
        public string IFSCCode { get; set; }
        [Display(Name = "ESI / PF Number")]
        public string ESIPFNumber { get; set; }
        [Display(Name = "PAN Number")]
        public string PanNumber { get; set; }
        [Display(Name = "Aadhaar Card Number")]
        public string AdharNumber { get; set; }       
        public Nullable<int> ManagerID { get; set; }
         [Display(Name = "Mobile Number")]
        public string mobilenumber { get; set; }
        [Display(Name = "Alternate Number")]
        public string AlternateNumber { get; set; }
        [Display(Name = "Resume")]
        public string cv_path { get; set; }
        [Display(Name = "Address Proof")]
        public string addressproof_path { get; set; }
    }
    public class Department
    {        
        public int idDepartment { get; set; }       
        public string department_name { get; set; }       
    }

    public class ExpenseType
    {
        public int id { get; set; }
        public int idSchool { get; set; }
        [Required]
        [Display(Name = "Expense Category")]
        public string expense_type { get; set; }
        [Required]
        [Display(Name = "Active")]
        public bool isactive { get; set; }
        public System.DateTime ts_entered { get; set; }
        public System.DateTime ts_updated { get; set; }
        public int last_updated_by { get; set; }
        public int entered_by { get; set; }
    }
    public class vewUserRoles
    {
        public int id { get; set; }
        [Required]
        [Display(Name="User")]
        public int idUser { get; set; }
        public int idSchool { get; set; }
        [Required]
        [Display(Name="Role")]
        public int idRole { get; set; }
        [Display(Name="Name")]
        public string displayname { get; set; }
        [Display(Name="Role")]
        public string role_name { get; set; }
    }
    public class ExpenseTrans
    {
     public enum ExpenseStatus {
            Approved = 4, Rejected = 2, Cancelled = 3, PendingApproval = 1
        };


       
        public int id { get; set; }
        public int idSchool { get; set; }
        [Display(Name = "Expense Category")]
        public int expense_type_id { get; set; }
        [Required]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Expense Date")]        
        public Nullable<System.DateTime> expense_date { get; set; }
        [Display(Name = "Mode of Payment")]
        public string mode_of_payment { get; set; }
        [Display(Name = "Cheque/DD Number")]
        public Nullable<int> cheque_dd_no { get; set; }
        [Display(Name = "Bank Name")]
        public string issue_bank { get; set; }
        [Required]
        [Display(Name = "Amount")]
        public Nullable<decimal> amount { get; set; }
        [Required]
        [Display(Name = "Remarks")]
        public string remarks { get; set; }
        [Display(Name = "Expense Category")]
        public string expense_type { get; set; }
        public Nullable<int> approved_by { get; set; }
        public Nullable<System.DateTime> approved_dt { get; set; }
        public string approved_remarks { get; set; }
        public System.DateTime ts_entered { get; set; }
        public System.DateTime ts_updated { get; set; }
        public int last_updated_by { get; set; }
        public int entered_by { get; set; }
        public int idExpensestatus { get; set; }
    }

    public class Notification
    {
        public int id { get; set; }

        public int idSchool { get; set; }

        public string To { get; set; }

        public string Body { get; set; }

        public string subject { get; set; }

        public Nullable<int> sentBy { get; set; }

        public bool is_active { get; set; }

        public System.DateTime ts_entered { get; set; }

        public Nullable<System.DateTime> ts_updated { get; set; }
    }
    
    public class SmsNotification {
        public int id { get; set; }

        public int idSchool { get; set; }

        public string To { get; set; }

        public string Body { get; set; }

        public Nullable<int> sentBy { get; set; }

        public System.DateTime ts_entered { get; set; }

        public Nullable<System.DateTime> ts_updated { get; set; }
        }

}