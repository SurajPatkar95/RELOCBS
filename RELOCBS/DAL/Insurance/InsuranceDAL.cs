using RELOCBS.App_Code;
using RELOCBS.Common.ExceptionHandling;
using RELOCBS.Entities;
using RELOCBS.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;

namespace RELOCBS.DAL.Insurance
{
    public class InsuranceDAL
    {
        string FilePath = System.Configuration.ConfigurationManager.AppSettings["InsuranceFiles"].ToString();

        private CommonSubs _CSubs;

        public CommonSubs CSubs
        {

            get
            {
                if (this._CSubs == null)
                    this._CSubs = new CommonSubs();
                return this._CSubs;
            }
        }


        public IQueryable<Entities.InsuranceGrid> GetInsuranceGrid(DateTime? FromDate,DateTime? Todate,bool IsJobDate,bool IsInsuranceDate,Int64 MoveId,Int64 Insurance_Id,bool RMCBuss, int CompanyID)
        {
            int LoggedinUserID = UserSession.GetUserSession().LoginID;
            IQueryable<Entities.InsuranceGrid> List= new  List<Entities.InsuranceGrid>().AsQueryable();

            try
            {
                
                using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {
                        conn.AddCommand("[Ins].[GetJobInsuranceForGrid]", QueryType.Procedure);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IsJob", SqlDbType.Bit, 1, ParameterDirection.Input,IsJobDate);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IsInsurance", SqlDbType.Bit, 1, ParameterDirection.Input, IsInsuranceDate);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_FromDate", SqlDbType.DateTime, 0, ParameterDirection.Input, FromDate);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ToDate", SqlDbType.DateTime, 0, ParameterDirection.Input, Todate);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LoginID", SqlDbType.Int, 0, ParameterDirection.Input, UserSession.GetUserSession().LoginID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_MoveID", SqlDbType.Int, 0, ParameterDirection.Input, MoveId);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_InsPremiunForJobID", SqlDbType.Int,0, ParameterDirection.Input, Insurance_Id);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CompID", SqlDbType.Int, 0, ParameterDirection.Input, CompanyID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IsRMCBuss", SqlDbType.Bit, 1, ParameterDirection.Input, RMCBuss);
                        DataTable dt = (DataTable)conn.ExecuteProcedure(ProcedureReturnType.DataTable);

