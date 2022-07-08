using MvcValidationExtensions.Attribute;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RELOCBS.Entities
{
    public class SurveyData
    {
        [Key]
        public Int64 SurveyId { get; set; }

        public Int64 EnqId { get; set; }

        public string EnqNo { get; set; }

        public Int64 EnqDetailId { get; set; }

        public Int64 EnqshpID { get; set; }

        public DateTime? SurveyDate { get; set; }

        public TimeSpan? SurveyDateTime { get; set; }

        public DateTime PackDate { get; set; }

        public int NoOfDays { get; set; }

        public DateTime LoadDate { get; set; }

        public int SurveyConductedById { get; set; }

        public string SurveyConductedByName { get; set; }

        public int InsuredById { get; set; }

        public string InsuredByName { get; set; }

        public double InsuredAmount { get; set; }

        public int InsCurrencyID { get; set; }

        public DateTime QuotationSubmissionDate { get; set; }

        public DateTime DepartureDate { get; set; }

        public DateTime DeliveryDate { get; set; }

        public bool IsCompition { get; set; }

        public string CompititorName { get; set; }

        public int CompetitorId { get; set; }

        public bool RateAvailable { get; set; }

        public Int64 AvailableRate { get; set; }

        public int RateCurrencyID { get; set; }

        public string SurveyRemark { get; set; }

        public List<ServiceDetail> ServiceDetailList { get; set; }

        public ShipmentDetail shipmentDetail { get; set; }

        public string ShipperName { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public string BussLineName { get; set; }
        public DateTime EnqDate { get; set; }
        public string EnqReceivedbyName { get; set; }
        public string AgentName { get; set; }
        public string AccountName { get; set; }

        public DateTime? EnqReceivedDate { get; set; }
        public string Status { get; set; }
        public string EnqRemark { get; set; }
        public string RevenueBrName { get; set; }
        public string ServiceLine { get; set; }
        public string GoodsDesc { get; set; }
        public string SLCODE { get; set; }

        public string CompletedStatus { get; set; }

        public string CompletedBy { get; set; }

        public DateTime? CompletedDate { get; set; }
        public bool IsRMCBuss { get; set; }
        public bool IsEditVisible { get; set; }
        public string Mode { get; set; }

        public string ApproveStatus { get; set; }
    }

    public class SurveyViewModel
    {
        [Key]
        public Int64? SurveyId { get; set; }

        public Int64? EnqID { get; set; }

        public string EnqNo { get; set; }

        public Int32? EnqDetSequenceID { get; set; }

        public int? EnqStatusID { get; set; }

        public string EnqStatusName { get; set; }

        public string ShipmentStatusName { get; set; }

        //[DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? EnqStatusDate { get; set; }

        public DateTime? ShipmentStatusDate { get; set; }

        //[DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? EnqDate { get; set; }

        public DateTime? EnqRecievedDate { get; set; }

        public int? RevenueBrID { get; set; }

        public string RevenueBr { get; set; }

        public string BussLineName { get; set; }

        public int? AgentID { get; set; }

        public int? AccountID { get; set; }

        public int? AccountMngrID { get; set; }

        public string AccountMngrName { get; set; }

        public string ShipperName { get; set; }

        public int? ShipCategoryID { get; set; }

        public string EnqRemarks { get; set; }

        public EnquiryDetail EnquiryDetail { get; set; } = new EnquiryDetail();

        //[DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        [GreaterThanEqualTo("EnqRecievedDate", ErrorMessage = "Survey Date must be greater than Enq.Recieved Date")]
        [Required(ErrorMessage = "Survey Date required")]
        public DateTime? SurveyDate { get; set; }

        //[DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:HH:mm}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Survey Time required")]
        public TimeSpan? SurveyDateTime { get; set; }

        //[DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Sch. Pack Date required")]
        [GreaterThanEqualTo("SurveyDate", ErrorMessage = "Sch. Pack Date must be greater than Survey Date")]
        public DateTime? PackDate { get; set; }

        [Required(ErrorMessage = "No of days required")]
        public int? NoOfDays { get; set; }

        //[DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        [GreaterThanEqualTo("SurveyDate", ErrorMessage = "Sch. Load Date must be greater than Survey Date")]
        [Required(ErrorMessage = "Sch. Load Date required")]
        public DateTime? LoadDate { get; set; }

        [Required(ErrorMessage = "Survey Conducted By required")]
        public int? SurveyConductedById { get; set; }

        [Required(ErrorMessage = "Insurance By required")]
        public int? InsuredById { get; set; }

        public double? InsuredAmount { get; set; }

        public int? InsCurrencyID { get; set; }

        //[DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        //[Required(ErrorMessage = "Quotation Submission Date required")]
        public DateTime? QuotationSubmissionDate { get; set; }

        //[DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DepartureDate { get; set; }

        //[DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DeliveryDate { get; set; }

        //[Required(ErrorMessage = "Compition By required")]
        public bool? IsCompition { get; set; }

        public string CompititorName { get; set; }

        //[RequiredIf("IsCompition", ErrorMessage = "Must select Competitor if the Competion is Yes")]
        //public int? CompetitorId { get; set; }

        //[RequiredIf("IsCompition", ErrorMessage = "Must select Rate Available if the Competion is Yes")]
        public bool? RateAvailable { get; set; }

        //[RequiredIf("RateAvailable", ErrorMessage = "Must Enter Available Rate if the Rate Available is Yes")]
        public Int64? AvailableRate { get; set; }

        //[RequiredIf("RateAvailable", ErrorMessage = "Must Select RateCurrency if the Rate Available is Yes")]
        public int? RateCurrencyID { get; set; }


        public Int64? MonitoryEntitlement { get; set; }


        //// Commented due to not available required if condition for >=0 
        //[RequiredIf("MonitoryEntitlement >= 0", ErrorMessage = "Monitory Entitlement Currency required")]
        public int? MonitoryEntCurrID { get; set; }

        //[Required(ErrorMessage = "Survey Remarks By required")]
        public string SurveyRemarks { get; set; }

        public List<ServiceDetail> ServiceDetailList { get; set; } = new List<ServiceDetail>();



        public ShipmentDetail shipmentDetail { get; set; } = new ShipmentDetail();

        public string ServiceListHidden { get; set; }

        public string CostListHidden { get; set; }
        //public List<ShipmentItems> shipmentItems { get; set; } = new List<ShipmentItems>();

        public int? MoveId { get; set; }

        public int? IsCompleted { get; set; }

        public string CompletedStatus { get; set; }

        public string CompletedBy { get; set; }

        public DateTime? CompletedDate { get; set; }

        public Int64 CopyEnqDetailID { get; set; }

        public decimal? RoadKMS { get; set; }

        public string StageRemark { get; set; }
        public string SurveyCommItemListHidden { get; set; }
        public SurveyCommItem SurveyCommItem { get; set; } = new SurveyCommItem();
        public List<SurveyCommItem> SurveyCommItemList { get; set; } = new List<SurveyCommItem>();
    }

    public class ServiceDetail
    {
        [Key]
        public Int64 SurveyDetailsID { get; set; }

        public Int64? SurveyID { get; set; }

        public int RateCompID { get; set; }

        public string RateCompName { get; set; }

        public int CostHeadID { get; set; }

        public string CostHeadName { get; set; }

        public string RemarksOnCostHead { get; set; }

    }

    public class ShipmentDetail
    {

        public Int64? SurveyID { get; set; }

        public string SurveyLooseCased { get; set; }

        public decimal? SurveyDensityFact { get; set; }

        public string SurveyLCLorFCL { get; set; }

        //public int Entitlement_Volume_TobePackedID { get; set; }
        //public int Entitlement_Volume_TobePackedValue { get; set; }

        //public int Entitlement_Volume_NetID { get; set; }
        //public int Entitlement_Volume_NetValue { get; set; }

        //public int Entitlement_Volume_GrossID { get; set; }
        //public int Entitlement_Volume_GrossValue { get; set; }

        public int? Survey_VolumeUnitID { get; set; }

        public decimal? Survey_Volume_TobePackedValue { get; set; }

        public decimal? Survey_Volume_NetValue { get; set; }

        public decimal? Survey_Volume_GrossValue { get; set; }


        //public int Entitlement_Weight_TobePackedID { get; set; }
        //public int Entitlement_Weight_TobePackedValue { get; set; }

        //public int Entitlement_Weight_NetID { get; set; }
        //public int Entitlement_Weight_NetValue { get; set; }

        //public int Entitlement_Weight_GrossID { get; set; }
        //public int Entitlement_Weight_GrossValue { get; set; }

        //public int Entitlement_ContainerID { get; set; }
        //public int Entitlement_ContainerValue { get; set; }

        public int? Survey_WeightUnitID { get; set; }
        public decimal? Survey_Weight_ACWTValue { get; set; }

        public decimal? Survey_Weight_NetValue { get; set; }

        public decimal? Survey_Weight_GrossValue { get; set; }

        public int? Survey_ContainerID { get; set; }
        public int? Survey_ContainerValue { get; set; }

        public string OrgAdd { get; set; }

        public string OrgAdd2 { get; set; }

        public int? OrgCityID { get; set; }

        public string OrgPhone { get; set; }

        public string OrgPin { get; set; }

        public string DestAdd { get; set; }

        public string DestAdd2 { get; set; }

        public int? DestCityID { get; set; }

        public string DestPhone { get; set; }

        public string DestPin { get; set; }

        [MaxLength(100)]
        [RegularExpression(@"[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}", ErrorMessage = "Please enter correct email")]
        public string Email { get; set; }

        [MaxLength(100)]
        [RegularExpression(@"[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}", ErrorMessage = "Please enter correct Origin Email")]
        public string OrgEmail { get; set; }

        public string Remarks { get; set; }
    }

    public class ShipmentItems
    {
        public int ItemID { get; set; }

        public string ItemName { get; set; }

        public string ItemType { get; set; }

        public int VolumUnitID { get; set; }

        public int VolumUnitName { get; set; }

        public int VolumeValue { get; set; }

        public int WeightUnitID { get; set; }

        public int WeightUnitName { get; set; }

        public int WeightValue { get; set; }

    }

    public class SurveyCommItem
    {
        public Int64? SurveyItemDeiID { get; set; }
        public Int64? SurveyId { get; set; }
        public string ItemName { get; set; }
        public decimal? Quantity { get; set; }
        public int? QuantiyUnitID { get; set; }
        public string QuantiyUnit { get; set; }
        public int? NoOfPack { get; set; }
        public decimal? ExpectedVol { get; set; }
        public int? VolUnitID { get; set; }
        public string VolUnit { get; set; }
        public DateTime? SchDepDate { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}