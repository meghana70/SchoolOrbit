using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Net;
using System.IO;
using SchoolOrbit.Models;

namespace SchoolOrbit.Controllers
{
    public class NotificationController : ApplicationBaseController 
    {
        // GET: Notification
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SendSMS()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SendSMS([Bind(Include ="To,Body")] DomainModel.BusinessLayer.LogSMS smsNotification)
        {
            DomainModel.BusinessLayer.SMSService smsService = new DomainModel.BusinessLayer.SMSService(UserDetails.Current.SchoolId);
            String Sender = smsService.Sender;
                     String message = HttpUtility.UrlEncode(smsNotification.Body);
            //String strUrl = "http://sms.b2bsms.co.in/API/WebSMS/Http/v1.0a/index.php?username=vinoda&password=testsms&sender=" + Sender + "&to=9866009631&message=" + message + "&reqid=1&format={json|text}&route_id=60";
            String strUrl = "http://login.bulksmsgateway.in/sendmessage.php?user=vinoda&password=9866009631&message=" + message + "&sender=" +  Sender + "&mobile=" + smsNotification.To + "&type=3";
            WebRequest request = HttpWebRequest.Create(strUrl);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream s = (Stream)response.GetResponseStream();
            StreamReader readStream = new StreamReader(s);
            string dataString = readStream.ReadToEnd();
            response.Close();
            s.Close();
            readStream.Close();
            //Log SMS Request
            DomainModel.BusinessLayer.LogSMS smslog = new DomainModel.BusinessLayer.LogSMS();
            smslog.Body = smsNotification.Body;
            smslog.idSchool = UserDetails.Current.SchoolId;
            smslog.sentBy = UserDetails.Current.Iduser;
            smslog.To = smsNotification.To;
            var res = smslog.SMSLOG(smslog);
            return RedirectToAction("Inbox");
        }


        public ActionResult GetToList()
        {
            string sample = string.Empty;
            string lookupterm = string.Empty;
            lookupterm = Request.QueryString["q"];
            DomainModel.BusinessLayer.msgNotification msg = new DomainModel.BusinessLayer.msgNotification();
            var qryResults = msg.getToList(lookupterm,UserDetails.Current.SchoolId);
            //IEnumerable<MYTagInput> results = tg.Where(x => x.name.Contains(lookupterm));
           sample = JSONHelper.ToJSON(qryResults);
           return Content(sample);
        }

        public ActionResult DeleteMsg(string strIds,int ioBox)
        {
            string strForm = ioBox.ToString();
           
            DomainModel.BusinessLayer.msgNotification msg = new DomainModel.BusinessLayer.msgNotification();
            Boolean blnflg = msg.deleteMsgs(strIds, ioBox, UserDetails.Current.SchoolId, UserDetails.Current.Iduser);           
           
            return Content(strForm);
        }

        public ActionResult ReplyMsg(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DomainModel.BusinessLayer.msgNotification msg = new DomainModel.BusinessLayer.msgNotification();
            var qryResults = msg.getMsgDetails((int)id, UserDetails.Current.Iduser);
            return View(qryResults);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ReplyMsg([Bind(Include = "Id,To,Body,subject")] DomainModel.BusinessLayer.msgNotification ntf)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    DomainModel.BusinessLayer.msgNotification msg = new DomainModel.BusinessLayer.msgNotification();
                    Boolean blnFlg = msg.saveNotification(ntf, UserDetails.Current.Iduser, UserDetails.Current.SchoolId);
                    if (blnFlg == true)
                    {
                        return RedirectToAction("Inbox");
                    }
                    else
                    {
                        return View();
                    }
                }
            }
            catch
            {
                return View();
            }
            return View();
        }
         public ActionResult Compose()
        {
            return View();
        }

         // POST: Notification/Compose
         [HttpPost]
         [ValidateAntiForgeryToken]
         public ActionResult Compose([Bind(Include = "Id,To,Body,subject")] DomainModel.BusinessLayer.msgNotification ntf)
         {
             try
             {
                 if (ModelState.IsValid)
                 {
                     DomainModel.BusinessLayer.msgNotification msg = new DomainModel.BusinessLayer.msgNotification();
                     Boolean blnFlg = msg.saveNotification(ntf, UserDetails.Current.Iduser, UserDetails.Current.SchoolId);
                     if (blnFlg == true)
                     {
                         return RedirectToAction("Inbox");
                     }
                     else
                     {
                         return View();
                     }
                 }
             }
             catch
             {
                 return View();
             }
             return View();
         }
        public ActionResult Inbox()
        {
            DomainModel.BusinessLayer.msgNotification msg = new DomainModel.BusinessLayer.msgNotification();
            var qryResults = msg.getInbox(UserDetails.Current.SchoolId,UserDetails.Current.Iduser);
            return View(qryResults.ToList());
        }
        public ActionResult Outbox()
        {
            DomainModel.BusinessLayer.msgNotification msg = new DomainModel.BusinessLayer.msgNotification();
            var qryResults = msg.getOutbox(UserDetails.Current.SchoolId, UserDetails.Current.Iduser);
            return View(qryResults.ToList());
        }
        public ActionResult Details(int? id,int iobox)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DomainModel.BusinessLayer.msgNotification msg = new DomainModel.BusinessLayer.msgNotification();
            var qryMsg = msg.getMsgDetails((int)id,UserDetails.Current.Iduser);
            ViewBag.ioBoxId = iobox;
            return View(qryMsg);
        }
    }

    public static class JSONHelper
    {
        public static string ToJSON(this object obj)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            return serializer.Serialize(obj);
        }

        public static string ToJSON(this object obj, int recursionDepth)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            serializer.RecursionLimit = recursionDepth;
            return serializer.Serialize(obj);
        }
    }
}