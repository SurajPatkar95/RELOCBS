using RELOCBS.App_Code;
using RELOCBS.Common.ExceptionHandling;
using RELOCBS.Entities;
using RELOCBS.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml.Linq;

namespace RELOCBS.DAL.JobReport
{
	public class JobReportDAL
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
		public IQueryable<Entities.PJR_DJR> GetJobReportList(DateTime? FromDate, DateTime? Todate, bool IsPackStart, bool IsPackComplete, string JobNo,string Shipper,bool? RMCBuss = false,int Status=-1,Int16 JobType=1)
		{
			int LoggedinUserID = UserSession.GetUserSession().LoginID;
			try
			{

				IQueryable<Entities.PJR_DJR> data = null;

				using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
				{
					if (conn.Connect())
					{
                        string SPName = JobType == 0 ? "[Warehouse].[GetWHJobReportForGrid]" : "[Warehouse].[GetJobReportForGrid]";
                        conn.AddCommand(SPName, QueryType.Procedure);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IsPack", SqlDbType.Bit, 1, ParameterDirection.Input, IsPackStart);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IsComplete", SqlDbType.Bit, 1, ParameterDirection.Input, IsPackComplete);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_FromDate", SqlDbType.DateTime, 0, ParameterDirection.Input, FromDate);
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ToDate", SqlDbType.DateTime, 0, ParameterDirection.Input, Todate);
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LoginID", SqlDbType.Int, 0, ParameterDirection.Input, UserSession.GetUserSession().LoginID);
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_JobNo", SqlDbType.Int, 0, ParameterDirection.Input, CSubs.PSafeValue(JobNo));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Shipper",SqlDbType.VarChar, 50, ParameterDirection.Input, CSubs.PSafeValue(Shipper));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CompId", SqlDbType.Int, 0, ParameterDirection.Input, UserSession.GetUserSession().CompanyID);
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IsRMCBuss", SqlDbType.Bit, 1, ParameterDirection.Input, RMCBuss);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Status", SqlDbType.Int, 0, ParameterDirection.Input, Status);
                        
                        DataTable dt = (DataTable)conn.ExecuteProcedure(ProcedureReturnType.DataTable);

                        if (dt != null)
                        {
                            var result = (from rw in dt.AsEnumerable()
                                          select new Entities.PJR_DJR()
                                          {
                                              MoveID = Convert.ToInt64(rw["MoveID"]),
                                              JobNo = Convert.ToString(rw["JobNo"]),
                                              PJR_DJR_ID = rw["PJR_DJR_ID"] == DBNull.Value ? (Int64?)null : Convert.ToInt64(rw["PJR_DJR_ID"]),
                                              RateComponentID = rw["RateComponentID"] == DBNull.Value ? (Int32?)null : Convert.ToInt32(rw["RateComponentID"]),
                                              RateComponentName = Convert.ToString(rw["RateComponentName"]),
                                              CreatedDate = Convert.ToDateTime(rw["CreatedDate"]),
                                              CorprateName = Convert.ToString(rw["CorprateName"]),
                                              Shipper = Convert.ToString(rw["Shipper"]),
                                              PackStartDate = rw["Pack_StartDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(rw["Pack_StartDate"]),
                                              PackCompletionDate = rw["Pack_CompletionDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(rw["Pack_CompletionDate"]),
                                              NoOfDays = rw["NoOfDays"] == DBNull.Value ? (int?)null : Convert.ToInt32(rw["NoOfDays"]),
                                              ReportType = Convert.ToString(rw["ReportType"]),
                                              Status = Convert.ToString(rw["Status"]),
                                              HandlingBranch = Convert.ToString(rw["HandlingBranch"]),
                                              RevenueBranch = Convert.ToString(rw["RevenueBranch"]),
                                              BusinessLine = Convert.ToString(rw["BusinessLine"]),
                                              JobType=JobType
                                          }).ToList();
                            data = result.AsQueryable<Entities.PJR_DJR>();
                        }
                    }
					else
						throw new Exception(conn.ErrorMessage);
				}
				return data;
			}
			catch (Exception ex)
			{
				throw new DataAccessException(Convert.ToString(LoggedinUserID), "JobReportDAL", "GetJobReportList", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
			}

			//return null;


		}

