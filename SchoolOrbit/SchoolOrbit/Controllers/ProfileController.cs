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

namespace SchoolOrbit.Controllers
{
    public class ProfileController : ApplicationBaseController 
    {
        private SchoolOrbitEntities db = new SchoolOrbitEntities();
        private ApplicationUserManager _userManager;
        // GET: Profile
        public ActionResult Index()
        {
            var query = from x in db.AspNetUsers
                        join y in db.sys_user on x.IdUser equals y.Id
                        select new Profile()
                        {
                            Id = y.Id,
                            first_name = y.first_name ,
                            last_name = y.last_name,
                            Email = x.Email,
                            UserName = x.Email
                        };
            return View(query.ToList());
        }

        // GET: Profile/Details/5
        public ActionResult Details(int id)
        {
            var profile = (from x in db.sys_user
                         join y in db.AspNetUsers on x.Id equals y.IdUser
                         where x.Id == id
                         select new Profile()
                         {
                             Id = x.Id,
                             first_name = x.first_name,
                             last_name = x.last_name,
                             DOB = x.DOB,
                             photo_url = x.photo_url,
                             screen_name = x.screen_name,
                             Email = y.Email ,
                             UserName = y.UserName
                         }).SingleOrDefault();

            return View(profile);
            
        }

        // GET: Profile/Create
        public ActionResult Create()
        {
            return View();
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,first_name,middle_name,last_name,gender,DOB,screen_name,photo_url,permanent_address,permanent_city,permanent_state,permanent_country,permanent_zip_code,present_address,present_city,present_state,present_country,present_zip_code,paediatrician_details,allergic,blood_group,active,Email,UserName")] Profile profile)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    var user = new ApplicationUser { UserName = profile.UserName, Email = profile.Email, Displayname =  profile.screen_name };
                    var result = await UserManager.CreateAsync(user, "Admin@123");
                    if (result.Succeeded)
                    {
                        //var query = db.AspNetUsers.Where(l => l.Id == user.Id).Select(l => l.IdUser);
                        int idUser = Convert.ToInt32(db.AspNetUsers.Single(x => x.Id == user.Id).IdUser);
                        // insert into UserAccount table

                        sys_user useraccnt = new sys_user();

                        useraccnt.active = true;
                        useraccnt.allergic = profile.allergic;
                        useraccnt.blood_group = profile.blood_group;
                        useraccnt.DOB = profile.DOB;
                        useraccnt.first_name = profile.first_name;
                        useraccnt.gender = profile.gender;
                        useraccnt.Id = idUser;
                        useraccnt.last_name = profile.last_name;
                        useraccnt.middle_name = profile.middle_name;
                        useraccnt.paediatrician_details = profile.paediatrician_details;
                        useraccnt.permanent_address = profile.permanent_address;
                        useraccnt.permanent_city = profile.permanent_city;
                        useraccnt.permanent_country = profile.permanent_country;
                        useraccnt.permanent_state = profile.permanent_state;
                        useraccnt.permanent_zip_code = profile.permanent_zip_code;
                        useraccnt.photo_url = profile.photo_url;
                        useraccnt.present_address = profile.present_address;
                        useraccnt.present_city = profile.present_city;
                        useraccnt.present_country = profile.present_country;
                        useraccnt.present_state = profile.present_state;
                        useraccnt.present_zip_code = profile.present_zip_code;
                        useraccnt.screen_name = profile.screen_name;


                        db.sys_user.Add(useraccnt);
                        db.SaveChanges();

                    }
                    return RedirectToAction("Index", "Home", new { area = "" });
                }
                // TODO: Add insert logic here

                return View();
            }
            catch
            {
                return View();
            }
        }
        // GET: Profile/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Profile/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Profile/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Profile/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
