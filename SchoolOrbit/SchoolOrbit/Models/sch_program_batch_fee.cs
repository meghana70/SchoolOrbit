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
    
    public partial class sch_program_batch_fee
    {
        public int idBatch { get; set; }
        public int idFeeType { get; set; }
        public Nullable<decimal> fee { get; set; }
        public int idAcademicYear { get; set; }
    
        public virtual sch_fee_type sch_fee_type { get; set; }
        public virtual sch_program_batch sch_program_batch { get; set; }
        public virtual sys_academic_year sys_academic_year { get; set; }
    }
}
