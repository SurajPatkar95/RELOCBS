using PagedList;
using RELOCBS.App_Code;
using RELOCBS.BL;
using RELOCBS.BL.LoadChart;
using RELOCBS.Common;
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
    public class LoadChartController : BaseController
    {
        private string _PageID = "34";
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

        private LoadChartBL _loadChartBL;
        public LoadChartBL loadChartBL
        {
            get
            {
                if (this._loadChartBL == null)
                    this._loadChartBL = new LoadChartBL();
                return this._loadChartBL;
            }
        }


        // GET: LoadChart
        public ActionResult Index(int page = 1)
        {
            if (!CSubs.CheckPageRights(_PageID, PermissionType.VIEW))
            {
                return new HttpStatusCodeResult(403);
            }

            session.Set<string>("PageSession", "LoadChart");
            string sort = "";
            string sortdir = "";
            string search = "";
            Int64 TLCNo = -1;
            
            int pageSize = settings.GetSettingByKey<int>("pagination_pagesize");
            string OrderBy = "";
            int Order = 0;
            DateTime? Fromdate = null;//System.DateTime.Now.Date.AddDays(-2);
            DateTime? Todate = null;//System.DateTime.Now;
            int Transporter = -1;
            


            string SearchKey = string.Empty;
            if (Request.Form["FromDate"] != null && Request.Form["FromDate"].Trim() != "")
            {
                Fromdate = Convert.ToDateTime(Request.Form["FromDate"]);
            }

            if (Request.Form["ToDate"] != null && Request.Form["ToDate"].Trim() != "")
            {
                Todate = Convert.ToDateTime(Request.Form["ToDate"]);
            }

            if (Request.Form["Transporter"] != null && Request.Form["Transporter"].Trim() != "")
            {
                Transporter = Convert.ToInt32(Request.Form["Transporter"]);
            }

            if (Request.Form["TLCNo"] != null && Request.Form["TLCNo"].Trim() != "")
            {
                TLCNo = Convert.ToInt64(Request.Form["TLCNo"]);
            }
            string Shipper = string.Empty;
            Int64? JobNo = null;
            if (Request.Params["Shipper"] != null && Request.Params["Shipper"].Trim() != "")
            {
                Shipper = Request.Params["Shipper"].Trim().ToString();
            }
            if (Request.Form["JobNo"] != null && Request.Form["JobNo"].Trim() != "")
            {
                JobNo = Convert.ToInt64(Request.Form["JobNo"]);
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
            
            

            int totalRecord = 0;
            if (page < 1) page = 1;
            //int skip = (pageNo * pageSize) - pageSize;
            int skip = pageSize;
            pageSize = page;
            var data = loadChartBL.GetGrid(Fromdate, Todate, sort, sortdir, skip, pageSize, out totalRecord, Convert.ToString(TLCNo),Convert.ToString(Transporter),Shipper, JobNo);
            //var data =new List<Entities.LoadChartsGrid>();//= loadChartBL.GetGrid(Fromdate, Todate, IsJobDate, IsInsuranceDate, JobNo, Insurance_ID, sort, sortdir, skip, pageSize, out totalRecord);
            FillCombo();
            ViewData["TLCId"] = TLCNo;
            ViewData["TransporterId"] = Transporter;

            ViewBag.TotalRows = totalRecord;
            ViewBag.search = search;

            var itemsAsIPagedList = new StaticPagedList<Entities.LoadChartsGrid>(data, pageSize, skip, totalRecord);
            return Request.IsAjaxRequest() ? (ActionResult)PartialView("_AjaxPartial", itemsAsIPagedList) : View(itemsAsIPagedList);
        }

        // GET: LoadChart/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        private void FillCombo(bool IsCreate=false)
        {
			bool RMCBuss = !(UserSession.GetUserSession().BussinessLine == "NON RMC-BUSINESS");
			if (IsCreate)
            {
                ViewData["Currency"] = comboBL.GetCurrencyDropdown();
                ViewData["VehicleTypeList"] = comboBL.GetVehicleTypeDropdown();
                ViewData["BranchList"] = comboBL.GetCompanyBranchDropdown();
                ViewData["ModeList"] = comboBL.GetModeDropdown(ServiceLineID:1); ///new List<SelectListItem>();
                ViewData["ServiceLineList"] = comboBL.GetServiceLineDropdown(RMCBuss:RMCBuss);
                
                ViewData["AccessTypeList"] = CommonService.AccessType;
                ViewData["EmployeeList"] = comboBL.GetEmployeeDropdown();

                ViewData["SupervisorList"] = comboBL.GetEmployeeDropdown(SPTYPE: "CREWMASTER");
            }
            else
            {
                ViewData["TLCIdList"] = comboBL.GetLoadChartNoDropdown();
            }
            ViewData["TransporterList"] = comboBL.GetVendorDropdown();
            ViewData["JobNoList"] = new List<SelectListItem>();
            //ViewData["ControllerList"] = comboBL.GetControllerDropdown();
        }

        // GET: LoadChart/Create
        public ActionResult Create(Int64? LoadChartID)
        {
            FillCombo(true);
            var model = new LoadCharts();

            if (LoadChartID!=null && LoadChartID>0)
            {
                model = loadChartBL.GetDetail(Convert.ToInt64(LoadChartID));
            }

            return View(model);
        }

        // POST: LoadChart/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(LoadCharts model)
        {
            try
            {
                FillCombo(true);
                string message = string.Empty;
                if (ModelState.IsValid)
                {
                    bool res = false;

                    res = loadChartBL.Insert(model, out message);
                    if (!res)
                    {
                        ModelState.AddModelError(string.Empty, "Unable to save data.");
                        this.AddToastMessage("RELOCBS", message, ToastType.Error);
                        //return Json(result);
                        return View("Create", model);
                    }
                    else
                    {
                        this.AddToastMessage("RELOCBS", message, ToastType.Success);
                        return RedirectToAction("Create",new { LoadChartID =model.LoadChartID } );
                        //return Json(result);
                    }
                }
                else
                {
                    return View("Create", model);
                }
            }
            catch
            {
                return View(model);
            }
        }

        // GET: LoadChart/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: LoadChart/Edit/5
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

        // GET: LoadChart/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: LoadChart/Delete/5
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

        public JsonResult GetJobDetail(Int64 MoveID)
        {
            var model = loadChartBL.GetJobDetail(MoveID);
            return Json(model, JsonRequestBehavior.AllowGet);

        }


        public JsonResult GetLoadedList()
        {
            var lst = comboBL.GetLoadedAtDropdown().Select(i => new { i.Value, i.Text }).ToList();
            return Json(lst, JsonRequestBehavior.AllowGet);

        }

        public ActionResult GetLoadChartPrint(Int64 LoadChartID)
        {

            LoadCharts model = loadChartBL.GetDetail(LoadChartID);
            //ViewData["Instructions"] = model.Instructions;
            return View("LoadChart_Print", model);
        }
    }
}
