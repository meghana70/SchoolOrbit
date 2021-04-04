//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DomainModel.DataModel
{
    using System;
    using System.Collections.Generic;
    
    public partial class sys_school
    {
        public sys_school()
        {
            this.sch_department = new HashSet<sch_department>();
            this.sch_employee = new HashSet<sch_employee>();
            this.sch_employee_docs = new HashSet<sch_employee_docs>();
            this.sch_expense_trans = new HashSet<sch_expense_trans>();
            this.sch_expense_type = new HashSet<sch_expense_type>();
            this.sch_fee_type = new HashSet<sch_fee_type>();
            this.sch_notification = new HashSet<sch_notification>();
            this.sch_program_batch = new HashSet<sch_program_batch>();
            this.sch_program_section = new HashSet<sch_program_section>();
            this.sch_sms_notification = new HashSet<sch_sms_notification>();
            this.sch_student = new HashSet<sch_student>();
            this.sch_teacher = new HashSet<sch_teacher>();
        }
    
        public int idSchool { get; set; }
        public System.DateTime ts_entered { get; set; }
        public Nullable<System.DateTime> ts_updated { get; set; }
        public int idBusiness { get; set; }
        public string school_name { get; set; }
    
        public virtual ICollection<sch_department> sch_department { get; set; }
        public virtual ICollection<sch_employee> sch_employee { get; set; }
        public virtual ICollection<sch_employee_docs> sch_employee_docs { get; set; }
        public virtual ICollection<sch_expense_trans> sch_expense_trans { get; set; }
        public virtual ICollection<sch_expense_type> sch_expense_type { get; set; }
        public virtual ICollection<sch_fee_type> sch_fee_type { get; set; }
        public virtual ICollection<sch_notification> sch_notification { get; set; }
        public virtual ICollection<sch_program_batch> sch_program_batch { get; set; }
        public virtual ICollection<sch_program_section> sch_program_section { get; set; }
        public virtual ICollection<sch_sms_notification> sch_sms_notification { get; set; }
        public virtual ICollection<sch_student> sch_student { get; set; }
        public virtual ICollection<sch_teacher> sch_teacher { get; set; }
        public virtual sys_business sys_business { get; set; }
    }
}