using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RELOCBS.Entities
{
    public class CostHeadCommisionableForRMC
    {
        public int RMCCommiCostID { get; set; } = -1;
        public int RMCID { get; set; }
        public int CostHeadID { get; set; }
        public DateTime? EffectiveFrom { get; set; } = System.DateTime.Now;
        public DateTime? EffectiveTo { get; set; }
        public bool? Isactive { get; set; }
        public string RMCName { get; set; }
        public string CostHeadName { get; set; }

    }
}