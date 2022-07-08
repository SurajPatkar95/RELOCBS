using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RELOCBS.Entities
{
    public class HistoryRate
    {
        public string AgentName { get; set; }
        public DateTime? EffectiveFrom { get; set; }
        public DateTime? EffectiveTo { get; set; }
        public string CityName_1 { get; set; }
        public string PortName_1 { get; set; }

        public string CityName_2 { get; set; }
        public string PortName_2 { get; set; }

        public float? WeightFrom { get; set; }
        public float? WeightTo { get; set; }
        public string RateItemVal1 { get; set; }
        public string RateItemVal2 { get; set; }
        public string RateItemVal3 { get; set; }
        public string RateItemVal4 { get; set; }
        public string RateItemVal5 { get; set; }
        public string RateItemVal6 { get; set; }
        public Boolean? Isactive { get; set; }
        public Int64? OrgRMCAgentEffectDateID { get; set; }

        public string ModeName { get; set; }

        public string RateCurr { get; set; }

        public string ConvRate { get; set; }

        public List<Rate> rates { get; set; } = new List<Rate>();
    }

    public class Rate
    {
        public Int64 OrgRMCAgentEffectDateID { get; set; }
        public string AgentName { get; set; }
        public string EffectiveFrom { get; set; }
        public string EffectiveTo { get; set; }
        public string CityName { get; set; }
        public string PortName { get; set; }
        public string WeightFrom { get; set; }
        public string WeightTo { get; set; }
        public string RateItemVal1 { get; set; }
        public string RateItemVal2 { get; set; }
        public string RateItemVal3 { get; set; }
        public string RateItemVal4 { get; set; }
        public string RateItemVal5 { get; set; }
        public string RateItemVal6 { get; set; }
        public string RateItemVal7 { get; set; }
        public string RateItemVal8 { get; set; }
        public string OrgPrice { get; set; }
        public string Isactive { get; set; }
        
    }


    public class HistorySplTHC
    {
        public Int64 MastTransId { get; set; }
        public string RMCName { get; set; }
        public string AgentName { get; set; }
        public string DestCity { get; set; }
        public string OrgContinentName { get; set; }
        public bool? Isactive { get; set; }
        public DateTime? EffectiveFrom { get; set; }
        public DateTime? EffectiveTo { get; set; }
        public List<SubHistorySplTHC> SubTHC { get; set; }
    }

    public class SubHistorySplTHC
    {
        public Int64 MastTransId { get; set; }
        public Int64 TransId { get; set; }
        public string Mode { get; set; }
        public int SlabValue { get; set; }
        public decimal THCValue { get; set; }
        public bool? Isactive { get; set; }
        public DateTime? EffectiveFrom { get; set; }
        public DateTime? EffectiveTo { get; set; }
    }

}