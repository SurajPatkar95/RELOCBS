using RELOCBS.App_Code;
using RELOCBS.Common.ExceptionHandling;
using RELOCBS.DAL.Account;
using RELOCBS.Entities;
using RELOCBS.Models;
using RELOCBS.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace RELOCBS.BL.Account
{
    public class UserBL
    {
        private HttpContext _httpContext;
        private readonly TimeSpan _expirationTimeSpan;
        private UserInformationModel _cachedUser;
        private UserDAL _userDAL;
        
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

        public bool ValidateUser(string LoginText, string Password, out UserInformationModel user)
        {
            bool IsADAuthenticated;
            user = null;
            try
            {

                user = null;

                IsADAuthenticated = IsLADAPAuthenticated(LoginText, Password);

                return _userDAL.ValidateUser(LoginText, Password, IsADAuthenticated, out user);

            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(ex.ToString());
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
                    if (ldap2.IsAuthenticated(System.Configuration.ConfigurationManager.AppSettings["AD_Domain_Secondary"].ToString(), username, pwd) == false)
                    //if (ldap1.IsAuthenticated(System.Configuration.ConfigurationManager.AppSettings["AD_Domain"].ToString(), username, CSubs.Decrypt(pwd) == false)
                    {
                        IsAuthenticated = false;
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

        public bool ChangePassword(string LoginText, string OldPassword, string NewPassword)
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
                throw new BussinessLogicException("", "UserBL", "ChangePassword", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

            return true;
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
            session.Set<UserInformationModel>("UserSession", user);
            UserSession.SetUserMenu(Convert.ToString(user.LoginID));

        }

        public virtual void SignOut()
        {
            _cachedUser = null;
            FormsAuthentication.SignOut();
            HttpContext.Current.Session.Abandon();
            HttpContext.Current.Session.Clear();
            HttpContext.Current.Session.RemoveAll();
        }
    }
}