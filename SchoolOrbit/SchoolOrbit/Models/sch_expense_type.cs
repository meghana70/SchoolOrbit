//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SchoolOrbit.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class sch_expense_type
    {
        public sch_expense_type()
        {
            this.sch_expense_trans = new HashSet<sch_expense_trans>();
        }
    
        public int id { get; set; }
        public int idSchool { get; set; }
        public string expense_type { get; set; }
        public bool isactive { get; set; }
        public System.DateTime ts_entered { get; set; }
        public System.DateTime ts_updated { get; set; }
        public int last_updated_by { get; set; }
        public int entered_by { get; set; }
    
        public virtual sys_school sys_school { get; set; }
        public virtual ICollection<sch_expense_trans> sch_expense_trans { get; set; }
    }
}
