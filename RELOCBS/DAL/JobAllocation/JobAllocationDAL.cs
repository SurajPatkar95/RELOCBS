using RELOCBS.App_Code;
using RELOCBS.Common.ExceptionHandling;
using RELOCBS.Entities;
using RELOCBS.Extensions;
using RELOCBS.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml.Linq;

namespace RELOCBS.DAL.JobAllocation
{
    public class JobAllocationDAL
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

        public bool Insert(RELOCBS.Entities.JobAllocationModel jobAllocation, out string result)
        {
            result = string.Empty;

            try
            {

                using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {
                        
                        string ActivityXml = string.Empty;
                        ActivityXml = new XElement("Activities", from emp in jobAllocation.activities
                                                               select new XElement("Activity",
                                                      new XElement("TypeID", emp.ActivityID),
                                                      new XElement("FromDate", emp.FromDate),
                                                      new XElement("ToDate", emp.ToDate),
                                                      new XElement("RepTime", emp.RepTime),
                                                      new XElement("FromLoc", emp.FromLocation),
                                                      new XElement("ToLoc", emp.ToLocation)

                                                  )).ToString();

                        conn.AddCommand("[Warehouse].[AddEditJobAllocation]", QueryType.Procedure);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_MoveID", SqlDbType.BigInt, 0, ParameterDirection.InputOutput, jobAllocation.MoveID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_InstID", SqlDbType.BigInt, 0, ParameterDirection.InputOutput, jobAllocation.InstID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ActualBeginDate", SqlDbType.DateTime, 0, ParameterDirection.Input, Convert.ToDateTime(jobAllocation.ActualBeginDate));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ActualCompleteDate", SqlDbType.DateTime, 0, ParameterDirection.Input, Convert.ToDateTime(jobAllocation.ActulaCompleteDate));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_TurnaroundTime", SqlDbType.Time, 0, ParameterDirection.Input, jobAllocation.TurnaroundTime!=null ? (TimeSpan)jobAllocation.TurnaroundTime :(TimeSpan?)null);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Special_Instructions", SqlDbType.NVarChar, 500, ParameterDirection.Input, (jobAllocation.Special_Instructions));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Remark", SqlDbType.NVarChar, 500, ParameterDirection.Input, (jobAllocation.Remark));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ActivitiesXML", SqlDbType.Xml, -1, ParameterDirection.Input, ActivityXml);
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
                                jobAllocation.InstID = Convert.ToInt64(conn.GetParameterValue(ParameterOF.PROCEDURE, "@SP_InstID"));
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
                throw new DataAccessException(Convert.ToString(UserSession.GetUserSession().LoginID), "jobAllocationDAL", "Insert", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
            //return true;
        }


