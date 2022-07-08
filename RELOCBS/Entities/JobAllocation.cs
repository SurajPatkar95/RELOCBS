using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RELOCBS.Entities
{
   public class JobAllocationModel
    {
        
        public Int64 MoveID { get; set; }
        public Int64? InstID { get; set; }

        public string JobNo { get; set; }
        public DateTime JobOpenDate { get; set; }

        public DateTime? ExpBeginDate { get; set; }
        public DateTime? ExpCompDate { get; set; }

        public string CurrentStatus { get; set; }

        [Required(ErrorMessage = "Actual Begin Date Time required")]
        public DateTime? ActualBeginDate { get; set; }

        [Required(ErrorMessage = "Actual Completed Date Time required")]
        public DateTime? ActulaCompleteDate { get; set; }

        [Required(ErrorMessage = "Turn around Time required")]
        public TimeSpan? TurnaroundTime { get; set; }

        public string Special_Instructions { get; set; }
        public string Remark { get; set; }

        public int JA_CurrentStatusID { get; set; }
        public DateTime? ScheduledDate { get; set; }
        public int ScheduledBy { get; set; }
        public DateTime? CompletedDate { get; set; }
        public int CompletedBy { get; set; }
        public DateTime? RejectedDate { get; set; }
        public int RejectedBy { get; set; }
        public string RejectedRemark { get; set; }
        
        public List<JobActivity> activities { get; set; } = new List<JobActivity>();
        public string activityList { get; set; }
        public int TabIndex { get; set; }

    }

    public class JobActivity
    {
        public Int64 ActivityID { get; set; }
        public Int64 InstID { get; set; }
        public Int64 MoveID { get; set; }
        
        public int TabIndex { get; set; } = 0;

        [Required(ErrorMessage = "Activity Type required")]
        public int ActivityTypeID { get; set; }

        public string ActivityName { get; set; }
        
        [Required(ErrorMessage ="Start Datetime required")]
        public DateTime? FromDate { get; set; }
        [Required(ErrorMessage = "End Datetime required")]
        public DateTime? ToDate { get; set; }

        [Required(ErrorMessage = "From Location required")]
        public string FromLocation { get; set; }
        [Required(ErrorMessage = "To Location required")]
        public string ToLocation { get; set; }

        [Required(ErrorMessage = "Time required")]
        public TimeSpan? RepTime { get; set; }

        public string Remark { get; set; }

        public JobCrew jobCrew { get; set; } = new JobCrew();

        public List<OutsideLabour> outsideLabours { get; set; } = new List<OutsideLabour>(); 

        public List<JobService> services { get; set; } = new List<JobService>();

        public List<JobVehicle> jobVehicleList { get; set; } = new List<JobVehicle>();

        public DocUpload docUpload { get; set; } = new DocUpload();

        public List<MaterialUsed> materialUsed { get; set; } = new List<MaterialUsed>();

    }

    public class JobAllocationGrid
    {
        
        public Int64 MoveID { get; set; }
        public Int64? JAID { get; set; }

        public string JobNo { get; set; }

        public DateTime? ExpBeginDate { get; set; }
        public DateTime? ExpCompDate { get; set; }
        
        public DateTime? ActualBeginDate { get; set; }
        public DateTime? ActulaCompleteDate { get; set; }
        public TimeSpan? TurnaroundTime { get; set; }

        public string CrewCode { get; set; }
        public string Superviser { get; set; }
        public string Status { get; set; }
        
    }


    public class JobCrew
    {
        public Int64 ActivityID { get; set; }
        public int? CrewID { get; set; }

        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

        public int? SuperviserID { get; set; }
        
        public List<CrewMember> members { get; set; } = new List<CrewMember>();
    }

    public class JobService
    {
        public Int64 SD_ID { get; set; }

        public Int64? ASC_ID { get; set; }

        public Int64 ActivityID { get; set; }
        public Int64 MoveId { get; set; }

        public int ServiceID { get; set; }
        public string ServiceName { get; set; }

        public string Description { get; set; }

        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        
        public double Cost { get; set; }

        public Int32 PJR_Status { get; set; }

    }

    public class JACrewGrid
    {
        public Int64 ActivityID { get; set; }

        public Int64 MoveID { get; set; }

        public Int64 CrewID { get; set; }
        public string CrewCode { get; set; }

        public int SuperviserID { get; set; }
        public string Superviser { get; set; }

        public int PackerCount { get; set; }
        public int HandyManCount { get; set; }
        public int LoaderCount { get; set; }


    }

    public class DocUpload
    {
        public Int64 ActivityID { get; set; }
        
        public Int32? DocTypeID { get; set; }
        
        public HttpPostedFileBase file { get; set; }

        public List<DocList> docLists { get; set; } = new List<DocList>();
    }

    
    public class JobVehicle
    {
        public int rowIndex { get; set; } = -1;
        public Int64 MoveID { get; set; }

        public Int64 V_ID { get; set; }

        public Int64 ActivityID { get; set; }
        
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

        public Int32 PJR_Status { get; set; }

        public string V_Remark { get; set; }

        public int? Approve_StatusId { get; set; }

        public string Approve_Status { get; set; }

        public bool IsApprover { get; set; }

        public string Approve_Remark { get; set; }
        public DateTime? Approve_Date { get; set; }
        public string Approve_By { get; set; }

        public int? MovementID { get; set; }
        public string MovementName { get; set; }

        public int? SupplierID { get; set; }
        public string SupplierName { get; set; }

        public int? DimensionID { get; set; }
        public string DimensionName { get; set; }

        public int? ReasonID { get; set; }
        public string ReasonName { get; set; }

        public string FromLocation { get; set; }
        public string ToLocation { get; set; }
        public string VolumeCFT { get; set; }
    }


    public class DocList
    {
        public Int64 ActivityID { get; set; }

        public Int64 DocID { get; set; }
        
        public Int32 DocTypeID { get; set; }

        public String DocType { get; set; }

        public String DocumentName { get; set; }

        public HttpPostedFileBase file { get; set; }
    }


    public class MaterialUsed
    {
        public Int64 M_ID { get; set; }

        public int MaterailId { get; set; }

        public string Materail { get; set; }
        
        public int? UsedQty { get; set; }

        public int? IssuedQty { get; set; }

        public int? ReturnQty { get; set; }

        public float? Cost { get; set; }

        public float? Rate { get; set; }

        public Int32 PJR_Status { get; set; }
    }

    public class OutsideLabour
    {
        public Int64? OLabourID { get; set; }
        public int? CrewVendorID { get; set; }
        public string VendorName { get; set; }
        public int? NoOfPerson { get; set; }
        public float? VendorCost { get; set; }
        public Int32 PJR_Status { get; set; }

    }

    public class CrewMemberRootObject
    {
        public List<CrewMember> members { get; set; }
    }

    public class JobServiceRootObject
    {
        public List<JobService> services { get; set; }
    }

    //public List<OutsideLabour> outsideLabours { get; set; } = new List<OutsideLabour>();

    public class JobVehicleRootObject
    {
        public List<JobVehicle> jobVehicleList { get; set; }
    }

    public class MaterialUsedRootObject
    {
        public List<MaterialUsed> materialUsed { get; set; }
    }

    public class DocListRootObject
    {
        public List<DocList> docLists { get; set; }
    }

    public class EmployeeAllocation
    {
        public List<EmpLeaves> leaves { get; set; } = new List<EmpLeaves>();

        public List<CrewMember>  existingAllocation  { get; set; } = new List<CrewMember>();
    }

    public class HireVehileSendForApprove
    {

        public Int64? V_ID { get; set; }
        public string Remark { get; set; }
        public float? Cost { get; set; }

    }

    public class HireVehileApproval
    {
        public Int64? V_ID { get; set; }
        public string IsApproved { get; set; }
        public string Remark { get; set; }
    }

}