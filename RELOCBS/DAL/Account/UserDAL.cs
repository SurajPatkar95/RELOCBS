using RELOCBS.App_Code;
using RELOCBS.Common.ExceptionHandling;
using RELOCBS.DAL.Repository;
using RELOCBS.Entities;
using RELOCBS.Models;
using RELOCBS.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Xml.Linq;

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

        public bool ValidateUser(string LoginText, string Password, bool IsADAuthenticated, out UserInformationModel user, out string Msg)
        {
            try
            {
                user = null;
                Msg = "";

                using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {


                        if (IsADAuthenticated)
                        {

                            DataTable dt = CSubs.GetDataTable("SELECT CAST(LOGINID AS NVARCHAR) LOGINID, LoginText FROM VW_ACCESSUSERLOGIN WHERE ISACTIVE=1 AND ADLOGINTEXT=" + CSubs.QSafeValue(LoginText));

                            if (dt == null || dt.Rows.Count <= 0)
                            {


                                Msg = "User does not have active login.Please Contact IT for Login";

                                return false;
                            }

                            if (dt.Rows.Count != 1)
                            {
                                Msg = "Duplicate user Login.Please Contact IT for Login";
                                return false;
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

                                            Msg = dtlogin.Rows[0][1].ToString();
                                            return false;
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
                                                IsFirstLogin = (Boolean)dtlogin.Rows[0]["IsFirstLogin"],
                                                EmpID = Convert.ToString(dtlogin.Rows[0]["EmpID"])
                                            };

                                            user = userModel;
                                            Msg = "Success";
                                            //PerformLogin();
                                        }
                                    }
                                    else
                                    {
                                        Msg = "User details not found.";
                                        return false;
                                    }
                                }
                                else
                                {

                                    Msg = "Unable to connect DB for Login"; //conn.ErrorMessage;
                                    return false;
                                }


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
                                        Msg = dtlogin.Rows[0][1].ToString();
                                        return false;
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
                                            IsFirstLogin = (Boolean)dtlogin.Rows[0]["IsFirstLogin"],
                                            EmpID = Convert.ToString(dtlogin.Rows[0]["EmpID"])
                                        };

                                        user = userModel;
                                        Msg = "Success";
                                        //PerformLogin();
                                    }
                                }
                                else
                                {
                                    Msg = "Login details not found.";
                                    return false;
                                }
                            }
                            else
                            {
                                Msg = "Unable to connect DB for Login"; //conn.ErrorMessage;
                                return false;
                            }

                        }

                    }
                    else
                    {
                        Msg = "Unable to connet to db server";
                        return false;
                    }
                }



            }
            catch (Exception ex)
            {
                throw new DataAccessException("", "UserDAL", "ValidateUser", ex.ToString(), ex);
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

        public bool ChangePassword(ChangePasswordViewModel model, out string Message)
        {
            bool result = false;
            try
            {
                using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {
                        conn.AddCommand("Access.ChangePassword", QueryType.Procedure);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LOGINID", SqlDbType.Int, 0, ParameterDirection.Input, model.UserID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_OLDPASSWORD", SqlDbType.NVarChar, 20, ParameterDirection.Input, CSubs.PSafeValue(model.OldPassword));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_NEWPASSWORD", SqlDbType.NVarChar, 20, ParameterDirection.Input, CSubs.PSafeValue(model.Password));
                        conn.ExecuteProcedure(ProcedureReturnType.SingleValue);
                        if (!conn.IsError)
                        {
                            Message = "Password changed succesfully";
                            return result = true;
                            // PerformLogin();
                        }
                        else
                        {
                            Message = conn.ErrorMessage;
                        }

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
                using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
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

        public void UpdateLoginStatus(UserInformationModel user, LoginStatus status)
        {
            try
            {
                CSubs.ExecuteQuery(String.Format("[Access].[UpdateLoginStatus] @SP_LoginID={0}, @SP_LoginSessionID={1}, @SP_INOUT={2}, @SP_ClientIP={3}", CSubs.QSafeValue(Convert.ToString(user.LoginID)), CSubs.QSafeValue(user.SessionID), CSubs.QSafeValue(status.ToString("g")), CSubs.QSafeValue(user.LoginIP)));

                if (LoginStatus.IN == status)
                {
                    CSubs.ExecuteQuery(string.Format("update Access.UserLogin set TempSessionId={1} where LoginID={0}", CSubs.QSafeValue(Convert.ToString(user.LoginID)), CSubs.QSafeValue(user.SessionID)));
                }
                else if (LoginStatus.OUT == status)
                {
                    CSubs.ExecuteQuery(string.Format("update Access.UserLogin set TempSessionId=null where LoginID={0}", CSubs.QSafeValue(Convert.ToString(user.LoginID))));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public bool Insert(User user, out string result, out string password, out string UserId)
        {
            result = string.Empty;
            password = user.LoginID == null || user.LoginID == -1 ? CSubs.GetRandomInt(5) : null;
            try
            {
                using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {
                        conn.AddCommand("[Access].[AddEditUser]", QueryType.Procedure);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LOGINID", SqlDbType.Int, 0, ParameterDirection.InputOutput, user.LoginID == null ? -1 : Convert.ToInt32(user.LoginID));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_EMPID", SqlDbType.Int, 50, ParameterDirection.Input, user.EmpId);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LOGINTEXT", SqlDbType.NVarChar, 100, ParameterDirection.Input, CSubs.PSafeValue(user.LoginText));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LOGINPASSWORD", SqlDbType.NVarChar, 50, ParameterDirection.Input, CSubs.PSafeValue(password));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LOGINTYPECODE", SqlDbType.NVarChar, 50, ParameterDirection.Input, CSubs.PSafeValue(user.LoginType));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CLIENTLOGINNAME", SqlDbType.NVarChar, 100, ParameterDirection.Input, CSubs.PSafeValue(user.UserName));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CLIENTLOGINEMAIL", SqlDbType.NVarChar, 100, ParameterDirection.Input, "");
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ADLOGINTEXT", SqlDbType.NVarChar, 100, ParameterDirection.Input, CSubs.PSafeValue(user.ADLoginText));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_PASSWORDEXPIRYDAYS", SqlDbType.SmallInt, 0, ParameterDirection.Input, 60);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LOGINEXPIRYDATE", SqlDbType.DateTime, 0, ParameterDirection.Input, user.LoginExpiryDate);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ISACTIVE", SqlDbType.Bit, 0, ParameterDirection.Input, user.isActive);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_MODIFIEDBY", SqlDbType.Int, 0, ParameterDirection.Input, UserSession.GetUserSession().LoginID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ACTIVEDIRLOGIN", SqlDbType.Bit, 0, ParameterDirection.Input, user.IsADLogin);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_STANDARDLOGIN", SqlDbType.Bit, 0, ParameterDirection.Input, user.IsStandardLogin);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@RETURNSTATUS", SqlDbType.SmallInt, 0, ParameterDirection.ReturnValue);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_MESSAGE", SqlDbType.NVarChar, 500, ParameterDirection.Output);
                        conn.ExecuteProcedure(ProcedureReturnType.SingleValue);

                        if (!conn.IsError)
                        {
                            int ReturnStatus = Convert.ToInt32(conn.GetParameterValue(ParameterOF.PROCEDURE, "@RETURNSTATUS"));
                            UserId = Convert.ToString(conn.GetParameterValue(ParameterOF.PROCEDURE, "@SP_LOGINID"));
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
                throw new DataAccessException(Convert.ToString(UserSession.GetUserSession().LoginID), "UserDAL", "Insert", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
            //return true;
        }

        public DataTable GetUserForGrid(int pPageIndex, int pPageSize, string pOrderBy, int pOrder, string SearchKey, int LoggedinUserID, out int totalCount)
        {
            try
            {
                totalCount = 0;
                DataTable dt = new DataTable();
                dt = CSubs.GetDataTable(string.Format("[Comm].[GETUserForGrid]  @SP_PageIndex={0},@SP_PageSize={1},@SP_OrderBy={2},@SP_Order={3},@SP_SearchString={4},@SP_LoginID={5}",
                CSubs.QSafeValue(Convert.ToString(pPageIndex)),
                CSubs.QSafeValue(Convert.ToString(pPageSize)),
                CSubs.QSafeValue(pOrderBy),
                CSubs.QSafeValue(Convert.ToString(pOrder)),
                CSubs.QSafeValue(SearchKey),
                Convert.ToString(LoggedinUserID)));

                if (dt != null && dt.Rows.Count > 0)
                {
                    totalCount = Convert.ToInt32(dt.Rows[0]["TotalRows"]);
                }


                return dt;
            }
            catch (Exception ex)
            {
                throw new DataAccessException("", "UserDAL", "GetUserForGrid", ex.ToString(), ex);
            }

        }

        public DataTable GetUserDetails(int? EmpId, out string Msg)
        {
            try
            {
                Msg = "";
                DataTable dt = new DataTable();
                using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {
                        conn.AddCommand("[Comm].[GETUserDetail]", QueryType.Procedure);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LOGINID", SqlDbType.Int, 0, ParameterDirection.Input, 0);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_EmpId", SqlDbType.NVarChar, 20, ParameterDirection.Input, EmpId);
                        dt = (DataTable)conn.ExecuteProcedure(ProcedureReturnType.DataTable);
                    }
                    else
                    {
                        Msg = "Unable to connet to db server";
                    }
                }
                return dt;
            }
            catch (Exception ex)
            {
                throw new DataAccessException("", "UserDAL", "GetUserForGrid", ex.ToString(), ex);
            }
        }

        public DataSet GetUserMapping(int UserId, out string Msg)
        {
            try
            {
                Msg = "";
                DataSet ds = new DataSet();
                using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {
                        conn.AddCommand("[Access].[GetUserMapping]", QueryType.Procedure);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_UserID", SqlDbType.Int, 0, ParameterDirection.Input, UserId);
                        ds = (DataSet)conn.ExecuteProcedure(ProcedureReturnType.DataSet);
                    }
                    else
                    {
                        Msg = "Unable to connet to db server";
                    }
                }
                return ds;
            }
            catch (Exception ex)
            {
                throw new DataAccessException("", "UserDAL", "GetUserMapping", ex.ToString(), ex);
            }
        }

        public bool SaveMapping(XElement country, XElement comp, XElement role, XElement city, XElement rmc1, XElement warehouse, bool rmc, bool nonrmc, bool orientation, int UserID, string Type, XElement Report, XElement serviceline, XElement rvbranch, XElement clickrestrict)
        {
            string result = string.Empty;
            try
            {
                using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {
                        if (Type.ToLower() == "report")
                        {
                            conn.AddCommand("[Access].[AddEditReportUserMap]", QueryType.Procedure);
                            conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Loginid", SqlDbType.Int, 0, ParameterDirection.Input, UserSession.GetUserSession().LoginID);
                            conn.AddParameter(ParameterOF.PROCEDURE, "@SP_RepUserID", SqlDbType.Int, 0, ParameterDirection.Input, UserID);
                            conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ReportID", SqlDbType.Xml, -1, ParameterDirection.Input, Report.HasElements ? Convert.ToString(Report) : null);
                        }

                        else
                        {
                            conn.AddCommand("[Access].[AddEditUserMapToParam]", QueryType.Procedure);
                            conn.AddParameter(ParameterOF.PROCEDURE, "@SP_UserID", SqlDbType.Int, 0, ParameterDirection.Input, UserSession.GetUserSession().LoginID);
                            conn.AddParameter(ParameterOF.PROCEDURE, "@SP_RoleUserID", SqlDbType.Int, 0, ParameterDirection.Input, UserID);
                            conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CompID", SqlDbType.Xml, -1, ParameterDirection.Input, comp.HasElements ? Convert.ToString(comp) : null);
                            conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CountryID", SqlDbType.Xml, -1, ParameterDirection.Input, country.HasElements ? Convert.ToString(country) : null);
                            conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CityID", SqlDbType.Xml, -1, ParameterDirection.Input, city.HasElements ? Convert.ToString(city) : null);
                            conn.AddParameter(ParameterOF.PROCEDURE, "@SP_RMCID", SqlDbType.Xml, -1, ParameterDirection.Input, rmc1.HasElements ? Convert.ToString(rmc1) : null);
                            conn.AddParameter(ParameterOF.PROCEDURE, "@SP_RoleID", SqlDbType.Xml, -1, ParameterDirection.Input, role.HasElements ? Convert.ToString(role) : null);
                            conn.AddParameter(ParameterOF.PROCEDURE, "@SP_WarehouseID", SqlDbType.Xml, -1, ParameterDirection.Input, warehouse.HasElements ? Convert.ToString(warehouse) : null);
                            conn.AddParameter(ParameterOF.PROCEDURE, "@SP_servicelineID", SqlDbType.Xml, -1, ParameterDirection.Input, serviceline.HasElements ? Convert.ToString(serviceline) : null);
                            conn.AddParameter(ParameterOF.PROCEDURE, "@SP_rvbranchID", SqlDbType.Xml, -1, ParameterDirection.Input, rvbranch.HasElements ? Convert.ToString(rvbranch) : null);
                            conn.AddParameter(ParameterOF.PROCEDURE, "@SP_clickrestrictID", SqlDbType.Xml, -1, ParameterDirection.Input, clickrestrict.HasElements ? Convert.ToString(clickrestrict) : null);

                            conn.AddParameter(ParameterOF.PROCEDURE, "@SP_AllowRMC", SqlDbType.Bit, 0, ParameterDirection.Input, rmc);
                            conn.AddParameter(ParameterOF.PROCEDURE, "@SP_AllowNonRMC", SqlDbType.Bit, 0, ParameterDirection.Input, nonrmc);
                            conn.AddParameter(ParameterOF.PROCEDURE, "@SP_AllowOrientation", SqlDbType.Bit, 0, ParameterDirection.Input, orientation);
                            conn.AddParameter(ParameterOF.PROCEDURE, "@SP_From", SqlDbType.NVarChar, 50, ParameterDirection.Input, CSubs.PSafeValue(Type));
                        }
                        conn.AddParameter(ParameterOF.PROCEDURE, "@RETURNSTATUS", SqlDbType.SmallInt, 0, ParameterDirection.ReturnValue);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@Out_Message", SqlDbType.NVarChar, -1, ParameterDirection.Output);
                        conn.ExecuteProcedure(ProcedureReturnType.SingleValue);
                        if (!conn.IsError)
                        {
                            string returnstats = Convert.ToString(conn.GetParameterValue(ParameterOF.PROCEDURE, "@RETURNSTATUS"));
                            //int ReturnStatus = Convert.ToInt32(conn.GetParameterValue(ParameterOF.PROCEDURE, "@RETURNSTATUS"));
                            string message = Convert.ToString(conn.GetParameterValue(ParameterOF.PROCEDURE, "@Out_Message"));

                            if (returnstats == "0")
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
                throw new DataAccessException(Convert.ToString(UserSession.GetUserSession().LoginID), "UserDAL", "SaveMapping", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
        }

        public bool UpdateLoginAttempt(List<User> users)
        {
            try
            {
                using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {
                        conn.AddCommand("[Access].[UpdateLoginAttempt]", QueryType.Procedure);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_UserID", SqlDbType.Int, 0, ParameterDirection.Input);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LoginID", SqlDbType.Int, 0, ParameterDirection.Input, UserSession.GetUserSession().LoginID);

                        foreach (User user in users)
                        {
                            conn.SetParameterValue(ParameterOF.PROCEDURE, "@SP_UserID", user.UserID);
                            conn.ExecuteProcedure(ProcedureReturnType.SingleValue);
                        }

                    }
                    else
                        throw new Exception(conn.ErrorMessage);
                }

                return true;
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(UserSession.GetUserSession().LoginID), "UserDAL", "UpdateLoginAttempt", ex.ToString(), ex);
            }
        }

        public bool UpdateLastLogin(List<User> users)
        {
            try
            {
                using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {
                        conn.AddCommand("[Access].[UpdateLastLogin]", QueryType.Procedure);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_UserID", SqlDbType.Int, 0, ParameterDirection.Input);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LoginID", SqlDbType.Int, 0, ParameterDirection.Input, UserSession.GetUserSession().LoginID);
                        foreach (User user in users)
                        {
                            conn.SetParameterValue(ParameterOF.PROCEDURE, "@SP_UserID", user.UserID);
                            conn.ExecuteProcedure(ProcedureReturnType.SingleValue);
                        }

                    }
                    else
                        throw new Exception(conn.ErrorMessage);
                }

                return true;
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(UserSession.GetUserSession().LoginID), "UserDAL", "UpdateLoginAttempt", ex.ToString(), ex);
            }
        }

        public bool ResetPassword(List<User> users, out string pwd)
        {
            try
            {
                using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {
                        pwd = CSubs.GetRandomInt(5);
                        conn.AddCommand("[Access].[ResetPassword]", QueryType.Procedure);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LOGINID", SqlDbType.Int, 0, ParameterDirection.Input);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LOGINPASSWORD", SqlDbType.VarChar, 20, ParameterDirection.Input, pwd);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_MODIFIEDBY", SqlDbType.Int, 0, ParameterDirection.Input, UserSession.GetUserSession().LoginID);

                        foreach (User user in users)
                        {

                            conn.SetParameterValue(ParameterOF.PROCEDURE, "@SP_LOGINID", user.UserID);
                            conn.ExecuteProcedure(ProcedureReturnType.SingleValue);
                            //conn.AddCommand(string.Format("[Access].[ResetPassword] @SP_LOGINID={0}, @SP_LOGINPASSWORD={1}, @SP_MODIFIEDBY={2}", loginid, password, Session["LoginID"].ToString()), QueryType.QueryText);
                            //conn.ExecuteQuery(QueryReturnType.SingleValue);

                        }
                    }
                    else
                        throw new Exception(conn.ErrorMessage);
                }

                return true;
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(UserSession.GetUserSession().LoginID), "UserDAL", "ResetPassword", ex.ToString(), ex);
            }
        }

        public bool UpdateCompany(UserInformationModel user)
        {
            try
            {

                bool RMCBuss = user.BussinessLine != "NON RMC-BUSINESS";
                using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {
                        conn.AddCommand("[Access].[AddEditLoginCompany]", QueryType.Procedure);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LOGINID", SqlDbType.Int, 0, ParameterDirection.Input, user.LoginID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CompID", SqlDbType.Int, 0, ParameterDirection.Input, user.CompanyID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IsRmcBuss", SqlDbType.Bit, 0, ParameterDirection.Input, RMCBuss);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@RETURNSTATUS", SqlDbType.SmallInt, 0, ParameterDirection.ReturnValue);
                        conn.ExecuteProcedure(ProcedureReturnType.SingleValue);
                        if (!conn.IsError)
                        {
                            string returnstats = Convert.ToString(conn.GetParameterValue(ParameterOF.PROCEDURE, "@RETURNSTATUS"));
                            //int ReturnStatus = Convert.ToInt32(conn.GetParameterValue(ParameterOF.PROCEDURE, "@RETURNSTATUS"));
                            //string message = Convert.ToString(conn.GetParameterValue(ParameterOF.PROCEDURE, "@Out_Message"));

                            if (returnstats == "0")
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

                }

                return true;
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(UserSession.GetUserSession().LoginID), "UserDAL", "ResetPassword", ex.ToString(), ex);
            }
        }

        public bool CopyUserRights(int CopyFromLoginID, int CopyToLoginID, string OverwriteAppend, out string result)
        {
            result = string.Empty;
            try
            {
                using (CDALSQL conn = new CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {
                        conn.AddCommand("[Access].[CopyUserRights]", QueryType.Procedure);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CopyFromLoginID", SqlDbType.Int, 0, ParameterDirection.Input, CopyFromLoginID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CopyToLoginID", SqlDbType.Int, 0, ParameterDirection.Input, CopyToLoginID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_OverwriteAppend", SqlDbType.VarChar, 1, ParameterDirection.Input, OverwriteAppend);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LoginID", SqlDbType.Int, 0, ParameterDirection.Input, UserSession.GetUserSession().LoginID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@RETURNSTATUS", SqlDbType.SmallInt, 0, ParameterDirection.ReturnValue);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@Out_Message", SqlDbType.NVarChar, -1, ParameterDirection.Output);
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
                throw new DataAccessException(Convert.ToString(UserSession.GetUserSession().LoginID), "UserDAL", "CopyUserRights", Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
        }
    }

    public enum LoginStatus
    {
        IN,
        OUT,
        EXPIRED,
        FORCEOUT
    }
}