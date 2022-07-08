using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RELOCBS.Entities
{
    public class Role
    {
        [Key]
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public string HFGridValue { get; set; }
    }

    public class MenuRole 
    {
        public int MenuId { get; set; }
        public string MenuName { get; set; }
        public string SubMenuName { get; set; }
        public string ParentMenuName { get; set; }
        public String IsActive { get; set; }
        public bool Allow_Edit { get; set; }
        public bool Allow_View { get; set; }
        public bool Allow_Delete { get; set; }
        public bool Allow_Add { get; set; }
    }
}