        public bool InsertActivity(RELOCBS.Entities.JobActivity jobActivity, out string result)
        {
            result = string.Empty;
            //string FilePath=configuration
            try
            {

                using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {
                        string CrewMembersXml = string.Empty;
                        string DocXml = string.Empty;
                        string VehicleXml = string.Empty;
                        string ServiceXml = string.Empty;
                        string MaterialXml = string.Empty;

                        ServiceXml = new XElement("Services", from emp in jobActivity.services
                                                              select new XElement("Service",
                                                     new XElement("ID", emp.SD_ID),
                                                     new XElement("ServiceID", emp.ServiceID),
                                                     new XElement("FromDate", emp.FromDate),
                                                     new XElement("ToDate", emp.ToDate),
                                                     new XElement("Cost", emp.Cost)

                                                 )).ToString();

                        CrewMembersXml = new XElement("Crews", from emp in jobActivity.jobCrew.members
                                                               select new XElement("Crew",
                                                      new XElement("ID", emp.CWMID),
                                                      new XElement("EmpID", emp.EmpID),
                                                      new XElement("FromDate", emp.EffectiveFrom),
                                                      new XElement("ToDate", emp.EffectiveTo)

                                                  )).ToString();

                        VehicleXml = new XElement("Vehicles", from emp in jobActivity.jobVehicleList
                                                              select new XElement("Vehicle",
                                                              new XElement("ID", emp.V_ID),
                                                     new XElement("PurposeID", emp.PurposeID),
                                                     new XElement("VehicleID", emp.VehicleID),
                                                     new XElement("DriverID", emp.DriverID),
                                                     new XElement("VehicleNo", emp.VehicleNo),
                                                     new XElement("Driver", emp.Driver),
                                                     new XElement("VehicleType", emp.VehicleType),
                                                     new XElement("DriverType", emp.DriverType),
                                                     new XElement("FromDate", emp.FromDate),
                                                     new XElement("ToDate", emp.ToDate)

                                                 )).ToString();

                        DocXml = new XElement("Downloads", from emp in jobActivity.docUpload.docLists
                                                                 select new XElement("Download",
                                                        new XElement("DocID", emp.DocID),
                                                        new XElement("DocTypeID", emp.DocTypeID)
                                                    )).ToString();

                        MaterialXml = new XElement("Materials", from emp in jobActivity.materialUsed
                                                                select new XElement("Material",
                                                        new XElement("ID", emp.M_ID),
                                                       new XElement("MaterailId", emp.MaterailId),
                                                       new XElement("Rate", emp.Rate),
                                                       new XElement("ReturnQty", emp.ReturnQty),
                                                       new XElement("IssuedQty", emp.IssuedQty),
                                                       new XElement("UsedQty", emp.UsedQty)
                                                   )).ToString();

                        conn.AddCommand("[Warehouse].[AddEditJobAllocationActivity]", QueryType.Procedure);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_InstID", SqlDbType.BigInt, 0, ParameterDirection.Input, jobActivity.InstID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ActivityID", SqlDbType.BigInt, 0, ParameterDirection.InputOutput, jobActivity.ActivityID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ActivityTypeID", SqlDbType.Int, 0, ParameterDirection.Input, jobActivity.ActivityTypeID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CrewID", SqlDbType.Int, 0, ParameterDirection.Input, jobActivity.jobCrew.CrewID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_SuperviserID", SqlDbType.Int, 0, ParameterDirection.Input, jobActivity.jobCrew.SuperviserID);

                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_FromLoc", SqlDbType.VarChar, 50, ParameterDirection.Input, CSubs.PSafeValue(jobActivity.FromLocation));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ToLoc", SqlDbType.VarChar, 50, ParameterDirection.Input, CSubs.PSafeValue(jobActivity.ToLocation));

                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_FromDate", SqlDbType.DateTime, 0, ParameterDirection.Input, Convert.ToDateTime(jobActivity.FromDate));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ToDate", SqlDbType.DateTime, 0, ParameterDirection.Input, Convert.ToDateTime(jobActivity.ToDate));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_RepTime", SqlDbType.Time, 0, ParameterDirection.Input, (TimeSpan)jobActivity.RepTime);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Remark", SqlDbType.NVarChar, 500, ParameterDirection.Input, (jobActivity.Remark));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CrewXML", SqlDbType.Xml, -1, ParameterDirection.Input, CrewMembersXml);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_AdditionalServiceXML", SqlDbType.Xml, -1, ParameterDirection.Input, ServiceXml);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_DocXml", SqlDbType.Xml, -1, ParameterDirection.Input, DocXml);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_VehiclesXML", SqlDbType.Xml, -1, ParameterDirection.Input, VehicleXml);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_MaterialXml", SqlDbType.Xml, -1, ParameterDirection.Input, MaterialXml);

                        if (jobActivity.docUpload.file!=null)
                        {
                            conn.AddParameter(ParameterOF.PROCEDURE, "@SP_DocTypeID", SqlDbType.Int, 0, ParameterDirection.Input, jobActivity.docUpload.DocTypeID);
                            //conn.AddParameter(ParameterOF.PROCEDURE, "@SP_FilePath", SqlDbType.NVarChar, 500, ParameterDirection.Input, CSubs.PSafeValue());
                        }
                        

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
                                jobActivity.ActivityID = Convert.ToInt64(conn.GetParameterValue(ParameterOF.PROCEDURE, "@SP_ActivityID"));
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
                throw new DataAccessException(Convert.ToString(UserSession.GetUserSession().LoginID), "jobAllocationDAL", "InsertActivity", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
            //return true;
        }

        public IQueryable<JobInstGrid> GetForGrid(int LoginID,int CompID, Int64[] JobNo,string Shipper,int AllocationStatus =-1,DateTime? FromDate=null, DateTime? ToDate=null,int WarehouseID = -1, bool? RMCBuss = false,int JobType=1)
        {

            try
            {
                string JobStr=string.Empty;
                if (JobNo.Count() > 0)
                {
                    //JobStr = new XElement("Activities", JobNo.Select(i => new XElement("MoveID", i))).ToString();
                    var branchesXml = JobNo.Select(i => new XElement("Activity",new XAttribute("MoveID", i)));
                    JobStr = new XElement("Activities", branchesXml).ToString();
                }

                string SPName = JobType == 1 ? "GetJobAllocationForGrid" : "GetWHJobAllocationForGrid";

                string query = string.Format("exec [Warehouse].["+ SPName + "]  @SP_LoginID={0},@SP_FromDate={1},@SP_ToDate={2},@SP_MOVEID={3},@SP_AllocationStatus={4},@SP_CompID={5},@SP_IsRMCBuss={6},@SP_WarehouseID={7},@SP_SHIPPER={8}",
                Convert.ToString(LoginID)
                , CSubs.QSafeValue( FromDate!=null ? (Convert.ToDateTime(FromDate)).ToString("dd-MMM-yyyy") : "")
                , CSubs.QSafeValue(ToDate != null ? (Convert.ToDateTime(ToDate)).ToString("dd-MMM-yyyy") : "")
                , CSubs.QSafeValue(Convert.ToString(JobStr))
                , CSubs.QSafeValue(Convert.ToString(AllocationStatus))
                , CSubs.QSafeValue(Convert.ToString(CompID))
                , CSubs.QSafeValue(Convert.ToString(RMCBuss))
                ,CSubs.QSafeValue(Convert.ToString(WarehouseID))
                ,CSubs.QSafeValue(Convert.ToString(Shipper))
                );

                DataSet dataSet = CSubs.GetDataSet(query);
                IQueryable<JobInstGrid> data;

                if (dataSet!=null && dataSet.Tables.Count>0)
                {
                    var result = (from rw in dataSet.Tables[0].AsEnumerable()
                                  select new JobInstGrid()
                                  {
                                      EnqDetailID = !string.IsNullOrWhiteSpace(Convert.ToString(rw["EnqDetailID"])) ? Convert.ToInt64(rw["EnqDetailID"]) : -1,
                                      SurveyID = !string.IsNullOrWhiteSpace(Convert.ToString(rw["SurveyID"])) ? Convert.ToInt64(rw["SurveyID"]) : -1,
                                      MoveID = Convert.ToInt64(rw["MoveID"]),
                                      JobNo = Convert.ToString(rw["JobNo"]),
                                      //FromCity = Convert.ToString(rw["OrgCity"]),
                                      //ExitPort = Convert.ToString(rw["ExitPort"]),
                                      //EntryPort = Convert.ToString(rw["EntryPort"]),
                                      //ToCity = Convert.ToString(rw["DestCity"]),
                                      JobOpenDate = Convert.ToDateTime(rw["JobDate"]),
                                      Mode = Convert.ToString(rw["ModeName"]),
                                      Shipper = Convert.ToString(rw["Shipper"]),
                                      Client = Convert.ToString(rw["Client"]),
                                      Corporate = Convert.ToString(rw["Corporate"]),
                                      //Volume = Convert.ToInt64(rw["WeightFrom"]),
                                      Status = Convert.ToString(rw["JobStatus"]),
                                      ServiceLine = Convert.ToString(rw["ServiceLine"]),
                                      RateComponentID = Convert.ToInt32(rw["RateComponentID"]),
                                      RateComponentName = Convert.ToString(rw["RateComponentName"]),
                                      JobReport_Status = Convert.ToString(rw["JobReport_Status"]),
                                      JobReport_Type = Convert.ToString(rw["JobReport_Type"]),
                                      PJR_AddEdit = Convert.ToString(rw["PJR_AddEdit"]),
                                      PJR_DJR_ID = !string.IsNullOrWhiteSpace(Convert.ToString(rw["PJR_DJR_ID"])) ? Convert.ToInt64(rw["PJR_DJR_ID"]) : (Int64?)null,
                                      RevenueBranch = Convert.ToString(rw["RevenueBranch"]),
                                      HandlingBranch = Convert.ToString(rw["HandlingBranch"]),
                                      BusinessLine = Convert.ToString(rw["BusinessLine"]),
                                      instructionSheetGrids = (from item in dataSet.Tables[1].AsEnumerable()
                                                               where item.Field<Int64>("MoveID") == Convert.ToInt64(rw["MoveID"]) && item.Field<int>("RateComponentID") == Convert.ToInt32(rw["RateComponentID"])
                                                               select new InstructionSheetGrid()
                                                               {
                                                                   MoveID = Convert.ToInt64(item["MoveID"]),
                                                                   InstID = Convert.ToInt64(item["InstID"]),
                                                                   SpecialInstructions = Convert.ToString(item["Special_Instruction"]),
                                                                   InstDate = Convert.ToDateTime(item["InstructionDate"]),
                                                                   ExpectedBeginDateTime = item["ExpectedBeginDate"] != null ? Convert.ToDateTime(item["ExpectedBeginDate"]) : (DateTime?)null,
                                                                   ExpectedCompletionDateTime = item["ExpectedCompletionDate"] != null ? Convert.ToDateTime(item["ExpectedCompletionDate"]) : (DateTime?)null,
                                                                   Inst_Status = Convert.ToString(item["Status"]),
                                                                   InstType = Convert.ToString(item["RateComponentName"]),
                                                                   Edit = Convert.ToString(item["Edit"]),
                                                                   Delete = Convert.ToString(item["Delete"]),
                                                                   BatchID = !string.IsNullOrWhiteSpace(Convert.ToString(item["JobDiaryBatchID"])) ? Convert.ToInt64(item["JobDiaryBatchID"]) : (Int64?)null,
                                                                   //CreatedDate = Convert.ToDateTime(rw["CreatedDate"]),
                                                                   //CreatedBy = Convert.ToString(rw["CreatedBy"]),
                                                                   //ModifiedBy = Convert.ToString(rw["ModifiedBy"]),
                                                                   //ModifiedDate = Convert.ToDateTime(rw["ModifiedDate"]),
                                                                   BranchName = Convert.ToString(item["BranchName"]),
                                                                   WarehouseID = Convert.ToInt32(item["WarehouseID"]),
                                                                   WarehouseName = Convert.ToString(item["Warehoue_Name"]),
                                                                   RateComponentID = Convert.ToInt32(item["RateComponentID"]),
                                                                   RateComponentName = Convert.ToString(item["RateComponentName"]),
                                                                   JobReport_Type = Convert.ToString(item["JobReport_Type"]),
                                                               }).ToList()

                                  }).ToList();


                    //foreach (var Job in result)
                    //{

                    //    Job.instructionSheetGrids = (from rw in dataSet.Tables[1]//.Select("MoveID = " + CSubs.QSafeValue(MoveID))
                    //                                 .AsEnumerable()
                    //                                 where rw.Field<Int64>("MoveID") == Job.MoveID && rw.Field<int>("RateComponentID") == Job.RateComponentID

                    //                                 select new InstructionSheetGrid()
                    //                                 {
                    //                                     MoveID = Convert.ToInt64(rw["MoveID"]),
                    //                                     InstID = Convert.ToInt64(rw["InstID"]),
                    //                                     SpecialInstructions = Convert.ToString(rw["Special_Instruction"]),
                    //                                     InstDate = Convert.ToDateTime(rw["InstructionDate"]),
                    //                                     ExpectedBeginDateTime = rw["ExpectedBeginDate"] != null ? Convert.ToDateTime(rw["ExpectedBeginDate"]) : (DateTime?)null,
                    //                                     ExpectedCompletionDateTime = rw["ExpectedCompletionDate"] != null ? Convert.ToDateTime(rw["ExpectedCompletionDate"]) : (DateTime?)null,
                    //                                     Inst_Status = Convert.ToString(rw["Status"]),
                    //                                     InstType = Convert.ToString(rw["RateComponentName"]),
                    //                                     Edit = Convert.ToString(rw["Edit"]),
                    //                                     Delete = Convert.ToString(rw["Delete"]),
                    //                                     BatchID = !string.IsNullOrWhiteSpace(Convert.ToString(rw["JobDiaryBatchID"])) ? Convert.ToInt64(rw["JobDiaryBatchID"]) : (Int64?)null,
                    //                                     //CreatedDate = Convert.ToDateTime(rw["CreatedDate"]),
                    //                                     //CreatedBy = Convert.ToString(rw["CreatedBy"]),
                    //                                     //ModifiedBy = Convert.ToString(rw["ModifiedBy"]),
                    //                                     //ModifiedDate = Convert.ToDateTime(rw["ModifiedDate"]),
                    //                                     BranchName = Convert.ToString(rw["BranchName"]),
                    //                                     WarehouseID = Convert.ToInt32(rw["WarehouseID"]),
                    //                                     WarehouseName = Convert.ToString(rw["Warehoue_Name"]),
                    //                                     RateComponentID = Convert.ToInt32(rw["RateComponentID"]),
                    //                                     RateComponentName = Convert.ToString(rw["RateComponentName"]),
                    //                                     JobReport_Type = Convert.ToString(rw["JobReport_Type"]),
                    //                                 }).ToList();

                    //    //if (Job.instructionSheetGrids.Count(p => p.Inst_Status.ToUpper() == "COMPLETED") == Job.instructionSheetGrids.Count)
                    //    //{
                    //    //    Job.Status = "Completed";
                    //    //}
                    //}

                   data = result.AsQueryable<JobInstGrid>();

                  return data;
                }

                return new List<JobInstGrid>().AsQueryable();
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoginID), "JobAllocationDAL", "GetForGrid", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
        }

        public DataSet GetDetailById(int LoginID, Int64 MoveId, Int64 InstID)
        {
            DataSet Dtobj = new DataSet();

            try
            {
                string query = string.Format("exec [Warehouse].[GetJobAllocationForDisplay] @SP_MoveId={0},@SP_InstID={1},@SP_LoginID={2}",
                 CSubs.QSafeValue(Convert.ToString(MoveId)), CSubs.QSafeValue(Convert.ToString(InstID))
                 , CSubs.QSafeValue(Convert.ToString(LoginID)));

                Dtobj = CSubs.GetDataSet(query);
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoginID), "JobAllocationDAL", "GetDetailById", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
            
            return Dtobj;
        }

        public JobDiaryModel GetBulkInstDetailById(int LoginID,Int64[] InstID,Int64? BatchID=-1,Int16 JobType=1)
        {
            JobDiaryModel jobDiary = new JobDiaryModel();
            try
            {
                jobDiary.InstIds = InstID.OfType<Int64>().ToList();
                string InstIDxml= new XElement("InstIDs", from emp in jobDiary.InstIds
                                                          select new XElement("InstID", new XAttribute("ID", Convert.ToString(emp).Trim()))
                                              ).ToString();

                //if (!string.IsNullOrWhiteSpace(InstIDxml))
                //{
                //    InstIDxml = Regex.Replace(InstIDxml, @"\t|\n|\r| ", ""); //InstIDxml.Replace("\r\n", "");
                //}
                string SPName = JobType == 1 ? "GetJobAllocationGridForDisplay" : "GetWHJobAllocationGridForDisplay";
                string query = string.Format("exec [Warehouse].["+ SPName + "] @SP_LoginID={0},@SP_InstIDXML={1},@SP_BatchID={2}",
                Convert.ToString(LoginID), CSubs.QSafeValue(Convert.ToString(InstIDxml)), CSubs.QSafeValue(Convert.ToString(BatchID)));
                DataSet dataSet = CSubs.GetDataSet(query);
                var result = (from rw in dataSet.Tables[0].AsEnumerable()
                              select new InstructionSheetGrid()
                              {
                                  MoveID = Convert.ToInt64(rw["MoveID"]),
                                  InstID = Convert.ToInt64(rw["InstID"]),
                                  JobNo = Convert.ToString(rw["JobNo"]),
                                  Shipper = Convert.ToString(rw["Shipper"]),
                                  InstDate = Convert.ToDateTime(rw["InstructionDate"]),
                                  SpecialInstructions = Convert.ToString(rw["Special_Instruction"]),
                                  BranchName = Convert.ToString(rw["BranchName"]),
                                  WarehouseName = Convert.ToString(rw["Warehoue_Name"]),
                                  InstType = Convert.ToString(rw["RateComponentName"]),
                                  Inst_Status = Convert.ToString(rw["Status"]),
                                  BatchID = !string.IsNullOrWhiteSpace(Convert.ToString(rw["JobDiaryBatchID"])) ? Convert.ToInt64(rw["JobDiaryBatchID"]) : (Int64?)null
                                  //Edit = Convert.ToString(rw["Edit"]),
                                  //Delete = Convert.ToString(rw["Delete"]),
                              }).ToList();


                foreach (var Job in result)
                {

                    string ID = Convert.ToString(Job.InstID);
                    Job.jobActivities = (from rw in dataSet.Tables[1].Select("InstID = " + CSubs.QSafeValue(ID)).AsEnumerable()
                                                 select new Inst_Activities()
                                                 {
                                                     //MoveID = Convert.ToInt64(rw["MoveID"]),
                                                     InstID = Convert.ToInt64(rw["InstID"]),
                                                     ActivityID = Convert.ToInt64(rw["ActivityID"]),
                                                     ActivityTypeID = Convert.ToInt32(rw["ActivityTypeID"]),
                                                     ActivityTypeName= Convert.ToString(rw["ActivityTypeName"]),
                                                     FromDate = Convert.ToDateTime(rw["FromDate"]),
                                                     ToDate = Convert.ToDateTime(rw["ToDate"]),
                                                     RepTime =string.IsNullOrWhiteSpace(Convert.ToString(rw["Rep_Time"]))?  (TimeSpan?)null : (TimeSpan)(rw["Rep_Time"]),
                                                     FromLocation = Convert.ToString(rw["From_Loc"]),
                                                     ToLocation = Convert.ToString(rw["To_Loc"]),
                                                     ACT_Status = Convert.ToString(rw["ACT_Status"]),
                                                     ACT_StatusID = string.IsNullOrWhiteSpace(Convert.ToString(rw["Act_StatusID"])) ? (Int32?)(null) : Convert.ToInt32(rw["Act_StatusID"]),
                                                     ACT_BatchID = string.IsNullOrWhiteSpace(Convert.ToString(rw["JA_BatchID"])) ? (Int64?)(null) : Convert.ToInt64(rw["JA_BatchID"]),
                                                     RejectRemark = Convert.ToString(rw["RejectRemark"]),
                                                     Inst_BatchID = string.IsNullOrWhiteSpace(Convert.ToString(rw["JobDiaryBatchID"])) ? (Int64?)(null) : Convert.ToInt64(rw["JobDiaryBatchID"]),
                                                     NumberOfDays = string.IsNullOrWhiteSpace(Convert.ToString(rw["NoOfDays"])) ? (int?)(null) : Convert.ToInt32(rw["NoOfDays"]),
                                                     JobType= JobType
                                                 }).ToList();

                }

                jobDiary.instructionSheetGrids = result.AsEnumerable<InstructionSheetGrid>();
                jobDiary.JobType = JobType;
                return jobDiary;

            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoginID), "JobAllocationDAL", "GetBulkInstDetailById", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }


        }

        public ActivityAllocationModel GetActivityJobAllocationById(int LoginID, Int64[] ActivityID, Int64? BatchID = -1,Int16 JobType=1)
        {
            ActivityAllocationModel jobDiary = new ActivityAllocationModel();
            try
            {
                jobDiary.ActivityIds = ActivityID.OfType<Int64>().ToList();
                jobDiary.JobType = JobType;
                string InstIDxml = new XElement("ActivityIDs", from emp in jobDiary.ActivityIds
                                                               select new XElement("ActivityID", new XAttribute("ID", Convert.ToString(emp).Trim()))
                                              ).ToString();

                

                string query = string.Format("exec [Warehouse].[GetActivityAllocationForDisplay] @SP_LoginID={0},@SP_BatchID={1},@SP_ActivityIDXML={2},@SP_JobType={3}",
                Convert.ToString(LoginID),CSubs.QSafeValue(Convert.ToString(BatchID)),
                CSubs.QSafeValue(InstIDxml), CSubs.QSafeValue(Convert.ToString(JobType))
                );
                DataSet dataSet = CSubs.GetDataSet(query);

                if (dataSet!=null && dataSet.Tables.Count > 0 && dataSet.Tables[0] != null && dataSet.Tables[0].Rows.Count > 0)
                {
                    jobDiary.ACT_Status = Convert.ToString(dataSet.Tables[0].Rows[0]["Act_Status"]);
                    jobDiary.ACT_StatusID = !string.IsNullOrWhiteSpace(Convert.ToString(dataSet.Tables[0].Rows[0]["Act_StatusID"])) ? Convert.ToInt32(dataSet.Tables[0].Rows[0]["Act_StatusID"]) : 0;
                    jobDiary.CancelRemark = Convert.ToString(dataSet.Tables[0].Rows[0]["RejectedRemark"]);
                    jobDiary.JobNo = string.Join(", ", dataSet.Tables[0].Rows.OfType<DataRow>().Select(r => Convert.ToString(r["JobNo"])).Distinct());
                    jobDiary.ShipperName = string.Join(", ", dataSet.Tables[0].Rows.OfType<DataRow>().Select(r => Convert.ToString(r["Shipper"])).Distinct());
                    jobDiary.BatchID = !string.IsNullOrWhiteSpace(Convert.ToString(dataSet.Tables[0].Rows[0]["JA_BatchID"])) ? Convert.ToInt64(dataSet.Tables[0].Rows[0]["JA_BatchID"]) : -1 ;
                    jobDiary.RateComponentID = Convert.ToInt32(dataSet.Tables[0].Rows[0]["RateComponentID"]);
                    jobDiary.BranchID = Convert.ToInt32(dataSet.Tables[0].Rows[0]["BranchID"]); 
                    jobDiary.CompanyID = Convert.ToInt32(dataSet.Tables[0].Rows[0]["CompanyID"]);
                    jobDiary.PackInventID = !string.IsNullOrWhiteSpace(Convert.ToString(dataSet.Tables[0].Rows[0]["PackInventID"])) ? Convert.ToInt32(dataSet.Tables[0].Rows[0]["PackInventID"]) : (int?)null;
                }

                ////Services details
                if (dataSet != null && dataSet.Tables.Count > 1 && dataSet.Tables[1] != null && dataSet.Tables[1].Rows.Count > 0)
                {
                    jobDiary.services = (from item in dataSet.Tables[1].AsEnumerable()
                                        select new JobService()
                                        {
                                            SD_ID = Convert.ToInt64(item["SD_ID"]),
                                            //ActivityID = Convert.ToInt64(item["ActivityID"]),
                                            //MoveId = Convert.ToInt64(item["MoveID"]),
                                            ServiceID = Convert.ToInt32(item["ServiceID"]),
                                            ServiceName = Convert.ToString(item["ServiceName"]),
                                            Description = Convert.ToString(item["ServiceDescription"]),
                                            FromDate = item["FromDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(item["FromDate"]),
                                            ToDate = item["FromDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(item["FromDate"]),
                                            Cost = Convert.ToDouble(item["Cost"]),
                                            PJR_Status = Convert.ToInt32(item["PJR_Status"]),
                                        }).ToList();

                }


                ////Crew details
                if (dataSet != null && dataSet.Tables.Count > 2 && dataSet.Tables[2] != null && dataSet.Tables[2].Rows.Count > 0)
                {
                    jobDiary.jobCrew.members = (from item in dataSet.Tables[2].AsEnumerable()
                                               select new CrewMember()
                                               {
                                                   CWMID = Convert.ToInt64(item["CD_ID"]),
                                                   EmpID = Convert.ToInt32(item["EmpID"]),
                                                   CardEmpCode = Convert.ToString(item["CardEmpCode"]),
                                                   EmpName = Convert.ToString(item["EmpName"]),
                                                   EffectiveFrom = Convert.ToDateTime(item["FromDateTime"]),
                                                   EffectiveTo = Convert.ToDateTime(item["ToDateTime"]),
                                                   PJR_Status = Convert.ToInt32(item["PJR_Status"]),
                                                   ShowIsSupervisor = Convert.ToBoolean(item["ShowSupervisor"]),
                                                   IsSupervisor =Convert.ToBoolean(item["IsSupervisor"])
                                               }).ToList();

                }

                ////Vehicle
                if (dataSet != null && dataSet.Tables.Count > 3 && dataSet.Tables[3] != null && dataSet.Tables[3].Rows.Count > 0)
                {

                    jobDiary.jobVehicleList = (from item in dataSet.Tables[3].AsEnumerable()
                                              select new JobVehicle()
                                              {
                                                  //ActivityID = Convert.ToInt64(item["ActivityID"]),
                                                  //MoveID = Convert.ToInt64(item["MoveID"]),
                                                  V_ID = Convert.ToInt64(item["VD_ID"]),
                                                  PurposeID = Convert.ToInt32(item["PurposeID"]),
                                                  Purpose = Convert.ToString(item["PurposeName"]),
                                                  VehicleID = !string.IsNullOrWhiteSpace(Convert.ToString(item["VehicleID"]))? Convert.ToInt32(item["VehicleID"]) : (int?)null,
                                                  VehicleNo = Convert.ToString(item["VehicleNo"]),
                                                  DriverID = !string.IsNullOrWhiteSpace(Convert.ToString(item["DriverID"])) ? Convert.ToInt32(item["DriverID"]) : (int?)null,
                                                  Driver = Convert.ToString(item["DriverName"]),
                                                  VehicleTypeID = Convert.ToString(item["VehicleTypeID"]),
                                                  VehicleType = Convert.ToString(item["VehicleType"]),
                                                  DriverType = Convert.ToString(item["DriverType"]),
                                                  DriverTypeID = Convert.ToString(item["DriverTypeID"]),
                                                  FromDate = item["FromDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(item["FromDate"]),
                                                  ToDate = item["ToDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(item["ToDate"]),
                                                  PJR_Status = Convert.ToInt32(item["PJR_Status"]),
                                                  V_Remark = Convert.ToString(item["Remark"]),
                                                  V_Cost = !string.IsNullOrWhiteSpace(Convert.ToString(item["ApproxCost"])) ? Convert.ToInt64(item["ApproxCost"]) : (float?)null,
                                                  Approve_StatusId = !string.IsNullOrWhiteSpace(Convert.ToString(item["StatusID"])) ? Convert.ToInt32(item["StatusID"]) : (Int32?)null,
                                                  Approve_Status = Convert.ToString(item["StatusName"]),
                                                  IsApprover = Convert.ToBoolean(item["IsApprover"]),
                                                  Approve_By = Convert.ToString(item["ApprovedBy"]),
                                                  Approve_Date = item["ApprovedDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(item["ApprovedDate"]),
                                                  Approve_Remark = Convert.ToString(item["ApprovedRemark"]),

                                                  MovementID = !string.IsNullOrWhiteSpace(Convert.ToString(item["MovementID"])) ? Convert.ToInt32(item["MovementID"]) : (Int32?)null,
                                                  MovementName = Convert.ToString(item["MovementName"]),
                                                  SupplierID = !string.IsNullOrWhiteSpace(Convert.ToString(item["SupplierID"])) ? Convert.ToInt32(item["SupplierID"]) : (Int32?)null,
                                                  SupplierName = Convert.ToString(item["SupplierName"]),
                                                  DimensionID = !string.IsNullOrWhiteSpace(Convert.ToString(item["DimensionID"])) ? Convert.ToInt32(item["DimensionID"]) : (Int32?)null,
                                                  DimensionName = Convert.ToString(item["DimensionName"]),
                                                  ReasonID = !string.IsNullOrWhiteSpace(Convert.ToString(item["ReasonID"])) ? Convert.ToInt32(item["ReasonID"]) : (Int32?)null,
                                                  ReasonName = Convert.ToString(item["ReasonName"]),
                                                  FromLocation = Convert.ToString(item["FromLocation"]),
                                                  ToLocation = Convert.ToString(item["ToLocation"]),
                                                  VolumeCFT = Convert.ToString(item["VolumeCFT"])
                                              }).ToList();
                }


                ///Documents

                if (dataSet != null && dataSet.Tables.Count > 4 && dataSet.Tables[4] != null && dataSet.Tables[4].Rows.Count > 0)
                {
                    jobDiary.docUpload.docLists = (from item in dataSet.Tables[4].AsEnumerable()
                                                  select new JobDocument()
                                                  {
                                                      FileID = Convert.ToInt32(item["FileID"]),
                                                      DocType = Convert.ToString(item["DocTypeName"]),
                                                      DocName = Convert.ToString(item["DocName"]),
                                                      DocDescription = Convert.ToString(item["Description"]),
                                                      FileName = Convert.ToString(item["DocFileName"]),
                                                      UploadBy = Convert.ToString(item["UploadBy"]),

                                                  }).ToList();

                }
                ////Material
                if (dataSet != null && dataSet.Tables.Count > 5 && dataSet.Tables[5] != null && dataSet.Tables[5].Rows.Count > 0)
                {
                    jobDiary.materialUsed = (from item in dataSet.Tables[5].AsEnumerable()
                                            select new MaterialUsed()
                                            {
                                                M_ID = Convert.ToInt64(item["M_ID"]),
                                                MaterailId = Convert.ToInt32(item["MaterailID"]),
                                                Materail = Convert.ToString(item["MaterialName"]),
                                                IssuedQty = !string.IsNullOrWhiteSpace(Convert.ToString(item["Issued_qty"])) ? Convert.ToInt32(item["Issued_qty"]) : (int?)null,
                                                //UsedQty = !string.IsNullOrWhiteSpace(Convert.ToString(item["Used_qty"])) ? Convert.ToInt32(item["Used_qty"]) : (int?)null,
                                                //ReturnQty = !string.IsNullOrWhiteSpace(Convert.ToString(item["Return_qty"])) ? Convert.ToInt32(item["Return_qty"]) : (int?)null,
                                                Rate = Convert.ToInt64(item["Rate"]),
                                                PJR_Status = Convert.ToInt32(item["PJR_Status"]),
                                            }).ToList();

                }

                if (dataSet != null && dataSet.Tables.Count > 6 && dataSet.Tables[6] != null && dataSet.Tables[6].Rows.Count > 0)
                {
                    jobDiary.outsideLabours = (from item in dataSet.Tables[6].AsEnumerable()
                                              select new OutsideLabour()
                                              {
                                                  OLabourID = Convert.ToInt64(item["OLabourID"]),
                                                  CrewVendorID = Convert.ToInt32(item["VendorID"]),
                                                  VendorName = Convert.ToString(item["VendorName"]),
                                                  NoOfPerson = Convert.ToInt32(item["NumberOfPerson"]),
                                                  PJR_Status = Convert.ToInt32(item["PJR_Status"]),
                                              }).ToList();
                }
                else
                {
                    jobDiary.outsideLabours.Add(new OutsideLabour() { CrewVendorID = 0 });
                }

                return jobDiary;

            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoginID), "JobAllocationDAL", "GetActivityJobAllocationById", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

        }

        public DataSet GetJobActivityDetailById(int LoginID, Int64 InstID, Int64? ActivityID)
        {
            DataSet Dtobj = new DataSet();

            try
            {
                string query = string.Format("exec [Warehouse].[GetJobActivityForDisplay] @SP_InstID={0},@SP_ActivityID={1},@SP_LoginID={2}",
                CSubs.QSafeValue(Convert.ToString(InstID)), CSubs.QSafeValue(Convert.ToString(ActivityID)),CSubs.QSafeValue(Convert.ToString(LoginID)));

                Dtobj = CSubs.GetDataSet(query);
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoginID), "JobAllocationDAL", "GetJobActivityDetailById", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
            return Dtobj;
        }

        public DataSet GetCrewMembers(int LoginID,DateTime From, DateTime To, int CrewID,Int64 BatchID)
        {

            DataSet Dtobj = new DataSet();

            try
            {
                string query = string.Format("exec [Warehouse].[GetJobAllocationCrewDetails] @SP_JA_BachID={0},@SP_CrewID={1},@SP_LoginID={2},@SP_FromDate={3},@SP_ToDate={4}",
                 CSubs.QSafeValue(Convert.ToString(BatchID)), CSubs.QSafeValue(Convert.ToString(CrewID)),CSubs.QSafeValue(Convert.ToString(LoginID))
                 , CSubs.QSafeValue(Convert.ToString(From)), CSubs.QSafeValue(Convert.ToString(To)));

                Dtobj = CSubs.GetDataSet(query);
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoginID), "JobAllocationDAL", "GetCrewMembers", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

            return Dtobj;
        }

        public Dictionary<string,string> GetVacantCrew(DateTime From, DateTime To,Int64 BatchID, int LoginID)
        {
            Dictionary<string, string> CrewList = new Dictionary<string, string>();

            try
            {
                string query = string.Format("EXEC [Warehouse].[GetVacantCrew] @SP_FromDate={0},@SP_ToDate={1},@SP_JA_BachID={2},@SP_LoginID={3},@SP_CompID={4}",
                 CSubs.QSafeValue(Convert.ToString(From)), CSubs.QSafeValue(Convert.ToString(To)), CSubs.QSafeValue(Convert.ToString(BatchID)),CSubs.QSafeValue(Convert.ToString(Convert.ToString(LoginID)))
                 ,CSubs.QSafeValue(Convert.ToString(UserSession.GetUserSession().CompanyID))
                 );

                DataTable dt = CSubs.GetDataTable(query);

                if (dt!=null && dt.Rows.Count>0)
                {
                    CrewList = dt.AsEnumerable().ToDictionary<DataRow, string, string>(row => row[0].ToString() , row => row[1].ToString());
                }

                return CrewList;
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoginID), "JobAllocationDAL", "GetVacantCrew", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

        }

        public DataSet GetPJR_DJR_Details(int LoginID, int MoveID,int? instID=-1)
        {
            DataSet Dtobj = new DataSet();

            try
            {
                 string query = string.Format("[Warehouse].[GetJobPJR_DJRForDisplay]  @SP_MoveID={0},@SP_InstID={1},@SP_LoginID={2}",
                 CSubs.QSafeValue(Convert.ToString(MoveID)), CSubs.QSafeValue(Convert.ToString(instID)), CSubs.QSafeValue(Convert.ToString(LoginID)));

                Dtobj = CSubs.GetDataSet(query);

            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoginID), "JobAllocationDAL", "GetPJR_DJR_Details", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

            return Dtobj;
        }

        public bool InsertActivityAllocation(ActivityAllocationModel jobActivity, out string result)
        {
            result = string.Empty;
            //string FilePath=configuration
            try
            {

                using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {
                        string ActivityXml = string.Empty;
                        string InstXml = string.Empty;

                        string CrewMembersXml = string.Empty;
                        string DocXml = string.Empty;
                        string VehicleXml = string.Empty;
                        string ServiceXml = string.Empty;
                        string MaterialXml = string.Empty;
                        string OutLabourXml = string.Empty;

                        if (jobActivity.ActivityIds != null && jobActivity.ActivityIds.Count>0)
                        {
                            ActivityXml = new XElement("Activity", from emp in jobActivity.ActivityIds
                                                                   select new XElement("Activities",
                                                                   new XElement("ActivityID", Convert.ToString(emp).Trim()))).ToString();

                            if (!string.IsNullOrWhiteSpace(ActivityXml))
                            {
                                ActivityXml = Regex.Replace(ActivityXml, @"\t|\n|\r| ", ""); //InstIDxml.Replace("\r\n", "");
                            }
                        }

                        if (jobActivity.InstIds!=null && jobActivity.InstIds.Count > 0)
                        {
                            InstXml = new XElement("Inst", from emp in jobActivity.InstIds
                                                                   select new XElement("Insts",
                                                                   new XElement("InstID", Convert.ToString(emp).Trim()))).ToString();

                            if (!string.IsNullOrWhiteSpace(InstXml))
                            {
                                InstXml = Regex.Replace(InstXml, @"\t|\n|\r| ", ""); //InstIDxml.Replace("\r\n", "");
                            }
                        }

                        if (jobActivity.services != null && jobActivity.services.Count>0)
                        {
                            ServiceXml = new XElement("Services", from emp in jobActivity.services
                                                                  select new XElement("Service",
                                                         new XElement("ID", emp.SD_ID),
                                                         new XElement("ServiceID", emp.ServiceID),
                                                         new XElement("Description", emp.Description),
                                                         new XElement("FromDate", emp.FromDate),
                                                         new XElement("ToDate", emp.ToDate),
                                                         new XElement("Cost", emp.Cost)

                                                     )).ToString();

                            if (!string.IsNullOrWhiteSpace(ServiceXml))
                            {
                                ServiceXml = Regex.Replace(ServiceXml, @"\t|\n|\r", ""); //InstIDxml.Replace("\r\n", "");
                            }
                        }
                        
                        if (jobActivity.jobCrew.members != null && jobActivity.jobCrew.members.Count>0)
                        {
                            CrewMembersXml = new XElement("Crews", from emp in jobActivity.jobCrew.members
                                                                   select new XElement("Crew",
                                                          new XElement("ID", emp.CWMID),
                                                          new XElement("EmpID", emp.EmpID),
                                                          new XElement("FromDate", emp.EffectiveFrom),
                                                          new XElement("ToDate", emp.EffectiveTo),
                                                          new XElement("IsSupervisor", emp.IsSupervisor)
                                                      )).ToString();
                            if (!string.IsNullOrWhiteSpace(CrewMembersXml))
                            {
                                CrewMembersXml = Regex.Replace(CrewMembersXml, @"\t|\n|\r", ""); //InstIDxml.Replace("\r\n", "");
                            }
                        }
                        
                        if (jobActivity.jobVehicleList != null && jobActivity.jobVehicleList.Count>0)
                        {
                            VehicleXml = new XElement("Vehicles", from emp in jobActivity.jobVehicleList
                                                                  select new XElement("Vehicle",
                                                                  new XElement("ID", emp.V_ID),
                                                         new XElement("PurposeID", emp.PurposeID),
                                                         new XElement("VehicleID", emp.VehicleID),
                                                         new XElement("DriverID", emp.DriverID),
                                                         new XElement("VehicleNo", emp.VehicleNo),
                                                         new XElement("Driver", emp.Driver),
                                                         new XElement("VehicleType", emp.VehicleTypeID),
                                                         new XElement("DriverType", emp.DriverTypeID),
                                                         new XElement("FromDate", emp.FromDate),
                                                         new XElement("ToDate", emp.ToDate),
                                                         new XElement("Remark", emp.V_Remark),
                                                         new XElement("Cost", emp.V_Cost),
                                                         new XElement("MovementID",emp.MovementID),
                                                         new XElement("SupplierID", emp.SupplierID),
                                                         new XElement("DimensionID", emp.DimensionID),
                                                         new XElement("ReasonID", emp.ReasonID),
                                                         new XElement("FromLoc", emp.FromLocation),
                                                         new XElement("ToLoc", emp.ToLocation),
                                                         new XElement("VolCFT", emp.VolumeCFT)
                                                     )).ToString();

                            if (!string.IsNullOrWhiteSpace(VehicleXml))
                            {
                                VehicleXml = Regex.Replace(VehicleXml, @"\t|\n|\r", ""); //InstIDxml.Replace("\r\n", "");
                            }
                        }
                        
                        //if (jobActivity.docUpload.docLists != null && jobActivity.docUpload.docLists.Count>0)
                        //{
                        //    DocXml = new XElement("Downloads", from emp in jobActivity.docUpload.docLists
                        //                                       select new XElement("Download",
                        //                              new XElement("DocID", emp.DocID),
                        //                              new XElement("DocTypeID", emp.DocTypeID)
                        //                          )).ToString();
                        //}
                        
                        if (!string.IsNullOrWhiteSpace(DocXml))
                        {
                            DocXml = Regex.Replace(DocXml, @"\t|\n|\r", ""); //InstIDxml.Replace("\r\n", "");
                        }

                        if (jobActivity.materialUsed!=null && jobActivity.materialUsed.Count>0)
                        {
                            MaterialXml = new XElement("Materials", from emp in jobActivity.materialUsed
                                                                    select new XElement("Material",
                                                                    new XElement("ID", emp.M_ID),
                                                           new XElement("MaterailId", emp.MaterailId),
                                                           new XElement("Rate", emp.Rate),
                                                           new XElement("ReturnQty", emp.ReturnQty),
                                                           new XElement("IssuedQty", emp.IssuedQty),
                                                           new XElement("UsedQty", emp.UsedQty)
                                                       )).ToString();

                            if (!string.IsNullOrWhiteSpace(MaterialXml))
                            {
                                MaterialXml = Regex.Replace(MaterialXml, @"\t|\n|\r", ""); //InstIDxml.Replace("\r\n", "");
                            }

                        }

                        if (jobActivity.outsideLabours.Count>0 && jobActivity.outsideLabours[0].CrewVendorID>0)
                        {
                            OutLabourXml= new XElement("OutLabours", from emp in jobActivity.outsideLabours
                                                                     select new XElement("outsideLabour",
                                                                     new XElement("ID", emp.OLabourID),
                                                           new XElement("VendorID", emp.CrewVendorID),
                                                           new XElement("NoOfPerson", emp.NoOfPerson)
                                                       )).ToString();

                            if (!string.IsNullOrWhiteSpace(OutLabourXml))
                            {
                                OutLabourXml = Regex.Replace(OutLabourXml, @"\t|\n|\r", ""); //InstIDxml.Replace("\r\n", "");
                            }
                        } 


                        conn.AddCommand("[Warehouse].[AddEditActivityAllocation]", QueryType.Procedure);
                        //conn.AddParameter(ParameterOF.PROCEDURE, "@SP_InstID", SqlDbType.BigInt, 0, ParameterDirection.Input, jobActivity.InstID);
                        //conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ActivityID", SqlDbType.BigInt, 0, ParameterDirection.InputOutput, jobActivity.ActivityID);
                        //conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ActivityTypeID", SqlDbType.Int, 0, ParameterDirection.Input, jobActivity.ActivityTypeID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CrewID", SqlDbType.Int, 0, ParameterDirection.Input, jobActivity.jobCrew.CrewID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_SuperviserID", SqlDbType.Int, 0, ParameterDirection.Input, jobActivity.jobCrew.SuperviserID);
                        //conn.AddParameter(ParameterOF.PROCEDURE, "@SP_FromLoc", SqlDbType.VarChar, 50, ParameterDirection.Input, CSubs.PSafeValue(jobActivity.FromLocation));
                        //conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ToLoc", SqlDbType.VarChar, 50, ParameterDirection.Input, CSubs.PSafeValue(jobActivity.ToLocation));
                        //conn.AddParameter(ParameterOF.PROCEDURE, "@SP_FromDate", SqlDbType.DateTime, 0, ParameterDirection.Input, Convert.ToDateTime(jobActivity.FromDate));
                        //conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ToDate", SqlDbType.DateTime, 0, ParameterDirection.Input, Convert.ToDateTime(jobActivity.ToDate));
                        //conn.AddParameter(ParameterOF.PROCEDURE, "@SP_RepTime", SqlDbType.Time, 0, ParameterDirection.Input, (TimeSpan)jobActivity.RepTime);
                        //conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Remark", SqlDbType.NVarChar, 500, ParameterDirection.Input, (jobActivity.Remark));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_BatchID", SqlDbType.BigInt, 0, ParameterDirection.InputOutput, jobActivity.BatchID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_InstIdXml", SqlDbType.Xml, 0, ParameterDirection.Input, InstXml);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ActivityIdXML", SqlDbType.Xml, -1, ParameterDirection.Input, ActivityXml);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CrewXML", SqlDbType.Xml, -1, ParameterDirection.Input, CrewMembersXml);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_AdditionalServiceXML", SqlDbType.Xml, -1, ParameterDirection.Input, ServiceXml);
                        //conn.AddParameter(ParameterOF.PROCEDURE, "@SP_DocXml", SqlDbType.Xml, -1, ParameterDirection.Input, DocXml);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_VehiclesXML", SqlDbType.Xml, -1, ParameterDirection.Input, VehicleXml);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_MaterialXml", SqlDbType.Xml, -1, ParameterDirection.Input, MaterialXml);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_OutLabourXml", SqlDbType.Xml, -1, ParameterDirection.Input, OutLabourXml);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_SubmitType", SqlDbType.VarChar, 50, ParameterDirection.Input, CSubs.PSafeValue(jobActivity.submit));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CancelRemark", SqlDbType.NVarChar, 250, ParameterDirection.Input, CSubs.PSafeValue(jobActivity.CancelRemark));

                        //if (jobActivity.docUpload.file != null)
                        //{
                        //    conn.AddParameter(ParameterOF.PROCEDURE, "@SP_DocTypeID", SqlDbType.Int, 0, ParameterDirection.Input, jobActivity.docUpload.DocTypeID);
                        //    conn.AddParameter(ParameterOF.PROCEDURE, "@SP_DocID", SqlDbType.Int, 0, ParameterDirection.Output);
                        //    string FilePath = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["JobActityFile"]);
                        //    conn.AddParameter(ParameterOF.PROCEDURE, "@SP_FilePath", SqlDbType.NVarChar, 500, ParameterDirection.InputOutput, CSubs.PSafeValue(FilePath));
                        //    conn.AddParameter(ParameterOF.PROCEDURE, "@SP_DocName", SqlDbType.NVarChar, 500, ParameterDirection.Input, CSubs.PSafeValue(jobActivity.docUpload.file.FileName));
                        //    conn.AddParameter(ParameterOF.PROCEDURE, "@SP_EXT", SqlDbType.NVarChar, 10, ParameterDirection.Input, CSubs.PSafeValue(Path.GetExtension(jobActivity.docUpload.file.FileName)));
                        //}
                        
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
                                jobActivity.BatchID = Convert.ToInt64(conn.GetParameterValue(ParameterOF.PROCEDURE, "@SP_BatchID"));
                                //if (jobActivity.docUpload.file != null)
                                //{
                                //    string File = Convert.ToString(conn.GetParameterValue(ParameterOF.PROCEDURE, "@SP_FilePath"));
                                //    string DocID = Convert.ToString(conn.GetParameterValue(ParameterOF.PROCEDURE, "@SP_DocID"));
                                //    if (!string.IsNullOrWhiteSpace(File) && !string.IsNullOrWhiteSpace(DocID))
                                //    {
                                //        jobActivity.docUpload.file.SaveAs(File);
                                //    }
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
                throw new DataAccessException(Convert.ToString(UserSession.GetUserSession().LoginID), "jobAllocationDAL", "InsertActivity", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
        }

        public bool DeleteActivityAllocation(Int64 BrachID,Int64 ActivityID,int LoginID, out string result)
        {
            result = string.Empty;
            try
            {
                using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {
                        conn.AddCommand("[Warehouse].[DeleteActivityAllocation]", QueryType.Procedure);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_BatchID", SqlDbType.BigInt, 0, ParameterDirection.Input, (BrachID));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ACTIVITYID", SqlDbType.BigInt, 0, ParameterDirection.Input, (ActivityID));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LOGINID", SqlDbType.Int, 0, ParameterDirection.Input, UserSession.GetUserSession().LoginID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@RETURNSTATUS", SqlDbType.SmallInt, 0, ParameterDirection.ReturnValue);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_MESSAGE", SqlDbType.NVarChar, 500, ParameterDirection.Output);
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
                throw new DataAccessException(Convert.ToString(LoginID), "JobAllocationDAL", "DeleteActivityAllocation", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

        }

        public Dictionary<int,float> GetMaterialRate(int[] MaterailID, int LoginID)
        {
            Dictionary<int, float> CrewList = new Dictionary<int, float>();

            try
            {
                //string InstIDxml = new XElement("MaterialIDs", from emp in MaterailID select new XElement("MaterialID", Convert.ToString(emp).Trim())).ToString();

                var branchesXml = MaterailID.Select(i => new XElement("Material", new XAttribute("MaterialID", i)));
                string JobStr = new XElement("MaterialIDs", branchesXml).ToString();

                //if (!string.IsNullOrWhiteSpace(JobStr))
                //{
                //    //JobStr = Regex.Replace(JobStr, @"\t|\n|\r| ", ""); //InstIDxml.Replace("\r\n", "");
                //}

                string query = string.Format("EXEC [Warehouse].[GetMaterialCost] @SP_MaterialIDs={0},@SP_LoginID={1}",
                CSubs.QSafeValue(Convert.ToString(JobStr)),CSubs.QSafeValue(Convert.ToString(LoginID)));

                DataTable dt = CSubs.GetDataTable(query);

                if (dt != null && dt.Rows.Count > 0)
                {
                    CrewList = dt.AsEnumerable().ToDictionary<DataRow, int, float>(row => Convert.ToInt32(row[0]), row => Convert.ToInt64(row[1]));
                }

                return CrewList;
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(UserSession.GetUserSession().LoginID), "jobAllocationDAL", "GetMaterialRate", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
        }

        public DataTable getActivityDetailById(int LoginID, Int64 InstID, Int64? ActivityID = -1)
        {
            try
            {
                string query = string.Format("exec [Warehouse].[GetActivityForDisplay] @SP_InstID={0},@SP_ActivityID={1},@SP_LoginID={2}",
                CSubs.QSafeValue(Convert.ToString(InstID)), CSubs.QSafeValue(Convert.ToString(ActivityID)), CSubs.QSafeValue(Convert.ToString(LoginID)));

                DataTable Dtobj = CSubs.GetDataTable(query);

                return Dtobj;
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(UserSession.GetUserSession().LoginID), "jobAllocationDAL", "getActivityDetailById", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
        }

        public bool AddEditActivity(JobActivity model, out string result)
        {
            result = string.Empty;

            try
            {

                using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {
                        
                        conn.AddCommand("[Warehouse].[AddEditActivity]", QueryType.Procedure);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_InstID", SqlDbType.BigInt, 0, ParameterDirection.Input, model.InstID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ActivityID", SqlDbType.BigInt, 0, ParameterDirection.InputOutput, model.ActivityID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ActivityTypeID", SqlDbType.Int, 0, ParameterDirection.Input, model.ActivityTypeID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_FromLoc", SqlDbType.VarChar, 50, ParameterDirection.Input, CSubs.PSafeValue(model.FromLocation));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ToLoc", SqlDbType.VarChar, 50, ParameterDirection.Input, CSubs.PSafeValue(model.ToLocation));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_FromDate", SqlDbType.DateTime, 0, ParameterDirection.Input, model.FromDate);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ToDate", SqlDbType.DateTime, 0, ParameterDirection.Input, model.ToDate);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_RepTime", SqlDbType.Time, 0, ParameterDirection.Input, model.RepTime);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Remark", SqlDbType.NVarChar, 500, ParameterDirection.Input, CSubs.PSafeValue(model.Remark));
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
                                model.ActivityID = Convert.ToInt64(conn.GetParameterValue(ParameterOF.PROCEDURE, "@SP_ActivityID"));
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
                throw new DataAccessException(Convert.ToString(UserSession.GetUserSession().LoginID), "jobAllocationDAL", "AddEditActivity", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
        }

        public JobDocument GetDownloadFile(Int64 DocID, Int64 BatchID, int LoginID)
        {
            JobDocument job = new JobDocument();
            try
            {
                DataTable dt = CSubs.GetDataTable(string.Format("exec [Warehouse].[GetDocumentDetail] @SP_DocID={0},@SP_BatchID={1},@SP_LoginID={2}", 
                    CSubs.QSafeValue(Convert.ToString(DocID))
                    , CSubs.QSafeValue(Convert.ToString(BatchID))
                    , CSubs.QSafeValue(Convert.ToString(LoginID))));

                if (dt != null && dt.Rows.Count > 0)
                {
                    job.FileID = Convert.ToInt32(dt.Rows[0]["FileID"]);
                    job.DocTypeID = Convert.ToInt32(dt.Rows[0]["DocTypeID"]);
                    //job.DocNameID = Convert.ToInt32(dt.Rows[0]["DocNameID"]);
                    job.FilePath = Convert.ToString(dt.Rows[0]["DocFilePath"]);
                    job.FileName = Convert.ToString(dt.Rows[0]["DocFileName"]);
                }

            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoginID), "JobAllocationDAL", "GetDownloadFile", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

            return job;

        }

        public DataSet GetEmployeeAllocation(int LoginID,int EmpID,DateTime FromDate,DateTime ToDate,Int64? BatchID=-1)
        {
            try
            {
                DataSet ds = CSubs.GetDataSet(string.Format("exec [Warehouse].[GetCrewMemberExistingAllocationGrid] @SP_EmpID={0},@SP_FromDate={1},@SP_ToDate={2},@SP_LoginID={3},@SP_BatchID={4}",
                    CSubs.QSafeValue(Convert.ToString(EmpID))
                    , CSubs.QSafeValue(FromDate != null ? FromDate.ToString("dd-MMM-yyyy HH:mm") : null)
                    , CSubs.QSafeValue(ToDate!=null ? ToDate.ToString("dd-MMM-yyyy HH:mm") : null)
                    , CSubs.QSafeValue(Convert.ToString(LoginID))
                    , CSubs.QSafeValue(Convert.ToString(BatchID))
                    ));
                
                return ds;
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoginID), "JobAllocationDAL", "GetEmployeeAllocation", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

        }

        public bool UpdateHiredVehicleApprovalStatus(ActivityAllocationModel model,int LoginID, out string result)
        {
            try
            {
                using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {
                        conn.AddCommand("[Warehouse].[UpdateHiredVehicleApproveStatus]", QueryType.Procedure);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_SubmitType", SqlDbType.VarChar, 50, ParameterDirection.Input, CSubs.PSafeValue(model.submit));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_V_ID", SqlDbType.Int, 0, ParameterDirection.Input, model.submit.Equals("ApprovalSave",StringComparison.OrdinalIgnoreCase)? model.hireVehileApproval.V_ID : model.hireVehileSendForApprove.V_ID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Remark", SqlDbType.VarChar, 500, ParameterDirection.Input, model.submit.Equals("ApprovalSave", StringComparison.OrdinalIgnoreCase) ? CSubs.PSafeValue(model.hireVehileApproval.Remark) :CSubs.PSafeValue(model.hireVehileSendForApprove.Remark));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Cost", SqlDbType.Float, 0, ParameterDirection.Input, model.hireVehileSendForApprove.Cost);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@sp_IsApproved", SqlDbType.Bit, 0, ParameterDirection.Input, model.hireVehileApproval.IsApproved=="1");
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LoginID", SqlDbType.Int, 0, ParameterDirection.Input,LoginID);
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
                throw new DataAccessException(Convert.ToString(LoginID), "jobAllocationDAL", "UpdateHiredVehicleApprovalStatus", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

        }

        public bool InsertSupervisorForDigitalInventory(ActivityAllocationModel model, int LoginID, out string result)
        {
            try
            {
                using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {
                        string ActivityXml = string.Empty;
                        if (model.ActivityIds != null && model.ActivityIds.Count > 0)
                        {
                            ActivityXml = new XElement("Activity", from emp in model.ActivityIds
                                                                   select new XElement("Activities",
                                                                   new XElement("ActivityID", Convert.ToString(emp).Trim()))).ToString();

                            if (!string.IsNullOrWhiteSpace(ActivityXml))
                            {
                                ActivityXml = Regex.Replace(ActivityXml, @"\t|\n|\r| ", ""); //InstIDxml.Replace("\r\n", "");
                            }
                        }
                        string InstXml = string.Empty;
                        if (model.InstIds != null && model.InstIds.Count > 0)
                        {
                            InstXml = new XElement("Inst", from emp in model.InstIds
                                                           select new XElement("Insts",
                                                           new XElement("InstID", Convert.ToString(emp).Trim()))).ToString();

                            if (!string.IsNullOrWhiteSpace(InstXml))
                            {
                                InstXml = Regex.Replace(InstXml, @"\t|\n|\r| ", ""); //InstIDxml.Replace("\r\n", "");
                            }
                        }

                        string CrewMembersXml = string.Empty;
                        if (model.jobCrew.members != null && model.jobCrew.members.Count > 0)
                        {
                            CrewMembersXml = new XElement("Crews", from emp in model.jobCrew.members
                                                                   select new XElement("Crew",
                                                          new XElement("ID", emp.CWMID),
                                                          new XElement("EmpID", emp.EmpID),
                                                          new XElement("FromDate", emp.EffectiveFrom),
                                                          new XElement("ToDate", emp.EffectiveTo),
                                                          new XElement("IsSupervisor", emp.IsSupervisor)
                                                      )).ToString();
                            if (!string.IsNullOrWhiteSpace(CrewMembersXml))
                            {
                                CrewMembersXml = Regex.Replace(CrewMembersXml, @"\t|\n|\r", ""); //InstIDxml.Replace("\r\n", "");
                            }
                        }

                        conn.AddCommand("[Warehouse].[InsertSupervisorForDigitalInventory]", QueryType.Procedure);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_SubmitType", SqlDbType.VarChar, 50, ParameterDirection.Input, CSubs.PSafeValue(model.submit));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_BatchID", SqlDbType.BigInt, 0, ParameterDirection.InputOutput, model.BatchID);
                        //conn.AddParameter(ParameterOF.PROCEDURE, "@SP_InstIdXml", SqlDbType.Xml, 0, ParameterDirection.Input, InstXml);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ActivityIdXML", SqlDbType.Xml, -1, ParameterDirection.Input, ActivityXml);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CrewXML", SqlDbType.Xml, -1, ParameterDirection.Input, CrewMembersXml);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LoginID", SqlDbType.Int, 0, ParameterDirection.Input, LoginID);
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
                throw new DataAccessException(Convert.ToString(LoginID), "jobAllocationDAL", "InsertSupervisorForDigitalInventory", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

        }

        public DataSet GetCrewUtilizationDashboard(int LoginID,int CompId,bool IsRmc,DateTime ForMonthDate,int WarehoseId)
        {
            try
            {
                string query = string.Format("exec [Report].[WH_CrewUtilization] @SP_ForMonthDate={0},@SP_WarehouseId={1},@SP_LoginID={2},@SP_CompID={3},@SP_IsRmcBuss={4}",
                CSubs.QSafeValue(Convert.ToString(ForMonthDate)), CSubs.QSafeValue(Convert.ToString(WarehoseId)), CSubs.QSafeValue(Convert.ToString(LoginID)),
                CompId,IsRmc);

                DataSet Dtobj = CSubs.GetDataSet(query);

                return Dtobj;
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(UserSession.GetUserSession().LoginID), "jobAllocationDAL", "GetCrewUtilizationDashboard", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
        }
    }
}