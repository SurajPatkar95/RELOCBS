using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RELOCBS.Entities
{
    public class DebitNote
    {
        public Int64 DebitNoteId { get; set; }
        public Int64 DNCreditNoteId { get; set; }
        public string DebitNoteNo { get; set; }
        public string CreditNoteNo { get; set; }
        public bool IsCreateCreditNote { get; set; }
        public bool IsShowCreditNote { get; set; }

        public string IRNNo { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "SBU is required.")]
        public int? SBUId { get; set; }
        public string SBU { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Revenue branch is required.")]
        public int? RevenueBrIdHidden { get; set; }
        public int? RevenueBrId { get; set; }
        public string RevenueBr { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Debit Note Type is required.")]
        public int? DNTypeId { get; set; }
        public string DNType { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Currency is required.")]
        public int? CurrencyID { get; set; }
        public string Currency { get; set; }

        public Debtor Debtor { get; set; } = new Debtor();

        public string TaxType { get; set; }
        public string InvType { get; set; }
        public int? TaxRateId { get; set; }
        public decimal? TaxRate { get; set; }
        public string POSCountry { get; set; }

        public bool? IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ApprovedBy { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public bool? IsDeleted { get; set; }
        public string DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public DateTime? DebitNoteDate { get; set; }
        public DateTime? CreditNoteDate { get; set; }
        public string DebitNoteStatus { get; set; }
        public string CreditNoteStatus { get; set; }
        public DebitNoteDetails DebitNoteDetails { get; set; } = new DebitNoteDetails();
        public List<DebitNoteDetails> DebitNoteDetailsList { get; set; } = new List<DebitNoteDetails>();
        public AddressDetails AddressDetails { get; set; } = new AddressDetails();

        [Required(AllowEmptyStrings = false, ErrorMessage = "Debit note details are required.")]
        public string DebitNoteDetailsListHidden { get; set; }
        public string DrOrCrNote { get; set; }

        public byte[] QRSignedValue { get; set; }
        public byte[] BankQRCode { get; set; }
    }

    public class DebitNoteDetails
    {
        public Int64 DebitNoteDetailId { get; set; }
        public Int64? DebitNoteId { get; set; }
        public Int64? DNCreditNoteDetailId { get; set; }
        public Int64? DNCreditNoteId { get; set; }
        public int? DNCostHeadId { get; set; }
        public string CostHead { get; set; }
        public string Description { get; set; }
        public string JobNo { get; set; }
        public string SacCode { get; set; }
        public int? CurrencyID { get; set; }
        public string Currency { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? Rate { get; set; }
        public int? UnitId { get; set; }
        public string Unit { get; set; }
        public decimal? DebitAmount { get; set; }
        public decimal? CreditAmount { get; set; }
        public decimal? MaxCreditAmount { get; set; }
        public decimal? TaxPercent { get; set; }
        public decimal? SGSTAmount { get; set; }
        public decimal? CGSTAmount { get; set; }
        public decimal? IGSTAmount { get; set; }
        public decimal? VATAmount { get; set; }
        public decimal? TotalAmount { get; set; }
        public bool? IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }

    public class Debtor
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please select Debtor.")]
        public int? DebtorId { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Place of supply is required.")]
        public int? POSStateId { get; set; }
        public string POSState { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Debtor name is required.")]
        public string DebtorName { get; set; }

        [MaxLength(200, ErrorMessage = "Maximum 200 charachter.")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Address is required.")]
        public string Address1 { get; set; }

        [MaxLength(200, ErrorMessage = "Maximum 200 charachter.")]
        public string Address2 { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "City is required.")]
        public int? CityIDHidden { get; set; }
        public int? CityID { get; set; }
        public string City { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "PIN code is required.")]
        public string PINCode { get; set; }
        public string State { get; set; }
        public string StateCode { get; set; }

        public string GSTNo { get; set; }

        //[Required(AllowEmptyStrings = false, ErrorMessage = "PAN number is required.")]
        [RegularExpression("[A-Za-z]{5}[0-9]{4}[A-Za-z]{1}", ErrorMessage = "PAN number is invalid.")]
        public string PANNo { get; set; }
        public bool? IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }

    public class AddressDetails
    {
        //BillBottomlines
        public string GSTNoOur { get; set; }
        public string PANNoOur { get; set; }
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
    }

    public class DNFundTranfer
    {
        public string BillNo { get; set; }
        public DateTime FromDate { get; set; } = DateTime.Now.AddMonths(-1);
        public DateTime ToDate { get; set; } = DateTime.Now;
        public string Status { get; set; }
        public int[] RevenueBranchId { get; set; }
        public List<DNInvoice> DNInvoiceList { get; set; } = new List<DNInvoice>();
    }

    public class DNInvoice
    {
        public string Layout { get; set; }
        public string JobNo { get; set; }
        public string BillNo { get; set; }
        public DateTime? BillDate { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerReference { get; set; }
        public string SalesDefinition { get; set; }
        public string Comment { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string LineNo { get; set; }
        public string ItemCode { get; set; }
        public string Amount { get; set; }
        public string Description { get; set; }
        public string UnitofSale { get; set; }
        public string Qty { get; set; }
        public string Rate { get; set; }
        public string Currency { get; set; }
        public string Value { get; set; }
        public string IGST { get; set; }
        public string CGST { get; set; }
        public string SGST { get; set; }
        public string BussLine { get; set; }
        public string Location { get; set; }
        public string Project { get; set; }
        public string Miscellaneous { get; set; }
        public string TaxCode { get; set; }
        public string MISLocation { get; set; }
        public string Employee { get; set; }
        public string Account { get; set; }
        public string AccountCode { get; set; }
        public string FAClientCode { get; set; }
        public string FACode { get; set; }
        public bool? IsExport { get; set; }
        public string CBSRefID { get; set; }
        public string DebitOrCredit { get; set; }
        public string DebtorName { get; set; }
        public string Credit_Debit_Marker { get; set; }
        public string GSTFlag { get; set; }
    }
}