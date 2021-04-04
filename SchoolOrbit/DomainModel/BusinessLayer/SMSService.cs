using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.BusinessLayer
{
   public class SMSService
    {
        private DataModel.SOEntities db = new DataModel.SOEntities();
        public int id { get; set; }
        public int idSchool { get; set; }
        public string ServiceProvidername { get; set; }
        public string Sender { get; set; }
        public string login { get; set; }
        public string password { get; set; }
        public System.DateTime ts_entered { get; set; }
        public Nullable<System.DateTime> ts_updated { get; set; }
        public int ts_updated_by { get; set; }
        public int Balance { get; set; }
        public string WebApi { get; set; }
        public string BalanceWebapi { get; set; }


       public SMSService(int Schoolid)
       {
            var xSMSAPi = db.sch_sms_service.Where(x => x.idSchool == Schoolid).FirstOrDefault();

            if (xSMSAPi != null)
            {
                Sender = xSMSAPi.Sender;
                login = xSMSAPi.login;
                password = xSMSAPi.password;
                WebApi = xSMSAPi.WebApi;
                BalanceWebapi = xSMSAPi.BalanceWebapi;
            }

            
        }       

    }

    public class LogSMS
    {
        private DataModel.SOEntities db = new DataModel.SOEntities();
        public int id { get; set; }

        public int idSchool { get; set; }

        public string To { get; set; }

        public string Body { get; set; }

        public Nullable<int> sentBy { get; set; }

        public System.DateTime ts_entered { get; set; }

        public Nullable<System.DateTime> ts_updated { get; set; }

        public bool SMSLOG(LogSMS log)
        {
            try {
                DataModel.sch_sms_notification SmsNotification = new DataModel.sch_sms_notification();
                SmsNotification.Body = log.Body;
                SmsNotification.idSchool = log.idSchool;
                SmsNotification.sentBy = log.sentBy;
                SmsNotification.To = log.To;
                SmsNotification.ts_entered = DateTime.Now;
                SmsNotification.ts_updated = DateTime.Now;
                db.sch_sms_notification.Add(SmsNotification);
                db.SaveChanges();
                return true;
            }
            catch(Exception ex)
            {
                return false;
                throw ex;
            }
            
        }

    }
}
