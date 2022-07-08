using MvcValidationExtensions.Attribute;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Web;
using System.Web.Mvc;

namespace RELOCBS.Entities
{
    public class MoveManage
    {
        public int? SurveyID { get; set; }

        public int EnqID { get; set; }

        public int EnqDetailID { get; set; }

        public int RMCID { get; set; }

        public string RMCName { get; set; }

        public int BusinessLineID { get; set; }

        public string BusinessLineName { get; set; }

        public int GoodsDescriptionID { get; set; }

        public string GoodsDescriptionName { get; set; }

        public int ModeID { get; set; }
        public string ModeName { get; set; }

        public int FromLocationID { get; set; }

        public string FromLocationName { get; set; }

        public int ToLocationID { get; set; }

        public string ToLocationName { get; set; }

        public float ConversionRate { get; set; }

        public int WeightUnitID { get; set; }

        public string WeightUnitName { get; set; }

        public float WeightUnitFrom { get; set; }

        public float WeightUnitTo { get; set; }

        public string RateReceived { get; set; }

        public int RateCompRateWtID { get; set; }

        public int RateCompRateWtBatchID { get; set; }

        public List<CostHeadDetail> CostHeadList { get; set; }

    }

    public class MoveManageViewModel
    {
        public Int64? MoveID { get; set; }

        public Int64? InvoiceID { get; set; }

        public string JobNo { get; set; }

        public string OldJobNo { get; set; }

        public DateTime? JobDate { get; set; }

        public Int64? SurveyID { get; set; }

        public Int64? EnqDetailID { get; set; }

        public Int64? EnqID { get; set; }

        public Int64? EnqShpNo { get; set; }

        public Int32? ServiceLineID { get; set; }

        public string ServiceLine { get; set; }

        public int CompanyID { get; set; }

        public int? SurveyNo { get; set; }

        public string EnqNo { get; set; }

        public int? RateCompRateWtID { get; set; }

        public int? RateCompRateWtBatchID { get; set; }

        public MovJobOpening MoveJob { get; set; } = new MovJobOpening();

        public string Flag { get; set; }

        public bool RMCBuss { get; set; }

        public bool IsSOCost { get; set; }

        public List<RmcFees> RMCFees { get; set; }

        public string RMCType { get; set; }

        public string DestApprove { get; set; }
        public bool IsDestApprove { get; set; }
        public bool IsCSSenttoApprove { get; set; }
        public int? CSSenttoApproveUser { get; set; }
        public bool IsDTD { get; set; }

		public decimal DefaultGPPercent { get; set; }
		public bool GPSendForApproval { get; set; }
		public bool IsGPApproved { get; set; }

		//GPApprovalDetail for get
		public bool IsGPProcess { get; set; }
		public bool IsGPSendAppr { get; set; }
		public bool IsGPSendSD { get; set; }
		public bool IsGPAppr { get; set; }
		public string GPRemark { get; set; }
		//GPApprovalDetail end

		[Display(Name = "Origin Warehouse")]
		public int? OrgWarehouse { get; set; }
		[Display(Name = "Destination Warehouse")]
		public int? DestWarehouse { get; set; }
		public bool IsOutSourced { get; set; }
		public bool IsDestWHSave { get; set; }
		public bool IsOrgWHSave { get; set; }

        public bool ISGDPRNationalty { get; set; }
        public bool ISGDPRFileUploaded { get; set; }
        //public int BaseCurrID { get; set; }
        //public string BaseCurr { get; set; }

        //Survey Info
        public PackingSO SurveySOList { get; set; } = new PackingSO();
        public ReportView SurveyReport { get; set; } = new ReportView();
        public Detail SurveyDetail { get; set; } = new Detail();
        public PackingCost SurveyCostList { get; set; } = new PackingCost();


        //Packing Info
        public Detail PackingDetail { get; set; } = new Detail();
        public PackingSO PackingSOList { get; set; } = new PackingSO();
        public PackingCost PackingCostList { get; set; } = new PackingCost();
        public ReportView PackingReport { get; set; } = new ReportView();

        //Freight Info
        public Detail FreightDetail { get; set; } = new Detail();
        public PackingSO FreightSOList { get; set; } = new PackingSO();
        public PackingCost FreightCostList { get; set; } = new PackingCost();
        public ReportView FreightReport { get; set; } = new ReportView();

        //Delivery Info
        public Detail DeliveryDetail { get; set; } = new Detail();
        public PackingSO DeliverySOList { get; set; } = new PackingSO();
        public PackingCost DeliveryCostList { get; set; } = new PackingCost();
        public ReportView DeliveryReport { get; set; } = new ReportView();

        public DataSet dsCostSheet { get; set; }

        public MoveCostMaster MoveCostMst { get; set; } = new MoveCostMaster();

        public EmailList MoveEmail { get; set; } = new EmailList();

        public int RowID { get; set; }

        public Int64? RequestDocsGroupID { get; set; }

        public JobDocUpload RequestDocsUpload { get; set; } = new JobDocUpload();

        public List<JobDocUpload> RequestDocsUploadList { get; set; } = new List<JobDocUpload>();

        public EmailList RequestDocsEmail { get; set; } = new EmailList();

        public JobDocUpload jobDocUpload { get; set; } = new JobDocUpload();

        public int TabIndex { get; set; }

        public int? UpdatedBatchId { get; set; }

