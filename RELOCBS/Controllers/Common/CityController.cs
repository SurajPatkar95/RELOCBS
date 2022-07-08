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
using RELOCBS.Common;
using RELOCBS.BL;

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
            ViewData["State"] = new List<SelectListItem>();
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
                ViewData["State"] = comboBL.GetStateDropdown(SPTYPE: "ALLACTIVE", CountryID: data.CountryID);
                AjaxResponse result = new AjaxResponse();
                if (ModelState.IsValid)
                {
                    string Message;
                    result.Success = cityBL.Insert(data,out Message);
                    if (result.Success)
                    {
                        ViewData["Country"] = cityBL.GetCountryByContinent("");
                        ViewData["State"] = comboBL.GetStateDropdown(SPTYPE: "ALLACTIVE", CountryID: data.CountryID);
                        result.Message = Message;
                        result.Result = this.RenderPartialViewToString("Create", data);
                        return Json(result);
                    }
                    else
                    {
                        ViewData["Country"] = cityBL.GetCountryByContinent("");
                        ViewData["State"] = comboBL.GetStateDropdown(SPTYPE: "ALLACTIVE", CountryID: data.CountryID);
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
            ViewData["State"] = comboBL.GetStateDropdown(SPTYPE: "ALLACTIVE", CountryID: data.CountryID);
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
                ViewData["State"] = comboBL.GetStateDropdown(SPTYPE: "ALLACTIVE", CountryID:city.CountryID);
                AjaxResponse result = new AjaxResponse();
                string message = string.Empty;
                if (ModelState.IsValid)
                {
                    result.Success = cityBL.Update(city,out message);

                    if (result.Success)
                    {
                        ViewData["Country"] = cityBL.GetCountryByContinent("");
                        ViewData["State"] = comboBL.GetStateDropdown(SPTYPE: "ALLACTIVE", CountryID: city.CountryID);
                        result.Result = this.RenderPartialViewToString("Create", city);
                        return Json(result);
                    }
                    else
                    {
                        ViewData["Country"] = cityBL.GetCountryByContinent("");
                        ViewData["State"] = comboBL.GetStateDropdown(SPTYPE: "ALLACTIVE", CountryID: city.CountryID);
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
            Dictionary<string, string> param = new Dictionary<string, string>();
            try
            {

                string SearchKey = string.Empty;
                if (Request.Form["SearchKey"] != null && Request.Form["SearchKey"].Trim() != "")
                {
                    param.Add("@SP_SearchString", Request.Form["SearchKey"]);
                }

                param.Add("@SP_LoginID", Convert.ToString(UserSession.GetUserSession().LoginID));

                CommonService.GenerateExcel(this.Response,"City", "[Comm].[GETCityForGrid_ExpToExl]", param);
            }
            catch (Exception ex)
            {
                this.AddToastMessage("RELOCBS", "UnExpected Error occured", ToastType.Error);
            }
            return View();
        }

        public JsonResult GetState(int CountryID)
        {
            var data = comboBL.GetStateDropdown(SPTYPE: "ALLACTIVE", CountryID: CountryID) ;
            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }
}
