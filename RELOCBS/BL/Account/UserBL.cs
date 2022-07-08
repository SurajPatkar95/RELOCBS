using RELOCBS.App_Code;
using RELOCBS.Common.ExceptionHandling;
using RELOCBS.DAL.Account;
using RELOCBS.Entities;
using RELOCBS.Models;
using RELOCBS.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Xml.Linq;

namespace RELOCBS.BL.Account
{
    public class UserBL
    {
        private HttpContext _httpContext;
        private readonly TimeSpan _expirationTimeSpan;
        private UserInformationModel _cachedUser;
        private UserDAL _userDAL;

        public UserDAL userDAL
        {

            get
            {
                if (this._userDAL == null)
                    this._userDAL = new UserDAL();
                return this._userDAL;
            }
        }

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

        private CustomSessionStore _session;
        public CustomSessionStore session
        {
            get
            {
                if (this._session == null)
                    this._session = new CustomSessionStore();
                return this._session;
            }
        }


        public UserBL()
        {

            _userDAL = new UserDAL();
            this._expirationTimeSpan = FormsAuthentication.Timeout;
        }


        public bool Insert(SysUser entity)
        {
            try
            {


            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(ex.ToString());
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException("", "UserBL", "Insert", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }
            return true;
        }

        public bool Update(SysUser entity)
        {
            try
            {

            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(ex.ToString());
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException("", "UserBL", "Update", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

            return true;
        }

        public bool DeleteById(int id)
        {
            try
            {

            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(ex.ToString());
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException("", "UserBL", "DeleteById", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }


            return true;
        }

        public bool ValidateUser(string LoginText, string Password, out UserInformationModel user, out string Msg)
        {
            bool IsADAuthenticated = false;
            user = null;
            Msg = string.Empty;
            try
            {

                user = null;

                //Commented for temporary purpose..AD Logins are not available.
                IsADAuthenticated = IsLADAPAuthenticated(LoginText, Password);

                return _userDAL.ValidateUser(LoginText, Password, IsADAuthenticated, out user, out Msg);

            }
            catch (DataAccessException ex)
            {
                /////Log Invali user trying to login into table
                return false;
                // throw new BussinessLogicException(ex.ToString());
            }
            catch (BussinessLogicException ex)
            {
                /////Log Invali user trying to login into table
                return false;

            }
            catch (Exception ex)
            {
                throw new BussinessLogicException("", "UserBL", "ValidateUser", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

        }


        public bool IsLADAPAuthenticated(string username, string pwd)
        {

            bool IsAuthenticated = true;
            //DirectoryEntry entry = new DirectoryEntry(_path,
            //domainAndUsername, pwd);
            try
            {
                LdapAuthentication ldap1 = new LdapAuthentication(System.Configuration.ConfigurationManager.AppSettings["AD_Path"].ToString());
                LdapAuthentication ldap2 = new LdapAuthentication(System.Configuration.ConfigurationManager.AppSettings["AD_Path_Secondary"].ToString());
                if (ldap1.IsAuthenticated(System.Configuration.ConfigurationManager.AppSettings["AD_Domain"].ToString(), username, pwd) == false)
                {
                    CSubs.LogError(this, "IsLADAPAuthenticated-1", ldap1.ErrorMessage);

                    if (ldap2.IsAuthenticated(System.Configuration.ConfigurationManager.AppSettings["AD_Domain_Secondary"].ToString(), username, pwd) == false)
                    //if (ldap1.IsAuthenticated(System.Configuration.ConfigurationManager.AppSettings["AD_Domain"].ToString(), username, CSubs.Decrypt(pwd) == false)
                    {
                        IsAuthenticated = false;
                        CSubs.LogError(this, "IsLADAPAuthenticated-2", ldap2.ErrorMessage);
                    }
                    else
                    {
                        IsAuthenticated = true;
                    }
                }
                else
                {
                    IsAuthenticated = true;
                }


            }
            catch (Exception ex)
            {
                throw new BussinessLogicException("", "UserBL", "IsLADAPAuthenticated", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }


            return IsAuthenticated;
        }



        public User GetUserByUsername(string UserName)
        {
            User user = null;
            try
            {



            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(ex.ToString());
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException("", "UserBL", "GetUserByUsername", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

            return user;

        }

        public bool ChangePassword(ChangePasswordViewModel model, out string message)
        {
            try
            {
                message = string.Empty;
                return userDAL.ChangePassword(model, out message);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(ex.ToString());
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(model.UserID.ToString(), "UserBL", "ChangePassword", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }
        }

        public bool ForgetPassword(string LoginText, string NewPassword)
        {
            try
            {

            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(ex.ToString());
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException("", "UserBL", "ForgetPassword", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

            return true;
        }

        public virtual void SignIn(UserInformationModel user, bool createPersistentCookie)
        {
            var now = DateTime.UtcNow.ToLocalTime();
            //UserSession.AbandonSession();
            //var newSessionId = UserSession.CreateSessionId(HttpContext.Current);
            //UserSession.SetSessionId(HttpContext.Current, newSessionId);

            var ticket = new FormsAuthenticationTicket(
                1 /*version*/,
                user.LoginID.ToString(),
                now,
                now.Add(_expirationTimeSpan),
                createPersistentCookie,
                user.UserName,
                FormsAuthentication.FormsCookiePath);

            var encryptedTicket = FormsAuthentication.Encrypt(ticket);

            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
            cookie.HttpOnly = true;
            if (ticket.IsPersistent)
            {
                cookie.Expires = ticket.Expiration;
            }
            cookie.Secure = FormsAuthentication.RequireSSL;
            cookie.Path = FormsAuthentication.FormsCookiePath;
            if (FormsAuthentication.CookieDomain != null)
            {
                cookie.Domain = FormsAuthentication.CookieDomain;
            }

            HttpContext.Current.Response.Cookies.Add(cookie);

            //Response.Cookies.Add(Cockie);

            //_httpContext.Response.Cookies.Add(cookie);
            _cachedUser = user;
            //UserInformationModel UserInfo = new UserInformationModel()
            //{
            //    LoginID = Convert.ToInt32(user.LoginID)
            //};


            user.LoginIP = getIPAddress();
            user.SessionID = HttpContext.Current.Session.SessionID + '-' + DateTime.Now.ToString("yyyyMdHHmmss");
            userDAL.UpdateLoginStatus(user, LoginStatus.IN);

            session.Set<UserInformationModel>("UserSession", user);
            /////Update the Status of the user feedback form
            new UserFeedback.UserFeedbackBL().UpdateUserFeedbackSession();


            UserSession.SetUserMenu(Convert.ToString(user.LoginID));

        }

        public virtual void SignOut()
        {
            userDAL.UpdateLoginStatus(UserSession.GetUserSession(), LoginStatus.OUT);
            _cachedUser = null;
            session.Set<UserInformationModel>("UserSession", null);
            session.Set<UserInformationModel>("UserMenuTable", null);
            session.Set<UserInformationModel>("UserMenu", null);

            FormsAuthentication.SignOut();
            UserSession.AbandonSession();
        }

        public virtual void ExpiredSignOut()
        {

            userDAL.UpdateLoginStatus(UserSession.GetUserSession(), LoginStatus.EXPIRED);
            _cachedUser = null;
            session.Set<UserInformationModel>("UserSession", null);
            FormsAuthentication.SignOut();
            UserSession.AbandonSession();
        }

        public static string getIPAddress()
        {
            try
            {

                string ipAddress = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                if (string.IsNullOrEmpty(ipAddress))
                {
                    ipAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                }

                if (string.IsNullOrWhiteSpace(ipAddress))
                {
                    Console.WriteLine(string.Join("|", new List<object> {
                    HttpContext.Current.Request.UserHostAddress,
                    HttpContext.Current.Request.Headers["X-Forwarded-For"],
                    HttpContext.Current.Request.Headers["REMOTE_ADDR"]
                })
                );

                    var ip = HttpContext.Current.Request.UserHostAddress;
                    if (HttpContext.Current.Request.Headers["X-Forwarded-For"] != null)
                    {
                        ipAddress = HttpContext.Current.Request.Headers["X-Forwarded-For"];
                        Console.WriteLine(ip + "|X-Forwarded-For");
                    }
                    else if (HttpContext.Current.Request.Headers["REMOTE_ADDR"] != null)
                    {
                        ipAddress = HttpContext.Current.Request.Headers["REMOTE_ADDR"];
                        Console.WriteLine(ip + "|REMOTE_ADDR");
                    }
                }


                return ipAddress;
            }
            catch
            {
                //Console.Error.WriteLine(ex.Message);
            }
            return null;
        }

        public bool Insert(User user, out string result, out string password, out string UserId)
        {
            try
            {
                return userDAL.Insert(user, out result, out password, out UserId);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "CityBL", "Insert", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

        }

        public IEnumerable<User> GetUserForGrid(int pPageIndex, int pPageSize, string pOrderBy, int pOrder, string SearchKey, int LoggedinUserID, out int totalCount)
        {

            try
            {
                IEnumerable<User> userList = new List<User>();

                string Msg = "";
                DataTable dt = userDAL.GetUserForGrid(pPageIndex, pPageSize, pOrderBy, pOrder, SearchKey, LoggedinUserID, out totalCount);

                if (dt != null)
                {
                    userList = dt.AsEnumerable().
                        Select(dataRow => new User
                        {
                            LoginID = Convert.ToInt32(dataRow["LoginID"]),
                            UserName = Convert.ToString(dataRow["EmpName"]),
                            EmpId = Convert.ToInt32(dataRow["EmpID"]),
                            LoginText = Convert.ToString(dataRow["LoginText"]),
                            LastLogInDateTime = dataRow["LastLogInDateTime"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(Convert.ToString(dataRow["LastLogInDateTime"])),
                            PasswordExpiryDate = dataRow["PasswordExpiryDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(Convert.ToString(dataRow["PasswordExpiryDate"])),
                            PasswordExpiryDays = dataRow["PasswordExpiryDays"] == DBNull.Value ? 0 : Convert.ToInt32(Convert.ToString(dataRow["PasswordExpiryDays"])),
                            LoginType = Convert.ToString(dataRow["LoginTypeDesc"]),
                            isActive = Convert.ToBoolean(dataRow["Isactive"]),
                            LastLoginDate = dataRow["LoginExpiryDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(Convert.ToString(dataRow["LoginExpiryDate"])),
                            AttemptCount = dataRow["AttemptCount"] == DBNull.Value ? 0 : Convert.ToInt32(Convert.ToString(dataRow["AttemptCount"])),
                            DeactivatedOn = dataRow["LastInactiveDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(Convert.ToString(dataRow["LastInactiveDate"])),
                        }).ToList();
                }
                return userList;
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException("", "UserBL", "GetUserForGrid", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

        }

        public User GetUserDetails(int? EmpId)
        {

            try
            {
                //Msg = "";
                User userObj = new User();
                string Msg = "";
                DataTable dt = userDAL.GetUserDetails(EmpId, out Msg);
                if (dt != null && dt.Rows.Count > 0)
                {
                    userObj = (from rw in dt.AsEnumerable()
                               select new User()
                               {
                                   LoginID = Convert.ToInt32(rw["LoginID"]),
                                   UserID = Convert.ToInt32(rw["LoginID"]),
                                   UserName = Convert.ToString(rw["EmpName"]),
                                   LoginText = Convert.ToString(rw["LoginText"]),
                                   ADLoginText = Convert.ToString(rw["ADLoginText"]),
                                   isActive = Convert.ToBoolean(rw["IsActive"]),
                                   EmpId = Convert.ToInt32(rw["EmpID"]),
                                   IsADLogin = Convert.ToBoolean(rw["AllowADLogin"]),
                                   IsStandardLogin = Convert.ToBoolean(rw["AllowDBLogin"]),
                                   LoginType = Convert.ToString(rw["LoginTypeCode"]),
                                   LastLoginDate = rw["LastLogInDateTime"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(Convert.ToString(rw["LastLogInDateTime"])),
                                   LoginExpiryDate = rw["LoginExpiryDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(Convert.ToString(rw["LoginExpiryDate"])),
                                   PasswordExpiryDate = rw["PasswordExpiryDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(Convert.ToString(rw["PasswordExpiryDate"])),
                                   PasswordExpiryDays = rw["PasswordExpiryDays"] == DBNull.Value ? 0 : Convert.ToInt32(Convert.ToString(rw["PasswordExpiryDays"])),
                                   ActivatedOn = rw["CreatedDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(Convert.ToString(rw["CreatedDate"])),
                                   DeactivatedOn = rw["LastInactiveDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(Convert.ToString(rw["LastInactiveDate"])),
                                   DeactivatedBy = Convert.ToString(rw["LastInactiveBy"]),
                               }).First();
                    //return userObj;
                }
                return userObj;
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException("", "UserBL", "GetUserDetails", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

        }

        public List<List<SelectListItem>> GetUserMapping(int UserId)
        {

            try
            {
                //Msg = "";
                UserLocation userObj = new UserLocation();
                List<SelectListItem> CompList = new List<SelectListItem>();
                List<SelectListItem> BranchList = new List<SelectListItem>();
                List<SelectListItem> RoleList = new List<SelectListItem>();
                List<SelectListItem> BussList = new List<SelectListItem>();
                List<SelectListItem> RMCList = new List<SelectListItem>();
                List<SelectListItem> WarehouseList = new List<SelectListItem>();
                List<SelectListItem> ReportList = new List<SelectListItem>();
                List<SelectListItem> ServicelineList = new List<SelectListItem>();
                List<SelectListItem> RVBranchList = new List<SelectListItem>();
                List<SelectListItem> ClickRestrictList = new List<SelectListItem>();

                string Msg = "";
                DataSet dt = userDAL.GetUserMapping(UserId, out Msg);

                if (dt != null)
                {
                    /////CompList
                    if (dt.Tables[0] != null && dt.Tables[0].Rows.Count > 0)
                    {

                        Int32 itemOffset;
                        Int32 endOffset;

                        new SelectListItem
                        {
                            Value = null,
                            Text = " "
                        };

                        itemOffset = 0;
                        endOffset = dt.Tables[0].Rows.Count;

                        for (Int32 i = itemOffset; i < endOffset; i++)
                        {
                            SelectListItem item = new SelectListItem() { Value = dt.Tables[0].Rows[i][0].ToString(), Text = dt.Tables[0].Rows[i][1].ToString() };
                            CompList.Add(item);
                            //item.DataBind();
                        }

                        //userObj.CompList = (from rw in dt.Tables[0].AsEnumerable()
                        //           select new UserLoc()
                        //           {
                        //               LoginID = Convert.ToString(rw["LoginID"]),
                        //               UserName = Convert.ToString(rw["LoginText"]),
                        //               isActive = Convert.ToBoolean(rw["isActive"]),
                        //               EmpId = Convert.ToInt32(rw["EmpID"]),
                        //               IsADLogin = Convert.ToBoolean(rw["AllowADLogin"]),
                        //               IsStandardLogin = Convert.ToBoolean(rw["AllowDBLogin"])
                        //           }).First();
                    }
                    /////BranchList
                    if (dt.Tables[1] != null && dt.Tables[1].Rows.Count > 0)
                    {
                        Int32 itemOffset;
                        Int32 endOffset;

                        new SelectListItem
                        {
                            Value = null,
                            Text = " "
                        };

                        itemOffset = 0;
                        endOffset = dt.Tables[1].Rows.Count;

                        for (Int32 i = itemOffset; i < endOffset; i++)
                        {
                            SelectListItem item = new SelectListItem() { Value = dt.Tables[1].Rows[i][0].ToString(), Text = dt.Tables[1].Rows[i][1].ToString() };
                            BranchList.Add(item);
                            //item.DataBind();
                        }
                    }
                    /////RoleList
                    if (dt.Tables[2] != null && dt.Tables[2].Rows.Count > 0)
                    {
                        Int32 itemOffset;
                        Int32 endOffset;

                        new SelectListItem
                        {
                            Value = null,
                            Text = " "
                        };

                        itemOffset = 0;
                        endOffset = dt.Tables[2].Rows.Count;

                        for (Int32 i = itemOffset; i < endOffset; i++)
                        {
                            SelectListItem item = new SelectListItem() { Value = dt.Tables[2].Rows[i][0].ToString(), Text = dt.Tables[2].Rows[i][1].ToString() };
                            RoleList.Add(item);
                            //item.DataBind();
                        }
                    }

                    ///BussList
                    if (dt.Tables[3] != null && dt.Tables[3].Rows.Count > 0)
                    {
                        Int32 itemOffset;
                        Int32 endOffset;

                        new SelectListItem
                        {
                            Value = null,
                            Text = " "
                        };

                        itemOffset = 0;
                        endOffset = dt.Tables[3].Rows.Count;

                        for (Int32 i = itemOffset; i < endOffset; i++)
                        {
                            SelectListItem item = new SelectListItem() { Value = dt.Tables[3].Rows[i][0].ToString(), Text = dt.Tables[3].Rows[i][1].ToString() };
                            BussList.Add(item);
                            //item.DataBind();
                        }
                    }

                    /////RMCList
                    if (dt.Tables[4] != null && dt.Tables[4].Rows.Count > 0)
                    {
                        Int32 itemOffset;
                        Int32 endOffset;

                        new SelectListItem
                        {
                            Value = null,
                            Text = " "
                        };

                        itemOffset = 0;
                        endOffset = dt.Tables[4].Rows.Count;

                        for (Int32 i = itemOffset; i < endOffset; i++)
                        {
                            SelectListItem item = new SelectListItem() { Value = dt.Tables[4].Rows[i][0].ToString(), Text = dt.Tables[4].Rows[i][1].ToString() };
                            RMCList.Add(item);
                            //item.DataBind();
                        }
                    }
                    /////WarehouseList
                    if (dt.Tables[5] != null && dt.Tables[5].Rows.Count > 0)
                    {

                        Int32 itemOffset;
                        Int32 endOffset;

                        new SelectListItem
                        {
                            Value = null,
                            Text = " "
                        };

                        itemOffset = 0;
                        endOffset = dt.Tables[5].Rows.Count;

                        for (Int32 i = itemOffset; i < endOffset; i++)
                        {
                            SelectListItem item = new SelectListItem() { Value = dt.Tables[5].Rows[i]["ID"].ToString(), Text = dt.Tables[5].Rows[i]["NAME"].ToString() };
                            WarehouseList.Add(item);
                            //item.DataBind();
                        }
                    }

                    /////ReportList
                    if (dt.Tables[6] != null && dt.Tables[6].Rows.Count > 0)
                    {

                        Int32 itemOffset;
                        Int32 endOffset;

                        new SelectListItem
                        {
                            Value = null,
                            Text = " "
                        };

                        itemOffset = 0;
                        endOffset = dt.Tables[6].Rows.Count;

                        for (Int32 i = itemOffset; i < endOffset; i++)
                        {
                            SelectListItem item = new SelectListItem() { Value = dt.Tables[6].Rows[i]["ID"].ToString(), Text = dt.Tables[6].Rows[i]["NAME"].ToString() };
                            ReportList.Add(item);
                            //item.DataBind();
                        }
                    }


                    /////ServicelineList
                    if (dt.Tables[7] != null && dt.Tables[7].Rows.Count > 0)
                    {

                        Int32 itemOffset;
                        Int32 endOffset;

                        new SelectListItem
                        {
                            Value = null,
                            Text = " "
                        };

                        itemOffset = 0;
                        endOffset = dt.Tables[7].Rows.Count;

                        for (Int32 i = itemOffset; i < endOffset; i++)
                        {
                            SelectListItem item = new SelectListItem() { Value = dt.Tables[7].Rows[i]["ID"].ToString(), Text = dt.Tables[7].Rows[i]["NAME"].ToString() };
                            ServicelineList.Add(item);
                            //item.DataBind();
                        }
                    }

                    /////rvbranchList
                    if (dt.Tables[8] != null && dt.Tables[8].Rows.Count > 0)
                    {

                        Int32 itemOffset;
                        Int32 endOffset;

                        new SelectListItem
                        {
                            Value = null,
                            Text = " "
                        };

                        itemOffset = 0;
                        endOffset = dt.Tables[8].Rows.Count;

                        for (Int32 i = itemOffset; i < endOffset; i++)
                        {
                            SelectListItem item = new SelectListItem() { Value = dt.Tables[8].Rows[i]["ID"].ToString(), Text = dt.Tables[8].Rows[i]["NAME"].ToString() };
                            RVBranchList.Add(item);
                            //item.DataBind();
                        }
                    }

                    /////ClickRestrictList
                    if (dt.Tables[9] != null && dt.Tables[9].Rows.Count > 0)
                    {

                        Int32 itemOffset;
                        Int32 endOffset;

                        new SelectListItem
                        {
                            Value = null,
                            Text = " "
                        };

                        itemOffset = 0;
                        endOffset = dt.Tables[9].Rows.Count;

                        for (Int32 i = itemOffset; i < endOffset; i++)
                        {
                            SelectListItem item = new SelectListItem() { Value = dt.Tables[9].Rows[i]["ID"].ToString(), Text = dt.Tables[9].Rows[i]["NAME"].ToString() };
                            ClickRestrictList.Add(item);
                            //item.DataBind();
                        }
                    }
                }
                return new List<List<SelectListItem>>() { CompList, BranchList, RoleList, BussList, RMCList, WarehouseList, ReportList, ServicelineList, RVBranchList, ClickRestrictList };

            }
            catch (Exception ex)
            {
                throw new BussinessLogicException("", "UserBL", "GetUserDetails", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

        }

        public bool SaveMapping(UserLocation model)
        {
            try
            {
                var country = model.CountryList != null ? new XElement("root", model.CountryList.Select(x => new XElement("CountryIDs", new XElement("CountryID", x)))) : new XElement("CountryIDs");

                var comp = model.CompList != null ? new XElement("root", model.CompList.Select(x => new XElement("CompIDs", new XElement("CompID", x)))) : new XElement("CompID");
                var role = model.RoleList != null ? new XElement("root", model.RoleList.Select(x => new XElement("RoleIDs", new XElement("RoleID", x)))) : new XElement("RoleID");
                var city = model.CityList != null ? new XElement("root", model.CityList.Select(x => new XElement("CityIDs", new XElement("CityID", x)))) : new XElement("CityID");
                var rmc1 = model.RMCList != null ? new XElement("root", model.RMCList.Select(x => new XElement("RMCIDs", new XElement("RMCID", x)))) : new XElement("RMCID");

                var Warehouse = model.MappedWarehouseList != null ? new XElement("root", model.MappedWarehouseList.Select(x => new XElement("WarehouseIDs", new XElement("WarehouseID", x)))) : new XElement("WarehouseID");

                var Report = model.MappedReportList != null ? new XElement("RepIDs", model.MappedReportList.Select(x => new XElement("RepID", new XElement("ID", x)))) : new XElement("ID");

                var serviceline = model.MappedServicelineList != null ? new XElement("root", model.MappedServicelineList.Select(x => new XElement("ServicelineIDs", new XElement("ServicelineID", x)))) : new XElement("ServicelineID");
                var rvbranch = model.MappedrvbranchList != null ? new XElement("root", model.MappedrvbranchList.Select(x => new XElement("RvBranchIDs", new XElement("RvBranchID", x)))) : new XElement("RvBranchID");
                var clickrestrict = model.MappedclickrestrictList != null ? new XElement("root", model.MappedclickrestrictList.Select(x => new XElement("ClickIDs", new XElement("ClickID", x)))) : new XElement("ClickID");

                bool rmc = false;
                bool nonrmc = false;
                bool orientation = false;
                if (model.BussList != null)
                {
                    rmc = model.BussList.Contains("RMC-BUSINESS") ? true : false;
                    nonrmc = model.BussList.Contains("NON RMC-BUSINESS") ? true : false;
                    orientation = model.BussList.Contains("ORIENTATION SERVICE") ? true : false;
                }

                return userDAL.SaveMapping(country, comp, role, city, rmc1, Warehouse, rmc, nonrmc, orientation, model.UserID, model.Type, Report, serviceline, rvbranch, clickrestrict);
                //return true;
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "UserBL", "SaveMapping", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

        }

        public bool UpdateLoginAttempt(List<User> users)
        {
            try
            {
                return userDAL.UpdateLoginAttempt(users);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "UserBL", "UpdateLoginAttempt", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

        }

        public bool UpdateLastLogin(List<User> users)
        {
            try
            {
                return userDAL.UpdateLastLogin(users);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "UserBL", "UpdateLastLogin", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

        }

        public bool ResetPassword(List<User> users, out string pwd)
        {
            try
            {
                return userDAL.ResetPassword(users, out pwd);
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
                return userDAL.UpdateCompany(user);
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(UserSession.GetUserSession().LoginID), "UserDAL", "ResetPassword", ex.ToString(), ex);
            }

        }

        public bool CopyUserRights(int CopyFromLoginID, int CopyToLoginID, string OverwriteAppend, out string result)
        {
            try
            {
                return userDAL.CopyUserRights(CopyFromLoginID, CopyToLoginID, OverwriteAppend, out result);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "UserBL", "CopyUserRights", Properties.Resources.UnExpectedErrorAtBL, ex);
            }
        }
    }
}