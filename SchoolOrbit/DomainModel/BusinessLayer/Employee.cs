using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.BusinessLayer
{

    public class Employee
    {
        private DataModel.SOEntities db = new DataModel.SOEntities();
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
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
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
        public List<Employee> GetEmployees(int SchoolId)
        {
            var qryEmp = from emp in db.sch_employee
                        join ua in db.sys_user on emp.idUser equals ua.Id
                        where emp.idSchool == SchoolId
                        select new Employee()
                        {
                            employee_number = emp.employee_number,
                            first_name = ua.first_name,
                            last_name = ua.last_name,
                            photo_url = ua.photo_url,
                            gender = ua.gender,
                            DOB = ua.DOB,
                            Id = emp.idUser,
                            department_name = emp.sch_department.department_name,
                            designation = emp.designation
                        };
            return qryEmp.ToList();
        }
        public Employee GetEmployeeById(int? id,int SchoolId)
        {
            var qryemp = (from emp in db.sch_employee
                          join ua in db.sys_user on emp.idUser equals ua.Id
                          from usr in db.AspNetUsers 
                           .Where(w=>w.IdUser == ua.Id).DefaultIfEmpty()                          
                          where emp.idSchool == SchoolId & emp.idUser == id
                          select new DomainModel.BusinessLayer.Employee()
                          {
                              Id = emp.idUser,
                              employee_number = emp.employee_number,
                              first_name = ua.first_name,
                              last_name = ua.last_name,
                              DOB = ua.DOB,
                              photo_url = ua.photo_url,
                              screen_name = ua.screen_name,
                              allergic = ua.allergic,
                              blood_group = ua.blood_group,
                              gender = ua.gender,
                              middle_name = ua.middle_name,
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
                              UserName = usr.UserName,
                              Email = usr.Email,
                              department_name = emp.sch_department.department_name,
                              designation = emp.designation,
                              WorkStartDate = emp.WorkStartDate,
                              WorkEndDate = emp.WorkEndDate,
                              Qualification = emp.Qualification,
                              AlternateNumber = emp.AlternateNumber,
                              mobilenumber = emp.mobilenumber,
                              cv_path = emp.cv_path,
                              addressproof_path = emp.addressproof_path,
                              University = emp.University,
                              BankAccountNo = emp.BankAccountNo,
                              BankName = emp.BankName,
                              IFSCCode = emp.IFSCCode,
                              ESIPFNumber = emp.ESIPFNumber,
                              PanNumber = emp.PanNumber,
                              AdharNumber = emp.AdharNumber,
                              PassingYear =emp.PassingYear,
                              idDepartment =(int) emp.idDepartment,
                              ManagerID = emp.ManagerID
                          }).SingleOrDefault();
            return qryemp;
        }
        public Boolean addEmployee(Employee emp,int SchoolId, string addrproof, string cvpath)
        {
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    int idUser = 0;
                    int userexist = 0;
                    string strEmpEmail = "";
                    string strUserName = "";
                    string strDisplayName = "";
                    if (emp.Email != null)
                    {
                        var qryuser = db.AspNetUsers.Where(x => x.Email == emp.Email).FirstOrDefault();
                        if (qryuser != null)
                        {
                            userexist = Convert.ToInt32(qryuser.IdUser);
                        }
                        strEmpEmail = emp.Email;
                    }
                    //else
                    //{
                    //    strEmpEmail = emp.first_name.ToLower() + "." + emp.last_name.ToLower() + "@schoolorbit.com";
                    //}
                    if (userexist > 0)
                    {
                        idUser = userexist;
                    }
                    else
                    {
                        strUserName = emp.first_name.Substring(0, 1).ToLower() + "" + emp.last_name.Substring(0, 1).ToLower();                                                
                        if (emp.screen_name != null)
                        {
                            strDisplayName = emp.screen_name;
                        }
                        else
                        {
                            strDisplayName = emp.first_name + " " + emp.last_name;
                        }
                        //var user = new ApplicationUser { UserName = strUserName, Email = strEmpEmail, Displayname = strDisplayName };
                        //var result = await UserManager.CreateAsync(user, "Employee@123");
                      
                        //if (result.Succeeded)
                        //{
                        //    var qryUser = db.AspNetUsers.Find(user.Id);
                        //    idUser = qryUser.IdUser;
                        //    qryUser.UserName = qryUser.UserName + (idUser + 1000);

                            // insert into UserAccount table
                            DataModel.sys_user useraccnt = new DataModel.sys_user();
                            useraccnt.active = true;
                            useraccnt.allergic = emp.allergic;
                            useraccnt.blood_group = emp.blood_group;
                            useraccnt.DOB = emp.DOB;
                            useraccnt.first_name = emp.first_name;
                            useraccnt.gender = emp.gender;
                           // useraccnt.Id = idUser;
                            useraccnt.last_name = emp.last_name;
                            useraccnt.middle_name = emp.middle_name;
                            useraccnt.permanent_address = emp.permanent_address;
                            useraccnt.permanent_city = emp.permanent_city;
                            useraccnt.permanent_country = emp.permanent_country;
                            useraccnt.permanent_state = emp.permanent_state;
                            useraccnt.permanent_zip_code = emp.permanent_zip_code;
                            useraccnt.photo_url = "/pictures/default-avatar.png";
                            useraccnt.present_address = emp.present_address;
                            useraccnt.present_city = emp.present_city;
                            useraccnt.present_country = emp.present_country;
                            useraccnt.present_state = emp.present_state;
                            useraccnt.present_zip_code = emp.present_zip_code;
                            useraccnt.screen_name = strDisplayName;
                            useraccnt.aadhaar_number = emp.AdharNumber;
                            useraccnt.contact_number = emp.mobilenumber;
                            useraccnt.contact_number_alternate = emp.AlternateNumber;
                            db.sys_user.Add(useraccnt);
                            db.SaveChanges();

                            if (strEmpEmail != "")// this condition has been added to check if Employee has a valid email to create user login.
                            {
                                UserLogin Login = new UserLogin();
                                strUserName = strUserName + useraccnt.Id;
                                var res = Login.CreateUser(useraccnt.Id, UserLogin.LoginUserType.employee, strEmpEmail, strUserName);
                            }                            
                            
                            DataModel.sch_employee Employ = new DataModel.sch_employee();
                           
                            Employ.idDepartment = Convert.ToInt32(emp.department_name);
                            Employ.BankAccountNo = emp.BankAccountNo;
                            Employ.BankName = emp.BankName;

                            Employ.designation = emp.designation;
                            Employ.employee_number = emp.employee_number;
                            Employ.ESIPFNumber = emp.ESIPFNumber;
                            Employ.IFSCCode = emp.IFSCCode;
                            Employ.ManagerID = emp.ManagerID;
                            Employ.PanNumber = emp.PanNumber;
                            Employ.PassingYear = emp.PassingYear;
                            Employ.Qualification = emp.Qualification;
                            //Employ.AdharNumber = emp.AdharNumber;
                            //Employ.mobilenumber = emp.mobilenumber;
                            //Employ.AlternateNumber = emp.AlternateNumber;
                            Employ.University = emp.University;
                            Employ.WorkEndDate = emp.WorkEndDate;
                            Employ.WorkStartDate = emp.WorkStartDate;
                            Employ.is_active = true;
                            // End
                            // School Id should come Roles 
                            Employ.idSchool =SchoolId;
                            // end
                            Employ.idUser = useraccnt.Id;

                            if (addrproof != "")
                            {
                               
                                Employ.addressproof_path = addrproof;
                            }
                            if (cvpath != "")
                            {                                
                                Employ.cv_path = cvpath;
                            }
                            db.sch_employee.Add(Employ);
                            db.SaveChanges();
                        }
                  //  }
                    transaction.Commit();
                    return true;
                }
                catch
                {
                    transaction.Rollback();
                    return false;
                }

            }
        }
        public Boolean updateEmp(Employee emp,int idSchool,string addrproof,string cvpath)
        {
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    
                   // user.UserName = emp.UserName;

                    var sysuser = db.sys_user.Where(y => y.Id == emp.Id).FirstOrDefault();
                    sysuser.active = true;
                    sysuser.allergic = emp.allergic;
                    sysuser.blood_group = emp.blood_group;
                    sysuser.DOB = emp.DOB;
                    sysuser.first_name = emp.first_name;
                    sysuser.gender = emp.gender;
                    sysuser.last_name = emp.last_name;
                    sysuser.middle_name = emp.middle_name;
                    sysuser.permanent_address = emp.permanent_address;
                    sysuser.permanent_city = emp.permanent_city;
                    sysuser.permanent_country = emp.permanent_country;
                    sysuser.permanent_state = emp.permanent_state;
                    sysuser.permanent_zip_code = emp.permanent_zip_code;
                    sysuser.photo_url = emp.photo_url;
                    sysuser.present_address = emp.present_address;
                    sysuser.present_city = emp.present_city;
                    sysuser.present_country = emp.present_country;
                    sysuser.present_state = emp.present_state;
                    sysuser.present_zip_code = emp.present_zip_code;
                    sysuser.aadhaar_number = emp.AdharNumber;
                    sysuser.contact_number = emp.mobilenumber;
                    sysuser.contact_number_alternate = emp.AlternateNumber;

                    var user = db.AspNetUsers.Where(x => x.IdUser == emp.Id).FirstOrDefault();
                    if (user != null)
                    {
                        user.Email = emp.Email;
                    }
                    else
                    {
                        if (emp.Email != "")// this condition has been added to check if Employee has a valid email to create user login.
                        {
                            UserLogin Login = new UserLogin();
                            String strUserName = emp.first_name.Substring(0, 1).ToLower() + "" + emp.last_name.Substring(0, 1).ToLower();
                            strUserName = strUserName + sysuser.Id;
                            var res = Login.CreateUser(sysuser.Id, UserLogin.LoginUserType.employee, emp.Email, strUserName);
                        }
                    }                    

                    var employee = db.sch_employee.Where(z => z.idUser == emp.Id & z.idSchool == idSchool).FirstOrDefault();
                   
                    employee.idDepartment = Convert.ToInt32(emp.department_name);
                    employee.BankAccountNo = emp.BankAccountNo;
                    employee.BankName = emp.BankName;
                    //employee.AdharNumber = emp.AdharNumber;
                    //employee.AlternateNumber = emp.AlternateNumber;
                    //employee.mobilenumber = emp.mobilenumber;
                    employee.designation = emp.designation;
                    employee.employee_number = emp.employee_number;
                    employee.ESIPFNumber = emp.ESIPFNumber;
                    employee.IFSCCode = emp.IFSCCode;
                    employee.ManagerID = emp.ManagerID;
                    employee.PanNumber = emp.PanNumber;
                    employee.PassingYear = emp.PassingYear;
                    employee.Qualification = emp.Qualification;
                    employee.University = emp.University;
                    employee.WorkEndDate = emp.WorkEndDate;
                    employee.WorkStartDate = emp.WorkStartDate;

                    if (addrproof != "")
                    {
                        employee.addressproof_path = addrproof;
                    }
                    if (cvpath != "")
                    {
                        employee.cv_path = cvpath;
                    }
                    db.SaveChanges();
                    transaction.Commit();
                    return true;
                }
                catch
                {
                    transaction.Rollback();
                    return false;
                }
            }    
        }
        public Boolean deleteEmp(int id)
        {
            try
            {
                var schemployee = db.sch_employee.Where(x => x.idUser == id).FirstOrDefault();
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
       public String checkEmployeeEmail(String sEmail,int? iSchId,int? iEmpId){
          String strRes = "";
          try{
              if (iEmpId != null)
                {
                    var qry = (from usr in db.AspNetUsers 
                               join emp in db.sch_employee on usr.IdUser equals emp.idUser 
                               where usr.Email == sEmail && usr.IdUser != iEmpId && emp.idSchool == iSchId 
                               select usr.Displayname).ToList();
                    if (qry.Count > 0)
                    {
                        strRes = "<font color='red'>Duplicate Email Address</font>";
                    }
                }
                else
                {
                    var qry = (from usr in db.AspNetUsers
                               join emp in db.sch_employee on usr.IdUser equals emp.idUser
                               where usr.Email == sEmail && emp.idSchool == iSchId
                               select usr.Displayname).ToList();
                    if (qry.Count > 0)
                    {
                        strRes = "<font color='red'>Duplicate Email Address</font>";
                    }
                }

            } catch(Exception ex){
                throw ex;
           }
          return strRes;
        }
    }
}
