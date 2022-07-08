using Newtonsoft.Json;
using RELOCBS.Common;
using RELOCBS.Common.ExceptionHandling;
using RELOCBS.DAL.Billing;
using RELOCBS.DAL.DebitNote;
using RELOCBS.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Dynamic;

namespace RELOCBS.BL.DebitNote
{
    public class DebitNoteBL
    {
        private DebitNoteDAL _debitNoteDAL;
        public DebitNoteDAL debitNoteDAL
        {
            get
            {
                if (this._debitNoteDAL == null)
                    this._debitNoteDAL = new DebitNoteDAL();
                return this._debitNoteDAL;
            }
        }

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

        public IEnumerable<Entities.DebitNote> GetDebitNoteList(string Sort, string SortDir, int Skip, int PageSize, DateTime? FromDate, DateTime? ToDate,
            string SearchType, string SearchValue, Int64? DebitNoteId, string DrOrCrNote, out int TotalCount)
        {
            IQueryable<Entities.DebitNote> DebitNotesList;
            TotalCount = 0;
            try
            {
                DataSet DebitNotesDs = debitNoteDAL.GetDebitNoteList(FromDate, ToDate, SearchType, SearchValue, DebitNoteId, DrOrCrNote);
                if (DebitNotesDs != null && DebitNotesDs.Tables.Count > 0 && DebitNotesDs.Tables[0].Rows.Count > 0)
                {
                    var result = (from rw in DebitNotesDs.Tables[0].AsEnumerable()
                                  select new Entities.DebitNote()
                                  {
                                      DebitNoteId = rw["DebitNoteId"] == DBNull.Value ? 0 : Convert.ToInt64(rw["DebitNoteId"]),
                                      DNCreditNoteId = rw["DNCreditNoteId"] == DBNull.Value ? 0 : Convert.ToInt64(rw["DNCreditNoteId"]),
                                      DebitNoteNo = rw["DebitNoteNo"] == DBNull.Value ? null : Convert.ToString(rw["DebitNoteNo"]),
                                      CreditNoteNo = rw["CreditNoteNo"] == DBNull.Value ? null : Convert.ToString(rw["CreditNoteNo"]),
                                      CreatedDate = rw["CreatedDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(rw["CreatedDate"]),
                                      DebitNoteDate = rw["DebitNoteDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(rw["DebitNoteDate"]),
                                      CreditNoteDate = rw["CreditNoteDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(rw["CreditNoteDate"]),
                                      DebitNoteStatus = rw["DebitNoteStatus"] == DBNull.Value ? null : Convert.ToString(rw["DebitNoteStatus"]),
                                      CreditNoteStatus = rw["CreditNoteStatus"] == DBNull.Value ? null : Convert.ToString(rw["CreditNoteStatus"]),
                                      SBUId = rw["SBUId"] == DBNull.Value ? (int?)null : Convert.ToInt32(rw["SBUId"]),
                                      SBU = rw["SBU"] == DBNull.Value ? null : Convert.ToString(rw["SBU"]),
                                      RevenueBrId = rw["RevenueBrId"] == DBNull.Value ? (int?)null : Convert.ToInt32(rw["RevenueBrId"]),
                                      RevenueBr = rw["RevenueBr"] == DBNull.Value ? null : Convert.ToString(rw["RevenueBr"]),
                                      DNTypeId = rw["DNTypeId"] == DBNull.Value ? (int?)null : Convert.ToInt32(rw["DNTypeId"]),
                                      DNType = rw["Type"] == DBNull.Value ? null : Convert.ToString(rw["Type"]),
                                      IsCreateCreditNote = rw["IsCreateCreditNote"] == DBNull.Value ? false : Convert.ToBoolean(rw["IsCreateCreditNote"]),
                                      IsShowCreditNote = rw["IsShowCreditNote"] == DBNull.Value ? false : Convert.ToBoolean(rw["IsShowCreditNote"]),
                                      Debtor = {
                                          DebtorId = rw["DebtorId"] == DBNull.Value ?    (int?)null : Convert.ToInt32(rw["DebtorId"]),
                                          DebtorName = rw["DebtorName"] == DBNull.Value ? null : Convert.ToString(rw["DebtorName"])
                                      }
                                  }).ToList();
                    DebitNotesList = result.AsQueryable();

                    TotalCount = DebitNotesList.Count();
                    DebitNotesList = DebitNotesList.OrderBy(Sort + " " + SortDir);
                    if (PageSize > 0)
                    {
                        DebitNotesList = DebitNotesList.Skip(Skip).Take(PageSize);
                    }
                    return DebitNotesList.ToList();
                }
                else
                {
                    return new List<Entities.DebitNote>();
                }
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "DebitNoteBL", "GetDebitNoteList", Properties.Resources.UnExpectedErrorAtBL, ex);
            }
        }

        public Entities.DebitNote GetDebitNoteDetailsById(Int64 DebitNoteId, Int64 DNCreditNoteId, string DrOrCrNote, out IEnumerable<Entities.DebitNoteDetails> DebitNoteDetailsList)
        {
            Entities.DebitNote DebitNoteObj = new Entities.DebitNote();
            DebitNoteDetailsList = null;
            try
            {
                DataSet DebitNoteDs = debitNoteDAL.GetDebitNoteDetailsById(DebitNoteId, DNCreditNoteId, DrOrCrNote);

                if (DebitNoteDs != null)
                {
                    if (DebitNoteDs.Tables.Count > 0 && DebitNoteDs.Tables[0].Rows.Count > 0)
                    {
                        DebitNoteObj = (from rw in DebitNoteDs.Tables[0].AsEnumerable()
                                        select new Entities.DebitNote()
                                        {
                                            DebitNoteId = rw["DebitNoteId"] == DBNull.Value ? 0 : Convert.ToInt64(rw["DebitNoteId"]),
                                            DNCreditNoteId = rw["DNCreditNoteId"] == DBNull.Value ? 0 : Convert.ToInt64(rw["DNCreditNoteId"]),
                                            DebitNoteNo = rw["DebitNoteNo"] == DBNull.Value ? null : Convert.ToString(rw["DebitNoteNo"]),
                                            CreditNoteNo = rw["CreditNoteNo"] == DBNull.Value ? null : Convert.ToString(rw["CreditNoteNo"]),
                                            SBUId = rw["SBUId"] == DBNull.Value ? (int?)null : Convert.ToInt32(rw["SBUId"]),
                                            SBU = rw["SBU"] == DBNull.Value ? null : Convert.ToString(rw["SBU"]),
                                            RevenueBrId = rw["RevenueBrId"] == DBNull.Value ? (int?)null : Convert.ToInt32(rw["RevenueBrId"]),
                                            RevenueBr = rw["RevenueBr"] == DBNull.Value ? null : Convert.ToString(rw["RevenueBr"]),
                                            DNTypeId = rw["DNTypeId"] == DBNull.Value ? (int?)null : Convert.ToInt32(rw["DNTypeId"]),
                                            DNType = rw["Type"] == DBNull.Value ? null : Convert.ToString(rw["Type"]),
                                            TaxType = rw["TaxType"] == DBNull.Value ? null : Convert.ToString(rw["TaxType"]),
                                            InvType = rw["InvType"] == DBNull.Value ? null : Convert.ToString(rw["InvType"]),
                                            IsActive = rw["IsActive"] == DBNull.Value ? (bool?)null : Convert.ToBoolean(rw["IsActive"]),
                                            CreatedDate = rw["CreatedDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(rw["CreatedDate"]),
                                            CreatedBy = rw["CreatedBy"] == DBNull.Value ? null : Convert.ToString(rw["CreatedBy"]),
                                            ModifiedDate = rw["ModifiedDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(rw["ModifiedDate"]),
                                            ModifiedBy = rw["ModifiedBy"] == DBNull.Value ? null : Convert.ToString(rw["ModifiedBy"]),
                                            ApprovedDate = rw["ApprovedDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(rw["ApprovedDate"]),
                                            ApprovedBy = rw["ApprovedBy"] == DBNull.Value ? null : Convert.ToString(rw["ApprovedBy"]),
                                            DebitNoteStatus = rw["DebitNoteStatus"] == DBNull.Value ? null : Convert.ToString(rw["DebitNoteStatus"]),
                                            CreditNoteStatus = rw["CreditNoteStatus"] == DBNull.Value ? null : Convert.ToString(rw["CreditNoteStatus"]),
                                            DebitNoteDate = rw["DebitNoteDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(rw["DebitNoteDate"]),
                                            CreditNoteDate = rw["CreditNoteDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(rw["CreditNoteDate"]),

                                            Debtor = new Entities.Debtor
                                            {
                                                POSStateId = rw["POSStateId"] == DBNull.Value ? (int?)null : Convert.ToInt32(rw["POSStateId"]),
                                                POSState = rw["POSState"] == DBNull.Value ? null : Convert.ToString(rw["POSState"]),
                                                DebtorId = rw["DebtorId"] == DBNull.Value ? (int?)null : Convert.ToInt32(rw["DebtorId"]),
                                                DebtorName = rw["DebtorName"] == DBNull.Value ? null : Convert.ToString(rw["DebtorName"]),
                                                Address1 = rw["Address1"] == DBNull.Value ? null : Convert.ToString(rw["Address1"]),
                                                Address2 = rw["Address2"] == DBNull.Value ? null : Convert.ToString(rw["Address2"]),
                                                CityID = rw["CityID"] == DBNull.Value ? (int?)null : Convert.ToInt32(rw["CityID"]),
                                                City = rw["City"] == DBNull.Value ? null : Convert.ToString(rw["City"]),
                                                State = rw["State"] == DBNull.Value ? null : Convert.ToString(rw["State"]),
                                                StateCode = rw["StateCode"] == DBNull.Value ? null : Convert.ToString(rw["StateCode"]),
                                                PINCode = rw["PINCode"] == DBNull.Value ? null : Convert.ToString(rw["PINCode"]),
                                                GSTNo = rw["GSTNo"] == DBNull.Value ? null : Convert.ToString(rw["GSTNo"]),
                                                PANNo = rw["PANNo"] == DBNull.Value ? null : Convert.ToString(rw["PANNo"])
                                            }
                                        }).First();
                    }
                    if (DebitNoteDs.Tables.Count > 1 && DebitNoteDs.Tables[1].Rows.Count > 0)
                    {
                        DebitNoteObj.CurrencyID = DebitNoteDs.Tables[1].Rows[0]["CurrencyID"] == DBNull.Value ? (int?)null : Convert.ToInt32(DebitNoteDs.Tables[1].Rows[0]["CurrencyID"]);
                        DebitNoteObj.Currency = DebitNoteDs.Tables[1].Rows[0]["Currency"] == DBNull.Value ? null : Convert.ToString(DebitNoteDs.Tables[1].Rows[0]["Currency"]);

                        DebitNoteObj.DebitNoteDetailsList = DebitNoteDs.Tables[1].AsEnumerable().
                            Select(rw => new Entities.DebitNoteDetails
                            {
                                DebitNoteDetailId = rw["DebitNoteDetailId"] == DBNull.Value ? 0 : Convert.ToInt64(rw["DebitNoteDetailId"]),
                                DebitNoteId = rw["DebitNoteId"] == DBNull.Value ? (Int64?)null : Convert.ToInt64(rw["DebitNoteId"]),
                                DNCreditNoteDetailId = rw["DNCreditNoteDetailId"] == DBNull.Value ? 0 : Convert.ToInt64(rw["DNCreditNoteDetailId"]),
                                DNCreditNoteId = rw["DNCreditNoteId"] == DBNull.Value ? (Int64?)null : Convert.ToInt64(rw["DNCreditNoteId"]),
                                DNCostHeadId = rw["DNCostHeadId"] == DBNull.Value ? (int?)null : Convert.ToInt32(rw["DNCostHeadId"]),
                                CostHead = rw["CostHead"] == DBNull.Value ? null : Convert.ToString(rw["CostHead"]),
                                Description = rw["Description"] == DBNull.Value ? null : Convert.ToString(rw["Description"]),
                                JobNo = rw["JobNo"] == DBNull.Value ? null : Convert.ToString(rw["JobNo"]),
                                SacCode = rw["SacCode"] == DBNull.Value ? null : Convert.ToString(rw["SacCode"]),
                                CurrencyID = rw["CurrencyID"] == DBNull.Value ? (int?)null : Convert.ToInt32(rw["CurrencyID"]),
                                Currency = rw["Currency"] == DBNull.Value ? null : Convert.ToString(rw["Currency"]),
                                Quantity = rw["Quantity"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(rw["Quantity"]),
                                Rate = rw["Rate"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(rw["Rate"]),
                                UnitId = rw["UnitId"] == DBNull.Value ? (int?)null : Convert.ToInt32(rw["UnitId"]),
                                Unit = rw["Unit"] == DBNull.Value ? null : Convert.ToString(rw["Unit"]),
                                DebitAmount = rw["DebitAmount"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(rw["DebitAmount"]),
                                CreditAmount = rw["CreditAmount"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(rw["CreditAmount"]),
                                MaxCreditAmount = rw["MaxCreditAmount"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(rw["MaxCreditAmount"]),
                                TaxPercent = rw["TaxPercent"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(rw["TaxPercent"]),
                                CGSTAmount = rw["CGSTAmount"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(rw["CGSTAmount"]),
                                SGSTAmount = rw["SGSTAmount"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(rw["SGSTAmount"]),
                                IGSTAmount = rw["IGSTAmount"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(rw["IGSTAmount"]),
                                VATAmount = rw["VATAmount"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(rw["VATAmount"]),
                                TotalAmount = rw["TotalAmount"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(rw["TotalAmount"])
                            }).ToList();
                    }
                    if (DebitNoteDs.Tables.Count > 2 && DebitNoteDs.Tables[2].Rows.Count > 0)
                    {
                        DebitNoteObj.AddressDetails.Line1 = Convert.ToString(DebitNoteDs.Tables[2].Rows[0]["Line1"]);
                        DebitNoteObj.AddressDetails.Line2 = Convert.ToString(DebitNoteDs.Tables[2].Rows[0]["Line2"]);
                        DebitNoteObj.AddressDetails.Line3 = Convert.ToString(DebitNoteDs.Tables[2].Rows[0]["Line3"]);
                        DebitNoteObj.AddressDetails.Line4 = Convert.ToString(DebitNoteDs.Tables[2].Rows[0]["Line4"]);
                        DebitNoteObj.AddressDetails.Line5 = Convert.ToString(DebitNoteDs.Tables[2].Rows[0]["Line5"]);
                        DebitNoteObj.AddressDetails.BottomLine1 = Convert.ToString(DebitNoteDs.Tables[2].Rows[0]["BottomLine1"]);
                        DebitNoteObj.AddressDetails.BottomLine2 = Convert.ToString(DebitNoteDs.Tables[2].Rows[0]["BottomLine2"]);
                        DebitNoteObj.AddressDetails.BottomLine3 = Convert.ToString(DebitNoteDs.Tables[2].Rows[0]["BottomLine3"]);
                        DebitNoteObj.AddressDetails.CompanyNameLine1 = Convert.ToString(DebitNoteDs.Tables[2].Rows[0]["CompNameLine1"]);
                        DebitNoteObj.AddressDetails.CompanyNameLine2 = Convert.ToString(DebitNoteDs.Tables[2].Rows[0]["CompNameLine2"]);
                        DebitNoteObj.AddressDetails.PANNoOur = Convert.ToString(DebitNoteDs.Tables[2].Rows[0]["PANNo"]);
                        DebitNoteObj.AddressDetails.GSTNoOur = Convert.ToString(DebitNoteDs.Tables[2].Rows[0]["GSTNo"]);
                    }

                    if (DebitNoteDs.Tables.Count > 3 && DebitNoteDs.Tables[3].Rows.Count > 0)
                    {
                        string QRSignedValue = string.Empty, BankQRValue = string.Empty;
                        DebitNoteObj.IRNNo = Convert.ToString(DebitNoteDs.Tables[3].Rows[0]["IRNNo"]);
                        QRSignedValue = Convert.ToString(DebitNoteDs.Tables[3].Rows[0]["SignedQRCode"]);
                        BankQRValue = Convert.ToString(DebitNoteDs.Tables[3].Rows[0]["BankQRCode"]);

                        if (!string.IsNullOrEmpty(QRSignedValue))
                        {
                            EInvoiceHandlerDebitNote _handler = new EInvoiceHandlerDebitNote();
                            DebitNoteObj.QRSignedValue = _handler.generatecode(QRSignedValue);
                        }

                        if (!string.IsNullOrWhiteSpace(BankQRValue))
                        {
                            EInvoiceHandlerDebitNote _handler = new EInvoiceHandlerDebitNote();
                            DebitNoteObj.BankQRCode = _handler.generatecode(BankQRValue);
                        }
                    }
                }
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "DebitNoteBL", "GetDebitNoteDetailsById", Properties.Resources.UnExpectedErrorAtBL, ex);
            }
            return DebitNoteObj;
        }

        public bool SaveDebitNote(Entities.DebitNote debitNote, string Status, string DrOrCrNote, out string result)
        {
            try
            {
                return debitNoteDAL.SaveDebitNoteDetails(debitNote, Status, DrOrCrNote, out result);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "DebitNoteBL", "SaveDebitNote", Properties.Resources.UnExpectedErrorAtBL, ex);
            }
        }

        public bool DeleteDebitNote(out string result, Int64 DebitNoteId = -1, Int64 DNCreditNoteId = -1, string DrOrCrNote = "")
        {
            try
            {
                return debitNoteDAL.DeleteDebitNote(DebitNoteId, DNCreditNoteId, DrOrCrNote, out result);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "DebitNoteBL", "DeleteDebitNote", Properties.Resources.UnExpectedErrorAtBL, ex);
            }
        }

        public Entities.Debtor GetDebtorDetails(int DebtorId = -1)
        {
            Entities.Debtor DebtorObj = new Entities.Debtor();
            try
            {
                DataSet DebtorDs = debitNoteDAL.GetDebtorDetails(DebtorId);

                if (DebtorDs != null && DebtorDs.Tables.Count > 0 && DebtorDs.Tables[0].Rows.Count > 0)
                {
                    DebtorObj = (from rw in DebtorDs.Tables[0].AsEnumerable()
                                 select new Entities.Debtor()
                                 {
                                     DebtorId = rw["DebtorId"] == DBNull.Value ? (int?)null : Convert.ToInt32(rw["DebtorId"]),
                                     DebtorName = rw["DebtorName"] == DBNull.Value ? null : Convert.ToString(rw["DebtorName"]),
                                     Address1 = rw["Address1"] == DBNull.Value ? null : Convert.ToString(rw["Address1"]),
                                     Address2 = rw["Address2"] == DBNull.Value ? null : Convert.ToString(rw["Address2"]),
                                     CityID = rw["CityID"] == DBNull.Value ? (int?)null : Convert.ToInt32(rw["CityID"]),
                                     City = rw["City"] == DBNull.Value ? null : Convert.ToString(rw["City"]),
                                     PINCode = rw["PINCode"] == DBNull.Value ? null : Convert.ToString(rw["PINCode"]),
                                     GSTNo = rw["GSTNo"] == DBNull.Value ? null : Convert.ToString(rw["GSTNo"]),
                                     PANNo = rw["PANNo"] == DBNull.Value ? null : Convert.ToString(rw["PANNo"])
                                 }).First();
                }
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "DebitNoteBL", "GetDebtorDetails", Properties.Resources.UnExpectedErrorAtBL, ex);
            }
            return DebtorObj;
        }

        public Entities.DebitNote GetDebitNoteTaxType(int RevenueBrId, int POSStateId)
        {
            Entities.DebitNote DebitNoteTaxTypeObj = new Entities.DebitNote();
            try
            {
                DataSet DebitNoteTaxTypeDs = debitNoteDAL.GetDebitNoteTaxType(RevenueBrId, POSStateId);

                if (DebitNoteTaxTypeDs != null && DebitNoteTaxTypeDs.Tables.Count > 0 && DebitNoteTaxTypeDs.Tables[0].Rows.Count > 0)
                {
                    DebitNoteTaxTypeObj.TaxType = Convert.ToString(DebitNoteTaxTypeDs.Tables[0].Rows[0]["TaxType"]);
                }
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "DebitNoteBL", "GetDebitNoteTaxType", Properties.Resources.UnExpectedErrorAtBL, ex);
            }
            return DebitNoteTaxTypeObj;
        }

        public Entities.DebitNote GetDebitNoteInvType(int CityID)
        {
            Entities.DebitNote GetDebitNoteInvTypeObj = new Entities.DebitNote();
            try
            {
                DataSet GetDebitNoteInvTypeDs = debitNoteDAL.GetDebitNoteInvType(CityID);

                if (GetDebitNoteInvTypeDs != null && GetDebitNoteInvTypeDs.Tables.Count > 0 && GetDebitNoteInvTypeDs.Tables[0].Rows.Count > 0)
                {
                    GetDebitNoteInvTypeObj.InvType = Convert.ToString(GetDebitNoteInvTypeDs.Tables[0].Rows[0]["InvType"]);
                }
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "DebitNoteBL", "GetDebitNoteInvoiceType", Properties.Resources.UnExpectedErrorAtBL, ex);
            }
            return GetDebitNoteInvTypeObj;
        }

        public Entities.DebitNote GetDebitNoteTaxRate(int SBUId, int DNCostHeadId)
        {
            Entities.DebitNote DebitNoteTaxRateObj = new Entities.DebitNote();
            try
            {
                DataSet DebitNoteTaxRateDs = debitNoteDAL.GetDebitNoteTaxRate(SBUId, DNCostHeadId);

                if (DebitNoteTaxRateDs != null && DebitNoteTaxRateDs.Tables.Count > 0 && DebitNoteTaxRateDs.Tables[0].Rows.Count > 0)
                {
                    DebitNoteTaxRateObj.TaxRate = Convert.ToDecimal(DebitNoteTaxRateDs.Tables[0].Rows[0]["TaxRate"]);
                    DebitNoteTaxRateObj.DebitNoteDetails.SacCode = Convert.ToString(DebitNoteTaxRateDs.Tables[0].Rows[0]["SacCode"]);
                }
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "DebitNoteBL", "GetDebitNoteTaxRate", Properties.Resources.UnExpectedErrorAtBL, ex);
            }
            return DebitNoteTaxRateObj;
        }

        public string GenerateEInvoiceDebitNote(string InvoiceNo)
        {
            try
            {
                DataSet ds = debitNoteDAL.GenerateEInvoiceDebitNote(InvoiceNo);
                EInvoiceHandlerDebitNote ForCallJson = new EInvoiceHandlerDebitNote();

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
                throw new DataAccessException(Convert.ToString(UserSession.GetUserSession().LoginID), "DebitNoteBL", "GenerateEInvoice", Properties.Resources.UnExpectedErrorAtBL, ex);
            }
        }

        public IEnumerable<Entities.DNInvoice> GetDNTansferToFAList(Entities.DNFundTranfer fundTranfer, string sort, string sortdir, int pageSize, out DataTable dt)
        {
            IQueryable<Entities.DNInvoice> DNInvoiceList;
            dt = null;
            try
            {
                DataSet ds = debitNoteDAL.GetDNTansferToFAList(fundTranfer);
                if (ds != null && ds.Tables.Count > 0)
                {
                    dt = ds.Tables[0];
                    var result = (from rw in ds.Tables[0].AsEnumerable()
                                  select new Entities.DNInvoice()
                                  {
                                      Layout = rw["Layout"] == DBNull.Value ? null : Convert.ToString(rw["Layout"]),
                                      BillNo = rw["Sales Invoice Number"] == DBNull.Value ? null : Convert.ToString(rw["Sales Invoice Number"]),
                                      BillDate = rw["Invoice Date"] == DBNull.Value ? default(DateTime) : Convert.ToDateTime(rw["Invoice Date"]),
                                      JobNo = rw["Job No"] == DBNull.Value ? null : Convert.ToString(rw["Job No"]),
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
                                      DebitOrCredit = rw["DebitOrCredit"] == DBNull.Value ? null : Convert.ToString(rw["DebitOrCredit"]),
                                      DebtorName = rw["DebtorName"] == DBNull.Value ? null : Convert.ToString(rw["DebtorName"]),
                                      GSTFlag = rw["GSTFlag"] == DBNull.Value ? null : Convert.ToString(rw["GSTFlag"]),
                                  }).ToList();
                    DNInvoiceList = result.AsQueryable();

                    return DNInvoiceList.ToList();
                }
                else
                {
                    return new List<Entities.DNInvoice>();
                }
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "DebitNoteBL", "GetDNTansferToFAList", Properties.Resources.UnExpectedErrorAtBL, ex);
            }
        }

        public bool InsertDNTransferToFA(Entities.DNFundTranfer fundTranfer, out string result)
        {
            try
            {
                return debitNoteDAL.InsertDNTransferToFA(fundTranfer, UserSession.GetUserSession().LoginID, out result);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "DebitNoteBL", "InsertDNTransferToFA", Properties.Resources.UnExpectedErrorAtBL, ex);
            }
        }
    }
}