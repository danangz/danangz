//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DaNangZ.DbFirst.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Category
    {
        public Category()
        {
            this.Entries = new HashSet<Entry>();
        }
    
        public int Id { get; set; }
        public string CategoryName { get; set; }
        public string StatusId { get; set; }
        public string InsBy { get; set; }
        public System.DateTime InsAt { get; set; }
        public string UpdBy { get; set; }
        public Nullable<System.DateTime> UpdAt { get; set; }
    
        public virtual ICollection<Entry> Entries { get; set; }
    }
}
