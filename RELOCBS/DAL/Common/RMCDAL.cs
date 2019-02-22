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
    public class RMCDAL
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


        public bool Insert(RMCViewModel rmc, out string result)
        {
            result = string.Empty;

            try
            {

                using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {

                        conn.AddCommand("[Comm].[AddEditRMC]", QueryType.Procedure);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_RMCID", SqlDbType.Int, 0, ParameterDirection.InputOutput, rmc.RMCID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_RMCName", SqlDbType.NVarChar, 250, ParameterDirection.Input, CSubs.PSafeValue(rmc.RMCName));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CityID", SqlDbType.Int, 0, ParameterDirection.Input, rmc.CityID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CountryID", SqlDbType.Int, 0, ParameterDirection.Input, rmc.CountryID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IsActive", SqlDbType.Bit, 0, ParameterDirection.Input, rmc.isActive);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LOGINID", SqlDbType.Int, 0, ParameterDirection.Input, UserSession.GetUserSession().LoginID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@RETURNSTATUS", SqlDbType.SmallInt, 0, ParameterDirection.ReturnValue);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Message", SqlDbType.NVarChar, 500, ParameterDirection.Output);
                        conn.ExecuteProcedure(ProcedureReturnType.SingleValue);


                        if (!conn.IsError)
                        {
                            int ReturnStatus = Convert.ToInt32(conn.GetParameterValue(ParameterOF.PROCEDURE, "@RETURNSTATUS"));
                            result = Convert.ToString(conn.GetParameterValue(ParameterOF.PROCEDURE, "@SP_Message"));

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
                throw new DataAccessException(Convert.ToString(UserSession.GetUserSession().LoginID), "RMCDAL", "Insert", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
            //return true;
        }

        public bool Update(RMCViewModel rmc, out string result)
        {
            result = string.Empty;
            try
            {
                using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {

                        conn.AddCommand("[Comm].[AddEditRMC]", QueryType.Procedure);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_RMCID", SqlDbType.Int, 0, ParameterDirection.InputOutput, rmc.RMCID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_RMCName", SqlDbType.NVarChar, 250, ParameterDirection.Input, CSubs.PSafeValue(rmc.RMCName));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CityID", SqlDbType.Int, 0, ParameterDirection.Input, rmc.CityID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CountryID", SqlDbType.Int, 0, ParameterDirection.Input, rmc.CountryID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IsActive", SqlDbType.Bit, 0, ParameterDirection.Input, rmc.isActive);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LOGINID", SqlDbType.Int, 0, ParameterDirection.Input, UserSession.GetUserSession().LoginID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@RETURNSTATUS", SqlDbType.SmallInt, 0, ParameterDirection.ReturnValue);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Message", SqlDbType.NVarChar, 500, ParameterDirection.Output);
                        conn.ExecuteProcedure(ProcedureReturnType.SingleValue);

                        if (!conn.IsError)
                        {
                            int ReturnStatus = Convert.ToInt32(conn.GetParameterValue(ParameterOF.PROCEDURE, "@RETURNSTATUS"));
                            result = Convert.ToString(conn.GetParameterValue(ParameterOF.PROCEDURE, "@SP_Message"));
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
                throw new DataAccessException(Convert.ToString(UserSession.GetUserSession().LoginID), "RMCDAL", "Update", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
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

                        conn.AddCommand("Comm.RMCDelete", QueryType.Procedure);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_RMCID", SqlDbType.Int, 0, ParameterDirection.Input, id);
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
                throw new DataAccessException(Convert.ToString(UserSession.GetUserSession().LoginID), "RMCDAL", "DeleteById", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

        }

        public DataTable GetDetailById(int? id, int LoginID)
        {
            DataTable RMCDetailDt = new DataTable();

            try
            {
                string query = string.Format("exec Comm.GETCityForGrid @SP_CityID={0},@SP_LoginID={1}",
                 CSubs.QSafeValue(Convert.ToString(id)), CSubs.QSafeValue(Convert.ToString(LoginID)));
                RMCDetailDt = CSubs.GetDataTable(query);
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoginID), "RMCDAL", "GetDetailById", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

            return RMCDetailDt;

        }

        public IEnumerable<RMC> GetRMCList(int pPageIndex, int pPageSize, string pOrderBy, int pOrder, int? RateTypeGrpID, int? CountryID, int? CityID, int? pisActive, string SearchKey, int LoggedinUserID, out int TotalCount)
        {
            TotalCount = 0;
            try
            {
                string query = string.Format("exec Comm.GETRMCForGrid @SP_PageIndex={0},@SP_PageSize={1},@SP_OrderBy={2},@SP_Order={3},@SP_OriginCityID={4},@SP_DesitnationCityID={5},@SP_isActive={6},@SP_SearchString={7},@SP_LoginID={8}",
                 CSubs.QSafeValue(Convert.ToString(pPageIndex)),
                CSubs.QSafeValue(Convert.ToString(pPageSize)),
                CSubs.QSafeValue(pOrderBy),
                CSubs.QSafeValue(Convert.ToString(pOrder)),
                 CSubs.QSafeValue(Convert.ToString(RateTypeGrpID)),
                CSubs.QSafeValue(Convert.ToString(CountryID)),
                CSubs.QSafeValue(Convert.ToString(CityID)),
                 CSubs.QSafeValue(Convert.ToString(pisActive)),
                CSubs.QSafeValue(SearchKey),
                Convert.ToString(LoggedinUserID)
                );

                DataTable dataTable = CSubs.GetDataTable(query);
                if (dataTable != null && dataTable.Rows.Count > 0)
                {
                    TotalCount = Convert.ToInt32(Convert.ToString(dataTable.Rows[0]["TotalRows"]));
                }

                var result = (from rw in dataTable.AsEnumerable()
                              select new RMC()
                              {
                                  RMCName = Convert.ToString(rw["RMCName"]),
                                  ShortRMCName = Convert.ToString(rw["ShortRMCName"]),
                                  RMCID = Convert.ToInt32(rw["RMCID"]),
                                  CityID = Convert.ToInt32(rw["CityID"]),
                                  CountryID = Convert.ToInt32(rw["CountryID"]),
                                  CityName = Convert.ToString(rw["CityName"]),
                                  CountryName = Convert.ToString(rw["CountryName"]),
                                  isActive = Convert.ToBoolean(rw["isActive"])
                              }).ToList();


                return result;
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoggedinUserID), "RMCDAL", "GetRMCList", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

            //return null;

        }


    }
}