		public DataSet GetPJR_DJR_Details(int LoginID, Int64 MoveID,int ComponentID, Int64 PJRDJRID = -1, Int16 JobType=1)
		{
			DataSet Dtobj = new DataSet();

			try
			{
				string query = string.Format("[Warehouse].[GetJobPJR_DJRForDisplay] @SP_MoveID={0},@SP_RateComponentID={1},@SP_PJRDJRID={2},@SP_LoginID={3},@SP_JobType={4}",
				CSubs.QSafeValue(Convert.ToString(MoveID)),
                CSubs.QSafeValue(Convert.ToString(ComponentID)),
                CSubs.QSafeValue(Convert.ToString(PJRDJRID)),
                CSubs.QSafeValue(Convert.ToString(LoginID)),
                CSubs.QSafeValue(Convert.ToString(JobType))
                );

				Dtobj = CSubs.GetDataSet(query);

			}
			catch (Exception ex)
			{
				throw new DataAccessException(Convert.ToString(LoginID), "JobReportDAL", "GetPJR_DJR_Details", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
			}

			return Dtobj;
		}

        public bool InsertPJR_DJR(PJR_DJR model,string submit, int LoginID,out string result)
        {
            try
            {
                using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {

                        string CrewMembersXml = string.Empty;
                        string VehicleXml = string.Empty;
                        string ServiceXml = string.Empty;
                        string MaterialXml = string.Empty;
                        string OutLabourXml = string.Empty;
                        string DocXml = string.Empty;
                        bool IsCompleted = submit.ToUpper() == "COMPLETED" ? true : false;
                        if (model.services.Count > 0)
                        {
                            ServiceXml = new XElement("Services", from emp in model.services
                                                                  select new XElement("Service",
                                                         new XElement("ID", emp.SD_ID),
                                                         new XElement("ServiceID", emp.ServiceID),
                                                         new XElement("FromDate", emp.FromDate),
                                                         new XElement("ToDate", emp.ToDate),
                                                         new XElement("Cost", emp.Cost)

                                                     )).ToString();

                            if (!string.IsNullOrWhiteSpace(ServiceXml))
                            {
                                ServiceXml = Regex.Replace(ServiceXml, @"\t|\n|\r", ""); //InstIDxml.Replace("\r\n", "");
                            }
                        }

                        if (model.inHouseLaboursCost.Count > 0)
                        {
                            CrewMembersXml = new XElement("Crews", from emp in model.inHouseLaboursCost
                                                                   select new XElement("Crew",
                                                          new XElement("ID", emp.C_ID),
                                                          new XElement("EmpID", emp.EmpID),
                                                          new XElement("NoOfDays", emp.NoOfDays),
                                                          new XElement("Rate", emp.Rate),
                                                          new XElement("OT_hours", emp.OT_hours),
                                                          new XElement("OT_Rate", emp.OT_Rate),
                                                          new XElement("OT_Remark",emp.OT_Remark)

                                                      )).ToString();
                            if (!string.IsNullOrWhiteSpace(CrewMembersXml))
                            {
                                CrewMembersXml = Regex.Replace(CrewMembersXml, @"\t|\n|\r", ""); //InstIDxml.Replace("\r\n", "");
                            }
                        }

                        if (model.Vehicles.Count > 0)
                        {
                            VehicleXml = new XElement("Vehicles", from emp in model.Vehicles
                                                                  select new XElement("Vehicle",
                                                         new XElement("ID", emp.VD_ID),
                                                         new XElement("PurposeID", emp.PurposeID),
                                                         new XElement("VehicleID", emp.VehicleID),
                                                         new XElement("DriverID", emp.DriverID),
                                                         new XElement("VehicleNo", emp.VehicleNo),
                                                         new XElement("Driver", emp.Driver),
                                                         new XElement("VehicleType", emp.VehicleTypeID),
                                                         new XElement("DriverType", emp.DriverTypeID),
                                                         new XElement("FromDate", emp.FromDate),
                                                         new XElement("ToDate", emp.ToDate),
                                                         new XElement("Cost", emp.V_Cost)

                                                     )).ToString();

                            if (!string.IsNullOrWhiteSpace(VehicleXml))
                            {
                                VehicleXml = Regex.Replace(VehicleXml, @"\t|\n|\r", ""); //InstIDxml.Replace("\r\n", "");
                            }
                        }
                        
                        if (model.materialCosts.Count > 0)
                        {
                            MaterialXml = new XElement("Materials", from emp in model.materialCosts
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

                        if (model.outLabourCosts.Count > 0)
                        {
                            OutLabourXml = new XElement("OutLabours", from emp in model.outLabourCosts
                                                                      select new XElement("outsideLabour",
                                                            new XElement("ID", emp.L_ID),
                                                            new XElement("VendorID", emp.Labour_VendorId),
                                                            new XElement("NoOfPerson", emp.Labour_OutsideNo),
                                                            new XElement("Cost", emp.Labour_Cost)
                                                        )).ToString();

                            if (!string.IsNullOrWhiteSpace(OutLabourXml))
                            {
                                OutLabourXml = Regex.Replace(OutLabourXml, @"\t|\n|\r", ""); //InstIDxml.Replace("\r\n", "");
                            }
                        }

                        ////Document
                        //if (model.docUpload.docLists != null && model.docUpload.docLists.Count > 0)
                        //{
                        //    DocXml = new XElement("Downloads", from emp in model.docUpload.docLists
                        //                                       select new XElement("Download",
                        //                              new XElement("DocID", emp.DocID),
                        //                              new XElement("DocTypeID", emp.DocTypeID)
                        //                          )).ToString();
                        //}

                        //if (!string.IsNullOrWhiteSpace(DocXml))
                        //{
                        //    DocXml = Regex.Replace(DocXml, @"\t|\n|\r", ""); //InstIDxml.Replace("\r\n", "");
                        //}

                        conn.AddCommand("[Warehouse].[AddEdit_PJR_DJR]", QueryType.Procedure);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_PRJID", SqlDbType.BigInt, 0, ParameterDirection.InputOutput, model.PJR_DJR_ID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_MoveID", SqlDbType.BigInt, 0, ParameterDirection.Input, model.MoveID);
                        //conn.AddParameter(ParameterOF.PROCEDURE, "@SP_PRJID", SqlDbType.BigInt, 0, ParameterDirection.InputOutput, model.PJR_DJR_ID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_RateComponentID", SqlDbType.Int, 0, ParameterDirection.Input, model.RateComponentID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Volume", SqlDbType.Float, 0, ParameterDirection.Input, model.Volume);

                        conn.AddParameter(ParameterOF.PROCEDURE, "@sp_PackStartDate", SqlDbType.DateTime, 0, ParameterDirection.Input, model.PackStartDate);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@sp_NoOfDays", SqlDbType.Int, 0, ParameterDirection.Input, model.NoOfDays);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@sp_PackCompleteDate", SqlDbType.DateTime, 0, ParameterDirection.Input, model.PackCompletionDate);

                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_NoOfPackages", SqlDbType.Int, 0, ParameterDirection.Input, model.NoOfPkgs);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CrewXML", SqlDbType.Xml, -1, ParameterDirection.Input, CrewMembersXml);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_AdditionalServiceXML", SqlDbType.Xml, -1, ParameterDirection.Input, ServiceXml);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_VehiclesXML", SqlDbType.Xml, -1, ParameterDirection.Input, VehicleXml);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_MaterialXml", SqlDbType.Xml, -1, ParameterDirection.Input, MaterialXml);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_OutLabourXml", SqlDbType.Xml, -1, ParameterDirection.Input, OutLabourXml);

                        //conn.AddParameter(ParameterOF.PROCEDURE, "@SP_DocXml", SqlDbType.Xml, -1, ParameterDirection.Input, DocXml);
                        //if (model.docUpload.file != null)
                        //{
                        //    conn.AddParameter(ParameterOF.PROCEDURE, "@SP_DocTypeID", SqlDbType.Int, 0, ParameterDirection.Input, model.docUpload.DocTypeID);
                        //    conn.AddParameter(ParameterOF.PROCEDURE, "@SP_DocID", SqlDbType.Int, 0, ParameterDirection.Output);
                        //    string FilePath = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["JobActityFile"]);
                        //    conn.AddParameter(ParameterOF.PROCEDURE, "@SP_FilePath", SqlDbType.NVarChar, 500, ParameterDirection.InputOutput, CSubs.PSafeValue(FilePath));
                        //    conn.AddParameter(ParameterOF.PROCEDURE, "@SP_DocName", SqlDbType.NVarChar, 500, ParameterDirection.Input, CSubs.PSafeValue(model.docUpload.file.FileName));
                        //    conn.AddParameter(ParameterOF.PROCEDURE, "@SP_EXT", SqlDbType.NVarChar, 10, ParameterDirection.Input, CSubs.PSafeValue(Path.GetExtension(model.docUpload.file.FileName)));
                        //}
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IsCompleted", SqlDbType.Bit, 0, ParameterDirection.Input, IsCompleted);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LoginID", SqlDbType.Int, 0, ParameterDirection.Input, UserSession.GetUserSession().LoginID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Remark", SqlDbType.NVarChar, 500, ParameterDirection.Input, CSubs.PSafeValue(model.Remark));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@RETURNSTATUS", SqlDbType.SmallInt, 0, ParameterDirection.ReturnValue);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_MESSAGE", SqlDbType.NVarChar, -1, ParameterDirection.Output);
                        conn.ExecuteProcedure(ProcedureReturnType.SingleValue);