        public String CombinationID { get; set; }

        public FollowUpDetails FollowUp { get; set; }

        public ACODetails ACODetails { get; set; } = new ACODetails();

        public List<FollowUpDetails> FollowUpList { get; set; }
		
        public InsuranceBySD Insurance { get; set; } = new InsuranceBySD();
		

		public JobCancel JobCancel { get; set; } = new JobCancel();

        public bool HideSurveySave { get; set; }

        public bool ShowAdvanceCaution { get; set; }
        [Required(ErrorMessage = "Insurance By is mandatory")]
        public int? InsurBy { get; set; }
        [Required(ErrorMessage = "HO SD is mandatory")]
        public int? HoSdEmpID { get; set; }
        [Required(ErrorMessage = "Branch SD is mandatory")]
        public int? BrSdEmpID { get; set; }
        [Required(ErrorMessage = "Dest Branch SD is mandatory")]
        public int? DestBrSdEmpID { get; set; }
        public string Project { get; set; }

        public List<TabList> TabList { get; set; } = new List<TabList>();

        public JobVendorEvaluation vendorEvaluation { get; set; } = new JobVendorEvaluation();

        public string JobStatus { get; set; }

        public string ForwardingFlag { get; set; }

        public string ClientType { get; set; }

        /// <summary>
        /// For Private Client below fields need to be Compulsary (Cheque_No,Cheque_Amt)
        /// </summary>

        [Display(Name = "Cheque No./UTR No./Cash")]
        public string Cheque_No { get; set; }

        [Display(Name = "Amount")]
        public string Cheque_Amt { get; set; }
        [Display(Name = "Remark")]
        public string Cheque_Remark { get; set; }

        public int? SurveyerID { get; set; }
        public Int16 ShowSendToMobile { get; set; }

        public bool ISDeliveryDateValid { get; set; }

		public bool IsGCCInsurance { get; set; }

		public List<EmailActiviryHistory> EmailActivityhistory { get; set; }
        public InsuranceDetail insuranceDetail { get; set; } = new InsuranceDetail();
		public GCCInsuranceDetail GCCinsuranceDetail { get; set; } = new GCCInsuranceDetail(); 
		public bool IsShowCloseJob { get; set; }
        public string CloseJobRemark { get; set; }
        public string CloseJobBy { get; set; }
        public DateTime? CloseJobDate { get; set; }
        public bool IsInvPrepared { get; set; }
        public ShipperFeedback ShipperFeedback { get; set; }

        public int? TransitAgent { get; set; }

		public int? GPSendForApprovalUser { get; set; }

		public decimal? GPPercent { get; set; }
		public decimal? GPAmount { get; set; }
		public int GPMasterID { get; set; }
		public List<GPApprovalDisplayList> GPApprovalDisplayList { get; set; }
		public string BaseCurr { get; set; }
        public bool IsShowSTGUnlock { get; set; }
        public bool IsSTGUnlock { get; set; }
    }

    public class TabList
    {
        public int TabIndex { get; set; }
    }

    public class PackingSO
    {

        public string HFSOList { get; set; }
        public List<PackingSOList> SOList { get; set; } = new List<PackingSOList>();
    }

    public class PackingCost
    {
        public string HFCostList { get; set; }
        public bool CostListSaved { get; set; } = false;
        public List<PackingCostList> CostList { get; set; } = new List<PackingCostList>();
    }

    public class MovJobOpening
    {
        [Required(ErrorMessage = "Weight/Volume Unit is required")]
        public int WeightUnitID { get; set; }

        [Display(Name = "Weight Unit")]
        public string WeightUnitName { get; set; }

        [Display(Name = "Weight From")]
        [Required(ErrorMessage = "Weight From is required")]
        [Range(0, float.MaxValue, ErrorMessage = "Please enter a value bigger than {1}")]
        public float WeightUnitFrom { get; set; }

        [Display(Name = "Weight To")]
        [Required(ErrorMessage = "Weight To is required")]
        [Range(0, float.MaxValue, ErrorMessage = "Please enter a value bigger than {1}")]
        //[GreaterThanEqualTo("WeightUnitFrom", ErrorMessage = "Weight To must be greater than Weight From")]
        public float WeightUnitTo { get; set; }

        //[Required(ErrorMessage = "Shiping Line is required")]
        public int? ShipingLineID { get; set; }

        [Required(ErrorMessage = "Shipment Type is required")]
        public int ShipmentTypeID { get; set; }

        public int? ContainerTypeID { get; set; }

        public string ShipingLineName { get; set; }

        public DateTime? TentativeMoveDate { get; set; }

        [Required(ErrorMessage = "RMC is required")]
        public int RMCID { get; set; }

        [Display(Name = "RMC")]
        public string RMCName { get; set; }

        [Required(ErrorMessage = "BusinessLine is required")]
        public int BusinessLineID { get; set; }

        [Display(Name = "Business Line")]
        public string BusinessLineName { get; set; }

        [Required(ErrorMessage = "Goods Description is required")]
        public int GoodsDescriptionID { get; set; }

        [Display(Name = "Goods Description")]
        public string GoodsDescriptionName { get; set; }

        [Required(ErrorMessage = "Mode is required")]
        public int ModeID { get; set; }

        [Display(Name = "Mode")]
        public string ModeName { get; set; }

