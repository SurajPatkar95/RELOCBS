using RELOCBS.App_Code;
using RELOCBS.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace RELOCBS.Services.Implementation
{
    public class LoggerService : ILoggerService
    {
        private CommonSubs _cSubs;
        public CommonSubs CSubs
        {
            get
            {
                if (this._cSubs == null)
                    this._cSubs = new CommonSubs();
                return this._cSubs;

            }
        }

        public void InsertLog(LogContext context)
        {
            //throw new NotImplementedException();
        }

        public void InsertLog(LogLevel logLevel, string shortMessage, string fullMessage = "", string LoginID = null)
        {
            //throw new NotImplementedException();
        }

        public void InsertLog(string LoginID = null, string IPAddress = "", string PageUrl = "", string ActivityShortName = "", string Activity = "")
        {
            //throw new NotImplementedException();
        }

        public void Error(string Message, Exception exc, string Module = "", string objPage = "", string LoginID = "")
        {
            try
            {

                System.Web.HttpContext context = System.Web.HttpContext.Current;
                string FullMessage = "UserID : " + LoginID + Environment.NewLine +
                                     "DateTime  : " + DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss") + Environment.NewLine +
                                     "Page      : " + objPage.ToString() + Environment.NewLine +
                                     "Module    : " + Module.ToString() + Environment.NewLine +
                                     "Error     : " + Environment.NewLine + Environment.NewLine +
                                     Message + Environment.NewLine + Environment.NewLine +
                                     "****************************************************************************************"
                                     + Environment.NewLine;

                try
                {
                    string ErrorLogFolder = System.Configuration.ConfigurationManager.AppSettings["LogFolder"];
                    if (Directory.Exists(ErrorLogFolder) == false)
                    {
                        Directory.CreateDirectory(ErrorLogFolder);
                    }
                    string ErrorFilename = "RELOCBS_LOG_" + DateTime.Now.ToString("dd_MM_yyyy") + ".log";
                    string FullFileName = Path.Combine(ErrorLogFolder, ErrorFilename);
                    File.AppendAllText(FullFileName, FullMessage);
                }
                catch { }

                try
                {
                    CSubs.ExecuteQuery(String.Format("INSERT INTO Access.ErrorLog ([LoginID], ErrorPage, ErrorModule, ErrorDateTime, ErrorDescription) VALUES ({0},{1},{2},GETDATE(),{3})"
                                 , CSubs.QSafeValue(LoginID)
                                 , CSubs.QSafeValue(objPage.ToString())
                                 , CSubs.QSafeValue(Module)
                                 , CSubs.QSafeValue(Message)
                                 ));
                }
                catch { }

            }
            catch { }

            
        }

        /// <summary>
        /// Purpose: Implements the IDispose interface.
        /// </summary>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}