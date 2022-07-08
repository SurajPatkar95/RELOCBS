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
    public class CostHeadController : BaseController
    {

        private string _PageID = "55";

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

        private CostHeadBL _costHeadBL;
        public CostHeadBL costHeadBL
        {
            get
            {
                if (this._costHeadBL == null)
                    this._costHeadBL = new CostHeadBL();
                return this._costHeadBL;

            }
        }

        // GET: CostHead
        public ActionResult Index(int? page)
        {
            if (!CSubs.CheckPageRights(_PageID, PermissionType.VIEW))
            {
                return new HttpStatusCodeResult(403);
            }
            ViewBag.PageTitle = "CostHead Master";
            session.Set<string>("PageSession", ViewBag.PageTitle);
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
            var items = costHeadBL.GetList(pageIndex, pageSize, OrderBy, Order, SearchKey, UserSession.GetUserSession().LoginID, out totalCount);
            if (totalCount == 0 && pageIndex > 1)
            {
                pageIndex = 1;
                items = costHeadBL.GetList(pageIndex, pageSize, OrderBy, Order, SearchKey, UserSession.GetUserSession().LoginID, out totalCount);
            }

            var itemsAsIPagedList = new PagedList.StaticPagedList<CostHeadMaster>(items, pageIndex, pageSize, totalCount);

            return Request.IsAjaxRequest()
                ? (ActionResult)PartialView("_AjaxPartial", itemsAsIPagedList)
                : View(itemsAsIPagedList);
        }

        // GET: CostHead/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CostHead/Create
        public ActionResult Create()
        {
            
            CostHeadMaster model = new CostHeadMaster();
            model.IsActive = true;
            return Request.IsAjaxRequest()
                ? (ActionResult)PartialView("Create", model)
                : View(model);
        }

        // POST: CostHead/Create
        [HttpPost]
        public ActionResult Create(CostHeadMaster data)
        {
            try
            {
                RELOCBS.AjaxHelper.AjaxResponse result = new AjaxResponse();
                if (ModelState.IsValid)
                {
                    string Message;
                    result.Success = costHeadBL.Insert(data, out Message);
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

        // GET: CostHead/Edit/5
        public ActionResult Edit(int id)
        {
            if (id < 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CostHeadMaster data = costHeadBL.GetDetailById(id);
            
            if (data == null)
            {
                return HttpNotFound();
            }
            return Request.IsAjaxRequest()
                ? (ActionResult)PartialView("Edit", data)
                : View(data);
        }

        // POST: CostHead/Edit/5
        [HttpPost]
        public ActionResult Edit(CostHeadMaster data)
        {
            try
            {
                AjaxResponse result = new AjaxResponse();
                string message = string.Empty;
                if (ModelState.IsValid)
                {
                    result.Success = costHeadBL.Update(data, out message);

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

        // GET: CostHead/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CostHead/Delete/5
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
            var lst = comboBL.GetCostHeadDropdown().Select(i => new { i.Value, i.Text }).ToList();
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

                CommonService.GenerateExcel(this.Response, "Competitor", "[Comm].[GETCompetitorForGrid_ExpToExl]", param);

            }
            catch (Exception ex)
            {
                this.AddToastMessage("RELOCBS", "UnExpected Error occured", ToastType.Error);
            }
            return View();
        }
    }
}
