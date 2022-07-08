using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RELOCBS.Entities
{
    public class PJR_DJR
    {
        public Int64? PJR_DJR_ID { get; set; }
		public Int64 MoveID { get; set; }
        public Int32? RateComponentID { get; set; }
        public string RateComponentName { get; set; }
        public string ServiceLine { get; set; }
        
        public string JobNo { get; set; }
        public DateTime Job_Date { get; set; }
        public string CorprateName { get; set; }

        public string ReportType { get; set; }

        [Required]
        public float? Volume { get; set; }

        [Required]
        public  int? NoOfPkgs { get; set; }

        [Required]
        public DateTime? PackStartDate { get; set; }

        [Required]
        public int? NoOfDays { get; set; }

        public Int32 IsCompleted { get; set; }

        public string Status { get; set; }

        //[Required (ErrorMessage ="Pack Completion Date")]
        public DateTime? PackCompletionDate { get; set; }

        public string Shipper { get; set; }

        public string ShipperAddress { get; set; }

        public List<OutLabourCost> outLabourCosts { get; set; } = new List<OutLabourCost>();

        public List<JobVehicleCost> Vehicles { get; set; } = new List<JobVehicleCost>();
        
        public List<InHouseLabour> inHouseLaboursCost { get; set; } = new List<InHouseLabour>();

        public List<MaterialCost> materialCosts { get; set; } = new List<MaterialCost>();

        public List<JobService> services { get; set; } = new List<JobService>();

        public JobDocUpload docUpload { get; set; } = new JobDocUpload();

        public DateTime CreatedDate { get; set; }

		public int CreatedBy { get; set; }

        public Int16 JobType { get; set; }
        public string HandlingBranch { get; set; }
        public string RevenueBranch { get; set; }
        public string BusinessLine { get; set; }

        public string Remark { get; set; }


        public Int64? DIPDFId { get; set; }
    }

    public class InHouseLabour
    {
        public Int64 C_ID { get; set; }
        public Int64? CC_ID { get; set; }
        
        public int EmpID { get; set; }

        public string EmpName { get; set; }

        public float Rate { get; set; }

        public float Cost { get; set; }

        public int NoOfDays { get; set; }

        public float  OT_Rate   { get; set; }

        public int OT_hours { get; set; }

        public float OT_Cost { get; set; }

        public float Total { get; set; }

        public DateTime? Fromdate { get; set; }

        public DateTime? Todate { get; set; }

        public string OT_Remark { get; set; }

    }

    public class MaterialCost
    {
        public Int64 M_ID { get; set; }

        public Int64? MC_ID { get; set; }

        public int MaterailId { get; set; }

        public string Materail { get; set; }

        public int IssuedQty { get; set; }

        public int ReturnQty { get; set; }

        public int UsedQty { get; set; }
        
        public float Cost { get; set; }

        public float Rate { get; set; }

    }

    public class JobVehicleCost
    {
        public Int64 VD_ID { get; set; }

        public Int64? VC_ID { get; set; }

        public Int64? VehicleID { get; set; }

        public string VehicleType { get; set; }

        public string VehicleTypeID { get; set; }

        public string VehicleNo { get; set; }

        public Int64 PurposeID { get; set; }

        public string Purpose { get; set; }

        public string DriverType { get; set; }

        public string DriverTypeID { get; set; }

        public Int64? DriverID { get; set; }

        public string Driver { get; set; }

        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }

        public float? V_Cost { get; set; }
    }

    public class OutLabourCost
    {

        public Int64 L_ID { get; set; }

        public Int64? OLC_ID { get; set; }

        public int Labour_VendorId { get; set; }

        public string Labour_Vendor { get; set; }

        public int Labour_OutsideNo { get; set; }

        public float Labour_Cost { get; set; }
    }
}