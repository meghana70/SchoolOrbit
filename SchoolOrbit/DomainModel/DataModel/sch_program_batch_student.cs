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
    
    public partial class sch_program_batch_student
    {
        public int idBatch { get; set; }
        public int idUser { get; set; }
        public int idSection { get; set; }
        public Nullable<int> idFeePaymentPlan { get; set; }
        public Nullable<int> idDiscount { get; set; }
    
        public virtual sch_fee_discount sch_fee_discount { get; set; }
        public virtual sch_fee_payment_plan sch_fee_payment_plan { get; set; }
        public virtual sch_program_batch sch_program_batch { get; set; }
        public virtual sch_program_section sch_program_section { get; set; }
        public virtual sys_user sys_user { get; set; }
    }
}
