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
    public class InsuranceMasterController : BaseController
    {
        private string _PageID = "23";

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

        private InsuranceMasterBL  _insuranceMasterBL;
        public InsuranceMasterBL insuranceMasterBL
        {
            get
            {
                if (this._insuranceMasterBL == null)
                    this._insuranceMasterBL = new InsuranceMasterBL();
                return this._insuranceMasterBL;

            }
        }

        // GET: InsuranceMaster
        public ActionResult Index(int? page)
        {
            if (!CSubs.CheckPageRights(_PageID, PermissionType.VIEW))
            {
                return new HttpStatusCodeResult(403);
            }
            session.Set<string>("PageSession", "Insurance Master");

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
            var items = insuranceMasterBL.GetInsuranceMasterList(pageIndex, pageSize, OrderBy, Order, null, CityID, null,null, SearchKey,  out totalCount);
            if (totalCount == 0 && pageIndex > 1)
            {
                pageIndex = 1;
                items = insuranceMasterBL.GetInsuranceMasterList(pageIndex, pageSize, OrderBy, Order, null, CityID, null,null, SearchKey, out totalCount);
            }

            var itemsAsIPagedList = new StaticPagedList<Insurance_Master>(items, pageIndex, pageSize, totalCount);

            return Request.IsAjaxRequest()
                ? (ActionResult)PartialView("_AjaxPartial", itemsAsIPagedList)
                : View(itemsAsIPagedList);
        }


        private void fillCombo()
        {
			bool RMCBuss = !(UserSession.GetUserSession().BussinessLine == "NON RMC-BUSINESS");
			ViewData["SuperviserList"] = comboBL.GetEmployeeDropdown();
            //ViewData["EmployeeList"] = comboBL.GetEmployeeDropdown();
            ViewData["ServiceLineList"] = comboBL.GetServiceLineDropdown(RMCBuss:RMCBuss);
            ViewData["InsuranceCompList"] = comboBL.GetInsuranceCompanyDropdown();
            
        }

        // GET: InsuranceMaster/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: InsuranceMaster/Create
        public ActionResult Create()
        {
            fillCombo();
            Insurance_Master model = new Insurance_Master();
            model.Ins_M_Id = -1;
            model.IsActive = true;
            return Request.IsAjaxRequest() ? (ActionResult)PartialView("Create", model) : View(model);
        }

        // POST: InsuranceMaster/Create
        [HttpPost]
        public ActionResult Create(Insurance_Master model)
        {
            try
            {
                fillCombo();
                RELOCBS.AjaxHelper.AjaxResponse result = new AjaxResponse();
                if (ModelState.IsValid)
                {
                    string Message;
                    result.Success = insuranceMasterBL.Insert(model, out Message);
                    if (result.Success)
                    {
                        result.Message = Message;
                        result.Result = this.RenderPartialViewToString("Create", model);
                        return Json(result);
                    }
                    else
                    {
                        result.Success = false;
                        result.Message = Message;
                        ModelState.AddModelError(string.Empty, Message);
                        result.Result = this.RenderPartialViewToString("Create", model);
                        return Json(result);
                    }
                }
                return Request.IsAjaxRequest() ? (ActionResult)PartialView("Create", model) : View(model);
            }
            catch
            {
                return View();
            }
        }

        // GET: InsuranceMaster/Edit/5
        public ActionResult Edit(int id)
        {
            if (id <= 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Insurance_Master data = insuranceMasterBL.GetDetailById(id);
            fillCombo();
            if (data == null)
            {
                return HttpNotFound();
            }
            return Request.IsAjaxRequest() ? (ActionResult)PartialView("Edit", data) : View(data);
        }

        // POST: InsuranceMaster/Edit/5
        [HttpPost]
        public ActionResult Edit(Insurance_Master model)
        {
            try
            {
                fillCombo();
                AjaxResponse result = new AjaxResponse();
                string message = string.Empty;
                if (ModelState.IsValid)
                {
                    result.Success = insuranceMasterBL.Update(model, out message);

                    if (result.Success)
                    {
                        result.Result = this.RenderPartialViewToString("Edit", model);
                        return Json(result);
                    }
                    else
                    {
                        result.Success = false;
                        result.Message = message;
                        ModelState.AddModelError(string.Empty, message);
                        result.Result = this.RenderPartialViewToString("Edit", model);
                        return Json(result);
                    }
                }

                return Request.IsAjaxRequest() ? (ActionResult)PartialView("Edit", model): View(model);
            }
            catch
            {
                return View();
            }
        }

        // GET: InsuranceMaster/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: InsuranceMaster/Delete/5
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
            var lst = new System.Collections.ArrayList() { "Test" };// comboBL.getCrewCodeDropDown().Select(i => new { i.Value, i.Text }).ToList();
            return Json(lst, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DocDownload(int id)
        {
            JobDocument jobDocument = insuranceMasterBL.GetDownloadFile(id);
            if (!string.IsNullOrWhiteSpace(jobDocument.FilePath) && System.IO.File.Exists(jobDocument.FilePath))
            {
                //var filepath = System.IO.Path.Combine(Server.MapPath("/Files/"), fileName);
                return File(jobDocument.FilePath, MimeMapping.GetMimeMapping(jobDocument.FilePath), jobDocument.FileName);
            }

            return new HttpStatusCodeResult(404);

        }
    }
}
