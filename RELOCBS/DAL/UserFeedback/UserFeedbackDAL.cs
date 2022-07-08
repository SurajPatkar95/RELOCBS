using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using RELOCBS.App_Code;
using RELOCBS.Common.ExceptionHandling;
using RELOCBS.Entities;
using RELOCBS.Utility;

namespace RELOCBS.DAL.UserFeedback
{
    public class UserFeedbackDAL
    {
        private CommonSubs _cSubs;
        public CommonSubs CSubs
        {
            get
            {
                if (this._cSubs == null)
                    this._cSubs = new CommonSubs();
                return this._cSubs;
            }
        }
        

        public DataSet GetFeedbackQuestions(int LoginID)
        {
            DataSet questionsDs = new DataSet();

            try
            {
                questionsDs = CSubs.GetDataSet(string.Format("[feedback].[GetTemplateForUserFeedback] @SP_LoginID={0}", CSubs.QSafeValue(Convert.ToString(LoginID))));
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoginID), "UserFeedbackDAL", "GetFeedbackQuestions", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }


            return questionsDs;

        }

        public DataTable CheckUserFeedbackStatus(int LoginID)
        {
            DataTable UserfeedbackStatusDt = new DataTable();

            try
            {
                UserfeedbackStatusDt = CSubs.GetDataTable(string.Format("[feedback].[CheckUserFeedbackStatus] @SP_LoginID={0}", CSubs.QSafeValue(Convert.ToString(LoginID))));
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoginID), "UserFeedbackDAL", "CheckUserFeedbackStatus", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

            return UserfeedbackStatusDt;

        }

        public  bool SumbitUserFeedback(Entities.UserFeedback model,int LoginID, out string message)
        {
            message = string.Empty;
            try
            {
                using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {


                        conn.AddCommand("[feedback].[AddUserFeedback]", QueryType.Procedure);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_TemplateID", SqlDbType.BigInt, 0, ParameterDirection.Input, model.TemplateID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_QuestAnsXml", SqlDbType.Xml, -1, ParameterDirection.Input,model.AnswerList);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LOGINID", SqlDbType.Int, 0, ParameterDirection.Input, LoginID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@RETURNSTATUS", SqlDbType.SmallInt, 0, ParameterDirection.ReturnValue);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@Out_Message", SqlDbType.NVarChar, 500, ParameterDirection.Output);
                        conn.ExecuteProcedure(ProcedureReturnType.SingleValue);

                        if (!conn.IsError)
                        {
                            int ReturnStatus = Convert.ToInt32(conn.GetParameterValue(ParameterOF.PROCEDURE, "@RETURNSTATUS"));
                            message = Convert.ToString(conn.GetParameterValue(ParameterOF.PROCEDURE, "@Out_Message"));

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
                throw new DataAccessException(Convert.ToString(LoginID), "UserFeedbackDAL", "SumbitUserFeedback", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
        }
    }
}