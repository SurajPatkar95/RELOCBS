using Newtonsoft.Json;
using PagedList;
using RELOCBS.App_Code;
using RELOCBS.BL;
using RELOCBS.BL.Dashboard;
using RELOCBS.BL.JobAllocation;
using RELOCBS.BL.Survey;
using RELOCBS.CustomAttributes;
using RELOCBS.Entities;
using RELOCBS.Extensions;
using RELOCBS.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace RELOCBS.Controllers
{
    [AuthorizeUser]
    public class DashboardController : BaseController
    {
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

        private DashboardBL _BL;
        public DashboardBL BL
        {
            get
            {
                if (this._BL == null)
                    this._BL = new DashboardBL();
                return this._BL;

            }
        }

        // GET: Dashboard
        //[Route("{UserID}")] 
        public ActionResult Index(int key = 0)
        {

            Dashboard obj = new Dashboard();
            key = key <= 0 ? Convert.ToInt32(UserSession.GetUserSession().EmpID) : key;
            List<DataPoint> dataPoints = new List<DataPoint>();
            obj = BL.GetDataPointList(key);

            ViewBag.DataPoints = JsonConvert.SerializeObject(obj.dataPoint);
            ViewBag.DataPoints2 = JsonConvert.SerializeObject(obj.dataPoint2);
            ViewBag.DataPoints3 = JsonConvert.SerializeObject(obj.dataPoint3);
            ViewBag.DataPoints_Pie = JsonConvert.SerializeObject(obj.dataPoint_Pie);
            obj.UserDetails.UserID = key;
            FillCombo();


            return View(obj);
        }

        [HttpPost]
        public ActionResult Index(Dashboard obj)
        {
            //key = key <= 0 ? UserSession.GetUserSession().LoginID : key;
            List<DataPoint> dataPoints = new List<DataPoint>();
            obj = BL.GetDataPointList(Convert.ToInt32(obj.UserDetails.UserID));

            ViewBag.DataPoints = JsonConvert.SerializeObject(obj.dataPoint);
            ViewBag.DataPoints2 = JsonConvert.SerializeObject(obj.dataPoint2);
            ViewBag.DataPoints3 = JsonConvert.SerializeObject(obj.dataPoint3);
            ViewBag.DataPoints_Pie = JsonConvert.SerializeObject(obj.dataPoint_Pie);
            FillCombo();


            return View();
        }

        public void FillCombo(int JobStatusID = 0) {
            ViewData["SDList"] = comboBL.GetEmployeeDropdown(SPTYPE:"Dashboard");
            ViewData["ReasonList"] = comboBL.GetDashboardReasonList(JobStatusID :JobStatusID);
        }

        public ActionResult Survey(int Page)
        {
            session.Set<string>("PageSession", "Survey Scheduled Dashboard");
            if (!CSubs.CheckPageRights(Convert.ToString(Page), PermissionType.VIEW) || !CSubs.CheckPageRights("9", PermissionType.VIEW))
            {
                return new HttpStatusCodeResult(403);
            }
            ViewBag.BranchList = comboBL.GetCompanyBranchDropdown(ForPage: "report");

            //.Where(r => ((r.Text.ToUpper() == "EXPORT" || r.Text.ToUpper() == "IMPORT" || r.Text.ToUpper() == "DOMESTIC"));
            ViewBag.Page = Page;
            var model = new ScheduleSurveyDashboard();
            model.ForMonthDate = System.DateTime.Now;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Survey(int Page, ScheduleSurveyDashboard model)
        {
            session.Set<string>("PageSession", "Survey Scheduled Dashboard");
            ViewBag.BranchList = comboBL.GetCompanyBranchDropdown(ForPage: "report");

            ViewBag.Page = Page;
            ViewBag.FromDate = model.ForMonthDate;
            model.data = BL.GetSurveyDashboard(model);
            return View(model);
        }

        public ActionResult SurveyorEvents(int id, DateTime fromDate)
        {
            var data = BL.GetSurveyorEvents(id, fromDate);
            return Request.IsAjaxRequest() ? (ActionResult)PartialView("_SurveyorEvents", data) : PartialView("_SurveyorEvents", data);
        }

        public ActionResult CrewUtilization(int Page)
        {
            session.Set<string>("PageSession", "Crew Utilization Dashboard");
            if (!CSubs.CheckPageRights(Convert.ToString(Page), PermissionType.VIEW))
            {
                return new HttpStatusCodeResult(403);
            }
            ViewBag.Page = Page;
            var model = new CrewUtilizationDashboard();
            model.FromMonthDate = System.DateTime.Now.AddMonths(-1);
            model.ToMonthDate = System.DateTime.Now;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CrewUtilization(int Page, CrewUtilizationDashboard model)
        {
            session.Set<string>("PageSession", "Crew Utilization Dashboard");
            ViewBag.Page = Page;
            ViewBag.FromDate = model.FromMonthDate;
            ViewBag.ToDate = model.ToMonthDate;
            model.data = BL.GetCrewUtilization(model);
            return View(model);
        }


        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult CrewUtilizationExport(int Page, string GridHtml)
        {
            return File(Encoding.ASCII.GetBytes(GridHtml), "application/vnd.ms-excel", "CrewUtilizationExport.xls");
        }

        public ActionResult GetJobList(string label, string UserID, string flag)
        {


            //ViewBag.TotalRows = totalRecord;
            //ViewBag.search = search;
            Dashboard obj = new Dashboard();
            obj.JobDetail = BL.GetFollowUpDetails(label, UserID, flag);//new StaticPagedList<JobDetail>(data, 10, 1, 2000);
            
            FillCombo(Convert.ToInt32(label.Split('-')[0]));
            return PartialView("FollowupPartial", obj.JobDetail);
        }

        public ActionResult SaveFollowup(JobDetail objJobDetail)
        {
            try
            {
                string result = string.Empty;
                bool res = BL.SaveFollowup(objJobDetail, out result);
                if (true)//res)
                {
                    this.AddToastMessage("RELOCBS", result, ToastType.Success);
                }
                else
                {
                    this.AddToastMessage("RELOCBS", result, ToastType.Error);
                }
            }
            catch (Exception e)
            {
                this.AddToastMessage("RELOCBS", "Error in saving.", ToastType.Error);
            }
            return RedirectToAction("Index",new { UserID = 10063 });
        }
    }
}











