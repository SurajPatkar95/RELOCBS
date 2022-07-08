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

namespace RELOCBS.DAL.WH_Assessment
{
    public class WHRiskAssessmentDAL
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


        public IQueryable<Entities.WHAssessmentGrid> GetGrid(DateTime? FromDate, DateTime? Todate, bool? RMCBuss = false, int? WarehouseId = null)
        {
            int LoggedinUserID = UserSession.GetUserSession().LoginID;
            try
            {

                IQueryable<Entities.WHAssessmentGrid> data = null;

                using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {
                        conn.AddCommand("[Warehouse].[GetWHRiskAssessmentGrid]", QueryType.Procedure);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_FromDate", SqlDbType.DateTime, 0, ParameterDirection.Input, FromDate);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ToDate", SqlDbType.DateTime, 0, ParameterDirection.Input, Todate);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_WarehouseId", SqlDbType.Int, 0, ParameterDirection.Input, WarehouseId);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CompId", SqlDbType.Int, 0, ParameterDirection.Input, UserSession.GetUserSession().CompanyID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IsRMCBuss", SqlDbType.Bit, 1, ParameterDirection.Input, RMCBuss);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LoginID", SqlDbType.Int, 0, ParameterDirection.Input, UserSession.GetUserSession().LoginID);
                        DataTable dt = (DataTable)conn.ExecuteProcedure(ProcedureReturnType.DataTable);

                        if (dt != null)
                        {
                            var result = (from item in dt.AsEnumerable()
                                          select new Entities.WHAssessmentGrid()
                                          {
                                              TransId = !string.IsNullOrWhiteSpace(Convert.ToString(item["TransId"])) ? Convert.ToInt64(item["TransId"]) : -1,
                                              WarehouseId = Convert.ToInt32(item["WarehouseId"]),
                                              WarehouseName = Convert.ToString(item["Warehoue_Name"]),
                                              Area = Convert.ToString(item["Area"]),
                                              NoOfLiftVan = !string.IsNullOrWhiteSpace(Convert.ToString(item["NoOfLiftVan"])) ? Convert.ToInt32(item["NoOfLiftVan"]) : (int?)null,
                                              NoOfLiftVanStored = !string.IsNullOrWhiteSpace(Convert.ToString(item["NoOfLiftVanStored"])) ? Convert.ToInt32(item["NoOfLiftVanStored"]) :(int?)null,
                                              NoOfPeople = !string.IsNullOrWhiteSpace(Convert.ToString(item["NoOfPeople"])) ? Convert.ToInt32(item["NoOfPeople"]) : (int?)null,
                                              TotalVolCFT = !string.IsNullOrWhiteSpace(Convert.ToString(item["TotalVolCFT"])) ? Convert.ToInt64(item["TotalVolCFT"]):(Int64?)null,
                                              AuditDate = Convert.ToDateTime(item["AuditDate"]),
                                              Remark = Convert.ToString(item["Remark"]),
                                              CreatedBy = Convert.ToString(item["CreatedBy"]),
                                              CreatedDate =Convert.ToDateTime(item["CreatedDate"])
                                          }).ToList();
                            data = result.AsQueryable<Entities.WHAssessmentGrid>();
                        }
                    }
                    else
                        throw new Exception(conn.ErrorMessage);
                }
                return data;
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoggedinUserID), "WHRiskAssessmentDAL", "GetGrid", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

            //return null;


        }

        public DataSet GetDetail(int LoginID, Int64 TransId)
        {
            DataSet Dtobj = new DataSet();

            try
            {
                string query = string.Format("[Warehouse].[GetWHAssessmentDetail] @SP_TransId={0},@SP_LoginId={1}",
                CSubs.QSafeValue(Convert.ToString(TransId)), CSubs.QSafeValue(Convert.ToString(LoginID)));
                Dtobj = CSubs.GetDataSet(query);

            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoginID), "WHRiskAssessmentDAL", "GetDetail", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

            return Dtobj;
        }

        public DataTable GetWarehouseDetail(int LoginID, Int64 WarehouseId)
        {
            DataTable Dtobj = new DataTable();

            try
            {
                string query = string.Format("[Comm].[GetWarehouseDetail] @SP_WarehouseID={0},@SP_LoginID={1}",
                CSubs.QSafeValue(Convert.ToString(WarehouseId)), CSubs.QSafeValue(Convert.ToString(LoginID)));

                Dtobj = CSubs.GetDataTable(query);

            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoginID), "WHRiskAssessmentDAL", "GetWarehouseDetail", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

            return Dtobj;
        }

        public bool Insert(WHAssessmentViewModel model, int LoginID, out string result)
        {
            try
            {
                bool RMCBuss = false;
                if (UserSession.GetUserSession().BussinessLine == "NON RMC-BUSINESS")
                {
                    RMCBuss = false;
                }
                else
                {
                    RMCBuss = true;
                }

                string employeesXml = string.Empty;
                string questionsXml = string.Empty;
                string otherQuestionsXml = string.Empty;

                if (model.Participants.Length > 0)
                {
                    employeesXml = new XElement("Employees", from emp in model.Participants
                                                             select new XElement("Employee",
                                                    new XElement("EmpID", emp)

                                                )).ToString();
                }

                if (model.questions.Count > 0)
                {
                    questionsXml = new XElement("Questions", from emp in model.questions
                                                             select new XElement("Question",
                                                    new XElement("TransDetailId", emp.TransDetailId),
                                                    new XElement("QuestionId", emp.QuestionId),
                                                    new XElement("ScoreGiven", emp.ScoreGiven),
                                                    new XElement("ScoreObtained", emp.ScoreObtained),
                                                    new XElement("StatusId", emp.StatusId),
                                                    new XElement("Comments", emp.Comments)
                                                )).ToString();
                }

                if (model.otherQuestions.Count > 0)
                {
                    otherQuestionsXml = new XElement("Questions", from emp in model.otherQuestions
                                                                  select new XElement("Question",
                                                    new XElement("TransId", emp.TransId),
                                                    new XElement("TransDetailId", emp.TransDetailId),
                                                    new XElement("Parameter", emp.Parameter),
                                                    new XElement("Desired", emp.Desired),
                                                    new XElement("CategoryId", emp.CategoryId),
                                                    new XElement("ResponsibilityId", emp.ResponsibilityId),
                                                    new XElement("PriorityId", emp.PriorityId),
                                                    new XElement("Score", emp.Score),
                                                    new XElement("ScoreGiven", emp.ScoreGiven),
                                                    new XElement("ScoreObtained", emp.ScoreObtained),
                                                    new XElement("StatusId", emp.StatusId),
                                                    new XElement("Comments", emp.Comments)
                                                )).ToString();
                }

                using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {
                        
                        conn.AddCommand("[Warehouse].[AddEditWHRiskAssessment]", QueryType.Procedure);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_TransId", SqlDbType.BigInt, 0, ParameterDirection.InputOutput, model.TransId);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_WarehouseId", SqlDbType.Int, 0, ParameterDirection.Input, model.WarehouseId);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_AuditDate", SqlDbType.DateTime, 0, ParameterDirection.Input, model.AuditDate);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_NoOfPeople", SqlDbType.Int, 0, ParameterDirection.Input, model.NoOfPeople);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_NoOfLiftVan", SqlDbType.Int, 0, ParameterDirection.Input, model.NoOfLiftVan);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_TotalVolCFT", SqlDbType.Float, 0, ParameterDirection.Input, model.TotalVolCFT);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_NoOfLiftVanStored", SqlDbType.Int, 0, ParameterDirection.Input, model.NoOfLiftVanStored);
                        
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_EmployeesXml", SqlDbType.Xml, -1, ParameterDirection.Input, employeesXml);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_questionsXml", SqlDbType.Xml, -1, ParameterDirection.Input, questionsXml);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_OtherQuestionsXml", SqlDbType.Xml, -1, ParameterDirection.Input, otherQuestionsXml);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Remarks", SqlDbType.VarChar, -1, ParameterDirection.Input, CSubs.PSafeValue(model.Remark));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IsRMC", SqlDbType.Int, 0, ParameterDirection.Input, RMCBuss);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@sp_CompID", SqlDbType.Int, 0, ParameterDirection.Input, UserSession.GetUserSession().CompanyID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LoginID", SqlDbType.Int, 0, ParameterDirection.Input, UserSession.GetUserSession().LoginID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@RETURNSTATUS", SqlDbType.SmallInt, 0, ParameterDirection.ReturnValue);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_OutMsg", SqlDbType.NVarChar, -1, ParameterDirection.Output);
                        conn.ExecuteProcedure(ProcedureReturnType.SingleValue);

                        if (!conn.IsError)
                        {
                            int ReturnStatus = Convert.ToInt32(conn.GetParameterValue(ParameterOF.PROCEDURE, "@RETURNSTATUS"));
                            result = Convert.ToString(conn.GetParameterValue(ParameterOF.PROCEDURE, "@SP_OutMsg"));

                            if (ReturnStatus == 0)
                            {
                                model.TransId = Convert.ToInt64(conn.GetParameterValue(ParameterOF.PROCEDURE, "@SP_TransId"));
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
                throw new DataAccessException(Convert.ToString(LoginID), "WHRiskAssessmentDAL", "Insert", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
        }

        public bool Delete(Int64 ID, int LoginID, out string message)
        {
            message = string.Empty;
            try
            {
                using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {
                        conn.AddCommand("[Warehouse].[DeleteWHAssessment]", QueryType.Procedure);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_TransId", SqlDbType.BigInt, 0, ParameterDirection.Input, (ID));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LoginID", SqlDbType.Int, 0, ParameterDirection.Input, UserSession.GetUserSession().LoginID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@RETURNSTATUS", SqlDbType.SmallInt, 0, ParameterDirection.ReturnValue);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_MESSAGE", SqlDbType.NVarChar, -1, ParameterDirection.Output);
                        conn.ExecuteProcedure(ProcedureReturnType.SingleValue);

                        if (!conn.IsError)
                        {
                            int ReturnStatus = Convert.ToInt32(conn.GetParameterValue(ParameterOF.PROCEDURE, "@RETURNSTATUS"));
                            message = Convert.ToString(conn.GetParameterValue(ParameterOF.PROCEDURE, "@SP_MESSAGE"));

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
                throw new DataAccessException(Convert.ToString(LoginID), "WHRiskAssessmentDAL", "Delete", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

        }

        public bool InsertDocument(MoveManageViewModel SaveRate, int LoginID, out string result)
        {
            result = String.Empty;

            try
            {
                string FileName = string.Empty;
                string Extension = string.Empty;
                string FilePath = System.Configuration.ConfigurationManager.AppSettings["WarehouseAssessDMS"].ToString();
                int FileUploadCount = 0;

                if (SaveRate.jobDocUpload.file != null)
                {
                    using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString(), true, System.Data.IsolationLevel.ReadCommitted))
                    {
                        if (conn.Connect())
                        {

                            conn.AddCommand("[Warehouse].[AddEditDocFileUpload]", QueryType.Procedure);
                            conn.AddParameter(ParameterOF.PROCEDURE, "@SP_DocTypeID", SqlDbType.Int, 0, ParameterDirection.Input, SaveRate.jobDocUpload.DocTypeID);
                            conn.AddParameter(ParameterOF.PROCEDURE, "@SP_DocNameID", SqlDbType.Int, 0, ParameterDirection.Input, SaveRate.jobDocUpload.DocNameID);
                            conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Description", SqlDbType.NVarChar, 250, ParameterDirection.Input, CSubs.PSafeValue(SaveRate.jobDocUpload.DocDescription));

                            conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LOGINID", SqlDbType.Int, 0, ParameterDirection.Input, LoginID);
                            conn.AddParameter(ParameterOF.PROCEDURE, "@RETURNSTATUS", SqlDbType.SmallInt, 0, ParameterDirection.ReturnValue);
                            conn.AddParameter(ParameterOF.PROCEDURE, "@Out_Message", SqlDbType.NVarChar, 500, ParameterDirection.Output);
                            conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IdentificationID", SqlDbType.BigInt, 0, ParameterDirection.Input, SaveRate.jobDocUpload.ID);
                            conn.AddParameter(ParameterOF.PROCEDURE, "@SP_FilePath", SqlDbType.NVarChar, -1, ParameterDirection.InputOutput);
                            conn.AddParameter(ParameterOF.PROCEDURE, "@SP_FileName", SqlDbType.NVarChar, -1, ParameterDirection.Input);
                            conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Ext", SqlDbType.NVarChar, 50, ParameterDirection.Input);

                            foreach (var item in SaveRate.jobDocUpload.file)
                            {
                                if (item != null && item.ContentLength > 0)
                                {
                                    FilePath = System.Configuration.ConfigurationManager.AppSettings["WarehouseAssessDMS"].ToString();
                                    FileName = item.FileName;
                                    Extension = Path.GetExtension(item.FileName);
                                    if (!Directory.Exists(FilePath))
                                    {
                                        Directory.CreateDirectory(FilePath);
                                    }
                                    conn.SetParameterValue(ParameterOF.PROCEDURE, "@SP_FilePath", FilePath);
                                    conn.SetParameterValue(ParameterOF.PROCEDURE, "@SP_FileName", FileName);
                                    conn.SetParameterValue(ParameterOF.PROCEDURE, "@SP_Ext", Extension);
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
                                                item.SaveAs(FilePath);
                                                conn.CommitTransaction();
                                                FileUploadCount++;
                                            }
                                            catch (Exception ex)
                                            {
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
                throw new DataAccessException(Convert.ToString(LoginID), "MoveMangeDAL", "InsertDocument", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

        }

        
        public JobDocument GetDownloadFile(int FileID, int LoginID)
        {
            JobDocument job = new JobDocument();
            try
            {
                DataTable dt = CSubs.GetDataTable(string.Format("exec [Warehouse].[GetWHDocumentDetail] @SP_Fileid={0},@SP_LoginID={1}", CSubs.QSafeValue(Convert.ToString(FileID)), CSubs.QSafeValue(Convert.ToString(LoginID))));

                if (dt != null && dt.Rows.Count > 0)
                {
                    job.FileID = Convert.ToInt32(dt.Rows[0]["FileID"]);
                    job.DocTypeID = Convert.ToInt32(dt.Rows[0]["DocTypeID"]);
                    job.DocNameID = Convert.ToInt32(dt.Rows[0]["DocNameID"]);
                    job.FilePath = Convert.ToString(dt.Rows[0]["DocFilePath"]);
                    job.FileName = Convert.ToString(dt.Rows[0]["DocFileName"]);
                }

            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoginID), "MoveMangeDAL", "GetDownloadFile", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

            return job;

        }

        public bool DeleteDocument(int ID, int LoginID, out string result)
        {
            result = String.Empty;
            try
            {
                using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {

                        conn.AddCommand("[Warehouse].[DeleteDocFileUpload]", QueryType.Procedure);
                        //conn.AddParameter(ParameterOF.PROCEDURE, "@Sp_EditDeleteInsert", SqlDbType.VarChar, 1, ParameterDirection.Input, CSubs.PSafeValue("I"));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ID", SqlDbType.BigInt, 0, ParameterDirection.Input, ID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LoginID", SqlDbType.Int, 0, ParameterDirection.Input, LoginID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@RETURNSTATUS", SqlDbType.SmallInt, 0, ParameterDirection.ReturnValue);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@OutMsg", SqlDbType.NVarChar, 500, ParameterDirection.Output);
                        conn.ExecuteProcedure(ProcedureReturnType.SingleValue);

                        if (!conn.IsError)
                        {
                            int ReturnStatus = Convert.ToInt32(conn.GetParameterValue(ParameterOF.PROCEDURE, "@RETURNSTATUS"));
                            result = Convert.ToString(conn.GetParameterValue(ParameterOF.PROCEDURE, "@OutMsg"));

                            if (ReturnStatus == 0)
                            {
                                //SaveRate.MoveID = Convert.ToInt64(conn.GetParameterValue(ParameterOF.PROCEDURE, "@SP_MoveID"));
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
                return true;

            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoginID), "MoveMangeDAL", "DeleteDocument", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

        }

        public DataTable GetDocumentGrid(Int64 ID, String DocFromType, int DocTypeID = -1, int DocNameID = -1, string DocDescription = "")
        {
            DataTable Dtobj = new DataTable();

            try
            {
                string query = string.Format("exec [Warehouse].[GetDocumentGrid] @SP_ID={0},@SP_LoginID={1},@SP_DocTypeID={2},@SP_DocFromType={3},@SP_DocNameID={4},@SP_DocDescription={5}",
                    CSubs.QSafeValue(Convert.ToString(ID))
                 , CSubs.QSafeValue(Convert.ToString(UserSession.GetUserSession().LoginID))
                 , CSubs.QSafeValue(Convert.ToString(DocTypeID))
                 , CSubs.QSafeValue(DocFromType)
                 , CSubs.QSafeValue(Convert.ToString(DocNameID))
                 , CSubs.QSafeValue(DocDescription)
                 );

                Dtobj = CSubs.GetDataTable(query);
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(UserSession.GetUserSession().LoginID), "MoveMangeDAL", "GetDocumentGrid", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

            return Dtobj;

        }


        public DataSet GetReport(int LoginID, Int64 TransId)
        {
            DataSet Dtobj = new DataSet();

            try
            {
                string query = string.Format("[Warehouse].[GetWHAssessmentReport] @SP_TransId={0},@SP_LoginId={1}",
                CSubs.QSafeValue(Convert.ToString(TransId)), CSubs.QSafeValue(Convert.ToString(LoginID)));
                Dtobj = CSubs.GetDataSet(query);

            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoginID), "WHRiskAssessmentDAL", "GetReport", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

            return Dtobj;
        }
    }
}