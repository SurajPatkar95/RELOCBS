using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace RELOCBS.Entities
{
    public class GeneralPrice
    {
        public int RMCID { get; set; }
        public string RMCName { get; set; }

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

        public  List<CostHead>  CostHeadList { get; set; }
        
    }

    public class CostHead
    {
        public int CostHeadID { get; set; }

        public string CostHeadName { get; set; }

        public float Amount { get; set; }
    }

    public class GeneralPriceViewModel
    {
        public int CompanyID { get; set; }

        [Required(ErrorMessage = "Please Select RMC")]
        public int RMCID { get; set; }

        [Display(Name = "RMC")]
        public string RMCName { get; set; }

        [Required(ErrorMessage = "Please Select BusinessLine")]
        public int BusinessLineID { get; set; }

        [Display(Name = "Business Line")]
        public string BusinessLineName { get; set; }

        [Required(ErrorMessage = "Please Select Goods Description")]
        public int GoodsDescriptionID { get; set; }

        [Display(Name = "Goods Description")]
        public string GoodsDescriptionName { get; set; }

        [Required(ErrorMessage = "Please Select Mode")]
        public int ModeID { get; set; }

        [Display(Name = "Mode")]
        public string ModeName { get; set; }

        [Required(ErrorMessage = "Please Select Rate Component")]
        public int RateComponentID { get; set; }

        [Display(Name = "RateComponent")]
        public string RateComponentName { get; set; }

        [Required(ErrorMessage = "Please Select Agent")]
        public int AgentID { get; set; }

        [Display(Name = "Agent")]
        public string AgentName { get; set; }


        public int FromLocationID { get; set; }

        
        public string FromLocationName { get; set; }

        public int ToLocationID { get; set; }

        public string ToLocationName { get; set; }


        public int RateCurrencyID { get; set; }

        [Display(Name = "Rate Currency")]
        public string RateCurrencyName { get; set; }

        public int BaseCurrencyRateID { get; set; }

        [Display(Name = "Base Currency")]
        public string BaseCurrencyRateName { get; set; }

        public float ConversionRate { get; set; }

        [Required(ErrorMessage = "Please Select Weight Unit")]
        public int WeightUnitID { get; set; }

        [Display(Name = "Weight Unit")]
        public string WeightUnitName { get; set; }

        [Display(Name = "Weight From")]
        public float WeightUnitFrom { get; set; }

        [Display(Name = "Weight To")]
        public float WeightUnitTo { get; set; }

        [Display(Name = "Transit Time From")]
        public int TransitTimeFrom { get; set; }

        [Display(Name = "Transit Time To")]
        public int TransitTimeTo { get; set; }

        [Required(ErrorMessage = "Please Enter Rate")]
        public float Rate { get; set; }

        public bool ShowConstHeads { get; set; }

        public List<CostHead> CostHeadList { get; set; }

        public int RateCompRateWtID { get; set; }

        public int RateCompRateWtBatchID { get; set; }

    }

    public class RateComponentLabelViewModel
    {

        public string FromLocationLable { get; set; }
        public string ToLocationLable { get; set; }

        public  IEnumerable<SelectListItem> FromLocationDropDown { get; set; }
        public IEnumerable<SelectListItem> ToLocationDropDown { get; set; }
    }

}