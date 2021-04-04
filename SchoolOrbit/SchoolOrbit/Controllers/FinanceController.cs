using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SchoolOrbit.Models;
using SchoolOrbit.Filters;
using System.IO;
using System.Web.UI.WebControls;
using System.Web.UI;
using DomainModel.BusinessLayer;
namespace SchoolOrbit.Controllers
{
    [RequireHttps]
    [CustomAuthorize("Administrator", "Chairman")]
    public class FinanceController : ApplicationBaseController 
    {
        // GET: Finance/Expenditure
       
        public ActionResult Expenditure(DateTime? minExpDate, DateTime? maxExpDate)
        {
            DomainModel.BusinessLayer.Expense Exp = new DomainModel.BusinessLayer.Expense();
            var qryExp = Exp.GetExpenditure(UserDetails.Current.SchoolId, minExpDate, maxExpDate);

            //var query = from exp in x
            //            select new ExpenseTrans()
            //            {
            //                id = exp.id,
            //                amount = exp.amount,
            //                expense_date = exp.expense_date,
            //                expense_type = exp.expense_type,
            //                remarks = exp.remarks
            //            };

            System.Web.HttpContext.Current.Session["minExpDate"] = minExpDate;
            System.Web.HttpContext.Current.Session["maxExpDate"] = maxExpDate;
            return View(qryExp.ToList());

        }
      
        public ActionResult MonthlyExpReport()
        {
            return View();
        }
       
        public ActionResult GetMonthlyExpReport()
        {
            String strContent = string.Empty;
            int currentyear = DateTime.Now.Year;
            DomainModel.BusinessLayer.Expense Exp = new DomainModel.BusinessLayer.Expense();
            strContent = Exp.getMonthlyExpReport(UserDetails.Current.SchoolId,currentyear);
            return Content(strContent);
        }
        public ActionResult GetDashboardSubItems()
        {
            int mthCurrent = DateTime.Now.Month;
            int yrCurrent = DateTime.Now.Year;
            String strContent = "";
            Expense exp = new Expense();
            strContent = exp.getDashboardSubItems(UserDetails.Current.SchoolId, yrCurrent, mthCurrent);
            return Content(strContent);
        }
        public void ExportExpenses(DateTime? minExpDate, DateTime? maxExpDate)
        {

            if (System.Web.HttpContext.Current.Session["minExpDate"] != null && System.Web.HttpContext.Current.Session["maxExpDate"] != null)
            {
                minExpDate = (DateTime)System.Web.HttpContext.Current.Session["minExpDate"];
                maxExpDate = (DateTime)System.Web.HttpContext.Current.Session["maxExpDate"];
            }            
            //if (minExpDate != null && maxExpDate != null)
            //{
            //    query = query.Where(x => x.expense_date >= minExpDate && x.expense_date <= maxExpDate);
            //}
            //else
            //{
            //    query = query.Where(x => x.expense_date >= dtTmp);
            //}
            DomainModel.BusinessLayer.Expense Exp = new DomainModel.BusinessLayer.Expense();
            var x = Exp.GetExpenditure(UserDetails.Current.SchoolId, minExpDate, maxExpDate);

            //var query = from exp in x
            //            select new ExpenseTrans()
            //            {
            //                id = exp.id,
            //                amount = exp.amount,
            //                expense_date = exp.expense_date,
            //                expense_type = exp.expense_type,
            //                remarks = exp.remarks
            //            };

            //IList<ExpenseTrans> qryList= query.ToList();
            Export2Excel("ExpenseReport",x.ToList());
        }      
        public void Export2Excel(String strReport,IList<Expense> lstExpense)
        {
            StringWriter sw = new StringWriter();
            Response.ClearContent();
            Response.AddHeader("content-disposition", "attachment; filename="+strReport+".xls");
            Response.ContentType = "application/ms-excel";
            
            var grid = new GridView();
            grid.DataSource = lstExpense.ToList();
            grid.DataBind();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            grid.RenderControl(htw);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();
        }
        // GET: Finance/CreateExpenditure
        public ActionResult CreateExpenditure()
        {
            LoadDropdowns();
            return View();
        }
        
