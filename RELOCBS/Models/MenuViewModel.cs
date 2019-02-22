using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RELOCBS.Models
{
    public class MenuViewModel
    {
        public MenuViewModel()
        {
            SubMenu = new List<MenuViewModel>();
        }


        public int MenuID { get; set; }
        public string Title { get; set; }
        public string Action { get; set; }
        public string Controller { get; set; }
        public bool IsAction { get; set; }
        public string Link { get; set; }
        public string Class { get; set; }
        public string Target { get; set; }
        public string Value { get; set; }
        public string IMGPath { get; set; }
        public int ParentMenuID { get; set; }
        public List<MenuViewModel> SubMenu { get; set; }

    }
}