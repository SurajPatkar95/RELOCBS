using PagedList;
using RELOCBS.AjaxHelper;
using RELOCBS.App_Code;
using RELOCBS.BL;
using RELOCBS.BL.Common;
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
    public class EmployeeWagesController : BaseController
    {
        private string _PageID = "48";

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

        private EmployeeWagesBL _employeeBL;
        public EmployeeWagesBL employeeBL
        {
            get
            {
                if (this._employeeBL == null)
                    this._employeeBL = new EmployeeWagesBL();
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


        // GET: EmployeeWages
        public ActionResult Index(int? page)
        {
            if (!CSubs.CheckPageRights(_PageID, PermissionType.VIEW))
            {
                return new HttpStatusCodeResult(403);
            }
            session.Set<string>("PageSession", "Employee Wages Master");
            //ViewData["CityList"] = cityBL.GetCityDropdown();
            var pageIndex = (page ?? 1);
            int pageSize = settings.GetSettingByKey<int>("pagination_pagesize");
            int totalCount = 10;
            string OrderBy = "CardEmpCode";
            int Order = 0;
            int? EmpID = null;
            string SearchKey = string.Empty;
            if (Request.Form["SearchKey"] != null && Request.Form["SearchKey"].Trim() != "")
            {
                SearchKey = Request.Form["SearchKey"];
            }
            if (Request.Params["grid-column"] != null && Request.Params["grid-column"].Trim() != "")
            {
                OrderBy = Request.Params["grid-column"].Trim().ToString();
            }
            if (Request.Params["grid-dir"] != null && Request.Params["grid-column"].Trim() != "")
            {
                Order = Convert.ToInt32(Request.Params["grid-dir"].Trim().ToString());
            }
            //if (Request.Form["EmpID"] != null && Request.Form["EmpID"].Trim() != "")
            //{
            //    EmpID = Convert.ToInt32(Request.Form["EmpID"]);
            //}
            var items = employeeBL.GetEmployeeList(pageIndex, pageSize, OrderBy, Order, EmpID, null, SearchKey,  out totalCount);
            if (totalCount == 0 && pageIndex > 1)
            {
                pageIndex = 1;
                items = employeeBL.GetEmployeeList(pageIndex, pageSize, OrderBy, Order, EmpID, null, SearchKey, out totalCount);
            }
            var itemsAsIPagedList = new StaticPagedList<EmployeeWages>(items, pageIndex, pageSize, totalCount);
            TempData["EmpList"] = itemsAsIPagedList;
            FillDropDown();
            return Request.IsAjaxRequest()
                ? (ActionResult)PartialView("_AjaxPartial")
                : View(new EmployeeWages());
        }

        private void FillDropDown()
        {
            ViewData["CityList"] = new List<SelectListItem>(); //comboBL.GetCityDropdown();
            ViewData["CompList"] = comboBL.GetCompanyDropdown();
            ViewData["DesignationList"] = comboBL.getDesignationDropdown();
        }

        // GET: EmployeeWages/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: EmployeeWages/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: EmployeeWages/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
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

        // GET: EmployeeWages/Edit/5
        public ActionResult Edit(int id)
        {
            EmployeeWages data = employeeBL.GetDetailById(id);
            
            ViewBag.Flag = "1";
            FillDropDown();
            
            return PartialView("Edit", data);
        }

        // POST: EmployeeWages/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EmployeeWages emp)
        {
            try
            {
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
                        result.Result = this.RenderPartialViewToString("Edit", emp);
                        return Json(result);
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, message);
                    }
                }

                //}
                return Request.IsAjaxRequest() ? (ActionResult)PartialView("Edit", emp) : View(emp);
            }
            catch
            {
                return View(emp);
            }
        }

        // GET: EmployeeWages/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: EmployeeWages/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
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
