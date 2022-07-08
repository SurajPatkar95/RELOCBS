using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RELOCBS.Common;
using RELOCBS.CustomAttributes;

namespace RELOCBS.Entities
{
	[AuthorizeUser]
	public class Billing
    {
		public char BillType { get; set; }
		public int MoveID { get; set; }

		public string JobNo { get; set; }

		public DateTime? JobDate { get; set; }

		public int? CreditNoteID { get; set; }

		public string Project { get; set; }

		public int? BillID { get; set; }

		public string CreditNoteNo { get; set; }

		public string InvoiceNo { get; set; }

        public DateTime? InvoiceDate { get; set; }
		
		public DateTime? CreditNoteDate { get; set; }

		public string InvoiceStatus { get; set; }

		public string ServiceLn { get; set; }
		public string RevenueBr { get; set; }
		public string Mode { get; set; }
		
		//public int? CreditNoteID { get; set; }

		//public string CreditNoteNo { get; set; }

		//public DateTime? CreditNoteDate { get; set; }

		//public int WtUnitID { get; set; }

		public string WtValue { get; set; }
		public string GrossWtValue { get; set; }
		//public int VolumeUnitID { get; set; }
		public string VolumeValue { get; set; }
		public string GrossVolumeValue { get; set; }

		public bool IsWtValue { get; set; }
		public bool IsGrossWtValue { get; set; }
		public bool IsVolumeValue { get; set; }
		public bool IsGrossVolumeValue { get; set; }
		public bool IsGoodsDesc { get; set; }
		public string GoodsDesc { get; set; }
		public decimal? AdvanceRecv { get; set; }
		public string chequeNo { get; set; }
		public string BillRemarks { get; set; }
		public DateTime? BillSubDate { get; set; }
		public string VehicleNo { get; set; }
		
		public string BaseCurrName { get; set; }
		public string RateCurrName { get; set; }

		public int? RateCurrancyID { get; set; }
		public int? BaseCurrID { get; set; }

		public decimal ConvRate { get; set; }

        //public int ModeID { get; set; }

        //public int CustomerID { get; set; }

        //public int RMCID { get; set; }

        public string Shipper { get; set; }
        public string Account { get; set; }
		public string Client { get; set; }
		//public int ChangeAccountID { get; set; }

		public string OrgCity { get; set; }
		public string DestCity { get; set; }

		public string BillToID { get; set; }

		public string BillTo { get; set; }

		public int BillToClientID { get; set; }
		public int BillToAccountID { get; set; }
		public string BillToShipperName { get; set; }

		public string Attention { get; set; }

		public char AddressType { get; set; } = 'O';//'O' FOR ORIGINADD AND 'D' FOR DESTINATIONADD

		public string Address1 { get; set; }

		public string Address2 { get; set; }

		public int CityID { get; set; }
		public string BillToCity { get; set; }

		public string PinCode { get; set; }

		public string Phone { get; set; }

		public string Email { get; set; }

		public string GSTNo { get; set; }

        //public string DeliveryAddress { get; set; }

        public string FileNo { get; set; }

        public string WKNo { get; set; }

        public string Origin { get; set; }

        public string Destination { get; set; }

        public string DestinationAgent { get; set; }

        public string OriginAgent { get; set; }

        public DateTime? PackDate { get; set; }

        public DateTime? LoadDate { get; set; }

        public DateTime? DeliveryDate { get; set; }

		//public string NoodDays { get; set; }

		public DateTime? OrgStorageStart { get; set; }
		public DateTime? OrgStorageEnd { get; set; }
		public int? OrgStorageState { get; set; }
		public string OrgStorageStateNm { get; set; }

		public DateTime? DestStorageStart { get; set; }
        public DateTime? DestStorageEnd { get; set; }
		public int? DestStorageState { get; set; }
		public string DestStorageStateNm { get; set; }

		public string PreparedBy { get; set; }

		public string ApprovedBy { get; set; }

        //public string AuditBy { get; set; }

