using RELOCBS.Common.ExceptionHandling;
using RELOCBS.DAL.Insurance;
using RELOCBS.Entities;
using RELOCBS.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Linq.Dynamic;

namespace RELOCBS.BL.Insurance
{
    public class InsuranceBL
    {
        private InsuranceDAL _insuranceDAL;

        public InsuranceDAL insuranceDAL
        {

            get
            {
                if (this._insuranceDAL == null)
                    this._insuranceDAL = new InsuranceDAL();
                return this._insuranceDAL;
            }
        }


        public IEnumerable<Entities.InsuranceGrid> GetInsuranceGrid(DateTime? FromDate, DateTime? Todate, bool IsJobDate, bool IsInsuranceDate, Int64 MoveId, Int64 Insurance_Id, string sort, string sortdir, int skip, int pageSize, out int totalCount)
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
                IQueryable<Entities.InsuranceGrid> InsuranceGrid = insuranceDAL.GetInsuranceGrid(FromDate, Todate, IsJobDate, IsInsuranceDate, MoveId, Insurance_Id, RMCBuss, UserSession.GetUserSession().CompanyID);
                if (InsuranceGrid != null)
                {
                    totalCount = InsuranceGrid.Count();

                    if (pageSize > 1)
                    {
                        InsuranceGrid = InsuranceGrid.Skip((skip * (pageSize - 1))).Take(skip);
                    }
                    else
                    {
                        InsuranceGrid = InsuranceGrid.Take(skip);
                    }

                    InsuranceGrid = InsuranceGrid.OrderBy(sort + " " + sortdir);

                    return InsuranceGrid.ToList();
                }
                else
                {
                    return new List<Entities.InsuranceGrid>();
                }
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "InsuranceBL", "GetDetailById", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

        }

