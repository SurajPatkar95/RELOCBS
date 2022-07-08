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

namespace RELOCBS.DAL.VendorContract
{
    public class VendorContractDAL
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

        public IQueryable<Entities.VendorContractGrid> GetGrid(string VendorName, string MasterCode, string SubCode,bool RMCBuss)
        {
            int LoggedinUserID = UserSession.GetUserSession().LoginID;
            try
            {

                IQueryable<Entities.VendorContractGrid> data = null;

                using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {
                        conn.AddCommand("[VC].[GetVendorContractGrid]", QueryType.Procedure);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_VendorName", SqlDbType.VarChar, 250, ParameterDirection.Input, CSubs.PSafeValue(VendorName));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_MasterCode", SqlDbType.VarChar, 250, ParameterDirection.Input, CSubs.PSafeValue(MasterCode));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_SubCode", SqlDbType.VarChar, 250, ParameterDirection.Input, CSubs.PSafeValue(MasterCode));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CompId", SqlDbType.Int, 0, ParameterDirection.Input, UserSession.GetUserSession().CompanyID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IsRMCBuss", SqlDbType.Bit, 1, ParameterDirection.Input, RMCBuss);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LoginID", SqlDbType.Int, 0, ParameterDirection.Input, UserSession.GetUserSession().LoginID);
                        DataTable dt = (DataTable)conn.ExecuteProcedure(ProcedureReturnType.DataTable);

                        if (dt != null)
                        {
                            var result = (from item in dt.AsEnumerable()
                                          select new Entities.VendorContractGrid()
                                          {
                                              VContractId = Convert.ToInt32(item["VContractId"]),
                                              BusinessUnitName= Convert.ToString(item["BusinessUnitName"]),
                                              VendorCity = Convert.ToString(item["VendorCityName"]),
                                              VendorName = Convert.ToString(item["VendorName"]),
                                              Account_MasterCode = Convert.ToString(item["Account_MasterCode"]),
                                              Account_SubCode = Convert.ToString(item["Account_SubCode"]),
                                              ServiceCategoryName = Convert.ToString(item["ServiceCategoryName"]),
                                              BranchName = Convert.ToString(item["BranchName"]),
                                              Finance_EmpName = Convert.ToString(item["Finance_EmpName"]),
                                              Contact_PersonName = Convert.ToString(item["Contact_PersonName"]),
                                              Contact_PersonEmail = Convert.ToString(item["Contact_PersonEmail"]),
                                              Contact_PersonMobile = Convert.ToString(item["Contact_PersonMobile"]),
                                              Commencement_Date = Convert.ToDateTime(item["Commencement_Date"]),
                                              ExpiryDate = Convert.ToDateTime(item["ExpiryDate"]),
                                              GSTNo = Convert.ToString(item["GSTNo"]),
                                              IsMSME = Convert.ToString(item["IsMSME"]),
                                              Owner_Name = Convert.ToString(item["Owner_Name"]),
                                              PAN = Convert.ToString(item["PAN"]),
                                              LastModifiedBy = Convert.ToString(item["LastModifiedBy"]),
                                              LastModifiedDate = Convert.ToDateTime(item["LastModifiedDate"]),
                                              //Account_Reco_LastCompleteDate = item["Account_Reco_LastCompleteDate"] != DBNull.Value ? Convert.ToDateTime(item["Account_Reco_LastCompleteDate"]) : (DateTime?)null,
                                              //Certificate_LastDuesDate = item["Certificate_LastDuesDate"] != DBNull.Value ? Convert.ToDateTime(item["Certificate_LastDuesDate"]) : (DateTime?)null,
                                              //GSTR2A_Reco_LastCompleteDate = item["GSTR2A_Reco_LastCompleteDate"] != DBNull.Value ? Convert.ToDateTime(item["GSTR2A_Reco_LastCompleteDate"]) : (DateTime?)null,
                                          }).ToList();
                            data = result.AsQueryable<Entities.VendorContractGrid>();
                        }
                    }
                    else
                        throw new Exception(conn.ErrorMessage);
                }
                return data;
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoggedinUserID), "VendorContractDAL", "GetGrid", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

            //return null;


        }

        public DataTable GetDetail(int LoginID, Int64 TransId)
        {
            DataTable Dtobj = new DataTable();

            try
            {
                string query = string.Format("[VC].[GetVendorContractDetail] @SP_VContractId={0},@SP_LoginId={1}",
                CSubs.QSafeValue(Convert.ToString(TransId)), CSubs.QSafeValue(Convert.ToString(LoginID)));
                Dtobj = CSubs.GetDataTable(query);

            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoginID), "VendorContractDAL", "GetDetail", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
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
                throw new DataAccessException(Convert.ToString(LoginID), "VendorContractDAL", "GetWarehouseDetail", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

            return Dtobj;
        }

        public bool Insert(VendorContractModel model, int LoginID, out string result)
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

                using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {

                        conn.AddCommand("[VC].[AddEditVendorContract]", QueryType.Procedure);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_VContractId", SqlDbType.BigInt, 0, ParameterDirection.InputOutput, model.VContractId);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_BusinessUnitId", SqlDbType.Int, 0, ParameterDirection.Input, model.BusinessUnitId);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Account_MasterCode", SqlDbType.VarChar, 50, ParameterDirection.Input, CSubs.PSafeValue(model.Account_MasterCode));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Account_SubCode", SqlDbType.VarChar, 50, ParameterDirection.Input, CSubs.PSafeValue(model.Account_SubCode));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_BranchId", SqlDbType.Int, 0, ParameterDirection.Input, model.BranchId);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_VendorName", SqlDbType.VarChar, 250, ParameterDirection.Input, CSubs.PSafeValue(model.VendorName));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_VendorCityId", SqlDbType.Int, 0, ParameterDirection.Input, model.VendorCityId);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Finance_EmpId", SqlDbType.Int, 0, ParameterDirection.Input, model.Finance_EmpId);

                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_PAN", SqlDbType.VarChar, 50, ParameterDirection.Input, CSubs.PSafeValue(model.PAN));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_GSTNo", SqlDbType.VarChar, 50, ParameterDirection.Input, CSubs.PSafeValue(model.GSTNo));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ServiceCategoryId", SqlDbType.Int, 0, ParameterDirection.Input, model.ServiceCategoryId);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IsMSME", SqlDbType.VarChar, 10, ParameterDirection.Input, CSubs.PSafeValue(model.IsMSME));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Owner_Name", SqlDbType.VarChar, 250, ParameterDirection.Input, CSubs.PSafeValue(model.Owner_Name));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Contact_PersonName", SqlDbType.VarChar, 250, ParameterDirection.Input, CSubs.PSafeValue(model.Contact_PersonName));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Contact_PersonEmail", SqlDbType.VarChar, 250, ParameterDirection.Input, CSubs.PSafeValue(model.Contact_PersonEmail));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Contact_PersonMobile", SqlDbType.VarChar, 50, ParameterDirection.Input, CSubs.PSafeValue(model.Contact_PersonMobile));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Commencement_Date", SqlDbType.DateTime, 0, ParameterDirection.Input, model.Commencement_Date);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ExpiryDate", SqlDbType.DateTime, 0, ParameterDirection.Input, model.ExpiryDate);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Account_Reco_LastCompleteDate", SqlDbType.DateTime, 0, ParameterDirection.Input, model.Account_Reco_LastCompleteDate);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Certificate_LastDuesDate", SqlDbType.DateTime, 0, ParameterDirection.Input, model.Certificate_LastDuesDate);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_GSTR2A_Reco_LastCompleteDate", SqlDbType.DateTime, 0, ParameterDirection.Input, model.GSTR2A_Reco_LastCompleteDate);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IsRMC", SqlDbType.Int, 0, ParameterDirection.Input, RMCBuss);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@sp_CompID", SqlDbType.Int, 0, ParameterDirection.Input, UserSession.GetUserSession().CompanyID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LoginID", SqlDbType.Int, 0, ParameterDirection.Input, UserSession.GetUserSession().LoginID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_VCStatusID", SqlDbType.Int, 0, ParameterDirection.Input, model.VCStatusID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@RETURNSTATUS", SqlDbType.SmallInt, 0, ParameterDirection.ReturnValue);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@Out_Message", SqlDbType.NVarChar, -1, ParameterDirection.Output);
                        conn.ExecuteProcedure(ProcedureReturnType.SingleValue);

                        if (!conn.IsError)
                        {
                            int ReturnStatus = Convert.ToInt32(conn.GetParameterValue(ParameterOF.PROCEDURE, "@RETURNSTATUS"));
                            result = Convert.ToString(conn.GetParameterValue(ParameterOF.PROCEDURE, "@Out_Message"));

                            if (ReturnStatus == 0)
                            {
                                model.VContractId = Convert.ToInt32(conn.GetParameterValue(ParameterOF.PROCEDURE, "@SP_VContractId"));
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
                throw new DataAccessException(Convert.ToString(LoginID), "VendorContractDAL", "Insert", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
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
                        conn.AddCommand("[VC].[DeleteVendorContract] ", QueryType.Procedure);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_VContractID", SqlDbType.BigInt, 0, ParameterDirection.Input, (ID));
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
                throw new DataAccessException(Convert.ToString(LoginID), "VendorContractDAL", "Delete", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

        }
        
        public DataTable GetDocumentGrid(Int64 ID, String DocFromType, int DocTypeID = -1, int DocNameID = -1, string DocDescription = "")
        {
            DataTable Dtobj = new DataTable();

            try
            {
                string query = string.Format("exec [VC].[GetDocumentGrid]  @SP_ID={0},@SP_LoginID={1},@SP_DocTypeID={2},@SP_DocFromType={3},@SP_DocNameID={4},@SP_DocDescription={5}",
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
                throw new DataAccessException(Convert.ToString(UserSession.GetUserSession().LoginID), "VendorContractDAL", "GetDocumentGrid", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

            return Dtobj;

        }

        public VendorContractDocuments GetDownloadFile(int FileID, int LoginID)
        {
            VendorContractDocuments job = new VendorContractDocuments();
            try
            {
                DataTable dt = CSubs.GetDataTable(string.Format("exec [VC].[GetDocumentDetail] @SP_DocID={0},@SP_LoginID={1}", CSubs.QSafeValue(Convert.ToString(FileID)), CSubs.QSafeValue(Convert.ToString(LoginID))));

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
                throw new DataAccessException(Convert.ToString(LoginID), "VendorContractDAL", "GetDownloadFile", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
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

                        conn.AddCommand("[VC].[DeleteDocFileUpload]", QueryType.Procedure);
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
                throw new DataAccessException(Convert.ToString(LoginID), "VendorContractDAL", "DeleteDocument", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

        }

        public bool InsertDocument(VendorContractDocUpload docModel, int LoginID, out string result)
        {
            result = String.Empty;

            try
            {
                string FileName = string.Empty;
                string Extension = string.Empty;
                string FilePath = System.Configuration.ConfigurationManager.AppSettings["VendorContractDMS"].ToString();
                int FileUploadCount = 0;

                if (docModel.file != null)
                {
                    using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString(), true, System.Data.IsolationLevel.ReadCommitted))
                    {
                        if (conn.Connect())
                        {

                            conn.AddCommand("[VC].[AddEditDocFileUpload]", QueryType.Procedure);
                            conn.AddParameter(ParameterOF.PROCEDURE, "@SP_VContractID", SqlDbType.BigInt, 0, ParameterDirection.Input, docModel.ID);
                            conn.AddParameter(ParameterOF.PROCEDURE, "@SP_DocTypeID", SqlDbType.Int, 0, ParameterDirection.Input, docModel.DocTypeID);
                            conn.AddParameter(ParameterOF.PROCEDURE, "@SP_DocNameID", SqlDbType.Int, 0, ParameterDirection.Input, docModel.DocNameID);
                            conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Description", SqlDbType.NVarChar, 250, ParameterDirection.Input, CSubs.PSafeValue(docModel.DocDescription));
                            conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ExpiryDate", SqlDbType.Date, 0, ParameterDirection.Input, docModel.ExpiryDate);
                            conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LOGINID", SqlDbType.Int, 0, ParameterDirection.Input, LoginID);
                            conn.AddParameter(ParameterOF.PROCEDURE, "@RETURNSTATUS", SqlDbType.SmallInt, 0, ParameterDirection.ReturnValue);
                            conn.AddParameter(ParameterOF.PROCEDURE, "@Out_Message", SqlDbType.NVarChar, 500, ParameterDirection.Output);
                            conn.AddParameter(ParameterOF.PROCEDURE, "@SP_FilePath", SqlDbType.NVarChar, 500, ParameterDirection.InputOutput);
                            conn.AddParameter(ParameterOF.PROCEDURE, "@SP_FileName", SqlDbType.NVarChar, 250, ParameterDirection.Input);
                            conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Ext", SqlDbType.NVarChar, 50, ParameterDirection.Input);

                            foreach (var file in docModel.file)
                            {
                                if (file != null && file.ContentLength > 0)
                                {
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
                                            FilePath = Convert.ToString(conn.GetParameterValue(ParameterOF.PROCEDURE, "@SP_FilePath"));

                                            try
                                            {
                                                file.SaveAs(FilePath);
                                                conn.CommitTransaction();
                                                FileUploadCount++;
                                            }
                                            catch (Exception ex)
                                            {
                                                throw new ArgumentException("Unable to save File");
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
                                        throw new ArgumentException(conn.ErrorMessage);
                                    }

                                }


                            }


                        }
                        else
                            throw new ArgumentException(conn.ErrorMessage);
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
                    throw new ArgumentException("File Not Found");


            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoginID), "VendorContractDAL", "InsertDocument", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

        }
    }
}