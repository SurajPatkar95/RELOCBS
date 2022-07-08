using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RELOCBS.Entities;
using RELOCBS.BL;
using RELOCBS.CustomAttributes;
using System.Web.Helpers;
using System.Data;
using System.Web.Services;
using RELOCBS.Utility;
using RELOCBS.Services.Implementation;
using RELOCBS.App_Code;
using RELOCBS.BL.Enquiry;
using PagedList;
using RELOCBS.Extensions;
using RELOCBS.Common;
using System.Xml.Linq;
using Newtonsoft.Json;
using RELOCBS.AjaxHelper;

namespace RELOCBS.Controllers
{
	[AuthorizeUser]
	public class EnquiryController : BaseController
	{
		private string _PageID = "8";
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

		private EnquiryBL _enquiryBL;
		public EnquiryBL enquiryBL
		{
			get
			{
				if (this._enquiryBL == null)
					this._enquiryBL = new EnquiryBL();
				return this._enquiryBL;
			}
		}
		Enquiry _enquiry = new Enquiry();

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
		ComboBL combo = new ComboBL();

		private CommanBL _comBL;
		public CommanBL comBL
		{
			get
			{
				if (this._comBL == null)
					this._comBL = new CommanBL();
				return this._comBL;
			}
		}
		Status objstatus = new Status();


		public EnquiryController(Enquiry Enquiry)
		{
			_enquiry = Enquiry;
		}


		// GET: Enquiry
		public ActionResult Index(int page = 1)
		{
			if (!CSubs.CheckPageRights(_PageID, PermissionType.VIEW))
			{
				return new HttpStatusCodeResult(403);
			}

			session.Set<string>("PageSession", "Enquiry");
			string sort = "EnqID";
			string sortdir = "desc";
			string search = "";
			string EnqID = "";
			int pageSize = settings.GetSettingByKey<int>("pagination_pagesize");
			string OrderBy = "";
			int Order = 0;
			DateTime? Fromdate = null;//System.DateTime.Now.Date.AddDays(-2);
			DateTime? Todate = null;//System.DateTime.Now;
			string Shipper = "";

			string SearchKey = string.Empty;
			//if (Request.Form["FromDate"] != null && Request.Form["FromDate"].Trim() != "")
			//{
			//    Fromdate = Convert.ToDateTime(Request.Form["FromDate"]);
			//}
			if (Request.Form["EnqID"] != null && Request.Form["EnqID"].Trim() != "")
			{
				EnqID = Convert.ToString(Request.Form["EnqID"]);
			}
			//if (Request.Form["ToDate"] != null && Request.Form["ToDate"].Trim() != "")
			//{
			//    Todate = Convert.ToDateTime(Request.Form["ToDate"]);
			//}

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
			var data = enquiryBL.GetenquiryList(Fromdate, Todate, sort, sortdir, skip, pageSize, out totalRecord, EnqID, Shipper);
			if (data != null)
			{
				ViewBag.TotalRows = totalRecord;
				ViewBag.search = search;

				var itemsAsIPagedList = new StaticPagedList<Enquiry>(data, page, pageSize, totalRecord);

				return Request.IsAjaxRequest()
					? (ActionResult)PartialView("_GridPartial", itemsAsIPagedList)
					: View(itemsAsIPagedList);
			}
			else
			{
				return Request.IsAjaxRequest()
					? (ActionResult)PartialView("_GridPartial")
					: View();
			}
		}

