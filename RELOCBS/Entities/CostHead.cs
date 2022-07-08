using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RELOCBS.Entities
{
    public class CostHeadMaster
    {
        public int CostHeadID { get; set; }
        public string CostHeadName { get; set; }
        public bool IsActive { get; set; }
        public bool HasSubCostHead { get; set; }
        public string SSCCode { get; set; }
        public bool IsGSTApplicable { get; set; }
        public string FinanceCode { get; set; }
        public string ItemCode { get; set; }
        public string InvDescription { get; set; }
    }
}