        [Required(ErrorMessage = "From City is required")]
        public int FromLocationID { get; set; }
        [Display(Name = "From City")]
        public string FromLocationName { get; set; }

        [Required(ErrorMessage = "To City is required")]
        public int ToLocationID { get; set; }
        [Display(Name = "To City")]
        public string ToLocationName { get; set; }

        //[Required(ErrorMessage = "Entry Port is required")]
        public int EntryPointID { get; set; } = 0;

        [Display(Name = "Entry Port")]
        public string EntryPointName { get; set; }

        //[Required(ErrorMessage = "Exit Port is required")]
        public int ExitPointID { get; set; } = 0;

        [Display(Name = "Exit Port")]
        public string ExitPointName { get; set; }

        /// <summary>
        /// Billing collection properties
        /// </summary>
        public int? chgAccountMgr { get; set; }
        public string AccountMgr { get; set; }
        public int? BillingOnClientId { get; set; }
        public ShipperDetail Shipper { get; set; } = new ShipperDetail();
        public string RevenueBr { get; set; }
        public string ServiceLine { get; set; }
        public int? ClientId { get; set; }
        public string ClientGSTNO { get; set; }
        public int? AccountId { get; set; }
        public string AccountGSTNO { get; set; }
        public string PreparedBy { get; set; }

        public List<MoveRateComponent> MoveRateCompList { get; set; } = new List<MoveRateComponent>();


        public string HFVMoveRateCompList { get; set; }

        public string OrgAdd { get; set; }
        public string OrgAdd2 { get; set; }

        public int? OrgCityID { get; set; }

        public string OrgPhone { get; set; }
        public string OrgEmail { get; set; }
        public string OrgPin { get; set; }

        public string DestAdd { get; set; }
        public string DestAdd2 { get; set; }
        public int? DestCityID { get; set; }
        public string DestPhone { get; set; }
        public string DestEmail { get; set; }
        public string DestPin { get; set; }
        [MaxLength(100)]
        [RegularExpression(@"[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}", ErrorMessage = "Please enter correct email")]
        public string Email { get; set; }

        [Display(Name = "Work Order No")]
        public string WKNo { get; set; }

        [Display(Name = "File No./Ref No.")]
        public string RMCFileNo { get; set; }

        public int? MoveCoordinatorID { get; set; }

        [Display(Name = "Assign Move Coordinator")]
        public string MoveCoordinator { get; set; }

        public int? AssistingMoveCoordinatorID { get; set; }

        [Display(Name = "Assisting Move Coordinator")]
        public string AssistingMoveCoordinator { get; set; }

        //[DataType(DataType.Upload)]
        [Display(Name = "OFS Document")]
        public HttpPostedFileBase OFSDocument { get; set; }

        public List<MoveRateComponent> GetDefaultAgentList()
        {
            List<MoveRateComponent> List = new List<MoveRateComponent>();
            List.Add(new MoveRateComponent() { RateComponentID = 1 });
            List.Add(new MoveRateComponent() { RateComponentID = 2 });
            List.Add(new MoveRateComponent() { RateComponentID = 3 });
            return List;
        }

        public bool ShowGetCost { get; set; }

		public int? FinancePerson { get; set; }

	}

    public class Detail
    {

        public int MoveID { get; set; }
        public bool IsDone { get; set; } = false;
        public int? OrgAgentID { get; set; }
        public string OrgAgentName { get; set; }
        public int? FrtAgentID { get; set; }
        public int? DestAgentID { get; set; }
        public string DestAgentName { get; set; }
        public int? ExitPortID { get; set; }
        public string ExitPortName { get; set; }
        public int? EntryPortID { get; set; }
        public string EntryPortName { get; set; }

    }

    public class ReportView
    {
        //Survey
        public DateTime? SchSurveydate { get; set; }
        public DateTime? Surveydate { get; set; }
        public TimeSpan? SurveyDateTime { get; set; }

        public int? SurveyorID { get; set; }
        //Packing
        public int Createdby { get; set; }
        public decimal Createddate { get; set; }
        public int Isactive { get; set; }
        public DateTime? ScheduledPackDate { get; set; }

        //Destination
        public DateTime? DeliveryDate { get; set; }
        [Display(Name = "Claim")]
        public bool IsClaim { get; set; }


        public string PassportNo { get; set; }

        //Freight
        //Common Freight
        //public int? Noofpakage { get; set; }
        public DateTime? ShipmentCartedOn { get; set; }// also used for ShipmentLoadedOn in AirMode
        public bool IsBLSentToAgent { get; set; }
        public bool IsDirectCarting { get; set; }

        public DateTime? BLSentToAgentOn { get; set; }//also used for AWBSentToAgentOn in AirMode
        public DateTime? CustomeClearedOn { get; set; }
        public DateTime? BLReleaseOn { get; set; }//also used for AirwayBillReleaseOn in AirMode
        public bool IsSD { get; set; }
        public DateTime? SD { get; set; }//Shipment Ready for Dispatch
        public DateTime? OPS { get; set; }//Comfirm Ready for Dispatch
        public string Bill_No { get; set; }
        public string SealNo { get; set; }//only sea & road

        [Display(Name = "Tranship Shipment")]
        public bool TransitShipment { get; set; }

        //Sea
        public DateTime? SB_GivenOn { get; set; }
        public string ContainerNo { get; set; }
        //public string SchVessel { get; set; }
        public string SSAgent { get; set; }

