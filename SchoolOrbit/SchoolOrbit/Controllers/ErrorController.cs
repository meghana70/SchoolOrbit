using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SchoolOrbit.Controllers
{
    public class ErrorController : ApplicationBaseController 
    {
        // GET: Error
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult HandleError()
        {
            return View("HandleError");
        }

        public ActionResult UnAuthorized()
        {            
            return View();
        }
    }
}