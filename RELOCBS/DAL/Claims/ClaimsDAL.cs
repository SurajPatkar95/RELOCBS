using RELOCBS.App_Code;
using RELOCBS.Common.ExceptionHandling;
using RELOCBS.Entities;
using RELOCBS.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml.Linq;

namespace RELOCBS.DAL.Claims
{
    public class ClaimsDAL
    {
        string FilePath = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["ClaimFile"]);

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


        public IQueryable<Entities.ClaimGrid> GetClaimGrid(DateTime? FromDate, DateTime? Todate, bool IsJobDate, bool IsClaimDate, string JobNo, Int64 Claim_Id, bool RMCBuss, int CompanyID,out int IsFinanceUser)
        {
            int LoggedinUserID = UserSession.GetUserSession().LoginID;
            IQueryable<Entities.ClaimGrid> List = new List<Entities.ClaimGrid>().AsQueryable();
            IsFinanceUser = 0;
            try
            {

                using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {
                        conn.AddCommand("[Claim].[GetClaimDetailsForGrid]", QueryType.Procedure);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IsJob", SqlDbType.Bit, 1, ParameterDirection.Input, IsJobDate);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IsClaim", SqlDbType.Bit, 1, ParameterDirection.Input, IsClaimDate);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_FromDate", SqlDbType.DateTime, 0, ParameterDirection.Input, FromDate);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ToDate", SqlDbType.DateTime, 0, ParameterDirection.Input, Todate);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LoginID", SqlDbType.Int, 0, ParameterDirection.Input, UserSession.GetUserSession().LoginID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_JobNo", SqlDbType.VarChar, 50, ParameterDirection.Input, CSubs.PSafeValue(JobNo));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ClaimDetailIForJobID", SqlDbType.BigInt, 0, ParameterDirection.Input, Claim_Id);
                        //conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CompID", SqlDbType.Int, 0, ParameterDirection.Input, CompanyID);
                        //conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IsRMCBuss", SqlDbType.Bit, 1, ParameterDirection.Input, RMCBuss);
                        DataSet dataSet = (DataSet)conn.ExecuteProcedure(ProcedureReturnType.DataSet);

