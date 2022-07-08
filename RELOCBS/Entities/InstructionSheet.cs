using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RELOCBS.Entities
{
    public class InstructionSheet
    {
        public Int64 MoveID { get; set; }

        public Int64? InstID { get; set; }

        public Int32? StatusID { get; set; }

        public string Status { get; set; }

        public Int32? IsSentToWarehouse { get; set; }
        
        public string JobNo { get; set; }
        public DateTime JobOpenDate { get; set; }

        public int ShipperID { get; set; }
        public string Shipper { get; set; }
        public string ShipperAddress { get; set; }

        public int ModeID { get; set; }
        public string Mode { get; set; }

        public int ClientID { get; set; }
        public string Client { get; set; }
        public string ClientAddress { get; set; }

        public int CorporateID { get; set; }
        public string Corporate { get; set; }
        public string CorporateAddress { get; set; }

        public int WeightUnitID { get; set; }
        public string WeightUnit { get; set; }
        
        public float WeightUnitFrom { get; set; }
        public float WeightUnitTo { get; set; }

        public int InsTypeID { get; set; }
        public string  InsType { get; set; }

        public int    ServiceLineID  { get; set; }
        public string ServiceLine    { get; set; }
        public string ProjectService { get; set; }

        public int OriginCityID { get; set; }
        public string OriginCity { get; set; }

        public int DestinationCityID { get; set; }
        public string DestinationCity { get; set; }

        public float Volume { get; set; }
        

        public DateTime? ExpectedBeginDateTime { get; set; }

        public DateTime? ExpectedCompletionDateTime { get; set; }

        public DateTime? ActualBeginDateTime { get; set; }

        public DateTime? ActualCompletionDateTime { get; set; }

        public TimeSpan? TurnaroundTime { get; set; }

        public string SpecialInstructions { get; set; }

        public string Remarks { get; set; }

        public int WareHouseID { get; set; }

        public string WareHouseName { get; set; }

        public int BranchID { get; set; }

        public string BranchName { get; set; }

        public int? GoodsDescriptionID { get; set; }

        public string GoodsDescription { get; set; }
        
        public string ComponentType { get; set; }

        public int? ComponentTypeID { get; set; }

        public string OrgAdd1 { get; set; }

        public string OrgAdd2 { get; set; }

        public int? OrgCityID { get; set; }
        public string OrgCity { get; set; }

        public string OrgPincode { get; set; }

        public string OrgMobile { get; set; }

        public string OrgPhone { get; set; }

        [MaxLength(100)]
        [RegularExpression(@"[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}", ErrorMessage = "Please enter correct email")]
        public string OrgEmail { get; set; }

        public string DestAdd1 { get; set; }
        public string DestAdd2 { get; set; }

        public int? DestCityID { get; set; }

        public string DestCity { get; set; }

        public string DestPincode { get; set; }

        public string DestMobile { get; set; }

        public string DestPhone { get; set; }

        [MaxLength(100)]
        [RegularExpression(@"[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}", ErrorMessage = "Please enter correct email")]
        public string DestEmail { get; set; }


        public List<SelectListItem> Instructions { get; set; } = new List<SelectListItem>();

        
        [Display(Name = "Choose Multiple Instrictions")]
        public int[] SelectedMultiInstructionsId { get; set; }

        public List<ModeLables> modeLables { get; set; } = new List<ModeLables>();

        public List<Question> questions { get; set; } = new List<Question>();
        
        //public Domestic_Inst domestic = new Domestic_Inst();
        //public Import_Inst import_Inst = new Import_Inst();
        //public Export_Inst export_Inst = new Export_Inst();

        public List<CaseDimensions> Dimensions { get; set; } = new List<CaseDimensions>();
        public string HVDimensions { get; set; }
        public List<Inst_Activities> activities { get; set; } = new List<Inst_Activities>();
        public string HVactivityList { get; set; }

        public string CreatedBy { get; set; }
    }

    
    public class Question
    {
        public Int64? InstID { get; set; }
        public int? QuestionID { get; set; }
        public string Questions { get; set; }
        public bool IsSubItem { get; set; }
        public bool IsActive { get; set; }
        public string IsSaved { get; set; }
        public string hide { get; set; }

        public List<SubQuestion>  subQuestions { get; set; }
    }


    public class SubQuestion
    {
        public Int64? InstID { get; set; }
        public int? QuestionID { get; set; }
        public int? SubQuestionID { get; set; }
        public string SubQuestions { get; set; }
        public string DropdownType { get; set; }
        public string AnswerType { get; set; }
        public string AnswerText { get; set; }
        public string Answer { get; set; }
        public DateTime? AnswerDate { get; set; }
        public int? IDtoRefer { get; set; }
        public bool IsActive { get; set; }
        public string IsSaved { get; set; }
        public Int16 OrderBy { get; set; }

        public IEnumerable<SelectListItem> AnswerDropdown { get; set; }
    }

    public class ModeLables
    {
        public int InstID { get; set; }

        public int InfoID { get; set; }

        public int ModeID { get; set; }

        public string ModeName { get; set; }

        public int NoOfLables { get; set; }

        public string LabelStartFrom { get; set; }
        
    }

    //public class Import_Inst
    //{
    //}

    //public class Export_Inst
    //{
    //}

    //public class Domestic_Inst
    //{
    //}

    public class CaseDimensions
    {
        public Int64 CS_ID { get; set; }

        public Int64 InstID { get; set; }

        public int CaseTypeID { get; set; }

        public string CaseType { get; set; }

        public float Length { get; set; }

        public float Breadth { get; set; }
        public float Height { get; set; }

        public int UnitID { get; set; }

        public string UnitName { get; set; }

        public int NoOfPackages { get; set; }

        public Int16 InActive { get; set; } = 0;
    }

    public class Inst_Activities
    {
        public Int64 ActivityID { get; set; }
        public Int64 InstID { get; set; }

        [Required]
        public int ActivityTypeID { get; set; }

        public string ActivityTypeName { get; set; }

        [Required]
        public DateTime FromDate { get; set; }

        [Required]
        public DateTime ToDate { get; set; }

        [Required]
        public string FromLocation { get; set; }

        [Required]
        public string ToLocation { get; set; }

        public TimeSpan? RepTime { get; set; }

        public string Remark { get; set; }

        public Int16 InActive { get; set; } = 0;

        public Int32? ACT_StatusID { get; set; }
        public string ACT_Status { get; set; }

        public Int64? ACT_BatchID { get; set; }

        public string  Status { get; set; }

        public string RejectRemark { get; set; }

        public Int64? Inst_BatchID { get; set; }

        [Range(1, 999, ErrorMessage = "Please enter a Days between 1 and 999")]
        [Required]
        public int? NumberOfDays { get; set; }

        public Int16 JobType { get; set; }

    }

    public class JobInstGrid
    {
        public Int64? EnqDetailID { get; set; }
        public Int64? SurveyID { get; set; }

        public Int64 MoveID { get; set; }
        public string JobNo { get; set; }
        public DateTime JobOpenDate { get; set; }

        public string FromCity { get; set; }
        public string ToCity { get; set; }
        public string EntryPort { get; set; }
        public string ExitPort { get; set; }
        public float Volume { get; set; }
        public string Mode { get; set; }

        public string Shipper { get; set; }
        public string Corporate { get; set; }
        public string Client { get; set; }

        public string ServiceLine { get; set; }
        public string Status { get; set; }

        public int    RateComponentID   { get; set; }
        public string RateComponentName { get; set; }
        public string JobReport_Status  { get; set; }
        public string JobReport_Type    { get; set; }
        public Int64?  PJR_DJR_ID        { get; set; }

        public string PJR_AddEdit { get; set; }
        public List<InstructionSheetGrid> instructionSheetGrids { get; set; } = new List<InstructionSheetGrid>();

        public string RevenueBranch { get; set; }
        public string HandlingBranch { get; set; }
        public string BusinessLine { get; set; }

        
    }

    public class InstructionSheetGrid
    {
        public Int64 InstID { get; set; }

        public Int64 MoveID { get; set; }

        public string JobNo { get; set; }

        public string Shipper { get; set; }

        public DateTime InstDate { get; set; }

        public string InstType { get; set; }

        public DateTime? ExpectedBeginDateTime { get; set; }
        public DateTime? ExpectedCompletionDateTime { get; set; }

        public string SpecialInstructions { get; set; }

        public string Inst_Status { get; set; }

        public string Edit { get; set; }
        public string Delete { get; set; }
        public string View { get; set; }

        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public string BranchName { get; set; }
        public int WarehouseID { get; set; }
        public string WarehouseName { get; set; }

        public string Wt_Vol { get; set; }

        public Int64? BatchID { get; set; }

        public int RateComponentID { get; set; }
        public string RateComponentName { get; set; }
        public string JobReport_Type { get; set; }

        public List<Inst_Activities> jobActivities { get; set; } = new List<Inst_Activities>();
        public int IsSentToWarehouse { get; set; }

    }


    public class ActitiyRootObject
    {
        public List<Inst_Activities> Inst_Activities { get; set; }
    }

    public class DimensionRootObject
    {
        public List<CaseDimensions> Dimensions { get; set; }
    }

    public class JobDiaryModel
    {

        public List<Int64> InstIds { get; set; }
        public IEnumerable<InstructionSheetGrid> instructionSheetGrids { get; set; }
        public Int16 JobType { get; set; }
    }

    
    public class ActivityAllocationModel
    {
        public string JobNo { get; set; }

        public string ShipperName { get; set; }

        public List<Int64> ActivityIds { get; set; }

        public List<Int64> InstIds { get; set; }

        public Int64? BatchID { get; set; }

        public int ACT_StatusID { get; set; }

        public string ACT_Status { get; set; }

        public JobCrew jobCrew { get; set; } = new JobCrew();

        public List<OutsideLabour> outsideLabours { get; set; } = new List<OutsideLabour>();

        public List<JobService> services { get; set; } = new List<JobService>();

        public List<JobVehicle> jobVehicleList { get; set; } = new List<JobVehicle>();

        public JobDocUpload docUpload { get; set; } = new JobDocUpload();

        public List<MaterialUsed> materialUsed { get; set; } = new List<MaterialUsed>();

        public string HVCrewMembers { get; set; }
        public string HVServices { get; set; }
        public string HVVehicles { get; set; }
        public string HVMaterialUsed { get; set; }
        public string HVDocList { get; set; }

        public string CancelRemark { get; set; }

        public string submit { get; set; }

        public int TabIndex { get; set; } = 0;

        public int RateComponentID { get; set; }
        public int BranchID { get; set; }

        public Int16 JobType { get; set; }

        public HireVehileSendForApprove hireVehileSendForApprove { get; set; } = new HireVehileSendForApprove();
        public HireVehileApproval hireVehileApproval { get; set; } = new HireVehileApproval();

        public int CompanyID { get; set; }

        public int? PackInventID { get; set; }
    }

    public class CrewUtilizationDashobard
    {
        [Required(ErrorMessage = "Month required")]
        public DateTime ForMonthDate { get; set; }
        [Required(ErrorMessage ="Warehoue required")]
        public int WarehoseId { get; set; }
        public DataSet data { get; set; }
    }

}