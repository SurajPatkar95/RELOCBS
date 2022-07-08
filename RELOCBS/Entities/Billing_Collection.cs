using MvcValidationExtensions.Attribute;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RELOCBS.Entities
{
    public class Billing_Collection
    {
        
        //public Int64 SurveyID { get; set; }
        public Int64 EnqID { get; set; }
		public string EnqNo { get; set; }
		public Int64 EnqDetailID { get; set; }
		public int EnqSeqID { get; set; }
		public int? chgAccountMgr { get; set; }
        public string AccountMgr { get; set; }
		public string ClientGSTNO { get; set; }
		public string AccountGSTNO { get; set; }
		

		[Required (ErrorMessage = "Bill To is mandatory.")]
        public string BillingOn { get; set; }
        public int BillingOnClientId { get; set; }
        [Required]
        public int CreditApproved { get; set; }
        public bool Advance { get; set; }
        public decimal? Amount { get; set; }
        
        public string PaymentPreDelivery { get; set; }
        //[RequiredIf("PaymentPreDelivery=Balance")]
        public string PaymentPostDelivery { get; set; }
        public int? NoDays { get; set; }
        public bool PurchaseOrder { get; set; }
        public bool AthorizeQuote { get; set; }
        public bool Others { get; set; }
        public string Specify { get; set; }
        public string Remarks { get; set; }
        public ShipperDetail Shipper { get; set; } = new ShipperDetail();
        public string[] EnquiryDetailIds { get; set; }
        public DateTime Entrydate { get; set; }

        //Not used for saved
        public Int64 BillId { get; set; }
        public string RevenueBr { get; set; }
        public string ServiceLine { get; set; }
        public int ClientId { get; set; }
        public int AccountId { get; set; }
        public string PreparedBy { get; set; }
		public string Attention { get; set; }

        public string Billing_ContactPerson { get; set; }
        public string Billing_Address { get; set; }
        public string Billing_Email { get; set; }
        public string Billing_Tel { get; set; }

        public string Client_Entity { get; set; }
        public string Client_type  { get; set; }//for Report

		public string ClientType { get; set; }//For Dropdown
	}

    public class ShipperDetail
    {
        public string Title { get; set; }
        public string ShipperFName { get; set; }
        public string ShipperLName { get; set; }
        public int? ShipCategoryID { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Email { get; set; }
        public int? AddressCityID { get; set; }
        public string City { get; set; }
        public string PIN { get; set; }
		[RegularExpression(@"^\+?[0-9\/\\.@_-]+$", ErrorMessage = "InValid Phone Number.")]
		public string Phone1 { get; set; }
		[RegularExpression(@"^\+?[0-9\/\\.@_-]+$", ErrorMessage = "InValid Phone Number.")]
		public string Phone2 { get; set; }
        public DateTime? DOB { get; set; }
        public string Nationality  { get; set; }
        public string Designation { get; set; }
        public string ShipCategory { get; set; }
    }
}