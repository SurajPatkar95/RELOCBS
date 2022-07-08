using RELOCBS.Common.ExceptionHandling;
using RELOCBS.DAL.JobReport;
using RELOCBS.Entities;
using RELOCBS.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Dynamic;


namespace RELOCBS.BL
{
	public class JobReportBL
	{
		private JobReportDAL _jobreportDAL;

		public JobReportDAL jobreportDAL
		{

			get
			{
				if (this._jobreportDAL == null)
					this._jobreportDAL = new JobReportDAL();
				return this._jobreportDAL;
			}
		}

		public IEnumerable<Entities.PJR_DJR> GetJobReportList(DateTime? FromDate, DateTime? Todate, string sort, string sortdir, int skip, int pageSize, out int totalCount,bool IsPackStart,bool IsPackComplete, string JobNo = null,string Shipper=null,int Status=-1,Int16 JobType=1)
		{
			totalCount = 0;

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
				IQueryable<Entities.PJR_DJR> JobReportList = jobreportDAL.GetJobReportList(FromDate, Todate, IsPackStart, IsPackComplete, JobNo,Shipper, RMCBuss, Status, JobType);
				if (JobReportList != null)
				{
					totalCount = JobReportList.Count();
					
                    if (pageSize > 1)
                    {
                        JobReportList = JobReportList.Skip((skip * (pageSize - 1))).Take(skip);
                    }
                    else
                    {
                        JobReportList = JobReportList.Take(skip);
                    }

                    JobReportList = JobReportList.OrderBy(sort + " " + sortdir);

                    return JobReportList.ToList();
				}
				else
				{
					return new List<Entities.PJR_DJR>();
				}
			}
			catch (DataAccessException ex)
			{
				throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
			}
			catch (Exception ex)
			{
				throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "JobReportBL", "GetJobReportList", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
			}

		}

