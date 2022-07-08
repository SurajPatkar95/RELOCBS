using RELOCBS.App_Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RELOCBS.Entities;
using System.Data;
using RELOCBS.Common.ExceptionHandling;
using RELOCBS.Utility;
using System.IO;
using System.Text;

namespace RELOCBS.DAL.TransClearTax
{
	public class TransClearTaxDAL
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

		public DataTable getTransClearTax(Entities.TransClearTax tran)
		{
			int LoginID = UserSession.GetUserSession().LoginID;
			DataTable dt = new DataTable();
			try
			{
				//int CompanyID = UserSession.GetUserSession().CompanyID;
				//bool RMCBuss = UserSession.GetUserSession().BussinessLine != "NON RMC-BUSINESS";
				dt = CSubs.GetDataTable(string.Format("exec [E_Invoice].[ForCombo_InvoiceList]  @SP_AppId={0},@SP_FromDate={1},@SP_ToDate={2},@SP_TransToClearTax={3},@SP_Invoice_Number={4}"
					, CSubs.QSafeValue(Convert.ToString(tran.AppID))
					, CSubs.QSafeValue(Convert.ToString(tran.FromDate))
					, CSubs.QSafeValue(Convert.ToString(tran.ToDate))
					, CSubs.QSafeValue(Convert.ToString(tran.FileTransfer))
					, CSubs.QSafeValue(Convert.ToString(tran.InvNo))));
			}
			catch (Exception ex)
			{
				throw new DataAccessException(Convert.ToString(LoginID), "getTransClearTaxDAL", "TransClearTax", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
			}
			return dt;
		}

		public DataSet GenerateEInvoice(string InvoiceNo,string AppName)
		{
			DataSet InvoiceDetailDt = new DataSet();

			try
			{
				string query = string.Format("EXEC [E_Invoice].[EInvoiceSchemaOtherApp] @SP_CompID={0}, @SP_LoginID={1}, @SP_InvoiceNumber={2},@SP_AppName={3}",
				UserSession.GetUserSession().CompanyID, UserSession.GetUserSession().LoginID, CSubs.QSafeValue(InvoiceNo.ToString()), CSubs.QSafeValue(AppName));
				InvoiceDetailDt = CSubs.GetDataSet(query);
			}
			catch (Exception ex)
			{
				throw new DataAccessException(Convert.ToString(UserSession.GetUserSession().LoginID), "BillingDAL", "GetDetailById", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
			}

			return InvoiceDetailDt;
		}

		public DataTable GetPOSRule(string InvoiceNo, string AppName)
		{
			DataTable InvoiceDetailDt = new DataTable();

			try
			{
				string query = string.Format("EXEC [E_Invoice].[GetPosRuleForOtherApp] @SP_AppName={0},@SP_Inv_Number={1}",
				CSubs.QSafeValue(AppName.ToString()), CSubs.QSafeValue(InvoiceNo.ToString()));
				InvoiceDetailDt = CSubs.GetDataTable(query);
			}
			catch (Exception ex)
			{
				throw new DataAccessException(Convert.ToString(UserSession.GetUserSession().LoginID), "TransClearTaxDAL", "GetPOSRule", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
			}

			return InvoiceDetailDt;
		}
	}
}