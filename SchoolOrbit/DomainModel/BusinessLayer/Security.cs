using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
namespace DomainModel.BusinessLayer
{
 public class AuthorizeService
    {
        private DataModel.SOEntities db = new DataModel.SOEntities();
        public int iduser { get; set; }

        public int idschool { get; set; }

        public string displayname { get; set; }

        public int idrole { get; set; }

        public string role_name { get; set; }

        public string profilePic { get; set; }

        public Dictionary<int,string> getrole(int iduser, int idSchool)
        {
            var list = db.vew_user_roles.Where(d => d.idschool == idSchool && d.iduser  == iduser).ToList().Select(x => new { x.idrole, x.role_name });
            Dictionary<int, string> rolelst = new Dictionary<int, string>();
            foreach (var li in list)
            {
                rolelst.Add(li.idrole, li.role_name);
            }
            return rolelst;
        }

        public string getUserName(int idUser)
        {
            var qryUser = (from o in db.sys_user
                           where o.Id == idUser
                           select o.screen_name).FirstOrDefault();
            return qryUser;
        }
        
        public Boolean IsAuthorized(int iduser, String rolename)
        {
            var HasRole = db.vew_user_roles.Where(x => x.iduser == iduser && x.role_name  == rolename).FirstOrDefault();

            if (HasRole!=null)
            {
                return true;
            }

            return false ;
        }
        public AuthorizeService()
        {
            //Need to implement
        }

        public AuthorizeService (int iduser)
        {
            var user = db.vew_user_security_roles.Where(r => r.iduser == iduser).FirstOrDefault();
            if (user != null)
            {
                idschool = user.idschool;
                displayname = user.screen_name;
                idrole = user.idrole;
                role_name = user.role_name;
                profilePic = user.photo_url;
            }
        }

    }
}

    
