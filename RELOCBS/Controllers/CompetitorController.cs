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
    public class CompetitorController : BaseController
    {

        private string _PageID = "15";

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

        private CompetitorBL _competitorBL;
        public CompetitorBL competitorBL
        {
            get
            {
                if (this._competitorBL == null)
                    this._competitorBL = new CompetitorBL();
                return this._competitorBL;

            }
        }

        // GET: Competitor
        public ActionResult Index(int? page)
        {
            if (!CSubs.CheckPageRights(_PageID, PermissionType.VIEW))
            {
                return new HttpStatusCodeResult(403);
            }
            ViewBag.PageTitle = "Competitor Master";
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
            var items = competitorBL.GetCompetitorList(pageIndex, pageSize, OrderBy, Order, null, CityID, null, null, SearchKey, UserSession.GetUserSession().LoginID, out totalCount);
            if (totalCount == 0 && pageIndex > 1)
            {
                pageIndex = 1;
                items = competitorBL.GetCompetitorList(pageIndex, pageSize, OrderBy, Order, null, CityID, null, null, SearchKey, UserSession.GetUserSession().LoginID, out totalCount);
            }

            var itemsAsIPagedList = new PagedList.StaticPagedList<Competitor>(items, pageIndex, pageSize, totalCount);

            return Request.IsAjaxRequest()
                ? (ActionResult)PartialView("_AjaxPartial", itemsAsIPagedList)
                : View(itemsAsIPagedList);
        }

        // GET: Competitor/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Competitor/Create
        public ActionResult Create()
        {
            FillCombo();
            CompetitorViewModel model = new CompetitorViewModel();
            model.Isactive = true;
            return Request.IsAjaxRequest()
                ? (ActionResult)PartialView("Create", model)
                : View(model);
        }

        // POST: Competitor/Create
        [HttpPost]
        public ActionResult Create(CompetitorViewModel data)
        {
            try
            {
                FillCombo();
                RELOCBS.AjaxHelper.AjaxResponse result = new AjaxResponse();
                if (ModelState.IsValid)
                {
                    string Message;
                    result.Success = competitorBL.Insert(data, out Message);
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

        // GET: Competitor/Edit/5
        public ActionResult Edit(int id)
        {
            if (id <= 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CompetitorViewModel data = competitorBL.GetDetailById(id);
            FillCombo();
            if (data == null)
            {
                return HttpNotFound();
            }
            return Request.IsAjaxRequest()
                ? (ActionResult)PartialView("Edit", data)
                : View(data);
        }

        // POST: Competitor/Edit/5
        [HttpPost]
        public ActionResult Edit(CompetitorViewModel data)
        {
            try
            {
                FillCombo();
                AjaxResponse result = new AjaxResponse();
                string message = string.Empty;
                if (ModelState.IsValid)
                {
                    result.Success = competitorBL.Update(data, out message);

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

        // GET: Competitor/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Competitor/Delete/5
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

        private void FillCombo()
        {

            int ContinentID = -1;
            ViewData["City"] = comboBL.GetCityDropdown(ContinentID: ContinentID, CountryID: -1);
            ViewData["Country"] = comboBL.GETCountryDropdown(ContinentID: ContinentID);
            ViewData["Company"] = comboBL.GetCompanyDropdown();
        }

        public JsonResult GetAutoPopulateList()
        {
            var lst = comboBL.GetCompetitorDropdown().Select(i => new { i.Value, i.Text }).ToList();
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