                        if (dataSet != null && dataSet.Tables.Count>0)
                        {

                            if (dataSet.Tables.Count>0 && dataSet.Tables[0]!=null)
                            {
                                var result = (from item in dataSet.Tables[0].AsEnumerable()
                                              select new Entities.ClaimGrid()
                                              {
                                                  MoveID = Convert.ToInt64(item["MoveID"]),
                                                  Claim_ID = !string.IsNullOrWhiteSpace(Convert.ToString(item["ClaimDetailIForJobID"])) ? Convert.ToInt64(item["ClaimDetailIForJobID"]) : -1,
                                                  JobNo = Convert.ToString(item["JobID"]),
                                                  ClaimNo = Convert.ToString(item["ClaimNo"]),
                                                  JobDate = Convert.ToDateTime(item["JobOpenedDate"]),
                                                  Claim_Date = Convert.ToDateTime(item["ClaimDate"]),
                                                  ServiceLine = Convert.ToString(item["ServiceLine"]),
                                                  OrgCity = Convert.ToString(item["FromCity"]),
                                                  DestCity = Convert.ToString(item["ToCity"]),
                                                  Controller = Convert.ToString(item["ControllerName"]),
                                                  ShipperName = Convert.ToString(item["ShipperName"]),
                                                  Client = Convert.ToString(item["ClientName"]),
                                                  Corporate = Convert.ToString(item["CorporateName"]),
                                                  ClaimAmt = Convert.ToDecimal(item["ClaimAmt"]),
                                                  Status = Convert.ToString(item["Status"])
                                              }).AsQueryable();
                                List = result;
                            }
                            
                            if (dataSet.Tables.Count > 1 && dataSet.Tables[1] != null)
                            {
                                IsFinanceUser = Convert.ToInt32(dataSet.Tables[1].Rows[0]["IsFinanceRole"]);
                            }
                        }
                    }
                    else
                        throw new Exception(conn.ErrorMessage);
                }


                return List;
            }
            catch (Exception ex)
            {

                throw new DataAccessException(Convert.ToString(LoggedinUserID), "ClaimDAL", "GetClaimGrid", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }


        }

        public DataSet GetClaimDetails(int LoginID, Int64 MoveID, Int64 Claim_ID)
        {
            DataSet Ds = new DataSet();

            try
            {
                string query = string.Format("[Claim].[GetClaimDetails] @SP_MoveID={0},@SP_ClaimDetailIForJobID={1},@SP_LoginID={2}",
                CSubs.QSafeValue(Convert.ToString(MoveID)),
                CSubs.QSafeValue(Convert.ToString(Claim_ID)),
                CSubs.QSafeValue(Convert.ToString(LoginID)));

                Ds = CSubs.GetDataSet(query);

                return Ds;
            }
            catch (Exception ex)
            {

                throw new DataAccessException(Convert.ToString(LoginID), "ClaimDAL", "GetClaimDetails", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

        }

        public bool Insert(Claim model, int LoginID, out string result, bool SentToFinance = false,bool IsApprove=false)
        {
            try
            {
                using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {
                        string claimXml = string.Empty;
                        string DocXml = string.Empty;

                        if (model.details != null && model.details.Count > 0)
                        {
                            claimXml = new XElement("ClaimItemDetails", from emp in model.details
                                                                select new XElement("ClaimItemDetail",
                                                         new XElement("ClaimNatureID", emp.ClaimNatureID),
                                                         new XElement("ClaimItemDetailIsID", emp.ClaimItemDetailIsID),
                                                         //new XElement("ClaimItemCategoryId", emp.ClaimItemCategoryId),
                                                         new XElement("NumberOfItem", emp.NumberOfItem),
                                                         new XElement("Remarks", emp.Remarks),
                                                         new XElement("ClaimCategoryID",emp.ClaimItemCategoryId)

                                                     )).ToString();

                            //if (!string.IsNullOrWhiteSpace(claimXml))
                            //{
                            //    claimXml = Regex.Replace(claimXml, @"\t|\n|\r", ""); //InstIDxml.Replace("\r\n", "");
                            //}
                        }

                        //////Document
                        //if (model.docUpload.docLists != null && model.docUpload.docLists.Count > 0)
                        //{
                        //    DocXml = new XElement("Downloads", from emp in model.docUpload.docLists
                        //                                       select new XElement("Download",
                        //                              new XElement("DocID", emp.DocID),
                        //                              new XElement("DocTypeID", emp.DocTypeID)
                        //                          )).ToString();

                        //    if (!string.IsNullOrWhiteSpace(DocXml))
                        //    {
                        //        DocXml = Regex.Replace(DocXml, @"\t|\n|\r", ""); //InstIDxml.Replace("\r\n", "");
                        //    }
                        //}

                        conn.AddCommand("[Claim].[AddEditClaimDetails]", QueryType.Procedure);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ClaimDetailIForJobID", SqlDbType.BigInt, 0, ParameterDirection.InputOutput, model.Claim_ID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_MoveID", SqlDbType.BigInt, 0, ParameterDirection.Input, model.MoveID);
                        //conn.AddParameter(ParameterOF.PROCEDURE, "@SP_InsNumber", SqlDbType.BigInt, 0, ParameterDirection.Input, model.Policy_No);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IntDate", SqlDbType.DateTime, 0, ParameterDirection.Input, model.Int_Date);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_AckDate", SqlDbType.DateTime, 0, ParameterDirection.Input, model.Ackn_Date);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ClaimFileDate", SqlDbType.DateTime, 0, ParameterDirection.Input, model.Claim_File_Date);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ClaimAmt", SqlDbType.Float, 0, ParameterDirection.Input, model.Claim_Amt);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_RateCurrID", SqlDbType.Int, 0, ParameterDirection.Input, model.RateCurrencyID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_BaseCurrID", SqlDbType.Int, 0, ParameterDirection.Input, model.BaseCurrencyID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ConverRate", SqlDbType.Float, 0, ParameterDirection.Input, model.ExRate);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_PkgsPacked", SqlDbType.Int, 0, ParameterDirection.Input, model.PkgsPacked);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_PkgsDamaged", SqlDbType.Int, 0, ParameterDirection.Input, model.PkgsDamaged);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_OtherExp", SqlDbType.Float, 0, ParameterDirection.Input, model.OtherExp);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_RemarksForOtherExp", SqlDbType.VarChar, 500, ParameterDirection.Input, CSubs.PSafeValue(model.RemarksForOtherExp));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_InsClaimAmt", SqlDbType.Float, 0, ParameterDirection.Input, model.Ins_Claim_Amt);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_InsBaseeCurr", SqlDbType.Int, 0, ParameterDirection.Input, model.Ins_BaseCurr);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_InsRateCurr", SqlDbType.Int, 0, ParameterDirection.Input, model.Ins_RateCurr);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_InsConverrate", SqlDbType.Float, 0, ParameterDirection.Input, model.Ins_ConverRate);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_InsRoute", SqlDbType.Float, 0, ParameterDirection.Input, model.InsRoute);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CompPaidAmt", SqlDbType.Float, 0, ParameterDirection.Input, model.CompPaidAmt);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ClaimSettledDate", SqlDbType.DateTime, 0, ParameterDirection.Input, model.ClaimSettledDate);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_PayMode", SqlDbType.VarChar, 10, ParameterDirection.Input, CSubs.PSafeValue(model.PayMode));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ClaimStatusID", SqlDbType.Int, 10, ParameterDirection.Input, model.ClaimStatusID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_VoucherDate", SqlDbType.Date, 0, ParameterDirection.Input, model.VoucherDate);

                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ChqNumber", SqlDbType.VarChar, 10, ParameterDirection.Input, CSubs.PSafeValue(model.ChqNumber));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_DocRecdDate", SqlDbType.Date, 0, ParameterDirection.Input, model.DocRecdDate);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ChqToName", SqlDbType.VarChar, 50, ParameterDirection.Input, CSubs.PSafeValue(model.ChqToName));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ChqStatus", SqlDbType.VarChar, 20, ParameterDirection.Input, CSubs.PSafeValue(model.ChqStatus));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ClaimFormRecdDate", SqlDbType.Date, 0, ParameterDirection.Input, model.ClaimFormRecdDate);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ClaimFileRemarks", SqlDbType.VarChar, 500, ParameterDirection.Input, CSubs.PSafeValue(model.ClaimFileRemarks));

                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_InstToFinance", SqlDbType.VarChar, 500, ParameterDirection.Input, CSubs.PSafeValue(model.InstToFinance));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_InsRef", SqlDbType.VarChar, 20, ParameterDirection.Input, CSubs.PSafeValue(model.InsRef));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_VendorPaid", SqlDbType.Float, 0, ParameterDirection.Input, model.VendorPaid);

                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ClaimItemDetails", SqlDbType.Xml, 0, ParameterDirection.Input, claimXml);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ControllerID", SqlDbType.Int, 0, ParameterDirection.Input, model.ControllerID);

                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_PackSupervisorName", SqlDbType.VarChar, 50, ParameterDirection.Input, CSubs.PSafeValue(model.Pack_Superviser));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_DeliverySupervisorName", SqlDbType.VarChar, 50, ParameterDirection.Input, CSubs.PSafeValue(model.Delivery_Superviser));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_SettlementType", SqlDbType.VarChar, 20, ParameterDirection.Input, CSubs.PSafeValue(model.settlementType));
                        
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ClaimSurveyDate", SqlDbType.DateTime, 0, ParameterDirection.Input, model.SurveyDate);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ClaimSurveyAmt", SqlDbType.Float, 0, ParameterDirection.Input, model.SurveyAmt);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_InsPANo", SqlDbType.BigInt, 0, ParameterDirection.Input, model.P_A_No);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ClaimShipperAcceptedAmt", SqlDbType.Float, 0, ParameterDirection.Input, model.ClaimAmt_Accepted_Shipper);

                        //conn.AddParameter(ParameterOF.PROCEDURE, "@SP_DocXml", SqlDbType.Xml, -1, ParameterDirection.Input, DocXml);
                        //if (model.docUpload.file != null)
                        //{
                        //    conn.AddParameter(ParameterOF.PROCEDURE, "@SP_DocTypeID", SqlDbType.Int, 0, ParameterDirection.Input, model.docUpload.DocTypeID);
                        //    conn.AddParameter(ParameterOF.PROCEDURE, "@SP_DocID", SqlDbType.Int, 0, ParameterDirection.Output);
                        //    conn.AddParameter(ParameterOF.PROCEDURE, "@SP_FilePath", SqlDbType.NVarChar, 500, ParameterDirection.InputOutput, CSubs.PSafeValue(FilePath));
                        //    conn.AddParameter(ParameterOF.PROCEDURE, "@SP_DocName", SqlDbType.NVarChar, 500, ParameterDirection.Input, CSubs.PSafeValue(model.docUpload.file.FileName));
                        //    conn.AddParameter(ParameterOF.PROCEDURE, "@SP_EXT", SqlDbType.NVarChar, 10, ParameterDirection.Input, CSubs.PSafeValue(Path.GetExtension(model.docUpload.file.FileName)));
                        //}

                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IsSubmitToFinance", SqlDbType.Bit, 0, ParameterDirection.Input,SentToFinance);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IsApprove", SqlDbType.Bit, 0, ParameterDirection.Input, IsApprove);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_EmailTo", SqlDbType.NVarChar, 500, ParameterDirection.Input, model.Email.EmailTo);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_EmailCC", SqlDbType.NVarChar, 500, ParameterDirection.Input, model.Email.EmailCC);
                        //conn.AddParameter(ParameterOF.PROCEDURE, "@SP_EmailBCC", SqlDbType.NVarChar, 500, ParameterDirection.Input, model.Email.EmailBCC);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_EmailSubject", SqlDbType.NVarChar, -1, ParameterDirection.Input, model.Email.Subject);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_EmailBody", SqlDbType.NVarChar, -1, ParameterDirection.Input, model.Email.Body);

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
                                model.Claim_ID = Convert.ToInt64(conn.GetParameterValue(ParameterOF.PROCEDURE, "@SP_ClaimDetailIForJobID"));

                                //try
                                //{
                                //    if (model.docUpload.file != null)
                                //    {
                                //        string File = Convert.ToString(conn.GetParameterValue(ParameterOF.PROCEDURE, "@SP_FilePath"));
                                //        string DocID = Convert.ToString(conn.GetParameterValue(ParameterOF.PROCEDURE, "@SP_DocID"));
                                //        if (!string.IsNullOrWhiteSpace(File) && !string.IsNullOrWhiteSpace(DocID))
                                //        {
                                //            model.docUpload.file.SaveAs(File);
                                //        }
                                //    }
                                //}
                                //catch (Exception)
                                //{
                                //    result = "Unable to save file";
                                //    return false;
                                //}

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
                throw new DataAccessException(Convert.ToString(LoginID), "ClaimDAL", "Insert", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

        }

        public DataSet GetInsuranceDetail(int LoginID,Int64 MoveID,Int64 P_A_NO)
        {
            DataSet dt = new DataSet();

            try
            {
                string query = string.Format("[Claim].[GetInsDetails] @SP_Moveid={0},@SP_InsPANo={1},@SP_LoginID={2}",
                CSubs.QSafeValue(Convert.ToString(MoveID)),
                CSubs.QSafeValue(Convert.ToString(P_A_NO)),
                CSubs.QSafeValue(Convert.ToString(LoginID)));

                dt = CSubs.GetDataSet(query);
            }
            catch (Exception ex)
            {

                throw new DataAccessException(Convert.ToString(LoginID), "ClaimDAL", "GetInsuranceDetail", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

            return dt;
        }

        public DataSet GetPrintDetail(int LoginID, Int64 MoveID, Int64 ClaimID)
        {
            DataSet ds = new DataSet();

            try
            {
                string query = string.Format("[Claim].[GetClaimPrint] @SP_ClaimDetailIForJobID={0},@SP_Moveid={1},@SP_LoginID={2}",
                CSubs.QSafeValue(Convert.ToString(ClaimID)),
                CSubs.QSafeValue(Convert.ToString(MoveID)),
                CSubs.QSafeValue(Convert.ToString(LoginID)));

                ds = CSubs.GetDataSet(query);
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoginID), "ClaimDAL", "GetPrintDetail", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

            return ds;

        }

        public JobDocument GetDownloadFile(Int64 DocID, Int64 ClaimID, int LoginID)
        {
            JobDocument job = new JobDocument();
            try
            {
                DataTable dt = CSubs.GetDataTable(string.Format("exec [Claim].[GetClaimDocumentDetail] @SP_DocID={0},@SP_ClaimID={1},@SP_LoginID={2}",
                    CSubs.QSafeValue(Convert.ToString(DocID))
                    , CSubs.QSafeValue(Convert.ToString(ClaimID))
                    , CSubs.QSafeValue(Convert.ToString(LoginID))));

                if (dt != null && dt.Rows.Count > 0)
                {
                    job.FileID = Convert.ToInt32(dt.Rows[0]["DocID"]);
                    job.DocTypeID = Convert.ToInt32(dt.Rows[0]["DocTypeID"]);
                    //job.DocNameID = Convert.ToInt32(dt.Rows[0]["DocNameID"]);
                    job.FilePath = Convert.ToString(dt.Rows[0]["FilePath"]);
                    job.FileName = Convert.ToString(dt.Rows[0]["DocumentName"]);
                }

            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoginID), "JobReportDAL", "GetDownloadFile", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

            return job;

        }

        public DataSet GetEPrintDetail(int LoginID, Int64 MoveID, Int64 ClaimID)
        {
            DataSet ds = new DataSet();

            try
            {
                string query = string.Format("[Claim].[GetEClaimPrint] @SP_ClaimDetailIForJobID={0},@SP_Moveid={1},@SP_LoginID={2}",
                CSubs.QSafeValue(Convert.ToString(ClaimID)),
                CSubs.QSafeValue(Convert.ToString(MoveID)),
                CSubs.QSafeValue(Convert.ToString(LoginID)));

                ds = CSubs.GetDataSet(query);
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoginID), "ClaimDAL", "GetEPrintDetail", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

            return ds;

        }

        public EmailConfig GetClaimEmailDetail(Claim model, int LoginID)
        {
            EmailConfig email = new EmailConfig();

            try
            {
                string query = string.Format("[Claims].[GetClaimSendMailContent] @SP_ClaimDetailIForJobID={0},@SP_Moveid={1},@SP_LoginID={2}",
               CSubs.QSafeValue(Convert.ToString(model.Claim_ID)),
               CSubs.QSafeValue(Convert.ToString(model.MoveID)),
               CSubs.QSafeValue(Convert.ToString(LoginID)));

                DataSet ds = CSubs.GetDataSet(query);

                if (ds!=null && ds.Tables.Count>0)
                {

                    if (ds.Tables.Count >0 && ds.Tables[0]!=null && ds.Tables[0].Rows.Count>0)
                    {
                        email = (from item in ds.Tables[0].AsEnumerable()
                                 select new EmailConfig()
                                 {
                                     
                                     EmailFrom = Convert.ToString(item["EmailFrom"]),
                                     EmailFromPassowrd = Convert.ToString(item["EmailPassword"]),
                                     EmailTo = Convert.ToString(item["EmailTo"]),
                                     EmailCC = Convert.ToString(item["EmailCC"]),
                                     Subject = Convert.ToString(item["EmailSubject"]),
                                     Body = Convert.ToString(item["EmailBody"]),
                                     Host = Convert.ToString(item["Host"]),
                                     Port = Convert.ToInt32(item["SMTPPort"]),
                                     EnableSSL = Convert.ToBoolean(item["EmableSSL"]),
                                 }).FirstOrDefault();
                    }

                    if (ds.Tables.Count>1 && ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0)
                    {
                        email.attachments  = (from item in ds.Tables[1].AsEnumerable()
                                              select new EmailSendAttachment()
                                              {

                                                  FileName = Convert.ToString(item["DocFileName"]),
                                                  FilePath = Convert.ToString(item["DocFilePath"])
                                              }).ToList();
                                        
                    }
                }

                return email;
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoginID), "ClaimDAL", "GetClaimEmailDetail", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
        }
    }
}