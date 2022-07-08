using RELOCBS.App_Code;
using RELOCBS.Common.ExceptionHandling;
using RELOCBS.Utility;
using System;
using System.Data;

namespace RELOCBS.DAL.WOSCommon
{
    public class WOSSubServiceDAL
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

        public DataSet GetWOSSubServiceList(int LoginID)
        {
            DataSet WOSSubServiceDs = null;
            try
            {
                string query = string.Format("EXEC [WOS].[GetSubServiceMasterForGrid] @SP_LoginID={0}", LoginID);
                WOSSubServiceDs = CSubs.GetDataSet(query);
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoginID), "WOSSubServiceDAL", "GetWOSSubServiceList", Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
            return WOSSubServiceDs;
        }

        public DataSet GetWOSSubServiceById(int LoginID, int? SubServiceMastID)
        {
            DataSet WOSSubServiceDs = new DataSet();
            try
            {
                string query = string.Format("EXEC [WOS].[GetWOSSubServiceById] @SP_LoginID={0}, @SP_SubServiceMastID={1}", UserSession.GetUserSession().LoginID, CSubs.QSafeValue(Convert.ToString(SubServiceMastID)));
                WOSSubServiceDs = CSubs.GetDataSet(query);
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(UserSession.GetUserSession().LoginID), "WOSSubServiceDAL", "GetWOSSubServiceById", Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
            return WOSSubServiceDs;
        }

        public bool SaveWOSSubService(Entities.WOSSubService WOSSubServiceObj, out string result)
        {
            result = string.Empty;
            try
            {
                using (CDALSQL conn = new CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {
                        conn.AddCommand("[WOS].[AddEditSubServiceMaster]", QueryType.Procedure);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LoginID", SqlDbType.Int, 0, ParameterDirection.Input, UserSession.GetUserSession().LoginID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_SubServiceMastID", SqlDbType.Int, 0, ParameterDirection.InputOutput, WOSSubServiceObj.SubServiceMastID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ServiceMastID", SqlDbType.Int, 0, ParameterDirection.Input, WOSSubServiceObj.ServiceMastID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_SubServiceName", SqlDbType.VarChar, 100, ParameterDirection.Input, WOSSubServiceObj.SubServiceName);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IsActive", SqlDbType.Bit, 0, ParameterDirection.Input, WOSSubServiceObj.IsActive);

                        conn.AddParameter(ParameterOF.PROCEDURE, "@RETURNSTATUS", SqlDbType.SmallInt, 0, ParameterDirection.ReturnValue);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_OutMsg", SqlDbType.NVarChar, 500, ParameterDirection.Output);
                        conn.ExecuteProcedure(ProcedureReturnType.SingleValue);

                        if (!conn.IsError)
                        {
                            int ReturnStatus = Convert.ToInt32(conn.GetParameterValue(ParameterOF.PROCEDURE, "@RETURNSTATUS"));
                            result = Convert.ToString(conn.GetParameterValue(ParameterOF.PROCEDURE, "@SP_OutMsg"));

                            if (ReturnStatus == 0)
                            {
                                WOSSubServiceObj.SubServiceMastID = Convert.ToInt32(conn.GetParameterValue(ParameterOF.PROCEDURE, "@SP_SubServiceMastID"));
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
                throw new DataAccessException(Convert.ToString(UserSession.GetUserSession().LoginID), "WOSSubServiceDAL", "SaveWOSSubService", Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
        }
    }
}