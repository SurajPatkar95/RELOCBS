using RELOCBS.App_Code;
using RELOCBS.BL;
using RELOCBS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RELOCBS.BL.GMMS;
using RELOCBS.AjaxHelper;
using RELOCBS.Utility;
using RELOCBS.Extensions;
using System.Configuration;
using System.IO;
using RELOCBS.CustomAttributes;

namespace RELOCBS.Controllers
{
	[AuthorizeUser]
	public class LeadController : BaseController
	{

		private string _PageID = "17";

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


		private LeadBL _leadBL;
		public LeadBL leadBL
		{
			get
			{
				if (this._leadBL == null)
					this._leadBL = new LeadBL();
				return this._leadBL;

			}
		}

		//public LeadController() : base(){ }


		// GET: Lead
		public ActionResult Index(int? page)
		{
			LeadViewModel model = new LeadViewModel();
			BindDefaultValues(model);
			FillCombo(model.manageDet.ServiceLnId);
			if (!CSubs.CheckPageRights(_PageID, PermissionType.VIEW))
			{
				return new HttpStatusCodeResult(403);
			}

			var pageIndex = (page ?? 1);
			int pageSize = 10;
			int totalCount = 0;

			int? OriginCityID = null;
			int? DestinationCityID = null;
			int? RMCID = null;
			int? UpdatedBatchId = null;
			session.Set<string>("PageSession", "Lead Master");

			string OrderBy = "CreatedDate";
			String Order = "Desc";
			string SearchKey = string.Empty;

			if (Request.Params["RMCID"] != null && Request.Params["RMCID"].Trim() != "")
			{
				RMCID = Convert.ToInt32(Request.Params["RMCID"]);
				model.RMCID = Convert.ToInt32(RMCID);
			}

			if (Request.Params["FromCityID"] != null && Request.Params["FromCityID"].Trim() != "")
			{
				OriginCityID = Convert.ToInt32(Request.Params["FromCityID"]);
				model.FromCityID = Convert.ToInt32(OriginCityID);
			}

			if (Request.Params["ToCityID"] != null && Request.Params["ToCityID"].Trim() != "")
			{
				DestinationCityID = Convert.ToInt32(Request.Params["ToCityID"]);
				model.ToCityID = Convert.ToInt32(DestinationCityID);
			}
			if (Request.Params["IsRoad"] != null && Request.Params["IsRoad"].Trim() != "")
			{
				model.IsRoad = Convert.ToBoolean(Request.Params["IsRoad"]);
			}
			if (Request.Params["grid-column"] != null && Request.Params["grid-column"].Trim() != "")
			{
				OrderBy = Request.Params["grid-column"].Trim().ToString();
			}

			if (Request.Params["grid-dir"] != null && Request.Params["grid-column"].Trim() != "")
			{
				Order = Request.Params["grid-dir"].Trim().ToString();

				if (Order == "1")
				{
					Order = "Desc";
				}
			}

			if (Request.Params["UpdatedBatchId"] != null && Request.Params["UpdatedBatchId"].Trim() != "")
			{
				UpdatedBatchId = Convert.ToInt32(Request.Params["UpdatedBatchId"]);
			}

			model = BindDefaultValues(model);


			if (page < 1) page = 1;
			int skip = (Convert.ToInt32(page) * pageSize) - pageSize;

			var items = leadBL.GetForGrid(RMCID, OriginCityID, DestinationCityID, model.IsRoad, UpdatedBatchId, OrderBy, Order, skip, pageSize, out totalCount);

			//if (UpdatedBatchId != null && totalCount == 0 && pageIndex > 1)
			//{
			//    pageIndex = 1;
			//    items = leadBL.GetForGrid(RMCID, OriginCityID, DestinationCityID, model.IsRoad, UpdatedBatchId, OrderBy, Order, skip, pageSize, out totalCount);
			//}



			var itemsAsIPagedList = new PagedList.StaticPagedList<Lead>(items, pageIndex, pageSize, totalCount);
			ViewData["PagedList"] = itemsAsIPagedList;

			return Request.IsAjaxRequest()
			? (ActionResult)PartialView("_PartialGrid", model)
			: View("_PartialGrid", model);



		}

