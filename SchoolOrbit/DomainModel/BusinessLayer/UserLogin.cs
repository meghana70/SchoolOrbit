using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using System.Data.Entity;


namespace DomainModel.BusinessLayer
{
   public  class UserLogin
    {
       DataModel.SOEntities db = new DataModel.SOEntities();
        public enum LoginUserType
        {
            student = 4, employee = 3, parent = 2, Admin = 1
        };



        public Boolean  CreateUser(int idUser, LoginUserType loginType, String email, String userName)
        {
            var dbUSer = new MyDbContext();
            ApplicationDbInitializer dbApp = new ApplicationDbInitializer();            
            var res = dbApp.AddUser(idUser, loginType, email, userName);
             if (res)
             {
                
                 var xuser = db.AspNetUsers.Where(x => x.UserName == userName).FirstOrDefault();
                 xuser.IdUser = idUser;
                 db.SaveChanges();
                 return true;                
             }
             else
             {
                 return false;
             }
            
        }
        public string ChangePassword(int idUser)
        {
            var usr = db.AspNetUsers.FirstOrDefault(x => x.IdUser == idUser);
            string strRes = "";
            string strpwd = "Test@1234";
            var usermanager = new AppUserManager(new AppUserStore(new MyDbContext()));
            if (usr != null)
            {
                
                IdentityResult result = usermanager.ChangePassword(usr.Id, usr.PasswordHash, usermanager.PasswordHasher.HashPassword(strpwd));
                if (result.Succeeded)
                {
                    var emp = db.sch_employee.FirstOrDefault(y => y.idUser == idUser);
                    if (emp != null)
                    {
                        strRes = "1";
                    }
                    else
                    {
                        strRes = "2";
                    }
                }
                else
                {
                    string[] strErr = (string[])result.Errors;
                    strRes = strErr[0].ToString();
                }
            }
            else
            {
                strRes = "User not found!";
            }            
            return strRes;
        }

        public class MyDbContext : IdentityDbContext<IdentityUser>
        {
            public MyDbContext()
                : base("DefaultConnection")
            {
            }

            static MyDbContext()
            {
                new ApplicationDbInitializer();
            }
        }
        public class ApplicationDbInitializer : MyDbContext
        {
            public  Boolean AddUser(int idUser, LoginUserType loginType, String email, String userName)
            {
                var usermanager = new AppUserManager(new AppUserStore(new MyDbContext()));
                var tempuser = new IdentityUser() { UserName = userName, Email = email };
                usermanager.Create(tempuser, "Admin@123");
                usermanager.SaveAll();
                usermanager.Dispose();
                return true;                      
            }
           
        }
        public class AppUserManager : UserManager<IdentityUser>
        {
            public AppUserManager(AppUserStore store)
                : base(store)
            {

            }

            public void SaveAll()
            {
                var appstore = Store as AppUserStore;

                appstore.SaveAll();
            }
        }

        public class AppUserStore : UserStore<IdentityUser>
        {
            public AppUserStore(MyDbContext ctx)
                : base(ctx)
            {
                AutoSaveChanges = false;
            }

            public void SaveAll()
            {
                Context.SaveChanges();
            }
        }

    }
}