                        if (!conn.IsError)
                        {
                            int ReturnStatus = Convert.ToInt32(conn.GetParameterValue(ParameterOF.PROCEDURE, "@RETURNSTATUS"));
                            result = Convert.ToString(conn.GetParameterValue(ParameterOF.PROCEDURE, "@SP_MESSAGE"));

                            if (ReturnStatus == 0)
                            {
                                model.PJR_DJR_ID = Convert.ToInt64(conn.GetParameterValue(ParameterOF.PROCEDURE, "@SP_PRJID"));
                                //if (model.docUpload.file != null)
                                //{
                                //    string File = Convert.ToString(conn.GetParameterValue(ParameterOF.PROCEDURE, "@SP_FilePath"));
                                //    string DocID = Convert.ToString(conn.GetParameterValue(ParameterOF.PROCEDURE, "@SP_DocID"));
                                //    if (!string.IsNullOrWhiteSpace(File) && !string.IsNullOrWhiteSpace(DocID))
                                //    {
                                //        model.docUpload.file.SaveAs(File);
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
                throw new DataAccessException(Convert.ToString(LoginID), "JobReportDAL", "GetPJR_DJR_Details", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }


        }

        public JobDiaryModel GetInstructionSheetDetails(int LoginID,Int64 MoveID,int RateComponentID, Int16 JobType)
        {
            JobDiaryModel jobDiary = new JobDiaryModel();
            try
            {
                string query = string.Format("exec [Warehouse].[GetJobReportInstructionSheetGrid] @SP_LoginID={0},@SP_MoveID={1},@SP_RateComponentID={2},@SP_JobType={3}"
                    ,CSubs.QSafeValue(Convert.ToString(LoginID))
                    ,CSubs.QSafeValue(Convert.ToString(MoveID))
                    ,CSubs.QSafeValue(Convert.ToString(RateComponentID))
                    ,CSubs.QSafeValue(Convert.ToString(JobType)));
                DataSet dataSet = CSubs.GetDataSet(query);

                if (dataSet!=null && dataSet.Tables.Count>0)
                {
                    var result = (from rw in dataSet.Tables[0].AsEnumerable()
                                  select new InstructionSheetGrid()
                                  {
                                      MoveID = Convert.ToInt64(rw["MoveID"]),
                                      InstID = Convert.ToInt64(rw["InstID"]),
                                      InstDate = Convert.ToDateTime(rw["InstructionDate"]),
                                      SpecialInstructions = Convert.ToString(rw["Special_Instruction"]),
                                      BranchName = Convert.ToString(rw["BranchName"]),
                                      WarehouseName = Convert.ToString(rw["Warehoue_Name"]),
                                      InstType = Convert.ToString(rw["RateComponentName"]),
                                      Inst_Status = Convert.ToString(rw["Status"]),
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
                                                 ActivityTypeName = Convert.ToString(rw["ActivityTypeName"]),
                                                 FromDate = Convert.ToDateTime(rw["FromDate"]),
                                                 ToDate = Convert.ToDateTime(rw["ToDate"]),
                                                 RepTime = string.IsNullOrWhiteSpace(Convert.ToString(rw["Rep_Time"])) ?  (TimeSpan?)null : (TimeSpan)(rw["Rep_Time"]),
                                                 FromLocation = Convert.ToString(rw["From_Loc"]),
                                                 ToLocation = Convert.ToString(rw["To_Loc"]),
                                                 ACT_Status = Convert.ToString(rw["ACT_Status"]),
                                                 ACT_StatusID = string.IsNullOrWhiteSpace(Convert.ToString(rw["Act_StatusID"])) ? (Int32?)(null) : Convert.ToInt32(rw["Act_StatusID"]),
                                                 ACT_BatchID = string.IsNullOrWhiteSpace(Convert.ToString(rw["JA_BatchID"])) ? (Int64?)(null) : Convert.ToInt64(rw["JA_BatchID"]),
                                                 Status = Convert.ToString(rw["JA_BatchID"]),
                                                 NumberOfDays = string.IsNullOrWhiteSpace(Convert.ToString(rw["NoOfDays"])) ? (int?)(null) : Convert.ToInt32(rw["NoOfDays"]),
                                                 JobType = JobType
                                             }).ToList();

                    }

                    jobDiary.instructionSheetGrids = result.AsEnumerable<InstructionSheetGrid>();

                }


                return jobDiary;

            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoginID), "JobReportDAL", "GetInstructionSheetDetails", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
        }

