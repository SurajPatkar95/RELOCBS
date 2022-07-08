using RELOCBS.Common.ExceptionHandling;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using RELOCBS.Entities;
using RELOCBS.App_Code;
using RELOCBS.Utility;

namespace RELOCBS.DAL.Complaint
{
    public class ComplaintDAL
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

        public bool Insert(RELOCBS.Entities.Complaints model, out string result)
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

                        conn.AddCommand("[Complaints].[AddEditComplaint]", QueryType.Procedure);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ComplaintId", SqlDbType.BigInt, 0, ParameterDirection.InputOutput, model.ComplaintId);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_EnqID", SqlDbType.Int, 0, ParameterDirection.Input, model.EnqID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_EnqDetail_ID", SqlDbType.BigInt, 0, ParameterDirection.Input, model.EnqDetail_ID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_MoveID", SqlDbType.BigInt, 0, ParameterDirection.Input, model.MoveID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ClassificationId", SqlDbType.Int, 0, ParameterDirection.Input, model.ClassificationId);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Description", SqlDbType.VarChar, -1, ParameterDirection.Input, CSubs.PSafeValue(model.Description));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Logger_Name", SqlDbType.VarChar, 250, ParameterDirection.Input, CSubs.PSafeValue(model.Logger_Name));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Logger_Email", SqlDbType.VarChar, 500, ParameterDirection.Input, CSubs.PSafeValue(model.Logger_Email));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Logger_Phone", SqlDbType.VarChar, 50, ParameterDirection.Input, CSubs.PSafeValue(model.Logger_Phone));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Logger_Mobile", SqlDbType.VarChar, 50, ParameterDirection.Input, CSubs.PSafeValue(model.Logger_Mobile));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_StatusId", SqlDbType.Int, 0, ParameterDirection.Input, model.StatusId);

                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_SourceId", SqlDbType.Int, 0, ParameterDirection.Input, model.SourceId);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CompID", SqlDbType.Int, 0, ParameterDirection.Input, UserSession.GetUserSession().CompanyID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IsRMC", SqlDbType.Bit, 0, ParameterDirection.Input, IsRMCBuss);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LoginID", SqlDbType.Int, 0, ParameterDirection.Input, UserSession.GetUserSession().EmpID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@RETURNSTATUS", SqlDbType.SmallInt, 0, ParameterDirection.ReturnValue);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_MESSAGE", SqlDbType.NVarChar, -1, ParameterDirection.Output);
                        conn.ExecuteProcedure(ProcedureReturnType.SingleValue);

                        if (!conn.IsError)
                        {
                            int ReturnStatus = Convert.ToInt32(conn.GetParameterValue(ParameterOF.PROCEDURE, "@RETURNSTATUS"));
                            result = Convert.ToString(conn.GetParameterValue(ParameterOF.PROCEDURE, "@SP_MESSAGE"));

                            if (ReturnStatus == 0)
                            {
                                model.ComplaintId = Convert.ToInt64(conn.GetParameterValue(ParameterOF.PROCEDURE, "@SP_ComplaintId"));
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
                throw new DataAccessException(Convert.ToString(UserSession.GetUserSession().LoginID), "ComplaintDAL", "Insert", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
            //return true;
        }

        public IQueryable<RELOCBS.Entities.ComplaintGrid> GetForGrid(int LoginID, int CompID,bool IsRmc,int? classificationId,int? statusId,string shipper,string loggerName,string filterType,string filterValue)
        {

            try
            {
                string JobStr = string.Empty;

                string query = string.Format("EXEC [Complaints].[GetComplaintForGrid]  @SP_LoginID={0},@SP_IsRMC={1},@SP_CompID={2},@SP_ClassificationId={3},@SP_FilterValue={4},@SP_FilterName={5},@SP_LoggerName={6},@SP_ShipperName={7},@SP_StatusId={8}",
                Convert.ToString(LoginID)
                , IsRmc
                , CSubs.QSafeValue(Convert.ToString(CompID))
                , CSubs.QSafeValue(Convert.ToString(classificationId))
                , CSubs.QSafeValue(Convert.ToString(filterValue))
                , CSubs.QSafeValue(Convert.ToString(filterType))
                , CSubs.QSafeValue(Convert.ToString(loggerName))
                , CSubs.QSafeValue(Convert.ToString(shipper))
                , CSubs.QSafeValue(Convert.ToString(statusId))
                );

                DataSet dataSet = CSubs.GetDataSet(query);
                IQueryable<RELOCBS.Entities.ComplaintGrid> data;

                if (dataSet != null && dataSet.Tables.Count > 0)
                {
                    var result = (from rw in dataSet.Tables[0].AsEnumerable()
                                  select new RELOCBS.Entities.ComplaintGrid()
                                  {
                                      ComplaintId = Convert.ToInt64(rw["ComplaintId"]),
                                      EnqNo = Convert.ToString(rw["EnqNo"]),
                                      JoBNo = Convert.ToString(rw["JobID"]),
                                      Classification = Convert.ToString(rw["ClassificationName"]),
                                      Source = Convert.ToString(rw["SourceName"]),
                                      Status = Convert.ToString(rw["StatusName"]),
                                      Description = Convert.ToString(rw["Description"]),
                                      Logger_Name = Convert.ToString(rw["Logger_Name"]),
                                      Logger_Email = Convert.ToString(rw["Logger_Email"]),
                                      Logger_Phone = Convert.ToString(rw["Logger_Phone"]),
                                      Logger_Mobile = Convert.ToString(rw["Logger_Mobile"]),
                                      Shipper = Convert.ToString(rw["ShipperName"]),
                                      Serviceline = Convert.ToString(rw["ServiceLine"]),
                                      AccountName = Convert.ToString(rw["AccountName"]),
                                      AgentName = Convert.ToString(rw["AgentName"]),
                                      Mode = Convert.ToString(rw["Mode"]),
                                      LastCreatedDate = Convert.ToDateTime(rw["CreatedDate"]),
                                      LastCreatedBy = Convert.ToString(rw["CreatedBy"]),
                                      FromLocation = Convert.ToString(rw["FromCity"]),
                                      ToLocation = Convert.ToString(rw["ToCity"]),
                                  }).ToList();

                    data = result.AsQueryable<ComplaintGrid>();

                    return data;
                }

                return new List<ComplaintGrid>().AsQueryable();
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoginID), "ComplaintDAL", "GetForGrid", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
        }

        public DataSet GetDetailById(int LoginID, Int64 Id)
        {
            DataSet Dtobj = new DataSet();

            try
            {
                string query = string.Format("exec [complaints].[GetCompalintForDisplay] @SP_ComplaintId={0},@SP_LoginID={1}",
                 CSubs.QSafeValue(Convert.ToString(Id)), CSubs.QSafeValue(Convert.ToString(LoginID)));

                Dtobj = CSubs.GetDataSet(query);
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoginID), "ComplaintDAL", "GetDetailById", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

            return Dtobj;
        }

        public DataTable GetEnqJobDetailById(int EmpId, Int64 EnqDetailId, Int64 MoveId)
        {

            DataTable Dtobj = new DataTable();

            try
            {
                int CompId = UserSession.GetUserSession().CompanyID;
                bool IsRMCBUss = UserSession.GetUserSession().BussinessLine != "NON RMC-BUSINESS";
                string query = string.Format("exec [complaints].[getEnqJobDetail]  @SP_EnqDetailId={0},@SP_LoginId={1},@SP_MoveId={2},@SP_CompId={3},@SP_IsRmcBuss={4}",
                 CSubs.QSafeValue(Convert.ToString(EnqDetailId)), CSubs.QSafeValue(Convert.ToString(EmpId)), MoveId, CompId, IsRMCBUss);

                Dtobj = CSubs.GetDataTable(query);
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(UserSession.GetUserSession().EmpID), "ComplaintDAL", "GetEnqJobDetailById", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

            return Dtobj;
        }

        public bool Delete(int id, out string result)
        {
            result = string.Empty;
            var userVM = UserSession.GetUserSession();

            try
            {

                using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {

                        conn.AddCommand("[Complaints].[DeleteComplaint]", QueryType.Procedure);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ComplaintId", SqlDbType.BigInt, 0, ParameterDirection.InputOutput, id);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LoginID", SqlDbType.Int, 0, ParameterDirection.Input, userVM.EmpID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@RETURNSTATUS", SqlDbType.SmallInt, 0, ParameterDirection.ReturnValue);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_MESSAGE", SqlDbType.NVarChar, -1, ParameterDirection.Output);
                        conn.ExecuteProcedure(ProcedureReturnType.SingleValue);

                        if (!conn.IsError)
                        {
                            int ReturnStatus = Convert.ToInt32(conn.GetParameterValue(ParameterOF.PROCEDURE, "@RETURNSTATUS"));
                            result = Convert.ToString(conn.GetParameterValue(ParameterOF.PROCEDURE, "@SP_MESSAGE"));

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
                throw new DataAccessException(Convert.ToString(UserSession.GetUserSession().EmpID), "ComplaintDAL", "Delete", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
        }
    }
}