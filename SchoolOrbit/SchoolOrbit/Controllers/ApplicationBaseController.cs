using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SchoolOrbit.Models;
using System.Web.Routing;

namespace SchoolOrbit.Controllers
{
    public class ApplicationBaseController : Controller
    {
        private SchoolOrbitEntities db = new SchoolOrbitEntities();
        // GET: ApplicationBase
        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (User != null)
            {
                                
                var username = User.Identity.Name;

                if (!string.IsNullOrEmpty(username))
                {
                    if (Session["userDetails"] != null)
                    {
                        //var userDetails = (UserDetails)HttpContext.Session["userDetails"];
                        ViewData.Add("profilepic", UserDetails.Current.photo_url);
                        ViewData.Add("Displayname", UserDetails.Current.screen_name);
                        ViewData.Add("IdUser", UserDetails.Current.Iduser);
                        ViewData.Add("SchoolID", UserDetails.Current.SchoolId);
                    }
                    //else {
                    //var user = db.AspNetUsers.SingleOrDefault(u => u.UserName == username);
                    //string Displayname = user.Displayname;
                    //int IdUser =(int) user.IdUser;
                    //var profilepic = db.sys_user.SingleOrDefault(x => x.Id  == IdUser);
                    //ViewData.Add("profilepic", profilepic.photo_url);
                    //int SchoolID = UserDetails.Current.SchoolId;
                    ////int Userid = user.IdUser; 
                    //ViewData.Add("Displayname", Displayname);
                    //ViewData.Add("IdUser", IdUser);
                    //ViewData.Add("SchoolID", SchoolID);
                 
                    //    UserDetails usr = new UserDetails();
                    //    usr.Iduser = IdUser;
                    //    usr.photo_url = profilepic.photo_url;
                    //    usr.SchoolId = SchoolID;
                    //    usr.screen_name = user.Displayname;
                        
                    //    Session["userDetails"] = usr;
                    //}
                         
                }               
            }
            base.OnActionExecuted(filterContext);
        }

    }
}