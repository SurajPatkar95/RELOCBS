using Newtonsoft.Json;
using RELOCBS.App_Code;
using RELOCBS.Common.ExceptionHandling;
using RELOCBS.Entities;
using RELOCBS.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace RELOCBS.DAL.Dashboard
{
    public class DashboardDAL
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

        public DataSet GetSurveyDashboard(int LoginID, int CompId, bool IsRmc, ScheduleSurveyDashboard model)
        {
            try
            {  
                string query = string.Format("exec [Report].[ScheduleSurveyDashboard] @SP_ForMonthDate={0},@SP_BranchId={1},@SP_LoginID={2},@SP_CompID={3},@SP_IsRmcBuss={4}",
                CSubs.QSafeValue(Convert.ToString(model.ForMonthDate)), CSubs.QSafeValue(Convert.ToString(model.BranchId)), CSubs.QSafeValue(Convert.ToString(LoginID)),
                CompId, IsRmc);

                DataSet Dtobj = CSubs.GetDataSet(query);
                return Dtobj;
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(UserSession.GetUserSession().LoginID), "DashboardDAL", "GetSurveyDashboard", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
        }

        public IEnumerable<SurveyorEvent> GetSurveyorEvents(int LoginID, int CompanyID,int id,DateTime fromDate)
        {
            try
            {
                string query = string.Format("exec [Report].[ScheduleSurveyorDetail] @SP_ForMonthDate={0},@SP_EmpId={1},@SP_LoginID={2},@SP_CompID={3}",
               CSubs.QSafeValue(fromDate.ToString()),CSubs.QSafeValue(Convert.ToString(id)), LoginID,
                CompanyID);

                DataTable dataTable = CSubs.GetDataTable(query);

                var result = (from rw in dataTable.AsEnumerable()
                              select new SurveyorEvent()
                              {
                                  EnqNo = Convert.ToString(rw["EnqNo"]),
                                  SchTime = Convert.ToString(rw["SchSurveyDate"]),
                                  Shipper = Convert.ToString(rw["Shipper"]),
                                  Surveyor = Convert.ToString(rw["Surveyor"]),
                              }).ToList();

                return result;
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(UserSession.GetUserSession().LoginID), "DashboardDAL", "GetSurveyorEvents", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
        }

        public DataSet GetCrewUtilization(int LoginID, int CompanyID,bool IsRmcBuss,CrewUtilizationDashboard model)
        {
            try
            {
                string query = string.Format("exec [Report].[CrewUtilizationDashboard] @SP_FromMonthDate={0},@SP_ToMonthDate={1},@SP_LoginID={2},@SP_CompID={3},@SP_IsRmcBuss={4}",
                CSubs.QSafeValue(Convert.ToString(model.FromMonthDate)), CSubs.QSafeValue(Convert.ToString(model.ToMonthDate)), CSubs.QSafeValue(Convert.ToString(LoginID)),
                CompanyID, IsRmcBuss);

                DataSet Dtobj = CSubs.GetDataSet(query);
                return Dtobj;
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(UserSession.GetUserSession().LoginID), "DashboardDAL", "GetSurveyDashboard", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
        }

        public DataSet GetDataPointList(int UserID)
        {
            try
            {
                string query = string.Format("exec [Dashboard].[GetDashboardChartData] @SP_UserID={0}",
                CSubs.QSafeValue(Convert.ToString(UserID)));

                DataSet Dtobj = CSubs.GetDataSet(query);
                return Dtobj;
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(UserSession.GetUserSession().LoginID), "DashboardDAL", "GetSurveyDashboard", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
        }


        public DataSet GetFollowUpDetails(string UserID,string Status, string flag)
        {
            try
            {
                string query = string.Format("exec [Dashboard].[getFollowupDetForDate] @SP_LoginID={0},@SP_UserID={1},@SP_DateOrStage={2},@SP_StageID={3},@SP_FollowupDate={4}",
                CSubs.QSafeValue(Convert.ToString(UserSession.GetUserSession().LoginID)),CSubs.QSafeValue(Convert.ToString(UserID)),
                CSubs.QSafeValue(Convert.ToString(flag)),CSubs.QSafeValue(Convert.ToString(Status.Split('-')[0])),
                CSubs.QSafeValue(Convert.ToString(""))
                //CSubs.QSafeValue(Convert.ToString(Status))
                );

                DataSet Dtobj = CSubs.GetDataSet(query);
                return Dtobj;
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(UserSession.GetUserSession().LoginID), "DashboardDAL", "getFollowupDetForDate", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
        }


        public bool SaveFollowup(Entities.JobDetail ObjSave, int LoginID,out string result)
        {
            result = String.Empty;

            try
            {
                //XDocument doc = new XDocument();
                
                    var ReasonXML = ObjSave.Reason != null ? new XElement("ReasonDetails", ObjSave.Reason.Select(x => new XElement("ReasonDetail", new XElement("ReasonID", x)))) : new XElement("ReasonDetails");
                    
                    //System.Xml.Linq.XNode node = JsonConvert.DeserializeXNode(doc, "ReasonDetails");
                    //string ReasonXml = node.ToString();

                
                using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {
                        conn.AddCommand("[Dashboard].[AddEditDashboardFollowup]", QueryType.Procedure);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_TransactionID", SqlDbType.BigInt, 0, ParameterDirection.InputOutput, ObjSave.TransactionID == null ? 0 : ObjSave.TransactionID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_JobID", SqlDbType.BigInt, 0, ParameterDirection.Input, ObjSave.JobNo);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_JobStatusID", SqlDbType.Int, 500, ParameterDirection.Input, ObjSave.JobStatusID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_NextFollowUpDate", SqlDbType.DateTime, -1, ParameterDirection.Input, ObjSave.FollowupDate);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_NextFollowUpBy", SqlDbType.Int, 0, ParameterDirection.Input, LoginID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_OrderNo", SqlDbType.VarChar, 500, ParameterDirection.Input, ObjSave.OrderNo);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_DatesCaptured", SqlDbType.VarChar, 500, ParameterDirection.Input, null);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LoginID", SqlDbType.BigInt, -1, ParameterDirection.Input, LoginID);
                        
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Remarks", SqlDbType.VarChar, -1, ParameterDirection.Input, ObjSave.Remark);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IsClosed", SqlDbType.Bit, -1, ParameterDirection.Input, ObjSave.CancelFollowUP);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ReasonDetails", SqlDbType.Xml, -1, ParameterDirection.Input, Convert.ToString(ReasonXML));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@RETURNSTATUS", SqlDbType.SmallInt, 0, ParameterDirection.ReturnValue);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@Out_Message", SqlDbType.NVarChar, 500, ParameterDirection.Output);

                        conn.ExecuteProcedure(ProcedureReturnType.SingleValue);

                        if (!conn.IsError)
                        {
                            int ReturnStatus = Convert.ToInt32(conn.GetParameterValue(ParameterOF.PROCEDURE, "@RETURNSTATUS"));
                            result = Convert.ToString(conn.GetParameterValue(ParameterOF.PROCEDURE, "@Out_Message"));

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
                throw new DataAccessException(Convert.ToString(LoginID), "DashboardDAL", "SaveFollowup", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }


        }

    }
}