using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using Newtonsoft.Json;
using RELOCBS.Common;
using RELOCBS.Common.ExceptionHandling;
using RELOCBS.DAL.Billing;
using RELOCBS.Entities;
using RELOCBS.Utility;

namespace RELOCBS.BL.Billing
{
	public class BillingBL
	{
		private BillingDAL _billingDAL;
		public BillingDAL billingDAL
		{
			get
			{
				if (this._billingDAL == null)
					this._billingDAL = new BillingDAL();
				return this._billingDAL;
			}
		}

		public IEnumerable<Entities.Billing> GetForGrid(DateTime? FromDate, DateTime? Todate, string search, string searchtype, int? InvoiceID, string Shipper, string Status, string sort, string sortdir, int skip, int pageSize, char Type, out int totalCount, out bool RMCBuss)
		{
			try
			{
				RMCBuss = !UserSession.GetUserSession().BussinessLine.Equals("NON RMC-BUSINESS");

				IQueryable<Entities.Billing> BillList = billingDAL.GetForGrid(UserSession.GetUserSession().LoginID, FromDate, Todate, search, searchtype, InvoiceID, Shipper, Status, RMCBuss, Type);
				totalCount = BillList.Count();
				if (Type.Equals('I'))
				{
					//totalCount = surveyList.Count();

					if (!string.IsNullOrWhiteSpace(sort))
					{
						//BillList = BillList.OrderBy(sort + " " + sortdir);
					}


					if (pageSize > 0)
					{
						BillList = BillList.Skip(skip).Take(pageSize);
					}
				}

				return BillList.ToList();
			}
			catch (DataAccessException ex)
			{
				throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
			}
			catch (Exception ex)
			{
				throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "BillingBL", "GetForGrid", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
			}
		}

