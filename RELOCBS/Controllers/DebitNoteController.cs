using Newtonsoft.Json;
using PagedList;
using RELOCBS.App_Code;
using RELOCBS.BL;
using RELOCBS.BL.DebitNote;
using RELOCBS.Common;
using RELOCBS.CustomAttributes;
using RELOCBS.Entities;
using RELOCBS.Extensions;
using RELOCBS.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.Mvc;

namespace RELOCBS.Controllers
{
    [AuthorizeUser]
    public class DebitNoteController : BaseController
    {
        private string _PageID = "67";

        private CommonSubs _cSubs;
        public CommonSubs CSubs
        {
            get
            {
                if (_cSubs == null)
                    _cSubs = new CommonSubs();
                return _cSubs;
            }
        }

        private ComboBL _comboBL;
        public ComboBL comboBL
        {
            get
            {
                if (_comboBL == null)
                    _comboBL = new ComboBL();
                return _comboBL;
            }
        }

        private CommanBL _comBL;
        public CommanBL comBL
        {
            get
            {
                if (_comBL == null)
                    _comBL = new CommanBL();
                return _comBL;
            }
        }

        private DebitNoteBL _debitNoteBL;
        public DebitNoteBL debitNoteBL
        {
            get
            {
                if (this._debitNoteBL == null)
                    this._debitNoteBL = new DebitNoteBL();
                return this._debitNoteBL;
            }
        }

        DebitNote _debitNote = new DebitNote();
        public DebitNoteController(DebitNote DebitNote)
        {
            _debitNote = DebitNote;
        }

