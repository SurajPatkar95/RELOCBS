using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RELOCBS.Entities
{
    public class Claim
    {

        public Int64? Claim_ID { get; set; } = -1;
        
        //// Job related fields 
        public Int64 MoveID { get; set; }
        
        public string JobNo { get; set; }
        public DateTime JobDate { get; set; }

        public string Controller { get; set; }

        public int ControllerID { get; set; }

        public string ShipperName { get; set; }
        
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
        
        public DateTime? DeliveryDate { get; set; }
        //// Job related fields 
        
        //// Insurance related fields 
        public string InsuranceNo { get; set; }
        public DateTime? Insurance_Date { get; set; }
        public int InsuranceCompanyID { get; set; }
        public string InsuranceCompany { get; set; }

        public int Policy_No { get; set; }
        public string Policy_NoStr { get; set; }
        
        public Int64? P_A_No { get; set; }

        public decimal Sum_Insrd_Amt { get; set; }
        public decimal Sum_Insrd_Amt_Ex { get; set; }

        public decimal Prem_Paid { get; set; }
        public decimal Prem_Paid_Ex { get; set; }

        public decimal Prem_Recieved { get; set; }
        public decimal Prem_Recieved_Ex { get; set; }

        public decimal Total_InsuredAmout { get; set; }
        public decimal Total_InsuredAmout_Ex { get; set; }

        public decimal Total_PremiumPaid { get; set; }
        public decimal Total_PremiumPaid_Ex { get; set; }

        public decimal Total_BasePremAmt { get; set; }
        public decimal Total_BasePremAmt_Ex { get; set; }

        public string Pack_Superviser { get; set; }

        public string Delivery_Superviser { get; set; }
        //// Insurance related fields

        //// Claim related fields    
        public int BaseCurrencyID { get; set; }

        public int RateCurrencyID { get; set; }

        public decimal ExRate { get; set; }

        public DateTime? Int_Date { get; set; }
        public DateTime? Ackn_Date { get; set; }

        public DateTime? Claim_File_Date { get; set; }

        public string Docker_No { get; set; }

        public decimal Claim_Amt { get; set; }
        public decimal Claim_Amt_Ex { get; set; }

        public decimal ClaimAmt_Accepted_Shipper { get; set; }
        public decimal ClaimAmt_Accepted_Shipper_Ex { get; set; }

        public int PkgsPacked { get; set; }

        public int PkgsDamaged { get; set; }

        public decimal OtherExp { get; set; }
        public decimal? OtherExp_Ex { get; set; }

        public decimal Summary_Comp_payout { get; set; }
        public decimal? Summary_Comp_payout_Ex { get; set; }

        public string RemarksForOtherExp { get; set; }

        public decimal Ins_Claim_Amt { get; set; }

        public decimal Ins_Claim_Amt_Ex { get; set; }

        public int Ins_BaseCurr { get; set; }

        public int Ins_RateCurr { get; set; }

        public decimal Ins_ConverRate { get; set; }


        public decimal InsRoute { get; set; }
        public decimal InsRoute_Ex { get; set; }

        public decimal CompPaidAmt { get; set; }
        public decimal CompPaidAmt_Ex { get; set; }

        public decimal compPaidAdditionalAmt { get; set; }
        public decimal compPaidAdditionalAmt_Ex { get; set; }

        public DateTime? SurveyDate { get; set; }

        public decimal? SurveyAmt { get; set; }
        public decimal? SurveyAmt_Ex { get; set; }

        public DateTime? ClaimSettledDate { get; set; }

        public decimal? ClaimSettledAmt { get; set; }

        public string PayMode { get; set; }

        public int ClaimStatusID { get; set; }

        public DateTime? VoucherDate { get; set; }

        public string ChqNumber { get; set; }

        public DateTime? DocRecdDate { get; set; }

        public string ChqToName { get; set; }

        public string ChqStatus { get; set; }

        public string settlementType { get; set; }

        public DateTime? ClaimFormRecdDate { get; set; }

        public string ClaimFileRemarks { get; set; }

        public string InstToFinance { get; set; }

        public string InsRef { get; set; }

        public decimal? VendorPaid { get; set; }
        public decimal? VendorPaid_Ex { get; set; }

        public List<ClaimDetails> details { get; set; } = new List<ClaimDetails>();

        public string HFdetails { get; set; }

        public string HFDocs { get; set; }

        public DocUpload docUpload { get; set; } = new DocUpload();

        public EmailConfig Email { get; set; } = new EmailConfig();

        public bool IsSubmitToFinance { get; set; }

        public bool IsFinanceRole { get; set; }

        public bool IsApproved { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }

        public string SubmitToFinanceBy { get; set; }
        public DateTime? SubmitToFinanceDate { get; set; }

        public string ApprovedBy { get; set; }
        public DateTime? ApprovedDate { get; set; }

        public string Status { get; set; }

    }

    public class ClaimDetails
    {
        public Int64 ClaimItemDetailIJobID { get; set; }

        public int ClaimItemDetailIsID { get; set; }
        public string ClaimItemDetailsName { get; set; }
        
        public int ClaimNatureID { get; set; }
        public string ClaimNature { get; set; }

        public int ClaimItemCategoryId { get; set; }
        public string ClaimCategoryName { get; set; }
        
        public int NumberOfItem { get; set; }

        public string Remarks { get; set; }

    }

    public class ClaimGrid
    {
        public Int64 Claim_ID { get; set; }

        public Int64 MoveID { get; set; }

        public string ClaimNo { get; set; }
        public DateTime Claim_Date { get; set; }

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

        public decimal ClaimAmt { get; set; }

        public string Status { get; set; }

    }

    public class ClaimPrint
    {

        public string DocketNo { get; set; }

        public DateTime? ClaimDate { get; set; }

        public string JobNo { get; set; }

        public string Shipper { get; set; }

        public string Corporate { get; set; }

        public string ServiceLine { get; set; }

        public Decimal Amount_Insured { get; set; }

        public Decimal Premium_Recieved { get; set; }

        public Decimal Premium_Paid { get; set; }

        public Decimal Claim_Amount { get; set; }

        public Decimal Claim_Settled { get; set; }
        
        public Decimal Writer_Amount { get; set; }

        public Decimal Writer_ExAmount { get; set; }

        public decimal Total_Revenue { get; set; }
        public decimal Total_Estimated { get; set; }
        public decimal Gross_Profit { get; set; }
        public decimal Total { get; set; }

        public string Ins_Ref { get; set; }

        public string Outsource { get; set; }

        public string TypeSettlement { get; set; }

        public string ChequePayee { get; set; }

        public string NoteToFinance { get; set; }

        public string PreparedBy { get; set; }

        public string VerifiedBy { get; set; }

        public string AuthorisedBy { get; set; }
    }

    public class EClaimPrint
    {
        public string InsuredName { get; set; }

        public string PolicyNo { get; set; }

        public DateTime DateTimeofLoss { get; set; }

        public string LossLocation { get; set; }

        public string LossDescription { get; set; }

        public string Contact_Person { get; set; }

        public string ContactNo { get; set; }

        public string Contact_Mob { get; set; }

        public string Contact_Off { get; set; }

        public string EstimatedLossAmount { get; set; }

        public string LRNo { get; set; }

        public string CarrierName { get; set; }

        public string InvoiceNo { get; set; }

        public string Transit { get; set; }

        public string SenderName { get; set; }

        public string SenderContactNo { get; set; }

        public string SenderContactNo_Mob { get; set; }
        public string SenderContactNo_Off { get; set; }
        public string SenderEmail { get; set; }

        public string Email_notification  { get; set; }
        public string Toll_Free  { get; set; }

        public string FormatType { get; set; }

    }
}