        public InsuranceViewModel GetInsuranceDetails(Int64 MoveID, Int64 Insurance_ID = 0)
        {
            InsuranceViewModel model = new InsuranceViewModel();

            try
            {
                DataSet data = insuranceDAL.GetInsuranceDetails(UserSession.GetUserSession().LoginID, MoveID, Insurance_ID);

                if (data != null && data.Tables.Count > 0)
                {
                    if (data.Tables.Count > 0 && data.Tables[0] != null && data.Tables[0].Rows.Count > 0)
                    {
                        model = (from item in data.Tables[0].AsEnumerable()
                               select new InsuranceViewModel()
                               {
                                   MoveID = Convert.ToInt64(item["MoveID"]),
                                   Insurance_ID =!string.IsNullOrWhiteSpace(Convert.ToString(item["InsPremiunForJobID"])) ?  Convert.ToInt64(item["InsPremiunForJobID"]): -1,
                                   JobNo = Convert.ToString(item["JobID"]),
                                   //JobDate = Convert.ToDateTime(item["JobNo"]),
                                   ServiceLine = Convert.ToString(item["ServiceLine"]),
                                   OrgCity = Convert.ToString(item["FromCity"]),
                                   DestCity = Convert.ToString(item["ToCity"]),
                                   OrgAgent = Convert.ToString(item["OrgAgentName"]),
                                   DestAgent = Convert.ToString(item["DestAgentName"]),
                                   Controller = Convert.ToString(item["ControllerName"]),
                                   ShipperName = Convert.ToString(item["ShipperName"]),
                                   Client = Convert.ToString(item["ClientName"]),
                                   Corporate = Convert.ToString(item["CorporateName"]),
                                   Pack_Superviser = Convert.ToString(item["PackSupervisorName"]),

                                   ControllerID = !string.IsNullOrWhiteSpace(Convert.ToString(item["ControllerID"])) ? Convert.ToInt32(item["ControllerID"]) : 0,
                                   Pac_Disp_Date = !string.IsNullOrWhiteSpace(Convert.ToString(item["PackDispDate"])) ? Convert.ToDateTime(item["PackDispDate"]) : (DateTime?)null,
                                   Mode = Convert.ToString(item["TransportMode"]),
                                   BaseCurrencyID = !string.IsNullOrWhiteSpace(Convert.ToString(item["BaseCurrID"])) ? Convert.ToInt32(item["BaseCurrID"]) : 0,
                                   RateCurrencyID = !string.IsNullOrWhiteSpace(Convert.ToString(item["RateCurrID"])) ? Convert.ToInt32(item["RateCurrID"]) : 0,
                                   ExRate = !string.IsNullOrWhiteSpace(Convert.ToString(item["RateCurrID"])) ? Convert.ToDecimal(item["ConversRate"]) : 0,

                                   Open_Prem_Amt = !string.IsNullOrWhiteSpace(Convert.ToString(item["OpenPremAmt"])) ? Convert.ToDecimal(item["OpenPremAmt"]) : 0,
                                   Open_SI_Amt = !string.IsNullOrWhiteSpace(Convert.ToString(item["OpenSI"])) ? Convert.ToDecimal(item["OpenSI"]) : 0,

                                   InsuranceCompanyID = !string.IsNullOrWhiteSpace(Convert.ToString(item["InsCompID"])) ? Convert.ToInt32(item["InsCompID"]) : 0,
                                   Policy_No = !string.IsNullOrWhiteSpace(Convert.ToString(item["InsPremiumByWriterID"])) ? Convert.ToInt32(item["InsPremiumByWriterID"]) : 0,
                                   CertNo =Convert.ToString(item["CertNo"]),
                                   P_A_No = Convert.ToString(item["InsPremiunForJobID"]),
                                   Sum_Insrd_Amt = !string.IsNullOrWhiteSpace(Convert.ToString(item["InsuredAmount"])) ? Convert.ToDecimal(item["InsuredAmount"]) : 0,
                                   Sum_Insrd_Amt_Ex = !string.IsNullOrWhiteSpace(Convert.ToString(item["InsuredAmount_BaseAmt"])) ? Convert.ToDecimal(item["InsuredAmount_BaseAmt"]) : 0,

                                   Shp_Prem_Amt = !string.IsNullOrWhiteSpace(Convert.ToString(item["ShpPremiumAmt"])) ? Convert.ToDecimal(item["ShpPremiumAmt"]) : 0,
                                   Shp_Prem_Amt_Ex = !string.IsNullOrWhiteSpace(Convert.ToString(item["ShpPremiumAmt_BaseAmt"])) ? Convert.ToDecimal(item["ShpPremiumAmt_BaseAmt"]) : 0,
                                   Shp_Prem_Percent = !string.IsNullOrWhiteSpace(Convert.ToString(item["PremiumPercent"])) ? Convert.ToDecimal(item["PremiumPercent"]) : 0,

                                   Basic_Prem_Paid = !string.IsNullOrWhiteSpace(Convert.ToString(item["BasePremAmt"])) ? Convert.ToDecimal(item["BasePremAmt"]) : 0,
                                   Basic_Prem_Paid_Ex = !string.IsNullOrWhiteSpace(Convert.ToString(item["BasePremAmt_BaseAmt"])) ? Convert.ToDecimal(item["BasePremAmt_BaseAmt"]) : 0,

                                   Service_Tax_Paid = !string.IsNullOrWhiteSpace(Convert.ToString(item["TaxAmount"])) ? Convert.ToDecimal(item["TaxAmount"]) : 0,
                                   Service_Tax_Paid_Ex = !string.IsNullOrWhiteSpace(Convert.ToString(item["TaxAmount_BaseAmt"])) ? Convert.ToDecimal(item["TaxAmount_BaseAmt"]) : 0,

                                   Stamp_Duty_Paid = !string.IsNullOrWhiteSpace(Convert.ToString(item["StampDuty"])) ? Convert.ToDecimal(item["StampDuty"]) : 0,
                                   Stamp_Duty_Paid_Ex = !string.IsNullOrWhiteSpace(Convert.ToString(item["StampDuty_BaseAmt"])) ? Convert.ToDecimal(item["StampDuty_BaseAmt"]) : 0,

                                   Total_Prem_Paid = !string.IsNullOrWhiteSpace(Convert.ToString(item["TotalPremPaid"])) ? Convert.ToDecimal(item["TotalPremPaid"]) : 0,
                                   Total_Prem_Paid_Ex = !string.IsNullOrWhiteSpace(Convert.ToString(item["TotalPremPaid_BaseAmt"])) ? Convert.ToDecimal(item["TotalPremPaid_BaseAmt"]) : 0,

                                   Bal_Prem_Amt = !string.IsNullOrWhiteSpace(Convert.ToString(item["BalPremAmt"])) ? Convert.ToDecimal(item["BalPremAmt"]) : 0,
                                   Bal_SI = !string.IsNullOrWhiteSpace(Convert.ToString(item["BalSI"])) ? Convert.ToDecimal(item["BalSI"]) : 0,
                                   Status = !string.IsNullOrWhiteSpace(Convert.ToString(item["StatusID"])) ? Convert.ToInt32(item["StatusID"]) : 0,
                                   StatusRemark = Convert.ToString(item["Remarks"]),
                                   IsCoverNote = !string.IsNullOrWhiteSpace(Convert.ToString(item["IsCoverNote"])) ? Convert.ToBoolean(item["IsCoverNote"]) : false,
                                   BalPremPercent = !string.IsNullOrWhiteSpace(Convert.ToString(item["BalPremPercent"])) ? Convert.ToDecimal(item["BalPremPercent"]) : 0,
                                   TATinHrs= !string.IsNullOrWhiteSpace(Convert.ToString(item["TATinHrs"])) ? Convert.ToInt32(item["TATinHrs"]) : 0,
								   InsDelayReason = !string.IsNullOrWhiteSpace(Convert.ToString(item["InsDelayReason"])) ? Convert.ToInt32(item["InsDelayReason"]) : 0,
							   }).FirstOrDefault();

                        if (data.Tables.Count > 1 && data.Tables[1] != null && data.Tables[1].Rows.Count > 0)
                        {
                            //model.FileID = Convert.ToInt32(data.Tables[1].Rows[0]["FileID"]);
                            model.JobInsCreatedBy = Convert.ToString(data.Tables[1].Rows[0]["JobInsCreatedBy"]);
                        }
                    }

                }
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "InsuranceBL", "GetInsuranceDetails", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

            return model;
        }

        public bool Inset(InsuranceViewModel model, out string result)
        {
            try
            {
                return insuranceDAL.Insert(model, UserSession.GetUserSession().LoginID, out result);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "InsuranceBL", "Inset", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

        }
        
        public InsuranceAmoutDTO GetInsuranceAmounts(int InsCompID,Int64 policyNo, decimal Sum_Ins_Amt)
        {
            InsuranceAmoutDTO dTO = new InsuranceAmoutDTO();
            try
            {
                DataTable data = insuranceDAL.GetInsuranceAmounts(UserSession.GetUserSession().LoginID, InsCompID, policyNo, Sum_Ins_Amt);

                if (data != null && data.Rows.Count > 0)
                {

                    dTO = (from item in data.AsEnumerable()
                             select new InsuranceAmoutDTO()
                             {
                                 Bal_Prem_Amt     = !string.IsNullOrWhiteSpace(Convert.ToString(item["ClosingPremAmt"])) ? Convert.ToDecimal(item["ClosingPremAmt"]) : 0,
                                 Bal_SI           = !string.IsNullOrWhiteSpace(Convert.ToString(item["CloseSI"])) ? Convert.ToDecimal(item["CloseSI"]) : 0,
                                 Open_Prem_Amt    = !string.IsNullOrWhiteSpace(Convert.ToString(item["OpenPremAmt"])) ? Convert.ToDecimal(item["OpenPremAmt"]) : 0,
                                 Basic_Prem_Paid  = !string.IsNullOrWhiteSpace(Convert.ToString(item["BaseAmt"])) ? Convert.ToDecimal(item["BaseAmt"]) : 0,
                                 Service_Tax_Paid = !string.IsNullOrWhiteSpace(Convert.ToString(item["GST"])) ? Convert.ToDecimal(item["GST"]) : 0,
                                 Stamp_Duty_Paid  = !string.IsNullOrWhiteSpace(Convert.ToString(item["StampDuty"])) ? Convert.ToDecimal(item["StampDuty"]) : 0,
                                 Total_Prem_Paid  = !string.IsNullOrWhiteSpace(Convert.ToString(item["TotPremPaid"])) ? Convert.ToDecimal(item["TotPremPaid"]) : 0,
                                 Open_SI_Amt      = !string.IsNullOrWhiteSpace(Convert.ToString(item["OpenSI"])) ? Convert.ToDecimal(item["OpenSI"]) : 0,

                             }).FirstOrDefault();

                }
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "InsuranceBL", "GetInsuranceAmounts", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

            return dTO;


        }

        public InsurancePrint  GetPrintDetail(Int64 id)
        {
            InsurancePrint print = new InsurancePrint();

            try
            {
                DataTable data = insuranceDAL.GetPrintDetail(UserSession.GetUserSession().LoginID, id);

                if (data != null && data.Rows.Count > 0)
                {

                    print = (from item in data.AsEnumerable()
                             select new InsurancePrint()
                             {
                                 JobNo = Convert.ToString(item["JobID"]),
                                 ShipperName = Convert.ToString(item["ShipperName"]),
                                 From_To_City = Convert.ToString(item["FromToCity"]),
                                 PrintDate = Convert.ToDateTime(item["PrintDate"]),
                                 Controller = Convert.ToString(item["Controller"]),
                                 InsCompany = Convert.ToString(item["InsCompName"]),
                                 Sum_Ins_Amt = Convert.ToString(item["InsuredAmount"]),
                                 Pack_Disp_Date =!string.IsNullOrWhiteSpace(Convert.ToString(item["PackDispDate"]))? Convert.ToDateTime(item["PackDispDate"]) : (DateTime?)null,
                                 Description = Convert.ToString(item["Description"]),
                                 PA_NO = Convert.ToString(item["PA_No"]),
                                 CoverNote = Convert.ToString(item["CoverNote"])
                             }).FirstOrDefault();

                }
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "InsuranceBL", "GetInsuranceAmounts", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

            return print;
        }

        public JobDocument GetDownloadFile(int FileID)
        {
            JobDocument obj = new JobDocument();
            try
            {
                return insuranceDAL.GetDownloadFile(FileID, UserSession.GetUserSession().LoginID);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "InsuranceBL", "GetDownloadFile", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

            return obj;
        }
    }
}