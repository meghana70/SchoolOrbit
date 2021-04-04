using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.BusinessLayer
{
    public class UserRoles
    {
        private DataModel.SOEntities db = new DataModel.SOEntities();

        public int id { get; set; }
        [Required]
        [Display(Name = "User")]
        public int idUser { get; set; }
        public int idSchool { get; set; }
        [Required]
        [Display(Name = "Role")]
        public int idRole { get; set; }
        [Display(Name = "Name")]
        public string displayname { get; set; }
        [Display(Name = "Role")]
        public string role_name { get; set; }

        public UserRoles()
        {

        }
        public List<UserRoles> GetUserRoles(int SchoolId)
        {                       
            var usrRoles = (from o in db.vew_user_roles
                            where o.idschool == SchoolId
                            select new UserRoles
                            {
                                displayname = o.screen_name,
                                role_name = o.role_name,
                                id = o.id
                            });
            return usrRoles.ToList();
        }
        public List<Employee> GetEmployees(int SchoolId,string strSearch)
        {
            var lstEmp = (from usr in db.sys_user 
                          join emp in db.sch_employee on usr.Id equals emp.idUser
                          where  usr.screen_name.Contains(strSearch) && emp.idSchool == SchoolId
                          orderby usr.Id 
                          select new Employee()
                          {
                              screen_name  = usr.screen_name,
                              Id  = (int)usr.Id
                          });
            return lstEmp.ToList();
        }
        public UserRoles GetUserRoleById(int? id)
        {
            var qryUser = (from o in db.sch_user_roles
                           join usr in db.AspNetUsers on o.idUser equals usr.IdUser
                           join rls in db.sys_roles on o.idRole equals rls.id
                           where o.id == id
                           select new UserRoles()
                           {
                               displayname = usr.Displayname,
                               role_name = rls.role_name
                           }).FirstOrDefault();
            return qryUser;
        }
        public Boolean AddUserRoles(UserRoles usrRoles,int SchoolId,int idUser)
        {
            try
            {
                
                    var qryUsr = (from o in db.sch_user_roles
                                  where o.idUser == usrRoles.idUser && o.idRole == usrRoles.idRole && o.idSchool == SchoolId
                                  select o).FirstOrDefault();
                    if (qryUsr != null)
                    {
                        qryUsr.isactive = true;
                        qryUsr.last_updated_by = idUser;
                        qryUsr.ts_updated = DateTime.Now;
                    }
                    else
                    {
                        DataModel.sch_user_roles usrRole = new DataModel.sch_user_roles();
                        usrRole.idRole = usrRoles.idRole;
                        usrRole.idUser = usrRoles.idUser;
                        usrRole.idSchool = SchoolId;
                        usrRole.isactive = true;
                        usrRole.entered_by = idUser;
                        usrRole.ts_entered = DateTime.Now;
                        usrRole.last_updated_by = idUser;
                        usrRole.ts_updated = DateTime.Now;
                        db.sch_user_roles.Add(usrRole);
                    }

                    db.SaveChanges();

                    return true;             
            }
            catch (Exception ex)
            {
                throw ex;
                //return View();
            }
        }
        public Boolean DelUserRoles(int? id, int SchoolId, int idUser)
        {
            try
            {
                var qryUsr = db.sch_user_roles.Where(x => x.id == id && x.idSchool == SchoolId).FirstOrDefault();
                qryUsr.isactive = false;
                qryUsr.last_updated_by = idUser;
                qryUsr.ts_updated = DateTime.Now;
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
    public class Roles
    {
        private DataModel.SOEntities db = new DataModel.SOEntities();
        public int id { get; set; }
        public string role_name { get; set; }
        public List<Roles> GetRoles()
        {
            var qryRoles = (from r in db.sys_roles
                            where r.Active == true
                            orderby r.role_name 
                            select new Roles
                            {
                                id = r.id,
                                role_name = r.role_name
                            });
            return qryRoles.ToList();
        }
    }
}
