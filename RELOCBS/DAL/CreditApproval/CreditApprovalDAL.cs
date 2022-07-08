using RELOCBS.App_Code;
using RELOCBS.Common.ExceptionHandling;
using RELOCBS.Entities;
using RELOCBS.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace RELOCBS.DAL.CreditApproval
{
    public class CreditApprovalDAL
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
        
        public IQueryable<Entities.CreditApprovalGrid> GetBusinessEntityGrid( string search,string Status, int CorporateID=-1)
        {
            int LoggedinUserID = UserSession.GetUserSession().LoginID;
            IQueryable<Entities.CreditApprovalGrid> List = new List<Entities.CreditApprovalGrid>().AsQueryable();

            try
            {

                using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {
                        conn.AddCommand("[Moveman].[GetCreditLimitEntityForGrid]", QueryType.Procedure);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LoginID", SqlDbType.Int, 0, ParameterDirection.Input, LoggedinUserID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_SearchString", SqlDbType.VarChar, 50, ParameterDirection.Input, CSubs.PSafeValue(search));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CorporateID", SqlDbType.BigInt, 0, ParameterDirection.Input, CorporateID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Status", SqlDbType.VarChar, 50, ParameterDirection.Input, CSubs.PSafeValue(Status)); 
                        //conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CompID", SqlDbType.Int, 0, ParameterDirection.Input, CompanyID);
                        //conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IsRMCBuss", SqlDbType.Bit, 1, ParameterDirection.Input, RMCBuss);
                        DataTable data = (DataTable)conn.ExecuteProcedure(ProcedureReturnType.DataTable);

                        if (data != null && data.Rows.Count > 0)
                        {

                            var result = (from item in data.AsEnumerable()
                                          select new Entities.CreditApprovalGrid()
                                          {
                                              CreditLimitEntityID = Convert.ToInt32(item["CreditLimitEntityID"]),
                                              CorporateName = Convert.ToString(item["CorporateName"]),
                                              Cust_Contact_Name = Convert.ToString(item["Cust_Contact_Name"]),
                                              Cust_Contact_Number = Convert.ToString(item["Cust_Contact_Number"]),
                                              Cust_Contact_Designation = Convert.ToString(item["Cust_Contact_Designation"]),
                                              Cust_Contact_Email = Convert.ToString(item["Cust_Contact_Email"]),
                                              Addresss = Convert.ToString(item["CorpAddress"]),
                                              City = Convert.ToString(item["City"]),
                                              TurnoverAmt = Convert.ToInt64(item["TurnoverAmt"]),
                                              IsActive = Convert.ToBoolean(item["IsActive"]),
                                              Status = Convert.ToString(item["StatusName"]),
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

                throw new DataAccessException(Convert.ToString(LoggedinUserID), "CreditApprovalDAL", "GetBusinessEntityGrid", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }


        }

        public DataSet GetDetails(int LoginID,int EntityID)
        {
            try
            {
                string query = string.Format("[MoveMan].[GetCreditLimitEntityDetail] @SP_EntityID={0},@SP_LoginID={1}",
                CSubs.QSafeValue(Convert.ToString(EntityID)),
                CSubs.QSafeValue(Convert.ToString(LoginID)));

                DataSet  Ds = CSubs.GetDataSet(query);
                return Ds;
            }
            catch (Exception ex)
            {

                throw new DataAccessException(Convert.ToString(LoginID), "CreditApprovalDAL", "GetDetails", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
        }

        public DataTable GetCustApprovalFile(int LoginID,Int64 FileID,Int32 EntityID,Int64 FeedbackID)
        {

            try
            {
                string query = string.Format("[MoveMan].[GetCreditApprovalFile] @SP_EntityID={0},@SP_BussFeedbackID={1},@sp_fileid={2},@SP_LoginID={3}",
                CSubs.QSafeValue(Convert.ToString(EntityID)),
                CSubs.QSafeValue(Convert.ToString(FeedbackID)),
                CSubs.QSafeValue(Convert.ToString(FileID)),
                CSubs.QSafeValue(Convert.ToString(LoginID)
                ));

                DataTable Dt = CSubs.GetDataTable(query);
                return Dt;
            }
            catch (Exception ex)
            {

                throw new DataAccessException(Convert.ToString(LoginID), "CreditApprovalDAL", "GetDetails", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

        }

        public bool DeleteDocument(Int64 FileID,Int32 EntityID,Int64 FeedbackID, int LoginID, out string message)
        {
            message = string.Empty;
            try
            {
                using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {
                        conn.AddCommand("[Moveman].[DeleteCreditApprovalDoc]", QueryType.Procedure);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CreditLimitEntityID", SqlDbType.Int, 0, ParameterDirection.Input, EntityID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_BussDevFeedbackID", SqlDbType.BigInt, 0, ParameterDirection.Input, FeedbackID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_FileID", SqlDbType.BigInt, 0, ParameterDirection.Input, FileID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LoginID", SqlDbType.Int, 0, ParameterDirection.Input, LoginID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@RETURNSTATUS", SqlDbType.SmallInt, 0, ParameterDirection.ReturnValue);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Message", SqlDbType.NVarChar, -1, ParameterDirection.Output);
                        conn.ExecuteProcedure(ProcedureReturnType.SingleValue);

                        if (!conn.IsError)
                        {
                            int ReturnStatus = Convert.ToInt16(conn.GetParameterValue(ParameterOF.PROCEDURE, "@RETURNSTATUS"));
                            message = Convert.ToString(conn.GetParameterValue(ParameterOF.PROCEDURE, "@SP_Message"));

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

                throw new DataAccessException(Convert.ToString(LoginID), "CreditApprovalDAL", "DeleteDocument", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
        }

        public bool InsetCreditEntity(CreditLimitEntity model, int LoginID, out string result)
        {
            try
            {
                bool IsRMCBuss=true;

                if (UserSession.GetUserSession().BussinessLine == "NON RMC-BUSINESS")
                {
                    IsRMCBuss = false;
                }
                
                string CreditXml = string.Empty;
                if (model.buss_Dev != null && model.buss_Dev.Count > 0)
                {
                    CreditXml = new XElement("Credits", from emp in model.buss_Dev
                                                                     select new XElement("Credit",
                                                         new XElement("ProjectID", emp.ProjectID),
                                                         new XElement("ServicelineID", emp.ServiceLineID),
                                                         new XElement("Credit_Amount", emp.Credit_Amount),
                                                         new XElement("CurrID", emp.CurrID),
                                                         new XElement("Remark", emp.BillingInstructions_Remark),
                                                         new XElement("PeriodBasisID", emp.Credit_period_basisID),
                                                         new XElement("CreditDays", emp.CreditDays),
                                                         new XElement("AdvCollectPercent", emp.Adv_Percent)
                                                     )).ToString();
                }


                using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {
                        conn.AddCommand("[Moveman].[AddEditCreditLimitEntity]", QueryType.Procedure);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CreditLimitEntityID", SqlDbType.BigInt, 0, ParameterDirection.InputOutput, model.CreditLimitEntityID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CreditEntityCorpId", SqlDbType.Int, 0, ParameterDirection.Input, model.CorporateID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_TurnoverAmt", SqlDbType.Float, 0, ParameterDirection.Input, model.Turnover_Amt);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Cust_Contact_Name", SqlDbType.VarChar, 100, ParameterDirection.Input, CSubs.PSafeValue(model.Cust_Contact_Name));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Cust_Contact_Designation", SqlDbType.VarChar, 50, ParameterDirection.Input, CSubs.PSafeValue(model.Cust_Contact_Designation));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Cust_Contact_Email", SqlDbType.VarChar, 150, ParameterDirection.Input, CSubs.PSafeValue(model.Cust_Contact_Email));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Cust_Contact_Number", SqlDbType.VarChar, 20, ParameterDirection.Input, CSubs.PSafeValue(model.Cust_Contact_Number));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CreditsXml", SqlDbType.Xml, -1, ParameterDirection.Input, CreditXml);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LoginID", SqlDbType.Int, 0, ParameterDirection.Input, LoginID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CompID", SqlDbType.Int, 0, ParameterDirection.Input, UserSession.GetUserSession().CompanyID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IsRMC", SqlDbType.Bit, 0, ParameterDirection.Input, IsRMCBuss);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_EffectiveTo", SqlDbType.DateTime, 0, ParameterDirection.Input, model.EffectiveTo);
                        ////approval status update parameters
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_SendApprovalToEmpID", SqlDbType.Int, 0, ParameterDirection.Input, model.SendApprovalToEmpID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Remark", SqlDbType.VarChar, 1500, ParameterDirection.Input, CSubs.PSafeValue(model.ApproveRemark));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Type", SqlDbType.VarChar, 50, ParameterDirection.Input, CSubs.PSafeValue(model.ApproveType));
                        ////end approval status update parameters
                        conn.AddParameter(ParameterOF.PROCEDURE, "@RETURNSTATUS", SqlDbType.SmallInt, 0, ParameterDirection.ReturnValue);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Message", SqlDbType.NVarChar, -1, ParameterDirection.Output);
                        conn.ExecuteProcedure(ProcedureReturnType.SingleValue);

                        if (!conn.IsError)
                        {
                            int ReturnStatus = Convert.ToInt32(conn.GetParameterValue(ParameterOF.PROCEDURE, "@RETURNSTATUS"));
                            result = Convert.ToString(conn.GetParameterValue(ParameterOF.PROCEDURE, "@SP_Message"));

                            if (ReturnStatus == 0)
                            {
                                model.CreditLimitEntityID = Convert.ToInt32(conn.GetParameterValue(ParameterOF.PROCEDURE, "@SP_CreditLimitEntityID"));
                                
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

                throw new DataAccessException(Convert.ToString(LoginID), "CreditApprovalDAL", "InsetCreditEntity", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

        }
        
        public bool InsertDocument(CustApprovalUpload model, int LoginID, out string result)
        {
            result = string.Empty;
            try
            {
                string FileName = string.Empty;
                string Extension = string.Empty;
                string FilePath = System.Configuration.ConfigurationManager.AppSettings["CreditApprovalFiles"].ToString();
                int FileUploadCount = 0;

                if (model.ApprovalFile != null)
                {
                    using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString(), true, System.Data.IsolationLevel.ReadCommitted))
                    {
                        if (conn.Connect())
                        {
                            conn.AddCommand("[MoveMan].[AddEditCreditCustomerApprovalFile]", QueryType.Procedure);
                            conn.AddParameter(ParameterOF.PROCEDURE, "@SP_FeedbackID", SqlDbType.BigInt, 0, ParameterDirection.Input, model.Buss_Dev_FeedbackID);
                            conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ApprovalType", SqlDbType.VarChar, 50, ParameterDirection.Input, CSubs.PSafeValue(model.ApprovalType));
                            conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Description", SqlDbType.NVarChar, 250, ParameterDirection.Input, CSubs.PSafeValue(model.DocDescription));
                            conn.AddParameter(ParameterOF.PROCEDURE, "@SP_FilePath", SqlDbType.NVarChar, 500, ParameterDirection.InputOutput);
                            conn.AddParameter(ParameterOF.PROCEDURE, "@SP_FileName", SqlDbType.NVarChar, 250, ParameterDirection.Input);
                            conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Ext", SqlDbType.NVarChar, 50, ParameterDirection.Input);
                            conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LOGINID", SqlDbType.Int, 0, ParameterDirection.Input, LoginID);
                            conn.AddParameter(ParameterOF.PROCEDURE, "@RETURNSTATUS", SqlDbType.SmallInt, 0, ParameterDirection.ReturnValue);
                            conn.AddParameter(ParameterOF.PROCEDURE, "@Out_Message", SqlDbType.NVarChar, 500, ParameterDirection.Output);
                            foreach (var file in model.ApprovalFile)
                            {
                                if (file != null && file.ContentLength > 0)
                                {
                                    FilePath = System.Configuration.ConfigurationManager.AppSettings["CreditApprovalFiles"].ToString();
                                    FileName = file.FileName;
                                    Extension = Path.GetExtension(file.FileName);
                                    conn.SetParameterValue(ParameterOF.PROCEDURE, "@SP_FilePath", CSubs.PSafeValue(FilePath));
                                    conn.SetParameterValue(ParameterOF.PROCEDURE, "@SP_FileName", CSubs.PSafeValue(FileName));
                                    conn.SetParameterValue(ParameterOF.PROCEDURE, "@SP_Ext", CSubs.PSafeValue(Extension));
                                    conn.ExecuteProcedure(ProcedureReturnType.SingleValue);

                                    if (!conn.IsError)
                                    {
                                        int ReturnStatus = Convert.ToInt32(conn.GetParameterValue(ParameterOF.PROCEDURE, "@RETURNSTATUS"));
                                        result = Convert.ToString(conn.GetParameterValue(ParameterOF.PROCEDURE, "@Out_Message"));

                                        if (ReturnStatus == 0)
                                        {
                                            //SaveRate.MoveID = Convert.ToInt64(conn.GetParameterValue(ParameterOF.PROCEDURE, "@SP_MoveID"));
                                            FilePath = Convert.ToString(conn.GetParameterValue(ParameterOF.PROCEDURE, "@SP_FilePath"));

                                            try
                                            {
                                                file.SaveAs(FilePath);
                                                conn.CommitTransaction();
                                                FileUploadCount++;
                                            }
                                            catch (Exception)
                                            {
                                                conn.RollbackTransaction();
                                                throw new Exception("Unable to save File");
                                            }

                                        }
                                        else
                                        {
                                            conn.RollbackTransaction();
                                            return false;

                                        }
                                    }
                                    else
                                    {
                                        conn.RollbackTransaction();
                                        throw new Exception(conn.ErrorMessage);
                                    }

                                }


                            }


                        }
                        else
                            throw new Exception(conn.ErrorMessage);
                    }


                    if (FileUploadCount > 0)
                    {
                        return true;
                    }
                    else
                    {

                        result = "Uable to save files";
                        return false;
                    }


                }
                else
                    throw new Exception("File Not Found");
            }
            catch (Exception ex)
            {

                throw new DataAccessException(Convert.ToString(LoginID), "CreditApprovalDAL", "InsertDocument", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
        }

        public bool UpdateApprovalStatus(CreditLimitEntity model, int LoginID, out string result)
        {
            try
            {
                using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {
                        
                        conn.AddCommand("[MoveMan].[AddEditCreditApprovalStatus]", QueryType.Procedure);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CreditLimitEntityID", SqlDbType.Int, 0, ParameterDirection.Input, model.CreditLimitEntityID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_SendApprovalToEmpID", SqlDbType.Int, 0, ParameterDirection.Input, model.SendApprovalToEmpID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Remark", SqlDbType.VarChar, 1500, ParameterDirection.Input, CSubs.PSafeValue(model.ApproveRemark));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Type", SqlDbType.VarChar, 50, ParameterDirection.Input, CSubs.PSafeValue(model.ApproveType));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LoginID", SqlDbType.Int, 0, ParameterDirection.Input, LoginID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@RETURNSTATUS", SqlDbType.SmallInt, 0, ParameterDirection.ReturnValue);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Message", SqlDbType.NVarChar, -1, ParameterDirection.Output);
                        conn.ExecuteProcedure(ProcedureReturnType.SingleValue);

                        if (!conn.IsError)
                        {
                            int ReturnStatus = Convert.ToInt32(conn.GetParameterValue(ParameterOF.PROCEDURE, "@RETURNSTATUS"));
                            result = Convert.ToString(conn.GetParameterValue(ParameterOF.PROCEDURE, "@SP_Message"));

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

                throw new DataAccessException(Convert.ToString(LoginID), "CreditApprovalDAL", "InsertBussDevFeedback", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

        }

        public bool InsertClientEntityMap(EntityClientMap model, int LoginID, out string result)
        {
            try
            {
                using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {
                        var client = model.MappedClientList != null ? new XElement("root", model.MappedClientList.Select(x => new XElement("ClientIDs", new XElement("ClientID", x)))) : new XElement("ClientIDs");

                        conn.AddCommand("[Moveman].[AddEditCreditEntityClientMapping]", QueryType.Procedure);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_EntityID", SqlDbType.BigInt, 0, ParameterDirection.Input, model.CreditLimitEntityID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Remark", SqlDbType.VarChar, 500, ParameterDirection.Input, CSubs.PSafeValue(model.Remark));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ClientIDXml", SqlDbType.Xml, -1, ParameterDirection.Input, client.HasElements ? Convert.ToString(client) : null);

                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LoginID", SqlDbType.Int, 0, ParameterDirection.Input, LoginID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@RETURNSTATUS", SqlDbType.SmallInt, 0, ParameterDirection.ReturnValue);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Message", SqlDbType.NVarChar, -1, ParameterDirection.Output);
                        conn.ExecuteProcedure(ProcedureReturnType.SingleValue);

                        if (!conn.IsError)
                        {
                            int ReturnStatus = Convert.ToInt32(conn.GetParameterValue(ParameterOF.PROCEDURE, "@RETURNSTATUS"));
                            result = Convert.ToString(conn.GetParameterValue(ParameterOF.PROCEDURE, "@SP_Message"));

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

                throw new DataAccessException(Convert.ToString(LoginID), "CreditApprovalDAL", "InsertClientEntityMap", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

        }

        public DataSet GetPrint(int LoginID,int Id)
        {
            try
            {
                string query = string.Format("[MoveMan].[GetCreditLimitEntityPrint] @SP_EntityID={0},@SP_LoginID={1}",
                CSubs.QSafeValue(Convert.ToString(Id)),
                CSubs.QSafeValue(Convert.ToString(LoginID)));

                return CSubs.GetDataSet(query);
            }
            catch (Exception ex)
            {

                throw new DataAccessException(Convert.ToString(LoginID), "CreditApprovalDAL", "GetPrint", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
        }
    }
}