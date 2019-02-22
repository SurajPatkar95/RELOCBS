using RELOCBS.AjaxHelper;
using RELOCBS.CustomAttributes;
using RELOCBS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RELOCBS.Extensions;
using System.Net;
using System.Web.UI;
using System.IO;
using System.Data;
using System.Web.UI.WebControls;
using RELOCBS.App_Code;
using RELOCBS.BL.Common;
using PagedList;
using RELOCBS.Utility;
using RELOCBS.BL;

namespace RELOCBS.Controllers
{
    [AuthorizeUser]
    public class LaneController : BaseController
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

        private LaneBL _laneBL;
        public LaneBL laneBL
        {
            get
            {
                if (this._laneBL == null)
                    this._laneBL = new LaneBL();
                return this._laneBL;

            }
        }


        private ComboBL _comobBL;

        public ComboBL comboBL
        {
            get
            {

                if (_comobBL == null)
                {
                    _comobBL = new ComboBL();
                }

                return _comobBL;

            }
        }

        // GET: Lane
        public ActionResult Index(int? page)
        {
            if (!CSubs.CheckPageRights(_PageID, PermissionType.VIEW))
            {
                return new HttpStatusCodeResult(403);
            }
            FillDropdown();
            var pageIndex = (page ?? 1);
            int pageSize = settings.GetSettingByKey<int>("pagination_pagesize");
            int totalCount = 10;
            int? OriginCountryID = null;
            int? DestinationCountryID = null;
            int? OriginCityID = null;
            int? DestinationCityID = null;
            ViewBag.PageTitle = "Lane Master";
            string OrderBy = "";
            int Order = 0;
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

            var items = laneBL.GetLaneList(pageIndex, pageSize, OrderBy, Order, OriginCountryID, DestinationCountryID,null,SearchKey, UserSession.GetUserSession().LoginID, out totalCount);
            if (totalCount == 0 && pageIndex > 1)
            {
                pageIndex = 1;
                items = laneBL.GetLaneList(pageIndex, pageSize, OrderBy, Order, OriginCountryID, DestinationCountryID, null, SearchKey, UserSession.GetUserSession().LoginID, out totalCount);
            }
            var itemsAsIPagedList = new StaticPagedList<Lane>(items, pageIndex, pageSize, totalCount);

            return Request.IsAjaxRequest()
                ? (ActionResult)PartialView("_AjaxPartial", itemsAsIPagedList)
                : View(itemsAsIPagedList);
        }

        // GET: Lane/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Lane/Create
        public ActionResult Create()
        {
            FillDropdown();
            LaneViewModel model = new LaneViewModel();
            model.isActive = true;
            return Request.IsAjaxRequest()
                ? (ActionResult)PartialView("Create", model)
                : View(model);
        }

        // POST: Lane/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create(LaneViewModel lane)
        {
            FillDropdown();
            AjaxResponse result = new AjaxResponse();
            if (ModelState.IsValid)
            {
                string Message;
                bool i = laneBL.Insert(lane,out Message);
                result.Message = Message;
                if (i)
                {
                    result.Success = true;
                    result.Result = this.RenderPartialViewToString("Create", lane);
                    
                    return Json(result);
                }
                else
                {
                        result.Success = false;
                        ModelState.AddModelError(string.Empty, result.Message);
                        ViewData["ORGINCITYList1"] = comboBL.GetCityDropdown("ALLACTIVE");
                }
            }
            return Request.IsAjaxRequest()
                          ? (ActionResult)PartialView("Create", lane)
                          : View(lane);
        }

        // GET: Lane/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FillDropdown();
            LaneViewModel lane = laneBL.GetDetailById(id);
            //ViewData["OriginStateList"] = _spService.BindDropdown("StatesByCountryId", lane.OriginCountryID.ToString(), lane.OriginStateID.ToString());
            ViewData["ORGINCITYList"] = comboBL.GetCityDropdown("ALL",CountryID: lane.OriginCountryID, CityID: lane.OriginCityID);
            //ViewData["DestinationStateList"] = _spService.BindDropdown("StatesByCountryId", lane.DestinationCountryID.ToString(), lane.DestinationStateID.ToString());
            ViewData["DestinationCITYList"] = comboBL.GetCityDropdown("ALL",CountryID:lane.DestinationCountryID,CityID:lane.DestinationCityID);
            if (lane == null)
            {
                return HttpNotFound();
            }
            return Request.IsAjaxRequest()
                ? (ActionResult)PartialView("Edit", lane)
                : View(lane);
        }

        // POST: Lane/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(int id, LaneViewModel lane)
        {
            FillDropdown();
            AjaxResponse result = new AjaxResponse();
            if (ModelState.IsValid)
            {
                string Message=string.Empty;
                result.Success = laneBL.Update(lane,out Message);
                if (result.Success)
                {
                    result.Message = Message;
                    result.Result = this.RenderPartialViewToString("Create", lane);
                    return Json(result);
                }
                else
                {
                    
                        ModelState.AddModelError(string.Empty, Message);
                        //ViewData["DestinationStateList"] = _spService.BindDropdown("StatesByCountryId", "1", "1");
                        ViewData["DestinationCITYList"] = comboBL.GetCityDropdown("ALL", CountryID: -1, CityID: -1);

                }
            }
            return Request.IsAjaxRequest()
              ? (ActionResult)PartialView("Edit", lane)
              : View(lane);
        }

        // GET: Lane/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Delete(int id)
        {
            AjaxResponse result = new AjaxResponse();
            result.Success = false;

            if (ModelState.IsValid)
            {
                string Message=string.Empty;
                result.Success = laneBL.DeleteById(id, out Message);
                result.Result = Message;
                
            }
            return Json(result);
        }

        public void FillDropdown()
        {
            ViewData["ORGINCITYList"] = comboBL.GetCityDropdown("ALL");
            ViewData["OriginCountryList"] = comboBL.GETCountryDropdown("ALL");
            ViewData["DestinationCountryList"] = comboBL.GETCountryDropdown("ALL");
        }

        #region Excel
        public ActionResult ExportToExcel()
        {
            DataTable dtUser = new DataTable();
            try
            {
                GenerateExcel("Lane", "usp_Lane_ExpToExl", dtUser);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        public void GenerateExcel(string excelName, string spName, DataTable exptoExlParameters)
        {
            try
            {
                var gv = new GridView();
                DataTable dtgridData = new DataTable(); //laneBL.GetExportToExcelData(exptoExlParameters, spName);
                if (dtgridData.Rows.Count > 0)
                {
                    gv.DataSource = dtgridData;
                }
                else
                {
                    dtgridData = new DataTable();
                    dtgridData.Columns.Add("Message", typeof(string));
                    dtgridData.Rows.Add("There are no items to display! ");
                    gv.DataSource = dtgridData;
                }
                gv.DataBind();

                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment; filename=" + excelName + ".xls");
                Response.ContentType = "application/ms-excel";

                Response.Charset = "";
                System.IO.StringWriter objStringWriter = new StringWriter();
                HtmlTextWriter objHtmlTextWriter = new HtmlTextWriter(objStringWriter);
                gv.RenderControl(objHtmlTextWriter);
                Response.Output.Write(objStringWriter.ToString());
                Response.Flush();
                Response.End();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