        public string FS_DS { get; set; }
        public bool IsISF { get; set; }
        public string ISF_Ref { get; set; }
        public string LCL_FCL { get; set; }
        public int? ContainerTypeId { get; set; }
        //AirWays
        public int? AirLine { get; set; }
        public int? Courier { get; set; }
        //public decimal UnitFreightRate{ get; set; }
        //public decimal Total { get; set; }
        //Road
        public string TruckNo { get; set; }
        public int VehicleTypeId { get; set; }
        public float TotalCapacity { get; set; }
        public string EsordedBy { get; set; }
        public DateTime? LeftOnDate { get; set; }

        //Common
        public DateTime? Packdate { get; set; }
        public DateTime? Loaddate { get; set; }
        public string Mode { get; set; }
        public int? VolumeUnitID { get; set; } = 0;
        public decimal? TobePackedVol { get; set; } = 0;
        public decimal? NetVol { get; set; } = 0;
        public decimal? GrossVol { get; set; } = 0;
        public int WeightUnitID { get; set; } = 0;
        public decimal? NetWt { get; set; } = 0;
        public decimal? GrossWt { get; set; } = 0;
        public decimal? ACWTWt { get; set; } = 0;
        public string LooseCased { get; set; }
        public decimal? DensityFact { get; set; }
        public string LCLorFCL { get; set; }
        public int? ContainerSizeID { get; set; } = 0;
        public int? NoOfPacks { get; set; } = 0;
        public int? NoOfCrates { get; set; } = 0;
        public int? PortLoad { get; set; } = 0;
        public int? PortDischarge { get; set; } = 0;
        public int? TranshipPort { get; set; } = 0;
        public string HFTransitList { get; set; }
        public string HFTransitWtVolList { get; set; }
        public int? ForwardingBr { get; set; }
		public string StageRemarks { get; set; }

		public List<TranShipment> TranShipmentList { get; set; } = new List<TranShipment>();
        public List<TranShipmentWtVol> TranShipmentWtVolList { get; set; } = new List<TranShipmentWtVol>();
        public List<TranshipInvoice> transhipInvoices { get; set; } = new List<TranshipInvoice>();
        public List<TranshipInvoiceJobs> transhipInvoiceJobs { get; set; } = new List<TranshipInvoiceJobs>();


        public int? FCL_20 { get; set; }
        public int? FCL_40 { get; set; }
        public int? FCLHC_40 { get; set; }

        public int? SSLCarrierId { get; set; }
        public int? SSLAgentId { get; set; }
        public string SSLCarrier { get; set; }
        public string SSLAgent { get; set; }

        public bool THCPrepaid { get; set; }
        public bool THCCollect { get; set; }

        public string HFTransitInvoiceList { get; set; }
        public string HFTransitInvJobList { get; set; }
        public Int64? TransInvMasterID { get; set; }

        public string CSCreatedBY { get; set; }
        public DateTime? CSCreatedDate { get; set; }
        public string CSApprovedBY { get; set; }
        public DateTime? CSApprovedDate { get; set; }

        //RMC Service Evaluation
        public bool ServiceEvaluation { get; set; }
        public decimal? ServiceEvaluationScore { get; set; }
        public string ServiceEvaluationRemarks { get; set; }

        //Storage Dates
        public DateTime? OrgStgStartDate { get; set; }

        public DateTime? OrgStgEndDate { get; set; }

        public DateTime? DestStgStartDate { get; set; }

        public DateTime? DestStgEndDate { get; set; }

        public decimal? RoadKMS { get; set; }


        //Approval Details
        public string ApproveTitle { get; set; }
        public bool IsApprove { get; set; }
        public bool IsCSSenttoApprove { get; set; }
        public int? CSSenttoApproveUser { get; set; }

    }

    public class PackingSOList
    {
        public Int64? SurveyId { get; set; }
        public Int64? SurveyDetailId { get; set; }
        public int? CostHeadID { get; set; }

        [Display(Name = "CostHeadName")]
        public string CostHeadName { get; set; }

        [Display(Name = "Remark")]
        public string Remark { get; set; }

        public string Volume { get; set; }

        public int WtUnitID { get; set; }

        public string WtUnit { get; set; }

        public decimal? ExpectedCost { get; set; }

        public bool Isactive { get; set; }

        public string SOBatchId { get; set; }

        public int RateCompId { get; set; }
        public string RateCompName { get; set; }
        //[Display(Name = "Transit Time From")]
        //public DateTime? TransitTimeFrom { get; set; }

        //[Display(Name = "Transit Time To")]
        ////[GreaterThanEqualTo("TransitTimeFrom", ErrorMessage = "To Time must be greater than From Time")]
        //public DateTime? TransitTimeTo { get; set; }

        ////[Required(ErrorMessage = "Shiping Line is required")]
        //public int? ShipingLineID { get; set; }

        //public string ShipingLineName { get; set; }

        //public List<MoveInstruction> MoveInstructionList { get; set; } = new List<MoveInstruction>();

        //public string HFVMoveInstructionList { get; set; }

    }

    public class PackingCostList
    {

        public int RateCompId { get; set; }
        public string RateCompName { get; set; }

        public int? CostHeadID { get; set; }

        [Display(Name = "CostHeadName")]
        public string CostHeadName { get; set; }

        public int BaseCurrID { get; set; }

