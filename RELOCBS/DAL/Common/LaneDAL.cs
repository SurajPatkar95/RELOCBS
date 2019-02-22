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
    public class LaneDAL
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

        public bool Insert(LaneViewModel lane, out string result)
        {
            result = string.Empty;

            try
            {

                using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {

                        conn.AddCommand("[Comm].[AddEditLane]", QueryType.Procedure);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LaneID", SqlDbType.Int, 0, ParameterDirection.InputOutput, lane.LaneId);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LaneName", SqlDbType.NVarChar, 250, ParameterDirection.Input, CSubs.PSafeValue(lane.LaneName));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_OriginCityID", SqlDbType.Int, 0, ParameterDirection.Input, lane.OriginCityID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_DestinationCityID", SqlDbType.Int, 0, ParameterDirection.Input, lane.DestinationCityID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IsActive", SqlDbType.Bit, 0, ParameterDirection.Input, lane.isActive);
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
                throw new DataAccessException(Convert.ToString(UserSession.GetUserSession().LoginID), "LaneDAL", "Insert", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
            //return true;
        }

        public bool Update(LaneViewModel lane, out string result)
        {
            result = string.Empty;
            try
            {
                using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {

                        conn.AddCommand("[Comm].[AddEditLane]", QueryType.Procedure);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LaneID", SqlDbType.Int, 0, ParameterDirection.InputOutput, lane.LaneId);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LaneName", SqlDbType.NVarChar, 250, ParameterDirection.Input, CSubs.PSafeValue(lane.LaneName));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_OriginCityID", SqlDbType.Int, 0, ParameterDirection.Input, lane.OriginCityID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_DestinationCityID", SqlDbType.Int, 0, ParameterDirection.Input, lane.DestinationCityID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IsActive", SqlDbType.Bit, 0, ParameterDirection.Input, lane.isActive);
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
                throw new DataAccessException(Convert.ToString(UserSession.GetUserSession().LoginID), "LaneDAL", "Update", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
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

                        conn.AddCommand("Comm.LaneDelete", QueryType.Procedure);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LaneID", SqlDbType.Int, 0, ParameterDirection.Input, id);
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
                throw new DataAccessException(Convert.ToString(UserSession.GetUserSession().LoginID), "LaneDAL", "DeleteById", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

        }

        public DataTable GetDetailById(int? id, int LoginID)
        {
            DataTable LaneDetailDt = new DataTable();

            try
            {
                string query = string.Format("exec Comm.GETCityForGrid @SP_CityID={0},@SP_LoginID={1}",
                 CSubs.QSafeValue(Convert.ToString(id)), CSubs.QSafeValue(Convert.ToString(LoginID)));
                LaneDetailDt = CSubs.GetDataTable(query);
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoginID), "LaneDAL", "GetDetailById", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

            return LaneDetailDt;

        }

        public IEnumerable<Lane> GetLaneList(int pPageIndex, int pPageSize, string pOrderBy, int pOrder, int? OriginCountryID, int? DestinationCountryID, int? pisActive, string SearchKey, int LoggedinUserID, out int TotalCount)
        {
            TotalCount = 0;
            try
            {
                string query = string.Format("exec Comm.GETLaneForGrid @SP_PageIndex={0},@SP_PageSize={1},@SP_OrderBy={2},@SP_Order={3},@SP_OriginCityID={4},@SP_DesitnationCityID={5},@SP_isActive={6},@SP_SearchString={7},@SP_LoginID={8}",
                 CSubs.QSafeValue(Convert.ToString(pPageIndex)),
                CSubs.QSafeValue(Convert.ToString(pPageSize)),
                CSubs.QSafeValue(pOrderBy),
                CSubs.QSafeValue(Convert.ToString(pOrder)),
                CSubs.QSafeValue(Convert.ToString(OriginCountryID)),
                CSubs.QSafeValue(Convert.ToString(DestinationCountryID)),
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
                              select new Lane()
                              {
                                  LaneName = Convert.ToString(rw["LaneName"]),
                                  ShortLaneName = Convert.ToString(rw["ShortLaneName"]),
                                  LaneId = Convert.ToInt32(rw["LaneID"]),
                                  OriginCityID  = Convert.ToInt32(rw["OrgCityID"]),
                                  DestinationCityID = Convert.ToInt32(rw["DestCityID"]),
                                  OriginCity = Convert.ToString(rw["OCityName"]),
                                  DestinationCity = Convert.ToString(rw["DCityName"]),
                                  isActive = Convert.ToBoolean(rw["isActive"])
                              }).ToList();


                return result;
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoggedinUserID), "LaneDAL", "GetLaneList", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

            //return null;

        }
    }
}