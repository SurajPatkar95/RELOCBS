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

namespace RELOCBS.DAL
{
    public class VehicleKmsDAL
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


        public IQueryable<Entities.VehicleKmsGrid> GetGrid(DateTime? FromDate, DateTime? Todate, bool? RMCBuss = false, int? BranchID=null, int? VehicleNo = null, string Shipper = null, string JobNo = null)
        {
            int LoggedinUserID = UserSession.GetUserSession().LoginID;
            try
            {

                IQueryable<Entities.VehicleKmsGrid> data = null;

                using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {
                        conn.AddCommand("[Warehouse].[GetVehicleKmsGrid]", QueryType.Procedure);

                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_FromDate", SqlDbType.DateTime, 0, ParameterDirection.Input, FromDate);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ToDate", SqlDbType.DateTime, 0, ParameterDirection.Input, Todate);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_VehicleNo", SqlDbType.Int, 0, ParameterDirection.Input, VehicleNo);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_BranchId", SqlDbType.Int, 0, ParameterDirection.Input, BranchID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CompId", SqlDbType.Int, 0, ParameterDirection.Input, UserSession.GetUserSession().CompanyID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IsRMCBuss", SqlDbType.Bit, 1, ParameterDirection.Input, RMCBuss);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LoginID", SqlDbType.Int, 0, ParameterDirection.Input, UserSession.GetUserSession().LoginID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Shipper", SqlDbType.VarChar, 250, ParameterDirection.Input, CSubs.PSafeValue(Shipper));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_JobNo", SqlDbType.VarChar, 50, ParameterDirection.Input, CSubs.PSafeValue(JobNo));
                        DataTable dt = (DataTable)conn.ExecuteProcedure(ProcedureReturnType.DataTable);

                        if (dt != null)
                        {
                            var result = (from rw in dt.AsEnumerable()
                                          select new Entities.VehicleKmsGrid()
                                          {
                                              VehicleKmsID = Convert.ToInt64(rw["VehicleKmsId"]),
                                              VehicleNo = Convert.ToString(rw["Vehicle_No"]),
                                              BranchName = Convert.ToString(rw["BranchName"]),
                                              OdometerDate = Convert.ToDateTime(rw["OdometerDate"]) ,
                                              StartOdometer = Convert.ToInt64(rw["StartOdometer"]),
                                              EndOdometer = Convert.ToInt64(rw["EndOdometer"]),
                                              Remarks = Convert.ToString(rw["Remarks"]),
                                              CreatedBy = Convert.ToString(rw["ModifiedBy"]),
                                              CreatedDate = Convert.ToDateTime(rw["ModifiedDate"]),
                                              TotalOdometer = Convert.ToInt64(rw["TotalOdometer"]),
                                          }).ToList();
                            data = result.AsQueryable<Entities.VehicleKmsGrid>();
                        }
                    }
                    else
                        throw new Exception(conn.ErrorMessage);
                }
                return data;
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoggedinUserID), "VehicleKmsDAL", "GetGrid", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

            //return null;


        }

        public DataSet GetDetail(int LoginID, Int64 VehicleKmsID)
        {
            DataSet Dtobj = new DataSet();

            try
            {
                string query = string.Format("[Warehouse].[GetVehicleKmsDetails] @SP_VehicleKmsID={0},@SP_LoginID={1}",
                CSubs.QSafeValue(Convert.ToString(VehicleKmsID)),CSubs.QSafeValue(Convert.ToString(LoginID)));
                Dtobj = CSubs.GetDataSet(query);

            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoginID), "VehicleKmsDAL", "GetDetail", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

            return Dtobj;
        }

        public DataTable GetJobDetail(int LoginID, Int64 MoveID)
        {
            DataTable Dtobj = new DataTable();

            try
            {
                string query = string.Format("[Warehouse].[GetVehicleKmsDetails] @SP_MoveID={0},@SP_OnlyJobInfo=1",
                CSubs.QSafeValue(Convert.ToString(MoveID)));

                Dtobj = CSubs.GetDataTable(query);

            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoginID), "VehicleKmsDAL", "GetJobDetail", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

            return Dtobj;
        }

        public bool Insert(VehicleKms model, int LoginID, out string result)
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

                        string kmsJobsXml = string.Empty;
                        string travelLocationsXml = string.Empty;

                        if (model.kmsJobs.Count > 0)
                        {
                            kmsJobsXml = new XElement("Jobs", from emp in model.kmsJobs
                                                                      select new XElement("Job",
                                                             new XElement("MoveID", emp.MoveID),
                                                             new XElement("Remark", emp.Remark)

                                                         )).ToString();

                            
                        }

                        if (model.travelLocations.Count > 0)
                        {
                            travelLocationsXml = new XElement("travelLocations", from emp in model.travelLocations
                                                                             select new XElement("travelLocation",
                                                                    new XElement("FromLoc", emp.FromLocation),
                                                                    new XElement("ToLoc", emp.ToLocation),
                                                                    new XElement("Remark", emp.Remark)
                                                                )).ToString();
                        }

                        conn.AddCommand("[Warehouse].[AddEditVehicleKms]", QueryType.Procedure);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_VehicleKmsID", SqlDbType.BigInt, 0, ParameterDirection.InputOutput, model.VehicleKmsID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_VehicleID", SqlDbType.Int, 0, ParameterDirection.Input, model.VehicleID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_BranchID", SqlDbType.Int, 0, ParameterDirection.Input, model.BranchID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_OdometerDate", SqlDbType.DateTime, 0, ParameterDirection.Input, model.OdometerDate);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_StartOdometer", SqlDbType.Float, 0, ParameterDirection.Input, model.StartOdometer);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_EndOdometer", SqlDbType.Float, 0, ParameterDirection.Input, model.EndOdometer);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_kmsJobsXml", SqlDbType.Xml, -1, ParameterDirection.Input, kmsJobsXml);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_travelLocationsXml", SqlDbType.Xml, -1, ParameterDirection.Input, travelLocationsXml);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Remarks", SqlDbType.VarChar, 500, ParameterDirection.Input, CSubs.PSafeValue(model.Remarks));
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
                                model.VehicleKmsID = Convert.ToInt64(conn.GetParameterValue(ParameterOF.PROCEDURE, "@SP_VehicleKmsID"));
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
                throw new DataAccessException(Convert.ToString(LoginID), "VehicleKmsDAL", "Insert", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
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
                        conn.AddCommand("[Warehouse].[DeleteVehicleKms]", QueryType.Procedure);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_VehicleKmsID", SqlDbType.BigInt, 0, ParameterDirection.Input, (ID));
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
                throw new DataAccessException(Convert.ToString(LoginID), "VehicleKmsDAL", "Delete", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

        }
    }
}