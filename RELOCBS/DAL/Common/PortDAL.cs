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
    public class PortDAL
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

        public bool Insert(Port data, out string result)
        {
            result = string.Empty;

            try
            {

                using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {

                        conn.AddCommand("[Comm].[AddEditPort]", QueryType.Procedure);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_PortID", SqlDbType.Int, 0, ParameterDirection.InputOutput, (data.PortID <= 0 ? -1 : data.PortID));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_PortCode", SqlDbType.NVarChar, 50, ParameterDirection.Input, CSubs.PSafeValue(data.PortCode));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_PortName", SqlDbType.NVarChar, 100, ParameterDirection.Input, CSubs.PSafeValue(data.PortName));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@Sp_seaorair", SqlDbType.VarChar, 1, ParameterDirection.Input, CSubs.PSafeValue(data.AirorSea));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@Sp_ModeID", SqlDbType.Int, 0, ParameterDirection.Input, (data.ModeID));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@Sp_CountryID", SqlDbType.Int, 0, ParameterDirection.Input, (data.CountryID));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@Sp_CityID", SqlDbType.Int, 0, ParameterDirection.Input, (data.CityID));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IsActive", SqlDbType.Bit, 0, ParameterDirection.Input, data.Isactive);
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
                throw new DataAccessException(Convert.ToString(UserSession.GetUserSession().LoginID), "PortDAL", "Insert", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
            //return true;
        }

        public bool Update(Port data, out string result)
        {
            result = string.Empty;
            try
            {
                using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {

                        conn.AddCommand("[Comm].[AddEditPort]", QueryType.Procedure);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_PortID", SqlDbType.Int, 0, ParameterDirection.InputOutput, (data.PortID <= 0 ? -1 : data.PortID));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_PortCode", SqlDbType.NVarChar, 50, ParameterDirection.Input, CSubs.PSafeValue(data.PortCode));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_PortName", SqlDbType.NVarChar, 100, ParameterDirection.Input, CSubs.PSafeValue(data.PortName));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@Sp_seaorair", SqlDbType.VarChar, 1, ParameterDirection.Input, CSubs.PSafeValue(data.AirorSea));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@Sp_ModeID", SqlDbType.Int, 0, ParameterDirection.Input, (data.ModeID));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@Sp_CountryID", SqlDbType.Int, 0, ParameterDirection.Input, (data.CountryID));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@Sp_CityID", SqlDbType.Int, 0, ParameterDirection.Input, (data.CityID));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IsActive", SqlDbType.Bit, 0, ParameterDirection.Input, data.Isactive);
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
                throw new DataAccessException(Convert.ToString(UserSession.GetUserSession().LoginID), "PortDAL", "Update", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
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

                        conn.AddCommand("Comm.CityDelete", QueryType.Procedure);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CityID", SqlDbType.Int, 0, ParameterDirection.Input, id);
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
                throw new DataAccessException(Convert.ToString(UserSession.GetUserSession().LoginID), "PortDAL", "DeleteById", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

        }

        public DataTable GetDetailById(int? id, int LoginID)
        {
            DataTable CityDetailDt = new DataTable();

            try
            {
                string query = string.Format("exec [Comm].[GETPortDetail] @SP_PortID={0},@SP_LoginID={1}",
                 CSubs.QSafeValue(Convert.ToString(id)), CSubs.QSafeValue(Convert.ToString(LoginID)));
                CityDetailDt = CSubs.GetDataTable(query);
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoginID), "PortDAL", "GetDetailById", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

            return CityDetailDt;

        }

        public IEnumerable<Port> GetPortList(int pPageIndex, int pPageSize, string pOrderBy, int pOrder, int? pCityID,int modeid,string seaorair, int? pisActive, string SearchKey, int LoggedinUserID, out int TotalCount)
        {
            TotalCount = 0;
            try
            {
                string query = string.Format("exec Comm.GETPortForGrid @SP_PageIndex={0},@SP_PageSize={1},@SP_OrderBy={2},@SP_Order={3},@SP_CityID={4},@SP_isActive={5},@SP_SearchString={6},@SP_LoginID={7},@sp_seaorair={8},@sp_modeid={9}",
                 CSubs.QSafeValue(Convert.ToString(pPageIndex)),
                CSubs.QSafeValue(Convert.ToString(pPageSize)),
                CSubs.QSafeValue(pOrderBy),
                CSubs.QSafeValue(Convert.ToString(pOrder)),
                CSubs.QSafeValue(Convert.ToString(pCityID)),
                 CSubs.QSafeValue(Convert.ToString(pisActive)),
                CSubs.QSafeValue(SearchKey),
                Convert.ToString(LoggedinUserID),
                CSubs.QSafeValue(seaorair),
                CSubs.QSafeValue(Convert.ToString(modeid))
                );

                DataTable dataTable = CSubs.GetDataTable(query);
                if (dataTable != null && dataTable.Rows.Count > 0)
                {
                    TotalCount = Convert.ToInt32(Convert.ToString(dataTable.Rows[0]["TotalRows"]));
                }

                var result = (from rw in dataTable.AsEnumerable()
                              select new Port()
                              {
                                  PortName = Convert.ToString(rw["PortName"]),
                                  PortID = Convert.ToInt32(rw["PortID"]),
                                  CountryID = Convert.ToInt32(rw["CountryID"]),
                                  CityID = string.IsNullOrEmpty(Convert.ToString(rw["CityID"])) ? (int?)null : Convert.ToInt32(rw["CityID"]),
                                  PortCode = Convert.ToString(rw["PortCode"]),
                                  CountryName = Convert.ToString(rw["CountryName"]),
                                  CityName = Convert.ToString(rw["CityName"]),
                                  Isactive = Convert.ToBoolean(rw["isActive"]),
                                  AirorSea = Convert.ToString(rw["AirorSea"]),
                                  ModeID = Convert.ToInt32(rw["ModeID"]),
                              }).ToList();


                return result;
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoggedinUserID), "PortDAL", "GetPortList", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

            //return null;

        }


    }
}