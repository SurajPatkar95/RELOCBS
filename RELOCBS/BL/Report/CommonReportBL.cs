using RELOCBS.Common.ExceptionHandling;
using RELOCBS.DAL.Report;
using RELOCBS.Entities;
using RELOCBS.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RELOCBS.BL.Report
{
    public class CommonReportBL
    {
        private CommonReportDAL _reportDAL;

        public CommonReportDAL reportDAL
        {

            get
            {
                if (this._reportDAL == null)
                    this._reportDAL = new CommonReportDAL();
                return this._reportDAL;
            }
        }

        public IEnumerable<SelectListItem> GetReportsList(Entities.CommonReport report)
        {
            IEnumerable<SelectListItem> model;
            try
            {
                model = reportDAL.GetReportsList(report);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "CommonReportBL", "GetReportsList", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

            return model;
        }

        public IEnumerable<SelectListItem> GetDateColList(int  ReportId)
        {
            return reportDAL.GetDateColList(ReportId);
        }

        public DataTable GetReport(Entities.CommonReport report,string submit, out string result)
        {
            DataTable dt = new DataTable();
            result = string.Empty;

            try
            {
                dt = reportDAL.GetReport(report,out result);

                

                if (submit.ToUpper() == "GETCOLUMNSLIST")
                {
                    if (dt != null && dt.Columns.Count >= 1)
                    {
                        //report.ReportColumns = dt.Columns.Cast<ReportColumn>().Select(x => new Entities.ReportColumn() { ColumnID= x.ColumnName, ColumnName=x.ColumnName }).ToList();
                        report.ReportColumns = (from DataColumn x in dt.Columns
                                                select new Entities.ReportColumn() { ColumnID = x.ColumnName, ColumnName = x.ColumnName }
                                                ).ToList<Entities.ReportColumn>();
                    }

                }

                if (submit.ToUpper()=="GENERATE")
                {
                    if (dt == null || dt.Rows.Count <= 0)
                    {
                        result = "No records to download";
                    }

                    if (dt != null && dt.Columns.Count >= 1)
                    {
                        var toRemove = dt.Columns.Cast<DataColumn>().Select(x => x.ColumnName).Except(report.ReportColumns.Where(m => m.Selected == true).Select(m => m.ColumnID).ToList()).ToList();

                        foreach (var col in toRemove) dt.Columns.Remove(col);
                    }
                }
                
                return dt;
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "CommonReportBL", "GetReport", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }
        }
    }
}