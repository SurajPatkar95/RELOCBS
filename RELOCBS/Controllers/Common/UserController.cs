using PagedList;
using RELOCBS.AjaxHelper;
using RELOCBS.App_Code;
using RELOCBS.BL;
using RELOCBS.BL.Account;
using RELOCBS.Common;
using RELOCBS.CustomAttributes;
using RELOCBS.Entities;
using RELOCBS.Extensions;
using RELOCBS.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Xml.Linq;

namespace RELOCBS.Controllers.Common
{
    [AuthorizeUser]
    public class UserController : BaseController
    {
        UserBL _userBL;
        public UserBL userBL
        {

            get
            {
                if (this._userBL == null)
                    this._userBL = new UserBL();
                return this._userBL;
            }
        }

        ComboBL _comboBL;
        public ComboBL comboBL
        {

            get
            {
                if (this._comboBL == null)
                    this._comboBL = new ComboBL();
                return this._comboBL;
            }
        }

        private string _PageID = "4";

        private CommonSubs _cSubs;
        public CommonSubs CSubs
        {
            get
            {
                if (this._cSubs == null)
                    this._cSubs = new CommonSubs();
                return this._cSubs;

            }
        }

        // GET: User
        public ActionResult Index(int? page)
        {

            if (!CSubs.CheckPageRights(_PageID, PermissionType.VIEW))
            {
                return new HttpStatusCodeResult(403);
            }
            session.Set<string>("PageSession", "User Master");

            var pageIndex = (page ?? 1);
            int pageSize = settings.GetSettingByKey<int>("pagination_pagesize");
            int totalCount = 10;
            BindDropDown();
            string OrderBy = "";
            int Order = 0;
            string SearchKey = string.Empty;
            if (Request.Form["SearchKey"] != null && Request.Form["SearchKey"].Trim() != "")
            {
                SearchKey = Request.Form["SearchKey"];
            }
            if (Request.Params["grid-column"] != null && Request.Params["grid-column"].Trim() != "")
            {
                OrderBy = Request.Params["grid-column"].Trim().ToString();
            }
            if (Request.Params["grid-dir"] != null && Request.Params["grid-column"].Trim() != "")
            {
                Order = Convert.ToInt32(Request.Params["grid-dir"].Trim().ToString());
            }

            var items = userBL.GetUserForGrid(pageIndex, pageSize, OrderBy, Order, SearchKey, UserSession.GetUserSession().LoginID, out totalCount);

            if (totalCount == 0 && pageIndex > 1)
            {
                pageIndex = 1;
                items = userBL.GetUserForGrid(pageIndex, pageSize, OrderBy, Order, SearchKey, UserSession.GetUserSession().LoginID, out totalCount);
            }
            var itemsAsIPagedList = new StaticPagedList<User>(items, pageIndex, pageSize, totalCount);
            return Request.IsAjaxRequest()
                ? (ActionResult)PartialView("_AjaxPartial", itemsAsIPagedList)
                : View(itemsAsIPagedList);
        }


        [HttpPost]
        public JsonResult RestPassword(List<User> Users)
        {
            AjaxResponse response = new AjaxResponse();

            string pwd = string.Empty;
            response.Success = userBL.ResetPassword(Users, out pwd);
            if (response.Success)
            {
                if (!string.IsNullOrEmpty(pwd))
                {
                    response.Message = "Password '" + pwd + "' reset succesfully.";
                }
                else
                {
                    response.Message = "Unable to update";
                }
            }
            else
            {

                response.Message = "Unable to update";
            }

            return Json(response);
        }

        [HttpPost]
        public JsonResult RestAttempt(List<User> Users)
        {
            AjaxResponse response = new AjaxResponse();

            response.Success = userBL.UpdateLoginAttempt(Users);
            if (response.Success)
            {
                response.Message = "Update Successfully";
            }
            else
            {

                response.Message = "Unable to update";
            }

            return Json(response);

        }

