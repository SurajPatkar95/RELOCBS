using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RELOCBS.Entities
{
    public class WarehouseJob
    {
        public Int64? JobID { get; set; }
        public string JobNo { get; set; }
        [Required]
        public DateTime? JobOpenDate { get; set; }

        public bool IsRMCBuss { get; set; }

        public int RateComponentID { get; set; } = 1;
        public string RateComponentName { get; set; }

        public int CompanyID { get; set; }
        public string Company { get; set; }

        [Required]
        public int HandlingBranchID { get; set; }
        public string HandlingBranch { get; set; }

        [Required]
        public int RevenueBranchID { get; set; }
        public string RevenueBranch { get; set; }
        [Required]
        public int BusinessLineID { get; set; }
        public string BusinessLine { get; set; }

        public string JobCreatedBy { get; set; }
        public DateTime JobCreatedDate { get; set; }

        public string JobStatus { get; set; }

        public List<WHJob_InstructionSheet> WHJob_Instructions { get; set; } = new List<WHJob_InstructionSheet>();

        public int JobTypeId { get; set; }
        public string JobType { get; set; }

    }


    public class WHJob_InstructionSheet
    {
        public Int64 JobID { get; set; }
        public int JobTypeId { get; set; }
        public Int64? InstID { get; set; }
        public DateTime InstDate { get; set; }
        public string JobNo { get; set; }
        public DateTime JobOpenDate { get; set; }
        public string RevenueBranch { get; set; }
        public string HandlingBranch { get; set; }
        public string BusinessLine { get; set; }
        public string InsType { get; set; }

        public int RateComponentID { get; set; } = 1;
        public string RateComponentName { get; set; }

        public int BranchID { get; set; }
        public int WarehouseID { get; set; }

        public string WareHouseName { get; set; }
        public string BranchName { get; set; }

        public string SpecialInstructions { get; set; }
        public string Remarks { get; set; }

        public int? WeightUnitID { get; set; }
        public string WeightUnit { get; set; }

        public float? WeightUnitFrom { get; set; }
        public float? WeightUnitTo { get; set; }

        public Int32? StatusID { get; set; }
        public string Status { get; set; }
        public Int32? IsSentToWarehouse { get; set; }

        public string ContactPersonFName { get; set; }
        public string ContactLastFName { get; set; }

        public string Add1 { get; set; }
        public string Add2 { get; set; }
        public int? CityID { get; set; }
        public string City { get; set; }
        public int? Pincode { get; set; }

        [MaxLength(10)]
        public string Mobile { get; set; }
        [MaxLength(20)]
        public string Phone { get; set; }
        [MaxLength(100)]
        [RegularExpression(@"[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}", ErrorMessage = "Please enter correct email")]
        public string Email { get; set; }

        public List<Inst_Activities> activities { get; set; } = new List<Inst_Activities>();
        public string HVactivityList { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string Wt_Vol { get; set; }
        public string submit { get; set; }

    }
}