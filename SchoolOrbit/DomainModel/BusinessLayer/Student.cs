using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.BusinessLayer
{
   public  class Student
    {
        //test
        private DataModel.SOEntities db = new DataModel.SOEntities();
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
        [Display(Name = "Father/Guardian Phone")]
        public string father_guardian_phone_no { get; set; }
        [Required]
        [Display(Name = "Father/Guardian First Name")]
        public string father_guardian_first_name { get; set; }
        [Required]
        [Display(Name = "Father/Guardian Last Name")]
        public string father_guardian_last_name { get; set; }
        [Display(Name = "Father/Guardian Email")]
        public string father_guardian_email { get; set; }
        [Display(Name = "Father/Guardian Occupation")]
        public string father_guardian_occupation { get; set; }
        [Display(Name = "Mother Name")]
        public string mother_name { get; set; }
        [Display(Name = "Mother Phone")]
        public string mother_phone_no { get; set; }
        [Display(Name = "Mother Email")]
        public string mother_email { get; set; }
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
        [Display(Name = "Allergic")]
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



        public Student()
        {
           
        }

        public Student GetStudentdetails(int Uid, int schoolid)
        {
            var student = (from stu in db.sch_student
                           join ua in db.sys_user on stu.idUser equals ua.Id
                           join prnt in db.sch_parent on stu.idParent equals prnt.idUser
                           where stu.idSchool == schoolid & stu.idUser == Uid
                           select new DomainModel.BusinessLayer.Student()
                           {
                               Id = stu.idUser,
                               UserName = ua.username,
                               admission_date = stu.admission_date,
                               admission_number = stu.admission_number,
                               first_name = ua.first_name,
                               last_name = ua.last_name,
                               DOB = ua.DOB,
                               photo_url = ua.photo_url,
                               screen_name = ua.screen_name,
                               allergic = ua.allergic,
                               blood_group = ua.blood_group,
                               gender = ua.gender,
                               middle_name = ua.middle_name,
                               paediatrician_details = ua.paediatrician_details,
                               permanent_address = ua.permanent_address,
                               permanent_city = ua.permanent_city,
                               permanent_country = ua.permanent_country,
                               permanent_state = ua.permanent_state,
                               permanent_zip_code = ua.permanent_zip_code,
                               present_address = ua.present_address,
                               present_city = ua.present_city,
                               present_country = ua.present_country,
                               present_state = ua.present_state,
                               present_zip_code = ua.present_zip_code,
                               father_guardian_phone_no = prnt.father_guardian_phone_no,
                               father_guardian_first_name = prnt.father_guardian_name,
                               father_guardian_last_name = prnt.father_guardian_name,
                               father_guardian_email = prnt.father_guardian_email,
                               father_guardian_occupation = prnt.father_guardian_occupation,
                               mother_name = prnt.mother_name,
                               mother_email = prnt.mother_email,
                               mother_phone_no = prnt.mother_phone_no,
                               mother_occupation = prnt.mother_occupation
                           }).SingleOrDefault();

            return student;
        }
        public String GetParentDetails(string phone_no)
        {
            String strRes = string.Empty;
            var qry = (from prnt in db.sch_parent
                       where prnt.father_guardian_phone_no == phone_no
                       select prnt).ToList();
            if (qry.Count > 0)
            {
                foreach (var q in qry)
                {
                    strRes = "{\"fname\":\"" + q.father_guardian_name + "\",\"lname\":\"" + q.father_guardian_name + "\",\"femail\":\"" + (string.IsNullOrEmpty(q.father_guardian_email) ? "" : q.father_guardian_email) + "\",";
                    strRes += "\"foccp\":\"" + q.father_guardian_occupation + "\",\"mname\":\"" + q.mother_name + "\",\"memail\":\"" + q.mother_email + "\",";
                    strRes += "\"mphone\":\"" + (string.IsNullOrEmpty(q.mother_phone_no) ? "" : q.mother_phone_no) + "\",\"moccp\":\"" + (string.IsNullOrEmpty(q.mother_occupation) ? "" : q.mother_occupation) + "\"}";
                }
            }
            return strRes;
        }

        public int CheckAdmissionNumber(int admn_no, int sch_id, int? std_id)
        {

            if (std_id != null)
            {
                var qry = (from stu in db.sch_student
                           where stu.admission_number == admn_no & stu.idSchool == sch_id & stu.idUser != std_id
                           select stu).ToList();

                if (qry.Count > 0)
                {
                    return qry.Count;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                var qry = (from stu in db.sch_student
                           where stu.admission_number == admn_no & stu.idSchool == sch_id
                           select stu).ToList();
                if (qry.Count > 0)
                {
                    return qry.Count;
                }
                else
                {
                    return 0;
                }
            }
            
        }

        public List<Student> getstudents(int school_id)
        {
            var query = from stu in db.sch_student
                        join ua in db.sys_user on stu.idUser equals ua.Id
                        where stu.idSchool == school_id & stu.is_active == true & ua.active == true
                        select new  DomainModel.BusinessLayer.Student()
                        {
                            admission_date = stu.admission_date,
                            admission_number = stu.admission_number,
                            first_name = ua.first_name,
                            last_name = ua.last_name,
                            photo_url = ua.photo_url,
                            gender = ua.gender,
                            DOB = ua.DOB,
                            Id = stu.idUser
                        };
            return query.ToList();
        }

       public Boolean Addstudent(Student student,int idSchool){
           using (var transaction = db.Database.BeginTransaction())
           {
               try
               {
                   string strUserName = student.first_name.Substring(0, 1).ToLower() + "" + student.last_name.Substring(0, 1).ToLower();
                   string strStudentEmail = "";
                   if (student.Email != null)
                   {
                       strStudentEmail = student.Email;
                   }
                   else
                   {
                       strStudentEmail = student.first_name.ToLower() + "." + student.last_name.ToLower() + "@schoolorbit.com";
                   }
                   string strDisplayName = "";
                   if (student.screen_name != null)
                   {
                       strDisplayName = student.screen_name;
                   }
                   else
                   {
                       strDisplayName = student.first_name + " " + student.last_name;
                   }
                   string strParentEmail = "";
                   string strParentUserName = student.father_guardian_phone_no;
                   if (student.father_guardian_email != null)
                   {
                       strParentEmail = student.father_guardian_email;
                   }
                   else
                   {
                       strParentEmail = student.father_guardian_first_name.ToLower() + "." + student.father_guardian_last_name + "@schoolorbit.com";
                   }

                       DataModel.sys_user useraccnt = new DataModel.sys_user();
                       useraccnt.active = true;
                       useraccnt.allergic = student.allergic;
                       useraccnt.blood_group = student.blood_group;
                       useraccnt.DOB = student.DOB;
                       useraccnt.first_name = student.first_name;
                       useraccnt.gender = student.gender;
                       useraccnt.last_name = student.last_name;
                       useraccnt.middle_name = student.middle_name;
                       useraccnt.paediatrician_details = student.paediatrician_details;
                       useraccnt.permanent_address = student.permanent_address;
                       useraccnt.permanent_city = student.permanent_city;
                       useraccnt.permanent_country = student.permanent_country;
                       useraccnt.permanent_state = student.permanent_state;
                       useraccnt.permanent_zip_code = student.permanent_zip_code;
                       useraccnt.photo_url = "/pictures/default-avatar.png";
                       useraccnt.present_address = student.present_address;
                       useraccnt.present_city = student.present_city;
                       useraccnt.present_country = student.present_country;
                       useraccnt.present_state = student.present_state;
                       useraccnt.present_zip_code = student.present_zip_code;
                       useraccnt.screen_name = strDisplayName;
                       db.sys_user.Add(useraccnt);
                       db.SaveChanges();
                   //Create Login for Student
                       UserLogin Login = new UserLogin();
                       strUserName = strUserName + useraccnt.Id;
                       var res = Login.CreateUser(useraccnt.Id, UserLogin.LoginUserType.student, strStudentEmail, strUserName);

                  
                             //check parent already exists
                       var xuser = db.sch_parent.Where(x => x.father_guardian_email == student.father_guardian_email).FirstOrDefault();
                       DataModel.sys_user prntuseraccnt = new DataModel.sys_user();
                       if (xuser == null)
                       {
                           prntuseraccnt.active = true;
                           prntuseraccnt.DOB = Convert.ToDateTime("1900/01/01");
                           prntuseraccnt.first_name = student.father_guardian_first_name;
                           prntuseraccnt.gender = "M";
                           prntuseraccnt.last_name = student.father_guardian_last_name;
                           prntuseraccnt.permanent_address = student.permanent_address;
                           prntuseraccnt.permanent_city = student.permanent_city;
                           prntuseraccnt.permanent_country = student.permanent_country;
                           prntuseraccnt.permanent_state = student.permanent_state;
                           prntuseraccnt.permanent_zip_code = student.permanent_zip_code;
                           prntuseraccnt.photo_url = "/pictures/default-avatar.png";
                           prntuseraccnt.present_address = student.present_address;
                           prntuseraccnt.present_city = student.present_city;
                           prntuseraccnt.present_country = student.present_country;
                           prntuseraccnt.present_state = student.present_state;
                           prntuseraccnt.present_zip_code = student.present_zip_code;
                           prntuseraccnt.screen_name = student.father_guardian_first_name + " " + student.father_guardian_last_name;
                           db.sys_user.Add(prntuseraccnt);
                           db.SaveChanges();


                           //Create Login for Parent
                           UserLogin ParentLogin = new UserLogin();
                           var result = ParentLogin.CreateUser(prntuseraccnt.Id, UserLogin.LoginUserType.parent, strParentEmail, strParentUserName);

                           DataModel.sch_parent prnt = new DataModel.sch_parent();
                           prnt.idUser = prntuseraccnt.Id;
                           prnt.father_guardian_email = student.father_guardian_email;
                           prnt.father_guardian_name = student.father_guardian_first_name;
                           prnt.father_guardian_occupation = student.father_guardian_occupation;
                           prnt.father_guardian_phone_no = student.father_guardian_phone_no;
                           prnt.mother_name = student.mother_name;
                           prnt.mother_email = student.mother_email;
                           prnt.mother_phone_no = student.mother_phone_no;
                           prnt.mother_occupation = student.mother_occupation;
                           prnt.ts_entered = DateTime.Now;
                           prnt.ts_updated = DateTime.Now;
                           db.sch_parent.Add(prnt);
                           db.SaveChanges();
                       }
                       
                           //link parent id to student
                           DataModel.sch_student stud = new DataModel.sch_student();
                           stud.admission_date = student.admission_date;
                           // Need to put Validation for Duplicate Admission Numbers..
                           stud.admission_number = student.admission_number;
                           // End
                           // School Id should come Roles 
                           stud.idSchool = idSchool;
                           // end
                           stud.idUser = useraccnt.Id ;
                           stud.is_active = true;
                           if (xuser == null)
                           {
                               stud.idParent = prntuseraccnt.Id;
                           }
                           else
                           {
                               stud.idParent = xuser.idUser;
                           }
                           stud.ts_entered = DateTime.Now;
                           stud.ts_updated = DateTime.Now;
                           db.sch_student.Add(stud);
                           db.SaveChanges();
                           transaction.Commit();
                            return true;
                       }
                      
               catch (Exception Ex)
               {

                   transaction.Rollback();
                   return false;
                   throw Ex;
               }

           }
       }

       public Boolean deleteStudent(int id)
       {
           try
           {
               var schemployee = db.sch_student.Where(x => x.idUser == id).FirstOrDefault();
               schemployee.is_active = false;
               var sysuser = db.sys_user.Where(y => y.Id == id).FirstOrDefault();
               sysuser.active = false;
               db.SaveChanges();
               return true;
           }
           catch
           {
               return false;
           }
       }
        public Boolean SaveStudent(Student student,int idSchool){
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    //var user = db.sys_user.Where(x => x.Id == student.Id).First();
                    int idParent = 0;
                    //user.Email = student.Email;
                    //user.UserName = student.UserName;

                    var sysuser = db.sys_user.Where(y => y.Id == student.Id).FirstOrDefault();
                    sysuser.active = true;
                    sysuser.allergic = student.allergic;
                    sysuser.blood_group = student.blood_group;
                    sysuser.DOB = student.DOB;
                    sysuser.first_name = student.first_name;
                    sysuser.gender = student.gender;
                    sysuser.last_name = student.last_name;
                    sysuser.middle_name = student.middle_name;
                    sysuser.paediatrician_details = student.paediatrician_details;
                    sysuser.permanent_address = student.permanent_address;
                    sysuser.permanent_city = student.permanent_city;
                    sysuser.permanent_country = student.permanent_country;
                    sysuser.permanent_state = student.permanent_state;
                    sysuser.permanent_zip_code = student.permanent_zip_code;
                    //sysuser.photo_url = student.photo_url;
                    sysuser.present_address = student.present_address;
                    sysuser.present_city = student.present_city;
                    sysuser.present_country = student.present_country;
                    sysuser.present_state = student.present_state;
                    sysuser.present_zip_code = student.present_zip_code;

                    var stu = db.sch_student.Where(z => z.idUser == student.Id & z.idSchool == idSchool).FirstOrDefault();
                    stu.admission_number = student.admission_number;
                    stu.admission_date = student.admission_date;
                    stu.ts_updated = DateTime.Now;
                    idParent = stu.idParent;

                    var prnt = db.sch_parent.Where(x => x.idUser == idParent).FirstOrDefault();
                    //prnt.father_guardian_name = student.father_guardian_last_name + ", " + student.father_guardian_first_name;
                    prnt.father_guardian_email = student.father_guardian_email;
                    prnt.father_guardian_occupation = student.father_guardian_occupation;
                    prnt.father_guardian_phone_no = student.father_guardian_phone_no;
                    prnt.mother_name = student.mother_name;
                    prnt.mother_email = student.mother_email;
                    prnt.mother_phone_no = student.mother_phone_no;
                    prnt.mother_occupation = student.mother_occupation;
                    prnt.ts_updated = DateTime.Now;

                    db.SaveChanges();
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    return false;
                }
            }
            return true;
        }


    }
}
