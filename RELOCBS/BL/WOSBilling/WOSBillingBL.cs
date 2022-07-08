using RELOCBS.Common;
using RELOCBS.Common.ExceptionHandling;
using RELOCBS.DAL.WOSBilling;
using RELOCBS.Entities;
using RELOCBS.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Dynamic;
using System.Xml.Linq;

namespace RELOCBS.BL.WOSBilling
{
    public class WOSBillingBL
    {
        private WOSBillingDAL _WOSBillingDAL;
        public WOSBillingDAL WOSBillingDAL
        {
            get
            {
                if (_WOSBillingDAL == null)
                    _WOSBillingDAL = new WOSBillingDAL();
                return _WOSBillingDAL;
            }
        }

        public IEnumerable<Entities.WOSBilling> GetWOSInvoiceForGrid(DateTime? FromDate, DateTime? ToDate, string Search, string SearchType, int? InvoiceID, string Shipper, string Status, string Sort, string SortDir, int Skip, int PageSize, char Type, out int TotalCount)
        {
            IQueryable<Entities.WOSBilling> WOSInvoiceList;
            TotalCount = 0;
            try
            {
                DataSet WOSInvoiceListDs = WOSBillingDAL.GetWOSInvoiceForGrid(UserSession.GetUserSession().LoginID, FromDate, ToDate, Search, SearchType, InvoiceID, Shipper, Status, Type);

                if (WOSInvoiceListDs != null && WOSInvoiceListDs.Tables.Count > 0 && WOSInvoiceListDs.Tables[0].Rows.Count > 0)
                {
                    var result = (from rw in WOSInvoiceListDs.Tables[0].AsEnumerable()
                                  select new Entities.WOSBilling()
                                  {
                                      MoveID = Convert.ToInt32(rw["MoveID"]),
                                      WOSMoveID = Convert.ToInt64(rw["WOSMoveID"]),
                                      CreditNoteID = Convert.ToInt32(rw["CreditNoteID"]),
                                      BillID = Convert.ToInt32(rw["InvID"]),
                                      JobNo = Convert.ToString(rw["JobID"]),
                                      InvoiceNo = Convert.ToString(rw["ActInvNumber"]),
                                      CreditNoteNo = Convert.ToString(rw["ActCreditNumber"]),
                                      Client = Convert.ToString(rw["ClientName"]),
                                      InvoiceDate = Convert.ToDateTime(rw["Createddate"]),
                                      Shipper = Convert.ToString(rw["ShipperName"]),
                                      Mode = Convert.ToString(rw["ModeName"]),
                                      InvoiceStatus = Convert.ToString(rw["InvStatus"]),
                                      IsShowCreditNote = Convert.ToBoolean(rw["IsShowCreditNote"]),
                                      IsCreateCreditNote = Convert.ToBoolean(rw["IsCreateCreditNote"]),
                                      IsShowDelete = Convert.ToBoolean(rw["ShowDelete"]),
                                      StrgInvID = rw["StrgInvMasterID"] != DBNull.Value ? Convert.ToInt32(rw["StrgInvMasterID"]) : (int?)null,
                                      StrgJobID = rw["StrgJobmasterID"] != DBNull.Value ? Convert.ToInt32(rw["StrgJobmasterID"]) : (int?)null
                                  }).ToList();

                    WOSInvoiceList = result.AsQueryable();

                    TotalCount = WOSInvoiceList.Count();
                    WOSInvoiceList = WOSInvoiceList.OrderBy(Sort + " " + SortDir);
                    if (PageSize > 0)
                    {
                        WOSInvoiceList = WOSInvoiceList.Skip(Skip).Take(PageSize);
                    }
                    return WOSInvoiceList.ToList();
                }
                else
                {
                    return new List<Entities.WOSBilling>();
                }
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "WOSBillingBL", "GetWOSInvoiceForGrid", Properties.Resources.UnExpectedErrorAtBL, ex);
            }
        }

