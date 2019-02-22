using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RELOCBS.Entities
{
    public class User : SysUser
    {
        public User()
        {
            
        }
    }

    public partial class SysUser 
    {
        [Key]
        public int UserID { get; set; }
        public string LoginID { get; set; }
        public byte[] Password { get; set; }
        public int LanguageID { get; set; }
        public Nullable<int> UserTypeID { get; set; }
        public Nullable<int> TimeZoneID { get; set; }
        public Nullable<bool> isActive { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> LastLoginDate { get; set; }
        public string MobileNo { get; set; }

        [Display(Name = "Activated Date")]
        public Nullable<System.DateTime> ActivatedOn { get; set; }

        [Display(Name = "Deactivated Date")]
        public Nullable<System.DateTime> DeactivatedOn { get; set; }

    }
    
    
}