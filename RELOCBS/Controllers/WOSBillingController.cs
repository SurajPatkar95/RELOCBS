using PagedList;
using RELOCBS.App_Code;
using RELOCBS.BL;
using RELOCBS.BL.WOSBilling;
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
    public class WOSBillingController : BaseController
    {
        private string _PageID = "81";

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

        private WOSBillingBL _WOSBillingBL;
        public WOSBillingBL WOSBillingBL
        {
            get
            {
                if (_WOSBillingBL == null)
                    _WOSBillingBL = new WOSBillingBL();
                return _WOSBillingBL;
            }
        }

        WOSBilling _WOSBilling = new WOSBilling();
        public WOSBillingController(WOSBilling WOSBilling)
        {
            _WOSBilling = WOSBilling;
        }

        public ActionResult Index(int page = 1)
        {
            if (!CSubs.CheckPageRights(_PageID, PermissionType.VIEW))
            {
                return new HttpStatusCodeResult(403);
            }
            session.Set("PageSession", "WOS Billing");
            int pageSize = settings.GetSettingByKey<int>("pagination_pagesize");

            string sort = "BillID";
            string sortdir = "desc";
            string search = "";
            string searchType = "";
            int Order = 0;
            DateTime? Fromdate = null;
            DateTime? ToDate = null;
            int? InvoiceID = null;
            string Shipper = null;
            string Status = null;
            string SearchKey = string.Empty;

            if (Request.Form["FromDate"] != null && Request.Form["FromDate"].Trim() != "")
                Fromdate = Convert.ToDateTime(Request.Form["FromDate"]);

            if (Request.Form["ToDate"] != null && Request.Form["ToDate"].Trim() != "")
                ToDate = Convert.ToDateTime(Request.Form["ToDate"]);

            if (Request.Params["grid-column"] != null && Request.Params["grid-column"].Trim() != "")
                sort = Request.Params["grid-column"].Trim().ToString();

            if (Request.Params["grid-dir"] != null && Request.Params["grid-dir"].Trim() != "")
            {
                Order = Convert.ToInt32(Request.Params["grid-dir"].Trim().ToString());
                if (Order == 1)
                {
                    sortdir = "asc";
                }
            }

            if (Request.Form["InvoiceID"] != null && Request.Form["InvoiceID"].Trim() != "")
                InvoiceID = Convert.ToInt32(Request.Form["InvoiceID"]);

            if (Request.Form["SearchValue"] != null && Request.Form["SearchValue"].Trim() != "")
                search = Convert.ToString(Request.Form["SearchValue"]);

            if (Request.Form["SearchType"] != null && Request.Form["SearchType"].Trim() != "")
                searchType = Convert.ToString(Request.Form["SearchType"]);

            if (Request.Params["Shipper"] != null && Request.Params["Shipper"].Trim() != "")
                Shipper = Request.Params["Shipper"].Trim().ToString();

            if (Request.Params["Status"] != null && Request.Params["Status"].Trim() != "")
                Status = Request.Params["Status"].Trim().ToString();

            int totalRecord = 0;
            if (page < 1) page = 1;
            int skip = (page * pageSize) - pageSize;

            var data = WOSBillingBL.GetWOSInvoiceForGrid(Fromdate, ToDate, search, searchType, InvoiceID, Shipper, Status, sort, sortdir, skip, pageSize, 'I', out totalRecord);

            ViewBag.TotalRows = totalRecord;
            ViewBag.search = search;
            var itemsAsIPagedList = new StaticPagedList<WOSBilling>(data, page, pageSize, totalRecord);

            if (itemsAsIPagedList.Count > 0)
            {
                ViewBag.CreditNoteGrid = searchType == "CreditNoteNo" && itemsAsIPagedList.ToList().First().IsShowCreditNote ? GetCreditNote(Convert.ToInt32(itemsAsIPagedList.ToList().First().BillID)) : new List<WOSBilling>();
            }
            else
            {
                ViewBag.CreditNoteGrid = new List<WOSBilling>();
            }

            return Request.IsAjaxRequest() ? (ActionResult)PartialView("_GridPartial", itemsAsIPagedList) : View(itemsAsIPagedList);
        }

        public ActionResult GetCreditNoteList(int BillID)
        {
            List<WOSBilling> list = GetCreditNote(BillID);
            ViewBag.CreditNoteGrid = list;
            return PartialView("_GridPartialCreditNote", list);
        }

        public List<WOSBilling> GetCreditNote(int BillID)
        {
            int totalRecord = 0;
            string Sort = "CreditNoteID";
            string SortDir = "desc";
            List<WOSBilling> list = new List<WOSBilling>();
            list = WOSBillingBL.GetWOSInvoiceForGrid(null, null, null, null, BillID, null, null, Sort, SortDir, 0, 0, 'C', out totalRecord).ToList();
            return list;
        }

        [HttpGet]
        public ActionResult Create(string Key, char InvOrCreditNote)
        {
            try
            {
                WOSBilling ObjInv = new WOSBilling();

                if (InvOrCreditNote == 'I')
                    session.Set("PageSession", "Invoice");
                else
                    session.Set("PageSession", "Credit Note");

                Int64? MoveID = 0;
                Int64? WOSMoveID = 0;
                int RateCompID = 3;
                Int64? BillID = 0;
                Int64? CreditNoteID = 0;

                var list = CommonService.GetQueryString(Key);
                if (list.ContainsKey("BillID"))
                    BillID = Convert.ToInt64(list["BillID"]);

                if (list.ContainsKey("CreditNoteID"))
                    CreditNoteID = Convert.ToInt64(list["CreditNoteID"]);

                if (list.ContainsKey("WOSMoveID"))
                    WOSMoveID = Convert.ToInt64(list["WOSMoveID"]);

                if (list.ContainsKey("MoveID"))
                    MoveID = Convert.ToInt64(list["MoveID"]);

                int LoginID = UserSession.GetUserSession()?.LoginID ?? 0;



                ObjInv = WOSBillingBL.GetWOSInvoiceDetailsById(LoginID, MoveID, WOSMoveID, BillID, CreditNoteID, InvOrCreditNote, RateCompID);
                ObjInv.BillType = InvOrCreditNote;
                ViewBag.Comp = UserSession.GetUserSession().CompanyID == 2 ? "BTR" : "";
                ObjInv = SetDefaultValues(ObjInv);
                ViewBag.ShowStatement = Convert.ToBoolean(TempData["_IsStatement"]);
                int BillToAgentID = 0;
                if (ObjInv.BillToID == "Client")
                {
                    BillToAgentID = ObjInv.BillToClientID;
                }
                else if (ObjInv.BillToID == "Corporate")
                {
                    BillToAgentID = ObjInv.BillToAccountID;
                }



                GetDropDownLists(BillToAgentID);
                return View(ObjInv);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult Create(WOSBilling WOSBillingObj, string SubmitInvoice)
        {
            try
            {
                bool CNValidate = true;
                int? StrgInvID = WOSBillingObj.StrgInvID;
                GetDropDownLists(WOSBillingObj.BillToClientID);

                if (WOSBillingObj.OrgCountry == WOSBillingObj.DestCountry && WOSBillingObj.OrgCountry == "INDIA" && //WOSBillingObj.Mode == "Road" && 
                    SubmitInvoice.Equals("Send to Finance") && WOSBillingObj.BillType == 'I')
                {
                    CNValidate = true;//!string.IsNullOrEmpty(WOSBillingObj.VehicleNo) && !string.IsNullOrEmpty(Convert.ToString(WOSBillingObj.NoofPkgs));
                }
                if (ModelState.IsValid && CNValidate)
                {
                    bool Isauditamount = true;
                    bool SaveTopOnly = false;
                    string status = SubmitInvoice;
                    string outMsg = "";
                    if (WOSBillingObj.InvoiceStatus == "Approved" || WOSBillingObj.InvoiceStatus == "Finalized" || WOSBillingObj.InvoiceStatus == "Send To Finance")
                    {
                        Isauditamount = false;
                    }
                    if ((string.IsNullOrEmpty(WOSBillingObj.InvoiceStatus) || WOSBillingObj.InvoiceStatus == "Draft") && (SubmitInvoice.Equals("Save Invoice")))
                    {
                        status = "Draft";
                    }
                    if (SubmitInvoice.Equals("Approve"))
                    {
                        status = "Approved";
                        Isauditamount = false;
                    }
                    if (SubmitInvoice.Equals("Final Approve"))
                    {
                        status = "Finalized";
                        Isauditamount = false;
                    }

                    if (SubmitInvoice.Equals("Send to Finance") || SubmitInvoice.Equals("Send To Consultant"))
                    {
                        Isauditamount = false;
                    }
                    if (WOSBillingObj.BillType == 'C')
                    {
                        Isauditamount = false;
                    }
                    if (SubmitInvoice.Equals("Get Tax"))
                    {
                        SaveTopOnly = true;
                        status = "Draft";
                    }
                    bool success = false;
                    if (SubmitInvoice == "Statement of Charges")
                    {
                        success = true;// WOSBillingBL.SaveStatement(WOSbilling);
                    }
                    else
                    {
                        success = WOSBillingBL.SaveWOSInvoice(WOSBillingObj, status, Isauditamount, SaveTopOnly, out outMsg);
                    }

                    if (success)
                    {

                        if (!SaveTopOnly)
                        {
                            this.AddToastMessage("RELOCBS", outMsg, ToastType.Success);
                        }

                        if (SubmitInvoice.Equals("Send to Finance"))
                        {
                            return RedirectToActionPermanent("Index", "WOSJobOpening");
                        }
                        else
                        {
                            char InvOrCreditNote = WOSBillingObj.BillType;
                            TempData["_IsStatement"] = SubmitInvoice == "Statement of Charges";//remove if not required

                            return RedirectToActionPermanent("Create", "WOSBilling", new
                            {
                                Key = CommonService.GetCrypt("BillID=" + WOSBillingObj.BillID.ToString() + "&CreditNoteID=" + WOSBillingObj.CreditNoteID.ToString() +
                                "&WOSMoveID=" + WOSBillingObj.WOSMoveID.ToString() + "&MoveID=" + WOSBillingObj.MoveID.ToString(), 1),
                                InvOrCreditNote
                            });
                        }
                    }
                    else
                    {
                        this.AddToastMessage("RELOCBS", "Error in saving.", ToastType.Error);
                    }
                }
                else
                {
                    int LoginID = UserSession.GetUserSession()?.LoginID ?? 0;
                    char InvOrCreditNote = 'I';

                    char Type = WOSBillingObj.BillType;
                    WOSBillingObj = WOSBillingBL.GetWOSInvoiceDetailsById(LoginID, WOSBillingObj.MoveID, WOSBillingObj.WOSMoveID, WOSBillingObj.BillID, WOSBillingObj.CreditNoteID, InvOrCreditNote, 3);
                    WOSBillingObj.BillType = Type;
                    WOSBillingObj.StrgInvID = StrgInvID;
                    WOSBillingObj = SetDefaultValues(WOSBillingObj);
                    if (!CNValidate)
                    {
                        ModelState.AddModelError("", "Vehicle No and No. of pkgs is mandatory.");
                    }
                    else
                    {
                        this.AddToastMessage("RELOCBS", "Error in saving.", ToastType.Error);
                    }
                }

                ViewBag.Comp = UserSession.GetUserSession().CompanyID == 2 ? "BTR" : "";
                return View(WOSBillingObj);
            }
            catch
            {
                return View();
            }
        }

        private void GetDropDownLists(int RMCID)
        {
            try
            {
                ViewData["CurrencyList"] = ComboBL.GetCurrencyDropdown();
                ViewData["CityList"] = ComboBL.GetCityDropdown(ContinentID: -1, CountryID: -1);
                ViewData["RateCurrency"] = ComboBL.GetRateCurrencyDropdown();
                ViewData["Sequence"] = ComboBL.SequenceDropDown(1);
                ViewData["StorageState"] = ComboBL.GetStorageStateDropdown(1);
                ViewData["PaymentTerm"] = ComboBL.GetPaymentTermList();
                ViewData["BTRServiceList"] = ComboBL.GetBTRServiceList();
                //ViewData["BillingEntityList"] = ComboBL.GetBillingEntityList(1, BillType: 'I');
                //ViewData["CreditNoteEntityList"] = ComboBL.GetBillingEntityList(1, BillType: 'C');
                ViewData["BillingEntityList"] = ComboBL.GetBillingEntityList(RMCID, BillType: 'I', BussLine: 'W');
                ViewData["CreditNoteEntityList"] = ComboBL.GetBillingEntityList(RMCID, BillType: 'C', BussLine: 'W');
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void GetDropDownLists_TransferToFA()
        {
            try
            {
                List<SelectListItem> StatusList = new List<SelectListItem>();
                StatusList.Add(new SelectListItem() { Value = "Exported", Text = "Exported" });
                StatusList.Add(new SelectListItem() { Value = "Finalized", Text = "Finalized" });
                ViewData["StatusList"] = StatusList;

                bool RMCBuss = (UserSession.GetUserSession().BussinessLine == "RMC-BUSINESS");
                ViewData["RevenueBranchList"] = ComboBL.GetCompanyBranchDropdown(CompanyID: UserSession.GetUserSession().CompanyID, IsRMCBuss: RMCBuss, IsRev: true);
                ViewData["ServiceLineList"] = ComboBL.GetServiceLineDropdown(BussLine: "ORIENTATION SERVICE");
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private WOSBilling SetDefaultValues(WOSBilling ObjInv)
        {
            if ((ObjInv.SLShortName == "DMMS" || ObjInv.SLShortName == "MSTG") && ObjInv.BillType == 'I')
            {
                ObjInv.RateCurrancyID = Convert.ToInt32(ComboBL.GetRateCurrencyDropdown().Where(a => a.Value == "2").FirstOrDefault().Value);
                ObjInv.ConvRate = 1;
            }
            return ObjInv;
        }

        public JsonResult GetAddressDetials(int Client_AccountID, Int64 WOSMoveID, string BillTo, char OrgorDest, int BillingEntityID, char BillType = 'I')
        {
            AddressList address = new AddressList();
            address = WOSBillingBL.GetAddressDetials(WOSMoveID, BillTo, OrgorDest, BillingEntityID, BillType);
            return Json(new { result = address }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCityDropdownList(string City)
        {
            int CityID = string.IsNullOrEmpty(City) ? 0 : Convert.ToInt32(City);
            List<SelectListItem> CityList = ComboBL.GetCityDropdown(SPTYPE: "SINGLE", ContinentID: -1, CountryID: -1, CityID: CityID).ToList();
            return Json(new { CityList }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult BillFormat_Print(int? key, char? BillType, bool IsStatement = false, bool IsConsignment = false, int? StrgKey = null)
        {
            WOSBilling BillingObj = new WOSBilling();
            int? BillID = 0; int? CreditnoteID = 0;
            if (BillType == 'I')
            {
                BillID = key;
            }
            else
            {
                CreditnoteID = key;
            }

            int LoginID = UserSession.GetUserSession()?.LoginID ?? 0;
            BillingObj = WOSBillingBL.GetWOSInvoiceDetailsById(LoginID, 0, 0, BillID, CreditnoteID, (char)BillType, 3);

            BillingObj.BillType = (char)BillType;
            if (IsStatement)
            {
                return PartialView("Statement_Print", BillingObj);
            }
            if (IsConsignment)
            {
                return PartialView("ConsignmentNote_Print", BillingObj);
            }
            else
            {
                return View(BillingObj);
            }
        }

        public ActionResult BTRBillFormat_Print(int? key, char? BillType, bool IsStatement = false, bool IsConsignment = false, int? StrgKey = null)
        {
            WOSBilling BillingObj = new WOSBilling();
            int? BillID = 0; int? creditnoteID = 0;
            if (BillType == 'I')
            {
                BillID = key;
            }
            else
            {
                creditnoteID = key;
            }

            int LoginID = UserSession.GetUserSession()?.LoginID ?? 0;
            BillingObj = WOSBillingBL.GetWOSInvoiceDetailsById(LoginID, 0, 0, BillID, creditnoteID, (char)BillType, 3);

            BillingObj.BillType = (char)BillType;
            if (IsStatement)
            {
                return PartialView("Statement_Print", BillingObj);
            }
            if (IsConsignment)
            {
                return PartialView("ConsignmentNote_Print", BillingObj);
            }
            else
            {
                return View(BillingObj);
            }
        }

        public ActionResult ArabicBillFormat_Print(int? key, char? BillType, bool IsStatement = false, bool IsConsignment = false, int? StrgKey = null)
        {
            Billing objmodel = new Billing();
            int? BillID = 0; int? CreditnoteID = 0;
            if (BillType == 'I')
            {
                BillID = key;
            }
            else
            {
                CreditnoteID = key;
            }
            int LoginID = UserSession.GetUserSession()?.LoginID ?? 0;
            objmodel = WOSBillingBL.GetWOSInvoiceDetailsById(LoginID, 0, 0, BillID, CreditnoteID, (char)BillType, 3);

            objmodel.BillType = (char)BillType;
            if (IsStatement)
            {
                return PartialView("Statement_Print", objmodel);
            }
            if (IsConsignment)
            {
                return PartialView("ConsignmentNote_Print", objmodel);
            }
            else
            {
                return View(objmodel);
                //return View();
            }
            /*try
			{
				// TODO: Add delete logic here

				return RedirectToAction("Index");
			}
			catch
			{
				return View();
			}*/
        }

        public ActionResult CancelInvoice(int key)
        {
            try
            {
                string result = string.Empty;
                bool res = new BL.Billing.BillingBL().CancelInvoice(key, out result);
                if (res)
                {
                    if (string.IsNullOrEmpty(result)) result = "Invoice deleted successfully.";
                    this.AddToastMessage("RELOCBS", result, ToastType.Success);
                }
                else
                {
                    if (string.IsNullOrEmpty(result)) result = "Error occured while deleting invoice.";
                    this.AddToastMessage("RELOCBS", result, ToastType.Error);
                }
            }
            catch
            {
                this.AddToastMessage("RELOCBS", "Error in saving.", ToastType.Error);
            }
            return RedirectToAction("Index");
        }

        public JsonResult GenerateEInvoice(string InvNo)
        {
            string result = string.Empty;
            result = new BL.Billing.BillingBL().GenerateEInvoice(InvNo);
            return Json(new { result }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DownloadToExcel(string InvNo)
        {
            string AppName = "NewCBS";

            Dictionary<string, string> exptoExlParameters = new Dictionary<string, string>();
            string errormsg = string.Empty;
            string htmlstring = string.Empty;
            int Colcount = 0;
            try
            {
                DataTable dtgridData = new DataTable();
                exptoExlParameters.Add("@SP_InvoiceNumber", Convert.ToString(InvNo));
                exptoExlParameters.Add("@SP_LoginID", Convert.ToString(UserSession.GetUserSession().LoginID));
                exptoExlParameters.Add("@SP_CompID", Convert.ToString(UserSession.GetUserSession().CompanyID));
                exptoExlParameters.Add("@SP_AppName", Convert.ToString(AppName));
                string param = string.Join(",", exptoExlParameters.Select(x => x.Key + "=" + CSubs.QSafeValue(x.Value)).ToArray()).TrimEnd(',');
                string query = string.Format("EXEC {0} {1}", "[E_Invoice].[EInvoiceSchema]", param);
                dtgridData = CSubs.GetDataTable(query);
                Colcount = dtgridData.Columns.Count;
                htmlstring = "<tr>";
                foreach (DataColumn col in dtgridData.Columns)
                {
                    htmlstring += "<th bgcolor='#DCDCDC'>" + col.ColumnName + "</th>";
                }
                htmlstring += "</tr>";

                foreach (DataRow row in dtgridData.Rows)
                {
                    if (string.IsNullOrEmpty(Convert.ToString(row[0])))
                        htmlstring += "<tr>";
                    else
                        htmlstring += "<tr>";

                    foreach (DataColumn col in dtgridData.Columns)
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
            return Json(new { htmlstring, errormsg, ColCount = Colcount }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult WOSTransferToFA(WOSFundTranfer fundTranfer, int Page = 1)
        {
            try
            {
                if (!CSubs.CheckPageRights(_PageID, PermissionType.VIEW))
                {
                    return new HttpStatusCodeResult(403);
                }

                session.Set("PageSession", "WOS - Transfer To FA");

                string BillNo = string.Empty;
                string Sort = "BillNo";
                string SortDir = "desc";
                string Search = string.Empty;
                int PageSize = settings.GetSettingByKey<int>("pagination_pagesize");

                GetDropDownLists_TransferToFA();

                ViewBag.htmlString = Convert.ToString(TempData["htmlTable"]);
                DataTable dt = new DataTable();

                var result = WOSBillingBL.GetWOSTansferToFAList(fundTranfer, Sort, SortDir, PageSize, out dt);
                fundTranfer.WOSInvoiceList = result.ToList();
                TempData["WOSTansferToFAList"] = dt;
                return View(fundTranfer);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ActionResult Export(WOSFundTranfer fundTranfer)
        {
            try
            {
                if (TempData["WOSTansferToFAList"] != null)
                {
                    DataTable dt = ((DataTable)TempData["WOSTansferToFAList"]);
                    DataTable dtWOSTransferToFA = dt.Clone();
                    fundTranfer.WOSInvoiceList = fundTranfer.WOSInvoiceList.Where(x => x.IsExport == true).ToList();
                    string message = string.Empty;
                    bool result = false;
                    if (fundTranfer.Status == "Exported")
                    {
                        result = true;
                        message = "Data Exported Successfully.";
                    }
                    else
                    {
                        result = WOSBillingBL.InsertWOSTransferToFA(fundTranfer, out message);
                    }

                    if (result)
                    {
                        foreach (var item in fundTranfer.WOSInvoiceList)
                        {
                            DataRow row = dt.Select("[InvOrCredit] ='" + item.InvOrCredit + "' and [CBSRefID]='" + item.CBSRefID + "'").First();
                            row.SetField("ID PAID TO RECEIVED FROM 2ND LEG", item.AccountCode);

                            string Code = string.Empty;

                            string CustomerCode = string.Empty;
                            string[] arrCustomerCode = Convert.ToString(row["CUSTOMER CODE"]).Split(new char[] { '-' });
                            if (arrCustomerCode.Length > 1)
                                CustomerCode = arrCustomerCode[1];

                            Code = item.AccountCode + "-" + CustomerCode;
                            row.SetField("CUSTOMER CODE", Code);

                            string PlaceOfSupply = string.Empty;
                            string[] arrPlaceOfSupply = Convert.ToString(row["Place of Supply"]).Split(new char[] { '-' });
                            if (arrPlaceOfSupply.Length > 1)
                                PlaceOfSupply = arrPlaceOfSupply[1];

                            Code = item.AccountCode + "-" + PlaceOfSupply;
                            row.SetField("Place of Supply", Code);
                            dtWOSTransferToFA.Rows.Add(row.ItemArray);
                        }

                        if (dtWOSTransferToFA.Columns.Contains("CBSRefID")) dtWOSTransferToFA.Columns.Remove("CBSRefID");
                        if (dtWOSTransferToFA.Columns.Contains("InvOrCredit")) dtWOSTransferToFA.Columns.Remove("InvOrCredit");
                        if (dtWOSTransferToFA.Columns.Contains("BillTo")) dtWOSTransferToFA.Columns.Remove("BillTo");

                        TempData["htmlTable"] = MakeHtmlTable(dtWOSTransferToFA);

                        this.AddToastMessage("RELOCBS", message, ToastType.Success);
                    }
                    else
                    {
                        this.AddToastMessage("RELOCBS", message, ToastType.Error);
                    }
                }
            }
            catch
            {
                this.AddToastMessage("RELOCBS", "Error in saving.", ToastType.Error);
            }
            return RedirectToAction("WOSTransferToFA", "WOSBilling");
        }

        public ActionResult WOSTransferToFAGCC(WOSFundTranfer fundTranfer, int Page = 1)
        {
            try
            {
                if (!CSubs.CheckPageRights(_PageID, PermissionType.VIEW))
                {
                    return new HttpStatusCodeResult(403);
                }

                session.Set("PageSession", "WOS - Transfer To FA GCC");

                string BillNo = string.Empty;
                string Sort = "BillNo";
                string SortDir = "desc";
                string Search = string.Empty;
                int PageSize = settings.GetSettingByKey<int>("pagination_pagesize");

                GetDropDownLists_TransferToFA();

                ViewBag.htmlString = Convert.ToString(TempData["htmlTable"]);
                DataTable dt = new DataTable();

                var result = WOSBillingBL.GetWOSTansferToFAGCCList(fundTranfer, Sort, SortDir, PageSize, out dt);
                fundTranfer.WOSInvoiceList = result.ToList();
                TempData["WOSTansferToFAGCCList"] = dt;
                return View(fundTranfer);
            }
            catch (Exception)
            {
                throw;
            }
        }

        //public ActionResult GCCExport(WOSFundTranfer fundTranfer)
        //{
        //    try
        //    {
        //        if (TempData["WOSTansferToFAGCCList"] != null)
        //        {
        //            DataTable dt = ((DataTable)TempData["WOSTansferToFAGCCList"]);
        //            DataTable dtWOSTransferToFAGCC = dt.Clone();
        //            fundTranfer.WOSInvoiceList = fundTranfer.WOSInvoiceList.Where(x => x.IsExport == true).ToList();
        //            string message = string.Empty;
        //            bool result = false;
        //            if (fundTranfer.Status == "Exported")
        //            {
        //                result = true;
        //                message = "Data Exported Successfully.";
        //            }
        //            else
        //            {
        //                result = WOSBillingBL.InsertWOSTransferToFA(fundTranfer, out message);
        //            }

        //            if (result)
        //            {
        //                foreach (var item in fundTranfer.WOSInvoiceList)
        //                {
        //                    DataRow row = dt.Select("[InvOrCredit] ='" + item.InvOrCredit + "' and [CBSRefID]='" + item.CBSRefID + "'").First();
        //                    dtWOSTransferToFAGCC.Rows.Add(row.ItemArray);
        //                }

        //                if (dtWOSTransferToFAGCC.Columns.Contains("CBSRefID")) dtWOSTransferToFAGCC.Columns.Remove("CBSRefID");
        //                if (dtWOSTransferToFAGCC.Columns.Contains("InvOrCredit")) dtWOSTransferToFAGCC.Columns.Remove("InvOrCredit");
        //                if (dtWOSTransferToFAGCC.Columns.Contains("BillTo")) dtWOSTransferToFAGCC.Columns.Remove("BillTo");
        //                if (dtWOSTransferToFAGCC.Columns.Contains("SrNo")) dtWOSTransferToFAGCC.Columns.Remove("SrNo");

        //                TempData["htmlTable"] = MakeHtmlTable(dtWOSTransferToFAGCC);

        //                this.AddToastMessage("RELOCBS", message, ToastType.Success);
        //            }
        //            else
        //            {
        //                this.AddToastMessage("RELOCBS", message, ToastType.Error);
        //            }
        //        }
        //    }
        //    catch
        //    {
        //        this.AddToastMessage("RELOCBS", "Error in saving.", ToastType.Error);
        //    }
        //    return RedirectToAction("WOSTransferToFAGCC", "WOSBilling");
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GCCExport(WOSFundTranfer model, string SearchForText)
        {
            //_PageID = "62";
            //if (!CSubs.CheckPageRights(_PageID, PermissionType.VIEW))
            //{
                //return new HttpStatusCodeResult(403);
            //}
            session.Set<string>("PageSession", "Transfer To FA Dubai");
            //FillCombo(ForPage: "TransFA_Duabi");

            //if (ModelState.IsValid)
            //{
            ///Check if the Exported or Finalized is selected
            if (!string.IsNullOrWhiteSpace(SearchForText))
            {

                string message = string.Empty;

                if (TempData["WOSTansferToFAGCCList"] != null)
                {
                    DataTable data = (DataTable)TempData["WOSTansferToFAGCCList"];
                    //System.Data.DataTable dtFAList = data.Clone();
                    model.SearchFor = SearchForText;
                    DataTable result = WOSBillingBL.GetDubaiTAExport(model, data, out message);

                    if (result != null && result.Rows.Count > 0)
                    {
                        TempData["htmltable"] = MakeHtmlTable(result);
                        TempData["WOSTansferToFAList"] = null;
                        this.AddToastMessage("RELOCBS", message, ToastType.Success);
                    }
                    else
                    {
                        this.AddToastMessage("RELOCBS", message, ToastType.Error);
                    }
                }
                else
                {
                    this.AddToastMessage("RELOCBS", "No data to Export", ToastType.Error);
                }
            }
            else
            {
                this.AddToastMessage("RELOCBS", "Status is required", ToastType.Error);
            }
            //}

            return RedirectToAction("WOSTransferToFAGCC", "WOSBilling");
        }

        private string MakeHtmlTable(DataTable dt)
        {
            try
            {
                string[] table = new string[dt.Rows.Count];
                long counter = 1;
                string htmlstring = "<tr>";
                foreach (DataColumn col in dt.Columns)
                {
                    htmlstring += "<th bgcolor=\"#DCDCDC\">" + col.ColumnName + "</th>";
                }
                htmlstring += "</tr>";
                foreach (DataRow row in dt.Rows)
                {
                    table[counter - 1] = "<tr><td>" + string.Join("</td><td>", (from o in row.ItemArray select o.ToString().Trim()).ToArray()) + "</td></tr>";
                    counter += 1;
                }
                return "<table border=\"2px\">" + htmlstring + string.Join("", table) + "</table>";
            }
            catch (Exception)
            {
                throw;
            }
        }

        public JsonResult GetEntityDropDownList(int RMCID)
        {
            List<SelectListItem> EntityList = ComboBL.GetBillingEntityList(RMCID, BillType: 'I', BussLine: 'W').ToList();
            return Json(new { EntityList = EntityList }, JsonRequestBehavior.AllowGet);
        }
    }
}