       void LoadDropdowns(){
           Dictionary<int, string> ExpenseType = new Dictionary<int, string>();          
           DomainModel.BusinessLayer.ListCollection LC = new DomainModel.BusinessLayer.ListCollection();
           ExpenseType = LC.LoadExpenseType((int)UserDetails.Current.SchoolId);
           ViewBag.ExpenseCategory = new SelectList(ExpenseType.Select(x => new SelectListItem() { Text = x.Value.ToString(), Value = x.Key.ToString() }), "Value", "Text");
           List<SelectListItem> lstModeofPayment = new List<SelectListItem>()
                {             
                    new SelectListItem(){Value="Cash",Text="Cash"},
                    new SelectListItem(){Value="Cheque",Text="Cheque"},
                    new SelectListItem(){Value="Demand Draft",Text="Demand Draft"}
                };
           ViewBag.ModeofPayment = new SelectList(lstModeofPayment, "Value", "Text");
        }

        // POST: Finance/CreateExpenditure
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateExpenditure(HttpPostedFileBase vocher,HttpPostedFileBase vocher2, [Bind(Include = "expense_type_id,amount,remarks,expense_date,mode_of_payment,cheque_dd_no,issue_bank")] ExpenseTrans Exp)
        {         
            try
            {
                if (ModelState.IsValid)
                {
                    LoadDropdowns();
                    HttpPostedFileBase voch1 = Request.Files["vocher"];
                    HttpPostedFileBase voch2 = Request.Files["vocher2"];

                    DomainModel.BusinessLayer.Expense exptrans = new DomainModel.BusinessLayer.Expense();
                    exptrans.amount = Exp.amount;
                    exptrans.entered_by = Exp.entered_by;
                    exptrans.expense_date = Exp.expense_date;
                    exptrans.expense_type_id = Exp.expense_type_id;                   
                    exptrans.idSchool = UserDetails.Current.SchoolId;
                    exptrans.last_updated_by = UserDetails.Current.Iduser;
                    exptrans.remarks = Exp.remarks;
                    exptrans.mode_of_payment = Exp.mode_of_payment;
                    exptrans.cheque_dd_no = Exp.cheque_dd_no;
                    exptrans.issue_bank = Exp.issue_bank;
                    int Exptransid = 0;

                    Exptransid = exptrans.AddExpense(exptrans);

                    if (Exptransid > 0)
                    {
                                        
                        string TempFolder = "/Uploads";                        
                        var serverPath = HttpContext.Server.MapPath(TempFolder);
                        if (Directory.Exists(serverPath) == false)
                        {
                            Directory.CreateDirectory(serverPath);
                        }
                        if (voch1.FileName != "")
                        {
                            var fileext = Path.GetExtension(voch1.FileName);
                            var fileName = String.Concat("Fin_Voch1_", Exptransid, fileext);
                            var fullFileName = Path.Combine(serverPath, fileName);
                            voch1.SaveAs(fullFileName);
                            string vochblob = fileName;
                            //Saving files to cloud Strorage..
                            DomainModel.BusinessLayer.BlobStorage bs = new DomainModel.BusinessLayer.BlobStorage();
                            Boolean res = false;
                            res = bs.SaveBlob((int)DomainModel.BusinessLayer.BlobStorage.StorageContainer.finance, vochblob, fullFileName);
                            //end
                            System.IO.File.Delete(fullFileName);
                            //Saving Attachments
                            Boolean Attach = exptrans.SaveAttachment(UserDetails.Current.Iduser, Exptransid, "/finance/" + fileName);
                            //end
                        }
                        if (voch2.FileName != "")
                        {
                            var fileext = Path.GetExtension(voch2.FileName);
                            var fileName = String.Concat("Fin_Voch2_", Exptransid, fileext);
                            var fullFileName = Path.Combine(serverPath, fileName);
                            voch2.SaveAs(fullFileName);
                            string vochblob2 = fileName;
                            DomainModel.BusinessLayer.BlobStorage bs = new DomainModel.BusinessLayer.BlobStorage();
                            Boolean res = false;
                            res = bs.SaveBlob((int)DomainModel.BusinessLayer.BlobStorage.StorageContainer.finance, vochblob2, fullFileName);
                            System.IO.File.Delete(fullFileName);
                            Boolean Attach = exptrans.SaveAttachment(UserDetails.Current.Iduser, Exptransid, "/finance/" + fileName);
                        }
                    }

                    return RedirectToAction("Expenditure");
                }
               
            }
            catch (Exception ex)
            {
                throw ex;
                //return View();
            }
            return View();
        }

