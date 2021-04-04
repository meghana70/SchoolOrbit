using System;
using System.Web.Mvc;
using SchoolOrbit.Models;
using SchoolOrbit.Filters;
using DomainModel.BusinessLayer;
namespace SchoolOrbit.Controllers
{
    [RequireHttps]    
    public class HomeController : ApplicationBaseController 
    {
        private SchoolOrbitEntities db = new SchoolOrbitEntities();

        
        [CustomAuthorize("Administrator", "Chairman")]
        public ActionResult Index()
        {            
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            return View();
        }
        [CustomAuthorize("Administrator", "Chairman")]
        public ActionResult GetMonthlyExpReportChart()
        {
            String strexpdata = "";
            int currentyear = DateTime.Now.Year;
            Expense exp = new Expense();
            strexpdata = exp.getMonthlyExpReport4Chart(UserDetails.Current.SchoolId, currentyear);
            return Content(strexpdata);
        }
        [CustomAuthorize("Administrator", "Chairman")]
        public ActionResult GetDashboardSubItems()
        {
            int mthCurrent = DateTime.Now.Month;
            int yrCurrent = DateTime.Now.Year;
            String strContent = "";
            Expense exp = new Expense();
            strContent = exp.getDashboardSubItems(UserDetails.Current.SchoolId, yrCurrent, mthCurrent);
            return Content(strContent);
        }
        [CustomAuthorize("Administrator", "Chairman")]
        public ActionResult GetMonthlyExpReport()
        {
            int yrCurrent = DateTime.Now.Year;
            String strContent = "";
            Expense exp = new Expense();
            strContent = exp.getMonthlyExpReport(UserDetails.Current.SchoolId, yrCurrent);
            return Content(strContent);            
        }
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            return View();
        }
        [CustomAuthorize("Parent")]
        public ActionResult ParentHome()
        {
            return View();
        }
    }
}