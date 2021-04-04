using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DomainModel.BusinessLayer
{
    public class msgNotification
    {
        private DataModel.SOEntities db = new DataModel.SOEntities();
        public int id { get; set; }

        public int idSchool { get; set; }
        [Required]
        public string To { get; set; }
        public string ToList { get; set; } 
        public string jsonToList{get;set;}
        [Required]
        
        public string Body { get; set; }
        [Required]        
        [Display(Name="Subject")]
        public string subject { get; set; }
        [Display(Name="From")]
        public string sentByName { get; set; }
        public Nullable<int> sentBy { get; set; }
        public bool is_active { get; set; }
        [Display(Name="Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public System.DateTime ts_entered { get; set; }
        public Nullable<System.DateTime> ts_updated { get; set; }
        public bool is_read { get; set; }

        public class MYTagInput
        {
            public string id { get; set; }
            public string name { get; set; }
        }

        public List<MYTagInput> getToList(string strSearch,int idSchool)
        {          
            var tg = (from usr in db.sys_user
                      join emp in db.sch_employee on usr.Id equals emp.idUser
                      where usr.screen_name.Contains(strSearch) && usr.active == true && emp.idSchool == idSchool
                      select new MYTagInput()
                      {
                          id=usr.Id.ToString(),
                          name=usr.screen_name
                      });         
            return tg.ToList();
        }
        public List<msgNotification> getInbox(int idSchool,int idUser)
        {
            var qryInbox = (from scn in db.sch_notification
                            join scmn in db.sch_mynotification on scn.id equals scmn.idNotification
                            join usr in db.sys_user on scn.sentBy equals usr.Id 
                            where scn.idSchool == idSchool && scmn.idUser == idUser && scmn.is_active == true
                            orderby scn.ts_entered descending
                            select new msgNotification()
                            {
                                id=scn.id,
                                sentByName = usr.screen_name,
                                subject=scn.subject,
                                ts_entered =scn.ts_entered,
                                is_read = scmn.read 
                            });
            return qryInbox.ToList();
        }

        public List<msgNotification> getOutbox(int idSchool, int idUser)
        {
            var qryOutbox = (from scn in db.vewNotifications  
                            where scn.idschool == idSchool && scn.sentby == idUser
                            orderby scn.ts_entered descending
                            select new msgNotification()
                            {
                                id = scn.id,
                                ToList = scn.ToList,
                                subject = scn.subject,
                                ts_entered = scn.ts_entered
                             //   is_read = scmn.read 
                            });
            return qryOutbox.ToList();
        }

        public msgNotification getMsgDetails(int id,int idUser)
        {
            var qryResults = (from scn in db.vewNotifications 
                              where scn.id == id
                              select new msgNotification()
                              {
                                  To = scn.To,
                                  ToList =scn.ToList,
                                  jsonToList =scn.jsonToList,
                                  subject = scn.subject,
                                  Body=scn.body,
                                  ts_entered = scn.ts_entered,
                                  sentByName =scn.sentByname
                              }).FirstOrDefault();
            var qryMyNtf = (from myntf in db.sch_mynotification
                            where myntf.idNotification == id && myntf.idUser == idUser
                            select myntf).FirstOrDefault();
            if (qryMyNtf != null)
            {
                if (qryMyNtf.read == false)
                {
                    qryMyNtf.read = true;
                    db.SaveChanges();
                }
            }            
            return qryResults;
        }
        public Boolean deleteMsgs(string strIds,int ioBox, int idSchool, int idUser)
        {            
            try
            {
                string[] lstIds = strIds.Split(',');
                if (ioBox == 1)
                {
                    var mynotification = db.sch_mynotification.Where(x => x.idUser == idUser && lstIds.Contains(x.idNotification.ToString())).ToList();
                    mynotification.ForEach(a => a.is_active = false);
                    db.SaveChanges();

                }
                else if (ioBox == 2)
                {
                    var ntf = db.sch_notification.Where(x => lstIds.Contains(x.id.ToString())).ToList();
                    ntf.ForEach(a =>{
                        a.is_active = false;
                        a.ts_updated = DateTime.Now;
                    });
                    db.SaveChanges();

                }
                return true;
            }
            catch
            {
                return false;
            }                                
        }
        public Boolean saveNotification(msgNotification msg,int idUser,int idSchool)
        {
            try
            {
                DataModel.sch_notification ntf = new DataModel.sch_notification();
                ntf.To = msg.To;
                ntf.subject = msg.subject;
                ntf.Body = msg.Body;
                ntf.sentBy = idUser;
                ntf.idSchool = idSchool;
                ntf.is_active = true;
                ntf.ts_entered = DateTime.Now;
                ntf.ts_updated = DateTime.Now;
                db.sch_notification.Add(ntf);
                db.SaveChanges();
                foreach (string mnt in msg.To.Split(','))
                {
                    DataModel.sch_mynotification mntf = new DataModel.sch_mynotification();
                    mntf.idNotification  = ntf.id;
                    mntf.idUser =Convert.ToInt32(mnt);
                    mntf.read = false;
                    mntf.is_active = true;
                    db.sch_mynotification.Add(mntf);
                }                             
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