        public ActionResult EditExpenditure(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LoadDropdowns();
             DomainModel.BusinessLayer.Expense exptrans = new DomainModel.BusinessLayer.Expense();
            var qryExpTrans = exptrans.GetExpenseTrans(Convert.ToInt32(id), Convert.ToInt32(UserDetails.Current.SchoolId));
               var trans = new ExpenseTrans();
                   trans. id = qryExpTrans.id;
                   trans.expense_type_id = qryExpTrans.expense_type_id;
                   trans.expense_date = qryExpTrans.expense_date;
                   trans.mode_of_payment = qryExpTrans.mode_of_payment;
                   trans.cheque_dd_no = qryExpTrans.cheque_dd_no;
                   trans.remarks = qryExpTrans.remarks;
                   trans.amount = qryExpTrans.amount;
                   trans.issue_bank = qryExpTrans.issue_bank;                       
                               
            List<string> qryExpAtt = new List<string>();
            qryExpAtt = exptrans.GetAttachment(Convert.ToInt32(id));
            String strAttachment = "";
            foreach (var item in qryExpAtt)
            {
                strAttachment = strAttachment + item.ToString() + ",";
            }
            if (strAttachment != "")
            {
                ViewBag.attachment = strAttachment;
            }
            return View(trans);
        }
        [HttpPost]
        public ActionResult EditExpenditure(HttpPostedFileBase vocher, HttpPostedFileBase vocher2, [Bind(Include = "id,expense_type_id,amount,remarks,expense_date,mode_of_payment,cheque_dd_no,issue_bank")] ExpenseTrans Exp)
        {

            try
            {
                if (ModelState.IsValid)
                {
                    LoadDropdowns();
                    HttpPostedFileBase voch1 = Request.Files["vocher"];
                    HttpPostedFileBase voch2 = Request.Files["vocher2"];
                    DomainModel.BusinessLayer.Expense exptrans = new DomainModel.BusinessLayer.Expense();
                    exptrans.id = Exp.id;
                    exptrans.amount = Exp.amount;
                    exptrans.entered_by = Exp.entered_by;
                    exptrans.expense_date = Exp.expense_date;
                    exptrans.expense_type_id = Exp.expense_type_id;
                    exptrans.idSchool = UserDetails.Current.SchoolId;
                    exptrans.last_updated_by = UserDetails.Current.Iduser;
                    exptrans.remarks = Exp.remarks;
                    exptrans.mode_of_payment = Exp.mode_of_payment;
                    exptrans.cheque_dd_no = Exp.cheque_dd_no;
                    exptrans.issue_bank = Exp.issue_bank;

                    int Exptransid = exptrans.SaveExpense(exptrans);
                    if (Exptransid > 0)
                    {
                        string TempFolder = "/Uploads";
                        var serverPath = HttpContext.Server.MapPath(TempFolder);
                        if (Directory.Exists(serverPath) == false)
                        {
                            Directory.CreateDirectory(serverPath);
                        }
                        if (voch1.FileName != "")
                        {
                            var fileext = Path.GetExtension(voch1.FileName);
                            var fileName = String.Concat("Fin_Voch1_", Exptransid, fileext);
                            var fullFileName = Path.Combine(serverPath, fileName);
                            voch1.SaveAs(fullFileName);
                            string vochblob = fileName;
                            //Saving files to cloud Strorage..
                            DomainModel.BusinessLayer.BlobStorage bs = new DomainModel.BusinessLayer.BlobStorage();
                            Boolean res = false;
                            res = bs.SaveBlob((int)DomainModel.BusinessLayer.BlobStorage.StorageContainer.finance, vochblob, fullFileName);
                            //end
                            System.IO.File.Delete(fullFileName);
                            //Saving Attachments
                            Boolean Attach = exptrans.SaveAttachment(UserDetails.Current.Iduser, Exptransid, "/finance/" + fileName);
                            //end
                        }
                        if (voch2.FileName != "")
                        {
                            var fileext = Path.GetExtension(voch2.FileName);
                            var fileName = String.Concat("Fin_Voch2_", Exptransid, fileext);
                            var fullFileName = Path.Combine(serverPath, fileName);
                            voch2.SaveAs(fullFileName);
                            string vochblob2 = fileName;
                            DomainModel.BusinessLayer.BlobStorage bs = new DomainModel.BusinessLayer.BlobStorage();
                            Boolean res = false;
                            res = bs.SaveBlob((int)DomainModel.BusinessLayer.BlobStorage.StorageContainer.finance, vochblob2, fullFileName);
                            System.IO.File.Delete(fullFileName);
                            Boolean Attach = exptrans.SaveAttachment(UserDetails.Current.Iduser, Exptransid, "/finance/" + fileName);
                        }

                    }
                    return RedirectToAction("Expenditure");
                }
                return View();
            }
            catch
            {
                return View();
            }

        }
    