		public ActionResult Create(string Key)
		{
			var list = CommonService.GetQueryString(Key);

			int EnqId = Convert.ToInt32(list["EnqID"]);
			_enquiry.EnqID = EnqId;
			IEnumerable<EnquiryDetail> empList = new List<EnquiryDetail>();
			DataTable EnqDt = new DataTable();
			_enquiry = enquiryBL.GetDetailById(0, out empList, out EnqDt, EnqId);
			GetDropDownList(_enquiry.RMCBuss, _enquiry.Designation);
			objstatus = comBL.GetStatusById(EnqId, "Enquiry", "Main");
			_enquiry.EnqStatus = objstatus.StatusName;
			_enquiry.StatusDate = objstatus.StatusDate;
			session.Set<DataTable>("ReportSession", EnqDt);
			ClientDetails _clientDet = new ClientDetails();
			if (_enquiry.ClientDetails != null)
			{
				_clientDet = ClientDetailBL.GetClientDetail(_enquiry.ClientDetails.Client, 'C');
				if (_enquiry.ChangeAcctMgrID == null || _enquiry.ChangeAcctMgrID <= 0)
				{
					_enquiry.ClientDetails.AccountMgr = _clientDet.AccountMgr;

				}
				_enquiry.ClientDetails.AccountGSTNO = _clientDet.AccountGSTNO;
				_clientDet = ClientDetailBL.GetClientDetail(_enquiry.ClientDetails.Client, 'A');
				_enquiry.ClientDetails.ClientGSTNO = _clientDet.ClientGSTNO;
			}
			ViewBag.EnquiryDetailList = empList;
			_enquiry.CompId = UserSession.GetUserSession().CompanyID;
			return View(_enquiry);
		}
		[HttpPost]
		public ActionResult Create(Enquiry Enquiry, string EnqId)
		{
			Enquiry.EnqID = Convert.ToInt32(EnqId);
			GetDropDownList(Enquiry.RMCBuss, Enquiry.Designation);
			_enquiry = Enquiry;
			return View(_enquiry);
		}

		[HttpGet]
		public ActionResult Save(Enquiry enquiry)
		{
			GetDropDownList(enquiry.RMCBuss, enquiry.Designation);
			FillAddGrid(enquiry.EnquiryDetail);
			return View("Create", enquiry);
		}

