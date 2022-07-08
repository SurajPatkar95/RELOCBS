using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RELOCBS.Entities
{
    public class WOSService
    {
        public int ServiceMastID { get; set; }

        public Int64 WOSMoveID { get; set; }

        [Required(ErrorMessage = "Service name is required.")]
        public string ServiceName { get; set; }

        public int BillSeqID { get; set; }
        public bool IsActive { get; set; } = true;
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public WOSSubService WOSSubService { get; set; }
        public List<WOSSubService> WOSSubServiceList { get; set; }
    }

    public class WOSSubService
    {
        public int SubServiceMastID { get; set; }
        public int SrNo { get; set; }
        public int ServiceMastID { get; set; }
        public string ServiceName { get; set; }

        [Required(ErrorMessage = "Subservice name is required.")]
        public string SubServiceName { get; set; }

        public decimal? MastCostAmount { get; set; }
        public decimal? MastRevenueAmount { get; set; }
        public decimal? BillCostAmount { get; set; }
        public decimal? BillRevenueAmount { get; set; }

        public decimal? UnbilledAmount { get; set; }
        public bool ToBill { get; set; }

        public bool IsChecked { get; set; }

        public bool IsActive { get; set; } = true;
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}