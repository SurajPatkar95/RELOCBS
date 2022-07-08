using RELOCBS.App_Code;
using RELOCBS.Common.ExceptionHandling;
using RELOCBS.Entities;
using RELOCBS.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace RELOCBS.DAL.Common
{
    public class VendorEvolQuestionDAL
    {
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

        public bool Insert(JobVendorEvalQuestion model, out string result)
        {
            result = string.Empty;

            try
            {

                using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {

                        conn.AddCommand("[MoveMan].[AddEditVendorEvolQuestionMaster] ", QueryType.Procedure);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_QuestionID", SqlDbType.Int, 0, ParameterDirection.InputOutput, -1);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_QuestDetail", SqlDbType.NVarChar, 500, ParameterDirection.Input, CSubs.PSafeValue(model.QuestionDetail));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CompanyID", SqlDbType.Int, 0, ParameterDirection.Input, model.CompanyID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_RateCompID", SqlDbType.Int, 0, ParameterDirection.Input, model.RateCompID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IsRMCBuss", SqlDbType.Bit, 0, ParameterDirection.Input, model.IsRMCBuss);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_WeightAge", SqlDbType.Int, 0, ParameterDirection.Input, model.Weightage);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IsActive", SqlDbType.Bit, 0, ParameterDirection.Input, model.IsActive);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LOGINID", SqlDbType.Int, 0, ParameterDirection.Input, UserSession.GetUserSession().LoginID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@RETURNSTATUS", SqlDbType.SmallInt, 0, ParameterDirection.ReturnValue);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@OutMsg", SqlDbType.NVarChar, 500, ParameterDirection.Output);
                        conn.ExecuteProcedure(ProcedureReturnType.SingleValue);

                        if (!conn.IsError)
                        {
                            int ReturnStatus = Convert.ToInt32(conn.GetParameterValue(ParameterOF.PROCEDURE, "@RETURNSTATUS"));
                            result = Convert.ToString(conn.GetParameterValue(ParameterOF.PROCEDURE, "@OutMsg"));
                            
                            if (ReturnStatus == 0)
                            {
                                model.QuestionID = Convert.ToInt32(conn.GetParameterValue(ParameterOF.PROCEDURE, "@SP_QuestionID"));
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
                throw new DataAccessException(Convert.ToString(UserSession.GetUserSession().LoginID), "VendorEvolQuestionDAL", "Insert", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
            //return true;
        }

        public bool Update(JobVendorEvalQuestion model, out string result)
        {
            result = string.Empty;
            try
            {
                using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {


                        conn.AddCommand("[MoveMan].[AddEditVendorEvolQuestionMaster] ", QueryType.Procedure);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_QuestionID", SqlDbType.Int, 0, ParameterDirection.InputOutput, model.QuestionID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_QuestDetail", SqlDbType.NVarChar, 500, ParameterDirection.Input, CSubs.PSafeValue(model.QuestionDetail));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CompanyID", SqlDbType.Int, 0, ParameterDirection.Input, model.CompanyID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_RateCompID", SqlDbType.Int, 0, ParameterDirection.Input, model.RateCompID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IsRMCBuss", SqlDbType.Bit, 0, ParameterDirection.Input, model.IsRMCBuss);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_WeightAge", SqlDbType.Int, 0, ParameterDirection.Input, model.Weightage);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IsActive", SqlDbType.Bit, 0, ParameterDirection.Input, model.IsActive);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LOGINID", SqlDbType.Int, 0, ParameterDirection.Input, UserSession.GetUserSession().LoginID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@RETURNSTATUS", SqlDbType.SmallInt, 0, ParameterDirection.ReturnValue);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@OutMsg", SqlDbType.NVarChar, 500, ParameterDirection.Output);
                        conn.ExecuteProcedure(ProcedureReturnType.SingleValue);
                        conn.ExecuteProcedure(ProcedureReturnType.SingleValue);

                        if (!conn.IsError)
                        {
                            int ReturnStatus = Convert.ToInt32(conn.GetParameterValue(ParameterOF.PROCEDURE, "@RETURNSTATUS"));
                            result = Convert.ToString(conn.GetParameterValue(ParameterOF.PROCEDURE, "@OutMsg"));
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
                            throw new ArgumentException(conn.ErrorMessage);

                    }
                    else
                        throw new ArgumentException(conn.ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(UserSession.GetUserSession().LoginID), "VendorEvolQuestionDAL", "Update", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
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

                        conn.AddCommand("Comm.VendorDelete", QueryType.Procedure);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_VendorID", SqlDbType.Int, 0, ParameterDirection.Input, id);
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
                throw new DataAccessException(Convert.ToString(UserSession.GetUserSession().LoginID), "VendorEvolQuestionDAL", "DeleteById", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

        }

        public DataTable GetDetailById(int? id, int LoginID)
        {
            DataTable CrewDetailDt = new DataTable();

            try
            {
                string query = string.Format("exec [MoveMan].[GetVendorEvolQuestionMaster] @SP_QuestionID={0},@SP_LoginID={1}",
                 CSubs.QSafeValue(Convert.ToString(id)), CSubs.QSafeValue(Convert.ToString(LoginID)));
                CrewDetailDt = CSubs.GetDataTable(query);
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoginID), "VendorEvolQuestionDAL", "GetDetailById", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

            return CrewDetailDt;

        }

        public IEnumerable<JobVendorEvalQuestion> GetList(int pPageIndex, int pPageSize, string pOrderBy, int pOrder,int? pisActive, string SearchKey, int LoggedinUserID, out int TotalCount)
        {
            TotalCount = 0;
            try
            {
                int CompanyID = UserSession.GetUserSession().CompanyID;
                bool RMCBuss = UserSession.GetUserSession().BussinessLine != "NON RMC-BUSINESS";

                string query = string.Format("exec [MoveMan].[GetVendorEvolQuestionMasterGrid] @SP_PageIndex={0},@SP_PageSize={1},@SP_OrderBy={2},@SP_Order={3},@SP_isActive={4},@SP_SearchString={5},@SP_LoginID={6},@SP_CompID={7},@SP_IsRMCBuss={8}",
                CSubs.QSafeValue(Convert.ToString(pPageIndex)),
                CSubs.QSafeValue(Convert.ToString(pPageSize)),
                CSubs.QSafeValue(pOrderBy),
                CSubs.QSafeValue(Convert.ToString(pOrder)),
                CSubs.QSafeValue(Convert.ToString(pisActive)),
                CSubs.QSafeValue(SearchKey),
                Convert.ToString(LoggedinUserID),
                Convert.ToString(CompanyID),
                CSubs.QSafeValue(Convert.ToString(RMCBuss))
                );

                DataTable dataTable = CSubs.GetDataTable(query);
                if (dataTable != null && dataTable.Rows.Count > 0)
                {
                    TotalCount = Convert.ToInt32(Convert.ToString(dataTable.Rows[0]["TotalRows"]));
                }

                var result = (from rw in dataTable.AsEnumerable()
                              select new JobVendorEvalQuestion()
                              {
                                  QuestionID = Convert.ToInt32(rw["VendorEvolQstmasterID"]),
                                  QuestionDetail = Convert.ToString(rw["QuestionDet"]),
                                  IsRMCBuss = Convert.ToBoolean(rw["IsForRMC"]),
                                  CompanyID = Convert.ToInt32(rw["CompanyID"]),
                                  RateCompID = Convert.ToInt32(rw["MoveCompID"]),
                                  RateComp = Convert.ToString(rw["RateComponentName"]),
                                  Company = Convert.ToString(rw["CompanyName"]),
                                  Weightage = Convert.ToInt32(rw["Weightage"]),
                                  Order = Convert.ToInt32(rw["QuestionOrder"]),
                                  IsActive = Convert.ToBoolean(rw["IsActive"])
                              }).ToList();


                return result;
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoggedinUserID), "VendorEvolQuestionDAL", "GetList", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

            //return null;

        }
    }
}