        [HttpPost]
        public JsonResult RestLastLogin(List<User> Users)
        {
            AjaxResponse response = new AjaxResponse();

            response.Success = userBL.UpdateLastLogin(Users);
            if (response.Success)
            {
                response.Message = "Update Successfully";
            }
            else
            {

                response.Message = "Unable to update";
            }
            return Json(response);
        }

        [HttpPost]
        public JsonResult CopyUserRights(int CopyFromLoginID, int CopyToLoginID, string OverwriteAppend)
        {
            AjaxResponse response = new AjaxResponse();
            string result = string.Empty;

            response.Success = userBL.CopyUserRights(CopyFromLoginID, CopyToLoginID, OverwriteAppend, out result);

            if (response.Success && !string.IsNullOrEmpty(result))
            {
                response.Message = result;
            }
            else
            {
                response.Message = "Error occured while saving.";
            }
            return Json(response);
        }

        public ActionResult Create(int? id)
        {
            string ContinentID = string.Empty;
            //ViewData["Country"] = userBL.GetCountryByContinent(ContinentID);
            BindDropDown();
            User model = userBL.GetUserDetails(id);
            if (model.LoginID == null || model.LoginID <= 0)
            {
                model.isActive = true;
            }

            return Request.IsAjaxRequest()
                ? (ActionResult)PartialView("Create", model)
                : View(model);
        }

        // POST: City/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(User data)
        {
            try
            {
                //ViewData["StateList"] = cityBL.BindDropdown("StatesByCountryId", "", "");
                //ViewData["Country"] = cityBL.GetCountryByContinent("");
                AjaxResponse result = new AjaxResponse();
                BindDropDown();
                if (ModelState.IsValid)
                {
                    string Message, password, UserId;
                    result.Success = userBL.Insert(data, out Message, out password, out UserId);
                    if (result.Success)
                    {

                        result.Result = this.RenderPartialViewToString("Create", data);
                        if (password != null)
                        {
                            result.Message = "User " + data.LoginText + " is saved successfully\n with password - " + password + "~" + UserId;
                        }
                        else
                        {
                            result.Message = Message + "~" + UserId;
                        }
                        return Json(result);
                    }
                    else
                    {
                        result.Message = Message;
                        ModelState.AddModelError(string.Empty, Message);
                    }
                }
                return Request.IsAjaxRequest()
                              ? (ActionResult)PartialView("Create", data)
                              : View(data);


            }
            catch (Exception ex)
            {
                return View();
            }
        }

        private void BindDropDown()
        {
            ViewData["LoginTypeList"] = comboBL.GetLoginTypeDropdown();
            ViewData["EmployeeList"] = comboBL.GetEmployeeDropdown();

            int totalCount = 0;
            IEnumerable<User> Users = userBL.GetUserForGrid(1, int.MaxValue, null, 0, null, UserSession.GetUserSession().LoginID, out totalCount);
            IEnumerable<SelectListItem> UserList = Users.ToList().ConvertAll(m => { return new SelectListItem() { Value = m.LoginID.ToString(), Text = m.LoginText.ToString() }; });
            ViewData["UserList"] = UserList;
        }

