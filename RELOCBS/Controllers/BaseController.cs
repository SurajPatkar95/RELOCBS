using RELOCBS.Entities;
using RELOCBS.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RELOCBS.Services.Implementation;
using RELOCBS.Utility;
using RELOCBS.Models;

namespace RELOCBS.Controllers
{
    /// <summary>
    /// This will be base controller for all controllers.
    /// like HomeController:BaseController
    /// </summary>
    public class BaseController : Controller
    {

        RELOCBS.Services.Implementation.Settings _settings;

        public RELOCBS.Services.Implementation.Settings settings
        {
            get
            {
                if (this._settings == null)
                    this._settings = new RELOCBS.Services.Implementation.Settings();
                return this._settings;
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

        private LoggerService _logger;

        public ILoggerService Logger
        {
            get
            {
                if (this._logger == null)
                    this._logger = new LoggerService();
                return this._logger;
            }
        }


        protected BaseController()
        {
            
            //ViewBag.UserMenu = UserSession.GetUserMenu();
            ViewBag.SiteTitle = settings.GetSettingByKey<string>("site_name");
            ViewBag.SiteTitleShort = settings.GetSettingByKey<string>("site_name_short");
            ViewBag.Copyright = settings.GetSettingByKey<string>("site_copyright");
            ViewBag.SiteNameSubtitle = settings.GetSettingByKey<string>("site_name_subtitle");
            ViewBag.LoginId =  (UserSession.GetUserSession()==null) ? 0 : UserSession.GetUserSession().LoginID;
        }

        

        //public ICommonServices Services
        //{
        //    get;
        //    set;
        //}

        /// <summary>
        /// Pushes an info message to the notification queue
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="durable">A value indicating whether the message should be persisted for the next request</param>
        protected virtual void NotifyInfo(string message, bool durable = true)
        {
            //Services.Notifier.Information(message, durable);
        }

        /// <summary>
        /// Pushes a warning message to the notification queue
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="durable">A value indicating whether the message should be persisted for the next request</param>
        protected virtual void NotifyWarning(string message, bool durable = true)
        {
            //Services.Notifier.Warning(message, durable);
        }

        /// <summary>
        /// Pushes a success message to the notification queue
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="durable">A value indicating whether the message should be persisted for the next request</param>
        protected virtual void NotifySuccess(string message, bool durable = true)
        {
            //Services.Notifier.Success(message, durable);
        }

        /// <summary>
        /// Pushes an error message to the notification queue
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="durable">A value indicating whether the message should be persisted for the next request</param>
        protected virtual void NotifyError(string message, bool durable = true)
        {
            //Services.Notifier.Error(message, durable);
        }

        /// <summary>
        /// Pushes an error message to the notification queue
        /// </summary>
        /// <param name="exception">Exception</param>
        /// <param name="durable">A value indicating whether a message should be persisted for the next request</param>
        /// <param name="logException">A value indicating whether the exception should be logged</param>
        protected virtual void NotifyError(Exception exception, bool durable = true, bool logException = true)
        {
            if (logException)
            {
                LogException(exception);
            }

           // Services.Notifier.Error(HttpUtility.HtmlEncode(exception.ToAllMessages()), durable);
        }

        /// <summary>
        /// Pushes an error message to the notification queue that the access to a resource has been denied
        /// </summary>
        /// <param name="durable">A value indicating whether a message should be persisted for the next request</param>
        /// <param name="log">A value indicating whether the message should be logged</param>
        protected virtual void NotifyAccessDenied(bool durable = true, bool log = true)
        {
            var message = "Admin.AccessDenied.Description";

            if (log)
            {
                Logger.Error(message, null, Convert.ToString(UserSession.GetUserSession().LoginID));
            }

            //Services.Notifier.Error(message, durable);
        }

        protected virtual ActionResult RedirectToReferrer()
        {
            if (Request.UrlReferrer != null && !string.IsNullOrWhiteSpace(Convert.ToString(Request.UrlReferrer)))
            {
                return Redirect(Request.UrlReferrer.ToString());
            }

            return RedirectToRoute("HomePage");
        }

        protected virtual ActionResult RedirectToHomePageWithError(string reason, bool durable = true)
        {
            string message = ""; //("Common.RequestProcessingFailed", this.RouteData.Values["controller"], this.RouteData.Values["action"], reason.NaIfEmpty());

            //Services.Notifier.Error(message, durable);

            return RedirectToRoute("HomePage");
        }


        /// <summary>
        /// On exception
        /// </summary>
        /// <param name="filterContext">Filter context</param>
        protected override void OnException(ExceptionContext filterContext)
        {
            if (filterContext.Exception != null)
            {
                string actionName = (string)filterContext.RouteData.Values["Action"];
                string ControllerName = (string)filterContext.RouteData.Values["Controller"];

                LogException( filterContext.Exception, ControllerName, actionName);
            }

            base.OnException(filterContext);
        }

        /// <summary>
        /// Log exception
        /// </summary>
        /// <param name="exc">Exception</param>
        private void LogException(Exception exc,string ControllerName="",string ActionName="")
        {
            int user = UserSession.GetUserSession()!= null ? UserSession.GetUserSession().LoginID : 0;
            Logger.Error(exc.Message, exc,ActionName, ControllerName,Convert.ToString(user));
        }

        /// <summary>
        /// Log Activity
        /// </summary>
        /// <param name="PageUrl"></param>
        /// <param name="ActivityShortName"></param>
        /// <param name="Activity"></param>
        protected void LogActivity(string PageUrl = "", string ActivityShortName = "", string Activity = "")
        {
            int user = UserSession.GetUserSession().LoginID;
            string IPAddress = Request.ServerVariables["HTTP_X_FORWARDED_FOR"] ?? Request.ServerVariables["REMOTE_ADDR"];
            Logger.InsertLog(Convert.ToString(user), IPAddress, PageUrl, ActivityShortName, Activity);
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //if (this.Services.WorkContext.CurrentUser != null && this.Services.WorkContext.CurrentUser.UserDetails == null && !(filterContext.ActionDescriptor.ControllerDescriptor.ControllerName == "Account" && filterContext.ActionDescriptor.ActionName == "Profile"))
            //{
              //  filterContext.Result = RedirectToAction("Profile", "Account");
            //}
        }

    }
}