		public Entities.Billing GetDetailById(int? MoveID, int? InvoiceID, int? CreditNoteID, char Type, int? RateCompID, string key = null, bool IsOthApp = false, int? StrgInvID = null)
		{
			Entities.Billing invObj = new Entities.Billing();

			try
			{
				string QRSignedValue = string.Empty, BankQRValue = string.Empty;
				DataSet invoiceDt = StrgInvID != null ? billingDAL.GetDetailById(MoveID: MoveID, StrgInvMasterID: StrgInvID, InvoiceID: InvoiceID, CreditNoteID: CreditNoteID, Type: Type, rateCompID: RateCompID)
					: billingDAL.GetDetailById(MoveID, InvoiceID, CreditNoteID, Type, RateCompID, key, IsOthApp);

				if (invoiceDt != null && invoiceDt.Tables.Count > 0 && invoiceDt.Tables[0].Rows.Count > 0)
				{
					invObj.CreditNoteNo = invoiceDt.Tables[0].Rows[0]["ActCreditNumber"] == DBNull.Value ? null : Convert.ToString(invoiceDt.Tables[0].Rows[0]["ActCreditNumber"]);
					invObj.CreditNoteDate = invoiceDt.Tables[0].Rows[0]["CreditNoteDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(invoiceDt.Tables[0].Rows[0]["CreditNoteDate"]);
					invObj.SLShortName = invoiceDt.Tables[0].Rows[0]["SLShortName"] == DBNull.Value ? null : Convert.ToString(invoiceDt.Tables[0].Rows[0]["SLShortName"]);
					invObj.BillID = invoiceDt.Tables[0].Rows[0]["InvID"] == DBNull.Value ? 0 : Convert.ToInt32(invoiceDt.Tables[0].Rows[0]["InvID"]);
					invObj.CreditNoteID = invoiceDt.Tables[0].Rows[0]["CreditNoteID"] == DBNull.Value ? 0 : Convert.ToInt32(invoiceDt.Tables[0].Rows[0]["CreditNoteID"]);
					invObj.Project = invoiceDt.Tables[0].Rows[0]["Project"] == DBNull.Value ? null : Convert.ToString(invoiceDt.Tables[0].Rows[0]["Project"]);
					invObj.JobNo = invoiceDt.Tables[0].Rows[0]["JobID"] == DBNull.Value ? null : Convert.ToString(invoiceDt.Tables[0].Rows[0]["JobID"]);
					invObj.JobDate = invoiceDt.Tables[0].Rows[0]["JobDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(invoiceDt.Tables[0].Rows[0]["JobDate"]);
					invObj.MoveID = invoiceDt.Tables[0].Rows[0]["MoveID"] == DBNull.Value ? 0 : Convert.ToInt32(invoiceDt.Tables[0].Rows[0]["MoveID"]);
					invObj.InvoiceNo = invoiceDt.Tables[0].Rows[0]["ActInvNumber"] == DBNull.Value ? "0" : Convert.ToString(invoiceDt.Tables[0].Rows[0]["ActInvNumber"]);
					invObj.InvoiceDate = invoiceDt.Tables[0].Rows[0]["InvDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(invoiceDt.Tables[0].Rows[0]["InvDate"]);
					invObj.InvoiceStatus = invoiceDt.Tables[0].Rows[0]["InVStatus"] == DBNull.Value ? null : Convert.ToString(invoiceDt.Tables[0].Rows[0]["InVStatus"]);
					invObj.BillToID = invoiceDt.Tables[0].Rows[0]["BillTo"] == DBNull.Value ? null : Convert.ToString(invoiceDt.Tables[0].Rows[0]["BillTo"]);
					invObj.AddressType = invoiceDt.Tables[0].Rows[0]["BillToOrgOrDest"] == DBNull.Value ? 'O' : Convert.ToChar(invoiceDt.Tables[0].Rows[0]["BillToOrgOrDest"]);
					invObj.Attention = invoiceDt.Tables[0].Rows[0]["AttentionFName"] == DBNull.Value ? null : Convert.ToString(invoiceDt.Tables[0].Rows[0]["AttentionFName"]);
					invObj.Address1 = invoiceDt.Tables[0].Rows[0]["Address1"] == DBNull.Value ? null : Convert.ToString(invoiceDt.Tables[0].Rows[0]["Address1"]);
					invObj.Address2 = invoiceDt.Tables[0].Rows[0]["Address2"] == DBNull.Value ? null : Convert.ToString(invoiceDt.Tables[0].Rows[0]["Address2"]);
					invObj.CityID = invoiceDt.Tables[0].Rows[0]["CityID"] == DBNull.Value ? 0 : Convert.ToInt32(invoiceDt.Tables[0].Rows[0]["CityID"]);
					invObj.PinCode = invoiceDt.Tables[0].Rows[0]["PinCode"] == DBNull.Value ? null : Convert.ToString(invoiceDt.Tables[0].Rows[0]["PinCode"]);
					invObj.Phone = invoiceDt.Tables[0].Rows[0]["Phone"] == DBNull.Value ? null : Convert.ToString(invoiceDt.Tables[0].Rows[0]["Phone"]);
					invObj.Email = invoiceDt.Tables[0].Rows[0]["EmailAdd"] == DBNull.Value ? null : Convert.ToString(invoiceDt.Tables[0].Rows[0]["EmailAdd"]);
					//invObj.GSTNo = invoiceDt.Tables[0].Rows[0]["BoxGSTNo"] == DBNull.Value ? null : Convert.ToString(invoiceDt.Tables[0].Rows[0]["BoxGSTNo"]); 
					invObj.OrgStorageStart = invoiceDt.Tables[0].Rows[0]["OrgStgStartDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(invoiceDt.Tables[0].Rows[0]["OrgStgStartDate"]);
					invObj.OrgStorageEnd = invoiceDt.Tables[0].Rows[0]["OrgStgEndDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(invoiceDt.Tables[0].Rows[0]["OrgStgEndDate"]);
					invObj.DestStorageStart = invoiceDt.Tables[0].Rows[0]["DestStgStartDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(invoiceDt.Tables[0].Rows[0]["DestStgStartDate"]);
					invObj.DestStorageEnd = invoiceDt.Tables[0].Rows[0]["DestStgEndDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(invoiceDt.Tables[0].Rows[0]["DestStgEndDate"]);
					invObj.LoadDate = invoiceDt.Tables[0].Rows[0]["LoadDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(invoiceDt.Tables[0].Rows[0]["LoadDate"]);
					invObj.DeliveryDate = invoiceDt.Tables[0].Rows[0]["Deliverydate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(invoiceDt.Tables[0].Rows[0]["Deliverydate"]);
					invObj.PackDate = invoiceDt.Tables[0].Rows[0]["PackDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(invoiceDt.Tables[0].Rows[0]["PackDate"]);
					invObj.PreparedBy = invoiceDt.Tables[0].Rows[0]["PreparedBy"] == DBNull.Value ? null : Convert.ToString(invoiceDt.Tables[0].Rows[0]["PreparedBy"]);
					invObj.ApprovedBy = invoiceDt.Tables[0].Rows[0]["ApprovedBy"] == DBNull.Value ? null : Convert.ToString(invoiceDt.Tables[0].Rows[0]["ApprovedBy"]);
					invObj.FinalApproveBy = invoiceDt.Tables[0].Rows[0]["FinalizedBy"] == DBNull.Value ? null : Convert.ToString(invoiceDt.Tables[0].Rows[0]["FinalizedBy"]);
					invObj.PreparedDate = invoiceDt.Tables[0].Rows[0]["PreparedDate"] == DBNull.Value ? null : Convert.ToString(invoiceDt.Tables[0].Rows[0]["PreparedDate"]);
					invObj.ApprovedDate = invoiceDt.Tables[0].Rows[0]["ApprovedDate"] == DBNull.Value ? null : Convert.ToString(invoiceDt.Tables[0].Rows[0]["ApprovedDate"]);
					invObj.FinalApproveDate = invoiceDt.Tables[0].Rows[0]["FinalizedDate"] == DBNull.Value ? null : Convert.ToString(invoiceDt.Tables[0].Rows[0]["FinalizedDate"]);
					invObj.TaxType = invoiceDt.Tables[0].Rows[0]["TaxType"] == DBNull.Value ? null : Convert.ToString(invoiceDt.Tables[0].Rows[0]["TaxType"]);
					invObj.BaseCurrID = invoiceDt.Tables[0].Rows[0]["BaseCurrID"] == DBNull.Value ? (int?)null : Convert.ToInt32(invoiceDt.Tables[0].Rows[0]["BaseCurrID"]);
					invObj.BaseCurrName = invoiceDt.Tables[0].Rows[0]["BaseCurrName"] == DBNull.Value ? null : Convert.ToString(invoiceDt.Tables[0].Rows[0]["BaseCurrName"]);
					invObj.RateTypeID = invoiceDt.Tables[0].Rows[0]["RateTypeID"] == DBNull.Value ? (int?)null : Convert.ToInt32(invoiceDt.Tables[0].Rows[0]["RateTypeID"]);
					invObj.RateCurrancyID = invoiceDt.Tables[0].Rows[0]["BillCurr"] == DBNull.Value ? (int?)null : Convert.ToInt32(invoiceDt.Tables[0].Rows[0]["BillCurr"]);
					invObj.ConvRate = invoiceDt.Tables[0].Rows[0]["ConvRate"] == DBNull.Value ? 1 : Convert.ToDecimal(invoiceDt.Tables[0].Rows[0]["ConvRate"]);
					invObj.FileNo = invoiceDt.Tables[0].Rows[0]["RMCFileNo"] == DBNull.Value ? null : Convert.ToString(invoiceDt.Tables[0].Rows[0]["RMCFileNo"]);
					invObj.WKNo = invoiceDt.Tables[0].Rows[0]["WorkOrderNo"] == DBNull.Value ? null : Convert.ToString(invoiceDt.Tables[0].Rows[0]["WorkOrderNo"]);
					invObj.Account = invoiceDt.Tables[0].Rows[0]["Account"] == DBNull.Value ? null : Convert.ToString(invoiceDt.Tables[0].Rows[0]["Account"]);
					invObj.Client = invoiceDt.Tables[0].Rows[0]["Client"] == DBNull.Value ? null : Convert.ToString(invoiceDt.Tables[0].Rows[0]["Client"]);
					invObj.Shipper = invoiceDt.Tables[0].Rows[0]["Shipper"] == DBNull.Value ? null : Convert.ToString(invoiceDt.Tables[0].Rows[0]["Shipper"]);
					invObj.WtValue = invoiceDt.Tables[0].Rows[0]["NetWt"] == DBNull.Value ? null : Convert.ToString(invoiceDt.Tables[0].Rows[0]["NetWt"]);
					invObj.VolumeValue = invoiceDt.Tables[0].Rows[0]["NetVol"] == DBNull.Value ? null : Convert.ToString(invoiceDt.Tables[0].Rows[0]["NetVol"]);
					invObj.OrgCity = invoiceDt.Tables[0].Rows[0]["OrgCity"] == DBNull.Value ? null : Convert.ToString(invoiceDt.Tables[0].Rows[0]["OrgCity"]);
					invObj.DestCity = invoiceDt.Tables[0].Rows[0]["DestCity"] == DBNull.Value ? null : Convert.ToString(invoiceDt.Tables[0].Rows[0]["DestCity"]);
					invObj.OriginAgent = invoiceDt.Tables[0].Rows[0]["OrgAgent"] == DBNull.Value ? null : Convert.ToString(invoiceDt.Tables[0].Rows[0]["OrgAgent"]);
					invObj.DestinationAgent = invoiceDt.Tables[0].Rows[0]["DestAgent"] == DBNull.Value ? null : Convert.ToString(invoiceDt.Tables[0].Rows[0]["DestAgent"]);
					invObj.BillToClientID = invoiceDt.Tables[0].Rows[0]["BillToClientID"] == DBNull.Value ? 0 : Convert.ToInt32(invoiceDt.Tables[0].Rows[0]["BillToClientID"]);
					invObj.BillToAccountID = invoiceDt.Tables[0].Rows[0]["BillToAccountID"] == DBNull.Value ? 0 : Convert.ToInt32(invoiceDt.Tables[0].Rows[0]["BillToAccountID"]);
					invObj.BillToShipperName = invoiceDt.Tables[0].Rows[0]["ShipperName"] == DBNull.Value ? null : Convert.ToString(invoiceDt.Tables[0].Rows[0]["ShipperName"]);
					invObj.ServiceLn = invoiceDt.Tables[0].Rows[0]["ServiceLine"] == DBNull.Value ? null : Convert.ToString(invoiceDt.Tables[0].Rows[0]["ServiceLine"]);
					invObj.RevenueBr = invoiceDt.Tables[0].Rows[0]["RevenueBr"] == DBNull.Value ? null : Convert.ToString(invoiceDt.Tables[0].Rows[0]["RevenueBr"]);
					invObj.Mode = invoiceDt.Tables[0].Rows[0]["ModeName"] == DBNull.Value ? null : Convert.ToString(invoiceDt.Tables[0].Rows[0]["ModeName"]);
					invObj.Remark = invoiceDt.Tables[0].Rows[0]["RemarksOnStatus"] == DBNull.Value ? null : Convert.ToString(invoiceDt.Tables[0].Rows[0]["RemarksOnStatus"]);
					//invObj.Other.PANNo = Convert.ToString(invoiceDt.Tables[0].Rows[0]["CompPanNo"]);
					invObj.Other.StateName = Convert.ToString(invoiceDt.Tables[0].Rows[0]["BillStateName"]);
					invObj.Other.StateCD = Convert.ToString(invoiceDt.Tables[0].Rows[0]["BillStateCD"]);
					invObj.Other.GSTNo = Convert.ToString(invoiceDt.Tables[0].Rows[0]["BillGSTNo"]);
					invObj.RateCurrName = Convert.ToString(invoiceDt.Tables[0].Rows[0]["BillCurrName"]);
					invObj.AEDCurr = invoiceDt.Tables[0].Rows[0]["AEDConversionRate"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(invoiceDt.Tables[0].Rows[0]["AEDConversionRate"]);
					invObj.Controller = Convert.ToString(invoiceDt.Tables[0].Rows[0]["Controller"]);
					invObj.GrossWtValue = Convert.ToString(invoiceDt.Tables[0].Rows[0]["GrossWt"]);
					invObj.GrossVolumeValue = Convert.ToString(invoiceDt.Tables[0].Rows[0]["GrossVol"]);
					invObj.GoodsDesc = Convert.ToString(invoiceDt.Tables[0].Rows[0]["GoodsDesc"]);
					invObj.IsGrossWtValue = Convert.ToBoolean(invoiceDt.Tables[0].Rows[0]["IsGrossWt"]);
					invObj.IsGrossVolumeValue = Convert.ToBoolean(invoiceDt.Tables[0].Rows[0]["IsGrossVol"]);
					invObj.IsWtValue = Convert.ToBoolean(invoiceDt.Tables[0].Rows[0]["IsNetWt"]);
					invObj.IsVolumeValue = Convert.ToBoolean(invoiceDt.Tables[0].Rows[0]["IsNetVol"]);
					invObj.IsGoodsDesc = Convert.ToBoolean(invoiceDt.Tables[0].Rows[0]["IsGoodsDesc"]);
					invObj.chequeNo = Convert.ToString(invoiceDt.Tables[0].Rows[0]["chequeNo"]);
					invObj.BillRemarks = Convert.ToString(invoiceDt.Tables[0].Rows[0]["BillRemarks"]);
					invObj.VehicleNo = Convert.ToString(invoiceDt.Tables[0].Rows[0]["VehicleNo"]);
					invObj.AdvanceRecv = invoiceDt.Tables[0].Rows[0]["AdvanceRecv"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(invoiceDt.Tables[0].Rows[0]["AdvanceRecv"]);
					invObj.NoofPkgs = invoiceDt.Tables[0].Rows[0]["NoofPkgs"] == DBNull.Value ? (int?)null : Convert.ToInt32(invoiceDt.Tables[0].Rows[0]["NoofPkgs"]);
					invObj.BillSubDate = invoiceDt.Tables[0].Rows[0]["BillSubDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(invoiceDt.Tables[0].Rows[0]["BillSubDate"]);
					invObj.BillToCity = Convert.ToString(invoiceDt.Tables[0].Rows[0]["BillToCity"]);
					invObj.BillAcknowledgement = Convert.ToString(invoiceDt.Tables[0].Rows[0]["BillAcknowledgement"]);
					invObj.OrgStorageState = invoiceDt.Tables[0].Rows[0]["OrgWHStateID"] == DBNull.Value ? (int?)null : Convert.ToInt32(invoiceDt.Tables[0].Rows[0]["OrgWHStateID"]);
					invObj.DestStorageState = invoiceDt.Tables[0].Rows[0]["DestWHStateID"] == DBNull.Value ? (int?)null : Convert.ToInt32(invoiceDt.Tables[0].Rows[0]["DestWHStateID"]);
					invObj.RMCType = Convert.ToString(invoiceDt.Tables[0].Rows[0]["RMCType"]);
					invObj.ParsifalAuditStartDate = invoiceDt.Tables[0].Rows[0]["ParsifalAuditStartDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(invoiceDt.Tables[0].Rows[0]["ParsifalAuditStartDate"]);
					invObj.MoneyReceivedDate = invoiceDt.Tables[0].Rows[0]["MoneyReceivedDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(invoiceDt.Tables[0].Rows[0]["MoneyReceivedDate"]);
					invObj.OrgStorageStateNm = Convert.ToString(invoiceDt.Tables[0].Rows[0]["WhOrgLocation"]);
					invObj.DestStorageStateNm = Convert.ToString(invoiceDt.Tables[0].Rows[0]["WhDestLocation"]);
					invObj.OrgCountry = Convert.ToString(invoiceDt.Tables[0].Rows[0]["OrgCountry"]);
					invObj.DestCountry = Convert.ToString(invoiceDt.Tables[0].Rows[0]["DestCountry"]);
					invObj.RMCID = Convert.ToInt32(invoiceDt.Tables[0].Rows[0]["RMCID"]);

					//////Storage Invoice Parameter
					if (StrgInvID != null)
					{
						invObj.StrgInvID = invoiceDt.Tables[0].Rows[0]["StrgInvMasterID"] == DBNull.Value ? (int?)null : Convert.ToInt32(invoiceDt.Tables[0].Rows[0]["StrgInvMasterID"]);
						invObj.StrgJobID = invoiceDt.Tables[0].Rows[0]["StrgJobMasterID"] == DBNull.Value ? (int?)null : Convert.ToInt32(invoiceDt.Tables[0].Rows[0]["StrgJobMasterID"]);
						invObj.StrgVolValue = invoiceDt.Tables[0].Rows[0]["WtVol"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(invoiceDt.Tables[0].Rows[0]["WtVol"]);
						invObj.StrgVolUnitID = invoiceDt.Tables[0].Rows[0]["WeightUnitID"] == DBNull.Value ? (int?)null : Convert.ToInt32(invoiceDt.Tables[0].Rows[0]["WeightUnitID"]);
						invObj.StrgVolUnit = invoiceDt.Tables[0].Rows[0]["WeightUnitName"] == DBNull.Value ? null : Convert.ToString(invoiceDt.Tables[0].Rows[0]["WeightUnitName"]);
					}
					invObj.BtrService = invoiceDt.Tables[0].Rows[0]["BTRServiceID"] == DBNull.Value ? (int?)null : Convert.ToInt32(invoiceDt.Tables[0].Rows[0]["BTRServiceID"]);
					invObj.BtrPaymentTerm = invoiceDt.Tables[0].Rows[0]["BTRPaymentTermID"] == DBNull.Value ? (int?)null : Convert.ToInt32(invoiceDt.Tables[0].Rows[0]["BTRPaymentTermID"]);
					invObj.BtrServiceName = invoiceDt.Tables[0].Rows[0]["BTRServiceName"] == DBNull.Value ? null : Convert.ToString(invoiceDt.Tables[0].Rows[0]["BTRServiceName"]);
					invObj.BtrPaymentTermName = invoiceDt.Tables[0].Rows[0]["BTRPaymentTermName"] == DBNull.Value ? null : Convert.ToString(invoiceDt.Tables[0].Rows[0]["BTRPaymentTermName"]);
					invObj.IsCollectionDate = invoiceDt.Tables[0].Rows[0]["IsCollectionDate"] == DBNull.Value ? false : Convert.ToBoolean(invoiceDt.Tables[0].Rows[0]["IsCollectionDate"]);
					invObj.BillAddInfo = Convert.ToString(invoiceDt.Tables[0].Rows[0]["BillAddInfo"]);
					invObj.PermitApproveInv = invoiceDt.Tables[0].Rows[0]["PermitApproveInv"] == DBNull.Value ? false : Convert.ToBoolean(invoiceDt.Tables[0].Rows[0]["PermitApproveInv"]);
					invObj.ParsifalApproveDate = invoiceDt.Tables[0].Rows[0]["ParsifalApproveDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(invoiceDt.Tables[0].Rows[0]["ParsifalApproveDate"]);
                    invObj.BillCategoryID = invoiceDt.Tables[0].Rows[0]["BillCategoryID"] == DBNull.Value ? (Int32?)null : Convert.ToInt32(invoiceDt.Tables[0].Rows[0]["BillCategoryID"]);
                    invObj.BillCategoryName = Convert.ToString(invoiceDt.Tables[0].Rows[0]["BillCategoryName"]);
                    invObj.SunCost = invoiceDt.Tables[0].Rows[0]["SunCost"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(invoiceDt.Tables[0].Rows[0]["SunCost"]);
                    invObj.Is_SunCostShow = invoiceDt.Tables[0].Rows[0]["IsSunCostShow"] == DBNull.Value ? false : Convert.ToBoolean(invoiceDt.Tables[0].Rows[0]["IsSunCostShow"]);
                    invObj.IsAnnexure = invoiceDt.Tables[0].Rows[0]["IsAnnexure"] == DBNull.Value ? false : Convert.ToBoolean(invoiceDt.Tables[0].Rows[0]["IsAnnexure"]);

                    if (invoiceDt.Tables[1] != null && invoiceDt.Tables[1].Rows.Count > 0)
					{
						invObj.BillItems = invoiceDt.Tables[1].AsEnumerable().
							Select(dataRow => new Entities.BillingItems
							{
								TaxType = dataRow["TaxType"] == DBNull.Value ? null : Convert.ToString(dataRow["TaxType"]),
								BillItemSeqID = dataRow["OrderSeq"] == DBNull.Value ? 0 : Convert.ToInt64(dataRow["OrderSeq"]),
								SacCode = dataRow["SACCode"] == DBNull.Value ? null : Convert.ToString(dataRow["SACCode"]),
								POSID = dataRow["POSID"] == DBNull.Value ? null : Convert.ToString(dataRow["POSID"]),
								ComponentID = dataRow["CompID"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["CompID"]),
								Component = dataRow["RateComp"] == DBNull.Value ? null : Convert.ToString(dataRow["RateComp"]),
								CostHeadID = dataRow["CostHeadID"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["CostHeadID"]),
								CostHead = dataRow["CostHead"] == DBNull.Value ? null : Convert.ToString(dataRow["CostHead"]),
								Description = dataRow["DESCRIPTION"] == DBNull.Value ? null : Convert.ToString(dataRow["DESCRIPTION"]),
								OriginalAmount = dataRow["OgAmount"] == DBNull.Value ? 0 : Convert.ToDecimal(dataRow["OgAmount"]),
								Amount = dataRow["Amount"] == DBNull.Value ? 0 : Convert.ToDecimal(dataRow["Amount"]),
								AuditedAmount = dataRow["AfterAuditAmt"] == DBNull.Value ? 0 : Convert.ToDecimal(dataRow["AfterAuditAmt"]),
								TaxApp = Convert.ToBoolean(dataRow["TaxApp"]),
								GSTVATPercent = dataRow["TaxPer"] == DBNull.Value ? 0 : Convert.ToDecimal(dataRow["TaxPer"]),
								Unbill = Convert.ToBoolean(dataRow["BringToUnbill"]),
								SGSTAmt = dataRow["SGST"] == DBNull.Value ? 0 : Convert.ToDecimal(dataRow["SGST"]),
								CGSTAmt = dataRow["CGST"] == DBNull.Value ? 0 : Convert.ToDecimal(dataRow["CGST"]),
								IGSTAmt = dataRow["IGST"] == DBNull.Value ? 0 : Convert.ToDecimal(dataRow["IGST"]),
								ShowReverseButton = dataRow["ShowRevButton"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["ShowRevButton"]),
								//ConvAmount = dataRow["ConversionAmount"] == DBNull.Value ? 0 : Convert.ToDecimal(dataRow["ConversionAmount"]),
								//GSTVATAmount = ((dataRow["ConversionRate"] == DBNull.Value ? 0 : Convert.ToDecimal(dataRow["ConversionRate"]) * 18) / 100)

							}).ToList();

						invObj.BtrTaxList = BTRTaxList(invObj);
					}

					if (invoiceDt.Tables[2] != null && invoiceDt.Tables[2].Rows.Count > 0)
					{
						invObj.Bank = invoiceDt.Tables[2].AsEnumerable().
							Select(dataRow => new Entities.BankDetails
							{
								Header = Convert.ToString(dataRow["InfoHeading"]),
								Value = Convert.ToString(dataRow["InfoValue"]),
							}).ToList();
						//invObj.Bank.BeneciaryBank = Convert.ToString(invoiceDt.Tables[2].Rows[0]["CompbankName"]);
						//invObj.Bank.BANKADDRESS = Convert.ToString(invoiceDt.Tables[2].Rows[0]["BankAddress1"])+" "+Convert.ToString(invoiceDt.Tables[2].Rows[0]["BankAddress2"]);
						//invObj.Bank.AccountForUAD = Convert.ToString(invoiceDt.Tables[2].Rows[0]["USDAcNo"]);
						//invObj.Bank.BENIFICIARYNAME = "";
						//invObj.Bank.BENIFICIARYADDRESS= "";
						//invObj.Bank.SWIFTCODE = Convert.ToString(invoiceDt.Tables[2].Rows[0]["SwiftCode"]);
						//invObj.Bank.IBAN = Convert.ToString(invoiceDt.Tables[2].Rows[0]["IBANNo"]);
						//invObj.Other.PANNo = Convert.ToString(invoiceDt.Tables[2].Rows[0]["CompPanNo"]);
						//invObj.Other.StateName = Convert.ToString(invoiceDt.Tables[2].Rows[0]["BillStateName"]);
						//invObj.Other.StateCD = Convert.ToString(invoiceDt.Tables[2].Rows[0]["BillStateCD"]);
						//invObj.Other.GSTNo = Convert.ToString(invoiceDt.Tables[2].Rows[0]["BillGSTNo"]);
						//invObj.Other.GSTNoOur = Convert.ToString(invoiceDt.Tables[2].Rows[0]["BillGSTNoOur"]);
						//invObj.Other.POS = Convert.ToString(invoiceDt.Tables[2].Rows[0]["BillPOS"]);
					}

					if (invoiceDt.Tables[3] != null && invoiceDt.Tables[3].Rows.Count > 0)
					{
						invObj.Other.POS = Convert.ToString(invoiceDt.Tables[3].Rows[0]["DestAdd"]);
						invObj.Other.AddressType = Convert.ToString(invoiceDt.Tables[3].Rows[0]["POSType"]);
						invObj.Other.Address = Convert.ToString(invoiceDt.Tables[3].Rows[0]["DestAdd2"]);


					}

					if (invoiceDt.Tables[4] != null && invoiceDt.Tables[4].Rows.Count > 0)
					{
						invObj.Other.Line1 = Convert.ToString(invoiceDt.Tables[4].Rows[0]["Line1"]);
						invObj.Other.Line2 = Convert.ToString(invoiceDt.Tables[4].Rows[0]["Line2"]);
						invObj.Other.Line3 = Convert.ToString(invoiceDt.Tables[4].Rows[0]["Line3"]);
						invObj.Other.Line4 = Convert.ToString(invoiceDt.Tables[4].Rows[0]["Line4"]);
						invObj.Other.Line5 = Convert.ToString(invoiceDt.Tables[4].Rows[0]["Line5"]);
						invObj.Other.BottomLine1 = Convert.ToString(invoiceDt.Tables[4].Rows[0]["BottomLine1"]);
						invObj.Other.BottomLine2 = Convert.ToString(invoiceDt.Tables[4].Rows[0]["BottomLine2"]);
						invObj.Other.BottomLine3 = Convert.ToString(invoiceDt.Tables[4].Rows[0]["BottomLine3"]);
						invObj.Other.AboveBankInfo = Convert.ToString(invoiceDt.Tables[4].Rows[0]["AboveBankInfo"]);
						invObj.Other.CompanyNameLine1 = Convert.ToString(invoiceDt.Tables[4].Rows[0]["CompNameLine1"]);
						invObj.Other.CompanyNameLine2 = Convert.ToString(invoiceDt.Tables[4].Rows[0]["CompNameLine2"]);
						invObj.Other.GSTNoOur = Convert.ToString(invoiceDt.Tables[4].Rows[0]["GSTNo"]);
						invObj.Other.PANNo = Convert.ToString(invoiceDt.Tables[4].Rows[0]["PANNo"]);
						invObj.Other.VATNo = Convert.ToString(invoiceDt.Tables[4].Rows[0]["VATNo"]);
					}

					if (invoiceDt.Tables[5] != null && invoiceDt.Tables[5].Rows.Count > 0)
					{
						invObj.GSTLogic.ServiceProvided = Convert.ToString(invoiceDt.Tables[5].Rows[0]["ServiceProvided"]);
						invObj.GSTLogic.ServiceProviderInIndia = Convert.ToString(invoiceDt.Tables[5].Rows[0]["ServiceProviderInIndia"]);
						invObj.GSTLogic.ServiceReceiverRegistionStatus = Convert.ToString(invoiceDt.Tables[5].Rows[0]["ServiceReceiverRegistionStatus"]);
						invObj.GSTLogic.ServiceReceiverInIndia = Convert.ToString(invoiceDt.Tables[5].Rows[0]["ServiceReceiverInIndia"]);
						invObj.GSTLogic.OriginInIndia = Convert.ToString(invoiceDt.Tables[5].Rows[0]["OriginInIndia"]);
						invObj.GSTLogic.DestInIndia = Convert.ToString(invoiceDt.Tables[5].Rows[0]["DestInIndia"]);
						invObj.GSTLogic.IsRoadMode = Convert.ToString(invoiceDt.Tables[5].Rows[0]["IsRoadMode"]);
						invObj.GSTLogic.IsRevCurrINR = Convert.ToString(invoiceDt.Tables[5].Rows[0]["IsRevCurrINR"]);
						invObj.GSTLogic.ServiceProviderStateID = Convert.ToString(invoiceDt.Tables[5].Rows[0]["ServiceProviderStateID"]);
						invObj.GSTLogic.IsPOS_InIndia = Convert.ToString(invoiceDt.Tables[5].Rows[0]["IsPOS_InIndia"]);
						invObj.GSTLogic.POS_Rule = Convert.ToString(invoiceDt.Tables[5].Rows[0]["POS_Rule"]);
						invObj.GSTLogic.POS_StateID = Convert.ToString(invoiceDt.Tables[5].Rows[0]["POS_StateID"]);
						invObj.GSTLogic.GSTTYPE = Convert.ToString(invoiceDt.Tables[5].Rows[0]["GSTTYPE"]);
						invObj.GSTLogic.GST_Percent = Convert.ToString(invoiceDt.Tables[5].Rows[0]["GST_Percent"]);
						invObj.GSTLogic.OrgStgPOS_Rule = Convert.ToString(invoiceDt.Tables[5].Rows[0]["OrgStgPOS_Rule"]);
						invObj.GSTLogic.OrgStgIsPOS_InIndia = Convert.ToString(invoiceDt.Tables[5].Rows[0]["OrgStgIsPOS_InIndia"]);
						invObj.GSTLogic.OrgStgPOS_StateID = Convert.ToString(invoiceDt.Tables[5].Rows[0]["OrgStgPOS_StateID"]);
						invObj.GSTLogic.DestStgPOS_Rule = Convert.ToString(invoiceDt.Tables[5].Rows[0]["DestStgPOS_Rule"]);
						invObj.GSTLogic.DestStgIsPOS_InIndia = Convert.ToString(invoiceDt.Tables[5].Rows[0]["DestStgIsPOS_InIndia"]);
						invObj.GSTLogic.DestStgPOS_StateID = Convert.ToString(invoiceDt.Tables[5].Rows[0]["DestStgPOS_StateID"]);
						invObj.InvType = Convert.ToString(invoiceDt.Tables[5].Rows[0]["InvType"]);
						invObj.ShowEInvoice = Convert.ToBoolean(invoiceDt.Tables[5].Rows[0]["ShowEInvoice"]);
					}

					if (invoiceDt.Tables[6] != null && invoiceDt.Tables[6].Rows.Count > 0)
					{
						invObj.StatementSub = Convert.ToString(invoiceDt.Tables[6].Rows[0]["StatementSub"]);
						invObj.Specification = Convert.ToString(invoiceDt.Tables[6].Rows[0]["Specification"]);
						invObj.StatementCreatedDate = invoiceDt.Tables[6].Rows[0]["CreatedDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(invoiceDt.Tables[6].Rows[0]["CreatedDate"]);
					}

					if (invoiceDt.Tables[7] != null && invoiceDt.Tables[7].Rows.Count > 0)
					{
						QRSignedValue = Convert.ToString(invoiceDt.Tables[7].Rows[0]["SignedQRCode"]);
						invObj.IRNNo = Convert.ToString(invoiceDt.Tables[7].Rows[0]["IRNNo"]);
						BankQRValue = Convert.ToString(invoiceDt.Tables[7].Rows[0]["BankQRCode"]);
					}

					if (invoiceDt.Tables[8] != null && invoiceDt.Tables[8].Rows.Count > 0)
					{
						invObj.Subject = invoiceDt.Tables[8].Rows[0]["SubjectInfo"] == DBNull.Value ? null : Convert.ToString(invoiceDt.Tables[8].Rows[0]["SubjectInfo"]);
						invObj.Note = Convert.ToString(invoiceDt.Tables[8].Rows[0]["NoteInfo"]);
						invObj.IsSubject = Convert.ToBoolean(invoiceDt.Tables[8].Rows[0]["ShowSubInInv"]);
						invObj.IsNote = Convert.ToBoolean(invoiceDt.Tables[8].Rows[0]["ShowNoteInInv"]);
					}
					if (invoiceDt.Tables[9] != null && invoiceDt.Tables[9].Rows.Count > 0)
					{
						invObj.LutMsg = Convert.ToString(invoiceDt.Tables[9].Rows[0]["LUTMsg"]);
						invObj.LutNo = Convert.ToString(invoiceDt.Tables[9].Rows[0]["LUTNo"]);
					}
					if (invoiceDt.Tables[10] != null && invoiceDt.Tables[10].Rows.Count > 0)
					{
						invObj.BillingEntity = invoiceDt.Tables[10].Rows[0]["BillingEntityID"] == DBNull.Value ? (int?)null : Convert.ToInt32(invoiceDt.Tables[10].Rows[0]["BillingEntityID"]);
						invObj.BillingEntityName = invoiceDt.Tables[10].Rows[0]["RMCEntityName"] == DBNull.Value ? null : Convert.ToString(invoiceDt.Tables[10].Rows[0]["RMCEntityName"]);
						invObj.Address1 = invoiceDt.Tables[10].Rows[0]["Address1"] == DBNull.Value ? invObj.Address1 : Convert.ToString(invoiceDt.Tables[10].Rows[0]["Address1"]);
						invObj.Address2 = invoiceDt.Tables[10].Rows[0]["Address2"] == DBNull.Value ? invObj.Address2 : Convert.ToString(invoiceDt.Tables[10].Rows[0]["Address2"]);
						invObj.CityID = invoiceDt.Tables[10].Rows[0]["CityID"] == DBNull.Value ? invObj.CityID : Convert.ToInt32(invoiceDt.Tables[10].Rows[0]["CityID"]);
						invObj.PinCode = invoiceDt.Tables[10].Rows[0]["PinCode"] == DBNull.Value ? invObj.PinCode : Convert.ToString(invoiceDt.Tables[10].Rows[0]["PinCode"]);
						invObj.Phone = invoiceDt.Tables[10].Rows[0]["Phone"] == DBNull.Value ? invObj.Phone : Convert.ToString(invoiceDt.Tables[10].Rows[0]["Phone"]);
						invObj.Email = invoiceDt.Tables[10].Rows[0]["EmailAdd"] == DBNull.Value ? invObj.Email : Convert.ToString(invoiceDt.Tables[10].Rows[0]["EmailAdd"]);
					}

					if (invoiceDt.Tables[11] != null && invoiceDt.Tables[11].Rows.Count > 0)
					{
						invObj.CreditNoteEntity = invoiceDt.Tables[11].Rows[0]["BillingEntityID"] == DBNull.Value ? (int?)null : Convert.ToInt32(invoiceDt.Tables[11].Rows[0]["BillingEntityID"]);
						invObj.CreditNoteEntityName = invoiceDt.Tables[11].Rows[0]["RMCEntityName"] == DBNull.Value ? null : Convert.ToString(invoiceDt.Tables[11].Rows[0]["RMCEntityName"]);
						invObj.Address1 = invoiceDt.Tables[11].Rows[0]["Address1"] == DBNull.Value ? invObj.Address1 : Convert.ToString(invoiceDt.Tables[11].Rows[0]["Address1"]);
						invObj.Address2 = invoiceDt.Tables[11].Rows[0]["Address2"] == DBNull.Value ? invObj.Address2 : Convert.ToString(invoiceDt.Tables[11].Rows[0]["Address2"]);
						invObj.CityID = invoiceDt.Tables[11].Rows[0]["CityID"] == DBNull.Value ? invObj.CityID : Convert.ToInt32(invoiceDt.Tables[11].Rows[0]["CityID"]);
						invObj.PinCode = invoiceDt.Tables[11].Rows[0]["PinCode"] == DBNull.Value ? invObj.PinCode : Convert.ToString(invoiceDt.Tables[11].Rows[0]["PinCode"]);
						invObj.Phone = invoiceDt.Tables[11].Rows[0]["Phone"] == DBNull.Value ? invObj.Phone : Convert.ToString(invoiceDt.Tables[11].Rows[0]["Phone"]);
						invObj.Email = invoiceDt.Tables[11].Rows[0]["EmailAdd"] == DBNull.Value ? invObj.Email : Convert.ToString(invoiceDt.Tables[11].Rows[0]["EmailAdd"]);
					}

                    if (invoiceDt.Tables[12] != null && invoiceDt.Tables[12].Rows.Count > 0)
                    {
                        invObj.ArabicData = invoiceDt.Tables[12].AsEnumerable().
                            Select(dataRow => new Entities.ArabicData
                            {
                                EngInfo = dataRow["EngInfo"] == DBNull.Value ? null : Convert.ToString(dataRow["EngInfo"]),
                                ArabicInfo = dataRow["ArabicInfo"] == DBNull.Value ? null : Convert.ToString(dataRow["ArabicInfo"])
                        
                                //ConvAmount = dataRow["ConversionAmount"] == DBNull.Value ? 0 : Convert.ToDecimal(dataRow["ConversionAmount"]),
                                //GSTVATAmount = ((dataRow["ConversionRate"] == DBNull.Value ? 0 : Convert.ToDecimal(dataRow["ConversionRate"]) * 18) / 100)

                            }).ToList().ToDictionary(ele=>ele.EngInfo,ele=>ele.ArabicInfo) ;
                        
                        
                        
                    }

                    if (invoiceDt.Tables[13] != null && invoiceDt.Tables[13].Rows.Count > 0)
                    {
                        invObj.ArabicCurrData = invoiceDt.Tables[13].AsEnumerable().
                            Select(dataRow => new Entities.ArabicData
                            {
                                EngInfo = dataRow["EngInfo"] == DBNull.Value ? null : Convert.ToString(dataRow["EngInfo"]),
                                ArabicInfo = dataRow["ArabicInfo"] == DBNull.Value ? null : Convert.ToString(dataRow["ArabicInfo"])

                                //ConvAmount = dataRow["ConversionAmount"] == DBNull.Value ? 0 : Convert.ToDecimal(dataRow["ConversionAmount"]),
                                //GSTVATAmount = ((dataRow["ConversionRate"] == DBNull.Value ? 0 : Convert.ToDecimal(dataRow["ConversionRate"]) * 18) / 100)

                            }).ToList().ToDictionary(ele => ele.EngInfo, ele => ele.ArabicInfo);



                    }

                    invObj.RMCBuss = !(UserSession.GetUserSession().BussinessLine == "NON RMC-BUSINESS");
					//QRSignedValue = "eyJhbGciOiJSUzI1NiIsImtpZCI6IjQ0NDQwNUM3ODFFNDgyNTA3MkIzNENBNEY4QkRDNjA2Qzg2QjU3MjAiLCJ0eXAiOiJKV1QiLCJ4NXQiOiJSRVFGeDRIa2dsQnlzMHlrLUwzR0JzaHJWeUEifQ.eyJkYXRhIjoie1wiU2VsbGVyR3N0aW5cIjpcIjI5QUFCQ1c2Mzg2UDJaQVwiLFwiQnV5ZXJHc3RpblwiOlwiMjlBQUJDVzUwOTNNMVpOXCIsXCJEb2NOb1wiOlwiUEkyMDIxMTQyOTAwMDMzXCIsXCJEb2NUeXBcIjpcIklOVlwiLFwiRG9jRHRcIjpcIjI4LzEwLzIwMjBcIixcIlRvdEludlZhbFwiOjI2ODgwLjAwLFwiSXRlbUNudFwiOjEsXCJNYWluSHNuQ29kZVwiOlwiOTk2NTExXCIsXCJJcm5cIjpcIjczNzgwODUzMmE1ZDgyNjA2OWFkNTMxOGM0ZDZiOGE3ZWQ2ZTQyYzRiZDJlZTYwOGUwY2IyYmM3MzUwYTMzZTNcIixcIklybkR0XCI6XCIyMDIwLTExLTA1IDE3OjI4OjAwXCJ9IiwiaXNzIjoiTklDIn0.CGFDHpbzhSgHseFkYyAKNPx4HHw51aqP0kNP6j9PQy3HXJ7T7yh2ugLHqsbDvQ2BZBbuTw-KRKabGeBGPRCSQYQBoLAW3F6ODcnXDfDut-Phk7SZlPS6l89bo4f-8ZlsAglX7l6XFBoPOm6sBzeYTnSvl8cyHj6IRcO3kzqqVAaxzu-GuGFYBigo3QBOzx_a9Nqw7G4AJlBYSCy61xEAXW8NEktzb80wL0l7m-Pb6kt2KQ76yqyTEfwPinDiNeF9vGq3pceNbvD3FHAd7049v9-q4PLBBjHusGwXNh1AbUB57jHEdlfXEqZ9ywqhVfE3RtneR8MwlNGUWbJTDZCGZQ";
					if (!string.IsNullOrEmpty(QRSignedValue))
					{
						EInvoiceHandler _handler = new EInvoiceHandler();
						invObj.Image = _handler.generatecode(QRSignedValue);
					}

					if (!string.IsNullOrWhiteSpace(BankQRValue))
					{
						EInvoiceHandler _handler = new EInvoiceHandler();
						invObj.BankQRCode = _handler.generatecode(BankQRValue);
					}

					if (invObj.BillItems != null && invObj.BillItems.Count() > 0 && UserSession.GetUserSession().CompanyID == 2)
					{
						foreach (var item in invObj.BillItems)
						{
							decimal Amount = 0;
							if (invObj.BaseCurrID != 2 && invObj.RateCurrancyID != 2)
							{
								Amount = item.AuditedAmount * invObj.ConvRate;
							}
							else if (invObj.BaseCurrID != 2 && invObj.RateCurrancyID == 2)
							{
								Amount = item.AuditedAmount * invObj.ConvRate;
							}
							else if (invObj.BaseCurrID == 2 && invObj.RateCurrancyID != 2)
							{
								Amount = item.AuditedAmount / invObj.ConvRate;
							}
							else
							{
								Amount = item.AuditedAmount / invObj.ConvRate;
							}
							if (item.TaxApp)
							{
								if (invObj.BtrTaxList.Where(x => x.Code == item.SacCode).Count() > 0)
								{
									var index = invObj.BtrTaxList.FindIndex(c => c.Code == item.SacCode);
									invObj.BtrTaxList[index].Goods = invObj.BtrTaxList[index].Goods + Amount;
								}
								else
								{
									//decimal amount = (item.AuditedAmount / item.ConvRate) * item.GSTVATPercent / 100;

									invObj.BtrTaxList.Add(new BTRTax()
									{
										Code = item.SacCode,
										Rate = item.GSTVATPercent,
										Goods = Amount,
										Tax = item.GSTVATPercent
									});
								}
							}
							else
							{
								if (invObj.BtrTaxList.Where(x => x.Code == "T0").Count() > 0)
								{
									var index = invObj.BtrTaxList.FindIndex(c => c.Code == item.SacCode);
									invObj.BtrTaxList[index].Goods = invObj.BtrTaxList[index].Goods + Amount;
								}
								else
								{
									//decimal amount = (item.AuditedAmount / item.ConvRate) * item.GSTVATPercent / 100;

									invObj.BtrTaxList.Add(new BTRTax()
									{
										Code = "T0",
										Rate = 0,
										Goods = Amount,
										Tax = 0,
									});
								}
							}



						}

					}

					return invObj;
				}



			}
			catch (DataAccessException ex)
			{
				throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
			}
			catch (Exception ex)
			{
				throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "BillingBL", "GetDetailById", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
			}

			return invObj;

		}

		public bool SaveInvoice(Entities.Billing model, string status, bool Isauditamount, bool SaveTopOnly, out string result)
		{
			try
			{
				//var ModeXML = model.ModeList != null ? new XElement("Modes", model.ModeList.Select(x => new XElement("ModeIDs", new XElement("ModeID", x)))) : new XElement("Modes");

				var billitem = new XElement("InvoiceDetails",
				from emp in model.BillItems
				select new XElement("InvoiceDetail",
							   new XElement("OrderSeq", emp.BillItemSeqID),
							   new XElement("InvoiceDetailID", emp.BillItemID),
							   new XElement("SacCode", emp.SacCode),
							   new XElement("POSID", emp.POSID),
							   new XElement("ComponentID", emp.ComponentID),
							   new XElement("CostHeadID", emp.CostHeadID),
							   new XElement("Description", emp.Description),
							   new XElement("Amount", emp.Amount),
							   new XElement("ConvRate", emp.ConvRate),
							   new XElement("ConvAmt", emp.ConvAmount),
							   new XElement("AfterAuditAmt", Isauditamount ? emp.Amount : emp.AuditedAmount),
							   new XElement("TaxPer", emp.GSTVATPercent),
							   new XElement("SGSTAmt", emp.SGSTAmt),
							   new XElement("CGSTAmt", emp.CGSTAmt),
							   new XElement("IGSTAmt", emp.IGSTAmt),
							   new XElement("VatAmt", emp.VatAmt),
							   new XElement("TaxApp", emp.TaxApp),
							   new XElement("IsActive", true),
							   new XElement("BringToUnbill", emp.Unbill)
						   ));

				model.InvTotalAmount = model.BillItems.Sum(x => x.TotalAmount);
				//result = "";
				return billingDAL.InsertBilling(model, billitem, status, UserSession.GetUserSession().LoginID, SaveTopOnly, out result);
				//return true;
			}
			catch (DataAccessException ex)
			{
				throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
			}
			catch (Exception ex)
			{
				throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "CityBL", "Insert", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
			}
		}

		public bool SaveStatement(Entities.Billing model)
		{
			try
			{
				//var ModeXML = model.ModeList != null ? new XElement("Modes", model.ModeList.Select(x => new XElement("ModeIDs", new XElement("ModeID", x)))) : new XElement("Modes");

				string result = string.Empty;
				int loginid = UserSession.GetUserSession().LoginID;
				return billingDAL.InsertStatement(model, loginid, out result);
				//return true;
			}
			catch (DataAccessException ex)
			{
				throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
			}
			catch (Exception ex)
			{
				throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "BillingBL", "SaveStatement", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
			}
		}

		public bool CancelInvoice(int BillID, out string result)
		{
			try
			{
				//var ModeXML = model.ModeList != null ? new XElement("Modes", model.ModeList.Select(x => new XElement("ModeIDs", new XElement("ModeID", x)))) : new XElement("Modes");

				//string result = string.Empty;
				int loginid = UserSession.GetUserSession().LoginID;
				return billingDAL.CancelInvoice(BillID, loginid, out result);
				//return true;
			}
			catch (DataAccessException ex)
			{
				throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
			}
			catch (Exception ex)
			{
				throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "BillingBL", "CancelInvoice", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
			}
		}

		//public bool InsertBilling(Entities.Billing SaveRate, XElement Billitems, int LoginID, out string result)

		//List to XML
		//var charges = new XElement("SFRFixedCosts",
		//		from emp in model.RMCFees
		//		select new XElement("SFRFixedCost",
		//					   new XElement("FixedCostID", emp.CostHeadId),
		//					   new XElement("FixedCostVal", emp.Amount),
		//					   new XElement("FixedCostPercent", emp.Percent)
		//				   ));

		public AddressList GetAddressDetials(int Client_AccountID, Int64 MoveID, string BillTo, char OrgorDest, int BillingEntityID = 0,char BillType = 'I')
		{
			try
			{
				//RMCBuss = !UserSession.GetUserSession().BussinessLine.Equals("NON RMC-BUSINESS");
				AddressList Address = new AddressList();
				DataTable dt = billingDAL.GetAddressDetials(Client_AccountID, MoveID, BillTo, OrgorDest, BillingEntityID,BillType);

				if (dt != null && dt.Rows.Count > 0)
				{
					Address.Address1 = dt.Rows[0]["Address1"] == DBNull.Value ? null : Convert.ToString(dt.Rows[0]["Address1"]);
					Address.Address2 = dt.Rows[0]["Address2"] == DBNull.Value ? null : Convert.ToString(dt.Rows[0]["Address2"]);
					Address.CityID = dt.Rows[0]["CityID"] == DBNull.Value ? 0 : Convert.ToInt32(dt.Rows[0]["CityID"]);
					Address.Pincode = dt.Rows[0]["PINCode"] == DBNull.Value ? null : Convert.ToString(dt.Rows[0]["PINCode"]);
					Address.GSTNO = dt.Rows[0]["GSTNO"] == DBNull.Value ? null : Convert.ToString(dt.Rows[0]["GSTNO"]);
				}

				return Address;
			}
			catch (DataAccessException ex)
			{
				throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
			}
			catch (Exception ex)
			{
				throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "BillingBL", "GetAddressDetials", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
			}
		}

		public string GenerateEInvoice(string InvoiceNo)
		{
			try
			{
				DataSet ds = billingDAL.GenerateEInvoice(InvoiceNo);
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
				res = billingDAL.InsertAPIResponse(LoginID, str, Parent, govtError, "NewCBS", out result);
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
				throw new DataAccessException(Convert.ToString(UserSession.GetUserSession().LoginID), "BillingBL", "GenerateEInvoice", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
			}

		}

		public List<BTRTax> BTRTaxList(Entities.Billing obj)
		{
			List<BTRTax> btrTax = new List<BTRTax>();
			//obj.BtrTaxList = invoiceDt.Tables[2].AsEnumerable().
			//				Select(dataRow => new Entities.BankDetails
			//				{
			//					Header = Convert.ToString(dataRow["InfoHeading"]),
			//					Value = Convert.ToString(dataRow["InfoValue"]),
			//				}).ToList();
			return btrTax;
		}


	}
}