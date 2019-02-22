using RELOCBS.App_Code;
using RELOCBS.DAL.Repository;
using RELOCBS.Entities;
using RELOCBS.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace RELOCBS.DAL.Account
{
    public class UserDAL : Repository<SysUser>, IDisposable
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

        public bool Insert(SysUser entity)
        {
            try
            {


            }
            catch (Exception ex)
            {
                throw ex;
            }
            return true;
        }

        public bool Update(SysUser entity)
        {
            try
            {

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return true;
        }

        public bool DeleteById(int id)
        {
            try
            {

            }
            catch (Exception ex)
            {
                throw ex;
            }


            return true;
        }

        public bool ValidateUser(string LoginText, string Password, bool IsADAuthenticated, out UserInformationModel user)
        {
            try
            {
                user = null;

                using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {


                        if (IsADAuthenticated)
                        {

                            DataTable dt = CSubs.GetDataTable("SELECT CAST(LOGINID AS NVARCHAR) LOGINID, LoginText FROM VW_ACCESSUSERLOGIN WHERE ISACTIVE=1 AND ADLOGINTEXT=" + CSubs.QSafeValue(LoginText));

                            if (dt==null || dt.Rows.Count<=0)
                            {
                                throw new ArgumentException("User does not have active login.Please Contact IT for Login");
                            }

                            if (dt.Rows.Count != 1)
                            {
                                throw new ArgumentException("Duplicate user Login.Please Contact IT for Login");
                            }
                            else
                            {

                                conn.AddCommand("Access.CheckLoginADWithOutPassword", QueryType.Procedure);
                                conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LOGINID", SqlDbType.Int, 0, ParameterDirection.Input, CSubs.PSafeValue(dt.Rows[0]["LOGINID"].ToString()));
                                DataTable dtlogin = new DataTable();
                                dtlogin = (DataTable)conn.ExecuteProcedure(ProcedureReturnType.DataTable);
                                if (!conn.IsError)
                                {
                                    if (dtlogin.Rows.Count > 0)
                                    {
                                        if (dtlogin.Rows[0][0].ToString() == "-1")
                                        {

                                            throw new ArgumentException(dtlogin.Rows[0][1].ToString());
                                        }
                                        else
                                        {
                                            UserInformationModel userModel = new UserInformationModel()
                                            {
                                                TempSessionID = HttpContext.Current.Session.SessionID,
                                                LoginID = Convert.ToInt32(dtlogin.Rows[0]["LoginID"].ToString()),
                                                UserName = dtlogin.Rows[0]["EmpName"].ToString(),
                                                LoginType = dtlogin.Rows[0]["LoginTypeCode"].ToString(),
                                                DaysLeft = Convert.ToInt32(dtlogin.Rows[0]["DAYSLEFT"].ToString()),
                                                IsDummy = dtlogin.Rows[0]["IsDummy"].ToString(),
                                                PasswordExpired = dtlogin.Rows[0]["PasswordExpired"].ToString(),
                                                //IsChangePassword = (Boolean)dtlogin.Rows[0][7],
                                                IsFirstLogin = (Boolean)dtlogin.Rows[0]["IsFirstLogin"]
                                            };

                                            user = userModel;

                                            //PerformLogin();
                                        }
                                    }
                                    else
                                        throw new ArgumentException("User details not found.");
                                }
                                else
                                    throw new ArgumentException(conn.ErrorMessage);


                            }


                        }
                        else
                        {
                            conn.AddCommand("Access.CheckLogin", QueryType.Procedure);
                            conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LOGINTEXT", SqlDbType.NVarChar, 300, ParameterDirection.Input, CSubs.PSafeValue(LoginText));
                            conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LOGINPASSWORD", SqlDbType.NVarChar, 20, ParameterDirection.Input, CSubs.PSafeValue(Password));

                            DataTable dtlogin = new DataTable();
                            dtlogin = (DataTable)conn.ExecuteProcedure(ProcedureReturnType.DataTable);
                            if (!conn.IsError)
                            {
                                if (dtlogin.Rows.Count > 0)
                                {
                                    if (dtlogin.Rows[0][0].ToString() == "-1")
                                    {
                                        throw new ArgumentException(dtlogin.Rows[0][1].ToString());
                                    }
                                    else
                                    {

                                        UserInformationModel userModel = new UserInformationModel()
                                        {
                                            TempSessionID = HttpContext.Current.Session.SessionID,
                                            LoginID = Convert.ToInt32(dtlogin.Rows[0]["LoginID"].ToString()),
                                            UserName = dtlogin.Rows[0]["EmpName"].ToString(),
                                            LoginType = dtlogin.Rows[0]["LoginTypeCode"].ToString(),
                                            DaysLeft = Convert.ToInt32(dtlogin.Rows[0]["DAYSLEFT"].ToString()),
                                            IsDummy = dtlogin.Rows[0]["IsDummy"].ToString(),
                                            PasswordExpired = dtlogin.Rows[0]["PasswordExpired"].ToString(),
                                            //IsChangePassword = (Boolean)dtlogin.Rows[0][7],
                                            IsFirstLogin = (Boolean)dtlogin.Rows[0]["IsFirstLogin"]
                                        };

                                        user = userModel;

                                        //PerformLogin();
                                    }
                                }
                                else
                                    throw new ArgumentException("Login details not found.");
                            }
                            else
                                throw new ArgumentException(conn.ErrorMessage);

                        }

                    }
                    else
                    {
                        throw new ArgumentException("Unable to connet to db server");
                    }
                }



            }
            catch (Exception ex)
            {
                throw ex;
            }

            return true;
        }

        public bool CheckUserADAuthentication(string UserName)
        {
            bool IsADAllowed = false;

            try
            {

                var Value = CSubs.GetValue(string.Format("SELECT 1 FROM ACCESS.USERLOGIN UL WHERE UL.IsActive=1 AND UL.AllowADLogin=1 AND UL.ADLOGINTEXT={0} OR UL.LoginText={0}", CSubs.QSafeValue(UserName)));

                if (Value != null)
                {
                    IsADAllowed = true;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return IsADAllowed;

        }

        public bool ChangePassword(UserInformationModel user, string OldPassword, string NewPassword)
        {
            bool result = false;
            try
            {
                using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["WSGDB"].ToString()))
                {
                    if (conn.Connect())
                    {
                        conn.AddCommand("Access.ChangePassword", QueryType.Procedure);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LOGINID", SqlDbType.Int, 0, ParameterDirection.Input, user.LoginID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_NEWPASSWORD", SqlDbType.NVarChar, 20, ParameterDirection.Input, CSubs.PSafeValue(CSubs.Decrypt(NewPassword)));
                        conn.ExecuteProcedure(ProcedureReturnType.SingleValue);
                        if (!conn.IsError)
                        {
                            return result = true;
                            // PerformLogin();
                        }
                        else
                            throw new ArgumentException(conn.ErrorMessage);
                    }
                    else
                        //  CtrlErrorMessageHolder.ShowMessage(conn.ErrorMessage, MessageType.ERROR, MessageDisplayType.INLINE);
                        throw new ArgumentException("UnExpected Error");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }

        public bool ForgetPassword(string LoginText, string NewPassword)
        {
            bool result = false;
            try
            {
                using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["WSGDB"].ToString()))
                {
                    if (conn.Connect())
                    {
                        conn.AddCommand("Access.ForgetPassword", QueryType.Procedure);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LOGINTEXT", SqlDbType.NVarChar, 300, ParameterDirection.Input, CSubs.PSafeValue(LoginText));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LOGINPASSWORD", SqlDbType.NVarChar, 20, ParameterDirection.Input, CSubs.PSafeValue(CSubs.GetRandomInt(5)));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_MESSAGE", SqlDbType.NVarChar, 500, ParameterDirection.Output);
                        conn.ExecuteProcedure(ProcedureReturnType.SingleValue);
                        if (!conn.IsError)
                        {
                            return result = true; //conn.GetParameterValue(ParameterOF.PROCEDURE, "@SP_MESSAGE").ToString());

                        }
                        else
                            //  CtrlErrorMessageHolder.ShowMessage(conn.ErrorMessage, MessageType.ERROR, MessageDisplayType.INLINE);
                            throw new ArgumentException("UnExpectedError");
                    }
                    else
                        //  CtrlErrorMessageHolder.ShowMessage(conn.ErrorMessage, MessageType.ERROR, MessageDisplayType.INLINE);
                        throw new ArgumentException("UnExpectedError");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

    }
}