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
    public class CommisionableCostHeadController : BaseController
    {
        private string _PageID = "5";

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

        private CostHeadCommisionableForRMCBL _forRMCBL;
        public CostHeadCommisionableForRMCBL forRMCBL
        {
            get
            {
                if (this._forRMCBL == null)
                    this._forRMCBL = new CostHeadCommisionableForRMCBL();
                return this._forRMCBL;

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


        // GET: CommisionableCostHead
        public ActionResult Index(int? page)
        {
            if (!CSubs.CheckPageRights(_PageID, PermissionType.VIEW))
            {
                return new HttpStatusCodeResult(403);
            }
            ViewBag.PageTitle = "Commisionable CostHead Master";
            session.Set<string>("PageSession", ViewBag.PageTitle);
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
            var items = forRMCBL.GetGridList(pageIndex, pageSize, OrderBy, Order,  null, SearchKey, UserSession.GetUserSession().LoginID, out totalCount);
            if (totalCount == 0 && pageIndex > 1)
            {
                pageIndex = 1;
                items = forRMCBL.GetGridList(pageIndex, pageSize, OrderBy, Order, null, SearchKey, UserSession.GetUserSession().LoginID, out totalCount);
            }
            var itemsAsIPagedList = new StaticPagedList<CostHeadCommisionableForRMC>(items, pageIndex, pageSize, totalCount);

            return Request.IsAjaxRequest()
                ? (ActionResult)PartialView("_AjaxPartial", itemsAsIPagedList)
                : View(itemsAsIPagedList);
        }

        // GET: CommisionableCostHead/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CommisionableCostHead/Create
        public ActionResult Create()
        {
            FillCombo();
            CostHeadCommisionableForRMC data = new CostHeadCommisionableForRMC();

            return Request.IsAjaxRequest()
                ? (ActionResult)PartialView("Create", data)
                : View(data);
        }

        // POST: CommisionableCostHead/Create
        [HttpPost]
        public ActionResult Create(CostHeadCommisionableForRMC data)
        {
            try
            {
                FillCombo();
                RELOCBS.AjaxHelper.AjaxResponse result = new AjaxResponse();
                if (ModelState.IsValid)
                {
                    string Message;
                    result.Success = forRMCBL.Insert(data, out Message);
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

        private void FillCombo()
        {
            ViewData["RMC"] = comboBL.GetRMCDropdown();
            ViewData["CostHead"] = comboBL.GetCostHeadDropdown();
        }

        // GET: CommisionableCostHead/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CommisionableCostHead/Edit/5
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Delete(int id)
        {
            try
            {

                AjaxResponse result = new AjaxResponse();
                string Message = string.Empty;

                if (ModelState.IsValid)
                {
                    result.Success = forRMCBL.DeleteById(id, out Message);

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
                    result.Result = "City delete failed. Please try again.";
                }

                return Json(result);
            }
            catch
            {
                return View();
            }
        }
    }
}
