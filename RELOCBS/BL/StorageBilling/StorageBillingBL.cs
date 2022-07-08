using RELOCBS.DAL.StorageBilling;
using RELOCBS.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Linq.Dynamic;
using RELOCBS.Common.ExceptionHandling;
using RELOCBS.Entities;
using System.Data;

namespace RELOCBS.BL.StorageBilling
{
	public class StorageBillingBL
	{
		private StorageBillingDAL _storageBillDAL;

		public StorageBillingDAL storageBillDAL
		{

			get
			{
				if (this._storageBillDAL == null)
					this._storageBillDAL = new StorageBillingDAL();
				return this._storageBillDAL;
			}
		}


		public IEnumerable<Entities.StorageBillGrid> GetBillGrid(DateTime? FromDate, DateTime? Todate, string BillType, string Shipper, Int64 MoveId, string sort, string sortdir, int skip, int pageSize, out int totalCount)
		{
			totalCount = 0;
			try
			{
				//GetBillGrid(DateTime ? FromDate, DateTime ? Todate, string BillType, string Shipper, Int64 MoveId, bool RMCBuss, int CompanyID)

				bool RMCBuss = false;
				if (UserSession.GetUserSession().BussinessLine == "NON RMC-BUSINESS")
				{
					RMCBuss = false;
				}
				else
				{
					RMCBuss = true;
				}
				IQueryable<Entities.StorageBillGrid> storage = storageBillDAL.GetBillGrid(FromDate, Todate, BillType, Shipper, MoveId, RMCBuss, UserSession.GetUserSession().CompanyID);
				if (storage != null)
				{
					totalCount = storage.Count();

					if (pageSize > 1)
					{
						storage = storage.Skip((skip * (pageSize - 1))).Take(skip);
					}
					else
					{
						storage = storage.Take(skip);
					}

					storage = storage.OrderBy(sort + " " + sortdir);

					return storage.ToList();
				}
				else
				{
					return new List<Entities.StorageBillGrid>();
				}
			}
			catch (DataAccessException ex)
			{
				throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
			}
			catch (Exception ex)
			{
				throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "StorageBillBL", "GetBillGrid", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
			}
		}

		public bool Insert(StorageBill model, out string result)
		{
			try
			{
				return storageBillDAL.Insert(model, UserSession.GetUserSession().LoginID, out result);
			}
			catch (DataAccessException ex)
			{
				throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
			}
			catch (Exception ex)
			{
				throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "StorageBillDAL", "Inset", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
			}

		}

