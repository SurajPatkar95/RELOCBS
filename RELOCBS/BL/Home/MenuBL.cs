using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using RELOCBS.DAL.Home;
using RELOCBS.Models;
using RELOCBS.Utility;

namespace RELOCBS.BL.Home
{
    public class MenuBL
    {

        RELOCBS.App_Code.CommonSubs _CSubs;

        public RELOCBS.App_Code.CommonSubs CSubs
        {
            get
            {
                if (this._CSubs == null)
                    this._CSubs = new RELOCBS.App_Code.CommonSubs();
                return this._CSubs;
            }
        }

        MenuDAL _menuDAL;

        public MenuDAL menuDAL
        {
            get
            {
                if (this._menuDAL == null)
                    this._menuDAL = new MenuDAL();
                return this._menuDAL;
            }
        }

        public List<MenuViewModel> CreateGenericMenu(string LoginID)
        {
            List<MenuViewModel> menuViewModelList = new List<MenuViewModel>();
            try
            {
                DataTable LocalDataSet = (DataTable)menuDAL.InitializeMenu(LoginID);

                if (LocalDataSet != null && LocalDataSet.Rows.Count > 0)
                {

                    #region All menu mapping to model
                    //DataRow[] drRootElements = LocalDataSet.Select();
                    //for (Int32 I = 0; I < drRootElements.Length; I++)
                    //{

                    //    string link = drRootElements[I]["MENULINK"].ToString();
                    //    if (link.StartsWith("https://") || link.StartsWith("https://"))
                    //    {
                    //        if (link.ToString().Contains("?"))
                    //            link = link + "&RELOCBS=" + CSubs.EncryptURL(LoginID);
                    //        else
                    //            link = link + "?RELOCBS=" + CSubs.EncryptURL(LoginID);
                    //    }

                    //    MenuViewModel menuViewModel = new MenuViewModel();
                    //    menuViewModel.MenuID = Convert.ToInt32(drRootElements[I]["MENUID"]);
                    //    menuViewModel.Link = link;
                    //    menuViewModel.Title = drRootElements[I]["MENUNAME"].ToString();
                    //    menuViewModel.Value = "Mnu-" + drRootElements[I]["MENUID"].ToString();
                    //    menuViewModel.ParentMenuID = Convert.ToInt32(drRootElements[I]["PARENTMENUID"]);
                    //    menuViewModel.IMGPath = Convert.ToString(drRootElements[I]["IMGPATH"]);
                    //    menuViewModel.Controller = Convert.ToString(drRootElements[I]["CONTROLLER"]);



                    //    if ((bool)drRootElements[I]["ISBLANK"])
                    //        menuViewModel.Target = "_blank";

                    //    //CreateMenuStructure(ref menuViewModel, LocalDataSet, LoginID);

                    //    if ((menuViewModel.SubMenu != null && menuViewModel.SubMenu.Count > 0) || menuViewModel.Link != "#")
                    //        if ((bool)drRootElements[I]["SHOWINMENU"] && (Int32)drRootElements[I]["ALLOW_VIEW"] == 1)
                    //            menuViewModelList.Add(menuViewModel);


                    //}
                    #endregion


                    #region
                    //////Setting the User Menu
                    UserSession.SetUserMenuTable(LocalDataSet);


                    //////Generate the tree structured Menu
                    DataRow[] drRootElements = LocalDataSet.Select("PARENTMENUID=0");
                    for (Int32 I = 0; I < drRootElements.Length; I++)
                    {
                        string link = drRootElements[I]["MENULINK"].ToString();
                        if (link.StartsWith("https://") || link.StartsWith("https://"))
                        {
                            if (link.ToString().Contains("?"))
                                link = link + "&RELOCBS=" + CSubs.EncryptURL(LoginID);
                            else
                                link = link + "?RELOCBS=" + CSubs.EncryptURL(LoginID);
                        }

                        MenuViewModel menuViewModel = new MenuViewModel();
                        menuViewModel.MenuID = Convert.ToInt32(drRootElements[I]["MENUID"]);
                        menuViewModel.Link = link;
                        menuViewModel.Title = drRootElements[I]["MENUNAME"].ToString();
                        menuViewModel.Value = "Mnu-" + drRootElements[I]["MENUID"].ToString();
                        menuViewModel.ParentMenuID = Convert.ToInt32(drRootElements[I]["PARENTMENUID"]);
                        menuViewModel.IMGPath = Convert.ToString(drRootElements[I]["IMGPATH"]);
                        menuViewModel.Controller = Convert.ToString(drRootElements[I]["CONTROLLER"]);



                        if ((bool)drRootElements[I]["ISBLANK"])
                            menuViewModel.Target = "_blank";

                        CreateMenuStructure(ref menuViewModel, LocalDataSet, LoginID);

                        if ((menuViewModel.SubMenu != null && menuViewModel.SubMenu.Count > 0) || menuViewModel.Link != "#")
                            if ((bool)drRootElements[I]["SHOWINMENU"] && (Int32)drRootElements[I]["ALLOW_VIEW"] == 1)
                                menuViewModelList.Add(menuViewModel);
                    }

                    #endregion
                }
            }
            catch (System.Threading.ThreadAbortException) { }   
            catch (Exception ex)
            {

                menuViewModelList = new List<MenuViewModel>();
                CSubs.LogError(this, "CreateGenericMenu", ex.ToString());
                //throw;
            }
            
            return menuViewModelList;
        }

        private void CreateMenuStructure(ref MenuViewModel menuViewModel, DataTable prmMenuTable, string LoginID)
        {
            DataRow[] DR = prmMenuTable.Select("PARENTMENUID=" + menuViewModel.Value.Split('-')[1]);
            for (Int32 I = 0; I < DR.Length; I++)
            {
                string link = DR[I]["MENULINK"].ToString();
                if (link.StartsWith("http://") || link.StartsWith("https://"))
                {
                    if (link.ToString().Contains("?"))
                        link = link + "&RELOCBS=" + CSubs.EncryptURL(LoginID);
                    else
                        link = link + "?RELOCBS=" + CSubs.EncryptURL(LoginID);
                }

                MenuViewModel menuViewModelChild = new MenuViewModel();
                menuViewModelChild.MenuID = Convert.ToInt32(DR[I]["MENUID"]);
                menuViewModelChild.Link = link;
                menuViewModelChild.Title = DR[I]["MENUNAME"].ToString();
                menuViewModelChild.Value = "Mnu-" + DR[I]["MENUID"].ToString();
                menuViewModelChild.ParentMenuID = Convert.ToInt32(DR[I]["PARENTMENUID"]);
                menuViewModelChild.IMGPath = Convert.ToString(DR[I]["IMGPATH"]);
                menuViewModelChild.Controller = Convert.ToString(DR[I]["CONTROLLER"]);


                if ((bool)DR[I]["ISBLANK"])
                    menuViewModelChild.Target = "_blank";

                CreateMenuStructure(ref menuViewModelChild, prmMenuTable, LoginID);

                if ((menuViewModelChild.SubMenu != null && menuViewModelChild.SubMenu.Count > 0) || menuViewModelChild.Link != "#")
                    if ((bool)DR[I]["SHOWINMENU"] && (Int32)DR[I]["ALLOW_VIEW"] == 1)
                         //menuViewModel.SubMenu = new List<MenuViewModel>();
                         menuViewModel.SubMenu.Add(menuViewModelChild);
            }
        }

    }
}