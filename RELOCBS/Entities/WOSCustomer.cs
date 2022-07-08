using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RELOCBS.Entities
{
    public class WOSCustomer
    {
        public int WOSCustomerID { get; set; }
        public Int64? WOSMoveID { get; set; }

        //[Required(AllowEmptyStrings = false, ErrorMessage = "Please select business line.")]
        public bool? IsRMC { get; set; } = true;

        //[Required(AllowEmptyStrings = false, ErrorMessage = "Please select client.")]
        public Int64? ClientID { get; set; }
        public string Client { get; set; }

        //[Required(AllowEmptyStrings = false, ErrorMessage = "Please select account.")]
        public Int64? AccountID { get; set; }
        public string Account { get; set; }
        public int? CompanyID { get; set; }

        //public string CustomerName { get; set; }

        public bool CheckAllCostList { get; set; }
        public Int64 CustServMapMasterID { get; set; }

        public Int64 JobCostMasterID { get; set; }

        public int? OriginCountryID { get; set; }
        public string OriginCountry { get; set; }

        //[Required(AllowEmptyStrings = false, ErrorMessage = "Please select destination country.")]
        public int? DestinationCountryID { get; set; }
        public string DestinationCountry { get; set; }

        //[Required(AllowEmptyStrings = false, ErrorMessage = "Please select cost currency.")]
        public int? CostCurrencyID { get; set; }
        public string CostCurrency { get; set; }

        //[Required(AllowEmptyStrings = false, ErrorMessage = "Please select revenue currency.")]
        public int? RevenueCurrencyID { get; set; }
        public string RevenueCurrency { get; set; }

        public decimal RevToCostCurrConverRate { get; set; }
        public decimal CostToRevCurrConverRate { get; set; }

        public decimal? AuditFee { get; set; }
        public decimal? AdminFee { get; set; }

        [Range(minimum: 0, maximum: 100, ErrorMessage = "Please enter valid percentage.")]
        public decimal? BillCommissionPercent { get; set; }

        //[Required(AllowEmptyStrings = false, ErrorMessage = "Please enter effective from date.")]
        public DateTime? EffectiveFrom { get; set; }
        public DateTime? EffectiveTo { get; set; }

        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public WOSService WOSService { get; set; } = new WOSService();
        public WOSSubService WOSSubService { get; set; } = new WOSSubService();
        public List<WOSService> WOSServiceList { get; set; } = new List<WOSService>();
        public List<WOSSubService> WOSSubServiceList { get; set; } = new List<WOSSubService>();
    }
}