		public StorageBill GetBillDetails(Int64 MoveID, Int64 StorageID, Int64? InvoiceID, Int64 CreditNoteID, string Type)
		{
			StorageBill model = new StorageBill();

			try
			{

				DataSet data = storageBillDAL.GetBillDetails(UserSession.GetUserSession().LoginID, MoveID, StorageID, InvoiceID, CreditNoteID, Type);

				if (data != null && data.Tables.Count > 0)
				{
					if (data.Tables.Count > 0 && data.Tables[0] != null && data.Tables[0].Rows.Count > 0)
					{
						model = (from item in data.Tables[0].AsEnumerable()
								 select new StorageBill()
								 {
									 MoveID = Convert.ToInt64(item["MoveID"]),
									 Controller = Convert.ToString(item["Controller"]),
									 ShipperName = Convert.ToString(item["ShipperName"]),
									 Client = Convert.ToString(item["ClientName"]),
									 Corporate = Convert.ToString(item["Corporate"]),
									 JobNo = Convert.ToString(item["JobID"]),
									 //JobDate = Convert.ToDateTime(item["JobNo"]),
									 ServiceLine = Convert.ToString(item["ServiceLine"]),
									 ShipperAddress = Convert.ToString(item["ShipperAddress"]),
									 QuotationID = Convert.ToString(item["QuotationID"]),
									 JobCommodity = Convert.ToString(item["Commodity"]),
									 BillID = !string.IsNullOrEmpty(Convert.ToString(item["StrgInvMasterID"])) ? Convert.ToInt64(item["StrgInvMasterID"]) : (Int64?)null,
									 MainInvID = !string.IsNullOrEmpty(Convert.ToString(item["MainInvID"])) ? Convert.ToInt64(item["MainInvID"]) : (Int64?)null,
									 BaseCurrName = Convert.ToString(item["BaseCurrName"]),
									 MainInvStatus = Convert.ToString(item["MainInvStatus"]),
								 }).FirstOrDefault();


						if (data.Tables.Count > 0 && data.Tables[1] != null && data.Tables[1].Rows.Count > 0)
						{
							model.DetailList = (from item in data.Tables[1].AsEnumerable()
												select new StorageBillDetails()
												{
													BillId = Convert.ToInt64(item["StrgInvMasterID"]),
													//StrgVolID = Convert.ToInt64(item["StrgVolDetailID"]),
													VolumeWt = Convert.ToDecimal(item["WtVol"]),
													VolumeUnit = Convert.ToString(item["WeightUnitName"]),
													BillDetailId = Convert.ToInt64(item["StrgInvDetailID"]),
													BillStartDate = !string.IsNullOrWhiteSpace(Convert.ToString(item["InvFromDate"])) ? Convert.ToDateTime(item["InvFromDate"]) : (DateTime?)null,
													BillToDate = !string.IsNullOrWhiteSpace(Convert.ToString(item["InvToDate"])) ? Convert.ToDateTime(item["InvToDate"]) : (DateTime?)null,
													OldBillToDate = !string.IsNullOrWhiteSpace(Convert.ToString(item["InvToDate"])) ? Convert.ToDateTime(item["InvToDate"]) : (DateTime?)null,
													CostHeadID = Convert.ToInt32(item["CostHeadID"]),
													CostHead = Convert.ToString(item["StrgCostHeadName"]),
													Amount = !string.IsNullOrEmpty(Convert.ToString(item["InvValue"])) ? Convert.ToDecimal(item["InvValue"]) : 0,
													ActualAmount = !string.IsNullOrEmpty(Convert.ToString(item["BillableInvValue"])) ? Convert.ToDecimal(item["BillableInvValue"]) : 0,
													Tax_Percent = !string.IsNullOrEmpty(Convert.ToString(item["GSTPercent"])) ? Convert.ToDecimal(item["GSTPercent"]) : 0,
													SGST = !string.IsNullOrEmpty(Convert.ToString(item["SGSTVal"])) ? Convert.ToDecimal(item["SGSTVal"]) : 0,
													CGST = !string.IsNullOrEmpty(Convert.ToString(item["CGSTVal"])) ? Convert.ToDecimal(item["CGSTVal"]) : 0,
													IGST = !string.IsNullOrEmpty(Convert.ToString(item["IGSTVal"])) ? Convert.ToDecimal(item["IGSTVal"]) : 0,
													VAT = !string.IsNullOrEmpty(Convert.ToString(item["VAT"])) ? Convert.ToDecimal(item["VAT"]) : 0,
													CommissionAmt = !string.IsNullOrEmpty(Convert.ToString(item["ReferalAmt"])) ? Convert.ToDecimal(item["ReferalAmt"]) : 0,
													AuditAmt = !string.IsNullOrEmpty(Convert.ToString(item["Auditfee"])) ? Convert.ToDecimal(item["Auditfee"]) : 0,
													AdminAmt = !string.IsNullOrEmpty(Convert.ToString(item["Adminfee"])) ? Convert.ToDecimal(item["Adminfee"]) : 0,
													

													//BillFrqID = Convert.ToInt32(item["BiilingFrqID"]),
													//BillFrq = Convert.ToString(item["FreqName"]),
													TaxType = Convert.ToString(item["TaxType"]),
													ShowDelete = Convert.ToBoolean(item["ShowDelete"]),
													IsActive= true,
                                                    CostAmt = !string.IsNullOrEmpty(Convert.ToString(item["CostAmt"])) ? Convert.ToDecimal(item["CostAmt"]) : 0,
                                                }).ToList();

							model.TaxType = model.DetailList.Count > 0 ? model.DetailList.First().TaxType : "";
						}

					}

				}
				else
				{
					model.MoveID = MoveID;
					model.StorageID = StorageID;
				}
			}
			catch (DataAccessException ex)
			{
				throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
			}
			catch (Exception ex)
			{
				throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "StorageBillBL", "GetBillDetails", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
			}

			return model;
		}

