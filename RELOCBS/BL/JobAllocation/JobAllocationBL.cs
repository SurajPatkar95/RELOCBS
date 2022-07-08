using RELOCBS.Common.ExceptionHandling;
using RELOCBS.DAL.JobAllocation;
using RELOCBS.Entities;
using RELOCBS.Extensions;
using RELOCBS.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace RELOCBS.BL.JobAllocation
{
    public class JobAllocationBL
    {

        private JobAllocationDAL _jobAllocationDAL;

        public JobAllocationDAL jobAllocationDAL
        {

            get
            {
                if (this._jobAllocationDAL == null)
                    this._jobAllocationDAL = new JobAllocationDAL();
                return this._jobAllocationDAL;
            }
        }

        public bool Insert(Entities.JobAllocationModel jobAllocation, out string result)
        {
            try
            {
                return jobAllocationDAL.Insert(jobAllocation, out result);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "JobAllocationBL", "Insert", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

        }

        public IEnumerable<JobInstGrid> GetForGrid(Int64[] JobNo,string Shipper, int AllocationStatus, DateTime? FromDate, DateTime? Todate,int WarehouseID,int JobType, string sort, string sortdir, int skip, int pageSize, out int totalCount)
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

                IQueryable<Entities.JobInstGrid> AllocationList = jobAllocationDAL.GetForGrid(UserSession.GetUserSession().LoginID, UserSession.GetUserSession().CompanyID, JobNo, Shipper, AllocationStatus, FromDate, Todate,WarehouseID, RMCBuss, JobType);

                totalCount = AllocationList.Count();
                if (pageSize > 1)
                {
                    AllocationList = AllocationList.Skip((skip * (pageSize - 1))).Take(skip);
                }
                else
                {
                    AllocationList = AllocationList.Take(skip);
                }

                //AllocationList = AllocationList.OrderBy(sort + " " + sortdir);

                return AllocationList.ToList();
                
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "JobAllocationBL", "GetForGrid", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }


        }

        public JobAllocationModel GetDetailById(Int64 MoveId, Int64 InstID)
        {

            JobAllocationModel JobAObj = new JobAllocationModel();
            try
            {
                JobAObj.MoveID = InstID;
                DataSet CostDs = jobAllocationDAL.GetDetailById(UserSession.GetUserSession().LoginID, MoveId, InstID);

                if (CostDs != null && CostDs.Tables.Count > 1)
                {

                    if (CostDs.Tables.Count > 0 && CostDs.Tables[0] != null && CostDs.Tables[0].Rows.Count > 0)
                    {

                        JobAObj.MoveID = Convert.ToInt64(CostDs.Tables[0].Rows[0]["MoveID"]);
                        JobAObj.InstID = CostDs.Tables[0].Rows[0]["InstID"] == DBNull.Value ? (Int64?)null : Convert.ToInt64(CostDs.Tables[0].Rows[0]["InstID"]);
                        JobAObj.JobNo = Convert.ToString(CostDs.Tables[0].Rows[0]["JobNo"]);
                        JobAObj.JobOpenDate = Convert.ToDateTime(CostDs.Tables[0].Rows[0]["JobOpenedDate"]);

                        JobAObj.ExpBeginDate = CostDs.Tables[0].Rows[0]["ExpBeginDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(CostDs.Tables[0].Rows[0]["ExpBeginDate"]);
                        JobAObj.ExpCompDate = CostDs.Tables[0].Rows[0]["ExpCompDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(CostDs.Tables[0].Rows[0]["ExpCompDate"]);

                        JobAObj.ActualBeginDate = CostDs.Tables[0].Rows[0]["ActualBeginDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(CostDs.Tables[0].Rows[0]["ActualBeginDate"]);
                        JobAObj.ActulaCompleteDate = CostDs.Tables[0].Rows[0]["ActulaCompleteDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(CostDs.Tables[0].Rows[0]["ActulaCompleteDate"]);
                        JobAObj.TurnaroundTime = CostDs.Tables[0].Rows[0]["TurnaroundTime"] == DBNull.Value ? (TimeSpan?)null : Convert.ToDateTime(CostDs.Tables[0].Rows[0]["TurnaroundTime"]).TimeOfDay;

                        //JobAObj.CrewID = CostDs.Tables[0].Rows[0]["CrewID"] == DBNull.Value ? (int?)null : Convert.ToInt32(CostDs.Tables[0].Rows[0]["CrewID"]);
                        //JobAObj.SuperviserID = CostDs.Tables[0].Rows[0]["SuperviserID"] == DBNull.Value ? (int?)null : Convert.ToInt32(CostDs.Tables[0].Rows[0]["SuperviserID"]);
                        JobAObj.ScheduledDate = CostDs.Tables[0].Rows[0]["ScheduledDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(CostDs.Tables[0].Rows[0]["ScheduledDate"]);
                        JobAObj.CompletedDate = CostDs.Tables[0].Rows[0]["CompletedDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(CostDs.Tables[0].Rows[0]["CompletedDate"]);
                        JobAObj.RejectedDate = CostDs.Tables[0].Rows[0]["RejectedDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(CostDs.Tables[0].Rows[0]["RejectedDate"]);

                        JobAObj.Remark = Convert.ToString(CostDs.Tables[0].Rows[0]["Remark"]);
                        JobAObj.Special_Instructions = Convert.ToString(CostDs.Tables[0].Rows[0]["Special_Instructions"]);
                        JobAObj.JA_CurrentStatusID = Convert.ToInt32(CostDs.Tables[0].Rows[0]["JA_CurrentStatusID"]);
                        JobAObj.CurrentStatus = Convert.ToString(CostDs.Tables[0].Rows[0]["CurrentStatus"]);


                    }

                    ////Activity
                    if (CostDs.Tables.Count > 1 && CostDs.Tables[1] != null && CostDs.Tables[1].Rows.Count > 0)
                    {

                        JobAObj.activities = (from item in CostDs.Tables[1].AsEnumerable()
                                              select new JobActivity()
                                              {
                                                  //RateComponentRateID = Convert.ToInt32(item["RateCompRateID"]),
                                                  InstID = Convert.ToInt64(item["InstID"]),
                                                  ActivityID = Convert.ToInt64(item["ActivityID"]),
                                                  //MoveId = Convert.ToInt64(item["MoveID"]),
                                                  ActivityTypeID = Convert.ToInt32(item["ActivityTypeID"]),
                                                  ActivityName = Convert.ToString(item["ActivityTypeName"]),
                                                  Remark = Convert.ToString(item["Remark"]),
                                                  FromDate = item["FromDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(item["FromDate"]),
                                                  ToDate = item["ToDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(item["ToDate"]),
                                                  RepTime = item["Rep_Time"] == DBNull.Value ? (TimeSpan?)null : (TimeSpan)(item["Rep_Time"]),
                                                  FromLocation = Convert.ToString(item["From_Loc"]),
                                                  ToLocation = Convert.ToString(item["To_Loc"]),

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
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "JobAllocationBL", "GetDetailById", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

            return JobAObj;


        }


        public JobDiaryModel GetBulkInstDetailById(Int64[] InstID,Int64? BatchID,Int16 JobType)
        {
            

            try
            {
                JobDiaryModel obj = jobAllocationDAL.GetBulkInstDetailById(UserSession.GetUserSession().LoginID, InstID,BatchID, JobType);
                return obj;
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "JobAllocationBL", "GetBulkInstDetailById", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

            

        }

        public ActivityAllocationModel GetActivityJobAllocationById(Int64[] InstID, Int64[] ActivityID, Int64? BatchID,Int16 JobType)
        {
            try
            {
                ActivityAllocationModel obj = jobAllocationDAL.GetActivityJobAllocationById(UserSession.GetUserSession().LoginID, ActivityID, BatchID, JobType);
                if (InstID != null && InstID.Count() > 0)
                {
                    obj.InstIds = InstID.ToList();
                }
                return obj;
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "JobAllocationBL", "GetBulkInstDetailById", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

        }

        public JobActivity GetJobActivityDetailById(Int64 InstID, Int64? ActivityID)
        {
            JobActivity JobAObj = new JobActivity();
            try
            {
                JobAObj.InstID = InstID;
                DataSet CostDs = jobAllocationDAL.GetJobActivityDetailById(UserSession.GetUserSession().LoginID, InstID, ActivityID);

                if (CostDs != null && CostDs.Tables.Count > 1)
                {
                    
                    ////Activity
                    if (CostDs.Tables.Count > 0 && CostDs.Tables[0] != null && CostDs.Tables[0].Rows.Count > 0)
                    {

                        JobAObj = (from item in CostDs.Tables[0].AsEnumerable()
                                              select new JobActivity()
                                              {
                                                  //RateComponentRateID = Convert.ToInt32(item["RateCompRateID"]),
                                                  InstID = Convert.ToInt64(item["InstID"]),
                                                  ActivityID = Convert.ToInt64(item["ActivityID"]),
                                                  //MoveId = Convert.ToInt64(item["MoveID"]),
                                                  ActivityTypeID = Convert.ToInt32(item["ActivityTypeID"]),
                                                  ActivityName = Convert.ToString(item["ActivityTypeName"]),
                                                  Remark = Convert.ToString(item["Remark"]),
                                                  FromDate = item["FromDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(item["FromDate"]),
                                                  ToDate = item["ToDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(item["ToDate"]),
                                                  RepTime = item["Rep_Time"] == DBNull.Value ? (TimeSpan?)null : (TimeSpan)(item["Rep_Time"]),
                                                  FromLocation = Convert.ToString(item["From_Loc"]),
                                                  ToLocation = Convert.ToString(item["To_Loc"]),

                                              }).FirstOrDefault();
                    }
                    
                    ////Services details
                    if (CostDs.Tables.Count > 1 && CostDs.Tables[1] != null && CostDs.Tables[1].Rows.Count > 0)
                    {
                        JobAObj.services = (from item in CostDs.Tables[1].AsEnumerable()
                                            select new JobService()
                                            {
                                                SD_ID = Convert.ToInt64(item["SD_ID"]),
                                                ActivityID=Convert.ToInt64(item["ActivityID"]),
                                                //MoveId = Convert.ToInt64(item["MoveID"]),
                                                ServiceID = Convert.ToInt32(item["ServiceID"]),
                                                ServiceName = Convert.ToString(item["ServiceName"]),
                                                Description = Convert.ToString(item["ServiceDescription"]),
                                                FromDate = item["FromDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(item["FromDate"]),
                                                ToDate = item["FromDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(item["FromDate"]),
                                                Cost = Convert.ToDouble(item["Cost"]),
                                            }).ToList();

                    }


                    ////Crew details
                    if (CostDs.Tables.Count > 2 && CostDs.Tables[2] != null && CostDs.Tables[2].Rows.Count > 0)
                    {
                        JobAObj.jobCrew.members = (from item in CostDs.Tables[2].AsEnumerable()
                                                     select new CrewMember()
                                                     {
                                                         EmpID = Convert.ToInt32(item["EmpID"]),
                                                         CardEmpCode = Convert.ToString(item["CardEmpCode"]),
                                                         EmpName = Convert.ToString(item["EmpName"]),
                                                         EffectiveFrom = Convert.ToDateTime(item["FromDateTime"]),
                                                         EffectiveTo = Convert.ToDateTime(item["ToDateTime"])

                                                     }).ToList();

                    }

                    ////Vehicle
                    if (CostDs.Tables.Count > 3 && CostDs.Tables[3] != null && CostDs.Tables[3].Rows.Count > 0)
                    {

                        JobAObj.jobVehicleList = (from item in CostDs.Tables[3].AsEnumerable()
                                                             select new JobVehicle()
                                                             {
                                                                 ActivityID = Convert.ToInt64(item["ActivityID"]),
                                                                 //MoveID = Convert.ToInt64(item["MoveID"]),
                                                                 PurposeID = Convert.ToInt32(item["PurposeID"]),
                                                                 Purpose = Convert.ToString(item["PurposeName"]),
                                                                 VehicleID =!string.IsNullOrWhiteSpace(Convert.ToString(item["VehicleID"])) ? Convert.ToInt32(item["VehicleID"]) : (Int32?)null,
                                                                 VehicleNo = Convert.ToString(item["VehicleNo"]),
                                                                 DriverID = !string.IsNullOrWhiteSpace(Convert.ToString(item["DriverID"])) ? Convert.ToInt32(item["DriverID"]) : (Int32?)null,
                                                                 Driver = Convert.ToString(item["DriverName"]),
                                                                 VehicleType = Convert.ToString(item["VehicleType"]),
                                                                 DriverType = Convert.ToString(item["DriverType"]),
                                                                 FromDate = item["FromDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(item["FromDate"]),
                                                                 ToDate = item["ToDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(item["ToDate"]),
                                                                 V_Remark = Convert.ToString(item["Remark"]),
                                                                 V_Cost = !string.IsNullOrWhiteSpace(Convert.ToString(item["ApproxCost"])) ? Convert.ToInt64(item["ApproxCost"]) : (float?)null,
                                                                 Approve_StatusId = !string.IsNullOrWhiteSpace(Convert.ToString(item["StatusID"])) ? Convert.ToInt32(item["StatusID"]) : (Int32?)null,
                                                                 Approve_Status = Convert.ToString(item["StatusName"]),
                                                                 IsApprover = Convert.ToBoolean(Convert.ToString(item["IsApprover"])),
                                                                 Approve_By = Convert.ToString(item["ApprovedBy"]),
                                                                 Approve_Date = item["ApprovedDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(item["ApprovedDate"]),
                                                                 Approve_Remark = Convert.ToString(item["ApprovedRemark"]),
                                                                 MovementID = !string.IsNullOrWhiteSpace(Convert.ToString(item["DriverID"])) ? Convert.ToInt32(item["DriverID"]) : (Int32?)null,
                                                                 MovementName = Convert.ToString(item["DriverName"]),
                                                                 SupplierID = !string.IsNullOrWhiteSpace(Convert.ToString(item["DriverID"])) ? Convert.ToInt32(item["DriverID"]) : (Int32?)null,
                                                                 SupplierName = Convert.ToString(item["DriverName"]),
                                                                 DimensionID = !string.IsNullOrWhiteSpace(Convert.ToString(item["DriverID"])) ? Convert.ToInt32(item["DriverID"]) : (Int32?)null,
                                                                 DimensionName = Convert.ToString(item["DriverName"]),
                                                                 ReasonID = !string.IsNullOrWhiteSpace(Convert.ToString(item["DriverID"])) ? Convert.ToInt32(item["DriverID"]) : (Int32?)null,
                                                                 ReasonName = Convert.ToString(item["DriverName"]),
                                                                 FromLocation = Convert.ToString(item["FromLocation"]),
                                                                 ToLocation = Convert.ToString(item["ToLocation"]),
                                                                 VolumeCFT = Convert.ToString(item["VolumeCFT"])
                                                             }).ToList();
                    }


                    ///Documents

                    if (CostDs.Tables.Count > 4 && CostDs.Tables[4] != null && CostDs.Tables[4].Rows.Count > 0)
                    {
                        JobAObj.docUpload.docLists = (from item in CostDs.Tables[4].AsEnumerable()
                                                      select new DocList()
                                                      {
                                                          ActivityID = Convert.ToInt64(item["ActivityID"]),
                                                          DocID = Convert.ToInt64(item["JA_DocID"]),
                                                          DocTypeID = Convert.ToInt32(item["DocTypeID"]),
                                                          DocType = Convert.ToString(item["DocTypeName"]),
                                                          DocumentName = Convert.ToString(item["DocumentName"]),
                                                          file = (HttpPostedFileBase)new MemoryPostedFile(System.IO.File.ReadAllBytes(Convert.ToString(item["DocumentPath"])))

                                                      }).ToList();

                    }

                    if (CostDs.Tables.Count > 5 && CostDs.Tables[5] != null && CostDs.Tables[5].Rows.Count > 0)
                    {
                        JobAObj.materialUsed = (from item in CostDs.Tables[5].AsEnumerable()
                                                      select new MaterialUsed()
                                                      {
                                                          MaterailId = Convert.ToInt32(item["MaterailID"]),
                                                          Materail = Convert.ToString(item["MaterialName"]),
                                                          IssuedQty =!string.IsNullOrWhiteSpace(Convert.ToString(item["Issued_qty"]))? Convert.ToInt32(item["Issued_qty"]): (int?)null,
                                                          UsedQty = !string.IsNullOrWhiteSpace(Convert.ToString(item["Used_qty"])) ? Convert.ToInt32(item["Used_qty"]) : (int?)null,
                                                          ReturnQty = !string.IsNullOrWhiteSpace(Convert.ToString(item["Return_qty"])) ? Convert.ToInt32(item["Return_qty"]) : (int?)null,
                                                          Rate = Convert.ToInt64(item["Rate"]),

                                                      }).ToList();

                    }

                    if (CostDs.Tables.Count > 6 && CostDs.Tables[6] != null && CostDs.Tables[6].Rows.Count > 0)
                    {
                        JobAObj.outsideLabours = (from item in CostDs.Tables[6].AsEnumerable()
                         select new OutsideLabour()
                         {
                             CrewVendorID = Convert.ToInt32(item["CrewVendorID"]),
                             VendorName = Convert.ToString(item["VendorName"]),
                             NoOfPerson = Convert.ToInt32(item["NoOfPerson"]),
                             VendorCost = Convert.ToInt64(item["VendorCost"])
                         }).ToList();
                    }
                    else
                    {
                        JobAObj.outsideLabours.Add(new OutsideLabour() { CrewVendorID = 0 });
                    }


                }


            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "JobAllocationBL", "GetDetailById", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

            return JobAObj;

        }

        public JobCrew GetCrewMembers(DateTime From, DateTime To, int CrewID, Int64 BatchID = -1)
        {
            JobCrew crew = new JobCrew();
            try
            {

                DataSet CostDs = jobAllocationDAL.GetCrewMembers(UserSession.GetUserSession().LoginID,From,To,CrewID, BatchID);

                if (CostDs != null && CostDs.Tables.Count > 1)
                {

                    if (CostDs.Tables[0] != null && CostDs.Tables[0].Rows.Count > 0)
                    {
                        crew.CrewID = Convert.ToInt32(CostDs.Tables[0].Rows[0]["CrewID"]);
                        crew.SuperviserID = Convert.ToInt32(CostDs.Tables[0].Rows[0]["SuperviserID"]);
                    }

                    if (CostDs.Tables[1] != null && CostDs.Tables[1].Rows.Count > 0)
                    {
                        crew.members = (from item in CostDs.Tables[1].AsEnumerable()
                                        select new CrewMember()
                                        {

                                            EmpID = Convert.ToInt32(item["EmpID"]),
                                            CardEmpCode = Convert.ToString(item["CardEmpCode"]),
                                            EmpName = Convert.ToString(item["EmpName"]),
                                            EffectiveFrom = item["FromDateTime"] != DBNull.Value ? Convert.ToDateTime(item["FromDateTime"]) : (Nullable<DateTime>)null,
                                            EffectiveTo = item["ToDateTime"] != DBNull.Value ? Convert.ToDateTime(item["ToDateTime"]) : (Nullable<DateTime>)null,

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
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "JobAllocationBL", "GetCrewMembers", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

            return crew;
        }
        
        public bool InsertActivity(Entities.JobActivity jobActivity, out string result)
        {
            try
            {
                return jobAllocationDAL.InsertActivity(jobActivity, out result);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "JobAllocationBL", "InsertActivity", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

        }

        public Dictionary<string,string> GetVacantCrew(DateTime From, DateTime To,Int64 BatchID=-1)
        {
            
            try
            {
                return jobAllocationDAL.GetVacantCrew(From,To, BatchID, UserSession.GetUserSession().LoginID);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "JobAllocationBL", "GetVacantCrew", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }
        }

        public PJR_DJR GetPJR_DJR_Details(int MoveID,int? instID=1)
        {
            PJR_DJR pJR = new PJR_DJR();

            try
            {
                DataSet data= jobAllocationDAL.GetPJR_DJR_Details(UserSession.GetUserSession().LoginID, MoveID, instID);

                if (data!=null && data.Tables.Count>0)
                {
                    if (data.Tables.Count> 0 && data.Tables[0] != null && data.Tables[0].Rows.Count > 0)
                    {
                        pJR = (from item in data.Tables[0].AsEnumerable()
                                   select new PJR_DJR()
                                   {
                                       PJR_DJR_ID = Convert.ToInt64(item["PJR_DJR_ID"]),
                                       MoveID = Convert.ToInt64(item["MoveID"]),
                                       JobNo = Convert.ToString(item["JobNo"]),
                                       //MoveId = Convert.ToInt64(item["MoveID"]),
                                       NoOfPkgs = Convert.ToInt32(item["NoOfPkgs"]),
                                       Volume = Convert.ToInt64(item["Volume"]),
                                       CorprateName = Convert.ToString(item["CorprateName"]),
                                       Shipper = Convert.ToString(item["Shipper"]),
                                       Remark = Convert.ToString(item["Remark"]),
                                       ReportType = Convert.ToString(item["ReportType"]),
                                       Status = Convert.ToString(item["Status"]),
                                   }).FirstOrDefault();
                        
                    }

                    ////Service Details
                    if (data.Tables.Count > 1 && data.Tables[1] != null && data.Tables[1].Rows.Count > 0)
                    {
                        pJR.services = (from item in data.Tables[1].AsEnumerable()
                                            select new JobService()
                                            {
                                                //SD_ID = Convert.ToInt64(item["SD_ID"]),
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
                                                       EmpID = Convert.ToInt32(item["EmpID"]),
                                                       EmpName = Convert.ToString(item["EmpName"]),
                                                       NoOfDays = Convert.ToInt32(item["NoOfDays"]),
                                                       OT_Cost = Convert.ToInt64(item["OT_Cost"]),
                                                       OT_Rate = Convert.ToInt64(item["OT_Rate"]),
                                                       OT_hours = Convert.ToInt32(item["OT_hours"]),
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
                                                      VD_ID = Convert.ToInt64(item["PurposeID"]),
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
                        pJR.outLabourCosts = (from item in data.Tables[4].AsEnumerable()
                                             select new OutLabourCost()
                                             {
                                                 Labour_VendorId = Convert.ToInt32(item["Labour_VendorId"]),
                                                 Labour_Vendor = Convert.ToString(item["Labour_Vendor"]),
                                                 Labour_Cost = Convert.ToInt64(item["Labour_Cost"]),
                                                 Labour_OutsideNo = Convert.ToInt32(item["Used_qty"]),
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
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "JobAllocationBL", "GetPJR_DJR_Details", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

            return pJR;
        }

        public bool InsertActivityAllocation(ActivityAllocationModel model,out string result)
        {
            try
            {
                return jobAllocationDAL.InsertActivityAllocation(model, out result);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "JobAllocationBL", "InsertActivityAllocation", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

        }

        public bool DeleteActivityAllocation(Int64 BrachID,Int64 ActivityID,int LoginID, out string result)
        {
            try
            {
                return jobAllocationDAL.DeleteActivityAllocation(BrachID, ActivityID,LoginID, out result);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "JobAllocationBL", "DeleteActivityAllocation", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

        }

        public Dictionary<int,float> GetMaterialRate(int[] MaterailID)
        {
            try
            {
                return jobAllocationDAL.GetMaterialRate(MaterailID, UserSession.GetUserSession().LoginID);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "JobAllocationBL", "GetMaterialRate", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

        }

        public JobActivity getActivityDetailById(Int64 InstID,Int64? ActivityID)
        {
            JobActivity obj=new JobActivity();
            try
            {
                DataTable dt = jobAllocationDAL.getActivityDetailById(UserSession.GetUserSession().LoginID, InstID, ActivityID);

                if (dt!=null && dt.Rows.Count > 0)
                {
                    obj.InstID = InstID;
                    obj.ActivityID = Convert.ToInt64(dt.Rows[0]["ActivityID"]);
                    obj.ActivityTypeID = Convert.ToInt32(dt.Rows[0]["ActivityTypeID"]);
                    obj.ActivityName = Convert.ToString(dt.Rows[0]["ActivityTypeName"]);
                    obj.FromDate = !string.IsNullOrWhiteSpace(Convert.ToString(dt.Rows[0]["FromDate"]))? Convert.ToDateTime(dt.Rows[0]["FromDate"]) : (DateTime?)null;
                    obj.ToDate = !string.IsNullOrWhiteSpace(Convert.ToString(dt.Rows[0]["ToDate"])) ? Convert.ToDateTime(dt.Rows[0]["ToDate"]) : (DateTime?)null;
                    obj.FromLocation =  Convert.ToString(dt.Rows[0]["From_Loc"]);
                    obj.ToLocation = Convert.ToString(dt.Rows[0]["To_Loc"]);
                    obj.Remark = Convert.ToString(dt.Rows[0]["Remark"]);
                    obj.RepTime = !string.IsNullOrWhiteSpace(Convert.ToString(dt.Rows[0]["Rep_Time"])) ? (TimeSpan)(dt.Rows[0]["Rep_Time"]) : (TimeSpan?)null;
                }
                else
                {
                    obj.InstID = InstID;
                    obj.ActivityID = -1;
                }

            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "JobAllocationBL", "getActivityDetailById", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

            return obj;
        }

        public bool AddEditActivity(JobActivity model, out string message)
        {

            try
            {

                return jobAllocationDAL.AddEditActivity(model, out message);

            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "JobAllocationBL", "AddEditActivity", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }
        }

        public JobDocument GetDownloadFile(Int64 DocID, Int64? BatchID=-1)
        {
            JobDocument obj = new JobDocument();
            try
            {
                if (BatchID==null)
                {
                    BatchID = -1;
                }
                
                return jobAllocationDAL.GetDownloadFile(DocID, Convert.ToInt64(BatchID), UserSession.GetUserSession().LoginID);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "JobAllocationBL", "GetDownloadFile", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

            return obj;
        }

        public EmployeeAllocation GetEmployeeAllocation(int EmpID, DateTime FromDate, DateTime ToDate,Int64? BatchID)
        {
            EmployeeAllocation employee = new EmployeeAllocation();

            try
            {
                DataSet data = jobAllocationDAL.GetEmployeeAllocation(UserSession.GetUserSession().LoginID, EmpID, FromDate,ToDate, BatchID);

                if (data != null && data.Tables.Count > 0)
                {
                    if (data.Tables.Count > 0 && data.Tables[0] != null && data.Tables[0].Rows.Count > 0)
                    {
                        employee.existingAllocation = (from item in data.Tables[0].AsEnumerable()
                                                       select new CrewMember()
                                                       {
                                                           EmpName = Convert.ToString(item["EmpName"]),
                                                           EffectiveFrom = Convert.ToDateTime(item["FromDate"]),
                                                           EffectiveTo = Convert.ToDateTime(item["ToDate"]),
                                                           JobNo = Convert.ToString(item["JobNo"]),
                                                       }).ToList();

                    }

                    if (data.Tables.Count > 1 && data.Tables[1] != null && data.Tables[1].Rows.Count > 0)
                    {
                        employee.leaves = (from item in data.Tables[1].AsEnumerable()
                                                       select new EmpLeaves()
                                                       {
                                                           EmpName = Convert.ToString(item["EmpName"]),
                                                           FromDate = Convert.ToDateTime(item["FromDate"]),
                                                           ToDate = Convert.ToDateTime(item["ToDate"]),
                                                       }).ToList();
                    }
                }

                return employee;
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "JobAllocationBL", "GetEmployeeAllocation", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }
        }

        public List<JobDocument> GetJobDocuments(Int64 MoveID)
        {
            try
            {
                DataSet DocDs = new RELOCBS.DAL.MoveMange.MoveMangeDAL().GetEmailNDdocuments(Convert.ToInt32(MoveID));
                List<JobDocument> list=new List<JobDocument>();

                if (DocDs != null && DocDs.Tables.Count >= 1)
                {

                    if (DocDs.Tables.Count > 1 && DocDs.Tables[1] != null && DocDs.Tables[1].Rows.Count > 0)
                    {
                        list = (from item in DocDs.Tables[1].AsEnumerable()
                                           select new JobDocument()
                                           {
                                               FileID = Convert.ToInt32(item["FileID"]),
                                               DocType = Convert.ToString(item["DocTypeName"]),
                                               DocName = Convert.ToString(item["DocName"]),
                                               DocDescription = Convert.ToString(item["Description"]),
                                               FileName = Convert.ToString(item["DocFileName"]),
                                               UploadBy = Convert.ToString(item["UploadBy"]),
                                               ///FilePath = CommonSubs.EncryptSf(Convert.ToString(item["DocFilePath"]))
                                           }).ToList();
                    }

                }

                return list;
            }
            catch (Exception)
            {

                throw;
            }


        }

        public bool UpdateHiredVehicleApprovalStatus(ActivityAllocationModel model, out string result)
        {
            try
            {
                return jobAllocationDAL.UpdateHiredVehicleApprovalStatus(model, UserSession.GetUserSession().LoginID, out result);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "JobAllocationBL", "UpdateHiredVehicleApprovalStatus", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

        }

        public bool UpdateHiredVehicleApprovalStatus(ActivityAllocationModel model,int LoginID, out string result)
        {
            try
            {
                return jobAllocationDAL.UpdateHiredVehicleApprovalStatus(model, LoginID, out result);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(LoginID), "JobAllocationBL", "UpdateHiredVehicleApprovalStatus_1", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

        }

        public bool InsertSupervisorForDigitalInventory(ActivityAllocationModel model, out string result)
        {
            try
            {
                return jobAllocationDAL.InsertSupervisorForDigitalInventory(model, UserSession.GetUserSession().LoginID, out result);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "JobAllocationBL", "InsertSupervisorForDigitalInventory", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

        }

        public DataSet GetCrewUtilizationDashboard(DateTime ForMonthDate, int WarehoseId)
        {
            try
            {
                bool IsRmcBuss = UserSession.GetUserSession().BussinessLine != "NON RMC-BUSINESS"?true:false;
                return jobAllocationDAL.GetCrewUtilizationDashboard(UserSession.GetUserSession().LoginID, UserSession.GetUserSession().CompanyID, IsRmcBuss, ForMonthDate, WarehoseId);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "JobAllocationBL", "GetCrewUtilizationDashboard", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }
        }
    }
}