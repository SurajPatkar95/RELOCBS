using Newtonsoft.Json;
using PagedList;
using RELOCBS.AjaxHelper;
using RELOCBS.App_Code;
using RELOCBS.BL;
using RELOCBS.BL.Common;
using RELOCBS.BL.JobAllocation;
using RELOCBS.BL.JobInstruction;
using RELOCBS.BL.MoveMange;
using RELOCBS.BL.WH_Job;
using RELOCBS.Common;
using RELOCBS.CustomAttributes;
using RELOCBS.Entities;
using RELOCBS.Extensions;
using RELOCBS.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace RELOCBS.Controllers
{
    [AuthorizeUser]
    public class JobAllocationController : BaseController
    {
        private string _PageID = "18";

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

        private JobAllocationBL _jobAllocationBL;
        public JobAllocationBL jobAllocationBL
        {
            get
            {
                if (this._jobAllocationBL == null)
                    this._jobAllocationBL = new JobAllocationBL();
                return this._jobAllocationBL;

            }
        }

        private CrewBL _crewBL;
        public CrewBL crewBL
        {
            get
            {
                if (this._crewBL == null)
                    this._crewBL = new CrewBL();
                return this._crewBL;

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

        private MoveManageBL _moveManageBL;
        public MoveManageBL moveManageBL
        {
            get
            {
                if (this._moveManageBL == null)
                    this._moveManageBL = new MoveManageBL();
                return this._moveManageBL;

            }
        }

        private WarehouseJobBL _warehouseJobBL;
        public WarehouseJobBL warehouseJobBL
        {
            get
            {
                if (this._warehouseJobBL == null)
                    this._warehouseJobBL = new WarehouseJobBL();
                return this._warehouseJobBL;

            }
        }
        
        private DAL.ComboDAL _comboDAL;
        public DAL.ComboDAL comboDAL
        {
            get
            {
                if (this._comboDAL == null)
                    this._comboDAL = new DAL.ComboDAL();
                return this._comboDAL;

            }
        }

        // GET: JobAllocation
        public ActionResult Index(int? page = 1)
        {
            if (!CSubs.CheckPageRights(_PageID, PermissionType.VIEW))
            {
                return new HttpStatusCodeResult(403);
            }

            fillCombo(WarehouseType : "ALLACTIVEUSERWISE");

            int pageNo = Convert.ToInt32(page);

            session.Set<string>("PageSession", "Job Diary");
            string sort = "MoveID";
            string sortdir = "asc";
            string search = "";

            int pageSize = settings.GetSettingByKey<int>("pagination_pagesize");
            string OrderBy = "";
            int Order = 0;
            DateTime? Fromdate=null; //= System.DateTime.Now.Date.AddDays(-7);
            DateTime? Todate=null;//= System.DateTime.Now;
            Int64[] JobNo = new Int64[0];
            int AllocationStatus = -1;
            string Shipper = string.Empty;
            int WarehouseID = -1;
            int SelectedJobType = 1; 

            if (TempData["WarehouseID"]!=null)
            {
                WarehouseID = Convert.ToInt32(TempData["WarehouseID"]);
                TempData.Keep();
                ViewData["WarehouseID"] = WarehouseID;
                ViewData["Warehouse"] =((IEnumerable<SelectListItem>)(ViewData["WarehouseList"])).Where(p=>p.Value == WarehouseID.ToString()).First().Text;
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
                JobNo = Request.Form["JobNo"].Split(',').Select(Int64.Parse).ToArray();

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

            if (Request.Form["JobType"]!=null && Request.Form["JobType"].Trim()!= "")
            {
                SelectedJobType = Convert.ToInt16(Request.Form["JobType"]);
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

            ViewData["SelectedJobType"] = SelectedJobType;

            int totalRecord = 0;
            if (pageNo < 1) pageNo = 1;
            //int skip = (pageNo * pageSize) - pageSize;
            int skip = pageSize;
            pageSize = pageNo;
            var data = jobAllocationBL.GetForGrid(JobNo, Shipper, AllocationStatus, Fromdate, Todate, WarehouseID, SelectedJobType, sort, sortdir, skip, pageSize, out totalRecord);

            ViewBag.TotalRows = totalRecord;
            ViewBag.search = search;
            var itemsAsIPagedList = new StaticPagedList<JobInstGrid>(data, pageSize, skip, totalRecord);

            return Request.IsAjaxRequest()
                ? (ActionResult)PartialView("_AjaxPartial", itemsAsIPagedList)
                : View(itemsAsIPagedList);
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Index(string JobNo,string JobStatus,DateTime? from,DateTime? to)
        //{
        //    //jQuery DataTables Param
        //    var draw = Request.Form.GetValues("draw").FirstOrDefault();
        //    //Find paging info
        //    var start = Request.Form.GetValues("start").FirstOrDefault();
        //    var length = Request.Form.GetValues("length").FirstOrDefault();
        //    //Find order columns info
        //    var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault()
        //                            + "][name]").FirstOrDefault();
        //    var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
        //    int pageSize = length != null ? Convert.ToInt32(length) : 0;
        //    int skip = start != null ? Convert.ToInt16(start) : 0;
        //    int recordsTotal = 0;

        //    //using (MyDatabaseEntities dc = new MyDatabaseEntities())
        //    //{
        //    //    var v = (from a in dc.Customers select a);

        //    //    //SEARCHING...
        //    //    if (!string.IsNullOrEmpty(JobNo))
        //    //    {
        //    //        v = v.Where(a => a.ContactName.Contains(JobNo));
        //    //    }
        //    //    if (!string.IsNullOrEmpty(JobStatus))
        //    //    {
        //    //        v = v.Where(a => a.Country == JobStatus);
        //    //    }
        //    //    //SORTING...  (For sorting we need to add a reference System.Linq.Dynamic)
        //    //    if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
        //    //    {
        //    //        v = v.OrderBy(sortColumn + " " + sortColumnDir);
        //    //    }

        //    //    recordsTotal = v.Count();
        //    //    var data = v.Skip(skip).Take(pageSize).ToList();
        //    //    return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data },
        //    //        JsonRequestBehavior.AllowGet);

        //    //}

        //    var v = new [] {
        //        new {
        //            WisDate ="2019-10-20",
        //            JobNo ="5040040",
        //            createdon="2019-10-20",
        //            CorprateidNM="test",
        //            Branch="ABC",
        //            Address="test",
        //            ActivityType="test",
        //            ServiceLineName="test",
        //            ODCity="Mumbai-Delhi",
        //            VolCFT="test",
        //            WisMode="Air",
        //            status="Pending"
        //        },
        //        new {
        //            WisDate ="2019-10-20",
        //            JobNo ="5040040120",
        //            createdon="2019-10-20",
        //            CorprateidNM="test",
        //            Branch="ABC",
        //            Address="test",
        //            ActivityType="test",
        //            ServiceLineName="test",
        //            ODCity="Jaipur-Boston",
        //            VolCFT="test",
        //            WisMode="Sea",
        //            status="Completed"
        //        }
        //    };
        //    recordsTotal = v.Count();
        //    var data = v.Skip(skip).Take(pageSize).ToList();
        //    return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data },
        //        JsonRequestBehavior.AllowGet);

        //}

        private void fillCombo(string WarehouseType= "ALLACTIVE",int RateComponentID=-1,int BranchID=-1,int LoginID=0,int CompID=0)
        {
            

            

            ViewData["AllocationStatusList"] = LoginID == 0 ? comboBL.GetJobAllocationStatusDropdown() : comboDAL.GetJobAllocationStatusDropdown(LoginID);
            ViewData["CrewList"] = LoginID == 0 ? comboBL.getCrewDropdown(Allitems: false) : comboDAL.getCrewDropdown(LoginID, CompID);
            ViewData["EmployeeList"] = LoginID == 0 ? comboBL.GetEmployeeDropdown(SPTYPE:"CREWALLOCATION",BranchID: BranchID) : comboDAL.GetEmployeeDropdown(Convert.ToString(LoginID), SPTYPE: "CREWALLOCATION", BranchID: BranchID);
            ViewData["SuperviserList"] = LoginID == 0 ? comboBL.GetEmployeeDropdown(SPTYPE: "SUPERVISOR",BranchID: BranchID) : comboDAL.GetEmployeeDropdown(Convert.ToString(LoginID), SPTYPE: "SUPERVISOR", BranchID: BranchID);
            ViewData["VendorList"] = LoginID == 0 ? comboBL.GetVendorDropdown() : comboDAL.GetVendorDropdown(LoginID,CompID,"ALLACTIVE",-1);
            ViewData["PurposeList"] = LoginID == 0 ? comboBL.getPurposeDropdown() : comboDAL.getPurposeDropdown(LoginID);
            ViewData["VehicleList"] = LoginID == 0 ? comboBL.GetVehicleDropdown(BranchID:BranchID) : comboDAL.GetVehicleDropdown(LoginID,BranchID: BranchID,CompanyID:CompID);
            ViewData["DriverList"] = LoginID == 0 ? comboBL.GetEmployeeDropdown(SPTYPE: "DRIVER", BranchID: BranchID) : comboDAL.GetEmployeeDropdown(Convert.ToString(LoginID), SPTYPE: "DRIVER", BranchID: BranchID);
            
            ViewData["ActivityTypeList"] = LoginID == 0 ? comboBL.GetActivityTypeDropdown() : comboDAL.GetActivityTypeDropdown(LoginID);
            ViewData["ADServiceTypeList"] = LoginID == 0 ? comboBL.GetCostHeadDropdown(MoveCompID:RateComponentID) : comboDAL.GetCostHeadDropdown(Convert.ToString(LoginID),-1,MoveCompID: RateComponentID,IsRMCBUss:false);
            ViewData["MaterialTypeList"] = LoginID == 0 ? comboBL.getMaterialDropdown() : comboDAL.getMaterialDropdown(LoginID);
            ViewData["JobNoList"] = LoginID == 0 ? comboBL.getJobNolDropdown() : comboDAL.getJobNolDropdown(-1,"",false,LoginID:LoginID,CompanyID: 0,SPTYPE:"ALLACTIVE");
            ViewData["OwnershipTypeList"] = CommonService.VehicleType;
            ViewData["WarehouseList"] = LoginID == 0 ? comboBL.GetWarehouseDropdown(SPTYPE: WarehouseType) : comboDAL.GetWarehouseDropdown(LoginID,CompID ,SPTYPE: WarehouseType);

            ViewData["DocTypeList"] = LoginID == 0 ? comboBL.GetJobDocTypelDropdown(DocFromType: "JobDiary") : comboDAL.GetJobDocTypelDropdown(LoginID, DocFromType: "JobDiary");
            ViewData["DocNameList"] = LoginID == 0 ? comboBL.GetJobDocNamelDropdown(DocTypeID:10) : comboDAL.GetJobDocNamelDropdown(LoginID,DocTypeID: 10);
            
            ViewData["VehicleMovementList"] = LoginID == 0 ? comboBL.GetVehicleMovementMasterDropdown() : comboDAL.GetVehicleMovementMasterDropdown(Convert.ToString(LoginID));
            ViewData["VehicleDimensionList"] = LoginID == 0 ? comboBL.GetVehicleDimensionDropdown() : comboDAL.GetVehicleDimensionDropdown(Convert.ToString(LoginID));
            ViewData["VehicleReasonList"] = LoginID == 0 ? comboBL.GetVehicleReasonDropdown() : comboDAL.GetVehicleReasonDropdown(Convert.ToString(LoginID));

        }


        public ActionResult Create(Int64 MoveId, Int64 InstID)
        {
            fillCombo();

            if (MoveId > 0 && InstID > 0)
            {
                JobAllocationModel jobAllocation = jobAllocationBL.GetDetailById(MoveId, InstID);
                return View(jobAllocation);
            }
            else
            {
                return new HttpStatusCodeResult(403);
            }

        }

        //[HttpPost]
        public ActionResult JobDiary(Int64[] InstID,Int64? BatchID,Int32 YT=0,Int16 JobType=1)
        {

            fillCombo();

            if (InstID!=null && InstID.Length > 0)
            {
                TempData["InstIDs"] = InstID;
                TempData["BID"] = BatchID;

                if (YT==1)
                {
                    return RedirectToAction("BulkCreate", "JobAllocation", new { BatchID = BatchID, JobType = JobType });
                }
                else
                {
                    return Json(new { url = Url.Action("BulkCreate", "JobAllocation" ,new { BatchID = BatchID , JobType = JobType}) }, JsonRequestBehavior.AllowGet);
                }
                //return RedirectToAction("BulkCreate","JobAllocation" ,new { InstIDs = InstID, BID = BatchID });
            }
            else
            {
                return new HttpStatusCodeResult(403);
            }
        }

        [HttpGet]
        public ActionResult BulkCreate(Int64[] InstIDs, Int64? BID,Int16 JobType)
        {
            if (TempData["InstIDs"] != null && ((Int64[])TempData["InstIDs"]).Length > 0)
            {
                InstIDs = ((Int64[])TempData["InstIDs"]);

                if (TempData.ContainsKey("BID") && TempData["BID"]!=null)
                      BID = string.IsNullOrEmpty(Convert.ToString(TempData["BID"])) ? ((Int64?)null) : Convert.ToInt64(Convert.ToString(TempData["BID"]));

                TempData.Keep();

                var data = jobAllocationBL.GetBulkInstDetailById(InstIDs, BID, JobType);
                return View(data);

            }
            else
            {
                return RedirectToAction("Index","Home");
            }

        }

        [HttpPost]
        public ActionResult BulkCreate(JobDiaryModel diaryModel)
        {
            
                //var data = jobAllocationBL.GetBulkInstDetailById(InstIDs, BID);
                return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(JobAllocationModel jobAllocation, string submit)
        {

            fillCombo();
            string message = string.Empty;
            if (ModelState.IsValid)
            {
                bool res = false;
                 List<JobActivity> activities= JsonConvert.DeserializeObject<List<JobActivity>>(jobAllocation.activityList);
                jobAllocation.activities = activities;

                if (jobAllocation.activities.Count > 0)
                {
                    res = jobAllocationBL.Insert(jobAllocation, out message);
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

                    return RedirectToAction("Create", new { MoveID = jobAllocation.MoveID, InstID = jobAllocation.InstID });
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Atleast one activity is required.");
                    this.AddToastMessage("RELOCBS", message, ToastType.Error);
                }

                return View("Create", jobAllocation);

            }
            else
            {
                return View("Create", jobAllocation);
            }
        }

        // GET: JobAllocation
        public ActionResult Dashboard()
        {
            return View();
        }

        public ActionResult CrewAllocation()
        {
            return View();
        }

        public PartialViewResult GetCrewMembers(DateTime From, DateTime To,int Crewid, Int64 BatchID = -1)
        {
            try
            {
                fillCombo();
                ActivityAllocationModel obj = new ActivityAllocationModel();
                obj.jobCrew = jobAllocationBL.GetCrewMembers(From, To, Crewid, BatchID);
                return PartialView("_CrewAllocationCreate", obj);
            }
            catch (Exception)
            {
                return null;
            }
        }
        
        public JsonResult GetVacantCrew(DateTime From,DateTime To,Int64 BatchID = -1)
        {
            var stateList = jobAllocationBL.GetVacantCrew(From,To, BatchID);
            return Json(stateList, JsonRequestBehavior.AllowGet);
        }


        public ActionResult PJR_DJR(int id)
        {
            try
            {
                fillCombo();
                PJR_DJR pdObj = new PJR_DJR();
                if (id > 0)
                {
                    pdObj = jobAllocationBL.GetPJR_DJR_Details(id);
                }
                return PartialView("_JobReportCreate", pdObj);
            }
            catch (Exception)
            {
                return null;
            }

        }

        public double GetMaterialCost(int id)
        {
            double Cost = 0;
            try
            {


                Cost = Convert.ToDouble((new MaterialBL()).GetDetailById(id).Rate);

            }
            catch (Exception)
            {
            }

            return Cost;

        }

        public ActionResult CreateActivity(int InstID, int ActivityID)
        {
            fillCombo();
            JobActivity model = new JobActivity();
            if (/*ActivityID > 0 &&*/ InstID > 0)
            {
                model = jobAllocationBL.GetJobActivityDetailById(InstID, ActivityID);
            }
            return PartialView("_ActivityDetail", model);
        }


        public ActionResult CreateActivityAllocation(Int64[] InstID,Int64[] ActivityID, Int64? BatchID,Int16 JobType)
        {
            fillCombo();

            if (ActivityID != null && ActivityID.Length > 0)
            {
               var model = jobAllocationBL.GetActivityJobAllocationById(InstID, ActivityID, BatchID,JobType);
                /////Job Allocation Doc Type
               model.docUpload.DocTypeID = 10;
               model.docUpload.DocFromType = "JobDiary";
               model.docUpload.ID = Convert.ToInt64(model.BatchID);
                fillCombo(RateComponentID:model.RateComponentID,BranchID:model.BranchID);
                return Request.IsAjaxRequest() ? (ActionResult)PartialView("_ActivityAllocationPartial", model) : View("_ActivityAllocationPartial", model);

            }
            else
            {
                return new HttpStatusCodeResult(403);
            }
            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandleError]
        public ActionResult CreateActivityAllocation(ActivityAllocationModel model, string submit,Int16 JobType)
        {
            fillCombo(RateComponentID:model.RateComponentID, BranchID: model.BranchID);
            int SelectedTabIndex = model.TabIndex;
            int DocTypeID = model.docUpload.DocTypeID, DocumentNameID = model.docUpload.DocNameID;
            Int64 ID = model.docUpload.ID;
            string FromType=model.docUpload.DocFromType;
            JobType = model.JobType;
            if (!string.IsNullOrWhiteSpace(model.HVCrewMembers))
            {
                CrewMemberRootObject crewMembers = (CrewMemberRootObject)JsonConvert.DeserializeObject(Convert.ToString(model.HVCrewMembers), (typeof(CrewMemberRootObject)));  //JsonConvert.DeserializeObject<List<Inst_Activities>>(model.HVactivityList);
                model.jobCrew.members = crewMembers.members;
            }

            //if (model.docUpload.docLists==null && !string.IsNullOrWhiteSpace(model.HVDocList))
            //{
            //    DocListRootObject docList = (DocListRootObject)JsonConvert.DeserializeObject(Convert.ToString(model.HVDocList), (typeof(DocListRootObject)));  //JsonConvert.DeserializeObject<List<Inst_Activities>>(model.HVactivityList);
            //    model.docUpload.docLists = docList.docLists;
            //}
            if (!string.IsNullOrWhiteSpace(model.HVMaterialUsed))
            {
                MaterialUsedRootObject materialUsed = (MaterialUsedRootObject)JsonConvert.DeserializeObject(Convert.ToString(model.HVMaterialUsed), (typeof(MaterialUsedRootObject)));  //JsonConvert.DeserializeObject<List<Inst_Activities>>(model.HVactivityList);
                model.materialUsed = materialUsed.materialUsed;
            }
            if (!string.IsNullOrWhiteSpace(model.HVServices))
            {
                JobServiceRootObject services = (JobServiceRootObject)JsonConvert.DeserializeObject(Convert.ToString(model.HVServices), (typeof(JobServiceRootObject)));  //JsonConvert.DeserializeObject<List<Inst_Activities>>(model.HVactivityList);
                model.services = services.services;
            }

            if (!string.IsNullOrWhiteSpace(model.HVVehicles))
            {
                JobVehicleRootObject jobVehicle = (JobVehicleRootObject)JsonConvert.DeserializeObject(Convert.ToString(model.HVVehicles), (typeof(JobVehicleRootObject)));  //JsonConvert.DeserializeObject<List<Inst_Activities>>(model.HVactivityList);
                model.jobVehicleList = jobVehicle.jobVehicleList;
            }

            if (!model.submit.Equals("UPLOAD", StringComparison.OrdinalIgnoreCase))
            {
                ModelState.Remove("docUpload.ID");
                ModelState.Remove("docUpload.DocFromType");
                ModelState.Remove("docUpload.DocTypeID");
                ModelState.Remove("docUpload.DocNameID");
                ModelState.Remove("docUpload.file");
            }

            if (ModelState.IsValid)
            {
                // Save it in database

                //Return Success message
                RELOCBS.AjaxHelper.AjaxResponse result = new AjaxResponse();
                string message = "";
                ModelState.Clear();
                result.Success = false;
                if (model.submit.Equals("UPLOAD", StringComparison.OrdinalIgnoreCase))
                {
                    MoveManageViewModel obj = new MoveManageViewModel();
                    obj.jobDocUpload = model.docUpload;
                    result.Success = moveManageBL.InsertDocument(obj, UserSession.GetUserSession().LoginID, out message);
                }
                else if (model.submit.Equals("SendForApproval", StringComparison.OrdinalIgnoreCase) || model.submit.Equals("ApprovalSave", StringComparison.OrdinalIgnoreCase))
                {
                    result.Success = jobAllocationBL.UpdateHiredVehicleApprovalStatus(model, out message);
                }
                else if (model.submit.Equals("Send to Supervisor", StringComparison.OrdinalIgnoreCase))
                {
                    result.Success = jobAllocationBL.InsertSupervisorForDigitalInventory(model, out message);
                }
                else
                {
                    result.Success = jobAllocationBL.InsertActivityAllocation(model, out message);
                }
                
                if (!result.Success)
                {
                    ModelState.AddModelError(string.Empty, message);
                    //this.AddToastMessage("RELOCBS", message, ToastType.Error);
                }
                else
                {
                    model = jobAllocationBL.GetActivityJobAllocationById(model.InstIds!=null ? model.InstIds.ToArray() : Enumerable.Empty<long>().ToArray(), model.ActivityIds.ToArray(), model.BatchID,model.JobType);
                    //this.AddToastMessage("RELOCBS", message, ToastType.Success);
                    model.docUpload.ID = ID;
                    model.docUpload.DocTypeID = DocTypeID;
                    model.docUpload.DocNameID = DocumentNameID;
                    model.docUpload.DocFromType = FromType;
                    model.TabIndex = SelectedTabIndex;
                    model.JobType = JobType;
                    result.Message = message;
                    result.Result = this.RenderPartialViewToString("_ActivityAllocationPartial", model);
                    return Json(result);

                    //return Json(new { status = "success", Message = message });
                }

                return Request.IsAjaxRequest() ? (ActionResult)PartialView("_ActivityAllocationPartial", model) : View("_ActivityAllocationPartial", model);
            }
            
            return Request.IsAjaxRequest() ? (ActionResult)PartialView("_ActivityAllocationPartial", model) : View("_ActivityAllocationPartial", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandleError]
        public ActionResult CreateActivity(JobActivity model, string submit)
        {

            fillCombo();
            if (ModelState.IsValid)
            {
                // Save it in database

                //Return Success message
                string message = "";
                ModelState.Clear();
                bool res = false;
                res = jobAllocationBL.InsertActivity(model, out message);
                if (!res)
                {
                    ModelState.AddModelError(string.Empty, message);
                    this.AddToastMessage("RELOCBS", message, ToastType.Error);
                    //return Json(result);
                }
                else
                {
                    //model = jobAllocationBL.GetJobActivityDetailById(model.InstID, model.ActivityID);
                    this.AddToastMessage("RELOCBS", message, ToastType.Success);
                    return Json(new { status ="success" , Message = message });
                    //return Json(result);
                }

                //return Json(new { status = res ? "success" : "failure", Message = message });
                return PartialView("_ActivityDetail", model);
            }

            //return Json(new
            //{
            //    status = "failure",
            //    formErrors = ModelState.Where(kvp=> kvp.Value.Errors.Count>0).Select(kvp => new { key = kvp.Key, errors = kvp.Value.Errors.Select(e => e.ErrorMessage) })
            //});
            return PartialView("_ActivityDetail", model);
        }

        //public ActionResult DeleteActivityAllocation()
        //{
        //    return View();
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteActivityAllocation(Int64 BatchID, Int64 ActivityID,Int64 Inst_BatchID,Int16 JobType)
        {
            AjaxResponse result = new AjaxResponse();
            try
            {
                // TODO: Add delete logic here
                
                string message = string.Empty;
                bool res = false;
                res = jobAllocationBL.DeleteActivityAllocation(BatchID, ActivityID, UserSession.GetUserSession().LoginID, out message);
                if (!res)
                {
                    result.Success = false;
                    ModelState.AddModelError(string.Empty, "Unable to Delete.");
                    result.Message = message;

                    this.AddToastMessage("RELOCBS", message, ToastType.Error);
                    return Json(result);
                    //return Json(result);
                }
                else
                {
                    result.Success = true;
                    result.Message = message;
                    this.AddToastMessage("RELOCBS", message, ToastType.Success);
                    JobDiaryModel obj = jobAllocationBL.GetBulkInstDetailById(new long[0], Inst_BatchID, JobType);
                    return PartialView("_ActivityGridPartial", obj);
                    //return Json(result);
                }
                
                //ViewBag.Result = result;
                //return RedirectToAction("Create", new { SurveyID = SurveyID, RateCompRateWtID = RateCompRateWtID });

            }
            catch
            {
                result.Success = false;
                result.Message = "Unexpected error occurs";
                return Json(result);
            }
        }

        public ActionResult GetMaterialRate(int[] MaterailID)
        {
            var data = jobAllocationBL.GetMaterialRate(MaterailID).Select(u => new
            {
                ID = u.Key,
                Rate = u.Value
            });
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddActivity(Int64 InstID,Int64? ActivityID)
        {
            fillCombo();
            JobActivity data = jobAllocationBL.getActivityDetailById(InstID, ActivityID);
            return PartialView("_AddActivityPartial", data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddActivity(JobActivity model)
        {
            fillCombo();

            if (ModelState.IsValid)
            {
                
                string message = "";
                ModelState.Clear();
                bool res = false;
                res = jobAllocationBL.AddEditActivity(model, out message);
                if (!res)
                {
                    ModelState.AddModelError(string.Empty, message);
                    this.AddToastMessage("RELOCBS", message, ToastType.Error);
                }
                else
                {
                    //JobActivity data = jobAllocationBL.getActivityDetailById(model.InstID, model.ActivityID);
                    this.AddToastMessage("RELOCBS", message, ToastType.Success);
                    //return Json(new { status = "success", Message = message });
                }

                return RedirectToAction("BulkCreate", "JobAllocation");
            }
            //else
            //{
            //    this.AddToastMessage("RELOCBS","", ToastType.Error);
            //}
            return RedirectToAction("BulkCreate", "JobAllocation");

            // return PartialView("_AddActivityPartial", model);
        }

        public ActionResult GetInstructionSheet(Int64 MoveID,Int64 InstID,Int16 JobType)
        {

            fillCombo();
            if (JobType == 0)
            {
                var model = warehouseJobBL.GetWHInstructionSheetDetail(MoveID, InstID);
                return PartialView("_WHInstructionSheetPartial", model);
            }
            else
            {
                var model = jobInstructionBL.GetDetailById(MoveID, InstID);
                return PartialView("_InstructionSheetPartial", model);
            }

            
            
        }

        

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DownloadDoc(Int64 DocID, Int64? BatchID)
        {

            JobDocument jobDocument = jobAllocationBL.GetDownloadFile(DocID,BatchID);
            if (!string.IsNullOrWhiteSpace(jobDocument.FilePath) && System.IO.File.Exists(jobDocument.FilePath))
            {
                //var filepath = System.IO.Path.Combine(Server.MapPath("/Files/"), fileName);
                return File(jobDocument.FilePath, MimeMapping.GetMimeMapping(jobDocument.FilePath), jobDocument.FileName);
            }

            return new HttpStatusCodeResult(404);
        }

        [HttpGet]
        public ActionResult GetEmployeeAllocationDetail(int EmpID,DateTime FromDate,DateTime ToDate,Int64? BatchID=-1)
        {
            EmployeeAllocation model = jobAllocationBL.GetEmployeeAllocation(EmpID,FromDate,ToDate, BatchID);
            return PartialView("_EmployeeAllocationPartial", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangeWarehouse(int WarehouseID)
        {
            TempData["WarehouseID"] = WarehouseID;
            
            return RedirectToAction("Index", "JobAllocation");
        }

        public ActionResult GetJobDocumentsList(Int64 MoveID)
        {
            var model = jobAllocationBL.GetJobDocuments(MoveID);
            return PartialView("_JobDocuments", model);
        }


        public ActionResult JobAllocationReport()
        {
            _PageID = "49";

            if (!CSubs.CheckPageRights(_PageID, PermissionType.VIEW))
            {
                return new HttpStatusCodeResult(403);
            }
            fillCombo(WarehouseType: "ALLACTIVEUSERWISE");
            session.Set<string>("PageSession", "Job Diary");

            return View();
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult JobAllocationReport(int? page)
        {
            fillCombo(WarehouseType: "ALLACTIVEUSERWISE");

            Dictionary<string, string> param = new Dictionary<string, string>();
            try
            {

                DateTime Fromdate = System.DateTime.Now.Date.AddDays(-7);
                DateTime Todate = System.DateTime.Now;
                
                int AllocationStatus = -1;
                string Shipper = string.Empty;
                int WarehouseID = -1;

                if (Request.Form["FromDate"] != null && Request.Form["FromDate"].Trim() != "")
                {
                    Fromdate = Convert.ToDateTime(Request.Form["FromDate"]);
                    param.Add("@SP_FromDate", Fromdate.ToString("dd-MMM-yyyy"));
                }

                if (Request.Form["ToDate"] != null && Request.Form["ToDate"].Trim() != "")
                {
                    Todate = Convert.ToDateTime(Request.Form["ToDate"]);
                    param.Add("@SP_ToDate", Todate.ToString("dd-MMM-yyyy"));
                }
                
                if (Request.Form["Shipper"] != null && Request.Form["Shipper"].Trim() != "")
                {
                    Shipper = Convert.ToString(Request.Form["Shipper"]);
                    param.Add("@SP_SHIPPER", Convert.ToString(Request.Form["Shipper"]));
                }

                if (Request.Form["AllocationStatus"] != null && Request.Form["AllocationStatus"].Trim() != "")
                {
                    param.Add("@SP_AllocationStatus", Convert.ToString(Request.Form["AllocationStatus"]));
                }

                if (Request.Form["WarehouseID"] != null && Request.Form["WarehouseID"].Trim() != "")
                {
                    param.Add("@SP_WarehouseID", Convert.ToString(Request.Form["WarehouseID"])); 
                }


                string SearchKey = string.Empty;
                if (Request.Form["SearchKey"] != null && Request.Form["SearchKey"].Trim() != "")
                {
                    param.Add("@SP_SearchString", Request.Form["SearchKey"]);
                }

                param.Add("@SP_CompID", UserSession.GetUserSession().CompanyID.ToString());

                if (UserSession.GetUserSession().BussinessLine == "NON RMC-BUSINESS")
                {
                    param.Add("@SP_IsRMCBuss", "false"); 
                }
                else
                {
                    param.Add("@SP_IsRMCBuss", "true");
                }
                
                param.Add("@SP_LoginID", Convert.ToString(UserSession.GetUserSession().LoginID));

                bool result = CommonService.GenerateExcel(this.Response, "JobAllocationReport", "[Warehouse].[GetJobAllocationReport]", param);

                if (!result)
                {
                    this.AddToastMessage("RELOCBS", "No records to Download", ToastType.Error);
                }

                return View();
            }
            catch (Exception ex)
            {
                this.AddToastMessage("RELOCBS", "UnExpected Error occured", ToastType.Error);
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteDoc(int id)
        {
            bool res = false;
            string message = string.Empty;
            AjaxResponse result = new AjaxResponse();
            res = moveManageBL.DeleteDocument(id, out message);
            result.Message = message;
            result.Success = res;
            return Json(result);
        }

        public ActionResult GetSurveyReport(int EnqDetailID, int SurveyID)
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

        [HttpGet]
        public ActionResult GetVehicle(int VehicleNo)
        {
            var data = new VehicleBL().GetDetailById(VehicleNo);
            return Json(new { Supplier = data.VendorID, Dimension = data.DimensionId }, JsonRequestBehavior.AllowGet);
            
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult RequestApproval(string V1, Int16 V2, int V3, int V4, int V5,string qCode)
        {

            if(CommonService.CheckValidApprovalLink(V5,qCode))
            {
                session.Set<string>("PageSession", "Job Diary - Request Approval");

                Int64[] InstID = null;
                Int64[] ActivityID = new Int64[] { V3 };
                Int64? BatchID = V4;
                Int16 JobType = V2;
                int LoginID = V5;

                ViewBag.V2 = V2;
                ViewBag.V3 = V3;
                ViewBag.V4 = V4;
                ViewBag.V5 = V5;
                ViewBag.V5 = V5;
                ViewBag.qCode = qCode;
                ViewBag.FromRequestApproval = true;
                var model = new DAL.JobAllocation.JobAllocationDAL().GetActivityJobAllocationById(LoginID, ActivityID, BatchID, JobType);
                /////Job Allocation Doc Type
                model.docUpload.DocTypeID = 10;
                model.docUpload.DocFromType = "JobDiary";
                model.docUpload.ID = Convert.ToInt64(model.BatchID);
                model.TabIndex = 2;
                fillCombo(RateComponentID: model.RateComponentID, BranchID: model.BranchID, LoginID: LoginID, CompID: model.CompanyID);
                return View(model);
            }

            return RedirectToAction("Index", "Home");
            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult RequestApproval(ActivityAllocationModel model, string V1, Int16 V2, int V3, int V4, int V5, string qCode)
        {
            session.Set<string>("PageSession", "Job Diary - Request Approval");
            string message = string.Empty;
            Int64[] InstID = null;
            Int64[] ActivityID = new Int64[] { V3 };
            Int64? BatchID = V4;
            Int16 JobType = V2;
            int LoginID = V5;
            ViewBag.V2 = V2;
            ViewBag.V3 = V3;
            ViewBag.V4 = V4;
            ViewBag.V5 = V5;
            ViewBag.qCode = qCode;
            ViewBag.FromRequestApproval = true;
            
            if (!string.IsNullOrWhiteSpace(model.HVCrewMembers))
            {
                CrewMemberRootObject crewMembers = (CrewMemberRootObject)JsonConvert.DeserializeObject(Convert.ToString(model.HVCrewMembers), (typeof(CrewMemberRootObject)));  //JsonConvert.DeserializeObject<List<Inst_Activities>>(model.HVactivityList);
                model.jobCrew.members = crewMembers.members;
            }
            
            if (!string.IsNullOrWhiteSpace(model.HVMaterialUsed))
            {
                MaterialUsedRootObject materialUsed = (MaterialUsedRootObject)JsonConvert.DeserializeObject(Convert.ToString(model.HVMaterialUsed), (typeof(MaterialUsedRootObject)));  //JsonConvert.DeserializeObject<List<Inst_Activities>>(model.HVactivityList);
                model.materialUsed = materialUsed.materialUsed;
            }
            if (!string.IsNullOrWhiteSpace(model.HVServices))
            {
                JobServiceRootObject services = (JobServiceRootObject)JsonConvert.DeserializeObject(Convert.ToString(model.HVServices), (typeof(JobServiceRootObject)));  //JsonConvert.DeserializeObject<List<Inst_Activities>>(model.HVactivityList);
                model.services = services.services;
            }

            if (!string.IsNullOrWhiteSpace(model.HVVehicles))
            {
                JobVehicleRootObject jobVehicle = (JobVehicleRootObject)JsonConvert.DeserializeObject(Convert.ToString(model.HVVehicles), (typeof(JobVehicleRootObject)));  //JsonConvert.DeserializeObject<List<Inst_Activities>>(model.HVactivityList);
                model.jobVehicleList = jobVehicle.jobVehicleList;
            }

            if (!model.submit.Equals("UPLOAD", StringComparison.OrdinalIgnoreCase))
            {
                ModelState.Remove("docUpload.ID");
                ModelState.Remove("docUpload.DocFromType");
                ModelState.Remove("docUpload.DocTypeID");
                ModelState.Remove("docUpload.DocNameID");
                ModelState.Remove("docUpload.file");
            }
            fillCombo(RateComponentID: model.RateComponentID, BranchID: model.BranchID, LoginID: LoginID,CompID: model.CompanyID);
            if (ModelState.IsValid)
            {
                AjaxResponse result = new AjaxResponse();
                ModelState.Clear();

                if(model.submit.Equals("ApprovalSave", StringComparison.OrdinalIgnoreCase))
                {
                    result.Success = jobAllocationBL.UpdateHiredVehicleApprovalStatus(model, LoginID, out message);
                    if (!result.Success)
                    {
                        ModelState.AddModelError(string.Empty, message);
                        this.AddToastMessage("RELOCBS", message, ToastType.Error);
                    }
                    else
                    {
                        this.AddToastMessage("RELOCBS", message, ToastType.Success);
                        model = new DAL.JobAllocation.JobAllocationDAL().GetActivityJobAllocationById(LoginID, ActivityID, BatchID, JobType); 
                        
                    }
                }

                
            }
            model.TabIndex = 2;
            return View(model);
        }

        [HttpGet]
        public ActionResult CrewUtilization(int Page)
        {

            if (!CSubs.CheckPageRights(Convert.ToString(Page), PermissionType.VIEW) || !CSubs.CheckPageRights("19", PermissionType.VIEW))
            {
                return new HttpStatusCodeResult(403);
            }

            ViewBag.WarehouseList = comboBL.GetWarehouseDropdown();
            ViewBag.Page = Page;
            var model = new CrewUtilizationDashobard();
            model.ForMonthDate = System.DateTime.Now;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CrewUtilization(int Page,CrewUtilizationDashobard model)
        {
            ViewBag.WarehouseList = comboBL.GetWarehouseDropdown();
            ViewBag.Page = Page;
            model.data = jobAllocationBL.GetCrewUtilizationDashboard(model.ForMonthDate, model.WarehoseId);
            return View(model);
        }

    }
    
}