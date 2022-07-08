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
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RELOCBS.Controllers
{
    [AuthorizeUser]
    public class CurrencyConversionController : BaseController
    {
        private string _PageID = "56";

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

        private CurrencyConversionBL _conversionBL;
        public CurrencyConversionBL conversionBL
        {
            get
            {
                if (this._conversionBL == null)
                    this._conversionBL = new CurrencyConversionBL();
                return this._conversionBL;

            }
        }

        // GET: CurrencyConversion
        public ActionResult Index(int? page)
        {
            if (!CSubs.CheckPageRights(_PageID, PermissionType.VIEW))
            {
                return new HttpStatusCodeResult(403);
            }
            ViewBag.PageTitle = "Upload Currency Conversion";
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
            var items = conversionBL.GetList(pageIndex, pageSize, OrderBy, Order, null, null, null, SearchKey, UserSession.GetUserSession().LoginID, out totalCount);
            if (totalCount == 0 && pageIndex > 1)
            {
                pageIndex = 1;
                items = conversionBL.GetList(pageIndex, pageSize, OrderBy, Order, null, null, null, SearchKey, UserSession.GetUserSession().LoginID, out totalCount);
            }

            var itemsAsIPagedList = new PagedList.StaticPagedList<CurrencyConversion>(items, pageIndex, pageSize, totalCount);

            return Request.IsAjaxRequest()
                ? (ActionResult)PartialView("_AjaxPartial", itemsAsIPagedList)
                : View(itemsAsIPagedList);
        }

        // GET: CurrencyConversion/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CurrencyConversion/Create
        public ActionResult Create()
        {
            return Request.IsAjaxRequest()
                ? (ActionResult)PartialView("Create")
                : View();
        }

        // POST: CurrencyConversion/Create
        [HttpPost]
        public ActionResult Create(HttpPostedFileBase FileUpload)
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

        public ActionResult Importexcel()
        {
            return Request.IsAjaxRequest()
                ? (ActionResult)PartialView("Importexcel")
                : View();
        }

        [HttpPost]
        public ActionResult Importexcel(HttpPostedFileBase FileUpload)
        {
            RELOCBS.AjaxHelper.AjaxResponse result = new AjaxResponse();
            string Message;
            try
            {
                if (ModelState.IsValid)
                {
                    if (FileUpload != null && FileUpload.ContentLength > 0)
                    {
                        string extension = System.IO.Path.GetExtension(FileUpload.FileName).ToLower();
                        string path = Server.MapPath("~/uploads");
                        string path1 = Path.Combine(path, FileUpload.FileName);
                        string[] validFileTypes = { ".xls", ".xlsx", ".csv" };
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                        if (validFileTypes.Contains(extension))
                        {
                            result.Success = conversionBL.Insert(FileUpload, path1, out Message);
                            if (result.Success)
                            {
                                result.Message = Message;
                                result.Result = this.RenderPartialViewToString("Importexcel");
                                return Json(result);
                            }
                            else
                            {
                                ModelState.AddModelError(string.Empty, Message);
                            }
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "Please Upload Files in .xls, .xlsx or .csv format");
                        }
                    }
                }
                return Request.IsAjaxRequest()
                              ? (ActionResult)PartialView("Importexcel")
                              : View();

            }
            catch (Exception)
            {

                return Request.IsAjaxRequest()
                              ? (ActionResult)PartialView("Importexcel")
                              : View();
            }
            
        }

        // GET: CurrencyConversion/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CurrencyConversion/Edit/5
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

        // GET: CurrencyConversion/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CurrencyConversion/Delete/5
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

        public ActionResult DownloadExcel()
        {
            byte[] fileBytes = RELOCBS.Properties.Resources.CurrencyConversionFormat;
            //byte[] fileBytes = (byte[])RELOCBS.Properties.Resources.ResourceManager.GetObject(format.ResourceName, Properties.Resources.Culture);

            string fileName = "CurrencyConversionFormat.xlsx";
            if (fileBytes == null || !fileBytes.Any())
                 return new HttpStatusCodeResult(404);
            
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }
    }
}
