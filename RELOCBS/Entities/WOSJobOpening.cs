using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Web;

namespace RELOCBS.Entities
{
    public class WOSJobOpening
    {
        public Int64 WOSMoveID { get; set; }

        public Int64 MoveID { get; set; }

        public string WOSJobID { get; set; }
        public DateTime? WOSJobOpenedDate { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please select business line.")]
        public bool? IsRMCBus { get; set; } = true;

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please select client.")]
        public Int64? ClientID { get; set; }
        public string ClientName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please select account.")]
        public Int64? AccountID { get; set; }
        public string AccountName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter assignee first name.")]
        public string AssigneeFName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter assignee last name.")]
        public string AssigneeLName { get; set; }

        public string AssigneeName { get; set; }

        public Int64? FromCityID { get; set; }
        public string FromCityName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please select to city.")]
        public Int64? ToCityID { get; set; }
        public string ToCityName { get; set; }

        public string FileNo { get; set; }
        public string WONumber { get; set; }
        public string OldJobNo { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter tentative billing month.")]
        public DateTime? TentativeBillingMonth { get; set; }

        public string JobRemarks { get; set; }

        public int? CompanyID { get; set; }
        public string CompanyName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please select service line.")]
        public Int64? ServiceLineID { get; set; }
        public string ServiceLine { get; set; }
        public string SLShortName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please select revenue branch.")]
        public int? RevBranchID { get; set; }
        public string RevBrName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please select service branch.")]
        public int? JobBrID { get; set; }
        public string JobBrName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please select consultant.")]
        public int? SDId { get; set; }
        public string SDName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please select client relations consultant.")]
        public int? SRId { get; set; }
        public string SRName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please select KAM.")]
        public int? KAM { get; set; }
        public string KAMName { get; set; }

        public string EmailTo { get; set; }
        public string EmailCc { get; set; }
        public string EmailBcc { get; set; }

        public string ActiveTab { get; set; }

        public string JobStatus { get; set; }
        public bool? IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public Assignee Assignee { get; set; } = new Assignee();

        public WOSCustomer WOSCustomer { get; set; } = new WOSCustomer();
        public List<WOSCustomer> WOSCustomerList { get; set; } = new List<WOSCustomer>();

        public WOSBilling WOSBilling { get; set; } = new WOSBilling();

        public List<WOSJobDocUpload> WOSJobDocUploadList { get; set; } = new List<WOSJobDocUpload>();
        public WOSJobDocUpload WOSJobDocUpload { get; set; } = new WOSJobDocUpload();

        public DataSet dsCostSheet { get; set; } = new DataSet();

        public bool? IsCostSheetSaved { get; set; }
        public bool? IsCostSheetApproved { get; set; }
        public string CostSheetApproved { get; set; }
        public bool? IsCSSentToApprove { get; set; }
        public int? CSSentToApproveUser { get; set; }

        public string CSPreparedBy { get; set; }
        public DateTime? CSPreparedDate { get; set; }
        public string CSApprovedBy { get; set; }
        public DateTime? CSApprovedDate { get; set; }

        public string JobCancelRemark { get; set; }

        public WOSHouse WOSHousing { get; set; }
        public WOSHouseDetails WOSHouseDetails { get; set; }

        public List<TabList> TabList { get; set; } = new List<TabList>();
    }

    public class Assignee
    {
        public Int64? WOSMoveID { get; set; }
        public Int64? MoveID { get; set; }
        public string WOSJobID { get; set; }

        //public string AssigneeListHidden { get; set; }

        public string Status { get; set; }
        //public string Customer { get; set; }
        //public string CustomerContact { get; set; }
        //public string Type { get; set; }
        //public string TransfereeLocation { get; set; }
        //public string Account { get; set; }
        //public string Coordinator { get; set; }
        //public string Manager { get; set; }
        //public string MovingTo { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Last Name is Required")]
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string MaidenName { get; set; }
        public string AssigneeTitleId { get; set; }

        public string AssigneeRef { get; set; }
        public int? GenderId { get; set; }
        public bool IsSmoker { get; set; }
        public DateTime? BirthDate { get; set; }
        public string Languages { get; set; }
        public string Religion { get; set; }

        [EmailAddress(ErrorMessage = "Invalid Email Address.")]
        [Required(ErrorMessage = "Email address is required.")]
        public string EmailId { get; set; }

        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public Int64? CityID { get; set; }
        public string City { get; set; }
        public string PINCode { get; set; }

        public int? TransfereeId { get; set; }

        //public string PassportNo { get; set; }
        //public string PassportIssuedBy { get; set; }
        //public DateTime? PassportIssueDate { get; set; }
        //public DateTime? PassportExpiryDate { get; set; }
        //public int? CountryId_Nationality { get; set; }
        //public int? CountryId_OtherNationality { get; set; }
        //public string PlaceOfBirth { get; set; }
        //public int? CountryId_CountryOfBirth { get; set; }
        //public string DrivingLicenseNumber { get; set; }
        //public int? CountryId_DrivingLicenseIssuedBy { get; set; }
        //public DateTime? DrivingLicenseIssueDate { get; set; }
        //public DateTime? DrivingLicenseExpiryDate { get; set; }
        //public string IDCardNo { get; set; }
        //public string IDCardIssuedBy { get; set; }
        //public DateTime? IDCardIssueDate { get; set; }
        //public DateTime? IDCardExpiryDate { get; set; }
        //public string SSN { get; set; }
        //public string VAT { get; set; }
        //public string TransfereeWorkReference { get; set; }

        public string LastName_Partner { get; set; }
        public string FirstName_Partner { get; set; }
        public string MiddleName_Partner { get; set; }
        public string MaidenName_Partner { get; set; }
        public string Title_Partner { get; set; }

        [EmailAddress(ErrorMessage = "Invalid Partner Email Address.")]
        public string EmailId_Partner { get; set; }

        public int? GenderId_Partner { get; set; }
        public bool IsSmoker_Partner { get; set; }
        public DateTime? BirthDate_Partner { get; set; }
        public string Languages_Partner { get; set; }
        public string Religion_Partner { get; set; }
        public string Occupation_Partner { get; set; }
        //public string PassportNo_Partner { get; set; }
        //public string PassportIssuedBy_Partner { get; set; }
        //public DateTime? PassportIssueDate_Partner { get; set; }
        //public DateTime? PassportExpiryDate_Partner { get; set; }
        //public int? CountryId_Nationality_Partner { get; set; }
        //public int? CountryId_OtherNationality_Partner { get; set; }
        //public string PlaceOfBirth_Partner { get; set; }
        //public int? CountryId_CountryOfBirth_Partner { get; set; }
        //public string DrivingLicenseNumber_Partner { get; set; }
        //public int? CountryId_DrivingLicenseIssuedBy_Partner { get; set; }
        //public DateTime? DrivingLicenseIssueDate_Partner { get; set; }
        //public DateTime? DrivingLicenseExpiryDate_Partner { get; set; }
        //public string IDCardNo_Partner { get; set; }
        //public string IDCardIssuedBy_Partner { get; set; }
        //public DateTime? IDCardIssueDate_Partner { get; set; }
        //public DateTime? IDCardExpiryDate_Partner { get; set; }
        //public string SSN_Partner { get; set; }
        //public string VAT_Partner { get; set; }
        //public DateTime? PrivacyAgreementAcceptanceDate { get; set; }

        public int? MaritalStatusId { get; set; }
        public DateTime? MarriageDate { get; set; }
        public bool IsMarriageCertAvailable { get; set; }
        //public bool? MyProperty { get; set; }
        public bool IsPetsMoving { get; set; }
        public string PetsDescription { get; set; }
        public string FamilyDetails { get; set; }
        public bool IsMovingTogether { get; set; }
        public string NextOfKinDetails { get; set; }
        //public string MarriagePlace { get; set; }
        //public int? CountryId_CountryOfMarriage { get; set; }
        //public int? ParentTransfereeId { get; set; }
        public string SpecialHealthProblem { get; set; }

        public int? NoOfChildren { get; set; }
        public List<ChildDetails> ChildDetailsList { get; set; } = new List<ChildDetails>();
        public string ChildDetailsListHidden { get; set; }

        public DateTime? HomeFindingVisitDate { get; set; }
        public string DestinationServicesDates { get; set; }
        public DateTime? ArrivalDate { get; set; }
        public DateTime? ArrivalTime { get; set; }
        public string HotelName { get; set; }
        public string HotelContactNo { get; set; }

        public int? CurrencyId { get; set; }

        public string RentOrPurchase { get; set; }
        public decimal? AllowancePerMonth { get; set; }
        //public string HouseOrApartment { get; set; }
        public bool IsHouse { get; set; }
        public bool IsApartment { get; set; }
        public string OtherHousingPreference { get; set; }
        public bool IsAllowanceIncludesUtilities { get; set; }
        public decimal? AllowancePerMonthWithUtilities { get; set; }
        public string FurnishedOrUnfurnished { get; set; }
        public bool IsAllowanceIncludesFurniture { get; set; }
        public decimal? AllowancePerMonthWithFurniture { get; set; }
        public string EssentialFeatures { get; set; }

        public int? NoOfBedrooms { get; set; }
        public int? NoOfBathrooms { get; set; }
        public int? NoOfLivingRooms { get; set; }
        public int? NoOfDiningRooms { get; set; }

        public bool IsPreferredCommunities { get; set; }
        public string PreferredCommunitiesDetails { get; set; }
        public int? LocationId { get; set; }
        public string MaxCommutingTime { get; set; }
        public string ByCarOrPublicTransport { get; set; }

        public string WithinEasyReachOfTheProperty { get; set; }

        public bool IsDriversLicenseAvailable { get; set; }
        public int? CountryId_DrivingLicenseIssuedBy { get; set; }

        public string VehiclesImportLeasePurchase { get; set; }
        public int? NoOfVehicles { get; set; }
        public string TypeOfVehicles { get; set; }

        public List<ChatDetails> ChatDetailsList { get; set; } = new List<ChatDetails>();
        public List<TaskDetails> TaskDetailsList { get; set; } = new List<TaskDetails>();

        public int RowID { get; set; }
        public Int64? RequestDocsGroupID { get; set; }
        //public WOSJobDocUpload RequestDocsUpload { get; set; } = new WOSJobDocUpload();
        public List<WOSJobDocUpload> WOSJobDocUploadList { get; set; } = new List<WOSJobDocUpload>();
        public WOSJobDocUpload WOSJobDocUpload { get; set; } = new WOSJobDocUpload();

        public bool IsFinalSubmit { get; set; }

        public DateTime? CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public bool? IsActive { get; set; }
    }

    public class ChildDetails
    {
        public Int64 ChildDetailsId { get; set; }
        public string ChildName { get; set; }
        public int? ChildAge { get; set; }
        public string SchoolType { get; set; }
        public int? SchoolTypeId { get; set; }
        public string SpecificSchoolDetails { get; set; }
        public string SpecialSchoolRequirements { get; set; }
    }

    public class ChatDetails
    {
        public Int64 ChatID { get; set; }
        public Int64 WOSMoveID { get; set; }
        public DateTime? ChatDateTime { get; set; }
        public string ChatBy { get; set; }
        public string ChatMsg { get; set; }
    }

    public class TaskDetails
    {
        public Int64 ProgressID { get; set; }
        public int TaskMasterID { get; set; }
        public Int64 WOSMoveID { get; set; }
        public DateTime? ScheduleDate { get; set; }
        public DateTime? ActualDate { get; set; }
        public string TaskDescription { get; set; }
        public string Remarks { get; set; }
    }

    public class WOSJobDocUpload
    {
        public Int64 ID { get; set; }
        public Int64? WOSMoveID { get; set; }
        public int? FileID { get; set; }
        public Int64? JobDocTypeId { get; set; }
        public string DocFromType { get; set; }
        public int? DocTypeID { get; set; }
        public int? DocNameID { get; set; }
        public int[] DocNameIDList { get; set; }
        public int[] DocNameIDListSelected { get; set; }
        public string DocNameIDListHidden { get; set; }
        public string DocDescription { get; set; }
        public bool IsShowToAssignee { get; set; }
        public string Remarks { get; set; }
        public string FileName { get; set; }
        public HttpPostedFileBase[] file { get; set; }
        public HttpPostedFileBase[] ExtFile { get; set; }
        public List<JobDocument> docLists { get; set; } = new List<JobDocument>();
        public string DocTypeText { get; set; }
        public string DocNameText { get; set; }
        public string UploadBy { get; set; }
        public int? UploadById { get; set; }
        public DateTime? UploadDate { get; set; }
    }

    public class WOSHouse
    {
        public Int64? WOSMoveID { get; set; }
        public Int64? HouseMasterID { get; set; }
        public string OwnerTitle { get; set; }
        public string OwnerFName { get; set; }
        public string OwnerLName { get; set; }
        public string EmailId { get; set; }
        public string PhoneNo { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public Int64? CityID { get; set; }
        public string City { get; set; }
        public string PINCode { get; set; }
        public bool? IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }

    public class WOSHouseDetails
    {
        public Int64? HouseJobTransID { get; set; }
        public Int64? WOSMoveID { get; set; }
        public Int64? HouseMasterID { get; set; }
        public DateTime? LeaseSignedDate { get; set; }
        public DateTime? LeaseRenewedUntilDate { get; set; }
        public DateTime? LeaseExpiryDate { get; set; }
        public bool IsFixedPeriod { get; set; }
        public DateTime? FinalArrDate { get; set; }
        public DateTime? HHGArrDate { get; set; }
        public DateTime? WorkStartDate { get; set; }
        public DateTime? OfficialHandoverPropertyDate { get; set; }
        public DateTime? InspectionDateIn { get; set; }
        public DateTime? ShortTermInspDate { get; set; }
        public DateTime? SettlingDate { get; set; }
        public DateTime? ExpMoveOutDate { get; set; }
        public DateTime? ActMoveOutDate { get; set; }
        public DateTime? InspectionDateOut { get; set; }
        public DateTime? ProperyReleaseByDate { get; set; }
        public DateTime? OfficialHandbackPropertyDate { get; set; }
        public DateTime? InactiveDate { get; set; }
        public bool IsClosed { get; set; }
        public bool? IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}