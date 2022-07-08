using Newtonsoft.Json;
using RELOCBS.App_Code;
using RELOCBS.Common.ExceptionHandling;
using RELOCBS.Utility;
using System;
using System.Data;
using System.Linq;
using System.Xml.Linq;

namespace RELOCBS.DAL.DebitNote
{
    public class DebitNoteDAL
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

        public DataSet GetDebitNoteList(DateTime? FromDate, DateTime? ToDate, string SearchType, string SearchValue, Int64? DebitNoteId, string DrOrCrNote)
        {
            DataSet DebitNotesDs = null;
            try
            {
                string query = string.Format("EXEC [Inv].[GetDebitNotesForGrid] @SP_LoginID={0}, @SP_DebitNoteFromDate={1}, @SP_DebitNoteToDate={2}, @SP_SearchType={3}, " +
                    "@SP_SearchValue={4}, @SP_DebitNoteId={5}, @SP_DrOrCrNote={6}",
                UserSession.GetUserSession().LoginID, CSubs.QSafeValue(Convert.ToString(FromDate)), CSubs.QSafeValue(Convert.ToString(ToDate)),
                CSubs.QSafeValue(Convert.ToString(SearchType)), CSubs.QSafeValue(Convert.ToString(SearchValue)), CSubs.QSafeValue(Convert.ToString(DebitNoteId)),
                CSubs.QSafeValue(Convert.ToString(DrOrCrNote)));

                DebitNotesDs = CSubs.GetDataSet(query);
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(UserSession.GetUserSession().LoginID), "DebitNoteDAL", "GetDebitNoteList", Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
            return DebitNotesDs;
        }

        public DataSet GetDebitNoteDetailsById(Int64 DebitNoteId, Int64 DNCreditNoteId, string DrOrCrNote)
        {
            DataSet DebitNoteDetailsDs = new DataSet();
            try
            {
                string query = string.Format("EXEC [Inv].[GetDebitNoteDetailsForDisplay] @SP_LoginID={0}, @SP_DebitNoteId={1}, @SP_DNCreditNoteId={2}, @SP_DrOrCrNote={3}",
                UserSession.GetUserSession().LoginID, CSubs.QSafeValue(Convert.ToString(DebitNoteId)), CSubs.QSafeValue(Convert.ToString(DNCreditNoteId)), CSubs.QSafeValue(Convert.ToString(DrOrCrNote)));
                DebitNoteDetailsDs = CSubs.GetDataSet(query);
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(UserSession.GetUserSession().LoginID), "DebitNoteDAL", "GetDebitNoteDetailsById", Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
            return DebitNoteDetailsDs;
        }

        public bool SaveDebitNoteDetails(Entities.DebitNote debitNote, string Status, string DrOrCrNote, out string result)
        {
            result = string.Empty;
            try
            {
                using (CDALSQL conn = new CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {
                        XNode node = JsonConvert.DeserializeXNode(debitNote.DebitNoteDetailsListHidden, "DebitNoteDetails");

                        string DebitNoteDetailsXml = node.ToString();

                        conn.AddCommand("[Inv].[AddEditDebitNote]", QueryType.Procedure);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LoginID", SqlDbType.Int, 0, ParameterDirection.Input, UserSession.GetUserSession().LoginID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_DrOrCrNote", SqlDbType.VarChar, 1, ParameterDirection.Input, DrOrCrNote);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_DebitNoteId", SqlDbType.BigInt, 0, ParameterDirection.InputOutput, debitNote.DebitNoteId);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_DNCreditNoteId", SqlDbType.BigInt, 0, ParameterDirection.InputOutput, debitNote.DNCreditNoteId);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_SBUId", SqlDbType.BigInt, 0, ParameterDirection.Input, debitNote.SBUId);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_RevenueBrId", SqlDbType.Int, 0, ParameterDirection.Input, debitNote.RevenueBrIdHidden);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_DNTypeId", SqlDbType.Int, 0, ParameterDirection.Input, debitNote.DNTypeId);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_POSStateId", SqlDbType.Int, 0, ParameterDirection.Input, debitNote.Debtor.POSStateId);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_TaxType", SqlDbType.VarChar, 25, ParameterDirection.Input, debitNote.TaxType);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_InvType", SqlDbType.VarChar, 25, ParameterDirection.Input, debitNote.InvType);

                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_DebtorId", SqlDbType.Int, 0, ParameterDirection.Input, debitNote.Debtor.DebtorId);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_DebtorName", SqlDbType.VarChar, 200, ParameterDirection.Input, debitNote.Debtor.DebtorName);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Address1", SqlDbType.VarChar, 200, ParameterDirection.Input, debitNote.Debtor.Address1);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Address2", SqlDbType.VarChar, 200, ParameterDirection.Input, debitNote.Debtor.Address2);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CityID", SqlDbType.Int, 0, ParameterDirection.Input, debitNote.Debtor.CityIDHidden);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_PINCode", SqlDbType.VarChar, 20, ParameterDirection.Input, debitNote.Debtor.PINCode);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_GSTNo", SqlDbType.VarChar, 25, ParameterDirection.Input, debitNote.Debtor.GSTNo);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_PANNo", SqlDbType.VarChar, 25, ParameterDirection.Input, debitNote.Debtor.PANNo);

                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Status", SqlDbType.VarChar, 50, ParameterDirection.Input, Status);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_DebitNoteDetailsXML", SqlDbType.Xml, 0, ParameterDirection.Input, DebitNoteDetailsXml);

                        conn.AddParameter(ParameterOF.PROCEDURE, "@RETURNSTATUS", SqlDbType.SmallInt, 0, ParameterDirection.ReturnValue);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@Out_Message", SqlDbType.NVarChar, 500, ParameterDirection.Output);
                        conn.ExecuteProcedure(ProcedureReturnType.SingleValue);

                        if (!conn.IsError)
                        {
                            int ReturnStatus = Convert.ToInt32(conn.GetParameterValue(ParameterOF.PROCEDURE, "@RETURNSTATUS"));
                            result = Convert.ToString(conn.GetParameterValue(ParameterOF.PROCEDURE, "@Out_Message"));

                            if (ReturnStatus == 0)
                            {
                                debitNote.DebitNoteId = Convert.ToInt64(conn.GetParameterValue(ParameterOF.PROCEDURE, "@SP_DebitNoteId"));
                                debitNote.DNCreditNoteId = Convert.ToInt64(conn.GetParameterValue(ParameterOF.PROCEDURE, "@SP_DNCreditNoteId"));
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
                throw new DataAccessException(Convert.ToString(UserSession.GetUserSession().LoginID), "DebitNoteDAL", "SaveDebitNoteDetails", Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
        }

        public bool DeleteDebitNote(Int64 DebitNoteId, Int64 DNCreditNoteId, string DrOrCrNote, out string result)
        {
            result = string.Empty;
            try
            {
                using (CDALSQL conn = new CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {
                        conn.AddCommand("[Inv].[DeleteDebitNote]", QueryType.Procedure);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LoginID", SqlDbType.Int, 0, ParameterDirection.Input, UserSession.GetUserSession().LoginID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_DrOrCrNote", SqlDbType.VarChar, 1, ParameterDirection.Input, DrOrCrNote);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_DebitNoteId", SqlDbType.BigInt, 0, ParameterDirection.InputOutput, DebitNoteId);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_DNCreditNoteId", SqlDbType.BigInt, 0, ParameterDirection.InputOutput, DNCreditNoteId);

                        conn.AddParameter(ParameterOF.PROCEDURE, "@RETURNSTATUS", SqlDbType.SmallInt, 0, ParameterDirection.ReturnValue);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@Out_Message", SqlDbType.NVarChar, 500, ParameterDirection.Output);
                        conn.ExecuteProcedure(ProcedureReturnType.SingleValue);

                        if (!conn.IsError)
                        {
                            int ReturnStatus = Convert.ToInt32(conn.GetParameterValue(ParameterOF.PROCEDURE, "@RETURNSTATUS"));
                            result = Convert.ToString(conn.GetParameterValue(ParameterOF.PROCEDURE, "@Out_Message"));

                            if (ReturnStatus == 0)
                            {
                                DebitNoteId = Convert.ToInt64(conn.GetParameterValue(ParameterOF.PROCEDURE, "@SP_DebitNoteId"));
                                DNCreditNoteId = Convert.ToInt64(conn.GetParameterValue(ParameterOF.PROCEDURE, "@SP_DNCreditNoteId"));
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
                throw new DataAccessException(Convert.ToString(UserSession.GetUserSession().LoginID), "DebitNoteDAL", "DeleteDebitNote", Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
        }

        public DataSet GetDebtorDetails(int DebtorId)
        {
            DataSet DebtorDetailsDs = new DataSet();
            try
            {
                string query = string.Format("EXEC [Comm].[GetDebtorDetailsForDisplay] @SP_LoginID={0}, @SP_DebtorId={1}",
                UserSession.GetUserSession().LoginID, CSubs.QSafeValue(Convert.ToString(DebtorId)));
                DebtorDetailsDs = CSubs.GetDataSet(query);
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(UserSession.GetUserSession().LoginID), "DebitNoteDAL", "GetDebtorDetails", Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
            return DebtorDetailsDs;
        }

        public DataSet GetDebitNoteTaxType(int RevenueBrId, int POSStateId)
        {
            DataSet DebitNoteTaxTypeDs = new DataSet();
            try
            {
                string query = string.Format("EXEC [Inv].[GetDebitNoteTaxType] @SP_LoginID={0}, @SP_RevenueBrId={1}, @SP_POSStateId={2}",
                UserSession.GetUserSession().LoginID, CSubs.QSafeValue(Convert.ToString(RevenueBrId)), CSubs.QSafeValue(Convert.ToString(POSStateId)));
                DebitNoteTaxTypeDs = CSubs.GetDataSet(query);
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(UserSession.GetUserSession().LoginID), "DebitNoteDAL", "GetDebitNoteTaxType", Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
            return DebitNoteTaxTypeDs;
        }

        public DataSet GetDebitNoteInvType(int CityID)
        {
            DataSet GetDebitNoteInvTypeDs = new DataSet();
            try
            {
                string query = string.Format("EXEC [Inv].[GetDebitNoteInvType] @SP_LoginID={0}, @SP_CityID={1}",
                UserSession.GetUserSession().LoginID, CSubs.QSafeValue(Convert.ToString(CityID)));
                GetDebitNoteInvTypeDs = CSubs.GetDataSet(query);
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(UserSession.GetUserSession().LoginID), "DebitNoteDAL", "GetDebitNoteInvType", Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
            return GetDebitNoteInvTypeDs;
        }

        public DataSet GetDebitNoteTaxRate(int SBUId, int DNCostHeadId)
        {
            DataSet DebitNoteTaxRateDs = new DataSet();
            try
            {
                string query = string.Format("EXEC [Inv].[GetDebitNoteTaxRate] @SP_LoginID={0}, @SP_SBUId={1}, @SP_DNCostHeadId={2}",
                UserSession.GetUserSession().LoginID, CSubs.QSafeValue(Convert.ToString(SBUId)), CSubs.QSafeValue(Convert.ToString(DNCostHeadId)));
                DebitNoteTaxRateDs = CSubs.GetDataSet(query);
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(UserSession.GetUserSession().LoginID), "DebitNoteDAL", "GetDebitNoteTaxRate", Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
            return DebitNoteTaxRateDs;
        }

        public DataSet GenerateEInvoiceDebitNote(string InvoiceNo)
        {
            DataSet InvoiceDetailDt = new DataSet();

            try
            {
                string query = string.Format("EXEC [E_Invoice].[EInvoiceSchemaDebitNote] @SP_LoginID={0}, @SP_InvoiceNo={1}",
                    UserSession.GetUserSession().LoginID, CSubs.QSafeValue(Convert.ToString(InvoiceNo)));
                InvoiceDetailDt = CSubs.GetDataSet(query);
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(UserSession.GetUserSession().LoginID), "DebitNoteDAL", "GenerateEInvoice", Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

            return InvoiceDetailDt;
        }

        public DataSet GetDNTansferToFAList(Entities.DNFundTranfer fa)
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

                using (CDALSQL conn = new CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {
                        conn.AddCommand("[Inv].[GetDNTransferToFAForGrid]", QueryType.Procedure);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LoginID", SqlDbType.Int, 0, ParameterDirection.Input, LoginID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CompId", SqlDbType.Int, 0, ParameterDirection.Input, CompanyID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_FromDate", SqlDbType.DateTime, 0, ParameterDirection.Input, fa.FromDate);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ToDate", SqlDbType.DateTime, 0, ParameterDirection.Input, fa.ToDate);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_BillNo", SqlDbType.VarChar, 20, ParameterDirection.Input, fa.BillNo);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Status", SqlDbType.VarChar, 30, ParameterDirection.Input, fa.Status);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_RevenueBranchXml", SqlDbType.Xml, 0, ParameterDirection.Input, RevenueBranchXml);

                        ds = (DataSet)conn.ExecuteProcedure(ProcedureReturnType.DataSet);
                    }
                    else
                        throw new Exception(conn.ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoginID), "DebitNoteDAL", "GetDNTansferToFAList", Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
            return ds;
        }

        public bool InsertDNTransferToFA(Entities.DNFundTranfer fundTranfer, int LoginID, out string result)
        {
            result = string.Empty;

            try
            {
                XElement BillItems = new XElement("SelectedLists",
                    from emp in fundTranfer.DNInvoiceList
                    select new XElement("SelectedList",
                    new XElement("CBSRefID", emp.CBSRefID),
                    new XElement("DebitOrCredit", emp.DebitOrCredit),
                    new XElement("AccountCode", emp.AccountCode)));


                string BillItemsString = BillItems.HasElements ? Convert.ToString(BillItems) : null;

                using (CDALSQL conn = new CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {
                        conn.AddCommand("[Inv].[AddEditDNTransferToFA]", QueryType.Procedure);
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

                            //if (ReturnStatus == 0)
                            //{
                            //    debitNote.DebitNoteId = Convert.ToInt64(conn.GetParameterValue(ParameterOF.PROCEDURE, "@SP_DebitNoteId"));
                            //    debitNote.DNCreditNoteId = Convert.ToInt64(conn.GetParameterValue(ParameterOF.PROCEDURE, "@SP_DNCreditNoteId"));
                            //    return true;
                            //}
                            //else
                            //{
                            //    return false;
                            //}
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
                throw new DataAccessException(Convert.ToString(LoginID), "DebitNoteDAL", "InsertDNTransferToFA", Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
        }
    }
}