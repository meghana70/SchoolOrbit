using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.BusinessLayer
{

   public  class ExpenseType
    {
       private DataModel.SOEntities db = new DataModel.SOEntities();

        public int id { get; set; }
        public int idSchool { get; set; }
        public string expense_type { get; set; }
        public bool isactive { get; set; }
        public System.DateTime ts_entered { get; set; }
        public System.DateTime ts_updated { get; set; }
        public int last_updated_by { get; set; }
        public int entered_by { get; set; }

        public List<ExpenseType> GetExpenseType(int SchoolId)
        {
            var query = from exp in db.sch_expense_type
                        where exp.idSchool == SchoolId
                        select new ExpenseType()
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
            return query.ToList();
        }


        public ExpenseType GetExpenseTypeDetails(int Transid, int SchoolId)
        {
            var expenseType = (from o in db.sch_expense_type
                               where o.id == Transid && o.idSchool == SchoolId
                              select new ExpenseType
                              {
                                  id = o.id,
                                  expense_type = o.expense_type,
                                  isactive = o.isactive
                              }).SingleOrDefault();

            return expenseType;
        }

        public int AddExpenseType(ExpenseType Exp)
        {
            try
            {
                DataModel.sch_expense_type ExpCat = new DataModel.sch_expense_type();
                ExpCat.entered_by = Exp.entered_by ;
                ExpCat.expense_type = Exp.expense_type;
                ExpCat.idSchool = Exp.idSchool;
                ExpCat.isactive = Exp.isactive;
                ExpCat.last_updated_by = Exp.last_updated_by;
                ExpCat.ts_entered = DateTime.Now;
                ExpCat.ts_updated = DateTime.Now;
                db.sch_expense_type.Add(ExpCat);
                db.SaveChanges();
                return ExpCat.id;
            }

            catch (Exception ex)
            {
                throw ex;

            }

        }

        public int SaveExpenseType(ExpenseType ExpCat)
        {
            try
            {
                var exptrans = db.sch_expense_type.Where(x => x.id == ExpCat.id && x.idSchool == ExpCat.idSchool).FirstOrDefault();
                 exptrans.expense_type = ExpCat.expense_type;
                 exptrans.last_updated_by = ExpCat.last_updated_by;              
                 exptrans.ts_updated = DateTime.Now;
                 exptrans.isactive = ExpCat.isactive;
                 db.SaveChanges();
                 return exptrans.id;
            }

            catch (Exception ex)
            {
                throw ex;

            }

        }


    }


}
