using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RELOCBS.Entities
{
    public class StorageBill
    {
        public Int64 MoveID { get; set; }

        public Int64 StorageID { get; set; }

        public Int64? BillID { get; set; }

        public Int64? MainInvID { get; set; }
        
        public char BillType { get; set; }

        public string JobNo { get; set; }

        public DateTime JobDate { get; set; }

        public int? CreditNoteID { get; set; }

        public string Project { get; set; }

        public String ServiceLine { get; set; }

        public string QuotationID { get; set; }

        public String ShipperName { get; set; }

        public String ShipperAddress { get; set; }

        public String Controller { get; set; }

        public String Client { get; set; }

        public String Corporate { get; set; }

        public String JobCommodity { get; set; }

        public DateTime? BillToDate { get; set; }

        public decimal Total { get; set; }

        public decimal TaxTotal { get; set; }

        public string  Note { get; set; }

        public int PreparedByID { get; set; }
        public string PreparedBy { get; set; }
        public DateTime PreparedDate { get; set; }

        public int ApprovedByID { get; set; }
        public string ApprovedBy { get; set; }
        public DateTime ApprovedDate { get; set; }

        public int FinalApproveByID { get; set; }
        public string FinalApproveBy { get; set; }
        public string FinalApproveDate { get; set; }


        public int StatusID { get; set; }
        public string StatusName { get; set; }

        public string InvoiceStatus { get; set; }

        public string Remarks { get; set; }

        public string TaxType { get; set; }

        public List<StorageBillDetails> DetailList { get; set; } = new List<StorageBillDetails>();
        public int ProcessRowIndex { get; set; }

        public string SLShortName { get; set; }
        public int? RateTypeID { get; set; }

        public GSTLogic GSTLogic { get; set; } = new GSTLogic();

        public bool IsCreateCreditNote { get; set; }
        public bool IsShowCreditNote { get; set; }

        public string StatementSub { get; set; }

        public string Specification { get; set; }

        public bool IsShowDelete { get; set; }

        public string OrgCountry { get; set; }
        public string DestCountry { get; set; }

        public string InvType { get; set; }
        public bool ShowEInvoice { get; set; }

        public byte[] Image { get; set; }

        public string IRNNo { get; set; }


        public DateTime? OrgStorageStart { get; set; }
        public DateTime? OrgStorageEnd { get; set; }
        public int? OrgStorageState { get; set; }
        public string OrgStorageStateNm { get; set; }

        public DateTime? DestStorageStart { get; set; }
        public DateTime? DestStorageEnd { get; set; }
        public int? DestStorageState { get; set; }
        public string DestStorageStateNm { get; set; }

        public string Remark { get; set; }
        public string RevenueBr { get; set; }
        public string Mode { get; set; }

        public string CreditNoteNo { get; set; }

        public string InvoiceNo { get; set; }

        public DateTime? InvoiceDate { get; set; }

        public DateTime? CreditNoteDate { get; set; }

        public int BillToAccountID { get; set; }

        public int BillToClientID { get; set; }

        public string OrgCity { get; set; }
        public string DestCity { get; set; }

        public string BillToID { get; set; }

        public string BillTo { get; set; }


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

        public OtherDetails Other { get; set; } = new OtherDetails();

        public string BaseCurrName { get; set; }
        public string RateCurrName { get; set; }

        public int? RateCurrancyID { get; set; }
        public int? BaseCurrID { get; set; }

        public decimal ConvRate { get; set; }


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

        public DateTime? PackDate { get; set; }
        public DateTime? LoadDate { get; set; }
        public DateTime? DeliveryDate { get; set; }

        public int? NoofPkgs { get; set; }
        public string BillAcknowledgement { get; set; }

        public DateTime? ParsifalAuditStartDate { get; set; }
        public DateTime? MoneyReceivedDate { get; set; }

        public decimal? AEDCurr { get; set; }

        public bool RMCBuss { get; set; }
        public string RMCType { get; set; }

        public string MainInvStatus { get; set; }

		
		
	}

    public class StorageBillDetails
    {
        public Int64 BillId { get; set; }

        public Int64? BillDetailId { get; set; }

        public Int64 StrgVolID { get; set; }

        public int VolumeUnitID { get; set; }
        public string VolumeUnit { get; set; }

        public decimal VolumeWt { get; set; }

        public DateTime? BillStartDate { get; set; }

        public DateTime? BillToDate { get; set; }

        public DateTime? OldBillToDate { get; set; }

        public DateTime DateFrom { get; set; }

        public DateTime? DateTo { get; set; }

        public decimal Amount { get; set; }

        public decimal ActualAmount { get; set; }

        public int CostHeadID { get; set; }

        public string CostHead { get; set; }

        public int BillFrqID { get; set; }

        public string BillFrq { get; set; }

        public string Particular { get; set; }

        public decimal? Tax_Percent { get; set; } = 0;

        public decimal? CGST { get; set; } = 0;

        public decimal? SGST { get; set; } = 0;

        public decimal? IGST { get; set; } = 0;

        public decimal? VAT { get; set; } = 0;

        public decimal? Total { get; set; } = 0;

        public string TaxType { get; set; }

        public string Description { get; set; }

        public int BillItemSeqID { get; set; }

        public decimal ConvRate { get; set; }

        public decimal ConvAmount { get; set; }

        public string SacCode { get; set; }

        public string POSID { get; set; }

        public string Component { get; set; }

        public int ComponentID { get; set; }
		public bool ShowDelete { get; set; }

		public bool IsActive { get; set; }

		public decimal? CommissionAmt { get; set; } = 0;
		public decimal? AuditAmt { get; set; } = 0;
		public decimal? AdminAmt { get; set; } = 0;

        public decimal? CostAmt { get; set; }
    }


    public class StorageBillGrid
    {

        public Int64 MoveID { get; set; }

        public Int64 StorageID { get; set; }

        public string JobNo { get; set; }

        public string Shipper { get; set; }

        public string Client { get; set; }

        public string Corporate { get; set; }

        public string ServiceLine { get; set; }

        public string Warehouse { get; set; }

        public DateTime? LastBillDate { get; set; }

        public string BillStatus { get; set; }

    }


    public class StorageSubBillGrid
    {
        public Int64 MoveID { get; set; }
        public Int64 ProcessID { get; set; }
		public Int64 BillID { get; set; }
		public Int64 StorageID { get; set; }
		public string InvNo { get; set; }
		public DateTime InvFromDate { get; set; }
        public DateTime? InvToDate { get; set; }
        public int? StatusID { get; set; }
        public string Status { get; set; }
        public string ApprovedBy { get; set; }
        public string FinalizedBy { get; set; }
        public string MainInvStatus { get; set; }
        public string MainInvNo { get; set; }
		public bool IsShowDlt { get; set; }
	}
}