        //
        //// GET: Finance/ExpenseList
        public ActionResult ExpenseList()
        {

            DomainModel.BusinessLayer.ExpenseType exptype = new DomainModel.BusinessLayer.ExpenseType();

            var x = exptype.GetExpenseType(Convert.ToInt32(UserDetails.Current.SchoolId));

            var query = from exp in x
                        select new DomainModel.BusinessLayer.ExpenseType()
                        {
                            id = exp.id,
                            idSchool = exp.idSchool,
                            expense_type = exp.expense_type,
                            entered_by = exp.entered_by,
                            isactive = exp.isactive,
                            last_updated_by = exp.last_updated_by,
                            ts_entered = exp.ts_entered,
                            ts_updated = exp.ts_updated
                        };

           
                      
            return View(query.ToList());
        }
        public ActionResult ExpTypeDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            DomainModel.BusinessLayer.ExpenseType exptype = new DomainModel.BusinessLayer.ExpenseType();

            var x = exptype.GetExpenseTypeDetails(Convert.ToInt32(id),Convert.ToInt32(UserDetails.Current.SchoolId));

            DomainModel.BusinessLayer.ExpenseType qryExpType = new DomainModel.BusinessLayer.ExpenseType();
            qryExpType. id = x.id;
            qryExpType. expense_type = x.expense_type;
            qryExpType.isactive = x.isactive;

            return View(qryExpType);
        }

        public ActionResult ExpTranDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DomainModel.BusinessLayer.Expense exptrans = new DomainModel.BusinessLayer.Expense();
            var qryExpTrans = exptrans.GetExpenseTrans(Convert.ToInt32(id), Convert.ToInt32(UserDetails.Current.SchoolId));
            var trans = new ExpenseTrans();
            trans.id = qryExpTrans.id;
            trans.expense_type = qryExpTrans.expense_type;
            trans.expense_type_id = qryExpTrans.expense_type_id;
            trans.expense_date = qryExpTrans.expense_date;
            trans.mode_of_payment = qryExpTrans.mode_of_payment;
            trans.cheque_dd_no = qryExpTrans.cheque_dd_no;
            trans.remarks = qryExpTrans.remarks;
            trans.amount = qryExpTrans.amount;
            trans.issue_bank = qryExpTrans.issue_bank;

            List<string> qryExpAtt = new List<string>();
            qryExpAtt = exptrans.GetAttachment(Convert.ToInt32(id));
            String strAttachment = "";
            foreach (var item in qryExpAtt)
            {
                strAttachment = strAttachment + item.ToString();
            }
            if (strAttachment != "")
            {
                ViewBag.attachment = strAttachment;
            }

