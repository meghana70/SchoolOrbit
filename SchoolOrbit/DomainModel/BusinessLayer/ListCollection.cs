using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace DomainModel.BusinessLayer
{
    public class ListCollection
    {
        private DataModel.SOEntities db = new DataModel.SOEntities();
        public class lstItem
        {
            public string lstValue { get; set; }
            public string lstText { get; set; }
        }
        public List<string> LoadCountry()
        {
            var query = from con in db.sys_country select con.country_name;
            return query.ToList();
        }
        public List<string> LoadState()
        {

            var query = from c in db.sys_states
                        select c.state_name;

            return query.ToList();
        }
        public List<lstItem> LoadGender()
        {

            List<lstItem> lstGender = new List<lstItem>()
            {             
                new lstItem(){lstValue="M",lstText="Male"},
                new lstItem(){lstValue="F",lstText="Female"}
            };
            return lstGender.ToList();
        }
        public List<lstItem> LoadDepartments(int SchoolId)
        {
            var lstDept = (from o in db.sch_department
                           where o.idSchool == SchoolId
                           orderby o.department_name
                           select new lstItem()
                           {
                               lstValue = o.idDepartment.ToString(),
                               lstText = o.department_name
                           });
            return lstDept.ToList();

        }
        public Dictionary<int, string> LoadExpenseType(int idSchool)
        {
            var list = db.sch_expense_type.Where(d => d.idSchool == idSchool && d.isactive == true).ToList().Select(x => new { x.id, x.expense_type });
            Dictionary<int, string> lst = new Dictionary<int, string>();
            foreach (var li in list)
            {
                lst.Add(li.id, li.expense_type);
            }
            return lst;
        }
    }
}
