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
    
    public partial class sys_academic_year
    {
        public sys_academic_year()
        {
            this.sch_program_batch_fee = new HashSet<sch_program_batch_fee>();
        }
    
        public int idAcademicYear { get; set; }
    
        public virtual ICollection<sch_program_batch_fee> sch_program_batch_fee { get; set; }
    }
}
