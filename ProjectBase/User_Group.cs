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
    
    public partial class User_Group
    {
        public long Id { get; set; }
        public long User_Id { get; set; }
        public long Group_Id { get; set; }
        public Nullable<System.DateTime> Created_Date { get; set; }
        public string Created_By { get; set; }
        public Nullable<System.DateTime> Edited_Date { get; set; }
        public string Edited_By { get; set; }
        public string Is_Deleted { get; set; }
        public Nullable<System.DateTime> Deleted_Date { get; set; }
        public string Deleted_By { get; set; }
    
        public virtual Group Group { get; set; }
        public virtual User User { get; set; }
    }
}