        public Entities.WOSBilling GetWOSInvoiceDetailsById(int LoginID, Int64? MoveID, Int64? WOSMoveID, Int64? BillID, Int64? CreditNoteID, char InvOrCreditNote, int RateCompID)
        {
            Entities.WOSBilling invObj = new Entities.WOSBilling();
            try
            {
                string QRSignedValue = string.Empty, BankQRValue = string.Empty;

                DataSet invoiceDt = WOSBillingDAL.GetWOSInvoiceDetailsById(LoginID, MoveID, WOSMoveID, BillID, CreditNoteID, InvOrCreditNote, RateCompID);

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
                    invObj.WOSMoveID = invoiceDt.Tables[0].Rows[0]["WOSMoveID"] == DBNull.Value ? 0 : Convert.ToInt32(invoiceDt.Tables[0].Rows[0]["WOSMoveID"]);
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
                    invObj.PreparedDate = invoiceDt.Tables[0].Rows[0]["PreparedDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(invoiceDt.Tables[0].Rows[0]["PreparedDate"]);
                    invObj.ApprovedDate = invoiceDt.Tables[0].Rows[0]["ApprovedDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(invoiceDt.Tables[0].Rows[0]["ApprovedDate"]);
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
                    invObj.RMCID = invoiceDt.Tables[0].Rows[0]["RMCID"] == DBNull.Value ? 0 : Convert.ToInt32(invoiceDt.Tables[0].Rows[0]["RMCID"]);
                    invObj.SunCost = invoiceDt.Tables[0].Rows[0]["SunCost"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(invoiceDt.Tables[0].Rows[0]["SunCost"]);
                    invObj.Is_SunCostShow = invoiceDt.Tables[0].Rows[0]["IsSunCostShow"] == DBNull.Value ? false : Convert.ToBoolean(invoiceDt.Tables[0].Rows[0]["IsSunCostShow"]);
                    if (false)//StrgInvID != null)
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

                    if (invoiceDt.Tables[1] != null && invoiceDt.Tables[1].Rows.Count > 0)
                    {
                        invObj.BillItems = invoiceDt.Tables[1].AsEnumerable().
                            Select(dataRow => new BillingItems
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
                            }).ToList();
                    }

                    if (invoiceDt.Tables[2] != null && invoiceDt.Tables[2].Rows.Count > 0)
                    {
                        invObj.Bank = invoiceDt.Tables[2].AsEnumerable().
                            Select(dataRow => new BankDetails
                            {
                                Header = Convert.ToString(dataRow["InfoHeading"]),
                                Value = Convert.ToString(dataRow["InfoValue"]),
                            }).ToList();
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

                            }).ToList().ToDictionary(ele => ele.EngInfo, ele => ele.ArabicInfo);



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

                    invObj.RMCBuss = true;//!(UserSession.GetUserSession().BussinessLine == "NON RMC-BUSINESS");

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
                throw new BussinessLogicException(Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "WOSBillingBL", "GetWOSInvoiceDetailsById", Properties.Resources.UnExpectedErrorAtBL, ex);
            }
            return invObj;
        }

        public bool SaveWOSInvoice(Entities.WOSBilling WOSInvoiceObj, string Status, bool IsAuditAmount, bool SaveTopOnly, out string result)
        {
            try
            {
                var BillItem = new XElement("InvoiceDetails",
                from emp in WOSInvoiceObj.BillItems
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
                               new XElement("AfterAuditAmt", IsAuditAmount ? emp.Amount : emp.AuditedAmount),
                               new XElement("TaxPer", emp.GSTVATPercent),
                               new XElement("SGSTAmt", emp.SGSTAmt),
                               new XElement("CGSTAmt", emp.CGSTAmt),
                               new XElement("IGSTAmt", emp.IGSTAmt),
                               new XElement("VatAmt", emp.VatAmt),
                               new XElement("TaxApp", emp.TaxApp),
                               new XElement("IsActive", true),
                               new XElement("BringToUnbill", emp.Unbill)
                           ));

                WOSInvoiceObj.InvTotalAmount = WOSInvoiceObj.BillItems.Sum(x => x.TotalAmount);

                return WOSBillingDAL.SaveWOSInvoice(WOSInvoiceObj, BillItem, Status, UserSession.GetUserSession().LoginID, SaveTopOnly, out result);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "WOSBillingBL", "SaveWOSInvoice", Properties.Resources.UnExpectedErrorAtBL, ex);
            }
        }

        public AddressList GetAddressDetials(Int64 WOSMoveID, string BillTo, char OrgorDest, int BillingEntityID = 0, char BillType = 'I')
        {
            try
            {
                AddressList AddressList = new AddressList();
                DataTable dt = WOSBillingDAL.GetAddressDetials(WOSMoveID, BillTo, OrgorDest, BillingEntityID, BillType);

                if (dt != null && dt.Rows.Count > 0)
                {
                    AddressList.Address1 = dt.Rows[0]["Address1"] == DBNull.Value ? null : Convert.ToString(dt.Rows[0]["Address1"]);
                    AddressList.Address2 = dt.Rows[0]["Address2"] == DBNull.Value ? null : Convert.ToString(dt.Rows[0]["Address2"]);
                    AddressList.CityID = dt.Rows[0]["CityID"] == DBNull.Value ? 0 : Convert.ToInt32(dt.Rows[0]["CityID"]);
                    AddressList.Pincode = dt.Rows[0]["PINCode"] == DBNull.Value ? null : Convert.ToString(dt.Rows[0]["PINCode"]);
                    AddressList.GSTNO = dt.Rows[0]["GSTNO"] == DBNull.Value ? null : Convert.ToString(dt.Rows[0]["GSTNO"]);
                }
                return AddressList;
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

        public IEnumerable<WOSInvoice> GetWOSTansferToFAList(WOSFundTranfer fundTranfer, string sort, string sortdir, int pageSize, out DataTable dt)
        {
            IQueryable<WOSInvoice> WOSInvoiceList;
            dt = null;
            try
            {
                DataSet ds = WOSBillingDAL.GetWOSTansferToFAList(fundTranfer);
                if (ds != null && ds.Tables.Count > 0)
                {
                    dt = ds.Tables[0];
                    List<WOSInvoice> result = (from rw in ds.Tables[0].AsEnumerable()
                                               select new WOSInvoice()
                                               {
                                                   Layout = rw["LAYOUT"] == DBNull.Value ? null : Convert.ToString(rw["LAYOUT"]),
                                                   BillNo = rw["SALES INVOICE NUMBER"] == DBNull.Value ? null : Convert.ToString(rw["SALES INVOICE NUMBER"]),
                                                   BillDate = rw["INVOICE DATE"] == DBNull.Value ? default(DateTime) : Convert.ToDateTime(rw["INVOICE DATE"]),
                                                   SalesDefinition = rw["SALES DEFINITION"] == DBNull.Value ? null : Convert.ToString(rw["SALES DEFINITION"]),
                                                   FAClientCode = rw["CUSTOMER CODE"] == DBNull.Value ? null : Convert.ToString(rw["CUSTOMER CODE"]),
                                                   CustomerReference = rw["CUSTOMER REFERENCE"] == DBNull.Value ? null : Convert.ToString(rw["CUSTOMER REFERENCE"]),
                                                   Comment = rw["COMMENT"] == DBNull.Value ? null : Convert.ToString(rw["COMMENT"]),
                                                   FromDate = rw["FROM DATE"] == DBNull.Value ? null : Convert.ToString(rw["FROM DATE"]),
                                                   ToDate = rw["TO DATE"] == DBNull.Value ? null : Convert.ToString(rw["TO DATE"]),
                                                   LineNo = rw["LINE NUMBER"] == DBNull.Value ? null : Convert.ToString(rw["LINE NUMBER"]),
                                                   ItemCode = rw["ITEM CODE"] == DBNull.Value ? null : Convert.ToString(rw["ITEM CODE"]),
                                                   Description = rw["DESCRIPTION"] == DBNull.Value ? null : Convert.ToString(rw["DESCRIPTION"]),
                                                   UnitofSale = rw["UNIT OF SALE"] == DBNull.Value ? null : Convert.ToString(rw["UNIT OF SALE"]),
                                                   Qty = rw["QTY"] == DBNull.Value ? null : Convert.ToString(rw["QTY"]),
                                                   Rate = rw["RATE"] == DBNull.Value ? null : Convert.ToString(rw["RATE"]),
                                                   Currency = rw["CURRENCY"] == DBNull.Value ? null : Convert.ToString(rw["CURRENCY"]),
                                                   Value = rw["VALUE"] == DBNull.Value ? null : Convert.ToString(rw["VALUE"]),
                                                   CGST = rw["CGST"] == DBNull.Value ? null : Convert.ToString(rw["CGST"]),
                                                   SGST = rw["SGST"] == DBNull.Value ? null : Convert.ToString(rw["SGST"]),
                                                   IGST = rw["IGST"] == DBNull.Value ? null : Convert.ToString(rw["IGST"]),
                                                   FACode = rw["SBU BUSINESS LINE PRODUCTS"] == DBNull.Value ? null : Convert.ToString(rw["SBU BUSINESS LINE PRODUCTS"]),
                                                   Location = rw["LOCATION"] == DBNull.Value ? null : Convert.ToString(rw["LOCATION"]),
                                                   Project = rw["ID JOB NO PROJECT REVENUE BRANCH"] == DBNull.Value ? null : Convert.ToString(rw["ID JOB NO PROJECT REVENUE BRANCH"]),
                                                   Miscellaneous = rw["MISCELLANEOUS"] == DBNull.Value ? null : Convert.ToString(rw["MISCELLANEOUS"]),
                                                   TaxCode = rw["TAX CODE"] == DBNull.Value ? null : Convert.ToString(rw["TAX CODE"]),
                                                   MISLocation = rw["MIS LOCATION"] == DBNull.Value ? null : Convert.ToString(rw["MIS LOCATION"]),
                                                   Employee = rw["FUNCTION EMPLOYEE"] == DBNull.Value ? null : Convert.ToString(rw["FUNCTION EMPLOYEE"]),
                                                   AccountCode = rw["ID PAID TO RECEIVED FROM 2ND LEG"] == DBNull.Value ? null : Convert.ToString(rw["ID PAID TO RECEIVED FROM 2ND LEG"]),
                                                   GSTFlag = rw["GSTFlag"] == DBNull.Value ? null : Convert.ToString(rw["GSTFlag"]),
                                                   CBSRefID = rw["CBSRefID"] == DBNull.Value ? null : Convert.ToString(rw["CBSRefID"]),
                                                   InvOrCredit = rw["InvOrCredit"] == DBNull.Value ? null : Convert.ToString(rw["InvOrCredit"]),
                                                   BillTo = rw["BillTo"] == DBNull.Value ? null : Convert.ToString(rw["BillTo"]),
                                               }).ToList();
                    WOSInvoiceList = result.AsQueryable();

                    return WOSInvoiceList.ToList();
                }
                else
                {
                    return new List<WOSInvoice>();
                }
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "WOSBillingBL", "GetWOSTansferToFAList", Properties.Resources.UnExpectedErrorAtBL, ex);
            }
        }

        public bool InsertWOSTransferToFA(WOSFundTranfer fundTranfer, out string result)
        {
            try
            {
                return WOSBillingDAL.InsertWOSTransferToFA(fundTranfer, UserSession.GetUserSession().LoginID, out result);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "WOSBillingBL", "InsertWOSTransferToFA", Properties.Resources.UnExpectedErrorAtBL, ex);
            }
        }

        public IEnumerable<WOSInvoice> GetWOSTansferToFAGCCList(WOSFundTranfer fundTranfer, string sort, string sortdir, int pageSize, out DataTable dt)
        {
            IQueryable<WOSInvoice> WOSInvoiceList;
            dt = null;
            try
            {
                DataSet ds = WOSBillingDAL.GetWOSTansferToFAGCCList(fundTranfer);
                if (ds != null && ds.Tables.Count > 0)
                {
                    dt = ds.Tables[0];
                    List<WOSInvoice> result = (from rw in ds.Tables[0].AsEnumerable()
                                               select new WOSInvoice()
                                               {
                                                   Layout = Convert.ToString(rw["Layout"]),
                                                   BillNo = Convert.ToString(rw["Transaction Reference"]),
                                                   BillDate = rw["Transaction Date"] == DBNull.Value ? default(DateTime) : Convert.ToDateTime(rw["Transaction Date"]),
                                                   Account = Convert.ToString(rw["Transaction Reference"]),
                                                   FAClientCode = rw["COA"] == DBNull.Value ? null : Convert.ToString(rw["COA"]),
                                                   AccountCode = rw["Account Code"] == DBNull.Value ? null : Convert.ToString(rw["Account Code"]),
                                                   FACode = rw["SBU_BL_PRD"] == DBNull.Value ? null : Convert.ToString(rw["SBU_BL_PRD"]),
                                                   Description = rw["Description"] == DBNull.Value ? null : Convert.ToString(rw["Description"]),
                                                   JobNo = rw["JOB"] == DBNull.Value ? null : Convert.ToString(rw["JOB"]),
                                                   Amount = rw["Transaction Amount"] == DBNull.Value ? null : Convert.ToString(rw["Transaction Amount"]),
                                                   Currency = rw["Currency Code"] == DBNull.Value ? null : Convert.ToString(rw["Currency Code"]),
                                                   Credit_Debit_Marker = rw["Debit/Credit marker"] == DBNull.Value ? null : Convert.ToString(rw["Debit/Credit marker"]),
                                                   CBSRefID = rw["CBSRefID"] == DBNull.Value ? null : Convert.ToString(rw["CBSRefID"]),
                                                   InvOrCredit = rw["InvOrCredit"] == DBNull.Value ? null : Convert.ToString(rw["InvOrCredit"]),
                                                   BillTo = rw["BillTo"] == DBNull.Value ? null : Convert.ToString(rw["BillTo"])
                                               }).ToList();
                    WOSInvoiceList = result.AsQueryable();

                    return WOSInvoiceList.ToList();
                }
                else
                {
                    return new List<WOSInvoice>();
                }
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "WOSBillingBL", "GetWOSTansferToFAGCCList", Properties.Resources.UnExpectedErrorAtBL, ex);
            }
        }

        public DataTable GetDubaiTAExport(Entities.WOSFundTranfer model, DataTable ExportDt, out string message)
        {
            bool res = false;
            try
            {
                if (ExportDt == null || ExportDt.Rows.Count <= 0)
                {
                    message = "No records to download";
                    return ExportDt;
                }

                if (string.IsNullOrWhiteSpace(model.SearchFor))
                {
                    message = "Status is required.";
                    return ExportDt;
                }
                ExportDt.CaseSensitive = false;
                if (model.SearchFor.ToUpper().Equals("FINALIZED"))
                {
                    var billitem = new XElement("SelectedLists",
                                        from emp in model.InvGrid.Where(x => x.IsExport == true && !string.IsNullOrWhiteSpace(x.CBSRefID))
                                        select new XElement("SelectedList",
                                                       new XElement("CBSRefID", emp.CBSRefID),
                                                       new XElement("InvOrCredit", emp.InvOrCredit),
                                                       new XElement("AccountCode", emp.AccountCode)//,
                                                                                                   //new XElement("FAClientCode", emp.FAClientCode)
                                                   ));

                    res = WOSBillingDAL.InsertWOSTransferToFA(model, UserSession.GetUserSession().LoginID, out message);//fundtranferDAL.InsertTransferFA(billitem, UserSession.GetUserSession().LoginID, out message);
                }
                else
                {
                    res = true;
                    message = "Data Exported Successfully.";
                }
                List<string> items = model.InvGrid.Where(x => x.IsExport == true).Select(x => x.BillNo).ToList();

                var rows = ExportDt.AsEnumerable().Where(row => items.Contains(row.Field<string>("TRANSACTION REFERENCE")));

                if (rows.Any())
                {
                    ExportDt = new DataTable();
                    ExportDt = rows.CopyToDataTable();

                    if (ExportDt.Columns.Contains("InvOrCredit"))
                        ExportDt.Columns.Remove("InvOrCredit");

                    if (ExportDt.Columns.Contains("CBSRefID"))
                        ExportDt.Columns.Remove("CBSRefID");

                    if (ExportDt.Columns.Contains("BillTo"))
                        ExportDt.Columns.Remove("BillTo");

                    if (ExportDt.Columns.Contains("SrNo"))
                        ExportDt.Columns.Remove("SrNo");

                }

                return ExportDt;
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "FundTransferBL", "GetDubaiTAExport", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }
        }
    }
}