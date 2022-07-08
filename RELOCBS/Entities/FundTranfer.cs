using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RELOCBS.Entities
{
	public class FundTranfer
	{
		public string BillNo { get; set; }
		public DateTime FromDate { get; set; } = DateTime.Now;
		public DateTime ToDate { get; set; } = DateTime.Now;
		public string SearchFor { get; set; }
		public int[] ServiceLineId { get; set; }
		public int[] RevenueBranchId { get; set; }
		public List<InvoiceGrid> InvGrid { get; set; } = new List<InvoiceGrid>();
	}

	public class InvoiceGrid
	{
		public string Layout { get; set; }
		public string JobNo { get; set; }
		public string BillNo { get; set; }
		public DateTime BillDate { get; set; }
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
		public bool IsExport { get; set; }
		public string CBSRefID { get; set; }
		public string InvOrCredit { get; set; }
		public string BillTo { get; set; }
        public string Credit_Debit_Marker { get; set; }
		public string GSTFlag { get; set; }
		
	}

    public class FundTranfer_GCCOther:FundTranfer
    {
        public int CompanyId { get; set; }
        public string Type { get; set; }
    }

    


}