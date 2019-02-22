using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Web;

namespace RELOCBS.Entities
{
    public class LaneRate
    {
        public int LaneID { get; set; }

        [Required(ErrorMessage = "Please select RMC")]
        public int RMCID { get; set; }

        [Required(ErrorMessage = "Please select EffectiveFrom Date")]
        public DateTime EffectiveFromDate { get; set; }

        [Required(ErrorMessage = "Please select From Date")]
        public DateTime FromDate { get; set; }

        [Required(ErrorMessage = "Please select To Date")]
        public DateTime Todate { get; set; }

        [Required(ErrorMessage = "Please select Currency")]
        public int CurrencyID { get; set; }

        public DataTable dtTable { get; set; }

    }

    public class RateUploadall
    {

        [Required(ErrorMessage = "Please select Vendor")]
        public int VendorId { get; set; }

        [Required(ErrorMessage = "Please select RMC")]
        public int RMCID { get; set; }

        [Required(ErrorMessage = "Please select RMC")]
        public String RMCName { get; set; }

        [Required(ErrorMessage = "Please select Vendor")]
        public String VendorName { get; set; }

        public int uploadType { get; set; }

    }

    public class RateUpload
    {
        public RateUploadall RateUploadall { get; set; }

        public OriginRate OriginRate { get; set; }

        public FreightRate FreightRate { get; set; }

        public DestinationRate DestinationRate { get; set; }

        public BlanketRate BlanketRate { get; set; }

        public AccesRate AccesRate { get; set; }

        //public PermanentStorageRate PermanentStorageRate { get; set; }

        public string RateTypeFlagValue { set; get; }
        public string ShippmentMode { set; get; }

        // add Commneted due change in requirement
        //public BrookfieldOriginRate BrookfieldOriginRate { get; set; }
        //public BrookfieldFreightRate BrookfieldFreightRate { get; set; }
        //public BrookfieldRate BrookfieldRate { get; set; }

        //public BrookfieldRevenueRate BrookfieldRevenueRate { get; set; }
        //public BrookfieldRevenueFreightRate BrookfieldRevenueFreightRate { get; set; }

        // end add add Commneted due change in requirement
    }

    public class OriginRate
    {
        public int CityID { get; set; }

        [Required(ErrorMessage = "Please select City")]
        public string CityName { get; set; }

        public int LaneID { get; set; }

        public int VendorId { get; set; }

        public int SeaPortId { get; set; }

        public int AirPortId { get; set; }

        [Required(ErrorMessage = "Please select Airport")]
        public string AirPortName { get; set; }

        [Required(ErrorMessage = "Please select Seaport")]
        public string SeaPortName { get; set; }

        public int RMCID { get; set; }

        [Required(ErrorMessage = "Please select From Date")]
        public DateTime FromDate { get; set; }

        [Required(ErrorMessage = "Please select To Date")]
        public DateTime Todate { get; set; }

        [Required(ErrorMessage = "Please select Currency")]
        public int CurrencyID { get; set; }

        public decimal ConversionRateToUSD { get; set; }

        public int? DTPSea { get; set; }

        public int? DTPSeaMax { get; set; }

        public int? DTPAir { get; set; }

        public int? DTPAirMax { get; set; }

        //public int? PTDSea { get; set; }
        //public int? PTDAir { get; set; }

        public DataTable dtTable { get; set; }
    }


    public class DestinationRate
    {
        public int CityID { get; set; }

        [Required(ErrorMessage = "Please select City")]
        public string CityName { get; set; }

        public int LaneID { get; set; }

        [Required(ErrorMessage = "Please select RMC")]
        public int VendorId { get; set; }

        public int SeaPortId { get; set; }

        public int AirPortId { get; set; }

        [Required(ErrorMessage = "Please select Airport")]
        public string AirPortName { get; set; }

        [Required(ErrorMessage = "Please select Seaport")]
        public string SeaPortName { get; set; }

        public int RMCID { get; set; }

        [Required(ErrorMessage = "Please select EffectiveFrom Date")]
        public DateTime EffectiveFromDate { get; set; }

        [Required(ErrorMessage = "Please select From Date")]
        public DateTime FromDate { get; set; }

        [Required(ErrorMessage = "Please select To Date")]
        public DateTime Todate { get; set; }

        public decimal ConversionRateToUSD { get; set; }

