using RELOCBS.Common.ExceptionHandling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;

namespace RELOCBS
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_PostAuthenticateRequest(Object sender, EventArgs e)
        {
            if (FormsAuthentication.CookiesSupported == true)
            {
                if (Request.Cookies[FormsAuthentication.FormsCookieName] != null)
                {
                    try
                    {
                        HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
                        FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);

                        //let us take out the username now                
                        string username = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
                        string roles = string.Empty;

                        //using (userDbEntities entities = new userDbEntities())
                        //{
                        //    User user = entities.Users.SingleOrDefault(u => u.username == username);

                        //    roles = user.Roles;
                        //}
                        ////let us extract the roles from our own custom cookie


                        //Let us set the Pricipal with our user specific details
                        HttpContext.Current.User = new System.Security.Principal.GenericPrincipal(
                          new System.Security.Principal.GenericIdentity(username, "Forms"), roles.Split(';'));
                    }
                    catch (Exception)
                    {
                        //somehting went wrong
                    }
                }
            }

        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception[] exs = this.Context.AllErrors;

            Exception ex = Server.GetLastError();

            if (ex is HttpRequestValidationException)
            {
                Response.Clear();
                Response.StatusCode = 200;
                Response.Write(@"
                <html><head><title>HTML Not Allowed</title>
                <script language='JavaScript'><!--
                function back() { history.go(-1); } //--></script></head>
                <body style='font-family: Arial, Sans-serif;'>
                <h1>Oops!</h1>
                <p>I'm sorry, but HTML entry is not allowed on that page.</p>
                <p>Please make sure that your entries do not contain 
                any angle brackets like &lt; or &gt;.</p>
                <p><a href='javascript:back()'>Go back</a></p>
                </body></html>
                ");
                Response.End();
            }
            else
            if (ex as System.Threading.ThreadAbortException != null)// || ex as BLLException != null)
            {
                //Do not handle any ThreadAbortException or BLLException here
                //Only handle other custom exception (DALException in this example)
                //and any other unhandled Exceptions here

                //ErrorHandler.ReportError(ex);
                Response.Redirect(string.Format("Error.aspx?aspxerrorpath={0}",
                        Request.Url.PathAndQuery));
            }
            else if (ex as BussinessLogicException != null)// || ex as BLLException != null)
            {
                //Do not handle any ThreadAbortException or BLLException here
                //Only handle other custom exception (DALException in this example)
                //and any other unhandled Exceptions here

                //ErrorHandler.ReportError(ex);
                Response.Redirect(string.Format("Error.aspx?aspxerrorpath={0}",
                        Request.Url.PathAndQuery));
            }

        }

    }
}
