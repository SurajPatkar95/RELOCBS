using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using RELOCBS.AjaxHelper;
using RELOCBS.App_Code;
using RELOCBS.BL;
using RELOCBS.BL.Pricing;
using RELOCBS.CustomAttributes;
using RELOCBS.Entities;
using RELOCBS.Utility;
using RELOCBS.Extensions;
using RELOCBS.Common;

namespace RELOCBS.Controllers
{
    [AuthorizeUser]
    public class QuotationController : BaseController
    {

        private string _PageID = "45";

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

        private QuotingBL _quotingBL;

        public QuotingBL quotingBL
        {

            get
            {
                if (this._quotingBL == null)
                    this._quotingBL = new QuotingBL();
                return this._quotingBL;
            }
        }

        // GET: Quotation
        public ActionResult Index()
        {
            return View();
        }

        // GET: Quotation/Details/5
        public ActionResult Details(int id, int batchid)
        {
            session.Set<string>("PageSession", "Quotation");
            QuotingViewModel costViewModel = new QuotingViewModel();
            costViewModel = quotingBL.GetDetailById(-1, id,batchid);
            FillCombo(Convert.ToString(costViewModel.SurveyID), costViewModel.ModeID.ToString(), costViewModel.ServiceLineID, costViewModel.CostHeadList.Count);
            return Request.IsAjaxRequest()
            ? (ActionResult)PartialView("Details", costViewModel)
            : View(costViewModel);
        }

		// GET: Quotation/Create
		public ActionResult Create(int SurveyID,int RateCompRateWtID,int RateCompRateBatchId)
		{
            session.Set<string>("PageSession", "Quotation");
            QuotingViewModel costViewModel = new QuotingViewModel();
            costViewModel = quotingBL.GetDetailById(SurveyID, RateCompRateWtID, RateCompRateBatchId);
			TempData["SubCostList"] = null;
			string SubCostDiv = string.Empty;
			foreach (var item in costViewModel.CostHeadList)
			{
				item.IsSubCost = new BL.CommanBL().IsSubCostHead(item.CostHeadID);
				SubCostDiv += GetSubCostHeadList(item.CostHeadID, item.RateComponentID, Convert.ToInt32(costViewModel.SurveyID),
					Convert.ToInt32(RateCompRateWtID), Convert.ToInt32(RateCompRateBatchId), 1, 0);
			}
			ViewBag.SubCostList = SubCostDiv;
			costViewModel.RateCompRateBatchID = RateCompRateBatchId;
			FillCombo(costViewModel.SurveyID.ToString(), costViewModel.ModeID.ToString(),costViewModel.ServiceLineID, costViewModel.CostHeadList.Count);
            return Request.IsAjaxRequest()
            ? (ActionResult)PartialView("Create", costViewModel)
            : View(costViewModel);
        }

        private void FillCombo(string SurveyID, string ModeID,int ServiceLineID, int? count)
        {
            ViewData["RMC"] = comboBL.GetRMCDropdown();
            ViewData["BusinessLine"] = comboBL.GetBusinessLineDropdown();
            ViewData["GoodsDescription"] = comboBL.GetGoodsDescriptionDropdown();
            ViewData["Mode"] = comboBL.GetModeDropdown(ServiceLineID:ServiceLineID);
            ViewData["RateComponent"] = comboBL.GetRateComponentDropdown();
            ViewData["Agent"] = comboBL.GetRateAgentDropdown();
            //ViewData["FromLocation"] = comboBL.GetFromLocationDropdown();
            //ViewData["ToLocation"] = comboBL.GetToLocationDropdown();
            ViewData["RateCurrency"] = comboBL.GetRateCurrencyDropdown();
            ViewData["BaseCurrencyRate"] = comboBL.GetBaseCurrencyRateDropdown();
            ViewData["WeightUnit"] = comboBL.GetMeasurementUnitDropdown('A');
            ViewData["CostHeadList"] = comboBL.GetCostHeadDropdown();

            ViewData["CityList"] = comboBL.GetCityDropdown();
            ViewData["PortList"] = comboBL.GetPortDropdown();
            ViewData["ShippingLineList"] = comboBL.GetShippingLineDropdown(ModeID);
			ViewData["Sequence"] = comboBL.SequenceDropDown(Count: count);

			List<SelectListItem> ApprovalList = comboBL.GetQuoteApprovalUserList(SurveyID: SurveyID).ToList();
			ViewData["ApprovalUserList"] = ApprovalList;
			ViewBag.ApprovalCount = ApprovalList.Count();

		}

