using PagedList;
using RELOCBS.AjaxHelper;
using RELOCBS.App_Code;
using RELOCBS.BL;
using RELOCBS.BL.Common;
using RELOCBS.Extensions;
using RELOCBS.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RELOCBS.Entities;
using System.Net;
using RELOCBS.CustomAttributes;

namespace RELOCBS.Controllers
{
	[AuthorizeUser]
	public class VendorEvolQuestionController : BaseController
    {
        private string _PageID = "50";

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

        private VendorEvolQuestionBL _vendorBL;
        public VendorEvolQuestionBL vendorBL
        {
            get
            {
                if (this._vendorBL == null)
                    this._vendorBL = new VendorEvolQuestionBL();
                return this._vendorBL;

            }
        }
        // GET: VendorEvolQuestion
        public ActionResult Index(int? page)
        {
            if (!CSubs.CheckPageRights(_PageID, PermissionType.VIEW))
            {
                return new HttpStatusCodeResult(403);
            }
            session.Set<string>("PageSession", "Agent Evaluation Question Master");
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
            var items = vendorBL.GetList(pageIndex, pageSize, OrderBy, Order, null, SearchKey, UserSession.GetUserSession().LoginID, out totalCount);
            if (totalCount == 0 && pageIndex > 1)
            {
                pageIndex = 1;
                items = vendorBL.GetList(pageIndex, pageSize, OrderBy, Order, null, SearchKey, UserSession.GetUserSession().LoginID, out totalCount);
            }

            var itemsAsIPagedList = new StaticPagedList<JobVendorEvalQuestion>(items, pageIndex, pageSize, totalCount);

            return Request.IsAjaxRequest()
                ? (ActionResult)PartialView("_AjaxPartial", itemsAsIPagedList)
                : View(itemsAsIPagedList);
        }

        // GET: VendorEvolQuestion/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: VendorEvolQuestion/Create
        public ActionResult Create()
        {
            ViewData["RateComp"] = comboBL.GetRateComponentDropdown();
            JobVendorEvalQuestion model = new JobVendorEvalQuestion();
            model.CompanyID=UserSession.GetUserSession().CompanyID;
            model.IsRMCBuss=UserSession.GetUserSession().BussinessLine != "NON RMC-BUSINESS";
            model.IsActive = true;
            model.Weightage = 1;
            return Request.IsAjaxRequest()
                ? (ActionResult)PartialView("Create", model)
                : View(model);
        }

        // POST: VendorEvolQuestion/Create
        [HttpPost]
        public ActionResult Create(JobVendorEvalQuestion data)
        {
            try
            {
                ViewData["RateComp"] = comboBL.GetRateComponentDropdown();
                RELOCBS.AjaxHelper.AjaxResponse result = new AjaxResponse();
                if (ModelState.ContainsKey("Answer"))
                    ModelState["Answer"].Errors.Clear();
                if (ModelState.IsValid)
                {
                    string Message;
                    result.Success = vendorBL.Insert(data, out Message);
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
                return View(data);
            }
        }

        // GET: VendorEvolQuestion/Edit/5
        public ActionResult Edit(int id)
        {
            if (id <= 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JobVendorEvalQuestion data = vendorBL.GetDetailById(id);
            ViewData["RateComp"] = comboBL.GetRateComponentDropdown();
            if (data == null)
            {
                return HttpNotFound();
            }
            return Request.IsAjaxRequest()
                ? (ActionResult)PartialView("Edit", data)
                : View(data);
        }

        // POST: VendorEvolQuestion/Edit/5
        [HttpPost]
        public ActionResult Edit(JobVendorEvalQuestion data)
        {
            try
            {
                ViewData["RateComp"] = comboBL.GetRateComponentDropdown();
                AjaxResponse result = new AjaxResponse();
                string message = string.Empty;
                if (ModelState.ContainsKey("Answer"))
                    ModelState["Answer"].Errors.Clear();
                if (ModelState.IsValid)
                {
                    result.Success = vendorBL.Update(data, out message);

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
                return View(data);
            }
        }

        // GET: VendorEvolQuestion/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: VendorEvolQuestion/Delete/5
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
            var lst = comboBL.GetVendorDropdown().Select(i => new { i.Value, i.Text }).ToList();
            return Json(lst, JsonRequestBehavior.AllowGet);
        }
    }
}
