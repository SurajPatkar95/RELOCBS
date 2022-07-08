using RELOCBS.Common.ExceptionHandling;
using RELOCBS.DAL.Claims;
using RELOCBS.Entities;
using RELOCBS.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Linq.Dynamic;
using System.Data;
using System.Net.Mail;
using System.Net;
using System.IO;

namespace RELOCBS.BL.Claims
{
    public class ClaimsBL
    {
        private ClaimsDAL _claimDAL;

        public ClaimsDAL claimDAL
        {

            get
            {
                if (this._claimDAL == null)
                    this._claimDAL = new ClaimsDAL();
                return this._claimDAL;
            }
        }


        public IEnumerable<Entities.ClaimGrid> GetClaimGrid(DateTime? FromDate, DateTime? Todate, bool IsJobDate, bool IsClaimDate, string JobNo, Int64 Claim_Id, string sort, string sortdir, int skip, int pageSize, out int totalCount,out int IsFinanceUser)
        {
            totalCount = 0;

            try
            {
                bool RMCBuss = false;
                if (UserSession.GetUserSession().BussinessLine == "NON RMC-BUSINESS")
                {
                    RMCBuss = false;
                }
                else
                {
                    RMCBuss = true;
                }
                IQueryable<Entities.ClaimGrid> ClaimGrid = claimDAL.GetClaimGrid(FromDate, Todate, IsJobDate, IsClaimDate, JobNo, Claim_Id, RMCBuss, UserSession.GetUserSession().CompanyID,out IsFinanceUser);
                if (ClaimGrid != null)
                {
                    totalCount = ClaimGrid.Count();

                    if (pageSize > 1)
                    {
                        ClaimGrid = ClaimGrid.Skip((skip * (pageSize - 1))).Take(skip);
                    }
                    else
                    {
                        ClaimGrid = ClaimGrid.Take(skip);
                    }

                    ClaimGrid = ClaimGrid.OrderBy(sort + " " + sortdir);

                    return ClaimGrid.ToList();
                }
                else
                {
                    return new List<Entities.ClaimGrid>();
                }
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "ClaimBL", "GetDetailById", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

        }

        public Claim GetClaimDetails(Int64 MoveID, Int64 Claim_ID = 0)
        {
            Claim model = new Claim();

            try
            {
                DataSet data = claimDAL.GetClaimDetails(UserSession.GetUserSession().LoginID, MoveID, Claim_ID);

                if (data != null && data.Tables.Count > 0)
                {
                    if (data.Tables.Count > 0 && data.Tables[0] != null && data.Tables[0].Rows.Count > 0)
                    {
                        model = (from item in data.Tables[0].AsEnumerable()
                                 select new Claim()
                                 {
                                     MoveID = Convert.ToInt64(item["MoveID"]),
                                     Claim_ID = -1,
                                     JobNo = Convert.ToString(item["JobID"]),
                                     //JobDate = Convert.ToDateTime(item["JobNo"]),
                                     ServiceLine = Convert.ToString(item["ServiceLine"]),
                                     OrgCity = Convert.ToString(item["FromCity"]),
                                     DestCity = Convert.ToString(item["ToCity"]),
                                     OrgAgent = Convert.ToString(item["OrgAgentName"]),
                                     DestAgent = Convert.ToString(item["DestAgentName"]),
                                     ShipperName = Convert.ToString(item["ShipperName"]),
                                     Client = Convert.ToString(item["ClientName"]),
                                     Corporate = Convert.ToString(item["CorporateName"]),
                                     Mode = Convert.ToString(item["TransportMode"]),
                                     DeliveryDate = !string.IsNullOrWhiteSpace(Convert.ToString(item["Deliverydate"]))? Convert.ToDateTime(item["Deliverydate"]) :(DateTime?)null ,
                                     ControllerID = !string.IsNullOrWhiteSpace(Convert.ToString(item["BussinessLineID"])) ? Convert.ToInt32(item["BussinessLineID"]) : 0,
                                     Controller =Convert.ToString(item["BussLineName"])
                    }).FirstOrDefault();

                        if (data.Tables.Count > 1 && data.Tables[1] != null && data.Tables[1].Rows.Count > 0)
                        {

                            DataRow dataRow = data.Tables[1].Rows[0];
                            model.Claim_ID = Convert.ToInt64(dataRow["ClaimDetailIForJobID"]);
                            model.Ackn_Date = !string.IsNullOrWhiteSpace(Convert.ToString(dataRow["AckDate"])) ? Convert.ToDateTime(dataRow["AckDate"]) : (DateTime?)null;
                            model.Int_Date = !string.IsNullOrWhiteSpace(Convert.ToString(dataRow["IntDate"])) ? Convert.ToDateTime(dataRow["IntDate"]) : (DateTime?)null;
                            model.Claim_File_Date = !string.IsNullOrWhiteSpace(Convert.ToString(dataRow["ClaimFileDate"])) ? Convert.ToDateTime(dataRow["ClaimFileDate"]) : (DateTime?)null;
                            model.Claim_Amt = !string.IsNullOrWhiteSpace(Convert.ToString(dataRow["ClaimAmt"])) ? Convert.ToDecimal(dataRow["ClaimAmt"]) : 0;
                            model.Claim_Amt_Ex = !string.IsNullOrWhiteSpace(Convert.ToString(dataRow["ClaimAmt_Ex"])) ? Convert.ToDecimal(dataRow["ClaimAmt_Ex"]) : 0;
                            model.BaseCurrencyID = !string.IsNullOrWhiteSpace(Convert.ToString(dataRow["BaseCurrID"])) ? Convert.ToInt32(dataRow["BaseCurrID"]) : 0;
                            model.RateCurrencyID = !string.IsNullOrWhiteSpace(Convert.ToString(dataRow["RateCurrID"])) ? Convert.ToInt32(dataRow["RateCurrID"]) : 0;
                            model.ExRate = !string.IsNullOrWhiteSpace(Convert.ToString(dataRow["ConverRate"])) ? Convert.ToDecimal(dataRow["ConverRate"]) : 0;
                            model.PkgsPacked = !string.IsNullOrWhiteSpace(Convert.ToString(dataRow["PkgsPacked"])) ? Convert.ToInt32(dataRow["PkgsPacked"]) : 0;
                            model.PkgsDamaged = !string.IsNullOrWhiteSpace(Convert.ToString(dataRow["PkgsDamaged"])) ? Convert.ToInt32(dataRow["PkgsDamaged"]) : 0;
                            model.OtherExp = !string.IsNullOrWhiteSpace(Convert.ToString(dataRow["OtherExp"])) ? Convert.ToDecimal(dataRow["OtherExp"]) : 0;
                            model.OtherExp_Ex = !string.IsNullOrWhiteSpace(Convert.ToString(dataRow["OtherExp_Ex"])) ? Convert.ToDecimal(dataRow["OtherExp_Ex"]) : 0;
                            model.RemarksForOtherExp = Convert.ToString(dataRow["RemarksForOtherExp"]);
                            model.Ins_Claim_Amt = !string.IsNullOrWhiteSpace(Convert.ToString(dataRow["InsClaimAmt"])) ? Convert.ToDecimal(dataRow["InsClaimAmt"]) : 0;
                            model.Ins_Claim_Amt_Ex = !string.IsNullOrWhiteSpace(Convert.ToString(dataRow["InsClaimAmt_Ex"])) ? Convert.ToDecimal(dataRow["InsClaimAmt_Ex"]) : 0;
                            model.Ins_BaseCurr = !string.IsNullOrWhiteSpace(Convert.ToString(dataRow["InsBaseeCurr"])) ? Convert.ToInt32(dataRow["InsBaseeCurr"]) : 0;
                            model.Ins_RateCurr = !string.IsNullOrWhiteSpace(Convert.ToString(dataRow["InsRateCurr"])) ? Convert.ToInt32(dataRow["InsRateCurr"]) : 0;
                            model.Ins_ConverRate = !string.IsNullOrWhiteSpace(Convert.ToString(dataRow["InsConverrate"])) ? Convert.ToDecimal(dataRow["InsConverrate"]) : 0;
                            model.InsRoute = !string.IsNullOrWhiteSpace(Convert.ToString(dataRow["InsRoute"])) ? Convert.ToDecimal(dataRow["InsRoute"]) : 0;
                            model.InsRoute_Ex = !string.IsNullOrWhiteSpace(Convert.ToString(dataRow["InsRoute_Ex"])) ? Convert.ToDecimal(dataRow["InsRoute_Ex"]) : 0;
                            model.CompPaidAmt = !string.IsNullOrWhiteSpace(Convert.ToString(dataRow["CompPaidAmt"])) ? Convert.ToDecimal(dataRow["CompPaidAmt"]) : 0;
                            model.CompPaidAmt_Ex = !string.IsNullOrWhiteSpace(Convert.ToString(dataRow["CompPaidAmt_Ex"])) ? Convert.ToDecimal(dataRow["CompPaidAmt_Ex"]) : 0;
                            model.ClaimSettledDate = !string.IsNullOrWhiteSpace(Convert.ToString(dataRow["ClaimSettledDate"])) ? Convert.ToDateTime(dataRow["ClaimSettledDate"]) : (DateTime?)null;
                            model.PayMode = Convert.ToString(dataRow["PayMode"]);
                            model.ClaimStatusID = !string.IsNullOrWhiteSpace(Convert.ToString(dataRow["ClaimStatusID"])) ? Convert.ToInt32(dataRow["ClaimStatusID"]) : 0; 
                            model.VoucherDate = !string.IsNullOrWhiteSpace(Convert.ToString(dataRow["VoucherDate"])) ? Convert.ToDateTime(dataRow["VoucherDate"]) : (DateTime?)null;
                            model.ChqNumber = Convert.ToString(dataRow["ChqNumber"]);
                            model.DocRecdDate = !string.IsNullOrWhiteSpace(Convert.ToString(dataRow["DocRecdDate"])) ? Convert.ToDateTime(dataRow["DocRecdDate"]) : (DateTime?)null;
                            model.ChqToName = Convert.ToString(dataRow["ChqToName"]);
                            model.ChqStatus = Convert.ToString(dataRow["ChqStatus"]);
                            model.ClaimFormRecdDate = !string.IsNullOrWhiteSpace(Convert.ToString(dataRow["ClaimFormRecdDate"])) ? Convert.ToDateTime(dataRow["ClaimFormRecdDate"]) : (DateTime?)null;
                            model.ClaimFileRemarks = Convert.ToString(dataRow["ClaimFileRemarks"]);
                            model.InstToFinance = Convert.ToString(dataRow["InstToFinance"]);
                            model.InsRef = Convert.ToString(dataRow["InsRef"]);
                            model.VendorPaid = !string.IsNullOrWhiteSpace(Convert.ToString(dataRow["VendorPaid"])) ? Convert.ToDecimal(dataRow["VendorPaid"]) : 0;
                            model.VendorPaid_Ex = !string.IsNullOrWhiteSpace(Convert.ToString(dataRow["VendorPaid_Ex"])) ? Convert.ToDecimal(dataRow["VendorPaid_Ex"]) : 0;
                            //model.ControllerID = !string.IsNullOrWhiteSpace(Convert.ToString(dataRow["ControllerID"])) ? Convert.ToInt32(dataRow["ControllerID"]) : 0;
                            model.Pack_Superviser = Convert.ToString(dataRow["PackSupervisorName"]);
                            model.Delivery_Superviser = Convert.ToString(dataRow["DeliverySupervisorName"]);
                            model.settlementType = Convert.ToString(dataRow["SettlementType"]);
                            model.SurveyDate = !string.IsNullOrWhiteSpace(Convert.ToString(dataRow["ClaimSurveyDate"])) ? Convert.ToDateTime(dataRow["ClaimSurveyDate"]) : (DateTime?)null;
                            model.SurveyAmt = !string.IsNullOrWhiteSpace(Convert.ToString(dataRow["ClaimSurveyAmt"])) ? Convert.ToDecimal(dataRow["ClaimSurveyAmt"]) : 0;
                            model.SurveyAmt_Ex = !string.IsNullOrWhiteSpace(Convert.ToString(dataRow["ClaimSurveyAmt_Ex"])) ? Convert.ToDecimal(dataRow["ClaimSurveyAmt_Ex"]) : 0;
                            model.ClaimAmt_Accepted_Shipper = !string.IsNullOrWhiteSpace(Convert.ToString(dataRow["ClaimShipperAcceptedAmt"])) ? Convert.ToDecimal(dataRow["ClaimShipperAcceptedAmt"]) : 0;
                            model.ClaimAmt_Accepted_Shipper_Ex = !string.IsNullOrWhiteSpace(Convert.ToString(dataRow["ClaimShipperAcceptedAmt_Ex"])) ? Convert.ToDecimal(dataRow["ClaimShipperAcceptedAmt_Ex"]) : 0;
                            model.IsSubmitToFinance = Convert.ToBoolean(dataRow["IsSubmitToFinance"]);
                            model.IsFinanceRole = Convert.ToBoolean(dataRow["IsFinanceRole"]);
                            model.IsApproved = Convert.ToBoolean(dataRow["IsApproved"]);
                            model.Status = Convert.ToString(dataRow["Status"]);
                        }

                        if (data.Tables.Count > 2 && data.Tables[2] != null && data.Tables[2].Rows.Count > 0)
                        {
                            model.details = (from item in data.Tables[2].AsEnumerable()
                                             select new ClaimDetails()
                                             {
                                                 ClaimItemDetailIJobID = Convert.ToInt64(item["ClaimItemDetailIJobID"]),
                                                 ClaimItemDetailIsID =!string.IsNullOrWhiteSpace(Convert.ToString(item["ClaimItemDetailIsID"])) ? Convert.ToInt32(item["ClaimItemDetailIsID"]) : 0,
                                                 ClaimItemCategoryId = Convert.ToInt32(item["ClaimItemCategoryId"]),
                                                 ClaimNature = Convert.ToString(item["ClaimNature"]),
                                                 ClaimItemDetailsName = Convert.ToString(item["ClaimItemDetailsName"]),
                                                 ClaimCategoryName = Convert.ToString(item["ClaimCategoryName"]),
                                                 ClaimNatureID = Convert.ToInt32(item["ClaimNatureID"]),
                                                 NumberOfItem = !string.IsNullOrWhiteSpace(Convert.ToString(item["NumberOfItem"])) ? Convert.ToInt32(item["NumberOfItem"]) : 0,
                                                 Remarks = Convert.ToString(item["Remarks"]),
                                             }).ToList();

                        }


                        if (data.Tables.Count > 3 && data.Tables[3] != null && data.Tables[3].Rows.Count > 0)
                        {
                            DataRow item = data.Tables[3].Rows[0];
                            model.InsuranceCompanyID = Convert.ToInt32(item["InsCompID"]);
                            model.InsuranceNo = Convert.ToString(item["InsPANo"]);
                            model.Insurance_Date = Convert.ToDateTime(item["CreatedDate"]);
                            model.Ins_RateCurr = Convert.ToInt32(item["RateCurrID"]);
                            model.Ins_ConverRate = Convert.ToDecimal(item["ConversRate"]);
                            model.Ins_BaseCurr = Convert.ToInt32(item["BaseCurrID"]);
                            model.Sum_Insrd_Amt = Convert.ToDecimal(item["InsuredAmount"]);
                            model.Sum_Insrd_Amt_Ex = Convert.ToDecimal(item["ConverInsuredAmt"]);
                            model.Prem_Paid = Convert.ToDecimal(item["PremiumPaid"]);
                            model.Prem_Paid_Ex = Convert.ToDecimal(item["ConverPremiumPaid"]);
                            model.Prem_Recieved = Convert.ToDecimal(item["BasePremAmt"]);
                            model.Prem_Recieved_Ex = Convert.ToDecimal(item["ConverBasePremAmt"]);
                            model.P_A_No =!string.IsNullOrWhiteSpace(Convert.ToString(item["InsPANo"])) ? Convert.ToInt64(item["InsPANo"]) : (Int64?)null;
                        }

                        if (data.Tables.Count > 4 && data.Tables[4] != null && data.Tables[4].Rows.Count > 0)
                        {
                            model.docUpload.docLists = (from item in data.Tables[4].AsEnumerable()
                                                      select new DocList()
                                                      {
                                                          //ActivityID = Convert.ToInt64(item["ActivityID"]),
                                                          DocID = Convert.ToInt64(item["DocID"]),
                                                          DocTypeID = Convert.ToInt32(item["DocTypeID"]),
                                                          DocType = Convert.ToString(item["DocTypeName"]),
                                                          DocumentName = Convert.ToString(item["DocumentName"]),
                                                          //file = (HttpPostedFileBase)new MemoryPostedFile(System.IO.File.ReadAllBytes(Convert.ToString(item["DocumentPath"])))

                                                      }).ToList();

                        }
                    }
                    
                }

                data = data = claimDAL.GetInsuranceDetail(UserSession.GetUserSession().LoginID, MoveID, -1);
                if (data != null && data.Tables.Count > 0 && data.Tables[0] != null && data.Tables[0].Rows.Count > 0)
                {
                    DataRow item = data.Tables[0].Rows[0];
                    model.Total_InsuredAmout = !string.IsNullOrEmpty(Convert.ToString(item["InsuredAmount"])) ? Convert.ToDecimal(item["InsuredAmount"]) : 0;
                    model.Total_BasePremAmt = !string.IsNullOrEmpty(Convert.ToString(item["BasePremAmt"])) ? Convert.ToDecimal(item["BasePremAmt"]) : 0;
                    model.Total_PremiumPaid = !string.IsNullOrEmpty(Convert.ToString(item["PremiumPaid"])) ? Convert.ToDecimal(item["PremiumPaid"]) : 0;
                    model.Total_InsuredAmout_Ex = !string.IsNullOrEmpty(Convert.ToString(item["InsuredAmountConvert"])) ? Convert.ToDecimal(item["InsuredAmountConvert"]) : 0;
                    model.Total_BasePremAmt_Ex = !string.IsNullOrEmpty(Convert.ToString(item["BasePremAmtConvert"])) ? Convert.ToDecimal(item["BasePremAmtConvert"]) : 0;
                    model.Total_PremiumPaid_Ex = !string.IsNullOrEmpty(Convert.ToString(item["PremiumPaidConvert"])) ? Convert.ToDecimal(item["PremiumPaidConvert"]) : 0;
                }
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "ClaimDAL", "GetClaimDetails", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

            return model;
        }

        public bool Inset(Claim model, out string result, bool IsApprove = false)
        {
            try
            {
                return claimDAL.Insert(model, UserSession.GetUserSession().LoginID, out result, IsApprove: IsApprove);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "ClaimDAL", "Inset", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

        }

        public Claim GetInsuranceDetail(Int64 MoveID,Int64 P_A_NO)
        {
            Claim model = new Claim();

            try
            {
                DataSet data = claimDAL.GetInsuranceDetail(UserSession.GetUserSession().LoginID, MoveID, P_A_NO);

                if (data != null && data.Tables.Count > 0 && data.Tables[0] != null && data.Tables[0].Rows.Count > 0)
                {
                    DataRow item = data.Tables[0].Rows[0];
                    model.Total_InsuredAmout = Convert.ToDecimal(item["InsuredAmount"]);
                    model.Total_BasePremAmt = Convert.ToDecimal(item["BasePremAmt"]);
                    model.Total_PremiumPaid = Convert.ToDecimal(item["PremiumPaid"]);
                    model.Total_InsuredAmout_Ex = !string.IsNullOrEmpty(Convert.ToString(item["InsuredAmountConvert"])) ? Convert.ToDecimal(item["InsuredAmountConvert"]) : 0;
                    model.Total_BasePremAmt_Ex = !string.IsNullOrEmpty(Convert.ToString(item["BasePremAmtConvert"])) ? Convert.ToDecimal(item["BasePremAmtConvert"]) : 0;
                    model.Total_PremiumPaid_Ex = !string.IsNullOrEmpty(Convert.ToString(item["PremiumPaidConvert"])) ? Convert.ToDecimal(item["PremiumPaidConvert"]) : 0;
                }


                if (data != null && data.Tables.Count>1 && data.Tables[1]!=null && data.Tables[1].Rows.Count > 0)
                {
                    DataRow item = data.Tables[1].Rows[0];
                    model.InsuranceCompanyID = Convert.ToInt32(item["InsCompID"]);
                    model.InsuranceNo = Convert.ToString(item["InsPANo"]);
                    model.Insurance_Date = Convert.ToDateTime(item["CreatedDate"]);
                    model.Ins_RateCurr = Convert.ToInt32(item["RateCurrID"]);
                    model.Ins_ConverRate = Convert.ToDecimal(item["ConversRate"]);
                    model.Ins_BaseCurr = Convert.ToInt32(item["BaseCurrID"]);
                    model.Sum_Insrd_Amt = Convert.ToDecimal(item["InsuredAmount"]);
                    model.Sum_Insrd_Amt_Ex = Convert.ToDecimal(item["ConverInsuredAmt"]);
                    model.Prem_Paid = Convert.ToDecimal(item["PremiumPaid"]);
                    model.Prem_Paid_Ex = Convert.ToDecimal(item["ConverPremiumPaid"]);
                    model.Prem_Recieved = Convert.ToDecimal(item["BasePremAmt"]);
                    model.Prem_Recieved_Ex = Convert.ToDecimal(item["ConverBasePremAmt"]);
                    model.P_A_No = !string.IsNullOrWhiteSpace(Convert.ToString(item["InsPANo"])) ? Convert.ToInt64(item["InsPANo"]) : (Int64?)null;
                }
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "ClaimBL", "GetInsuranceDetail", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

            return model;

        }

        public ClaimPrint GetPrintDetail(Int64 MoveID,Int64 ClaimID)
        {
            ClaimPrint model = new ClaimPrint();

            try
            {
                DataSet data = claimDAL.GetPrintDetail(UserSession.GetUserSession().LoginID, MoveID, ClaimID);

                if (data != null)
                {
                    if (data.Tables.Count > 0 && data.Tables[0] != null && data.Tables[0].Rows.Count > 0)
                    {
                        DataRow item = data.Tables[0].Rows[0];

                        model.JobNo = Convert.ToString(item["JobID"]);
                        model.Shipper = Convert.ToString(item["ShipperName"]);
                        model.Corporate = Convert.ToString(item["CorporateName"]);
                        model.ServiceLine = Convert.ToString(item["ServiceLine"]);

                        
                    }

                    if (data.Tables.Count > 1 && data.Tables[1] != null && data.Tables[1].Rows.Count > 0)
                    {
                        DataRow item = data.Tables[1].Rows[0];
                        model.DocketNo = Convert.ToString(item["ClaimDetailIForJobID"]);
                        model.Claim_Amount = Convert.ToDecimal(item["ClaimAmt"]);
                        model.Claim_Settled = Convert.ToDecimal(item["ClaimShipperAcceptedAmt"]);
                        model.Writer_Amount = Convert.ToDecimal(item["Writer_Amount"]);
                        model.Writer_ExAmount = Convert.ToDecimal(item["Writer_ExAmount"]);
                        model.Total = Convert.ToDecimal(item["Total"]);
                        model.Ins_Ref = Convert.ToString(item["InsRef"]);
                        model.ClaimDate = !string.IsNullOrEmpty(Convert.ToString( item["ClaimSettledDate"])) ? Convert.ToDateTime(item["ClaimSettledDate"]) : (DateTime?)null;
                        model.TypeSettlement = Convert.ToString(item["SettlementType"]);
                        model.ChequePayee = Convert.ToString(item["ChqToName"]);
                        model.NoteToFinance = Convert.ToString(item["InstToFinance"]);
                        model.PreparedBy = Convert.ToString(item["PreparedBy"]);
                        model.VerifiedBy = Convert.ToString(item["VerifiedBy"]);
                        model.AuthorisedBy = Convert.ToString(item["AuthorisedBy"]);
                    }

                    if (data.Tables.Count > 2 && data.Tables[2] != null && data.Tables[2].Rows.Count > 0)
                    {
                        DataRow item = data.Tables[2].Rows[0];
                        model.Premium_Paid = Convert.ToDecimal(item["PremiumPaid"]);
                        model.Premium_Recieved = Convert.ToDecimal(item["BasePremAmt"]);
                        model.Amount_Insured = Convert.ToDecimal(item["InsuredAmount"]);
                    }

                    if (data.Tables.Count > 3 && data.Tables[3] != null && data.Tables[3].Rows.Count > 0)
                    {
                        DataRow item = data.Tables[3].Rows[0];
                        model.Total_Revenue = Convert.ToDecimal(item["RevenAmt"]);
                        model.Total_Estimated = Convert.ToDecimal(item["CostAmt"]);
                        model.Gross_Profit = Convert.ToDecimal(item["ProfitAmt"]);
                    }
                }
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "ClaimBL", "GetPrintDetail", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

            return model;
        }

        public JobDocument GetDownloadFile(Int64 DocID, Int64? ClaimID = -1)
        {
            JobDocument obj = new JobDocument();
            try
            {
                if (ClaimID == null)
                {
                    ClaimID = -1;
                }

                return claimDAL.GetDownloadFile(DocID, Convert.ToInt64(ClaimID), UserSession.GetUserSession().LoginID);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "JobReportBL", "GetDownloadFile", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

            return obj;
        }

        public EClaimPrint GetEPrintDetail(Int64 MoveID, Int64 ClaimID)
        {
            EClaimPrint model = new EClaimPrint();

            try
            {
                DataSet data = claimDAL.GetEPrintDetail(UserSession.GetUserSession().LoginID, MoveID, ClaimID);

                if (data != null)
                {
                    if (data.Tables.Count > 0 && data.Tables[0] != null && data.Tables[0].Rows.Count > 0)
                    {
                        DataRow item = data.Tables[0].Rows[0];
                        
                        model.Transit = Convert.ToString(item["Transit"]);

                        model.InsuredName = Convert.ToString(item["ShipperName"]);
                        model.PolicyNo = Convert.ToString(item["PolicyNo"]);
                        model.DateTimeofLoss = Convert.ToDateTime(item["DateTimeofLoss"]);
                        model.LossLocation = Convert.ToString(item["LossLocation"]);
                        model.LossDescription = Convert.ToString(item["LossDescription"]);

                        model.Contact_Person = Convert.ToString(item["ShipperName"]);
                        //model.ContactNo = Convert.ToString(item["ShipperName"]);
                        model.Contact_Mob = Convert.ToString(item["Contact_Mob"]);
                        model.Contact_Off = Convert.ToString(item["Contact_Off"]);

                        model.EstimatedLossAmount = Convert.ToString(item["EstimatedLossAmount"]);

                        model.SenderName = Convert.ToString(item["SenderName"]);
                        //model.SenderContactNo = Convert.ToString(item["ServiceLine"]);
                        model.SenderContactNo_Mob = Convert.ToString(item["SenderContactNo_Mob"]);
                        model.SenderContactNo_Off = Convert.ToString(item["SenderContactNo_Off"]);
                        model.SenderEmail = Convert.ToString(item["SenderEmail"]);

                        model.Email_notification = Convert.ToString(item["Email_notification"]);
                        model.Toll_Free = Convert.ToString(item["Toll_Free"]);
                        model.FormatType = Convert.ToString(item["FormatType"]); 
                    }
                }
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "ClaimBL", "GetEPrintDetail", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

            return model;
        }

        public bool SentToFinance(Claim model, out string result)
        {
            try
            {


                EmailConfig email = claimDAL.GetClaimEmailDetail(model, UserSession.GetUserSession().LoginID);
                model.Email = email;
                if (SendClaimMail(email))
                {
                    return claimDAL.Insert(model, UserSession.GetUserSession().LoginID, out result,SentToFinance:true);
                }
                else
                {
                    result = "Email Send Fail.";
                    return false;
                }


                
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "ClaimDAL", "Inset", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

        }

        public bool SendClaimMail(EmailConfig model)
        {
            bool IsEmailSend = false;
            try
            {
                using (MailMessage mm = new MailMessage())

                {
                    mm.Subject = model.Subject;
                    mm.Body = model.Body;
                    //if (model.Attachments != null)
                    //{
                    foreach (EmailSendAttachment itm in model.attachments)
                    {
                        if (File.Exists(itm.FilePath))
                        {
                            mm.Attachments.Add(new Attachment(itm.FilePath));
                        }
                    }
                    //mm.Attachments.Add(new Attachment(esa.FilePath+ ""+""+esa.FileName));
                    //}

                    mm.IsBodyHtml = true;

                    //mm.To.Add("ashley@writercorporation.com");

                    mm.To.Add(model.EmailTo);
                    mm.From = new MailAddress(model.EmailFrom);
                    //mm.From = new MailAddress("sanjay.yadav.rprj@writercorporation.com");


                    using (SmtpClient smtp = new SmtpClient(model.Host, model.Port))
                    {
                        //SMTPSettings _settingsObj = new SMTPSettings();
                        //_settingsObj = _emailService.GetSMTPSettings(1).SingleOrDefault();
                        //smtp.Host = "outlook.office365.com";// _settingsObj.Host;// "smtp.gmail.com";
                        //smtp.Host = model.Host;/////"mail.writercorporation.com";
                        //smtp.Port = model.Port; ///25; //_settingsObj.Port;
                        // NetworkCredential NetworkCred = new NetworkCredential(_settingsObj.SendTestEmailTo, _settingsObj.Password);
                        //smtp.UseDefaultCredentials = true;
                        smtp.Credentials = new NetworkCredential(model.EmailFrom, model.EmailFromPassowrd) ;
                        smtp.EnableSsl = model.EnableSSL;//false;
                        
                        smtp.Send(mm);
                        //return true;
                        IsEmailSend = true;

                    }
                    return IsEmailSend;
                }
            }
            catch (Exception ex)
            {
                return IsEmailSend;
            }


            //return View();
        }
    }
}