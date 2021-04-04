using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using SchoolOrbit.Models;
using Microsoft.AspNet.Identity;
using System.Linq;
using System;

namespace SchoolOrbit.Filters
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        private SchoolOrbitEntities db = new SchoolOrbitEntities();

        private readonly string[] allowedroles;
        public CustomAuthorizeAttribute(params string[] roles)
        {
            this.allowedroles = roles;
        }
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (HttpContext.Current.User.Identity.IsAuthenticated)
            {
                IsSessionExpired();
                bool authorize = false;
                foreach (var role in allowedroles)
                {
                    if (role == UserDetails.Current.role_name)
                    {
                        authorize = true;
                    }

                }
                return authorize;
            }
            return true;
        }
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectToRouteResult(
                                   new RouteValueDictionary
                                   {
                                       { "action", "UnAuthorized" },
                                       { "controller", "Error" }
                                   });

        }


        public int getUserDetails(string username)
        {
            var user = db.AspNetUsers.SingleOrDefault(u => u.UserName == username);
            string Displayname = user.Displayname;
            int IdUser = (int)user.IdUser;
            return IdUser;
        }

        public void getProfileInfo()
        {
            int userId = getUserDetails(HttpContext.Current.User.Identity.GetUserName());
            DomainModel.BusinessLayer.AuthorizeService _authorizeService = new DomainModel.BusinessLayer.AuthorizeService(userId);
            UserDetails usr = new UserDetails();
            usr.Iduser = userId;
            usr.photo_url = _authorizeService.profilePic;
            usr.SchoolId = _authorizeService.idschool;
            usr.screen_name = _authorizeService.displayname;
            usr.role_name = _authorizeService.role_name;
            HttpContext.Current.Session["userDetails"] = usr;
        }


        private bool IsSessionExpired()
        {
            if (HttpContext.Current.Session["userDetails"] == null)
            {
                getProfileInfo();
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}  

