using PagedList;
using RELOCBS.AjaxHelper;
using RELOCBS.App_Code;
using RELOCBS.BL;
using RELOCBS.BL.Enquiry;
using RELOCBS.BL.MoveMange;
using RELOCBS.BL.Survey;
using RELOCBS.Common;
using RELOCBS.CustomAttributes;
using RELOCBS.Entities;
using RELOCBS.Extensions;
using RELOCBS.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace RELOCBS.Controllers
{
    [AuthorizeUser]
    public class MoveManageController : BaseController
    {
        private string _PageID = "10";

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

        private MoveManageBL _moveManageBL;
        public MoveManageBL moveManageBL
        {
            get
            {
                if (this._moveManageBL == null)
                    this._moveManageBL = new MoveManageBL();
                return this._moveManageBL;

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

        private CommanBL _commanBL;
        public CommanBL commanBL
        {
            get
            {
                if (this._commanBL == null)
                    this._commanBL = new CommanBL();
                return this._commanBL;

            }
        }

        private BL.Enquiry.ClientDetailBL _ClientDetailBL;
        public BL.Enquiry.ClientDetailBL ClientDetailBL
        {
            get
            {
                if (this._ClientDetailBL == null)
                    this._ClientDetailBL = new BL.Enquiry.ClientDetailBL();
                return this._ClientDetailBL;

            }
        }

        // GET: MoveManage
        public ActionResult Index(int page = 1)
        {
            //_PageID = "10";
            if (!CSubs.CheckPageRights(_PageID, PermissionType.VIEW))
            {
                return new HttpStatusCodeResult(403);
            }
            session.Set<string>("PageSession", "Job Opening");
            string sort = "";
            string sortdir = "";
            string search = "";
            string searchType = "";
            int pageSize = settings.GetSettingByKey<int>("pagination_pagesize");
            string OrderBy = "";
            int Order = 0;
            DateTime? Fromdate = null;
            DateTime? Todate = null;
            bool IsJobDate = false;
            string Shipper = null;
            string SearchKey = "";
            bool RMCBuss = !UserSession.GetUserSession().BussinessLine.Equals("NON RMC-BUSINESS");
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
            if (Request.Form["Isjob"] != null && Request.Form["Isjob"].Trim() != "")
            {
                IsJobDate = Convert.ToBoolean(Request.Form["Isjob"]);
            }
            else
            {
                IsJobDate = RMCBuss;
            }
            if (Request.Params["Shipper"] != null && Request.Params["Shipper"].Trim() != "")
            {
                Shipper = Request.Params["Shipper"].Trim().ToString();
            }
            int totalRecord = 0;
            if (page < 1) page = 1;
            int skip = (page * pageSize) - pageSize;
            ViewData["JobGridSearchList"] =comboBL.GetJobGridSearchList();
            var data = moveManageBL.GetForGrid(search, searchType, Shipper, sort, sortdir, skip, pageSize, out totalRecord, out RMCBuss);

            ViewBag.TotalRows = totalRecord;
            ViewBag.RMCBuss = RMCBuss;
            ViewBag.search = search;
            var itemsAsIPagedList = new StaticPagedList<MoveManageGrid>(data, page, pageSize, totalRecord);

            return Request.IsAjaxRequest()
                ? (ActionResult)PartialView("_AjaxPartial", itemsAsIPagedList)
                : View(itemsAsIPagedList);
        }

        // GET: MoveManage/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: MoveManage/Create
        public ActionResult Create(int Indx, int SurveyID, int? MoveID, bool IsGetCost = false)
        {
            MoveManageViewModel MoveViewModel = new MoveManageViewModel();
            try
            {
                TempData.Keep("SaveType");

                List<PackingSOList> PackingSOList = new List<PackingSOList>();
                bool IsSurveyGetCost = Indx == 1 && IsGetCost;
                bool IsPackingGetCost = Indx == 2 && IsGetCost;
                bool IsDeliveryGetCost = Indx == 4 && IsGetCost;
                MoveViewModel = moveManageBL.GetDetailById(SurveyID, MoveID, IsSurveyGetCost, IsPackingGetCost, IsDeliveryGetCost);
                MoveViewModel.dsCostSheet = moveManageBL.GetCostSheet(MoveViewModel.MoveID);
                //MoveViewModel.EmailActivityhistory = moveManageBL.GetMailActivityHistory(MoveViewModel.MoveID); 
                if (MoveViewModel.RMCBuss && MoveViewModel.MoveJob.ClientId <= 0)
                {
                    MoveViewModel.MoveJob.ClientId = commanBL.GetClientByRMC(MoveViewModel.RMCBuss, MoveViewModel.MoveJob.RMCID);
                }
                MoveViewModel.TabIndex = Indx;
                ClientDetails ClientDet = ClientDetailBL.GetClientDetail(Convert.ToInt32(MoveViewModel.MoveJob.ClientId), 'A');
                MoveViewModel.MoveJob.ClientGSTNO = ClientDet.ClientGSTNO;
                ClientDetails AccountDet = ClientDetailBL.GetClientDetail(Convert.ToInt32(MoveViewModel.MoveJob.AccountId), 'C');
                MoveViewModel.MoveJob.AccountGSTNO = AccountDet.AccountGSTNO;
                List<int> RateCompForSurveyCost = new List<int>();
                if (MoveViewModel.MoveJob.ModeID != 3)
                {
                    if (MoveViewModel.SurveyDetail.OrgAgentID != null && MoveViewModel.SurveyDetail.OrgAgentID > 0)
                        RateCompForSurveyCost.Add(Convert.ToInt32(RELOCBS.Common.CommonService.RateComp.Origin));
                    if (MoveViewModel.SurveyDetail.FrtAgentID != null && MoveViewModel.SurveyDetail.FrtAgentID > 0)
                        RateCompForSurveyCost.Add(Convert.ToInt32(RELOCBS.Common.CommonService.RateComp.Freight));
                    if (MoveViewModel.SurveyDetail.DestAgentID != null && MoveViewModel.SurveyDetail.DestAgentID > 0)
                        RateCompForSurveyCost.Add(Convert.ToInt32(RELOCBS.Common.CommonService.RateComp.Destination));
                }
                else
                {
                    RateCompForSurveyCost.Add(Convert.ToInt32(RELOCBS.Common.CommonService.RateComp.Origin));
                    RateCompForSurveyCost.Add(Convert.ToInt32(RELOCBS.Common.CommonService.RateComp.Freight));
                    RateCompForSurveyCost.Add(Convert.ToInt32(RELOCBS.Common.CommonService.RateComp.Destination));
                }
                TempData["SubCostList"] = null;
                string SubCostDiv = string.Empty;
                foreach (var item in MoveViewModel.SurveyCostList.CostList)
                {
                    item.IsSubCost = new BL.CommanBL().IsSubCostHead(Convert.ToInt32(item.CostHeadID));
                    SubCostDiv += GetSubCostHeadList(Convert.ToInt32(item.CostHeadID), item.RateCompId, Convert.ToInt32(MoveViewModel.SurveyID),
                        0, 0, 1, 0, Convert.ToInt32(MoveViewModel.MoveID));
                }
                foreach (var item in MoveViewModel.PackingCostList.CostList)
                {
                    item.IsSubCost = new BL.CommanBL().IsSubCostHead(Convert.ToInt32(item.CostHeadID));
                    SubCostDiv += GetSubCostHeadList(Convert.ToInt32(item.CostHeadID), item.RateCompId, Convert.ToInt32(MoveViewModel.SurveyID),
                        0, 0, 1, 0, Convert.ToInt32(MoveViewModel.MoveID));
                }
                foreach (var item in MoveViewModel.DeliveryCostList.CostList)
                {
                    item.IsSubCost = new BL.CommanBL().IsSubCostHead(Convert.ToInt32(item.CostHeadID));
                    SubCostDiv += GetSubCostHeadList(Convert.ToInt32(item.CostHeadID), item.RateCompId, Convert.ToInt32(MoveViewModel.SurveyID),
                        0, 0, 1, 0, Convert.ToInt32(MoveViewModel.MoveID));
                }
                ViewBag.SubCostList = SubCostDiv;
                if (MoveViewModel.RMCType == "Other Type")
                {
                    int BaseCurrrID = commanBL.GetBaseCurrByRMC(UserSession.GetUserSession().BussinessLine != "NON RMC-BUSINESS", MoveViewModel.MoveJob.RMCID, UserSession.GetUserSession().CompanyID, UserSession.GetUserSession().LoginID, MoveID);
                    string BaseCurr = comboBL.GetCurrencyDropdown().Where(x => x.Value == BaseCurrrID.ToString()).First().Text;
                    if (MoveViewModel.SurveyCostList.CostList.Count > 0)
                    {
                        MoveViewModel.SurveyCostList.CostList.First().BaseCurrID = BaseCurrrID;
                        MoveViewModel.SurveyCostList.CostList.First().BaseCurr = BaseCurr;
                    }
                    if (MoveViewModel.PackingCostList.CostList.Count > 0)
                    {
                        MoveViewModel.PackingCostList.CostList.First().BaseCurrID = BaseCurrrID;
                        MoveViewModel.PackingCostList.CostList.First().BaseCurr = BaseCurr;
                    }
                    if (MoveViewModel.FreightCostList.CostList.Count > 0)
                    {
                        MoveViewModel.FreightCostList.CostList.First().BaseCurrID = BaseCurrrID;
                        MoveViewModel.FreightCostList.CostList.First().BaseCurr = BaseCurr;
                    }
                    if (MoveViewModel.DeliveryCostList.CostList.Count > 0)
                    {
                        MoveViewModel.DeliveryCostList.CostList.First().BaseCurrID = BaseCurrrID;
                        MoveViewModel.DeliveryCostList.CostList.First().BaseCurr = BaseCurr;
                    }
                }

                ModelState.Remove("PassportNo");

                int LoginID = UserSession.GetUserSession().LoginID;

                int? SFTemplateID = null;
                if (UserSession.GetUserSession().CompanyID == 2)//BTR Shipper Feedback
                    SFTemplateID = 1;
                else if (MoveViewModel.MoveJob.BusinessLineName  == "LOCAL")//Shipper Feedback - INDIA NON RMC
                    SFTemplateID = 2;
                else if (MoveViewModel.MoveJob.BusinessLineName == "AGENT")//Agent Feedback - INDIA NON RMC
                    SFTemplateID = 3;

                MoveViewModel.ShipperFeedback = moveManageBL.GetShipperFeedbackTemplate(LoginID, MoveID, SFTemplateID, null, true);

                MoveViewModel.ShipperFeedback.MoveID = MoveViewModel.MoveID;

                FillCombo(Convert.ToString(MoveViewModel.MoveJob.ModeID), MoveViewModel.RMCBuss, RateCompForSurveyCost, MoveViewModel.RMCType, Convert.ToInt32(MoveViewModel.ServiceLineID), TransitMasterID: MoveViewModel.FreightReport.TransInvMasterID, MoveId: Convert.ToString(MoveViewModel.MoveID), IsGDPRNationality: MoveViewModel.ISGDPRNationalty);
                return Request.IsAjaxRequest()
                ? (ActionResult)PartialView("Create", MoveViewModel)
                : View(MoveViewModel);
            }
            catch (Exception ex)
            {

                CSubs.LogError("MoveManagement", "SendMail", ex.Message + " : " + ex.StackTrace);
                return View(MoveViewModel);
            }

            //}
            //else
            //{
            //	return RedirectToAction("Index", new { page = 1 });
            //}
            //}

            //else
            //{
            //	FillCombo(Convert.ToString(MoveViewModel.MoveJob.ModeID), MoveViewModel.RMCBuss, null, MoveViewModel.RMCType);
            //	return Request.IsAjaxRequest()
            //	? (ActionResult)PartialView("Create", MoveViewModel)
            //	: View(MoveViewModel);
            //}
        }

        private void FillCombo(string ModeID, bool ISRMCBuss, List<int> RateCompForSurveyCost, string RMCType = null, int ServiceLineID = 0, Int64? TransitMasterID = null, string MoveId = null, bool IsGDPRNationality = false)
        {
            //bool RMCBuss = !(UserSession.GetUserSession().BussinessLine == "NON RMC-BUSINESS");
            int LoginID = UserSession.GetUserSession().LoginID;
            int CompID = UserSession.GetUserSession().CompanyID;
            string Mode = "";
            if (ModeID == "1")
            {
                Mode = "S";
            }
            else if (ModeID == "2")
            {
                Mode = "A";
            }
            ViewData["RMC"] = comboBL.GetRMCDropdown();
            ViewData["BusinessLine"] = comboBL.GetBusinessLineDropdown();
            ViewData["GoodsDescription"] = comboBL.GetGoodsDescriptionDropdown();

            ViewData["ServiceLine"] = comboBL.GetServiceLineDropdown(RMCBuss: ISRMCBuss);
            ViewData["Mode"] = comboBL.GetModeDropdown(ServiceLineID: ServiceLineID);
            ViewData["RateCompForSurveyCost"] = new List<SelectListItem>();
            List<SelectListItem> RateComponentDropdown = comboBL.GetRateComponentDropdown().ToList();
            ViewData["RateComponent"] = RateComponentDropdown;
            if (ModeID == "3")
            {
                //List<SelectListItem> RateComponentDropdown = comboBL.GetRateComponentDropdown().ToList();
                //ViewData["RateComponent"] = RateComponentDropdown;
                ViewData["RateComponentOnlyDTD"] = comboBL.GetRateComponentDropdown().Where(x => x.Value == "4").ToList();
                //comboBL.GetRateComponentDropdown().Where(x => x.Value != "4").ToList().AddRange(RateComponentDropdown.Where(x => x.Value == "3").ToList());

                ViewData["RateComponentOnlyDelivery"] = RateComponentDropdown.Where(x => x.Value == "3").ToList();

                ViewData["RateComponentWODTD"] = RateComponentDropdown;
                if (RateCompForSurveyCost != null)
                {

                    ViewData["RateCompForSurveyCost"] = (RateComponentDropdown.Where(l => RateCompForSurveyCost.ToList().Contains(Convert.ToInt32(l.Value)))).ToList();
                    ((List<SelectListItem>)ViewData["RateCompForSurveyCost"]).AddRange(comboBL.GetRateComponentDropdown().Where(x => x.Value == "4").ToList());
                }
            }
            else
            {

                //ViewData["RateComponentOnlyDTD"] = comboBL.GetRateComponentDropdown().Where(x => x.Value == "4").ToList();
                RateComponentDropdown = comboBL.GetRateComponentDropdown().Where(x => x.Value != "4").ToList();
                ViewData["RateComponentOnlyDelivery"] = RateComponentDropdown.Where(x => x.Value == "3").ToList();
                ViewData["RateComponentWODTD"] = RateComponentDropdown;
                if (RateCompForSurveyCost != null)
                {
                    ViewData["RateCompForSurveyCost"] = RateComponentDropdown.Where(l => RateCompForSurveyCost.ToList().Contains(Convert.ToInt32(l.Value)));
                }
            }
            ViewData["Client"] = comboBL.GetAgentDropdown(CORA: ISRMCBuss ? "R" : null);
            ViewData["Agent"] = comboBL.GetAgentDropdown(CORA: "A");
            ViewData["Account"] = comboBL.GetAgentDropdown(CORA: "C");
            ViewData["Port"] = comboBL.GetPortDropdown(SeaOrAir: Mode);
            //ViewData["FromLocation"] = comboBL.GetFromLocationDropdown();
            //ViewData["ToLocation"] = comboBL.GetToLocationDropdown();
            ViewData["RateCurrency"] = comboBL.GetRateCurrencyDropdown();
            ViewData["BaseCurrencyRate"] = comboBL.GetBaseCurrencyRateDropdown();
            ViewData["WeightUnit"] = comboBL.GetMeasurementUnitDropdown('A');
            var stgflag = ModeID == "5" ? "For DSP" : null;
            ViewData["CostHeadList"] = comboBL.GetCostHeadDropdown(ForCombo: stgflag);
            ViewData["SOCostHeadList"] = comboBL.GetCostHeadDropdown(ForCombo: "Service Order");
            ViewData["CityList"] = comboBL.GetCityDropdown();
            ViewData["PortList"] = comboBL.GetPortDropdown(SeaOrAir: Mode);
            ViewData["ShippingLineList"] = comboBL.GetShippingLineDropdown(ModeID);
            ViewData["ShipperTypeList"] = comboBL.GetShipperCategoryDropdown();
            ViewData["ShipmenTypeList"] = comboBL.GetShipmentTypeDropdown();
            ViewData["AcctMgrList"] = comboBL.GetEmployeeDropdown();
            ViewData["FinanceList"] = comboBL.GetEmployeeDropdown(SPTYPE: "FinancePerson");
            ViewData["ContainerUnitList"] = comboBL.GetContainerSizeDropdown();
            ViewData["MoveCoordinatorList"] = comboBL.GetEmployeeDropdown(IsMoveCordination: true);
            ViewData["InsuredByList"] = comboBL.GetInsuranceTypeDropdown();
            ViewData["CourierList"] = comboBL.GetCourierDropDown();
            ViewData["BranchList"] = comboBL.GetCompanyBranchDropdown();
            ViewData["NationalityList"] = comboBL.GetNationalityDropDown();
            ViewData["TitleList"] = comboBL.GetTitleDropdown();
            //ViewData["AirLineList"] = comboBL.GetShippingLineDropdown(ModeID: "2");
            //if ((string.IsNullOrEmpty(RMCType) ? RMCType : RMCType.ToUpper()) == "CARTUS TYPE")
            //{
            //	ViewData["VolumeUnitList"] = comboBL.GetMeasurementUnitDropdown('V').Where(x => x.Text.ToUpper() == "CBM");
            //	ViewData["WeightUnitList"] = comboBL.GetMeasurementUnitDropdown('W').Where(x => x.Text.ToUpper() == "LBS");
            //}
            //else if ((string.IsNullOrEmpty(RMCType) ? RMCType : RMCType.ToUpper()) == "BROOKFIELD TYPE")
            //{
            //	ViewData["VolumeUnitList"] = comboBL.GetMeasurementUnitDropdown('V').Where(x => x.Text.ToUpper() == "CBM");
            //	ViewData["WeightUnitList"] = comboBL.GetMeasurementUnitDropdown('W').Where(x => x.Text.ToUpper() == "KG");
            //}
            //else
            //{
            ViewData["VolumeUnitList"] = comboBL.GetMeasurementUnitDropdown('V');
            ViewData["WeightUnitList"] = comboBL.GetMeasurementUnitDropdown('W');
            //}

            ViewData["SurveyConductedByList"] = comboBL.GetEmployeeDropdown();
            ViewData["JobActivityList"] = comboBL.GetJobActivityList(MoveID: MoveId);
            ViewData["DocTypeList"] = comboBL.GetJobDocTypelDropdown(DocFromType: "MoveMan");
            ViewData["DocNameList"] = new List<SelectListItem>(); //comboBL.GetJobDocNamelDropdown();
            ViewData["ReportList"] = IsGDPRNationality && CompID == 1 ? CommonService.Reports : CommonService.Reports.Where(x => !(x.Value == "9" || x.Value == "10"));
            //ViewData["ShippingLineAgentList"] = comboBL.GetShippingLineAgentDropdown();
            //ViewData["ShippingCarrierList"] = comboBL.GetShippingCarrierDropdown(Convert.ToInt32(ModeID));
            ViewData["TransitInvoiceTypeList"] = comboBL.GetTransitInvoiceTypeDropdown();
            ViewData["TransitJobNoList"] = comboBL.getJobNolDropdown(SPTYPE: "ALLACTIVETRAINSITINVJOB", Modeid: Convert.ToInt32(ModeID), MasterID: TransitMasterID);

            ViewData["JobStatusSDList"] = comboBL.GetJobStatusSDDropdown();
            ViewData["BillingStatusList"] = comboBL.GetBillingStatusDropdown();
            List<SelectListItem> ApprovalList = comboBL.GetApprovalUserList(IsRMCBuss: ISRMCBuss, MoveId: MoveId).ToList();
            ViewData["ApprovalUserList"] = ApprovalList;
            ViewBag.ApprovalCount = ApprovalList.Count();

            List<SelectListItem> GPApprovalList = comboBL.GetGPApprovalUserList(IsRMCBuss: ISRMCBuss, MoveId: MoveId, IsSendForApproval: true, LoginID: LoginID, CompanyID: CompID).ToList();
            ViewData["GPApprovalUserList"] = GPApprovalList;
            List<SelectListItem> LCLFCLList = RELOCBS.Common.CommonService.LCLFCL.ToList();
            if (UserSession.GetUserSession().CompanyID == 2 && ModeID == "3")
            {
                LCLFCLList = LCLFCLList.Where(x => x.Text != "Cased" && x.Text != "LCL" && x.Text != "FCL" && x.Text != " ").ToList();
            }
            else
            {
                LCLFCLList = LCLFCLList.Where(x => x.Text != "Part Load" && x.Text != "Direct" && x.Text != " ").ToList();
            }
            ViewData["LCLFCLList"] = LCLFCLList;
            ViewData["WarehouseList"] = comboBL.GetWarehouseDropdown();

        }

        // POST: MoveManage/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateInfo(int Indx, int SurveyID, int? MoveID, int? EnqDetId, MoveManageViewModel ViewData, string Save)
        {
            try
            {
                //Indx = 1;
                bool IsGetCost = Save == "Get Cost";
                AjaxResponse result = new AjaxResponse();
                string message = string.Empty;
                List<int> RateCompForSurveyCost = new List<int>();
                if (ViewData.SurveyDetail.OrgAgentID == null)
                    RateCompForSurveyCost.Add(Convert.ToInt32(RELOCBS.Common.CommonService.RateComp.Origin));
                if (ViewData.SurveyDetail.FrtAgentID == null)
                    RateCompForSurveyCost.Add(Convert.ToInt32(RELOCBS.Common.CommonService.RateComp.Freight));
                if (ViewData.SurveyDetail.DestAgentID == null)
                    RateCompForSurveyCost.Add(Convert.ToInt32(RELOCBS.Common.CommonService.RateComp.Destination));

                FillCombo(Convert.ToString(ViewData.MoveJob.ModeID), ViewData.RMCBuss, RateCompForSurveyCost, ViewData.RMCType, TransitMasterID: ViewData.FreightReport.TransInvMasterID);
                ViewData.MoveID = MoveID != null ? Convert.ToInt32(MoveID) : 0;
                ViewData.EnqDetailID = ViewData.EnqDetailID != null ? ViewData.EnqDetailID : EnqDetId;
                if (ModelState.IsValid)
                {
                    //if (ViewData.MoveInstructionMst.HFVMoveInstructionList.containsHtmlTags())
                    //{d
                    //    ModelState.AddModelError("", "Invalid Cost Heads");
                    //    this.AddToastMessage("RELOCBS", "Invalid Cost Heads", ToastType.Error);
                    //    return Request.IsAjaxRequest() ? (ActionResult)PartialView("Create", ViewData)
                    //    : View(ViewData);
                    //}
                    string errmsg = string.Empty;
                    int InvoiceRateComp = 0;
                    bool res = false;
                    string SaveType = null;
                    //# btnSaveSurvey, #btnSavePacking ,#btnSaveFreight, #btnSaveDelivery
                    if (Indx == 1)
                    {
                        if (IsGetCost || !ViewData.SurveyCostList.CostListSaved)
                        {
                            ViewData.SurveyCostList.HFCostList = null;
                        }
                        res = moveManageBL.InsertSurvey(ViewData, UserSession.GetUserSession().LoginID, out message);


                        if (res)
                        {

                            if (ViewData.GPSendForApproval)
                            {
                                //GPApproval obj = new GPApproval();

                                res = moveManageBL.InsertGPApproval(ViewData, UserSession.GetUserSession().LoginID, "Survey Stage", out message);
                                SaveType = null;
                            }
                            else
                            {
                                SaveType = "btnSaveSurvey";
                            }
                            if (Save == "Proceed for Approval")
                            {
                                //bool IsApprove = Save == "Pending";
                                SaveType = null;
                                if (CheckGPValid(Convert.ToInt32(ViewData.MoveID), Indx, ViewData.BaseCurr))
                                {
                                    ViewData.SurveyReport.IsCSSenttoApprove = true;

                                    res = moveManageBL.ApproveSurvey(ViewData, true, UserSession.GetUserSession().LoginID, out message);
                                }
                                else
                                {
                                    errmsg = message = "GP is Low. Please send for GP approval";
                                    res = false;
                                }

                                //errmsg = IsApprove ? "Failed to Approve." : "Failed to Disapprove.";
                                //InvoiceRateComp = Convert.ToInt32(RELOCBS.Common.CommonService.RateComp.Destination);

                            }
                            if (Save == "Pending" || Save == "Approved")
                            {
                                SaveType = null;
                                if (CheckGPValid(Convert.ToInt32(ViewData.MoveID), Indx, ViewData.BaseCurr) || Save == "Approved")
                                {
                                    bool IsApprove = Save == "Pending";
                                    res = moveManageBL.ApproveSurvey(ViewData, IsApprove, UserSession.GetUserSession().LoginID, out message);
                                    errmsg = IsApprove ? "Failed to Approve." : "Failed to Disapprove.";
                                }
                                else
                                {
                                    errmsg = message = "GP is Low. Please send for GP approval";
                                    res = false;
                                }

                                //InvoiceRateComp = Convert.ToInt32(RELOCBS.Common.CommonService.RateComp.survey);

                            }
                            //IsGetCost = true;
                        }
                        else
                        {
                            errmsg = "Unable to save Survey data.";
                        }

                    }
                    if (Indx == 2)
                    {
                        if (IsGetCost || !ViewData.PackingCostList.CostListSaved)
                        {
                            ViewData.PackingCostList.HFCostList = null;
                        }
                        res = moveManageBL.InsertPacking(ViewData, UserSession.GetUserSession().LoginID, out message);
                        //SaveType = "btnSavePacking";
                        if (res)
                        {
                            if (ViewData.GPSendForApproval)
                            {
                                //GPApproval obj = new GPApproval();

                                res = moveManageBL.InsertGPApproval(ViewData, UserSession.GetUserSession().LoginID, "Packing Stage", out message);
                                SaveType = null;
                            }
                            else
                            {
                                SaveType = "btnSavePacking";
                            }
                            if (Save == "Proceed for Approval")
                            {
                                //bool IsApprove = Save == "Pending";
                                SaveType = null;
                                if (CheckGPValid(Convert.ToInt32(ViewData.MoveID), Indx, ViewData.BaseCurr))
                                {
                                    ViewData.PackingReport.IsCSSenttoApprove = true;

                                    res = moveManageBL.ApprovePacking(ViewData, true, UserSession.GetUserSession().LoginID, out message);
                                }
                                else
                                {
                                    errmsg = message = "GP is Low. Please send for GP approval";
                                    res = false;
                                }

                                //errmsg = IsApprove ? "Failed to Approve." : "Failed to Disapprove.";
                                //InvoiceRateComp = Convert.ToInt32(RELOCBS.Common.CommonService.RateComp.Destination);

                            }
                            if (Save == "Pending" || Save == "Approved")
                            {
                                SaveType = null;
                                if (CheckGPValid(Convert.ToInt32(ViewData.MoveID), Indx, ViewData.BaseCurr) || Save == "Approved")
                                {
                                    bool IsApprove = Save == "Pending";
                                    res = moveManageBL.ApprovePacking(ViewData, IsApprove, UserSession.GetUserSession().LoginID, out message);
                                    errmsg = IsApprove ? "Failed to Approve." : "Failed to Disapprove.";
                                    InvoiceRateComp = Convert.ToInt32(RELOCBS.Common.CommonService.RateComp.Destination);
                                }
                                else
                                {
                                    errmsg = message = "GP is Low. Please send for GP approval";
                                    res = false;
                                }

                            }
                            //IsGetCost = true;
                        }
                        else
                        {
                            errmsg = "Unable to save Packing data.";
                        }
                        //errmsg = "Unable to save Packing data.";
                        InvoiceRateComp = Convert.ToInt32(RELOCBS.Common.CommonService.RateComp.Origin);
                    }
                    else if (Indx == 3)
                    {
                        res = moveManageBL.InsertFreight(ViewData, UserSession.GetUserSession().LoginID, out message);
                        errmsg = "Unable to save Freight data.";
                    }
                    else if (Indx == 4)
                    {
                        if (IsGetCost || !ViewData.DeliveryCostList.CostListSaved)
                        {
                            ViewData.DeliveryCostList.HFCostList = null;
                        }
                        res = moveManageBL.InsertDelivery(ViewData, UserSession.GetUserSession().LoginID, out message);
                        errmsg = "Unable to save Delivery data.";
                        InvoiceRateComp = Convert.ToInt32(RELOCBS.Common.CommonService.RateComp.Destination);

                        if (res)
                        {
                            if (ViewData.GPSendForApproval)
                            {
                                //GPApproval obj = new GPApproval();

                                res = moveManageBL.InsertGPApproval(ViewData, UserSession.GetUserSession().LoginID, "Final Stage", out message);
                                SaveType = null;
                            }
                            else
                            {
                                SaveType = "btnSaveDelivery";
                            }
                            if (Save == "Proceed for Approval")
                            {
                                //bool IsApprove = Save == "Pending";
                                SaveType = null;
                                if (CheckGPValid(Convert.ToInt32(ViewData.MoveID), Indx, ViewData.BaseCurr))
                                {
                                    ViewData.IsCSSenttoApprove = true;

                                    res = moveManageBL.ApproveDelivery(ViewData, true, UserSession.GetUserSession().LoginID, out message);
                                }
                                else
                                {
                                    errmsg = message = "GP is Low. Please send for GP approval";
                                    res = false;
                                }

                                //errmsg = IsApprove ? "Failed to Approve." : "Failed to Disapprove.";
                                //InvoiceRateComp = Convert.ToInt32(RELOCBS.Common.CommonService.RateComp.Destination);
                            }
                            if (Save == "Pending" || Save == "Approved")
                            {
                                SaveType = null;
                                if (CheckGPValid(Convert.ToInt32(ViewData.MoveID), Indx, ViewData.BaseCurr) || Save == "Approved")
                                {
                                    bool IsApprove = Save == "Pending";
                                    res = moveManageBL.ApproveDelivery(ViewData, IsApprove, UserSession.GetUserSession().LoginID, out message);
                                    errmsg = IsApprove ? "Failed to Approve." : "Failed to Disapprove.";
                                    InvoiceRateComp = Convert.ToInt32(RELOCBS.Common.CommonService.RateComp.Destination);
                                }
                                else
                                {
                                    errmsg = message = "GP is Low. Please send for GP approval";
                                    res = false;
                                }


                            }
                            //IsGetCost = true;

                        }
                        else
                        {
                            errmsg = "Unable to save Delivery data.";
                        }
                    }
                    else if (Indx == 6)
                    {
                        if (SendMailFunction(ViewData.MoveEmail))
                        {
                            res = moveManageBL.InsertEmailData(ViewData, UserSession.GetUserSession().LoginID, out message);
                            errmsg = "Unable to save Email data.";
                        }
                        else
                        {
                            CSubs.LogError("MoveManagement", "Email Info", "Email Send Fail.");
                        }
                    }
                    else if (Indx == 7)
                    {
                        res = moveManageBL.InsertDocument(ViewData, UserSession.GetUserSession().LoginID, out message);
                        errmsg = "Unable to save File data.";

                    }
                    if (!res)
                    {
                        result.Success = false;
                        ModelState.AddModelError(string.Empty, errmsg);
                        result.Message = message;

                        this.AddToastMessage("RELOCBS", message, ToastType.Error);

                    }
                    else
                    {

                        result.Success = true;
                        result.Message = message;
                        if (!IsGetCost && !(Save == "Proceed"))
                        {
                            this.AddToastMessage("RELOCBS", message, ToastType.Success);
                        }

                        //return Json(result);
                        if (Save == "Proceed")
                        {
                            long? key = 0; string type = null;
                            if (ViewData.InvoiceID != null && ViewData.InvoiceID > 0)
                            {
                                key = ViewData.InvoiceID;
                                type = "AI";
                            }
                            else
                            {
                                key = ViewData.MoveID;
                                type = "NI";
                            }
                            return RedirectToAction("Create", "Billing", new { key = key, PageIndex = InvoiceRateComp, type = type });
                        }
                    }

                    TempData["SaveType"] = SaveType;

                    //ViewBag.Result = result;
                    return RedirectToAction("Create", new { Indx = Indx, SurveyID = ViewData.SurveyID, MoveID = ViewData.MoveID, IsGetCost = IsGetCost });
                }
                else
                {
                    //  return Request.IsAjaxRequest() ? (ActionResult)PartialView("Create", ViewData)
                    //return View("Create", ViewData);
                    return RedirectToAction("Create", new { Indx = Indx, SurveyID = ViewData.SurveyID, MoveID = ViewData.MoveID, IsGetCost = IsGetCost });
                    //return RedirectToAction("Create", new { Indx = Indx, SurveyID = ViewData.SurveyID, MoveID = ViewData.MoveID});
                }
            }
            catch (Exception ex)
            {
                this.AddToastMessage("RELOCBS", RELOCBS.Properties.Resources.UnExpectedErrorAtPL, ToastType.Error);
                return View("Create", ViewData);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateCost(int Indx, int SurveyID, int? MoveID, MoveManageViewModel ViewData)
        {
            try
            {
                Indx = 2;
                AjaxResponse result = new AjaxResponse();
                string message = string.Empty;
                List<int> RateCompForSurveyCost = new List<int>();
                if (ViewData.SurveyDetail.OrgAgentID == null)
                    RateCompForSurveyCost.Add(Convert.ToInt32(RELOCBS.Common.CommonService.RateComp.Origin));
                if (ViewData.SurveyDetail.FrtAgentID == null)
                    RateCompForSurveyCost.Add(Convert.ToInt32(RELOCBS.Common.CommonService.RateComp.Freight));
                if (ViewData.SurveyDetail.DestAgentID == null)
                    RateCompForSurveyCost.Add(Convert.ToInt32(RELOCBS.Common.CommonService.RateComp.Destination));
                FillCombo(ViewData.MoveCostMst.ModeID.ToString(), ViewData.RMCBuss, RateCompForSurveyCost, ViewData.RMCType, TransitMasterID: ViewData.FreightReport.TransInvMasterID);
                ViewData.MoveJob.ModeID = MoveID != null ? Convert.ToInt32(MoveID) : 0;
                if (ModelState.IsValid)
                {

                    if (ViewData.MoveCostMst.HFVMoveCostHeadList.containsHtmlTags())
                    {
                        ModelState.AddModelError("", "Invalid Instructions");
                        this.AddToastMessage("RELOCBS", "Invalid Instructions", ToastType.Error);
                        return Request.IsAjaxRequest() ? (ActionResult)PartialView("Create", ViewData)
                        : View(ViewData);
                    }


                    bool res = false;
                    res = moveManageBL.InsertMoveCost(ViewData, UserSession.GetUserSession().LoginID, out message);
                    if (!res)
                    {
                        result.Success = false;
                        ModelState.AddModelError(string.Empty, "Unable to save Cost data.");
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
                    return RedirectToAction("Create", new { Indx = Indx, SurveyID = ViewData.SurveyID, MoveID = ViewData.MoveID });
                }
                else
                {
                    return Request.IsAjaxRequest() ? (ActionResult)PartialView("Create", ViewData)
                  : View("Create", ViewData);
                }
            }
            catch
            {
                this.AddToastMessage("RELOCBS", RELOCBS.Properties.Resources.UnExpectedErrorAtPL, ToastType.Error);
                return View("Create", ViewData);
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateJob(int Indx, int SurveyID, int? MoveID, MoveManageViewModel ViewData)
        {
            try
            {
                Indx = 0;
                AjaxResponse result = new AjaxResponse();
                string message = string.Empty;
                List<int> RateCompForSurveyCost = new List<int>();
                if (ViewData.SurveyDetail.OrgAgentID == null)
                    RateCompForSurveyCost.Add(Convert.ToInt32(RELOCBS.Common.CommonService.RateComp.Origin));
                if (ViewData.SurveyDetail.FrtAgentID == null)
                    RateCompForSurveyCost.Add(Convert.ToInt32(RELOCBS.Common.CommonService.RateComp.Freight));
                if (ViewData.SurveyDetail.DestAgentID == null)
                    RateCompForSurveyCost.Add(Convert.ToInt32(RELOCBS.Common.CommonService.RateComp.Destination));
                FillCombo(ViewData.MoveCostMst.ModeID.ToString(), ViewData.RMCBuss, RateCompForSurveyCost, ViewData.RMCType, TransitMasterID: ViewData.FreightReport.TransInvMasterID);
                ViewData.MoveID = MoveID != null ? Convert.ToInt32(MoveID) : 0;
                ViewData.MoveJob.WeightUnitTo = ViewData.MoveJob.WeightUnitFrom;
                //if (ViewData.Project == "IMP" || ViewData.Project == "EXP")
                //{
                //	ModelState.AddModelError("BrSdEmpID", "Branch SD is Mandatory.");
                //}
                //if (ViewData.Project == "NMD")
                //{
                //	ModelState.AddModelError("DestBrSdEmpID", "Dest. Branch SD is Mandatory.");
                //}

                if (ModelState.IsValid)
                {

                    if (ViewData.MoveJob.HFVMoveRateCompList != null)
                    {
                        if (ViewData.MoveJob.HFVMoveRateCompList.containsHtmlTags())
                        {
                            ModelState.AddModelError("", "Invalid Instructions.");
                            this.AddToastMessage("RELOCBS", "Invalid RateComponet and Agent list", ToastType.Error);
                            return Request.IsAjaxRequest() ? (ActionResult)PartialView("Create", ViewData)
                            : View(ViewData);
                        }
                    }
                    bool res = false;
                    res = moveManageBL.InsertJobOpening(ViewData, UserSession.GetUserSession().LoginID, out message);
                    if (!res)
                    {
                        result.Success = false;
                        ModelState.AddModelError(string.Empty, "Unable to save Job data.");
                        result.Message = message;

                        this.AddToastMessage("RELOCBS", message, ToastType.Error);
                        //return Json(result);
                    }
                    else
                    {
                        ////OFS Document save after MoveID generated/Job Opened
                        if (ViewData.MoveID != null && ViewData.MoveID > 0 && ViewData.MoveJob != null && ViewData.MoveJob.OFSDocument != null && ViewData.MoveJob.OFSDocument.ContentLength > 0)
                        {
                            string OFSMsg = string.Empty;
                            moveManageBL.InsertOFSDoc(ViewData, ViewData.MoveID, UserSession.GetUserSession().LoginID, out OFSMsg);
                        }
                        result.Success = true;
                        result.Message = message;

                        this.AddToastMessage("RELOCBS", message, ToastType.Success);
                        if (ViewData.MoveJob.EntryPointID <= 0 && ViewData.MoveJob.ExitPointID <= 0 && ViewData.RMCBuss && ViewData.RMCType != "Other Type" && ViewData.MoveJob.ModeID != 3)
                        {
                            this.AddToastMessage("Warning", "Entry Port and Exit Port should be entered to procced with Survey.", ToastType.Warning);
                        }

                        //return Json(result);
                    }

                    //ViewBag.Result = result;
                    return RedirectToAction("Create", new { Indx = Indx, SurveyID = ViewData.SurveyID, MoveID = ViewData.MoveID });
                }
                else
                {

                    return Request.IsAjaxRequest() ? (ActionResult)PartialView("Create", ViewData)
                     : View("Create", ViewData);
                    //return RedirectToAction("Create", new { Indx = Indx, SurveyID = ViewData.SurveyID, MoveID = ViewData.MoveID });
                }
            }
            catch (Exception ex)
            {
                this.AddToastMessage("RELOCBS", "error", ToastType.Error);//RELOCBS.Properties.Resources.UnExpectedErrorAtPL
                return View("Create", ViewData);
            }
        }

        // GET: MoveManage/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: MoveManage/Edit/5
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

        // GET: MoveManage/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: MoveManage/Delete/5
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
        public JsonResult GetShippingLineList(int mode)
        {
            ViewData["ShippingLineList"] = comboBL.GetShippingLineDropdown(Convert.ToString(mode));

            var lstItem = ((IEnumerable<SelectListItem>)ViewData["ShippingLineList"]).Select(i => new { i.Value, i.Text }).ToList();
            return Json(lstItem, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetaJAXClientDetails(int ClientId, char Mode)
        {
            return Json(new { AccountMgr = GetClientDetails(ClientId, Mode) }, JsonRequestBehavior.AllowGet);
        }

        private string GetClientDetails(int? ClientId, char Mode)
        {
            //return (ClientId==null || ClientId==0) ? "" : ClientDetailBL.GetClientDetail((int)ClientId).AccountMgr;
            return "";
        }

        public JsonResult GetAgentGridCombo(int RateCompID, int AgentId, int SurveyID)
        {

            DataTable dt = TempData["dtAgentList"] != null && AgentId != -1 ? (DataTable)TempData["dtAgentList"] : new DataTable();
            List<IEnumerable<SelectListItem>> list = moveManageBL.GetAgentGridCombo(RateCompID, AgentId, SurveyID, ref dt);
            TempData["dtAgentList"] = dt;
            if (list.Count < 2)
            {
                if (AgentId == -1)
                {
                    return Json(new { AgentList = list[0], FromPortList = "", ToPortList = "" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { AgentList = "", FromPortList = list[0], ToPortList = "" }, JsonRequestBehavior.AllowGet);
                }

            }
            else
            {
                return Json(new { AgentList = "", FromPortList = list[0], ToPortList = list[1] }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetCost(int AgentId, int CityId, int RMCId, int GoodsDescId, DateTime JobDate, decimal ConversionRate, int CostHeadId)
        {
            PackingCostList list = moveManageBL.GetCost(AgentId, CityId, RMCId, GoodsDescId, JobDate, ConversionRate, CostHeadId);

            return Json(new
            {
                CostVal = list.CostValue,
                Revenueval = list.RevenueValue,
                CurrencyID = list.RateCurrID,
                WtUnitID = list.WtUnitID,
                //WtVol = list[0]["Wt_Vol_No"],
                Per = list.Per
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult JobDocDownload(int id)
        {
            JobDocument jobDocument = moveManageBL.GetDownloadFile(id);
            if (!string.IsNullOrWhiteSpace(jobDocument.FilePath) && System.IO.File.Exists(jobDocument.FilePath))
            {
                //var filepath = System.IO.Path.Combine(Server.MapPath("/Files/"), fileName);
                return File(jobDocument.FilePath, MimeMapping.GetMimeMapping(jobDocument.FilePath), jobDocument.FileName);
            }

            return new HttpStatusCodeResult(404);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult JobDocDelete(int Indx, int SurveyID, int? MoveID, bool IsGetCost, int id)
        {
            bool res = false;
            string message = string.Empty;
            AjaxResponse result = new AjaxResponse();
            if (ModelState.IsValid)
            {
                res = moveManageBL.DeleteDocument(id, out message);
                if (!res)
                {
                    result.Success = false;
                    ModelState.AddModelError(string.Empty, "Unable to delete document.");
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
                return RedirectToAction("Create", new { Indx = Indx, SurveyID = SurveyID, MoveID = MoveID, IsGetCost = IsGetCost });
            }
            else
            {
                return Request.IsAjaxRequest() ? (ActionResult)PartialView("Create", ViewData)
                  : View("Create", ViewData);
            }

        }

        public JsonResult GetInvoice(int MoveID, bool IsStatement = false)
        {
            try
            {
                List<SelectListItem> InvoiceList = new List<SelectListItem>();
                string errormsg = null;
                InvoiceList = comboBL.GetInvoiceForMoveDropdown(MoveID, IsStatement).ToList();
                errormsg = InvoiceList.Count < 0 && IsStatement ? "No Invoice allcated to this job." : "";
                return Json(new { InvoiceList = InvoiceList, errormsg = errormsg }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        //public bool SendMailFunction(EmailList model)
        //{
        //    bool IsEmailSend = false;
        //    try
        //    {
        //        using (MailMessage mm = new MailMessage())
        //        {
        //            mm.Subject = model.Subject;
        //            mm.Body = model.Body.Replace("&amp;", "&").Replace("&lt;", "<").Replace("&gt;", ">").Replace("&quot;", "\"");
        //            //if (model.Attachments != null)
        //            //{
        //            //	foreach (EmailSendAttachment itm in model.Attachments)
        //            //	{
        //            //		if (itm == null) { continue; }
        //            //		EmailSendAttachment esa = new EmailSendAttachment();
        //            //		esa.FileExtension = itm.FileExtension;
        //            //		esa.FileName = itm.FileName;
        //            //		esa.FilePath = itm.FilePath;
        //            //		esa.UploadFile = itm.UploadFile;


        //            //		//mm.Attachments.Add(new Attachment(esa.FilePath+ ""+""+esa.FileName));

        //            //		Attachment att = new Attachment(new MemoryStream(esa.UploadFile), esa.FileName);
        //            //		mm.Attachments.Add(att);
        //            //	}
        //            //}

        //            mm.IsBodyHtml = true;

        //            mm.To.Add(model.EmailTo);
        //            if (!string.IsNullOrEmpty(model.EmailCC))
        //            {
        //                mm.CC.Add(model.EmailCC);
        //            }

        //            if (!string.IsNullOrEmpty(model.EmailBCC))
        //            {
        //                mm.Bcc.Add(model.EmailBCC);
        //            }


        //            //mm.To.Add(model.EmailTo);
        //            //mm.From = new MailAddress("ashley@writercorporation.com");
        //            if (string.IsNullOrEmpty(model.EmailFrom))
        //                mm.From = new MailAddress("system@writerrelocations.com");
        //            else
        //                mm.From = new MailAddress(model.EmailFrom);



        //            using (SmtpClient smtp = new SmtpClient())
        //            {
        //                //SMTPSettings _settingsObj = new SMTPSettings();
        //                //_settingsObj = _emailService.GetSMTPSettings(1).SingleOrDefault();

        //                //   smtp.Host = "outlook.office365.com";// _settingsObj.Host;// "smtp.gmail.com";
        //                smtp.Host = "172.16.1.177";
        //                smtp.EnableSsl = false;

        //                // NetworkCredential NetworkCred = new NetworkCredential(_settingsObj.SendTestEmailTo, _settingsObj.Password);
        //                //NetworkCredential NetworkCred = new NetworkCredential("minal.sonar@vnvcs.com", "minal@28");
        //                //NetworkCredential NetworkCred = new NetworkCredential("ashley@writercorporation.com", "mar@88888");
        //                //NetworkCredential NetworkCred = new NetworkCredential("darren.quadros", "%nW^3&}=");
        //                smtp.UseDefaultCredentials = false;
        //                //smtp.Credentials = NetworkCred;
        //                smtp.Port = 25; //_settingsObj.Port;
        //                smtp.Send(mm);
        //                //return true;
        //                IsEmailSend = true;

        //            }
        //            return IsEmailSend;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        CSubs.LogError("MoveManagement", "SendMail", ex.Message);
        //        return IsEmailSend;
        //    }


        //    //return View();
        //}


        public bool SendMailFunction(EmailList model)
        {
            bool IsEmailSend = false;
            try
            {
                using (MailMessage mm = new MailMessage())
                {
                    mm.Subject = model.Subject;
                    mm.Body = model.Body.Replace("&amp;", "&").Replace("&lt;", "<").Replace("&gt;", ">").Replace("&quot;", "\"");
                    //if (model.Attachments != null)
                    //{
                    //	foreach (EmailSendAttachment itm in model.Attachments)
                    //	{
                    //		if (itm == null) { continue; }
                    //		EmailSendAttachment esa = new EmailSendAttachment();
                    //		esa.FileExtension = itm.FileExtension;
                    //		esa.FileName = itm.FileName;
                    //		esa.FilePath = itm.FilePath;
                    //		esa.UploadFile = itm.UploadFile;


                    //		//mm.Attachments.Add(new Attachment(esa.FilePath+ ""+""+esa.FileName));

                    //		Attachment att = new Attachment(new MemoryStream(esa.UploadFile), esa.FileName);
                    //		mm.Attachments.Add(att);
                    //	}
                    //}

                    mm.IsBodyHtml = true;

                    mm.To.Add(model.EmailTo);
                    if (!string.IsNullOrEmpty(model.EmailCC))
                    {
                        mm.CC.Add(model.EmailCC);
                    }

                    if (!string.IsNullOrEmpty(model.EmailBCC))
                    {
                        mm.Bcc.Add(model.EmailBCC);
                    }


                    //mm.To.Add(model.EmailTo);
                    //mm.From = new MailAddress("ashley@writercorporation.com");
                    if (string.IsNullOrEmpty(model.EmailFrom))
                        mm.From = new MailAddress("system@writerrelocations.com");
                    else
                        mm.From = new MailAddress(model.EmailFrom);

                    string EmailSmtpHost = System.Configuration.ConfigurationManager.AppSettings["EmailSmtpHost"].ToString();
                    int EmailSmtpPort = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["EmailSmtpPort"].ToString());
                    bool EnableSsl = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["EmailEnableSsl"].ToString());

                    using (SmtpClient smtp = new SmtpClient())
                    {
                        //SMTPSettings _settingsObj = new SMTPSettings();
                        //_settingsObj = _emailService.GetSMTPSettings(1).SingleOrDefault();

                        //   smtp.Host = "outlook.office365.com";// _settingsObj.Host;// "smtp.gmail.com";
                        smtp.Host = EmailSmtpHost;// "172.16.1.177";
                        smtp.EnableSsl = EnableSsl;

                        // NetworkCredential NetworkCred = new NetworkCredential(_settingsObj.SendTestEmailTo, _settingsObj.Password);
                        //NetworkCredential NetworkCred = new NetworkCredential("minal.sonar@vnvcs.com", "minal@28");
                        //NetworkCredential NetworkCred = new NetworkCredential("ashley@writercorporation.com", "mar@88888");
                        //NetworkCredential NetworkCred = new NetworkCredential("darren.quadros", "%nW^3&}=");
                        smtp.UseDefaultCredentials = false;
                        //smtp.Credentials = NetworkCred;
                        smtp.Port = EmailSmtpPort; //_settingsObj.Port;
                        smtp.Send(mm);
                        //return true;
                        IsEmailSend = true;

                    }
                    return IsEmailSend;
                }
            }
            catch (Exception ex)
            {
                CSubs.LogError("MoveManagement", "SendMail", ex.Message + " : " + ex.StackTrace);

                //CSubs.LogError("MoveManagement", "SendMail", ex.Message);
                return IsEmailSend;
            }


            //return View();
        }

        public JsonResult ExportToExcel(string MoveID)
        {
            Dictionary<string, string> exptoExlParameters = new Dictionary<string, string>();
            string errormsg = string.Empty;
            string htmlstring = string.Empty;
            int Colcount = 0;
            try
            {
                DataTable dtgridData = new DataTable();
                exptoExlParameters.Add("@SP_MoveID", Convert.ToString(MoveID));
                exptoExlParameters.Add("@SP_LoginID", Convert.ToString(UserSession.GetUserSession().LoginID));
                exptoExlParameters.Add("@SP_IsXlFormat", Convert.ToString(1));
                string param = string.Join(",", exptoExlParameters.Select(x => x.Key + "=" + CSubs.QSafeValue(x.Value)).ToArray()).TrimEnd(',');
                string query = string.Format("EXEC {0} {1}", "[MoveMan].[GetCostSheet]", param);
                dtgridData = CSubs.GetDataTable(query);
                Colcount = dtgridData.Columns.Count;
                htmlstring = "<tr>";
                foreach (DataColumn col in dtgridData.Columns)
                {
                    htmlstring += "<th bgcolor='#DCDCDC'>" + col.ColumnName + "</th>";
                }
                htmlstring += "</tr>";
                //htmlstring += "<tr>";
                foreach (DataRow row in dtgridData.Rows)
                {
                    if (string.IsNullOrEmpty(Convert.ToString(row[0])))
                        htmlstring += "<tr>";
                    else
                        htmlstring += "<tr>";


                    foreach (DataColumn col in dtgridData.Columns)
                    {
                        if (string.IsNullOrEmpty(Convert.ToString(row[0])))
                            htmlstring += "<td bgcolor='#B0C4DE' style=\"font-weight:bold\">" + row[col.ColumnName].ToString() + "</td>";
                        else
                            htmlstring += "<td>" + row[col.ColumnName].ToString() + "</td>";

                    }
                    htmlstring += "</tr>";


                }

                //htmlstring += "<tr>";
                //string SearchKey = string.Empty;
                //if (Request.Form["SearchKey"] != null && Request.Form["SearchKey"].Trim() != "")
                //{
                //	param.Add("@SP_SearchString", Request.Form["SearchKey"]);
                //}


                //CommonService.GenerateExcel(this.Response, "CostSheet", "[MoveMan].[GetCostSheet]", param);

            }
            catch (Exception ex)
            {
                this.AddToastMessage("RELOCBS", "UnExpected Error occured", ToastType.Error);
            }

            return Json(new { htmlstring = htmlstring, errormsg = errormsg, ColCount = Colcount }, JsonRequestBehavior.AllowGet);
            //return View();
        }

        public ActionResult SaveFollowUpDetails(int Indx, int SurveyID, int? MoveID, MoveManageViewModel ViewData)
        {
            bool res = false;
            string message = string.Empty;
            AjaxResponse result = new AjaxResponse();
            if (ModelState.IsValid)
            {
                res = moveManageBL.InsertFollowUpDetials(ViewData, out message);
                if (!res)
                {
                    result.Success = false;
                    ModelState.AddModelError(string.Empty, "Unable to save Job data.");
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
                return RedirectToAction("Create", new { Indx = Indx, SurveyID = SurveyID, MoveID = MoveID });
            }
            else
            {
                return Request.IsAjaxRequest() ? (ActionResult)PartialView("Create", ViewData)
                  : View("Create", ViewData);
            }
        }

        public ActionResult SaveCloseJobDetails(int Indx, int SurveyID, int? MoveID, MoveManageViewModel ViewData)
        {
            bool res = false;
            string message = string.Empty;
            AjaxResponse result = new AjaxResponse();
            if (ModelState.IsValid)
            {
                res = moveManageBL.InsertCloseJobDetials(ViewData, out message);
                if (!res)
                {
                    result.Success = false;
                    ModelState.AddModelError(string.Empty, "Unable to close Job data.");
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
                return RedirectToAction("Create", new { Indx = Indx, SurveyID = SurveyID, MoveID = MoveID });
            }
            else
            {
                return Request.IsAjaxRequest() ? (ActionResult)PartialView("Create", ViewData)
                  : View("Create", ViewData);
            }
        }

        public ActionResult CancelJob(int Indx, int SurveyID, int? MoveID, MoveManageViewModel ViewData)
        {
            bool res = false;
            string message = string.Empty;
            AjaxResponse result = new AjaxResponse();
            if (ModelState.IsValid)
            {
                res = moveManageBL.CancelJob(ViewData, out message);
                if (!res)
                {
                    result.Success = false;
                    ModelState.AddModelError(string.Empty, "Unable to Cancel Job data.");
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
                return RedirectToAction("Create", new { Indx = Indx, SurveyID = SurveyID, MoveID = MoveID });
            }
            else
            {
                return Request.IsAjaxRequest() ? (ActionResult)PartialView("Create", ViewData)
                  : View("Create", ViewData);
            }
        }

        public ActionResult ApproveAdvanceCaution(int Indx, int SurveyID, int? MoveID, MoveManageViewModel ViewData)
        {
            bool res = false;
            string message = string.Empty;
            AjaxResponse result = new AjaxResponse();
            if (ModelState.IsValid)
            {
                res = moveManageBL.ApproveAdvanceCaution(ViewData, out message);
                if (!res)
                {
                    result.Success = false;
                    ModelState.AddModelError(string.Empty, "Unable to Approve Advance Caution.");
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
                return RedirectToAction("Create", new { Indx = Indx, SurveyID = SurveyID, MoveID = MoveID });
            }
            else
            {
                return Request.IsAjaxRequest() ? (ActionResult)PartialView("Create", ViewData)
                  : View("Create", ViewData);
            }
        }

        public ActionResult ExportSunIndex(int page = 1)
        {
            //_PageID = "47";
            //if (!CSubs.CheckPageRights(_PageID, PermissionType.VIEW))
            //{
            //	return new HttpStatusCodeResult(403);
            //}
            session.Set<string>("PageSession", "Export Job For Sun");
            string sort = "";
            string sortdir = "";
            string search = "";

            int pageSize = settings.GetSettingByKey<int>("pagination_pagesize");
            string OrderBy = "";
            int Order = 0;
            DateTime? Fromdate = null;
            DateTime? Todate = null;
            bool IsJobDate = false;
            string Shipper = null;
            string SearchKey = string.Empty;
            bool RMCBuss = !UserSession.GetUserSession().BussinessLine.Equals("NON RMC-BUSINESS");
            if (Request.Form["FromDate"] != null && Request.Form["FromDate"].Trim() != "")
            {
                Fromdate = Convert.ToDateTime(Request.Form["FromDate"]);
            }

            if (Request.Form["ToDate"] != null && Request.Form["ToDate"].Trim() != "")
            {
                Todate = Convert.ToDateTime(Request.Form["ToDate"]);
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
            int totalRecord = 0;
            if (page < 1) page = 1;
            int skip = (page * pageSize) - pageSize;

            bool IsWOS = UserSession.GetUserSession().BussinessLine == "ORIENTATION SERVICE";
            var data = moveManageBL.GetForSunGrid(Fromdate, Todate, sort, sortdir, skip, pageSize, out totalRecord, IsWOS);

            ViewBag.TotalRows = totalRecord;
            ViewBag.RMCBuss = RMCBuss;
            ViewBag.search = search;
            var itemsAsIPagedList = new StaticPagedList<ExportSunJob>(data, page, pageSize, totalRecord);

            return Request.IsAjaxRequest()
                ? (ActionResult)PartialView("_ExportSunGrid", itemsAsIPagedList)
                : View(itemsAsIPagedList);
        }

        public ActionResult ExportIndexToExcel()
        {
            Dictionary<string, string> param = new Dictionary<string, string>();
            try
            {
                if (Request.Form["FromDate"] != null && Request.Form["FromDate"].Trim() != "")
                {
                    param.Add("@SP_FromDate", Convert.ToDateTime(Request.Form["FromDate"]).ToString("dd-MMM-yyyy"));
                }

                if (Request.Form["ToDate"] != null && Request.Form["ToDate"].Trim() != "")
                {
                    param.Add("@SP_ToDate", Convert.ToDateTime(Request.Form["ToDate"]).ToString("dd-MMM-yyyy"));
                }

                if (Request.Form["SearchType"] != null && Request.Form["SearchType"].Trim() != "")
                {
                    param.Add("@SP_SearchType", Convert.ToString(Request.Form["SearchType"]));
                }

                if (Request.Form["search"] != null && Request.Form["search"].Trim() != "")
                {
                    param.Add("@SP_SearchNo", Convert.ToString(Request.Form["search"]));
                }

                param.Add("@SP_LoginID", Convert.ToString(UserSession.GetUserSession().LoginID));
                param.Add("@SP_CompId", Convert.ToString(UserSession.GetUserSession().CompanyID));

                CommonService.GenerateExcel(this.Response, "Survey", "[Survey].[GetSurveyForGrid_ExpToExl]", param);
            }
            catch (Exception ex)
            {
                this.AddToastMessage("RELOCBS", "UnExpected Error occured", ToastType.Error);
            }
            return View();
        }

        public JsonResult ExportSunToExcel(DateTime? FromDate, DateTime? ToDate)
        {
            Dictionary<string, string> param = new Dictionary<string, string>();
            try
            {
                bool IsWOS = UserSession.GetUserSession().BussinessLine == "ORIENTATION SERVICE";

                param.Add("@SP_FromDate", Convert.ToDateTime(FromDate).ToString("dd-MMM-yyyy"));
                param.Add("@SP_ToDate", Convert.ToDateTime(ToDate).ToString("dd-MMM-yyyy"));
                //if (Request.Form["FromDate"] != null && Request.Form["FromDate"].Trim() != "")
                //{
                //	param.Add("@SP_FromDate", Convert.ToDateTime(Request.Form["FromDate"]).ToString("dd-MMM-yyyy"));
                //}

                //if (Request.Form["ToDate"] != null && Request.Form["ToDate"].Trim() != "")
                //{
                //	param.Add("@SP_ToDate", Convert.ToDateTime(Request.Form["ToDate"]).ToString("dd-MMM-yyyy"));
                //}

                param.Add("@SP_LoginID", Convert.ToString(UserSession.GetUserSession().LoginID));
                param.Add("@SP_CompId", Convert.ToString(UserSession.GetUserSession().CompanyID));
                param.Add("@SP_IsWOS", Convert.ToString(IsWOS));

                CommonService.GenerateExcel(this.Response, "Export Job For Sun", "[MoveMan].[GetJobForSun]", param);
                //ExportSunIndex();
            }
            catch (Exception ex)
            {
                this.AddToastMessage("RELOCBS", "UnExpected Error occured", ToastType.Error);
            }
            return Json(null, JsonRequestBehavior.AllowGet);
        }


        public ActionResult SaveInsuranceDetails(int Indx, int SurveyID, int? MoveID, MoveManageViewModel ViewData)
        {
            bool res = false;
            string message = string.Empty;
            AjaxResponse result = new AjaxResponse();
            if (ModelState.IsValid)
            {
                res = moveManageBL.InsertInsuranceBySD(ViewData, out message);
                if (!res)
                {
                    result.Success = false;
                    ModelState.AddModelError(string.Empty, "Unable to save insurance Detail.");
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
                return RedirectToAction("Create", new { Indx = Indx, SurveyID = SurveyID, MoveID = MoveID });
            }
            else
            {
                return Request.IsAjaxRequest() ? (ActionResult)PartialView("Create", ViewData)
                  : View("Create", ViewData);
            }
        }

        public ActionResult SaveVendorEvaluation(int Indx, int SurveyID, int? MoveID, MoveManageViewModel ViewData)
        {
            bool res = false;
            string message = string.Empty;
            AjaxResponse result = new AjaxResponse();


            if (UserSession.GetUserSession().CompanyName.Contains("BTR"))
            {

                if (ViewData.vendorEvaluation.OrgEvalQuestions.Exists(x => x.AnswerType.Equals("RADIOBTN", StringComparison.OrdinalIgnoreCase)) && ViewData.vendorEvaluation.OrgEvalQuestions.Exists(x => x.Answer == null) && ViewData.vendorEvaluation.OrgEvalQuestions.Exists(x => x.Answer != null))
                {
                    message = "Need to answer all Origin Partner questions";
                    ModelState.AddModelError(string.Empty, message);
                    this.AddToastMessage("RELOCBS", message, ToastType.Error);
                    return Request.IsAjaxRequest() ? (ActionResult)PartialView("Create", ViewData)
                    : View("Create", ViewData);
                }

                if (ViewData.vendorEvaluation.DestEvalQuestions.Exists(x => x.AnswerType.Equals("RADIOBTN", StringComparison.OrdinalIgnoreCase)) && ViewData.vendorEvaluation.DestEvalQuestions.Exists(x => x.Answer == null) && ViewData.vendorEvaluation.DestEvalQuestions.Exists(x => x.Answer != null))
                {
                    message = "Need to answer all Destination Partner questions";
                    ModelState.AddModelError(string.Empty, message);
                    this.AddToastMessage("RELOCBS", message, ToastType.Error);
                    return Request.IsAjaxRequest() ? (ActionResult)PartialView("Create", ViewData)
                    : View("Create", ViewData);
                }
            }
            else
            {

                if (ViewData.vendorEvaluation.OrgEvalQuestions.Exists(x => x.AnswerType.Equals("RADIOBTN", StringComparison.OrdinalIgnoreCase)) && ViewData.vendorEvaluation.OrgEvalQuestions.Exists(x => x.Answer == null))
                {
                    message = "Need to answer all Origin Partner questions";
                    ModelState.AddModelError(string.Empty, message);
                    this.AddToastMessage("RELOCBS", message, ToastType.Error);
                    return Request.IsAjaxRequest() ? (ActionResult)PartialView("Create", ViewData)
                    : View("Create", ViewData);
                }
                else if (ViewData.vendorEvaluation.OrgEvalQuestions.Exists(x => x.AnswerType.Equals("LIST", StringComparison.OrdinalIgnoreCase)) && ViewData.vendorEvaluation.OrgEvalQuestions.Exists(x => x.AnswerOptionID == null))
                {
                    message = "Need to answer all Origin Partner questions";
                    ModelState.AddModelError(string.Empty, message);
                    this.AddToastMessage("RELOCBS", message, ToastType.Error);
                    return Request.IsAjaxRequest() ? (ActionResult)PartialView("Create", ViewData)
                    : View("Create", ViewData);
                }

                if (ViewData.vendorEvaluation.DestEvalQuestions.Exists(x => x.AnswerType.Equals("RADIOBTN", StringComparison.OrdinalIgnoreCase)) && ViewData.vendorEvaluation.DestEvalQuestions.Exists(x => x.Answer == null))
                {
                    message = "Need to answer all Destination Partner questions";
                    ModelState.AddModelError(string.Empty, message);
                    this.AddToastMessage("RELOCBS", message, ToastType.Error);
                    return Request.IsAjaxRequest() ? (ActionResult)PartialView("Create", ViewData)
                    : View("Create", ViewData);
                }
                else if (ViewData.vendorEvaluation.DestEvalQuestions.Exists(x => x.AnswerType.Equals("LIST", StringComparison.OrdinalIgnoreCase)) && ViewData.vendorEvaluation.DestEvalQuestions.Exists(x => x.AnswerOptionID == null))
                {
                    message = "Need to answer all Destination Partner questions";
                    ModelState.AddModelError(string.Empty, message);
                    this.AddToastMessage("RELOCBS", message, ToastType.Error);
                    return Request.IsAjaxRequest() ? (ActionResult)PartialView("Create", ViewData)
                    : View("Create", ViewData);
                }
            }

            List<int> RateCompForSurveyCost = new List<int>();
            if (ViewData.SurveyDetail.OrgAgentID == null)
                RateCompForSurveyCost.Add(Convert.ToInt32(RELOCBS.Common.CommonService.RateComp.Origin));
            if (ViewData.SurveyDetail.FrtAgentID == null)
                RateCompForSurveyCost.Add(Convert.ToInt32(RELOCBS.Common.CommonService.RateComp.Freight));
            if (ViewData.SurveyDetail.DestAgentID == null)
                RateCompForSurveyCost.Add(Convert.ToInt32(RELOCBS.Common.CommonService.RateComp.Destination));

            FillCombo(Convert.ToString(ViewData.MoveJob.ModeID), ViewData.RMCBuss, RateCompForSurveyCost, ViewData.RMCType, Convert.ToInt32(ViewData.ServiceLineID), TransitMasterID: ViewData.FreightReport.TransInvMasterID, MoveId: Convert.ToString(ViewData.MoveID));
            if (ModelState.IsValid)
            {
                res = moveManageBL.InsertVendorEvaluation(ViewData, out message);
                if (!res)
                {
                    result.Success = false;
                    ModelState.AddModelError(string.Empty, "Unable to save vendor evaluation.");
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
                return RedirectToAction("Create", new { Indx = Indx, SurveyID = SurveyID, MoveID = MoveID });
            }
            else
            {
                return Request.IsAjaxRequest() ? (ActionResult)PartialView("Create", ViewData)
                  : View("Create", ViewData);
            }
        }

        public ActionResult AgentEvaluationReport()
        {
            _PageID = "52";

            if (!CSubs.CheckPageRights(_PageID, PermissionType.VIEW))
            {
                return new HttpStatusCodeResult(403);
            }
            ViewData["JobAssignedToList"] = comboBL.GetEmployeeDropdown(IsMoveCordination: true);
            session.Set<string>("PageSession", "Agent Evaluation Report");

            return View();
        }

        [HttpPost]
        public ActionResult AgentEvaluationReport(int? page)
        {
            ViewData["JobAssignedToList"] = comboBL.GetEmployeeDropdown(IsMoveCordination: true);

            Dictionary<string, string> param = new Dictionary<string, string>();
            try
            {

                DateTime Fromdate = System.DateTime.Now.Date.AddDays(-7);
                DateTime Todate = System.DateTime.Now;


                string Shipper = string.Empty;


                if (Request.Form["FromDate"] != null && Request.Form["FromDate"].Trim() != "")
                {
                    Fromdate = Convert.ToDateTime(Request.Form["FromDate"]);
                    param.Add("@SP_FromDate", Fromdate.ToString("dd-MMM-yyyy"));
                }

                if (Request.Form["ToDate"] != null && Request.Form["ToDate"].Trim() != "")
                {
                    Todate = Convert.ToDateTime(Request.Form["ToDate"]);
                    param.Add("@SP_ToDate", Todate.ToString("dd-MMM-yyyy"));
                }

                if (Request.Form["Shipper"] != null && Request.Form["Shipper"].Trim() != "")
                {
                    Shipper = Convert.ToString(Request.Form["Shipper"]);
                    param.Add("@SP_SHIPPER", Convert.ToString(Request.Form["Shipper"]));
                }

                string SearchKey = string.Empty;
                if (Request.Form["JobAssingedTo"] != null && Request.Form["JobAssingedTo"].Trim() != "")
                {
                    param.Add("@SP_JobAssingedTo", Convert.ToString(Request.Form["JobAssingedTo"]));
                }

                param.Add("@SP_CompID", UserSession.GetUserSession().CompanyID.ToString());

                if (UserSession.GetUserSession().BussinessLine == "NON RMC-BUSINESS")
                {
                    param.Add("@SP_IsRMCBuss", "false");
                }
                else
                {
                    param.Add("@SP_IsRMCBuss", "true");
                }

                param.Add("@SP_LoginID", Convert.ToString(UserSession.GetUserSession().LoginID));

                bool result = CommonService.GenerateExcel(this.Response, "AgentEvaluation", "[MoveMan].[GetVendorEvolForJobReport]", param);

                if (!result)
                {
                    this.AddToastMessage("RELOCBS", "No records to Download", ToastType.Error);
                }

                return View();
            }
            catch
            {
                return View();
            }
        }

        public ActionResult GetJobLabel(string Key)
        {

            if (!string.IsNullOrEmpty(Key))
            {

                Dictionary<string, string> list = CommonService.GetQueryString(Key);
                Int64 MoveID = -1;
                if (list.ContainsKey("MoveID"))
                {
                    MoveID = Convert.ToInt64(list["MoveID"]);
                }
                if (MoveID > 0)
                {
                    return View("JobLabel", moveManageBL.GetJobLable(MoveID));
                }

            }
            return new HttpStatusCodeResult(403);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetReport(FormCollection formcollection)
        {
            string MID = Convert.ToString(formcollection["MoveID"]);
            string RID = Convert.ToString(formcollection["ReportID"]);
            string SID = Convert.ToString(formcollection["SurveyID"]);
            string EDID = Convert.ToString(formcollection["EnqDetailID"]);
            string EID = Convert.ToString(formcollection["EnqID"]);
            string EnqNo = Convert.ToString(formcollection["EnqNo"]);
            int ReportID, JobReportID = 0;
            Int64 MoveID, EnqDetailID, SurveyID, EnqID, RateCompRateWtID = 0, RateCompRateWtBatchID = 0;
            bool IsLumsum = false, IsVoxme = false;


            Int64.TryParse(MID, out MoveID);

            if (Int64.TryParse(EDID, out EnqDetailID) && Int64.TryParse(SID, out SurveyID) && Int64.TryParse(EID, out EnqID) && Int32.TryParse(RID, out ReportID))
            {
                if (ReportID == 4)
                {

                    Tuple<Int64, Int64, Int64, bool> result = moveManageBL.GetJobReportParams(MoveID, SurveyID, ReportID);
                    RateCompRateWtID = result.Item1;
                    RateCompRateWtBatchID = result.Item2;
                    IsLumsum = result.Item4;
                    if (RateCompRateWtID == -1 && RateCompRateWtBatchID == -1)
                    {
                        return new HttpStatusCodeResult(422, "Quote not saved.");
                        //return new HttpStatusCodeResult(500, "Quote not saved.");


                    }
                }

                EncryptedQueryString query = new EncryptedQueryString();
                query["PageID"] = _PageID;

                switch (ReportID)
                {
                    case 1:////Enq
                        query["ReportID"] = "10001";
                        query["EnqID"] = Convert.ToString(EnqID).Trim();
                        break;
                    case 2:///EnqDetail
						query["ReportID"] = "10002";
                        query["EnqDetailID"] = Convert.ToString(EnqDetailID);
                        query["SurveyID"] = Convert.ToString(SurveyID);
                        break;
                    case 3:////Estimate Compare
                        query["ReportID"] = "10005";
                        query["surveyid"] = Convert.ToString(SurveyID);
                        break;
                    case 4:////Quote Lumsum/Breakdown
                        query["ReportID"] = "10006";
                        query["surveyid"] = Convert.ToString(SurveyID);
                        query["Wtid"] = Convert.ToString(RateCompRateWtID);
                        query["Batchid"] = Convert.ToString(RateCompRateWtBatchID);
                        query["IsLumsum"] = Convert.ToString(IsLumsum);
                        query["IsEmail"] = "1";
                        break;
                    case 5:////Voxme 
                        IsVoxme = true;
                        break;
                    case 7:////PJR
                        JobReportID = 1;
                        break;
                    case 8:////DJR
                        JobReportID = 3;
                        break;
                    default:
                        break;
                }


                if (IsVoxme)
                {
                    JobDocument jobDocument = new SurveyBL().GetVoxmeReport(EnqID, EnqNo);
                    if (!string.IsNullOrWhiteSpace(jobDocument.FilePath) && System.IO.File.Exists(jobDocument.FilePath))
                    {

                        return File(jobDocument.FilePath, MimeMapping.GetMimeMapping(jobDocument.FilePath), jobDocument.FileName);
                    }
                    return new HttpStatusCodeResult(404);
                }
                else if (ReportID == 7 || ReportID == 8)
                {
                    string param = "MoveID=ParamValue0&ComponentID=ParamValue1&PJRDJRID=ParamValue2";
                    return RedirectToAction("GetJobReportForMove", "JobReport", new { Key = CommonService.GenerateQueryString(param, new string[] { MoveID.ToString(), JobReportID.ToString(), "0" }) });
                }
                else
                {
                    if (ReportID == 6)
                    {
                        return RedirectToAction("GetReport", "Billing_Collection", new { EnqDetailID = EnqDetailID });
                    }
                    else
                    {
                        return this.RedirectSameDomain("/Reports/ReportViewer.aspx?args=" + query.ToString());
                    }

                }


            }

            return RedirectToAction("Index", "Home");
        }

        public ActionResult GetDocumentList(Int64 id, Int16? DocTypeID, int? DocNameID, string DocDescription = "")
        {
            MoveManageViewModel ViewData = new MoveManageViewModel();
            int DTypeID = DocTypeID == null ? -1 : Convert.ToInt16(DocTypeID);
            int DNameID = DocNameID == null ? -1 : Convert.ToInt32(DocNameID);
            ViewData.jobDocUpload = moveManageBL.GetDocumentGrid(id, "MoveMan", "MoveMan", DTypeID, DNameID, DocDescription);
            return PartialView("_DocListPartial", ViewData);
        }

        public ActionResult GetTransitVessel(Int64 id)
        {

            DataTable result = moveManageBL.GetTrasshipmentVessel(id);

            bool value = CommonService.GenerateExcel(this.Response, result, "Transit-Sailing-Flight_" + "_" + Guid.NewGuid().ToString());

            if (!value)
            {
                return new HttpStatusCodeResult(404, "No records");
            }

            return View();
        }

        [HttpGet]
        public JsonResult GetJobWt(Int64 id)
        {
            var data = moveManageBL.getGetJobWtDetail(id);
            return Json(new { data = new { MoveID = data.MoveID, JobNo = data.JobNo, Shipper = data.Shipper, WtVolUnit = data.WtVolUnit, WtVol = data.WtVol, Vol = data.Vol, VolUnit = data.VolUnit, WtVolUnitId = data.WtVolUnitId, VolUnitId = data.VolUnitId, NoOfPacks = data.NoOfPacks } }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult CheckCreditPrivateClient(Int64 id, bool IsWIS = false)
        {
            var data = moveManageBL.CheckCreditPrivateClient(id, IsWIS);
            return Json(new { result = data }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SurveyToMobile(JobSurveyForMobile model)
        {
            bool res = false;
            string message = string.Empty;
            AjaxResponse result = new AjaxResponse();
            if (ModelState.IsValid)
            {
                res = new EnquiryBL().UpdateSurveyorIdGMMS(model, out message);
                if (!res)
                {
                    result.Success = false;
                    ModelState.AddModelError(string.Empty, "Unable to save survey send to mobile.");
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
                return RedirectToAction("Create", new { Indx = model.Indx, SurveyID = model.SurveyID, MoveID = model.MoveID });
            }
            else
            {
                return Request.IsAjaxRequest() ? (ActionResult)PartialView("Create", ViewData)
                  : View("Create", ViewData);
            }
        }

        [HttpGet]
        public JsonResult GetMailFormat(Int64 TransID, Int64 ActivityID, Int64 MoveID)
        {
            var data = moveManageBL.GetMailFormat(ActivityID, MoveID, TransID);
            return Json(new { result = data }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetMailHistList(Int64 ActivityID, Int64 MoveID)
        {
            var data = moveManageBL.GetMailActivityHistory(MoveID, ActivityID);
            return Json(new { result = data }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SaveACODetails(int Indx, int SurveyID, int? MoveID, MoveManageViewModel ViewData)
        {
            bool res = false;
            string message = string.Empty;
            AjaxResponse result = new AjaxResponse();
            if (ModelState.IsValid)
            {
                res = moveManageBL.InsertACODetails(ViewData, out message);
                if (!res)
                {
                    result.Success = false;
                    ModelState.AddModelError(string.Empty, "Unable to save ACO Detail.");
                    result.Message = message;
                    this.AddToastMessage("RELOCBS", message, ToastType.Error);
                }
                else
                {
                    result.Success = true;
                    result.Message = message;
                    this.AddToastMessage("RELOCBS", message, ToastType.Success);
                }
                return RedirectToAction("Create", new { Indx = Indx, SurveyID = SurveyID, MoveID = MoveID });
            }
            else
            {
                return Request.IsAjaxRequest() ? (ActionResult)PartialView("Create", ViewData)
                  : View("Create", ViewData);
            }
        }

        public JsonResult GetACODetails(Int64 MoveID)
        {
            try
            {
                ACODetails ACODetailsObj = new ACODetails();
                string errormsg = null;
                ACODetailsObj = moveManageBL.GetACODetails(MoveID);
                errormsg = ACODetailsObj == null ? "ACO Details not found." : "";
                return Json(new { ACODetails = ACODetailsObj, errormsg = errormsg }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public ActionResult RequestDocs(int? MoveID)
        {
            try
            {
                MoveManageViewModel MoveViewModel = moveManageBL.GetDetailById(0, MoveID, false, false, false);
                ViewData["DocTypeList"] = comboBL.GetJobDocTypelDropdown(DocFromType: "MoveMan");
                ViewData["DocNameList"] = new List<SelectListItem>();
                return View(MoveViewModel);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult RequestDocs(MoveManageViewModel ViewData)
        {
            try
            {
                bool result = false;
                string message = string.Empty;

                result = moveManageBL.InsertRequestDocsData(ViewData, UserSession.GetUserSession().LoginID, out message);
                if (result)
                {
                    Int64? RequestDocsGroupID = ViewData.RequestDocsGroupID;

                    string ReqDocsUploadUrl = System.Configuration.ConfigurationManager.AppSettings["ReqDocsUploadUrl"].ToString();
                    string url = ReqDocsUploadUrl + CommonService.GetCrypt("MoveID=" + ViewData.MoveID.ToString() + "&RequestDocsGroupID=" + ViewData.RequestDocsGroupID.ToString(), 1);

                    ViewData.RequestDocsEmail.Body += "&lt;/br&gt;&lt;p&gt;Click here to upload documents : " + url + "&lt;/p&gt;";//&lt;p&gt;If you have any queries related to this email, please contact Information Technology Dept at reloit@writerrelocations.com&lt;/p&gt;";

                    if (SendMailFunction(ViewData.RequestDocsEmail))
                    {
                        ViewData.MoveEmail = ViewData.RequestDocsEmail;
                        string errorMessage = string.Empty;
                        result = moveManageBL.InsertEmailData(ViewData, UserSession.GetUserSession().LoginID, out errorMessage);
                        this.AddToastMessage("RELOCBS", message, ToastType.Success);
                    }
                    else
                    {
                        this.AddToastMessage("RELOCBS", message, ToastType.Error);
                        CSubs.LogError("MoveManagement", "RequestDocs", "Email Send Fail.");
                    }
                }
                else
                {
                    this.AddToastMessage("RELOCBS", message, ToastType.Error);
                    CSubs.LogError("MoveManagement", "RequestDocs", "Error while saving data.");
                }

                return RedirectToAction("RequestDocs", new { ViewData.MoveID });
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult RequestDocsUpload(string Key)
        {
            try
            {
                var list = CommonService.GetQueryString(Key);

                Int64 MoveID = 0;
                Int64 RequestDocsGroupID = 0;

                if (list.ContainsKey("MoveID"))
                    MoveID = Convert.ToInt64(list["MoveID"]);

                if (list.ContainsKey("RequestDocsGroupID"))
                    RequestDocsGroupID = Convert.ToInt64(list["RequestDocsGroupID"]);

                MoveManageViewModel MoveViewModel = moveManageBL.GetRequestDocsDetails(MoveID, RequestDocsGroupID);
                MoveViewModel.MoveID = MoveID;
                MoveViewModel.RequestDocsGroupID = RequestDocsGroupID;

                return View(MoveViewModel);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult RequestDocsUpload(MoveManageViewModel ViewData)
        {
            try
            {
                bool result = false;
                string message = string.Empty;

                ViewData.jobDocUpload = ViewData.RequestDocsUploadList[ViewData.RowID];
                ViewData.jobDocUpload.file = ViewData.jobDocUpload.ExtFile;
                ViewData.jobDocUpload.ID = ViewData.MoveID ?? 0;

                result = moveManageBL.InsertDocument(ViewData, 0, out message);
                if (result)
                {
                    string errorMessage = string.Empty;
                    result = moveManageBL.InsertRequestDocsData(ViewData, 0, out errorMessage);
                    this.AddToastMessage("RELOCBS", message, ToastType.Success);
                }
                else
                {
                    this.AddToastMessage("RELOCBS", message, ToastType.Error);
                    CSubs.LogError("MoveManagement", "RequestDocsUpload", "Error while saving file.");
                }

                return RedirectToAction("RequestDocsUpload", new { Key = CommonService.GetCrypt("MoveID=" + ViewData.MoveID.ToString() + "&RequestDocsGroupID=" + ViewData.RequestDocsGroupID.ToString(), 1) });
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [AllowAnonymous]
        public ActionResult AddEditShipperFeedback(MoveManageViewModel ViewData, string SaveButton)
        {
            try
            {
                bool result = false;
                string message = string.Empty;

                int LoginID = UserSession.GetUserSession()?.LoginID ?? 0;
                int CompanyID = UserSession.GetUserSession()?.CompanyID ?? 0;

                if (SaveButton.ToUpper() == "SUBMIT")
                {
                    result = moveManageBL.AddEditShipperFeedback(ViewData, LoginID, CompanyID, true, null, out message);
                    if (result)
                    {
                        this.AddToastMessage("RELOCBS", message, ToastType.Success);
                    }
                    else
                    {
                        this.AddToastMessage("RELOCBS", message, ToastType.Error);
                        CSubs.LogError("MoveManagement", "AddEditShipperFeedback", "Error while saving.");
                    }
                    return RedirectToAction("ShipperFeedback", new { Key = CommonService.GetCrypt("MoveID=" + ViewData.MoveID + "&SFTemplateID=" + ViewData.ShipperFeedback.SFTemplateID + "&ShipperFeedbackID=" + ViewData.ShipperFeedback.ShipperFeedbackID, 1) });
                }
                else if (SaveButton.ToUpper() == "SEND EMAIL" || SaveButton.ToUpper() == "RESEND EMAIL")
                {
                    result = moveManageBL.AddEditShipperFeedback(ViewData, LoginID, CompanyID, false, null, out message);
                    if (result)
                    {
                        string ApplicationUrl = System.Configuration.ConfigurationManager.AppSettings["ApplicationUrl"].ToString();
                        string url = ApplicationUrl + "MoveManage/ShipperFeedback?Key=" + CommonService.GetCrypt("MoveID=" + ViewData.MoveID + "&SFTemplateID=" +
                            ViewData.ShipperFeedback.SFTemplateID + "&ShipperFeedbackID=" + ViewData.ShipperFeedback.ShipperFeedbackID, 1);

                        result = moveManageBL.AddEditShipperFeedback(ViewData, LoginID, CompanyID, false, url, out message);
                        if (result)
                        {
                            this.AddToastMessage("RELOCBS", message, ToastType.Success);
                        }
                        else
                        {
                            this.AddToastMessage("RELOCBS", "Email Send Fail", ToastType.Error);
                            CSubs.LogError("MoveManagement", "AddEditShipperFeedback", "Email Send Fail.");
                        }
                    }
                    else
                    {
                        this.AddToastMessage("RELOCBS", message, ToastType.Error);
                        CSubs.LogError("MoveManagement", "AddEditShipperFeedback", "Error while saving.");
                    }
                }
                return RedirectToAction("Create", new { Indx = 9, SurveyID = ViewData.SurveyID, MoveID = ViewData.MoveID, IsGetCost = false });
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [AllowAnonymous]
        public ActionResult ShipperFeedback(string Key)
        {
            try
            {
                var list = CommonService.GetQueryString(Key);

                Int64 MoveID = 0;
                int SFTemplateID = 0;
                Int64 ShipperFeedbackID = 0;

                if (list.ContainsKey("MoveID"))
                    MoveID = Convert.ToInt64(list["MoveID"]);

                if (list.ContainsKey("SFTemplateID"))
                    SFTemplateID = Convert.ToInt32(list["SFTemplateID"]);

                if (list.ContainsKey("ShipperFeedbackID"))
                    ShipperFeedbackID = Convert.ToInt64(list["ShipperFeedbackID"]);

                int LoginID = UserSession.GetUserSession()?.LoginID ?? 0;
                MoveManageViewModel MoveViewModel = new MoveManageViewModel();
                MoveViewModel.ShipperFeedback = moveManageBL.GetShipperFeedbackTemplate(LoginID, MoveID, SFTemplateID, ShipperFeedbackID, false);
                MoveViewModel.MoveID = MoveID;

                return View(MoveViewModel);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public JsonResult GetGPAmount(decimal RevAmt, decimal GPPercent, int MoveID, string BaseCurr)
        {
            try
            {

                string errormsg = null;
                Tuple<decimal, decimal, int> result = moveManageBL.GetGPAmount(RevAmt, GPPercent, MoveID, BaseCurr);
                //errormsg = InvoiceList.Count < 0 && IsStatement ? "No Invoice allcated to this job." : "";
                return Json(new { GPAmount = result.Item1, GPPercent = result.Item2, GPMasterID = result.Item3, errormsg = errormsg }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public JsonResult GetGDPRNationality(string NationaLity)
        {
            try
            {

                string errormsg = null;
                bool result = moveManageBL.GetGDPRNationality(NationaLity);
                //errormsg = InvoiceList.Count < 0 && IsStatement ? "No Invoice allcated to this job." : "";
                return Json(new { ISGDPRNationalty = result, errormsg = errormsg }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }


        public bool CheckGPValid(int MoveID, int TabIndex, string BaseCurr)
        {
            bool returnvar = true;
            DataSet dsCostSheet = moveManageBL.GetCostSheet(MoveID);

            DataTable drRev = dsCostSheet != null ? dsCostSheet.Tables[0].Select("MoveCompName = 'TOTAL'").CopyToDataTable() : null;
            DataTable drPercent = dsCostSheet != null ? dsCostSheet.Tables[0].Select("MoveCompName = 'GP %'").CopyToDataTable() : null;


            //string PrevStage = GPApprovalDisplayList.FirstOrDefault().Stage;
            decimal GPTotalRevenue = 0, MasterRev = 0;
            decimal GPPercent = 0, MasterPercent = 0;
            if (TabIndex == 4)
            {
                GPTotalRevenue = Convert.ToDecimal(drRev.Rows[0]["DeliveryRevenVal"]);
                GPPercent = Convert.ToDecimal(drPercent.Rows[0]["DeliveryRevenVal"]);

                //Model.GPAmount = GPTotalRevenue;
                //GPTotalCost = Model.SurveyCostList.CostList.Sum(x => x.CostValue);
            }
            else if (TabIndex == 2)
            {
                GPTotalRevenue = Convert.ToDecimal(drRev.Rows[0]["PackRevenVal"]);
                GPPercent = Convert.ToDecimal(drPercent.Rows[0]["PackRevenVal"]);
                //Model.GPAmount = GPTotalRevenue;
                //GPTotalCost = Model.PackingCostList.CostList.Sum(x => x.CostValue);
            }
            else if (TabIndex == 1)
            {
                GPTotalRevenue = Convert.ToDecimal(drRev.Rows[0]["SurveyRevenVal"]);
                GPPercent = Convert.ToDecimal(drPercent.Rows[0]["SurveyRevenVal"]);
                //GPTotalRevenue = Model.DeliveryCostList.CostList.Sum(x => x.RevenueValue);
                //Model.GPAmount = GPTotalRevenue;
                //GPTotalCost = Model.DeliveryCostList.CostList.Sum(x => x.CostValue);
            }
            Tuple<decimal, decimal, bool, string> Prevresult = moveManageBL.GetPrevGPAmount(MoveID);

            //BaseCurr = Prevresult.Item4;
            Tuple<decimal, decimal, int> result = moveManageBL.GetGPAmount(GPTotalRevenue, GPPercent, MoveID, BaseCurr);
            if ((Prevresult.Item3 && (Prevresult.Item1 != GPTotalRevenue || Prevresult.Item2 != GPPercent)) || !(Prevresult.Item3))
            {
                if (!(Prevresult.Item3) && result.Item1 > 0)
                {
                    returnvar = result.Item2 == 0 ? !(result.Item2 >= GPPercent) : !(result.Item2 > GPPercent && result.Item1 > GPTotalRevenue);
                }
                else
                {
                    returnvar = !(result.Item2 > GPPercent && result.Item1 > GPTotalRevenue);
                }
            }

            return returnvar;
        }

        public ActionResult UnlockSTGDate(int Indx, int SurveyID, int? MoveID, MoveManageViewModel ViewData)
        {
            bool res = false;
            string message = string.Empty;
            AjaxResponse result = new AjaxResponse();
            if (ModelState.IsValid)
            {
                res = moveManageBL.UnlockSTGDate(ViewData, out message);
                if (!res)
                {
                    result.Success = false;
                    ModelState.AddModelError(string.Empty, "Unable to Unlock.");
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
                return RedirectToAction("Create", new { Indx = Indx, SurveyID = SurveyID, MoveID = MoveID });
            }
            else
            {
                return Request.IsAjaxRequest() ? (ActionResult)PartialView("Create", ViewData)
                  : View("Create", ViewData);
            }
        }
    }
}