		public PJR_DJR GetPJR_DJR_Details(Int64 MoveID,int ComponentID, Int64 PJRDJRID = -1, Int16 JobType = 1)
		{
			PJR_DJR pJR = new PJR_DJR();

			try
			{
				DataSet data = jobreportDAL.GetPJR_DJR_Details(UserSession.GetUserSession().LoginID, MoveID, ComponentID, PJRDJRID,JobType);

				if (data != null && data.Tables.Count > 0)
				{
					if (data.Tables.Count > 0 && data.Tables[0] != null && data.Tables[0].Rows.Count > 0)
					{
						pJR = (from item in data.Tables[0].AsEnumerable()
							   select new PJR_DJR()
							   {
								   PJR_DJR_ID = !string.IsNullOrWhiteSpace(Convert.ToString(item["PJR_DJR_ID"]))? Convert.ToInt64(item["PJR_DJR_ID"]) : -1,
								   MoveID = Convert.ToInt64(item["MoveID"]),
								   JobNo = Convert.ToString(item["JobNo"]),
								   //MoveId = Convert.ToInt64(item["MoveID"]),
								   NoOfPkgs = !string.IsNullOrWhiteSpace(Convert.ToString(item["NoOfPkgs"]))? Convert.ToInt32(item["NoOfPkgs"]): (int?)null,
								   Volume = !string.IsNullOrWhiteSpace(Convert.ToString(item["Volume"])) ? Convert.ToInt64(item["Volume"]) : (float?)null,
								   CorprateName = Convert.ToString(item["CorprateName"]),
								   Shipper = Convert.ToString(item["Shipper"]),
                                   NoOfDays = !string.IsNullOrWhiteSpace(Convert.ToString(item["NoOfDays"])) ? Convert.ToInt32(item["NoOfDays"]) : (int?)null,
                                   PackStartDate = !string.IsNullOrWhiteSpace(Convert.ToString(item["Pack_StartDate"])) ? Convert.ToDateTime(item["Pack_StartDate"]) : (DateTime?)null,
                                   PackCompletionDate = !string.IsNullOrWhiteSpace(Convert.ToString(item["Pack_CompletionDate"])) ? Convert.ToDateTime(item["Pack_CompletionDate"]) : (DateTime?)null,
                                   IsCompleted = !string.IsNullOrWhiteSpace(Convert.ToString(item["IsCompleted"])) ? Convert.ToInt32(item["IsCompleted"]) : 0,
                                   Status = Convert.ToString(item["Status"]),
                                   RateComponentID = Convert.ToInt32(item["RateComponentID"]),
                                   ReportType = Convert.ToString(item["ReportType"]),
                                   JobType = JobType,
                                   Remark = Convert.ToString(item["Remark"]),
                                   CreatedDate = !string.IsNullOrWhiteSpace(Convert.ToString(item["IsCompleted"])) ? Convert.ToDateTime(item["CreatedDate"]) : System.DateTime.Now,
                               }).FirstOrDefault();

					}

					////Service Details
					if (data.Tables.Count > 1 && data.Tables[1] != null && data.Tables[1].Rows.Count > 0)
					{
						pJR.services = (from item in data.Tables[1].AsEnumerable()
										select new JobService()
										{
                                            ASC_ID = !string.IsNullOrWhiteSpace(Convert.ToString(item["ASC_ID"])) ? Convert.ToInt64(item["ASC_ID"]) : (Int64?)null,
                                            SD_ID = Convert.ToInt64(item["SD_ID"]),
											//ActivityID = Convert.ToInt64(item["ActivityID"]),
											//MoveId = Convert.ToInt64(item["MoveID"]),
											ServiceID = Convert.ToInt32(item["ServiceID"]),
											ServiceName = Convert.ToString(item["ServiceName"]),
                                            Description = Convert.ToString(item["ServiceDescription"]),
                                            Cost = Convert.ToDouble(item["Cost"])
										}).ToList();
					}

					////Crew details
					if (data.Tables.Count > 2 && data.Tables[2] != null && data.Tables[2].Rows.Count > 0)
					{
						pJR.inHouseLaboursCost = (from item in data.Tables[2].AsEnumerable()
												  select new InHouseLabour()
												  {
                                                      CC_ID = !string.IsNullOrWhiteSpace(Convert.ToString(item["CC_ID"])) ? Convert.ToInt64(item["CC_ID"]) : (Int64?)null,
                                                      C_ID = Convert.ToInt64(item["CD_ID"]),
                                                      EmpID = Convert.ToInt32(item["EmpID"]),
													  EmpName = Convert.ToString(item["EmpName"]),
													  NoOfDays = Convert.ToInt32(item["NoOfDays"]),
                                                      Rate = Convert.ToInt32(item["Rate"]),
                                                      Cost = Convert.ToInt32(item["Cost"]),
													  OT_Rate = Convert.ToInt64(item["OT_Rate"]),
													  OT_hours = Convert.ToInt32(item["OT_hours"]),
                                                      OT_Cost = Convert.ToInt64(item["OT_Cost"]),
                                                      Total = Convert.ToInt64(item["Total"]),
                                                      Fromdate = item["FromDateTime"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(item["FromDateTime"]),
                                                      Todate   = item["ToDateTime"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(item["ToDateTime"]),
                                                      OT_Remark = Convert.ToString(item["OT_Remark"]),
                                                  }).ToList();
					}

					/////Vehicles
					if (data.Tables.Count > 3 && data.Tables[3] != null && data.Tables[3].Rows.Count > 0)
					{
						pJR.Vehicles = (from item in data.Tables[3].AsEnumerable()
										select new JobVehicleCost()
										{
                                            //MoveID = Convert.ToInt64(item["MoveID"]),
                                            VC_ID = !string.IsNullOrWhiteSpace(Convert.ToString(item["VC_ID"])) ? Convert.ToInt64(item["VC_ID"]) : (Int64?)null,
                                            VD_ID = Convert.ToInt64(item["VD_ID"]),
                                            PurposeID = Convert.ToInt32(item["PurposeID"]),
											Purpose = Convert.ToString(item["PurposeName"]),
											VehicleID = string.IsNullOrWhiteSpace(Convert.ToString(item["VehicleID"])) ? (int?)null :  Convert.ToInt32(item["VehicleID"]),
											VehicleNo = Convert.ToString(item["VehicleNo"]),
											DriverID = string.IsNullOrWhiteSpace(Convert.ToString(item["DriverID"] )) ? (int?)null : Convert.ToInt32(item["DriverID"]),
											Driver = Convert.ToString(item["DriverName"]),
											VehicleType = Convert.ToString(item["VehicleType"]),
											DriverType = Convert.ToString(item["DriverType"]),
											V_Cost = !string.IsNullOrWhiteSpace(Convert.ToString(item["Cost"])) ? Convert.ToInt64(item["Cost"]) : (Int64?)null,
											FromDate = item["FromDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(item["FromDate"]),
											ToDate = item["ToDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(item["ToDate"])

										}).ToList();
					}

					////Material Cost

					if (data.Tables.Count > 4 && data.Tables[4] != null && data.Tables[4].Rows.Count > 0)
					{
						pJR.materialCosts = (from item in data.Tables[4].AsEnumerable()
											 select new MaterialCost()
											 {
                                                 MC_ID = !string.IsNullOrWhiteSpace(Convert.ToString(item["MC_ID"])) ? Convert.ToInt64(item["MC_ID"]) : (Int64?)null,
                                                 M_ID = Convert.ToInt32(item["M_ID"]),
                                                 MaterailId = Convert.ToInt32(item["MaterailID"]),
												 Materail = Convert.ToString(item["MaterialName"]),
												 IssuedQty = Convert.ToInt32(item["Issued_qty"]),
												 UsedQty = Convert.ToInt32(item["Used_qty"]),
												 ReturnQty = Convert.ToInt32(item["Return_qty"]),
												 Rate = Convert.ToInt64(item["Rate"]),
												 Cost = Convert.ToInt64(item["Cost"])
											 }).ToList();

					}

					////External Vendor Labour Cost
					if (data.Tables.Count > 5 && data.Tables[5] != null && data.Tables[5].Rows.Count > 0)
					{
                        pJR.outLabourCosts = (from item in data.Tables[5].AsEnumerable()
                                              select new OutLabourCost()
                                              {
                                                  OLC_ID = !string.IsNullOrWhiteSpace(Convert.ToString(item["OLC_ID"])) ? Convert.ToInt64(item["OLC_ID"]) : (Int64?)null,
                                                  L_ID = Convert.ToInt64(item["OLabourID"]),
                                                  Labour_VendorId = Convert.ToInt32(item["Labour_VendorId"]),
                                                  Labour_Vendor = Convert.ToString(item["Labour_Vendor"]),
                                                  Labour_Cost = Convert.ToInt64(item["Labour_Cost"]),
                                                  Labour_OutsideNo = Convert.ToInt32(item["NumberOfPerson"])
                                              }).ToList();
                    }

                    ///Documents

                    if (data.Tables.Count > 6 && data.Tables[6] != null && data.Tables[6].Rows.Count > 0)
                    {
                        pJR.docUpload.docLists = (from item in data.Tables[6].AsEnumerable()
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

                    ///Digital Inventory pdf id
                    if (data.Tables.Count > 7 && data.Tables[7] != null && data.Tables[7].Rows.Count > 0)
                    {
                        pJR.DIPDFId = data.Tables[7].Rows[0]["DIPDFId"] == DBNull.Value ? (Int64?)null : Convert.ToInt64(data.Tables[7].Rows[0]["DIPDFId"]);
                    }
                }
			}
			catch (DataAccessException ex)
			{
				throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
			}
			catch (Exception ex)
			{
				throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "JobReportBL", "GetPJR_DJR_Details", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
			}

			return pJR;
		}

        public bool InsetPJR_DJR(PJR_DJR model,string submit, out string result)
        {
            try
            {
                return jobreportDAL.InsertPJR_DJR(model, submit, UserSession.GetUserSession().LoginID ,out result);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "JobReportBL", "InsetPJR_DJR", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

        }

        public JobDiaryModel GetInstructionSheetDetails(Int64 MoveID,int RateComponentID, Int16 JobType)
        {
            JobDiaryModel InstGrid = new JobDiaryModel();
            try
            {
                InstGrid = jobreportDAL.GetInstructionSheetDetails(UserSession.GetUserSession().LoginID, MoveID, RateComponentID,JobType);
                
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "JobReportBL", "GetInstructionSheetDetails", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

           return  InstGrid;
        }

        public JobDocument GetDownloadFile(Int64 DocID, Int64? PJR_DJR_ID = -1)
        {
            JobDocument obj = new JobDocument();
            try
            {
                if (PJR_DJR_ID == null)
                {
                    PJR_DJR_ID = -1;
                }

                return jobreportDAL.GetDownloadFile(DocID, Convert.ToInt64(PJR_DJR_ID), UserSession.GetUserSession().LoginID);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "JobReportBL", "GetDownloadFile", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

            return obj;
        }

        public bool Delete_PJR_DJR_Cost(Int64 ID,Int64 PJRDJRID,string Type, out string message)
        {
            try
            {
                return jobreportDAL.Delete_PJR_DJR_Cost(ID, PJRDJRID,Type, UserSession.GetUserSession().LoginID, out message);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "JobReportBL", "Delete_PJR_DJR_Cost", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

        }

        public PJR_DJR GetPJR_DJR_Report(Int64 MoveID,int RateComponentID,Int64 PJRID,Int16 JobType)
        {
            PJR_DJR pJR = new PJR_DJR();

            try
            {
                DataSet data = jobreportDAL.GetPJR_DJR_Report(UserSession.GetUserSession().LoginID, MoveID, RateComponentID, PJRID, JobType);

                if (data != null && data.Tables.Count > 0)
                {
                    if (data.Tables.Count > 0 && data.Tables[0] != null && data.Tables[0].Rows.Count > 0)
                    {
                        pJR = (from item in data.Tables[0].AsEnumerable()
                               select new PJR_DJR()
                               {
                                   PJR_DJR_ID = !string.IsNullOrWhiteSpace(Convert.ToString(item["PJR_DJR_ID"])) ? Convert.ToInt64(item["PJR_DJR_ID"]) : -1,
                                   MoveID = Convert.ToInt64(item["MoveID"]),
                                   JobNo = Convert.ToString(item["JobNo"]),
                                   //MoveId = Convert.ToInt64(item["MoveID"]),
                                   NoOfPkgs = !string.IsNullOrWhiteSpace(Convert.ToString(item["NoOfPkgs"])) ? Convert.ToInt32(item["NoOfPkgs"]) : (int?)null,
                                   Volume = !string.IsNullOrWhiteSpace(Convert.ToString(item["Volume"])) ? Convert.ToInt64(item["Volume"]) : (float?)null,
                                   CorprateName = Convert.ToString(item["CorprateName"]),
                                   Shipper = Convert.ToString(item["Shipper"]),
                                   NoOfDays = !string.IsNullOrWhiteSpace(Convert.ToString(item["NoOfDays"])) ? Convert.ToInt32(item["NoOfDays"]) : (int?)null,
                                   PackStartDate = !string.IsNullOrWhiteSpace(Convert.ToString(item["Pack_StartDate"])) ? Convert.ToDateTime(item["Pack_StartDate"]) : (DateTime?)null,
                                   PackCompletionDate = !string.IsNullOrWhiteSpace(Convert.ToString(item["Pack_CompletionDate"])) ? Convert.ToDateTime(item["Pack_CompletionDate"]) : (DateTime?)null,
                                   IsCompleted = !string.IsNullOrWhiteSpace(Convert.ToString(item["IsCompleted"])) ? Convert.ToInt32(item["IsCompleted"]) : 0,
                                   Status = Convert.ToString(item["Status"]),
                                   RateComponentID = Convert.ToInt32(item["RateComponentID"]),
                                   ReportType = Convert.ToString(item["ReportType"]),
                                   ServiceLine = Convert.ToString(item["ServiceLine"]),
                                   Job_Date = Convert.ToDateTime(item["Job_Date"]),
                                   CreatedDate = Convert.ToDateTime(item["CreatedDate"]),
                                   BusinessLine = Convert.ToString(item["BusinessLine"]),
                                   HandlingBranch = Convert.ToString(item["HandlingBranch"]),
                                   RevenueBranch = Convert.ToString(item["RevenueBranch"]),
                                   JobType = JobType,
                                   Remark = Convert.ToString(item["Remark"]),
                               }).FirstOrDefault();

                    }

                    ////Service Details
                    if (data.Tables.Count > 1 && data.Tables[1] != null && data.Tables[1].Rows.Count > 0)
                    {
                        pJR.services = (from item in data.Tables[1].AsEnumerable()
                                        select new JobService()
                                        {
                                            SD_ID = Convert.ToInt64(item["SD_ID"]),
                                            //ActivityID = Convert.ToInt64(item["ActivityID"]),
                                            //MoveId = Convert.ToInt64(item["MoveID"]),
                                            ServiceID = Convert.ToInt32(item["ServiceID"]),
                                            ServiceName = Convert.ToString(item["ServiceName"]),
                                            Description = Convert.ToString(item["ServiceDescription"]),
                                            Cost = Convert.ToDouble(item["Cost"])
                                        }).ToList();
                    }

                    ////Crew details
                    if (data.Tables.Count > 2 && data.Tables[2] != null && data.Tables[2].Rows.Count > 0)
                    {
                        pJR.inHouseLaboursCost = (from item in data.Tables[2].AsEnumerable()
                                                  select new InHouseLabour()
                                                  {
                                                      C_ID = Convert.ToInt64(item["CD_ID"]),
                                                      EmpID = Convert.ToInt32(item["EmpID"]),
                                                      EmpName = Convert.ToString(item["EmpName"]),
                                                      NoOfDays = Convert.ToInt32(item["NoOfDays"]),
                                                      Rate = Convert.ToInt32(item["Rate"]),
                                                      Cost = Convert.ToInt32(item["Cost"]),
                                                      OT_Rate = Convert.ToInt64(item["OT_Rate"]),
                                                      OT_hours = Convert.ToInt32(item["OT_hours"]),
                                                      OT_Cost = Convert.ToInt64(item["OT_Cost"]),
                                                      Total = Convert.ToInt64(item["Total"]),
                                                      OT_Remark = Convert.ToString(item["OT_Remark"]),
                                                  }).ToList();
                    }

                    /////Vehicles
                    if (data.Tables.Count > 3 && data.Tables[3] != null && data.Tables[3].Rows.Count > 0)
                    {
                        pJR.Vehicles = (from item in data.Tables[3].AsEnumerable()
                                        select new JobVehicleCost()
                                        {
                                            //MoveID = Convert.ToInt64(item["MoveID"]),
                                            VD_ID = Convert.ToInt64(item["VD_ID"]),
                                            PurposeID = Convert.ToInt32(item["PurposeID"]),
                                            Purpose = Convert.ToString(item["PurposeName"]),
                                            VehicleID = Convert.ToInt32(item["VehicleID"]),
                                            VehicleNo = Convert.ToString(item["VehicleNo"]),
                                            DriverID = Convert.ToInt32(item["DriverID"]),
                                            Driver = Convert.ToString(item["DriverName"]),
                                            VehicleType = Convert.ToString(item["VehicleType"]),
                                            DriverType = Convert.ToString(item["DriverType"]),
                                            V_Cost = Convert.ToInt64(item["Cost"]),
                                            FromDate = item["FromDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(item["FromDate"]),
                                            ToDate = item["ToDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(item["ToDate"])

                                        }).ToList();
                    }

                    ////Material Cost

                    if (data.Tables.Count > 4 && data.Tables[4] != null && data.Tables[4].Rows.Count > 0)
                    {
                        pJR.materialCosts = (from item in data.Tables[4].AsEnumerable()
                                             select new MaterialCost()
                                             {
                                                 M_ID = Convert.ToInt32(item["M_ID"]),
                                                 MaterailId = Convert.ToInt32(item["MaterailID"]),
                                                 Materail = Convert.ToString(item["MaterialName"]),
                                                 IssuedQty = Convert.ToInt32(item["Issued_qty"]),
                                                 UsedQty = Convert.ToInt32(item["Used_qty"]),
                                                 ReturnQty = Convert.ToInt32(item["Return_qty"]),
                                                 Rate = Convert.ToInt64(item["Rate"]),
                                                 Cost = Convert.ToInt64(item["Cost"])
                                             }).ToList();

                    }

                    ////External Vendor Labour Cost
                    if (data.Tables.Count > 5 && data.Tables[5] != null && data.Tables[5].Rows.Count > 0)
                    {
                        pJR.outLabourCosts = (from item in data.Tables[5].AsEnumerable()
                                              select new OutLabourCost()
                                              {
                                                  L_ID = Convert.ToInt64(data.Tables[5].Rows[0]["OLabourID"]),
                                                  Labour_VendorId = Convert.ToInt32(data.Tables[5].Rows[0]["Labour_VendorId"]),
                                                  Labour_Vendor = Convert.ToString(data.Tables[5].Rows[0]["Labour_Vendor"]),
                                                  Labour_Cost = Convert.ToInt64(data.Tables[5].Rows[0]["Labour_Cost"]),
                                                  Labour_OutsideNo = Convert.ToInt32(data.Tables[5].Rows[0]["NumberOfPerson"])
                                              }).ToList();
                    }

                    ///Documents

                    if (data.Tables.Count > 6 && data.Tables[6] != null && data.Tables[6].Rows.Count > 0)
                    {
                        pJR.docUpload.docLists = (from item in data.Tables[6].AsEnumerable()
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

                }
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "JobReportBL", "GetPJR_DJR_Report", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

            return pJR;

        }

		public PJR_DJR GetPJR_DJR_ReportForMove(Int64 MoveID, int RateComponentID, Int64 PJRID, out string msg)
		{
			PJR_DJR pJR = new PJR_DJR();
			msg = "";
			try
			{
				DataSet data = jobreportDAL.GetPJR_DJR_ReportForMove(UserSession.GetUserSession().LoginID, MoveID, RateComponentID, PJRID, out msg);

				if (data != null && data.Tables.Count > 0)
				{
					if (data.Tables.Count > 0 && data.Tables[0] != null && data.Tables[0].Rows.Count > 0)
					{
						pJR = (from item in data.Tables[0].AsEnumerable()
							   select new PJR_DJR()
							   {
								   PJR_DJR_ID = !string.IsNullOrWhiteSpace(Convert.ToString(item["PJR_DJR_ID"])) ? Convert.ToInt64(item["PJR_DJR_ID"]) : -1,
								   MoveID = Convert.ToInt64(item["MoveID"]),
								   JobNo = Convert.ToString(item["JobNo"]),
								   //MoveId = Convert.ToInt64(item["MoveID"]),
								   NoOfPkgs = !string.IsNullOrWhiteSpace(Convert.ToString(item["NoOfPkgs"])) ? Convert.ToInt32(item["NoOfPkgs"]) : (int?)null,
								   Volume = !string.IsNullOrWhiteSpace(Convert.ToString(item["Volume"])) ? Convert.ToInt64(item["Volume"]) : (float?)null,
								   CorprateName = Convert.ToString(item["CorprateName"]),
								   Shipper = Convert.ToString(item["Shipper"]),
								   NoOfDays = !string.IsNullOrWhiteSpace(Convert.ToString(item["NoOfDays"])) ? Convert.ToInt32(item["NoOfDays"]) : (int?)null,
								   PackStartDate = !string.IsNullOrWhiteSpace(Convert.ToString(item["Pack_StartDate"])) ? Convert.ToDateTime(item["Pack_StartDate"]) : (DateTime?)null,
								   PackCompletionDate = !string.IsNullOrWhiteSpace(Convert.ToString(item["Pack_CompletionDate"])) ? Convert.ToDateTime(item["Pack_CompletionDate"]) : (DateTime?)null,
								   IsCompleted = !string.IsNullOrWhiteSpace(Convert.ToString(item["IsCompleted"])) ? Convert.ToInt32(item["IsCompleted"]) : 0,
								   Status = Convert.ToString(item["Status"]),
								   RateComponentID = Convert.ToInt32(item["RateComponentID"]),
								   ReportType = Convert.ToString(item["ReportType"]),
								   ServiceLine = Convert.ToString(item["ServiceLine"]),
								   Job_Date = Convert.ToDateTime(item["Job_Date"]),
								   CreatedDate = Convert.ToDateTime(item["CreatedDate"]),
                                   Remark = Convert.ToString(item["Remark"]),
                               }).FirstOrDefault();

					}


					////Service Details
					if (data.Tables.Count > 1 && data.Tables[1] != null && data.Tables[1].Rows.Count > 0)
					{
						pJR.services = (from item in data.Tables[1].AsEnumerable()
										select new JobService()
										{
											SD_ID = Convert.ToInt64(item["SD_ID"]),
											//ActivityID = Convert.ToInt64(item["ActivityID"]),
											//MoveId = Convert.ToInt64(item["MoveID"]),
											ServiceID = Convert.ToInt32(item["ServiceID"]),
											ServiceName = Convert.ToString(item["ServiceName"]),
											Description = Convert.ToString(item["ServiceDescription"]),
											Cost = Convert.ToDouble(item["Cost"])
										}).ToList();
					}

					////Crew details
					if (data.Tables.Count > 2 && data.Tables[2] != null && data.Tables[2].Rows.Count > 0)
					{
						pJR.inHouseLaboursCost = (from item in data.Tables[2].AsEnumerable()
												  select new InHouseLabour()
												  {
													  C_ID = Convert.ToInt64(item["CD_ID"]),
													  EmpID = Convert.ToInt32(item["EmpID"]),
													  EmpName = Convert.ToString(item["EmpName"]),
													  NoOfDays = Convert.ToInt32(item["NoOfDays"]),
													  Rate = Convert.ToInt32(item["Rate"]),
													  Cost = Convert.ToInt32(item["Cost"]),
													  OT_Rate = Convert.ToInt64(item["OT_Rate"]),
													  OT_hours = Convert.ToInt32(item["OT_hours"]),
													  OT_Cost = Convert.ToInt64(item["OT_Cost"]),
													  Total = Convert.ToInt64(item["Total"]),
													  OT_Remark = Convert.ToString(item["OT_Remark"]),
												  }).ToList();
					}

					/////Vehicles
					if (data.Tables.Count > 3 && data.Tables[3] != null && data.Tables[3].Rows.Count > 0)
					{
						pJR.Vehicles = (from item in data.Tables[3].AsEnumerable()
										select new JobVehicleCost()
										{
											//MoveID = Convert.ToInt64(item["MoveID"]),
											VD_ID = Convert.ToInt64(item["VD_ID"]),
											PurposeID = Convert.ToInt32(item["PurposeID"]),
											Purpose = Convert.ToString(item["PurposeName"]),
											VehicleID = Convert.ToInt32(item["VehicleID"]),
											VehicleNo = Convert.ToString(item["VehicleNo"]),
											DriverID = Convert.ToInt32(item["DriverID"]),
											Driver = Convert.ToString(item["DriverName"]),
											VehicleType = Convert.ToString(item["VehicleType"]),
											DriverType = Convert.ToString(item["DriverType"]),
											V_Cost = Convert.ToInt64(item["Cost"]),
											FromDate = item["FromDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(item["FromDate"]),
											ToDate = item["ToDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(item["ToDate"])

										}).ToList();
					}

					////Material Cost

					if (data.Tables.Count > 4 && data.Tables[4] != null && data.Tables[4].Rows.Count > 0)
					{
						pJR.materialCosts = (from item in data.Tables[4].AsEnumerable()
											 select new MaterialCost()
											 {
												 M_ID = Convert.ToInt32(item["M_ID"]),
												 MaterailId = Convert.ToInt32(item["MaterailID"]),
												 Materail = Convert.ToString(item["MaterialName"]),
												 IssuedQty = Convert.ToInt32(item["Issued_qty"]),
												 UsedQty = Convert.ToInt32(item["Used_qty"]),
												 ReturnQty = Convert.ToInt32(item["Return_qty"]),
												 Rate = Convert.ToInt64(item["Rate"]),
												 Cost = Convert.ToInt64(item["Cost"])
											 }).ToList();

					}

					////External Vendor Labour Cost
					if (data.Tables.Count > 5 && data.Tables[5] != null && data.Tables[5].Rows.Count > 0)
					{
						pJR.outLabourCosts = (from item in data.Tables[5].AsEnumerable()
											  select new OutLabourCost()
											  {
												  L_ID = Convert.ToInt64(data.Tables[5].Rows[0]["OLabourID"]),
												  Labour_VendorId = Convert.ToInt32(data.Tables[5].Rows[0]["Labour_VendorId"]),
												  Labour_Vendor = Convert.ToString(data.Tables[5].Rows[0]["Labour_Vendor"]),
												  Labour_Cost = Convert.ToInt64(data.Tables[5].Rows[0]["Labour_Cost"]),
												  Labour_OutsideNo = Convert.ToInt32(data.Tables[5].Rows[0]["NumberOfPerson"])
											  }).ToList();
					}

					///Documents

					if (data.Tables.Count > 6 && data.Tables[6] != null && data.Tables[6].Rows.Count > 0)
					{
						pJR.docUpload.docLists = (from item in data.Tables[6].AsEnumerable()
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

				}
			}
			catch (DataAccessException ex)
			{
				throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
			}
			catch (Exception ex)
			{
				throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "JobReportBL", "GetPJR_DJR_ReportForMove", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
			}

			return pJR;

		}

        public string GetDigitalInventoryPDF( Int64 ID)
        {
            string result=string.Empty;
            try
            {

                DataTable pdfDt = jobreportDAL.GetDigitalInventoryPDF(UserSession.GetUserSession().LoginID, ID);
                if (pdfDt!=null && pdfDt.Rows.Count>0)
                {
                    result = pdfDt.Rows[0]["FilePath"].ToString();
                }
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "JobReportBL", "GetDigitalInventoryPDF", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

            return result;

        }
    }
}