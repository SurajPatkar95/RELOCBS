using RELOCBS.Models;
using RELOCBS.Services.Implementation;
using RELOCBS.Services.Interfaces;
using RELOCBS.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using RELOCBS.App_Code;
using RELOCBS.DAL.Account;
using System.Web.Security;

namespace RELOCBS.CustomAttributes
{
    public class AuthorizeUserAttribute : AuthorizeAttribute
    {
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

        private CommonSubs _cSubs;
        public CommonSubs cSubs
        {
            get
            {
                if (this._cSubs == null)
                    this._cSubs = new CommonSubs();
                return this._cSubs;
            }
        }

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

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var authroized = base.AuthorizeCore(httpContext);
            if (!authroized)
            {
                // the user is not authenticated or the forms authentication
                // cookie has expired
                return false;
            }

            // Now check the session:
            var currentLoginUser = UserSession.GetUserSession();
            if (currentLoginUser == null)
            {
                // the session has expired
                return false;
            }
            else if (currentLoginUser!=null && currentLoginUser.SessionID != Convert.ToString(cSubs.GetValue("Select TempSessionId from Access.UserLogin where LoginID=" + cSubs.QSafeValue(currentLoginUser.LoginID.ToString()))) && new string[] { "AA", "SA", "CU", "LU", "AU" }.Contains(currentLoginUser.LoginType))
            {
                userDAL.UpdateLoginStatus(UserSession.GetUserSession(), LoginStatus.FORCEOUT);
                session.Set<UserInformationModel>("UserSession", null);
                session.Set<UserInformationModel>("UserMenuTable", null);
                session.Set<UserInformationModel>("UserMenu", null);
                FormsAuthentication.SignOut();
                UserSession.AbandonSession();

                return false;
            }
            

            return true;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            var context = filterContext.HttpContext;
            if (context.Request.IsAjaxRequest())
            {
                context.Response.StatusCode = 440;
                filterContext.Result = new JsonResult { Data = "Session Timeout. Redirecting...", ContentType = "application/json", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
            }
            else
            {
                filterContext.Result = new RedirectToRouteResult(
                    new System.Web.Routing.RouteValueDictionary
                    {
                            { "Controller", "Account" },
                            { "Action", "Login" }
                    });
            }

        }

    }
}