                        if (dt != null)
                        {
                            var result = (from item in dt.AsEnumerable()
                                          select new Entities.InsuranceGrid()
                                          {
                                              MoveID = Convert.ToInt64(item["MoveID"]),
                                              Insurance_ID = !string.IsNullOrWhiteSpace(Convert.ToString(item["InsPremiunForJobID"])) ? Convert.ToInt64(item["InsPremiunForJobID"]) : -1,
                                              JobNo = Convert.ToString(item["JobID"]),
                                              InsuranceNo = Convert.ToString(item["InsNumber"]),
                                              JobDate = Convert.ToDateTime(item["JobOpenedDate"]),
                                              Insurance_Date = Convert.ToDateTime(item["InsuranceDate"]),
                                              ServiceLine = Convert.ToString(item["ServiceLine"]),
                                              OrgCity = Convert.ToString(item["FromCity"]),
                                              DestCity = Convert.ToString(item["ToCity"]),
                                              Controller = Convert.ToString(item["ControllerName"]),
                                              ShipperName = Convert.ToString(item["ShipperName"]),
                                              Client = Convert.ToString(item["ClientName"]),
                                              Corporate = Convert.ToString(item["CorporateName"]),
                                              InsuranceCompany = Convert.ToString(item["InsCompName"]),
                                              PolicyNo = Convert.ToString(item["InsPoilicyNo"]),
                                              PackSupervisorName = Convert.ToString(item["PackSupervisorName"]),
                                              ControllerName = Convert.ToString(item["ControllerName"]),
                                              InsuredAmount = Convert.ToDecimal(item["InsuredAmount"]),
                                          }).AsQueryable();
                            List = result;
                        }
                    }
                    else
                        throw new Exception(conn.ErrorMessage);
                }


                return List;
            }
            catch (Exception ex)
            {

                throw new DataAccessException(Convert.ToString(LoggedinUserID), "InsuranceDAL", "GetInsuranceGrid", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }


        }

        public DataSet GetInsuranceDetails(int LoginID,Int64 MoveID, Int64 Insurance_ID)
        {
            DataSet Ds = new DataSet();

            try
            {
                string query = string.Format("[Ins].[GetInsuranceDetails] @SP_MoveID={0},@SP_InsPremiunForJobID={1},@SP_LoginID={2}",
                CSubs.QSafeValue(Convert.ToString(MoveID)),
                CSubs.QSafeValue(Convert.ToString(Insurance_ID)),
                CSubs.QSafeValue(Convert.ToString(LoginID)));

                Ds = CSubs.GetDataSet(query);

                return Ds;
            }
            catch (Exception ex)
            {

                throw new DataAccessException(Convert.ToString(LoginID), "InsuranceDAL", "GetInsuranceDetails", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

        }

        public bool Insert(InsuranceViewModel model, int LoginID, out string result)
        {
            try
            {
                using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {
                        
                        conn.AddCommand("[Ins].[AddEditInsPremiumForJob]", QueryType.Procedure);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_InsPremiunForJobID", SqlDbType.BigInt, 0, ParameterDirection.InputOutput, model.Insurance_ID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_MoveID", SqlDbType.BigInt, 0, ParameterDirection.Input, model.MoveID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_InsCompID", SqlDbType.BigInt, 0, ParameterDirection.Input, model.InsuranceCompanyID);
                        //conn.AddParameter(ParameterOF.PROCEDURE, "@SP_InsNumber", SqlDbType.BigInt, 0, ParameterDirection.Input, model.Policy_No);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_PackDispDate", SqlDbType.DateTime, 0, ParameterDirection.Input, model.Pac_Disp_Date);
                        
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_PackSupervisorName", SqlDbType.VarChar, 50, ParameterDirection.Input,CSubs.PSafeValue(model.Pack_Superviser));
                        //conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Pac_Disp_Date", SqlDbType.DateTime, 0, ParameterDirection.Input, model.Pac_Disp_Date);
                        //conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Open_Prem_Amt", SqlDbType.Decimal, 0, ParameterDirection.Input, model.Open_Prem_Amt);
                        //conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Open_SI_Amt", SqlDbType.Decimal, 0, ParameterDirection.Input, model.Open_SI_Amt);
                        
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CertNo", SqlDbType.VarChar, 50, ParameterDirection.Input, CSubs.PSafeValue(model.CertNo));
                        //conn.AddParameter(ParameterOF.PROCEDURE, "@SP_P_A_No", SqlDbType.VarChar, 50, ParameterDirection.Input, CSubs.PSafeValue(model.P_A_No));
                        
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_RateCurrID", SqlDbType.Int, 0, ParameterDirection.Input, model.RateCurrencyID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_BaseCurrID", SqlDbType.Int, 0, ParameterDirection.Input, model.BaseCurrencyID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ConversRate", SqlDbType.Float, 0, ParameterDirection.Input, model.ExRate);

                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_InsuredAmount", SqlDbType.Float, 0, ParameterDirection.Input, model.Sum_Insrd_Amt);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_PremiumPercent", SqlDbType.Float, 0, ParameterDirection.Input, model.Shp_Prem_Percent);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_BasePremAmt", SqlDbType.Float, 0, ParameterDirection.Input, model.Basic_Prem_Paid);
                        
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_TaxAmount", SqlDbType.Float, 0, ParameterDirection.Input, model.Service_Tax_Paid);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_StampDuty", SqlDbType.Float, 0, ParameterDirection.Input, model.Stamp_Duty_Paid);
                        //conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Total_Prem_Paid", SqlDbType.Decimal, 0, ParameterDirection.Input, model.Total_Prem_Paid);
                        

                        //conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Bal_Prem_Amt", SqlDbType.Decimal, 0, ParameterDirection.Input, model.Bal_Prem_Amt);
                        //conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Bal_SI", SqlDbType.Decimal, 0, ParameterDirection.Input, model.Bal_SI);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_StatusID", SqlDbType.Int, 0, ParameterDirection.Input, model.Status);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ControllerID", SqlDbType.Int, 0, ParameterDirection.Input, model.ControllerID);
                        
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Remarks", SqlDbType.VarChar, 250, ParameterDirection.Input, CSubs.PSafeValue(model.StatusRemark));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_InsPremiumByWriterID", SqlDbType.Int, 0, ParameterDirection.Input, model.Policy_No);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IsCoverNote", SqlDbType.Bit, 0, ParameterDirection.Input, model.IsCoverNote);
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_InsDelayReason", SqlDbType.Int, 0, ParameterDirection.Input, model.InsDelayReason);
						



						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IsActive", SqlDbType.Bit, 0, ParameterDirection.Input, true);

                        if (model.CoverNoteFile != null && model.CoverNoteFile.ContentLength > 0)
                        {
                            string FileName = model.CoverNoteFile.FileName;
                            string Extension = Path.GetExtension(model.CoverNoteFile.FileName);
                            conn.AddParameter(ParameterOF.PROCEDURE, "@SP_FileName", SqlDbType.NVarChar, 250, ParameterDirection.Input, CSubs.PSafeValue(FileName));
                            conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Ext", SqlDbType.NVarChar, 50, ParameterDirection.Input, CSubs.PSafeValue(Extension));
                            conn.AddParameter(ParameterOF.PROCEDURE, "@SP_FilePath", SqlDbType.NVarChar, 500, ParameterDirection.InputOutput, CSubs.PSafeValue(FilePath));

                        }

                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LoginID", SqlDbType.Int, 0, ParameterDirection.Input, UserSession.GetUserSession().LoginID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@RETURNSTATUS", SqlDbType.SmallInt, 0, ParameterDirection.ReturnValue);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@OutMsg", SqlDbType.NVarChar, -1, ParameterDirection.Output);
                        conn.ExecuteProcedure(ProcedureReturnType.SingleValue);

                        if (!conn.IsError)
                        {
                            int ReturnStatus = Convert.ToInt32(conn.GetParameterValue(ParameterOF.PROCEDURE, "@RETURNSTATUS"));
                            result = Convert.ToString(conn.GetParameterValue(ParameterOF.PROCEDURE, "@OutMsg"));

                            if (ReturnStatus == 0)
                            {
                                model.Insurance_ID = Convert.ToInt64(conn.GetParameterValue(ParameterOF.PROCEDURE, "@SP_InsPremiunForJobID"));

                                if (model.CoverNoteFile != null && model.CoverNoteFile.ContentLength > 0)
                                {
                                    FilePath = Convert.ToString(conn.GetParameterValue(ParameterOF.PROCEDURE, "@SP_FilePath"));

                                    try
                                    {
                                        if (!string.IsNullOrWhiteSpace(FilePath))
                                        {
                                            model.CoverNoteFile.SaveAs(FilePath);
                                            //conn.CommitTransaction();
                                        }

                                        return true;
                                    }
                                    catch (Exception)
                                    {
                                        //conn.RollbackTransaction();
                                        //throw new Exception("Unable to save File");
                                        result = result + ".Unable to save File";
                                        return false;
                                    }

                                }

                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                        else
                            throw new Exception(conn.ErrorMessage);


                    }
                    else
                        throw new Exception(conn.ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoginID), "InsuranceDAL", "Insert", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

        }

        public DataTable GetInsuranceAmounts(int LoginID,int InsCompID,Int64 policyNo, decimal Sum_Ins_Amt)
        {
            DataTable dataTable = new DataTable();

            try
            {
               string query = string.Format("[Ins].[GetTaxDetails] @SP_InsCompID={0},@SP_InsuredAmount={1},@SP_LoginID={2},@SP_InsPremiumByWriterID={3}",
               CSubs.QSafeValue(Convert.ToString(InsCompID)),
               CSubs.QSafeValue(Convert.ToString(Sum_Ins_Amt)),
               CSubs.QSafeValue(Convert.ToString(LoginID)),
               CSubs.QSafeValue(Convert.ToString(policyNo))
               );

               dataTable = CSubs.GetDataTable(query);
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoginID), "InsuranceDAL", "GetInsuranceAmounts", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

            return dataTable;

        }

        public DataTable GetPrintDetail(int LoginID, Int64 id)
        {
            DataTable data = new DataTable();

            try
            {
                string query = string.Format("[Ins].[GetInsurancPrint] @SP_InsPremiunForJobID={0},@SP_LoginID={1}",
               CSubs.QSafeValue(Convert.ToString(id)),
               CSubs.QSafeValue(Convert.ToString(LoginID)));

                data = CSubs.GetDataTable(query);
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoginID), "InsuranceDAL", "GetPrintDetail", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

            return data;
        }

        public JobDocument GetDownloadFile(int FileID, int LoginID)
        {
            JobDocument job = new JobDocument();
            try
            {
                DataTable dt = CSubs.GetDataTable(string.Format("exec [Ins].[GetInsuranceDetailsForJobDoc] @SP_Fileid={0},@SP_LoginID={1}", CSubs.QSafeValue(Convert.ToString(FileID)), CSubs.QSafeValue(Convert.ToString(LoginID))));

                if (dt != null && dt.Rows.Count > 0)
                {
                    job.FileID = Convert.ToInt32(dt.Rows[0]["FileID"]);
                    job.FilePath = Convert.ToString(dt.Rows[0]["DocFilePath"]);
                    job.FileName = Convert.ToString(dt.Rows[0]["DocFileName"]);
                }

            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoginID), "InsuranceDAL", "GetDownloadFile", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

            return job;

        }
    }
}