        public int RateCurrID { get; set; }

        public int RevRateCurrID { get; set; }

        public int WtUnitID { get; set; }


        public int Per { get; set; }

        public decimal? Rate { get; set; }
        public decimal? RevRate { get; set; }

        public decimal? ConversionRate { get; set; }

        public decimal? RevConversionRate { get; set; }

        public decimal? CostValue { get; set; }

        public decimal? BaseRevenueValue { get; set; }
        public decimal? RevenueValue { get; set; }

        public string WtUnitName { get; set; }

        public string WtVol { get; set; }

        public string BaseCurr { get; set; }

        public string RateCurr { get; set; }
        public string RevRateCurr { get; set; }

        public bool Isactive { get; set; }
        public bool ToBill { get; set; }
        public bool IsSubCost { get; set; }

        public decimal? Balance { get; set; }
    }


    public class MoveCostMaster
    {

        [Required(ErrorMessage = "RMC is required")]
        public int RMCID { get; set; }

        [Display(Name = "RMC")]
        public string RMCName { get; set; }

        [Required(ErrorMessage = "BusinessLine is required")]
        public int BusinessLineID { get; set; }

        [Display(Name = "Business Line")]
        public string BusinessLineName { get; set; }

        [Required(ErrorMessage = "Goods Description is required")]
        public int GoodsDescriptionID { get; set; }

        [Display(Name = "Goods Description")]
        public string GoodsDescriptionName { get; set; }

        [Required(ErrorMessage = "Mode is required")]
        public int ModeID { get; set; }

        [Display(Name = "Mode")]
        public string ModeName { get; set; }

        public int? RateComponentID { get; set; }

        [Display(Name = "RateComponent")]
        public string RateComponentName { get; set; }


        public int? AgentID { get; set; }

        [Display(Name = "Agent")]
        public string AgentName { get; set; }

        [Required(ErrorMessage = "From City is required")]
        public int FromLocationID { get; set; }
        [Display(Name = "From City")]
        public string FromLocationName { get; set; }

        [Required(ErrorMessage = "To City is required")]
        public int ToLocationID { get; set; }
        [Display(Name = "To City")]
        public string ToLocationName { get; set; }


        public int EntryPointID { get; set; } = 0;

        [Display(Name = "Entry Port")]
        public string EntryPointName { get; set; }


        public int ExitPointID { get; set; } = 0;

        [Display(Name = "Exit Port")]
        public string ExitPointName { get; set; }


        public int? RateCurrencyID { get; set; }

        [Display(Name = "Rate Currency")]
        public string RateCurrencyName { get; set; }

        public int? BaseCurrencyRateID { get; set; }

        [Display(Name = "Base Currency")]
        public string BaseCurrencyRateName { get; set; }

        public float? ConversionRate { get; set; }


        [Display(Name = "Transit Time From")]
        public int? TransitTimeFrom { get; set; }

        [Display(Name = "Transit Time To")]
        //[GreaterThanEqualTo("TransitTimeFrom", ErrorMessage = "To Time must be greater than From Time")]
        public int? TransitTimeTo { get; set; }


        [Required(ErrorMessage = "Weight/Volume Unit is required")]
        public int WeightUnitID { get; set; }

        [Display(Name = "Weight Unit")]
        public string WeightUnitName { get; set; }

        [Display(Name = "Weight From")]
        [Required(ErrorMessage = "Weight From is required")]
        [Range(0, float.MaxValue, ErrorMessage = "Please enter a value bigger than {1}")]
        public float WeightUnitFrom { get; set; }

        [Display(Name = "Weight To")]
        [Required(ErrorMessage = "Weight To is required")]
        [Range(0, float.MaxValue, ErrorMessage = "Please enter a value bigger than {1}")]
        [GreaterThanEqualTo("WeightUnitFrom", ErrorMessage = "Weight To must be greater than Weight From")]
        public float WeightUnitTo { get; set; }

        //[Required(ErrorMessage = "Shiping Line is required")]
        public int? ShipingLineID { get; set; }

        public string ShipingLineName { get; set; }

        public List<MoveCostHead> MoveCostHeadList { get; set; } = new List<MoveCostHead>();

        public string HFVMoveCostHeadList { get; set; }
    }

    public class MoveInstruction
    {
        public Int64 MoveInstructionID { get; set; }

        public int? CostHeadID { get; set; }

        public string CostHeadName { get; set; }

        public int RateComponentID { get; set; }

        public string RateComponentName { get; set; }

        public int AgentID { get; set; }

        public string AgentName { get; set; }

        public DateTime? TransitTimeFrom { get; set; }

        public DateTime? TransitTimeTo { get; set; }

        public string Instruction { get; set; }

        public string Remark { get; set; }

    }

    public class MoveRateComponent
    {

        public int MoveID { get; set; }

        public int RateComponentID { get; set; }

        public string RateComponentName { get; set; }

        public int AgentID { get; set; }

        public string AgentName { get; set; }

        public int JobAgentID { get; set; }

        public string JobAgentName { get; set; }

        public string ActJobAgentID { get; set; }

        public string ActJobAgentName { get; set; }

        public int ExitPortID { get; set; } = 0;

        public string ExitPort { get; set; }

        public int EntryPortID { get; set; } = 0;

        public string EntryPort { get; set; }

        public string ActEntryPort { get; set; }
        public string ActExitPort { get; set; }
    }

