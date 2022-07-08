using PagedList;
using RELOCBS.AjaxHelper;
using RELOCBS.App_Code;
using RELOCBS.BL;
using RELOCBS.BL.WOSCommon;
using RELOCBS.Common;
using RELOCBS.CustomAttributes;
using RELOCBS.Entities;
using RELOCBS.Extensions;
using RELOCBS.Utility;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace RELOCBS.Controllers
{
    [AuthorizeUser]
    public class WOSServiceController : BaseController
    {
        private string _PageID = "76";

        private CommonSubs _CSubs;
        public CommonSubs CSubs
        {
            get
            {
                if (_CSubs == null)
                    _CSubs = new CommonSubs();
                return _CSubs;
            }
        }

        private ComboBL _ComboBL;
        public ComboBL ComboBL
        {
            get
            {
                if (_ComboBL == null)
                    _ComboBL = new ComboBL();
                return _ComboBL;
            }
        }

        private WOSComboBL _WOSComboBL;
        public WOSComboBL WOSComboBL
        {
            get
            {
                if (_WOSComboBL == null)
                    _WOSComboBL = new WOSComboBL();
                return _WOSComboBL;
            }
        }

        private CommanBL _ComBL;
        public CommanBL ComBL
        {
            get
            {
                if (_ComBL == null)
                    _ComBL = new CommanBL();
                return _ComBL;
            }
        }

        private WOSServiceBL _WOSServiceBL;
        public WOSServiceBL WOSServiceBL
        {
            get
            {
                if (_WOSServiceBL == null)
                    _WOSServiceBL = new WOSServiceBL();
                return _WOSServiceBL;
            }
        }

        WOSService WOSServiceObj = new WOSService();
        public WOSServiceController(WOSService WOSService)
        {
            this.WOSServiceObj = WOSService;
        }

        public ActionResult Index(int Page = 1)
        {
            try
            {
                if (!CSubs.CheckPageRights(_PageID, PermissionType.VIEW))
                {
                    return new HttpStatusCodeResult(403);
                }

                session.Set("PageSession", "WOS Service Master");

                int? ServiceMastID = null;
                string Sort = "ServiceMastID";
                string SortDir = "asc";
                int PageSize = settings.GetSettingByKey<int>("pagination_pagesize");
                int Order = 0;

                if (Request.Form["ServiceMastID"] != null && Request.Form["ServiceMastID"].Trim() != "")
                    ServiceMastID = Convert.ToInt32(Request.Form["ServiceMastID"]);

                if (Request.Params["grid-column"] != null && Request.Params["grid-column"].Trim() != "")
                    Sort = Request.Params["grid-column"].Trim().ToString();

                if (Request.Params["grid-dir"] != null && Request.Params["grid-dir"].Trim() != "")
                {
                    Order = Convert.ToInt32(Request.Params["grid-dir"].Trim().ToString());
                    if (Order == 1)
                        SortDir = "asc";
                    else
                        SortDir = "desc";
                }

                int TotalRecord = 0;
                if (Page < 1) Page = 1;
                int Skip = (Page * PageSize) - PageSize;

                var WOSServiceList = WOSServiceBL.GetWOSServiceList(Sort, SortDir, Skip, PageSize, out TotalRecord);

                var ItemsAsIPagedList = new StaticPagedList<WOSService>(WOSServiceList, Page, PageSize, TotalRecord);

                return Request.IsAjaxRequest() ? (ActionResult)PartialView("_GridPartial", ItemsAsIPagedList) : View(ItemsAsIPagedList);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpGet]
        public ActionResult Create(string Key)
        {
            try
            {
                var list = CommonService.GetQueryString(Key);

                int? ServiceMastID = null;
                if (list.ContainsKey("ServiceMastID"))
                    ServiceMastID = Convert.ToInt32(list["ServiceMastID"]);

                WOSService WOSServiceObj = WOSServiceBL.GetWOSServiceById(ServiceMastID);

                return Request.IsAjaxRequest() ? (ActionResult)PartialView("Create", WOSServiceObj) : View(WOSServiceObj);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult Create(WOSService WOSServiceObj)
        {
            try
            {
                string message = string.Empty;
                string Status = string.Empty;

                AjaxResponse result = new AjaxResponse();

                if (ModelState.IsValid)
                {
                    result.Success = WOSServiceBL.SaveWOSService(WOSServiceObj, out message);
                    if (result.Success)
                    {
                        result.Message = message;
                        result.Result = this.RenderPartialViewToString("Create", WOSServiceObj);
                        return Json(result);
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, message);
                    }
                }
                return Request.IsAjaxRequest() ? (ActionResult)PartialView("Create", WOSServiceObj) : View(WOSServiceObj);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public ActionResult ExportToExcel()
        {
            try
            {
                Dictionary<string, string> param = new Dictionary<string, string>();
                try
                {
                    string SearchKey = string.Empty;
                    if (Request.Form["ServiceName"] != null && Request.Form["ServiceName"].Trim() != "")
                        param.Add("@SP_ServiceName", Request.Form["ServiceName"]);

                    param.Add("@SP_LoginID", Convert.ToString(UserSession.GetUserSession().LoginID));

                    CommonService.GenerateExcel(this.Response, "Agent", "[Comm].[GETAgentForGrid_ExpToExl]", param);
                }
                catch (Exception ex)
                {
                    this.AddToastMessage("RELOCBS", "UnExpected Error occured", ToastType.Error);
                }
                return View();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void GetDropDownLists()
        {
            ViewData["ServiceList"] = WOSComboBL.GetWOSServiceDropdown();
        }
    }
}