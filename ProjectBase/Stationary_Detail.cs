//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ProjectBase
{
    using System;
    using System.Collections.Generic;
    
    public partial class Stationary_Detail
    {
        public long Id { get; set; }
        public Nullable<long> Book_Id { get; set; }
        public Nullable<long> Stationary_Id { get; set; }
        public Nullable<int> Book_Qty { get; set; }
        public string Book_Authors { get; set; }
        public System.DateTime Create_Date { get; set; }
        public string Create_By { get; set; }
        public Nullable<System.DateTime> Edit_Date { get; set; }
        public string Edit_By { get; set; }
        public Nullable<System.DateTime> Delete_Date { get; set; }
        public string Delete_By { get; set; }
        public string Is_Delete { get; set; }
    
        public virtual Book Book { get; set; }
        public virtual Stationary Stationary { get; set; }
    }
}
