using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RELOCBS.Entities
{
	public class TransClearTax
	{
		public int AppID { get; set; }
		public DateTime? FromDate { get; set; } = null;
		public DateTime? ToDate { get; set; } = null;
		public bool FileTransfer { get; set; }
		public string InvNo { get; set; }
		public List<InvoiceList> InvoiceList { get; set; } = new List<InvoiceList>();
	}

	public class InvoiceList
	{
		public string InvNo { get; set; }
		public DateTime InvDate { get; set; }
		public string InvType { get; set; }
		public string POS { get; set; }
		public string POSStatus { get; set; }
		public bool ShowEinvoice { get; set; }
		public string GSTType { get; set; }
	}
}