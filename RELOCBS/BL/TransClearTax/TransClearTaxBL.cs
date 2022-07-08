using RELOCBS.App_Code;
using RELOCBS.Common.ExceptionHandling;
using RELOCBS.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using RELOCBS;
using RELOCBS.DAL.TransClearTax;
using RELOCBS.DAL.Billing;
using Newtonsoft.Json;
using RELOCBS.Common;
using System.IO;
using System.Text;

namespace RELOCBS.BL.TransClearTax
{
	public class TransClearTaxBL
	{
		private CommonSubs _CSubs;
		private TransClearTaxDAL _TransClearTaxDAL;

		public CommonSubs CSubs
		{

			get
			{
				if (this._CSubs == null)
					this._CSubs = new CommonSubs();
				return this._CSubs;
			}
		}

		public TransClearTaxDAL transClearTaxDAL
		{

			get
			{
				if (this._TransClearTaxDAL == null)
					this._TransClearTaxDAL = new TransClearTaxDAL();
				return this._TransClearTaxDAL;
			}
		}

		public Entities.TransClearTax getTransClearTax(Entities.TransClearTax trans)
		{
			//Entities.TransClearTax trans = new Entities.TransClearTax();
			int LoginID = UserSession.GetUserSession().LoginID;
			try
			{
				//int CompanyID = UserSession.GetUserSession().CompanyID;
				//bool RMCBuss = UserSession.GetUserSession().BussinessLine != "NON RMC-BUSINESS";
				DataTable dt = transClearTaxDAL.getTransClearTax(trans);
				if (dt != null && dt.Rows.Count > 0)
				{
					trans.InvoiceList = (from item in dt.AsEnumerable()
										 select new Entities.InvoiceList()
										 {
											 InvNo = Convert.ToString(item["InvNo"]),
											 InvDate = Convert.ToDateTime(item["InvDate"]),
											 InvType = Convert.ToString(item["InvType"]),
											 POS = Convert.ToString(item["POS"]),
											 POSStatus = Convert.ToString(item["POSStatus"]),
											 ShowEinvoice = Convert.ToBoolean(item["ShowE_invoice"]),
											 GSTType = Convert.ToString(item["GSTType"]),
											 ///FilePath = CommonSubs.EncryptSf(Convert.ToString(item["DocFilePath"]))
										 }).ToList();

				}
			}
			catch (Exception ex)
			{
				throw new DataAccessException(Convert.ToString(LoginID), "TransClearTaxBL", "getTransClearTax", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
			}
			return trans;
		}

		public string GenerateEInvoice(string InvoiceNo, string AppName)
		{
			try
			{
				DataSet ds = transClearTaxDAL.GenerateEInvoice(InvoiceNo, AppName);
				EInvoiceHandler ForCallJson = new EInvoiceHandler();

				DataTable firstTable = ds.Tables[0];

				string str = "";

				str = ForCallJson.callEInvoiceAPI(Convert.ToString(firstTable.Rows[0]["OwnerID"]), Convert.ToString(firstTable.Rows[0]["GSTNumber"]), Convert.ToString(firstTable.Rows[0]["AuthToken"]), "EInvoice", firstTable, InvoiceNo);
				//str = ForCallJson.callEInvoiceAPI("c804031b-d8ef-4215-bb3e-1b27d3e38943", "29AAFCD5862R000", "c804031b-d8ef-4215-bb3e-1b27d3e38943", "29AAFCD5862R000", firstTable);
				dynamic data = JsonConvert.DeserializeObject(str);
				DataTable Parent = new DataTable();
				DataTable error = new DataTable();
				DataTable cleartaxError = new DataTable();
				DataTable govtError = new DataTable();
				if (data != null && Enumerable.Count(data) > 0)
				{

					Parent.Columns.Add("document_status", typeof(string));
					Parent.Columns.Add("Success", typeof(string));
					Parent.Columns.Add("Inv_No", typeof(string));
					Parent.Columns.Add("transaction_id", typeof(string));
					Parent.Columns.Add("AckNo", typeof(string));
					Parent.Columns.Add("AckDt", typeof(string));
					Parent.Columns.Add("Irn", typeof(string));
					Parent.Columns.Add("SignedInvoice", typeof(string));
					Parent.Columns.Add("SignedQRCode", typeof(string));
					Parent.Columns.Add("Status", typeof(string));
					Parent.Columns.Add("JSONSource", typeof(string));

					for (int i = 0; i < Enumerable.Count(data); i++)
					{
						DataRow dr = Parent.NewRow();
						dr["document_status"] = Convert.ToString(data[i].document_status);
						dr["Success"] = Convert.ToString(data[i].govt_response.Success);
						dr["Inv_No"] = Convert.ToString(data[i].transaction.DocDtls.No);
						dr["transaction_id"] = Convert.ToString(data[i].transaction_id);
						dr["AckNo"] = Convert.ToString(data[i].govt_response.AckNo);
						dr["AckDt"] = Convert.ToString(data[i].govt_response.AckDt);
						dr["Irn"] = Convert.ToString(data[i].govt_response.Irn);
						dr["SignedInvoice"] = Convert.ToString(data[i].govt_response.SignedInvoice);
						dr["SignedQRCode"] = Convert.ToString(data[i].govt_response.SignedQRCode);
						dr["Status"] = Convert.ToString(data[i].govt_response.Status);
						dr["JSONSource"] = Convert.ToString(data[i]);


						Parent.Rows.Add(dr);

						if (data[i].errors != null)
						{
							cleartaxError = JsonConvert.DeserializeObject<DataTable>(Convert.ToString(data[i].errors.errors));
							if (cleartaxError.Columns.Count == 3)
							{
								cleartaxError.Columns.Add("Inv_No", typeof(string));
							}

							foreach (DataRow Cdr in cleartaxError.Rows)
							{
								Cdr["Inv_No"] = Convert.ToString(data[i].transaction.DocDtls.No);
								cleartaxError.AcceptChanges();
							}

							error.Merge(cleartaxError);

						}
						if (Convert.ToString(data[i].govt_response.Success) != "Y")
						{
							govtError = JsonConvert.DeserializeObject<DataTable>(Convert.ToString(data[i].govt_response.ErrorDetails));

							if (govtError.Columns.Count == 3)
							{
								govtError.Columns.Add("Inv_No", typeof(string));
							}
							if (govtError.Columns.Count == 2)
							{
								govtError.Columns.Add("Inv_No", typeof(string));
								govtError.Columns.Add("error_code", typeof(string));
							}
							foreach (DataRow Gdr in govtError.Rows)
							{
								Gdr["Inv_No"] = Convert.ToString(data[i].transaction.DocDtls.No);
								govtError.AcceptChanges();
								if (string.IsNullOrEmpty(Convert.ToString(Gdr["error_code"])))
								{
									Gdr["error_code"] = "0";
								}
							}
							error.Merge(govtError);
						}
					}
				}
				int LoginID = UserSession.GetUserSession().LoginID;
				string result = "";
				bool res = false;
				BillingDAL billingDAL = new BillingDAL();
				res = billingDAL.InsertAPIResponse(LoginID, str, Parent, govtError,AppName, out result);
				DataTable dtResult = new DataTable();
				if (govtError.Rows.Count > 0)
				{
					dtResult = billingDAL.GetAPIErrorDetails(InvoiceNo);
				}
				else
				{
					dtResult = billingDAL.GetIRNDetails(InvoiceNo);
				}
				result = CommonService.MakeHtmlTable(dtResult);
				return result;
			}
			catch (Exception ex)
			{
				throw new DataAccessException(Convert.ToString(UserSession.GetUserSession().LoginID), "TransClearTaxBL", "GenerateEInvoice", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
			}
		}

		public string GetQRCode(string InvoiceNo, string AppName)
		{
			try
			{
				BillingDAL billingDAL = new BillingDAL();
				EInvoiceHandler objInv = new EInvoiceHandler();
				DataTable dtResult = billingDAL.GetIRNDetails(InvoiceNo, true);
				if (dtResult != null && dtResult.Rows.Count > 0)
				{
					string QRCode = Convert.ToString(dtResult.Rows[0]["SignedQRCode"]);
					string FileName = InvoiceNo;
					byte[] ss = objInv.generatecode(QRCode, true, FileName, AppName);
					return "Image successfully saved";
				}
				else
				{
					return "Image for given Invoice does not exist.";
				}
			}
			catch (Exception ex)
			{
				throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "TransClearTaxBL", "GetQRCode", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
			}
			

		}

		public Entities.GSTLogic GetPOSRule(string InvoiceNo, string AppName)
		{
			Entities.GSTLogic trans = new Entities.GSTLogic();
			int LoginID = UserSession.GetUserSession().LoginID;

			try
			{
				//int CompanyID = UserSession.GetUserSession().CompanyID;
				//bool RMCBuss = UserSession.GetUserSession().BussinessLine != "NON RMC-BUSINESS";
				DataTable dt = transClearTaxDAL.GetPOSRule(InvoiceNo, AppName);
				if (dt != null && dt.Rows.Count > 0)
				{
					trans = (from item in dt.AsEnumerable()
							 select new Entities.GSTLogic()
							 {
								 ServiceProvided = Convert.ToString(item["ServiceProvided"]),
								 ServiceProviderInIndia = Convert.ToString(item["ServiceProviderInIndia"]),
								 ServiceReceiverRegistionStatus = Convert.ToString(item["ServiceReceiverRegistionStatus"]),
								 ServiceReceiverInIndia = Convert.ToString(item["ServiceReceiverInIndia"]),
								 OriginInIndia = Convert.ToString(item["OriginInIndia"]),
								 DestInIndia = Convert.ToString(item["DestInIndia"]),
								 IsRoadMode = Convert.ToString(item["IsRoadMode"]),
								 IsRevCurrINR = Convert.ToString(item["IsRevCurrINR"]),
								 ServiceProviderStateID = Convert.ToString(item["ServiceProviderStateID"]),
								 IsPOS_InIndia = Convert.ToString(item["IsPOS_InIndia"]),
								 POS_Rule = Convert.ToString(item["POS_Rule"]),
								 POS_StateID = Convert.ToString(item["POS_StateID"]),
								 GSTTYPE = Convert.ToString(item["GSTTYPE"]),
								 GST_Percent = Convert.ToString(item["GST_Percent"]),
								 //OrgStgPOS_Rule = Convert.ToString(item["OrgStgPOS_Rule"]),
								 //OrgStgIsPOS_InIndia = Convert.ToString(item["OrgStgIsPOS_InIndia"]),
								 //OrgStgPOS_StateID = Convert.ToString(item["OrgStgPOS_StateID"]),
								 //DestStgPOS_Rule = Convert.ToString(item["DestStgPOS_Rule"]),
								 //DestStgIsPOS_InIndia = Convert.ToString(item["DestStgIsPOS_InIndia"]),
								 //DestStgPOS_StateID = Convert.ToString(item["DestStgPOS_StateID"])
								 ///FilePath = CommonSubs.EncryptSf(Convert.ToString(item["DocFilePath"]))
							 }).ToList().First();

				}
			}
			catch (Exception ex)
			{
				throw new DataAccessException(Convert.ToString(LoginID), "TransClearTaxBL", "GetPOSRule", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
			}
			return trans;
		}
	}
}