using RELOCBS.App_Code;
using RELOCBS.Common.ExceptionHandling;
using RELOCBS.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using RELOCBS.Entities;

namespace RELOCBS.DAL.LoadChart
{
    public class LoadChartDAL
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


        public IQueryable<Entities.LoadChartsGrid> GetGrid(DateTime? FromDate, DateTime? Todate, bool? RMCBuss = false, string TCLId = null, string TransporterId = null,string Shipper = null, Int64? JobNo = null)
        {
            int LoggedinUserID = UserSession.GetUserSession().LoginID;
            try
            {

                IQueryable<Entities.LoadChartsGrid> data = null;

                using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {
                        conn.AddCommand("[Warehouse].[GetLoadChartGrid]", QueryType.Procedure);
                        
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_FromDate", SqlDbType.DateTime, 0, ParameterDirection.Input, FromDate);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ToDate", SqlDbType.DateTime, 0, ParameterDirection.Input, Todate);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_TCLId", SqlDbType.Int, 0, ParameterDirection.Input, CSubs.PSafeValue(TCLId));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_TransporterId", SqlDbType.Int, 0, ParameterDirection.Input, CSubs.PSafeValue(TransporterId));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CompId", SqlDbType.Int, 0, ParameterDirection.Input, UserSession.GetUserSession().CompanyID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IsRMCBuss", SqlDbType.Bit, 1, ParameterDirection.Input, RMCBuss);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LoginID", SqlDbType.Int, 0, ParameterDirection.Input, UserSession.GetUserSession().LoginID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Shipper", SqlDbType.VarChar, 250, ParameterDirection.Input, CSubs.PSafeValue(Shipper));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_MoveId", SqlDbType.Int, 0, ParameterDirection.Input, JobNo);
                        DataTable dt = (DataTable)conn.ExecuteProcedure(ProcedureReturnType.DataTable);

                        if (dt != null)
                        {
                            var result = (from rw in dt.AsEnumerable()
                                          select new Entities.LoadChartsGrid()
                                          {
                                              LoadChartID = Convert.ToInt64(rw["LoadChartMasterID"]),
                                              TLCID = Convert.ToString(rw["LoadChartMasterID"]),
                                              Transporter = Convert.ToString(rw["Transporter"]),
                                              LoadChartDate = rw["StartDate"] !=DBNull.Value ?  Convert.ToDateTime(rw["StartDate"]) : (DateTime?)null,
                                              SealNo = Convert.ToString(rw["SealNo"]),
                                              TruckNo = Convert.ToString(rw["VehicleNumber"]),
                                              Branch = Convert.ToString(rw["CompBranchName"]),
                                              EscartBranch = Convert.ToString(rw["EscartBranch"]),
                                              EscartByEMP = Convert.ToString(rw["EscartByEMP"]),
                                          }).ToList();
                            data = result.AsQueryable<Entities.LoadChartsGrid>();
                        }
                    }
                    else
                        throw new Exception(conn.ErrorMessage);
                }
                return data;
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoggedinUserID), "LoadChartDAL", "GetGrid", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

            //return null;


        }

        public DataSet GetDetail(int LoginID, Int64 LoadChartID)
        {
            DataSet Dtobj = new DataSet();

            try
            {
                string query = string.Format("[Warehouse].[GetLoadChartDetails] @SP_LoadChartMasterID={0}",//,@SP_RateComponentID={1}",
                CSubs.QSafeValue(Convert.ToString(LoadChartID))
                //,CSubs.QSafeValue(Convert.ToString(LoginID))
                );

                Dtobj = CSubs.GetDataSet(query);

            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoginID), "LoadChartDAL", "GetDetail", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

            return Dtobj;
        }

        public DataTable GetJobDetail(int LoginID, Int64 MoveID)
        {
            DataTable Dtobj = new DataTable();

            try
            {
                string query = string.Format("[Warehouse].[GetLoadChartDetails] @SP_MoveID={0},@SP_OnlyJobInfo=1",
                CSubs.QSafeValue(Convert.ToString(MoveID))
                );

                Dtobj = CSubs.GetDataTable(query);

            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoginID), "LoadChartDAL", "GetJobDetail", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

            return Dtobj;
        }

        public bool Insert(LoadCharts model, int LoginID, out string result)
        {
            try
            {
                using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {

                        string BranchAccessXml = string.Empty;
                        string ShipmentXml = string.Empty;

                        if (model.loadChartShipments.Count > 0)
                        {
                            ShipmentXml = new XElement("LoadDetails", from emp in model.loadChartShipments
                                                                  select new XElement("LoadDetail",
                                                         new XElement("MoveID", emp.MoveID),
                                                         new XElement("NoOfPacsDetails", emp.NoOfPacsDetails),
                                                         new XElement("Vol", emp.Vol),
                                                         new XElement("RevAmt", emp.Revenue),
                                                         new XElement("CostAmt", emp.ApproxCost),
                                                         new XElement("LoadedAt", emp.LoadAt),
                                                         new XElement("SupervisorID", emp.LoadedBySupervisorID)

                                                     )).ToString();

                            //if (!string.IsNullOrWhiteSpace(ServiceXml))
                            //{
                            //    ServiceXml = Regex.Replace(ServiceXml, @"\t|\n|\r", ""); //InstIDxml.Replace("\r\n", "");
                            //}
                        }

                        if (model.BranchAccess.Count > 0)
                        {
                            BranchAccessXml = new XElement("OrgDestViaDtls", from emp in model.BranchAccess
                                                                   select new XElement("OrgDestViaDtl",
                                                          new XElement("OrgDestVia", emp.BranchAccessTypeID),
                                                          new XElement("BrID", emp.BranchID)

                                                      )).ToString();
                            //if (!string.IsNullOrWhiteSpace(CrewMembersXml))
                            //{
                            //    CrewMembersXml = Regex.Replace(CrewMembersXml, @"\t|\n|\r", ""); //InstIDxml.Replace("\r\n", "");
                            //}
                        }
                        
                        conn.AddCommand("[Warehouse].[AddEditLoadChart]", QueryType.Procedure);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LoadChartMasterID", SqlDbType.BigInt, 0, ParameterDirection.InputOutput, model.LoadChartID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_BranchID", SqlDbType.Int, 0, ParameterDirection.Input, model.BranchID);
                        //conn.AddParameter(ParameterOF.PROCEDURE, "@SP_PRJID", SqlDbType.BigInt, 0, ParameterDirection.InputOutput, model.PJR_DJR_ID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ModeID", SqlDbType.Int, 0, ParameterDirection.Input, model.ModeID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_TransporterID", SqlDbType.Int, 0, ParameterDirection.Input, model.TransporterID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_VehicleNumber", SqlDbType.VarChar, 20, ParameterDirection.Input, CSubs.PSafeValue(model.TruckNo));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_SealNo", SqlDbType.VarChar, 20, ParameterDirection.Input, CSubs.PSafeValue(model.SealNo));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_VechTypeID", SqlDbType.Int, 0, ParameterDirection.Input, model.VehicleTypeID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_StartDate", SqlDbType.DateTime, 0, ParameterDirection.Input, model.LeftOnDate);
                        
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_EscartByEMPID", SqlDbType.Int, 0, ParameterDirection.Input, model.EscortedByID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_EscartBrID", SqlDbType.Int, 0, ParameterDirection.Input, model.EscortedByBranchID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LoadedAt", SqlDbType.VarChar, 50, ParameterDirection.Input, CSubs.PSafeValue(model.LoadedAtBranchID));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Via", SqlDbType.VarChar, 50, ParameterDirection.Input, CSubs.PSafeValue(model.LoadedViaBranchID));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LoadedTo", SqlDbType.VarChar, 50, ParameterDirection.Input, CSubs.PSafeValue(model.LoadedToBranchID));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_OutSideVeh", SqlDbType.Bit, 0, ParameterDirection.Input, model.IsOutSideVehicle);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_DirectDelivery", SqlDbType.Bit, 0, ParameterDirection.Input, model.IsDirectDelivery);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_TallyChartPrepared", SqlDbType.Bit, 0, ParameterDirection.Input, model.IsTallyChartPrepared);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_TallyChartSentToLoc", SqlDbType.Bit, 0, ParameterDirection.Input, model.IsTallyChartSentToLoc);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CostForVeh", SqlDbType.Float, 0, ParameterDirection.Input, model.CostForVehicle);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LoadDetails", SqlDbType.Xml, -1, ParameterDirection.Input, ShipmentXml);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_OrgDestVia", SqlDbType.Xml, -1, ParameterDirection.Input, BranchAccessXml);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Remarks", SqlDbType.VarChar, 500, ParameterDirection.Input,CSubs.PSafeValue(model.Remarks));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Notes", SqlDbType.VarChar, 500, ParameterDirection.Input, CSubs.PSafeValue(model.Notes));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Common", SqlDbType.VarChar, 500, ParameterDirection.Input, CSubs.PSafeValue(model.Common));
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
                                model.LoadChartID = Convert.ToInt64(conn.GetParameterValue(ParameterOF.PROCEDURE, "@SP_LoadChartMasterID"));
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
                throw new DataAccessException(Convert.ToString(LoginID), "LoadChartDAL", "Insert", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
        }

    }
}