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
using SchoolOrbit.Models;
using SchoolOrbit.Filters;

namespace SchoolOrbit.Controllers
{
    [RequireHttps]
    [CustomAuthorize("Administrator", "Chairman")]
    public class StudentController : ApplicationBaseController
    {
        // GET: Student
        public ActionResult Index()
        {
            DomainModel.BusinessLayer.Student std = new DomainModel.BusinessLayer.Student();
            var qrystd = std.getstudents(UserDetails.Current.SchoolId);                        
            return View(qrystd.ToList());           
        }
        public ActionResult chkAdmnNo(int admn_no, int sch_id,int? std_id)
        {
            string strRes = string.Empty;
            DomainModel.BusinessLayer.Student sa = new DomainModel.BusinessLayer.Student();
            int qry = sa.CheckAdmissionNumber(admn_no, sch_id, std_id);
            if (qry > 0)
            {
                strRes = "<font color='red'>Duplicate Admission Number</font>";
            }       
           
            return Content(strRes);
        }
        public ActionResult getParentDetails(string phone_no)
        {
            string strRes = "";
            DomainModel.BusinessLayer.Student sa = new DomainModel.BusinessLayer.Student();
            strRes = sa.GetParentDetails(phone_no);
            return Content(strRes);
        }
        // GET: Student/Details/5
        public ActionResult Details(int? id)
        {
           if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }            
            DomainModel.BusinessLayer.Student studentfactory = new DomainModel.BusinessLayer.Student();
            var stu = studentfactory.GetStudentdetails(Convert.ToInt32(id), UserDetails.Current.SchoolId);         
            return View(stu);            
        }

        void LoadDropdowns()
        {
            List<string> country = new List<string>();
            List<string> state = new List<string>();
            DomainModel.BusinessLayer.ListCollection LC = new DomainModel.BusinessLayer.ListCollection();
            country = LC.LoadCountry();
            state = LC.LoadState();
            ViewBag.CountryId = new SelectList(country);
            ViewBag.StatesId = new SelectList(state);
            //List<SelectListItem> lstGender = new List<SelectListItem>()
            //{             
            //    new SelectListItem(){Value="M",Text="Male"},
            //    new SelectListItem(){Value="F",Text="Female"}
            //};
            //ViewBag.Gender = new SelectList(lstGender, "Value", "Text");
            var lstGender = LC.LoadGender();
            ViewBag.Gender = new SelectList(lstGender, "lstValue", "lstText");
        }
        void LoadEditDropdowns(DomainModel.BusinessLayer.Student std)
        {
            List<string> lstcountry = new List<string>();
            List<string> lststate = new List<string>();
            DomainModel.BusinessLayer.ListCollection LC = new DomainModel.BusinessLayer.ListCollection();
            lstcountry = LC.LoadCountry();
            lststate = LC.LoadState();
            ViewBag.PresentCountry = new SelectList(lstcountry, std.present_country);
            ViewBag.PresentStates = new SelectList(lststate, std.present_state);
            ViewBag.PermanentCountry = new SelectList(lstcountry, std.permanent_country);
            ViewBag.PermanentStates = new SelectList(lststate, std.permanent_state);         
            var lstGender = LC.LoadGender();
            ViewBag.GenderCd = new SelectList(lstGender, "lstValue", "lstText",std.gender);
        }
        // GET: Student/Create
        public ActionResult Create()
        {
            LoadDropdowns();
            return View();
        }

        // POST: Student/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,first_name,middle_name,last_name,gender,DOB,screen_name,permanent_address,permanent_city,permanent_state,permanent_country,permanent_zip_code,present_address,present_city,present_state,present_country,present_zip_code,paediatrician_details,allergic,blood_group,Email,UserName,admission_number,admission_date,transfer_date,father_guardian_phone_no,father_guardian_first_name,father_guardian_last_name,father_guardian_email,father_guardian_occupation,mother_name,mother_phone_no,mother_email,mother_occupation")] DomainModel.BusinessLayer.Student  student)
        {
            LoadDropdowns();
            try
            {               
                if (ModelState.IsValid)
                {
                    DomainModel.BusinessLayer.Student stu = new DomainModel.BusinessLayer.Student();                   
                    Boolean blnFlg = stu.Addstudent(student,UserDetails.Current.SchoolId);
                    if (blnFlg == true)
                    {
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

        // GET: Student/Edit/5
        public ActionResult Edit(int? id)
        {
           
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DomainModel.BusinessLayer.Student studentfactory = new DomainModel.BusinessLayer.Student();
            var qrystd = studentfactory.GetStudentdetails(Convert.ToInt32(id), UserDetails.Current.SchoolId);
            LoadEditDropdowns(qrystd);
            return View(qrystd);  
           
        }

        // POST: Student/Edit/5
        [HttpPost]

        public ActionResult Edit([Bind(Include = "Id,first_name,middle_name,last_name,gender,DOB,screen_name,permanent_address,permanent_city,permanent_state,permanent_country,permanent_zip_code,present_address,present_city,present_state,present_country,present_zip_code,paediatrician_details,allergic,blood_group,Email,UserName,admission_number,admission_date,transfer_date,father_guardian_phone_no,father_guardian_first_name,father_guardian_last_name,father_guardian_email,father_guardian_occupation,mother_name,mother_phone_no,mother_email,mother_occupation,photo_url")] DomainModel.BusinessLayer.Student student)
        {
            try
            {
                DomainModel.BusinessLayer.Student std = new DomainModel.BusinessLayer.Student();
                var qrystd = std.GetStudentdetails(student.Id, UserDetails.Current.SchoolId);
                LoadEditDropdowns(qrystd);
                
                if (ModelState.IsValid)
                {
                 
                   Boolean  blnFlg = std.SaveStudent(student,UserDetails.Current.SchoolId);
                    if (blnFlg == true)
                    {
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
                   
                

                


    //    // GET: Student/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DomainModel.BusinessLayer.Student studentfactory = new DomainModel.BusinessLayer.Student();
            var qrystd = studentfactory.GetStudentdetails(Convert.ToInt32(id), UserDetails.Current.SchoolId );
            return View(qrystd);
        }

        // POST: Student/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int? id, FormCollection collection)
        {
            try
            {
                DomainModel.BusinessLayer.Student std = new DomainModel.BusinessLayer.Student();
                Boolean blnFlg = std.deleteStudent((int)id);
                if (blnFlg == true)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return View();
                };
            }
            catch
            {
                return View();
            }
        }
    }
}
