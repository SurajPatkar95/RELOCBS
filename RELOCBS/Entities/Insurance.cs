using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RELOCBS.Entities
{
    public class InsuranceViewModel
    {

        public Int64? Insurance_ID { get; set; } = -1;

        public Int64 MoveID { get; set; }

        public string InsuranceNo { get; set; }
        public DateTime Insurance_Date { get; set; }

        public string JobNo { get; set; }
        public DateTime JobDate { get; set; }

        public string Controller { get; set; }

        public int ControllerID { get; set; }

        public string ShipperName { get; set; }
        
        public string ShipperAddress1 { get; set; }

        public string ShipperAddress2 { get; set; }

        public int ShipperCityID { get; set; }

        public string ShipperPin { get; set; }

        public string ShipperMobile { get; set; }

        public string ShipperPhone { get; set; }

        public string Corporate { get; set; }
        public int CorporateID { get; set; }

        public string Client { get; set; }
        public int ClientID { get; set; }

        public string ServiceLine { get; set; }
        
        public string OrgBranch { get; set; }
        public int OrgBranchID { get; set; }

        public string OrgCity { get; set; }

        public int OrgCityID { get; set; }

        public string DestCity { get; set; }

        public int DestCityID { get; set; }

        public string OrgAgent { get; set; }

        public string DestAgent { get; set; }

        public string Mode { get; set; }

        public string Pack_Superviser { get; set; }

        public DateTime? Pac_Disp_Date { get; set; }

        public int BaseCurrencyID { get; set; }

        public int RateCurrencyID { get; set; }

        public decimal ExRate { get; set; }

        public decimal Open_Prem_Amt { get; set; }

        public decimal Open_SI_Amt { get; set; }
        
        public int InsuranceCompanyID { get; set; }
        public string InsuranceCompany { get; set; }

        public int Policy_No { get; set; }
        public string Policy_NoStr { get; set; }

        public string CertNo { get; set; }

        public string P_A_No { get; set; }

        public decimal Sum_Insrd_Amt { get; set; }
        public int Sum_Insrd_Amt_ExCurr { get; set; }
        public decimal Sum_Insrd_Amt_ExRate { get; set; }
        public decimal Sum_Insrd_Amt_Ex { get; set; }

        public Decimal Shp_Prem_Percent { get; set; }
        public decimal Shp_Prem_Amt { get; set; }
        public int Shp_Prem_Amt_ExCurr { get; set; }
        public decimal Shp_Prem_Amt_ExRate { get; set; }
        public decimal Shp_Prem_Amt_Ex { get; set; }

        public decimal Basic_Prem_Paid { get; set; }
        public int Basic_Prem_Paid_ExCurr { get; set; }
        public decimal Basic_Prem_Paid_ExRate { get; set; }
        public decimal Basic_Prem_Paid_Ex { get; set; }

        public decimal Service_Tax_Paid { get; set; }
        public int Service_Tax_Paid_ExCurr { get; set; }
        public decimal Service_Tax_Paid_ExRate { get; set; }
        public decimal Service_Tax_Paid_Ex { get; set; }

        public decimal Stamp_Duty_Paid { get; set; }
        public int Stamp_Duty_Paid_ExCurr { get; set; }
        public decimal Stamp_Duty_Paid_ExRate { get; set; }
        public decimal Stamp_Duty_Paid_Ex { get; set; }

        public decimal Total_Prem_Paid { get; set; }
        public int Total_Prem_Paid_ExCurr { get; set; }
        public decimal Total_Prem_Paid_ExRate { get; set; }
        public decimal Total_Prem_Paid_Ex { get; set; }

        public decimal Bal_Prem_Amt { get; set; }

        public decimal Bal_SI { get; set; }

        public int Status { get; set; }
		public int? InsDelayReason { get; set; }

		public string StatusRemark { get; set; }

        public bool IsCoverNote { get; set; } = false;

        [Display(Name = "CoverNote File")]
        //[RegularExpression(@"([a-zA-Z0-9\s_\\.\-:])+(.DOC|.doc|.DOCX|.docx|.pdf|.PDF)$", ErrorMessage = "Incorrect file format(.pdf,.doc,.docx only)")]
        public HttpPostedFileBase CoverNoteFile { get; set; }

        public int? FileID { get; set; }

        public decimal BalPremPercent { get; set; }

        public int TATinHrs { get; set; }

        public string JobInsCreatedBy { get; set; }
    }

    public class InsuranceGrid
    {
        public Int64 Insurance_ID { get; set; }

        public Int64 MoveID { get; set; }

        public string InsuranceNo { get; set; }
        public DateTime Insurance_Date { get; set; }

        public string JobNo { get; set; }
        public DateTime JobDate { get; set; }

        public string Controller { get; set; }

        public int ControllerID { get; set; }

        public string ShipperName { get; set; }

        public string ShipperAddress1 { get; set; }

        public string ShipperAddress2 { get; set; }

        public int ShipperCityID { get; set; }

        public string ShipperPin { get; set; }

        public string ShipperMobile { get; set; }

        public string ShipperPhone { get; set; }

        public string Corporate { get; set; }
        public int CorporateID { get; set; }

        public string Client { get; set; }
        public int ClientID { get; set; }

        public string ServiceLine { get; set; }

        public string OrgBranch { get; set; }
        public int OrgBranchID { get; set; }

        public string OrgCity { get; set; }

        public int OrgCityID { get; set; }

        public string DestCity { get; set; }

        public int DestCityID { get; set; }

        public string Mode { get; set; }
        
        public string Shipper { get; set; }

        public string InsuranceCompany { get; set; }

        public string PolicyNo { get; set; }

        public string PackSupervisorName { get; set; }

        public string ControllerName { get; set; }

        public decimal InsuredAmount { get; set; }

    }

    public class Insurance_Master
    {
        public int? Ins_M_Id { get; set; }
        
        public string Ins_Name { get; set; }
        
        [Required]
        [Display(Name = "Ins Nm")]
        public int InsComp_ID { get; set; }

        [Required]
        [Display(Name = "Pol No")]
        public string Policy_No { get; set; }

        [Required]
        [Display(Name = "Pol Dt")]
        public DateTime? Policy_Date { get; set; }

        [Required]
        [Display(Name = "Prem %")]
        public decimal? Prem_Percent_Amt { get; set; }

        [Required]
        [Display(Name = "GST %")]
        public decimal? Service_Tax { get; set; }

        [Required]
        [Display(Name = "Stmp Dty")]
        public decimal? Stamp_Duty { get; set; }

        [Display(Name = "Min. Prem")]
        public decimal? Min_Prem { get; set; }

        
        [Display(Name = "Bal SI")]
        public decimal? Bal_SI { get; set; }

        [Display(Name = "Bal Prem")]
        public decimal? Bal_Prem { get; set; }

        [Required]
        [Display(Name = "Sum Ins")]
        public decimal? Sum_Ins { get; set; }

        
        [Display(Name = "Premium")]
        public decimal? Premium_Amt { get; set; }

        [Required]
        [Display(Name = "Cheq Date")]
        public DateTime? Cheq_Date { get; set; }

        [Required]
        [Display(Name = "Cheq No.")]
        public string Cheq_No { get; set; }

        [Display(Name = "IsActive")]
        public bool IsActive { get; set; }

        [Display(Name = "Policy doc.")]
        public HttpPostedFileBase PostedFile { get; set; }

        public int? FileID { get; set; }

        public int CompID { get; set; }
        public string CompanyName { get; set; }

    }

    public class InsuranceAmoutDTO
    {
        public decimal Open_Prem_Amt { get; set; }

        public decimal Open_SI_Amt { get; set; }

        public decimal Sum_Insrd_Amt { get; set; }

        public decimal Basic_Prem_Paid { get; set; }

        public decimal Service_Tax_Paid { get; set; }

        public decimal Stamp_Duty_Paid { get; set; }

        public decimal Total_Prem_Paid { get; set; }

        public decimal Bal_Prem_Amt { get; set; }

        public decimal Bal_SI { get; set; }

    }

    public class InsurancePrint
    {

        public string JobNo { get; set; }

        public string PA_NO { get; set; }

        public string InsCompany { get; set; }

        public DateTime PrintDate { get; set; }

        public string Controller { get; set; }

        public string Description { get; set; }

        public string ShipperName { get; set; }

        public string From_To_City { get; set; }

        public string Sum_Ins_Amt { get; set; }

        public DateTime? Pack_Disp_Date { get; set; }

        public string IsCoverNote { get; set; }

        public string CoverNote { get; set; }
    }
}