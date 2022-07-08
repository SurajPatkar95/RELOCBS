using RELOCBS.App_Code;
using RELOCBS.BL;
using RELOCBS.BL.Common;
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
    public class EmployeeLeaveRegisterController : BaseController
    {

        private string _PageID = "40";

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

        private EmployeeBL _employeeBL;
        public EmployeeBL employeeBL
        {
            get
            {
                if (this._employeeBL == null)
                    this._employeeBL = new EmployeeBL();
                return this._employeeBL;

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

        private void fillCombo()
        {

            ViewData["EmployeeList"] = comboBL.GetEmployeeDropdown(CompanyID: UserSession.GetUserSession().CompanyID);
            ViewData["DesignationList"] = comboBL.getDesignationDropdown();
        }

        // GET: EmployeeLeaveRegister
        public ActionResult Index()
        {
            if (!CSubs.CheckPageRights(_PageID, PermissionType.VIEW))
            {
                return new HttpStatusCodeResult(403);
            }
            ViewBag.PageTitle = "Employee Leave Register";
            session.Set<string>("PageSession", ViewBag.PageTitle);
            fillCombo();
            EmployeeLeaveRegister employeeLeaveRegister = new EmployeeLeaveRegister();
            employeeLeaveRegister.EmployeeList = comboBL.GetEmployeeDropdown(CompanyID:UserSession.GetUserSession().CompanyID).Select(i => new { i.Value, i.Text }).ToDictionary(x => Convert.ToInt32(x.Value), x => x.Text);

            ViewBag.ForMonth = null;
            if (Request.Form["FromMonthDate"]!=null)
            {
                ViewBag.ForMonth = Convert.ToDateTime(Request.Form["FromMonthDate"]);
            }
            return View(employeeLeaveRegister);

        }

        [HttpPost]
        public ActionResult Index(EmployeeLeaveDetail employeeLeaveRegister)
        {
            if (!CSubs.CheckPageRights(_PageID, PermissionType.ADD) || !CSubs.CheckPageRights(_PageID, PermissionType.EDIT))
            {
                return new HttpStatusCodeResult(403);
            }

            fillCombo();
            string message = string.Empty;
            EmployeeLeaveRegister employee = new EmployeeLeaveRegister();
            employee.EmployeeList = comboBL.GetEmployeeDropdown(CompanyID: UserSession.GetUserSession().CompanyID).Select(i => new { i.Value, i.Text }).ToDictionary(x => Convert.ToInt32(x.Value), x => x.Text);
            ViewBag.ForMonth = null;
            if (Request.Form["FromMonthDate"] != null)
            {
                ViewBag.ForMonth = Convert.ToDateTime(Request.Form["FromMonthDate"]);
            }
            if (ModelState.IsValid)
            {
                bool res = false;
                res = employeeBL.InsertLeave(employeeLeaveRegister, out message);
                if (!res)
                {
                    ModelState.AddModelError(string.Empty, "Unable to save Enquiry data.");
                    this.AddToastMessage("RELOCBS", message, ToastType.Error);
                    //return Json(result);
                }
                else
                {
                    employee.employeeLeave = employeeBL.GetEmpLeaveDetail(employeeLeaveRegister.employee.EmpID);
                    this.AddToastMessage("RELOCBS", message, ToastType.Success);
                    //return Json(result);
                }
                //ViewBag.Result = result;
                //return RedirectToAction("create/"+data.EnquiryDetail.EnqDetailID);
                return View(employee);
            }
            else
            {
                return View(employee);
            }
        }

        // GET: EmployeeLeaveRegister/Details/5
        public ActionResult Details(int id)
        {
            fillCombo();
            EmployeeLeaveRegister employeeLeaveRegister = new EmployeeLeaveRegister();
            employeeLeaveRegister.EmployeeList =  comboBL.GetEmployeeDropdown(CompanyID: UserSession.GetUserSession().CompanyID).Select(i => new { i.Value, i.Text }).ToDictionary(x => Convert.ToInt32(x.Value), x => x.Text);
            EmployeeLeaveDetail employeeLeave = employeeBL.GetEmpLeaveDetail(id);
            return PartialView("_EmployeeLeave", employeeLeave);
        }

        // GET: EmployeeLeaveRegister/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: EmployeeLeaveRegister/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: EmployeeLeaveRegister/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: EmployeeLeaveRegister/Edit/5
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

        // GET: EmployeeLeaveRegister/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: EmployeeLeaveRegister/Delete/5
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

        
        [HttpPost]
        public ActionResult Export(DateTime FromMonthDate)
        {
            if (!CSubs.CheckPageRights(_PageID, PermissionType.VIEW))
            {
                return new HttpStatusCodeResult(403);
            }

            Dictionary<string, string> param = new Dictionary<string, string>();
            try
            {
                if (Request.Form["FromMonthDate"] != null && Request.Form["FromMonthDate"].Trim() != "")
                {
                    param.Add("@SP_ForMonthDate", Convert.ToString(Request.Form["FromMonthDate"]));
                }
                if (Request.Form["BranchId"] != null && Request.Form["BranchId"].Trim() != "")
                {
                    param.Add("@SP_BranchId", Request.Form["BranchId"]);
                }
                if (Request.Form["WarehouseId"] != null && Request.Form["WarehouseId"].Trim() != "")
                {
                    param.Add("@SP_WarehouseId", Request.Form["WarehouseId"]);
                }

                param.Add("@SP_LoginID", Convert.ToString(UserSession.GetUserSession().LoginID));
                param.Add("@SP_CompId", Convert.ToString(UserSession.GetUserSession().CompanyID));

                CommonService.GenerateExcel(this.Response, "EmployeeLeaves", "[Comm].[GETEmployeeLeaveForGrid_ExpToExl]", param);
            }
            catch (Exception ex)
            {
                this.AddToastMessage("RELOCBS", "UnExpected Error occured", ToastType.Error);
            }
            return Content("");
        }

    }
}
