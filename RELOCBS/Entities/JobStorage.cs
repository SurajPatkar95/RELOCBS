using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RELOCBS.Entities
{
    public class StorageJobDetail
    {
        public Int64 MoveID { get; set; }

        public string JobNo { get; set; }

        public DateTime JobDate { get; set; }

        public String ServiceLine { get; set; }

        public String QuotationID { get; set; }

        public String ShipperName { get; set; }

        public String ShipperAddress { get; set; }

        public String Controller { get; set; }

        public String Client { get; set; }

        public String Corporate { get; set; }

        public String JobCommodity { get; set; }
    }

    public class JobStorage
    {
        public StorageJobDetail jobDetail { get; set; } = new StorageJobDetail();

        /// <summary>
        /// Storage 
        /// </summary>
        public Int64? StorageID { get; set; }

        [Display(Name = "Commodity")]
        public int StorageCommodityID { get; set; }

        [Display(Name = "Currancy")]
        public int? CurrID { get; set; }

        [Display(Name = "Warehouse")]
		[Required (ErrorMessage = "Warehouse is mandatory.")]
        public int WarehouseID { get; set; }

        [Required]
        [Display(Name = "Stg Entry Date")]
        public DateTime? StorageEntryDate { get; set; }

        [Display(Name = "Bill Start Date")]
        public DateTime? BillStartDate { get; set; }

        [Display(Name = "Stg Exit Date")]
        public DateTime? StorageExitDate { get; set; }

        [Display(Name = "File Close Date")]
        public DateTime? FileCloseDate { get; set; }

        [Display(Name ="Insurance")]
        public bool IsInsured { get; set; }

        [Display(Name = "InsuredBy")]
        public int? InsuredByID { get; set; }

        [Display(Name = "Pack Date")]
        public DateTime? PackDate { get; set; }

		[Display(Name = "Load Date")]
		public DateTime? LoadDate { get; set; }
		
		[Display(Name = "Strg State")]
		[Required(ErrorMessage = "Warehouse State is mandatory.")]
		public int? StrgStateID { get; set; }

        [Display(Name = "Branch SD")]
        public int? SD_BranchID { get; set; }

        [Display(Name = "HO SD")]
        public int? SD_HOID { get; set; }

        [Display(Name = "Doc Rec Date")]
        public DateTime? DocRecDate { get; set; }

        [Display(Name = "Storage Details")]
        public String StorageDetails { get; set; }

        /// <summary>
        /// Storage Detail
        /// </summary>
        public Int64? StorageDetailID { get; set; }

        [Display(Name = "Volume Unit")]
        public Int32? VolumeUnitID { get; set; }

        [Display(Name = "Volume")]
        public decimal? VolumeCFT { get; set; }

        [Display(Name = "Volume Date")]
        public DateTime? VolumeDate { get; set; }

        [Display(Name = "Remark")]
        public string VolumeRemark { get; set; }

        public List<StorageDetails> StorageList { get; set; } = new List<StorageDetails>();


        /// <summary>
        /// Job Rate
        /// </summary>
        /// 
        public Int64? RateStorageDetailID { get; set; }
        public decimal Strg_Inc_percent { get; set; }
        public DateTime? AsOnDate { get; set; }
        public int Months { get; set; }
        public int CostHeadID { get; set; }
        public decimal CFTRate { get; set; }
        public decimal RateperCFT { get; set; }
        public int RateVolumeUnitID { get; set; }
        public decimal RateVolumeCFT { get; set; }
        public decimal? InsuranceValue { get; set; }
        public decimal? InsurancePercent { get; set; }
        public int? InsuranceCycleID { get; set; }

        public string InsuranceCycle { get; set; }

        public int IsRateVolumeBilled { get; set; }

        public DateTime? InsuranceDate { get; set; }

        public DateTime? RateFromDate { get; set; }

        public List<StorageDetails> rateStorageList { get; set; } = new List<StorageDetails>();

        public List<StorageRateDetails> ratesList { get; set; } = new List<StorageRateDetails>();

        public string HFrateList { get; set; }

        public int TabIndex { get; set; }

        public string CreatedBY { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ApprovedBY { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public string BtnApproveLable { get; set; }
        public bool ApprovalStatus { get; set; }
        public bool Approver { get; set; }

        public string BaseCurrName { get; set; }

        [Display(Name = "Warehouse City")]
        [Required(ErrorMessage = "Warehouse City is mandatory.")]
        public int? StrgCityID { get; set; }
    }

    public class JobStorageGrid
    {
        public Int64 MoveID { get; set; }

        public string JobNo { get; set; }

        public DateTime JobDate { get; set; }

        public String ServiceLine { get; set; }

        public string QuotationID { get; set; }

        public String ShipperName { get; set; }

        public String ShipperAddress { get; set; }

        public String Controller { get; set; }

        public String Client { get; set; }

        public String Corporate { get; set; }

        public String JobCommodity { get; set; }

        public Int64 StorageID { get; set; }

        public string Warehouse { get; set; }
        public DateTime? StorageEntryDate { get; set; }


        public DateTime? BillStartDate { get; set; }

        public DateTime? StorageExitDate { get; set; }

        public DateTime? FileCloseDate { get; set; }

        public string InsuredBy { get; set; }
    }

    public class StorageDetails
    {
        public Int64 StorageDetailID { get; set; }

        public Int32? VolumeUnitID { get; set; }

        public String VolumeUnit { get; set; }

        public decimal VolumeCFT { get; set; }
        
        public DateTime? VolumeDate { get; set; }

        public string VolumeRemark { get; set; }

        public decimal InsuranceValue { get; set; }

        public decimal InsurancePercent { get; set; }

        public int InsuranceCycleID { get; set; }

        public string InsuranceCycle { get; set; }

        public DateTime? BillAsOnDate { get; set; } 

        public Int32 IsBilled { get; set; }

        public DateTime? InsuranceDate { get; set; }
    }

    public class InsuranceDetails
    {
        public Int64 StorageDetailID { get; set; }

        public Int32 VolumeUnitID { get; set; }

        public decimal VolumeCFT { get; set; }

        public DateTime? VolumeDate { get; set; }

        public DateTime? BillDate { get; set; }

        public Int32 WarehouseLocationID { get; set; }
        public String WarehouseLocation { get; set; }

        public decimal Insurance_Value { get; set; }

        public decimal Insurance_Percent { get; set; }

        public Int32 InsuranceCycleID { get; set; }

        public string InsuranceRemark { get; set; }

        public Int32 IsBilled { get; set; }
    }

    public class StorageRate
    {
        public Int64 StorageID { get; set; }

        public StorageJobDetail RateJobDetail { get; set; } = new StorageJobDetail();

        /// <summary>
        /// Storage Detail
        /// </summary>
        public Int64? StorageDetailID { get; set; }

        public int VolumeUnitID { get; set; }
        public decimal VolumeCFT { get; set; }
        public DateTime? VolumeDate { get; set; }
        public string VolumeRemark { get; set; }

        public decimal Strg_Inc_percent { get; set; }

        public DateTime? AsOnDate { get; set; }

        public int Months { get; set; }

        public int CostHeadID { get; set; }

        public decimal  CFTRate { get; set; }

        public decimal RateperCFT { get; set; }

        public int RateVolumeUnitID { get; set; }
        public decimal RateVolumeCFT { get; set; }

        public List<StorageDetails> rateStorageList { get; set; } = new List<StorageDetails>();

        public List<StorageRateDetails> ratesList { get; set; } = new List<StorageRateDetails>();

    }

    public class StorageRateDetails
    {

        public Int64? StorageDetailID { get; set; }
        public string VolumeUnit { get; set; }
        public int VolumeUnitID { get; set; }
        public decimal VolumeCFT { get; set; }
        public int CostHeadid { get; set; }
        public string CostHead { get; set; }
        public decimal Rate { get; set; }
        public decimal RatePerUnit { get; set; }
        public int RatePeriodID { get; set; }
        public string RatePeriod { get; set; }
        public Int32 IsBilled { get; set; }

    }

    public class JobStorageApproveDTO
    {
        public Int64 MoveID { get; set; }
        public Int64? StorageID { get; set; }
        public int Index { get; set; } = 0;
        public bool IsApproved { get; set; }
    }

}