using PagedList;
using RELOCBS.AjaxHelper;
using RELOCBS.App_Code;
using RELOCBS.BL;
using RELOCBS.BL.Survey;
using RELOCBS.Common;
using RELOCBS.CustomAttributes;
using RELOCBS.Entities;
using RELOCBS.Extensions;
using RELOCBS.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace RELOCBS.Controllers
{
    [AuthorizeUser]
    public class SurveyController : BaseController
    {
        private string _PageID = "9";

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

        private SurveyBL _surveyBL;
        public SurveyBL surveyBL
        {
            get
            {
                if (this._surveyBL == null)
                    this._surveyBL = new SurveyBL();
                return this._surveyBL;

            }
        }

        private ComboBL _comboBL;
        public ComboBL comboBL
        {
            get
            {
                if (this._comboBL == null)
                    this._comboBL = new ComboBL();
                return this._comboBL;

            }
        }

        private CommanBL _comBL;
        public CommanBL comBL
        {
            get
            {
                if (this._comBL == null)
                    this._comBL = new CommanBL();
                return this._comBL;
            }
        }

        // GET: Survey
        public ActionResult Index(int page = 1)
        {
            if (!CSubs.CheckPageRights(_PageID, PermissionType.VIEW))
            {
                return new HttpStatusCodeResult(403);
            }

            session.Set<string>("PageSession", "Survey");
            string sort = "";
            string sortdir = "";
            string search = "";
            string searchType = "";

            int pageSize = settings.GetSettingByKey<int>("pagination_pagesize");
            string OrderBy = "";
            int Order = 0;
            DateTime? Fromdate = null;
            DateTime? Todate = null;
            string Shipper = "";
            string SearchKey = string.Empty;
            if (Request.Form["FromDate"] != null && Request.Form["FromDate"].Trim() != "")
            {
                Fromdate = Convert.ToDateTime(Request.Form["FromDate"]);
            }

            if (Request.Form["ToDate"] != null && Request.Form["ToDate"].Trim() != "")
            {
                Todate = Convert.ToDateTime(Request.Form["ToDate"]);
            }

            if (Request.Form["search"] != null && Request.Form["search"].Trim() != "")
            {
                search = Convert.ToString(Request.Form["search"]);
            }

            if (Request.Form["SearchType"] != null && Request.Form["SearchType"].Trim() != "")
            {
                searchType = Convert.ToString(Request.Form["SearchType"]);
            }


            if (Request.Params["grid-column"] != null && Request.Params["grid-column"].Trim() != "")
            {
                sort = Request.Params["grid-column"].Trim().ToString();
            }
            if (Request.Params["grid-dir"] != null && Request.Params["grid-dir"].Trim() != "")
            {
                Order = Convert.ToInt32(Request.Params["grid-dir"].Trim().ToString());

                if (Order == 1)
                {
                    sortdir = "asc";
                }
                else
                {
                    sortdir = "desc";
                }

            }
            if (Request.Params["Shipper"] != null && Request.Params["Shipper"].Trim() != "")
            {
                Shipper = Request.Params["Shipper"].Trim().ToString();
            }

            int totalRecord = 0;
            if (page < 1) page = 1;
            int skip = (page * pageSize) - pageSize;
            var data = surveyBL.GetsurveyList(Fromdate, Todate, Shipper, searchType, search, sort, sortdir, skip, pageSize, out totalRecord);
            ViewBag.TotalRows = totalRecord;
            ViewBag.search = search;

            var itemsAsIPagedList = new StaticPagedList<SurveyData>(data, page, pageSize, totalRecord);

            return Request.IsAjaxRequest()
                ? (ActionResult)PartialView("_AjaxPartial", itemsAsIPagedList)
                : View(itemsAsIPagedList);
        }

        // GET: Survey/Details/5
        public ActionResult Details(int id)
        {
            if (id > 0)
            {

                SurveyViewModel data = surveyBL.GetSurveyCostHeads(id);
                FillCombo(UserSession.GetUserSession().CompanyID, data.EnquiryDetail.Mode, data.shipmentDetail.SurveyLooseCased, data.EnquiryDetail.LooseCased, data.EnquiryDetail.ServiceLineID);

                return Request.IsAjaxRequest()
                ? (ActionResult)PartialView("Details", data)
                : View(data);
            }
            else
            {
                return RedirectToAction("Index", "Survey");
            }
        }

        // GET: Survey/Create
        public ActionResult Create(int EnqDetailID, int IsView = 0, int CopyEnqDetailID = -1, bool Copy = false)
        {
            if (EnqDetailID > 0)
            {
                int CompID = UserSession.GetUserSession().CompanyID;/////NEED TO UNCOMMENT DEFAULT SET TO 1
                SurveyViewModel data = new SurveyViewModel();
                Status objstatus = new Status();
                data.shipmentDetail = new ShipmentDetail();
                data.shipmentDetail.SurveyLCLorFCL = "LCL";
                data.shipmentDetail.SurveyLooseCased = "Loose";

                if (Copy && CopyEnqDetailID > 0)
                {
                    data = surveyBL.GetCopyEnqSurvey(EnqDetailID, CopyEnqDetailID);

                }
                else
                {
                    data = surveyBL.GetDetailById(EnqDetailID);
                }

                data.SurveyCommItemList = surveyBL.GetCommItemDetails(UserSession.GetUserSession().LoginID, data.SurveyId);

                data.CopyEnqDetailID = GetCopyShippmentList(EnqDetailID);

                objstatus = comBL.GetStatusById(Convert.ToInt64(data.SurveyId), "Survey", "Sub");
                data.ShipmentStatusName = objstatus.StatusName;
                data.ShipmentStatusDate = objstatus.StatusDate;

                objstatus = comBL.GetStatusById(Convert.ToInt64(data.EnqID), "Enquiry", "Main");
                data.EnqStatusName = objstatus.StatusName;
                data.EnqStatusDate = objstatus.StatusDate;

                FillCombo(CompID, data.EnquiryDetail.Mode, data.shipmentDetail.SurveyLooseCased, data.EnquiryDetail.LooseCased, data.EnquiryDetail.ServiceLineID);

                if (data.EnquiryDetail.Mode == 4)
                {
                    data.shipmentDetail.SurveyLCLorFCL = " ";
                    data.shipmentDetail.SurveyLooseCased = " ";
                    data.EnquiryDetail.LCLFCL = " ";
                    data.EnquiryDetail.LooseCased = " ";
                }

                session.Set<string>("PageSession", "Survey");
                ViewBag.IsView = IsView;

                return Request.IsAjaxRequest()
                ? (ActionResult)PartialView("Create", data)
                : View(data);

            }
            else
            {
                return RedirectToAction("Index", "Survey");
            }

        }

        [HttpPost]
        [MultipleButton(Name = "action", Argument = "Completed")]
        public ActionResult SurveyCompleted(int EnqDetailID, SurveyViewModel data)
        {
            try
            {

                AjaxResponse result = new AjaxResponse();
                string message = string.Empty;
                int CompID = UserSession.GetUserSession().CompanyID;
                FillCombo(CompID, data.EnquiryDetail.Mode, data.shipmentDetail.SurveyLooseCased, data.EnquiryDetail.LooseCased, data.EnquiryDetail.ServiceLineID);
                ViewBag.IsView = 0;
                data.CopyEnqDetailID = GetCopyShippmentList(EnqDetailID);
                if (ModelState.IsValid)
                {
                    bool res = false;
                    res = surveyBL.Completed(data, out message);
                    if (!res)
                    {
                        result.Success = false;
                        ModelState.AddModelError(string.Empty, "Unable to save Survey data.");
                        result.Message = message;

                        this.AddToastMessage("RELOCBS", message, ToastType.Error);
                        //return Json(result);
                    }
                    else
                    {
                        result.Success = true;
                        result.Message = message;

                        this.AddToastMessage("RELOCBS", message, ToastType.Success);
                        //return Json(result);
                    }

                    ViewBag.Result = result;
                    //return RedirectToAction("create/"+data.EnquiryDetail.EnqDetailID);
                    return RedirectToAction("Create", new { EnqDetailID = data.EnquiryDetail.EnqDetailID });

                }
                else
                {
                    return Request.IsAjaxRequest() ? (ActionResult)PartialView("Create", data)
                  : View("Create", data);
                }
            }
            catch (Exception ex)
            {

                this.AddToastMessage("RELOCBS", ex.ToString(), ToastType.Success);
                return View();
            }
        }

        private void FillCombo(int? CompID, int? Mode, string SurveyLooseCased, string LooseCased, int? ServiceLineID = 0)
        {
            bool RMCBuss = !(UserSession.GetUserSession().BussinessLine == "NON RMC-BUSINESS");
            ViewData["EnqStatusList"] = comboBL.GetEnqInfoSourceDropdown();
            ViewData["RevenueBrList"] = comboBL.GetCompanyBranchDropdown(CompanyID: CompID);
            ViewData["AgentList"] = comboBL.GetAgentDropdown();
            ViewData["AccountList"] = comboBL.GetAgentDropdown();
            ViewData["ShipCategoryList"] = comboBL.GetShipperCategoryDropdown();

            ViewData["SurveyConductedByList"] = comboBL.GetEmployeeDropdown();
            ViewData["InsuredByList"] = comboBL.GetInsuranceTypeDropdown();

            ViewData["CompetitorList"] = comboBL.GetCompetitorDropdown();
            ViewData["CurrencyList"] = comboBL.GetCurrencyDropdown();
            ViewData["VolumeUnitList"] = comboBL.GetMeasurementUnitDropdown('V');
            ViewData["WeightUnitList"] = comboBL.GetMeasurementUnitDropdown('W');
            ViewData["ContainerUnitList"] = comboBL.GetContainerSizeDropdown();
            ViewData["RateCompList"] = comboBL.GetRateComponentDropdown();
            ViewData["CityList"] = comboBL.GetCityDropdown();

            ViewData["ShipmentTypeList"] = comboBL.GetShipmentTypeDropdown();
            ViewData["ModeList"] = comboBL.GetModeDropdown(ServiceLineID: Convert.ToInt32(ServiceLineID));
            ViewData["GoodsDescList"] = comboBL.GetGoodsDescriptionDropdown();
            ViewData["ServiceLineList"] = comboBL.GetServiceLineDropdown(RMCBuss: RMCBuss);
            ViewData["HandlingBrList"] = comboBL.GetCompanyBranchDropdown();

            ViewData["CostHeadList"] = comboBL.GetCostHeadDropdown();
            ViewData["AccountMgrList"] = comboBL.GetEmployeeDropdown(SPTYPE: "AccountMgr");

            ViewData["LoosedCasedList"] = comboBL.LoosedCasedDropDown(ModeID: Mode).ToList();
            ViewData["LCLFCLList"] = comboBL.LCLFCLDropDown(LOOSEDCASED: LooseCased, Mode: Mode).ToList();
            ViewData["SurveyLCLFCLList"] = comboBL.LCLFCLDropDown(LOOSEDCASED: SurveyLooseCased, Mode: Mode).ToList();
        }

        private Int64 GetCopyShippmentList(Int64 EnqDetailID)
        {
            List<SelectListItem> CopyEnqShipmentList = comboBL.GetCopyEnqShipmentDropdown(EnqDetailID).ToList();
            if (CopyEnqShipmentList.Count() == 1)
            {
                EnqDetailID = Convert.ToInt64(CopyEnqShipmentList.First().Value);
                CopyEnqShipmentList.First().Selected = true;
            }
            ViewData["CopyShippmentList"] = CopyEnqShipmentList.AsEnumerable();
            ViewBag.IsCopyShow = CopyEnqShipmentList.Count() > 0 ? true : false;
            return EnqDetailID;
        }

        // POST: Survey/Create
        [HttpPost]
        [MultipleButton(Name = "action", Argument = "Create")]
        public ActionResult Create(int EnqDetailID, SurveyViewModel data)
        {
            try
            {
                AjaxResponse result = new AjaxResponse();
                string message = string.Empty;
                int CompID = UserSession.GetUserSession().CompanyID;
                FillCombo(CompID, data.EnquiryDetail.Mode, data.shipmentDetail.SurveyLooseCased, data.EnquiryDetail.LooseCased, data.EnquiryDetail.ServiceLineID);
                ViewBag.IsView = 0;
                data.CopyEnqDetailID = GetCopyShippmentList(EnqDetailID);
                if (ModelState.IsValid)
                {
                    bool res = false;
                    res = surveyBL.Insert(data, out message);
                    if (!res)
                    {
                        result.Success = false;
                        ModelState.AddModelError(string.Empty, "Unable to save Survey data.");
                        result.Message = message;

                        this.AddToastMessage("RELOCBS", message, ToastType.Error);
                        //return Json(result);
                    }
                    else
                    {
                        result.Success = true;
                        result.Message = message;

                        this.AddToastMessage("RELOCBS", message, ToastType.Success);
                        //return Json(result);
                    }

                    ViewBag.Result = result;
                    //return RedirectToAction("create/"+data.EnquiryDetail.EnqDetailID);
                    return RedirectToAction("Create", new { EnqDetailID = data.EnquiryDetail.EnqDetailID });

                }
                else
                {
                    return Request.IsAjaxRequest() ? (ActionResult)PartialView("Create", data)
                  : View("Create", data);
                }
            }
            catch (Exception ex)
            {

                this.AddToastMessage("RELOCBS", ex.ToString(), ToastType.Success);
                return View();
            }
        }

        // GET: Survey/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Survey/Edit/5
        [HttpPost]
        public ActionResult Edit(SurveyViewModel data)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // POST: Survey/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult ExportToExcel()
        {
            Dictionary<string, string> param = new Dictionary<string, string>();
            try
            {
                if (Request.Form["FromDate"] != null && Request.Form["FromDate"].Trim() != "")
                {
                    param.Add("@SP_FromDate", Convert.ToDateTime(Request.Form["FromDate"]).ToString("dd-MMM-yyyy"));
                }

                if (Request.Form["ToDate"] != null && Request.Form["ToDate"].Trim() != "")
                {
                    param.Add("@SP_ToDate", Convert.ToDateTime(Request.Form["ToDate"]).ToString("dd-MMM-yyyy"));
                }

                if (Request.Form["SearchType"] != null && Request.Form["SearchType"].Trim() != "")
                {
                    param.Add("@SP_SearchType", Convert.ToString(Request.Form["SearchType"]));
                }

                if (Request.Form["search"] != null && Request.Form["search"].Trim() != "")
                {
                    param.Add("@SP_SearchNo", Convert.ToString(Request.Form["search"]));
                }

                param.Add("@SP_LoginID", Convert.ToString(UserSession.GetUserSession().LoginID));
                param.Add("@SP_CompId", Convert.ToString(UserSession.GetUserSession().CompanyID));

                CommonService.GenerateExcel(this.Response, "Survey", "[Survey].[GetSurveyForGrid_ExpToExl]", param);
            }
            catch (Exception ex)
            {
                this.AddToastMessage("RELOCBS", "UnExpected Error occured", ToastType.Error);
            }
            return View();
        }

        public ActionResult GetReport(int EnqDetailID, int SurveyID)
        {
            EncryptedQueryString query = new EncryptedQueryString();
            query["PageID"] = _PageID;
            query["ReportID"] = "10002";
            query["EnqDetailID"] = Convert.ToString(EnqDetailID);
            query["SurveyID"] = Convert.ToString(SurveyID);
            ViewBag.queryStr = "args=" + query.ToString();

            return this.RedirectSameDomain("/Reports/ReportViewer.aspx?args=" + query.ToString());
            //return Redirect("/Reports/ReportViewer.aspx");
        }

        public JsonResult GetSearchDropDown(string SearchType)
        {
            //string errormsg = null;
            List<SelectListItem> SearchTypeList = new List<SelectListItem>();
            SearchTypeList = comboBL.GetSearchTypeDropdown(SearchType).ToList();
            return Json(new { SearchTypeList = SearchTypeList }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCopyAddress(Int64 EnqDetailID, bool IsOrigin = false, bool IsDest = false)
        {
            //string errormsg = null;

            var Address = surveyBL.GetEnquiryDetailAddress(EnqDetailID, IsOrigin, IsDest);
            return Json(new { Address = Address }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetVoxmeReport(Int64 EnqID, String EnqNo)
        {
            try
            {
                JobDocument jobDocument = surveyBL.GetVoxmeReport(EnqID, EnqNo);
                if (!string.IsNullOrWhiteSpace(jobDocument.FilePath) && System.IO.File.Exists(jobDocument.FilePath))
                {

                    return File(jobDocument.FilePath, MimeMapping.GetMimeMapping(jobDocument.FilePath), jobDocument.FileName);
                }

                return new HttpStatusCodeResult(404);
            }
            catch (Exception)
            {

                return new HttpStatusCodeResult(403);
            }

        }

        [HttpPost]
        [MultipleButton(Name = "action", Argument = "SaveCommItemDetails")]
        public ActionResult SaveCommItemDetails(int EnqDetailID, SurveyViewModel data)
        {
            try
            {
                AjaxResponse result = new AjaxResponse();
                string message = string.Empty;
                int CompID = UserSession.GetUserSession().CompanyID;
                FillCombo(CompID, data.EnquiryDetail.Mode, data.shipmentDetail.SurveyLooseCased, data.EnquiryDetail.LooseCased, data.EnquiryDetail.ServiceLineID);
                ViewBag.IsView = 0;
                data.CopyEnqDetailID = GetCopyShippmentList(EnqDetailID);

                if (ModelState.IsValid)
                {
                    bool res = false;
                    res = surveyBL.SaveCommItemDetails(data, out message);
                    if (!res)
                    {
                        result.Success = false;
                        ModelState.AddModelError(string.Empty, "Unable to save Survey data.");
                        result.Message = message;

                        this.AddToastMessage("RELOCBS", message, ToastType.Error);
                    }
                    else
                    {
                        result.Success = true;
                        result.Message = message;

                        this.AddToastMessage("RELOCBS", message, ToastType.Success);
                    }

                    ViewBag.Result = result;
                    return RedirectToAction("Create", new { EnqDetailID = data.EnquiryDetail.EnqDetailID });
                }
                else
                {
                    return RedirectToAction("Create", new { EnqDetailID = data.EnquiryDetail.EnqDetailID });
                }
            }
            catch (Exception ex)
            {
                this.AddToastMessage("RELOCBS", ex.ToString(), ToastType.Success);
                return RedirectToAction("Create", new { EnqDetailID = data.EnquiryDetail.EnqDetailID });
            }
        }
    }
}