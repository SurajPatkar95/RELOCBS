using System;
using System.Collections.Generic;

namespace RELOCBS.Entities
{
    public class WOSBilling : Billing
    {
        public Int64? WOSMoveID { get; set; }

        public int? RevBranchID { get; set; }
        public string RevBranch { get; set; }

        public int? JobBranchID { get; set; }
        public string JobBranch { get; set; }

        public int? SDId { get; set; }
        public string SDName { get; set; }

        public int? SRId { get; set; }
        public string SRName { get; set; }

        public int? KAM { get; set; }
        public string KAMName { get; set; }

        public new int RMCID { get; set; }
        public string RMC { get; set; }

        public int? RevenueCurrID { get; set; }
        public string RevenueCurr { get; set; }

        public int? CostCurrID { get; set; }
        public string CostCurr { get; set; }
        public string FinalApprovedBy { get; set; }
        public new DateTime? PreparedDate { get; set; }
        public new DateTime? ApprovedDate { get; set; }
        public DateTime? FinalApprovedDate { get; set; }

        public WOSCustomer WOSCustomer { get; set; } = new WOSCustomer();
        public Dictionary<string, string> ArabicData { get; set; }
        public Dictionary<string, string> ArabicCurrData { get; set; }
    }

    public class WOSFundTranfer
    {
        public string BillNo { get; set; }
        public DateTime FromDate { get; set; } = DateTime.Now.AddMonths(-1);
        public DateTime ToDate { get; set; } = DateTime.Now;
        public string Status { get; set; }
        public int[] ServiceLineId { get; set; }
        public int[] RevenueBranchId { get; set; }
        public List<WOSInvoice> WOSInvoiceList { get; set; } = new List<WOSInvoice>();
        public string SearchFor { get; set; }
        public List<WOSInvoice> InvGrid { get; set; } = new List<WOSInvoice>();
    }

    public class WOSInvoice
    {
        public string Layout { get; set; }
        public string JobNo { get; set; }
        public string BillNo { get; set; }
        public DateTime? BillDate { get; set; }
        //public string CustomerCode { get; set; }
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
        //public string BussLine { get; set; }
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
        public string InvOrCredit { get; set; }
        public string BillTo { get; set; }
        public string Credit_Debit_Marker { get; set; }
        public string GSTFlag { get; set; }
    }

    
}