        public JobDocument GetDownloadFile(Int64 DocID, Int64 PJR_DJR_ID, int LoginID)
        {
            JobDocument job = new JobDocument();
            try
            {
                DataTable dt = CSubs.GetDataTable(string.Format("exec [Warehouse].[GetPJR_DJRDocumentDetail] @SP_DocID={0},@SP_PJR_DJR_ID={1},@SP_LoginID={2}",
                    CSubs.QSafeValue(Convert.ToString(DocID))
                    , CSubs.QSafeValue(Convert.ToString(PJR_DJR_ID))
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
                throw new DataAccessException(Convert.ToString(LoginID), "JobReportDAL", "GetDownloadFile", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

            return job;

        }

        public bool Delete_PJR_DJR_Cost(Int64 ID,Int64 PJRDJRID,string Type, int LoginID, out string message)
        {
            message = string.Empty;
            try
            {
                using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {
                        conn.AddCommand("[Warehouse].[DeletePJR_DJR_AllocationCost]", QueryType.Procedure);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ID", SqlDbType.BigInt, 0, ParameterDirection.Input, (ID));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_PJRID", SqlDbType.BigInt, 0, ParameterDirection.Input, (PJRDJRID));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_TYPE", SqlDbType.NVarChar, 50, ParameterDirection.Input, CSubs.PSafeValue(Type));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LOGINID", SqlDbType.Int, 0, ParameterDirection.Input, UserSession.GetUserSession().LoginID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@RETURNSTATUS", SqlDbType.SmallInt, 0, ParameterDirection.ReturnValue);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_MESSAGE", SqlDbType.NVarChar, 500, ParameterDirection.Output);
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
                throw new DataAccessException(Convert.ToString(LoginID), "JobReportDAL", "Delete_PJR_DJR_Cost", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

        }

        public DataSet GetPJR_DJR_Report(int LoginID, Int64 MoveID,int RateComponentID,Int64 PJRID,Int16 JobType)
        {
            DataSet Dtobj = new DataSet();

            try
            {
                string query = string.Format("[Warehouse].[GetJobPJR_DJRForReport]  @SP_MoveID={0},@SP_RateComponentID={1},@SP_PJRDJRID={2},@SP_LoginID={3},@SP_JobType={4}",
                CSubs.QSafeValue(Convert.ToString(MoveID)),
                CSubs.QSafeValue(Convert.ToString(RateComponentID)),
                CSubs.QSafeValue(Convert.ToString(PJRID)),
                CSubs.QSafeValue(Convert.ToString(LoginID)),
                CSubs.QSafeValue(Convert.ToString(JobType))
                );

                Dtobj = CSubs.GetDataSet(query);

            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoginID), "JobReportDAL", "GetPJR_DJR_Report", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

            return Dtobj;

        }

		public DataSet GetPJR_DJR_ReportForMove(int LoginID, Int64 MoveID, int RateComponentID, Int64 PJRID, out string msg)
		{
			DataSet Dtobj = new DataSet();
			msg = "";
			try
			{
				using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
				{
					if (conn.Connect())
					{
						conn.AddCommand("[Warehouse].[GetJobPJR_DJRForReport_ForMove]", QueryType.Procedure);
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_MoveID", SqlDbType.BigInt, 0, ParameterDirection.Input, MoveID);
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_RateComponentID", SqlDbType.Int, 0, ParameterDirection.Input, RateComponentID);
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LoginID", SqlDbType.Int, 0, ParameterDirection.Input, LoginID);
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_OutMessage", SqlDbType.NVarChar, -1, ParameterDirection.Output);

						Dtobj = (DataSet)conn.ExecuteProcedure(ProcedureReturnType.DataSet);

						
						msg = Convert.ToString(conn.GetParameterValue(ParameterOF.PROCEDURE, "@SP_OutMessage"));	
						
					}
					else
						throw new Exception(conn.ErrorMessage);
				}

			}
			catch (Exception ex)
			{
				throw new DataAccessException(Convert.ToString(LoginID), "JobReportDAL", "GetPJR_DJR_ReportForMove", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
			}
			return Dtobj;


		}

        public DataTable GetDigitalInventoryPDF (int LoginID,Int64 ID)
        {
            try
            {
                string query = string.Format("[Warehouse].[GetDigitalInventoryPDF]  @SP_ID={0},@SP_LoginID={1}", ID, LoginID);

                return CSubs.GetDataTable(query);
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoginID), "JobReportDAL", "GetDigitalInventoryPDF", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

        }
    }
}