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
    
    public partial class sch_teacher
    {
        public int idSchool { get; set; }
        public int idUser { get; set; }
        public int idSubject { get; set; }
    
        public virtual sys_school sys_school { get; set; }
    }
}
