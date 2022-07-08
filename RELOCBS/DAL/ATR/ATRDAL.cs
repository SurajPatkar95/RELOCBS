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

namespace RELOCBS.DAL.ATR
{
    public class ATRDAL
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

        public bool Insert(RELOCBS.Entities.ATRPoint model, out string result)
        {
            result = string.Empty;

            try
            {
                bool IsRMCBuss = true;

                if (UserSession.GetUserSession().BussinessLine == "NON RMC-BUSINESS")
                {
                    IsRMCBuss = false;
                }

                using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {
                        
                        conn.AddCommand("[ATR].[AddEditATRPoint]", QueryType.Procedure);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ATRPointId", SqlDbType.BigInt, 0, ParameterDirection.InputOutput, model.ATRPointId);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IssueHeading", SqlDbType.VarChar, -1, ParameterDirection.Input, CSubs.PSafeValue(model.IssueHeading));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IssueDescription", SqlDbType.VarChar, -1, ParameterDirection.Input, CSubs.PSafeValue(model.IssueDescription));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_AuditReportSource", SqlDbType.VarChar, -1, ParameterDirection.Input, CSubs.PSafeValue(model.AuditReportSource));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_MonthOfIssue", SqlDbType.DateTime, 0, ParameterDirection.Input, Convert.ToDateTime(model.MonthOfIssue));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CategoryId", SqlDbType.Int, 0, ParameterDirection.Input, model.CategoryId);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_RiskId", SqlDbType.Int, 0, ParameterDirection.Input, model.RiskId);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ComplianceStatusId", SqlDbType.Int, 0, ParameterDirection.Input, model.ComplianceStatusId);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_RepsDeptId", SqlDbType.Int, 0, ParameterDirection.Input, model.DepartmentId);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_RepsFirstId", SqlDbType.Int, 0, ParameterDirection.Input, model.FirstPersonRespLoginId);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_RepsSecondId", SqlDbType.Int, 0, ParameterDirection.Input, model.SecondPersonRespLoginId);
                        
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Remark", SqlDbType.VarChar, -1, ParameterDirection.Input, CSubs.PSafeValue(model.Remark));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CompID", SqlDbType.Int, 0, ParameterDirection.Input, UserSession.GetUserSession().CompanyID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IsRMC", SqlDbType.Bit, 0, ParameterDirection.Input, IsRMCBuss);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CloseDate", SqlDbType.DateTime, 0, ParameterDirection.Input, model.CloseDate);
                        
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LoginID", SqlDbType.Int, 0, ParameterDirection.Input, UserSession.GetUserSession().LoginID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@RETURNSTATUS", SqlDbType.SmallInt, 0, ParameterDirection.ReturnValue);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_MESSAGE", SqlDbType.NVarChar, -1, ParameterDirection.Output);
                        conn.ExecuteProcedure(ProcedureReturnType.SingleValue);

                        if (!conn.IsError)
                        {
                            int ReturnStatus = Convert.ToInt32(conn.GetParameterValue(ParameterOF.PROCEDURE, "@RETURNSTATUS"));
                            result = Convert.ToString(conn.GetParameterValue(ParameterOF.PROCEDURE, "@SP_MESSAGE"));

                            if (ReturnStatus == 0)
                            {
                                model.ATRPointId = Convert.ToInt64(conn.GetParameterValue(ParameterOF.PROCEDURE, "@SP_ATRPointId"));
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
                throw new DataAccessException(Convert.ToString(UserSession.GetUserSession().LoginID), "ATRDAL", "Insert", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
            //return true;
        }
        
        public IQueryable<RELOCBS.Entities.ATRPointGrid> GetForGrid(int LoginID, int CompID, int? DepartmentId=-1, string IssueHeading="", int? ComplianceStatusId=-1, DateTime? IssuedMonth=null)
        {

            try
            {
                string JobStr = string.Empty;
                
                string query = string.Format("EXEC [ATR].[GetATRPointForGrid] @SP_LoginID={0},@SP_IssuedMonth={1},@SP_CompID={2},@SP_Dept={3},@SP_IssueHeading={4},@SP_ComplStatusId={5}",
                Convert.ToString(LoginID)
                , CSubs.QSafeValue(IssuedMonth != null ? (Convert.ToDateTime(IssuedMonth)).ToString("dd-MMM-yyyy") : "")
                , CSubs.QSafeValue(Convert.ToString(CompID))
                , CSubs.QSafeValue(Convert.ToString(DepartmentId))
                , CSubs.QSafeValue(Convert.ToString(IssueHeading))
                , CSubs.QSafeValue(Convert.ToString(ComplianceStatusId))
                );

                DataSet dataSet = CSubs.GetDataSet(query);
                IQueryable< RELOCBS.Entities.ATRPointGrid> data;

                if (dataSet != null && dataSet.Tables.Count > 0)
                {
                    var result = (from rw in dataSet.Tables[0].AsEnumerable()
                                  select new RELOCBS.Entities.ATRPointGrid()
                                  {
                                      ATRPointId = Convert.ToInt64(rw["ATRPointId"]),
                                      IssuedMonth = Convert.ToString(rw["IssuedMonth"]),
                                      IssueHeading = Convert.ToString(rw["IssueHeading"]),
                                      IssueDescription = Convert.ToString(rw["IssueDescription"]),
                                      MonthOfIssue = Convert.ToDateTime(rw["MonthOfIssue"]),
                                      AuditReportSource = Convert.ToString(rw["AuditReportSource"]),
                                      ResponsibleDepartment = Convert.ToString(rw["ResponsibleDepartment"]),
                                      FirstPersonResponsible = Convert.ToString(rw["FirstPersonResponsible"]),
                                      SecondPersonResponsible = Convert.ToString(rw["SecondPersonResponsible"]),
                                      RiskName = Convert.ToString(rw["RiskName"]),
                                      CategoryName = Convert.ToString(rw["CategoryName"]),
                                      AuditeeStatus = Convert.ToString(rw["AuditeeStatus"]),
                                      ComplianceStatus = Convert.ToString(rw["ComplianceStatus"]),
                                      CreatedDate   = Convert.ToDateTime(rw["CreatedDate"]),
                                      CreatedBy = Convert.ToString(rw["CreatedBy"])
                                  }).ToList();
                    
                    data = result.AsQueryable<ATRPointGrid>();

                    return data;
                }

                return new List<ATRPointGrid>().AsQueryable();
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoginID), "ATRDAL", "GetForGrid", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
        }

        public DataSet GetDetailById(int LoginID, Int64 Id)
        {
            DataSet Dtobj = new DataSet();

            try
            {
                string query = string.Format("exec [ATR].[GetATRPointForDisplay] @SP_ATRPointId={0},@SP_LoginID={1}",
                 CSubs.QSafeValue(Convert.ToString(Id)), CSubs.QSafeValue(Convert.ToString(LoginID)));

                Dtobj = CSubs.GetDataSet(query);
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoginID), "ATRDAL", "GetDetailById", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

            return Dtobj;
        }

        public DataSet GetMgtResponseDetailById(int LoginID, Int64 Id)
        {
            DataSet Dtobj = new DataSet();

            try
            {
                 string query = string.Format("exec [ATR].[GetATRResponseForDisplay]  @SP_ATRPointId={0},@SP_LoginID={1}",
                 CSubs.QSafeValue(Convert.ToString(Id)), CSubs.QSafeValue(Convert.ToString(LoginID)));

                Dtobj = CSubs.GetDataSet(query);
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoginID), "ATRDAL", "GetMgtResponseDetailById", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

            return Dtobj;
        }

        public bool InsertManagementResponse(ATRPointReponse model,out string result)
        {
            int LoginID = UserSession.GetUserSession().LoginID;
            try
            {
                using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {

                        string CommFrom = "Mgt";
                        
                        conn.AddCommand("[ATR].[AddEditATRResponse]", QueryType.Procedure);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CommID", SqlDbType.Int, 0, ParameterDirection.InputOutput);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ATRPointID", SqlDbType.Int, 0, ParameterDirection.Input, model.aTRPoint.ATRPointId);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CommStatusID", SqlDbType.Int, 0, ParameterDirection.Input, model.AuditeeStatusId);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CommFrom", SqlDbType.VarChar, 10, ParameterDirection.Input, CSubs.PSafeValue(CommFrom));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CommittedDate", SqlDbType.DateTime, 0, ParameterDirection.Input, model.CommittedDate);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IsCompliance", SqlDbType.Bit, 0, ParameterDirection.Input, model.IsCompliance);

                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ATTCH", SqlDbType.Xml, 0, ParameterDirection.Input, getFilexml(model.files,model.aTRPoint.ATRPointId));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_REMARK", SqlDbType.NVarChar, -1, ParameterDirection.Input, CSubs.PSafeValue(model.MgtReponse));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_DocDescription", SqlDbType.NVarChar, 500, ParameterDirection.Input, CSubs.PSafeValue(model.DocDescription));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LOGINID", SqlDbType.Int, 0, ParameterDirection.Input, LoginID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_MESSAGE", SqlDbType.NVarChar, 500, ParameterDirection.Output);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@RETURNSTATUS", SqlDbType.SmallInt, 0, ParameterDirection.ReturnValue);
                        conn.ExecuteProcedure(ProcedureReturnType.SingleValue);

                        if (!conn.IsError)
                        {
                            string message = conn.GetParameterValue(ParameterOF.PROCEDURE, "@SP_MESSAGE").ToString();
                            int return_status = (int)conn.GetParameterValue(ParameterOF.PROCEDURE, "@RETURNSTATUS");
                            result = message;
                            if (return_status == 0)
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
                throw new DataAccessException(Convert.ToString(LoginID), "ATRDAL", "InsertManagementResponse", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }


        }

        string getFilexml(HttpPostedFileBase [] files,Int64? ATRPointId)
        {
            System.Text.StringBuilder strAttch = new System.Text.StringBuilder();

            string ErrorMessage = string.Empty;
            //if (!CSubs.ValidateFileForSecurity(files, out ErrorMessage))
            //    throw new ArgumentException(ErrorMessage);

            if (files!=null && files.Where(s=>s!=null).Count()>0 && files.Length > 0)
            {
                strAttch.Append("<ITEMS>");
                string path = System.Configuration.ConfigurationManager.AppSettings["ATRDMS"].ToString().Trim();
                //path = path.EndsWith("\\") == true ? path : string.Format(path + "{0}", "\\");
                if (!System.IO.Directory.Exists(Path.GetDirectoryName(path)))
                {
                    System.IO.Directory.CreateDirectory(Path.GetDirectoryName(path));
                }
                foreach (HttpPostedFileBase item in files)
                {
                    if (item!=null && item.ContentLength>0)
                    {
                        //// Save File on local storage
                        string FilePath;
                        FilePath = Path.Combine(path, Convert.ToString(ATRPointId));

                        string FileExtension, FileName, SaveFileName, StoreFilePath;
                        FileExtension = Path.GetExtension(item.FileName);
                        FileName = item.FileName;
                        SaveFileName = Path.GetFileNameWithoutExtension(item.FileName) + "_" + UserSession.GetUserSession().ToString() + "_" + DateTime.Now.ToString("ddMMyyyyHHmmss") + FileExtension;
                        StoreFilePath = Path.Combine(FilePath, SaveFileName);
                        if (!System.IO.Directory.Exists(FilePath))
                        {
                            System.IO.Directory.CreateDirectory(FilePath);
                        }
                        item.SaveAs(StoreFilePath);

                        #region File Xml
                        strAttch.Append("<ITEM>");
                        strAttch.Append("<C1>");
                        strAttch.Append(SaveFileName);
                        strAttch.Append("</C1>");
                        strAttch.Append("<C2>");
                        strAttch.Append(FileName);
                        strAttch.Append("</C2>");
                        strAttch.Append("<C3>");
                        strAttch.Append(FilePath);
                        strAttch.Append("</C3>");
                        strAttch.Append("</ITEM>");
                        #endregion File
                    }
                }

                strAttch.Append("</ITEMS>");
            }
            return strAttch.ToString();
        }

        public bool CheckIsHO(int LoginID)
        {
            try
            {
                
                string query = string.Format("exec [ATR].[CheckATRHO] @SP_LoginID={0}",CSubs.QSafeValue(Convert.ToString(LoginID)));

               DataTable Dtobj = CSubs.GetDataTable(query);

                if (Dtobj!=null && Dtobj.Rows.Count>0)
                {
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoginID), "ATRDAL", "CheckIsHO", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

        }

        public ATRPointDoc GetDownloadFile(int FileID, int LoginID)
        {
            ATRPointDoc job = new ATRPointDoc();
            try
            {
                DataTable dt = CSubs.GetDataTable(string.Format("exec [ATR].[GetDocumentDetail] @SP_Fileid={0},@SP_LoginID={1}", CSubs.QSafeValue(Convert.ToString(FileID)), CSubs.QSafeValue(Convert.ToString(LoginID))));

                if (dt != null && dt.Rows.Count > 0)
                {
                    job.FileID = Convert.ToInt32(dt.Rows[0]["FileID"]);
                    job.FilePath = Convert.ToString(dt.Rows[0]["DocFilePath"]);
                    job.FileName = Convert.ToString(dt.Rows[0]["DocFileName"]);
                }

            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoginID), "ATRDAL", "GetDownloadFile", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

            return job;

        }

        public bool DeleteDocument(Int64 ID, int LoginID, out string result)
        {
            result = String.Empty;
            try
            {
                using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {

                        conn.AddCommand("[ATR].[DeleteDocFileUpload]", QueryType.Procedure);
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
                throw new DataAccessException(Convert.ToString(LoginID), "ATRDAL", "DeleteDocument", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

        }


    }
}