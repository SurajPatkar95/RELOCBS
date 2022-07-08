using RELOCBS.App_Code;
using RELOCBS.Common.ExceptionHandling;
using RELOCBS.Entities;
using RELOCBS.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml.Linq;

namespace RELOCBS.DAL.WH_Job
{
    public class WarehouseJobDAL
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


        public IQueryable<Entities.WarehouseJob> GetList(DateTime? FromDate, DateTime? Todate, bool IsJobDate, bool IsActivityDate, Int64? JobNo = null, string ContactPerson="", bool? RMCBuss = false, int Status = -1, int JobTypeId = -1)
        {
            int LoggedinUserID = UserSession.GetUserSession().LoginID;
            try
            {

                IQueryable<Entities.WarehouseJob> data = null;

                using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {
                        conn.AddCommand("[Warehouse].[GetWarehouseJobForGrid]", QueryType.Procedure);
                        
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IsJobDate", SqlDbType.Bit, 1, ParameterDirection.Input, IsJobDate);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IsActivityDate", SqlDbType.Bit, 1, ParameterDirection.Input, IsActivityDate);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_FromDate", SqlDbType.DateTime, 0, ParameterDirection.Input, FromDate);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ToDate", SqlDbType.DateTime, 0, ParameterDirection.Input, Todate);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LoginID", SqlDbType.Int, 0, ParameterDirection.Input, UserSession.GetUserSession().LoginID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_JobID", SqlDbType.Int, 0, ParameterDirection.Input, JobNo);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ContactPerson", SqlDbType.VarChar, 50, ParameterDirection.Input, CSubs.PSafeValue(ContactPerson));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CompID", SqlDbType.Int, 0, ParameterDirection.Input, UserSession.GetUserSession().CompanyID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IsRMCBuss", SqlDbType.Bit, 1, ParameterDirection.Input, RMCBuss);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Status", SqlDbType.Int, 0, ParameterDirection.Input, Status);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_JobTypeId", SqlDbType.SmallInt, 0, ParameterDirection.Input, JobTypeId);

                        DataSet dt = (DataSet)conn.ExecuteProcedure(ProcedureReturnType.DataSet);

                        if (!conn.IsError)
                        {
                            if (dt != null)
                            {
                                var result = (from rw in dt.Tables[0].AsEnumerable()
                                              select new Entities.WarehouseJob()
                                              {
                                                  JobID = !string.IsNullOrWhiteSpace(Convert.ToString(rw["JobID"])) ? Convert.ToInt64(rw["JobID"]) : -1,
                                                  JobNo = Convert.ToString(rw["JobNo"]),
                                                  //RevenueBranchID = Convert.ToInt32(rw["RevenueBranchID"]),
                                                  RevenueBranch = Convert.ToString(rw["RevenuBranch"]),
                                                  //HandlingBranchID = Convert.ToInt32(rw["HandlingBranchID"]),
                                                  HandlingBranch = Convert.ToString(rw["HandlingBranch"]),
                                                  RateComponentName = Convert.ToString(rw["RateComponentName"]),
                                                  JobOpenDate = Convert.ToDateTime(rw["JobOpenDate"]),
                                                  CompanyID = Convert.ToInt32(rw["CompanyID"]),
                                                  JobStatus = Convert.ToString(rw["JobStatus"]),
                                                  //IsRMCBuss = Convert.ToBoolean(rw["IsRMCBuss"]),
                                                  BusinessLine = Convert.ToString(rw["BusinessLine"]),
                                                  JobType=  Convert.ToString(rw["JobType"]),
                                                  WHJob_Instructions = (from item in dt.Tables[1].AsEnumerable()
                                                                        where item.Field<Int64>("JobID") == Convert.ToInt64(rw["JobID"]) && item.Field<int>("RateComponentID") == Convert.ToInt32(rw["RateComponentID"])
                                                                        select new WHJob_InstructionSheet()
                                                                        {
                                                                            JobID = Convert.ToInt64(item["JobID"]),
                                                                            InstID = Convert.ToInt64(item["InstID"]),
                                                                            SpecialInstructions = Convert.ToString(item["Special_Instruction"]),
                                                                            InstDate = Convert.ToDateTime(item["InstructionDate"]),
                                                                            Status = Convert.ToString(item["Status"]),
                                                                            BranchName = Convert.ToString(item["BranchName"]),
                                                                            WarehouseID = Convert.ToInt32(item["WarehouseID"]),
                                                                            WareHouseName = Convert.ToString(item["Warehoue_Name"]),
                                                                            RateComponentID = Convert.ToInt32(item["RateComponentID"]),
                                                                            RateComponentName = Convert.ToString(item["RateComponentName"]),
                                                                            WeightUnit = Convert.ToString(item["Wt_Vol"]),
                                                                            CreatedBy = Convert.ToString(item["CreatedBy"]),
                                                                            CreatedDate = Convert.ToDateTime(item["CreatedDate"]),
                                                                            ModifiedDate = !string.IsNullOrWhiteSpace(Convert.ToString(item["ModifiedDate"])) ? Convert.ToDateTime(item["ModifiedDate"]) : (DateTime?)null,
                                                                            ModifiedBy = Convert.ToString(item["ModifiedBy"]),
                                                                        }).ToList()

                                              }).ToList();
                                data = result.AsQueryable<Entities.WarehouseJob>();
                            }
                        }
                        else
                        {
                            throw new Exception(conn.ErrorMessage);
                        }

                        
                    }
                    else
                        throw new Exception(conn.ErrorMessage);
                }
                return data;
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoggedinUserID), "WarehouseJobDAL", "GetList", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

            //return null;
            
        }

        public DataSet GetDetails(int LoginID, Int64 JobID)
        {
            DataSet Dtobj = new DataSet();

            try
            {
                string query = string.Format("[Warehouse].[GetWarehouseJobDetailForDisplay] @SP_JobID={0},@SP_LoginID={1}",
                CSubs.QSafeValue(Convert.ToString(JobID)),
                CSubs.QSafeValue(Convert.ToString(LoginID)));

                Dtobj = CSubs.GetDataSet(query);

            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoginID), "WarehouseJobDAL", "GetDetails", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

            return Dtobj;
        }

        public bool Insert(WarehouseJob model, string submit, int LoginID, out string result)
        {
            try
            {
                using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {
                        
                        conn.AddCommand("[Warehouse].[AddEdit_WarehouseJob]", QueryType.Procedure);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_JobID", SqlDbType.BigInt, 0, ParameterDirection.InputOutput, model.JobID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_RevenueBranchID", SqlDbType.Int, 0, ParameterDirection.Input, model.RevenueBranchID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_RateComponentID", SqlDbType.Float, 0, ParameterDirection.Input, model.RateComponentID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@sp_HandlingBranchID", SqlDbType.Int, 0, ParameterDirection.Input, model.HandlingBranchID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@sp_BusinessLineID", SqlDbType.Int, 0, ParameterDirection.Input, model.BusinessLineID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@sp_JobOpenDate", SqlDbType.DateTime, 0, ParameterDirection.Input, model.JobOpenDate);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@sp_IsRMCBuss", SqlDbType.Bit, 0, ParameterDirection.Input, model.IsRMCBuss);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@sp_CompanyID", SqlDbType.Int, 0, ParameterDirection.Input, model.CompanyID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LoginID", SqlDbType.Int, 0, ParameterDirection.Input, UserSession.GetUserSession().LoginID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_JobTypeID", SqlDbType.SmallInt, 0, ParameterDirection.Input, model.JobTypeId);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@RETURNSTATUS", SqlDbType.SmallInt, 0, ParameterDirection.ReturnValue);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_MESSAGE", SqlDbType.NVarChar, -1, ParameterDirection.Output);
                        conn.ExecuteProcedure(ProcedureReturnType.SingleValue);

                        if (!conn.IsError)
                        {
                            int ReturnStatus = Convert.ToInt32(conn.GetParameterValue(ParameterOF.PROCEDURE, "@RETURNSTATUS"));
                            result = Convert.ToString(conn.GetParameterValue(ParameterOF.PROCEDURE, "@SP_MESSAGE"));

                            if (ReturnStatus == 0)
                            {
                                model.JobID = Convert.ToInt64(conn.GetParameterValue(ParameterOF.PROCEDURE, "@SP_JobID"));
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
                throw new DataAccessException(Convert.ToString(LoginID), "WarehouseJobDAL", "Insert", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }


        }

        public bool InsertinstructionSheet(WHJob_InstructionSheet model, string submit, int LoginID, out string result)
        {
            try
            {
                using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {

                        
                        string ActivityXml = string.Empty;
                        bool IsSentToWarehouse = false;

                        if (submit.ToUpper() == "SENTTOWAREHOUSE")
                        {
                            IsSentToWarehouse = true;
                        }
                        if (model.activities != null && model.activities.Count > 0)
                        {
                            ActivityXml = Convert.ToString(new XElement("Activities", from emp in model.activities
                                                                                      select new XElement("Activity",
                                                                             new XElement("ACTID", emp.ActivityID),
                                                                             new XElement("TypeID", emp.ActivityTypeID),
                                                                             new XElement("FromDate", Convert.ToDateTime(emp.FromDate).ToString("dd-MMM-yyyy HH:mm")),
                                                                             new XElement("ToDate", Convert.ToDateTime(emp.ToDate).ToString("dd-MMM-yyyy HH:mm")),
                                                                             new XElement("RepTime", emp.RepTime.Value.ToString()),
                                                                             new XElement("FromLoc", emp.FromLocation),
                                                                             new XElement("ToLoc", emp.ToLocation),
                                                                             new XElement("Remark", emp.Remark),
                                                                             new XElement("Days", emp.NumberOfDays),
                                                                             new XElement("Deleted", emp.InActive)

                                                                         )));
                        }
                        conn.AddCommand("[Warehouse].[AddEditInstructionSheet]", QueryType.Procedure);
                        
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_InstID", SqlDbType.BigInt, 0, ParameterDirection.InputOutput, model.InstID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_MoveID", SqlDbType.BigInt, 0, ParameterDirection.Input, model.JobID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Special_Instructions", SqlDbType.NVarChar, 500, ParameterDirection.Input, CSubs.PSafeValue(model.SpecialInstructions));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Remark", SqlDbType.NVarChar, 500, ParameterDirection.Input, CSubs.PSafeValue(model.Remarks));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_BranchID", SqlDbType.Int, 0, ParameterDirection.Input, model.BranchID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_WarehouseID", SqlDbType.Int, 0, ParameterDirection.Input, model.WarehouseID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_RateComponentID", SqlDbType.Int, 0, ParameterDirection.Input, model.RateComponentID);

                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ActivitiesXML", SqlDbType.Xml, -1, ParameterDirection.Input, ActivityXml);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_OrgAddrs1", SqlDbType.NVarChar, 150, ParameterDirection.Input, CSubs.PSafeValue(model.Add1));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_OrgAddrs2", SqlDbType.NVarChar, 150, ParameterDirection.Input, CSubs.PSafeValue(model.Add2));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_OrgCityID", SqlDbType.Int, 0, ParameterDirection.Input, model.CityID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_OrgPincode", SqlDbType.Int, 0, ParameterDirection.Input, model.Pincode);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_OrgEmail", SqlDbType.VarChar, 150, ParameterDirection.Input, CSubs.PSafeValue(model.Email));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_OrgPhone", SqlDbType.VarChar, 20, ParameterDirection.Input, CSubs.PSafeValue(model.Phone));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_OrgMobile", SqlDbType.VarChar, 20, ParameterDirection.Input, CSubs.PSafeValue(model.Mobile));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@WeightUnitID", SqlDbType.Int, 0, ParameterDirection.Input, model.WeightUnitID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@WeightFrom", SqlDbType.Float, 0, ParameterDirection.Input, model.WeightUnitFrom);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@WeightTo", SqlDbType.Float, 0, ParameterDirection.Input, model.WeightUnitTo);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IsSentToWarehouse", SqlDbType.Bit, 0, ParameterDirection.Input, IsSentToWarehouse);
                        
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
                                model.InstID = Convert.ToInt64(conn.GetParameterValue(ParameterOF.PROCEDURE, "@SP_InstID"));
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
                throw new DataAccessException(Convert.ToString(LoginID), "WarehouseJobDAL", "InsertinstructionSheet", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }


        }

        public DataSet GetWHInstructionSheetDetail(int LoginID,Int64 MoveID,Int64 InstID=-1)
        {
            DataSet Dtobj = new DataSet();

            try
            {
                string query = string.Format("exec [Warehouse].[GetJobInstructionSheetForDisplay] @SP_InstID={0},@SP_MoveID={1},@SP_LoginID={2},@SP_IsWHJob=1",
                 CSubs.QSafeValue(Convert.ToString(InstID)), CSubs.QSafeValue(Convert.ToString(MoveID))
                 ,CSubs.QSafeValue(Convert.ToString(LoginID)));

                Dtobj = CSubs.GetDataSet(query);

            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoginID), "WarehouseJobDAL", "GetWHInstructionSheetDetail", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

            return Dtobj;

        }

        public DataSet GetWHInstructionPrintDetail(int LoginID, Int64 MoveID, Int64 InstID = -1)
        {
            DataSet Dtobj = new DataSet();

            try
            {
                string query = string.Format("exec [Warehouse].[GetJobInstructionSheetForPrint] @SP_InstID={0},@SP_MoveID={1},@SP_LoginID={2},@SP_IsWHJob=1",
                 CSubs.QSafeValue(Convert.ToString(InstID)), CSubs.QSafeValue(Convert.ToString(MoveID)), CSubs.QSafeValue(Convert.ToString(LoginID)));

                Dtobj = CSubs.GetDataSet(query);
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoginID), "JobInstructionDAL", "GetWHInstructionPrintDetail", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
            
            return Dtobj;

        }

    }
}