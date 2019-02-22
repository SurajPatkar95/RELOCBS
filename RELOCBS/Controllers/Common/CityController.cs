using PagedList;
using RELOCBS.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using RELOCBS.App_Code;
using RELOCBS.Utility;
using RELOCBS.BL.Common;
using RELOCBS.CustomAttributes;
using RELOCBS.Extensions;
using RELOCBS.AjaxHelper;
using System.Net;

namespace RELOCBS.Controllers.Common
{
    [AuthorizeUser]
    [Route("Common/City/{action=index}/{id?}")]
    public class CityController : BaseController
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

        private CityBL _cityBL;
        public CityBL cityBL
        {
            get
            {
                if (this._cityBL == null)
                    this._cityBL = new CityBL();
                return this._cityBL;

            }
        }


        // GET: City
        //[Route("Common/City")]
        public ActionResult Index(int? page)
        {
            if (!CSubs.CheckPageRights(_PageID, PermissionType.VIEW))
            {
                return new HttpStatusCodeResult(403);
            }
            ViewBag.PageTitle = "City Master";
            ViewData["CityList"] = cityBL.GetCityDropdown();
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
            var items = cityBL.GetCityList(pageIndex, pageSize, OrderBy, Order, CityID, null, SearchKey, UserSession.GetUserSession().LoginID, out totalCount);
            if (totalCount == 0 && pageIndex > 1)
            {
                pageIndex = 1;
                items = cityBL.GetCityList(pageIndex, pageSize, OrderBy, Order, CityID, null, SearchKey, UserSession.GetUserSession().LoginID, out totalCount);
            }
            var itemsAsIPagedList = new StaticPagedList<City>(items, pageIndex, pageSize, totalCount);

            return Request.IsAjaxRequest()
                ? (ActionResult)PartialView("_AjaxPartial", itemsAsIPagedList)
                : View(itemsAsIPagedList);
        }

        // GET: City/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: City/Create
        public ActionResult Create()
        {
            string ContinentID = string.Empty;
            ViewData["Country"] = cityBL.GetCountryByContinent(ContinentID);
            CityViewModel model = new CityViewModel();
            model.isActive = true;
            return Request.IsAjaxRequest()
                ? (ActionResult)PartialView("Create", model)
                : View(model);
        }

        // POST: City/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create(CityViewModel data)
        {
            try
            {

                //ViewData["StateList"] = cityBL.BindDropdown("StatesByCountryId", "", "");
                ViewData["Country"] = cityBL.GetCountryByContinent("");
                AjaxResponse result = new AjaxResponse();
                if (ModelState.IsValid)
                {
                    string Message;
                    result.Success = cityBL.Insert(data,out Message);
                    if (result.Success)
                    {
                        ViewData["Country"] = cityBL.GetCountryByContinent("");
                        result.Message = Message;
                        result.Result = this.RenderPartialViewToString("Create", data);
                        return Json(result);
                    }
                    else
                    {
                        ViewData["Country"] = cityBL.GetCountryByContinent("");
                        ModelState.AddModelError(string.Empty, Message);
                    }
                }
                return Request.IsAjaxRequest()
                              ? (ActionResult)PartialView("Create", data)
                              : View(data);


            }
            catch( Exception ex)
            {
                return View();
            }
        }

        // GET: City/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CityViewModel data = cityBL.GetDetailById(id);
            ViewData["Country"] = cityBL.GetCountryByContinent("");
            if (data == null)
            {
                return HttpNotFound();
            }
            return Request.IsAjaxRequest()
                ? (ActionResult)PartialView("Edit", data)
                : View(data);

            
        }

        // POST: City/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(CityViewModel city)
        {
            try
            {
                ViewData["Country"] = cityBL.GetCountryByContinent("");
                AjaxResponse result = new AjaxResponse();
                string message = string.Empty;
                if (ModelState.IsValid)
                {
                    result.Success = cityBL.Update(city,out message);

                    if (result.Success)
                    {
                        ViewData["Country"] = cityBL.GetCountryByContinent("");
                        result.Result = this.RenderPartialViewToString("Create", city);
                        return Json(result);
                    }
                    else
                    {
                        ViewData["Country"] = cityBL.GetCountryByContinent("");
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
        

        // POST: City/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Delete(int id)
        {
            try
            {

                AjaxResponse result = new AjaxResponse();
                string Message=string.Empty;

                if (ModelState.IsValid)
                {
                    result.Success = cityBL.DeleteById(id,out Message);

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

        [Route("City/GetAutoPopulateList")]
        public JsonResult GetAutoPopulateList()
        {
            var lst = cityBL.GetCityDropdown().Select(i => new { i.Value, i.Text }).ToList();
            return Json(lst, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ExportToExcel()
        {
            DataTable dtUser = new DataTable();
            try
            {
                GenerateExcel("City", "usp_City_ExpToExl", dtUser);
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
                DataTable dtgridData = new DataTable(); //_spService.GetExportToExcelData(exptoExlParameters, spName);
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
    }
}