        //{ 1='All';2='Company' }
        public ActionResult GetLocationList(int LoginId, string type)
        {
            var Lists = userBL.GetUserMapping(LoginId);
            /*IEnumerable<MenuRole> menulist = roleBL.GetDetailById(Role);
            userBL.get LoginId*/
            UserLocation LocationList = new UserLocation();
            LocationList.UserID = LoginId;
            LocationList.Type = type;

            SelectListComparer listComparer = new SelectListComparer();

            ViewData["DefCompList"] = comboBL.GetCompanyDropdown().Except(Lists[0], listComparer).ToList();
            ViewData["CompList"] = Lists[0];
            //ViewData["DefCountryList"] = comboBL.GETCountryDropdown().Except(Lists.Item2).ToList();
            //ViewData["CountryList"] = Lists.Item2;
            //ViewData["DefCityList"] = comboBL.GetCityDropdown().Except(Lists.Item3).ToList();
            //ViewData["CityList"] = Lists.Item3;
            string CompIdXml = string.Empty;

            if (Lists[0].Count > 0)
            {
                CompIdXml = new XElement("CompIDs",
               from Comp in Lists[0]
               select new XElement("CompID",
                            new XAttribute("ID", Comp.Value)
                          )).ToString();
            }
            ViewData["DefCityList"] = comboBL.GetCompBranchList(SPTYPE: "COMPWISEBRANCH", CompIDXml: CompIdXml).Except(Lists[1], listComparer).ToList();
            ViewData["CityList"] = Lists[1];
            ViewData["DefRoleList"] = comboBL.GetRoleDropdown().Except(Lists[2], listComparer).ToList();
            ViewData["RoleList"] = Lists[2];
            ViewData["DefBussList"] = CommonService.RMC.Except(Lists[3], listComparer).ToList();
            ViewData["BussList"] = Lists[3];
            ViewData["DefRMCList"] = comboBL.GetRMCDropdown().Except(Lists[4], listComparer).ToList();
            ViewData["RMCList"] = Lists[4];//new List<SelectListItem>();
            ViewData["WarehouseList"] = comboBL.GetWarehouseDropdown(SPTYPE: "ALLFORUSERMAPPING").Except(Lists[5], listComparer).ToList();
            ViewData["MappedWarehouseList"] = Lists[5];//new List<SelectListItem>();
            ViewData["ReportList"] = comboBL.GetReportDropdown().Except(Lists[6], listComparer).ToList();
            ViewData["MappedReportList"] = Lists[6];//new List<SelectListItem>();
            ViewData["ServicelineList"] = comboBL.GetServiceLineDropdown(ForPage: "RoleMap").Except(Lists[7], listComparer).ToList();
            ViewData["MappedServicelineList"] = Lists[7];
            ViewData["rvbranchList"] = comboBL.GetCompBranchList(SPTYPE: "COMPWISEBRANCH", CompIDXml: CompIdXml).Except(Lists[8], listComparer).ToList();
            ViewData["MappedrvbranchList"] = Lists[8];
            ViewData["clickrestrictList"] = comboBL.GetClickRestrictDropdown().Except(Lists[9], listComparer).ToList();
            ViewData["MappedclickrestrictList"] = Lists[9];
            ViewBag.Type = type;
            return PartialView("LocationPartial", LocationList);
        }



        //public ActionResult User()
        //{
        //    /*string a = "10";
        //    String b = "10";


        //    if (a.Equals(b))
        //    {

        //    }
        //    if (a == b)
        //    {

        //    }*/

        //    return View();
        //}

        [HttpPost]
        public ActionResult SaveMapping(UserLocation data)
        {
            try
            {
                BindDropDown();
                //ViewData["StateList"] = cityBL.BindDropdown("StatesByCountryId", "", "");
                //ViewData["Country"] = cityBL.GetCountryByContinent("");
                AjaxResponse result = new AjaxResponse();
                if (ModelState.IsValid)
                {
                    //string Message, password, UserId;
                    result.Success = userBL.SaveMapping(data);
                    if (result.Success)
                    {
                        this.AddToastMessage("RELOCBS", "Mapping saved Succesfully");
                    }
                    else
                    {
                        //ViewData["Country"] = cityBL.GetCountryByContinent("");
                        ModelState.AddModelError(string.Empty, "Error in Saving");
                    }
                }
                /*return Request.IsAjaxRequest()
                              ? (ActionResult)PartialView("Create", data)
                              : View(data);*/
                return RedirectToAction("Index");

            }
            catch (Exception ex)
            {
                return View();
            }

        }

        public ActionResult GetUpdatedUserList()
        {
            BindDropDown();
            IEnumerable<SelectListItem> UserList = (IEnumerable<SelectListItem>)ViewData["UserList"];
            return Json(UserList);
        }
    }
}