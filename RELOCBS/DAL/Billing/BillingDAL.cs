using Newtonsoft.Json;
using RELOCBS.App_Code;
using RELOCBS.Common.ExceptionHandling;
using RELOCBS.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace RELOCBS.DAL.Billing
{

	public class BillingDAL
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

		public IQueryable<Entities.Billing> GetForGrid(int LoginID, DateTime? FromDate, DateTime? ToDate, string search, string searchtype, int? InvoiceID, string Shipper, string Status, bool RMCBuss, char Type)
		{

			try
			{
				/*if (RELOCBS.Common.CommonService.RMCBuss == UserSession.GetUserSession().BussinessLine)
                {

                }*/


				string query = string.Format("exec [Inv].[GetInvForGrid]  @SP_LoginID={0},@SP_InvoiceID={1},@SP_InvStatus={2},@SP_Type={3},@SP_Shipper={4},@SP_IsRMCBuss={5},@SP_FilterName={6},@SP_FilterValue={7}",
				Convert.ToString(LoginID), CSubs.QSafeValue(Convert.ToString(InvoiceID)), CSubs.QSafeValue(Status), CSubs.QSafeValue(Type.ToString()), CSubs.QSafeValue(Shipper), RMCBuss
				, CSubs.QSafeValue(searchtype), CSubs.QSafeValue(search));

				DataTable dataTable = CSubs.GetDataTable(query);
				var result = new List<Entities.Billing>();
				if (dataTable != null)
				{
					result = (from rw in dataTable.AsEnumerable()
							  select new Entities.Billing()
							  {
								  //SurveyId = Convert.ToInt64(rw["SurveyID"]),
								  CreditNoteID = Convert.ToInt32(rw["CreditNoteID"]),
								  BillID = Convert.ToInt32(rw["InvID"]),
								  JobNo = Convert.ToString(rw["JobID"]),
								  InvoiceNo = Convert.ToString(rw["ActInvNumber"]),
								  CreditNoteNo = Convert.ToString(rw["ActCreditNumber"]),
								  Client = Convert.ToString(rw["ClientName"]),
								  InvoiceDate = Convert.ToDateTime(rw["Createddate"]),
								  Shipper = Convert.ToString(rw["ShipperName"]),
								  Mode = Convert.ToString(rw["ModeName"]),
								  //BillTo = Convert.ToString(rw["BillTo"]),
								  InvoiceStatus = Convert.ToString(rw["InvStatus"]),
								  IsShowCreditNote = Convert.ToBoolean(rw["IsShowCreditNote"]),
								  IsCreateCreditNote = Convert.ToBoolean(rw["IsCreateCreditNote"]),
								  IsShowDelete = Convert.ToBoolean(rw["ShowDelete"]),
								  StrgInvID = rw["StrgInvMasterID"] != DBNull.Value ? Convert.ToInt32(rw["StrgInvMasterID"]) : (int?)null,
								  StrgJobID = rw["StrgJobmasterID"] != DBNull.Value ? Convert.ToInt32(rw["StrgJobmasterID"]) : (int?)null
							  }).ToList();
				}


				IQueryable<Entities.Billing> data = result.AsQueryable<Entities.Billing>();

				return data;

			}
			catch (Exception ex)
			{
				throw new DataAccessException(Convert.ToString(LoginID), "BillingDAL", "GetForGrid", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
			}
		}

		public DataSet GetDetailById(int? MoveID, int? InvoiceID, int? CreditNoteID, char Type, int? RateCompID, string key = null, bool IsOthApp = false)
		{
			DataSet InvoiceDetailDt = new DataSet();

			try
			{
				bool RMCBuss = !UserSession.GetUserSession().BussinessLine.Equals("NON RMC-BUSINESS");
				if (IsOthApp)
				{
					string query = string.Format("EXEC [Inv].[GetInvoiceDetails_OtherApp] @SP_InvoiceID = {0}, @SP_CompanyID ={1}, @SP_IsRmcBuss = {2}, @SP_LoginID = {3}, @Out_Message = {4}",
						key, UserSession.GetUserSession().CompanyID, RMCBuss, UserSession.GetUserSession().LoginID, CSubs.QSafeValue(null));
					InvoiceDetailDt = CSubs.GetDataSet(query);
				}
				else
				{
					string query = string.Format("EXEC [Inv].[GetInvoiceDetails] @SP_InvoiceID={0}, @SP_MoveID={1}, @RateCompID={2},@SP_LoginID={3},@SP_InvOrCreditNote={4},@SP_CreditNoteID={5}",
				InvoiceID, MoveID, RateCompID, UserSession.GetUserSession().LoginID, CSubs.QSafeValue(Type.ToString()), CreditNoteID);
					InvoiceDetailDt = CSubs.GetDataSet(query);
				}


			}
			catch (Exception ex)
			{
				throw new DataAccessException(Convert.ToString(UserSession.GetUserSession().LoginID), "BillingDAL", "GetDetailById", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
			}

			return InvoiceDetailDt;

		}

		public DataSet GetDetailById(int? MoveID, int? StrgInvMasterID, int? InvoiceID, int? CreditNoteID, char Type, int? rateCompID)
		{
			try
			{
				string query = string.Format("EXEC [Strg].[GetStrgInvDetails_Main] @SP_InvoiceID = {0}, @SP_CreditNoteID ={1}, @SP_MoveID = {2}, @SP_LoginID = {3}, @SP_InvOrCreditNote = {4},@Sp_StrgInvMasterID={5},@RateCompID={6}",
						 InvoiceID, CreditNoteID, MoveID, UserSession.GetUserSession().LoginID, CSubs.QSafeValue(Type.ToString()), StrgInvMasterID, rateCompID);
				return CSubs.GetDataSet(query);
			}
			catch (Exception ex)
			{
				throw new DataAccessException(Convert.ToString(UserSession.GetUserSession().LoginID), "BillingDAL", "GetDetailById(StrgInvMasterID)", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
			}
		}

		public bool InsertBilling(Entities.Billing SaveRate, XElement Billitems, string status, int LoginID, bool SaveTopOnly, out string result)
		{
			result = String.Empty;

			try
			{
				string BillitemsString = Billitems.HasElements ? Convert.ToString(Billitems) : null;
				//System.Xml.Linq.XNode node = JsonConvert.DeserializeXNode(SaveRate.MoveJob.HFVMoveRateCompList, "CostHeadwiseDetails");
				//string QuotingHeadXml = node.ToString();
				string SPNAME = SaveRate.StrgInvID != null && SaveRate.StrgInvID > 0 ? "[Strg].[AddEditStrgInvoices_Main]" : "[Inv].[AddEditInvoice]";

				using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
				{
					if (conn.Connect())
					{
						conn.AddCommand(SPNAME, QueryType.Procedure);
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_MoveID", SqlDbType.BigInt, 0, ParameterDirection.Input, SaveRate.MoveID);
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_InvID", SqlDbType.BigInt, 0, ParameterDirection.InputOutput, SaveRate.BillID == null ? 0 : SaveRate.BillID);
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_InvDate", SqlDbType.DateTime, 0, ParameterDirection.Input, SaveRate.InvoiceDate == null && SaveRate.InvoiceStatus == "Approved" ? DateTime.Now : SaveRate.InvoiceDate);
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CurrID", SqlDbType.BigInt, 0, ParameterDirection.Input, SaveRate.RateCurrancyID);
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ConvrRate", SqlDbType.Float, 0, ParameterDirection.Input, SaveRate.ConvRate);
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_BillTo", SqlDbType.VarChar, 10, ParameterDirection.Input, SaveRate.BillToID);
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_BillToOrgOrDest", SqlDbType.VarChar, 1, ParameterDirection.Input, SaveRate.AddressType);
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LoginID", SqlDbType.BigInt, 0, ParameterDirection.Input, UserSession.GetUserSession().LoginID);
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_AttentionFName", SqlDbType.VarChar, 100, ParameterDirection.Input, SaveRate.Attention);
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_AttentionLName", SqlDbType.VarChar, 30, ParameterDirection.Input, null);
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Address1", SqlDbType.VarChar, 200, ParameterDirection.Input, SaveRate.Address1);
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Address2", SqlDbType.VarChar, 200, ParameterDirection.Input, SaveRate.Address2);
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_EmailID", SqlDbType.VarChar, 50, ParameterDirection.Input, SaveRate.Email);
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CityID", SqlDbType.Int, 0, ParameterDirection.Input, SaveRate.CityID);
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_PinCode", SqlDbType.VarChar, 10, ParameterDirection.Input, SaveRate.PinCode);
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Phone", SqlDbType.VarChar, 30, ParameterDirection.Input, SaveRate.Phone);

						//conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Phone2", SqlDbType.VarChar, 15, ParameterDirection.Input, SaveRate.MoveJob.Shipper.Phone2);
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_OrgStgStartDt", SqlDbType.DateTime, 0, ParameterDirection.Input, SaveRate.OrgStorageStart);
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_OrgStgEndDt", SqlDbType.DateTime, 0, ParameterDirection.Input, SaveRate.OrgStorageEnd);
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_DestStgStartDt", SqlDbType.DateTime, 0, ParameterDirection.Input, SaveRate.DestStorageStart);
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_DestStgEndDt", SqlDbType.DateTime, 0, ParameterDirection.Input, SaveRate.DestStorageEnd);
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_OrgWHStateID", SqlDbType.Int, 0, ParameterDirection.Input, SaveRate.OrgStorageState);
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_DestWHStateID", SqlDbType.Int, 0, ParameterDirection.Input, SaveRate.DestStorageState);
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_BillToClientID", SqlDbType.Int, 0, ParameterDirection.Input, SaveRate.BillToClientID);
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_BillToAccountID", SqlDbType.Int, 0, ParameterDirection.Input, SaveRate.BillToAccountID);
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ShipperName", SqlDbType.VarChar, 100, ParameterDirection.Input, SaveRate.BillToShipperName);
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Remarks", SqlDbType.VarChar, 250, ParameterDirection.Input, null);
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_SaveTopOnly", SqlDbType.Bit, 0, ParameterDirection.Input, SaveTopOnly);
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_InvStatus", SqlDbType.VarChar, 20, ParameterDirection.Input, status);
						conn.AddParameter(ParameterOF.PROCEDURE, "@RETURNSTATUS", SqlDbType.SmallInt, 0, ParameterDirection.ReturnValue);
						conn.AddParameter(ParameterOF.PROCEDURE, "@Out_Message", SqlDbType.NVarChar, 500, ParameterDirection.Output);
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_InvDetails", SqlDbType.Xml, 0, ParameterDirection.Input, BillitemsString);
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_RemarksOnStatus", SqlDbType.VarChar, 1000, ParameterDirection.Input, SaveRate.Remark);
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_TaxType", SqlDbType.VarChar, 10, ParameterDirection.Input, SaveRate.TaxType);
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_InvOrCreditNote", SqlDbType.VarChar, 10, ParameterDirection.Input, SaveRate.BillType);
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CreditNoteID", SqlDbType.BigInt, 0, ParameterDirection.InputOutput, SaveRate.CreditNoteID == null ? 0 : SaveRate.CreditNoteID);
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_AEDConversionRate", SqlDbType.Float, 0, ParameterDirection.Input, SaveRate.AEDCurr);
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IsGrossWt", SqlDbType.Bit, 0, ParameterDirection.Input, SaveRate.IsGrossWtValue);
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IsGrossVol", SqlDbType.Bit, 0, ParameterDirection.Input, SaveRate.IsGrossVolumeValue);
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IsNetWt", SqlDbType.Bit, 0, ParameterDirection.Input, SaveRate.IsWtValue);
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IsNetVol", SqlDbType.Bit, 0, ParameterDirection.Input, SaveRate.IsVolumeValue);
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IsGoodDesc", SqlDbType.Bit, 0, ParameterDirection.Input, SaveRate.IsGoodsDesc);
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_chequeNo", SqlDbType.VarChar, 50, ParameterDirection.Input, SaveRate.chequeNo);
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_BillRemarks", SqlDbType.VarChar, 100, ParameterDirection.Input, SaveRate.BillRemarks);
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_VehicleNo", SqlDbType.VarChar, 50, ParameterDirection.Input, SaveRate.VehicleNo);
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_AdvanceRecv", SqlDbType.Float, 0, ParameterDirection.Input, SaveRate.AdvanceRecv);
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_BillSubDate", SqlDbType.DateTime, 0, ParameterDirection.Input, SaveRate.BillSubDate);
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_BillAcknowledgement", SqlDbType.VarChar, 50, ParameterDirection.Input, SaveRate.BillAcknowledgement);
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_BillGSTNo", SqlDbType.VarChar, 20, ParameterDirection.Input, SaveRate.Other.GSTNo);
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ReferenceNo", SqlDbType.VarChar, 100, ParameterDirection.Input, SaveRate.FileNo);
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_NoofPkgs", SqlDbType.BigInt, 0, ParameterDirection.Input, SaveRate.NoofPkgs);
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_MoneyReceivedDate", SqlDbType.Date, 0, ParameterDirection.Input, SaveRate.MoneyReceivedDate);
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ParsifalAuditStartDate", SqlDbType.Date, 0, ParameterDirection.Input, SaveRate.ParsifalAuditStartDate);
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ParsifalApproveDate", SqlDbType.Date, 0, ParameterDirection.Input, SaveRate.ParsifalApproveDate);

						//////Storage Invoice Parameter
						if (SaveRate.StrgInvID != null)
						{
							conn.AddParameter(ParameterOF.PROCEDURE, "@SP_StrgJobMasterID", SqlDbType.BigInt, 0, ParameterDirection.Input, SaveRate.StrgJobID);
							conn.AddParameter(ParameterOF.PROCEDURE, "@SP_StrgInvMasterID", SqlDbType.BigInt, 0, ParameterDirection.Input, SaveRate.StrgInvID);
							conn.AddParameter(ParameterOF.PROCEDURE, "@SP_SubjectInfo", SqlDbType.VarChar, -1, ParameterDirection.Input, SaveRate.Subject);
							conn.AddParameter(ParameterOF.PROCEDURE, "@SP_NoteInfo", SqlDbType.VarChar, -1, ParameterDirection.Input, SaveRate.Note);
							conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ShowSubInInv", SqlDbType.Bit, 0, ParameterDirection.Input, SaveRate.IsSubject);
							conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ShowNoteInInv", SqlDbType.Bit, 0, ParameterDirection.Input, SaveRate.IsNote);
							conn.AddParameter(ParameterOF.PROCEDURE, "@SP_WtVolID", SqlDbType.Int, -1, ParameterDirection.Input, SaveRate.StrgVolUnitID);
							conn.AddParameter(ParameterOF.PROCEDURE, "@SP_WtVolValue", SqlDbType.Float, -1, ParameterDirection.Input, SaveRate.StrgVolValue);
						}

						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ServiceID", SqlDbType.Int, -1, ParameterDirection.Input, SaveRate.BtrService);
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_PaymentTerm", SqlDbType.Int, -1, ParameterDirection.Input, SaveRate.BtrPaymentTerm);
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IsCollectionDate", SqlDbType.Bit, -1, ParameterDirection.Input, SaveRate.IsCollectionDate);
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_BillingEntityID", SqlDbType.Int, -1, ParameterDirection.Input, SaveRate.BillingEntity);
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CreditNoteEntityID", SqlDbType.Int, -1, ParameterDirection.Input, SaveRate.CreditNoteEntity);
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_BillAddInfo", SqlDbType.VarChar, 50, ParameterDirection.Input, SaveRate.BillAddInfo);
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_InvTotalAmount", SqlDbType.Decimal, 0, ParameterDirection.Input, SaveRate.InvTotalAmount);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_BillCategoryID", SqlDbType.Int, 0, ParameterDirection.Input, SaveRate.BillCategoryID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_SunCost", SqlDbType.Decimal, 0, ParameterDirection.Input, SaveRate.SunCost);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IsAnnexure", SqlDbType.Bit, 0, ParameterDirection.Input, SaveRate.IsAnnexure);
                        conn.ExecuteProcedure(ProcedureReturnType.SingleValue);

						if (!conn.IsError)
						{
							int ReturnStatus = Convert.ToInt32(conn.GetParameterValue(ParameterOF.PROCEDURE, "@RETURNSTATUS"));
							result = Convert.ToString(conn.GetParameterValue(ParameterOF.PROCEDURE, "@Out_Message"));

							if (ReturnStatus == 0)
							{
								if (SaveRate.BillType == 'I')
								{
									SaveRate.BillID = Convert.ToInt32(conn.GetParameterValue(ParameterOF.PROCEDURE, "@SP_InvID"));
								}
								else if (SaveRate.BillType == 'C')
								{
									SaveRate.CreditNoteID = Convert.ToInt32(conn.GetParameterValue(ParameterOF.PROCEDURE, "@SP_CreditNoteID"));
								}

								return true;
							}
							else
							{
								return false;
							}
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
				throw new DataAccessException(Convert.ToString(LoginID), "BillingDAL", "InsertInvoice", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
			}


		}

		public bool InsertStatement(Entities.Billing SaveRate, int LoginID, out string result)
		{
			result = String.Empty;

			try
			{
				using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
				{
					if (conn.Connect())
					{
						conn.AddCommand("[Inv].[AddEditStatementOfCharges]", QueryType.Procedure);
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_InvID", SqlDbType.BigInt, 0, ParameterDirection.Input, SaveRate.BillID == null ? 0 : SaveRate.BillID);
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_StatementSub", SqlDbType.VarChar, 500, ParameterDirection.Input, SaveRate.StatementSub);
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Specification", SqlDbType.VarChar, 500, ParameterDirection.Input, SaveRate.Specification);
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LoginID", SqlDbType.BigInt, 0, ParameterDirection.Input, LoginID);
						conn.AddParameter(ParameterOF.PROCEDURE, "@RETURNSTATUS", SqlDbType.SmallInt, 0, ParameterDirection.ReturnValue);
						conn.AddParameter(ParameterOF.PROCEDURE, "@Out_Message", SqlDbType.NVarChar, 500, ParameterDirection.Output);

						conn.ExecuteProcedure(ProcedureReturnType.SingleValue);

						if (!conn.IsError)
						{
							int ReturnStatus = Convert.ToInt32(conn.GetParameterValue(ParameterOF.PROCEDURE, "@RETURNSTATUS"));
							result = Convert.ToString(conn.GetParameterValue(ParameterOF.PROCEDURE, "@Out_Message"));

							if (ReturnStatus == 0)
							{
								return true;
							}
							else
							{
								return false;
							}
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
				throw new DataAccessException(Convert.ToString(LoginID), "BillingDAL", "InsertStatement", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
			}


		}

		public bool CancelInvoice(int BillID, int LoginID, out string result)
		{
			result = String.Empty;

			try
			{
				using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
				{
					if (conn.Connect())
					{
						conn.AddCommand("[Inv].[CancelInvoice]", QueryType.Procedure);
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_InvID", SqlDbType.BigInt, 0, ParameterDirection.Input, BillID);
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LoginID", SqlDbType.BigInt, 0, ParameterDirection.Input, LoginID);
						conn.AddParameter(ParameterOF.PROCEDURE, "@RETURNSTATUS", SqlDbType.SmallInt, 0, ParameterDirection.ReturnValue);
						conn.AddParameter(ParameterOF.PROCEDURE, "@Out_Message", SqlDbType.NVarChar, 500, ParameterDirection.Output);

						conn.ExecuteProcedure(ProcedureReturnType.SingleValue);

						if (!conn.IsError)
						{
							int ReturnStatus = Convert.ToInt32(conn.GetParameterValue(ParameterOF.PROCEDURE, "@RETURNSTATUS"));
							result = Convert.ToString(conn.GetParameterValue(ParameterOF.PROCEDURE, "@Out_Message"));

							if (ReturnStatus == 0)
							{
								return true;
							}
							else
							{
								return false;
							}
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
				throw new DataAccessException(Convert.ToString(LoginID), "BillingDAL", "CancelInvoice", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
			}
		}

		public DataTable GetAddressDetials(int Client_AccountID, Int64 MoveID, string BillTo, char OrgorDest,int BillingEntityID,char BillType)
		{
			//@SP_Client_AccountID,@SP_MoveID,@SP_BillTo,@SP_OrgorDest
			DataTable AddressDt = new DataTable();

			try
			{

				string query = string.Format("EXEC [Inv].[GetAddressForInvoice] @SP_MoveID={0},@SP_BillTo={1},@SP_BillToOrgOrDest={2},@SP_BillingEntityID={3},@SP_BillType={4}",
				MoveID, BillTo, OrgorDest, CSubs.QSafeValue(Convert.ToString(BillingEntityID>0? BillingEntityID.ToString() : null)), CSubs.QSafeValue(Convert.ToString(BillType)));
				AddressDt = CSubs.GetDataTable(query);
			}
			catch (Exception ex)
			{
				throw new DataAccessException(Convert.ToString(UserSession.GetUserSession().LoginID), "BillingDAL", "GetAddressDetials", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
			}

			return AddressDt;
		}

		public DataSet GenerateEInvoice(string InvoiceNo)
		{
			DataSet InvoiceDetailDt = new DataSet();

			try
			{
				string query = string.Format("EXEC [E_Invoice].[EInvoiceSchema] @SP_CompID={0}, @SP_LoginID={1}, @SP_InvoiceNumber={2},@SP_AppName={3}",
				UserSession.GetUserSession().CompanyID, UserSession.GetUserSession().LoginID, CSubs.QSafeValue(InvoiceNo.ToString()), "NewCBS");
				InvoiceDetailDt = CSubs.GetDataSet(query);
			}
			catch (Exception ex)
			{
				throw new DataAccessException(Convert.ToString(UserSession.GetUserSession().LoginID), "BillingDAL", "GetDetailById", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
			}

			return InvoiceDetailDt;
		}

		public bool InsertAPIResponse(int LoginID, string Jsonstr, DataTable dt, DataTable ErrorTable, string AppName, out string result)
		{
			result = String.Empty;
			DataTable Err = new DataTable("ErrorDetails");

			Err.Columns.Add("Inv_no", typeof(string));
			Err.Columns.Add("error_code", typeof(string));
			Err.Columns.Add("error_message", typeof(string));
			Err.Columns.Add("error_Source", typeof(string));

			foreach (DataRow item in ErrorTable.Rows)
			{
				DataRow dr = Err.NewRow();
				dr["Inv_no"] = Convert.ToString(item["Inv_no"]);
				dr["error_code"] = Convert.ToString(item["error_code"]);
				dr["error_message"] = Convert.ToString(item["error_message"]);
				dr["error_Source"] = Convert.ToString(item["error_Source"]);
				Err.Rows.Add(dr);
			}
			try
			{
				using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
				{
					if (conn.Connect())
					{
						conn.AddCommand("[E_Invoice].[AddEditReponseFromAPI]", QueryType.Procedure);
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LoginID", SqlDbType.BigInt, 0, ParameterDirection.Input, LoginID);
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_document_status", SqlDbType.VarChar, 100, ParameterDirection.Input, dt.Rows[0]["document_status"]);
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Success", SqlDbType.VarChar, 5, ParameterDirection.Input, dt.Rows[0]["Success"]);
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Inv_No", SqlDbType.VarChar, 20, ParameterDirection.Input, dt.Rows[0]["Inv_No"]);
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_transaction_id", SqlDbType.VarChar, 50, ParameterDirection.Input, dt.Rows[0]["transaction_id"]);
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_AckNo", SqlDbType.VarChar, 50, ParameterDirection.Input, dt.Rows[0]["AckNo"]);
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_AckDt", SqlDbType.VarChar, 50, ParameterDirection.Input, dt.Rows[0]["AckDt"]);
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Irn", SqlDbType.NVarChar, 250, ParameterDirection.Input, dt.Rows[0]["Irn"]);
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_SignedInvoice", SqlDbType.NVarChar, -1, ParameterDirection.Input, dt.Rows[0]["SignedInvoice"]);
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_SignedQRCode", SqlDbType.NVarChar, -1, ParameterDirection.Input, dt.Rows[0]["SignedQRCode"]);
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Status", SqlDbType.VarChar, 100, ParameterDirection.Input, dt.Rows[0]["Status"]);
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_AppName", SqlDbType.VarChar, 20, ParameterDirection.Input, AppName);
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_EntireJson", SqlDbType.NVarChar, -1, ParameterDirection.Input, Jsonstr);
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ErrorTable", SqlDbType.Structured, 0, ParameterDirection.Input, Err);
						conn.AddParameter(ParameterOF.PROCEDURE, "@RETURNSTATUS", SqlDbType.SmallInt, 0, ParameterDirection.ReturnValue);
						conn.AddParameter(ParameterOF.PROCEDURE, "@Out_Message", SqlDbType.NVarChar, 500, ParameterDirection.Output);

						conn.ExecuteProcedure(ProcedureReturnType.SingleValue);

						if (!conn.IsError)
						{
							int ReturnStatus = Convert.ToInt32(conn.GetParameterValue(ParameterOF.PROCEDURE, "@RETURNSTATUS"));
							result = Convert.ToString(conn.GetParameterValue(ParameterOF.PROCEDURE, "@Out_Message"));

							if (ReturnStatus == 0)
							{
								return true;
							}
							else
							{
								return false;
							}
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
				throw new DataAccessException(Convert.ToString(LoginID), "BillingDAL", "CancelInvoice", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
			}

		}

		public DataTable GetAPIErrorDetails(string InvoiceNo)
		{
			DataTable InvoiceDetailDt = new DataTable();

			try
			{
				string query = string.Format("EXEC [E_Invoice].[GetErrorDetails] @SP_LoginID={0},@SP_InvoiceNumber={1}",
				UserSession.GetUserSession().LoginID, CSubs.QSafeValue(InvoiceNo.ToString()));
				InvoiceDetailDt = CSubs.GetDataTable(query);
			}
			catch (Exception ex)
			{
				throw new DataAccessException(Convert.ToString(UserSession.GetUserSession().LoginID), "BillingDAL", "GetAPIErrorDetails", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
			}

			return InvoiceDetailDt;
		}

		public DataTable GetIRNDetails(string InvoiceNo, bool IsQrCodeReq = false)
		{
			DataTable InvoiceDetailDt = new DataTable();

			try
			{
				string query = string.Format("EXEC [E_Invoice].[GetIRNandQRCodeInfo] @SP_LoginID={0},@SP_InvoiceNumber={1}",
				UserSession.GetUserSession().LoginID, CSubs.QSafeValue(InvoiceNo.ToString()));
				InvoiceDetailDt = CSubs.GetDataTable(query);
				if (!IsQrCodeReq)
				{
					InvoiceDetailDt.Columns.Remove("SignedQRCode");
				}

			}
			catch (Exception ex)
			{
				throw new DataAccessException(Convert.ToString(UserSession.GetUserSession().LoginID), "BillingDAL", "GetIRNDetails", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
			}

			return InvoiceDetailDt;
		}
	}
}