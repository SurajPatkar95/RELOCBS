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
    public class CostHeadCommisionableForRMCDAL
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

        public bool Insert(CostHeadCommisionableForRMC model, out string result)
        {
            result = string.Empty;
            try
            {
                using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {

                        conn.AddCommand("[Comm].[AddEditRMCCommisionableCostHead]", QueryType.Procedure);
                        //conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CountryID", SqlDbType.Int, 0, ParameterDirection.InputOutput, country.CountryID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_RMCCommiCostID", SqlDbType.Int, 0, ParameterDirection.InputOutput, model.RMCCommiCostID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_RMCID", SqlDbType.Int, 0, ParameterDirection.Input, model.RMCID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CostHeadID", SqlDbType.Int, 0, ParameterDirection.Input, model.CostHeadID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LOGINID", SqlDbType.Int, 0, ParameterDirection.Input, UserSession.GetUserSession().LoginID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@RETURNSTATUS", SqlDbType.SmallInt, 0, ParameterDirection.ReturnValue);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@Out_Message", SqlDbType.NVarChar, 500, ParameterDirection.Output);
                        conn.ExecuteProcedure(ProcedureReturnType.SingleValue);

                        if (!conn.IsError)
                        {
                            int ReturnStatus = Convert.ToInt32(conn.GetParameterValue(ParameterOF.PROCEDURE, "@RETURNSTATUS"));
                            result = Convert.ToString(conn.GetParameterValue(ParameterOF.PROCEDURE, "@Out_Message"));

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
                throw new DataAccessException(Convert.ToString(UserSession.GetUserSession().LoginID), "CostHeadCommisionableForRMCDAL", "Insert", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
            return true;
        }

        public bool Update(CostHeadCommisionableForRMC model, out string result)
        {
            result = string.Empty;
            try
            {
                using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {

                        conn.AddCommand("[Comm].[AddEditRMCCommisionableCostHead]", QueryType.Procedure);
                        //conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CountryID", SqlDbType.Int, 0, ParameterDirection.InputOutput, country.CountryID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_RMCCommiCostID", SqlDbType.Int, 0, ParameterDirection.InputOutput, model.RMCCommiCostID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_RMCID", SqlDbType.Int, 0, ParameterDirection.Input, model.RMCID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CostHeadID", SqlDbType.Int, 0, ParameterDirection.Input, model.CostHeadID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LOGINID", SqlDbType.Int, 0, ParameterDirection.Input, UserSession.GetUserSession().LoginID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@RETURNSTATUS", SqlDbType.SmallInt, 0, ParameterDirection.ReturnValue);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@Out_Message", SqlDbType.NVarChar, 500, ParameterDirection.Output);
                        conn.ExecuteProcedure(ProcedureReturnType.SingleValue);

                        if (!conn.IsError)
                        {
                            int ReturnStatus = Convert.ToInt32(conn.GetParameterValue(ParameterOF.PROCEDURE, "@RETURNSTATUS"));
                            result = Convert.ToString(conn.GetParameterValue(ParameterOF.PROCEDURE, "@Out_Message"));

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
                throw new DataAccessException(Convert.ToString(UserSession.GetUserSession().LoginID), "CostHeadCommisionableForRMCDAL", "Update", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

            return true;
        }

        public bool DeleteById(int id, out string result)
        {
            try
            {
                using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {

                        conn.AddCommand("[Comm].[DeleteRMCCommisionableCostHead]", QueryType.Procedure);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_RMCCommiCostID", SqlDbType.Int, 0, ParameterDirection.Input, id);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LOGINID", SqlDbType.Int, 0, ParameterDirection.Input, UserSession.GetUserSession().LoginID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@RETURNSTATUS", SqlDbType.SmallInt, 0, ParameterDirection.ReturnValue);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@Out_Message", SqlDbType.NVarChar, 500, ParameterDirection.Output);
                        conn.ExecuteProcedure(ProcedureReturnType.SingleValue);

                        if (!conn.IsError)
                        {
                            int ReturnStatus = Convert.ToInt32(conn.GetParameterValue(ParameterOF.PROCEDURE, "@RETURNSTATUS"));
                            result = Convert.ToString(conn.GetParameterValue(ParameterOF.PROCEDURE, "@Out_Message"));

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
                throw new DataAccessException(Convert.ToString(UserSession.GetUserSession().LoginID), "CostHeadCommisionableForRMCDAL", "DeletedById", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }


            return true;
        }

        public DataTable GetDetailById(int? id, int LoginID)
        {
            DataTable CountryDetailDt = new DataTable();

            try
            {
                string query = string.Format("EXEC [Comm].[DeleteRMCCommisionableCostHead] @SP_RMCCommiCostID={0},@SP_LoginID={1}",
                 CSubs.QSafeValue(Convert.ToString(id)), CSubs.QSafeValue(Convert.ToString(LoginID)));
                CountryDetailDt = CSubs.GetDataTable(query);
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoginID), "CostHeadCommisionableForRMCDAL", "GetDetailById", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

            return CountryDetailDt;

        }

        public IEnumerable<CostHeadCommisionableForRMC> GetGridList(int pPageIndex, int pPageSize, string pOrderBy, int pOrder, int? pisActive, string SearchKey, int LoggedinUserID, out int TotalCount)
        {

            try
            {
                TotalCount = 0;

                string query = string.Format("exec [Comm].[GETRMCCommisionableCostHeadForGrid] @SP_PageIndex={0},@SP_PageSize={1},@SP_OrderBy={2},@SP_Order={3},@SP_isActive={4},@SP_SearchString={5},@SP_LoginID={6}",
                    CSubs.QSafeValue(Convert.ToString(pPageIndex)),
                    CSubs.QSafeValue(Convert.ToString(pPageSize)),
                    CSubs.QSafeValue(pOrderBy),
                    CSubs.QSafeValue(Convert.ToString(pOrder)),
                    CSubs.QSafeValue(Convert.ToString(pisActive)),
                    CSubs.QSafeValue(SearchKey),
                    Convert.ToString(LoggedinUserID)
                    );


                DataTable dataTable = CSubs.GetDataTable(query);
                if (dataTable != null && dataTable.Rows.Count > 0)
                {
                    TotalCount = Convert.ToInt32(Convert.ToString(dataTable.Rows[0]["TotalRows"]));
                }

                var result = (from rw in CSubs.GetDataTable(query).AsEnumerable()
                              select new CostHeadCommisionableForRMC()
                              {
                                  RMCCommiCostID = Convert.ToInt32(rw["RMCCommiCostID"]),
                                  CostHeadID = Convert.ToInt32(rw["CostHeadID"]),
                                  CostHeadName = Convert.ToString(rw["CostHeadName"]),
                                  RMCID = Convert.ToInt32(rw["RMCID"]),
                                  RMCName = Convert.ToString(rw["RMCName"]),
                                  Isactive = Convert.ToBoolean(rw["Isactive"])
                              }).ToList();


                return result;



            }
            catch (Exception ex)
            {

                throw new DataAccessException(Convert.ToString(LoggedinUserID), "CostHeadCommisionableForRMCDAL", "GetGridList", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex); ;
            }

            
        }

    }
}