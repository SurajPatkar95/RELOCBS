using RELOCBS.App_Code;
using RELOCBS.Common.ExceptionHandling;
using RELOCBS.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;

namespace RELOCBS.DAL.Report
{
    public class CommonReportDAL
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

        public IEnumerable<SelectListItem> GetReportsList(Entities.CommonReport report)
        {
            int LoggedinUserID = UserSession.GetUserSession().LoginID;
            int CompId = UserSession.GetUserSession().CompanyID;
            try
            {
                return CSubs.BindDropdown(string.Format("[Report].[GetReportList] @sp_CompID={0},@SP_Loginid={1}", CSubs.QSafeValue(Convert.ToString(CompId)), CSubs.QSafeValue(Convert.ToString(LoggedinUserID))));
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoggedinUserID), "CommonReportDAL", "GetReportsList", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
        }

        public DataTable GetReport(Entities.CommonReport report,out string result)
        {
            int LoggedinUserID = UserSession.GetUserSession().LoginID;
            try
            {
                DataTable dt = new DataTable();
                result = string.Empty;
				bool RMCBuss = !(UserSession.GetUserSession().BussinessLine == "NON RMC-BUSINESS");
				string ServiceLineXml = null;
				string BranchXml = null;
				if (report.ServiceLineId != null)
				{
					XElement xmlElements2 = new XElement("root", report.ServiceLineId.Select(x => new XElement("SerLineIDs", new XElement("SerLineID", x))));

					ServiceLineXml = xmlElements2.ToString();

				}

				if (report.RevenueBranchId != null)
				{
					XElement xmlElements = new XElement("root", report.RevenueBranchId.Select(x => new XElement("BrIDs", new XElement("BrID", x))));
					//XElement xmlElements = new XElement("BrIDs", fa.RevenueBranchId.Select(i => new XElement("BrID", i)));
					BranchXml = xmlElements.ToString();

				}
				//XElement xmlElements = new XElement("BrIDs", report.RevenueBranchId.Select(i => new XElement("BrID", i)));
				//XElement xmlElements2 = new XElement("SerLineIDs", report.ServiceLineId.Select(i => new XElement("SerLineID", i)));
				//string ServiceLineXml = xmlElements2.ToString();
				//string BranchXml = xmlElements.ToString();

				using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
					
                    if (conn.Connect())
                    {
                        conn.AddCommand("[Report].[GetAllReportDetails]", QueryType.Procedure);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ReportID", SqlDbType.Int, 0, ParameterDirection.Input, report.ReportID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_FromDate", SqlDbType.DateTime, 0, ParameterDirection.Input, report.FromDate);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ToDate", SqlDbType.DateTime, 0, ParameterDirection.Input, report.Todate);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_DateName", SqlDbType.Int, 0, ParameterDirection.Input, report.SelectedDateType);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_SerLine", SqlDbType.Xml, -1, ParameterDirection.Input, ServiceLineXml);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Branch", SqlDbType.Xml, -1, ParameterDirection.Input, BranchXml);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CompID", SqlDbType.Int, 0, ParameterDirection.Input, UserSession.GetUserSession().CompanyID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IsRMCBuss", SqlDbType.Bit, -1, ParameterDirection.Input, RMCBuss);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LoginID", SqlDbType.Int, 0, ParameterDirection.Input, UserSession.GetUserSession().LoginID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@Out_Message", SqlDbType.NVarChar, -1, ParameterDirection.Output);
                        dt = (DataTable)conn.ExecuteProcedure(ProcedureReturnType.DataTable);

                        if(conn.IsError)
                        {
                            result = RELOCBS.Properties.Resources.UnExpectedErrorAtPL;
                            CSubs.LogError("CommonReportBL", "GetReport", conn.ErrorMessage);
                            return dt;
                        }
                    }
                    else
                        throw new Exception(conn.ErrorMessage);
                }
                return dt;
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoggedinUserID), "CommonReportDAL", "GetGrid", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

            //return null;


        }

        public IEnumerable<SelectListItem> GetDateColList(int ReportId)
        {
            return CSubs.BindDropdown(string.Format("[Report].[GetReportDateFilterName]  @SP_ReportID={0},@SP_Loginid={1},@sp_CompID={2}", CSubs.QSafeValue(Convert.ToString(ReportId)), CSubs.QSafeValue(Convert.ToString(UserSession.GetUserSession().LoginID)), CSubs.QSafeValue(Convert.ToString(UserSession.GetUserSession().CompanyID))), false);
        }
    }
}