        [Required(ErrorMessage = "Please select Currency")]
        public int CurrencyID { get; set; }

        public int? PTDSea { get; set; }

        public int? PTDAir { get; set; }

        public int? PTDSeaMax { get; set; }

        public int? PTDAirMax { get; set; }

        public DataTable dtTable { get; set; }

    }

    public class FreightRate
    {
        public int CityID { get; set; }

        public int LaneID { get; set; }

        [Required(ErrorMessage = "Please select RMC")]
        public int VendorId { get; set; }

        public int OriginSeaPortId { get; set; }

        public int OriginAirPortId { get; set; }

        public int DestSeaPortId { get; set; }

        public int DestAirPortId { get; set; }

        [Required(ErrorMessage = "Please select Origin Airport")]
        public string OAirPortName { get; set; }

        [Required(ErrorMessage = "Please select Origin Seaport")]
        public string OSeaPortName { get; set; }

        [Required(ErrorMessage = "Please select Destination Airport")]
        public string DAirPortName { get; set; }

        [Required(ErrorMessage = "Please select Destination Seaport")]
        public string DSeaPortName { get; set; }

        public DateTime FromDate { get; set; }

        [Required(ErrorMessage = "Please select Currency")]
        public int CurrencyID { get; set; }

        public decimal ConversionRateToUSD { get; set; }

        public int? PTPAir { get; set; }

        public int? PTPAirMax { get; set; }

        public int? PTPSea { get; set; }

        public int? PTPSeaMax { get; set; }

        public DataTable dtTable { get; set; }

    }

    public class BlanketRate
    {
        public int FromCityID { get; set; }

        [Required(ErrorMessage = "Please select From City")]
        public string FromCityName { get; set; }

        public int ToCityID { get; set; }

        [Required(ErrorMessage = "Please select To City")]
        public string ToCityName { get; set; }

        [Required(ErrorMessage = "Please select From Date")]
        public DateTime FromDate { get; set; }

        [Required(ErrorMessage = "Please select To Date")]
        public DateTime Todate { get; set; }

        [Required(ErrorMessage = "Please select Door To Dort Min Days")]
        public int? DTDMin { get; set; }

        [Required(ErrorMessage = "Please select Door To Dort Max Days")]
        public int? DTDMax { get; set; }

        [Required(ErrorMessage = "Please select Currency")]
        public int CurrencyID { get; set; }

        public decimal ConversionRateToUSD { get; set; }

        public DataTable dtTable { get; set; }

    }

    public class AccesRate
    {
        public int CityID { get; set; }
        public int RMCID { get; set; }

        [Required(ErrorMessage = "Please select City")]
        public string CityName { get; set; }

        public int VendorId { get; set; }

        public int SearPortId { get; set; }

        public int AirPortId { get; set; }

        [Required(ErrorMessage = "Please select from Date")]
        public DateTime FromDate { get; set; }

        public DateTime Todate { get; set; }

        [Required(ErrorMessage = "Please select Currency")]
        public int CurrencyID { get; set; }

        public decimal ConversionRateToUSD { get; set; }

        public DataTable dtTable { get; set; }

    }


    /// <summary>
    /// Upload excel/xml Format Model
    /// </summary>
    public class SaveOriginRate
    {
        public String WeightSlab { get; set; }
        public double? OriginRate { get; set; }
        public double? OriginTHC { get; set; }
        public double? LiftVanOriginRate { get; set; }
        public double? LiftVanOriginTHC { get; set; }
        public double? MiscRate { get; set; }
    }

    public class SaveDestinationRate
    {
        public String WeightSlab { get; set; }
        public double? DestinationRate { get; set; }
        public double? DestinationTHC { get; set; }
        public double? DestinationAQIS { get; set; }
        public double? LiftVanDestinationRate { get; set; }
        public double? LiftVanDestinationTHC { get; set; }
        public double? LiftVanDestinationAQIS { get; set; }
        public double? MiscRate { get; set; }
    }

    public class SaveFreightRate
    {
        public String WeightSlab { get; set; }
        public double? FreightRate { get; set; }
        public double? FreightAmount { get; set; }
        public double? MiscRate { get; set; }

    }

    public class SaveBlanketRate
    {
        public int WeightSlabID { get; set; }

        public string WeightSlab { get; set; }

        public decimal? Rates { get; set; }

        public decimal? Amount { get; set; }

        public decimal? MiscAmount { get; set; }

    }


}