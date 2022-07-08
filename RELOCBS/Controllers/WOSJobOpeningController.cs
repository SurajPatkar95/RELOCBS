using PagedList;
using RELOCBS.AjaxHelper;
using RELOCBS.App_Code;
using RELOCBS.BL;
using RELOCBS.BL.WOSCommon;
using RELOCBS.BL.WOSJobOpening;
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

namespace RELOCBS.Controllers
{
    [AuthorizeUser]
    public class WOSJobOpeningController : BaseController
    {
        private string _PageID = "71";

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

        private WOSJobOpeningBL _WOSJobOpeningBL;
        public WOSJobOpeningBL WOSJobOpeningBL
        {
            get
            {
                if (_WOSJobOpeningBL == null)
                    _WOSJobOpeningBL = new WOSJobOpeningBL();
                return _WOSJobOpeningBL;
            }
        }

        WOSJobOpening _WOSJobOpening = new WOSJobOpening();
        public WOSJobOpeningController(WOSJobOpening WOSJobOpening)
        {
            _WOSJobOpening = WOSJobOpening;
        }

        public ActionResult Index(int Page = 1)
        {
            try
            {
                if (!CSubs.CheckPageRights(_PageID, PermissionType.VIEW))
                {
                    return new HttpStatusCodeResult(403);
                }

                session.Set("PageSession", "Job Opening");

                Int64? WOSJobID = null;
                //string WONumber = string.Empty;
                string Sort = "WOSMoveID";
                string SortDir = "desc";
                string Search = string.Empty;
                string SearchType = string.Empty;
                int PageSize = settings.GetSettingByKey<int>("pagination_pagesize");
                int Order = 0;
                DateTime? FromDate = null;
                DateTime? ToDate = null;
                string AssigneeName = string.Empty;

                if (Request.Form["WOSJobID"] != null && Request.Form["WOSJobID"].Trim() != "")
                    WOSJobID = Convert.ToInt32(Request.Form["WOSJobID"]);

                if (Request.Form["FromDate"] != null && Request.Form["FromDate"].Trim() != "")
                    FromDate = Convert.ToDateTime(Request.Form["FromDate"]);

                if (Request.Form["ToDate"] != null && Request.Form["ToDate"].Trim() != "")
                    ToDate = Convert.ToDateTime(Request.Form["ToDate"]);

                if (Request.Form["AssigneeName"] != null && Request.Form["AssigneeName"].Trim() != "")
                    AssigneeName = Convert.ToString(Request.Form["AssigneeName"]);

                if (Request.Form["SearchValue"] != null && Request.Form["SearchValue"].Trim() != "")
                    Search = Convert.ToString(Request.Form["SearchValue"]);

                if (Request.Form["SearchType"] != null && Request.Form["SearchType"].Trim() != "")
                    SearchType = Convert.ToString(Request.Form["SearchType"]);

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
                if (Page < 1)
                    Page = 1;
                int Skip = (Page * PageSize) - PageSize;

                int LoginID = UserSession.GetUserSession()?.LoginID ?? 0;
                int? CompId = null;
                bool? IsJobDate = false;
                bool? IsRMCBuss = (UserSession.GetUserSession().BussinessLine == "RMC-BUSINESS");

                var WOSJobOpeningList = WOSJobOpeningBL.GetWOSJobOpeningList(Sort, SortDir, Skip, PageSize, FromDate, ToDate, LoginID, CompId, IsJobDate, IsRMCBuss, AssigneeName, SearchType, Search, out TotalRecord);

                var itemsAsIPagedList = new StaticPagedList<WOSJobOpening>(WOSJobOpeningList, Page, PageSize, TotalRecord);
                return Request.IsAjaxRequest() ? (ActionResult)PartialView("_GridPartial", itemsAsIPagedList) : View(itemsAsIPagedList);
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
                session.Set("PageSession", "Job Opening");

                var list = CommonService.GetQueryString(Key);
                Int64 WOSMoveID = 0;

                if (list.ContainsKey("WOSMoveID"))
                    WOSMoveID = Convert.ToInt64(list["WOSMoveID"]);

                _WOSJobOpening.WOSMoveID = WOSMoveID;

                IEnumerable<WOSJobOpening> WOSJobOpeningList = new List<WOSJobOpening>();

                int LoginID = UserSession.GetUserSession()?.LoginID ?? 0;

                _WOSJobOpening = WOSJobOpeningBL.GetWOSJobDetailsById(WOSMoveID, LoginID, out WOSJobOpeningList);

                if (_WOSJobOpening.WOSCustomer.RevenueCurrencyID != null && _WOSJobOpening.WOSCustomer.CostCurrencyID != null)
                    _WOSJobOpening.WOSCustomer.WOSSubServiceList = WOSJobOpeningBL.GetCostSheetForJob(WOSMoveID, _WOSJobOpening.WOSCustomer.RevenueCurrencyID, _WOSJobOpening.WOSCustomer.CostCurrencyID, LoginID);

                _WOSJobOpening.dsCostSheet = WOSJobOpeningBL.GetWOSCostSheet(LoginID, WOSMoveID);

                GetDropDownLists();

                if (TempData["ActiveTab"] != null)
                    _WOSJobOpening.ActiveTab = Convert.ToString(TempData["ActiveTab"]);

                ViewBag.WOSJobOpeningList = WOSJobOpeningList;
                return View(_WOSJobOpening);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult Create(WOSJobOpening WOSJobOpeningObj, string Save)
        {
            try
            {
                string message = string.Empty;
                GetDropDownLists();

                if (ModelState.IsValid)
                {
                    bool result = false;
                    int LoginID = UserSession.GetUserSession()?.LoginID ?? 0;

                    if (Save == "Cancel Job")
                    {
                        result = WOSJobOpeningBL.CancelWOSJob(WOSJobOpeningObj, LoginID, out message);
                    }
                    else
                    {
                        if (WOSJobOpeningObj.ActiveTab == "Dashboard")
                        {
                            // add CompanyID for new job
                            if (WOSJobOpeningObj.WOSMoveID == 0) WOSJobOpeningObj.CompanyID = UserSession.GetUserSession().CompanyID;

                            result = WOSJobOpeningBL.SaveWOSJobDetails(WOSJobOpeningObj, LoginID, out message);
                        }
                        else if (WOSJobOpeningObj.ActiveTab == "Housing Preference")
                        {
                            WOSJobOpeningObj.WOSHouseDetails.WOSMoveID = WOSJobOpeningObj.WOSMoveID;
                            result = WOSJobOpeningBL.SaveWOSHouseDetails(WOSJobOpeningObj.WOSHouseDetails, LoginID, out message);
                        }
                        else if (WOSJobOpeningObj.ActiveTab == "Service Cost Details" && WOSJobOpeningObj.WOSCustomer.WOSSubServiceList?.Count(r => r.IsChecked) > 0)
                        {
                            if (Save == "Save" || Save == "Proceed")
                            {
                                WOSJobOpeningObj.WOSCustomer.WOSMoveID = WOSJobOpeningObj.WOSMoveID;
                                result = WOSJobOpeningBL.SaveCostSheet(WOSJobOpeningObj.WOSCustomer, LoginID, out message);

                                if (result && Save == "Proceed")
                                {
                                    this.AddToastMessage("RELOCBS", "Data saved successfully.", ToastType.Success);
                                    char InvOrCreditNote = 'I';
                                    Int64 BillID = WOSJobOpeningObj.WOSBilling.BillID ?? 0;
                                    Int64 CreditNoteID = WOSJobOpeningObj.WOSBilling.CreditNoteID ?? 0;
                                    Int64 WOSMoveID = WOSJobOpeningObj.WOSMoveID;
                                    Int64 MoveID = WOSJobOpeningObj.MoveID;

                                    return RedirectToAction("Create", "WOSBilling", new
                                    {
                                        Key = CommonService.GetCrypt("BillID=" + BillID.ToString() + "&CreditNoteID=" + CreditNoteID.ToString() +
                                        "&WOSMoveID=" + WOSMoveID.ToString() + "&MoveID=" + MoveID.ToString(), 1),
                                        InvOrCreditNote
                                    });
                                }
                            }
                            else if (Save == "Proceed for Approval")
                            {
                                WOSJobOpeningObj.IsCSSentToApprove = true;
                                result = WOSJobOpeningBL.ApproveCostSheet(WOSJobOpeningObj, true, LoginID, out message);
                            }
                            else if (Save == "Pending" || Save == "Approved")
                            {
                                bool IsApprove = Save == "Pending";
                                result = WOSJobOpeningBL.ApproveCostSheet(WOSJobOpeningObj, IsApprove, LoginID, out message);
                                if (string.IsNullOrEmpty(message))
                                    message = IsApprove ? "Failed to Approve." : "Failed to Disapprove.";
                            }
                        }
                        else if (WOSJobOpeningObj.ActiveTab == "Cost Sheet")
                        {
                            //No data saved on Cost Sheet tab
                            result = true;
                        }
                        else if (WOSJobOpeningObj.ActiveTab == "DMS (Internal)")
                        {
                            WOSJobOpeningObj.WOSJobDocUpload.WOSMoveID = WOSJobOpeningObj.WOSMoveID;
                            WOSJobOpeningObj.WOSJobDocUpload.ID = WOSJobOpeningObj.MoveID;
                            result = WOSJobOpeningBL.InsertDocument(WOSJobOpeningObj.WOSJobDocUpload, LoginID, out message);
                        }
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
                    TempData["ActiveTab"] = WOSJobOpeningObj.ActiveTab;
                    return RedirectToAction("Create", new { Key = CommonService.GetCrypt("WOSMoveID=" + WOSJobOpeningObj.WOSMoveID.ToString(), 1) });
                }
                else
                {
                    this.AddToastMessage("RELOCBS", message, ToastType.Error);
                    ModelState.AddModelError(string.Empty, "Error occured while saving.");
                    return View("Create", WOSJobOpeningObj);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult AssigneeDetails(string Key)
        {
            try
            {
                session.Set("PageSession", "Assignee Details");

                int LoginID = UserSession.GetUserSession()?.LoginID ?? 0;
                string EMPorAssingee = LoginID > 0 ? "E" : "A";
                Int64 WOSMoveID = 0;
                var list = CommonService.GetQueryString(Key);

                if (list.ContainsKey("WOSMoveID"))
                    WOSMoveID = Convert.ToInt64(list["WOSMoveID"]);

                _WOSJobOpening.WOSMoveID = WOSMoveID;

                GetDropDownLists();
                _WOSJobOpening.Assignee = WOSJobOpeningBL.GetAssigneeDetailsById(LoginID, WOSMoveID);
                _WOSJobOpening.Assignee.TaskDetailsList = WOSJobOpeningBL.GetTaskInfo(LoginID, WOSMoveID, EMPorAssingee);

                List<ChatDetails> ChatDetailsList = WOSJobOpeningBL.GetChatting(LoginID, WOSMoveID, EMPorAssingee);
                int ChatDetailsCount = ChatDetailsList.Count > 0 ? ChatDetailsList.Count : 1;
                var itemsAsIPagedListChat = new StaticPagedList<ChatDetails>(ChatDetailsList, 1, ChatDetailsCount, ChatDetailsCount);
                ViewBag.ChatDetailsList = itemsAsIPagedListChat;

                return Request.IsAjaxRequest() ? (ActionResult)PartialView("_ChatPartial", itemsAsIPagedListChat) : View(_WOSJobOpening.Assignee);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult AssigneeDetails(Assignee AssigneeObj, string SaveButton)
        {
            try
            {
                session.Set("PageSession", "Assignee Details");
                string message = string.Empty;
                if (ModelState.IsValid)
                {
                    if (SaveButton.ToUpper() == "FINAL SUBMIT")
                    {
                        AssigneeObj.IsFinalSubmit = true;
                    }

                    bool result = false;
                    int LoginID = UserSession.GetUserSession()?.LoginID ?? 0;
                    result = WOSJobOpeningBL.AddEditAssignee(AssigneeObj, LoginID, out message);

                    if (!result)
                    {
                        ModelState.AddModelError(string.Empty, "Unable to save assignee data.");
                        this.AddToastMessage("RELOCBS", "Error occured while saving.", ToastType.Error);
                    }
                    else
                    {
                        this.AddToastMessage("RELOCBS", "Data saved successfully.", ToastType.Success);
                    }
                    return RedirectToAction("AssigneeDetails", new { Key = CommonService.GetCrypt("WOSMoveID=" + AssigneeObj.WOSMoveID.ToString(), 1) });
                }
                else
                {
                    int LoginID = UserSession.GetUserSession()?.LoginID ?? 0;
                    string EMPorAssingee = LoginID > 0 ? "E" : "A";
                    GetDropDownLists();
                    List<ChatDetails> ChatDetailsList = WOSJobOpeningBL.GetChatting(LoginID, AssigneeObj.WOSMoveID ?? 0, EMPorAssingee);
                    int ChatDetailsCount = ChatDetailsList.Count > 0 ? ChatDetailsList.Count : 1;
                    var itemsAsIPagedListChat = new StaticPagedList<ChatDetails>(ChatDetailsList, 1, ChatDetailsCount, ChatDetailsCount);
                    ViewBag.ChatDetailsList = itemsAsIPagedListChat;
                    return View("AssigneeDetails", AssigneeObj);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public ActionResult SendLink(Int64 WOSMoveID, string EmailTo, string EmailCc, string EmailBcc)
        {
            string message = string.Empty;
            try
            {
                bool result = false;
                int LoginID = UserSession.GetUserSession()?.LoginID ?? 0;

                string ApplicationUrl = System.Configuration.ConfigurationManager.AppSettings["ApplicationUrl"].ToString();
                string Url = ApplicationUrl + "WOSJobOpening/AssigneeDetails?Key=" + CommonService.GetCrypt("WOSMoveID=" + WOSMoveID, 1);

                result = WOSJobOpeningBL.SendLinkToAssignee(WOSMoveID, EmailTo, EmailCc, EmailBcc, Url, LoginID, out message);
                if (result)
                {
                    this.AddToastMessage("RELOCBS", message, ToastType.Success);
                }
                else
                {
                    this.AddToastMessage("RELOCBS", "Error occured while sending email.", ToastType.Error);
                    CSubs.LogError("WOSJobOpening", "SendLink", "Email send fail.");
                }
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void GetDropDownLists()
        {
            try
            {
                if (UserSession.GetUserSession() != null)
                {
                    List<SelectListItem> BusinessList = new List<SelectListItem>
                    {
                        new SelectListItem() { Value = "false", Text = "NON RMC-BUSINESS" },
                        new SelectListItem() { Value = "true", Text = "RMC-BUSINESS" }
                    };
                    ViewData["BusinessList"] = BusinessList;
                    ViewData["RMCList"] = ComboBL.GetRMCDropdown();
                    ViewData["ServiceLineList"] = ComboBL.GetServiceLineDropdown(BussLine: "ORIENTATION SERVICE");
                    ViewData["RevBranchList"] = ComboBL.GetCompanyBranchDropdown();
                    ViewData["EmployeeList"] = ComboBL.GetEmployeeDropdown();
                    ViewData["DocTypeList"] = ComboBL.GetJobDocTypelDropdown(DocFromType: "WOS");
                    List<SelectListItem> ApprovalList = ComboBL.GetWOSCSApprovalUserList().ToList();
                    ViewData["ApprovalUserList"] = ApprovalList;
                    ViewBag.ApprovalCount = ApprovalList.Count();
                    ViewData["HouseList"] = WOSComboBL.GetWOSHouseDropdown();
                }

                ViewData["CityList"] = ComboBL.GetCityDropdown();
                ViewData["TitleList"] = ComboBL.GetTitleDropdown();
                ViewData["CountryList"] = ComboBL.GETCountryDropdown();
                ViewData["GenderList"] = ComboBL.GetGenderDropdown();
                ViewData["MaritalStatusList"] = ComboBL.GetMaritalStatusDropdown();
                ViewData["CurrencyList"] = ComboBL.GetCurrencyDropdown();
                ViewData["SchoolTypeList"] = WOSComboBL.GetWOSSchoolTypeDropdown();
                ViewData["LocationList"] = WOSComboBL.GetWOSLocationTypeDropdown();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [AllowAnonymous]
        [HttpPost]
        public JsonResult SendMessage(Int64 WOSMoveID, string EMPorAssingee, string ChatMsg)
        {
            string message = string.Empty;
            try
            {
                bool result = false;
                int LoginID = UserSession.GetUserSession()?.LoginID ?? 0;

                ChatDetails Conversation = new ChatDetails();
                Conversation.WOSMoveID = WOSMoveID;
                Conversation.ChatBy = EMPorAssingee;
                Conversation.ChatMsg = ChatMsg;

                result = WOSJobOpeningBL.AddEditChatting(Conversation, LoginID, out message);

                return Json(new { result, message }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpGet]
        public ActionResult RequestDocs(Int64 WOSMoveID)
        {
            try
            {
                int LoginID = UserSession.GetUserSession()?.LoginID ?? 0;
                IEnumerable<WOSJobOpening> WOSJobOpeningList = new List<WOSJobOpening>();
                _WOSJobOpening = WOSJobOpeningBL.GetWOSJobDetailsById(WOSMoveID, LoginID, out WOSJobOpeningList);

                Assignee AssigneeObj = WOSJobOpeningBL.GetAssigneeDetailsById(LoginID, WOSMoveID);
                if (AssigneeObj != null && AssigneeObj.WOSJobDocUploadList != null)
                {
                    ViewBag.WOSJobDocUploadList = AssigneeObj.WOSJobDocUploadList;
                }
                else
                {
                    ViewBag.WOSJobDocUploadList = new List<WOSJobDocUpload>();
                }

                ViewData["DocTypeList"] = ComboBL.GetJobDocTypelDropdown(DocFromType: "WOS");
                ViewData["DocNameList"] = new List<SelectListItem>();
                return View(_WOSJobOpening);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult RequestDocs(WOSJobOpening WOSJobOpeningObj)
        {
            try
            {
                bool result = false;
                string message = string.Empty;

                WOSJobOpeningObj.Assignee.WOSMoveID = WOSJobOpeningObj.WOSMoveID;

                string ApplicationUrl = System.Configuration.ConfigurationManager.AppSettings["ApplicationUrl"].ToString();
                string Url = ApplicationUrl + "WOSJobOpening/AssigneeDetails?Key=" + CommonService.GetCrypt("WOSMoveID=" + WOSJobOpeningObj.WOSMoveID, 1);

                result = WOSJobOpeningBL.InsertRequestDocsData(WOSJobOpeningObj.Assignee, UserSession.GetUserSession().LoginID, Url, out message);
                if (result)
                {
                    this.AddToastMessage("RELOCBS", message, ToastType.Success);
                }
                else
                {
                    this.AddToastMessage("RELOCBS", message, ToastType.Error);
                    CSubs.LogError("WOSJobOpening", "RequestDocs", "Error while saving data.");
                }

                return RedirectToAction("RequestDocs", new { WOSJobOpeningObj.WOSMoveID });
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult RequestDocsUpload(Assignee AssigneeObj)
        {
            try
            {
                bool result = false;
                string message = string.Empty;
                int LoginID = UserSession.GetUserSession()?.LoginID ?? 0;

                AssigneeObj.WOSJobDocUpload = AssigneeObj.WOSJobDocUploadList[AssigneeObj.RowID];
                AssigneeObj.WOSJobDocUpload.file = AssigneeObj.WOSJobDocUpload.ExtFile;
                AssigneeObj.WOSJobDocUpload.WOSMoveID = AssigneeObj.WOSMoveID ?? 0;
                AssigneeObj.WOSJobDocUpload.ID = AssigneeObj.MoveID ?? 0;
                AssigneeObj.WOSJobDocUpload.IsShowToAssignee = true;

                result = WOSJobOpeningBL.InsertDocument(AssigneeObj.WOSJobDocUpload, LoginID, out message);
                if (result)
                {
                    string errorMessage = string.Empty;
                    this.AddToastMessage("RELOCBS", message, ToastType.Success);
                }
                else
                {
                    this.AddToastMessage("RELOCBS", message, ToastType.Error);
                    CSubs.LogError("WOSJobOpening", "RequestDocsUpload", "Error while saving file.");
                }

                return RedirectToAction("AssigneeDetails", new { Key = CommonService.GetCrypt("WOSMoveID=" + AssigneeObj.WOSMoveID.ToString(), 1) });
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public ActionResult GetCostSheetForJob(Int64 WOSMoveID, int? RevenueCurrencyID, int? CostCurrencyID)
        {
            try
            {
                int LoginID = UserSession.GetUserSession()?.LoginID ?? 0;
                GetDropDownLists();

                _WOSJobOpening.WOSCustomer.WOSSubServiceList = WOSJobOpeningBL.GetCostSheetForJob(WOSMoveID, RevenueCurrencyID, CostCurrencyID, LoginID);

                _WOSJobOpening.WOSCustomer.RevenueCurrencyID = RevenueCurrencyID;
                _WOSJobOpening.WOSCustomer.CostCurrencyID = CostCurrencyID;

                return Request.IsAjaxRequest() ? (ActionResult)PartialView("_ServicePartial", _WOSJobOpening) : View(_WOSJobOpening);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public JsonResult ExportToExcel(Int64 WOSMoveID)
        {
            Dictionary<string, string> exptoExlParameters = new Dictionary<string, string>();
            string errormsg = string.Empty;
            string htmlstring = string.Empty;
            int Colcount = 0;
            try
            {
                int LoginID = UserSession.GetUserSession()?.LoginID ?? 0;

                DataSet CostSheetDs = WOSJobOpeningBL.GetWOSCostSheet(LoginID, WOSMoveID, true);
                DataTable CostSheetDt = CostSheetDs.Tables[0];

                Colcount = CostSheetDt.Columns.Count;
                htmlstring = "<tr>";
                foreach (DataColumn col in CostSheetDt.Columns)
                {
                    htmlstring += "<th bgcolor='#DCDCDC'>" + col.ColumnName + "</th>";
                }
                htmlstring += "</tr>";

                foreach (DataRow row in CostSheetDt.Rows)
                {
                    if (string.IsNullOrEmpty(Convert.ToString(row[0])))
                        htmlstring += "<tr>";
                    else
                        htmlstring += "<tr>";

                    foreach (DataColumn col in CostSheetDt.Columns)
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
            return Json(new { htmlstring = htmlstring, errormsg = errormsg, ColCount = Colcount }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetDocumentList(Int64 id, Int16? DocTypeID, int? DocNameID, string DocDescription = "")
        {
            WOSJobOpening WOSJobOpeningObj = new WOSJobOpening();
            int DTypeID = DocTypeID == null ? -1 : Convert.ToInt32(DocTypeID);
            int DNameID = DocNameID == null ? -1 : Convert.ToInt32(DocNameID);
            return PartialView("_DocListPartial", WOSJobOpeningObj);
        }

        [AllowAnonymous]
        public ActionResult JobDocDownload(int FileID)
        {
            int LoginID = UserSession.GetUserSession()?.LoginID ?? 0;
            JobDocument JobDocumentObj = WOSJobOpeningBL.GetJobDocumentDetail(FileID, LoginID);
            if (!string.IsNullOrWhiteSpace(JobDocumentObj.FilePath) && System.IO.File.Exists(JobDocumentObj.FilePath))
            {
                return File(JobDocumentObj.FilePath, MimeMapping.GetMimeMapping(JobDocumentObj.FilePath), JobDocumentObj.FileName);
            }
            return new HttpStatusCodeResult(404);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult JobDocDelete(int FileID, WOSJobOpening WOSJobOpeningObj)
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
            TempData["ActiveTab"] = "DMS (Internal)";
            return RedirectToAction("Create", new { Key = CommonService.GetCrypt("WOSMoveID=" + WOSJobOpeningObj.WOSMoveID.ToString(), 1) });
        }
    }
}