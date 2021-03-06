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

namespace RELOCBS.Controllers
{
    [AuthorizeUser]
    public class WarehouseController : BaseController
    {
        private string _PageID = "39";

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

        private WarehouseBL _warehouseBL;
        public WarehouseBL warehouseBL
        {
            get
            {
                if (this._warehouseBL == null)
                    this._warehouseBL = new WarehouseBL();
                return this._warehouseBL;

            }
        }

        private void FillCombo()
        {

            int ContinentID = -1;
            ViewData["City"] = comboBL.GetCityDropdown(ContinentID: ContinentID, CountryID: -1);
            ViewData["Country"] = comboBL.GETCountryDropdown(ContinentID: ContinentID);
            ViewData["Company"] = comboBL.GetCompanyDropdown();
            ViewData["Branch"] = comboBL.GetCompanyBranchDropdown();
        }

        // GET: Warehouse
        public ActionResult Index(int? page)
        {
            if (!CSubs.CheckPageRights(_PageID, PermissionType.VIEW))
            {
                return new HttpStatusCodeResult(403);
            }
            session.Set<string>("PageSession", "Warehouse Master");
            //ViewData["CityList"] = comboBL.GetCityDropdown();
            var pageIndex = (page ?? 1);
            int pageSize = settings.GetSettingByKey<int>("pagination_pagesize");
            int totalCount = 10;
            string OrderBy = "";
            int Order = 0;
            int? CityID = null;
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
            //if (Request.Form["CityID"] != null && Request.Form["CityID"].Trim() != "")
            //{
            //    CityID = Convert.ToInt32(Request.Form["CityID"]);
            //}
            var items = warehouseBL.GetWarehouseBrachList(pageIndex, pageSize, OrderBy, Order, null, CityID, UserSession.GetUserSession().CompanyID, null, SearchKey, UserSession.GetUserSession().LoginID, out totalCount);
            if (totalCount == 0 && pageIndex > 1)
            {
                pageIndex = 1;
                items = warehouseBL.GetWarehouseBrachList(pageIndex, pageSize, OrderBy, Order, null, CityID, UserSession.GetUserSession().CompanyID, null, SearchKey, UserSession.GetUserSession().LoginID, out totalCount);
            }

            var itemsAsIPagedList = new StaticPagedList<Warehouse>(items, pageIndex, pageSize, totalCount);

            return Request.IsAjaxRequest()
                ? (ActionResult)PartialView("_AjaxPartial", itemsAsIPagedList)
                : View(itemsAsIPagedList);
        }

        // GET: Warehouse/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Warehouse/Create
        public ActionResult Create()
        {
            FillCombo();
            Warehouse model = new Warehouse();
            model.IsActive = true;
            return Request.IsAjaxRequest()
                ? (ActionResult)PartialView("Create", model)
                : View(model);
        }

        // POST: Warehouse/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Warehouse data)
        {
            try
            {
                FillCombo();
                RELOCBS.AjaxHelper.AjaxResponse result = new AjaxResponse();
                if (ModelState.IsValid)
                {
                    string Message;
                    result.Success = warehouseBL.Insert(data, out Message);
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
                return Request.IsAjaxRequest()
                              ? (ActionResult)PartialView("Create", data)
                              : View(data);


            }
            catch
            {
                return View();
            }
        }

        // GET: Warehouse/Edit/5
        public ActionResult Edit(int id)
        {
            if (id <= 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Warehouse data = warehouseBL.GetDetailById(id);
            FillCombo();
            if (data == null)
            {
                return HttpNotFound();
            }
            return Request.IsAjaxRequest()
                ? (ActionResult)PartialView("Edit", data)
                : View(data);
        }

        // POST: Warehouse/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Warehouse data)
        {
            try
            {
                FillCombo();
                AjaxResponse result = new AjaxResponse();
                string message = string.Empty;
                if (ModelState.IsValid)
                {
                    result.Success = warehouseBL.Update(data, out message);

                    if (result.Success)
                    {
                        result.Result = this.RenderPartialViewToString("Create", data);
                        return Json(result);
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, message);
                    }
                }
                return Request.IsAjaxRequest()
                  ? (ActionResult)PartialView("Edit", data)
                  : View(data);
            }
            catch
            {
                return View();
            }
        }

        // GET: Warehouse/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Warehouse/Delete/5
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

        public JsonResult GetAutoPopulateList()
        {
            var lst = comboBL.GetCompanyBranchDropdown().Select(i => new { i.Value, i.Text }).ToList();
            return Json(lst, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ExportToExcel()
        {
            Dictionary<string, string> param = new Dictionary<string, string>();
            try
            {

                string SearchKey = string.Empty;
                if (Request.Form["SearchKey"] != null && Request.Form["SearchKey"].Trim() != "")
                {
                    param.Add("@SP_SearchString", Request.Form["SearchKey"]);
                }

                param.Add("@SP_LoginID", Convert.ToString(UserSession.GetUserSession().LoginID));

                CommonService.GenerateExcel(this.Response, "Warehouse", "[Comm].[GETWarehouseForGrid_ExpToExl]", param);
            }
            catch (Exception ex)
            {
                this.AddToastMessage("RELOCBS", "UnExpected Error occured", ToastType.Error);
            }
            return View();
        }
    }
}