		public ActionResult GetFSFRDet(int UpdatedBatchId, bool IsRoad = false, int IsDisplay = 0)
		{
			int totalCount;
			var items = leadBL.GetForGrid(null, null, null, false, UpdatedBatchId, null, null, 0, 0, out totalCount);
			ViewData["dtSea"] = items.Where(x => x.pricing.ModeID == 1 || x.pricing.ModeID == 3);
			ViewData["dtAir"] = items.Where(x => x.pricing.ModeID == 2);
			ViewBag.IsDisplay = (IsDisplay <= 0);
			ViewBag.IsRoad = (IsRoad);
			return PartialView("_FSFRGrid", items);
		}

		public LeadViewModel BindDefaultValues(LeadViewModel model)
		{
			bool RMCBuss = !(UserSession.GetUserSession().BussinessLine == "NON RMC-BUSINESS");
			SelectListItem serviceLnList = comboBL.GetServiceLineDropdown(RMCBuss: RMCBuss).Where(x => x.Text == "GMMS EXP").FirstOrDefault();
			//SelectListItem serviceLnList = comboBL.GetServiceLineDropdown(RMCBuss:RMCBuss).Where(x => x.Text == "CENDANT (ACADIA)").FirstOrDefault();
			model.manageDet.ServiceLnId = Convert.ToInt32(serviceLnList.Value);
			model.manageDet.ServiceLn = serviceLnList.Text;
			if (UserSession.GetUserSession().CompanyID == 2)
			{
				SelectListItem HandlingBrList = comboBL.GetCompanyBranchDropdown().FirstOrDefault();
				model.manageDet.HandlingBrId = Convert.ToInt32(HandlingBrList.Value);
				model.manageDet.HandlingBr = HandlingBrList.Text;
			}
			else
			{
				SelectListItem HandlingBrList = comboBL.GetCompanyBranchDropdown().Where(x => x.Text.ToUpper() == "MUMBAI").FirstOrDefault();
				model.manageDet.HandlingBrId = Convert.ToInt32(HandlingBrList.Value);
				model.manageDet.HandlingBr = HandlingBrList.Text;
			}
			

			SelectListItem ShipTypeList = comboBL.GetShipmentTypeDropdown().Where(x => x.Text.ToUpper() == "DOOR TO DOOR").FirstOrDefault();
			model.manageDet.ShipmentTypeId = Convert.ToInt32(ShipTypeList.Value);
			model.manageDet.ShipmentType = ShipTypeList.Text;

            //SelectListItem CommodityList = comboBL.GetGoodsDescriptionDropdown().Where(x => x.Text.ToUpper() == "HOUSEHOLD GOODS").FirstOrDefault();
            //model.manageDet.CommodityId = Convert.ToInt32(CommodityList.Value);
            //model.manageDet.Commodity = CommodityList.Text;

            ViewData["CommodityList"] = comboBL.GetGoodsDescriptionDropdown(SPTYPE : "LEAD");


            SelectListItem EnqSourceList = comboBL.GetEnqInfoSourceDropdown().Where(x => x.Text.ToUpper() == "EMAIL").FirstOrDefault();
			model.manageDet.EnqSourceID = Convert.ToInt32(EnqSourceList.Value);
			//model.manageDet.Commodity = CommodityList.Text;

			SelectListItem BussinessLineList = comboBL.GetBusinessLineDropdown().Where(x => x.Text.ToUpper() == "GMMS").FirstOrDefault();
			model.manageDet.BussinessLineID = Convert.ToInt32(BussinessLineList.Value);
			//model.manageDet.Commodity = CommodityList.Text;

			SelectListItem MoveQuoteList = comboBL.GetMoveQuoteClassDropdown().Where(x => x.Text.ToUpper() == "QUOTE").FirstOrDefault();
			model.manageDet.MoveQuoteID = Convert.ToInt32(MoveQuoteList.Value);

			//model.manageDet.ContainerTypeID = 20;
			//model.manageDet.Commodity = CommodityList.Text;

			return model;
		}

