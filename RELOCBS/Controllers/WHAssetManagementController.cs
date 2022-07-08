using PagedList;
using RELOCBS.AjaxHelper;
using RELOCBS.App_Code;
using RELOCBS.BL;
using RELOCBS.BL.WHAssetManagement;
using RELOCBS.Common;
using RELOCBS.CustomAttributes;
using RELOCBS.Entities;
using RELOCBS.Extensions;
using RELOCBS.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;

namespace RELOCBS.Controllers
{
    [AuthorizeUser]
    public class WHAssetManagementController : BaseController
    {
        private string _PageID = "87";

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

        private WHAssetManagementBL _WHAssetManagementBL;
        public WHAssetManagementBL WHAssetManagementBL
        {
            get
            {
                if (_WHAssetManagementBL == null)
                    _WHAssetManagementBL = new WHAssetManagementBL();
                return _WHAssetManagementBL;
            }
        }

        WHAssetMaster _WHAssetMaster = new WHAssetMaster();
        public WHAssetManagementController(WHAssetMaster WHAssetMasterObj)
        {
            _WHAssetMaster = WHAssetMasterObj;
        }

        public ActionResult Index(int Page = 1)
        {
            try
            {
                if (!CSubs.CheckPageRights(_PageID, PermissionType.VIEW))
                {
                    return new HttpStatusCodeResult(403);
                }

                session.Set("PageSession", "WH InOut Asset Index");
                string Sort = "MoveID";
                string SortDir = "desc";
                string Search = string.Empty;
                string SearchType = string.Empty;

                int PageSize = settings.GetSettingByKey<int>("pagination_pagesize");
                int Order = 0;
                DateTime? FromDate = null;
                DateTime? ToDate = null;
                string JobID = string.Empty;
                string RefJobID = string.Empty;

                if (Request.Form["FromDate"] != null && Request.Form["FromDate"].Trim() != "")
                    FromDate = Convert.ToDateTime(Request.Form["FromDate"]);

                if (Request.Form["ToDate"] != null && Request.Form["ToDate"].Trim() != "")
                    ToDate = Convert.ToDateTime(Request.Form["ToDate"]);

                if (Request.Form["JobID"] != null && Request.Form["JobID"].Trim() != "")
                    JobID = Convert.ToString(Request.Form["JobID"]);

                if (Request.Form["RefJobID"] != null && Request.Form["RefJobID"].Trim() != "")
                    RefJobID = Convert.ToString(Request.Form["RefJobID"]);

                if (Request.Form["SearchValue"] != null && Request.Form["SearchValue"].Trim() != "")
                    Search = Convert.ToString(Request.Form["SearchValue"]);

                if (Request.Form["SearchType"] != null && Request.Form["SearchType"].Trim() != "")
                    SearchType = Convert.ToString(Request.Form["SearchType"]);

                if (Request.Params["grid-column"] != null && Request.Params["grid-column"].Trim() != "")
                    Sort = Request.Params["grid-column"].Trim().ToString();

                if (Request.Params["grid-dir"] != null && Request.Params["grid-dir"].Trim() != "")
                {
                    Order = Convert.ToInt32(Request.Params["grid-dir"].Trim().ToString());
                    if (Order == 1) SortDir = "asc";
                }

                int TotalRecord = 0;
                if (Page < 1) Page = 1;
                int Skip = (Page * PageSize) - PageSize;

                int LoginID = UserSession.GetUserSession()?.LoginID ?? 0;

                var WHInOutAssetList = WHAssetManagementBL.GetWHInOutAssetList(LoginID, FromDate, ToDate, JobID, RefJobID, Search, SearchType, Sort, SortDir, Skip, PageSize, out TotalRecord);

                var itemsAsIPagedList = new StaticPagedList<WHInOutAssetMaster>(WHInOutAssetList, Page, PageSize, TotalRecord);

                return Request.IsAjaxRequest() ? (ActionResult)PartialView("_AjaxPartial", itemsAsIPagedList) : View(itemsAsIPagedList);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpGet]
        public ActionResult Create(Int64? MoveID)
        {
            try
            {
                session.Set("PageSession", "WH InOut Asset");

                int LoginID = UserSession.GetUserSession()?.LoginID ?? 0;

                _WHAssetMaster = WHAssetManagementBL.GetAssetInwardDetails(LoginID, MoveID, null, null);

                _WHAssetMaster.BingoSheetDataSet = WHAssetManagementBL.GetBingoChart(LoginID, _WHAssetMaster.WHInAssetMaster.MoveID);

                GetDropDownLists(MoveID: _WHAssetMaster.WHInAssetMaster.MoveID);

                if (TempData["ActiveTab"] != null)
                    _WHAssetMaster.ActiveTab = Convert.ToString(TempData["ActiveTab"]);

                if (TempData["CurrLiftVanID"] != null)
                    _WHAssetMaster.WHLocationMap.LiftVanID = Convert.ToInt64(TempData["CurrLiftVanID"]);

                return View(_WHAssetMaster);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult Create(WHAssetMaster WHAssetMasterObj, string Save)
        {
            try
            {
                string message = string.Empty;
                GetDropDownLists(MoveID: WHAssetMasterObj.WHInAssetMaster.MoveID);

                if (ModelState.IsValid)
                {
                    bool result = false;
                    int LoginID = UserSession.GetUserSession()?.LoginID ?? 0;

                    if (WHAssetMasterObj.ActiveTab == "Inward")
                    {
                        result = WHAssetManagementBL.SaveAssetInwardDetails(WHAssetMasterObj, LoginID, out message);
                    }
                    else if (WHAssetMasterObj.ActiveTab == "Location Map")
                    {
                        TempData["CurrLiftVanID"] = WHAssetMasterObj.WHLocationMap.LiftVanID;
                        result = WHAssetManagementBL.SaveAssetLiftVanMapping(WHAssetMasterObj, LoginID, out message);
                    }
                    else if (WHAssetMasterObj.ActiveTab == "Outward")
                    {
                        result = WHAssetManagementBL.SaveAssetOutwardDetails(WHAssetMasterObj, LoginID, out message);
                    }
                    else if (WHAssetMasterObj.ActiveTab == "DMS")
                    {
                        WHAssetMasterObj.WHJobDocUpload.MoveID = WHAssetMasterObj.WHJobDocUpload.ID = Convert.ToInt64(WHAssetMasterObj.WHInAssetMaster.MoveID);
                        result = WHAssetManagementBL.InsertDocument(WHAssetMasterObj.WHJobDocUpload, LoginID, out message);
                    }

                    if (result)
                    {
                        if (string.IsNullOrEmpty(message)) message = "Data saved successfully.";
                        this.AddToastMessage("RELOCBS", message, ToastType.Success);
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(message)) message = "Error occured.";
                        this.AddToastMessage("RELOCBS", message, ToastType.Error);
                        ModelState.AddModelError(string.Empty, "Error occured while saving.");
                    }
                    TempData["ActiveTab"] = WHAssetMasterObj.ActiveTab;
                    return RedirectToAction("Create", new { MoveID = WHAssetMasterObj.WHInAssetMaster.MoveID });
                }
                else
                {
                    this.AddToastMessage("RELOCBS", message, ToastType.Error);
                    ModelState.AddModelError(string.Empty, "Error occured while saving.");
                    return View("Create", WHAssetMasterObj);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult SendLink(Int64 MoveID, string EmailTo, string EmailCc, string EmailBcc)
        {
            string message = string.Empty;
            try
            {
                bool result = false;
                int LoginID = UserSession.GetUserSession()?.LoginID ?? 0;

                string ApplicationUrl = System.Configuration.ConfigurationManager.AppSettings["ApplicationUrl"].ToString();
                string url = ApplicationUrl + "WHAssetManagement/WHInOutAssetReport?Key=" + CommonService.GetCrypt("MoveID=" + MoveID, 1);

                result = WHAssetManagementBL.SendLinkWHAssetReport(MoveID, EmailTo, EmailCc, EmailBcc, url, LoginID, out message);
                if (!result)
                {
                    CSubs.LogError("WOSJobOpening", "SendLink", "Email send fail.");
                }
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [AllowAnonymous]
        public ActionResult WHInOutAssetReport(int Page = 1, string Key = null)
        {
            try
            {
                int PageSize = settings.GetSettingByKey<int>("pagination_pagesize");
                int TotalRecord = 0;
                if (Page < 1) Page = 1;
                int Skip = (Page * PageSize) - PageSize;

                string Sort = "MoveID";
                string SortDir = "desc";
                int Order = 0;

                if (Request.Params["grid-column"] != null && Request.Params["grid-column"].Trim() != "")
                    Sort = Request.Params["grid-column"].Trim().ToString();

                if (Request.Params["grid-dir"] != null && Request.Params["grid-dir"].Trim() != "")
                {
                    Order = Convert.ToInt32(Request.Params["grid-dir"].Trim().ToString());
                    if (Order == 1) SortDir = "asc";
                }

                Int64? MoveID = 0;
                Int64? ClientID = 0;

                if (!string.IsNullOrEmpty(Key))
                {
                    var list = CommonService.GetQueryString(Key);

                    if (list.ContainsKey("MoveID"))
                        MoveID = Convert.ToInt64(list["MoveID"]);
                    if (list.ContainsKey("ClientID"))
                        ClientID = Convert.ToInt64(list["ClientID"]);
                }

                int LoginID = UserSession.GetUserSession()?.LoginID ?? 0;
                string RefJobID = string.Empty;

                if (Request.Form["RefJobID"] != null && Request.Form["RefJobID"].Trim() != "")
                    RefJobID = Convert.ToString(Request.Form["RefJobID"]);

                var WHInOutAssetList = WHAssetManagementBL.GetWHInOutAssetReport(LoginID, "Job Wise InOut Details", MoveID, RefJobID, Sort, SortDir, Skip, PageSize, out TotalRecord);

                ViewBag.MoveID = MoveID;

                var itemsAsIPagedList = new StaticPagedList<WHInOutAssetMaster>(WHInOutAssetList, Page, PageSize, TotalRecord);

                return Request.IsAjaxRequest() ? (ActionResult)PartialView("_AjaxPartialReport", itemsAsIPagedList) : View(itemsAsIPagedList);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [AllowAnonymous]
        public JsonResult ExportToExcel(Int64? MoveID = null, string RefJobID = null)
        {
            Dictionary<string, string> exptoExlParameters = new Dictionary<string, string>();
            string errormsg = string.Empty;
            string htmlstring = string.Empty;
            int ColCount = 0;
            try
            {
                int LoginID = UserSession.GetUserSession()?.LoginID ?? 0;

                DataTable dtGridData = new DataTable();

                exptoExlParameters.Add("@SP_ReportName", "Job Wise InOut Details");
                exptoExlParameters.Add("@SP_MoveID", Convert.ToString(MoveID));
                exptoExlParameters.Add("@SP_RefJobID", RefJobID);
                string param = string.Join(",", exptoExlParameters.Select(x => x.Key + "=" + CSubs.QSafeValue(x.Value)).ToArray()).TrimEnd(',');
                string query = string.Format("EXEC {0} {1}", "[Warehouse].[AssetsReports]", param);

                dtGridData = CSubs.GetDataTable(query);
                ColCount = dtGridData.Columns.Count;
                htmlstring = "<tr>";
                foreach (DataColumn col in dtGridData.Columns)
                {
                    htmlstring += "<th bgcolor='#DCDCDC'>" + col.ColumnName + "</th>";
                }
                htmlstring += "</tr>";
                foreach (DataRow row in dtGridData.Rows)
                {
                    if (string.IsNullOrEmpty(Convert.ToString(row[0])))
                        htmlstring += "<tr>";
                    else
                        htmlstring += "<tr>";

                    foreach (DataColumn col in dtGridData.Columns)
                    {
                        if (string.IsNullOrEmpty(Convert.ToString(row[0])))
                            htmlstring += "<td bgcolor='#B0C4DE' style=\"font-weight:bold\">" + row[col.ColumnName].ToString() + "</td>";
                        else
                            htmlstring += "<td>" + row[col.ColumnName].ToString() + "</td>";
                    }
                    htmlstring += "</tr>";
                }
            }
            catch (Exception ex)
            {
                this.AddToastMessage("RELOCBS", "UnExpected Error occured", ToastType.Error);
            }
            return Json(new { htmlstring, errormsg, ColCount }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ValidateBoxNo(Int64? MoveID, Int64? BoxNo)
        {
            try
            {
                int LoginID = UserSession.GetUserSession()?.LoginID ?? 0;
                bool result = WHAssetManagementBL.ValidateBoxNo(LoginID, MoveID, BoxNo);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public JsonResult GetDetailsByAssetDetID(Int64? AssetDetID)
        {
            try
            {
                int LoginID = UserSession.GetUserSession()?.LoginID ?? 0;
                WHInAssetDetails WHInAssetDetailsObj = WHAssetManagementBL.GetDetailsByAssetDetID(LoginID, AssetDetID);
                return Json(new { WHInAssetDetailsObj }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public JsonResult GetAssetLiftVanMapping(Int64? LiftVanID, Int64? MoveID)
        {
            try
            {
                int LoginID = UserSession.GetUserSession()?.LoginID ?? 0;

                WHAssetMaster WHAssetMasterObj = WHAssetManagementBL.GetAssetLiftVanMapping(LoginID, LiftVanID, MoveID);

                List<SelectListItem> AssetList = WHAssetMasterObj.WHLocationMap.LiftVanDetailsList.ConvertAll(a =>
                {
                    return new SelectListItem() { Value = a.AssetDetID.ToString(), Text = a.AssetDesc.ToString() };
                });

                List<SelectListItem> DefAssetList = WHAssetMasterObj.WHLocationMap.DefLiftVanDetailsList.ConvertAll(a =>
                {
                    return new SelectListItem() { Value = a.AssetDetID.ToString(), Text = a.AssetDesc.ToString() };
                });

                var OtherAssetList = WHAssetMasterObj.WHLocationMap.OtherLiftVanDetailsList;

                return Json(new { AssetList, DefAssetList, OtherAssetList }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public JsonResult GetInAssetDetails(Int64? MoveID, Int64? InMastID)
        {
            try
            {
                int LoginID = UserSession.GetUserSession()?.LoginID ?? 0;
                _WHAssetMaster = WHAssetManagementBL.GetAssetInwardDetails(LoginID, MoveID, InMastID, null);
                return Json(_WHAssetMaster.WHInAssetMaster.WHInAssetDetailsList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public JsonResult GetOutAssetDetails(Int64? MoveID, Int64? OutMasterID)
        {
            try
            {
                int LoginID = UserSession.GetUserSession()?.LoginID ?? 0;
                _WHAssetMaster = WHAssetManagementBL.GetAssetInwardDetails(LoginID, MoveID, null, OutMasterID);
                return Json(_WHAssetMaster.WHOutAssetMaster.WHOutAssetDetailsList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public JsonResult GetInOutWHDeliveryAddress(Int64? MoveID, string AddressType)
        {
            try
            {
                int LoginID = UserSession.GetUserSession()?.LoginID ?? 0;
                WHOutAssetMaster WHOutAssetMasterObj = WHAssetManagementBL.GetInOutWHDeliveryAddress(LoginID, MoveID, AddressType);
                return Json(WHOutAssetMasterObj, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult JobDocDelete(int FileID, WHAssetMaster WHAssetMasterObj)
        {
            bool res = false;
            string message = string.Empty;
            AjaxResponse result = new AjaxResponse();

            res = new BL.MoveMange.MoveManageBL().DeleteDocument(FileID, out message);
            if (!res)
            {
                result.Success = false;
                ModelState.AddModelError(string.Empty, "Unable to delete document.");
                result.Message = message;
                this.AddToastMessage("RELOCBS", message, ToastType.Error);
            }
            else
            {
                result.Success = true;
                result.Message = message;
                this.AddToastMessage("RELOCBS", message, ToastType.Success);
            }
            TempData["ActiveTab"] = "DMS";
            return RedirectToAction("Create", new { MoveID = WHAssetMasterObj.WHInAssetMaster.MoveID });
        }

        private void GetDropDownLists(int BranchID = -1, Int64? MoveID = null, Int64? InMastID = null)
        {
            ViewData["JobNoList"] = ComboBL.GetJobNoForAssetManagement(MoveID: MoveID);
            ViewData["WarehoueList"] = ComboBL.GetWarehouseDropdown(BranchID: BranchID);
            ViewData["VolUnitList"] = ComboBL.GetMeasurementUnitDropdown(UnitType: 'B');
            ViewData["DimUnitList"] = ComboBL.GetMeasurementUnitDropdown(UnitType: 'I');
            ViewData["InAssetDetailsList"] = ComboBL.GetInAssetDetails(MoveID: MoveID, InMastID: InMastID);
            ViewData["DocTypeList"] = ComboBL.GetJobDocTypelDropdown(DocFromType: "Warehouse");
            ViewData["LiftVanList"] = ComboBL.GetLiftVanList(MoveID: MoveID);
            ViewData["DefAssetList"] = new List<SelectListItem>();
            ViewData["AssetList"] = new List<SelectListItem>();
        }
    }
}