using PagedList;
using RELOCBS.App_Code;
using RELOCBS.BL.Common;
using RELOCBS.BL;
using RELOCBS.Entities;
using RELOCBS.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RELOCBS.AjaxHelper;
using RELOCBS.Extensions;
using System.Net;

namespace RELOCBS.Controllers
{
    public class CompanyController : BaseController
    {
        private string _PageID = "7";

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

        private  ComboBL  _comboBL;
        public ComboBL comboBL
        {
            get
            {
                if (this._comboBL == null)
                    this._comboBL = new ComboBL();
                return this._comboBL;

            }
        }

        private CompanyBL _companyBL;
        public CompanyBL companyBL
        {
            get
            {
                if (this._companyBL == null)
                    this._companyBL = new CompanyBL();
                return this._companyBL;

            }
        }

        // GET: Company
        public ActionResult Index(int? page)
        {
            if (!CSubs.CheckPageRights(_PageID, PermissionType.VIEW))
            {
                return new HttpStatusCodeResult(403);
            }
            ViewBag.PageTitle = "Company Master";
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
            var items = companyBL.GetCompanyList(pageIndex, pageSize, OrderBy, Order, null, null, CityID, null, SearchKey, UserSession.GetUserSession().LoginID, out totalCount);
            if (totalCount == 0 && pageIndex > 1)
            {
                pageIndex = 1;
                items = companyBL.GetCompanyList(pageIndex, pageSize, OrderBy, Order,null ,null, CityID, null, SearchKey, UserSession.GetUserSession().LoginID, out totalCount);
            }

            var itemsAsIPagedList = new StaticPagedList<Company>(items, pageIndex, pageSize, totalCount);

            return Request.IsAjaxRequest()
                ? (ActionResult)PartialView("_AjaxPartial", itemsAsIPagedList)
                : View(itemsAsIPagedList);
        }

        // GET: Company/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Company/Create
        public ActionResult Create()
        {
            int ContinentID = -1;
            ViewData["City"] = comboBL.GetCityDropdown(ContinentID:ContinentID,CountryID:-1);
            ViewData["Country"] = comboBL.GETCountryDropdown(ContinentID: ContinentID);
            CityViewModel model = new CityViewModel();
            model.isActive = true;
            return Request.IsAjaxRequest()
                ? (ActionResult)PartialView("Create", model)
                : View(model);
        }

        // POST: Company/Create
        [HttpPost]
        public ActionResult Create(CompanyViewModel data)
        {
            try
            {
                ViewData["City"] = comboBL.GetCityDropdown(ContinentID: -1, CountryID: -1);
                ViewData["Country"] = comboBL.GETCountryDropdown(ContinentID: -1);
                RELOCBS.AjaxHelper.AjaxResponse result = new AjaxResponse();
                if (ModelState.IsValid)
                {
                    string Message;
                    result.Success = companyBL.Insert(data, out Message);
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

        // GET: Company/Edit/5
        public ActionResult Edit(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CompanyViewModel data = companyBL.GetDetailById(id);
            ViewData["City"] = comboBL.GetCityDropdown(ContinentID: -1, CountryID: -1);
            ViewData["Country"] = comboBL.GETCountryDropdown(ContinentID: -1);
            if (data == null)
            {
                return HttpNotFound();
            }
            return Request.IsAjaxRequest()
                ? (ActionResult)PartialView("Edit", data)
                : View(data);
        }

        // POST: Company/Edit/5
        [HttpPost]
        public ActionResult Edit(CompanyViewModel city)
        {
            try
            {
                ViewData["City"] = comboBL.GetCityDropdown(ContinentID: -1, CountryID: -1);
                ViewData["Country"] = comboBL.GETCountryDropdown(ContinentID: -1);
                AjaxResponse result = new AjaxResponse();
                string message = string.Empty;
                if (ModelState.IsValid)
                {
                    result.Success = companyBL.Update(city, out message);

                    if (result.Success)
                    {
                        result.Result = this.RenderPartialViewToString("Create", city);
                        return Json(result);
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, message);
                    }
                }
                return Request.IsAjaxRequest()
                  ? (ActionResult)PartialView("Edit", city)
                  : View(city);
            }
            catch
            {
                return View();
            }
        }

        // GET: Company/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Company/Delete/5
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
