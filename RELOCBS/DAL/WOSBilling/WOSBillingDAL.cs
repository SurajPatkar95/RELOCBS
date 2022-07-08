using RELOCBS.App_Code;
using RELOCBS.Common.ExceptionHandling;
using RELOCBS.Utility;
using System;
using System.Data;
using System.Linq;
using System.Xml.Linq;

namespace RELOCBS.DAL.WOSBilling
{
    public class WOSBillingDAL
    {
        private CommonSubs _CSubs;
        public CommonSubs CSubs
        {
            get
            {
                if (_CSubs == null)
                    _CSubs = new CommonSubs();
                return _CSubs;
            }
        }

        public DataSet GetWOSInvoiceForGrid(int LoginID, DateTime? FromDate, DateTime? ToDate, string search, string searchtype, int? InvoiceID, string Shipper, string Status, char Type)
        {
            DataSet WOSInvoiceListDs = null;
            try
            {
                string query = string.Format("exec [WOS].[GetInvForGrid] @SP_LoginID={0}, @SP_InvoiceID={1}, @SP_InvStatus={2}, @SP_Type={3}, @SP_Shipper={4}, @SP_FilterName={5}, @SP_FilterValue={6}",
                    Convert.ToString(LoginID), CSubs.QSafeValue(Convert.ToString(InvoiceID)), CSubs.QSafeValue(Status), CSubs.QSafeValue(Type.ToString()),
                    CSubs.QSafeValue(Shipper), CSubs.QSafeValue(searchtype), CSubs.QSafeValue(search));

                WOSInvoiceListDs = CSubs.GetDataSet(query);
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoginID), "WOSBillingDAL", "GetWOSInvoiceForGrid", Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
            return WOSInvoiceListDs;
        }