        public ActionResult Index(int Page = 1)
        {
            try
            {
                if (!CSubs.CheckPageRights(_PageID, PermissionType.VIEW))
                {
                    return new HttpStatusCodeResult(403);
                }

                session.Set("PageSession", "Debit Note");

                Int64? DebitNoteId = null;
                string Sort = "DebitNoteId";
                string SortDir = "desc";
                int PageSize = settings.GetSettingByKey<int>("pagination_pagesize");
                int Order = 0;
                DateTime? FromDate = null;
                DateTime? ToDate = null;
                string SearchType = string.Empty;
                string SearchValue = string.Empty;

                if (Request.Form["DebitNoteId"] != null && Request.Form["DebitNoteId"].Trim() != "")
                    DebitNoteId = Convert.ToInt64(Request.Form["DebitNoteId"]);

                if (Request.Form["FromDate"] != null && Request.Form["FromDate"].Trim() != "")
                    FromDate = Convert.ToDateTime(Request.Form["FromDate"]);

                if (Request.Form["ToDate"] != null && Request.Form["ToDate"].Trim() != "")
                    ToDate = Convert.ToDateTime(Request.Form["ToDate"]);

                if (Request.Form["SearchType"] != null && Request.Form["SearchType"].Trim() != "")
                    SearchType = Convert.ToString(Request.Form["SearchType"]);

                if (Request.Form["SearchValue"] != null && Request.Form["SearchValue"].Trim() != "")
                    SearchValue = Convert.ToString(Request.Form["SearchValue"]);

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
                var DebitNoteList = debitNoteBL.GetDebitNoteList(Sort, SortDir, Skip, PageSize, FromDate, ToDate, SearchType, SearchValue, DebitNoteId, "D", out TotalRecord);

                var itemsAsIPagedList = new StaticPagedList<DebitNote>(DebitNoteList, Page, PageSize, TotalRecord);

                if (SearchType == "CreditNoteNo" && itemsAsIPagedList.Count > 0 && itemsAsIPagedList.ToList().First().IsShowCreditNote)
                {
                    ViewBag.CreditNoteList = GetCreditNote(Convert.ToInt64(itemsAsIPagedList.ToList().First().DebitNoteId));
                }
                else
                {
                    ViewBag.CreditNoteList = new List<DebitNote>();
                }

                return Request.IsAjaxRequest() ? (ActionResult)PartialView("_GridPartial", itemsAsIPagedList) : View(itemsAsIPagedList);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<DebitNote> GetCreditNote(Int64 DebitNoteId)
        {
            try
            {
                int TotalRecord = 0;
                string Sort = "DNCreditNoteId";
                string SortDir = "desc";
                List<DebitNote> CreditNoteList = debitNoteBL.GetDebitNoteList(Sort, SortDir, 0, 0, null, null, null, null, DebitNoteId, "C", out TotalRecord).ToList();
                return CreditNoteList;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public ActionResult GetCreditNoteList(Int64 DebitNoteId)
        {
            try
            {
                List<DebitNote> CreditNoteList = GetCreditNote(DebitNoteId);
                ViewBag.CreditNoteList = CreditNoteList;
                return PartialView("_GridPartialCreditNote", CreditNoteList);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpGet]
        public ActionResult Create(string Key, string DrOrCrNote)
        {
            try
            {
                if (DrOrCrNote == "D")
                    session.Set("PageSession", "Debit Note");
                else
                    session.Set("PageSession", "Credit Note");

                var list = CommonService.GetQueryString(Key);
                Int64 DebitNoteId = 0;
                Int64 DNCreditNoteId = 0;

                if (list.ContainsKey("DebitNoteId"))
                    DebitNoteId = Convert.ToInt64(list["DebitNoteId"]);

                if (list.ContainsKey("DNCreditNoteId"))
                    DNCreditNoteId = Convert.ToInt64(list["DNCreditNoteId"]);

                _debitNote.DebitNoteId = DebitNoteId;
                _debitNote.DNCreditNoteId = DNCreditNoteId;

                IEnumerable<DebitNoteDetails> DebitNoteDetailsList = new List<DebitNoteDetails>();

                _debitNote = debitNoteBL.GetDebitNoteDetailsById(DebitNoteId, DNCreditNoteId, DrOrCrNote, out DebitNoteDetailsList);

                if (DrOrCrNote == "C" && TempData["DebitNoteDetailsList"] != null)
                {
                    this.AddToastMessage("RELOCBS", "Error occured while saving credit note.", ToastType.Error);
                    _debitNote.DebitNoteDetailsList = (List<DebitNoteDetails>)TempData["DebitNoteDetailsList"];
                    TempData["DebitNoteDetailsList"] = null;
                }

                GetDropDownLists();
                ViewBag.DebitNoteDetailsList = DebitNoteDetailsList;
                _debitNote.DrOrCrNote = DrOrCrNote;
                return View(_debitNote);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult Create(DebitNote debitNote, string SaveButton)
        {
            try
            {
                string message = string.Empty;
                string Status = string.Empty;
                string DrOrCrNote = string.Empty;

                if (SaveButton.ToUpper() == "SAVE DEBIT NOTE")
                {
                    Status = "Draft";
                    DrOrCrNote = "D";
                }
                else if (SaveButton.ToUpper() == "APPROVE DEBIT NOTE")
                {
                    Status = "Finalized";
                    DrOrCrNote = "D";
                }
                else if (SaveButton.ToUpper() == "SAVE CREDIT NOTE")
                {
                    Status = "Draft";
                    DrOrCrNote = "C";
                }
                else if (SaveButton.ToUpper() == "APPROVE CREDIT NOTE")
                {
                    Status = "Finalized";
                    DrOrCrNote = "C";
                }

                if ((!ModelState.IsValid || (debitNote.DNType.ToUpper() == "STOCK TRANSFER" && debitNote.Debtor.DebtorId <= 0)) && DrOrCrNote == "D")
                {
                    if (debitNote.DNType?.ToUpper() == "STOCK TRANSFER" && debitNote.Debtor.DebtorId <= 0)
                    {
                        ModelState.AddModelError(string.Empty, "Please select debtor from debtor master for Stock Transfer debit note type.");
                    }

                    if (!string.IsNullOrEmpty(debitNote.DebitNoteDetailsListHidden))
                    {
                        debitNote.DebitNoteDetailsList = (from r in JsonConvert.DeserializeXNode(debitNote.DebitNoteDetailsListHidden, "DebitNoteDetails").Root.Elements("DebitNoteDetail")
                                                          select new DebitNoteDetails
                                                          {
                                                              DebitNoteDetailId = (Int64?)r.Element("DebitNoteDetailId") ?? 0,
                                                              DebitNoteId = (Int64?)r.Element("DebitNoteId"),
                                                              DNCreditNoteDetailId = (Int64?)r.Element("DNCreditNoteDetailId") ?? 0,
                                                              DNCreditNoteId = (Int64?)r.Element("DNCreditNoteId"),
                                                              DNCostHeadId = (int?)r.Element("DNCostHeadId"),
                                                              CostHead = (string)r.Element("CostHead"),
                                                              Description = (string)r.Element("Description"),
                                                              JobNo = (string)r.Element("JobNo"),
                                                              SacCode = (string)r.Element("SacCode"),
                                                              CurrencyID = (int?)r.Element("CurrencyID"),
                                                              Currency = (string)r.Element("Currency"),
                                                              Quantity = (decimal?)r.Element("Quantity"),
                                                              Rate = (decimal?)r.Element("Rate"),
                                                              UnitId = (int?)r.Element("UnitId"),
                                                              Unit = (string)r.Element("Unit"),
                                                              DebitAmount = (decimal?)r.Element("DebitAmount"),
                                                              CreditAmount = (decimal?)r.Element("CreditAmount"),
                                                              MaxCreditAmount = (decimal?)r.Element("MaxCreditAmount"),
                                                              TaxPercent = (decimal)r.Element("TaxPercent"),
                                                              CGSTAmount = (decimal?)r.Element("CGSTAmount"),
                                                              SGSTAmount = (decimal?)r.Element("SGSTAmount"),
                                                              IGSTAmount = (decimal?)r.Element("IGSTAmount"),
                                                              VATAmount = (decimal?)r.Element("VATAmount"),
                                                              TotalAmount = (decimal?)r.Element("TotalAmount")
                                                          }).ToList();
                    }

                    if (DrOrCrNote == "D")
                    {
                        GetDropDownLists();
                        debitNote.DrOrCrNote = DrOrCrNote;
                        return View("Create", debitNote);
                    }
                    else
                    {
                        TempData["DebitNoteDetailsList"] = debitNote.DebitNoteDetailsList;
                        return RedirectToAction("Create", new { Key = CommonService.GetCrypt("DebitNoteId=" + debitNote.DebitNoteId.ToString() + "&DNCreditNoteId=" + debitNote.DNCreditNoteId.ToString(), 1), DrOrCrNote });
                    }
                }
                else
                {
                    bool result = false;

                    result = debitNoteBL.SaveDebitNote(debitNote, Status, DrOrCrNote, out message);
                    if (!result)
                    {
                        ModelState.AddModelError(string.Empty, "Error occured while saving " + DrOrCrNote == "D" ? "debit note." : "credit note.");
                        this.AddToastMessage("RELOCBS", message, ToastType.Error);
                    }
                    else
                    {
                        this.AddToastMessage("RELOCBS", message, ToastType.Success);
                    }
                    return RedirectToAction("Create", new { Key = CommonService.GetCrypt("DebitNoteId=" + debitNote.DebitNoteId.ToString() + "&DNCreditNoteId=" + debitNote.DNCreditNoteId.ToString(), 1), DrOrCrNote });
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public ActionResult Delete(string Key, string DrOrCrNote)
        {
            try
            {
                string message = string.Empty;
                var list = CommonService.GetQueryString(Key);

                Int64 DebitNoteId = 0;
                Int64 DNCreditNoteId = 0;

                if (list.ContainsKey("DebitNoteId"))
                    DebitNoteId = Convert.ToInt64(list["DebitNoteId"]);

                if (list.ContainsKey("DNCreditNoteId"))
                    DNCreditNoteId = Convert.ToInt64(list["DNCreditNoteId"]);

                _debitNote.DebitNoteId = DebitNoteId;
                _debitNote.DNCreditNoteId = DNCreditNoteId;

                bool result = false;
                result = debitNoteBL.DeleteDebitNote(out message, DebitNoteId, DNCreditNoteId, DrOrCrNote);

                if (!result)
                {
                    ModelState.AddModelError(string.Empty, "Error occured while deleting " + DrOrCrNote == "C" ? "credit note." : "debitnote.");
                    this.AddToastMessage("RELOCBS", message, ToastType.Error);
                }
                else
                {
                    this.AddToastMessage("RELOCBS", message, ToastType.Success);
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public JsonResult GetRevenueBranchList(string ReportName, int? SBUId)
        {
            try
            {
                var lstItem = comboBL.GetCompanyBranchDropdown(IsRMCBuss: !(UserSession.GetUserSession().BussinessLine == "NON RMC-BUSINESS"), ForPage: "DebitNote", SBUId: SBUId);
                return Json(new { lstItem }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public JsonResult GetDNCostHeadList(int? DNTypeId)
        {
            try
            {
                var lstItem = comboBL.GetDebitNoteCostHeadDropdown(DNTypeId: DNTypeId);
                return Json(new { lstItem }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public JsonResult GetDebtorDetails(int DebtorId)
        {
            try
            {
                Debtor Debtor = debitNoteBL.GetDebtorDetails(DebtorId);
                string ErrorMsg = Debtor == null ? "Debtor details not found." : "";
                return Json(new { Debtor, ErrorMsg }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public JsonResult GetDebitNoteTaxType(int RevenueBrId, int POSStateId)
        {
            try
            {
                DebitNote DebitNoteTaxTypeObj = debitNoteBL.GetDebitNoteTaxType(RevenueBrId, POSStateId);
                string ErrorMsg = DebitNoteTaxTypeObj == null ? "Error occured." : "";
                string TaxType = string.Empty;
                if (DebitNoteTaxTypeObj != null)
                {
                    TaxType = DebitNoteTaxTypeObj.TaxType;
                }
                return Json(new { TaxType, ErrorMsg }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public JsonResult GetDebitNoteInvType(int CityID)
        {
            try
            {
                DebitNote GetDebitNoteInvTypeObj = debitNoteBL.GetDebitNoteInvType(CityID);
                string ErrorMsg = GetDebitNoteInvTypeObj == null ? "Error occured." : "";
                string InvType = string.Empty;
                if (GetDebitNoteInvTypeObj != null)
                {
                    InvType = GetDebitNoteInvTypeObj.InvType;
                }
                return Json(new { InvType, ErrorMsg }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public JsonResult GetDebitNoteTaxRate(int SBUId, int DNCostHeadId)
        {
            try
            {
                DebitNote DebitNoteTaxRateObj = debitNoteBL.GetDebitNoteTaxRate(SBUId, DNCostHeadId);
                string ErrorMsg = DebitNoteTaxRateObj == null ? "Error occured." : "";
                decimal? TaxRate = null;
                string SacCode = string.Empty;
                if (DebitNoteTaxRateObj != null)
                {
                    TaxRate = DebitNoteTaxRateObj.TaxRate;
                    SacCode = DebitNoteTaxRateObj.DebitNoteDetails.SacCode;
                }
                return Json(new { TaxRate, SacCode, ErrorMsg }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ActionResult DebitNoteInvoice_Print(string Key, string DrOrCrNote)
        {
            try
            {
                var list = CommonService.GetQueryString(Key);

                Int64 DebitNoteId = 0;
                Int64 DNCreditNoteId = 0;

                if (list.ContainsKey("DebitNoteId"))
                    DebitNoteId = Convert.ToInt64(list["DebitNoteId"]);

                if (list.ContainsKey("DNCreditNoteId"))
                    DNCreditNoteId = Convert.ToInt64(list["DNCreditNoteId"]);

                IEnumerable<DebitNoteDetails> DebitNoteDetailsList = new List<DebitNoteDetails>();
                _debitNote = debitNoteBL.GetDebitNoteDetailsById(DebitNoteId, DNCreditNoteId, DrOrCrNote, out DebitNoteDetailsList);
                _debitNote.DrOrCrNote = DrOrCrNote;
                return View(_debitNote);
            }
            catch
            {
                throw;
            }
        }

        public JsonResult GenerateEInvoiceDebitNote(string InvoiceNo)
        {
            try
            {
                string result = string.Empty;
                result = debitNoteBL.GenerateEInvoiceDebitNote(InvoiceNo);
                return Json(new { result }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public JsonResult DownloadToExcel(string InvoiceNo)
        {
            try
            {
                Dictionary<string, string> exptoExlParameters = new Dictionary<string, string>();
                string ErrorMsg = string.Empty;
                string htmlstring = string.Empty;
                int ColCount = 0;
                try
                {
                    DataTable dtgridData = new DataTable();
                    exptoExlParameters.Add("@SP_InvoiceNo", Convert.ToString(InvoiceNo));
                    exptoExlParameters.Add("@SP_LoginID", Convert.ToString(UserSession.GetUserSession().LoginID));
                    string param = string.Join(",", exptoExlParameters.Select(x => x.Key + "=" + CSubs.QSafeValue(x.Value)).ToArray()).TrimEnd(',');
                    string query = string.Format("EXEC {0} {1}", "[E_Invoice].[EInvoiceSchemaDebitNote]", param);
                    dtgridData = CSubs.GetDataTable(query);
                    ColCount = dtgridData.Columns.Count;
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
                return Json(new { htmlstring, ErrorMsg, ColCount }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public ActionResult DNTransferToFA(DNFundTranfer fundTranfer, int Page = 1)
        {
            try
            {
                if (!CSubs.CheckPageRights(_PageID, PermissionType.VIEW))
                {
                    return new HttpStatusCodeResult(403);
                }

                session.Set("PageSession", "Debit Note Transfer To FA");

                string BillNo = string.Empty;
                string Sort = "BillNo";
                string SortDir = "desc";
                string Search = string.Empty;
                int PageSize = settings.GetSettingByKey<int>("pagination_pagesize");

                GetDropDownLists();

                ViewBag.htmlString = Convert.ToString(TempData["htmlTable"]);
                DataTable dt = new DataTable();

                var result = debitNoteBL.GetDNTansferToFAList(fundTranfer, Sort, SortDir, PageSize, out dt);
                fundTranfer.DNInvoiceList = result.ToList();
                TempData["DNTansferToFAList"] = dt;
                return View(fundTranfer);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ActionResult Export(DNFundTranfer fundTranfer)
        {
            try
            {
                if (TempData["DNTansferToFAList"] != null)
                {

                    DataTable dt = ((DataTable)TempData["DNTansferToFAList"]);
                    DataTable dtDNTransferToFA = dt.Clone();
                    fundTranfer.DNInvoiceList = fundTranfer.DNInvoiceList.Where(x => x.IsExport == true).ToList();
                    string message = string.Empty;
                    bool result = false;
                    if (fundTranfer.Status == "Exported")
                    {
                        result = true;
                        message = "Data Exported Successfully.";
                    }
                    else
                    {
                        result = debitNoteBL.InsertDNTransferToFA(fundTranfer, out message);
                    }

                    if (result)
                    {
                        foreach (var item in fundTranfer.DNInvoiceList)
                        {
                            DataRow row = dt.Select("[DebitOrCredit] ='" + item.DebitOrCredit + "' and [CBSRefID]='" + item.CBSRefID + "'").First();
                            row.SetField("Paid to /Received From (2nd Leg)", item.AccountCode);

                            string Code = string.Empty;

                            string CustomerCode = string.Empty;
                            string[] arrCustomerCode = Convert.ToString(row["Customer Code"]).Split(new char[] { '-' });
                            if (arrCustomerCode.Length > 1)
                                CustomerCode = arrCustomerCode[1];

                            Code = item.AccountCode + "-" + CustomerCode;
                            row.SetField("Customer Code", Code);

                            string PlaceOfSupply = string.Empty;
                            string[] arrPlaceOfSupply = Convert.ToString(row["Place of Supply"]).Split(new char[] { '-' });
                            if (arrPlaceOfSupply.Length > 1)
                                PlaceOfSupply = arrPlaceOfSupply[1];

                            Code = item.AccountCode + "-" + PlaceOfSupply;
                            row.SetField("Place of Supply", Code);
                            dtDNTransferToFA.Rows.Add(row.ItemArray);
                        }
                        dtDNTransferToFA.Columns.Remove("CBSRefID");
                        dtDNTransferToFA.Columns.Remove("DebitOrCredit");
                        dtDNTransferToFA.Columns.Remove("DebtorName");

                        TempData["htmlTable"] = MakeHtmlTable(dtDNTransferToFA);

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
            return RedirectToAction("DNTransferToFA", "DebitNote");
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

        private void GetDropDownLists()
        {
            try
            {
                ViewData["SBUList"] = comboBL.GetSBUDropdown();
                ViewData["DNTypeList"] = comboBL.GetDebitNoteTypeDropdown();
                ViewData["StateList"] = comboBL.GetStorageStateDropdown(0);
                ViewData["CurrencyList"] = comboBL.GetCurrencyDropdown();
                ViewData["DebtorList"] = comboBL.GetDebtorDropdown();
                ViewData["DNUnitList"] = comboBL.GetDebitNoteUnitDropdown();

                List<SelectListItem> StatusList = new List<SelectListItem>();
                StatusList.Add(new SelectListItem() { Value = "Exported", Text = "Exported" });
                StatusList.Add(new SelectListItem() { Value = "Finalized", Text = "Finalized" });
                ViewData["StatusList"] = StatusList;

                bool RMCBuss = !(UserSession.GetUserSession().BussinessLine == "NON RMC-BUSINESS");
                ViewData["RevenueBranchList"] = comboBL.GetCompanyBranchDropdown(CompanyID: UserSession.GetUserSession().CompanyID, IsRMCBuss: RMCBuss, IsRev: true);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}