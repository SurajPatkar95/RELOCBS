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

namespace RELOCBS.DAL.StorageBilling
{
	public class StorageBillingDAL
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

		public IQueryable<Entities.StorageBillGrid> GetBillGrid(DateTime? FromDate, DateTime? Todate, string BillType, string Shipper, Int64 MoveId, bool RMCBuss, int CompanyID)
		{
			int LoggedinUserID = UserSession.GetUserSession().LoginID;
			IQueryable<Entities.StorageBillGrid> List = new List<Entities.StorageBillGrid>().AsQueryable();

			try
			{

				using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
				{
					if (conn.Connect())
					{
						conn.AddCommand("[Strg].[GetJobListForBill_Grid]", QueryType.Procedure);
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Type", SqlDbType.VarChar, 10, ParameterDirection.Input, BillType);
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_BillDateFrom", SqlDbType.DateTime, 0, ParameterDirection.Input, FromDate);
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_BillDateTo", SqlDbType.DateTime, 0, ParameterDirection.Input, Todate);
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Shipper", SqlDbType.VarChar, 100, ParameterDirection.Input, CSubs.PSafeValue(Shipper));
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_MoveID", SqlDbType.Int, 0, ParameterDirection.Input, MoveId);
						//conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CompID", SqlDbType.Int, 0, ParameterDirection.Input, CompanyID);
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IsRMCBuss", SqlDbType.Bit, 1, ParameterDirection.Input, RMCBuss);
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LoginID", SqlDbType.Int, 0, ParameterDirection.Input, UserSession.GetUserSession().LoginID);
						DataTable dt = (DataTable)conn.ExecuteProcedure(ProcedureReturnType.DataTable);

						if (dt != null)
						{
							var result = (from item in dt.AsEnumerable()
										  select new Entities.StorageBillGrid()
										  {
											  MoveID = Convert.ToInt64(item["MoveID"]),
											  LastBillDate = !string.IsNullOrWhiteSpace(Convert.ToString(item["LastBillDate"])) ? Convert.ToDateTime(item["LastBillDate"]) : (DateTime?)null,
											  JobNo = Convert.ToString(item["JobID"]),
											  StorageID = Convert.ToInt64(item["StrgJobMasterID"]),
											  //JobDate = Convert.ToDateTime(item["JobOpenedDate"]),
											  ServiceLine = Convert.ToString(item["ServiceLine"]),
											  Shipper = Convert.ToString(item["ShipperName"]),
											  Client = Convert.ToString(item["ClientName"]),
											  Corporate = Convert.ToString(item["CorporateName"]),
											  Warehouse = Convert.ToString(item["Warehoue_Name"]),
										  }).AsQueryable();
							List = result;
						}
					}
					else
						throw new Exception(conn.ErrorMessage);
				}


				return List;
			}
			catch (Exception ex)
			{

				throw new DataAccessException(Convert.ToString(LoggedinUserID), "StorageBillingDAL", "GetBillGrid", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
			}


		}

		public DataSet GetBillDetails(int LoginID, Int64 MoveID, Int64 StorageID, Int64? InvoiceID = -1, Int64? CreditNoteID = -1, string Type = "I")
		{
			DataSet Ds = new DataSet();

			try
			{
				string query = string.Format("[Strg].[GetStrgInvDetails] @SP_MoveID={0},@SP_StrgJobMasterID={1},@SP_LoginID={2},@SP_StrgInvMasterID={3}",
			   CSubs.QSafeValue(Convert.ToString(MoveID)),
			   CSubs.QSafeValue(Convert.ToString(StorageID)),
			   CSubs.QSafeValue(Convert.ToString(LoginID)),
			   CSubs.QSafeValue(Convert.ToString(InvoiceID))
			   );

				Ds = CSubs.GetDataSet(query);

			}
			catch (Exception ex)
			{

				throw new DataAccessException(Convert.ToString(LoginID), "StorageBillingDAL", "GetBillDetails", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
			}

			return Ds;

		}

		public bool GetStrgBillProcess(int LoginID, StorageBill bill, int ProcessRowIndex, out string result)
		{

			result = string.Empty;
			try
			{
				using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
				{
					if (conn.Connect())
					{
						conn.AddCommand("[Strg].[ProcessStrgInvoices]", QueryType.Procedure);
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_StrgJobMasterID", SqlDbType.BigInt, 0, ParameterDirection.Input, bill.StorageID);
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_StrgInvMasterID", SqlDbType.BigInt, 0, ParameterDirection.InputOutput, bill.BillID);

						if (ProcessRowIndex > 0)
						{
							StorageBillDetails Detail = bill.DetailList[ProcessRowIndex - 1];

							string ProcessFor = string.Empty;
							if (Detail.CostHeadID == 1)
							{
								ProcessFor = "STRG";
							}
							else if (Detail.CostHeadID == 2)
							{
								ProcessFor = "INSR";
							}
							else if (Detail.CostHeadID == 3)
							{
								ProcessFor = "PEST";
							}

							conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IsSingleCostProces", SqlDbType.Bit, 0, ParameterDirection.Input, true);
							conn.AddParameter(ParameterOF.PROCEDURE, "@SP_SingleProcessFor", SqlDbType.VarChar, 50, ParameterDirection.Input, CSubs.PSafeValue(ProcessFor));
							conn.AddParameter(ParameterOF.PROCEDURE, "@SP_SingleBillDateTo", SqlDbType.DateTime, 0, ParameterDirection.Input, Detail.BillToDate);
							conn.AddParameter(ParameterOF.PROCEDURE, "@SP_BillDateTo", SqlDbType.DateTime, 0, ParameterDirection.Input, Detail.BillToDate);
						}
						else
						{
							conn.AddParameter(ParameterOF.PROCEDURE, "@SP_BillDateTo", SqlDbType.DateTime, 0, ParameterDirection.Input, bill.BillToDate);
						}
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LoginID", SqlDbType.Int, 0, ParameterDirection.Input, LoginID);
						conn.AddParameter(ParameterOF.PROCEDURE, "@RETURNSTATUS", SqlDbType.SmallInt, 0, ParameterDirection.ReturnValue);
						conn.AddParameter(ParameterOF.PROCEDURE, "@Out_Message", SqlDbType.NVarChar, -1, ParameterDirection.Output);
						conn.ExecuteProcedure(ProcedureReturnType.SingleValue);

						if (!conn.IsError)
						{

							int ReturnStatus = Convert.ToInt32(conn.GetParameterValue(ParameterOF.PROCEDURE, "@RETURNSTATUS"));
							result = Convert.ToString(conn.GetParameterValue(ParameterOF.PROCEDURE, "@Out_Message"));

							if (ReturnStatus == 0)
							{
								bill.BillID = Convert.ToInt64(conn.GetParameterValue(ParameterOF.PROCEDURE, "@SP_StrgInvMasterID"));
								//model.StorageDetailID = Convert.ToInt64(conn.GetParameterValue(ParameterOF.PROCEDURE, "@SP_InsPremiunForJobID"));

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

				throw new DataAccessException(Convert.ToString(LoginID), "StorageBillingDAL", "GetStrgBillProcess", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
			}

		}

		public bool Insert(StorageBill model, int LoginID, out string result)
		{
			result = string.Empty;

			try
			{
				using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
				{
					if (conn.Connect())
					{

						string InvDetailsXml = string.Empty;

						if (model.DetailList != null && model.DetailList.Count > 0)
						{
							model.DetailList = model.DetailList.Where(m => m.IsActive == true).ToList();

							InvDetailsXml = Convert.ToString(new XElement("InvoiceDetails", from emp in model.DetailList
																							select new XElement("InvoiceDetail",
																				 new XElement("CostHeadID", emp.CostHeadID),
																				 new XElement("InvFromDate", emp.BillStartDate != null ? Convert.ToDateTime(emp.BillStartDate).ToString("dd-MMM-yyyy HH:mm") : ""),
																				 new XElement("InvToDate", emp.BillToDate != null ? Convert.ToDateTime(emp.BillToDate).ToString("dd-MMM-yyyy HH:mm") : ""),
                                                                                 new XElement("Cost", String.Format("{0:0.####}", emp.CostAmt)),
                                                                                 new XElement("Amount", String.Format("{0:0.####}", emp.Amount)),
																				 new XElement("AfterAuditAmt", String.Format("{0:0.####}", emp.ActualAmount)),
																				 new XElement("TaxPer", String.Format("{0:0.####}", emp.Tax_Percent)),
																				 new XElement("SGSTAmt", String.Format("{0:0.####}", emp.SGST)),
																				 new XElement("CGSTAmt", String.Format("{0:0.####}", emp.CGST)),
																				 new XElement("IGSTAmt", String.Format("{0:0.####}", emp.IGST)),
																				 new XElement("VatAmt", String.Format("{0:0.####}", emp.VAT)),
																				 new XElement("ReferalAmt", String.Format("{0:0.####}", emp.CommissionAmt)),
																				 new XElement("AuditFee", String.Format("{0:0.####}", emp.AuditAmt)),
																				 new XElement("AdminFee", String.Format("{0:0.####}", emp.AdminAmt)),
                                                                                 new XElement("CostAmt", String.Format("{0:0.####}", emp.CostAmt))

                                                                             )));
						}

						conn.AddCommand("[Strg].[AddEditStrgInvoices]", QueryType.Procedure);
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_StrgJobMasterID", SqlDbType.BigInt, 0, ParameterDirection.Input, model.StorageID);
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_StrgInvMasterID", SqlDbType.BigInt, 0, ParameterDirection.InputOutput, model.BillID);
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_BillDateTo", SqlDbType.DateTime, 0, ParameterDirection.Input, model.BillToDate);
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_InvDetails", SqlDbType.Xml, -1, ParameterDirection.Input, InvDetailsXml);

						//conn.AddParameter(ParameterOF.PROCEDURE, "@SP_InsCompID", SqlDbType.BigInt, 0, ParameterDirection.InputOutput, model.StorageDetailID);
						//conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CommodityID", SqlDbType.Int, 0, ParameterDirection.Input, model.StatusID);
						//conn.AddParameter(ParameterOF.PROCEDURE, "@SP_StrgJobMasterID", SqlDbType.BigInt, 0, ParameterDirection.InputOutput, model.StorageID);
						//conn.AddParameter(ParameterOF.PROCEDURE, "@SP_PackDate", SqlDbType.DateTime, 0, ParameterDirection.Input, model.PackDate);
						//conn.AddParameter(ParameterOF.PROCEDURE, "@SP_PackStateID", SqlDbType.Int, 0, ParameterDirection.Input, model.PackStatusID);
						//conn.AddParameter(ParameterOF.PROCEDURE, "@SP_StorageDetails", SqlDbType.VarChar, 500, ParameterDirection.Input, CSubs.PSafeValue(model.StorageDetails));
						//conn.AddParameter(ParameterOF.PROCEDURE, "@SP_WarehouseID", SqlDbType.Int, 50, ParameterDirection.Input, model.WarehouseID);
						////conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CurrID", SqlDbType.Int, 0, ParameterDirection.Input, model.CurrID);
						//conn.AddParameter(ParameterOF.PROCEDURE, "@SP_SDFromBr", SqlDbType.Int, 0, ParameterDirection.Input, model.SD_BranchID);
						//conn.AddParameter(ParameterOF.PROCEDURE, "@SP_SDFromHO", SqlDbType.Int, 0, ParameterDirection.Input, model.SD_HOID);

						//conn.AddParameter(ParameterOF.PROCEDURE, "@SP_BillStartDate", SqlDbType.DateTime, 0, ParameterDirection.Input, model.BillStartDate);
						//conn.AddParameter(ParameterOF.PROCEDURE, "@SP_FileCloseDate", SqlDbType.DateTime, 0, ParameterDirection.Input, model.FileCloseDate);

						//conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IsInsured", SqlDbType.Bit, 0, ParameterDirection.Input, model.IsInsured);
						//conn.AddParameter(ParameterOF.PROCEDURE, "@SP_InsuredBy", SqlDbType.Int, 0, ParameterDirection.Input, model.InsuredByID);

						//////conn.AddParameter(ParameterOF.PROCEDURE, "@SP_StorageDetailID", SqlDbType.BigInt, 0, ParameterDirection.Input, model.StorageDetailID);

						//conn.AddParameter(ParameterOF.PROCEDURE, "@SP_WtUnitID", SqlDbType.Int, 0, ParameterDirection.Input, model.VolumeUnitID);
						//conn.AddParameter(ParameterOF.PROCEDURE, "@SP_VolAsOnTheDate", SqlDbType.Float, 0, ParameterDirection.Input, model.VolumeCFT);
						//conn.AddParameter(ParameterOF.PROCEDURE, "@SP_DateOfVolEntry", SqlDbType.DateTime, 0, ParameterDirection.Input, model.VolumeDate);
						//conn.AddParameter(ParameterOF.PROCEDURE, "@SP_VolRemarks", SqlDbType.VarChar, 500, ParameterDirection.Input, CSubs.PSafeValue(model.VolumeRemark));

						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LoginID", SqlDbType.Int, 0, ParameterDirection.Input, UserSession.GetUserSession().LoginID);
						conn.AddParameter(ParameterOF.PROCEDURE, "@RETURNSTATUS", SqlDbType.SmallInt, 0, ParameterDirection.ReturnValue);
						conn.AddParameter(ParameterOF.PROCEDURE, "@Out_Message", SqlDbType.NVarChar, -1, ParameterDirection.Output);
						conn.ExecuteProcedure(ProcedureReturnType.SingleValue);

						if (!conn.IsError)
						{
							int ReturnStatus = Convert.ToInt32(conn.GetParameterValue(ParameterOF.PROCEDURE, "@RETURNSTATUS"));
							result = Convert.ToString(conn.GetParameterValue(ParameterOF.PROCEDURE, "@Out_Message"));

							if (ReturnStatus == 0)
							{
								model.BillID = Convert.ToInt64(conn.GetParameterValue(ParameterOF.PROCEDURE, "@SP_StrgInvMasterID"));
								//model.StorageDetailID = Convert.ToInt64(conn.GetParameterValue(ParameterOF.PROCEDURE, "@SP_InsPremiunForJobID"));

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

				throw new DataAccessException(Convert.ToString(LoginID), "StorageBillingDAL", "Insert", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
			}

			return false;
		}

		public DataTable GetStrgSubBillGrid(int LoginID, Int64 MoveID, Int64 StorageID, bool IsRmcBuss)
		{
			DataTable dt = new DataTable();

			try
			{
				string query = string.Format("[Strg].[GetStrgInvListForGrid] @SP_StrgJobMasterID={0},@SP_LoginID={1}",
				CSubs.QSafeValue(Convert.ToString(StorageID)),
				CSubs.QSafeValue(Convert.ToString(LoginID))
				//CSubs.QSafeValue(Convert.ToString(IsRmcBuss))
				);

				dt = CSubs.GetDataTable(query);
			}
			catch (Exception ex)
			{

				throw new DataAccessException(Convert.ToString(LoginID), "StorageBillingDAL", "GetStrgSubBillGrid", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
			}

			return dt;

		}

		public DataTable GetAdditionalCostTaxStatus(int LoginID, int CostHeadId)
		{
			try
			{

				string query = string.Format("[Strg].[GetCostTaxInfo] @SP_CostHeadID={0},@SP_LOGINID={1}",
			   CSubs.QSafeValue(Convert.ToString(CostHeadId)),
			   CSubs.QSafeValue(Convert.ToString(LoginID)));

				return CSubs.GetDataTable(query);

			}
			catch (Exception ex)
			{

				throw new DataAccessException(Convert.ToString(LoginID), "StorageBillingDAL", "GetAdditionalCostTaxStatus", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
			}

		}

		public bool Delete(int id, int LoginID, out string result)
		{
			result = string.Empty;

			try
			{
				using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
				{
					if (conn.Connect())
					{

						string InvDetailsXml = string.Empty;
						conn.AddCommand("[Strg].[StrgDeleteInvoice]", QueryType.Procedure);
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_StrgInvMasterID", SqlDbType.Int, 0, ParameterDirection.Input, id);
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LoginID", SqlDbType.Int, 0, ParameterDirection.Input, UserSession.GetUserSession().LoginID);
						conn.AddParameter(ParameterOF.PROCEDURE, "@RETURNSTATUS", SqlDbType.SmallInt, 0, ParameterDirection.ReturnValue);
						conn.AddParameter(ParameterOF.PROCEDURE, "@Out_Message", SqlDbType.NVarChar, -1, ParameterDirection.Output);
						conn.ExecuteProcedure(ProcedureReturnType.SingleValue);

						if (!conn.IsError)
						{
							int ReturnStatus = Convert.ToInt32(conn.GetParameterValue(ParameterOF.PROCEDURE, "@RETURNSTATUS"));
							result = Convert.ToString(conn.GetParameterValue(ParameterOF.PROCEDURE, "@Out_Message"));
							return ReturnStatus == 0 ? true : false;
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

				throw new DataAccessException(Convert.ToString(LoginID), "StorageBillingDAL", "Delete", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
			}
		}
	}
}