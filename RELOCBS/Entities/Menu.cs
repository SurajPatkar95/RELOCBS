using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RELOCBS.Entities
{
    public partial class UserMenu : Entity
    {
        [Key]
        public Int64 RN { get; set; }
        public int ModuleID { get; set; }
        public string DisName { get; set; }
        public int HierarchyLEVEL { get; set; }
        public string MenuNode { get; set; }
        public string ParentNode { get; set; }
        public int MenuID { get; set; }
        public string DisplayName { get; set; }
        public string ControlName { get; set; }
        public string URL { get; set; }
        public Nullable<int> Sequence { get; set; }
        public string IMGPath { get; set; }
    }


    public partial class UserMenuPage : Entity
    {
        [Key]
        public int ModuleID { get; set; }
        public string DisName { get; set; }
        public int MenuID { get; set; }
        public string DisplayName { get; set; }
        public string ControlName { get; set; }
        public string URL { get; set; }
        public Nullable<int> Sequence { get; set; }
        public string IMGPath { get; set; }
        public int PageID { get; set; }
        public string PageTitle { get; set; }
        public bool IsDefaultPage { get; set; }
    }


    public partial class UserMenuPageAction : Entity
    {
        public int PageID { get; set; }
        public string PageTitle { get; set; }

        [Key]
        public int ActionID { get; set; }
        public string btncaption { get; set; }
        public string IMGPath { get; set; }
        public bool ISBasic { get; set; }
        public Int16 ActionGroup { get; set; }
    }
}