		[HttpPost]
		public ActionResult Save(Enquiry enquiry, string GridList)
		{
			GetDropDownList(enquiry.RMCBuss, enquiry.Designation);
			string message = string.Empty;

			bool AccMgr_IsValid = (!string.IsNullOrEmpty(enquiry.ClientDetails.AccountMgr) || enquiry.ChangeAcctMgrID != null);
			bool IsLost = true;
			if (!string.IsNullOrEmpty(enquiry.LostRemarks) || !string.IsNullOrEmpty(enquiry.LostTo) || enquiry.LostReasonID != null)
			{
				IsLost = (!string.IsNullOrEmpty(enquiry.LostRemarks) || !string.IsNullOrEmpty(enquiry.LostTo))?enquiry.LostReasonID != null : true ;
			}

			bool IsLastName = true; bool IsEmail = true; bool IsCity = true; bool IsFirstName = true;
			if (enquiry.CompId == 2)
			{
				IsLastName = !string.IsNullOrEmpty(enquiry.ShipperLName);
				IsFirstName = !string.IsNullOrEmpty(enquiry.ShipperFName);
				IsEmail = !string.IsNullOrEmpty(enquiry.Email);
				IsCity = !string.IsNullOrEmpty(Convert.ToString(enquiry.AddressCityID));
			}
			
            if (ModelState.IsValid && AccMgr_IsValid && IsLost && IsLastName && IsEmail && IsCity && IsFirstName)
            {
                bool res = false;

                res = enquiryBL.Insert(enquiry, out message);
                if (!res)
                {
                    ModelState.AddModelError(string.Empty, "Unable to save Enquiry data.");
                    this.AddToastMessage("RELOCBS", message, ToastType.Error);
                    //return Json(result);
                }
                else
                {
                    this.AddToastMessage("RELOCBS", message, ToastType.Success);
                    //return Json(result);
                }
                //ViewBag.Result = result;
                //return RedirectToAction("create/"+data.EnquiryDetail.EnqDetailID);
                return RedirectToAction("Create", new { Key = CommonService.GetCrypt("EnqID="+enquiry.EnqID.ToString(),1) });
            }
            else
            {
				enquiry.RevenueBr = ((IEnumerable<SelectListItem>)ViewData["RevenueBranchList"]).Where(x => x.Value == enquiry.RevenueBrId.ToString()).ToList().First().Text;

				if (!string.IsNullOrEmpty(enquiry.EnquiryListHidden))
                {

                    ViewBag.EnquiryDetailList = JsonConvert.DeserializeXNode(enquiry.EnquiryListHidden, "EnqDetails");
					//(XDocument.Parse(enquiry.EnquiryListHidden)).Descendants();//.ToList<EnquiryDetail>(
					
					ViewBag.EnquiryDetailList = (from r in JsonConvert.DeserializeXNode(enquiry.EnquiryListHidden, "EnqDetails").Root.Elements("EnqDetail")
                                                 select new EnquiryDetail
                                                 {
                                                     EnqDetailID = string.IsNullOrEmpty((string)r.Element("EnqDetailID"))? 0 :(long)r.Element("EnqDetailID"),
													 EnqID = (long)r.Element("EnqID"),
													 HandlingBrId = (int?)r.Element("HandlingBrId"),
													 HandlingBr = (string)r.Element("HandlingBr"),
													 ServiceLineID = (int?)r.Element("ServiceLineID"),
													 ServiceLine = (string)r.Element("ServiceLine"),
													 //((IEnumerable<SelectListItem>)ViewData["ServiceLineList"]).Where(x => x.Value == r.Element("ServiceLineID").Value).ToList().First().Text,
													 FromCity = (int?)r.Element("FromCity"),
													 FCity = (string)r.Element("FCity"),//((IEnumerable<SelectListItem>)ViewData["OriCityList"]).Where(x => x.Value == r.Element("FromCity").Value).ToList().First().Text,
													 ToCityID = (int?)r.Element("ToCityID"),
													 ToCity = (string)r.Element("ToCity"),
													 //ToCity = ((IEnumerable<SelectListItem>)ViewData["DesCityList"]).Where(x => x.Value == r.Element("ToCityID").Value).ToList().First().Text,

													 Mode = (int?)r.Element("ModeID"),
													 TMode = (string)r.Element("Mode"),
													 ShipmentTypeID = (int?)r.Element("ShipmentTypeID"),
													 ShipmentType = (string)r.Element("ShipmentType"),
													 //ShipmentType = (combo.GetShipmentTypeDropdown(ServiceLineID: Convert.ToInt32(r.Element("ServiceLineID").Value)).ToList()).Where(x => x.Value == (string)r.Element("ShipmentTypeID")).ToList().First().Text,
													 GoodsDescId = (int?)r.Element("GoodsDescId"),
													 GoodsDesc = (string)r.Element("GoodsDesc"),
													 //GoodsDesc = (combo.GetGoodsDescriptionDropdown(ServiceLineID: Convert.ToInt32(r.Element("ServiceLineID").Value)).ToList()).Where(x => x.Value == (string)r.Element("GoodsDescId")).ToList().First().Text,
													 WtUnitid = string.IsNullOrEmpty((string)r.Element("WtUnitid")) ? null : (int?)r.Element("WtUnitid"),
													 //WtUnit = string.IsNullOrEmpty((string)r.Element("WtUnitid")) ? null : ((IEnumerable<SelectListItem>)ViewData["WeightUnitList"]).Where(x => x.Value == (string)r.Element("WtUnitid")).ToList().First().Text,
													 WtNet = string.IsNullOrEmpty((string)r.Element("WtNet")) ? 0 : (decimal?)r.Element("WtNet"),
													 WtGross = string.IsNullOrEmpty((string)r.Element("WtGross")) ? 0 : (decimal?)r.Element("WtGross"),
													 WtACWT = string.IsNullOrEmpty((string)r.Element("WtACWT")) ? 0 : (decimal?)r.Element("WtACWT"),
													 VolumeUnitID = string.IsNullOrEmpty((string)r.Element("VolumeUnitid")) ? null : (int?)r.Element("VolumeUnitid"),
													 //VolumeUnit = string.IsNullOrEmpty((string)r.Element("VolumeUnitid")) ? null : ((IEnumerable<SelectListItem>)ViewData["VolumeUnitList"]).Where(x => x.Value == (string)r.Element("VolumeUnitid").Value).ToList().First().Text,
													 VolumeToPack = string.IsNullOrEmpty((string)r.Element("VolumeToPack")) ? 0 : (decimal?)r.Element("VolumeToPack"),
													 VolumeNet = string.IsNullOrEmpty((string)r.Element("VolumeNet")) ? 0 : (decimal?)r.Element("VolumeNet"),
													 VolumeGross = string.IsNullOrEmpty((string)r.Element("VolumeGross")) ? 0 : (decimal?)r.Element("VolumeGross"),
													 LooseCased = (string)r.Element("LooseCased"),
													 DensiyFactor = string.IsNullOrEmpty((string)r.Element("DensityFact")) ? 0 : (decimal?)r.Element("DensityFact"),
													 LCLFCL = (string)r.Element("LCLorFCL"),
													 ContainerTypeID = string.IsNullOrEmpty((string)r.Element("ContainerTypeID")) ? null : (int?)r.Element("ContainerTypeID"),
													 //ContainerType = string.IsNullOrEmpty((string)r.Element("ContainerTypeID")) ? null : ((IEnumerable<SelectListItem>)ViewData["ContainerList"]).Where(x => x.Value == (string)r.Element("ContainerTypeID")).ToList().First().Text,
													 Remarks = (string)r.Element("Remarks"),
													 TentativeMovedate = (DateTime?)r.Element("TentativeMovedate"),
													 SchSurveyorID = string.IsNullOrEmpty((string)r.Element("SchSurveyorID")) ? null : (int?)r.Element("SchSurveyorID"),
													 //SchSurveyor = string.IsNullOrEmpty((string)r.Element("SchSurveyorID")) ? null : Convert.ToInt32(r.Element("SchSurveyorID"))>0?((IEnumerable<SelectListItem>)ViewData["SurveyorList"]).Where(x => x.Value == (string)r.Element("SchSurveyorID")).ToList().First().Text : null,
													 SchSurveyorRemark = (string)r.Element("SchSurveyorRemarks"),
													 SchSurveyDate = string.IsNullOrEmpty((string)r.Element("SchSurveyDate")) ? null : (DateTime?)r.Element("SchSurveyDate"),
												 }).ToList();
                    
                }

				if (!AccMgr_IsValid)
				{
					ModelState.AddModelError(string.Empty, "Please select Account Manager.");
				}

				if (!IsLost)
				{
					ModelState.AddModelError(string.Empty, "Please select Enquiry Reason.");
				}
				if (!IsLastName)
				{
					ModelState.AddModelError(string.Empty, "Please enter Shipper Last Name.");
				}
				if (!IsCity)
				{
					ModelState.AddModelError(string.Empty, "Please select Shipper City.");
				}
				if (!IsEmail)
				{
					ModelState.AddModelError(string.Empty, "Please enter Shipper Email.");
				}
				if (!IsFirstName)
				{
					ModelState.AddModelError(string.Empty, "Please enter Shipper First Name.");
				}
				
				return View("Create", enquiry);
            }
        }

