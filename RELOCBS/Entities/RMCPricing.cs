using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Web;

namespace RELOCBS.Entities
{
    public class RMCPricing
    {
        public int LeadID { get; set; }

        public int? MoveID { get; set; }

        [Display(Name = "Origin Port")]
        public int OrgPortID { get; set; }

        [Display(Name = "Destination Port")]
        public int DestPortID { get; set; }

        [Display(Name = "Origin Vendor")]
        public string[] OrgVendorID { get; set; }

        [Display(Name = "Destination Vendor")]
        public string[] DestVendorID { get; set; }

        [Display(Name = "Freight Vendor")]
        public List<string> FreightVendorID { get; set; }

        [Display(Name = "RMC")]
        public int RMCID { get; set; }

        [Display(Name = "Weight Slabs")]
        public List<int> WeightSlabs { get; set; }

        [Display(Name = "Transit Time")]
        public int TransitTime { get; set; }

        [Display(Name = "Shipment Mode")]
        public List<int> ShipmentModeID { get; set; }

        [Display(Name = "Services")]
        public string Services { get; set; }

        public string Origin { get; set; }
        public int FromCityId { get; set; }

        public string Destination { get; set; }
        public int ToCityId { get; set; }

        public bool IsRoad { get; set; }

        public int CalculationMethod { get; set; }

        //public LaneData LaneData { get; set; }

        //public RmcFees RmcFees { get; set; }


        public string CombinationNo { get; set; }
        public string SFRGridList { get; set; }

        public DataTable PricingCombinations { get; set; }

        public DataTable AgentCombination { get; set; }

        public string CurrentShipmentModeName { get; set; }

        //public BufferPricingData BufferPricingData { get; set; }

        public List<RmcFees> RMCFees { get; set; }

        public List<PricingBuffer> PricingSeaBuffer { get; set; }= new List<PricingBuffer>();

		public List<PricingBuffer> PricingAirBuffer { get; set; } = new List<PricingBuffer>();


		public List<SFRCaculationList> SFRCalculationList { get; set; }

        public string[] ModeList { get; set; }

        public int? SelectedCombinationID { get; set; }

        //public List<PricingCombination> BlanketRates { get; set; }

        public int? SelectedBlanketRateCombinationID { get; set; }

		public string NormalRev { get; set; }
	}

    public partial class PricingBuffer
    {
        public int ModeId { get; set; }
        public string BufferSlab { get; set; }
        public decimal? BufferCost { get; set; }
    }

    public partial class RmcFees
    {
        public string CostHeadId { get; set; }
        public string CostHeadName { get; set; }
        public decimal? Amount { get; set; }
        public decimal? Percent { get; set; }
        public char BAFlag { get; set; }
    }

        public partial class SFRCaculationList
    {
        public string UniqID { get; set; }
        public string AgentsDetail { get; set; }
        public string WeightFrom { get; set; }
        public string CostVal { get; set; }
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
        public string TransitFrom { get; set; }
        public string TransitTo { get; set; }
        public string GrpRefID { get; set; }
        public string GrpRefId { get; set; }
        /*public string AgentsDetail { get; set; }
        public string WeightFrom { get; set; }
        public string UniqueID { get; set; }*/
        public string OriginPort { get; set; }
        public string DestPort { get; set; }
        public string OriginVendor { get; set; }
        public string DestVendor { get; set; }
        public string TransitTime { get; set; }
        public string ModeID { get; set; }
        
    }

}