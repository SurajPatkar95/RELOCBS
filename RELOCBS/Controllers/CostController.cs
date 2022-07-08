using Newtonsoft.Json;
using PagedList;
using RELOCBS.AjaxHelper;
using RELOCBS.App_Code;
using RELOCBS.BL;
using RELOCBS.BL.Enquiry;
using RELOCBS.BL.GMMS;
using RELOCBS.BL.Pricing;
using RELOCBS.BL.Survey;
using RELOCBS.Common;
using RELOCBS.CustomAttributes;
using RELOCBS.Entities;
using RELOCBS.Extensions;
using RELOCBS.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;

namespace RELOCBS.Controllers
{
    [AuthorizeUser]
    public class CostController : BaseController
    {
        private string _PageID = "45";

        public CostController()
        {
            _PageID = "45";
        }

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

        private ClientDetailBL _clientDetailBL;
        public ClientDetailBL ClientDetailBL
        {
            get
            {
                if (this._clientDetailBL == null)
                    this._clientDetailBL = new ClientDetailBL();
                return this._clientDetailBL;
            }
        }

        private CommanBL _commonBL;

        public CommanBL commonBL
        {
            get
            {
                if (this._commonBL == null)
                    this._commonBL = new CommanBL();
                return this._commonBL;
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

        private CostBL _costBL;

        public CostBL costBL
        {

            get
            {
                if (this._costBL == null)
                    this._costBL = new CostBL();
                return this._costBL;
            }
        }


        private CostUploadBL _costUploadBL;

        public CostUploadBL costUploadBL
        {

            get
            {
                if (this._costUploadBL == null)
                    this._costUploadBL = new CostUploadBL();
                return this._costUploadBL;
            }
        }

        // GET: Cost
        public ActionResult Index(int page = 1)
        {
            _PageID = "45";

            if (!CSubs.CheckPageRights(_PageID, PermissionType.VIEW))
            {
                return new HttpStatusCodeResult(403);
            }

            session.Set<string>("PageSession", "Pricing");
            string sort = "";
            string sortdir = "";
            string search = "";
            string searchType = "";

            int pageSize = settings.GetSettingByKey<int>("pagination_pagesize");
            string OrderBy = "";
            int Order = 0;
            DateTime? Fromdate = null;
            DateTime? Todate = null;
            string Shipper = "";
            string SearchKey = string.Empty;
            if (Request.Form["FromDate"] != null && Request.Form["FromDate"].Trim() != "")
            {
                Fromdate = Convert.ToDateTime(Request.Form["FromDate"]);
            }

            if (Request.Form["ToDate"] != null && Request.Form["ToDate"].Trim() != "")
            {
                Todate = Convert.ToDateTime(Request.Form["ToDate"]);
            }

            if (Request.Form["search"] != null && Request.Form["search"].Trim() != "")
            {
                search = Convert.ToString(Request.Form["search"]);
            }

            if (Request.Form["SearchType"] != null && Request.Form["SearchType"].Trim() != "")
            {
                searchType = Convert.ToString(Request.Form["SearchType"]);
            }


            if (Request.Params["grid-column"] != null && Request.Params["grid-column"].Trim() != "")
            {
                sort = Request.Params["grid-column"].Trim().ToString();
            }
            if (Request.Params["grid-dir"] != null && Request.Params["grid-dir"].Trim() != "")
            {
                Order = Convert.ToInt32(Request.Params["grid-dir"].Trim().ToString());

                if (Order == 1)
                {
                    sortdir = "asc";
                }
                else
                {
                    sortdir = "desc";
                }

            }
            if (Request.Params["Shipper"] != null && Request.Params["Shipper"].Trim() != "")
            {
                Shipper = Request.Params["Shipper"].Trim().ToString();
            }

            int totalRecord = 0;
            if (page < 1) page = 1;
            int skip = (page * pageSize) - pageSize;
            var surveyBL = new SurveyBL();
            var data = surveyBL.GetsurveyList(Fromdate, Todate, Shipper, searchType, search, sort, sortdir, skip, pageSize, out totalRecord);
            ViewBag.TotalRows = totalRecord;
            ViewBag.search = search;

            var itemsAsIPagedList = new StaticPagedList<SurveyData>(data, page, pageSize, totalRecord);

            return Request.IsAjaxRequest()
                ? (ActionResult)PartialView("_AjaxPartial", itemsAsIPagedList)
                : View(itemsAsIPagedList);
        }

        // GET: Cost/Details/5
        public ActionResult Details(int surveyid, int Wtid, int Batchid)
        {
            CostViewModel costViewModel = new CostViewModel();
            costViewModel = costBL.GetDetailById(surveyid, Wtid, Batchid);

            FillCombo(costViewModel.ModeID.ToString(), ServiceLineID: costViewModel.ServiceLineID);

            return Request.IsAjaxRequest()
            ? (ActionResult)PartialView("Details", costViewModel)
            : View(costViewModel);
        }

        private void FillCombo(string ModeID, int RMCID = -1, int? ServiceLineID = 0)
        {
            bool RMCBuss = !(UserSession.GetUserSession().BussinessLine == "NON RMC-BUSINESS");

            ViewData["RMC"] = comboBL.GetRMCDropdown();
            ViewData["BusinessLine"] = comboBL.GetBusinessLineDropdown();
            ViewData["GoodsDescription"] = comboBL.GetGoodsDescriptionDropdown();
            ViewData["Mode"] = comboBL.GetModeDropdown(ServiceLineID: Convert.ToInt32(ServiceLineID));
            ViewData["RateComponent"] = comboBL.GetRateComponentDropdown();
            ViewData["Agent"] = comboBL.GetAgentDropdown(CORA: "A");
            //ViewData["FromLocation"] = comboBL.GetFromLocationDropdown();
            //ViewData["ToLocation"] = comboBL.GetToLocationDropdown();
            ViewData["RateCurrency"] = comboBL.GetRateCurrencyDropdown();
            ViewData["BaseCurrencyRate"] = comboBL.GetBaseCurrencyRateDropdown();
            ViewData["WeightUnit"] = comboBL.GetMeasurementUnitDropdown(UnitType: 'B');
            var stgflag = ModeID == "4" ? "For Strg" : null;
            stgflag = ModeID == "5" ? "For DSP" : null;
            ViewData["CostHeadList"] = comboBL.GetCostHeadDropdown(RMCID, ForCombo: stgflag);

            ViewData["CityList"] = comboBL.GetCityDropdown();
            ViewData["PortList"] = comboBL.GetPortDropdown(SeaOrAir: (ModeID == "1" ? "S" : "A"));
            ViewData["SeaPortList"] = comboBL.GetPortDropdown(SeaOrAir: "S");
            ViewData["AirPortList"] = comboBL.GetPortDropdown(SeaOrAir: "A");
            ViewData["ShippingLineList"] = comboBL.GetShippingLineDropdown(ModeID);

            ViewData["CountryList"] = comboBL.GETCountryDropdown();
            ViewData["YESNO"] = CommonService.YesNo;
            ViewData["ServiceLine"] = comboBL.GetServiceLineDropdown(RMCBuss: RMCBuss);
            ViewData["Continent"] = comboBL.GETContinentDropdown();

            ViewData["EmpTypeList"] = comboBL.GetServiceEmpType();
            ViewData["CurrencyList"] = comboBL.GetCurrencyDropdown();
        }

        private void CostUploadCombo()
        {
            List<SelectListItem> selectListItems = comboBL.GetBusinessLineDropdown().ToList();
            var selected = selectListItems.Where(x => x.Text.ToUpper() == "GMMS").First();
            selected.Selected = true;
            ViewData["BusinessLine"] = new SelectList(selectListItems, "Value", "Text", selected).AsEnumerable<SelectListItem>();

            selectListItems = comboBL.GetGoodsDescriptionDropdown().ToList();
            selected = selectListItems.Where(x => x.Text.ToUpper() == "HOUSEHOLD GOODS").First();
            selected.Selected = true;
            ViewData["GoodsDescription"] = (new SelectList(selectListItems, "Value", "Text", selected)).AsEnumerable<SelectListItem>();

        }

        // GET: Cost/Create
        public ActionResult Create(int? SurveyID = 0, int? RateCompRateWtID = 0, int? RateCompRateBatchID = 0)
        {
            _PageID = "45";
            session.Set<string>("PageSession", "Estimation");
            CostViewModel costViewModel = new CostViewModel();
            String ModeID = "-1";
            int RMCID = -1;

            if (SurveyID > 0)
            {
                costViewModel = costBL.GetDetailById(SurveyID, RateCompRateWtID, (int)RateCompRateBatchID);

                costViewModel.BaseCurrencyRateID = commonBL.GetBaseCurrByRMC(!(UserSession.GetUserSession().BussinessLine == "NON RMC-BUSINESS"), costViewModel.RMCID, UserSession.GetUserSession().CompanyID);

                ModeID = Convert.ToString(costViewModel.ModeID);
                if (RateCompRateBatchID > 0)
                {
                    costViewModel.RateCompRateBatchId = 0;
                    costViewModel.RateCompRateWtID = 0;
                }
                else
                {
                    if (RateCompRateWtID <= 0)
                    {
                        costViewModel.RMCID = 3;
                        RMCID = costViewModel.RMCID;
                    }
                }
                costViewModel.RMCID = 1;

                costViewModel.RateCurrencyID = costViewModel.BaseCurrencyRateID;

                costViewModel.ConversionRate = 1;
                costViewModel.CompanyID = UserSession.GetUserSession().CompanyID;
                TempData["CostHeadList"] = costViewModel.CostHeadList;
                TempData["SubCostList"] = null;
                string SubCostDiv = string.Empty;
                foreach (var item in costViewModel.CostHeadList)
                {
                    item.IsSubCost = new BL.CommanBL().IsSubCostHead(item.CostHeadID);
                    SubCostDiv += GetSubCostHeadList(item.CostHeadID, item.RateComponentID, Convert.ToInt32(costViewModel.SurveyID),
                        Convert.ToInt32(costViewModel.RateCompRateWtID), costViewModel.RateCompRateBatchId, 1, 1);
                }
                ViewBag.SubCostList = SubCostDiv;
            }
            FillCombo(ModeID, RMCID, costViewModel.ServiceLineID);

            return Request.IsAjaxRequest()
            ? (ActionResult)PartialView("Create", costViewModel)
            : View(costViewModel);
        }

        public JsonResult GetaJAXClientDetails(int ClientId, char Mode)
        {
            return Json(new { result = ClientDetailBL.GetClientDetail(ClientId, Mode) }, JsonRequestBehavior.AllowGet);
        }
        // POST: Cost/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CostViewModel CostData, int? SurveyID = 0, int? RateCompRateWtID = 0)
        {
            try
            {
                TempData.Keep();
                AjaxResponse result = new AjaxResponse();
                string message = string.Empty;
                FillCombo(CostData.ModeID.ToString(), ServiceLineID: CostData.ServiceLineID);
                //if (CostData.EntryPointID == null)
                //{
                //	ModelState.AddModelError("EntryPointID","Entry port is mandatory");
                //}
                if (CostData.ExitPointID == null && CostData.Project == "EXP" && CostData.ModeID != 3)
                {
                    if (!(CostData.ModeID == 4 || CostData.ModeID == 5))
                    {
                        ModelState.AddModelError("ExitPointID", "Exit port is mandatory");
                        CostData.ExitPointID = 0;
                    }

                }
                if (CostData.ExitPointID == null)
                {
                    CostData.ExitPointID = 0;
                }
                //if (CostData.EntrytPointID == null)
                //{
                //	CostData.ExitPointID = 0;
                //}
                if (CostData.ModeID == 3 || CostData.ModeID == 4 || CostData.ModeID == 5)
                {
                    ModelState.Remove("EntryPointID");
                    //ModelState.Remove("ExitPointID");
                    CostData.ExitPointID = 0;
                    CostData.EntryPointID = 0;
                }
                //else if (CostData.Project != "EXP")
                //{
                //	ModelState.Remove("ExitPointID");
                //	//CostData.ExitPointID 
                //}

                if (ModelState.IsValid)
                {

                    if (CostData.HFVCostList.containsHtmlTags())
                    {
                        ModelState.AddModelError("", "Invalid Cost Heads");
                        this.AddToastMessage("RELOCBS", "Invalid Cost Heads", ToastType.Error);
                        return Request.IsAjaxRequest() ? (ActionResult)PartialView("Create", CostData)
                        : View(CostData);
                    }

                    bool res = false;
                    CostData.RateCompRateWtID = RateCompRateWtID;
                    res = costBL.InsertCost(CostData, UserSession.GetUserSession().LoginID, out message);
                    if (!res)
                    {
                        result.Success = false;
                        ModelState.AddModelError(string.Empty, "Unable to save estimate data.");
                        result.Message = message;

                        this.AddToastMessage("RELOCBS", message, ToastType.Error);
                        //return Json(result);
                    }
                    else
                    {
                        result.Success = true;
                        result.Message = message;

                        this.AddToastMessage("RELOCBS", message, ToastType.Success);
                        //return Json(result);
                    }

                    //ViewBag.Result = result;
                    return RedirectToAction("Create", new { SurveyID = CostData.SurveyID, RateCompRateWtID = CostData.RateCompRateWtID });
                }
                else
                {
                    CostData = GetCostModel(CostData.SurveyID, CostData.RateCompRateWtID, CostData.RateCompRateBatchId);
                    return Request.IsAjaxRequest() ? (ActionResult)PartialView("Create", CostData)
                  : View(CostData);
                }
            }
            catch
            {
                return View(CostData);
            }
        }

        // GET: Cost/Edit/5
        public ActionResult Edit(int? SurveyID = 0, int? RateCompRateWtID = 0, int RateCompRateBatchId = 0)
        {
            CostViewModel costViewModel = new CostViewModel();

            costViewModel = costBL.GetDetailById(SurveyID, RateCompRateWtID, RateCompRateBatchId);
            costViewModel.RMCID = 1;
            FillCombo(costViewModel.ModeID.ToString(), costViewModel.RMCID, costViewModel.ServiceLineID);
            costViewModel.RateCompRateBatchId = RateCompRateBatchId;
            //
            TempData["SubCostList"] = null;
            string SubCostDiv = string.Empty;
            foreach (var item in costViewModel.CostHeadList)
            {
                item.IsSubCost = new BL.CommanBL().IsSubCostHead(item.CostHeadID);
                SubCostDiv += GetSubCostHeadList(item.CostHeadID, item.RateComponentID, Convert.ToInt32(costViewModel.SurveyID),
                    Convert.ToInt32(RateCompRateWtID), costViewModel.RateCompRateBatchId, 1, 1);
            }
            ViewBag.SubCostList = SubCostDiv;
            TempData["CostHeadList"] = costViewModel.CostHeadList;
            costViewModel.ConversionRate = 1;
            costViewModel.CompanyID = UserSession.GetUserSession().CompanyID;
            return Request.IsAjaxRequest()
            ? (ActionResult)PartialView("Edit", costViewModel)
            : View(costViewModel);
        }

        // POST: Cost/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CostViewModel CostData, int RateCompRateWtID, int? SurveyID = 0, int RateCompRateBatchId = 0)
        {
            try
            {
                TempData.Keep();
                AjaxResponse result = new AjaxResponse();
                string message = string.Empty;
                FillCombo(CostData.ModeID.ToString(), ServiceLineID: CostData.ServiceLineID);

                //if (CostData.EntryPointID == null)
                //{
                //	ModelState.AddModelError("EntryPointID", "Entry port is mandatory");
                //}
                //if (CostData.ExitPointID == null && CostData.Project == "EXP")
                //{
                //	ModelState.AddModelError("ExitPointID", "Exit port is mandatory");
                //}


                if (ModelState.IsValid)
                {
                    if (CostData.HFVCostList.containsHtmlTags())
                    {
                        ModelState.AddModelError("", "Invalid Cost Heads");
                        this.AddToastMessage("RELOCBS", "Invalid Cost Heads", ToastType.Error);
                        return Request.IsAjaxRequest() ? (ActionResult)PartialView("Create", CostData)
                        : View(CostData);
                    }

                    bool res = false;
                    res = costBL.InsertCost(CostData, UserSession.GetUserSession().LoginID, out message);
                    if (!res)
                    {
                        result.Success = false;
                        ModelState.AddModelError(string.Empty, "Unable to save estimate data.");
                        result.Message = message;

                        this.AddToastMessage("RELOCBS", message, ToastType.Error);
                        //return Json(result);
                        return Request.IsAjaxRequest() ? (ActionResult)PartialView("Edit", CostData)
                        : View(CostData);
                    }
                    else
                    {
                        result.Success = true;
                        result.Message = message;

                        this.AddToastMessage("RELOCBS", message, ToastType.Success);
                        //return Json(result);
                        return RedirectToAction("Create", new { SurveyID = CostData.SurveyID, RateCompRateWtID = CostData.RateCompRateWtID });
                    }
                }
                else
                {
                    //CostData = GetEditCostModel(CostData.SurveyID, CostData.RateCompRateWtID, CostData.RateCompRateBatchId);
                    //this.AddToastMessage("RELOCBS", "Unable to save estimate data.", ToastType.Error);
                    return Request.IsAjaxRequest() ? (ActionResult)PartialView("Edit", CostData)
                  : View(CostData);
                }
            }
            catch
            {

                return View(CostData);
            }
        }


        // POST: Cost/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[MultipleButton(Name = "action", Argument = "Delete")]
        public ActionResult Delete(int SurveyID, int RateCompRateWtID, int RateCompRateBatchID)
        {
            try
            {
                // TODO: Add delete logic here

                AjaxResponse result = new AjaxResponse();
                string message = string.Empty;
                bool res = false;
                res = costBL.Delete(SurveyID, RateCompRateWtID, RateCompRateBatchID, UserSession.GetUserSession().LoginID, out message);
                if (!res)
                {
                    result.Success = false;
                    ModelState.AddModelError(string.Empty, "Unable to Delete Estimate data.");
                    result.Message = message;

                    this.AddToastMessage("RELOCBS", message, ToastType.Error);
                    //return Json(result);
                }
                else
                {
                    result.Success = true;
                    result.Message = message;

                    this.AddToastMessage("RELOCBS", message, ToastType.Success);
                    //return Json(result);
                }

                //ViewBag.Result = result;
                return RedirectToAction("Create", new { SurveyID = SurveyID, RateCompRateWtID = RateCompRateWtID });

            }
            catch
            {
                return RedirectToAction("Create", new { SurveyID = SurveyID, RateCompRateWtID = RateCompRateWtID });
            }
        }

        public ActionResult CopyRate(int? param1 = 0, int? param2 = 0, int? param3 = 0)
        {

            ///if (true)
            {

                int? SurveyID = param1;
                int? RateCompRateWtID = param2;
                int RateCompRateBatchID = Convert.ToInt32(param3);
                CostViewModel priceViewModel = new CostViewModel();
                priceViewModel = costBL.GetDetailById(SurveyID, RateCompRateWtID, RateCompRateBatchID);
                //FillCombo(priceViewModel.ModeID.ToString());

                return PartialView("_PartialCopyRateGrid", priceViewModel.CostHeadList);
                //return Json(priceViewModel, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public ActionResult CopyRate(CostViewModel PricingData)
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

        [HttpPost]
        public JsonResult GetShippingLineList(int mode)
        {
            ViewData["ShippingLineList"] = comboBL.GetShippingLineDropdown(Convert.ToString(mode));

            var lstItem = ((IEnumerable<SelectListItem>)ViewData["ShippingLineList"]).Select(i => new { i.Value, i.Text }).ToList();
            return Json(lstItem, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PartialGrid(int? OriginID, int? DestinationID, int? ExitPortID = -1, int? EntryPortID = -1)
        {
            var model = costBL.GetGridFromCity(UserSession.GetUserSession().LoginID, OriginID, DestinationID, ExitPortID, EntryPortID);
            return PartialView("_PartialGrid", model);
        }

        public ActionResult CostUpload()
        {
            session.Set<string>("PageSession", "Cost Upload");
            FillCombo("-1");
            CostUploadCombo();
            GMMSRateUpload rateUpload = new GMMSRateUpload();


            if (TempData["CostModel"] != null)
            {
                //rateUpload = (GMMSRateUpload)TempData["CostModel"];
                rateUpload = (GMMSRateUpload)JsonConvert.DeserializeObject(Convert.ToString(TempData["CostModel"]), (typeof(GMMSRateUpload)));
                TempData.Remove("CostModel");
            }
            else
            {
                rateUpload = costUploadBL.GetDataForUpload();
            }


            return View(rateUpload);
        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult SaveUploadRates([Bind(Exclude = "file")] GMMSRateUpload rateUpload)
        {
            try
            {

                // TODO: Add update logic here

                AjaxResponse result = new AjaxResponse();
                string message = string.Empty;
                FillCombo("-1");
                ModelState.Remove("file"); // This will remove the key 

                if (rateUpload.CostOrRevenueSelected.ToUpper() == "REVENUE") /////////Remove Agent validation if file Upload type is REVENUE
                {
                    ModelState.Remove("AgentID");
                }

                if (ModelState.IsValid)
                {
                    result.Message = costUploadBL.Validate(rateUpload);

                    ////Get Special THC Datatable
                    rateUpload.THCdtTable = costUploadBL.GetTHCSlabFromJson(rateUpload.SpecialTHCList);

                    if (string.IsNullOrWhiteSpace(result.Message))
                    {
                        bool res = false;

                        if (!string.IsNullOrWhiteSpace(rateUpload.CostOrRevenueSelected))
                        {
                            if (rateUpload.CostOrRevenueSelected.ToUpper() == "COST")
                            {
                                res = costUploadBL.UploadRate(rateUpload, out message);
                            }
                            else
                            {
                                res = costUploadBL.UploadPricing(rateUpload, out message);
                            }


                            if (!res)
                            {
                                result.Success = false;
                                ModelState.AddModelError(string.Empty, "Unable to upload data.");
                                result.Message = message;

                                this.AddToastMessage("RELOCBS", message, ToastType.Error);
                                //return Json(result);

                            }
                            else
                            {
                                result.Success = true;
                                result.Message = message;

                                this.AddToastMessage("RELOCBS", message, ToastType.Success);
                                //return Json(result);
                                rateUpload.dtTable = new DataTable();
                                rateUpload.CostRateList = "";
                                rateUpload.THCdtTable = new DataTable();
                                rateUpload.SpecialTHCList = "";
                                //return View("CostUpload", rateUpload);
                                TempData.Add("CostModel", JsonConvert.SerializeObject(rateUpload));
                                return RedirectToAction("CostUpload");
                            }
                        }
                        else
                        {
                            result.Success = false;
                            ModelState.AddModelError(string.Empty, "Upload Type is required");
                            this.AddToastMessage("RELOCBS", result.Message, ToastType.Error);
                        }


                    }
                    else
                    {
                        result.Success = false;
                        ModelState.AddModelError(string.Empty, result.Message);
                        this.AddToastMessage("RELOCBS", result.Message, ToastType.Error);
                    }

                    return Request.IsAjaxRequest() ? (ActionResult)PartialView("CostUpload", rateUpload)
                            : View("CostUpload", rateUpload);
                }
                else
                {
                    return Request.IsAjaxRequest() ? (ActionResult)PartialView("CostUpload", rateUpload)
                  : View("CostUpload", rateUpload);
                }
            }
            catch
            {
                this.AddToastMessage("RELOCBS", "Unable to upload File", ToastType.Error);
                return View("CostUpload", rateUpload);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //public ActionResult CostUpload(HttpPostedFileBase file, GMMSRateUpload R, int? id)
        public ActionResult CostUpload(GMMSRateUpload R, int? id)
        {

            if (R.CostOrRevenueSelected.ToUpper() == "REVENUE") /////////Remove Agent validation if file Upload type is REVENUE
            {
                ModelState.Remove("AgentID");
            }

            DataTable dt1 = new DataTable();
            DataTable dtExcel = new DataTable();
            FillCombo("-1");
            HttpPostedFileBase FileRate = R.file;

            List<HttpPostedFileBase> Files = new List<HttpPostedFileBase>();
            Files.Add(R.file);
            string FileResult = string.Empty;
            ////Get Special THC Datatable
            R.THCdtTable = costUploadBL.GetTHCSlabFromJson(R.SpecialTHCList);
            if (!CSubs.ValidateFileForSecurity(Files, out FileResult))
            {

                ModelState.AddModelError(string.Empty, FileResult);
                this.AddToastMessage("RELOCBS", FileResult, ToastType.Error);
                return Request.IsAjaxRequest() ? (ActionResult)PartialView("CostUpload", R) : View("CostUpload", R);
            }

            //PricingBL pricingBL = new PricingBL();

            if (FileRate != null && FileRate.ContentLength > 0)
            {
                string fileExtension = System.IO.Path.GetExtension(FileRate.FileName);

                if (fileExtension == ".xls" || fileExtension == ".xlsx")
                {
                    string fileLocation = Server.MapPath("~/uploads/") + FileRate.FileName;
                    if (System.IO.File.Exists(fileLocation))
                    {
                        System.IO.File.SetAttributes(fileLocation, FileAttributes.Normal);
                        //   System.IO.File.Delete(fileLocation);
                    }
                    FileRate.SaveAs(fileLocation);

                    string excelConnectionString = string.Empty;
                    excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" +
                    fileLocation + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
                    //connection String for xls file format.
                    if (fileExtension == ".xls")
                    {
                        excelConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" +
                        fileLocation + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
                    }
                    //connection String for xlsx file format.
                    else if (fileExtension == ".xlsx")
                    {
                        excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" +
                        fileLocation + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
                    }
                    DataTable dt = new DataTable();

                    //Create Connection to Excel work book and add oledb namespace
                    using (OleDbConnection excelConnection = new OleDbConnection(excelConnectionString))
                    {
                        excelConnection.Open();

                        dt = excelConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                        if (dt == null)
                        {
                            return null;
                        }

                        String[] excelSheets = new String[dt.Rows.Count];
                        int t = 0;
                        //excel data saves in temp file here.
                        foreach (DataRow row in dt.Rows)
                        {
                            excelSheets[t] = row["TABLE_NAME"].ToString();
                            t++;
                        }

                        string query = string.Empty;

                        //if (R.RateComponentID == 1)
                        //{
                        //    query = string.Format("Select top 24 * from [{0}]", excelSheets[0]);
                        //}
                        //else if (R.RateComponentID == 2)
                        //{
                        //    query = string.Format("Select top 15 * from [{0}]", excelSheets[0]);
                        //}
                        ////else if (R.RateUploadall.uploadType == 6)
                        ////{
                        ////    query = string.Format("Select * from [{0}]", excelSheets[0]);
                        ////}
                        //else if (R.RateComponentID == 4)
                        //{
                        //    query = string.Format("Select top 18 * from [{0}]", excelSheets[0]);
                        //}
                        //else
                        {
                            query = string.Format("Select * from [{0}]", excelSheets[0]);
                        }

                        using (OleDbDataAdapter dataAdapter = new OleDbDataAdapter(query, excelConnection))
                        {
                            dataAdapter.Fill(dtExcel);
                        }
                        if (excelConnection.State == ConnectionState.Open)
                        {
                            excelConnection.Close();
                        }
                    }

                }
                if (fileExtension.ToString().ToLower().Equals(".xml"))
                {
                    string fileLocation = Server.MapPath("~/uploads/") + FileRate.FileName;
                    if (System.IO.File.Exists(fileLocation))
                    {
                        System.IO.File.Delete(fileLocation);
                    }
                    FileRate.SaveAs(fileLocation);
                    XmlTextReader xmlreader = new XmlTextReader(fileLocation);
                    dtExcel.ReadXml(xmlreader);
                    xmlreader.Close();
                }

                dt1 = costUploadBL.StripEmptyRows(dtExcel);


                if (dt1 != null)
                {
                    R.dtTable = costUploadBL.CalculateAmtToConvertsion(dt1, R.ConversionRate, R.RateComponentID, R.RMCID);
                    R.dtTable = dt1;
                    foreach (DataColumn dc in R.dtTable.Columns) // trim column names
                    {
                        dc.ColumnName = dc.ColumnName.Trim();
                    }

                    if (!CommonService.IsAllColumnExist(dt1, new List<String>() { "Mode", "WeightSlabFrom", "WeightSlabTo" }))
                    {
                        ModelState.AddModelError(string.Empty, "Required first 3 Columns WeightSlabFrom, WeightSlabTo, Mode.");
                        R.dtTable = null;
                    }

                    // DataTable dtSample = pricingBL.GetProperWeightSlab(R.RateComponentID,R.RMCID);

                    //var missedWeightSlabList = dtSample.AsEnumerable().Select(r => r.Field<string>("WeightSlab"))
                    //                 .Except(dt1.AsEnumerable().Select(r => r.Field<string>("WeightSlab")));

                    //if (missedWeightSlabList.Count() > 0)
                    //{
                    //R.dtTable = null;
                    //ModelState.AddModelError(string.Empty, "Please select proper template");
                    //}

                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid file to upload");
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Upload file is required");
            }


            return Request.IsAjaxRequest() ? (ActionResult)PartialView("CostUpload", R) : View("CostUpload", R);

        }

        public ActionResult AccessorialServices()
        {
            session.Set<string>("PageSession", "Accessorial Services");
            FillCombo("-1");
            AccessorialServicesViewModel data = new AccessorialServicesViewModel();
            return View(data);
        }

        [HttpPost]
        public ActionResult AccessorialServices(AccessorialServicesViewModel data)
        {
            try
            {

                AjaxResponse result = new AjaxResponse();
                string message = string.Empty;
                FillCombo("-1");

                if (ModelState.IsValid)
                {

                    bool res = false;
                    res = costUploadBL.CityRateAdd(data, out message);
                    if (!res)
                    {
                        result.Success = false;
                        ModelState.AddModelError(string.Empty, "Unable to add record.");
                        result.Message = message;

                        this.AddToastMessage("RELOCBS", message, ToastType.Error);

                    }
                    else
                    {
                        result.Success = true;
                        result.Message = message;
                        data.Amount = 0;
                        data.USDRate = 0;
                        data.CostHeadID = 0;
                        this.AddToastMessage("RELOCBS", message, ToastType.Success);

                    }

                    return Request.IsAjaxRequest() ? (ActionResult)PartialView("AccessorialServices", data)
                            : View("AccessorialServices", data);
                }
                else
                {
                    return Request.IsAjaxRequest() ? (ActionResult)PartialView("AccessorialServices", data)
                  : View("AccessorialServices", data);
                }
            }
            catch
            {
                this.AddToastMessage("RELOCBS", "Unable to save", ToastType.Error);
                return View("AccessorialServices", data);
            }
        }

        public ActionResult ShowAgentCityAccessService(int cityid, int rmcid)
        {
            AccessServiceAgentList model = costBL.GetCitywiseAccessServiceRate(cityid, rmcid);

            List<SelectListItem> listItems = new List<SelectListItem>();
            listItems.Add(new SelectListItem() { Text = "%", Value = "P" });
            listItems.Add(new SelectListItem() { Text = "USD", Value = "F" });
            ViewData["USDPM"] = listItems;

            return Request.IsAjaxRequest() ? (ActionResult)PartialView("_AccessServiceAgentList", model)
                    : View("_AccessServiceAgentList", model);

        }

        [HttpPost]
        public ActionResult AgentAccessService(AccessServiceAgentList data)
        {
            AjaxResponse result = new AjaxResponse();
            try
            {


                string message = string.Empty;
                FillCombo("-1");

                if (ModelState.IsValid)
                {

                    bool res = false;
                    res = costUploadBL.CityRevenueAdd(data, out message);
                    if (!res)
                    {
                        result.Success = false;
                        ModelState.AddModelError(string.Empty, "Unable to add record.");
                        result.Message = message;

                    }
                    else
                    {
                        result.Success = true;
                        result.Message = message;

                        //this.AddToastMessage("RELOCBS", message, ToastType.Success);

                    }


                }
                else
                {
                    result.Success = false;
                    result.Message = "Need all data";
                }
            }
            catch
            {
                result.Success = false;
                result.Message = "Unable to save data";
                return Json(result);
            }

            return Json(result);

        }

        public ActionResult CostUploadFormat(int RMCId = 0, int ComponentId = 0, string CostOrRevenue = "", string FileType = "", string Mode = "ALL")
        {

            CostUploadFormat format = costBL.GetCostUploadFormat(RMCId, ComponentId, FileType, CostOrRevenue, Mode);

            if (format.FileID > 0)
            {
                //byte[] fileBytes = RELOCBS.Properties.Resources.OriginCostUploadFormat;
                byte[] fileBytes = (byte[])RELOCBS.Properties.Resources.ResourceManager.GetObject(format.ResourceName, Properties.Resources.Culture);

                string fileName = format.FileName;
                if (fileBytes == null || !fileBytes.Any())
                    return new HttpStatusCodeResult(404);


                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
            }
            else
            {

                return new HttpStatusCodeResult(404);
            }



        }

        public ActionResult CityCostUpload()
        {

            FillCombo("-1");
            //CostUploadCombo();
            CityCostUpload data = new CityCostUpload();
            return View(data);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CityCostUpload(CityCostUpload data)
        {
            AjaxResponse result = new AjaxResponse();
            try
            {


                string message = string.Empty;
                FillCombo("-1");

                ModelState.Remove("file");

                if (ModelState.IsValid)
                {

                    bool res = false;
                    res = costUploadBL.CityCostUpload(data, out message);
                    if (!res)
                    {
                        result.Success = false;
                        ModelState.AddModelError(string.Empty, "Unable to add record.");
                        result.Message = message;

                    }
                    else
                    {
                        result.Success = true;
                        result.Message = message;

                        //this.AddToastMessage("RELOCBS", message, ToastType.Success);

                    }


                }
                else
                {
                    result.Success = false;
                    result.Message = "Need all data";
                }
            }
            catch
            {
                result.Success = false;
                result.Message = "Unable to save data";
                return Json(result);
            }

            return Json(result);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CityCostFormat(CityCostUpload data)
        {

            ViewData["CostHead"] = comboBL.GetCostHeadDropdown();
            ViewData["YESNO"] = CommonService.YesNo;

            //HttpPostedFileBase FileRate = data.file;
            HttpPostedFileBase FileRate = data.file;
            AjaxResponse response = new AjaxResponse();

            if (FileRate != null && FileRate.ContentLength > 0)
            {
                DataTable dtExcel = new DataTable();
                List<HttpPostedFileBase> Files = new List<HttpPostedFileBase>();
                Files.Add(data.file);
                string FileResult = string.Empty;
                if (!CSubs.ValidateFileForSecurity(Files, out FileResult))
                {

                    ModelState.AddModelError(string.Empty, FileResult);
                    response.Message = FileResult;

                    return Json(response);
                }

                //PricingBL pricingBL = new PricingBL();
                FillCombo("-1");

                if (FileRate != null && FileRate.ContentLength > 0)
                {
                    string fileExtension = System.IO.Path.GetExtension(FileRate.FileName);

                    if (fileExtension == ".xls" || fileExtension == ".xlsx")
                    {
                        string fileLocation = Server.MapPath("~/uploads/") + FileRate.FileName;
                        if (System.IO.File.Exists(fileLocation))
                        {
                            System.IO.File.SetAttributes(fileLocation, FileAttributes.Normal);
                            //   System.IO.File.Delete(fileLocation);
                        }
                        FileRate.SaveAs(fileLocation);

                        string excelConnectionString = string.Empty;
                        excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" +
                        fileLocation + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
                        //connection String for xls file format.
                        if (fileExtension == ".xls")
                        {
                            excelConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" +
                            fileLocation + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
                        }
                        //connection String for xlsx file format.
                        else if (fileExtension == ".xlsx")
                        {
                            excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" +
                            fileLocation + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
                        }
                        DataTable dt = new DataTable();

                        //Create Connection to Excel work book and add oledb namespace
                        using (OleDbConnection excelConnection = new OleDbConnection(excelConnectionString))
                        {
                            excelConnection.Open();

                            dt = excelConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                            if (dt == null)
                            {
                                return null;
                            }

                            String[] excelSheets = new String[dt.Rows.Count];
                            int t = 0;
                            //excel data saves in temp file here.
                            foreach (DataRow row in dt.Rows)
                            {
                                excelSheets[t] = row["TABLE_NAME"].ToString();
                                t++;
                            }

                            string query = string.Format("Select * from [{0}]", excelSheets[0]);


                            using (OleDbDataAdapter dataAdapter = new OleDbDataAdapter(query, excelConnection))
                            {
                                dataAdapter.Fill(dtExcel);
                            }
                            if (excelConnection.State == ConnectionState.Open)
                            {
                                excelConnection.Close();
                            }
                        }

                    }
                    if (fileExtension.ToString().ToLower().Equals(".xml"))
                    {
                        string fileLocation = Server.MapPath("~/uploads/") + FileRate.FileName;
                        if (System.IO.File.Exists(fileLocation))
                        {
                            System.IO.File.Delete(fileLocation);
                        }
                        FileRate.SaveAs(fileLocation);
                        XmlTextReader xmlreader = new XmlTextReader(fileLocation);
                        dtExcel.ReadXml(xmlreader);
                        xmlreader.Close();
                    }

                    DataTable dt1 = costUploadBL.StripEmptyRows(dtExcel);

                    if (dt1 != null && dt1.Columns.Count == 4 && dt1.Rows.Count > 0)
                    {
                        //R.dtTable = pricingBL.CalculateAmtToUSD(dt1, R.ConversionRate, R.RateComponentID);
                        //R.dtTable = dt1;


                        if (!CommonService.IsAllColumnExist(dt1, new List<String>() { "CostHeadID", "CostHeadName", "Rate", "ServiceIncluded" }))
                        {
                            ModelState.AddModelError(string.Empty, "Required first 4 Columns CostHeadID, CostHeadName, Rate, ServiceIncluded.");
                            response.Message = "Required first 4 Columns CostHeadID, CostHeadName,Rate, ServiceIncluded.";
                            return Json(response);
                        }
                        else
                        {
                            data.CityCostHeadList = (from item in dt1.AsEnumerable()
                                                     select new CityCostHead()
                                                     {
                                                         CostHeadID = Convert.ToInt32(item[0]),
                                                         CostHeadName = Convert.ToString(item[1]),
                                                         Amount = Convert.ToInt64(item[2]),
                                                         USDRate = Convert.ToInt64(item[2]) * data.ConversionRate,
                                                         ServiceIncluded = Convert.ToString(item[3]).ToUpper() == "YES" ? 1 : 0
                                                     }).ToList();
                        }

                    }
                    else
                    {
                        response.Message = "Invalid file to upload.";
                        ModelState.AddModelError(string.Empty, "Invalid file to upload");
                    }
                }
                else
                {
                    response.Message = "Upload file is required";
                    ModelState.AddModelError(string.Empty, "Upload file is required");
                }


                return PartialView("_CityCostUploadList", data);
            }
            else
            {
                response.Message = "Upload file is required";
                ModelState.AddModelError(string.Empty, "Upload file is required");
            }

            return PartialView("_CityCostUploadList", data);
        }

        public ActionResult HistoryRates(int ComponentID, int RMCID, int? AgentID, int? City1, int? City2, int? Port1, int? Port2, char RevnOrCost, int? ExitPortAir, int? EntryPortAir, int? page = 1)
        {
            session.Set<string>("PageSession", "History Rates");

            ViewBag.ComponentID = ComponentID;
            ViewBag.RMCID = RMCID;
            ViewBag.AgentID = AgentID;
            ViewBag.City1 = City1;
            ViewBag.City2 = City2;
            ViewBag.Port1 = Port1;
            ViewBag.Port2 = Port2;
            ViewBag.RevnOrCost = RevnOrCost;

            //if (!CSubs.CheckPageRights(_PageID, PermissionType.VIEW))
            //{
            //    return new HttpStatusCodeResult(403);
            //}

            var pageIndex = (page ?? 1);
            int pageSize = 20;// settings.GetSettingByKey<int>("pagination_pagesize");
            int totalCount = 20;
            string OrderBy = "";
            //int Order = 0;

            var items = costBL.getHistoryRate(ComponentID, RMCID, AgentID, City1, City2, Port1, Port2, RevnOrCost, ExitPortAir, EntryPortAir, OrderBy, OrderBy, pageSize, pageIndex, out totalCount).AsEnumerable();

            if (totalCount == 0 && pageIndex > 1)
            {
                pageIndex = 1;
                items = costBL.getHistoryRate(ComponentID, RMCID, AgentID, City1, City2, Port1, Port2, RevnOrCost, ExitPortAir, EntryPortAir, OrderBy, OrderBy, pageSize, pageIndex, out totalCount).AsEnumerable();
            }
            var itemsAsIPagedList = new StaticPagedList<HistoryRate>(items, pageIndex, pageSize, totalCount);
            ViewData["PagedList"] = itemsAsIPagedList;
            return Request.IsAjaxRequest() ? (ActionResult)PartialView("HistoryRates", items) : View(items);

        }

        public ActionResult PricingUpload()
        {
            session.Set<string>("PageSession", "Price Upload");
            GMMSRateUpload rateUpload = new GMMSRateUpload();
            rateUpload = costUploadBL.GetDataForUpload();
            FillCombo("-1");

            return View(rateUpload);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PricingUpload(GMMSRateUpload R, int? id)
        {
            DataTable dt1 = new DataTable();
            DataTable dtExcel = new DataTable();
            FillCombo("-1");
            HttpPostedFileBase FileRate = R.file;

            List<HttpPostedFileBase> Files = new List<HttpPostedFileBase>();
            Files.Add(R.file);
            string FileResult = string.Empty;
            if (!CSubs.ValidateFileForSecurity(Files, out FileResult))
            {

                ModelState.AddModelError(string.Empty, FileResult);
                this.AddToastMessage("RELOCBS", FileResult, ToastType.Error);
                return Request.IsAjaxRequest() ? (ActionResult)PartialView("PricingUpload", R) : View("PricingUpload", R);
            }

            //PricingBL pricingBL = new PricingBL();

            if (FileRate != null && FileRate.ContentLength > 0)
            {
                string fileExtension = System.IO.Path.GetExtension(FileRate.FileName);

                if (fileExtension == ".xls" || fileExtension == ".xlsx")
                {
                    string fileLocation = Server.MapPath("~/uploads/") + FileRate.FileName;
                    if (System.IO.File.Exists(fileLocation))
                    {
                        System.IO.File.SetAttributes(fileLocation, FileAttributes.Normal);
                        //   System.IO.File.Delete(fileLocation);
                    }
                    FileRate.SaveAs(fileLocation);

                    string excelConnectionString = string.Empty;
                    excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" +
                    fileLocation + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
                    //connection String for xls file format.
                    if (fileExtension == ".xls")
                    {
                        excelConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" +
                        fileLocation + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
                    }
                    //connection String for xlsx file format.
                    else if (fileExtension == ".xlsx")
                    {
                        excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" +
                        fileLocation + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
                    }
                    DataTable dt = new DataTable();

                    //Create Connection to Excel work book and add oledb namespace
                    using (OleDbConnection excelConnection = new OleDbConnection(excelConnectionString))
                    {
                        excelConnection.Open();

                        dt = excelConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                        if (dt == null)
                        {
                            return null;
                        }

                        String[] excelSheets = new String[dt.Rows.Count];
                        int t = 0;
                        //excel data saves in temp file here.
                        foreach (DataRow row in dt.Rows)
                        {
                            excelSheets[t] = row["TABLE_NAME"].ToString();
                            t++;
                        }

                        string query = string.Empty;

                        //if (R.RateComponentID == 1)
                        //{
                        //    query = string.Format("Select top 24 * from [{0}]", excelSheets[0]);
                        //}
                        //else if (R.RateComponentID == 2)
                        //{
                        //    query = string.Format("Select top 15 * from [{0}]", excelSheets[0]);
                        //}
                        ////else if (R.RateUploadall.uploadType == 6)
                        ////{
                        ////    query = string.Format("Select * from [{0}]", excelSheets[0]);
                        ////}
                        //else if (R.RateComponentID == 4)
                        //{
                        //    query = string.Format("Select top 18 * from [{0}]", excelSheets[0]);
                        //}
                        //else
                        {
                            query = string.Format("Select * from [{0}]", excelSheets[0]);
                        }

                        using (OleDbDataAdapter dataAdapter = new OleDbDataAdapter(query, excelConnection))
                        {
                            dataAdapter.Fill(dtExcel);
                        }
                        if (excelConnection.State == ConnectionState.Open)
                        {
                            excelConnection.Close();
                        }
                    }

                }
                if (fileExtension.ToString().ToLower().Equals(".xml"))
                {
                    string fileLocation = Server.MapPath("~/uploads/") + FileRate.FileName;
                    if (System.IO.File.Exists(fileLocation))
                    {
                        System.IO.File.Delete(fileLocation);
                    }
                    FileRate.SaveAs(fileLocation);
                    XmlTextReader xmlreader = new XmlTextReader(fileLocation);
                    dtExcel.ReadXml(xmlreader);
                    xmlreader.Close();
                }

                dt1 = costUploadBL.StripEmptyRows(dtExcel);

                if (dt1 != null)
                {
                    //R.dtTable = pricingBL.CalculateAmtToUSD(dt1, R.ConversionRate, R.RateComponentID);
                    R.dtTable = dt1;
                    foreach (DataColumn dc in R.dtTable.Columns) // trim column names
                    {
                        dc.ColumnName = dc.ColumnName.Trim();
                    }

                    if (!CommonService.IsAllColumnExist(dt1, new List<String>() { "Mode", "WeightSlabFrom", "WeightSlabTo" }))
                    {
                        ModelState.AddModelError(string.Empty, "Required first 3 Columns WeightSlabFrom, WeightSlabTo, Mode.");
                        R.dtTable = null;
                    }

                    // DataTable dtSample = pricingBL.GetProperWeightSlab(R.RateComponentID,R.RMCID);

                    //var missedWeightSlabList = dtSample.AsEnumerable().Select(r => r.Field<string>("WeightSlab"))
                    //                 .Except(dt1.AsEnumerable().Select(r => r.Field<string>("WeightSlab")));

                    //if (missedWeightSlabList.Count() > 0)
                    //{
                    //R.dtTable = null;
                    //ModelState.AddModelError(string.Empty, "Please select proper template");
                    //}

                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid file to upload");
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Upload file is required");
            }


            return Request.IsAjaxRequest() ? (ActionResult)PartialView("PricingUpload", R) : View("PricingUpload", R);

        }


        public ActionResult SavePricingUpload([Bind(Exclude = "file")] GMMSRateUpload rateUpload)
        {
            try
            {

                // TODO: Add update logic here

                AjaxResponse result = new AjaxResponse();
                string message = string.Empty;
                FillCombo("-1");
                ModelState.Remove("file"); // This will remove the key 
                if (ModelState.IsValid)
                {
                    result.Message = costUploadBL.Validate(rateUpload);
                    if (string.IsNullOrWhiteSpace(result.Message))
                    {
                        bool res = false;
                        res = costUploadBL.UploadPricing(rateUpload, out message);
                        if (!res)
                        {
                            result.Success = false;
                            ModelState.AddModelError(string.Empty, "Unable to upload data.");
                            result.Message = message;

                            this.AddToastMessage("RELOCBS", message, ToastType.Error);
                            //return Json(result);

                        }
                        else
                        {
                            result.Success = true;
                            result.Message = message;

                            this.AddToastMessage("RELOCBS", message, ToastType.Success);
                            //return Json(result);
                            return RedirectToAction("PricingUpload");
                        }

                    }
                    else
                    {
                        result.Success = false;
                        ModelState.AddModelError(string.Empty, result.Message);
                        this.AddToastMessage("RELOCBS", result.Message, ToastType.Error);
                    }

                    return Request.IsAjaxRequest() ? (ActionResult)PartialView("PricingUpload", rateUpload)
                            : View("CostUpload", rateUpload);
                }
                else
                {
                    return Request.IsAjaxRequest() ? (ActionResult)PartialView("PricingUpload", rateUpload)
                  : View("CostUpload", rateUpload);
                }
            }
            catch
            {
                this.AddToastMessage("RELOCBS", "Unable to upload File", ToastType.Error);
                return View("PricingUpload", rateUpload);
            }
        }

        public ActionResult GetReport(int surveyid, int Wtid)
        {
            EncryptedQueryString query = new EncryptedQueryString();
            query["PageID"] = _PageID;
            query["ReportID"] = "10003";
            query["surveyid"] = Convert.ToString(surveyid);
            query["Wtid"] = Convert.ToString(Wtid);
            ViewBag.queryStr = "args=" + query.ToString();

            return this.RedirectSameDomain("/Reports/ReportViewer.aspx?args=" + query.ToString());
            //return Redirect("/Reports/ReportViewer.aspx");
        }

        public ActionResult GetCompareRate(int surveyid)
        {
            EncryptedQueryString query = new EncryptedQueryString();
            query["PageID"] = _PageID;
            query["ReportID"] = "10005";
            query["surveyid"] = Convert.ToString(surveyid);
            //query["Wtid"] = Convert.ToString(Wtid);
            ViewBag.queryStr = "args=" + query.ToString();

            return this.RedirectSameDomain("/Reports/ReportViewer.aspx?args=" + query.ToString());

        }


        public ActionResult SubHistoryRates(int ComponentID, int RMCID, int? AgentID, int? City1, int? City2, int? Port1, int? Port2, char RevnOrCost, Int64 OrgRMCAgentEffectDateID, bool IsJobPage = false)
        {
            var items = costBL.getSubHistoryRate(ComponentID, RMCID, AgentID, City1, City2, Port1, Port2, RevnOrCost, OrgRMCAgentEffectDateID, IsJobPage).AsEnumerable();

            ViewBag.IsJobPage = IsJobPage;

            return PartialView("_HistoryRatesPartial", items);
        }

        public ActionResult CityCostUploadFormat()
        {

            byte[] fileBytes = (byte[])RELOCBS.Properties.Resources.CityCostFormat;

            string fileName = "CityCostFormat.xlsx";
            if (fileBytes == null || !fileBytes.Any())
                return new HttpStatusCodeResult(404);


            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }

        public ActionResult CityRevenue(int cityid, int rmcid)
        {
            string PageID = "16";
            CityCostRevenue model = new CityCostRevenue();
            if (CSubs.CheckPageRights(PageID, PermissionType.VIEW))
            {
                model = costBL.GetCitywiseRevenue(cityid, rmcid);
            }
            return Request.IsAjaxRequest() ? (ActionResult)PartialView("CityCostRevenue", model) : View("CityCostRevenue", model);

        }

        public CostViewModel GetCostModel(int? SurveyID = 0, int? RateCompRateWtID = 0, int? RateCompRateBatchID = 0)
        {
            CostViewModel costViewModel = new CostViewModel();
            String ModeID = "-1";
            int RMCID = -1;

            if (SurveyID > 0)
            {
                costViewModel = costBL.GetDetailById(SurveyID, RateCompRateWtID, (int)RateCompRateBatchID);



                ModeID = Convert.ToString(costViewModel.ModeID);
                if (RateCompRateBatchID > 0)
                {
                    costViewModel.RateCompRateBatchId = 0;
                    costViewModel.RateCompRateWtID = 0;
                }
                else
                {
                    if (RateCompRateWtID <= 0)
                    {
                        costViewModel.RMCID = 3;
                        RMCID = costViewModel.RMCID;
                    }
                }
                costViewModel.RMCID = 1;
                costViewModel.BaseCurrencyRateID = UserSession.GetUserSession().BaseCurrID;
                costViewModel.RateCurrencyID = UserSession.GetUserSession().BaseCurrID;
                costViewModel.ConversionRate = 1;
                TempData["CostHeadList"] = costViewModel.CostHeadList;
                TempData["SubCostList"] = null;
                string SubCostDiv = string.Empty;
                foreach (var item in costViewModel.CostHeadList)
                {
                    item.IsSubCost = new BL.CommanBL().IsSubCostHead(item.CostHeadID);
                    SubCostDiv += GetSubCostHeadList(item.CostHeadID, item.RateComponentID, Convert.ToInt32(costViewModel.SurveyID),
                        Convert.ToInt32(costViewModel.RateCompRateWtID), costViewModel.RateCompRateBatchId, 1, 1);
                }
                ViewBag.SubCostList = SubCostDiv;
            }
            FillCombo(ModeID, RMCID, costViewModel.ServiceLineID);
            return costViewModel;
        }

        public CostViewModel GetEditCostModel(int? SurveyID = 0, int? RateCompRateWtID = 0, int RateCompRateBatchID = 0)
        {
            CostViewModel costViewModel = new CostViewModel();
            costViewModel = costBL.GetDetailById(SurveyID, RateCompRateWtID, RateCompRateBatchID);
            costViewModel.RMCID = 1;
            FillCombo(costViewModel.ModeID.ToString(), costViewModel.RMCID, costViewModel.ServiceLineID);
            costViewModel.RateCompRateBatchId = RateCompRateBatchID;
            //
            TempData["SubCostList"] = null;
            string SubCostDiv = string.Empty;
            foreach (var item in costViewModel.CostHeadList)
            {
                item.IsSubCost = new BL.CommanBL().IsSubCostHead(item.CostHeadID);
                SubCostDiv += GetSubCostHeadList(item.CostHeadID, item.RateComponentID, Convert.ToInt32(costViewModel.SurveyID),
                    Convert.ToInt32(RateCompRateWtID), costViewModel.RateCompRateBatchId, 1, 1);
            }
            ViewBag.SubCostList = SubCostDiv;
            TempData["CostHeadList"] = costViewModel.CostHeadList;
            costViewModel.ConversionRate = 1;
            return costViewModel;
        }

        public int GetBaseCurrancyForRMC(int RMCID)
        {
            return costUploadBL.GetBaseCurrByRMC(RMCID);
        }

        public ActionResult THCHistory(int Component, int RMC, int? Agent, int? DestCity, int? OrgContinent, Int64? MasterTrans, int? page = 1)
        {
            session.Set<string>("PageSession", "History Rates");
            var pageIndex = (page ?? 1);
            int pageSize = 20;
            int totalCount = 20;
            string OrderBy = "";

            if (MasterTrans != null && MasterTrans > 0)
            {
                var items = costBL.getSubBTRTHCHistory(Component, RMC, Agent, DestCity, OrgContinent, MasterTrans).AsEnumerable();

                return PartialView("_THCHistoryPartial", items);
            }
            else
            {
                var items = costBL.getBTRTHCHistory(Component, RMC, Agent, DestCity, OrgContinent, OrderBy, OrderBy, pageSize, pageIndex, out totalCount).AsEnumerable();

                if (totalCount == 0 && pageIndex > 1)
                {
                    pageIndex = 1;
                    items = costBL.getBTRTHCHistory(Component, RMC, Agent, DestCity, OrgContinent, OrderBy, OrderBy, pageSize, pageIndex, out totalCount).AsEnumerable();
                }

                var itemsAsIPagedList = new StaticPagedList<HistorySplTHC>(items, pageIndex, pageSize, totalCount);
                ViewData["PagedList"] = itemsAsIPagedList;
                ViewBag.Component = Component;
                ViewBag.RMC = RMC;
                ViewBag.DestCity = DestCity;
                ViewBag.OrgContinent = OrgContinent;
                return Request.IsAjaxRequest() ? (ActionResult)PartialView("THCHistory", items) : View(items);
            }
        }

        public ActionResult RMCCompareRates(int ComponentID, int RMCID, int? AgentID, int? City1, int? City2, int? Port1, int? Port2, char RevnOrCost, int? ExitPortAir, int? EntryPortAir, int? page = 1)
        {
            session.Set<string>("PageSession", "Compare Rates");

            ViewBag.ComponentID = ComponentID;
            ViewBag.RMCID = RMCID;
            ViewBag.AgentID = AgentID;
            ViewBag.City1 = City1;
            ViewBag.City2 = City2;
            ViewBag.Port1 = Port1;
            ViewBag.Port2 = Port2;
            ViewBag.RevnOrCost = RevnOrCost;

            var items = costBL.getRMCCompareRate(ComponentID, RMCID, AgentID, City1, City2, Port1, Port2, RevnOrCost, ExitPortAir, EntryPortAir);
            return Request.IsAjaxRequest() ? (ActionResult)PartialView("RMCCompareRates", items) : View("RMCCompareRates", items);

        }

        public ActionResult GetWHServiceCost(Int64? SurveyID, Int64? RateCompRateID, Int64? RateCompRateBatchId)
        {
            if (SurveyID > 0)
            {
                CostViewModel costViewModel = costBL.GetDetailById(Convert.ToInt32(SurveyID));

                FillCombo(costViewModel.ModeID.ToString(), ServiceLineID: costViewModel.ServiceLineID);

                costViewModel.WHServiceCostList = costBL.GetWHServiceCosts(UserSession.GetUserSession().LoginID, SurveyID, RateCompRateID, RateCompRateBatchId);

                return (ActionResult)PartialView("_PartialWHServiceCost", costViewModel);
                //return Request.IsAjaxRequest() ? (ActionResult)PartialView("_PartialWHServiceCost", costViewModel) : View(costViewModel);
            }
            else
            {
                return RedirectToAction("Index", "Survey");
            }
        }

        [HttpPost]
        public ActionResult WHServiceCost(CostViewModel costViewModel)
        {
            try
            {
                AjaxResponse result = new AjaxResponse();
                string message = string.Empty;
                FillCombo(costViewModel.ModeID.ToString(), ServiceLineID: costViewModel.ServiceLineID);

                if (!string.IsNullOrEmpty(costViewModel.WHServiceCostListHidden))
                {
                    bool res = false;
                    res = costBL.SaveWHServiceCosts(costViewModel, out message);
                    if (!res)
                    {
                        result.Success = false;
                        ModelState.AddModelError(string.Empty, "Unable to save wh service cost details.");
                        result.Message = message;

                        this.AddToastMessage("RELOCBS", message, ToastType.Error);
                    }
                    else
                    {
                        result.Success = true;
                        result.Message = message;

                        this.AddToastMessage("RELOCBS", message, ToastType.Success);
                    }
                    return RedirectToAction("Create", new { SurveyID = costViewModel.SurveyID, RateCompRateWtID = costViewModel.RateCompRateWtID });
                }
                else
                {
                    result.Success = false;
                    result.Message = message;
                    ModelState.AddModelError(string.Empty, "Please add services.");

                    return RedirectToAction("Create", new { SurveyID = costViewModel.SurveyID, RateCompRateWtID = costViewModel.RateCompRateWtID });
                }
            }
            catch (Exception ex)
            {
                this.AddToastMessage("RELOCBS", ex.ToString(), ToastType.Success);
                return View();
            }
        }

        public ActionResult WHDeliveryReport_Print(Int64? SurveyID, Int64? RateCompRateID, Int64? RateCompRateBatchId)
        {
            try
            {
                CostViewModel costViewModel = costBL.GetDetailById(Convert.ToInt32(SurveyID));
                costViewModel.WHServiceCostList = costBL.GetWHServiceCosts(UserSession.GetUserSession().LoginID, SurveyID, RateCompRateID, RateCompRateBatchId);

                return View(costViewModel);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
