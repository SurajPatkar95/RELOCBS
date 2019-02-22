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
            var myvar = UserSession.GetUserSession();
            if (myvar==null)
            {
                // the session has expired
                return false;
            }

            return true;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectToRouteResult(
                        new RouteValueDictionary(
                            new
                            {
                                controller = "Account",
                                action = "Login"
                            })
                        );
        }

    }
}