using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using SchoolOrbit.Models;
using System.IO;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using DomainModel.BusinessLayer;
namespace SchoolOrbit.Controllers
{
    public class EmployeeController : ApplicationBaseController 
    {
        private SchoolOrbitEntities db = new SchoolOrbitEntities();
        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        // GET: Employee
        public ActionResult Index()
        {
              DomainModel.BusinessLayer.Employee emp=new DomainModel.BusinessLayer.Employee();
              var qryEmp = emp.GetEmployees(UserDetails.Current.SchoolId);
              return View(qryEmp.ToList());            
        }

        public ActionResult chkEmployeeEmail(String sEmail,int? iSchId,int? iEmpId)
        {
            string strRes = "";

            DomainModel.BusinessLayer.Employee emp = new DomainModel.BusinessLayer.Employee();
            strRes = emp.checkEmployeeEmail(sEmail, iSchId, iEmpId);
            
            return Content(strRes);
        }
        void LoadDropDowns()
        {
            List<string> lstcountry = new List<string>();
            List<string> lststate = new List<string>();
            DomainModel.BusinessLayer.ListCollection LC = new DomainModel.BusinessLayer.ListCollection();
            lstcountry = LC.LoadCountry();
            lststate = LC.LoadState();
            ViewBag.CountryId = new SelectList(lstcountry);
            ViewBag.StatesId = new SelectList(lststate);
            //var list = db.sch_department.Where(d => d.idSchool == 1).ToList().Select(x => new { x.department_name, x.idDepartment });
            //ViewBag.DropDownAOOs = new SelectList(list, "idDepartment", "department_name");

            //ViewBag.CountryId = new SelectList(db.sys_country, "country_name", "country_name");
            //ViewBag.StatesId = new SelectList(db.sys_states, "state_name", "state_name");
            //List<SelectListItem> lstGender = new List<SelectListItem>()
            //{             
            //    new SelectListItem(){Value="M",Text="Male"},
            //    new SelectListItem(){Value="F",Text="Female"}
            //};
            var lstDept = LC.LoadDepartments(UserDetails.Current.SchoolId);
            ViewBag.DropDownAOOs = new SelectList(lstDept, "lstValue", "lstText");
            var lstGender = LC.LoadGender();
            ViewBag.Gender = new SelectList(lstGender, "lstValue", "lstText");
        }
        void LoadEditDropDowns(DomainModel.BusinessLayer.Employee emp)
        {
            List<string> lstcountry = new List<string>();
            List<string> lststate = new List<string>();
            DomainModel.BusinessLayer.ListCollection LC = new DomainModel.BusinessLayer.ListCollection();
            lstcountry = LC.LoadCountry();
            lststate = LC.LoadState();
            var lstDept = LC.LoadDepartments(UserDetails.Current.SchoolId);
            var lstGender = LC.LoadGender();

            ViewBag.DropDownAOOs = new SelectList(lstDept, "lstValue", "lstText",emp.idDepartment);
            ViewBag.PresentCountry = new SelectList(lstcountry, emp.present_country);
            ViewBag.PresentStates = new SelectList(lststate, emp.present_state);
            ViewBag.PermanentCountry = new SelectList(lstcountry, emp.permanent_country);
            ViewBag.PermanentStates = new SelectList(lststate,  emp.permanent_state);
            ViewBag.GenderCd = new SelectList(lstGender, "lstValue", "lstText", emp.gender);
        }
        // GET: Employee/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DomainModel.BusinessLayer.Employee emp = new DomainModel.BusinessLayer.Employee();
            var qryEmp = emp.GetEmployeeById(id, UserDetails.Current.SchoolId);
            return View(qryEmp);
        }

        // GET: Employee/Create
        public ActionResult Create()
        {

            LoadDropDowns();          
            return View();
        }

