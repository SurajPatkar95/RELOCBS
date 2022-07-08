using RELOCBS.App_Code;
using RELOCBS.Common.ExceptionHandling;
using RELOCBS.Entities;
using RELOCBS.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace RELOCBS.DAL.FundTranfer
{
	public class FundTransferDAL
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

        public IQueryable<Entities.InvoiceGrid> GetFDGridDetails(Entities.FundTranfer fa,out DataTable dt)
		{
			
			int LoggedinUserID = UserSession.GetUserSession().LoginID;
			bool RMCBuss = !(UserSession.GetUserSession().BussinessLine == "NON RMC-BUSINESS");
			try
			{

				IQueryable<Entities.InvoiceGrid> data = null;
				string ServiceLineXml = null;
				string BranchXml = null;
				if (fa.ServiceLineId!=null)
				{
					XElement xmlElements2 = new XElement("root", fa.ServiceLineId.Select(x => new XElement("SerLineIDs", new XElement("SerLineID", x))));
					
					ServiceLineXml = xmlElements2.ToString();

				}

				if (fa.RevenueBranchId!=null)
				{
					XElement xmlElements = new XElement("root", fa.RevenueBranchId.Select(x => new XElement("BrIDs", new XElement("BrID", x))));
					//XElement xmlElements = new XElement("BrIDs", fa.RevenueBranchId.Select(i => new XElement("BrID", i)));
					BranchXml = xmlElements.ToString();
					
				}
				


				
				using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
				{
					if (conn.Connect())
					{
						conn.AddCommand("[Report].[TransferToFA]", QueryType.Procedure);
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_FromDate", SqlDbType.DateTime, 0, ParameterDirection.Input, fa.FromDate);
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ToDate", SqlDbType.DateTime, 0, ParameterDirection.Input, fa.ToDate);
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LoginID", SqlDbType.Int, 8, ParameterDirection.Input, LoggedinUserID);
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_InvoiceNumber", SqlDbType.VarChar, 15, ParameterDirection.Input, fa.BillNo);
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CompId", SqlDbType.Int, 8, ParameterDirection.Input, UserSession.GetUserSession().CompanyID);
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Status", SqlDbType.VarChar, 10, ParameterDirection.Input, fa.SearchFor);
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_SerLine", SqlDbType.Xml, -1, ParameterDirection.Input, ServiceLineXml);
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Branch", SqlDbType.Xml, -1, ParameterDirection.Input, BranchXml);
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IsRmcBuss", SqlDbType.Bit, 0, ParameterDirection.Input, RMCBuss);
						//conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IsRMCBuss", SqlDbType.Bit, 0, ParameterDirection.Input, RMCBuss);
						dt = (DataTable)conn.ExecuteProcedure(ProcedureReturnType.DataTable);

						if (dt != null)
						{
							var result = (from rw in dt.AsEnumerable()
										  select new Entities.InvoiceGrid()
										  {
											  Layout = Convert.ToString(rw["Layout"]),
											  BillNo = Convert.ToString(rw["Sales Invoice Number"]),
											  BillDate = rw["Invoice Date"] == DBNull.Value ? default(DateTime) : Convert.ToDateTime(rw["Invoice Date"]),
											  FAClientCode = rw["Customer Code"] == DBNull.Value ? null : Convert.ToString(rw["Customer Code"]),
											  AccountCode = rw["Paid to /Received From (2nd Leg)"] == DBNull.Value ? null : Convert.ToString(rw["Paid to /Received From (2nd Leg)"]),
											  CustomerReference = rw["Customer Reference"] == DBNull.Value ? null : Convert.ToString(rw["Customer Reference"]),
											  SalesDefinition = rw["Sales Definition"] == DBNull.Value ? null : Convert.ToString(rw["Sales Definition"]),
											  Comment = rw["Comment"] == DBNull.Value ? null : Convert.ToString(rw["Comment"]),
											  FromDate = rw["From Date"] == DBNull.Value ? null : Convert.ToString(rw["From Date"]),
											  ToDate = rw["To Date"] == DBNull.Value ? null : Convert.ToString(rw["To Date"]),
											  LineNo = rw["Line No"] == DBNull.Value ? null : Convert.ToString(rw["Line No"]),
											  ItemCode = rw["Item Code"] == DBNull.Value ? null : Convert.ToString(rw["Item Code"]),
											  Description = rw["Description"] == DBNull.Value ? null : Convert.ToString(rw["Description"]),
											  UnitofSale = rw["Unit of Sale"] == DBNull.Value ? null : Convert.ToString(rw["Unit of Sale"]),
											  Qty = rw["Qty"] == DBNull.Value ? null : Convert.ToString(rw["Qty"]),
											  Rate = rw["Rate"] == DBNull.Value ? null : Convert.ToString(rw["Rate"]),
											  Currency = rw["Currency"] == DBNull.Value ? null : Convert.ToString(rw["Currency"]),
											  Value = rw["Value"] == DBNull.Value ? null : Convert.ToString(rw["Value"]),
											  CGST = rw["CGST"] == DBNull.Value ? null : Convert.ToString(rw["CGST"]),
											  SGST = rw["SGST"] == DBNull.Value ? null : Convert.ToString(rw["SGST"]),
											  IGST = rw["IGST"] == DBNull.Value ? null : Convert.ToString(rw["IGST"]),
											  FACode = rw["SBU/Business Line/Product"] == DBNull.Value ? null : Convert.ToString(rw["SBU/Business Line/Product"]),
											  Project = rw["Job no/ Project /Revenue Branch"] == DBNull.Value ? null : Convert.ToString(rw["Job no/ Project /Revenue Branch"]),
											  Miscellaneous = rw["Miscellaneous"] == DBNull.Value ? null : Convert.ToString(rw["Miscellaneous"]),
											  TaxCode = rw["Tax Code"] == DBNull.Value ? null : Convert.ToString(rw["Tax Code"]),
											  MISLocation = rw["MIS Location"] == DBNull.Value ? null : Convert.ToString(rw["MIS Location"]),
											  Employee = rw["Function/Employee"] == DBNull.Value ? null : Convert.ToString(rw["Function/Employee"]),
											  CBSRefID = rw["CBSRefID"] == DBNull.Value ? null : Convert.ToString(rw["CBSRefID"]),
											  InvOrCredit = rw["InvOrCredit"] == DBNull.Value ? null : Convert.ToString(rw["InvOrCredit"]),
											  BillTo= rw["BillTo"] == DBNull.Value ? null : Convert.ToString(rw["BillTo"]),
											  GSTFlag = rw["GSTFlag"] == DBNull.Value ? null : Convert.ToString(rw["GSTFlag"]),
											  //Employee = rw["Paid to /Received From (2nd Leg)"] == DBNull.Value ? null : Convert.ToString(rw["Paid to /Received From (2nd Leg)"]),
										  }).ToList();
							data = result.AsQueryable<Entities.InvoiceGrid>();
						}
					}
					else
						throw new Exception(conn.ErrorMessage);
				}
				return data;
			}
			catch (Exception ex)
			{
				throw new DataAccessException(Convert.ToString(LoggedinUserID), "EnquiryDAL", "GetEnquiryList", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
			}
		}

		public bool InsertTransferFA(XElement Billitems, int LoginID, out string result)
		{
			result = String.Empty;

			try
			{
				string BillitemsString = Billitems.HasElements ? Convert.ToString(Billitems) : null;
				//System.Xml.Linq.XNode node = JsonConvert.DeserializeXNode(SaveRate.MoveJob.HFVMoveRateCompList, "CostHeadwiseDetails");
				//string QuotingHeadXml = node.ToString();
				using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
				{
					if (conn.Connect())
					{
						conn.AddCommand("[Inv].[AddEditTransferToFA]", QueryType.Procedure);
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ListSelected", SqlDbType.Xml, 0, ParameterDirection.Input, BillitemsString);
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LoginID", SqlDbType.BigInt, 0, ParameterDirection.Input, LoginID);
						conn.AddParameter(ParameterOF.PROCEDURE, "@RETURNSTATUS", SqlDbType.SmallInt, 0, ParameterDirection.ReturnValue);
						conn.AddParameter(ParameterOF.PROCEDURE, "@Out_Message", SqlDbType.NVarChar, 500, ParameterDirection.Output);
						conn.ExecuteProcedure(ProcedureReturnType.SingleValue);

						if (!conn.IsError)
						{
							int ReturnStatus = Convert.ToInt32(conn.GetParameterValue(ParameterOF.PROCEDURE, "@RETURNSTATUS"));
							result = Convert.ToString(conn.GetParameterValue(ParameterOF.PROCEDURE, "@Out_Message"));

							return ReturnStatus == 0;
							
						}
						else
							throw new Exception(conn.ErrorMessage);

					}
					else
						throw new Exception(conn.ErrorMessage);
				}


			}
			catch (Exception ex)
			{
				throw new DataAccessException(Convert.ToString(LoginID), "FundTransfer", "InsertTransferFA", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
			}


		}

        public DataTable GetTransferToFADubai(Entities.FundTranfer report, out string result)
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
                    BranchXml = xmlElements.ToString();

                }
                
                using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {
                        conn.AddCommand("[Report].[TransferToFA_Dubai]", QueryType.Procedure);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_FromDate", SqlDbType.DateTime, 0, ParameterDirection.Input, report.FromDate);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ToDate", SqlDbType.DateTime, 0, ParameterDirection.Input, report.ToDate);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_InvoiceNumber", SqlDbType.VarChar, 20, ParameterDirection.Input, CSubs.PSafeValue(report.BillNo));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CompID", SqlDbType.Int, 0, ParameterDirection.Input, UserSession.GetUserSession().CompanyID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Status", SqlDbType.VarChar, 20, ParameterDirection.Input, CSubs.PSafeValue(report.SearchFor));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_SerLine", SqlDbType.Xml, -1, ParameterDirection.Input, ServiceLineXml);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Branch", SqlDbType.Xml, -1, ParameterDirection.Input, BranchXml);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IsRMCBuss", SqlDbType.Bit, -1, ParameterDirection.Input, RMCBuss);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LoginID", SqlDbType.Int, 0, ParameterDirection.Input, UserSession.GetUserSession().LoginID);
                        //conn.AddParameter(ParameterOF.PROCEDURE, "@Out_Message", SqlDbType.NVarChar, -1, ParameterDirection.Output);
                        dt = (DataTable)conn.ExecuteProcedure(ProcedureReturnType.DataTable);

                        if (conn.IsError)
                        {
                            result = RELOCBS.Properties.Resources.UnExpectedErrorAtPL;
                            CSubs.LogError("FundTransferDAL", "GetTransferToFADubai", conn.ErrorMessage);
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
                throw new DataAccessException(Convert.ToString(LoggedinUserID), "FundTransferDAL", "GetTransferToFADubai", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

        }
    }
}