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
    
    public partial class webpages_Roles
    {
        public webpages_Roles()
        {
            this.UserProfiles = new HashSet<UserProfile>();
        }
    
        public int RoleId { get; set; }
        public string RoleName { get; set; }
    
        public virtual ICollection<UserProfile> UserProfiles { get; set; }
    }
}
