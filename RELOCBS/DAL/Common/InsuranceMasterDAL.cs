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

namespace RELOCBS.DAL.Common
{
    public class InsuranceMasterDAL
    {
        string FilePath = System.Configuration.ConfigurationManager.AppSettings["InsMasterFiles"].ToString();

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


        public bool Insert(Insurance_Master model, out string result)
        {
            result = string.Empty;

            try
            {

                using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {
                        conn.AddCommand("[Ins].[AddEditInsPremiumBtCo]", QueryType.Procedure);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_InsPremiumByWriterID", SqlDbType.Int, 0, ParameterDirection.InputOutput, -1);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_InsCompID", SqlDbType.Int, 0, ParameterDirection.Input, model.InsComp_ID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_PolicyNumber", SqlDbType.VarChar, 50, ParameterDirection.Input, CSubs.PSafeValue(model.Policy_No));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_PolicyFromDate", SqlDbType.DateTime, 0, ParameterDirection.Input, model.Policy_Date);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_PolicyExpiryDate", SqlDbType.DateTime, 0, ParameterDirection.Input, null);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_PremiumPercent", SqlDbType.Decimal, 0, ParameterDirection.Input, model.Prem_Percent_Amt);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ServiceTax", SqlDbType.Decimal, 0, ParameterDirection.Input, model.Service_Tax);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_StampDuty", SqlDbType.Decimal, 0, ParameterDirection.Input, model.Stamp_Duty);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_MinPrem", SqlDbType.Decimal, 0, ParameterDirection.Input, model.Min_Prem);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_InsuredAmt", SqlDbType.Decimal, 0, ParameterDirection.Input, model.Sum_Ins);
                        //conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Premium_Amt", SqlDbType.Decimal, 0, ParameterDirection.Input, model.Premium_Amt);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ChqDate", SqlDbType.DateTime, 0, ParameterDirection.Input, model.Cheq_Date);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ChqNo", SqlDbType.VarChar, 50, ParameterDirection.Input, CSubs.PSafeValue(model.Cheq_No));
                        //conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Bal_SI", SqlDbType.Decimal, 0, ParameterDirection.Input, model.Bal_SI);
                        //conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Bal_Prem", SqlDbType.Decimal, 0, ParameterDirection.Input, model.Bal_Prem);

                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CompID", SqlDbType.Int, 0, ParameterDirection.Input, model.CompID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LOGINID", SqlDbType.Int, 0, ParameterDirection.Input, UserSession.GetUserSession().LoginID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IsActive", SqlDbType.Bit, 0, ParameterDirection.Input, true);

                        if (model.PostedFile != null && model.PostedFile.ContentLength > 0)
                        {
                            string FileName = model.PostedFile.FileName;
                            string Extension = Path.GetExtension(model.PostedFile.FileName);
                            conn.AddParameter(ParameterOF.PROCEDURE, "@SP_FileName", SqlDbType.NVarChar, 250, ParameterDirection.Input, CSubs.PSafeValue(FileName));
                            conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Ext", SqlDbType.NVarChar, 50, ParameterDirection.Input, CSubs.PSafeValue(Extension));
                            conn.AddParameter(ParameterOF.PROCEDURE, "@SP_FilePath", SqlDbType.NVarChar, 500, ParameterDirection.InputOutput, CSubs.PSafeValue(FilePath));

                        }

                        conn.AddParameter(ParameterOF.PROCEDURE, "@RETURNSTATUS", SqlDbType.SmallInt, 0, ParameterDirection.ReturnValue);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@OutMsg", SqlDbType.NVarChar, 500, ParameterDirection.Output);
                        conn.ExecuteProcedure(ProcedureReturnType.SingleValue);

