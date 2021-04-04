using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.BusinessLayer
{

    public class Expense
    {
        private DataModel.SOEntities db = new DataModel.SOEntities();


        public enum ExpenseStatus
        {
            Approved = 4, Rejected = 2, Cancelled = 3, PendingApproval = 1
        };
        public int id { get; set; }
        public int idSchool { get; set; }
        [Display(Name = "Expense Category")]
        public int expense_type_id { get; set; }
        [Required]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Expense Date")]
        public Nullable<System.DateTime> expense_date { get; set; }
        [Display(Name = "Mode of Payment")]
        public string mode_of_payment { get; set; }
        [Display(Name = "Cheque/DD Number")]
        public Nullable<int> cheque_dd_no { get; set; }
        [Display(Name = "Bank Name")]
        public string issue_bank { get; set; }
        [Required]
        [Display(Name = "Amount")]
        public Nullable<decimal> amount { get; set; }
        [Required]
        [Display(Name = "Remarks")]
        public string remarks { get; set; }
        [Display(Name = "Expense Category")]
        public string expense_type { get; set; }
        public Nullable<int> approved_by { get; set; }
        public Nullable<System.DateTime> approved_dt { get; set; }
        public string approved_remarks { get; set; }
        public System.DateTime ts_entered { get; set; }
        public System.DateTime ts_updated { get; set; }
        public int last_updated_by { get; set; }
        public int entered_by { get; set; }
        public int idExpensestatus { get; set; }

        public List<Expense> GetExpenditure(int SchoolId, DateTime? minExpDate, DateTime? maxExpDate)
        {
           
            var query = from exp in db.sch_expense_trans
                        join typ in db.sch_expense_type on exp.expense_type_id equals typ.id
                        where exp.idSchool == SchoolId && exp.isdeleted == false
                        orderby exp.expense_date descending
                        select new Expense()
                        {
                            id = exp.id,
                            amount = exp.amount,
                            expense_date = exp.expense_date,
                            expense_type = typ.expense_type,
                            remarks = exp.remarks,
                            issue_bank = exp.issue_bank,
                            cheque_dd_no = exp.cheque_dd_no,
                            mode_of_payment = exp.mode_of_payment,
                        };
            if (minExpDate != null && maxExpDate != null)
            {
                query = query.Where(x => x.expense_date >= minExpDate && x.expense_date <= maxExpDate);
            }
            else
            {
                query = query.Take(25);
            }
            return query.ToList();
        }

        public Expense GetExpenseTrans(int Transid, int SchoolId)
        {
            var expense = (from exp in db.sch_expense_trans
                           join typ in db.sch_expense_type on exp.expense_type_id equals typ.id
                           where exp.idSchool == SchoolId && exp.isdeleted == false && exp.id == Transid
                           orderby exp.expense_date descending
                           select new Expense()
                           {
                               id = exp.id,
                               amount = exp.amount,
                               expense_date = exp.expense_date,
                               expense_type = typ.expense_type,
                               remarks = exp.remarks,
                               issue_bank = exp.issue_bank,
                               cheque_dd_no = exp.cheque_dd_no,
                               mode_of_payment = exp.mode_of_payment,
                           }).SingleOrDefault();

            return expense;
        }

        public List<String> GetAttachment(int Transid)
        {
            var qryExpAtt = (from att in db.sch_expense_attachment
                             where att.idexpense_trans == Transid
                             select att.vocher).ToList();

            return qryExpAtt;
        }

         public string getMonthlyExpReport4Chart(int idSchool,int CurrentYear)
       {
           string strRes = "";
           var exp = (from x in db.fn_sch_expense_consol_month_view(CurrentYear, idSchool)
                      select x).ToList();
           if (exp.Count > 0)
           {
               foreach (var ex in exp)
               {
                   strRes = ex.Jan + "," + ex.Feb + "," + ex.Mar + "," + ex.Apr + "," + ex.May + "," + ex.Jun + "," + ex.Jul + "," + ex.Aug + "," + ex.Sep;
                   strRes += "," + ex.Oct + "," + ex.Nov + "," + ex.Dec;
               }
           }
           return strRes;
       }
       public string getDashboardSubItems(int idSchool, int CurrentYear, int CurrentMth)
       {
           var expMTHrep = (from o in db.sch_expense_trans
                            where o.expense_date.Value.Month == CurrentMth && o.expense_date.Value.Year == CurrentYear && o.idSchool == idSchool
                            select new
                            {
                                amt = o.amount
                            }).Sum(p => p.amt);
           var expYTDrep = (from y in db.sch_expense_trans
                            where y.idSchool == idSchool 
                            select new
                            {
                                amt = y.amount
                            }).Sum(p => p.amt);
           String strContent = "";
           if (expMTHrep != null)
           { 
               expMTHrep.Value.ToString(); 
           }
           else
           {
               expMTHrep = 0;
           }
           if (expYTDrep != null)
           { 
               expYTDrep.Value.ToString(); 
           }
           else
           {
               expYTDrep = 0;
           }
           strContent = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(CurrentMth) + "," + expMTHrep.Value.ToString() + "," + expYTDrep.ToString();
           return strContent.Replace(".0000", ".00");
       }

       public string getMonthlyExpReport(int idSchool,int yrCurrent)
       {
           String strContent = "";
           var expMTHRep = (from o in db.fn_expense_rpt_month_view(yrCurrent, idSchool)
                            select o).ToList();

           decimal totalamount = 0;
           strContent = strContent + "<table class=\"table\">";
           strContent = strContent + "<tr><td>Month</td><td>Amount</td></tr>";
           foreach (var item in expMTHRep)
           {
               strContent = strContent + "<tr>";
               strContent = strContent + "<td>" + item.MonthName + "</td><td>" + item.Amount + "</td>";
               strContent = strContent + "</tr>";
               totalamount = totalamount + item.Amount;
           }
           strContent = strContent + "<tr><td class=\"text-bold text-green\">Total Amount</td><td class=\"text-bold text-green\">" + totalamount + "</td></tr>";
           strContent = strContent + "</table>";
           return strContent.Replace(".0000", ".00");
       }
       
       
        public int AddExpense(Expense Exp)
        {
            try
            {
                DataModel.sch_expense_trans exptrans = new DataModel.sch_expense_trans();
                exptrans.amount = Exp.amount;
                exptrans.entered_by = Exp.entered_by;
                exptrans.expense_date = Exp.expense_date;
                exptrans.expense_type_id = Exp.expense_type_id;
                exptrans.idExpensestatus = (int)Expense.ExpenseStatus.PendingApproval;
                exptrans.idSchool = Exp.idSchool;
                exptrans.last_updated_by = Exp.last_updated_by;
                exptrans.remarks = Exp.remarks;
                exptrans.mode_of_payment = Exp.mode_of_payment;
                exptrans.cheque_dd_no = Exp.cheque_dd_no;
                exptrans.issue_bank = Exp.issue_bank;
                exptrans.ts_entered = DateTime.Now;
                exptrans.ts_updated = DateTime.Now;
                db.sch_expense_trans.Add(exptrans);
                db.SaveChanges();
                return exptrans.id;
            }

            catch (Exception ex)
            {
                throw ex;

            }

        }


        public int SaveExpense(Expense Exp)
        {
            try
            {
                var exptrans = db.sch_expense_trans.Where(x => x.id == Exp.id && x.idSchool == Exp.idSchool).FirstOrDefault();
                exptrans.expense_type_id = Exp.expense_type_id;
                exptrans.amount = Exp.amount;
                exptrans.remarks = Exp.remarks;
                exptrans.mode_of_payment = Exp.mode_of_payment;
                exptrans.cheque_dd_no = Exp.cheque_dd_no;
                exptrans.issue_bank = Exp.issue_bank;
                exptrans.last_updated_by = Exp.last_updated_by;
                exptrans.ts_updated = DateTime.Now;
                db.SaveChanges();
                return Exp.id;
            }

            catch (Exception ex)
            {
                throw ex;

            }

        }

        public bool SaveAttachment(int CurrentUser, int Id, string Vocher)
        {
            try
            {
                var expattch = (from attch in db.sch_expense_attachment where attch.vocher == Vocher && attch.id == Id select attch).FirstOrDefault();
                if (expattch == null)
                {
                    DataModel.sch_expense_attachment att = new DataModel.sch_expense_attachment();
                    att.entered_by = CurrentUser;
                    att.idexpense_trans = Id;
                    att.isactive = true;
                    att.last_updated_by = CurrentUser;
                    att.ts_entered = DateTime.Now;
                    att.ts_updated = DateTime.Now;
                    att.vocher = Vocher;
                    db.sch_expense_attachment.Add(att);
                    db.SaveChanges();
                    return true;
                }
                else
                {
                    expattch.isactive = true;
                    expattch.last_updated_by = CurrentUser;
                    expattch.ts_updated = DateTime.Now;
                    expattch.vocher = Vocher;
                    db.SaveChanges();
                    return true;
                }

            }
            catch (Exception ex)
            {
                throw ex;

            }
        }

    }
    }
