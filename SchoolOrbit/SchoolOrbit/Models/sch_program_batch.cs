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
    
    public partial class sch_program_batch
    {
        public sch_program_batch()
        {
            this.sch_program_batch_fee = new HashSet<sch_program_batch_fee>();
            this.sch_program_batch_student = new HashSet<sch_program_batch_student>();
        }
    
        public int idBatch { get; set; }
        public int idSchool { get; set; }
        public int idProgram { get; set; }
        public System.DateTime date_start { get; set; }
        public System.DateTime date_end { get; set; }
        public bool is_active { get; set; }
        public int year { get; set; }
    
        public virtual sys_school sys_school { get; set; }
        public virtual ICollection<sch_program_batch_fee> sch_program_batch_fee { get; set; }
        public virtual ICollection<sch_program_batch_student> sch_program_batch_student { get; set; }
        public virtual sys_program sys_program { get; set; }
    }
}