    public class MoveCostHead
    {
        public int CostHeadID { get; set; }
        public string CostHeadName { get; set; }

        public int RateComponentID { get; set; }
        public string RateComponentName { get; set; }

        public int AgentID { get; set; }
        public string AgentName { get; set; }

        public int RateCurrencyID { get; set; }
        public string RateCurrencyName { get; set; }

        public int BaseCurrencyRateID { get; set; }
        public string BaseCurrencyRateName { get; set; }

        public decimal ConversionRate { get; set; }

        public decimal Amount { get; set; }


        [Required(ErrorMessage = "Transit From Time is required")]
        [Range(0, int.MaxValue, ErrorMessage = "Please enter a value bigger than {1}")]
        public int TransitTimeFrom { get; set; }

        [Required(ErrorMessage = "Transit To Time is required")]
        [Range(0, int.MaxValue, ErrorMessage = "Please enter a value bigger than {1}")]
        [GreaterThan("TransitTimeFrom", ErrorMessage = "To Time must be greater than From Time")]
        public int TransitTimeTo { get; set; }

    }

    public class MoveManageGrid
    {
        public Int64? ratecompanyratewtid { get; set; }

        public Int64? MoveId { get; set; }
        public string JobNo { get; set; }

        public Int64 SurveyId { get; set; }
        public Int64 EnqId { get; set; }
        public string EnqNo { get; set; }
        public Int64 EnqDetailId { get; set; }
        public Int64 EnqSeqID { get; set; }
        public bool IsRMCBuss { get; set; }

        public Int64 BillCollId { get; set; }

        public DateTime? SurveyDate { get; set; }
        public TimeSpan? SurveyDateTime { get; set; }
        public string SurveyConductedByName { get; set; }
        public string CompititorName { get; set; }
        public string ShipperName { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public string BussLineName { get; set; }
        public DateTime? EnqDate { get; set; }
        public string EnqReceivedbyName { get; set; }
        public string AgentName { get; set; }

        public string AccountName { get; set; }

        public DateTime? JobOpenedDate { get; set; }

        public string ServiceLine { get; set; }

        public string Mode { get; set; }

        public float WeightFrom { get; set; }

        public float WeightTo { get; set; }
        public string FromCity { get; set; }
        public string Exitport { get; set; }

        public string EntryPort { get; set; }
        public string ToCity { get; set; }

        public float TotEstimate { get; set; }

        public bool? UseForJob { get; set; }

        public bool? UsedForJob { get; set; }
        public string JobStatus { get; set; }
    }

    public class TranShipment
    {
        public string ScheduleVessel { get; set; }
        public DateTime? ETD { get; set; }
        public DateTime? ETA { get; set; }
        public DateTime? ActDep { get; set; }
        public DateTime? ActArr { get; set; }
        public int? TranshipPortId { get; set; }
        public string TranshipPortName { get; set; }
        public bool Isactive { get; set; }
        public int FromPortId { get; set; }
        public int OrderNo { get; set; }
    }

    public class TranShipmentWtVol
    {
        public DateTime? BLReleaseOn { get; set; }
        public string Bill_No { get; set; }
        public string SealNo { get; set; }
        public string ContainerNo { get; set; }
        public int? ContainerTypeId { get; set; }
        public string ContainerType { get; set; }
        public int? NoOfPacks { get; set; }
        public string AirLine { get; set; }
        public int? AirLineID { get; set; }
        public string Courier { get; set; }
        public int? CourierID { get; set; }
        public int? WtVolUnitId { get; set; }
        public string WtVolUnit { get; set; }
        public decimal? WtVol { get; set; }
        public int? VolUnitId { get; set; }
        public string VolUnit { get; set; }
        public decimal? Vol { get; set; }
        public string LCLorFCL { get; set; }
        public bool Isactive { get; set; }
        public decimal? Length { get; set; }
        public decimal? Breadth { get; set; }
        public decimal? Height { get; set; }

        public Int64 MoveID { get; set; }
        public string JobNo { get; set; }
        public string Shipper { get; set; }

        public Int64 MasterID { get; set; }

        public decimal? GrossWt { get; set; }
        public decimal? ACWTWt { get; set; }

    }

    public class EmailList
    {
        public int TransactionID { get; set; }
        public int ActivityID { get; set; }
        public string ActivityName { get; set; }
        public string EmailFrom { get; set; }
        public string EmailTo { get; set; }
        public string EmailCC { get; set; }
        public string EmailBCC { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public int SentBy { get; set; }
        public DateTime SentOn { get; set; }
    }

    public class JobDocUpload
    {

        [Required(ErrorMessage = "ID is required")]
        public Int64 ID { get; set; }

        public Int64? JobDocTypeId { get; set; }

        [Required(ErrorMessage = "From Type is required")]
        public string DocFromType { get; set; }

        [Required(ErrorMessage = "Document Type is required")]
        public Int32 DocTypeID { get; set; }

        [Required(ErrorMessage = "Document Name is required")]
        public Int32 DocNameID { get; set; }

        public int[] DocNameIDList { get; set; }

        public int[] DocNameIDListSelected { get; set; }

        public string DocNameIDListHidden { get; set; }

        public string DocDescription { get; set; }

        public string Remarks { get; set; }

        public string FileName { get; set; }

