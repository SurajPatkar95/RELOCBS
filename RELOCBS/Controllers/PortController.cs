using PagedList;
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
    public class PortController : BaseController
    {
        private string _PageID = "53";

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

        private PortBL _portBL;
        public PortBL portBL
        {
            get
            {
                if (this._portBL == null)
                    this._portBL = new PortBL();
                return this._portBL;

            }
        }



        // GET: Port
        public ActionResult Index(int? page, string Type)
        {
            if (Type == "S")
            {
                _PageID = "54";
            }

            if (!CSubs.CheckPageRights(_PageID, PermissionType.VIEW))
            {
                return new HttpStatusCodeResult(403);
            }
            string Title = Type == "A" ? "Air Port" : "Sea Port";
            int ModeID;
            if (Type == "A")
            {
                ModeID = 2;
            }
            else
            {
                ModeID = 1;
            }
            session.Set<string>("PageSession", Title);
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
            
            var items = portBL.GetPortList(pageIndex, pageSize, OrderBy, Order, CityID, Type, ModeID, SearchKey, UserSession.GetUserSession().LoginID, out totalCount);
            if (totalCount == 0 && pageIndex > 1)
            {
                pageIndex = 1;
                items = portBL.GetPortList(pageIndex, pageSize, OrderBy, Order, CityID, Type,ModeID, SearchKey, UserSession.GetUserSession().LoginID, out totalCount);
            }

            var itemsAsIPagedList = new StaticPagedList<Port>(items, pageIndex, pageSize, totalCount);
            ViewData["Type"] = Type;
            return Request.IsAjaxRequest()
                ? (ActionResult)PartialView("_AjaxPartial", itemsAsIPagedList)
                : View(itemsAsIPagedList);
        }

        // GET: Port/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Port/Create
        public ActionResult Create(string Type)
        {
            Port model = new Port();
            model.Isactive = true;
            model.AirorSea = Type;
            ViewData["Type"] = model.AirorSea;
            if (Type == "A")
            {
                model.ModeID = 2;
            }
            else
            {
                model.ModeID = 1;
            }
            FillCombo();
            return Request.IsAjaxRequest()
                ? (ActionResult)PartialView("Create", model)
                : View(model);
        }

        // POST: Port/Create
        [HttpPost]
        public ActionResult Create(Port data)
        {
            try
            {
                FillCombo();
                RELOCBS.AjaxHelper.AjaxResponse result = new AjaxResponse();
                ViewData["Type"] = data.AirorSea;
                if (ModelState.IsValid)
                {
                    string Message;
                    result.Success = portBL.Insert(data, out Message);
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

        // GET: Port/Edit/5
        public ActionResult Edit(int id)
        {
            if (id <= 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Port data = portBL.GetDetailById(id);
            FillCombo();
            ViewData["Type"] = data.AirorSea;
            if (data == null)
            {
                return HttpNotFound();
            }
            return Request.IsAjaxRequest()
                ? (ActionResult)PartialView("Edit", data)
                : View(data);
        }

        // POST: Port/Edit/5
        [HttpPost]
        public ActionResult Edit(Port data)
        {
            try
            {
                FillCombo();
                ViewData["Type"] = data.AirorSea;
                AjaxResponse result = new AjaxResponse();
                string message = string.Empty;
                if (ModelState.IsValid)
                {
                    result.Success = portBL.Update(data, out message);

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


        private void FillCombo()
        {
            ViewData["City"] = new List<SelectListItem>(); //comboBL.GetCityDropdown(ContinentID: -1, CountryID: -1);
            ViewData["Country"] = comboBL.GETCountryDropdown(ContinentID: -1);
        }

        // GET: Port/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Port/Delete/5
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

        public JsonResult GetAutoPopulateList(string SeaOrAir)
        {
            var lst = comboBL.GetPortDropdown(SeaOrAir: SeaOrAir).Select(i => new { i.Value, i.Text }).ToList();
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
