using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RELOCBS.Entities
{
    public class CreditLimitEntity
    {
        public int CreditLimitEntityID { get; set; }
        [Display(Name ="Corporate")]
        [Required(ErrorMessage = "Corporate required")]
        public int CorporateID { get; set; }

        [Range(0, 9999999999999999.99, ErrorMessage = "Invalid Amount; Max 18 digits")]
        public decimal? Turnover_Amt { get; set; }
        //[Required(ErrorMessage = "Head Quarters required")]
        public string Address { get; set; }
        public int CityId { get; set; }

        public string GSTIN_No { get; set; }

        [Required(ErrorMessage = "Contact Name required")]
        public string Cust_Contact_Name { get; set; }

        [Required(ErrorMessage = "Contact Number required")]
        [MaxLength(20,ErrorMessage ="20 Characters allowed")]
        [RegularExpression(@"^[0-9\.\-\/\#\(\)\+\ ]+$",ErrorMessage = "Invalid Character")]
        public string Cust_Contact_Number { get; set; }

        [Required(ErrorMessage = "Contact Email required")]
        [EmailAddress(ErrorMessage = "Please enter valid email")]
        public string Cust_Contact_Email { get; set; }
        [Required(ErrorMessage = "Contact Designation required")]
        public string Cust_Contact_Designation { get; set; }
        
        public string CreatedByName { get; set; }
        public DateTime CreatedDate { get; set; }

        public bool IsActive { get; set; }

        public List<Buss_Dev_Feedback> buss_Dev { get; set; } = new List<Buss_Dev_Feedback>();
        //public List<CreditEntityPeriods> creditPeriods { get; set; } = new List<CreditEntityPeriods>();
        public CustApprovalUpload CustApprovalUpload { get; set; } = new CustApprovalUpload();
        //public List<CustApprovalFiles> Approvalfiles { get; set; } = new List<CustApprovalFiles>();

        public EntityClientMap ClientMap { get; set; } = new EntityClientMap();
        //Tablist as dynamic 
        public List<TabList> TabList { get; set; } = new List<TabList>();
        public int TabIndex { get; set; }

        public string HFCredits { get; set; }
        //public string HFPeriods { get; set; }

        public float FromAmount { get; set; }
        public float ToAmount { get; set; }
        public bool IsApprover { get; set; }

        public string ApproveRemark { get; set; }
        public string ApproveByName { get; set; }
        public DateTime? ApproveDate { get; set; }

        public string Status { get; set; }
        public int StatusID { get; set; }

        public string ApproveType { get; set; }
        public int? SendApprovalToEmpID { get; set; }
        public float TotalCreditLimit { get; set; } = 0;
        public string TotalCreditLimitDisplay { get; set; } = "0";

        public int CompID { get; set; }
        public bool IsRMC { get; set; }

        public bool FromMail { get; set; } = false;

        [Required(ErrorMessage = "Validity Date required")]
        public DateTime? EffectiveTo { get; set; }

    }

    public class CustApprovalUpload
    {
        public Int64 Buss_Dev_FeedbackID { get; set; }
        //[Required(ErrorMessage = "Customer Approval Type required")]
        public string ApprovalType { get; set; }
        public String DocDescription { get; set; }
        //[Required(ErrorMessage = "File required")]
        public HttpPostedFileBase[] ApprovalFile { get; set; }
    }

    public class EntityClientMap
    {
        public int CreditLimitEntityID { get; set; }
        public string Remark { get; set; }
        public IEnumerable<SelectListItem> MappedClientNameList { get; set; }
        public IEnumerable<SelectListItem> UnmapClientNameList { get; set; }
        public Int64[] MappedClientList { get; set; }
        public Int64[] UnMappedClientList { get; set; }

    }

    public class CustApprovalFiles
    {
        public Int64 Buss_Dev_FeedbackID { get; set; }
        public Int64 FileID { get; set; }
        public string ApprovalType { get; set; }
        public String DocDescription { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
    }

    public class CreditLimitCompCategory
    {
        public int CompCategoryID { get; set; }
        public string CompCategoryName { get; set; }
    }

    public class Buss_Dev_Feedback
    {
        public int rowIndex { get; set; }
        public Int64 Buss_Dev_FeedbackID { get; set; }
        public int CreditLimitEntityID { get; set; }
        public string Project { get; set; }
        public int ProjectID { get; set; }
        public string ServiceLine { get; set; }
        public int ServiceLineID { get; set; }

        
        public float Credit_Amount { get; set; }
        public string Credit_Amount_Display { get; set; }
        public int CurrID { get; set; }
        public string CurrencyName { get; set; }
        public string BillingInstructions_Remark { get; set; }
        public bool IsActive { get; set; }

        //public string CreatedByName { get; set; }
        //public DateTime CreatedDate { get; set; }
        [Range(0, 100, ErrorMessage = "Invalid Percentage")]
        public float? Adv_Percent { get; set; }
        public int? CreditDays { get; set; }
        public int? Credit_period_basisID { get; set; }
        public string Credit_period_basis { get; set; }
    }
    
    public  class CreditEntityPeriods
    {
        public Int64? Credit_Entity_PeriodID { get; set; }
        public int CreditLimitEntityID { get; set; }
        public string Project { get; set; }
        public int ProjectID { get; set; }
        public string ServiceLine { get; set; }
        public int ServiceLineID { get; set; }
        public int Credit_days { get; set; }
        public string Credit_period_basis { get; set; }
        public int Credit_period_basisID { get; set; }
        public bool IsActive { get; set; }
    }

    public class CreditApprovalGrid
    {
        public int CreditLimitEntityID { get; set; }
        public string CorporateName { get; set; }
        public float TurnoverAmt { get; set; }
        public string Addresss { get; set; }
        public string City { get; set; }
        public string Cust_Contact_Name { get; set; }
        public string Cust_Contact_Number { get; set; }
        public string Cust_Contact_Email { get; set; }
        public string Cust_Contact_Designation { get; set; }
        public bool IsActive { get; set; }
        public string Status { get; set; }


    }

    public class CreditPeriodRootObject
    {
        public List<CreditEntityPeriods> periods { get; set; }
    }

    public class CreditRootObject
    {
        public List<Buss_Dev_Feedback> buss_Devs { get; set; }
    }

    public class CFAPrint
    {
        public int CreditLimitEntityID { get; set; }
        public string CorporateName { get; set; }
        
        public decimal? Turnover_Amt { get; set; }
       
        public string Address { get; set; }
        public string CityName { get; set; }

        public string GSTIN_No { get; set; }
        
        public string Cust_Contact_Name { get; set; }
        
        public string Cust_Contact_Number { get; set; }

        public string Cust_Contact_Email { get; set; }
        
        public string Cust_Contact_Designation { get; set; }

        public string CreatedByName { get; set; }
        public DateTime CreatedDate { get; set; }
        
        public List<Buss_Dev_Feedback> buss_Dev { get; set; } = new List<Buss_Dev_Feedback>();
        
        public CFAClientMap Clients { get; set; } = new CFAClientMap();
        
        public string ApproveRemark { get; set; }
        public string ApproveByName { get; set; }
        public DateTime? ApproveDate { get; set; }

        public string Status { get; set; }
        
        public string ApproveType { get; set; }
        public string SendApprovalToEmpNaame { get; set; }
        public string TotalCreditLimit { get; set; } = "0";
        public DateTime EffectiveTo { get; set; }

    }

    public class CFAClientMap
    {
        public string[] CorporateName { get; set; }
        public string Remark { get; set; }
        public int CreditLimitEntityID { get; set; }
    }

}