            ViewBag.BlobURL = DomainModel.BusinessLayer.BlobStorage.BlobStorageURL;
            return View(trans);
        }
        //public ActionResult ExpTranDelete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    var qryExpTrans = (from o in db.sch_expense_trans
        //                       join e in db.sch_expense_type on o.expense_type_id equals e.id
        //                       where o.id == id && o.idSchool == UserDetails.Current.SchoolId
        //                       select new ExpenseTrans()
        //                       {
        //                           id = o.id,
        //                           expense_type = e.expense_type,
        //                           expense_date = o.expense_date,
        //                           mode_of_payment = o.mode_of_payment,
        //                           cheque_dd_no = o.cheque_dd_no,
        //                           remarks = o.remarks,
        //                           amount = o.amount,
        //                           issue_bank = o.issue_bank
        //                       }).FirstOrDefault();
        //    var qryExpAtt = (from att in db.sch_expense_attachment
        //                     where att.idexpense_trans == id
        //                     select att.vocher).ToList();
        //    String strAttachment = "";
        //    foreach (var item in qryExpAtt)
        //    {
        //        strAttachment = strAttachment + item.ToString() + ",";
        //    }
        //    if (strAttachment != "")
        //    {
        //        ViewBag.attachment = strAttachment;
        //    }
        //    return View(qryExpTrans);
        //}
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult ExpTranDelete(int? id, FormCollection collection)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    try
        //    {
        //        var schexp = db.sch_expense_trans.Where(x => x.id == id && x.idSchool == UserDetails.Current.SchoolId).FirstOrDefault();
        //        schexp.isdeleted = true;
        //        schexp.last_updated_by = UserDetails.Current.Iduser;
        //        schexp.ts_updated = DateTime.Now;
        //        db.SaveChanges();
        //        return RedirectToAction("Expenditure");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //get
        public ActionResult NewExpenseCategory()
        {
            
            return View();
        }

        // POST: Finance/NewExpenseCategory
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NewExpenseCategory([Bind(Include = "expense_type,isactive")] DomainModel.BusinessLayer.ExpenseType Exp)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    DomainModel.BusinessLayer.ExpenseType ExpCat = new DomainModel.BusinessLayer.ExpenseType();
                    ExpCat.entered_by = UserDetails.Current.Iduser;
                    ExpCat.expense_type = Exp.expense_type;
                    ExpCat.idSchool = UserDetails.Current.SchoolId;
                    ExpCat.isactive = Exp.isactive;
                    ExpCat.last_updated_by = UserDetails.Current.Iduser;
                    ExpCat.ts_entered = DateTime.Now;
                    ExpCat.ts_updated = DateTime.Now;
                    int x = ExpCat.AddExpenseType(ExpCat);                    
                    return RedirectToAction("ExpenseList");
                }
                return View();
            }
            catch
            {
                return View();
            }
        }
        public ActionResult EditExpType(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DomainModel.BusinessLayer.ExpenseType exptype = new DomainModel.BusinessLayer.ExpenseType();
            var x = exptype.GetExpenseTypeDetails(Convert.ToInt32(id), Convert.ToInt32(UserDetails.Current.SchoolId));
            DomainModel.BusinessLayer.ExpenseType qryExpType = new DomainModel.BusinessLayer.ExpenseType();
            qryExpType.id = x.id;
            qryExpType.expense_type = x.expense_type;
            qryExpType.isactive = x.isactive;
            return View(qryExpType);
        }
        [HttpPost]
        public ActionResult EditExpType([Bind(Include = "id,expense_type,isactive")] DomainModel.BusinessLayer.ExpenseType Exp)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    DomainModel.BusinessLayer.ExpenseType ExpCat = new DomainModel.BusinessLayer.ExpenseType();
                    ExpCat.id = Exp.id;
                    ExpCat.expense_type = Exp.expense_type;
                    ExpCat.idSchool = UserDetails.Current.SchoolId;
                    ExpCat.isactive = Exp.isactive;
                    ExpCat.last_updated_by = UserDetails.Current.Iduser;
                    ExpCat.ts_entered = DateTime.Now;
                    ExpCat.ts_updated = DateTime.Now;
                    int res = ExpCat.SaveExpenseType(ExpCat);                  
                    return RedirectToAction("ExpenseList");
                }
                return View();
            }
            catch
            {
                return View();
            }
        }
       

    }
}
