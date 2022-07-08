using RELOCBS.AjaxHelper;
using RELOCBS.App_Code;
using RELOCBS.BL;
using RELOCBS.BL.StorageBilling;
using RELOCBS.CustomAttributes;
using RELOCBS.Entities;
using RELOCBS.Extensions;
using RELOCBS.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RELOCBS.Controllers
{
    [AuthorizeUser]
    public class StorageBillingController : BaseController
    {
        private string _PageID = "33";
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

        private StorageBillingBL _storageBillBL;
        public StorageBillingBL storageBillBL
        {
            get
            {
                if (this._storageBillBL == null)
                    this._storageBillBL = new StorageBillingBL();
                return this._storageBillBL;
            }
        }


        // GET: StorageBilling
        public ActionResult Index(int page = 1)
        {
            if (!CSubs.CheckPageRights(_PageID, PermissionType.VIEW))
            {
                return new HttpStatusCodeResult(403);
            }

            session.Set<string>("PageSession", "Storage Billing");
            string sort = "JobNo";
            string sortdir = "desc";
            string search = "";
            Int64 JobNo = -1;
            Int64 Strg_ID = -1;
            int pageSize = settings.GetSettingByKey<int>("pagination_pagesize");
            string OrderBy = "";
            int Order = 0;
            DateTime? Fromdate = null;//System.DateTime.Now.Date.AddDays(-2);
            DateTime? Todate = null;//System.DateTime.Now;
            string Shipper = "";
            string BillType=string.Empty;


            string SearchKey = string.Empty;
            if (Request.Form["FromDate"] != null && Request.Form["FromDate"].Trim() != "")
            {
                Fromdate = Convert.ToDateTime(Request.Form["FromDate"]);
            }
            if (Request.Form["JobNo"] != null && Request.Form["JobNo"].Trim() != "")
            {
                JobNo = Convert.ToInt64(Request.Form["JobNo"]);
            }

            if (Request.Form["Strg_ID"] != null && Request.Form["Strg_ID"].Trim() != "")
            {
                Strg_ID = Convert.ToInt64(Request.Form["Strg_ID"]);
            }

            if (Request.Form["ToDate"] != null && Request.Form["ToDate"].Trim() != "")
            {
                Todate = Convert.ToDateTime(Request.Form["ToDate"]);
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
            
            if (Request.Params["BillType"] != null && Request.Params["BillType"].Trim() != "")
            {
                BillType = Convert.ToString(Request.Params["BillType"]);

            }

            int totalRecord = 0;
            if (page < 1) page = 1;
            //int skip = (pageNo * pageSize) - pageSize;
            int skip = pageSize;
            pageSize = page;
            var data = storageBillBL.GetBillGrid(Fromdate, Todate, BillType, Shipper, JobNo, sort, sortdir, skip, pageSize, out totalRecord);
            fillCombo();
            ViewData["MoveId"] = JobNo;
            //ViewData["StrgId"] = Strg_ID;

            ViewBag.TotalRows = totalRecord;
            ViewBag.search = search;

            var itemsAsIPagedList = new PagedList.StaticPagedList<Entities.StorageBillGrid>(data, pageSize, skip, totalRecord);
            return Request.IsAjaxRequest() ? (ActionResult)PartialView("_AjaxPartial", itemsAsIPagedList) : View(itemsAsIPagedList);
        }

        private void fillCombo()
        {
            //ViewData["UnitList"] = comboBL.GetMeasurementUnitDropdown(UnitType: 'B');
            ViewData["EmpList"] = comboBL.GetEmployeeDropdown();
            ViewData["CommodityList"] = comboBL.GetGoodsDescriptionDropdown();
            ViewData["WarehouseList"] = comboBL.GetWarehouseDropdown();
            ViewData["CurrencyList"] = comboBL.GetCurrencyDropdown();
            ViewData["StatusList"] = comboBL.GetClaimStatusDropdown();
            //ViewData["InsuranceCompList"] = comboBL.GetInsuranceCompanyDropdown();
            ViewData["JobNoList"] = comboBL.getJobNolDropdown(IsStorage:true); ///new List<SelectListItem>();
            ViewData["AddBillCostHeadList"] = comboBL.GetStrgCostHeadDropdown(StrgCostType : "BILL");
        }

        // GET: StorageBilling/Details/5GetAddCostHeadTax
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: StorageBilling/Create type= "AI" (Alter iNVOICE) AND "NI" (New Invoice) AND "NC" (New CreditNote) AND "AC" (Alter CreditNote) 
        public ActionResult Create(Int64 MoveID,Int64 StorageID,Int64? BillID, string type)
        {
            session.Set<string>("PageSession", "Storage Billing");
            Int64 InvoiceID = 0; Int64 CreditNoteID = 0;
            char Type = 'I';
            if (type.Equals("AI"))
            {
                InvoiceID = Convert.ToInt64(BillID);
            }

            if (type.Equals("NC"))
            {
                Type = 'C';
            }

            if (type.Equals("AC"))
            {
                Type = 'C';
                CreditNoteID = Convert.ToInt64(BillID);
            }
            
            
            var model = storageBillBL.GetBillDetails(MoveID, StorageID, InvoiceID,CreditNoteID,type);
            model.BillType = Type;
            model.InvType = type;
            fillCombo();
            return View(model);
        }

        // POST: StorageBilling/Create  ///Int64 MoveID, Int64 StorageID, Int64? BillID, string type,
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(StorageBill model,string SubmitInvoice)
        {
            session.Set<string>("PageSession", "Storage Billing");
            string message=string.Empty;
            fillCombo();
            try
            {
                if (ModelState.IsValid)
                {
                    bool Isauditamount = true;
                    bool SaveTopOnly = false;
                    string status = SubmitInvoice;
                    if ((string.IsNullOrEmpty(model.InvoiceStatus) || model.InvoiceStatus == "Draft") && (SubmitInvoice.Equals("Save")))
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
                    if (SubmitInvoice.Equals("Send To Finance"))
                    {
                        Isauditamount = false;
                    }
                    if (SubmitInvoice.Equals("Process Invoice"))
                    {
                        SaveTopOnly = true;
                        status = "Draft";
                    }

                    if (SubmitInvoice.Equals("Process"))
                    {
                        SaveTopOnly = true;
                        status = "Draft";
                    }

                    bool success = SaveTopOnly ? storageBillBL.GetStrgBillProcess(model,model.ProcessRowIndex, out message) : storageBillBL.Insert(model, out message);//storageBillBL.SaveInvoice(billing, status, Isauditamount, SaveTopOnly);

                    if (success)
                    {
                        //if (!SaveTopOnly)
                        //{

                            if(!string.IsNullOrWhiteSpace(message))
                                this.AddToastMessage("RELOCBS", message, ToastType.Success);
                        //}

                        if (SubmitInvoice.Equals("Send to Finance"))
                        {
                            return RedirectToActionPermanent("Index", "MoveManage");
                        }
                        else
                        {
                            return RedirectToAction("Create", new { MoveID = model.MoveID, StorageID = model.StorageID,BillID=model.BillID,type = (model.BillType == 'C' ? "AC" : "AI") });

                            ///return RedirectToActionPermanent("Create", new { key = model.BillID, PageIndex = 3, type = (billing.BillType == 'C' ? "AC" : "AI") });
                        }

                    }
                    else
                    {
                        message = !string.IsNullOrWhiteSpace(message) ? message : "Unable to save data.";
                        ModelState.AddModelError(string.Empty, message);
                        this.AddToastMessage("RELOCBS", message, ToastType.Error);
                        return View("Create", model);
                    }
                    
                }
                else
                {
                    return View(model);
                }
            }
            catch
            {
                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetProcessBill(StorageBill model)
        {
            
            fillCombo();
            string message = string.Empty;
            var res = storageBillBL.GetStrgBillProcess(model,0,out message);
            if (!res)
            {
                ModelState.AddModelError(string.Empty, "Unable to save data.");
                this.AddToastMessage("RELOCBS", message, ToastType.Error);
                return RedirectToAction("Create", new { MoveID = model.MoveID, StorageID = model.StorageID, BillID = model.BillID });
            }
            else
            {
                this.AddToastMessage("RELOCBS", message, ToastType.Success);
                return RedirectToAction("Create", new { MoveID = model.MoveID, StorageID = model.StorageID, BillID = model.BillID });
            }
        }

        // GET: StorageBilling/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: StorageBilling/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, FormCollection collection)
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
        
        // POST: StorageBilling/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Delete(int id)
        {
            AjaxResponse result = new AjaxResponse();
            result.Success = false;
            result.Message = "unable to perform operation";
            try
            {
                string message;
                result.Success = storageBillBL.Delete(id,out message);
				result.Message = message;
                return Json(result);
            }
            catch
            {
                return Json(result);
            }
        }

        public ActionResult GetStorageBill(Int64 MoveID, Int64 StorageID)
        {
            fillCombo();
            var model = storageBillBL.GetStrgSubBillGrid(MoveID, StorageID);

            int totalRecord = model.Count();
            int page = 0;
            if (page < 1) page = 1;
            //int skip = (pageNo * pageSize) - pageSize;
            int skip = totalRecord;
            if (skip==0)
            {
                skip = 5;
            }
            var itemsAsIPagedList = new PagedList.StaticPagedList<Entities.StorageSubBillGrid>(model, page, skip, totalRecord);
            return Request.IsAjaxRequest() ? (ActionResult)PartialView("_AjaxInvoicePartial", itemsAsIPagedList) : View("_AjaxInvoicePartial", itemsAsIPagedList);
            
        }

        [HttpPost]
        public JsonResult GetAddCostHeadTax(int CostHeadID)
        {
            var IsTaxApplicable = storageBillBL.GetAdditionalCostTaxStatus(CostHeadID);

            return Json(IsTaxApplicable);

        }
    }
}
