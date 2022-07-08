using RELOCBS.App_Code;
using RELOCBS.BL;
using RELOCBS.BL.Report;
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
    public class ReportController : BaseController
    {
        private string _PageID = "58";

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

        private CommonReportBL _reportBL;
        public CommonReportBL reportBL
        {
            get
            {
                if (this._reportBL == null)
                    this._reportBL = new CommonReportBL();
                return this._reportBL;
            }
        }

        public ActionResult Index()
        {
            if (!CSubs.CheckPageRights(_PageID, PermissionType.VIEW))
            {
                return new HttpStatusCodeResult(403);
            }
            ViewBag.PageTitle = "Reports";
            session.Set<string>("PageSession", ViewBag.PageTitle);
            CommonReport model = new CommonReport();
            model.Todate = DateTime.Now;
            model.FromDate = DateTime.Now.AddDays(-2);

            FillCombo(model.ReportNameList, model.ReportID);
            model.ReportNameList = reportBL.GetReportsList(model);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(CommonReport model, string submit)
        {
            if (!CSubs.CheckPageRights(_PageID, PermissionType.VIEW))
            {
                return new HttpStatusCodeResult(403);
            }
            ViewBag.PageTitle = "Reports";
            session.Set<string>("PageSession", ViewBag.PageTitle);
            model.ReportNameList = reportBL.GetReportsList(model);

            string BussLine = null;
            if (model.BussLineID.Count() == 2)
            {
                BussLine = "Both";
            }
            else
            {
                if (model.BussLineID.FirstOrDefault() == "RMC-BUSINESS")
                {
                    BussLine = "RMC";
                }
                else if (model.BussLineID.FirstOrDefault() == "NON RMC-BUSINESS")
                {
                    BussLine = "Non RMC";
                }
            }
            FillCombo(model.ReportNameList, model.ReportID, model.ReportName, BussLine);

            if (ModelState.IsValid)
            {
                string message = string.Empty;
                DataTable result = reportBL.GetReport(model, submit, out message);
                if (submit.ToUpper() == "GENERATE")
                {
                    bool value = CommonService.GenerateExcel(this.Response, result, model.ReportName + "_" + Guid.NewGuid().ToString());

                    if (!value)
                    {
                        this.AddToastMessage("RELOCBS", message, ToastType.Error);
                    }
                }
            }
            return View(model);
        }

        private void FillCombo(IEnumerable<SelectListItem> reportList, int reportid, string ReportName = "", string BussLine = "")
        {
            ViewData["RevenueBranchList"] = comboBL.GetCompanyBranchDropdown(CompanyID: UserSession.GetUserSession().CompanyID, ForPage: "report");
            ViewData["ServiceLineList"] = GetServiceLineDropdown(ReportName, BussLine);
            ViewData["ReportNameList"] = reportList;
            ViewData["ReportDateColList"] = reportBL.GetDateColList(reportid);
            if (UserSession.GetUserSession().BussinessLine == "ORIENTATION SERVICE")
            {
                ViewData["BussLineList"] = comboBL.GetUserBussinessLineDropdown(LoginId: UserSession.GetUserSession().LoginID).ToList().Where(x => x.Value == "ORIENTATION SERVICE");
            }
            else
            {
                ViewData["BussLineList"] = comboBL.GetUserBussinessLineDropdown(LoginId: UserSession.GetUserSession().LoginID).ToList().Where(x => x.Value != "ORIENTATION SERVICE");
            }
        }

        public ActionResult GetDateColumn(int id, string ReportName)
        {
            var lstItem = reportBL.GetDateColList(id).Select(i => new { i.Value, i.Text }).ToList();
            return Json(new { lstItem }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetServiceLine(string BussLine, string ReportName)
        {
            var lstServiceLineList = GetServiceLineDropdown(ReportName, BussLine);
            return Json(new { lstServiceLineList }, JsonRequestBehavior.AllowGet);
        }

        public IEnumerable<SelectListItem> GetServiceLineDropdown(string ReportName = "", string BussLine = "")
        {
            if (string.IsNullOrEmpty(BussLine))
                BussLine = UserSession.GetUserSession().BussinessLine == "ORIENTATION SERVICE" ? "ORIENTATION SERVICE" : "Both";

            return comboBL.GetServiceLineDropdown(RMCBuss: !(UserSession.GetUserSession().BussinessLine == "NON RMC-BUSINESS"), ForPage: "report", BussLine: BussLine)
                .Where(r => ((r.Text.ToUpper() == "EXPORT" || r.Text.ToUpper() == "IMPORT" || r.Text.ToUpper() == "DOMESTIC") && ReportName.ToUpper() == "ACO TRACKER") || ReportName.ToUpper() != "ACO TRACKER");
        }
    }
}