using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;
using RELOCBS.App_Code;
using RELOCBS.BL;
using RELOCBS.Common;
using RELOCBS.Entities;
using RELOCBS.Models;
using RELOCBS.Utility;

namespace RELOCBS.Reports
{


    public partial class ReportViewer : System.Web.UI.Page
    {
        private ComboBL _comboBL;
        private CommonSubs _CSub;
        DataSet dataSet = new DataSet();
        //private Enquiry _Enq;
        public ComboBL comboBL
        {

            get
            {
                if (this._comboBL == null)
                    this._comboBL = new ComboBL();
                return this._comboBL;
            }
        }

        public CommonSubs CSub
        {
            get
            {
                if (this._CSub == null)
                    this._CSub = new CommonSubs();
                return this._CSub;
            }
        }

        //private CustomSessionStore _session;
        //public CustomSessionStore session
        //{
        //    get
        //    {
        //        if (this._session == null)
        //            this._session = new CustomSessionStore();
        //        return this._session;
        //    }
        //}

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                UserInformationModel user = UserSession.GetUserSession();

                if (user != null)
                {
                    EncryptedQueryString args = new EncryptedQueryString(Request.QueryString["args"]);

                    if (UserSession.HasPermission(Convert.ToString(args["PageID"]), EnumUtility.PageAction.VIEW))
                    {
                        if (!IsPostBack)
                        {
                            string ReportID = string.Empty;

                            if (args["ReportID"] != null)
                            {
                                ReportID = Convert.ToString(args["ReportID"]);
                            }

                            if (!string.IsNullOrWhiteSpace(ReportID))
                            {
                                
                                string Url = string.Empty;
                                string DataSetName = string.Empty;
                                ReportViewer1.LocalReport.DataSources.Clear();
                                //ReportDataSource RDS1;
                                switch (ReportID)
                                {
                                    case "10001":
                                        Url = Server.MapPath("~/Reports/Rpt_Enquiry.rdlc");
                                        //dataSet = (DataSet)session.Get<DataSet>("ReportSession");
                                        dataSet = (new DAL.Enquiry.EnquiryDL()).GetDetailById(0,Convert.ToInt32(args["EnqID"]));
                                        DataSetName = "DataSet";
                                        //RDS1 = new ReportDataSource(DataSetName, dataSet.Tables[0]);
                                        //ReportViewer1.LocalReport.DataSources.Add(RDS1);
                                        break;
                                    case "10002":
                                        //Url = Server.MapPath("~/Reports/Rpt_Survey.rdlc");
                                        Url = Server.MapPath("~/Reports/Survey_Rpt.rdlc");
                                        dataSet = (new DAL.Survey.SurveyDAL()).GetDetailById(user.LoginID, Convert.ToInt32(args["EnqDetailID"]), Convert.ToInt32(args["SurveyID"]));
                                        DataSetName = "DataSet";
                                        break;
                                    //RDS1 = new ReportDataSource("DataSet1", dataSet.Tables[0]);
                                    //RDS1.DataSourceId = DataSetName;
                                    //ReportViewer1.LocalReport.DataSources.Add(RDS1);
                                    case "10003":
                                        Url = Server.MapPath("~/Reports/NRM_CostEstimate_Rpt.rdlc");
                                        dataSet = (new DAL.Pricing.CostDAL()).GetDetailById(Convert.ToInt32(args["surveyid"]), Convert.ToInt32(args["Wtid"]), user.LoginID, Convert.ToInt32(args["Wtid"]));
                                        DataSetName = "DataSet";
                                        break;

                                    case "10004":
                                        Url = Server.MapPath("~/Reports/NRM_Quotation_Rpt.rdlc");
                                        dataSet = (new DAL.Pricing.QuotingDAL()).GetDetailById(Convert.ToInt32(args["surveyid"]), Convert.ToInt32(args["Wtid"]), user.LoginID,0);
                                        DataSetName = "DataSet";
                                        break;

                                    case "10005":
                                        Url = Server.MapPath("~/Reports/NRM_EstimateCompare.rdlc");
                                        DataSet ds = (new BL.Pricing.CostBL()).GetCompareRate(Convert.ToInt32(args["surveyid"]), user.LoginID);
                                        dataSet = new DataSet();
                                        DataSetName = "DataSet";
                                        if (ds!=null && ds.Tables.Count>2)
                                        {
                                            dataSet.Tables.Add(GetReportCells(ds.Tables[0]));
                                            dataSet.Tables.Add(GetReportCells(ds.Tables[1]));
                                            dataSet.Tables.Add(ds.Tables[2].Copy());
                                        }
                                        break;


                                    case "10006":
                                        Url = Server.MapPath("~/Reports/Quotation_Rpt.rdlc");
                                        dataSet = (new DAL.Pricing.QuotingDAL()).GetQuotingPrint(Convert.ToInt32(args["surveyid"]), Convert.ToInt32(args["Wtid"]), Convert.ToInt32(args["Batchid"]), Convert.ToBoolean(args["IsLumsum"]),user.CompanyID, user.LoginID, Convert.ToInt16(args["IsEmail"]));
                                        DataSetName = "DataSet";
                                        break;



                                    case "10007":
                                        Url = Server.MapPath("~/Reports/Rpt_BNC.rdlc");
                                        dataSet = (new DAL.Billing_Collection.BillingCollectionDAL()).GetDetailById(Convert.ToInt32(args["EnqDetailID"]));
                                        DataSetName = "DataSet";
                                        break;

                                    case "10008":
                                        Url = Server.MapPath("~/Reports/NRM_EstimateCompare.rdlc");
                                        DataSet dsQuote = (new BL.Pricing.QuotingBL()).GetCompareQuote(Convert.ToInt32(args["surveyid"]), user.LoginID);
                                        dataSet = new DataSet();
                                        DataSetName = "DataSet";
                                        if (dsQuote != null && dsQuote.Tables.Count > 2)
                                        {
                                            dataSet.Tables.Add(GetReportCells(dsQuote.Tables[0]));
                                            dataSet.Tables.Add(GetReportCells(dsQuote.Tables[1]));
                                            dataSet.Tables.Add(dsQuote.Tables[2].Copy());
                                        }
                                        break;

                                    default:
                                        break;
                                }

                                if (!string.IsNullOrWhiteSpace(Url) && dataSet != null && dataSet.Tables.Count > 0)
                                {
                                    if (!File.Exists(Url))
                                        return;

                                    ReportViewer1.LocalReport.ReportPath = Url;
                                    ReportViewer1.ProcessingMode = ProcessingMode.Local;
                                    ReportViewer1.SizeToReportContent = true;
                                    ReportViewer1.Width = Unit.Percentage(95);
                                    ReportViewer1.Height = Unit.Percentage(100);
                                    int Count = 1;
                                    foreach (System.Data.DataTable dt in dataSet.Tables)
                                    {
                                        ReportDataSource RDS1 = new ReportDataSource(DataSetName + Convert.ToString(Count), dt);
                                        ReportViewer1.LocalReport.DataSources.Add(RDS1);
                                        Count = Count + 1;
                                    }

                                    if (ReportID== "10001")
                                    {
                                      ReportViewer1.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(SetSubDataSource);
                                    }
                                    ReportViewer1.LocalReport.EnableExternalImages = true;
                                   // ReportViewer1.LocalReport.Refresh();
                                }
                            }

                        }

                    }

                }
                else
                {
                    var page = HttpContext.Current.Handler as Page;
                    Response.Redirect(page.GetRouteUrl("Default",
                        new { Controller = "Home", Action = "Index" }), false);
                    //Response.Redirect("/Home/Index",true);
                }
            }
            catch (System.Threading.ThreadAbortException) { }
            catch (Exception ex)
            {
                CSub.LogError(this.Page, "Page_Load", ex.ToString());
            }

        }

        private void GetReportData(Enquiry enq)
        {

        }

        public DataTable GetReportCells(DataTable table)
        {
            return ListtoDataTable.ToDataTable(ReportCell.ConvertTableToCells(table),table.TableName);
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), this.ClientID, ";", true);
        }

        public void SetSubDataSource(object sender, SubreportProcessingEventArgs e)
        {
            if (dataSet!=null && dataSet.Tables.Count>0)
            {
                e.DataSources.Add(new ReportDataSource("DataSet1", dataSet.Tables[1]));
            }
        }
    }
    
}