		public bool GetStrgBillProcess(StorageBill bill, int ProcessRowIndex, out string result)
		{
			try
			{
				return storageBillDAL.GetStrgBillProcess(UserSession.GetUserSession().LoginID, bill, ProcessRowIndex, out result);
			}
			catch (DataAccessException ex)
			{
				throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
			}
			catch (Exception ex)
			{
				throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "StorageBillBL", "GetStrgBillProcess", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
			}

		}

		public IEnumerable<StorageSubBillGrid> GetStrgSubBillGrid(Int64 MoveID, Int64 StorageID)
		{
			IEnumerable<StorageSubBillGrid> list = new List<StorageSubBillGrid>();

			try
			{
				bool IsRmcBuss = !(UserSession.GetUserSession().BussinessLine == "NON RMC-BUSINESS");

				DataTable data = storageBillDAL.GetStrgSubBillGrid(UserSession.GetUserSession().LoginID, MoveID, StorageID, IsRmcBuss);

				if (data != null && data.Rows.Count > 0)
				{
					list = (from item in data.AsEnumerable()
							select new StorageSubBillGrid()
							{
								MoveID = item["MoveID"] != DBNull.Value ? Convert.ToInt64(item["MoveID"]) : 0,
								StorageID = item["StrgJobMasterID"] != DBNull.Value ? Convert.ToInt64(item["StrgJobMasterID"]) : 0,
								//Controller = Convert.ToString(item["ControllerName"]),
								BillID = item["MainInvID"] != DBNull.Value ? Convert.ToInt64(item["MainInvID"]) : 0,
								InvNo = Convert.ToString(item["MainInvNo"]),
								ProcessID = item["StrgInvMasterID"] != DBNull.Value ? Convert.ToInt64(item["StrgInvMasterID"]) : 0,
								InvFromDate = Convert.ToDateTime(item["InvFromDate"]),
								InvToDate = !string.IsNullOrWhiteSpace(Convert.ToString(item["InvToDate"])) ? Convert.ToDateTime(item["InvToDate"]) : (DateTime?)null,
								Status = Convert.ToString(item["InvStatus"]),
								//JobDate = Convert.ToDateTime(item["JobNo"]),
								ApprovedBy = Convert.ToString(item["ApprovedBy"]),
								FinalizedBy = Convert.ToString(item["FinalizedBy"]),
								MainInvNo = Convert.ToString(item["MainInvNo"]),
								MainInvStatus = Convert.ToString(item["MainInvStatus"]),
								IsShowDlt = Convert.ToBoolean(item["ShowDelete"]),
							}).ToList();
				}
			}
			catch (DataAccessException ex)
			{
				throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
			}
			catch (Exception ex)
			{
				throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "StorageBillingBL", "GetStrgSubBillGrid", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
			}

			return list;

		}

		public bool GetAdditionalCostTaxStatus(int CostHeadId)
		{
			bool IsTaxApplicable = false;

			try
			{
				DataTable data = storageBillDAL.GetAdditionalCostTaxStatus(UserSession.GetUserSession().LoginID, CostHeadId);

				if (data != null && data.Rows.Count > 0)
				{
					IsTaxApplicable = Convert.ToString(data.Rows[0]["Istaxable"]) == "1" ? true : IsTaxApplicable;
				}

				return IsTaxApplicable;

			}
			catch (DataAccessException ex)
			{
				throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
			}
			catch (Exception ex)
			{
				throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "StorageBillingBL", "GetAdditionalCostTaxStatus", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
			}

		}

		public bool Delete(int id, out string result)
		{
			try
			{
				return storageBillDAL.Delete(id, UserSession.GetUserSession().LoginID, out result);
			}
			catch (DataAccessException ex)
			{
				throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
			}
			catch (Exception ex)
			{
				throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "StorageBillDAL", "Delete", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
			}
		}
	}
}