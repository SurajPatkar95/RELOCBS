using RELOCBS.App_Code;
using RELOCBS.BL;
using RELOCBS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RELOCBS.BL.JobInstruction;
using RELOCBS.Extensions;
using RELOCBS.Utility;
using PagedList;
using RELOCBS.CustomAttributes;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace RELOCBS.Controllers
{
    [AuthorizeUser]
    public class InstructionSheetController : BaseController
    {

        private string _PageID = "19";

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

        private JobInstructionBL _jobInstructionBL;
        public JobInstructionBL jobInstructionBL
        {
            get
            {
                if (this._jobInstructionBL == null)
                    this._jobInstructionBL = new JobInstructionBL();
                return this._jobInstructionBL;

            }
        }


        // GET: InstructionSheet
        public ActionResult Index(int? page = 1)
        {
            if (!CSubs.CheckPageRights(_PageID, PermissionType.VIEW))
            {
                return new HttpStatusCodeResult(403);
            }

            

            int pageNo = Convert.ToInt32(page);

            session.Set<string>("PageSession", "Instruction Sheet");
            string sort = "MoveID";
            string sortdir = "asc";
            string search = "";

            int pageSize = settings.GetSettingByKey<int>("pagination_pagesize");
            string OrderBy = "";
            int Order = 0;
            DateTime? Fromdate=null;
            DateTime? Todate=null;
            string JobNo = string.Empty;
            Int64 MoveId = -1;
            Int16 IsFromMove = 0;
            
            string SearchKey = string.Empty;
            Regex rgx = new Regex("[^a-zA-Z0-9 -]");
            Regex rgxNo = new Regex("[^0-9]");

            string Shipper = string.Empty;

            if (Request.Form["FromDate"] != null && Request.Form["FromDate"].Trim() != "")
            {
                Fromdate = Convert.ToDateTime(Request.Form["FromDate"]);
            }

            if (Request.Form["ToDate"] != null && Request.Form["ToDate"].Trim() != "")
            {
                Todate = Convert.ToDateTime(Request.Form["ToDate"]);
            }

            if (Request.Form["FromMove"] != null && Request.Form["FromMove"].Trim() != "")
            {
                IsFromMove = Convert.ToInt16(Request.Form["FromMove"]);
            }


            if (Request.Form["JobNo"] != null && Request.Form["JobNo"].Trim() != "")
            {
                MoveId = Convert.ToInt64(rgxNo.Replace(Request.Form["JobNo"], ""));
            }

            if (Request.Form["MoveID"] != null && Request.Form["MoveID"].Trim() != "")
            {
                MoveId = Convert.ToInt64(rgxNo.Replace(Request.Form["MoveID"], ""));
            }

            if (Request.Form["Shipper"] != null && Request.Form["Shipper"].Trim() != "")
            {
                Shipper = Convert.ToString(Request.Form["Shipper"]);
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
            ViewData["MoveId"] = MoveId;
            fillCombo(MoveId: MoveId);
            int totalRecord = 0;
            if (pageNo < 1) pageNo = 1;
            //int skip = (pageNo * pageSize) - pageSize;
            int skip = pageSize;
            pageSize = pageNo;
            var data = jobInstructionBL.GetForGrid(JobNo, MoveId, Fromdate, Todate, Shipper, sort, sortdir, skip, pageSize, out totalRecord);

            ViewBag.TotalRows = totalRecord;
            ViewBag.search = search;
            var itemsAsIPagedList = new StaticPagedList<JobInstGrid>(data, pageSize, skip, totalRecord);

            if (IsFromMove==1 && itemsAsIPagedList.Count<=0)
            {
                return RedirectToAction("Create",new { MoveID = MoveId });
            }

            return Request.IsAjaxRequest()
                ? (ActionResult)PartialView("_AjaxPartial", itemsAsIPagedList)
                : View(itemsAsIPagedList);
        }

        // GET: InstructionSheet/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        private void fillCombo(Int64 MoveId=-1,int ServiceLine=-1)
        {
            ViewData["AllocationStatusList"] = comboBL.GetJobAllocationStatusDropdown();
            ViewData["CrewList"] = comboBL.getCrewDropdown(Allitems: false);
            ViewData["EmployeeList"] = comboBL.GetEmployeeDropdown();
            ViewData["SuperviserList"] = comboBL.GetEmployeeDropdown(SPTYPE: "SUPERVISOR");
            ViewData["VendorList"] = comboBL.GetVendorDropdown();
            ViewData["PurposeList"] = comboBL.getPurposeDropdown();
            ViewData["VehicleList"] = comboBL.GetVehicleDropdown();
            ViewData["DriverList"] = comboBL.GetEmployeeDropdown(SPTYPE: "DRIVER");
            ViewData["DocTypeList"] = comboBL.GetDocTypeDropdown();
            ViewData["ActivityTypeList"] = comboBL.GetActivityTypeDropdown();
            ViewData["ADServiceTypeList"] = comboBL.GetCostHeadDropdown();
            ViewData["MaterialTypeList"] = comboBL.getMaterialDropdown();
            ViewData["JobNoList"] = comboBL.getJobNolDropdown(MoveId: MoveId);
            ViewData["CityList"] = comboBL.GetCityDropdown();
            ViewData["CaseTypeList"] = comboBL.GetCaseTypeDropdown();
            ViewData["UnitList"] = comboBL.GetMeasurementUnitDropdown('I');
            ViewData["SubQuestionList"] = comboBL.GetInst_SubQuestionDropdown();
            ViewData["CorporateList"] = comboBL.GetAgentDropdown(CORA:"C");
            ViewData["ClientList"] = comboBL.GetAgentDropdown(CORA: "A");
            ViewData["ModeList"] = comboBL.GetModeDropdown(ServiceLineID : ServiceLine);
            ViewData["WeightUnitList"] = comboBL.GetMeasurementUnitDropdown(UnitType: 'B');
            ViewData["BranchList"] = comboBL.GetCompanyBranchDropdown();
            ViewData["WarehoueList"] = comboBL.GetWarehouseDropdown();
            ViewData["GoodsDescriptionList"] = comboBL.GetGoodsDescriptionDropdown();
        }

        // GET: InstructionSheet/Create
        public ActionResult Create(Int64 MoveID, Int64? InstID)
        {

            if (!CSubs.CheckPageRights(_PageID, PermissionType.ADD) || !CSubs.CheckPageRights(_PageID, PermissionType.EDIT))
            {
                return new HttpStatusCodeResult(403);
            }

            if (MoveID>0)
            {
                //fillCombo();

                Int64 ID = -1;
                if (InstID != null)
                {
                    ID = Convert.ToInt64(InstID);
                }

                InstructionSheet model = jobInstructionBL.GetDetailById(MoveID, ID);
                fillCombo(ServiceLine :model.ServiceLineID);
                //ViewData["Instructions"] = model.Instructions;
                return View(model);
            }

            return new HttpStatusCodeResult(403);
        }

        // POST: InstructionSheet/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(InstructionSheet model, Int64 MoveID, Int64? InstID,string submit)
        {
            try
            {
                fillCombo(ServiceLine: model.ServiceLineID);
                string message = string.Empty;
                if (ModelState.IsValid)
                {
                    if(!string.IsNullOrWhiteSpace(model.HVactivityList))
                    {
                        ActitiyRootObject activities = (ActitiyRootObject)JsonConvert.DeserializeObject(Convert.ToString(model.HVactivityList), (typeof(ActitiyRootObject)));  //JsonConvert.DeserializeObject<List<Inst_Activities>>(model.HVactivityList);
                        model.activities = activities.Inst_Activities;
                    }
                    if (model.activities.Count > 0)
                    {
                        bool res = false;
                        
                        if (!string.IsNullOrEmpty(model.HVDimensions))
                        {
                            DimensionRootObject CaseDimensions = JsonConvert.DeserializeObject<DimensionRootObject>(model.HVDimensions);
                            model.Dimensions = CaseDimensions.Dimensions;
                        }

                        res = jobInstructionBL.Insert(model, submit, out message);
                        if (!res)
                        {
                            ModelState.AddModelError(string.Empty, "Unable to save data.");
                            this.AddToastMessage("RELOCBS", message, ToastType.Error);
                            //return Json(result);
                        }
                        else
                        {
                            this.AddToastMessage("RELOCBS", message, ToastType.Success);

                            //return Json(result);
                        }

                        return RedirectToAction("Create", new { MoveID = model.MoveID, InstID = model.InstID });

                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Atleast One activity required");
                        this.AddToastMessage("RELOCBS", "Atleast One activity required", ToastType.Error);
                        return View("Create", model);
                    }


                }
                else
                {
                    return View("Create", model);
                }
            }
            catch(Exception e)
            {
                return View("Create", model);
            }
        }

        // GET: InstructionSheet/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: InstructionSheet/Edit/5
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

        // GET: InstructionSheet/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: InstructionSheet/Delete/5
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

        public ActionResult GetInstructionSheetPrint(Int64 MoveID, Int64 InstID)
        {
            InstructionSheet model = jobInstructionBL.GetPrintDetail(MoveID, InstID);
            //ViewData["Instructions"] = model.Instructions;
            return View("InstructionSheet_Print",model);
        }
    }
}
