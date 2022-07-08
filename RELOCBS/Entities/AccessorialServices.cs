using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Web;

namespace RELOCBS.Entities
{
    public class AccessorialServicesViewModel
    {
        [Display(Name = "RMC")]
        [Required(ErrorMessage = "Please select RMC")]
        public int RMCID { get; set; }

        [Display(Name = "BusinessLine")]
        public int? BusinessLineID { get; set; }

        [Display(Name = "Goods Description")]
        public int? GoodsDescriptionID { get; set; }
        
        [Display(Name = "Country")]
        [Required(ErrorMessage = "Please select Country")]
        public int? CountryID { get; set; }

        [Display(Name = "City")]
        [Required(ErrorMessage = "Please select City")]
        public int CityID { get; set; }

        [Display(Name = "Agent")]
        [Required(ErrorMessage = "Agent is required.")]
        public int AgentID { get; set; }
        
        [Display(Name = "Cost Head")]
        [Required(ErrorMessage = "Cost Head is required.")]
        public int CostHeadID { get; set; }
        
        [Display(Name = "Agent Currency")]
        [Required(ErrorMessage = "Agent Currency is required.")]
        public int BaseCurrencyRateID { get; set; }

        [Required(ErrorMessage = "Rate per Unit is required.")]
        [Display(Name = "Rate per Unit")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "ServiceIncluded is required.")]
        [Display(Name = "ServiceIncluded")]
        public int ServiceIncluded { get; set; }

        [Required(ErrorMessage = "Conv.Rate to USD is required.")]
        [Display(Name = "Conv.Rate to USD")]
        public decimal ConversionRate { get; set; }
        
        [Display(Name = "Value in USD")]
        public decimal? USDRate { get; set; }

        [Required(ErrorMessage = "Rate Applicable From is required.")]
        [Display(Name = "Rate Applicable From")]
        public DateTime? EffectiveFromDate { get; set; }

        //[Display(Name = "Rate Currency")]
        //[Required(ErrorMessage = "Rate Currency is required.")]
        //public int RateCurrencyID { get; set; } = 1;

        //[Display(Name = "Unit")]
        //[Required(ErrorMessage = "Unit is required.")]
        //public int UnitID { get; set; }

        //public string HFVCostHeadList { get; set; }

        //public List<CostHead> CostHeadList { get; set; } = new List<CostHead>();

    }

    public class AccessServiceAgentList
    {
        [Display(Name = "City")]
        public int ASACityID { get; set; }
        public string City { get; set; }
        public int ASARMCID { get; set; }
        public string RMC { get; set; }


        public int ASAGoodDescriptionID { get; set; }
        public string GoodDescription { get; set; }

        public int MinMax { get; set; }

        public List<RadioMinMax> MinMaxList { get; set; } = new List<RadioMinMax>(){ new RadioMinMax{ID="1" , Type = "Min"},
            new RadioMinMax{ID="2" , Type = "Max"}};
        //public List<AdditionalServicesData> AdditionalServicesData { get; set; }
        public List<CitywiseAdditionalServiceRates> CitywiseAdditionalServiceRates { get; set; }
        //public List<CitywisePermStorageServiceRates> PermStorageServiceRates { get; set; }
        //public List<CitywiseCartageRates> CartageRates { get; set; }
        public DataTable AgentList { get; set; } = new DataTable();
    }

    public class RadioMinMax
    {
        public string ID { get; set; }
        public string Type { get; set; }
    }
    
    public class CitywiseAdditionalServiceRates
    {
        public int AdditionalServiceId { get; set; }

        public string ServiceName { get; set; }

        public Nullable<int> UOMID { get; set; }

        public Nullable<decimal> MinAmt { get; set; }

        public Nullable<decimal> MaxAmt { get; set; }

        public Nullable<decimal> WriterProfit { get; set; }

        public Nullable<decimal> Rate { get; set; }

        public string USDPM { get; set; }

    }


    public class CityCostUpload
    {
        [Display(Name = "RMC")]
        [Required(ErrorMessage = "Please select RMC")]
        public int RMCID { get; set; }

        [Display(Name = "BusinessLine")]
        public int? BusinessLineID { get; set; }

        [Display(Name = "Goods Description")]
        public int? GoodsDescriptionID { get; set; }

        [Display(Name = "Country")]
        [Required(ErrorMessage = "Please select Country")]
        public int? CountryID { get; set; }

        [Display(Name = "City")]
        [Required(ErrorMessage = "Please select City")]
        public int CityID { get; set; }

        [Display(Name = "Agent")]
        [Required(ErrorMessage = "Agent is required.")]
        public int AgentID { get; set; }

        [Display(Name = "Agent Currency")]
        [Required(ErrorMessage = "Agent Currency is required.")]
        public int BaseCurrencyRateID { get; set; }

        [Required(ErrorMessage = "Conv.Rate to USD is required.")]
        [Display(Name = "Conv.Rate to USD")]
        public decimal ConversionRate { get; set; }


        [Required(ErrorMessage = "Rate Applicable From is required.")]
        [Display(Name = "Rate Applicable From")]
        public DateTime? EffectiveFromDate { get; set; }

        [Display(Name = "Upload File")]
        [Required(ErrorMessage = "Please Upload File")]
        //[RegularExpression(@"([a-zA-Z0-9\s_\\.\-:])+(.xls|.xlsx|.xml)$", ErrorMessage = "Only excel files allowed.")]
        public HttpPostedFileBase file { get; set; }

        public List<CityCostHead> CityCostHeadList { get; set; } = new List<CityCostHead>();

    }

    public class CityCostHead
    {
        [Display(Name = "Cost Head")]
        [Required(ErrorMessage = "Cost Head is required.")]
        public int CostHeadID { get; set; }

        public string CostHeadName { get; set; }

        [Required(ErrorMessage = "Rate per Unit is required.")]
        [Display(Name = "Rate per Unit")]
        public decimal Amount { get; set; }

        [Display(Name = "Value in USD")]
        public decimal? USDRate { get; set; }

        [Required(ErrorMessage = "ServiceIncluded is required.")]
        [Display(Name = "ServiceIncluded")]
        public int ServiceIncluded { get; set; }

    }

    public class CityCostRevenue
    {

        public int CityId { get; set; }
        public string City { get; set; }
        public int RMCId { get; set; }
        public string RMC { get; set; }

        public DateTime EffectiveFrom { get; set; }

        public DateTime EffectiveTo { get; set; }

        public string GoodDescription { get; set; }

        public List<CityCostRevenueList> revenueLists { get; set; } = new List<CityCostRevenueList>();

    }


    public class CityCostRevenueList
    {
        public int CostHeadID { get; set; }

        public string CostHead { get; set; }

        public decimal Revenue { get; set; }

        public string Currency { get; set; }

    }

}