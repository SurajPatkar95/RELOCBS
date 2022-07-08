using PagedList;
using RELOCBS.AjaxHelper;
using RELOCBS.App_Code;
using RELOCBS.BL;
using RELOCBS.BL.Pricing;
using RELOCBS.CustomAttributes;
using RELOCBS.Entities;
using RELOCBS.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RELOCBS.Controllers
{
    [AuthorizeUser]
    public class GeneralPricingController : BaseController
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

        private GeneralPricingBL _generalPricingBL;

        public GeneralPricingBL generalPricingBL
        {

            get
            {
                if (this._generalPricingBL == null)
                    this._generalPricingBL = new GeneralPricingBL();
                return this._generalPricingBL;
            }
        }

        // GET: GeneralPricing
        public ActionResult Index(int RateComponetID, int? page=1)
        {
            if (!CSubs.CheckPageRights(_PageID, PermissionType.VIEW))
            {
                return new HttpStatusCodeResult(403);
            }

            session.Set<string>("PageSession", "Pricing");
            var pageIndex = (page ?? 1);
            int pageSize = settings.GetSettingByKey<int>("pagination_pagesize");
            int totalCount = 10;
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
            //if (Request.Form["CityID"] != null && Request.Form["CityID"].Trim() != "")
            //{
            //    CityID = Convert.ToInt32(Request.Form["CityID"]);
            //}
            var items = generalPricingBL.GetForGrid(UserSession.GetUserSession().LoginID,RateComponetID);
            if (totalCount == 0 && pageIndex > 1)
            {
                pageIndex = 1;
                items = generalPricingBL.GetForGrid(UserSession.GetUserSession().LoginID, RateComponetID);
            }
            var itemsAsIPagedList = new StaticPagedList<GeneralPrice>(items, pageIndex, pageSize, totalCount);

            string FromLoc = string.Empty;
            string ToLoc = string.Empty;
            generalPricingBL.GetRateComponentLable(RateComponetID, out FromLoc, out ToLoc);

            ViewBag.FromLocLable = FromLoc;
            ViewBag.ToLocLable = FromLoc;

            return Request.IsAjaxRequest()
                ? (ActionResult)PartialView("_AjaxPartial", itemsAsIPagedList)
                : View(itemsAsIPagedList);
        }

        // GET: GeneralPricing/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: GeneralPricing/Create
        public ActionResult Create()
        {
            ViewData["RMC"]= comboBL.GetRMCDropdown();
            ViewData["BusinessLine"]= comboBL.GetBusinessLineDropdown();
            ViewData["GoodsDescription"] = comboBL.GetGoodsDescriptionDropdown();
            ViewData["Mode"]= comboBL.GetModeDropdown();
            ViewData["RateComponent"] = comboBL.GetRateComponentDropdown();
            ViewData["Agent"] = comboBL.GetRateAgentDropdown();
            ViewData["FromLocation"] = comboBL.GetFromLocationDropdown();
            ViewData["ToLocation"] = comboBL.GetToLocationDropdown();
            ViewData["RateCurrency"] = comboBL.GetRateCurrencyDropdown();
            ViewData["BaseCurrencyRate"] = comboBL.GetBaseCurrencyRateDropdown();
            ViewData["WeightUnit"] = comboBL.GetMeasurementUnitDropdown('W');


            GeneralPriceViewModel priceViewModel = new GeneralPriceViewModel();
            priceViewModel.CostHeadList = new List<CostHead>();


            return Request.IsAjaxRequest()
            ? (ActionResult)PartialView("Create", priceViewModel)
            : View(priceViewModel);
        }

        // POST: GeneralPricing/Create
        [HttpPost]
        public ActionResult Create(GeneralPriceViewModel PricingData)
        {
            AjaxResponse result = new AjaxResponse();
            string message = string.Empty;

            if (ModelState.IsValid)
            {
                bool res = false;
                res = generalPricingBL.InsertRate(PricingData,UserSession.GetUserSession().LoginID, out message);
                if (!res)
                {
                    result.Success = false;
                    ModelState.AddModelError(string.Empty, "Unable to save pricing data.");
                    return Json(result);
                }
                else
                {
                    result.Success = true;
                    return Json(result);
                }
            }
            else
            {
                return Request.IsAjaxRequest() ? (ActionResult)PartialView("ImportRateUpload")
              : View();
            }
        }

        // GET: GeneralPricing/Edit/5
        public ActionResult Edit(int RateCompRateWtBatchID,int RateCompRateWtID)
        {
            ViewData["RMC"] = comboBL.GetRMCDropdown();
            ViewData["BusinessLine"] = comboBL.GetBusinessLineDropdown();
            ViewData["GoodsDescription"] = comboBL.GetGoodsDescriptionDropdown();
            ViewData["Mode"] = comboBL.GetModeDropdown();
            ViewData["RateComponent"] = comboBL.GetRateComponentDropdown();
            ViewData["Agent"] = comboBL.GetRateAgentDropdown();
            ViewData["FromLocation"] = comboBL.GetFromLocationDropdown();
            ViewData["ToLocation"] = comboBL.GetToLocationDropdown();
            ViewData["RateCurrency"] = comboBL.GetRateCurrencyDropdown();
            ViewData["BaseCurrencyRate"] = comboBL.GetBaseCurrencyRateDropdown();
            ViewData["WeightUnit"] = comboBL.GetMeasurementUnitDropdown('W');


            GeneralPriceViewModel priceViewModel = new GeneralPriceViewModel();
            priceViewModel.CostHeadList = new List<CostHead>();

            generalPricingBL.GetDetailById(RateCompRateWtBatchID, RateCompRateWtID);

            return Request.IsAjaxRequest()
            ? (ActionResult)PartialView("Edit", priceViewModel)
            : View(priceViewModel);
        }

        // POST: GeneralPricing/Edit/5
        [HttpPost]
        public ActionResult Edit(GeneralPriceViewModel PricingData)
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

        // GET: GeneralPricing/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: GeneralPricing/Delete/5
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


        public ActionResult CopyRate(int RateCompRateWtBatchID, int RateCompRateWtID)
        {
            ViewData["RMC"] = comboBL.GetRMCDropdown();
            ViewData["BusinessLine"] = comboBL.GetBusinessLineDropdown();
            ViewData["GoodsDescription"] = comboBL.GetGoodsDescriptionDropdown();
            ViewData["Mode"] = comboBL.GetModeDropdown();
            ViewData["RateComponent"] = comboBL.GetRateComponentDropdown();
            ViewData["Agent"] = comboBL.GetRateAgentDropdown();
            ViewData["FromLocation"] = comboBL.GetFromLocationDropdown();
            ViewData["ToLocation"] = comboBL.GetToLocationDropdown();
            ViewData["RateCurrency"] = comboBL.GetRateCurrencyDropdown();
            ViewData["BaseCurrencyRate"] = comboBL.GetBaseCurrencyRateDropdown();
            ViewData["WeightUnit"] = comboBL.GetMeasurementUnitDropdown('W');


            GeneralPriceViewModel priceViewModel = new GeneralPriceViewModel();
            priceViewModel.CostHeadList = new List<CostHead>();

            generalPricingBL.GetDetailById(RateCompRateWtBatchID, RateCompRateWtID);

            return Request.IsAjaxRequest()
            ? (ActionResult)PartialView("Edit", priceViewModel)
            : View(priceViewModel);
        }

        [HttpPost]
        public ActionResult CopyRate(GeneralPriceViewModel PricingData)
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

        public JsonResult GetRateComponetLabel(int RateComponentID)
        {
            RateComponentLabelViewModel Modelobj = new RateComponentLabelViewModel();
            string FromLoc=string.Empty;
            string ToLoc = string.Empty;
           generalPricingBL.GetRateComponentLable(RateComponentID, out FromLoc, out ToLoc);
            Modelobj.FromLocationLable = FromLoc;
            Modelobj.ToLocationLable = ToLoc;

            if (RateComponentID==1)
            {
                
                Modelobj.FromLocationDropDown = comboBL.GetCityDropdown();
                Modelobj.ToLocationDropDown = comboBL.GetPortDropdown(-1);
            }
            else if(RateComponentID == 2)
            {
                
                Modelobj.FromLocationDropDown = comboBL.GetPortDropdown(-1);
                Modelobj.ToLocationDropDown = comboBL.GetPortDropdown(-1);
            }
            else if (RateComponentID == 3)
            {
                
                Modelobj.FromLocationDropDown = comboBL.GetPortDropdown(-1);
                Modelobj.ToLocationDropDown = comboBL.GetCityDropdown();
            }
            else if (RateComponentID == 4)
            {
                
                Modelobj.FromLocationDropDown = comboBL.GetCityDropdown();
                Modelobj.ToLocationDropDown = comboBL.GetCityDropdown();
            }
            
            return Json(Modelobj, JsonRequestBehavior.AllowGet);

        }
    }
}
