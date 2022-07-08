using MvcValidationExtensions.Attribute;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Web;

namespace RELOCBS.Entities
{
    public class Lead
    {
        public Int64 LeadID { get; set; }

        public int RMCID { get; set; }
        public string RMC { get; set; }

        public int FromCityID { get; set; }
        public String FromCity { get; set; }

        public int ToCityID { get; set; }
        public String ToCity { get; set; }

        public PricingDet pricing { get; set; }

        public int CreatedByID { get; set; }
        public String CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }

		public string OrgAgent { get; set; }
		public string DestAgent { get; set; }
		public string OrgPort { get; set; }
		public string DestPort { get; set; }
	}

    public class PricingDet
    {
        public int ModeID { get; set; }
        public int UpdatedBatchID { get; set; }
        public string TransportModeName { get; set; }
        public int FSFRLeadDetailsID { get; set; }
        public int FSFRLeadDetailMastrID { get; set; }
        public int FixCostBatchSeedID { get; set; }
        public string WeightFrom { get; set; }
        //public string CostVal { get; set; }
        public string OrgCost { get; set; }
        public string FrtCost { get; set; }
        public string DestCost { get; set; }
        public string DtDCost { get; set; }
        public string Buff { get; set; }
        public string SFRAmt { get; set; }
        public string SFR { get; set; }
        public string RevSFR { get; set; }
        public string FSFR { get; set; }
        public string FSFRAmt { get; set; }
        public string NetRev { get; set; }
        public string GPVal { get; set; }
        public string GPPercent { get; set; }
		public string TransitTime { get; set; }
	}

    public class LeadViewModel
    {

        public int LeadID { get; set; }

        [Required(ErrorMessage = "RMC is required.")]
        public int RMCID { get; set; }

        [Display(Name = "RMC")]
        public string RMC { get; set; }

        [Required(ErrorMessage = "Origin city is required.")]
        public int FromCityID { get; set; }

        [Display(Name = "Origin city")]
        public String FromCity { get; set; }

        [Required(ErrorMessage = "Destination city is required.")]
        public int ToCityID { get; set; }

        [Display(Name = "Destination city")]
        public String ToCity { get; set; }

        public bool IsRoad { get; set; }

        public IEnumerable<Lead> LeadGrid { get; set; } = new List<Lead>();

        public MoveManageDet manageDet { get; set; } = new MoveManageDet();

        public string HFEnqDet { get; set; }

    }

    public class MoveManageDet
    {
        public Int64 FSFRLeadDetailsID { get; set; }
        public Int64 UpdatedBatchID { get; set; }
		[Required(ErrorMessage = "Revenue Branch is mandatory.")]
		public int RevenueBrId { get; set; }
		public int EnqSourceID { get; set; } = 0;
		public int BussinessLineID { get; set; } = 0;
		public int MoveQuoteID { get; set; } = 0;
		[Required(ErrorMessage = "First name is mandatory.")]
		public string ShipperFName { get; set; }
		[Required(ErrorMessage = "Last name is mandatory.")]
		public string ShipperLName { get; set; }
		[Required(ErrorMessage = "Shipper Category is mandatory.")]
		public int ShipCategoryID { get; set; }
		[Required(ErrorMessage = "Shipper Type is mandatory.")]
		public int ShipTypeID { get; set; } = 0;
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        [EmailAddress(ErrorMessage ="Invalid Shipper Email.")]
        public string Email { get; set; }
		[Required (ErrorMessage ="City is mandatory.")]
        public int AddressCityID { get; set; }  
        public string PIN { get; set; }
        public string Phone1 { get; set; }
        public string Phone2 { get; set; }
        public string Remarks { get; set; }
		[Required(ErrorMessage = "Corporate is mandatory.")]
		public int AccountID { get; set; }
		public int ClientID { get; set; } = 0;
        public string ShipperTitle { get; set; }
        public string EnqDetailsXML { get; set; }
        public DateTime? ShipperDOB { get; set; }
        public string ShipperDesig { get; set; }
        public string ShipperNationality { get; set; }
        public string RateAvailable { get; set; }
        public string AvailableRate { get; set; }
        public string RateCurrencyID { get; set; }

        //Enquiry Det
        public int ServiceLnId { get; set; }
        public string ServiceLn { get; set; }
        public int ShipmentTypeId { get; set; }
        public string ShipmentType { get; set; }
        public int CommodityId { get; set; }
        public string Commodity { get; set; }
        public int HandlingBrId { get; set; }
        public string HandlingBr { get; set; }
		[Required(ErrorMessage = "Sch. Move Date is mandatory.")]
		public DateTime? SchMoveDate { get; set; }
		
		public DateTime? SchSurveyDate { get; set; }

		//Document Details
		[Display(Name = "Work Order No")]
		[Required(ErrorMessage = "Work Order No. is mandatory.")]
		public string WKNo { get; set; }
        [Display(Name = "File No./Ref No.")]
		[Required(ErrorMessage = "File No./Ref No. is mandatory.")]
		public string RMCFileNo { get; set; }
		[Required(ErrorMessage = "Assign Move Cordinator is mandatory.")]
		public int? MoveCoordinatorID { get; set; }
        public string MoveCoordinator { get; set; }
        //[DataType(DataType.Upload)]
        [Display(Name = "OFS Document")]
        public HttpPostedFileBase OFSDocument { get; set; }


		//Weight Details
		public int AirWtUnitId { get; set; } = 0;
		public decimal AirWt { get; set; } = 0;
        public int AirCommodity { get; set; }
        public int SeaWtUnitId { get; set; } = 0;
		public decimal SearWt { get; set; } = 0;
        public int SeaCommodity { get; set; }
        public int SeaShippingLineId { get; set; } = 0;

		public int? ContainerTypeID { get; set; } = 20;

		public int RoadWtUnitId { get; set; } = 0;
		public decimal RoadWt { get; set; } = 0;
        public int? RoadCommodity { get; set; }


    }

    public class GMMSRateUpload
    {
        public int? LeadID { get; set; }

        [Display(Name = "RMC")]
        [Required(ErrorMessage = "Please select RMC")]
        public int RMCID { get; set; }


        [Display(Name = "Rate Component")]
        [Required(ErrorMessage = "Rate Component is required.")]
        public int RateComponentID { get; set; }

		[Display(Name = "Controller")]
		[Required(ErrorMessage = "BusinessLine is required.")]
		public int BusinessLineID { get; set; } = 1;
		public string BusinessLineName { get; set; } = "GMMS";
		[Display(Name = "Goods Description")]
		[Required(ErrorMessage = "Goods Description is required.")]
		public int GoodsDescriptionID { get; set; } = 1;
		public string GoodsDescription { get; set; } = "HOUSEHOLD";
		[Display(Name = "Agent")]
        [Required(ErrorMessage = "Agent is required.")]
        public int AgentID { get; set; }

        [Display(Name = "Base Currency")]
        [Required(ErrorMessage = "Base Currency is required.")]
        public int BaseCurrencyRateID { get; set; }

        [Display(Name = "Rate Currency")]
        [Required(ErrorMessage = "Rate Currency is required.")]
        public int RateCurrencyID { get; set; }

        [Required(ErrorMessage = "Conversion Rate is required.")]
        [Display(Name = "Conv.Rate")]
        public double ConversionRate { get; set; }

        public DataTable dtTable { get; set; } = new DataTable();

        public string CostRateList { get; set; }

        public List<TransportModeList> ServiceModeList { get; set; } = new List<TransportModeList>();


        [Display(Name = "Upload File")]
        [Required(ErrorMessage = "Please Upload File")]
        //[RegularExpression(@"([a-zA-Z0-9\s_\\.\-:])+(.xls|.xlsx|.xml)$", ErrorMessage = "Only excel files allowed.")]
        public HttpPostedFileBase file { get; set; }

        [Display(Name = "Upload Type")]
        [Required(ErrorMessage = "Upload Type is required.")]
        public string CostOrRevenueSelected { get; set; }

        public string ModeSelect { get; set; }

        public DataTable THCdtTable { get; set; }
        public string SpecialTHCList { get; set; }
    }

    public class TransportModeList
    {
        [Display(Name = "Mode")]
        [Required(ErrorMessage = "Mode is required.")]
        public int ModeID { get; set; }

        public string ModeName { get; set; }

        [Display(Name = "Origin City")]
        public int? FromCityID { get; set; }

        [Display(Name = "Destination City")]
        public int? ToCityID { get; set; }

        [Display(Name = "Entry Port")]
        public int? EntryPortID { get; set; }

        [Display(Name = "Exit Port")]
        public int? ExitPortID { get; set; }

        [Display(Name = "Transit Time From")]
        //[Range(typeof(int), "1", "365", ErrorMessage = "From Time must be greater than or equal to 1")]
        public int? TransitTimeFrom { get; set; }

        [Display(Name = "Transit Time To")]
        //[Range(typeof(int), "1", "365", ErrorMessage = "To Time must be greater than or equal to 1")]
        //[GreaterThanEqualTo("TransitTimeFrom", ErrorMessage = "To Time must be greater than From Time")]
        public int? TransitTimeTo { get; set; }
    }


}