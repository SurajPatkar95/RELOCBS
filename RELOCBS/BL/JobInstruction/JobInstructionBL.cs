using RELOCBS.Common.ExceptionHandling;
using RELOCBS.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RELOCBS.Entities;
using RELOCBS.DAL.JobInstruction;
using System.Data;
using System.Web.Mvc;

namespace RELOCBS.BL.JobInstruction
{
    public class JobInstructionBL
    {
        private ComboBL _comboBL;
        public ComboBL comboBL
        {
            get
            {
                if (this._comboBL == null)
                    this._comboBL = new ComboBL();
                return this._comboBL;

            }
        }

        private JobInstructionDAL  _jobInstructionDAL;

        public JobInstructionDAL jobInstructionDAL
        {

            get
            {
                if (this._jobInstructionDAL == null)
                    this._jobInstructionDAL = new JobInstructionDAL();
                return this._jobInstructionDAL;
            }
        }

        public bool Insert(Entities.InstructionSheet sheet,string submit, out string result)
        {
            try
            {
                return jobInstructionDAL.Insert(sheet, submit, out result);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "JobInstructionBL", "Insert", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

        }

        public IEnumerable<JobInstGrid> GetForGrid(string JobNo, Int64 Moveid, DateTime? FromDate, DateTime? Todate,string Shipper, string sort, string sortdir, int skip, int pageSize, out int totalCount)
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

                IQueryable<Entities.JobInstGrid> InstructionSheetList = jobInstructionDAL.GetForGrid(UserSession.GetUserSession().LoginID, UserSession.GetUserSession().CompanyID, JobNo, Moveid, FromDate, Todate, Shipper, RMCBuss);

                totalCount = InstructionSheetList.Count();
                
                if (pageSize > 1)
                {
                    InstructionSheetList = InstructionSheetList.Skip((skip * (pageSize - 1))).Take(skip);
                }
                else
                {
                    InstructionSheetList = InstructionSheetList.Take(skip);
                }

                //InstructionSheetList = InstructionSheetList.OrderBy(sort + " " + sortdir);

                return InstructionSheetList.ToList();

            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "JobInstructionBL", "GetForGrid", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }


        }