        // POST: Employee/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(HttpPostedFileBase addressproof, HttpPostedFileBase profile, [Bind(Include = "Id,first_name,middle_name,last_name,gender,DOB,screen_name,permanent_address,permanent_city,permanent_state,permanent_country,permanent_zip_code,present_address,present_city,present_state,present_country,present_zip_code,paediatrician_details,allergic,blood_group,Email,UserName,employee_number,idDepartment,department_name,designation,WorkStartDate,WorkEndDate,Qualification,PassingYear,University,BankAccountNo,BankName,IFSCCode,ESIPFNumber,PanNumber,AdharNumber,ManagerID,photo_url,mobilenumber,AlternateNumber")] DomainModel.BusinessLayer.Employee emp)
        {
            //var list = db.sch_department.Where(d => d.idSchool == 1).ToList().Select(x => new { x.department_name, x.idDepartment });           
            //ViewBag.DropDownAOOs = new SelectList(list, "idDepartment", "department_name");
            //ViewBag.CountryId = new SelectList(db.sys_country, "country_name", "country_name");
            //ViewBag.StatesId = new SelectList(db.sys_states, "state_name", "state_name");
            //List<SelectListItem> lstGender = new List<SelectListItem>()
            //        {             
            //            new SelectListItem(){Value="M",Text="Male"},
            //            new SelectListItem(){Value="F",Text="Female"}
            //        };
            //ViewBag.Gender = new SelectList(lstGender, "Value", "Text");  
            LoadDropDowns();
            //int Deptid = Convert.ToInt32(emp.department_name);
            try
            {
                try
                {
                    if (ModelState.IsValid)
                    {

                        HttpPostedFileBase addproof = Request.Files["addressproof"];
                        HttpPostedFileBase cv = Request.Files["profile"];
                        var fileName = "";
                        var cvfileName = "";
                        if (addproof.FileName != "")
                        {
                            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));
                            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                            CloudBlobContainer container = blobClient.GetContainerReference("employee");
                            string TempFolder = "/Uploads";
                            var serverPath = HttpContext.Server.MapPath(TempFolder);
                            if (Directory.Exists(serverPath) == false)
                            {
                                Directory.CreateDirectory(serverPath);
                            }
                            var fileext = Path.GetExtension(addproof.FileName);
                            fileName = String.Concat("ADP_", UserDetails.Current.Iduser, fileext);
                            var fullFileName = Path.Combine(serverPath, fileName);
                            addproof.SaveAs(fullFileName);
                            string addpblob = fileName;
                            CloudBlockBlob blockBlob = container.GetBlockBlobReference(addpblob);
                            using (var fileStream = System.IO.File.OpenRead(fullFileName))
                            {
                                blockBlob.UploadFromStream(fileStream);
                            }
                            System.IO.File.Delete(fullFileName);
                           
                        }
                        if (cv.FileName != "")
                        {
                            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));
                            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                            CloudBlobContainer container = blobClient.GetContainerReference("employee");
                            string TempFolder = "/Uploads";
                            var serverPath = HttpContext.Server.MapPath(TempFolder);
                            if (Directory.Exists(serverPath) == false)
                            {
                                Directory.CreateDirectory(serverPath);
                            }
                            var fileext = Path.GetExtension(cv.FileName);
                            cvfileName = String.Concat("PRO_", UserDetails.Current.Iduser, fileext);
                            var fullFileName = Path.Combine(serverPath, cvfileName);
                            addproof.SaveAs(fullFileName);
                            string pblob = cvfileName;
                            CloudBlockBlob blockBlob = container.GetBlockBlobReference(pblob);
                            using (var fileStream = System.IO.File.OpenRead(fullFileName))
                            {
                                blockBlob.UploadFromStream(fileStream);
                            }
                            System.IO.File.Delete(fullFileName);                            
                        }
                        DomainModel.BusinessLayer.Employee empl=new DomainModel.BusinessLayer.Employee();
                        Boolean blnFlg=empl.addEmployee(emp,UserDetails.Current.SchoolId,fileName,cvfileName);
                        if(blnFlg == true){
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            return View();
                        }

                    }
                    return View();
                }
                catch
                {
                    return View();
                }

            }
            catch
            {
                return View();
            }
        }

        // GET: Employee/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //var qryemp = (from emp in db.sch_employee
            //           join ua in db.sys_user on emp.idUser equals ua.Id
            //           join usr in db.AspNetUsers on ua.Id equals usr.IdUser                       
            //           where emp.idSchool == 1 & emp.idUser == id
            //           select new DomainModel.BusinessLayer.Employee()
            //           {
            //               Id = emp.idUser,
            //               employee_number = emp.employee_number,
            //               first_name = ua.first_name,
            //               last_name = ua.last_name,
            //               DOB = ua.DOB,
            //               photo_url = ua.photo_url,
            //               screen_name = ua.screen_name,
            //               allergic = ua.allergic,
            //               blood_group = ua.blood_group,
            //               gender = ua.gender,
            //               middle_name = ua.middle_name,                           
            //               permanent_address = ua.permanent_address,
            //               permanent_city = ua.permanent_city,
            //               permanent_country = ua.permanent_country,
            //               permanent_state = ua.permanent_state,
            //               permanent_zip_code = ua.permanent_zip_code,
            //               present_address = ua.present_address,
            //               present_city = ua.present_city,
            //               present_country = ua.present_country,
            //               present_state = ua.present_state,
            //               present_zip_code = ua.present_zip_code,
            //               UserName = usr.UserName,
            //               Email = usr.Email,              
            //               department_name=emp.sch_department.department_name,
            //               designation =emp.designation,
            //               WorkStartDate= emp.WorkStartDate,
            //               WorkEndDate =emp.WorkEndDate,
            //               Qualification =emp.Qualification,
                          
            //               University =emp.University,
            //               BankAccountNo = emp.BankAccountNo,
            //               BankName =emp.BankName,
            //               IFSCCode = emp.IFSCCode,
            //               ESIPFNumber =emp.ESIPFNumber,
            //               PanNumber =emp.PanNumber, 
            //               AdharNumber =emp.AdharNumber,
            //               addressproof_path = emp.addressproof_path ,
            //               cv_path = emp.cv_path,
            //               AlternateNumber = emp.AlternateNumber,
            //               mobilenumber = emp.mobilenumber,
            //               ManagerID =emp.ManagerID 
            //           }).SingleOrDefault();
            DomainModel.BusinessLayer.Employee emp = new DomainModel.BusinessLayer.Employee();
            //var list = db.sch_department.Where(d => d.idSchool == 1).ToList().Select(x => new { x.department_name, x.idDepartment });
            //ViewBag.DropDownAOOs = new SelectList(list, "idDepartment", "department_name");
            //ViewBag.PresentCountry = new SelectList(db.sys_country, "country_name", "country_name", qryemp.present_country);
            //ViewBag.PresentStates = new SelectList(db.sys_states, "state_name", "state_name", qryemp.present_state);
            //ViewBag.PermanentCountry = new SelectList(db.sys_country, "country_name", "country_name", qryemp.permanent_country);
            //ViewBag.PermanentStates = new SelectList(db.sys_states, "state_name", "state_name", qryemp.permanent_state);
            //List<SelectListItem> lstGender = new List<SelectListItem>()
            //{             
            //    new SelectListItem(){Value="M",Text="Male"},
            //    new SelectListItem(){Value="F",Text="Female"}
            //};
            //ViewBag.Gender = new SelectList(lstGender, "Value", "Text", qryemp.gender);
            var qryemp = emp.GetEmployeeById(id,UserDetails.Current.SchoolId);
            LoadEditDropDowns(qryemp);
            return View(qryemp);
        }

        // POST: Employee/Edit/5
        [HttpPost]
        public ActionResult Edit(HttpPostedFileBase addressproof, HttpPostedFileBase profile, [Bind(Include = "Id,first_name,middle_name,last_name,gender,DOB,screen_name,permanent_address,permanent_city,permanent_state,permanent_country,permanent_zip_code,present_address,present_city,present_state,present_country,present_zip_code,paediatrician_details,allergic,blood_group,Email,UserName,employee_number,idDepartment,department_name,designation,WorkStartDate,WorkEndDate,Qualification,Course,PassingYear,University,BankAccountNo,BankName,IFSCCode,ESIPFNumber,PanNumber,AdharNumber,Signature,Resume,ManagerID,photo_url,mobilenumber,AlternateNumber")] DomainModel.BusinessLayer.Employee emp)
        {
            try
            {
                //var list = db.sch_department.Where(d => d.idSchool == 1).ToList().Select(x => new { x.department_name, x.idDepartment });
                //List<SelectListItem> lstGender = new List<SelectListItem>()
                //{             
                //    new SelectListItem(){Value="M",Text="Male"},
                //    new SelectListItem(){Value="F",Text="Female"}
                //};
                //ViewBag.DropDownAOOs = new SelectList(list, "idDepartment", "department_name", emp.idDepartment);
                //ViewBag.PresentCountry = new SelectList(db.sys_country, "country_name", "country_name", emp.present_country);
                //ViewBag.PresentStates = new SelectList(db.sys_states, "state_name", "state_name", emp.present_state);
                //ViewBag.PermanentCountry = new SelectList(db.sys_country, "country_name", "country_name", emp.permanent_country);
                //ViewBag.PermanentStates = new SelectList(db.sys_states, "state_name", "state_name", emp.permanent_state);
                //ViewBag.Gender = new SelectList(lstGender, "Value", "Text", emp.gender);  
                DomainModel.BusinessLayer.Employee empl = new DomainModel.BusinessLayer.Employee();
                var qryemp = empl.GetEmployeeById(emp.Id,UserDetails.Current.SchoolId);
                LoadEditDropDowns(qryemp);
                if (ModelState.IsValid)
                {
                    HttpPostedFileBase addproof = Request.Files["addressproof"];
                    HttpPostedFileBase cv = Request.Files["profile"];
                    var fileName ="";
                    var cvfileName ="";
                    if (addproof.FileName != "")
                    {
                        CloudStorageAccount storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));
                        CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                        CloudBlobContainer container = blobClient.GetContainerReference("employee");
                        string TempFolder = "/Uploads";
                        var serverPath = HttpContext.Server.MapPath(TempFolder);
                        if (Directory.Exists(serverPath) == false)
                        {
                            Directory.CreateDirectory(serverPath);
                        }
                        var fileext = Path.GetExtension(addproof.FileName);
                        fileName = String.Concat("ADP_", emp.idUser, fileext);
                        var fullFileName = Path.Combine(serverPath, fileName);
                        addproof.SaveAs(fullFileName);
                        string addpblob = fileName;
                        CloudBlockBlob blockBlob = container.GetBlockBlobReference(addpblob);
                        using (var fileStream = System.IO.File.OpenRead(fullFileName))
                        {
                            blockBlob.UploadFromStream(fileStream);
                        }
                        System.IO.File.Delete(fullFileName);                        
                    }
                    if (cv.FileName != "")
                    {
                        CloudStorageAccount storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));
                        CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                        CloudBlobContainer container = blobClient.GetContainerReference("employee");
                        string TempFolder = "/Uploads";
                        var serverPath = HttpContext.Server.MapPath(TempFolder);
                        if (Directory.Exists(serverPath) == false)
                        {
                            Directory.CreateDirectory(serverPath);
                        }
                        var fileext = Path.GetExtension(cv.FileName);
                        cvfileName = String.Concat("PRO_", emp.idUser, fileext);
                        var fullFileName = Path.Combine(serverPath, cvfileName);
                        addproof.SaveAs(fullFileName);
                        string pblob = cvfileName;
                        CloudBlockBlob blockBlob = container.GetBlockBlobReference(pblob);
                        using (var fileStream = System.IO.File.OpenRead(fullFileName))
                        {
                            blockBlob.UploadFromStream(fileStream);
                        }
                        System.IO.File.Delete(fullFileName);                     
                    }
                    Boolean blnFlg = empl.updateEmp(emp,UserDetails.Current.SchoolId,fileName,cvfileName);
                    if (blnFlg == true) {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        return View();
                    }
                    
                }               
                             
                return View();
            }
            catch
            {
                return View();
            }
        }

        // GET: Employee/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //var qryemp = (from emp in db.sch_employee
            //              join ua in db.sys_user on emp.idUser equals ua.Id
            //              join usr in db.AspNetUsers on ua.Id equals usr.IdUser
            //              where emp.idSchool == 1 & emp.idUser == id
            //              select new DomainModel.BusinessLayer.Employee()
            //              {
            //                  Id = emp.idUser,
            //                  employee_number = emp.employee_number,
            //                  first_name = ua.first_name,
            //                  last_name = ua.last_name,
            //                  DOB = ua.DOB,
            //                  photo_url = ua.photo_url,
            //                  screen_name = ua.screen_name,
            //                  allergic = ua.allergic,
            //                  blood_group = ua.blood_group,
            //                  gender = ua.gender,
            //                  middle_name = ua.middle_name,
            //                  permanent_address = ua.permanent_address,
            //                  permanent_city = ua.permanent_city,
            //                  permanent_country = ua.permanent_country,
            //                  permanent_state = ua.permanent_state,
            //                  permanent_zip_code = ua.permanent_zip_code,
            //                  present_address = ua.present_address,
            //                  present_city = ua.present_city,
            //                  present_country = ua.present_country,
            //                  present_state = ua.present_state,
            //                  present_zip_code = ua.present_zip_code,
            //                  UserName = usr.UserName,
            //                  Email = usr.Email,
            //                  department_name = emp.sch_department.department_name,
            //                  designation = emp.designation,
            //                  WorkStartDate = emp.WorkStartDate,
            //                  WorkEndDate = emp.WorkEndDate,
            //                  Qualification = emp.Qualification,
                              
            //                  University = emp.University,
            //                  BankAccountNo = emp.BankAccountNo,
            //                  BankName = emp.BankName,
            //                  IFSCCode = emp.IFSCCode,
            //                  ESIPFNumber = emp.ESIPFNumber,
            //                  PanNumber = emp.PanNumber,
            //                  AdharNumber = emp.AdharNumber,
                              
            //                  ManagerID = emp.ManagerID
            //              }).SingleOrDefault();
            DomainModel.BusinessLayer.Employee emp = new DomainModel.BusinessLayer.Employee();
            var qryemp = emp.GetEmployeeById(id, UserDetails.Current.SchoolId);
            return View(qryemp);
        }

        // POST: Employee/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int? id, FormCollection collection)
        {
            try
            {
                DomainModel.BusinessLayer.Employee emp = new DomainModel.BusinessLayer.Employee();
                Boolean blnFlg = emp.deleteEmp((int)id);
                if (blnFlg == true)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return View();
                }                
            }
            catch
            {
                return View();
            }
        }
    }
}