        public string FinalApproveBy { get; set; }

		public string PreparedDate { get; set; }

		public string ApprovedDate { get; set; }
		
		//public string AuditBy { get; set; }

		public string FinalApproveDate { get; set; }

		public string TaxType { get; set; }

		public string Remark { get; set; }
		public string BillAcknowledgement { get; set; }
		
		public int? NoofPkgs { get; set; }

		public DateTime? ParsifalAuditStartDate { get; set; }
		public DateTime? ParsifalApproveDate { get; set; }
		public DateTime? MoneyReceivedDate { get; set; }

		public decimal? AEDCurr { get; set; }

		public bool RMCBuss { get; set; }
		public string RMCType { get; set; }

		public List<BillingItems> BillItems { get; set; } = new List<BillingItems>();
		public List<AddressList> AddressList { get; set; } = new List<AddressList>(4)
		{
			new AddressList(){ RateCompID=Convert.ToInt32(CommonService.RateComp.Origin),BillingTo ="Client"},
			new AddressList(){RateCompID=Convert.ToInt32(CommonService.RateComp.Origin),BillingTo ="Account"},
			new AddressList(){RateCompID=Convert.ToInt32(CommonService.RateComp.Origin),BillingTo ="Shipper"},
			new AddressList(){RateCompID=Convert.ToInt32(CommonService.RateComp.Destination),BillingTo ="Shipper"}
		};

		public List<BankDetails> Bank { get; set; } = new List<BankDetails>();
		public OtherDetails Other { get; set; } = new OtherDetails();

		public string SLShortName { get; set; }
		public int? RateTypeID { get; set; }
		public string Controller { get; set; }

		public GSTLogic GSTLogic { get; set; } = new GSTLogic();

		public bool IsCreateCreditNote { get; set; }
		public bool IsShowCreditNote { get; set; }

		public string StatementSub { get; set; }

		public string Specification { get; set; }

		public DateTime? StatementCreatedDate { get; set; }
		
		public bool IsShowDelete { get; set; }

		public string OrgCountry { get; set; }
		public string DestCountry { get; set; }

		public string InvType { get; set; }
		public bool ShowEInvoice { get; set; }

		public byte[] Image { get; set; }

		public string IRNNo { get; set; }

        public int? StrgInvID { get; set; }
        public int? StrgJobID { get; set; }
		public bool IsSubject { get; set; }
		public string Subject { get; set; }
		public bool IsNote { get; set; }
		public string Note { get; set; }

		public string LutNo { get; set; }
		public string LutMsg { get; set; }

		public int? StrgVolUnitID { get; set; }
		public string StrgVolUnit { get; set; }
		public decimal? StrgVolValue { get; set; }

		public List<BTRTax> BtrTaxList { get; set; } = new List<BTRTax>();

        public byte[] BankQRCode { get; set; }

		public int? BtrService { get; set; }
		public string BtrServiceName { get; set; }
		public bool IsCollectionDate { get; set; }
		public int? BtrPaymentTerm { get; set; }
		public string BtrPaymentTermName { get; set; }
		public int? BillingEntity { get; set; }
		public string BillingEntityName { get; set; }

		public int? CreditNoteEntity { get; set; }
		public string CreditNoteEntityName { get; set; }

		public int RMCID { get; set; }
		public string BillAddInfo { get; set; }
		public bool PermitApproveInv { get; set; }

		public decimal? InvTotalAmount { get; set; }
        public int? BillCategoryID { get; set; }
        public string BillCategoryName { get; set; }
        public decimal? SunCost { get; set; }
        public bool Is_SunCostShow { get; set; }
        public bool IsAnnexure { get; set; }

        public Dictionary<string,string> ArabicData { get; set; }
        public Dictionary<string, string> ArabicCurrData { get; set; }
    }
	
    public class BillingItems
    {
		public Int64 BillItemSeqID { get; set; }
		public Int64 BillItemID { get; set; }

        public string SacCode  { get; set; }
		public string POSID { get; set; }

