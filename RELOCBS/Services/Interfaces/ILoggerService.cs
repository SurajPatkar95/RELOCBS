using RELOCBS.Entities;
using RELOCBS.Services.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RELOCBS.Services.Interfaces
{
    public interface ILoggerService
    {
        /// <summary>
        /// Inserts a log item
        /// </summary>
        /// <param name="context">The log context</param>
        /// <returns>Always return <c>null</c></returns>
        void InsertLog(LogContext context);

        /// <summary>
        /// Inserts a log item
        /// </summary>
        /// <param name="logLevel">Log level</param>
        /// <param name="shortMessage">The short message</param>
        /// <param name="fullMessage">The full message</param>
        /// <param name="user">The user to associate log record with</param>
        /// <returns>Always return <c>null</c></returns>
        void InsertLog(LogLevel logLevel, string shortMessage, string fullMessage = "", string LoginID = "");

        /// <summary>
        /// Insert a log item
        /// </summary>
        /// <param name="user">User data</param>
        /// <param name="IPAddress">IP Address</param>
        /// <param name="PageUrl">Url</param>
        /// <param name="ActivityShortName">Activity Abbrivation</param>
        /// <param name="Activity">Actual Description</param>
        void InsertLog(string LoginID = "", string IPAddress = "", string PageUrl = "", string ActivityShortName = "", string Activity = "");

        void Error(string Message, Exception exc, string Module = "", string objPage = "", string LoginID = "");
    }
}
