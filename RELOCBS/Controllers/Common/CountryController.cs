using PagedList;
using RELOCBS.App_Code;
using RELOCBS.BL.Common;
using RELOCBS.CustomAttributes;
using RELOCBS.Entities;
using RELOCBS.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RELOCBS.Controllers.Common
{
    [AuthorizeUser]
    [Route("Common/Country/{action=index}/{id?}")]
    public class CountryController : BaseController
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

        private CountryBL _countryBL;
        public CountryBL countryBL
        {
            get
            {
                if (this._countryBL == null)
                    this._countryBL = new CountryBL();
                return this._countryBL;

            }
        }


        // GET: Country
        [Route("Common/Country")]
        public ActionResult Index(int? page)
        {
            if (!CSubs.CheckPageRights(_PageID, PermissionType.VIEW))
            {
                return new HttpStatusCodeResult(403);
            }
            ViewBag.PageTitle = "Country Master";
            ViewData["CountryList"] = countryBL.GetCountryDropdown();
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
            var items = countryBL.GetCountryList(pageIndex, pageSize, OrderBy, Order, CityID, null, SearchKey, UserSession.GetUserSession().LoginID, out totalCount);
            if (totalCount == 0 && pageIndex > 1)
            {
                pageIndex = 1;
                items = countryBL.GetCountryList(pageIndex, pageSize, OrderBy, Order, CityID, null, SearchKey, UserSession.GetUserSession().LoginID, out totalCount);
            }
            var itemsAsIPagedList = new StaticPagedList<Country>(items, pageIndex, pageSize, totalCount);

            return Request.IsAjaxRequest()
                ? (ActionResult)PartialView("_AjaxPartial", itemsAsIPagedList)
                : View(itemsAsIPagedList);
        }

        // GET: Country/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Country/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Country/Create
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

        // GET: Country/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Country/Edit/5
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

        // POST: Country/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        [Route("Country/GetAutoPopulateList")]
        public JsonResult GetAutoPopulateList()
        {
            var lst = countryBL.GetCountryDropdown().Select(i => new { i.Value, i.Text }).ToList();
            return Json(lst, JsonRequestBehavior.AllowGet);
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