		// GET: Lead/Details/5
		public ActionResult Details(int id)
		{
			return View();
		}

		// GET: Lead/Create
		public ActionResult Create()
		{
			ViewBag.InValid = false;
			LeadViewModel lead = new LeadViewModel();
			lead = BindDefaultValues(lead);
			FillCombo(lead.manageDet.ServiceLnId);
			session.Set<string>("PageSession", "Lead Master");
			return View(lead);
		}

		// POST: Lead/Create
		[HttpPost]
		[ValidateInput(false)]
		public ActionResult Create(LeadViewModel lead)
		{
			try
			{
				RELOCBS.AjaxHelper.AjaxResponse result = new AjaxResponse();
				FillCombo(lead.manageDet.ServiceLnId);
				if (ModelState.IsValid)
				{
					//string Message;
					//result.Success = leadBL.Insert(lead, out Message);
					//if (!result.Success)
					//{

					//    ModelState.AddModelError(string.Empty, "Unable to save Cost data.");
					//    result.Message = Message;

					//    this.AddToastMessage("RELOCBS", Message, ToastType.Error);
					//    //return Json(result);
					//}
					//else
					//{
					//    result.Message = Message;
					//    int totalCount=0;
					//    lead.LeadGrid = leadBL.GetForGrid(lead.RMCID, lead.FromCityID, lead.ToCityID, "", "", 0, 1,out totalCount).ToList();
					//    this.AddToastMessage("RELOCBS", Message, ToastType.Success);
					//    //return Json(result);
					//}
					return RedirectToAction("Index", "Pricing", new { OriginId = lead.FromCityID, DestinationId = lead.ToCityID, RMCId = lead.RMCID, IsRoad = lead.IsRoad });
				}
				return Request.IsAjaxRequest()
							  ? (ActionResult)PartialView(lead)
							  : View(lead);
			}
			catch (Exception ex)
			{
				return View();
			}
		}

		private void FillCombo(int ServiceLnID)
		{
			ViewData["CityList"] = comboBL.GetCityDropdown(ContinentID: -1, CountryID: -1);
			ViewData["RMCList"] = comboBL.GetRMCDropdown();
			ViewData["ShipperTypeList"] = comboBL.GetShipperTypeDropdown();
			ViewData["ShipperCategoryList"] = comboBL.GetShipperCategoryDropdown();
			ViewData["ShippingLineList"] = comboBL.GetShippingLineDropdown("1");
			ViewData["ContainerList"] = comboBL.GetContainerSizeDropdown();
			ViewData["ModeList"] = comboBL.GetModeDropdown(ServiceLineID: ServiceLnID);
			ViewData["RevenueBranchList"] = comboBL.GetCompanyBranchDropdown(IsRMCBuss:true,IsRev:true);
			ViewData["AccountList"] = comboBL.GetAgentDropdown(CORA: "C");
			ViewData["MoveCoordinatorList"] = comboBL.GetEmployeeDropdown(IsMoveCordination:true);
			ViewData["UnitList"] = comboBL.GetMeasurementUnitDropdown(UnitType: 'B');
		}

		// GET: Lead/Edit/5
		public ActionResult Edit(int id)
		{
			return View();
		}

		// POST: Lead/Edit/5
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

		// GET: Lead/Delete/5
		public ActionResult Delete(int id)
		{
			return View();
		}

		// POST: Lead/Delete/5
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