        public  InstructionSheet GetDetailById(Int64 MoveID, Int64 JAID)
        {

            InstructionSheet JobAObj = new InstructionSheet();
            try
            {
                JobAObj.MoveID = MoveID;
                DataSet CostDs = jobInstructionDAL.GetDetailById(UserSession.GetUserSession().LoginID, JAID, MoveID);

                if (CostDs != null && CostDs.Tables.Count > 1)
                {

                    if (CostDs.Tables.Count > 0 && CostDs.Tables[0] != null && CostDs.Tables[0].Rows.Count > 0)
                    {

                        JobAObj.MoveID = Convert.ToInt64(CostDs.Tables[0].Rows[0]["MoveID"]);
                        JobAObj.ServiceLineID = Convert.ToInt32(CostDs.Tables[0].Rows[0]["ServiceLineID"]);
                        JobAObj.ServiceLine = Convert.ToString(CostDs.Tables[0].Rows[0]["ServiceLine"]);
                        JobAObj.ProjectService = Convert.ToString(CostDs.Tables[0].Rows[0]["ServiceProject"]);
                        JobAObj.InstID = CostDs.Tables[0].Rows[0]["InstID"] == DBNull.Value ? (Int64?)null : Convert.ToInt64(CostDs.Tables[0].Rows[0]["InstID"]);
                        JobAObj.JobNo = Convert.ToString(CostDs.Tables[0].Rows[0]["JobNo"]);
                        //JobAObj.JobOpenDate = Convert.ToDateTime(CostDs.Tables[0].Rows[0]["JobOpenedDate"]);

                        JobAObj.ExpectedBeginDateTime = CostDs.Tables[0].Rows[0]["ExpectedBeginDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(CostDs.Tables[0].Rows[0]["ExpectedBeginDate"]);
                        JobAObj.ExpectedCompletionDateTime = CostDs.Tables[0].Rows[0]["ExpectedCompletionDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(CostDs.Tables[0].Rows[0]["ExpectedCompletionDate"]);
                        JobAObj.InsTypeID = CostDs.Tables[0].Rows[0]["RateComponentID"] == DBNull.Value ? 0 : Convert.ToInt32(CostDs.Tables[0].Rows[0]["RateComponentID"]);
                        //JobAObj.ActualBeginDate = CostDs.Tables[0].Rows[0]["ActualBeginDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(CostDs.Tables[0].Rows[0]["ActualBeginDate"]);
                        //JobAObj.ActulaCompleteDate = CostDs.Tables[0].Rows[0]["ActulaCompleteDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(CostDs.Tables[0].Rows[0]["ActulaCompleteDate"]);
                        //JobAObj.TurnaroundTime = CostDs.Tables[0].Rows[0]["TurnaroundTime"] == DBNull.Value ? (TimeSpan?)null : Convert.ToDateTime(CostDs.Tables[0].Rows[0]["TurnaroundTime"]).TimeOfDay;

                        //JobAObj.CrewID = CostDs.Tables[0].Rows[0]["CrewID"] == DBNull.Value ? (int?)null : Convert.ToInt32(CostDs.Tables[0].Rows[0]["CrewID"]);
                        //JobAObj.SuperviserID = CostDs.Tables[0].Rows[0]["SuperviserID"] == DBNull.Value ? (int?)null : Convert.ToInt32(CostDs.Tables[0].Rows[0]["SuperviserID"]);
                        //JobAObj.ScheduledDate = CostDs.Tables[0].Rows[0]["ScheduledDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(CostDs.Tables[0].Rows[0]["ScheduledDate"]);
                        //JobAObj.CompletedDate = CostDs.Tables[0].Rows[0]["CompletedDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(CostDs.Tables[0].Rows[0]["CompletedDate"]);
                        //JobAObj.RejectedDate = CostDs.Tables[0].Rows[0]["RejectedDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(CostDs.Tables[0].Rows[0]["RejectedDate"]);

                        JobAObj.Remarks = Convert.ToString(CostDs.Tables[0].Rows[0]["Remark"]);
                        JobAObj.SpecialInstructions = Convert.ToString(CostDs.Tables[0].Rows[0]["Special_Instruction"]);
                        //JobAObj.JA_CurrentStatusID = Convert.ToInt32(CostDs.Tables[0].Rows[0]["JA_CurrentStatusID"]);
                        //JobAObj.CurrentStatus = Convert.ToString(CostDs.Tables[0].Rows[0]["CurrentStatus"]);
                        JobAObj.ClientID = Convert.ToInt32(CostDs.Tables[0].Rows[0]["ClientID"]);
                        JobAObj.Client = Convert.ToString(CostDs.Tables[0].Rows[0]["ClientName"]); 
                        JobAObj.Shipper = Convert.ToString(CostDs.Tables[0].Rows[0]["Shipper"]);
                        JobAObj.CorporateID = Convert.ToInt32(CostDs.Tables[0].Rows[0]["CorporateID"]);
                        JobAObj.ModeID = Convert.ToInt32(CostDs.Tables[0].Rows[0]["ModeID"]);
                        JobAObj.Mode = Convert.ToString(CostDs.Tables[0].Rows[0]["ModeName"]); 
                        JobAObj.WeightUnitID = Convert.ToInt32(CostDs.Tables[0].Rows[0]["WeightUnitID"]);
                        JobAObj.WeightUnit = Convert.ToString(CostDs.Tables[0].Rows[0]["WeightUnitName"]);
                        JobAObj.WeightUnitFrom = Convert.ToInt64(CostDs.Tables[0].Rows[0]["WeightFrom"]);
                        JobAObj.WeightUnitTo = Convert.ToInt64(CostDs.Tables[0].Rows[0]["WeightTo"]);
                        JobAObj.BranchID = !string.IsNullOrWhiteSpace(Convert.ToString(CostDs.Tables[0].Rows[0]["BranchID"])) ? Convert.ToInt32(CostDs.Tables[0].Rows[0]["BranchID"]) : 0;
                        JobAObj.WareHouseID = !string.IsNullOrWhiteSpace(Convert.ToString(CostDs.Tables[0].Rows[0]["WarehouseID"])) ? Convert.ToInt32(CostDs.Tables[0].Rows[0]["WarehouseID"]) : 0;
                        JobAObj.GoodsDescriptionID = !string.IsNullOrWhiteSpace(Convert.ToString(CostDs.Tables[0].Rows[0]["GoodsDescID"])) ? Convert.ToInt32(CostDs.Tables[0].Rows[0]["GoodsDescID"]) : 0;

                        JobAObj.IsSentToWarehouse=!string.IsNullOrWhiteSpace(Convert.ToString(CostDs.Tables[0].Rows[0]["IsSentToWarehouse"])) ? Convert.ToInt32(CostDs.Tables[0].Rows[0]["IsSentToWarehouse"]) : 0;
                        JobAObj.StatusID=!string.IsNullOrWhiteSpace(Convert.ToString(CostDs.Tables[0].Rows[0]["StatusID"])) ? Convert.ToInt32(CostDs.Tables[0].Rows[0]["StatusID"]) : 0;
                        JobAObj.Status = Convert.ToString(CostDs.Tables[0].Rows[0]["StatusName"]);
                        JobAObj.WareHouseName = Convert.ToString(CostDs.Tables[0].Rows[0]["Warehoue_Name"]);
                        JobAObj.BranchName = Convert.ToString(CostDs.Tables[0].Rows[0]["CompBranchName"]);
                        JobAObj.Corporate = Convert.ToString(CostDs.Tables[0].Rows[0]["CorporateName"]);
                        JobAObj.OriginCity = Convert.ToString(CostDs.Tables[0].Rows[0]["OriginCity"]);
                        JobAObj.DestinationCity = Convert.ToString(CostDs.Tables[0].Rows[0]["DestinationCity"]);
                        JobAObj.OriginCityID =  !string.IsNullOrWhiteSpace(Convert.ToString(CostDs.Tables[0].Rows[0]["OriginCityID"])) ? Convert.ToInt32(CostDs.Tables[0].Rows[0]["OriginCityID"]) : 0 ;
                        JobAObj.DestinationCityID = !string.IsNullOrWhiteSpace(Convert.ToString(CostDs.Tables[0].Rows[0]["DestinationCityID"])) ? Convert.ToInt32(CostDs.Tables[0].Rows[0]["DestinationCityID"]) : 0;
                        JobAObj.ComponentTypeID = !string.IsNullOrWhiteSpace(Convert.ToString(CostDs.Tables[0].Rows[0]["RateComponentID"])) ? Convert.ToInt32(CostDs.Tables[0].Rows[0]["RateComponentID"]) : (int?)null; 
                    }

                    ////Activity
                    if (CostDs.Tables.Count > 1 && CostDs.Tables[1] != null && CostDs.Tables[1].Rows.Count > 0)
                    {

                        JobAObj.activities = (from item in CostDs.Tables[1].AsEnumerable()
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

                    ////Case dimesions
                    if (CostDs.Tables.Count > 2 && CostDs.Tables[2] != null && CostDs.Tables[2].Rows.Count > 0)
                    {

                        JobAObj.Dimensions = (from item in CostDs.Tables[2].AsEnumerable()
                                              select new CaseDimensions()
                                              {
                                                  //RateComponentRateID = Convert.ToInt32(item["RateCompRateID"]),
                                                  CS_ID = Convert.ToInt64(item["CS_ID"]),
                                                  InstID = Convert.ToInt64(item["InstID"]),
                                                  CaseTypeID = Convert.ToInt32(item["CaseTypeID"]),
                                                  CaseType = Convert.ToString(item["CaseTypeName"]),
                                                  Length = Convert.ToInt64(item["Length"]),
                                                  Breadth = Convert.ToInt64(item["Breadth"]),
                                                  Height = Convert.ToInt64(item["Height"]),
                                                  UnitID = Convert.ToInt32(item["UnitID"]),
                                                  UnitName = Convert.ToString(item["UnitName"]),
                                                  NoOfPackages = Convert.ToInt32(item["NoOfPackages"]),

                                              }).ToList();
                    }

                    ////other info Master like print labels
                    if (CostDs.Tables.Count > 3 && CostDs.Tables[3] != null && CostDs.Tables[3].Rows.Count > 0)
                    {

                        JobAObj.modeLables = (from item in CostDs.Tables[3].AsEnumerable()
                                                          select new ModeLables()
                                                          {
                                                              //RateComponentRateID = Convert.ToInt32(item["RateCompRateID"]),
                                                              InfoID = Convert.ToInt32(item["InfoID"]),
                                                              InstID = Convert.ToInt32(item["InstID"]),
                                                              ModeID = Convert.ToInt32(item["MoveTypeID"]),
                                                              ModeName = Convert.ToString(item["MoveTypeName"]),
                                                              NoOfLables = Convert.ToInt32(item["PrintCount"]),
                                                              LabelStartFrom = Convert.ToString(item["LabelStart"]),
                                                              
                                                          }).ToList();
                    }

                    /////Question Master
                    if (CostDs.Tables.Count > 4 && CostDs.Tables[4] != null && CostDs.Tables[4].Rows.Count > 0)
                    {
                        JobAObj.questions =
                            (from item in CostDs.Tables[4].AsEnumerable()
                             select new Question()
                             {
                                 //RateComponentRateID = Convert.ToInt32(item["RateCompRateID"]),
                                 QuestionID = Convert.ToInt32(item["QuestionID"]),
                                 Questions = Convert.ToString(item["Questions"]),
                                 IsSubItem = item["IsSubItem"] == null ? false : Convert.ToBoolean(item["IsSubItem"]),
                                 IsActive = Convert.ToBoolean(item["IsActive"]),
                                 IsSaved = Convert.ToString(item["IsSaved"]).ToUpper(),
                                 hide = Convert.ToString(item["IsSaved"]).ToUpper() == "YES" ? "" : "hide"

                             }).ToList();

                        JobAObj.SelectedMultiInstructionsId =CostDs.Tables[4].AsEnumerable().Where(r=>r.Field<string>("IsSaved").ToUpper()=="YES").Select(r => r.Field<int>("QuestionID")).ToArray();
                        
                        
                        foreach (Question row in JobAObj.questions)
                        {
                            JobAObj.Instructions.Add(new SelectListItem
                            {
                                Text = row.Questions,
                                Value = row.QuestionID.ToString(),
                                Selected = row.IsSaved.ToUpper() == "YES" ? true : false
                            });

                            ////Sub Question Master
                            if (CostDs.Tables.Count > 5 && CostDs.Tables[5] != null && CostDs.Tables[5].Rows.Count > 0 && CostDs.Tables[5].AsEnumerable().Where(r => r.Field<Int32>("QuestionID") == row.QuestionID).Count()>0)
                            {
                                row.subQuestions = (from item in CostDs.Tables[5].AsEnumerable().Where(r => r.Field<Int32>("QuestionID") == row.QuestionID).AsEnumerable()
                                                    select new SubQuestion()
                                                    {
                                                        InstID = Convert.ToInt64(item["InstID"]),
                                                        QuestionID = Convert.ToInt32(item["QuestionID"]),
                                                        SubQuestionID = Convert.ToInt32(item["SubQuestionID"]),
                                                        SubQuestions = Convert.ToString(item["SubQuestions"]),
                                                        DropdownType = Convert.ToString(item["DropdownType"]),
                                                        AnswerType = Convert.ToString(item["AnswerType"]),
                                                        AnswerDate = string.IsNullOrWhiteSpace(Convert.ToString(item["AnswerDate"])) ? (DateTime?)null : Convert.ToDateTime(item["AnswerDate"]),
                                                        AnswerText = Convert.ToString(item["AnswerText"]),
                                                        IDtoRefer = string.IsNullOrWhiteSpace(Convert.ToString(item["IDtoRefer"])) ? 0 : Convert.ToInt32(item["IDtoRefer"]),
                                                        IsActive = Convert.ToBoolean(item["IsActive"]),
                                                        IsSaved = Convert.ToString(item["IsSaved"]).ToUpper(),
                                                        OrderBy = Convert.ToInt16(item["OrderBy"]),
                                                        Answer = Convert.ToString(item["Answer"]),
                                                        AnswerDropdown = comboBL.GetInstQuestionAnswerDropdown(DropdownType: Convert.ToString(item["DropdownType"]))
                                                    }).ToList();
                            }
                        }
                    }

                    //////Origin/Destination Addresss
                    if (CostDs.Tables.Count>6 && CostDs.Tables[6] != null && CostDs.Tables[6].Rows.Count > 0)
                    {
                        JobAObj.OrgAdd1 = Convert.ToString(CostDs.Tables[6].Rows[0]["OrgAdd1"]);
                        JobAObj.OrgAdd2 = Convert.ToString(CostDs.Tables[6].Rows[0]["OrgAdd2"]);
                        JobAObj.OrgCityID = !string.IsNullOrWhiteSpace(Convert.ToString(CostDs.Tables[6].Rows[0]["OrgCityID"]))? Convert.ToInt32(CostDs.Tables[6].Rows[0]["OrgCityID"]) : 0 ;
                        JobAObj.OrgCity = Convert.ToString(CostDs.Tables[6].Rows[0]["OrgCity"]);
                        JobAObj.OrgPincode = Convert.ToString(CostDs.Tables[6].Rows[0]["OrgPinCode"]); 
                        JobAObj.OrgPhone = Convert.ToString(CostDs.Tables[6].Rows[0]["OrgPhone"]);
                        JobAObj.OrgMobile = Convert.ToString(CostDs.Tables[6].Rows[0]["OrgMob"]); 
                        JobAObj.OrgEmail = Convert.ToString(CostDs.Tables[6].Rows[0]["OrgEmail"]);
                        JobAObj.DestAdd1 = Convert.ToString(CostDs.Tables[6].Rows[0]["DestAdd1"]);
                        JobAObj.DestAdd2 = Convert.ToString(CostDs.Tables[6].Rows[0]["DestAdd2"]);
                        JobAObj.DestCityID = !string.IsNullOrWhiteSpace(Convert.ToString(CostDs.Tables[6].Rows[0]["DestCityID"])) ? Convert.ToInt32(CostDs.Tables[6].Rows[0]["DestCityID"]) : 0;
                        JobAObj.DestCity = Convert.ToString(CostDs.Tables[6].Rows[0]["DestCity"]);
                        JobAObj.DestPincode = Convert.ToString(CostDs.Tables[6].Rows[0]["DestPinCode"]);
                        JobAObj.DestPhone = Convert.ToString(CostDs.Tables[6].Rows[0]["DestPhone"]);
                        JobAObj.DestMobile = Convert.ToString(CostDs.Tables[6].Rows[0]["DestMob"]);
                        JobAObj.DestEmail = Convert.ToString(CostDs.Tables[6].Rows[0]["DestEmail"]);
                    }

                }


            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "JobInstructionBL", "GetDetailById", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

            return JobAObj;


        }

        public InstructionSheet GetPrintDetail(Int64 MoveID, Int64 JAID)
        {
            InstructionSheet JobAObj = new InstructionSheet();
            try
            {
                JobAObj.MoveID = MoveID;
                DataSet CostDs = jobInstructionDAL.GetPrintDetail(UserSession.GetUserSession().LoginID, JAID, MoveID);

                if (CostDs != null && CostDs.Tables.Count > 1)
                {

                    if (CostDs.Tables.Count > 0 && CostDs.Tables[0] != null && CostDs.Tables[0].Rows.Count > 0)
                    {

                        JobAObj.MoveID = Convert.ToInt64(CostDs.Tables[0].Rows[0]["MoveID"]);
                        JobAObj.ServiceLineID = Convert.ToInt32(CostDs.Tables[0].Rows[0]["ServiceLineID"]);
                        JobAObj.ServiceLine = Convert.ToString(CostDs.Tables[0].Rows[0]["ServiceLine"]);
                        JobAObj.ProjectService = Convert.ToString(CostDs.Tables[0].Rows[0]["ServiceProject"]);
                        JobAObj.InstID = CostDs.Tables[0].Rows[0]["InstID"] == DBNull.Value ? (Int64?)null : Convert.ToInt64(CostDs.Tables[0].Rows[0]["InstID"]);
                        JobAObj.JobNo = Convert.ToString(CostDs.Tables[0].Rows[0]["JobNo"]);
                        JobAObj.JobOpenDate = Convert.ToDateTime(CostDs.Tables[0].Rows[0]["JobOpenedDate"]);

                        JobAObj.ExpectedBeginDateTime = CostDs.Tables[0].Rows[0]["ExpectedBeginDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(CostDs.Tables[0].Rows[0]["ExpectedBeginDate"]);
                        JobAObj.ExpectedCompletionDateTime = CostDs.Tables[0].Rows[0]["ExpectedCompletionDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(CostDs.Tables[0].Rows[0]["ExpectedCompletionDate"]);
                        JobAObj.InsTypeID = CostDs.Tables[0].Rows[0]["RateComponentID"] == DBNull.Value ? 0 : Convert.ToInt32(CostDs.Tables[0].Rows[0]["RateComponentID"]);
                        JobAObj.InsType = Convert.ToString(CostDs.Tables[0].Rows[0]["InsType"]);
                        JobAObj.Remarks = Convert.ToString(CostDs.Tables[0].Rows[0]["Remark"]);
                        JobAObj.SpecialInstructions = Convert.ToString(CostDs.Tables[0].Rows[0]["Special_Instruction"]);
                        //JobAObj.JA_CurrentStatusID = Convert.ToInt32(CostDs.Tables[0].Rows[0]["JA_CurrentStatusID"]);
                        //JobAObj.CurrentStatus = Convert.ToString(CostDs.Tables[0].Rows[0]["CurrentStatus"]);
                        JobAObj.ClientID = Convert.ToInt32(CostDs.Tables[0].Rows[0]["ClientID"]);
                        JobAObj.Client = Convert.ToString(CostDs.Tables[0].Rows[0]["ClientName"]);
                        JobAObj.ClientAddress = Convert.ToString(CostDs.Tables[0].Rows[0]["ClientAddress"]);
                        JobAObj.Shipper = Convert.ToString(CostDs.Tables[0].Rows[0]["Shipper"]);
                        JobAObj.CorporateID = Convert.ToInt32(CostDs.Tables[0].Rows[0]["CorporateID"]);
                        JobAObj.Corporate = Convert.ToString(CostDs.Tables[0].Rows[0]["Corporate"]);
                        JobAObj.CorporateAddress = Convert.ToString(CostDs.Tables[0].Rows[0]["CorporateAddress"]);
                        JobAObj.ModeID = Convert.ToInt32(CostDs.Tables[0].Rows[0]["ModeID"]);
                        JobAObj.Mode = Convert.ToString(CostDs.Tables[0].Rows[0]["ModeName"]);
                        JobAObj.WeightUnitID = Convert.ToInt32(CostDs.Tables[0].Rows[0]["WeightUnitID"]);
                        JobAObj.WeightUnit = Convert.ToString(CostDs.Tables[0].Rows[0]["WeightUnitName"]);
                        JobAObj.WeightUnitFrom = Convert.ToInt64(CostDs.Tables[0].Rows[0]["WeightFrom"]);
                        JobAObj.WeightUnitTo = Convert.ToInt64(CostDs.Tables[0].Rows[0]["WeightTo"]);
                        JobAObj.BranchID = !string.IsNullOrWhiteSpace(Convert.ToString(CostDs.Tables[0].Rows[0]["BranchID"])) ? Convert.ToInt32(CostDs.Tables[0].Rows[0]["BranchID"]) : 0;
                        JobAObj.WareHouseID = !string.IsNullOrWhiteSpace(Convert.ToString(CostDs.Tables[0].Rows[0]["WarehouseID"])) ? Convert.ToInt32(CostDs.Tables[0].Rows[0]["WarehouseID"]) : 0;
                        JobAObj.GoodsDescriptionID = !string.IsNullOrWhiteSpace(Convert.ToString(CostDs.Tables[0].Rows[0]["GoodsDescID"])) ? Convert.ToInt32(CostDs.Tables[0].Rows[0]["GoodsDescID"]) : 0;
                        JobAObj.GoodsDescription = Convert.ToString(Convert.ToString(CostDs.Tables[0].Rows[0]["GoodsDescName"]));

                        JobAObj.IsSentToWarehouse = !string.IsNullOrWhiteSpace(Convert.ToString(CostDs.Tables[0].Rows[0]["IsSentToWarehouse"])) ? Convert.ToInt32(CostDs.Tables[0].Rows[0]["IsSentToWarehouse"]) : 0;
                        JobAObj.StatusID = !string.IsNullOrWhiteSpace(Convert.ToString(CostDs.Tables[0].Rows[0]["StatusID"])) ? Convert.ToInt32(CostDs.Tables[0].Rows[0]["StatusID"]) : 0;
                        JobAObj.Status = Convert.ToString(CostDs.Tables[0].Rows[0]["StatusName"]);
                        JobAObj.WareHouseName = Convert.ToString(CostDs.Tables[0].Rows[0]["Warehoue_Name"]);
                        JobAObj.BranchName = Convert.ToString(CostDs.Tables[0].Rows[0]["CompBranchName"]);
                        
                        JobAObj.OriginCity = Convert.ToString(CostDs.Tables[0].Rows[0]["OriginCity"]);
                        JobAObj.DestinationCity = Convert.ToString(CostDs.Tables[0].Rows[0]["DestinationCity"]);
                        JobAObj.OriginCityID = !string.IsNullOrWhiteSpace(Convert.ToString(CostDs.Tables[0].Rows[0]["OriginCityID"])) ? Convert.ToInt32(CostDs.Tables[0].Rows[0]["OriginCityID"]) : 0;
                        JobAObj.DestinationCityID = !string.IsNullOrWhiteSpace(Convert.ToString(CostDs.Tables[0].Rows[0]["DestinationCityID"])) ? Convert.ToInt32(CostDs.Tables[0].Rows[0]["DestinationCityID"]) : 0;
                        JobAObj.ComponentTypeID = !string.IsNullOrWhiteSpace(Convert.ToString(CostDs.Tables[0].Rows[0]["RateComponentID"])) ? Convert.ToInt32(CostDs.Tables[0].Rows[0]["RateComponentID"]) : (int?)null;
                        JobAObj.ComponentType = Convert.ToString(CostDs.Tables[0].Rows[0]["RateComponentName"]);
                        JobAObj.CreatedBy = Convert.ToString(CostDs.Tables[0].Rows[0]["createdBy"]); 
                    }

                    ////Activity
                    if (CostDs.Tables.Count > 1 && CostDs.Tables[1] != null && CostDs.Tables[1].Rows.Count > 0)
                    {

                        JobAObj.activities = (from item in CostDs.Tables[1].AsEnumerable()
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

                    ////Case dimesions
                    if (CostDs.Tables.Count > 2 && CostDs.Tables[2] != null && CostDs.Tables[2].Rows.Count > 0)
                    {

                        JobAObj.Dimensions = (from item in CostDs.Tables[2].AsEnumerable()
                                              select new CaseDimensions()
                                              {
                                                  //RateComponentRateID = Convert.ToInt32(item["RateCompRateID"]),
                                                  CS_ID = Convert.ToInt64(item["CS_ID"]),
                                                  InstID = Convert.ToInt64(item["InstID"]),
                                                  CaseTypeID = Convert.ToInt32(item["CaseTypeID"]),
                                                  CaseType = Convert.ToString(item["CaseTypeName"]),
                                                  Length = Convert.ToInt64(item["Length"]),
                                                  Breadth = Convert.ToInt64(item["Breadth"]),
                                                  Height = Convert.ToInt64(item["Height"]),
                                                  UnitID = Convert.ToInt32(item["UnitID"]),
                                                  UnitName = Convert.ToString(item["UnitName"]),
                                                  NoOfPackages = Convert.ToInt32(item["NoOfPackages"]),

                                              }).ToList();
                    }

                    ////other info Master like print labels
                    if (CostDs.Tables.Count > 3 && CostDs.Tables[3] != null && CostDs.Tables[3].Rows.Count > 0)
                    {

                        JobAObj.modeLables = (from item in CostDs.Tables[3].AsEnumerable()
                                              select new ModeLables()
                                              {
                                                  //RateComponentRateID = Convert.ToInt32(item["RateCompRateID"]),
                                                  InfoID = Convert.ToInt32(item["InfoID"]),
                                                  InstID = Convert.ToInt32(item["InstID"]),
                                                  ModeID = Convert.ToInt32(item["MoveTypeID"]),
                                                  ModeName = Convert.ToString(item["MoveTypeName"]),
                                                  NoOfLables = Convert.ToInt32(item["PrintCount"]),
                                                  LabelStartFrom = Convert.ToString(item["LabelStart"]),

                                              }).ToList();
                    }

                    /////Question Master
                    if (CostDs.Tables.Count > 4 && CostDs.Tables[4] != null && CostDs.Tables[4].Rows.Count > 0)
                    {
                        JobAObj.questions =
                            (from item in CostDs.Tables[4].AsEnumerable()
                             select new Question()
                             {
                                 //RateComponentRateID = Convert.ToInt32(item["RateCompRateID"]),
                                 QuestionID = Convert.ToInt32(item["QuestionID"]),
                                 Questions = Convert.ToString(item["Questions"]),
                                 IsSubItem = item["IsSubItem"] == null ? false : Convert.ToBoolean(item["IsSubItem"]),
                                 IsActive = Convert.ToBoolean(item["IsActive"]),
                                 IsSaved = Convert.ToString(item["IsSaved"]).ToUpper(),
                                 hide = Convert.ToString(item["IsSaved"]).ToUpper() == "YES" ? "" : "hide"

                             }).ToList();

                        JobAObj.SelectedMultiInstructionsId = CostDs.Tables[4].AsEnumerable().Where(r => r.Field<string>("IsSaved").ToUpper() == "YES").Select(r => r.Field<int>("QuestionID")).ToArray();


                        foreach (Question row in JobAObj.questions)
                        {
                            JobAObj.Instructions.Add(new SelectListItem
                            {
                                Text = row.Questions,
                                Value = row.QuestionID.ToString(),
                                Selected = row.IsSaved.ToUpper() == "YES" ? true : false
                            });

                            ////Sub Question Master
                            if (CostDs.Tables.Count > 5 && CostDs.Tables[5] != null && CostDs.Tables[5].Rows.Count > 0 && CostDs.Tables[5].AsEnumerable().Where(r => r.Field<Int32>("QuestionID") == row.QuestionID).Count() > 0)
                            {
                                row.subQuestions = (from item in CostDs.Tables[5].AsEnumerable().Where(r => r.Field<Int32>("QuestionID") == row.QuestionID).AsEnumerable()
                                                    select new SubQuestion()
                                                    {
                                                        InstID = Convert.ToInt64(item["InstID"]),
                                                        QuestionID = Convert.ToInt32(item["QuestionID"]),
                                                        SubQuestionID = Convert.ToInt32(item["SubQuestionID"]),
                                                        SubQuestions = Convert.ToString(item["SubQuestions"]),
                                                        DropdownType = Convert.ToString(item["DropdownType"]),
                                                        AnswerType = Convert.ToString(item["AnswerType"]),
                                                        AnswerDate = string.IsNullOrWhiteSpace(Convert.ToString(item["AnswerDate"])) ? (DateTime?)null : Convert.ToDateTime(item["AnswerDate"]),
                                                        AnswerText = Convert.ToString(item["AnswerText"]),
                                                        IDtoRefer = string.IsNullOrWhiteSpace(Convert.ToString(item["IDtoRefer"])) ? 0 : Convert.ToInt32(item["IDtoRefer"]),
                                                        IsActive = Convert.ToBoolean(item["IsActive"]),
                                                        IsSaved = Convert.ToString(item["IsSaved"]).ToUpper(),
                                                        OrderBy = Convert.ToInt16(item["OrderBy"]),
                                                        Answer = Convert.ToString(item["Answer"]),
                                                        AnswerDropdown = comboBL.GetInstQuestionAnswerDropdown(DropdownType: Convert.ToString(item["DropdownType"]))
                                                    }).ToList();
                            }
                        }
                    }

                    //////Origin/Destination Addresss
                    if (CostDs.Tables.Count > 6 && CostDs.Tables[6] != null && CostDs.Tables[6].Rows.Count > 0)
                    {
                        JobAObj.OrgAdd1    = Convert.ToString(CostDs.Tables[6].Rows[0]["OrgAdd1"]);
                        JobAObj.OrgAdd2    = Convert.ToString(CostDs.Tables[6].Rows[0]["OrgAdd2"]);
                        JobAObj.OrgCityID  = !string.IsNullOrWhiteSpace(Convert.ToString(CostDs.Tables[6].Rows[0]["OrgCityID"])) ? Convert.ToInt32(CostDs.Tables[6].Rows[0]["OrgCityID"]) : 0;
                        JobAObj.OrgCity    = Convert.ToString(CostDs.Tables[6].Rows[0]["OrgCity"]);
                        JobAObj.OrgPincode = Convert.ToString(CostDs.Tables[6].Rows[0]["OrgPinCode"]);
                        JobAObj.OrgPhone   = Convert.ToString(CostDs.Tables[6].Rows[0]["OrgPhone"]);
                        JobAObj.OrgMobile  = Convert.ToString(CostDs.Tables[6].Rows[0]["OrgMob"]);
                        JobAObj.OrgEmail   = Convert.ToString(CostDs.Tables[6].Rows[0]["OrgEmail"]);
                        JobAObj.DestAdd1 = Convert.ToString(CostDs.Tables[6].Rows[0]["DestAdd1"]);
                        JobAObj.DestAdd2 = Convert.ToString(CostDs.Tables[6].Rows[0]["DestAdd2"]);
                        JobAObj.DestCityID = !string.IsNullOrWhiteSpace(Convert.ToString(CostDs.Tables[6].Rows[0]["DestCityID"])) ? Convert.ToInt32(CostDs.Tables[6].Rows[0]["DestCityID"]) : 0;
                        JobAObj.DestCity = Convert.ToString(CostDs.Tables[6].Rows[0]["DestCity"]);
                        JobAObj.DestPincode = Convert.ToString(CostDs.Tables[6].Rows[0]["DestPinCode"]);
                        JobAObj.DestPhone = Convert.ToString(CostDs.Tables[6].Rows[0]["DestPhone"]);
                        JobAObj.DestMobile = Convert.ToString(CostDs.Tables[6].Rows[0]["DestMob"]);
                        JobAObj.DestEmail = Convert.ToString(CostDs.Tables[6].Rows[0]["DestEmail"]);
                    }

                }


            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "JobInstructionBL", "GetPrintDetail", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

            return JobAObj;
        }

    }
}