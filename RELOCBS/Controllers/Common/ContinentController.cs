using PagedList;
using RELOCBS.App_Code;
using RELOCBS.BL.Common;
using RELOCBS.CustomAttributes;
using RELOCBS.Entities;
using RELOCBS.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RELOCBS.Controllers.Common
{
    [AuthorizeUser]
    [Route("Common/Continent/{action=index}/{id?}")]
    public class ContinentController : BaseController
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

        private ContinentBL _continentBL;
        public ContinentBL continentBL
        {
            get
            {
                if (this._continentBL == null)
                    this._continentBL = new ContinentBL();
                return this._continentBL;

            }
        }


        // GET: Continent
        public ActionResult Index(int? page)
        {
            if (!CSubs.CheckPageRights(_PageID, PermissionType.VIEW))
            {
                return new HttpStatusCodeResult(403);
            }
            ViewBag.PageTitle = "Continent Master";
            ViewData["ContinentList"] = continentBL.GetContinentDropdown();
            var pageIndex = (page ?? 1);
            //var pageIndex = (page ?? 1) - 1;
            int pageSize = settings.GetSettingByKey<int>("pagination_pagesize");
            int totalCount = 10;
            string OrderBy = "";
            int Order = 0;
            int? ContinentID = null;
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
            
            var items = continentBL.GetContinentList(pageIndex, pageSize, OrderBy, Order, ContinentID, null, SearchKey, UserSession.GetUserSession().LoginID, out totalCount);
            if (totalCount == 0 && pageIndex > 1)
            {
                pageIndex = 1;
                items = continentBL.GetContinentList(pageIndex, pageSize, OrderBy, Order, ContinentID, null, SearchKey, UserSession.GetUserSession().LoginID, out totalCount);
            }
            var itemsAsIPagedList = new StaticPagedList<Continent>(items, pageIndex, pageSize, totalCount);

            return Request.IsAjaxRequest()
                ? (ActionResult)PartialView("_AjaxPartial", itemsAsIPagedList)
                : View(itemsAsIPagedList);
        }

        // GET: Continent/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Continent/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Continent/Create
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

        // GET: Continent/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Continent/Edit/5
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

        // GET: Continent/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Continent/Delete/5
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
