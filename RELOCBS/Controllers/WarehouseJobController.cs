using PagedList;
using RELOCBS.AjaxHelper;
using RELOCBS.App_Code;
using RELOCBS.BL;
using RELOCBS.BL.WH_Job;
using RELOCBS.CustomAttributes;
using RELOCBS.Entities;
using RELOCBS.Extensions;
using RELOCBS.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace RELOCBS.Controllers
{
    [AuthorizeUser]
    public class WarehouseJobController : BaseController
    {
        private string _PageID = "57";

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

        private WarehouseJobBL  _warehouseJobBL;
        public WarehouseJobBL warehouseJobBL
        {
            get
            {
                if (this._warehouseJobBL == null)
                    this._warehouseJobBL = new WarehouseJobBL();
                return this._warehouseJobBL;

            }
        }


        // GET: WarehouseJob
        public ActionResult Index(int? page = 1)
        {
            if (!CSubs.CheckPageRights(_PageID, PermissionType.VIEW))
            {
                return new HttpStatusCodeResult(403);
            }

            fillCombo(true);

            int pageNo = Convert.ToInt32(page);

            session.Set<string>("PageSession", "Open WH Job");
            string sort = "JobID";
            string sortdir = "asc";
            string search = "";
            bool IsJobDate=true, IsActivityDate=false;

            int pageSize = settings.GetSettingByKey<int>("pagination_pagesize");
            string OrderBy = "";
            int Order = 0;
            DateTime? Fromdate = null; //= System.DateTime.Now.Date.AddDays(-7);
            DateTime? Todate = null;//= System.DateTime.Now;
            Int64? JobNo=null;
            int AllocationStatus = -1;
            string Shipper = string.Empty;
            int WarehouseID = -1;

            if (TempData["WarehouseID"] != null)
            {
                WarehouseID = Convert.ToInt32(TempData["WarehouseID"]);
                TempData.Keep();
                ViewData["WarehouseID"] = WarehouseID;
                ViewData["Warehouse"] = ((IEnumerable<SelectListItem>)(ViewData["WarehouseList"])).Where(p => p.Value == WarehouseID.ToString()).First().Text;
            }

            string SearchKey = string.Empty;
            Regex rgx = new Regex("[^a-zA-Z0-9 -]");
            Regex rgxNo = new Regex("[^0-9]");

            if (Request.Form["FromDate"] != null && Request.Form["FromDate"].Trim() != "")
            {
                Fromdate = Convert.ToDateTime(Request.Form["FromDate"]);
            }

            if (Request.Form["ToDate"] != null && Request.Form["ToDate"].Trim() != "")
            {
                Todate = Convert.ToDateTime(Request.Form["ToDate"]);
            }

            if (Request.Form["JobNo"] != null && Request.Form["JobNo"].Trim() != "")
            {
                JobNo = Convert.ToInt64(Request.Form["JobNo"]);

                if (string.IsNullOrWhiteSpace(Request.Form["FromDate"]))
                {
                    Fromdate = null;
                }
            }

            if (Request.Form["Shipper"] != null && Request.Form["Shipper"].Trim() != "")
            {
                Shipper = rgx.Replace(Request.Form["Shipper"], "");
            }

            if (Request.Form["AllocationStatus"] != null && Request.Form["AllocationStatus"].Trim() != "")
            {
                AllocationStatus = Convert.ToInt32(Request.Form["AllocationStatus"]);
            }

            if (Request.Form["WarehouseID"] != null && Request.Form["WarehouseID"].Trim() != "")
            {
                WarehouseID = Convert.ToInt32(Request.Form["WarehouseID"]);
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
            if (pageNo < 1) pageNo = 1;
            //int skip = (pageNo * pageSize) - pageSize;
            int skip = pageSize;
            pageSize = pageNo;
            var data = warehouseJobBL.GetList(Fromdate, Todate,sort, sortdir, skip, pageSize, out totalRecord,IsJobDate,IsActivityDate, JobNo, Shipper, AllocationStatus);

            ViewBag.TotalRows = totalRecord;
            ViewBag.search = search;
            var itemsAsIPagedList = new StaticPagedList<WarehouseJob>(data, pageSize, skip, totalRecord);

            return Request.IsAjaxRequest()
                ? (ActionResult)PartialView("_AjaxPartial", itemsAsIPagedList)
                : View(itemsAsIPagedList);
        }

        // GET: WarehouseJob/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        private void fillCombo(bool IsSearch = false,int BranchID=-1,int JobTypeId=-1)
        {
            
            if (IsSearch)
            {
                ViewData["JobNoList"] = comboBL.GetWarehouseJobNolDropdown();
                ViewData["AllocationStatusList"] = comboBL.GetJobAllocationStatusDropdown();
            }
            else
            {
                ViewData["ActivityTypeList"] = comboBL.GetActivityTypeDropdown(JobTypeId: JobTypeId);
                ViewData["WeightUnitList"] = comboBL.GetMeasurementUnitDropdown(UnitType: 'B');
                ViewData["BranchList"] = comboBL.GetCompanyBranchDropdown();
                ViewData["WarehoueList"] = comboBL.GetWarehouseDropdown(BranchID: BranchID);
                //ViewData["GoodsDescriptionList"] = comboBL.GetGoodsDescriptionDropdown();
                ViewData["BussLineList"] = comboBL.GetBusinessLineDropdown();
                ViewData["WHJobTypeList"] = comboBL.GetWH_JobTypeDropdown();
            }
        }


        // GET: WarehouseJob/Create
        public ActionResult Create(Int64? JobID)
        {
            session.Set<string>("PageSession", "Open WH Job");

            fillCombo();
            WarehouseJob model = new WarehouseJob();
            model.JobOpenDate = System.DateTime.Now;
            model.IsRMCBuss = UserSession.GetUserSession().BussinessLine != "NON RMC-BUSINESS";
            model.CompanyID = UserSession.GetUserSession().CompanyID;
            if (JobID!=null)
            {
                model = warehouseJobBL.GetDetails(Convert.ToInt64(JobID));
            }
            return View(model);
        }

        // POST: WarehouseJob/Create
        [HttpPost]
        public ActionResult Create(WarehouseJob model, string submit)
        {
            try
            {
                session.Set<string>("PageSession", "Open WH Job");
                fillCombo();
                string message = string.Empty;
                if (ModelState.IsValid)
                {
                    bool res = false;

                    res = warehouseJobBL.Insert(model, submit, out message);
                    if (!res)
                    {
                        ModelState.AddModelError(string.Empty, "Unable to save data.");
                        this.AddToastMessage("RELOCBS", message, ToastType.Error);
                        return View("Create", model);
                    }
                    else
                    {
                        this.AddToastMessage("RELOCBS", message, ToastType.Success);
                        return RedirectToAction("Create", new { JobID = model.JobID });
                        //return RedirectToAction("Create", new { Key = CommonService.GenerateQueryString("MoveID=ParamValue0&ComponentID=ParamValue1&PJRDJRID=ParamValue2", new string[] { Convert.ToString(model.MoveID), Convert.ToString(model.RateComponentID), (string.IsNullOrWhiteSpace(Convert.ToString(model.PJR_DJR_ID)) ? "-1" : Convert.ToString(model.PJR_DJR_ID)) }) });
                    }
                }
                else
                {
                    return View("Create", model);
                }
            }
            catch
            {
                return View();
            }
        }


        // GET: WarehouseJob/Create
        public ActionResult CreateInstruction(Int64 JobID, Int64? InstID,int JobTypeId)
        {
            WHJob_InstructionSheet model = new WHJob_InstructionSheet();
            model.JobID = JobID;
            model.JobTypeId = JobTypeId;
            model.IsSentToWarehouse = 0;
            Int64 Instid = -1;
            
            
            if (InstID!=null)
            {
                Instid = Convert.ToInt64(InstID);
            }
            model = warehouseJobBL.GetWHInstructionSheetDetail(JobID, Instid);
            fillCombo(false, BranchID:model.BranchID,JobTypeId:JobTypeId);

            return Request.IsAjaxRequest()
                ? (ActionResult)PartialView("CreateInstruction", model)
                : View(model);
        }

        // POST: WarehouseJob/Create
        [HttpPost]
        public ActionResult CreateInstruction(WHJob_InstructionSheet model, Int64 JobID, Int64? InstID,int JobTypeId, string submit)
        {
            try
            {
                fillCombo(false, BranchID: model.BranchID,JobTypeId: JobTypeId);
                RELOCBS.AjaxHelper.AjaxResponse result = new AjaxResponse();
                if (ModelState.IsValid)
                {
                    string Message;
                    result.Success = warehouseJobBL.InsertInstructionSheet(model, model.submit, out Message);
                    if (result.Success)
                    {
                        result.Message = Message;
                        result.Result = this.RenderPartialViewToString("CreateInstruction", model);
                        return Json(result);
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, Message);
                    }
                }
                return Request.IsAjaxRequest()
                              ? (ActionResult)PartialView("CreateInstruction", model)
                              : View(model);

            }
            catch
            {
                return View();
            }
        }

        public ActionResult GetInstructions(int? page = 1)
        {
            if (!CSubs.CheckPageRights(_PageID, PermissionType.VIEW))
            {
                return new HttpStatusCodeResult(403);
            }
            fillCombo();
            int pageNo = Convert.ToInt32(page);
            string search = "";
            int pageSize = settings.GetSettingByKey<int>("pagination_pagesize");
            
            int Order = 0;
            Int64 JobNo = 0;
            string SearchKey = string.Empty;
            Regex rgx = new Regex("[^a-zA-Z0-9 -]");
            Regex rgxNo = new Regex("[^0-9]");
            
            if (Request.Form["JobID"] != null && Request.Form["JobID"].Trim() != "")
            {
                JobNo = Convert.ToInt64(Request.Form["JobID"]);
            }
            int totalRecord = 0;
            if (pageNo < 1) pageNo = 1;
            //int skip = (pageNo * pageSize) - pageSize;
            var data = warehouseJobBL.GetDetails(JobNo).WHJob_Instructions;
            int skip = data!=null && data.Count>0 ? data.Count : pageSize;
            totalRecord = data != null ? data.Count : 0;
            pageSize = pageNo;
            ViewBag.TotalRows = totalRecord;
            ViewBag.search = search;
            var itemsAsIPagedList = new StaticPagedList<WHJob_InstructionSheet>(data, pageSize, skip, totalRecord);

            return Request.IsAjaxRequest()
                ? (ActionResult)PartialView("_InstructionsPartial", itemsAsIPagedList)
                : View(itemsAsIPagedList);
        }

        public ActionResult GetInstructionSheetPrint(Int64 MoveID, Int64 InstID)
        {
            WHJob_InstructionSheet model = warehouseJobBL.GetWHInstructionPrintDetail(MoveID, InstID);
            //ViewData["Instructions"] = model.Instructions;
            return View("InstructionSheet_Print", model);
        }
        // GET: WarehouseJob/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: WarehouseJob/Edit/5
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

        // GET: WarehouseJob/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: WarehouseJob/Delete/5
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
    }
}
