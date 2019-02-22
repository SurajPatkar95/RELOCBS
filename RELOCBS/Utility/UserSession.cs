using RELOCBS.BL.Home;
using RELOCBS.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using RELOCBS.App_Code;

namespace RELOCBS.Utility
{
    public class UserSession
    {
        

        public static UserInformationModel GetUserSession()
        {
            //var session = HttpContext.Current.Session;

            CustomSessionStore _usersession = new CustomSessionStore();

            if (_usersession.Get<UserInformationModel>("UserSession") != null)
            {
                return _usersession.Get<UserInformationModel>("UserSession");
            }
            
            return null;
        }

        public static List<MenuViewModel> GetUserMenu()
        {
            List<MenuViewModel> DummyMenuObj = new List<MenuViewModel>();
            CustomSessionStore _usersession = new CustomSessionStore();

            if (_usersession.Get<List<MenuViewModel>>("UserMenu") != null)
            {
                return _usersession.Get<List<MenuViewModel>>("UserMenu");
            }

            return DummyMenuObj;
        }

        public static void SetUserMenu(String LoginID)
        {
            MenuBL _menuBL = new MenuBL();
            List<MenuViewModel> DummyMenuObj = new List<MenuViewModel>();

            CustomSessionStore _usersession = new CustomSessionStore();
            //if (_usersession.Get<List<MenuViewModel>>("UserMenu") == null)
            {
                _usersession.Set<List<MenuViewModel>>("UserMenu", _menuBL.CreateGenericMenu(LoginID));
            }


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



        }

        public static DataTable GetUserMenuTable()
        {
            CustomSessionStore _usersession = new CustomSessionStore();
            DataTable DummyMenuObj = new DataTable();

            if (_usersession.Get<DataTable>("UserMenuTable") != null)
            {
                return _usersession.Get<DataTable>("UserMenuTable");
            }

            return DummyMenuObj;

        }

        public static void SetUserMenuTable(DataTable MenuDt)
        {
            CustomSessionStore _usersession = new CustomSessionStore();
            //if (_usersession.Get<List<MenuViewModel>>("UserMenu") == null)
            {
                _usersession.Set<DataTable>("UserMenuTable", MenuDt);
            }

        }

        public static bool HasPermission(string _PageID, EnumUtility.PageAction permissionType)
        {
            CommonSubs CSubs = new CommonSubs();

            PermissionType value2 = (PermissionType)permissionType;

            if (CSubs.CheckPageRights(_PageID, value2))
            {
                return true;
            }

            return false;
        }

    }
}