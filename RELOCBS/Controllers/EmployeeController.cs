using PagedList;
using RELOCBS.AjaxHelper;
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
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace RELOCBS.Controllers.Common
{
    [AuthorizeUser]
    public class EmployeeController : BaseController
    {

        private string _PageID = "21";

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

        // GET: Employee
        public ActionResult Index(int? page)
        {

            if (!CSubs.CheckPageRights(_PageID, PermissionType.VIEW))
            {
                return new HttpStatusCodeResult(403);
            }
            session.Set<string>("PageSession", "Employee Master");
            //ViewData["CityList"] = cityBL.GetCityDropdown();
            var pageIndex = (page ?? 1);
            int pageSize = settings.GetSettingByKey<int>("pagination_pagesize");
            int totalCount = 10;
            string OrderBy = "CardEmpCode";
            int Order = 0;
            int? EmployeeId = null;
            int? BranchId = null;
            string SearchKey = string.Empty;
            if (Request.Form["SearchKey"] != null && Request.Form["SearchKey"].Trim() != "")
            {
                SearchKey = Request.Form["SearchKey"];
            }
            
            if (Request.Form["BranchId"] != null && Request.Form["BranchId"].Trim() != "")
            {
                BranchId = Convert.ToInt32(Request.Form["BranchId"]);
            }
            if (Request.Params["grid-column"] != null && Request.Params["grid-column"].Trim() != "")
            {
                OrderBy = Request.Params["grid-column"].Trim().ToString();
            }
            if (Request.Params["grid-dir"] != null && Request.Params["grid-column"].Trim() != "")
            {
                Order = Convert.ToInt32(Request.Params["grid-dir"].Trim().ToString());
            }
            //if (Request.Form["CityID"] != null && Request.Form["CityID"].Trim() != "")
            //{
            //    CityID = Convert.ToInt32(Request.Form["CityID"]);
            //}
            var items = employeeBL.GetEmployeeList(pageIndex, pageSize, OrderBy, Order, EmployeeId,BranchId, null, SearchKey, UserSession.GetUserSession().LoginID, out totalCount);
            if (totalCount == 0 && pageIndex > 1)
            {
                pageIndex = 1;
                items = employeeBL.GetEmployeeList(pageIndex, pageSize, OrderBy, Order, EmployeeId, BranchId, null, SearchKey, UserSession.GetUserSession().LoginID, out totalCount);
            }
            var itemsAsIPagedList = new StaticPagedList<Employee>(items, pageIndex, pageSize, totalCount);
            TempData["EmpList"] = itemsAsIPagedList;
            FillDropDown();
            return Request.IsAjaxRequest()
                ? (ActionResult)PartialView("_AjaxPartial")
                : View(new Employee());
        }

        // GET: City/Edit/5
        public ActionResult Edit(int id)
        {
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            Employee data = employeeBL.GetDetailById(id);
            //TempData.Peek("EmpList");
            ViewBag.Flag = "1";
            FillDropDown(CompID:data.CompId);
            //ViewData["Country"] = employeeBL.GetCountryByContinent("");
            /*if (data == null)
            {
                return HttpNotFound();
            }
            return Request.IsAjaxRequest()
                ? (ActionResult)PartialView("Edit", data)
                : View(data);*/
            return PartialView("Edit", data);

        }

        // POST: City/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Employee emp)
        {
            try
            {
                //if (!emp.IsActive && emp.DOL == null)
                //{
                //    ModelState.AddModelError(string.Empty, "DOL is mandatory for inactive employees.");
                //    return RedirectToAction("Index");
                //}
                //else
                //{
                    
                    AjaxResponse result = new AjaxResponse();
                    string message = string.Empty;
                    ViewBag.Flag = "1";
                    FillDropDown();
                    if (ModelState.IsValid)
                    {
                        result.Success = employeeBL.Insert(emp, "Update", out message);

                        if (result.Success)
                        {

                            result.Message = message;
                            result.Result = this.RenderPartialViewToString("Create", emp);
                            return Json(result);
                        }
                        else
                        {
                             ModelState.AddModelError(string.Empty, message);
                        }
                    }

                //}
                return Request.IsAjaxRequest() ? (ActionResult)PartialView("Edit", emp): View(emp);
            }
            catch
            {
                return View(emp);
            }
        }

        public ActionResult Create()
        {
            Employee model = new Employee();
            model.IsActive = true;
            ViewBag.Flag = "1";
            FillDropDown();
            return Request.IsAjaxRequest()
                ? (ActionResult)PartialView("Create", model)
                : View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Employee data)
        {
            try
            {
                //ViewData["StateList"] = cityBL.BindDropdown("StatesByCountryId", "", "");
                //ViewData["Country"] = employeeBL.GetCountryByContinent("");
                //if (!data.IsActive && data.DOL == null)
                //{
                //    ModelState.AddModelError(string.Empty, "DOL is mandatory for inactive employees.");
                //    return RedirectToAction("Index");
                //}
                FillDropDown();
                AjaxResponse result = new AjaxResponse();
                if (ModelState.IsValid)
                {
                    string Message;
                    result.Success = employeeBL.Insert(data, "Ïnsert",out Message);
                    if (result.Success)
                    {
                        result.Message = Message;
                        result.Result = this.RenderPartialViewToString("Create", data);
                        return Json(result);
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, Message);
                    }
                }
                
                    return Request.IsAjaxRequest() ? (ActionResult)PartialView("Create", data) : View(data);
            }
            catch
            {
                return View(data);
            }
        }

        private void FillDropDown(int CompID=-1)
        {
            ViewData["CityList"] = new List<SelectListItem>(); //comboBL.GetCityDropdown();
            ViewData["CompList"] = comboBL.GetCompanyDropdown();
            ViewData["DesignationList"] = comboBL.getDesignationDropdown();
            ViewData["BranchList"] = comboBL.GetCompanyBranchDropdown(CompanyID: CompID);
        }

        public ActionResult ExportToExcel()
        {
            Dictionary<string, string> param = new Dictionary<string, string>();
            try
            {
                
                if (Request.Form["SearchKey"] != null && Request.Form["SearchKey"].Trim() != "")
                {
                    param.Add("@SP_SearchString", Request.Form["SearchKey"]);
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

                CommonService.GenerateExcel(this.Response, "Employee", "[Comm].[GETEmployeeForGrid_ExpToExl]", param);
            }
            catch (Exception ex)
            {
                this.AddToastMessage("RELOCBS", "UnExpected Error occured", ToastType.Error);
            }
            return View();
        }

        public JsonResult GetAutoPopulateList()
        {
            var lst = comboBL.GetEmployeeDropdown().Select(i => new { i.Value, i.Text }).ToList();
            return Json(lst, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdateWarehouseMapping(int id)
        {
            EmployeeWarehouseMapping model = employeeBL.GetWarehouseMapping(id);
            ViewBag.Flag = "1";
            ViewBag.BranchList = comboBL.GetWarehouseDropdown();
            return Request.IsAjaxRequest()
                ? (ActionResult)PartialView("UpdateWarehouseMapping", model)
                : View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateWarehouseMapping(EmployeeWarehouseMapping data)
        {
            try
            {
                ViewBag.BranchList = comboBL.GetWarehouseDropdown();
                AjaxResponse result = new AjaxResponse();
                if (ModelState.IsValid)
                {
                    string Message;
                    result.Success = employeeBL.UpdateWarehouseMapping(data, out Message);
                    if (result.Success)
                    {
                        result.Message = Message;
                        result.Result = this.RenderPartialViewToString("UpdateWarehouseMapping", data);
                        return Json(result);
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, Message);
                    }
                }

                return Request.IsAjaxRequest() ? (ActionResult)PartialView("UpdateWarehouseMapping", data) : View(data);
            }
            catch
            {
                return View(data);
            }
        }


        public ActionResult SurveyorBranch(int page = 1)
        {
            String PageID = "83";
            if (!CSubs.CheckPageRights(PageID, PermissionType.VIEW))
            {
                return new HttpStatusCodeResult(403);
            }
            session.Set<string>("PageSession", "Surveyor Branch");
            string sort = "ATRPointId";
            string sortdir = "desc";
            int pageSize = settings.GetSettingByKey<int>("pagination_pagesize");
            int Order = 0;
            string IssueHeading = string.Empty;
            string SearchKey = string.Empty;
            if (Request.Form["SearchKey"] != null && Request.Form["SearchKey"].Trim() != "")
            {
                SearchKey = Convert.ToString(Request.Form["SearchKey"]);
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

            var data = employeeBL.GetSurveyorList(SearchKey, sort, sortdir, skip, pageSize, out totalRecord);
            

            ViewBag.TotalRows = totalRecord;
            ViewBag.IssueHeading = IssueHeading;

            var itemsAsIPagedList = new StaticPagedList<Entities.BranchSurveyorMappingGrid>(data, pageSize, skip, totalRecord);
            return Request.IsAjaxRequest() ? (ActionResult)PartialView("_SurveyorBranchPartial", itemsAsIPagedList) : View("SurveyorBranch", itemsAsIPagedList);
        }

        public ActionResult UpdateSurveyorBranch(int id)
        {
            BranchSurveyorMapping model = employeeBL.GetSurveyorBranchMapping(id);
            ViewBag.Flag = "1";
            ViewBag.BranchList = comboBL.GetCompanyBranchDropdown();
            return Request.IsAjaxRequest()
                ? (ActionResult)PartialView("UpdateSurveyorBranch", model)
                : View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateSurveyorBranch(BranchSurveyorMapping data)
        {
            try
            {
                ViewBag.BranchList = comboBL.GetCompanyBranchDropdown();
                AjaxResponse result = new AjaxResponse();
                if (ModelState.IsValid)
                {
                    string Message;
                    result.Success = employeeBL.UpdateSurveyorBranchMapping(data, out Message);
                    if (result.Success)
                    {
                        result.Message = Message;
                        result.Result = this.RenderPartialViewToString("UpdateSurveyorBranch", data);
                        return Json(result);
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, Message);
                    }
                }

                return Request.IsAjaxRequest() ? (ActionResult)PartialView("UpdateSurveyorBranch", data) : View(data);
            }
            catch
            {
                return View(data);
            }
        }
    }
}