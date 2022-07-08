using RELOCBS.App_Code;
using RELOCBS.Common.ExceptionHandling;
using RELOCBS.Utility;
using System;
using System.Data;

namespace RELOCBS.DAL.WOSCommon
{
    public class WOSServiceDAL
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

        public DataSet GetWOSServiceList(int LoginID)
        {
            DataSet WOSServiceDs = null;
            try
            {
                string query = string.Format("EXEC [WOS].[GetServiceMasterForGrid] @SP_LoginID={0}", LoginID);
                WOSServiceDs = CSubs.GetDataSet(query);
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoginID), "WOSServiceDAL", "GetWOSServiceList", Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
            return WOSServiceDs;
        }

        public DataSet GetWOSServiceById(int LoginID, int? ServiceMastID)
        {
            DataSet WOSServiceDs = new DataSet();
            try
            {
                string query = string.Format("EXEC [WOS].[GetWOSServiceById] @SP_LoginID={0}, @SP_ServiceMastID={1}", UserSession.GetUserSession().LoginID, CSubs.QSafeValue(Convert.ToString(ServiceMastID)));
                WOSServiceDs = CSubs.GetDataSet(query);
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(UserSession.GetUserSession().LoginID), "WOSServiceDAL", "GetWOSServiceById", Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
            return WOSServiceDs;
        }

        public bool SaveWOSService(Entities.WOSService WOSServiceObj, out string result)
        {
            result = string.Empty;
            try
            {
                using (CDALSQL conn = new CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {
                        conn.AddCommand("[WOS].[AddEditServiceMaster]", QueryType.Procedure);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LoginID", SqlDbType.Int, 0, ParameterDirection.Input, UserSession.GetUserSession().LoginID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ServiceMastID", SqlDbType.Int, 0, ParameterDirection.InputOutput, WOSServiceObj.ServiceMastID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ServiceName", SqlDbType.VarChar, 100, ParameterDirection.Input, WOSServiceObj.ServiceName);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IsActive", SqlDbType.Bit, 0, ParameterDirection.Input, WOSServiceObj.IsActive);

                        conn.AddParameter(ParameterOF.PROCEDURE, "@RETURNSTATUS", SqlDbType.SmallInt, 0, ParameterDirection.ReturnValue);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_OutMsg", SqlDbType.NVarChar, 500, ParameterDirection.Output);
                        conn.ExecuteProcedure(ProcedureReturnType.SingleValue);

                        if (!conn.IsError)
                        {
                            int ReturnStatus = Convert.ToInt32(conn.GetParameterValue(ParameterOF.PROCEDURE, "@RETURNSTATUS"));
                            result = Convert.ToString(conn.GetParameterValue(ParameterOF.PROCEDURE, "@SP_OutMsg"));

                            if (ReturnStatus == 0)
                            {
                                WOSServiceObj.ServiceMastID = Convert.ToInt32(conn.GetParameterValue(ParameterOF.PROCEDURE, "@SP_ServiceMastID"));
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
                throw new DataAccessException(Convert.ToString(UserSession.GetUserSession().LoginID), "WOSServiceDAL", "SaveWOSService", Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
        }
    }
}