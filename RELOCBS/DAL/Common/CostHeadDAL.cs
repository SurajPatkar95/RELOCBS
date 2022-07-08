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
    public class CostHeadDAL
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


        public bool Insert(CostHeadMaster comp, out string result)
        {
            result = string.Empty;

            try
            {

                using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {
                        conn.AddCommand("[Comm].[AddEditCostHead]", QueryType.Procedure);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CostHeadName", SqlDbType.VarChar, 100, ParameterDirection.Input, CSubs.PSafeValue(comp.CostHeadName));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@sp_SSCCode", SqlDbType.VarChar, 100, ParameterDirection.Input, CSubs.PSafeValue(comp.SSCCode));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@sp_FinanceCode", SqlDbType.VarChar, 20, ParameterDirection.Input, CSubs.PSafeValue(comp.FinanceCode));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@sp_ItemCode", SqlDbType.VarChar, 20, ParameterDirection.Input, CSubs.PSafeValue(comp.ItemCode));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@sp_InvDescription", SqlDbType.VarChar, 20, ParameterDirection.Input, CSubs.PSafeValue(comp.InvDescription));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_HasSubCostHead", SqlDbType.Bit, 0, ParameterDirection.Input, comp.HasSubCostHead);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IsGSTApplicable", SqlDbType.Bit, 0, ParameterDirection.Input, comp.IsGSTApplicable);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IsActive", SqlDbType.Bit, 0, ParameterDirection.Input, comp.IsActive);
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
                            throw new Exception(conn.ErrorMessage);

                    }
                    else
                        throw new Exception(conn.ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(UserSession.GetUserSession().LoginID), "CostHeadDAL", "Insert", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
            //return true;
        }

        public bool Update(CostHeadMaster comp, out string result)
        {
            result = string.Empty;
            try
            {
                using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {

                        conn.AddCommand("[Comm].[AddEditCostHead]", QueryType.Procedure);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CostHeadID", SqlDbType.Int, 0, ParameterDirection.InputOutput, comp.CostHeadID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CostHeadName", SqlDbType.VarChar, 100, ParameterDirection.Input, CSubs.PSafeValue(comp.CostHeadName));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@sp_SSCCode", SqlDbType.VarChar, 50, ParameterDirection.Input, CSubs.PSafeValue(comp.SSCCode));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@sp_FinanceCode", SqlDbType.VarChar, 50, ParameterDirection.Input, CSubs.PSafeValue(comp.FinanceCode));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@sp_ItemCode", SqlDbType.VarChar, 50, ParameterDirection.Input, CSubs.PSafeValue(comp.ItemCode));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@sp_InvDescription", SqlDbType.VarChar, 250, ParameterDirection.Input, CSubs.PSafeValue(comp.InvDescription));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_HasSubCostHead", SqlDbType.Bit, 0, ParameterDirection.Input, comp.HasSubCostHead);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IsGSTApplicable", SqlDbType.Bit, 0, ParameterDirection.Input, comp.IsGSTApplicable);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IsActive", SqlDbType.Bit, 0, ParameterDirection.Input, comp.IsActive);
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
                throw new DataAccessException(Convert.ToString(UserSession.GetUserSession().LoginID), "CostHeadDAL", "Update", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
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

                        conn.AddCommand("Comm.CostHeadDelete", QueryType.Procedure);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CostHeadID", SqlDbType.Int, 0, ParameterDirection.Input, id);
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
                throw new DataAccessException(Convert.ToString(UserSession.GetUserSession().LoginID), "CostHeadDAL", "DeleteById", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

        }

        public DataTable GetDetailById(int? id, int LoginID)
        {
            DataTable CompanyDetailDt = new DataTable();

            try
            {
                string query = string.Format("exec [Comm].[GETCostHeadDetail] @SP_CostheadID={0},@SP_LoginID={1}",
                 CSubs.QSafeValue(Convert.ToString(id)), CSubs.QSafeValue(Convert.ToString(LoginID)));
                CompanyDetailDt = CSubs.GetDataTable(query);
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoginID), "CostHeadDAL", "GetDetailById", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

            return CompanyDetailDt;

        }

        public IEnumerable<CostHeadMaster> GetList(int pPageIndex, int pPageSize, string pOrderBy, int pOrder, string SearchKey, int LoggedinUserID, out int TotalCount)
        {
            TotalCount = 0;
            try
            {
                string query = string.Format("exec Comm.GETCostHeadForGrid @SP_PageIndex={0},@SP_PageSize={1},@SP_OrderBy={2},@SP_Order={3},@SP_SearchString={4},@SP_LoginID={5}",
                 CSubs.QSafeValue(Convert.ToString(pPageIndex)),
                CSubs.QSafeValue(Convert.ToString(pPageSize)),
                CSubs.QSafeValue(pOrderBy),
                CSubs.QSafeValue(Convert.ToString(pOrder)),
                CSubs.QSafeValue(SearchKey),
                Convert.ToString(LoggedinUserID)
                );

                DataTable dataTable = CSubs.GetDataTable(query);
                if (dataTable != null && dataTable.Rows.Count > 0)
                {
                    TotalCount = Convert.ToInt32(Convert.ToString(dataTable.Rows[0]["TotalRows"]));
                }

                var result = (from rw in dataTable.AsEnumerable()
                              select new CostHeadMaster()
                              {
                                  CostHeadID = Convert.ToInt32(rw["CostHeadID"]),
                                  CostHeadName = Convert.ToString(rw["CostHeadName"]),
                                  SSCCode = Convert.ToString(rw["SSCCode"]),
                                  ItemCode = Convert.ToString(rw["ItemCode"]),
                                  FinanceCode = Convert.ToString(rw["FinanceCode"]),
                                  InvDescription = Convert.ToString(rw["InvDescription"]),
                                  HasSubCostHead = Convert.ToBoolean(rw["HasSubCostHead"]),
                                  IsGSTApplicable = Convert.ToBoolean(rw["IsGSTApplicable"]),
                                  IsActive = Convert.ToBoolean(rw["IsActive"]),

                              }).ToList();


                return result;
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoggedinUserID), "CostHeadDAL", "GetList", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

            //return null;

        }

    }
}