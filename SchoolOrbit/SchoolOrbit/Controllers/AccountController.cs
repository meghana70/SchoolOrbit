using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using SchoolOrbit.Models;
using System.Web.Routing;
using System.Net;
using DomainModel.BusinessLayer;
namespace SchoolOrbit.Controllers
{
    [RequireHttps]
    [Authorize]
    public class AccountController : ApplicationBaseController 
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private SchoolOrbitEntities db = new SchoolOrbitEntities();

        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager )
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set 
            { 
                _signInManager = value; 
            }
        }

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
        public ActionResult ViewChangePassword(int idUser)
        {
            AuthorizeService usr=new AuthorizeService();
            ViewBag.UserId = idUser;
            ViewBag.UserName = usr.getUserName(idUser);
            return PartialView("_ChangePassword");
        }
        public ActionResult ViewCreateLogin(int idUser)
        {
            AuthorizeService usr = new AuthorizeService();
            ViewBag.UserId = idUser;
            ViewBag.UserName = usr.getUserName(idUser);
            return PartialView("_CreateLogin");
        }
        public ActionResult ChangePassword(int idUser)
        {
            var strRes = "";
            UserLogin Login = new UserLogin();
            strRes = Login.ChangePassword(idUser);
            return Content(strRes);
        }
        public ActionResult CreateLogin(int idUser, string strEmail)
        {
            var strRes = "";
            UserLogin Login = new UserLogin();            
            Boolean blnRes = Login.CreateUser(idUser, UserLogin.LoginUserType.employee, strEmail, strEmail);
            if (blnRes)
            {
                DomainModel.BusinessLayer.Employee emp = new DomainModel.BusinessLayer.Employee();
                var empl = emp.GetEmployeeById(idUser, UserDetails.Current.SchoolId);
                if (empl != null)
                {
                    strRes = "1";
                }
                else
                {
                    strRes = "2";
                }
            }
            return Content(strRes);
        }
        public JsonResult GetUsers(string term)
        {
            UserRoles usrRoles = new UserRoles();
            var qryEmp = usrRoles.GetEmployees(UserDetails.Current.SchoolId,term);
            return Json(qryEmp, JsonRequestBehavior.AllowGet);
        }
        public ActionResult UserRoles()
        {
            UserRoles usrRoles = new UserRoles();
            Roles roles = new Roles();
            var qryRoles = roles.GetRoles();
            ViewBag.Roles = new SelectList(qryRoles, "role_name", "role_name");
            var qryUsrRoles = usrRoles.GetUserRoles(UserDetails.Current.SchoolId);
            //var x = (from o in qryUsrRoles
            //         select new UserRoles
            //         {
            //             displayname = o.displayname,
            //             role_name = o.role_name,
            //             id = o.id
            //         });
            return View(qryUsrRoles.ToList());
        }

        //GET: /Account/CreateUserRoles
        public ActionResult CreateUserRoles()
        {
            Roles roles = new Roles();
            var qryRoles = roles.GetRoles();
            ViewBag.Roles = new SelectList(qryRoles, "id", "role_name");
            //var lstEmp = (from usr in db.AspNetUsers
            //              join emp in db.sch_employee on usr.IdUser equals emp.idUser
            //              where emp.idSchool == UserDetails.Current.SchoolId
            //              orderby usr.Displayname
            //              select new
            //              {
            //                  name = usr.Displayname,
            //                  id = usr.IdUser
            //              }).ToList();
            //UserRoles usrRoles = new UserRoles();
            //var qryEmp = usrRoles.GetEmployees(UserDetails.Current.SchoolId);
            //ViewBag.Users = new SelectList(qryEmp, "Id", "screen_name");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateUserRoles([Bind(Include = "idRole,idUser")] UserRoles UsrRoles)
        {
            Roles roles = new Roles();
            var qryRoles = roles.GetRoles();
            ViewBag.Roles = new SelectList(qryRoles, "id", "role_name");
            UserRoles usrRoles = new UserRoles();
            //var qryEmp = usrRoles.GetEmployees(UserDetails.Current.SchoolId);
            //ViewBag.Users = new SelectList(qryEmp, "Id", "screen_name");
            Boolean blnFlg = false;
            try
            {
                if (ModelState.IsValid)
                {
                    blnFlg = usrRoles.AddUserRoles(UsrRoles,UserDetails.Current.SchoolId,UserDetails.Current.Iduser);
                    if (blnFlg == true)
                    {
                        return RedirectToAction("UserRoles");   
                    }
                    else
                    {
                        return View();
                    }
                    
                }

            }
            catch (Exception ex)
            {
                throw ex;
                //return View();
            }
            return View();
        }
        public ActionResult DeleteUserRoles(int? id)
        {
            if(id == null){
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserRoles usrRoles = new UserRoles();
            var qryUser = usrRoles.GetUserRoleById(id);
            return View(qryUser);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteUserRoles(int? id, FormCollection collection)
        {
            Boolean blnFlg = false;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            try
            {
                UserRoles usrRoles = new UserRoles();
                blnFlg = usrRoles.DelUserRoles(id, UserDetails.Current.SchoolId, UserDetails.Current.Iduser);
                return RedirectToAction("UserRoles");
            }
            catch
            {
                return View();
            }
        }


        public int getUserDetails(string username)
        {
            var user = db.AspNetUsers.SingleOrDefault(u => u.UserName == username);
            string Displayname = user.Displayname;
            int IdUser = (int)user.IdUser;
            return IdUser;
        }

        public Boolean getProfileInfo(String Username)
        {
            if (Username != "")
            {
                int userId = getUserDetails(Username);
                if (userId > 0)
                {
                    DomainModel.BusinessLayer.AuthorizeService _authorizeService = new DomainModel.BusinessLayer.AuthorizeService(userId);
                    UserDetails usr = new UserDetails();
                    usr.Iduser = userId;
                    usr.photo_url = _authorizeService.profilePic;
                    usr.SchoolId = _authorizeService.idschool;
                    usr.screen_name = _authorizeService.displayname;
                    usr.role_name = _authorizeService.role_name;
                    Session["userDetails"] = usr;
                    return true;
                }
            }
            return false;
        }




        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:                    
                    return RedirectToLocal(returnUrl, model.Email);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
            }
        }

        //
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // Require that the user has already logged in via username/password or external login
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // The following code protects for brute force attacks against the two factor codes. 
            // If a user enters incorrect codes for a specified amount of time then the user account 
            // will be locked out for a specified amount of time. 
            // You can configure the account lockout settings in IdentityConfig
            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent:  model.RememberMe, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl,"");
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid code.");
                    return View(model);
            }
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email, Displayname = model.DisplayName };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await SignInManager.SignInAsync(user, isPersistent:false, rememberBrowser:false);
                    
                    // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                    return RedirectToAction("Index", "Home");
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.Email);
                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View("ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                // string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                // var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);		
                // await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
                // return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/SendCode
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == null)
            {
                return View("Error");
            }
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Generate the token and send it
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl,"");
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    // If the user does not have an account, then prompt the user to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToLocal(returnUrl, model.Email);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut();
            Session.Abandon();
            //Session.clear();
            Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1));
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetNoStore();
            return RedirectToAction("Login", "Account");
        }


        //Profile Information



        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }
      


        private ActionResult RedirectToLocal(string returnUrl, string username)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            var res =  getProfileInfo(username);
            if (res)
            {
                if(UserDetails.Current.role_name == "Parent")
                {
                    return RedirectToAction("ParentHome", "Home");
                }

                if (UserDetails.Current.role_name == "Student")
                {
                    return RedirectToAction("StudentHome", "Home");
                }
            }

                return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}