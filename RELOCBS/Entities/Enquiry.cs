using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using RELOCBS.Common;
using MvcValidationExtensions.Attribute;

namespace RELOCBS.Entities
{
    public class Enquiry
    {
        public Int64 EnqID { get; set; }
		public string EnqNo { get; set; }
		public int CompId { get; set; }
        public string EnqStatus { get; set; }
        public DateTime? StatusDate { get; set; }
		[Required(AllowEmptyStrings = false, ErrorMessage = "Info Source is Required")]
		public int? EnqSourceID { get; set; }
        public DateTime? EnqDate { get; set; }
		public string ReloSmrtEnqNo { get; set; }

		[MaxLength(100, ErrorMessage = "Maximum 100 charachter.")]
        public string EnqFrom { get; set; }
        public string EnqReceivedby { get; set; }
        public DateTime? FollowupDate { get; set; }
        public int? BussinessLineID { get; set; }
        public string BussLineName { get; set; }
		[Required(AllowEmptyStrings = false, ErrorMessage = "Client is Required")]
		public int? AgentID { get; set; }
        public string AgentName { get; set; }
		[Required(AllowEmptyStrings = false, ErrorMessage = "Move Class is Required")]
		public int? MoveQuoteID { get; set; }
		[Required(AllowEmptyStrings = false, ErrorMessage = "Revenue Branch is Required")]
		public int? RevenueBrId { get; set; }
        public string RevenueBr { get; set; }
        //[Required (AllowEmptyStrings =false,ErrorMessage = "Shipper Name is Required")]
        [MaxLength(30, ErrorMessage = "Maximum 30 charachter.")]
        public string ShipperFName { get; set; }
        [MaxLength(30, ErrorMessage = "Maximum 30 charachter.")]
        public string ShipperLName { get; set; }
		[Required(AllowEmptyStrings = false, ErrorMessage = "Shipper Category is Required")]
		public int? ShipCategoryID { get; set; }
		[Required(AllowEmptyStrings = false, ErrorMessage = "Shipper Type is Required")]
		public int? ShipTypeID { get; set; }
        [MaxLength(200, ErrorMessage = "Maximum 200 charachter.")]
        public string Address1 { get; set; }
        [MaxLength(200, ErrorMessage = "Maximum 200 charachter.")]
        public string Address2 { get; set; }
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [MaxLength(100, ErrorMessage = "Maximum 100 charachter.")]
        public string Email { get; set; }
		public string Designation { get; set; }
		public string Nationality { get; set; }
		//public DateTime DOB { get; set; }
		public int? AddressCityID { get; set; }
        public string City { get; set; }
        [MaxLength(10, ErrorMessage = "Maximum 10 charachter.")]
        public string PIN { get; set; }
        [RegularExpression(@"^\+?[0-9\/\\.@_-]+$", ErrorMessage ="InValid Phone Number.")]
        //[MaxLength(15,ErrorMessage ="Maximum 15 charachter.")]
        public string Phone1 { get; set; }
        [RegularExpression(@"^\+?[0-9\/\\.@_-]+$", ErrorMessage = "InValid Phone Number.")]
        //[MaxLength(15, ErrorMessage = "Maximum 15 charachter.")]
        public string Phone2 { get; set; }
        public string Remarks { get; set; }
        public DateTime? Createddate { get; set; }
        public string Createdby { get; set; }
        public bool Isactive { get; set; }
        public string IsLost { get; set; }
		public bool IsEnqLost { get; set; }
		public DateTime? LostDate { get; set; }
        public int? LostToCompitID { get; set; }
		public string LostTo { get; set; }
		public int? LostReasonID { get; set; }
        public string LostRemarks { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public ClientDetails ClientDetails { get; set; }
        public EnquiryDetail EnquiryDetail { get; set; }
        public int? ChangeAcctMgrID { get; set; }
        [Required(ErrorMessage ="Atleast one shipment must be added.")]
        public string EnquiryListHidden { get; set; }
        public string ClientRef { get; set; }
        public string ClientCP { get; set; }
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [MaxLength(100, ErrorMessage = "Maximum 100 charachter.")]
        public string CPEmail { get; set; }
        public string ShipperTitle { get; set; }
		public DateTime? CompareTentEnqDate { get; set; }
		public DateTime? CompareContEnqDate { get; set; }
		[GreaterThanEqualTo("EnqDate", ErrorMessage = "Tentative Move Date must be greater than Enq.Date")]
        public DateTime? TentativeDate { get; set; }
        [LessThanEqualTo("EnqDate", ErrorMessage = "Contact Date must be Less than Enq.Date")]
        public DateTime? ContactDate { get; set; }
		public bool RMCBuss { get; set; }
		public EnqFollowUpDetails FollowUp { get; set; }
		public List<EnqFollowUpDetails> FollowUpList { get; set; }
	}