        [Required(ErrorMessage = "File is required")]
        public HttpPostedFileBase[] file { get; set; }

        public HttpPostedFileBase[] ExtFile { get; set; }

        public List<JobDocument> docLists { get; set; } = new List<JobDocument>();

        public String DocTypeText { get; set; }
        public String DocNameText { get; set; }

		//GCCInsurance
		public string PolicyNo { get; set; }
	}

    public class JobDocument
    {
        public Int32 FileID { get; set; }

        public Int32 DocTypeID { get; set; }

        public string DocType { get; set; }

        public Int32 DocNameID { get; set; }

        public string DocName { get; set; }

        public String DocDescription { get; set; }

        public String FileName { get; set; }

        public String FilePath { get; set; }

        public String UploadBy { get; set; }

        public int UploadById { get; set; }

        public HttpPostedFileBase[] file { get; set; }

        public DateTime UploadDate { get; set; }

		//GCC Insurance
		public string PolicyNo { get; set; }
        
        ////Agent/Vendor Invoice No. 
        public string InvRefNo { get; set; }
        public string AgentName { get; set; }
        public decimal Amount { get; set; }
    }

    public class FollowUpDetails
    {
        public DateTime? FollowUpDate { get; set; }
        public string FollowUpRemark { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
    }

    public class InsuranceBySD
    {
        public DateTime? InsPackDate { get; set; }
        public int? InsuranceCurrID { get; set; }
        public decimal InsuranceValue { get; set; }
        public decimal PremiumRate { get; set; }
        public decimal IDVCarValue { get; set; }
        public string VehMakeModel { get; set; }
        public bool IsSendForInsurance { get; set; }
    }


    public class ExportSunJob
    {
        public string Layout { get; set; }
        public string AnalysisDimensionID { get; set; }
        public string AnalysisDimension { get; set; }
        public string AnalysisCode { get; set; }
        public string Description { get; set; }
        public string LookupCode { get; set; }
        public string Budgetcheck { get; set; }
        public string Budgetstop { get; set; }
        public string Prohibitposting { get; set; }
        public string Navigationmethod { get; set; }
        public string Combinedbudgetcheck { get; set; }
        public string DAG { get; set; }
        public string Mode { get; set; }
        public string Status { get; set; }
        public string Jobdate { get; set; }
        public string Controller { get; set; }
        public string DestinationCity { get; set; }
        public string Agentrefno { get; set; }
        public string CorporateGroup { get; set; }
        public string OriginCity { get; set; }

    }

    public class JobCancel
    {
        public string CancelRemark { get; set; }
    }

    public class JobVendorEvaluation
    {
        public Int64 MoveID { get; set; }
        public string OrgRemark { get; set; }
        public string DestRemark { get; set; }
        public string OrgActionable { get; set; }
        public string DestActionable { get; set; }
        public string ShowActionable { get; set; }


        public List<int> OrgSelectedQuestions { get; set; } = new List<int>();

        public List<JobVendorEvalQuestion> OrgEvalQuestions { get; set; } = new List<JobVendorEvalQuestion>();

        public List<int> DestSelectedQuestions { get; set; } = new List<int>();

        public List<JobVendorEvalQuestion> DestEvalQuestions { get; set; } = new List<JobVendorEvalQuestion>();

        public bool IsFeedbackEntered { get; set; }
    }

    public class JobVendorEvalQuestion
    {
        public int QuestionID { get; set; }

        [Display(Name = "Question")]
        public string QuestionDetail { get; set; }

        [Display(Name = "Rate Component")]
        public int RateCompID { get; set; }

        public string RateComp { get; set; }

        [Display(Name = "Company")]
        public int CompanyID { get; set; }

        public string Company { get; set; }

        public bool IsRMCBuss { get; set; }

        public int Order { get; set; }

        public int Weightage { get; set; }

        //[Required(ErrorMessage = "Please select yes or no")]
        public bool? Answer { get; set; }

        public bool IsActive { get; set; }

        //[Required(ErrorMessage = "Please select one")]
        public int? AnswerOptionID { get; set; }

        public string AnswerType { get; set; }

        public string AnswerText { get; set; }

        public IEnumerable<SelectListItem> options { get; set; }

    }

    public class JobLabel
    {

        public string JobNo { get; set; }
        public string CLIENT { get; set; }
        public string MoveCordinator { get; set; }
        public string CORPORATE { get; set; }
        public string SHIPPER { get; set; }
        public string ORIGIN { get; set; }
        public string DESTINATION { get; set; }
        public string ORIGIN_Agent { get; set; }
        public string DESTINATION_Agent { get; set; }
        public string Shipper_Address { get; set; }
        public string Shipper_Phone1 { get; set; }
        public string Shipper_Phone2 { get; set; }
        public string Shipper_Fax { get; set; }
        public string Shipper_Email { get; set; }

        public string Client_Address { get; set; }
        public string Client_Phone1 { get; set; }
        public string Client_Phone2 { get; set; }
        public string Client_Fax { get; set; }

        public string Mode { get; set; }
        public string ShipmentType { get; set; }
        public string BusinessLine { get; set; }
        public string Insurance { get; set; }

    }

