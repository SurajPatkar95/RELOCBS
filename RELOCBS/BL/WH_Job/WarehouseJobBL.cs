using RELOCBS.Common.ExceptionHandling;
using RELOCBS.DAL.WH_Job;
using RELOCBS.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Linq.Dynamic;
using RELOCBS.Entities;
using System.Data;

namespace RELOCBS.BL.WH_Job
{
    public class WarehouseJobBL
    {
        private WarehouseJobDAL _warehouseJobDAL;

        public WarehouseJobDAL warehouseJobDAL
        {

            get
            {
                if (this._warehouseJobDAL == null)
                    this._warehouseJobDAL = new WarehouseJobDAL();
                return this._warehouseJobDAL;
            }
        }

        public IEnumerable<Entities.WarehouseJob> GetList(DateTime? FromDate, DateTime? Todate, string sort, string sortdir, int skip, int pageSize, out int totalCount, bool IsJobDate, bool IsActivityDate, Int64? JobNo = null, string Shipper = null, int Status = -1, int JobTypeId = -1)
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
                IQueryable<Entities.WarehouseJob> JobReportList = warehouseJobDAL.GetList(FromDate, Todate, IsJobDate, IsActivityDate, JobNo, Shipper, RMCBuss, Status,JobTypeId);
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
                    return new List<Entities.WarehouseJob>();
                }
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "WarehouseJobBL", "GetList", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

        }

        public WarehouseJob GetDetails(Int64 JobID)
        {
            WarehouseJob pJR = new WarehouseJob();

            try
            {
                DataSet data = warehouseJobDAL.GetDetails(UserSession.GetUserSession().LoginID, JobID);

                if (data != null && data.Tables.Count > 0)
                {
                    if (data.Tables.Count > 0 && data.Tables[0] != null && data.Tables[0].Rows.Count > 0)
                    {
                        pJR = (from item in data.Tables[0].AsEnumerable()
                               select new WarehouseJob()
                               {
                                   JobID = !string.IsNullOrWhiteSpace(Convert.ToString(item["JobID"])) ? Convert.ToInt64(item["JobID"]) : (Int64?)null,
                                   JobNo = Convert.ToString(item["JobNo"]),
                                   //MoveId = Convert.ToInt64(item["MoveID"]),
                                   RevenueBranchID = Convert.ToInt32(item["RevenuBranchID"]),
                                   HandlingBranchID = Convert.ToInt32(item["HandlingBranchID"]),
                                   JobOpenDate = Convert.ToDateTime(item["JobDate"]),
                                   CompanyID = Convert.ToInt32(item["CompanyID"]),
                                   IsRMCBuss = Convert.ToBoolean(item["IsRMCBuss"]),
                                   BusinessLineID = Convert.ToInt32(item["BusinessLineID"]),
                                   RateComponentID = Convert.ToInt32(item["RateComponentID"]),
                                   JobTypeId = item["JobTypeID"] == DBNull.Value? 0 : Convert.ToInt32(item["JobTypeID"]),
                               }).FirstOrDefault();

                    }

                    ////Instructions List
                    if (data.Tables.Count > 1 && data.Tables[1] != null && data.Tables[1].Rows.Count > 0)
                    {
                        pJR.WHJob_Instructions = (from item in data.Tables[1].AsEnumerable()
                                        select new WHJob_InstructionSheet()
                                        {
                                            JobID = Convert.ToInt64(item["JobID"]),
                                            InstID = !string.IsNullOrWhiteSpace(Convert.ToString(item["InstID"])) ? Convert.ToInt64(item["InstID"]) : (Int64?)null,
                                            InstDate = Convert.ToDateTime(item["InstructionDate"]),
                                            WareHouseName = Convert.ToString(item["Warehoue_Name"]),
                                            //ActivityID = Convert.ToInt64(item["ActivityID"]),
                                            //MoveId = Convert.ToInt64(item["MoveID"]),
                                            BranchName = Convert.ToString(item["BranchName"]),
                                            Status = Convert.ToString(item["Status"]),
                                            WeightUnit = Convert.ToString(item["Wt_Vol"]),
                                            CreatedBy = Convert.ToString(item["CreatedBy"]),
                                            CreatedDate = Convert.ToDateTime(item["CreatedDate"]),
                                            ModifiedDate = !string.IsNullOrWhiteSpace(Convert.ToString(item["ModifiedDate"])) ? Convert.ToDateTime(item["ModifiedDate"]) : (DateTime?)null,
                                            ModifiedBy = Convert.ToString(item["ModifiedBy"]),
                                            JobTypeId = !string.IsNullOrWhiteSpace(Convert.ToString(item["JobTypeID"])) ? Convert.ToInt32(item["JobTypeID"]) : -1, 
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
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "WarehouseJobBL", "GetDetails", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

            return pJR;
        }

        public bool Insert(WarehouseJob model, string submit, out string result)
        {
            try
            {
                return warehouseJobDAL.Insert(model, submit, UserSession.GetUserSession().LoginID, out result);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "WarehouseJobBL", "Insert", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

        }

        public bool InsertInstructionSheet(WHJob_InstructionSheet model, string submit, out string result)
        {
            try
            {
                return warehouseJobDAL.InsertinstructionSheet(model, submit, UserSession.GetUserSession().LoginID, out result);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "WarehouseJobBL", "InsertInstructionSheet", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

        }

        public WHJob_InstructionSheet GetWHInstructionSheetDetail(Int64 MoveID, Int64 InstID)
        {
            WHJob_InstructionSheet Instrunction = new WHJob_InstructionSheet();
            try
            {
                DataSet InstDs = warehouseJobDAL.GetWHInstructionSheetDetail(UserSession.GetUserSession().LoginID, MoveID, InstID);

                if (InstDs != null && InstDs.Tables.Count > 1)
                {

                    if (InstDs.Tables.Count > 0 && InstDs.Tables[0] != null && InstDs.Tables[0].Rows.Count > 0)
                    {
                        Instrunction.JobID = Convert.ToInt64(InstDs.Tables[0].Rows[0]["JobID"]);
                        //Instrunction.ServiceLineID = Convert.ToInt32(InstDs.Tables[0].Rows[0]["ServiceLineID"]);
                        //Instrunction.ServiceLine = Convert.ToString(InstDs.Tables[0].Rows[0]["ServiceLine"]);
                        //Instrunction.ProjectService = Convert.ToString(InstDs.Tables[0].Rows[0]["ServiceProject"]);
                        Instrunction.InstID = InstDs.Tables[0].Rows[0]["InstID"] == DBNull.Value ? (Int64?)null : Convert.ToInt64(InstDs.Tables[0].Rows[0]["InstID"]);
                        Instrunction.JobNo = Convert.ToString(InstDs.Tables[0].Rows[0]["JobNo"]);
                        Instrunction.Remarks = Convert.ToString(InstDs.Tables[0].Rows[0]["Remark"]);
                        Instrunction.SpecialInstructions = Convert.ToString(InstDs.Tables[0].Rows[0]["Special_Instruction"]);
                        Instrunction.WeightUnitID = !string.IsNullOrWhiteSpace(Convert.ToString(InstDs.Tables[0].Rows[0]["WeightUnitID"])) ? Convert.ToInt32(InstDs.Tables[0].Rows[0]["WeightUnitID"]) : 0;
                        Instrunction.WeightUnit = Convert.ToString(InstDs.Tables[0].Rows[0]["WeightUnitName"]);
                        Instrunction.WeightUnitFrom = !string.IsNullOrWhiteSpace(Convert.ToString(InstDs.Tables[0].Rows[0]["WeightFrom"])) ? Convert.ToInt64(InstDs.Tables[0].Rows[0]["WeightFrom"]) : (float?)null;
                        Instrunction.WeightUnitTo = !string.IsNullOrWhiteSpace(Convert.ToString(InstDs.Tables[0].Rows[0]["WeightTo"])) ? Convert.ToInt64(InstDs.Tables[0].Rows[0]["WeightTo"]) : (float?)null;
                        Instrunction.BranchID = !string.IsNullOrWhiteSpace(Convert.ToString(InstDs.Tables[0].Rows[0]["BranchID"])) ? Convert.ToInt32(InstDs.Tables[0].Rows[0]["BranchID"]) : 0;
                        Instrunction.WarehouseID = !string.IsNullOrWhiteSpace(Convert.ToString(InstDs.Tables[0].Rows[0]["WarehouseID"])) ? Convert.ToInt32(InstDs.Tables[0].Rows[0]["WarehouseID"]) : 0;
                        Instrunction.IsSentToWarehouse = !string.IsNullOrWhiteSpace(Convert.ToString(InstDs.Tables[0].Rows[0]["IsSentToWarehouse"])) ? Convert.ToInt32(InstDs.Tables[0].Rows[0]["IsSentToWarehouse"]) : 0;
                        Instrunction.StatusID = !string.IsNullOrWhiteSpace(Convert.ToString(InstDs.Tables[0].Rows[0]["StatusID"])) ? Convert.ToInt32(InstDs.Tables[0].Rows[0]["StatusID"]) : 0;
                        Instrunction.Status = Convert.ToString(InstDs.Tables[0].Rows[0]["StatusName"]);
                        Instrunction.WareHouseName = Convert.ToString(InstDs.Tables[0].Rows[0]["Warehoue_Name"]);
                        Instrunction.BranchName = Convert.ToString(InstDs.Tables[0].Rows[0]["CompBranchName"]);
                        Instrunction.RateComponentID = Convert.ToInt32(InstDs.Tables[0].Rows[0]["RateComponentID"]);
                        Instrunction.BusinessLine = Convert.ToString(InstDs.Tables[0].Rows[0]["BusinessLine"]);
                        Instrunction.RevenueBranch = Convert.ToString(InstDs.Tables[0].Rows[0]["RevenueBranch"]);
                        Instrunction.HandlingBranch = Convert.ToString(InstDs.Tables[0].Rows[0]["HandlingBranch"]);
                        Instrunction.JobTypeId = !string.IsNullOrWhiteSpace(Convert.ToString(InstDs.Tables[0].Rows[0]["JobTypeID"])) ? Convert.ToInt32(InstDs.Tables[0].Rows[0]["JobTypeID"]) : -1; 
                    }
                    
                    ////Activity
                    if (InstDs.Tables.Count > 1 && InstDs.Tables[1] != null && InstDs.Tables[1].Rows.Count > 0)
                    {

                        Instrunction.activities = (from item in InstDs.Tables[1].AsEnumerable()
                                              select new Inst_Activities()
                                              {
                                                  //RateComponentRateID = Convert.ToInt32(item["RateCompRateID"]),
                                                  ActivityID = Convert.ToInt64(item["ActivityID"]),
                                                  InstID = Convert.ToInt64(item["InstID"]),
                                                  ActivityTypeID = Convert.ToInt32(item["ActivityTypeID"]),
                                                  ActivityTypeName = Convert.ToString(item["ActivityTypeName"]),
                                                  Remark = Convert.ToString(item["Remark"]),
                                                  //FromDate = item["FromDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(item["FromDate"]),
                                                  //ToDate = item["ToDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(item["ToDate"]),
                                                  FromDate = Convert.ToDateTime(item["FromDate"]),
                                                  ToDate = Convert.ToDateTime(item["ToDate"]),
                                                  RepTime = string.IsNullOrWhiteSpace(Convert.ToString(item["Rep_Time"])) ? (TimeSpan?)null : (TimeSpan)(item["Rep_Time"]),
                                                  FromLocation = Convert.ToString(item["From_Loc"]),
                                                  ToLocation = Convert.ToString(item["To_Loc"]),
                                                  NumberOfDays = string.IsNullOrWhiteSpace(Convert.ToString(item["NoOfDays"])) ? (int?)(null) : Convert.ToInt32(item["NoOfDays"]),

                                              }).ToList();
                    }

                    //////Origin/Destination Addresss
                    if (InstDs.Tables.Count > 2 && InstDs.Tables[2] != null && InstDs.Tables[2].Rows.Count > 0)
                    {
                        Instrunction.Add1 = Convert.ToString(InstDs.Tables[2].Rows[0]["OrgAdd1"]);
                        Instrunction.Add2 = Convert.ToString(InstDs.Tables[2].Rows[0]["OrgAdd2"]);
                        Instrunction.CityID = !string.IsNullOrWhiteSpace(Convert.ToString(InstDs.Tables[2].Rows[0]["OrgCityID"])) ? Convert.ToInt32(InstDs.Tables[2].Rows[0]["OrgCityID"]) : 0;
                        Instrunction.City = Convert.ToString(InstDs.Tables[2].Rows[0]["OrgCity"]);
                        Instrunction.Pincode = !string.IsNullOrWhiteSpace(Convert.ToString(InstDs.Tables[2].Rows[0]["OrgPinCode"])) ? Convert.ToInt32(InstDs.Tables[2].Rows[0]["OrgPinCode"]) : 0;
                        Instrunction.Phone = Convert.ToString(InstDs.Tables[2].Rows[0]["OrgPhone"]);
                        Instrunction.Mobile = Convert.ToString(InstDs.Tables[2].Rows[0]["OrgMob"]);
                        Instrunction.Email = Convert.ToString(InstDs.Tables[2].Rows[0]["OrgEmail"]);
                    }
                }

               return Instrunction;
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "WarehouseJobBL", "GetWHInstructionSheetDetail", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }
        }

        public WHJob_InstructionSheet GetWHInstructionPrintDetail(Int64 MoveID, Int64 InstID)
        {
            WHJob_InstructionSheet Instrunction = new WHJob_InstructionSheet();
            try
            {
                DataSet InstDs = warehouseJobDAL.GetWHInstructionPrintDetail(UserSession.GetUserSession().LoginID, MoveID, InstID);

                if (InstDs != null && InstDs.Tables.Count > 1)
                {

                    if (InstDs.Tables.Count > 0 && InstDs.Tables[0] != null && InstDs.Tables[0].Rows.Count > 0)
                    {
                        Instrunction.JobID = Convert.ToInt64(InstDs.Tables[0].Rows[0]["JobID"]);
                        Instrunction.InstID = InstDs.Tables[0].Rows[0]["InstID"] == DBNull.Value ? (Int64?)null : Convert.ToInt64(InstDs.Tables[0].Rows[0]["InstID"]);
                        Instrunction.JobNo = Convert.ToString(InstDs.Tables[0].Rows[0]["JobNo"]);
                        Instrunction.JobOpenDate = Convert.ToDateTime(InstDs.Tables[0].Rows[0]["JobOpenDate"]);
                        Instrunction.Remarks = Convert.ToString(InstDs.Tables[0].Rows[0]["Remark"]);
                        Instrunction.SpecialInstructions = Convert.ToString(InstDs.Tables[0].Rows[0]["Special_Instruction"]);
                        Instrunction.WeightUnitID = !string.IsNullOrWhiteSpace(Convert.ToString(InstDs.Tables[0].Rows[0]["WeightUnitID"])) ? Convert.ToInt32(InstDs.Tables[0].Rows[0]["WeightUnitID"]) : 0;
                        Instrunction.WeightUnit = Convert.ToString(InstDs.Tables[0].Rows[0]["WeightUnitName"]);
                        Instrunction.WeightUnitFrom = !string.IsNullOrWhiteSpace(Convert.ToString(InstDs.Tables[0].Rows[0]["WeightFrom"])) ? Convert.ToInt64(InstDs.Tables[0].Rows[0]["WeightFrom"]) : (float?)null;
                        Instrunction.WeightUnitTo = !string.IsNullOrWhiteSpace(Convert.ToString(InstDs.Tables[0].Rows[0]["WeightFrom"])) ? Convert.ToInt64(InstDs.Tables[0].Rows[0]["WeightTo"]) : (float?)null;
                        Instrunction.BranchID = !string.IsNullOrWhiteSpace(Convert.ToString(InstDs.Tables[0].Rows[0]["BranchID"])) ? Convert.ToInt32(InstDs.Tables[0].Rows[0]["BranchID"]) : 0;
                        Instrunction.WarehouseID = !string.IsNullOrWhiteSpace(Convert.ToString(InstDs.Tables[0].Rows[0]["WarehouseID"])) ? Convert.ToInt32(InstDs.Tables[0].Rows[0]["WarehouseID"]) : 0;
                        Instrunction.IsSentToWarehouse = !string.IsNullOrWhiteSpace(Convert.ToString(InstDs.Tables[0].Rows[0]["IsSentToWarehouse"])) ? Convert.ToInt32(InstDs.Tables[0].Rows[0]["IsSentToWarehouse"]) : 0;
                        Instrunction.StatusID = !string.IsNullOrWhiteSpace(Convert.ToString(InstDs.Tables[0].Rows[0]["StatusID"])) ? Convert.ToInt32(InstDs.Tables[0].Rows[0]["StatusID"]) : 0;
                        Instrunction.Status = Convert.ToString(InstDs.Tables[0].Rows[0]["StatusName"]);
                        Instrunction.WareHouseName = Convert.ToString(InstDs.Tables[0].Rows[0]["Warehoue_Name"]);
                        Instrunction.BranchName = Convert.ToString(InstDs.Tables[0].Rows[0]["CompBranchName"]);
                        Instrunction.RateComponentID = Convert.ToInt32(InstDs.Tables[0].Rows[0]["RateComponentID"]);
                        Instrunction.BusinessLine = Convert.ToString(InstDs.Tables[0].Rows[0]["BusinessLine"]);
                        Instrunction.HandlingBranch = Convert.ToString(InstDs.Tables[0].Rows[0]["HandlingBranch"]);
                        Instrunction.RevenueBranch = Convert.ToString(InstDs.Tables[0].Rows[0]["RevenueBranch"]);
                        Instrunction.CreatedBy = Convert.ToString(InstDs.Tables[0].Rows[0]["createdBy"]);
                        Instrunction.CreatedDate = Convert.ToDateTime(InstDs.Tables[0].Rows[0]["CreatedDate"]);
                        Instrunction.InsType = Convert.ToString(InstDs.Tables[0].Rows[0]["InsType"]);
                    }

                    ////Activity
                    if (InstDs.Tables.Count > 1 && InstDs.Tables[1] != null && InstDs.Tables[1].Rows.Count > 0)
                    {

                        Instrunction.activities = (from item in InstDs.Tables[1].AsEnumerable()
                                                   select new Inst_Activities()
                                                   {
                                                       //RateComponentRateID = Convert.ToInt32(item["RateCompRateID"]),
                                                       ActivityID = Convert.ToInt64(item["ActivityID"]),
                                                       InstID = Convert.ToInt64(item["InstID"]),
                                                       ActivityTypeID = Convert.ToInt32(item["ActivityTypeID"]),
                                                       ActivityTypeName = Convert.ToString(item["ActivityTypeName"]),
                                                       Remark = Convert.ToString(item["Remark"]),
                                                       //FromDate = item["FromDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(item["FromDate"]),
                                                       //ToDate = item["ToDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(item["ToDate"]),
                                                       FromDate = Convert.ToDateTime(item["FromDate"]),
                                                       ToDate = Convert.ToDateTime(item["ToDate"]),
                                                       RepTime = string.IsNullOrWhiteSpace(Convert.ToString(item["Rep_Time"])) ? (TimeSpan?)null : (TimeSpan)(item["Rep_Time"]),
                                                       FromLocation = Convert.ToString(item["From_Loc"]),
                                                       ToLocation = Convert.ToString(item["To_Loc"]),
                                                       NumberOfDays = string.IsNullOrWhiteSpace(Convert.ToString(item["NoOfDays"])) ? (int?)(null) : Convert.ToInt32(item["NoOfDays"]),

                                                   }).ToList();
                    }

                    //////Origin/Destination Addresss
                    if (InstDs.Tables.Count > 2 && InstDs.Tables[2] != null && InstDs.Tables[2].Rows.Count > 0)
                    {
                        Instrunction.Add1 = Convert.ToString(InstDs.Tables[2].Rows[0]["OrgAdd1"]);
                        Instrunction.Add2 = Convert.ToString(InstDs.Tables[2].Rows[0]["OrgAdd2"]);
                        Instrunction.CityID = !string.IsNullOrWhiteSpace(Convert.ToString(InstDs.Tables[2].Rows[0]["OrgCityID"])) ? Convert.ToInt32(InstDs.Tables[2].Rows[0]["OrgCityID"]) : 0;
                        Instrunction.City = Convert.ToString(InstDs.Tables[2].Rows[0]["OrgCity"]);
                        Instrunction.Pincode = !string.IsNullOrWhiteSpace(Convert.ToString(InstDs.Tables[2].Rows[0]["OrgPinCode"])) ? Convert.ToInt32(InstDs.Tables[2].Rows[0]["OrgPinCode"]) : 0;
                        Instrunction.Phone = Convert.ToString(InstDs.Tables[2].Rows[0]["OrgPhone"]);
                        Instrunction.Mobile = Convert.ToString(InstDs.Tables[2].Rows[0]["OrgMob"]);
                        Instrunction.Email = Convert.ToString(InstDs.Tables[2].Rows[0]["OrgEmail"]);
                    }
                }

                return Instrunction;

            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "WarehouseJobBL", "GetWHInstructionPrintDetail", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }
        }
    }
}