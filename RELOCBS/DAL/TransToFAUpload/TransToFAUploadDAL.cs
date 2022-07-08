using Newtonsoft.Json;
using RELOCBS.App_Code;
using RELOCBS.Common.ExceptionHandling;
using RELOCBS.Entities;
using RELOCBS.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace RELOCBS.DAL.TransToFAUpload
{
    public class TransToFAUploadDAL
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

        public bool UploadTransFAOther(TransToFAUploadVM model, int LoginID, out string result)
        {
            result = String.Empty;

            try
            {
                
                using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString(), true, System.Data.IsolationLevel.ReadCommitted))
                {
                    if (conn.Connect())
                    {
                        conn.AddCommand("[E_Invoice].[UploadTransFAOther]", QueryType.Procedure);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_AppID", SqlDbType.Int, 0, ParameterDirection.Input, model.AppID);
                        int AppID = model.AppID != 2 && model.AppID != 3  ? 1 : model.AppID;
                        Dictionary<int, String> tblName = new Dictionary<int, string>();
                        tblName.Add(1, "@SP_TransFAOther");
                        tblName.Add(2, "@SP_TransFAReloSmart");
                        tblName.Add(3, "@SP_TransFAReloTracker");
                        string TableTypeParamName = tblName[AppID];
                        conn.AddParameter(ParameterOF.PROCEDURE, TableTypeParamName, SqlDbType.Structured, 0, ParameterDirection.Input, model.dtTable);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LOGINID", SqlDbType.Int, 0, ParameterDirection.Input, LoginID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@RETURNSTATUS", SqlDbType.SmallInt, 0, ParameterDirection.ReturnValue);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_MESSAGE", SqlDbType.NVarChar, 500, ParameterDirection.Output);

                        conn.ExecuteProcedure(ProcedureReturnType.SingleValue);
                        if (!conn.IsError)
                        {
                            int ReturnStatus = Convert.ToInt32(conn.GetParameterValue(ParameterOF.PROCEDURE, "@RETURNSTATUS"));
                            result = Convert.ToString(conn.GetParameterValue(ParameterOF.PROCEDURE, "@SP_MESSAGE"));

                            if (ReturnStatus == 0)
                            {
                                conn.CommitTransaction();
                                return true;
                            }
                            else
                            {
                                conn.RollbackTransaction();
                                return false;
                            }
                        }
                        else
                        {

                            result = conn.ErrorMessage;
                            conn.RollbackTransaction();
                            throw new ArgumentException(result);
                        }

                    }
                    else
                        throw new Exception(conn.ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoginID), "TransToFAUploadDAL", "UploadTransFAOther", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

            return true;

        }

        public DataTable GetUploadFormat(int AppID,int LoginID)
        {
            try
            {
                string query = string.Format("exec [E_Invoice].[ForCombo_AppList] @SP_SearchType='RESOURCE',@SP_LoginID={0},@SP_AppId={1}",
                   Convert.ToString(LoginID),
                   CSubs.QSafeValue(Convert.ToString(AppID))
                   );
                DataTable dataTable = CSubs.GetDataTable(query);

                return dataTable;
            }
            catch (Exception ex)
            {

                throw new DataAccessException(Convert.ToString(LoginID), "TransToFAUploadDAL", "GetUploadFormat", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

        }

    }
}