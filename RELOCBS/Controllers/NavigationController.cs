using RELOCBS.CustomAttributes;
using RELOCBS.Entities;
using RELOCBS.Models;
using RELOCBS.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RELOCBS.Controllers
{
    [AuthorizeUser]
    public class NavigationController : Controller
    {
        //
        // GET: /Navigation/
        public ActionResult NavLeft()
        {
            //List<MenuViewModel> menuViewModel = new List<MenuViewModel>();

            //MenuViewModel menu = new MenuViewModel() { MenuID = 1, Action = "Index", Controller = "Home", IsAction = true, Class = "class", SubMenu = null, Title = "Dashboard",ParentMenuID=0, IMGPath= "fa fa-th-large" };
            //menuViewModel.Add(menu);

            //menu = new MenuViewModel() { MenuID = 2, IsAction = false, Class = "class", Link = "javascript:void(0);", Title = "Application Setup", ParentMenuID = 0, IMGPath = "fa fa-th-large" };

            //menu.SubMenu = new List<MenuViewModel>();
            //MenuViewModel subMenu = new MenuViewModel() { Action = "Register", Controller = "Account", IsAction = true, Class = "", SubMenu = null, Title = "User Manager", ParentMenuID = 2, IMGPath = "fa fa-th-large" };
            //menu.SubMenu.Add(subMenu);

            //subMenu = new MenuViewModel() { Action = "Index", Controller = "Manage", IsAction = true, Class = "", SubMenu = null, Title = "Manage" , ParentMenuID = 2, IMGPath = "fa fa-th-large" };
            //menu.SubMenu.Add(subMenu);

            //subMenu = new MenuViewModel() { Action = "ChangePassword", Controller = "Manage", IsAction = true, Class = "", SubMenu = null, Title = "Change Password", ParentMenuID = 2, IMGPath = "fa fa-th-large" };
            //menu.SubMenu.Add(subMenu);

            //subMenu = new MenuViewModel() { IsAction = false, Link = "javascript:document.getElementById('logoutForm').submit()", Class = "", SubMenu = null, Title = "Logoff", ParentMenuID = 2, IMGPath = "fa fa-th-large" };
            //menu.SubMenu.Add(subMenu);

            //menuViewModel.Add(menu);


            return PartialView(UserSession.GetUserMenu());

            //return PartialView();

        }


        public ActionResult NavTop()
        {

            return PartialView();
        }

        public ActionResult Notifications()
        {
            return PartialView();
        }

        public ActionResult ShowNotification(int? id)
        {
            //IEnumerable<Notification> objTD = _spLeadService.Notification(_commonServices.WorkContext.CurrentUser.UserID);
            //return Request.IsAjaxRequest()
            //    ? (ActionResult)PartialView("_ShowNotification", objTD)
            //    : View(objTD);

            return PartialView();
        }

        public ActionResult NotificationCount(int? id)
        {
            //NotificationCount objTD = _spLeadService.NotificationCount(_commonServices.WorkContext.CurrentUser.UserID);
            //return Request.IsAjaxRequest()
            //    ? (ActionResult)PartialView("_NotificationCount", objTD)
            //    : View(objTD);

            return PartialView();
        }

    }
}
