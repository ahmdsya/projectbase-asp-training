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
    
    public partial class Order
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Order()
        {
            this.Order_Detail = new HashSet<Order_Detail>();
        }
    
        public long Id { get; set; }
        public string Order_No { get; set; }
        public System.DateTime Order_Date { get; set; }
        public long Order_Customer_Id { get; set; }
        public string Order_Customer_Address { get; set; }
        public System.DateTime Create_Date { get; set; }
        public string Create_By { get; set; }
        public Nullable<System.DateTime> Edit_Date { get; set; }
        public string Edit_By { get; set; }
        public Nullable<bool> Is_Deleted { get; set; }
    
        public virtual Customer Customer { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Order_Detail> Order_Detail { get; set; }
    }
}