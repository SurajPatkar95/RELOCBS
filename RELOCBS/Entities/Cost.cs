using MvcValidationExtensions.Attribute;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Web;

namespace RELOCBS.Entities
{
    public class Cost
    {
        public int RMCID { get; set; }
        public string RMCName { get; set; }

        public int? SurveyID { get; set; }

        public string EnqNo { get; set; }

        public int BusinessLineID { get; set; }
        public string BusinessLineName { get; set; }

        public int GoodsDescriptionID { get; set; }
        public string GoodsDescriptionName { get; set; }

        public int ModeID { get; set; }
        public string ModeName { get; set; }

        public int RateComponentID { get; set; }
        public string RateComponentName { get; set; }

        public int AgentID { get; set; }
        public string AgentName { get; set; }

        public int FromLocationID { get; set; }
        public string FromLocationName { get; set; }
        public int ToLocationID { get; set; }
        public string ToLocationName { get; set; }

        public int RateCurrencyID { get; set; }
        public string RateCurrencyName { get; set; }

        public int BaseCurrencyRateID { get; set; }
        public string BaseCurrencyRateName { get; set; }

        public float ConversionRate { get; set; }


        public int WeightUnitID { get; set; }
        public string WeightUnitName { get; set; }

        public float WeightUnitFrom { get; set; }
        public float WeightUnitTo { get; set; }

        public int TransitTimeFrom { get; set; }
        public int TransitTimeTo { get; set; }

        public float Rate { get; set; }

        public bool ShowConstHeads { get; set; }

        public string RateReceived { get; set; }

        public int RateCompRateWtID { get; set; }

        public int RateCompRateWtBatchID { get; set; }

        public List<CostHeadDetail> CostHeadList { get; set; }

    }

    public class CostHeadDetail
    {
        public bool IsSubCost { get; set; }
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

        public string CostHeadDescription { get; set; }
        public bool Checked { get; set; }

        public Int64? RateCompRateID { get; set; }
    }

    public class CostViewModel
    {
        public int CompanyID { get; set; }

        public int? SurveyID { get; set; }

        public int? EnqDetailID { get; set; }

        public int? EnqID { get; set; }

        public int? EnqDetSequenceID { get; set; }

        public int? SurveyNo { get; set; }
        public string EnqNo { get; set; }

        public string Shipper { get; set; }

        public string HFSubCostList { get; set; }

        [Required(ErrorMessage = "RMC is required")]
        public int RMCID { get; set; }

        [Display(Name = "RMC")]
        public string RMCName { get; set; }

        [Display(Name = "ServiceLine")]
        public int? ServiceLineID { get; set; }

        public string ServiceLineName { get; set; }
        public string Project { get; set; }

        [Required(ErrorMessage = "BusinessLine is required")]
        public int BusinessLineID { get; set; }

        [Display(Name = "Controller")]
        public string BusinessLineName { get; set; }

        [Required(ErrorMessage = "Goods Description is required")]
        public int GoodsDescriptionID { get; set; }

        [Display(Name = "Goods Desc.")]
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

        [Required(ErrorMessage = "Entry Port is required")]
        public int? EntryPointID { get; set; }

        [Display(Name = "Entry Port")]
        public string EntryPointName { get; set; }

        //[Required(ErrorMessage = "Exit Port is required")]
        public int? ExitPointID { get; set; }

        [Display(Name = "Exit Port")]
        public string ExitPointName { get; set; }

        public string Remarks { get; set; }

        public int? RateCurrencyID { get; set; }

        [Display(Name = "Rate Curr.")]
        public string RateCurrencyName { get; set; }

        public int? BaseCurrencyRateID { get; set; }

        [Display(Name = "Base Curr.")]
        public string BaseCurrencyRateName { get; set; }

        public float? ConversionRate { get; set; }

        [Display(Name = "Wt/Vol")]
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

        [Display(Name = "Transit Time From")]
        public int? TransitTimeFrom { get; set; } = 1;

        [Display(Name = "Transit Time To")]
        //[GreaterThanEqualTo("TransitTimeFrom", ErrorMessage = "To Time must be greater than From Time")]
        public int? TransitTimeTo { get; set; } = 1;

        //[Required(ErrorMessage = "Please Enter Rate")]
        public float Rate { get; set; }

        public bool ShowConstHeads { get; set; }

        public List<CostHeadDetail> CostHeadList { get; set; } = new List<CostHeadDetail>();

        public int? RateCompRateWtID { get; set; }

        public int? RateCompRateWtBatchID { get; set; }

        public int RateCompRateBatchId { get; set; }

        public string HFVCostList { get; set; }

        //[Required(ErrorMessage = "Shiping Line is required")]
        public int? ShipingLineID { get; set; }

        public string ShipingLineName { get; set; }

        public List<CostEstimateGrid> SurveyRateGridDt { get; set; } = new List<CostEstimateGrid>();

        public int? DefaultAgentID { get; set; }

        public int? TransitAgentID { get; set; }

        public WHServiceCost WHServiceCost { get; set; } = new WHServiceCost();
        public List<WHServiceCost> WHServiceCostList { get; set; } = new List<WHServiceCost>();
        public string WHServiceCostListHidden { get; set; }
    }

    public class CostEstimateGrid
    {
        public string Colour { get; set; }

        public Int64 RateCompRateBatchId { get; set; }
        public Int64? ratecompanyratewtid { get; set; }
        public Int64? SurveyID { get; set; }
        public int WtUnitID { get; set; }
        public string WtUnit { get; set; }
        public float WeightFrom { get; set; }

        public float WeightTo { get; set; }
        public string FromCity { get; set; }
        public string Exitport { get; set; }
        public string ExitportID { get; set; }

        public string Remarks { get; set; }

        public string EntryPort { get; set; }
        public string EntryPortID { get; set; }
        public string ToCity { get; set; }

        public float TotEstimate { get; set; }


    }

    public class WHServiceCost
    {
        public Int64? Ser_CostHeadRateID { get; set; }
        public Int64? SurveyID { get; set; }
        public Int64? RateCompRateID { get; set; }
        public Int64? RateCompRateBatchID { get; set; }
        public Int64? RateCompID { get; set; }
        public string RateComp { get; set; }
        public int? EmpTypeID { get; set; }
        public string EmpType { get; set; }
        public int? BaseCurrID { get; set; }
        public string BaseCurr { get; set; }
        public decimal? BaseCurrConversRate { get; set; }
        public int? RateCurrID { get; set; }
        public string RateCurr { get; set; }
        public decimal? RateCurrValue { get; set; }
        public decimal? WorkHrs { get; set; }

        public bool? IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}