        public JsonResult GetaJAXClientDetails(int ClientId, char Mode)
        {
            return Json(new { result = ClientDetailBL.GetClientDetail(ClientId, Mode) }, JsonRequestBehavior.AllowGet);
        }

   //     public string GetClientDetails(int ClientId, char Mode)
   //     {
			//string result
   //         ClientDetails _clientDet = new ClientDetails();
   //         _clientDet = ClientDetailBL.GetClientDetail(ClientId,Mode);
   //         return Mode.Equals('A')_clientDet.AccountMgr;
   //     }

        private void GetDropDownList(bool RMCBuss,string Designation)
        {
			RMCBuss = !(UserSession.GetUserSession().BussinessLine == "NON RMC-BUSINESS");
			ViewData["MoveQuoteList"] = combo.GetMoveQuoteClassDropdown();
            ViewData["EnqSourceList"] = combo.GetEnqInfoSourceDropdown();
            ViewData["BussinessLineList"] = combo.GetBusinessLineDropdown();
			ViewData["ClientList"] = combo.GetAgentDropdown(CORA:RMCBuss ? "R" : null);
			ViewData["AgentList"] = combo.GetAgentDropdown(CORA: "A");
            ViewData["AccountList"] = combo.GetAgentDropdown(CORA:"C");
            ViewData["RevenueBranchList"] = combo.GetCompanyBranchDropdown(IsRMCBuss:RMCBuss);
			ViewData["HandlingBranchList"] = combo.GetCompanyBranchDropdown();
			ViewData["AcctMgrList"] = combo.GetEmployeeDropdown();
            ViewData["ShipperTypeList"] = combo.GetShipperTypeDropdown();
            ViewData["ShipperCategoryList"] = combo.GetShipperCategoryDropdown();
            ViewData["DesCityList"] = combo.DestinationCityDropdown();
            ViewData["OriCityList"] = combo.OriginCityDropdown();
            ViewData["ServiceLineList"] = combo.GetServiceLineDropdown(RMCBuss:RMCBuss);
            ViewData["LostReasonList"] = combo.GetEnquiryLostReasonDropdown();
            ViewData["LostToList"] = combo.GetCompanyDropdown();
			ViewData["ModeList"] = combo.GetModeDropdown();
			ViewData["ShipmentTypeList"] = combo.GetShipmentTypeDropdown();
			ViewData["CommodityList"] = combo.GetGoodsDescriptionDropdown();
            ViewData["SurveyorList"] = combo.GetEmployeeDropdown();
            ViewData["ContainerList"] = combo.GetContainerSizeDropdown();
            ViewData["VolumeUnitList"] = combo.GetMeasurementUnitDropdown('V');
            ViewData["WeightUnitList"] = combo.GetMeasurementUnitDropdown('W');
			ViewData["NationalityList"] = combo.GetNationalityDropDown();
			ViewData["DesignationList"] = combo.getShipperDesignationDropdown();
			ViewData["TitleList"] = combo.GetTitleDropdown();
			int DesigCount = combo.getShipperDesignationDropdown().Where(x => x.Value == Designation).Count();

			if (DesigCount <=0 && !string.IsNullOrEmpty(Designation))
			{
				ViewBag.Desig = "Others";
			}
			else
			{
				ViewBag.Desig = Designation;
			}
		}