		public int CostHeadID { get; set; }
		public string CostHead { get; set; }
		public int ComponentID { get; set; }
		public string Component { get; set; }

		public string TaxType { get; set; }

		public string Description { get; set; }

        public decimal OriginalAmount { get; set; }

        public decimal Amount { get; set; }

		public decimal AuditedAmount { get; set; }

		public decimal ConvRate { get; set; }

        public decimal ConvAmount { get; set; }

		public bool TaxApp { get; set; }

		public decimal GSTVATPercent { get; set; }

        public decimal SGSTAmt { get; set; }

		public decimal CGSTAmt { get; set; }

		public decimal IGSTAmt { get; set; }

		public decimal VatAmt { get; set; }

		public decimal TotalAmount { get; set; }

        public string  BilledStatus { get; set; }

		public bool Unbill { get; set; }

		public bool ShowReverseButton { get; set; }
	}

	public class AddressList
	{
		public string BillingTo { get; set; }
		public int RateCompID { get; set; }
		public string Address1 { get; set; }
		public string Address2 { get; set; }
		public int CityID { get; set; }
		public string Pincode { get; set; }
		public string GSTNO { get; set; }
	}


	
    public class BillingList
    {
        public int MoveID { get; set; }
        public List<Billing> InvoiceList { get; set; } = new List<Billing>();
        public List<Billing> CreditNoteList { get; set; } = new List<Billing>();
    }

	public class OtherDetails
	{
		public string PANNo { get; set; }
		public string StateName { get; set; }
		public string StateCD { get; set; }
		public string POS { get; set; }
		public string AddressType { get; set; }
		public string Address { get; set; }
		public string GSTNo { get; set; }
		public string VATNo { get; set; }
		public string GSTNoOur { get; set; }

		//BillBottomlines
		public string Line1 { get; set; }
		public string Line2 { get; set; }
		public string Line3 { get; set; }
		public string Line4 { get; set; }
		public string Line5 { get; set; }
		public string BottomLine1 { get; set; }
		public string BottomLine2 { get; set; }
		public string BottomLine3 { get; set; }
		public string CompanyNameLine1 { get; set; }
		public string CompanyNameLine2 { get; set; }

		public string AboveBankInfo { get; set; }
	}

	public class BankDetails
	{
		public string Header { get; set; }
		public string Value { get; set; }
		//public string BANKADDRESS { get; set; }
		//public string AccountForUAD { get; set; }
		//public string BENIFICIARYNAME { get; set; }
		//public string BENIFICIARYADDRESS { get; set; }
		//public string SWIFTCODE { get; set; }
		//public string IBAN { get; set; }
		
	}

	public class GSTLogic
	{
		public string ServiceProvided { get; set; }
		public string ServiceProviderInIndia { get; set; }
		public string ServiceReceiverRegistionStatus { get; set; }
		public string ServiceReceiverInIndia { get; set; }
		public string OriginInIndia { get; set; }
		public string DestInIndia { get; set; }
		public string IsRoadMode { get; set; }
		public string IsRevCurrINR { get; set; }
		public string ServiceProviderStateID { get; set; }
		public string IsPOS_InIndia { get; set; }
		public string POS_Rule { get; set; }
		public string POS_StateID { get; set; }
		public string GSTTYPE { get; set; }
		public string GST_Percent { get; set; }
		public string OrgStgPOS_Rule { get; set; }
		public string OrgStgIsPOS_InIndia { get; set; }
		public string OrgStgPOS_StateID { get; set; }
		public string DestStgPOS_Rule { get; set; }
		public string DestStgIsPOS_InIndia { get; set; }
		public string DestStgPOS_StateID { get; set; }
	}

	public class BTRTax
	{
		public string Code { get; set; }
		public decimal Rate { get; set; }
		public decimal Goods { get; set; }
		public decimal Tax { get; set; }
	}


    public class ArabicData
    {
        public string EngInfo { get; set; }
        public string ArabicInfo { get; set; }
    }
}