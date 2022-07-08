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
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace RELOCBS.Controllers
{
    [AuthorizeUser]
    public class CrewController : BaseController
    {
        private string _PageID = "42";

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

        // GET: Company
        public ActionResult Index(int? page)
        {
            if (!CSubs.CheckPageRights(_PageID, PermissionType.VIEW))
            {
                return new HttpStatusCodeResult(403);
            }
            session.Set<string>("PageSession", "Crew Master");
            
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
            var items = crewBL.GetCrewList(pageIndex, pageSize, OrderBy, Order, null, CityID, null, SearchKey, out totalCount);
            if (totalCount == 0 && pageIndex > 1)
            {
                pageIndex = 1;
                items = crewBL.GetCrewList(pageIndex, pageSize, OrderBy, Order, null, CityID, null, SearchKey, out totalCount);
            }

            var itemsAsIPagedList = new StaticPagedList<Crew>(items, pageIndex, pageSize, totalCount);

            return Request.IsAjaxRequest()
                ? (ActionResult)PartialView("_AjaxPartial", itemsAsIPagedList)
                : View(itemsAsIPagedList);
        }

        // GET: Company/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }


        private void fillCombo()
        {
			bool RMCBuss = !(UserSession.GetUserSession().BussinessLine == "NON RMC-BUSINESS");
			ViewData["SuperviserList"] = comboBL.GetEmployeeDropdown(SPTYPE: "SUPERVISOR");
            ViewData["EmployeeList"] = comboBL.GetEmployeeDropdown(SPTYPE:"CREWMASTER");
            ViewData["ServiceLineList"] = comboBL.GetServiceLineDropdown(RMCBuss:RMCBuss);
        }

        // GET: Company/Create
        public ActionResult Create()
        {

            fillCombo();
            Crew model = new Crew();
            model.CrewID = -1;
            model.IsActive = true;
            return Request.IsAjaxRequest()
                ? (ActionResult)PartialView("Create", model)
                : View(model);
        }

        // POST: Company/Create
        [HttpPost]
        public ActionResult Create(Crew data)
        {
            try
            {
                fillCombo();
                RELOCBS.AjaxHelper.AjaxResponse result = new AjaxResponse();
                if (ModelState.IsValid)
                {
                    string Message;
                    result.Success = crewBL.Insert(data, out Message);
                    if (result.Success)
                    {
                        result.Message = Message;
                        result.Result = this.RenderPartialViewToString("Create", data);
                        return Json(result);
                    }
                    else
                    {
                        result.Success = false;
                        result.Message = Message;
                        ModelState.AddModelError(string.Empty, Message);
                        result.Result = this.RenderPartialViewToString("Create", data);
                        return Json(result);
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
            if (id <= 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Crew data = crewBL.GetDetailById(id);
            fillCombo();
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
        public ActionResult Edit(Crew data)
        {
            try
            {
                fillCombo();
                AjaxResponse result = new AjaxResponse();
                string message = string.Empty;
                if (ModelState.IsValid)
                {
                    result.Success = crewBL.Update(data, out message);

                    if (result.Success)
                    {
                        result.Result = this.RenderPartialViewToString("Edit", data);
                        return Json(result);
                    }
                    else
                    {
                        result.Success = false;
                        result.Message = message;
                        ModelState.AddModelError(string.Empty, message);
                        result.Result = this.RenderPartialViewToString("Edit", data);
                        return Json(result);
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

        // GET: Crew/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Crew/Delete/5
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

        
        public JsonResult GetAutoPopulateList()
        {
            var lst = new System.Collections.ArrayList(){ "Test"};// comboBL.getCrewCodeDropDown().Select(i => new { i.Value, i.Text }).ToList();
            return Json(lst, JsonRequestBehavior.AllowGet);
        }
    }
}