        private void FillAddGrid(EnquiryDetail EnquiryDet)
        {
            List<EnquiryDetail> a = new List<EnquiryDetail>();
            List<WebGridColumn> columns = new List<WebGridColumn>();
            if (EnquiryDet != null)
            {
                a.Add(EnquiryDet);
            }

            columns.Add(new WebGridColumn() { ColumnName = "HandlingBrId", Header = "HandlingBrId" });
            columns.Add(new WebGridColumn() { ColumnName = "ServiceLineID", Header = "ServiceLineID" });
            columns.Add(new WebGridColumn() { ColumnName = "FromCity", Header = "FromCity" });
            //columns.Add(new WebGridColumn() { Format = (item) => { return new HtmlString(string.Format("<a href= {0}>View</a>", Url.Action("Edit", "Edit", new { Id = item.Id }))); } });
            columns.Add(new WebGridColumn() { Format = (item) => { return new HtmlString(string.Format("<a href= 'javascript:void(0)'>View</a>")); } });
            ViewBag.Columns = columns;
            ViewBag.GridList = (IEnumerable<EnquiryDetail>)a;
        }
        [HttpPost]
        public ActionResult Survey(string EnquiryDetailId)
        {
            string RedirectUrl = string.Empty;
            if (string.IsNullOrEmpty(RedirectUrl))
            {
                RedirectUrl = "/Survey/Create?EnqDetailID=" + Convert.ToInt32(EnquiryDetailId);
            }
            return Json(new { redirectUrl = RedirectUrl });
        }

        public ActionResult GetReport(int EnqID)
        {
            EncryptedQueryString query = new EncryptedQueryString();
            query["PageID"] = _PageID;
            query["ReportID"] = "10001";
            query["EnqID"] =  Convert.ToString(EnqID).Trim();

            return this.RedirectSameDomain("/Reports/ReportViewer.aspx?args=" + query.ToString());
            //return Redirect("/Reports/ReportViewer.aspx");
        }