        // POST: Quotation/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int SurveyID, int RateCompRateWtID,int RateCompRateBatchID, QuotingViewModel QuotingData,string SubmitRate)
        {
			try
			{
				AjaxResponse result = new AjaxResponse();
				string message = string.Empty;
				FillCombo(Convert.ToString(SurveyID), QuotingData.ModeID.ToString(), QuotingData.ServiceLineID, QuotingData.CostHeadList.Count);
				if (ModelState.IsValid)
				{
					if (QuotingData.HFVQuotingList.containsHtmlTags())
					{
						ModelState.AddModelError("", "Invalid Cost Heads");
						this.AddToastMessage("RELOCBS", "Invalid Cost Heads", ToastType.Error);
						return Request.IsAjaxRequest() ? (ActionResult)PartialView("Create", ViewData)
						: View(ViewData);
					}

					bool res = false;

					if (SubmitRate.ToUpper() == "APPROVAL PENDING" || SubmitRate.ToUpper() == "REMOVE APPROVAL" || SubmitRate.ToUpper() == "PROCEED FOR APPROVAL")
                    {

                        bool IsRemoveApproval = false;
                        if (SubmitRate.ToUpper() == "REMOVE APPROVAL")
                        {
                            IsRemoveApproval = true;
                        }
						else if (SubmitRate.ToUpper() == "PROCEED FOR APPROVAL")
						{
							QuotingData.QuoteSentApprove = true;
						}
                        res = quotingBL.ApproveQuote(Convert.ToInt32(QuotingData.SurveyID), IsRemoveApproval, QuotingData.QuoteSentApprove, QuotingData.QuoteSenttoApproveUser, UserSession.GetUserSession().LoginID, QuotingData.RateCompRateBatchID,out message);
                    }
                    else
                    {
                        res = quotingBL.InsertQuoting(QuotingData, UserSession.GetUserSession().LoginID, out message);
                    }

                    if (!res)
                    {
                        result.Success = false;
                        ModelState.AddModelError(string.Empty, "Unable to save quoting data.");
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
                    return RedirectToAction("Create", new { SurveyID = QuotingData.SurveyID, RateCompRateWtID = QuotingData.RateCompRateWtID, RateCompRateBatchID = QuotingData.RateCompRateBatchID });
                }
                else
                {
                    return Request.IsAjaxRequest() ? (ActionResult)PartialView("Create", QuotingData)
                  : View(QuotingData);
                }
            }
            catch
            {
                return View(QuotingData);
            }
        }

        // GET: Quotation/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Quotation/Edit/5
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int SurveyID, int RateCompRateWtID, int RateCompRateBatchID)
        {
            try
            {
                // TODO: Add delete logic here

                AjaxResponse result = new AjaxResponse();
                string message = string.Empty;
                bool res = false;
                res = quotingBL.Delete(SurveyID, RateCompRateWtID, RateCompRateBatchID, UserSession.GetUserSession().LoginID, out message);
                if (!res)
                {
                    result.Success = false;
                    ModelState.AddModelError(string.Empty, "Unable to Delete Quotation data.");
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
                return RedirectToAction("Create", new { SurveyID = SurveyID, RateCompRateWtID = RateCompRateWtID, RateCompRateBatchID = RateCompRateBatchID });

            }
            catch
            {
                return RedirectToAction("Create", new { SurveyID = SurveyID, RateCompRateWtID = RateCompRateWtID, RateCompRateBatchID = RateCompRateBatchID });
            }
        }


        [HttpPost]
        public ActionResult UpdateForJob(int SurveyID, int RateCompRateWtID,int RateCompRateBatchID)
        {
            //QuotingViewModel costViewModel = new QuotingViewModel();
            AjaxResponse result = new AjaxResponse();
            try
            {



                string message = string.Empty;
                result.Message = "Invalid Inputs";
                if (ModelState.IsValid)
                {
                    bool res = false;
                    res = quotingBL.UpdateUseForJobStatus(SurveyID,RateCompRateWtID, UserSession.GetUserSession().LoginID, RateCompRateBatchID, out message);

                    //costViewModel = quotingBL.GetDetailById(SurveyID, RateCompRateWtID);
                    if (!res)
                    {
                        result.Success = false;
                        result.Message = message;
                        //this.AddToastMessage("RELOCBS", message, ToastType.Error);
                        //return Json(result);
                    }
                    else
                    {
                        result.Success = true;
                        result.Message = message;

                        //this.AddToastMessage("RELOCBS", message, ToastType.Success);
                        //return Json(result);
                    }

                    //ViewBag.Result = result;
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
            }
            catch
            {
                result.Message = "Unexpected Error Occured";
                return Json(result, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult GetPartialGrid(int SurveyID, int RateCompRateWtID)
        {
            QuotingViewModel costViewModel = new QuotingViewModel();
            costViewModel = quotingBL.GetDetailById(SurveyID, RateCompRateWtID);
            return Request.IsAjaxRequest() ? (ActionResult)PartialView("_PartialGrid", costViewModel.SurveyRateGridDt)
                  : PartialView("_PartialGrid", costViewModel.SurveyRateGridDt);
        }

        public ActionResult GetReport(int surveyid, int Wtid,int Batchid)
        {
            EncryptedQueryString query = new EncryptedQueryString();
            query["PageID"] = _PageID;
            query["ReportID"] = "10004";
            query["surveyid"] = Convert.ToString(surveyid);
            query["Wtid"] = Convert.ToString(Wtid);
            query["Batchid"] = Convert.ToString(Batchid);
            ViewBag.queryStr = "args=" + query.ToString();

            return this.RedirectSameDomain("/Reports/ReportViewer.aspx?args=" + query.ToString());
            //return Redirect("/Reports/ReportViewer.aspx");
        }

		private void FillComboForPrint(int? CityID)
		{
            bool RMCBuss = false;
            if (UserSession.GetUserSession().BussinessLine == "NON RMC-BUSINESS")
            {
                RMCBuss = false;
            }
            else
            {
                RMCBuss = true;
            }

            ViewData["AgentList"] = comboBL.GetAgentDropdown(CORA: "A");
            ViewData["AccountList"] = comboBL.GetAgentDropdown(CORA: "C");
            ViewData["ClientList"] = comboBL.GetAgentDropdown(CORA: RMCBuss ? "R" : null);
            ViewData["EmployeeList"] = comboBL.GetEmployeeDropdown(SPTYPE: "QUOTESENTBY");
			ViewData["CityList"] = (CityID!=null) ? comboBL.GetCityDropdown(SPTYPE:"Single",CityID:CityID) : new List<SelectListItem>();
			ViewData["TitleList"] = CommonService.Title;
			ViewData["RateCurrency"] = comboBL.GetRateCurrencyDropdown();
			ViewData["BaseCurrencyRate"] = comboBL.GetBaseCurrencyRateDropdown();
		}

		public ActionResult GetPrint(int SurveyID, int RateCompRateWtID, int RateCompRateWtBatchID, bool IsLumsum)
		{
			
			QuotePrint print = new QuotePrint();
			//print.CostHeadList.Add(new CostHeadDetail() { CostHeadID = 1, CostHeadName = "test", CostHeadDescription = "test desc", Amount = 2000 });
			//print.QuoteTermsList.Add(new QuoteTerm() { TermID = 1, TermName = "test" });
			//print.InclusionList.Add(new QuoteInclusionExclusion() { CostHeadID = 1, CostHeadName = "test" });
			//print.ExclusionList.Add(new QuoteInclusionExclusion() { CostHeadID = 1, CostHeadName = "test" });

			print = quotingBL.GetQuotingPrintDetail(SurveyID, RateCompRateWtID, RateCompRateWtBatchID, IsLumsum);
			print.IsLumsum = IsLumsum;
            if (print.Quoted_Curr==0)
            {
                print.Quoted_Curr = UserSession.GetUserSession().BaseCurrID;
                print.QuotedExchange_rate = 1;
            }
            FillComboForPrint(print.City);

            

            return Request.IsAjaxRequest() ? (ActionResult)PartialView("_PartialPrint", print) : PartialView("_PartialPrint", print);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult GetPrint(QuotePrint print, string SubmitButton)
		{
			EncryptedQueryString query = new EncryptedQueryString();
			query["PageID"] = _PageID;
			query["ReportID"] = "10006";
			query["surveyid"] = Convert.ToString(print.SurveyID);
			query["Wtid"] = Convert.ToString(print.RateCompRateWtID);
			query["Batchid"] = Convert.ToString(print.RateCompRateWtBatchID);
			query["IsLumsum"] = Convert.ToString(print.IsLumsum);
            string CompanyName = UserSession.GetUserSession().CompanyName;
            //ViewBag.queryStr = "args=" + query.ToString();
            
            bool res = false;
			string message = string.Empty;
			res = quotingBL.InsertQuotingPrint(print, UserSession.GetUserSession().LoginID, out message);
            //SubmitButton = "SaveEmail";

            if (SubmitButton == "Save")
			{
				return Json(new { ok = res, Report = false, message = message });
			}
			else if (SubmitButton == "SavePrint")
			{
				query["IsEmail"] = "0";
				return Json(new { ok = res, Report = true, message = message, newurl = GetHtmlQouteUrl(CompanyName.Equals("BTR", StringComparison.OrdinalIgnoreCase), query) });
			}
			else if (SubmitButton == "SaveEmail")
			{
				query["IsEmail"] = "1";
				return Json(new { ok = res, Report = true, message = message, newurl = GetHtmlQouteUrl(CompanyName.Equals("BTR",StringComparison.OrdinalIgnoreCase),query)});
			}

			return Json(print);
			//return Request.IsAjaxRequest() ? (ActionResult)PartialView("_PartialPrint", print) : PartialView("_PartialPrint", print);
		}

		public ActionResult GetCompareQuote(int surveyid)
        {
            EncryptedQueryString query = new EncryptedQueryString();
            query["PageID"] = _PageID;
            query["ReportID"] = "10008";
            query["surveyid"] = Convert.ToString(surveyid);
            //query["Wtid"] = Convert.ToString(Wtid);
            ViewBag.queryStr = "args=" + query.ToString();

            return this.RedirectSameDomain("/Reports/ReportViewer.aspx?args=" + query.ToString());

        }

        private string GetHtmlQouteUrl(bool IsHtmlPrint, EncryptedQueryString Args)
        {

            return this.RedirectSameDomain((!IsHtmlPrint ? "/Reports/ReportViewer.aspx": "/Quotation/GetQoutePrint") + "?args=" + Args.ToString()).Url;
        }

        public ActionResult GetQoutePrint(string args)
        {
            EncryptedQueryString QueryStrArgs = new EncryptedQueryString(args);
            if (UserSession.HasPermission(Convert.ToString(QueryStrArgs["PageID"]), EnumUtility.PageAction.VIEW))
            {
                QouteHtmlPrint print = quotingBL.GetQuotingPrint(Convert.ToInt32(QueryStrArgs["surveyid"]), Convert.ToInt32(QueryStrArgs["Wtid"]), Convert.ToInt32(QueryStrArgs["Batchid"]), Convert.ToBoolean(QueryStrArgs["IsLumsum"]), Convert.ToInt16(QueryStrArgs["IsEmail"]));
                return View("QoutePrint", print);
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
