using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RELOCBS.Entities
{
	public class Agent
	{

		public int AgentID { get; set; }

		public int AgentGroupID { get; set; }

		public string AgentGroupName { get; set; }

		public string AgentOrCorp { get; set; }

		public string AgentName { get; set; }

		public string ShortAgentName { get; set; }

		public string ContactPerson { get; set; }

		public string ContactPhone { get; set; }

		public string Address1 { get; set; }

		public string Address2 { get; set; }

		public string PINCode { get; set; }

		public int CityID { get; set; }

		public string CityName { get; set; }

		public int CompID { get; set; }

		public string CompanyName { get; set; }

		//public int BusinessLineID { get; set; }

		//public string BusinessLineName { get; set; }

		public bool Isactive { get; set; }

		public string GST { get; set; }

		public string Fin_AccountCode { get; set; }

		public DateTime CreatedDate { get; set; }

		public string ShortAddress1 { get; set; }
	}

	public class AgentViewModel
	{

		public int AgentID { get; set; }

		[Required(ErrorMessage = "Group is required")]
		public int? AgentGroupID { get; set; }

		[Display(Name = "Group")]
		public string AgentGroupName { get; set; }

		[Display(Name = "Agent")]
		[Required(ErrorMessage = "Name is required")]
		public string AgentName { get; set; }

		[Display(Name = "Short Name")]
		[Required(ErrorMessage = "Short Name is required")]
		public string ShortAgentName { get; set; }

		public string ContactPerson { get; set; }

		public string ContactPhone { get; set; }

		[Required(ErrorMessage = "Adderess 1 is required")]
		public string Address1 { get; set; }

		public string Address2 { get; set; }

		public string PinCode { get; set; }

		[Display(Name = "Email ID")]
		[EmailAddress(ErrorMessage = "Please enter valid email")]
		//[RegularExpression(@"[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}", ErrorMessage = "Please enter correct email")]
		public string EmailID { get; set; }

		[Required(ErrorMessage = "City is required")]
		public int CityID { get; set; }

		[Display(Name = "City")]
		public string CityName { get; set; }

		[Required(ErrorMessage = "Company is required")]
		public int CompID { get; set; }

		public string CompanyName { get; set; }

		//[Required(ErrorMessage = "BusinessLine is required")]
		//public int BusinessLineID { get; set; }

		//[Display(Name = "BusinessLine")]
		//public string BusinessLineName { get; set; }

		[Display(Name = "Type")]
		[Required(ErrorMessage = "Type is required")]
		public string AgentOrCorp { get; set; }

		[Display(Name = "GST")]
		//[Required(ErrorMessage = "GST is required")]
		public string GST { get; set; }

		public bool Isactive { get; set; }
		public bool? IsUseForRMC { get; set; }
		public bool? issez { get; set; }
		public bool? isonlyIGST { get; set; }

		public string VATNO { get; set; }

		[Display(Name = "AccountCode")]
		public string Fin_AccountCode { get; set; }

		public DateTime CreatedDate { get; set; }

		[Display(Name = "Writer Bank Details")]
		public int? DynamicBankID { get; set; }

        [Display(Name = "Vendor Code")]
        public string VendorCode { get; set; }

        public FinanceDetails FinanceDetails { get; set; }
    }

	public class AgentGroup
	{
		public int AgentGroupID { get; set; }

		[Display(Name = "Group")]
		[Required(ErrorMessage = "Group Name is required")]
		public string AgentGroupName { get; set; }

		//[Display(Name = "Short Name")]
		//[Required(ErrorMessage = "Short Name is required")]
		public string ShortAgentGroupName { get; set; }

		public string ContactPerson { get; set; }

		public string ContactPhone { get; set; }

		//[Required(ErrorMessage = "Adderess 1 is required")]
		public string Address1 { get; set; }

		public string Address2 { get; set; }

		public string PinCode { get; set; }


		//[Required(ErrorMessage = "City is required")]
		public int CityID { get; set; }

		[Display(Name = "City")]
		public string CityName { get; set; }

		[Display(Name = "Type")]
		[Required(ErrorMessage = "Type is required")]
		public string AgentOrCorp { get; set; }

		public int CompID { get; set; }

		public string CompanyName { get; set; }

		public bool Isactive { get; set; }

	}

	public class DynamicBankDetails
	{
		public int AgentID { get; set; }
		public List<BankList> bankList { get; set; } = new List<BankList>();
		//public int MyProperty { get; set; }
		//public int MyProperty { get; set; }
	}

	public class BankList
	{
		public string Header { get; set; }
		public string Value { get; set; }
	}

    public class FinanceDetails {
        public string Name { get; set; }
        public string Number { get; set; }
        public string Email { get; set; }
        public string ESC1_Name { get; set; }
        public string ESC1_Number { get; set; }
        public string ESC1_Email { get; set; }
        public string ESC2_Name { get; set; }
        public string ESC2_Number { get; set; }
        public string ESC2_Email { get; set; }
    }

}