		public JsonResult GetaJAXListBasedOnServiceLine(int ServiceLine)
		{
			List<SelectListItem> ModeList = combo.GetModeDropdown(ServiceLineID: ServiceLine).ToList();
			List<SelectListItem> ShipmentTypeList = combo.GetShipmentTypeDropdown(ServiceLineID: ServiceLine).ToList();
			List<SelectListItem> CommodityList = combo.GetGoodsDescriptionDropdown(ServiceLineID: ServiceLine).ToList();
			string Project = enquiryBL.GetProjectByServiceLine(ServiceLineID: ServiceLine);
			return Json(new { ModeList = ModeList, ShipmentTypeList= ShipmentTypeList, CommodityList= CommodityList, Project=Project }, JsonRequestBehavior.AllowGet);
		}

		public JsonResult GetListBasedOnMode(int? Mode)
		{
			List<SelectListItem> LoosedCasedList = combo.LoosedCasedDropDown(ModeID: Mode).ToList();
			return Json(new { LoosedCasedList = LoosedCasedList,IsStorage = Mode == 4 }, JsonRequestBehavior.AllowGet);
		}
		public JsonResult GetListBasedOnLoosedCased(string LOOSEDCASED,int? mode)
		{
			List<SelectListItem> LCLFCLList = combo.LCLFCLDropDown(LOOSEDCASED: LOOSEDCASED,Mode:mode).ToList();
			return Json(new { LCLFCLList = LCLFCLList, IsStorage = mode == 4}, JsonRequestBehavior.AllowGet);
		}


		public ActionResult SentToMobile(int? EnqID, int? EnqDetailID)
		{
			try
			{
				string message = string.Empty;
				bool res = enquiryBL.SentToMobile(EnqID, EnqDetailID, out message);
				if (!res)
				{
					//ModelState.AddModelError(string.Empty, "Unable to save Enquiry data.");
					this.AddToastMessage("RELOCBS", message, ToastType.Error);
					//return Json(result);
				}
				else
				{
					this.AddToastMessage("RELOCBS", message, ToastType.Success);
					//return Json(result);
				}
			}
			catch (Exception)
			{
				this.AddToastMessage("RELOCBS", "Error in Sending", ToastType.Error);
			}
			
			return RedirectToAction("Create", new { Key = CommonService.GetCrypt("EnqID=" + EnqID.ToString(), 1) });
			//return RedirectToAction("Create", "Enquiry", new { EnqID = EnqID });
		}

		public ActionResult SaveFollowUpDetails(Enquiry ViewData)
		{
			bool res = false;
			string message = string.Empty;
			AjaxResponse result = new AjaxResponse();
			ModelState.Remove("EnquiryListHidden");
			ModelState.Remove("TentativeDate");
			ModelState.Remove("ContactDate");
			ModelState.Remove("AgentID");
			ModelState.Remove("ShipTypeID");
			ModelState.Remove("ShipCategoryID");
			ModelState.Remove("ShipCategoryID");
			ModelState.Remove("EnqSourceID");
			ModelState.Remove("MoveQuoteID");
			ModelState.Remove("RevenueBrId");
			
			if (ModelState.IsValid)
			{
				res = enquiryBL.InsertFollowUpDetials(ViewData, out message);
				if (!res)
				{
					result.Success = false;
					ModelState.AddModelError(string.Empty, "Unable to save Follow Up.");
					result.Message = message;
					this.AddToastMessage("RELOCBS", message, ToastType.Error);
					//return Json(result);
				}
				else
				{
					result.Success = true;
					result.Message = message;
					this.AddToastMessage("RELOCBS", message, ToastType.Success);
				}
				return RedirectToAction("Create", new { Key = CommonService.GetCrypt("EnqID=" + ViewData.EnqID.ToString(), 1) });
			}
			else
			{
				return Request.IsAjaxRequest() ? (ActionResult)PartialView("Create", ViewData)
				  : View("Create", ViewData);
			}
		}
	}
}