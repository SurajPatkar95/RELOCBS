using MvcValidationExtensions.Attribute;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RELOCBS.Entities
{
    public class Quoting
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


    public class QuotingCostHeadDetail
    {
		public int OrderSeq { get; set; }
		public int RateComponentRateID { get; set; }

        public int CostHeadID { get; set; }
        public string CostHeadName { get; set; }
		public bool IsSubCost { get; set; }
		public int RateComponentID { get; set; }
        public string RateComponentName { get; set; }

        public int AgentID { get; set; }
        public string AgentName { get; set; }

        public int RateCurrencyID { get; set; }
        public string RateCurrencyName { get; set; }

        public int BaseCurrencyRateID { get; set; }
        public string BaseCurrencyRateName { get; set; }

        public float ConversionRate { get; set; }

        public decimal Amount { get; set; }

        public decimal BaseAmount { get; set; }

        public decimal QuotePercent { get; set; }

        public decimal QuotePercentAmount { get; set; }

        public decimal TotalAmount { get; set; }

        public int TransitTimeFrom { get; set; }

        public int TransitTimeTo { get; set; }

        public string CostHeadDescription { get; set; }

    }

    public class QuotingViewModel
    {
        public int CompanyID { get; set; }
        public int RateCompRateBatchID { get; set; }
        public int? SurveyID { get; set; }
		
		public int? EnqDetailID { get; set; }

        public int? EnqID { get; set; }

        public int? SurveyNo { get; set; }
        public string EnqNo { get; set; }

        public int? EnqDetSequenceID { get; set; }

        [Required(ErrorMessage = "RMC is required")]
        public int RMCID { get; set; }

        [Display(Name = "RMC")]
        public string RMCName { get; set; }

        [Required(ErrorMessage = "BusinessLine is required")]
        public int BusinessLineID { get; set; }

        [Display(Name = "Business Line")]
        public string BusinessLineName { get; set; }

        [Required(ErrorMessage = "ServiceLine is required")]
        public int ServiceLineID { get; set; }

        [Display(Name = "ServiceLine")]
        public string ServiceLineName { get; set; }


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

        [Required(ErrorMessage = "Entry Port is required")]
        public int EntryPointID { get; set; }

        [Display(Name = "Entry Port")]

        public string EntryPointName { get; set; }

        [Required(ErrorMessage = "Exit Port is required")]
        public int ExitPointID { get; set; }

        [Display(Name = "Exit Port")]

        public string ExitPointName { get; set; }

        public int RateCurrencyID { get; set; }

        [Display(Name = "Rate Currency")]
        public string RateCurrencyName { get; set; }

        public int BaseCurrencyRateID { get; set; }

        [Display(Name = "Base Currency")]
        public string BaseCurrencyRateName { get; set; }

        public float ConversionRate { get; set; }

        //[Required(ErrorMessage = "Wt/Vol Unit is required")]
        public int WeightUnitID { get; set; }

        [Display(Name = "Wt/Vol Unit")]
        public string WeightUnitName { get; set; }

        [Display(Name = "Wt/Vol")]
        [Required(ErrorMessage = "Wt/Vol  is required")]
        //[Range(0, float.MaxValue, ErrorMessage = "Please enter a value bigger than {1}")]
        public float WeightUnitFrom { get; set; }

        [Display(Name = "Weight To")]
        //[Required(ErrorMessage = "Weight To is required")]
        //[Range(0, float.MaxValue, ErrorMessage = "Please enter a value bigger than {1}")]
        //[GreaterThanEqualTo("WeightUnitFrom", ErrorMessage = "Weight To must be greater than Weight From")]
        public float WeightUnitTo { get; set; }

        //[Required(ErrorMessage = "Please Enter Rate")]
        public float Rate { get; set; }
        
        public List<QuotingCostHeadDetail> CostHeadList { get; set; } = new List<QuotingCostHeadDetail>();

        public int? RateCompRateWtID { get; set; }

        public int? RateCompRateWtBatchID { get; set; }

        public string HFVQuotingList { get; set; }

        //[Required(ErrorMessage = "Shiping Line is required")]
        public int? ShipingLineID { get; set; }

        public string ShipingLineName { get; set; }

        public List<QuotingGrid> SurveyRateGridDt { get; set; } = new List<QuotingGrid>();

        public string QuoteNo { get; set; }

        public string QuoteApprove { get; set; }
		public bool QuoteSentApprove { get; set; }
		public int? QuoteSenttoApproveUser { get; set; }

		public string EstimatedBy { get; set; }
		public string EstimatedDate { get; set; }
		public string ApprovedBy { get; set; }
		public string ApprovedDate { get; set; }
	}

    public class QuotingGrid
    {
        public string Colour { get; set; }

        public Int64? ratecompanyratewtid { get; set; }
        public Int64 ratecompratebatchid { get; set; }
        public Int64? SurveyID { get; set; }
        public Int64? EnqDetailID { get; set; }

        public float WeightFrom { get; set; }

        public float WeightTo { get; set; }
        public string FromCity { get; set; }
        public string Exitport { get; set; }

        public string EntryPort { get; set; }
        public string ToCity { get; set; }

        public float TotEstimate { get; set; }
        public float TotQuote { get; set; }
        public bool? UseForJob { get; set; }

        public bool? UsedForJob { get; set; }

        public string QuoteNo { get; set; }

    }

    public class QuotePrint
    {
        public int? SurveyID { get; set; }
        public int RateCompRateWtID { get; set; }
        public int RateCompRateWtBatchID { get; set; }

        public bool IsLumsum { get; set; }

        [Required(ErrorMessage = "Quote To is required.")]
        [Display(Name = "Quote To")]
        public string QuoteTo { get; set; }

        [Display(Name = "Account")]
        public int AccountID { get; set; }

        [Display(Name = "Client")]
        public int ClientID { get; set; }
        
        public string Shipper_Title { get; set; }
        
        [Display(Name = "Shipper")]
        public string ShipperFName { get; set; }
        
        public string ShipperLName { get; set; }

        [StringLength(1000)]
        [Required(ErrorMessage = "Quote Intro is required.")]
        [Display(Name = "Quote Intro")]
        public string QuoteIntro { get; set; }

		[Display(Name = "Subject")]
		public string Subject { get; set; }

		[Display(Name = "Base Currency")]
		public int Estimated_BaseCurr { get; set; }

		[Display(Name = "Quote Currency")]
		public int Quoted_Curr { get; set; }

		[Display(Name = "Exchange Rate")]
		public double QuotedExchange_rate { get; set; }

		[StringLength(250)]
        [Display(Name = "Remark")]
        public string Remarks { get; set; }


        [Required(ErrorMessage = "Attention is required.")]
        public string Attention { get; set; }

        [StringLength(250)]
        //[Required(ErrorMessage = "Attention is required.")]
        public string AttentionName { get; set; }

        [StringLength(150)]
        [Display(Name = "Qt Address1")]
        public string Address1 { get; set; }

        [StringLength(150)]
        [Display(Name = "Qt Address2")]
        public string Address2 { get; set; }

        [StringLength(150)]
        [Display(Name = "Qt Address3")]
        public string Address3 { get; set; }

        [Display(Name  = "Qt City")]
        public int? City { get; set; }
        
        [RegularExpression(@"^\d+$", ErrorMessage = "Invalid Zip")]
        //[RegularExpression(@"^\d{5}(-\d{4})?$", ErrorMessage = "Invalid Zip")]
        [StringLength(10, MinimumLength = 5)]
        [Display(Name = "Qt Zip")]
        public string Zip { get; set; }

        [Display(Name = "Qt Send By")]
        public int  SentBy { get; set; }

        public string Designation { get; set; }

        [StringLength(250)]
        [Display(Name = "Insurance")]
        public string Insurance { get; set; }

        public string AcctAddrs1 { get; set; }

        public string AcctAddrs2 { get; set; }

        public string AcctAddrs3 { get; set; }

        public int AcctCityID { get; set; }

        public string AcctZip { get; set; }

        public string ClientAddrs1 { get; set; }

        public string ClientAddrs2 { get; set; }

        public string ClientAddrs3 { get; set; }

        public int ClientCityID { get; set; }

        public string ClientZip { get; set; }

        public string ShipperAddrs1 { get; set; }

        public string ShipperAddrs2 { get; set; }
        
        public int ShipperCityID { get; set; }

        public string ShipperZip { get; set; }


        public List<CostHeadDetail> CostHeadList { get; set; } = new List<CostHeadDetail>();
        public List<QuoteTerm> QuoteTermsList { get; set; } = new List<QuoteTerm>();
        public List<QuoteInclusionExclusion> InclusionList { get; set; } = new List<QuoteInclusionExclusion>();
        public List<QuoteInclusionExclusion> ExclusionList { get; set; } = new List<QuoteInclusionExclusion>();

        public string QuoteNo { get; set; }

        public string QuoteApprove { get; set; }

        public Int64? QuoteSendToID { get; set; }

    }

    public class QuoteTerm
    {
        public int TermID { get; set; }
        public string TermName { get; set; }
        public string TermDescription { get; set; }
        public string saved { get; set; }
        public string Type { get; set; }
        public bool Checked { get; set; }

    }

    public class QuoteInclusionExclusion
    {
        public int CostHeadID { get; set; }
        public string CostHeadName { get; set; }
        public string CostHeadDescription { get; set; }
        public string saved { get; set; }
        public string Type { get; set; }
        public bool Checked { get; set; }
    }

    public class QouteHtmlPrint
    {
        public int SurveyID { get; set; }
        public int RateCompRateWtID { get; set; }
        public string QouteTo { get; set; }
        public string QtIntroduction { get; set; }
        public string QtTitle { get; set; }
        public string FName { get; set; }
        public string LName { get; set; }
        public string QtAttentionTitle { get; set; }
        public string QtAttention { get; set; }
        public string Insurance { get; set; }
        public string Remarks { get; set; }
        public bool Isactive { get; set; }
        public string QtAddress1 { get; set; }
        public string QtAddress2 { get; set; }
        public string QtAddress3 { get; set; }
        public string Cityid { get; set; }
        public string ZIp_Pin { get; set; }
        public string QtCityName { get; set; }
        public string SentBy { get; set; }
        public string SentByDesign { get; set; }
        public string QtSubject { get; set; }
        public int IsEmail { get; set; }
        public string Line1 { get; set; }
        public string Line2 { get; set; }
        public string Line3 { get; set; }
        public string Annotation { get; set; }
        public string QuoteNo { get; set; }

        public List<QouteCostHeadPrint> QouteCostHeads { get; set; } = new List<QouteCostHeadPrint>();
        public List<QouteTermsPrint> qouteTerms { get; set; } = new List<QouteTermsPrint>();
        public QouteInclusiveExclusivePrint inclusive { get; set; } = new QouteInclusiveExclusivePrint();
        public QouteInclusiveExclusivePrint exclusive { get; set; } = new QouteInclusiveExclusivePrint();
    }

    public class QouteCostHeadPrint
    {

        public int CostHeadID { get; set; }
        public string CostHeadName { get; set; }
        public string InfoForDisplay { get; set; }
        public string IsSaved { get; set; }
        public string QuoteCurrValue { get; set; }
        public string ConvCurrValue { get; set; }
    }

    public class QouteTermsPrint
    {

        public string TermsInclusiveExclusive { get; set; }
        public string MsgToDisplay { get; set; }
        public string IsSaved { get; set; }
        public string Amount { get; set; }
        public string MasterToDisplay { get; set; }    
    }

    public class QouteInclusiveExclusivePrint
    {
        public string TermsInclusiveExclusive { get; set; }
        public string MsgToDisplay { get; set; }
        public string IsSaved { get; set; }
        public string MasterToDisplay { get; set; }
            
    }

}