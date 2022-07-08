using Newtonsoft.Json;
using PagedList;
using RELOCBS.AjaxHelper;
using RELOCBS.App_Code;
using RELOCBS.BL;
using RELOCBS.BL.VehicleKmsTrack;
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
    public class VehicleKmsTrackController : BaseController
    {

        private string _PageID = "68";
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

        private VehicleKmsBL _vehicleKmsBL;
        public VehicleKmsBL vehicleKmsBL
        {
            get
            {
                if (this._vehicleKmsBL == null)
                    this._vehicleKmsBL = new VehicleKmsBL();
                return this._vehicleKmsBL;
            }
        }

        // GET: VehicleKmsTrack
        public ActionResult Index(int page = 1)
        {
            if (!CSubs.CheckPageRights(_PageID, PermissionType.VIEW))
            {
                return new HttpStatusCodeResult(403);
            }

            session.Set<string>("PageSession", "Vehicle Kms Tracking");
            string sort = "";
            string sortdir = "";
            string search = "";
            int VehicleNo = -1;
            int BranchID = -1;

            int pageSize = settings.GetSettingByKey<int>("pagination_pagesize");
            string OrderBy = "";
            int Order = 0;
            DateTime? Fromdate = null;//System.DateTime.Now.Date.AddDays(-2);
            DateTime? Todate = null;//System.DateTime.Now;
            
            string SearchKey = string.Empty;
            if (Request.Form["FromDate"] != null && Request.Form["FromDate"].Trim() != "")
            {
                Fromdate = Convert.ToDateTime(Request.Form["FromDate"]);
            }

            if (Request.Form["ToDate"] != null && Request.Form["ToDate"].Trim() != "")
            {
                Todate = Convert.ToDateTime(Request.Form["ToDate"]);
            }
            
            if (Request.Form["VehicleNo"] != null && Request.Form["VehicleNo"].Trim() != "")
            {
                VehicleNo = Convert.ToInt32(Request.Form["VehicleNo"]);
            }

            if (Request.Form["BranchID"] != null && Request.Form["BranchID"].Trim() != "")
            {
                BranchID = Convert.ToInt32(Request.Form["BranchID"]);
            }
            string Shipper = string.Empty;
            string JobNo = null;
            if (Request.Params["Shipper"] != null && Request.Params["Shipper"].Trim() != "")
            {
                Shipper = Request.Params["Shipper"].Trim().ToString();
            }
            if (Request.Form["JobNo"] != null && Request.Form["JobNo"].Trim() != "")
            {
                JobNo = Convert.ToString(Request.Form["JobNo"]);
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
            var data = vehicleKmsBL.GetGrid(Fromdate, Todate, sort, sortdir, skip, pageSize, out totalRecord, BranchID, VehicleNo, Shipper, JobNo);
            //var data =new List<Entities.LoadChartsGrid>();//= loadChartBL.GetGrid(Fromdate, Todate, IsJobDate, IsInsuranceDate, JobNo, Insurance_ID, sort, sortdir, skip, pageSize, out totalRecord);
            FillCombo();
            ViewData["VehicleNo"] = VehicleNo;
            

            ViewBag.TotalRows = totalRecord;
            ViewBag.search = search;

            var itemsAsIPagedList = new StaticPagedList<Entities.VehicleKmsGrid>(data, pageSize, skip, totalRecord);
            return Request.IsAjaxRequest() ? (ActionResult)PartialView("_AjaxPartial", itemsAsIPagedList) : View(itemsAsIPagedList);
        }

        private void FillCombo(bool IsCreate = false,int BranchId=-1)
        {
            bool RMCBuss = !(UserSession.GetUserSession().BussinessLine == "NON RMC-BUSINESS");
            int CompID = UserSession.GetUserSession().CompanyID;
            if (IsCreate)
            {
                ViewData["ServiceLineList"] = comboBL.GetServiceLineDropdown(RMCBuss: RMCBuss);
            }
            ViewData["VehicleNoList"] = comboBL.GetVehicleDropdown(CompanyID: CompID,BranchID: BranchId);
            ViewData["JobNoList"] = new List<SelectListItem>();
            ViewData["BranchList"] = comboBL.GetCompanyBranchDropdown();
            
        }

        // GET: VehicleKmsTrack/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: VehicleKmsTrack/Create
        public ActionResult Create(Int64? Id)
        {
            FillCombo(true);
            var model = new VehicleKms();

            if (Id != null && Id > 0)
            {
                model = vehicleKmsBL.GetDetail(Convert.ToInt64(Id));
            }

            return View(model);
        }
        // POST: VehicleKmsTrack/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(VehicleKms model)
        {
            try
            {
                FillCombo(true,model.BranchID);
                string message = string.Empty;
                if (ModelState.IsValid)
                {
                    bool res = false;
                    if (!string.IsNullOrWhiteSpace(model.HFVkmsJobs))
                    {
                        List<VehicleKmsJobs> kmsJobs = JsonConvert.DeserializeObject<List<VehicleKmsJobs>>(model.HFVkmsJobs);
                        model.kmsJobs = kmsJobs;
                    }
                    if (!string.IsNullOrWhiteSpace(model.HFVLocations))
                    {
                        List<VehicleKmsTravelLocation> travelLocations = JsonConvert.DeserializeObject<List<VehicleKmsTravelLocation>>(model.HFVLocations);
                        model.travelLocations = travelLocations;
                    }

                    res = vehicleKmsBL.Insert(model, out message);
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
                        return RedirectToAction("Create", new { Id = model.VehicleKmsID });
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
                return View("Create", model);
            }
        }

        
        // POST: VehicleKmsTrack/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            try
            {

                AjaxResponse result = new AjaxResponse();
                string Message = string.Empty;

                if (ModelState.IsValid)
                {
                    result.Success = vehicleKmsBL.Delete(id, out Message);

                    if (result.Success)
                    {
                        result.Result = Message;
                    }
                    else
                    {
                        ModelState.AddModelError(String.Empty, Message);
                    }

                }
                else
                {
                    result.Success = false;
                    result.Result = "Delete failed. Please try again.";
                }

                return Json(result);
            }
            catch
            {
                return View();
            }
        }

        public ActionResult GetVehicleList(int BranchID=-1)
        {
            var VehicleList = comboBL.GetVehicleDropdown(BranchID:BranchID,CompanyID: UserSession.GetUserSession().CompanyID).Select(s=> new { s.Value,s.Text }).ToList();
            return Json(VehicleList, JsonRequestBehavior.AllowGet);
        }
    }
}
