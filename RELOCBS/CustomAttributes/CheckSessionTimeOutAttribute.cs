using RELOCBS.App_Code;
using RELOCBS.DAL.Account;
using RELOCBS.Models;
using RELOCBS.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI.WebControls;

namespace RELOCBS.CustomAttributes
{
    [AttributeUsage(AttributeTargets.All, Inherited = true, AllowMultiple = false)]
    public class CheckSessionTimeOutAttribute : ActionFilterAttribute
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


        public override void OnActionExecuting(System.Web.Mvc.ActionExecutingContext filterContext)
        {
            var context = filterContext.HttpContext;
            if (context.Session != null)
            {
                if (context.Session.IsNewSession)
                {
                    string sessionCookie = context.Request.Headers["Cookie"];
                    if ((sessionCookie != null) && (sessionCookie.IndexOf("ASP.NET&#95;SessionId") >= 0))
                    {
                        FormsAuthentication.SignOut();
                        if (context.Request.IsAjaxRequest())
                        {
                            filterContext.Result = new JsonResult { Data = "Session Timeout. Redirecting...", ContentType = "application/json" };
                            context.Response.StatusCode = 440;
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

                       // return;

                    }
                }
                else if (!string.IsNullOrEmpty(context.Session.SessionID) && context.Session["UserSession"]!=null)
                {
                    var currentLoginUser = UserSession.GetUserSession();
                    if (currentLoginUser != null && currentLoginUser.SessionID != Convert.ToString(cSubs.GetValue("Select TempSessionId from Access.UserLogin where LoginID=" + cSubs.QSafeValue(currentLoginUser.LoginID.ToString()))) && new string[] { "AA", "SA", "CU", "LU", "AU" }.Contains(currentLoginUser.LoginType))
                    {
                        userDAL.UpdateLoginStatus(UserSession.GetUserSession(), DAL.Account.LoginStatus.FORCEOUT);
                        session.Set<UserInformationModel>("UserSession", null);
                        session.Set<UserInformationModel>("UserMenuTable", null);
                        session.Set<UserInformationModel>("UserMenu", null);
                        FormsAuthentication.SignOut();
                        UserSession.AbandonSession();


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
            base.OnActionExecuting(filterContext);
        }
    }
}