		[HttpPost]
		public ActionResult CreateJob(LeadViewModel lead)
		{
			try
			{
				FillCombo(lead.manageDet.ServiceLnId);
				//if (ModelState.IsValid)
				//{
					RELOCBS.AjaxHelper.AjaxResponse result = new AjaxResponse();
					string Message = string.Empty;
					Int64 MoveId = 0;

					result.Success = leadBL.InsertJob(lead, out Message, out MoveId);

					if (!result.Success)
					{
						ModelState.AddModelError(string.Empty, "Unable to Create Job.");
						result.Message = Message;

						this.AddToastMessage("RELOCBS", Message, ToastType.Error);
						return RedirectToAction("Create");
					}
					else
					{

						/*string FolderPath = ConfigurationManager.AppSettings["OFSDocument"].ToString();
						if (!Directory.Exists(FolderPath))
						{
							Directory.CreateDirectory(FolderPath);
						}

						string FileExtension = Path.GetExtension(lead.manageDet.OFSDocument.FileName);
						string FileName = "OFSDocument_" + MoveId + FileExtension;
						string FilePath = Path.Combine(FolderPath, FileName);
						lead.manageDet.OFSDocument.SaveAs(Server.MapPath(FilePath));*/
						result.Message = Message;

						int totalCount = 0;
						//lead.LeadGrid = leadBL.GetForGrid(lead.RMCID, lead.FromCityID, lead.ToCityID, lead.IsRoad, null, null, null, 5, 1, out totalCount).ToList();
						this.AddToastMessage("RELOCBS", Message, ToastType.Success);
						return RedirectToAction("Index", "MoveManage");
					}
				//}
				//else
				//{
				//	ViewBag.InValid = true;
				//	return RedirectToAction("Create");
				//	//return (ActionResult)PartialView("_PartialGrid", lead);
				//	//return (ActionResult)View("Create", lead);
				//}

				//return RedirectToAction("Index", "Pricing", new { OriginId = lead.FromCityID, DestinationId = lead.ToCityID, RMCId = lead.RMCID });

				return Request.IsAjaxRequest()
								  ? (ActionResult)PartialView(lead)
								  : View(lead);
			}
			catch (Exception ex)
			{
				return (ActionResult)PartialView("_PartialGrid", lead);
			}
		}

		public JsonResult GetRevenuelist(int? RMCID=0)
		{
			//RMCID;
			bool IsRMCBuss = !(UserSession.GetUserSession().BussinessLine == "NON RMC-BUSINESS");
			List<SelectListItem> RevenueList = comboBL.GetCompanyBranchDropdown(IsRMCBuss: IsRMCBuss, IsRev: true, RMCID: RMCID).ToList(); ;
			return Json(new { RevenueList = RevenueList }, JsonRequestBehavior.AllowGet);
		}

		public JsonResult GetRMCType(int? RMCID = 0)
		{
			//RMCID;
			//bool IsRMCBuss = !(UserSession.GetUserSession().BussinessLine == "NON RMC-BUSINESS");
			string RMCType = leadBL.GetRMCType(RMCID: RMCID);
			return Json(new { RMCType = RMCType }, JsonRequestBehavior.AllowGet);
		}

		//public JsonResult GetCityList(string term)
		//{
		//	Dictionary<int, string> dictCountry = new Dictionary<int, string>();
		//	dictCountry.Add(1, "India");
		//	dictCountry.Add(2, "Canada");
		//	dictCountry.Add(3, "United States");
		//	dictCountry.Add(4, "United Kingdom");
		//	dictCountry.Add(5, "United Arab Emirates");

		//	List<SelectListItem> CountryList = new List<SelectListItem>();
		//	SelectListItem singleCountry;

		//	var selectedCountryList = dictCountry.Where(m => m.Value.ToLower().Contains(term.ToLower())).ToList();
		//	foreach (var selectedCountry in selectedCountryList)
		//	{
		//		singleCountry = new SelectListItem();
		//		singleCountry.Value = selectedCountry.Key.ToString();
		//		singleCountry.Text = selectedCountry.Value;
		//		CountryList.Add(singleCountry);
		//	}

		//	return Json(new { CountryList=CountryList }, JsonRequestBehavior.AllowGet);
		//}
	}
}
