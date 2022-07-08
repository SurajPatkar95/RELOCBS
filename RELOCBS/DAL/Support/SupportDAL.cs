using RELOCBS.App_Code;
using RELOCBS.Common.ExceptionHandling;
using System;
using System.Data;

namespace RELOCBS.DAL.Support
{
    public class SupportDAL
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

        public bool RemoveInvApproval(Entities.Support SupportObj, int LoginID, out string result)
        {
            result = string.Empty;
            try
            {
                using (CDALSQL conn = new CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {
                        conn.AddCommand("[Support].[RemoveInvApproval]", QueryType.Procedure);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LoginID", SqlDbType.Int, 0, ParameterDirection.Input, LoginID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_InvNo", SqlDbType.VarChar, 20, ParameterDirection.Input, SupportObj.RemoveInvApproval.InvNo);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@RETURNSTATUS", SqlDbType.Int, 0, ParameterDirection.ReturnValue);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@Out_Message", SqlDbType.VarChar, 1000, ParameterDirection.Output);
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
                throw new DataAccessException(Convert.ToString(LoginID), "SupportDAL", "RemoveInvApproval", Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
        }

        public bool ChangeCityInJob(Entities.Support SupportObj, int LoginID, out string result)
        {
            result = string.Empty;
            try
            {
                using (CDALSQL conn = new CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {
                        conn.AddCommand("[Support].[ChangeCityInJob]", QueryType.Procedure);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LoginID", SqlDbType.Int, 0, ParameterDirection.Input, LoginID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_JobNo", SqlDbType.VarChar, 20, ParameterDirection.Input, SupportObj.ChangeCityInJob.JobNo);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_OrgOrDest", SqlDbType.VarChar, 5, ParameterDirection.Input, SupportObj.ChangeCityInJob.OrgOrDest);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_NewCityID", SqlDbType.BigInt, 0, ParameterDirection.Input, SupportObj.ChangeCityInJob.NewCityID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@RETURNSTATUS", SqlDbType.Int, 0, ParameterDirection.ReturnValue);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@Out_Message", SqlDbType.VarChar, 1000, ParameterDirection.Output);
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
                throw new DataAccessException(Convert.ToString(LoginID), "SupportDAL", "ChangeCityInJob", Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
        }

        public bool ChangeRefInInv(Entities.Support SupportObj, int LoginID, out string result)
        {
            result = string.Empty;
            try
            {
                using (CDALSQL conn = new CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {
                        conn.AddCommand("[Support].[ChangeRefInInv]", QueryType.Procedure);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LoginID", SqlDbType.Int, 0, ParameterDirection.Input, LoginID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_InvNo", SqlDbType.VarChar, 20, ParameterDirection.Input, SupportObj.ChangeRefInInv.InvNo);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_NewRefNo", SqlDbType.VarChar, 100, ParameterDirection.Input, SupportObj.ChangeRefInInv.NewRefNo);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@RETURNSTATUS", SqlDbType.Int, 0, ParameterDirection.ReturnValue);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@Out_Message", SqlDbType.VarChar, 1000, ParameterDirection.Output);
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
                throw new DataAccessException(Convert.ToString(LoginID), "SupportDAL", "ChangeRefInInv", Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
        }

        public bool ChangeRevBr(Entities.Support SupportObj, int LoginID, out string result)
        {
            result = string.Empty;
            try
            {
                using (CDALSQL conn = new CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {
                        conn.AddCommand("[Support].[ChangeRevBr]", QueryType.Procedure);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LoginID", SqlDbType.Int, 0, ParameterDirection.Input, LoginID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_JobNo", SqlDbType.VarChar, 20, ParameterDirection.Input, SupportObj.ChangeRevBr.JobNo);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_RevBrID", SqlDbType.VarChar, 15, ParameterDirection.Input, SupportObj.ChangeRevBr.RevBrID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@RETURNSTATUS", SqlDbType.Int, 0, ParameterDirection.ReturnValue);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@Out_Message", SqlDbType.VarChar, 1000, ParameterDirection.Output);
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
                throw new DataAccessException(Convert.ToString(LoginID), "SupportDAL", "ChangeRevBr", Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
        }
    }
}