    public partial class EnquiryViewModel
    {
        public int EnqID { get; set; }
        public int CompId { get; set; }
        public int EnqSourceID { get; set; }
        [Required]
        public DateTime? EnqDate { get; set; }
        public string EnqFrom { get; set; }
        public string EnqReceivedby { get; set; }
        public DateTime FollowupDate { get; set; }
        public int BussinessLineID { get; set; }
        public int AgentID { get; set; }
        public int MoveQuoteID { get; set; }
        public int RevenueBrId { get; set; }
        public string ShipperFName { get; set; }
        public string ShipperLName { get; set; }
        public int ShipCategoryID { get; set; }
        public string Address { get; set; }
        public int AddressCityID { get; set; }
        [RegularExpression(@"\d+", ErrorMessage = "Please Enter Valid Pincode")]
        public int PIN { get; set; }
        public string Phone1 { get; set; }
        public string Phone2 { get; set; }
        public string Remarks { get; set; }
        public DateTime Createddate { get; set; }
        public string Createdby { get; set; }
        public bool Isactive { get; set; }
		public bool IsLost { get; set; }
		public DateTime LostDate { get; set; }
        public int LostToCompitID { get; set; }
        public int LostReasonID { get; set; }
        public string LostRemarks { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
		public bool RMCBuss { get; set; }
    }

    public class EnquiryDetail
    {
        public Int64 EnqDetailID { get; set; }
        public Int64 EnqSequenceID { get; set; }
        public Int64 EnqID { get; set; }
        public int? HandlingBrId { get; set; }
        public int? ServiceLineID { get; set; }
        public int? FromCity { get; set; }
        public int? ToCityID { get; set; }
        public int? Mode { get; set; }
        public int? ShipmentTypeID { get; set; }
        public int? GoodsDescId { get; set; }
        public int? WtUnitid { get; set; }
        //[RegularExpression(@"^[0 - 9]([.][0 - 9])?$", ErrorMessage = "InValid Wt. Net")]
        public decimal? WtNet { get; set; }
        //[RegularExpression(@"^[0 - 9]([.][0 - 9])?$", ErrorMessage = "InValid Wt. Gross")]
        public decimal? WtGross { get; set; }
        //[RegularExpression(@"^[0 - 9]([.][0 - 9])?$", ErrorMessage = "InValid Wt. ACWT")]
        public decimal? WtACWT { get; set; }
        public int? VolumeUnitID { get; set; }
        //[RegularExpression(@"^[0 - 9]([.][0 - 9])?$", ErrorMessage = "InValid Volume To Pack")]
        public decimal? VolumeToPack { get; set; }
        //[RegularExpression(@"^[0 - 9]([.][0 - 9])?$", ErrorMessage = "InValid Volume Net")]
        public decimal? VolumeNet { get; set; }
        //[RegularExpression(@"^[0 - 9]([.][0 - 9])?$", ErrorMessage = "InValid Volume Gross")]
        public decimal? VolumeGross { get; set; }
        public int? ContainerTypeID { get; set; }
        public string Remarks { get; set; }
        public DateTime? TentativeMovedate { get; set; }
        public int? SchSurveyorID { get; set; }
        public DateTime? SchSurveyDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public bool IsActive { get; set; }
        //[RegularExpression(@"^[0 - 9]([.][0 - 9])?$", ErrorMessage = "InValid Density Factor")]
        public decimal? DensiyFactor { get; set; }
        public string LooseCased { get; set; }
        public string LCLFCL { get; set; }
        public string HandlingBr { get; set; }
        public string ServiceLine { get; set; }
        public string FCity { get; set; }
        public string ToCity  { get; set; }
        public string TMode { get; set; }
        public string ShipmentType { get; set; }
        public string GoodsDesc { get; set; }
        public string WtUnit { get; set; }
        public string VolumeUnit { get; set; }
        public string ContainerType{ get; set; }
        public string SchSurveyor { get; set; }
        public string SchSurveyorRemark { get; set; }
		public bool IsShowSurvey { get; set; }
		public bool AllowEdit { get; set; }
		public bool ShowSendToMobile { get; set; }
		public bool ShowFollowUp { get; set; }
		public string ShpStatus { get; set; }
	}


    public class ClientDetails
    {
		[Required(ErrorMessage = "Corporate is required.")]
		public int Client { get; set; }
        public string Account { get; set; }
        public string AccountMgr { get; set; }
        public string AccountType { get; set; }
		public string BussLnID { get; set; }
		public string BussLnName { get; set; }
		public string ContactPerson { get; set; }
		public string ClientGSTNO { get; set; }
		public string AccountGSTNO { get; set; }
	}

	public class EnqFollowUpDetails
	{
		public Int64 EnqDetID { get; set; }
		public DateTime? FollowUpDate { get; set; }
		public string FollowUpRemark { get; set; }
		public bool IsClose { get; set; }
		public string CreatedBy { get; set; }
		public DateTime? CreatedDate { get; set; }
	}
}