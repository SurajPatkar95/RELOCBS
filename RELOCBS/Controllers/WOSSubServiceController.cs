using PagedList;
using RELOCBS.AjaxHelper;
using RELOCBS.App_Code;
using RELOCBS.BL;
using RELOCBS.BL.WOSCommon;
using RELOCBS.Common;
using RELOCBS.CustomAttributes;
using RELOCBS.Entities;
using RELOCBS.Extensions;
using System;
using System.Web.Mvc;

namespace RELOCBS.Controllers
{
    [AuthorizeUser]
    public class WOSSubServiceController : BaseController
    {
        private string _PageID = "77";

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

        private WOSSubServiceBL _WOSSubServiceBL;
        public WOSSubServiceBL WOSSubServiceBL
        {
            get
            {
                if (_WOSSubServiceBL == null)
                    _WOSSubServiceBL = new WOSSubServiceBL();
                return _WOSSubServiceBL;
            }
        }

        WOSSubService WOSSubServiceObj = new WOSSubService();
        public WOSSubServiceController(WOSSubService WOSSubService)
        {
            this.WOSSubServiceObj = WOSSubService;
        }

        public ActionResult Index(int Page = 1)
        {
            try
            {
                if (!CSubs.CheckPageRights(_PageID, PermissionType.VIEW))
                {
                    return new HttpStatusCodeResult(403);
                }

                session.Set("PageSession", "WOS Sub Service Master");

                int? SubServiceMastID = null;
                string Sort = "SubServiceMastID";
                string SortDir = "asc";
                int PageSize = settings.GetSettingByKey<int>("pagination_pagesize");
                int Order = 0;

                if (Request.Form["SubServiceMastID"] != null && Request.Form["SubServiceMastID"].Trim() != "")
                    SubServiceMastID = Convert.ToInt32(Request.Form["SubServiceMastID"]);

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

                var WOSSubServiceList = WOSSubServiceBL.GetWOSSubServiceList(Sort, SortDir, Skip, PageSize, out TotalRecord);

                var ItemsAsIPagedList = new StaticPagedList<WOSSubService>(WOSSubServiceList, Page, PageSize, TotalRecord);

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
                int? SubServiceMastID = null;
                if (list.ContainsKey("SubServiceMastID"))
                    SubServiceMastID = Convert.ToInt32(list["SubServiceMastID"]);

                GetDropDownLists();
                WOSSubService WOSSubServiceObj = WOSSubServiceBL.GetWOSSubServiceById(SubServiceMastID);

                return Request.IsAjaxRequest() ? (ActionResult)PartialView("Create", WOSSubServiceObj) : View(WOSSubServiceObj);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult Create(WOSSubService WOSSubServiceObj)
        {
            try
            {
                string message = string.Empty;
                string Status = string.Empty;

                AjaxResponse result = new AjaxResponse();

                GetDropDownLists();

                if (ModelState.IsValid)
                {
                    result.Success = WOSSubServiceBL.SaveWOSSubService(WOSSubServiceObj, out message);
                    if (result.Success)
                    {
                        result.Message = message;
                        result.Result = this.RenderPartialViewToString("Create", WOSSubServiceObj);
                        return Json(result);
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, message);
                    }
                }
                return Request.IsAjaxRequest() ? (ActionResult)PartialView("Create", WOSSubServiceObj) : View(WOSSubServiceObj);
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