        public DataSet GetWOSInvoiceDetailsById(int LoginID, Int64? MoveID, Int64? WOSMoveID, Int64? BillID, Int64? CreditNoteID, char InvOrCreditNote, int? RateCompID)
        {
            DataSet WOSInvoiceDetailsDs = new DataSet();
            try
            {
                string query = string.Format("EXEC [WOS].[GetWOSInvoiceDetails] @SP_InvoiceID={0}, @SP_CreditNoteID={1}, @SP_MoveID={2}, @SP_WOSMoveID={3}, @RateCompID={4}, @SP_LoginID={5}, @SP_InvOrCreditNote={6}",
                    CSubs.QSafeValue(Convert.ToString(BillID)), CSubs.QSafeValue(Convert.ToString(CreditNoteID)), CSubs.QSafeValue(Convert.ToString(MoveID)), CSubs.QSafeValue(Convert.ToString(WOSMoveID)),
                    CSubs.QSafeValue(Convert.ToString(RateCompID)), CSubs.QSafeValue(Convert.ToString(LoginID)), CSubs.QSafeValue(Convert.ToString(InvOrCreditNote)));

                WOSInvoiceDetailsDs = CSubs.GetDataSet(query);
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(UserSession.GetUserSession().LoginID), "WOSBillingDAL", "GetWOSInvoiceDetailsById", Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
            return WOSInvoiceDetailsDs;
        }

        public bool SaveWOSInvoice(Entities.WOSBilling WOSBillingObj, XElement Billitems, string status, int LoginID, bool SaveTopOnly, out string result)
        {
            result = string.Empty;
            try
            {
                string BillItemsString = Billitems.HasElements ? Convert.ToString(Billitems) : null;

                using (CDALSQL conn = new CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {
                        conn.AddCommand("[WOS].[AddEditWOSInvoice]", QueryType.Procedure);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_WOSMoveID", SqlDbType.BigInt, 0, ParameterDirection.Input, WOSBillingObj.WOSMoveID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_MoveID", SqlDbType.BigInt, 0, ParameterDirection.Input, WOSBillingObj.MoveID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_InvID", SqlDbType.BigInt, 0, ParameterDirection.InputOutput, WOSBillingObj.BillID == null ? 0 : WOSBillingObj.BillID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_InvDate", SqlDbType.DateTime, 0, ParameterDirection.Input, WOSBillingObj.InvoiceDate == null && WOSBillingObj.InvoiceStatus == "Approved" ? DateTime.Now : WOSBillingObj.InvoiceDate);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CurrID", SqlDbType.BigInt, 0, ParameterDirection.Input, WOSBillingObj.RateCurrancyID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ConvrRate", SqlDbType.Float, 0, ParameterDirection.Input, WOSBillingObj.ConvRate);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_BillTo", SqlDbType.VarChar, 10, ParameterDirection.Input, WOSBillingObj.BillToID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_BillToOrgOrDest", SqlDbType.VarChar, 1, ParameterDirection.Input, WOSBillingObj.AddressType);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LoginID", SqlDbType.BigInt, 0, ParameterDirection.Input, UserSession.GetUserSession().LoginID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_AttentionFName", SqlDbType.VarChar, 100, ParameterDirection.Input, WOSBillingObj.Attention);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_AttentionLName", SqlDbType.VarChar, 30, ParameterDirection.Input, null);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Address1", SqlDbType.VarChar, 200, ParameterDirection.Input, WOSBillingObj.Address1);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Address2", SqlDbType.VarChar, 200, ParameterDirection.Input, WOSBillingObj.Address2);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_EmailID", SqlDbType.VarChar, 50, ParameterDirection.Input, WOSBillingObj.Email);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CityID", SqlDbType.Int, 0, ParameterDirection.Input, WOSBillingObj.CityID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_PinCode", SqlDbType.VarChar, 10, ParameterDirection.Input, WOSBillingObj.PinCode);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Phone", SqlDbType.VarChar, 30, ParameterDirection.Input, WOSBillingObj.Phone);

                        //conn.AddParameter(ParameterOF.PROCEDURE, "@SP_OrgStgStartDt", SqlDbType.DateTime, 0, ParameterDirection.Input, WOSBillingObj.OrgStorageStart);
                        //conn.AddParameter(ParameterOF.PROCEDURE, "@SP_OrgStgEndDt", SqlDbType.DateTime, 0, ParameterDirection.Input, WOSBillingObj.OrgStorageEnd);
                        //conn.AddParameter(ParameterOF.PROCEDURE, "@SP_DestStgStartDt", SqlDbType.DateTime, 0, ParameterDirection.Input, WOSBillingObj.DestStorageStart);
                        //conn.AddParameter(ParameterOF.PROCEDURE, "@SP_DestStgEndDt", SqlDbType.DateTime, 0, ParameterDirection.Input, WOSBillingObj.DestStorageEnd);
                        //conn.AddParameter(ParameterOF.PROCEDURE, "@SP_OrgWHStateID", SqlDbType.Int, 0, ParameterDirection.Input, WOSBillingObj.OrgStorageState);
                        //conn.AddParameter(ParameterOF.PROCEDURE, "@SP_DestWHStateID", SqlDbType.Int, 0, ParameterDirection.Input, WOSBillingObj.DestStorageState);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_BillToClientID", SqlDbType.Int, 0, ParameterDirection.Input, WOSBillingObj.BillToClientID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_BillToAccountID", SqlDbType.Int, 0, ParameterDirection.Input, WOSBillingObj.BillToAccountID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ShipperName", SqlDbType.VarChar, 100, ParameterDirection.Input, WOSBillingObj.BillToShipperName);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Remarks", SqlDbType.VarChar, 250, ParameterDirection.Input, null);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_SaveTopOnly", SqlDbType.Bit, 0, ParameterDirection.Input, SaveTopOnly);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_InvStatus", SqlDbType.VarChar, 20, ParameterDirection.Input, status);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@RETURNSTATUS", SqlDbType.SmallInt, 0, ParameterDirection.ReturnValue);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@Out_Message", SqlDbType.NVarChar, 500, ParameterDirection.Output);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_InvDetails", SqlDbType.Xml, 0, ParameterDirection.Input, BillItemsString);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_RemarksOnStatus", SqlDbType.VarChar, 1000, ParameterDirection.Input, WOSBillingObj.Remark);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_TaxType", SqlDbType.VarChar, 10, ParameterDirection.Input, WOSBillingObj.TaxType);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_InvOrCreditNote", SqlDbType.VarChar, 10, ParameterDirection.Input, WOSBillingObj.BillType);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CreditNoteID", SqlDbType.BigInt, 0, ParameterDirection.InputOutput, WOSBillingObj.CreditNoteID == null ? 0 : WOSBillingObj.CreditNoteID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_AEDConversionRate", SqlDbType.Float, 0, ParameterDirection.Input, WOSBillingObj.AEDCurr);
                        //conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IsGrossWt", SqlDbType.Bit, 0, ParameterDirection.Input, WOSBillingObj.IsGrossWtValue);
                        //conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IsGrossVol", SqlDbType.Bit, 0, ParameterDirection.Input, WOSBillingObj.IsGrossVolumeValue);
                        //conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IsNetWt", SqlDbType.Bit, 0, ParameterDirection.Input, WOSBillingObj.IsWtValue);
                        //conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IsNetVol", SqlDbType.Bit, 0, ParameterDirection.Input, WOSBillingObj.IsVolumeValue);
                        //conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IsGoodDesc", SqlDbType.Bit, 0, ParameterDirection.Input, WOSBillingObj.IsGoodsDesc);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_chequeNo", SqlDbType.VarChar, 50, ParameterDirection.Input, WOSBillingObj.chequeNo);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_BillRemarks", SqlDbType.VarChar, 100, ParameterDirection.Input, WOSBillingObj.BillRemarks);
                        //conn.AddParameter(ParameterOF.PROCEDURE, "@SP_VehicleNo", SqlDbType.VarChar, 50, ParameterDirection.Input, WOSBillingObj.VehicleNo);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_AdvanceRecv", SqlDbType.Float, 0, ParameterDirection.Input, WOSBillingObj.AdvanceRecv);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_BillSubDate", SqlDbType.DateTime, 0, ParameterDirection.Input, WOSBillingObj.BillSubDate);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_BillAcknowledgement", SqlDbType.VarChar, 50, ParameterDirection.Input, WOSBillingObj.BillAcknowledgement);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_BillGSTNo", SqlDbType.VarChar, 20, ParameterDirection.Input, WOSBillingObj.Other.GSTNo);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ReferenceNo", SqlDbType.VarChar, 100, ParameterDirection.Input, WOSBillingObj.FileNo);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_NoofPkgs", SqlDbType.BigInt, 0, ParameterDirection.Input, WOSBillingObj.NoofPkgs);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_MoneyReceivedDate", SqlDbType.Date, 0, ParameterDirection.Input, WOSBillingObj.MoneyReceivedDate);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ParsifalAuditStartDate", SqlDbType.Date, 0, ParameterDirection.Input, WOSBillingObj.ParsifalAuditStartDate);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ParsifalApproveDate", SqlDbType.Date, 0, ParameterDirection.Input, WOSBillingObj.ParsifalApproveDate);

                        //if (WOSBillingObj.StrgInvID != null)
                        //{
                        //    conn.AddParameter(ParameterOF.PROCEDURE, "@SP_StrgJobMasterID", SqlDbType.BigInt, 0, ParameterDirection.Input, WOSBillingObj.StrgJobID);
                        //    conn.AddParameter(ParameterOF.PROCEDURE, "@SP_StrgInvMasterID", SqlDbType.BigInt, 0, ParameterDirection.Input, WOSBillingObj.StrgInvID);
                        //    conn.AddParameter(ParameterOF.PROCEDURE, "@SP_SubjectInfo", SqlDbType.VarChar, -1, ParameterDirection.Input, WOSBillingObj.Subject);
                        //    conn.AddParameter(ParameterOF.PROCEDURE, "@SP_NoteInfo", SqlDbType.VarChar, -1, ParameterDirection.Input, WOSBillingObj.Note);
                        //    conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ShowSubInInv", SqlDbType.Bit, 0, ParameterDirection.Input, WOSBillingObj.IsSubject);
                        //    conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ShowNoteInInv", SqlDbType.Bit, 0, ParameterDirection.Input, WOSBillingObj.IsNote);
                        //    conn.AddParameter(ParameterOF.PROCEDURE, "@SP_WtVolID", SqlDbType.Int, -1, ParameterDirection.Input, WOSBillingObj.StrgVolUnitID);
                        //    conn.AddParameter(ParameterOF.PROCEDURE, "@SP_WtVolValue", SqlDbType.Float, -1, ParameterDirection.Input, WOSBillingObj.StrgVolValue);
                        //}

                        //conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ServiceID", SqlDbType.Int, -1, ParameterDirection.Input, WOSBillingObj.BtrService);
                        //conn.AddParameter(ParameterOF.PROCEDURE, "@SP_PaymentTerm", SqlDbType.Int, -1, ParameterDirection.Input, WOSBillingObj.BtrPaymentTerm);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IsCollectionDate", SqlDbType.Bit, -1, ParameterDirection.Input, WOSBillingObj.IsCollectionDate);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_BillingEntityID", SqlDbType.Int, -1, ParameterDirection.Input, WOSBillingObj.BillingEntity);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CreditNoteEntityID", SqlDbType.Int, -1, ParameterDirection.Input, WOSBillingObj.CreditNoteEntity);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_BillAddInfo", SqlDbType.VarChar, 50, ParameterDirection.Input, WOSBillingObj.BillAddInfo);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_InvTotalAmount", SqlDbType.Decimal, 0, ParameterDirection.Input, WOSBillingObj.InvTotalAmount);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_SunCost", SqlDbType.Decimal, 0, ParameterDirection.Input, WOSBillingObj.SunCost);
                        conn.ExecuteProcedure(ProcedureReturnType.SingleValue);

                        if (!conn.IsError)
                        {
                            int ReturnStatus = Convert.ToInt32(conn.GetParameterValue(ParameterOF.PROCEDURE, "@RETURNSTATUS"));
                            result = Convert.ToString(conn.GetParameterValue(ParameterOF.PROCEDURE, "@Out_Message"));

                            if (ReturnStatus == 0)
                            {
                                if (WOSBillingObj.BillType == 'I')
                                {
                                    WOSBillingObj.BillID = Convert.ToInt32(conn.GetParameterValue(ParameterOF.PROCEDURE, "@SP_InvID"));
                                }
                                else if (WOSBillingObj.BillType == 'C')
                                {
                                    WOSBillingObj.CreditNoteID = Convert.ToInt32(conn.GetParameterValue(ParameterOF.PROCEDURE, "@SP_CreditNoteID"));
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
                throw new DataAccessException(Convert.ToString(LoginID), "WOSBillingDAL", "SaveWOSInvoice", Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
        }

        public DataTable GetAddressDetials(Int64 WOSMoveID, string BillTo, char OrgorDest, int BillingEntityID, char BillType)
        {
            DataTable AddressDt = new DataTable();
            try
            {
                string query = string.Format("EXEC [WOS].[GetAddressForInvoice] @SP_WOSMoveID={0}, @SP_BillTo={1}, @SP_BillToOrgOrDest={2}, @SP_BillingEntityID={3}, @SP_BillType={4}",
                    CSubs.QSafeValue(Convert.ToString(WOSMoveID)), CSubs.QSafeValue(BillTo), CSubs.QSafeValue(Convert.ToString(OrgorDest)), CSubs.QSafeValue(Convert.ToString(BillingEntityID)), CSubs.QSafeValue(Convert.ToString(BillType)));

                AddressDt = CSubs.GetDataTable(query);
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(UserSession.GetUserSession().LoginID), "WOSBillingDAL", "GetAddressDetials", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

            return AddressDt;
        }

        public DataSet GetWOSTansferToFAList(Entities.WOSFundTranfer fa)
        {
            DataSet ds = null;
            int LoginID = UserSession.GetUserSession().LoginID;
            int CompanyID = UserSession.GetUserSession().CompanyID;
            bool RMCBuss = !(UserSession.GetUserSession().BussinessLine == "NON RMC-BUSINESS");
            try
            {
                string RevenueBranchXml = null;
                if (fa.RevenueBranchId != null)
                {
                    XElement xmlElements = new XElement("root", fa.RevenueBranchId.Select(x => new XElement("RevenueBranchIds", new XElement("RevenueBranchId", x))));
                    RevenueBranchXml = xmlElements.ToString();
                }

                string ServiceLineXml = null;
                if (fa.ServiceLineId != null)
                {
                    XElement xmlElements = new XElement("root", fa.ServiceLineId.Select(x => new XElement("ServiceLineIds", new XElement("ServiceLineId", x))));
                    ServiceLineXml = xmlElements.ToString();
                }

                using (CDALSQL conn = new CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {
                        conn.AddCommand("[WOS].[GetWOSTransferToFAForGrid]", QueryType.Procedure);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LoginID", SqlDbType.Int, 0, ParameterDirection.Input, LoginID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CompId", SqlDbType.Int, 0, ParameterDirection.Input, CompanyID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_FromDate", SqlDbType.DateTime, 0, ParameterDirection.Input, fa.FromDate);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ToDate", SqlDbType.DateTime, 0, ParameterDirection.Input, fa.ToDate);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_BillNo", SqlDbType.VarChar, 20, ParameterDirection.Input, fa.BillNo);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Status", SqlDbType.VarChar, 30, ParameterDirection.Input, fa.Status);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_RevenueBranchXml", SqlDbType.Xml, 0, ParameterDirection.Input, RevenueBranchXml);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ServiceLineXml", SqlDbType.Xml, 0, ParameterDirection.Input, ServiceLineXml);

                        ds = (DataSet)conn.ExecuteProcedure(ProcedureReturnType.DataSet);
                    }
                    else
                        throw new Exception(conn.ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoginID), "WOSBillingDAL", "GetWOSTansferToFAList", Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
            return ds;
        }

        public bool InsertWOSTransferToFA(Entities.WOSFundTranfer fundTranfer, int LoginID, out string result)
        {
            result = string.Empty;
            try
            {
                XElement BillItems = new XElement("SelectedLists",
                    from emp in fundTranfer.WOSInvoiceList
                    select new XElement("SelectedList",
                    new XElement("CBSRefID", emp.CBSRefID),
                    new XElement("InvOrCredit", emp.InvOrCredit),
                    new XElement("AccountCode", emp.AccountCode)));


                string BillItemsString = BillItems.HasElements ? Convert.ToString(BillItems) : null;

                using (CDALSQL conn = new CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {
                        conn.AddCommand("[WOS].[AddEditWOSTransferToFA]", QueryType.Procedure);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LoginID", SqlDbType.BigInt, 0, ParameterDirection.Input, LoginID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_SelectedList", SqlDbType.Xml, 0, ParameterDirection.Input, BillItemsString);
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
                throw new DataAccessException(Convert.ToString(LoginID), "WOSBillingDAL", "InsertWOSTransferToFA", Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
        }

        public DataSet GetWOSTansferToFAGCCList(Entities.WOSFundTranfer fa)
        {
            DataSet ds = null;
            int LoginID = UserSession.GetUserSession().LoginID;
            int CompanyID = UserSession.GetUserSession().CompanyID;
            bool RMCBuss = !(UserSession.GetUserSession().BussinessLine == "NON RMC-BUSINESS");
            try
            {
                string RevenueBranchXml = null;
                if (fa.RevenueBranchId != null)
                {
                    XElement xmlElements = new XElement("root", fa.RevenueBranchId.Select(x => new XElement("RevenueBranchIds", new XElement("RevenueBranchId", x))));
                    RevenueBranchXml = xmlElements.ToString();
                }

                string ServiceLineXml = null;
                if (fa.ServiceLineId != null)
                {
                    XElement xmlElements = new XElement("root", fa.ServiceLineId.Select(x => new XElement("ServiceLineIds", new XElement("ServiceLineId", x))));
                    ServiceLineXml = xmlElements.ToString();
                }

                using (CDALSQL conn = new CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {
                        conn.AddCommand("[WOS].[GetWOSTransferToFAGCCForGrid]", QueryType.Procedure);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LoginID", SqlDbType.Int, 0, ParameterDirection.Input, LoginID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CompId", SqlDbType.Int, 0, ParameterDirection.Input, CompanyID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_FromDate", SqlDbType.DateTime, 0, ParameterDirection.Input, fa.FromDate);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ToDate", SqlDbType.DateTime, 0, ParameterDirection.Input, fa.ToDate);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_BillNo", SqlDbType.VarChar, 20, ParameterDirection.Input, fa.BillNo);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Status", SqlDbType.VarChar, 30, ParameterDirection.Input, fa.Status);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_RevenueBranchXml", SqlDbType.Xml, 0, ParameterDirection.Input, RevenueBranchXml);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ServiceLineXml", SqlDbType.Xml, 0, ParameterDirection.Input, ServiceLineXml);

                        ds = (DataSet)conn.ExecuteProcedure(ProcedureReturnType.DataSet);
                    }
                    else
                        throw new Exception(conn.ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoginID), "WOSBillingDAL", "GetWOSTansferToFAGCCList", Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
            return ds;
        }
    }
}