                        if (!conn.IsError)
                        {
                            int ReturnStatus = Convert.ToInt32(conn.GetParameterValue(ParameterOF.PROCEDURE, "@RETURNSTATUS"));
                            result = Convert.ToString(conn.GetParameterValue(ParameterOF.PROCEDURE, "@OutMsg"));

                            if (ReturnStatus == 0)
                            {
                                try
                                {
                                    if (model.PostedFile != null && model.PostedFile.ContentLength > 0)
                                    {
                                        FilePath = Convert.ToString(conn.GetParameterValue(ParameterOF.PROCEDURE, "@SP_FilePath"));

                                        if (!string.IsNullOrWhiteSpace(FilePath))
                                        {
                                            try
                                            {
                                                model.PostedFile.SaveAs(FilePath);

                                                return true;
                                            }
                                            catch (Exception)
                                            {
                                                result = "Unable to save File";
                                                return false;
                                            }
                                            //conn.CommitTransaction();
                                        }
                                    }
                                    return true;
                                }
                                catch (Exception)
                                {
                                    //conn.RollbackTransaction();
                                    //throw new Exception("Unable to save File");
                                    result = "Unable to save File";
                                    return false;
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
                throw new DataAccessException(Convert.ToString(UserSession.GetUserSession().LoginID), "InsuranceMasterDAL", "Insert", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
            //return true;
        }

        public bool Update(Insurance_Master model, out string result)
        {
            result = string.Empty;
            
            try
            {
                using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {

                        conn.AddCommand("[Ins].[AddEditInsPremiumBtCo]", QueryType.Procedure);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_InsPremiumByWriterID", SqlDbType.Int, 0, ParameterDirection.InputOutput, model.Ins_M_Id);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_InsCompID", SqlDbType.Int, 0, ParameterDirection.Input, model.InsComp_ID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_PolicyNumber", SqlDbType.VarChar, 50, ParameterDirection.Input, CSubs.PSafeValue(model.Policy_No));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_PolicyFromDate", SqlDbType.DateTime, 0, ParameterDirection.Input, model.Policy_Date);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_PolicyExpiryDate", SqlDbType.DateTime, 0, ParameterDirection.Input, null);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_PremiumPercent", SqlDbType.Decimal, 0, ParameterDirection.Input, model.Prem_Percent_Amt);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ServiceTax", SqlDbType.Decimal, 0, ParameterDirection.Input, model.Service_Tax);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_StampDuty", SqlDbType.Decimal, 0, ParameterDirection.Input, model.Stamp_Duty);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_MinPrem", SqlDbType.Decimal, 0, ParameterDirection.Input, model.Min_Prem);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_InsuredAmt", SqlDbType.Decimal, 0, ParameterDirection.Input, model.Sum_Ins);
                        //conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Premium_Amt", SqlDbType.Decimal, 0, ParameterDirection.Input, model.Premium_Amt);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ChqDate", SqlDbType.DateTime, 0, ParameterDirection.Input, model.Cheq_Date);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ChqNo", SqlDbType.VarChar, 50, ParameterDirection.Input, CSubs.PSafeValue(model.Cheq_No));
                        //conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Bal_SI", SqlDbType.Decimal, 0, ParameterDirection.Input, model.Bal_SI);
                        //conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Bal_Prem", SqlDbType.Decimal, 0, ParameterDirection.Input, model.Bal_Prem);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CompID", SqlDbType.Int, 0, ParameterDirection.Input, model.CompID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LOGINID", SqlDbType.Int, 0, ParameterDirection.Input, UserSession.GetUserSession().LoginID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IsActive", SqlDbType.Bit, 0, ParameterDirection.Input, model.IsActive);

                        if (model.PostedFile != null && model.PostedFile.ContentLength > 0)
                        {
                            string FileName = model.PostedFile.FileName;
                            string Extension = Path.GetExtension(model.PostedFile.FileName);
                            conn.AddParameter(ParameterOF.PROCEDURE, "@SP_FileName", SqlDbType.NVarChar, 250, ParameterDirection.Input, CSubs.PSafeValue(FileName));
                            conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Ext", SqlDbType.NVarChar, 50, ParameterDirection.Input, CSubs.PSafeValue(Extension));
                            conn.AddParameter(ParameterOF.PROCEDURE, "@SP_FilePath", SqlDbType.NVarChar, 500, ParameterDirection.InputOutput, CSubs.PSafeValue(FilePath));

                        }
                        conn.AddParameter(ParameterOF.PROCEDURE, "@RETURNSTATUS", SqlDbType.SmallInt, 0, ParameterDirection.ReturnValue);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@OutMsg", SqlDbType.NVarChar, 500, ParameterDirection.Output);
                        conn.ExecuteProcedure(ProcedureReturnType.SingleValue);

                        if (!conn.IsError)
                        {
                            int ReturnStatus = Convert.ToInt32(conn.GetParameterValue(ParameterOF.PROCEDURE, "@RETURNSTATUS"));
                            result = Convert.ToString(conn.GetParameterValue(ParameterOF.PROCEDURE, "@OutMsg"));
                            if (ReturnStatus == 0)
                            {

                                if (model.PostedFile != null && model.PostedFile.ContentLength > 0)
                                {
                                    FilePath = Convert.ToString(conn.GetParameterValue(ParameterOF.PROCEDURE, "@SP_FilePath"));

                                    try
                                    {
                                        if (!string.IsNullOrWhiteSpace(FilePath))
                                        {
                                            model.PostedFile.SaveAs(FilePath);
                                            //conn.CommitTransaction();
                                        }

                                        return true;
                                    }
                                    catch (Exception)
                                    {
                                        //conn.RollbackTransaction();
                                        //throw new Exception("Unable to save File");
                                        result = "Unable to save File";
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
                            throw new ArgumentException(conn.ErrorMessage);

                    }
                    else
                        throw new ArgumentException(conn.ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(UserSession.GetUserSession().LoginID), "InsuranceMasterDAL", "Update", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

            // return true;
        }

        public bool DeleteById(int id, out string result)
        {
            result = string.Empty;

            try
            {
                using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {

                        conn.AddCommand("Comm.WarehouseDelete", QueryType.Procedure);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CompBranchID", SqlDbType.Int, 0, ParameterDirection.Input, id);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LOGINID", SqlDbType.Int, 0, ParameterDirection.Input, UserSession.GetUserSession().LoginID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@RETURNSTATUS", SqlDbType.SmallInt, 0, ParameterDirection.ReturnValue);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_MESSAGE", SqlDbType.NVarChar, 500, ParameterDirection.Output);
                        conn.ExecuteProcedure(ProcedureReturnType.SingleValue);

                        if (!conn.IsError)
                        {

                            int ReturnStatus = Convert.ToInt32(conn.GetParameterValue(ParameterOF.PROCEDURE, "@RETURNSTATUS"));
                            result = Convert.ToString(conn.GetParameterValue(ParameterOF.PROCEDURE, "@SP_MESSAGE"));

                            if (ReturnStatus == 0)
                            {

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
                throw new DataAccessException(Convert.ToString(UserSession.GetUserSession().LoginID), "InsuranceMasterDAL", "DeleteById", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

        }

        public DataTable GetDetailById(int? id, int LoginID)
        {
            DataTable CompanyDetailDt = new DataTable();

            try
            {
                string query = string.Format("exec [Ins].[GetInsPremiumBtCoDetail]  @SP_InsPremiumByWriterID={0},@SP_LoginID={1}",
                 CSubs.QSafeValue(Convert.ToString(id)), CSubs.QSafeValue(Convert.ToString(LoginID)));
                CompanyDetailDt = CSubs.GetDataTable(query);
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoginID), "InsuranceMasterDAL", "GetDetailById", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

            return CompanyDetailDt;

        }

        public IEnumerable<Insurance_Master> GetInsuranceMasterList(int pPageIndex, int pPageSize, string pOrderBy, int pOrder, int? CountryID, int? CityID, int? InsCompID, int? pisActive, string SearchKey, out int TotalCount)
        {
            TotalCount = 0;
            try
            {
                string query = string.Format("exec [Ins].[GetInsPremiumBtCoForGrid] @SP_PageIndex={0},@SP_PageSize={1},@SP_OrderBy={2},@SP_Order={3},@SP_InsCompID={4},@SP_isActive={5},@SP_SearchString={6},@SP_LoginID={7},@SP_CompID={8}",
                 CSubs.QSafeValue(Convert.ToString(pPageIndex)),
                CSubs.QSafeValue(Convert.ToString(pPageSize)),
                CSubs.QSafeValue(pOrderBy),
                CSubs.QSafeValue(Convert.ToString(pOrder)),
                CSubs.QSafeValue(Convert.ToString(InsCompID)),
                 CSubs.QSafeValue(Convert.ToString(pisActive)),
                CSubs.QSafeValue(SearchKey),
                Convert.ToString(UserSession.GetUserSession().LoginID),
                Convert.ToString(UserSession.GetUserSession().CompanyID)
                );

                DataTable dataTable = CSubs.GetDataTable(query);
                if (dataTable != null && dataTable.Rows.Count > 0)
                {
                    TotalCount = Convert.ToInt32(Convert.ToString(dataTable.Rows[0]["TotalRows"]));
                }

                var result = (from rw in dataTable.AsEnumerable()
                              select new Insurance_Master()
                              {
                                  Ins_M_Id = Convert.ToInt32(rw["InsPremiumByWriterID"]),
                                  Ins_Name = Convert.ToString(rw["InsCompName"]),
                                  Policy_No = Convert.ToString(rw["PolicyNumber"]),
                                  Policy_Date = Convert.ToDateTime(rw["PolicyFromDate"]),
                                  Prem_Percent_Amt = Convert.ToDecimal(rw["PremiumPercent"]),
                                  Service_Tax = Convert.ToDecimal(rw["Service_Tax"]),
                                  Stamp_Duty = Convert.ToDecimal(rw["StampDuty"]),
                                  Min_Prem = Convert.ToDecimal(rw["Min_Prem"]),
                                  Sum_Ins = Convert.ToDecimal(rw["InsuredAmt"]),
                                  Premium_Amt = Convert.ToDecimal(rw["OpenPremiumAmt"]),
                                  Cheq_Date = Convert.ToDateTime(rw["ChqDate"]),
                                  Cheq_No = Convert.ToString(rw["ChqNo"]),
                                  Bal_Prem = Convert.ToDecimal(rw["Bal_Prem"]),
                                  Bal_SI = Convert.ToDecimal(rw["Bal_SI"]),
                                  IsActive=Convert.ToBoolean(rw["IsActive"]),
                                  FileID =string.IsNullOrWhiteSpace(Convert.ToString(rw["FileID"])) ? 0 : Convert.ToInt32(rw["FileID"]),
                                  CompID = Convert.ToInt32(rw["CompID"]),
                              }).ToList();


                return result;
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(UserSession.GetUserSession().LoginID), "InsuranceMasterDAL", "GetInsuranceMasterList", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

            //return null;

        }

        public JobDocument GetDownloadFile(int FileID, int LoginID)
        {
            JobDocument job = new JobDocument();
            try
            {
                DataTable dt = CSubs.GetDataTable(string.Format("exec [Ins].[GetInsurancePremiumByWriterDoc] @SP_Fileid={0},@SP_LoginID={1}", CSubs.QSafeValue(Convert.ToString(FileID)), CSubs.QSafeValue(Convert.ToString(LoginID))));

                if (dt != null && dt.Rows.Count > 0)
                {
                    job.FileID = Convert.ToInt32(dt.Rows[0]["FileID"]);
                    job.FilePath = Convert.ToString(dt.Rows[0]["DocFilePath"]);
                    job.FileName = Convert.ToString(dt.Rows[0]["DocFileName"]);
                }

            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoginID), "InsuranceMasterDAL", "GetDownloadFile", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

            return job;

        }
    }
}