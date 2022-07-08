using Newtonsoft.Json;
using PagedList;
using RELOCBS.App_Code;
using RELOCBS.BL;
using RELOCBS.BL.Storage;
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
    public class StorageController : BaseController
    {
        private string _PageID = "29";
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

        private StorageBL _storageBL;
        public StorageBL storageBL
        {
            get
            {
                if (this._storageBL == null)
                    this._storageBL = new StorageBL();
                return this._storageBL;
            }
        }

        // GET: Storage
        public ActionResult Index(int page = 1)
        {
            if (!CSubs.CheckPageRights(_PageID, PermissionType.VIEW))
            {
                return new HttpStatusCodeResult(403);
            }

            session.Set<string>("PageSession", "Storage Rates");
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
            bool IsStrgDate = false;
            bool IsJobDate = false;


            string SearchKey = string.Empty;
            if (Request.Form["FromDate"] != null && Request.Form["FromDate"].Trim() != "")
            {
                Fromdate = Convert.ToDateTime(Request.Form["FromDate"]);
            }
            if (Request.Form["JobNo"] != null && Request.Form["JobNo"].Trim() != "")
            {
                JobNo = Convert.ToInt64(Request.Form["JobNo"]);
            }

            if (Request.Form["Shipper"] != null && Request.Form["Shipper"].Trim() != "")
            {
                Shipper = Convert.ToString(Request.Form["Shipper"]);
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
            if (Request.Params["IsJobDate"] != null && Request.Params["IsJobDate"].Trim() != "")
            {

                if (Request.Form.GetValues("IsJobDate") != null && Convert.ToString(Request.Form.GetValues("IsJobDate")[0]).Trim().ToUpper() == "TRUE")
                {
                    IsJobDate = true;
                }

            }
            if (Request.Params["IsStrgDate"] != null && Request.Params["IsStrgDate"].Trim() != "")
            {
                if (Request.Form.GetValues("IsStrgDate") != null && Convert.ToString(Request.Form.GetValues("IsStrgDate")[0]).Trim().ToUpper() == "TRUE")
                {
                    IsStrgDate = true;
                }

            }

            int totalRecord = 0;
            if (page < 1) page = 1;
            //int skip = (pageNo * pageSize) - pageSize;
            int skip = pageSize;
            pageSize = page;
            var data = storageBL.GetStorageGrid(Fromdate, Todate, IsJobDate, IsStrgDate, JobNo, Shipper, sort, sortdir, skip, pageSize, out totalRecord);
            fillCombo();
            ViewData["MoveId"] = JobNo;
            //ViewData["StrgId"] = Strg_ID;

            ViewBag.TotalRows = totalRecord;
            ViewBag.search = search;

            var itemsAsIPagedList = new StaticPagedList<Entities.JobStorageGrid>(data, pageSize, skip, totalRecord);
            return Request.IsAjaxRequest() ? (ActionResult)PartialView("_AjaxPartial", itemsAsIPagedList) : View(itemsAsIPagedList);
        }

        // GET: Storage/Details/5
        public ActionResult RedirectCreate(Int64 MoveID)
        {
            Int64 StrgID = storageBL.GetStorageIDForJob(MoveID);
            return StrgID > 0 ? RedirectToAction("Create", new { MoveID = MoveID, StorageID = StrgID, Index = 0 }) :
                RedirectToAction("Create", new { MoveID = MoveID, Index = 0 });    
        }

        private void fillCombo()
        {

            ViewData["UnitList"] = comboBL.GetMeasurementUnitDropdown(UnitType:'B');
            ViewData["EmpList"] = comboBL.GetEmployeeDropdown();
            ViewData["CommodityList"] = comboBL.GetGoodsDescriptionDropdown();
            ViewData["WarehouseList"] = comboBL.GetWarehouseStrgDropdown();
            ViewData["CurrencyList"] = comboBL.GetCurrencyDropdown();
            ViewData["StatesList"] = comboBL.GetStorageStateDropdown(0);
            ViewData["InsuranceCompList"] = comboBL.GetInsuranceTypeDropdown();
            ViewData["JobNoList"] = comboBL.getJobNolDropdown(IsStorage: true); ///new List<SelectListItem>();
            ViewData["CostHeadList"] = comboBL.GetStrgCostHeadDropdown(StrgCostType:"RATE"); ///new List<SelectListItem>();
            ViewData["PeriodList"] = comboBL.GetStrgBillingPeriodDropdown(); ///new List<SelectListItem>();
			ViewData["SplPeriodList"] = comboBL.GetStrgBillingPeriodDropdown(ComboType: "Spl");
		}


        // POST: Storage/CreateApprove
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult CreateApprove(JobStorageApproveDTO model)
        {
            string message = "Unable to perform operation";
            if (ModelState.IsValid)
            {
                bool res = storageBL.ApproveStorage(model,UserSession.GetUserSession().LoginID,out message);
                if (!res)
                {
                    ModelState.AddModelError(string.Empty, "Unable to save data.");
                    this.AddToastMessage("RELOCBS", message, ToastType.Error);
                }
                else
                {
                    this.AddToastMessage("RELOCBS", message, ToastType.Success);   
                }

            }
            else
            {
                this.AddToastMessage("RELOCBS", message, ToastType.Error);
                
            }

            return RedirectToAction("Create", new { MoveID = model.MoveID, StorageID = model.StorageID, Index = model.Index });

        }

        // GET: Storage/Create
        public ActionResult Create(Int64 MoveID,Int64? StorageID,int Index=0)
        {
            session.Set<string>("PageSession", "Storage Rates");
            var model = storageBL.GetStorageDetails(MoveID, StorageID);
            model.VolumeUnitID = 1;
            fillCombo();
            model.TabIndex = Index;
            return View(model);
           
        }

        // POST: Storage/Create
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Create(Int64 MoveID, Int64? StorageID, int Index, JobStorage model)
        {
            session.Set<string>("PageSession", "Storage Rates");
            try
            {
                fillCombo();
                string message = string.Empty;

                if (Index==1)
                {
                    ModelState.Remove("StorageEntryDate");
                } 

                if (ModelState.IsValid)
                {
                    bool res = false;

                    if (Index==0)
                    {
                        res = storageBL.Insert(model, out message);
                    }
                    if (Index == 1)
                    {
                        if (!string.IsNullOrWhiteSpace(model.HFrateList))
                        {
                            model.ratesList = JsonConvert.DeserializeObject<List<StorageRateDetails>>(model.HFrateList);
                        }

                        res = storageBL.InsertRate(model, out message);
                    }


                    if (!res)
                    {
                        ModelState.AddModelError(string.Empty, "Unable to save data.");
                        this.AddToastMessage("RELOCBS", message, ToastType.Error);
                        return View("Create", model);
                    }
                    else
                    {
                        this.AddToastMessage("RELOCBS", message, ToastType.Success);
                        return RedirectToAction("Create", new { MoveID = model.jobDetail.MoveID, StorageID = model.StorageID, Index=Index });
                    }
                }
                else
                {
                    return View(model);
                }
            }
            catch(Exception ex)
            {
                this.AddToastMessage("RELOCBS", "Unexpected error occured", ToastType.Success);
                return RedirectToAction("Create", new { MoveID = model.jobDetail.MoveID, StorageID = model.StorageID, Index = Index });
            }
        }

        // GET: Storage/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Storage/Edit/5
        [HttpPost]
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

        // GET: Storage/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Storage/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
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

        public ActionResult GetRates(Int64 StorageID,int WeightID)
        {
            StorageRate obj = storageBL.GetStorageRates(StorageID, WeightID);

            return PartialView("_RatesPartial", obj);
        }

        public PartialViewResult GetRateInsurance(Int64 MoveID,Int64 StorageID,Int64 StorageDetailID,string BaseCurr)
        {
            var model = storageBL.GetRateInsDetails(MoveID, StorageID, StorageDetailID);
            model.BaseCurrName = BaseCurr;
            fillCombo();
            return PartialView("_RatesPartial",model);
        } 

    }
}