    public class TranshipInvoice
    {
        public Int64? InvoiceId { get; set; }
        public int InvoiceTypeId { get; set; }
        public string InvoiceType { get; set; }
        public string InvoiceNo { get; set; }
        public DateTime InvoiceDate { get; set; }
        public decimal InvoiceAmt { get; set; }
        public string Remark { get; set; }
        public int CurrID { get; set; }
        public string Currancy { get; set; }
        public Int64 MasterID { get; set; }
        public string InvCredit { get; set; }
        public string InvCreditName { get; set; }
        public Int64? FirstInvID { get; set; }
    }

    public class TranshipInvoiceJobs
    {
        public int? InvoiceJobId { get; set; }
        public Int64 MoveId { get; set; }
        public string JobNo { get; set; }
        public decimal JobAmt { get; set; }
        public string Remark { get; set; }
    }

    public class JobSurveyForMobile
    {
        [Required(ErrorMessage = "Surveyer Name required")]
        public int SurveyerID { get; set; }

        [Required(ErrorMessage = "Survey required")]
        public int SurveyID { get; set; }

        [Required(ErrorMessage = "Enquiry required")]
        public int EnqID { get; set; }

        [Required(ErrorMessage = "Enquiry Detail required")]
        public int EnqDetailID { get; set; }

        public int MoveID { get; set; }

        public int Indx { get; set; }

    }

    public class EmailActiviryHistory
    {
        public int? MailTransID { get; set; }
        public int? ActivityID { get; set; }
        public string ActivityName { get; set; }
        public string SentBy { get; set; }
        public string SentDate { get; set; }
        //public string Remark { get; set; }
    }

    public class ACODetails
    {
        public Int64? MoveID { get; set; }
        public Int64? ACODetailsId { get; set; }
        public int? JobStatusSDId { get; set; }
        public int? BillingStatusId { get; set; }
        public string Remarks { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public bool IsActive { get; set; }
    }

    public class InsuranceDetail
    {
        //Insurance Detail
        public string ContactPerson { get; set; }
        public string ContactNumber { get; set; }
        public string EmailID { get; set; }
        public string FinancePerson { get; set; }
        public decimal? InsuranceValueAmount { get; set; }
        public int? InsuranceValueCurrency { get; set; }
        public int InsuranceBreakdown { get; set; }
        public int BreakdownInsurance { get; set; }
    }

	public class GCCInsuranceDetail
	{
		public int? MoveID { get; set; }
		public string InsuranceID { get; set; }
		public string InsuranceNumber { get; set; }
		public string Remarks { get; set; }
		public HttpPostedFileBase file { get; set; }
		public string CreatedBy { get; set; }
		public DateTime? CreatedDate { get; set; }
	}

	public class ShipperFeedback
    {
        public int? SFTemplateID { get; set; }
        public string TemplateName { get; set; }
        public string Description { get; set; }
        public Int64? ShipperFeedbackID { get; set; }
        public Int64? MoveID { get; set; }
        public string JobNo { get; set; }
        public int?[] SFQuestionIDList { get; set; }
        public string SFQuestionIDListHidden { get; set; }

        [MaxLength(100)]
        [EmailAddress(ErrorMessage = "Please enter correct Email Id.")]
        public string EmailTo { get; set; }
        public string FeedbackLinkSentBy { get; set; }
        public DateTime? FeedbackLinkSentDate { get; set; }
        public bool IsFeedbackSubmitted { get; set; }
        public string FeedbackSubmittedBy { get; set; }
        public DateTime? FeedbackSubmittedDate { get; set; }

        public List<SFQuestion> SFQuestions { get; set; } = new List<SFQuestion>();
        public string AnswerList { get; set; }
    }

    public class SFQuestion
    {
        public int? SFQuestionID { get; set; }
        public string QuestionText { get; set; }
        public string AnswerType { get; set; }
        public int? SrNo { get; set; }
        public bool IsQuestionChecked { get; set; }
        public int? SFAnswerOptionID { get; set; }
        public string AnswerText { get; set; }
        public List<SFAnswerOption> SFAnswerOption { get; set; } = new List<SFAnswerOption>();
    }

    public class SFAnswerOption
    {
        public int? SFAnswerOptionID { get; set; }
        public string AnswerText { get; set; }
        public int? SrNo { get; set; }
        public string ImageUrl { get; set; }
        public bool IsAnswerChecked { get; set; }
        public string Value { get; set; }
        public string Text { get; set; }
    }

    public class AgentInvoice : JobDocUpload
    {
        [Required(ErrorMessage = "Vendor Name required")]
        public int InvDmsAgentId { get; set; }

        public string  VendorCode { get; set; }

        public string VendorAddress { get; set; }

        [Required(ErrorMessage = "Inv. No./Ref. No. required")]
        public string InvoiceNo { get; set; }

        public decimal Amount { get; set; }
    }

	public class GPApproval : MoveManageViewModel
	{
		public string GPApprovalRemark { get; set; }
		public string Type { get; set; }
		public bool IsGPSendtoApproval { get; set; }
		public bool IsGPApprove { get; set; }
		public bool IsGPSendtoSD { get; set; }
		public bool IsApporvalFlag { get; set; }
		public int GPMasterID { get; set; }
	}

	public class GPApprovalDisplayList
	{
		public string Remark { get; set; }
		public string Status { get; set; }
		public string CreatedBy { get; set; }
		public string CreatedDate { get; set; }
		public string GPPercent { get; set; }
		public string RevAmount { get; set; }
        public string Stage { get; set; }
        public string BaseCurr { get; set; }
    }
}