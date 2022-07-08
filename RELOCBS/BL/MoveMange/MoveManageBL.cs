using RELOCBS.Common.ExceptionHandling;
using RELOCBS.DAL.MoveMange;
using RELOCBS.Entities;
using RELOCBS.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.Mvc;
using System.Xml.Linq;

namespace RELOCBS.BL.MoveMange
{
    public class MoveManageBL
    {
        private MoveMangeDAL _moveMangeDAL;

        public MoveMangeDAL moveMangeDAL
        {

            get
            {
                if (this._moveMangeDAL == null)
                    this._moveMangeDAL = new MoveMangeDAL();
                return this._moveMangeDAL;
            }
        }

        public DataSet GetCostSheet(Int64? MoveID, int LoginID = 0)
        {

            try
            {
                LoginID = LoginID == 0 ? UserSession.GetUserSession().LoginID : LoginID;
                return moveMangeDAL.GetCostSheet(LoginID, MoveID);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "MoveManageBL", "GetCostSheet", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

        }

        public bool InsertMove(MoveManageViewModel SaveData, int LoginID, out string result)
        {
            result = String.Empty;

            try
            {
                return moveMangeDAL.InsertMove(SaveData, LoginID, out result);

            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(LoginID), "MoveManageBL", "InsertMove", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

        }

        public bool InsertSurvey(MoveManageViewModel SaveData, int LoginID, out string result)
        {
            result = String.Empty;
            RELOCBS.BL.Survey.SurveyBL surveyBL = new RELOCBS.BL.Survey.SurveyBL();
            SurveyViewModel suveymodel = new SurveyViewModel()
            {
                SurveyId = SaveData.SurveyID,
                SurveyDate = SaveData.SurveyReport.Surveydate,
                SurveyDateTime = SaveData.SurveyReport.SurveyDateTime,
                LoadDate = SaveData.SurveyReport.Loaddate,
                PackDate = SaveData.SurveyReport.Packdate,
                SurveyConductedById = SaveData.SurveyReport.SurveyorID,
                shipmentDetail = new ShipmentDetail()
                {
                    SurveyDensityFact = SaveData.SurveyReport.DensityFact,
                    Survey_VolumeUnitID = SaveData.SurveyReport.VolumeUnitID,
                    Survey_WeightUnitID = SaveData.SurveyReport.WeightUnitID,
                    Survey_Volume_NetValue = SaveData.SurveyReport.NetVol,
                    Survey_Volume_GrossValue = SaveData.SurveyReport.GrossVol,
                    Survey_Weight_NetValue = SaveData.SurveyReport.NetWt,
                    Survey_Weight_GrossValue = SaveData.SurveyReport.GrossWt,
                    Survey_Volume_TobePackedValue = SaveData.SurveyReport.TobePackedVol,
                    Survey_Weight_ACWTValue = SaveData.SurveyReport.ACWTWt,
                    SurveyLCLorFCL = SaveData.SurveyReport.LCLorFCL,
                    SurveyLooseCased = SaveData.SurveyReport.LooseCased,
                    Survey_ContainerID = SaveData.SurveyReport.ContainerSizeID
                },
                EnquiryDetail = new EnquiryDetail()
                {
                    EnqID = Convert.ToInt64(SaveData.EnqID),
                    EnqDetailID = Convert.ToInt64(SaveData.EnqDetailID),
                },
                ServiceListHidden = SaveData.SurveySOList.HFSOList,
                CostListHidden = SaveData.SurveyCostList.HFCostList,
                MoveId = SaveData.MoveJob.ModeID,
                RoadKMS = SaveData.SurveyReport.RoadKMS,
                StageRemark = SaveData.SurveyReport.StageRemarks,
            };
            try
            {
                return surveyBL.Insert(suveymodel, out result, SaveData.SurveyReport.SchSurveydate);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(LoginID), "MoveManageBL", "InsertSurvey", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }
        }

        public bool InsertPacking(MoveManageViewModel SaveData, int LoginID, out string result)
        {
            result = String.Empty;

            try
            {
                return moveMangeDAL.InsertPacking(SaveData, LoginID, out result);

            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(LoginID), "MoveManageBL", "InsertPacking", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

        }

        public bool InsertFreight(MoveManageViewModel SaveData, int LoginID, out string result)
        {
            result = String.Empty;

            try
            {
                return moveMangeDAL.InsertFreight(SaveData, LoginID, out result);

            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(LoginID), "MoveManageBL", "InsertFreight", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

        }

        public bool InsertEmailData(MoveManageViewModel SaveData, int LoginID, out string result)
        {
            result = String.Empty;

            try
            {
                return moveMangeDAL.InsertEmailData(SaveData, LoginID, out result);

            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(LoginID), "MoveManageBL", "InsertEmailData", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

        }

        public bool InsertDelivery(MoveManageViewModel SaveData, int LoginID, out string result)
        {
            result = String.Empty;

            try
            {
                return moveMangeDAL.InsertDelivery(SaveData, LoginID, out result);

            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(LoginID), "MoveManageBL", "InsertDelivery", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

        }

        public bool ApproveDelivery(MoveManageViewModel SaveData, bool IsApprove, int LoginID, out string result)
        {
            result = String.Empty;

            try
            {
                return moveMangeDAL.ApproveDelivery(SaveData, IsApprove, LoginID, out result);

            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(LoginID), "MoveManageBL", "ApproveDelivery", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

        }

        public bool ApprovePacking(MoveManageViewModel SaveData, bool IsApprove, int LoginID, out string result)
        {
            result = String.Empty;

            try
            {
                return moveMangeDAL.ApprovePacking(SaveData, IsApprove, LoginID, out result);

            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(LoginID), "MoveManageBL", "ApprovePacking", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

        }

        public bool ApproveSurvey(MoveManageViewModel SaveData, bool IsApprove, int LoginID, out string result)
        {
            result = String.Empty;

            try
            {
                return moveMangeDAL.ApproveSurvey(SaveData, IsApprove, LoginID, out result);

            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(LoginID), "MoveManageBL", "ApproveSurvey", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

        }

        public bool InsertMoveCost(MoveManageViewModel SaveData, int LoginID, out string result)
        {
            result = String.Empty;

            try
            {
                return moveMangeDAL.InsertMoveCost(SaveData, LoginID, out result);

            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(LoginID), "MoveManageBL", "InsertMoveCost", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

        }

        public bool InsertJobOpening(MoveManageViewModel SaveData, int LoginID, out string result)
        {
            result = String.Empty;

            try
            {
                var charges = new XElement("FixedCostDetails",
                    from emp in new List<RmcFees>()
                    select new XElement("FixedCostDetail",
                               new XElement("FixedCostID", emp.CostHeadId),
                               new XElement("Amt", emp.Amount),
                               new XElement("PercentVal", emp.Percent)
                           ));
                if (SaveData.RMCFees != null && SaveData.RMCFees.Count() > 0)
                {
                    charges = new XElement("FixedCostDetails",
                    from emp in SaveData.RMCFees
                    select new XElement("FixedCostDetail",
                               new XElement("FixedCostID", emp.CostHeadId),
                               new XElement("Amt", emp.Amount),
                               new XElement("PercentVal", emp.Percent)
                           ));
                }

                SaveData.CompanyID = UserSession.GetUserSession().CompanyID;
                SaveData.RMCBuss = UserSession.GetUserSession().BussinessLine != "NON RMC-BUSINESS";
                return moveMangeDAL.InsertJobOpening(SaveData, LoginID, charges, out result);

            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(LoginID), "MoveManageBL", "InsertJobOpening", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }
        }

        public IEnumerable<ExportSunJob> GetForSunGrid(DateTime? FromDate, DateTime? Todate, string sort, string sortdir, int skip, int pageSize, out int totalCount, bool IsWOS = false)
        {
            try
            {
                //RMCBuss = !UserSession.GetUserSession().BussinessLine.Equals("NON RMC-BUSINESS");

                IQueryable<ExportSunJob> surveyList = moveMangeDAL.GetForSunGrid(UserSession.GetUserSession().LoginID, FromDate, Todate, IsWOS);

                totalCount = surveyList.Count();
                if (!string.IsNullOrEmpty(sort))
                {
                    surveyList = surveyList.OrderBy(sort + " " + sortdir);
                }

                if (pageSize > 0)
                {
                    surveyList = surveyList.Skip(skip).Take(pageSize);
                }
                return surveyList.ToList();
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "MoveManageBL", "GetForGrid", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }
        }

        public IEnumerable<MoveManageGrid> GetForGrid(string search, string searchtype, string Shipper, string sort, string sortdir, int skip, int pageSize, out int totalCount, out bool RMCBuss)
        {
            try
            {
                RMCBuss = !UserSession.GetUserSession().BussinessLine.Equals("NON RMC-BUSINESS");

                IQueryable<MoveManageGrid> surveyList = moveMangeDAL.GetForGrid(UserSession.GetUserSession().LoginID, search, searchtype, Shipper, RMCBuss);

                totalCount = surveyList.Count();
                if (!string.IsNullOrEmpty(sort))
                {
                    surveyList = surveyList.OrderBy(sort + " " + sortdir);
                }

                if (pageSize > 0)
                {
                    surveyList = surveyList.Skip(skip).Take(pageSize);
                }
                return surveyList.ToList();
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "MoveManageBL", "GetForGrid", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }
        }

        public MoveManageViewModel GetDetailById(int SurveyID, int? MoveID, bool IsSurveyGetCost, bool IsPackingGetCost, bool IsDeliveryGetCost)
        {
            MoveManageViewModel CostObj = new MoveManageViewModel();
            try
            {
                CostObj.IsDestApprove = false;
                CostObj.DestApprove = CostObj.IsDestApprove ? "Approved" : "Pending";
                bool orgFlag = false, frtFlag = false, destFlag = false;
                int RMCBuss = 0;
                if (UserSession.GetUserSession().BussinessLine == "NON RMC-BUSINESS")
                {
                    RMCBuss = 0;
                }
                else
                {
                    RMCBuss = 1;
                }
                DataSet CostDs = moveMangeDAL.GetDetailById(UserSession.GetUserSession().LoginID, SurveyID, MoveID, RMCBuss, IsSurveyGetCost, IsPackingGetCost, IsDeliveryGetCost);
                if (CostDs != null && CostDs.Tables.Count >= 1)
                {
                    CostObj.MoveID = MoveID;
                    CostObj.SurveyID = SurveyID;
                    CostObj.IsSOCost = false;
                    CostObj.RMCBuss = RMCBuss == 1;


                    #region Job Opening Details
                    if (CostDs.Tables.Count > 0 && CostDs.Tables[0] != null && CostDs.Tables[0].Rows.Count > 0)
                    {
                        CostObj.SurveyID = CostDs.Tables[0].Rows[0]["SurveyID"] == DBNull.Value ? (Int64?)null : Convert.ToInt64(CostDs.Tables[0].Rows[0]["SurveyID"]);
                        CostObj.EnqID = CostDs.Tables[0].Rows[0]["EnqID"] == DBNull.Value ? (Int64?)null : Convert.ToInt64(CostDs.Tables[0].Rows[0]["EnqID"]);
                        CostObj.EnqNo = Convert.ToString(CostDs.Tables[0].Rows[0]["EnqNo"]);
                        CostObj.EnqShpNo = CostDs.Tables[0].Rows[0]["EnqSeqNo"] == DBNull.Value ? (Int64?)null : Convert.ToInt64(CostDs.Tables[0].Rows[0]["EnqSeqNo"]);
                        CostObj.ServiceLineID = CostDs.Tables[0].Rows[0]["ServiceLineID"] == DBNull.Value ? (Int32?)null : Convert.ToInt32(CostDs.Tables[0].Rows[0]["ServiceLineID"]);
                        CostObj.ServiceLine = Convert.ToString(CostDs.Tables[0].Rows[0]["ServiceLine"]);
                        CostObj.EnqDetailID = CostDs.Tables[0].Rows[0]["EnqDetailID"] == DBNull.Value ? (Int64?)null : Convert.ToInt64(CostDs.Tables[0].Rows[0]["EnqDetailID"]);
                        CostObj.MoveJob.WeightUnitID = Convert.ToInt32(CostDs.Tables[0].Rows[0]["WeightUnitID"]);
                        CostObj.MoveJob.WeightUnitFrom = Convert.ToInt64(CostDs.Tables[0].Rows[0]["WeightFrom"]);
                        CostObj.MoveJob.WeightUnitTo = Convert.ToInt64(CostDs.Tables[0].Rows[0]["WeightTo"]);
                        CostObj.MoveJob.FromLocationID = Convert.ToInt32(CostDs.Tables[0].Rows[0]["OrgCityID"]);
                        CostObj.MoveJob.ToLocationID = Convert.ToInt32(CostDs.Tables[0].Rows[0]["DestCityID"]);
                        CostObj.MoveJob.ExitPointID = CostDs.Tables[0].Rows[0]["OrgPortID"] == DBNull.Value ? 0 : Convert.ToInt32(CostDs.Tables[0].Rows[0]["OrgPortID"]);
                        CostObj.MoveJob.EntryPointID = CostDs.Tables[0].Rows[0]["DestPortID"] == DBNull.Value ? 0 : Convert.ToInt32(CostDs.Tables[0].Rows[0]["DestPortID"]);
                        CostObj.MoveJob.ModeID = Convert.ToInt32(CostDs.Tables[0].Rows[0]["ModeID"]);
                        CostObj.MoveJob.RMCID = Convert.ToInt32(CostDs.Tables[0].Rows[0]["RMCID"]);
                        CostObj.MoveJob.RMCName = Convert.ToString(CostDs.Tables[0].Rows[0]["RMCName"]);
                        CostObj.MoveJob.BusinessLineID = Convert.ToInt32(CostDs.Tables[0].Rows[0]["BussinessLineID"]);
                        CostObj.MoveJob.BusinessLineName = Convert.ToString(CostDs.Tables[0].Rows[0]["BussLineName"]);
                        CostObj.MoveJob.GoodsDescriptionID = Convert.ToInt32(CostDs.Tables[0].Rows[0]["GoodsDescID"]);
                        CostObj.MoveJob.ShipingLineID = CostDs.Tables[0].Rows[0]["ShipinglineID"] == DBNull.Value ? (int?)null : Convert.ToInt32(CostDs.Tables[0].Rows[0]["ShipinglineID"]);
                        CostObj.MoveJob.ShipmentTypeID = CostDs.Tables[0].Rows[0]["ShipmentTypeID"] == DBNull.Value ? 0 : Convert.ToInt32(CostDs.Tables[0].Rows[0]["ShipmentTypeID"]);
                        CostObj.JobNo = Convert.ToString(CostDs.Tables[0].Rows[0]["JobId"]);
                        CostObj.JobDate = DateTime.Now;//Convert.ToDateTime(CostDs.Tables[0].Rows[0]["JobDate"]);
                        CostObj.MoveJob.ModeName = Convert.ToString(CostDs.Tables[0].Rows[0]["ModeName"]);
                        CostObj.MoveJob.TentativeMoveDate = CostDs.Tables[0].Rows[0]["TendativeMoveDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(CostDs.Tables[0].Rows[0]["TendativeMoveDate"]);

                        /////Billing and collection paramters
                        CostObj.MoveJob.BillingOnClientId = CostDs.Tables[0].Rows[0]["billingonClientID"] == DBNull.Value ? (int?)null : Convert.ToInt32(CostDs.Tables[0].Rows[0]["billingonClientID"]);
                        CostObj.MoveJob.ClientId = CostDs.Tables[0].Rows[0]["AgentID"] == DBNull.Value ? (int?)0 : Convert.ToInt32(CostDs.Tables[0].Rows[0]["AgentID"]);
                        CostObj.MoveJob.AccountId = CostDs.Tables[0].Rows[0]["AccountId"] == DBNull.Value ? (int?)null : Convert.ToInt32(CostDs.Tables[0].Rows[0]["AccountId"]);
                        CostObj.MoveJob.Shipper.Title = Convert.ToString(CostDs.Tables[0].Rows[0]["ShipperTitle"]);
                        CostObj.MoveJob.Shipper.ShipperFName = Convert.ToString(CostDs.Tables[0].Rows[0]["ShipperFName"]);
                        CostObj.MoveJob.Shipper.ShipperLName = Convert.ToString(CostDs.Tables[0].Rows[0]["ShipperLName"]);
                        CostObj.MoveJob.Shipper.Address1 = Convert.ToString(CostDs.Tables[0].Rows[0]["Address1"]);
                        CostObj.MoveJob.Shipper.Address2 = Convert.ToString(CostDs.Tables[0].Rows[0]["Address2"]);
                        CostObj.MoveJob.Shipper.Email = Convert.ToString(CostDs.Tables[0].Rows[0]["Email"]);
                        CostObj.MoveJob.Shipper.AddressCityID = CostDs.Tables[0].Rows[0]["AddressCityId"] == DBNull.Value ? (int?)null : Convert.ToInt32(CostDs.Tables[0].Rows[0]["AddressCityId"]);
                        CostObj.MoveJob.Shipper.PIN = Convert.ToString(CostDs.Tables[0].Rows[0]["PIN"]);
                        CostObj.MoveJob.Shipper.Phone1 = Convert.ToString(CostDs.Tables[0].Rows[0]["Phone1"]);
                        CostObj.MoveJob.Shipper.Phone2 = Convert.ToString(CostDs.Tables[0].Rows[0]["Phone2"]);
                        CostObj.MoveJob.Shipper.ShipCategoryID = CostDs.Tables[0].Rows[0]["ShipCategoryID"] == DBNull.Value ? (int?)null : Convert.ToInt32(CostDs.Tables[0].Rows[0]["ShipCategoryID"]);
                        CostObj.MoveJob.Shipper.DOB = CostDs.Tables[0].Rows[0]["ShipperDOB"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(CostDs.Tables[0].Rows[0]["ShipperDOB"]);
                        CostObj.MoveJob.Shipper.Designation = Convert.ToString(CostDs.Tables[0].Rows[0]["ShipperDesig"]);
                        CostObj.MoveJob.Shipper.Nationality = Convert.ToString(CostDs.Tables[0].Rows[0]["ShipperNationality"]);
                        /////Copied the values of Job to Cost head
                        CostObj.MoveCostMst.WeightUnitID = Convert.ToInt32(CostDs.Tables[0].Rows[0]["WeightUnitID"]);
                        CostObj.MoveCostMst.WeightUnitFrom = Convert.ToInt64(CostDs.Tables[0].Rows[0]["WeightFrom"]);
                        CostObj.MoveCostMst.WeightUnitTo = Convert.ToInt64(CostDs.Tables[0].Rows[0]["WeightTo"]);
                        CostObj.MoveCostMst.FromLocationID = Convert.ToInt32(CostDs.Tables[0].Rows[0]["OrgCityID"]);
                        CostObj.MoveCostMst.ToLocationID = Convert.ToInt32(CostDs.Tables[0].Rows[0]["DestCityID"]);
                        CostObj.MoveCostMst.ExitPointID = CostDs.Tables[0].Rows[0]["OrgPortID"] == DBNull.Value ? 0 : Convert.ToInt32(CostDs.Tables[0].Rows[0]["OrgPortID"]);
                        CostObj.MoveCostMst.EntryPointID = CostDs.Tables[0].Rows[0]["DestPortID"] == DBNull.Value ? 0 : Convert.ToInt32(CostDs.Tables[0].Rows[0]["DestPortID"]);
                        CostObj.MoveCostMst.ModeID = Convert.ToInt32(CostDs.Tables[0].Rows[0]["ModeID"]);
                        CostObj.MoveCostMst.RMCID = Convert.ToInt32(CostDs.Tables[0].Rows[0]["RMCID"]);
                        CostObj.MoveCostMst.BusinessLineID = Convert.ToInt32(CostDs.Tables[0].Rows[0]["BussinessLineID"]);
                        CostObj.MoveCostMst.GoodsDescriptionID = Convert.ToInt32(CostDs.Tables[0].Rows[0]["GoodsDescID"]);
                        CostObj.MoveCostMst.ShipingLineID = CostDs.Tables[0].Rows[0]["ShipinglineID"] == DBNull.Value ? (int?)null : Convert.ToInt32(CostDs.Tables[0].Rows[0]["ShipinglineID"]);

                        CostObj.MoveJob.OrgAdd = CostDs.Tables[0].Rows[0]["OrgAdd"] == DBNull.Value ? null : Convert.ToString(CostDs.Tables[0].Rows[0]["OrgAdd"]);
                        CostObj.MoveJob.OrgAdd2 = CostDs.Tables[0].Rows[0]["OrgAdd2"] == DBNull.Value ? null : Convert.ToString(CostDs.Tables[0].Rows[0]["OrgAdd2"]);
                        CostObj.MoveJob.DestAdd = CostDs.Tables[0].Rows[0]["DestAdd"] == DBNull.Value ? null : Convert.ToString(CostDs.Tables[0].Rows[0]["DestAdd"]);
                        CostObj.MoveJob.DestAdd2 = CostDs.Tables[0].Rows[0]["DestAdd2"] == DBNull.Value ? null : Convert.ToString(CostDs.Tables[0].Rows[0]["DestAdd2"]);
                        CostObj.MoveJob.OrgCityID = CostDs.Tables[0].Rows[0]["SOrgCityID"] == DBNull.Value ? (int?)null : Convert.ToInt32(CostDs.Tables[0].Rows[0]["SOrgCityID"]);
                        CostObj.MoveJob.DestCityID = CostDs.Tables[0].Rows[0]["SDestCityID"] == DBNull.Value ? (int?)null : Convert.ToInt32(CostDs.Tables[0].Rows[0]["SDestCityID"]);
                        CostObj.MoveJob.OrgEmail = CostDs.Tables[0].Rows[0]["OrgEmail"] == DBNull.Value ? null : Convert.ToString(CostDs.Tables[0].Rows[0]["OrgEmail"]);
                        CostObj.MoveJob.DestEmail = CostDs.Tables[0].Rows[0]["DestEmail"] == DBNull.Value ? null : Convert.ToString(CostDs.Tables[0].Rows[0]["DestEmail"]);
                        CostObj.MoveJob.OrgPhone = CostDs.Tables[0].Rows[0]["OrgPhone"] == DBNull.Value ? null : Convert.ToString(CostDs.Tables[0].Rows[0]["OrgPhone"]);
                        CostObj.MoveJob.DestPhone = CostDs.Tables[0].Rows[0]["DestPhone"] == DBNull.Value ? null : Convert.ToString(CostDs.Tables[0].Rows[0]["DestPhone"]);
                        CostObj.MoveJob.OrgPin = CostDs.Tables[0].Rows[0]["OrgPin"] == DBNull.Value ? null : Convert.ToString(CostDs.Tables[0].Rows[0]["OrgPin"]);
                        CostObj.MoveJob.DestPin = CostDs.Tables[0].Rows[0]["DestPin"] == DBNull.Value ? null : Convert.ToString(CostDs.Tables[0].Rows[0]["DestPin"]);
                        CostObj.MoveJob.OrgAdd = CostDs.Tables[0].Rows[0]["OrgAdd"] == DBNull.Value ? null : Convert.ToString(CostDs.Tables[0].Rows[0]["OrgAdd"]);
                        CostObj.MoveJob.RMCFileNo = CostDs.Tables[0].Rows[0]["RMCFileNo"] == DBNull.Value ? null : Convert.ToString(CostDs.Tables[0].Rows[0]["RMCFileNo"]);
                        CostObj.MoveJob.WKNo = CostDs.Tables[0].Rows[0]["WorkOrderNo"] == DBNull.Value ? null : Convert.ToString(CostDs.Tables[0].Rows[0]["WorkOrderNo"]);
                        CostObj.MoveJob.MoveCoordinatorID = CostDs.Tables[0].Rows[0]["JobAssignedToID"] == DBNull.Value ? (int?)0 : Convert.ToInt32(CostDs.Tables[0].Rows[0]["JobAssignedToID"]);
                        CostObj.MoveJob.AssistingMoveCoordinatorID = CostDs.Tables[0].Rows[0]["JobAssitedToID"] == DBNull.Value ? (int?)0 : Convert.ToInt32(CostDs.Tables[0].Rows[0]["JobAssitedToID"]);
                        CostObj.UpdatedBatchId = CostDs.Tables[0].Rows[0]["UpdatedBatchID"] == DBNull.Value ? (int?)0 : Convert.ToInt32(CostDs.Tables[0].Rows[0]["UpdatedBatchID"]);
                        CostObj.CombinationID = CostDs.Tables[0].Rows[0]["CombinationID"] == DBNull.Value ? null : Convert.ToString(CostDs.Tables[0].Rows[0]["CombinationID"]);
                        CostObj.MoveJob.ContainerTypeID = CostDs.Tables[0].Rows[0]["ContainerTypeID"] == DBNull.Value ? null : (int?)Convert.ToInt32(CostDs.Tables[0].Rows[0]["ContainerTypeID"]);
                        CostObj.RMCType = CostDs.Tables[0].Rows[0]["RMCType"] == DBNull.Value ? null : Convert.ToString(CostDs.Tables[0].Rows[0]["RMCType"]);
                        CostObj.OldJobNo = CostDs.Tables[0].Rows[0]["OldCbsJobNo"] == DBNull.Value ? null : Convert.ToString(CostDs.Tables[0].Rows[0]["OldCbsJobNo"]);
                        CostObj.HideSurveySave = Convert.ToBoolean(CostDs.Tables[0].Rows[0]["HideSurveySave"]);
                        CostObj.InsurBy = CostDs.Tables[0].Rows[0]["InsurBy"] == DBNull.Value ? 0 : Convert.ToInt32(CostDs.Tables[0].Rows[0]["InsurBy"]);
                        CostObj.HoSdEmpID = CostDs.Tables[0].Rows[0]["HoSdEmpID"] == DBNull.Value ? (int?)0 : Convert.ToInt32(CostDs.Tables[0].Rows[0]["HoSdEmpID"]);
                        CostObj.BrSdEmpID = CostDs.Tables[0].Rows[0]["BrSdEmpID"] == DBNull.Value ? (int?)0 : Convert.ToInt32(CostDs.Tables[0].Rows[0]["BrSdEmpID"]);
                        CostObj.DestBrSdEmpID = CostDs.Tables[0].Rows[0]["DestBrSdEmpID"] == DBNull.Value ? (int?)0 : Convert.ToInt32(CostDs.Tables[0].Rows[0]["DestBrSdEmpID"]);
                        CostObj.Project = CostDs.Tables[0].Rows[0]["Project"] == DBNull.Value ? null : Convert.ToString(CostDs.Tables[0].Rows[0]["Project"]);
                        CostObj.MoveJob.RevenueBr = CostDs.Tables[0].Rows[0]["RevenueBr"] == DBNull.Value ? null : Convert.ToString(CostDs.Tables[0].Rows[0]["RevenueBr"]);
                        CostObj.MoveJob.ShowGetCost = Convert.ToBoolean(CostDs.Tables[0].Rows[0]["ShowGetCost"]);
                        CostObj.MoveJob.FromLocationName = Convert.ToString(CostDs.Tables[0].Rows[0]["OrgCityName"]);
                        CostObj.MoveJob.ToLocationName = Convert.ToString(CostDs.Tables[0].Rows[0]["DestCityName"]);
                        CostObj.JobStatus = Convert.ToString(CostDs.Tables[0].Rows[0]["JobStatus"]);
                        CostObj.JobCancel.CancelRemark = Convert.ToString(CostDs.Tables[0].Rows[0]["CancelRemark"]);
                        CostObj.ClientType = Convert.ToString(CostDs.Tables[0].Rows[0]["ClientType"]);

                        CostObj.Cheque_Amt = Convert.ToString(CostDs.Tables[0].Rows[0]["Cheque_Amt"]);
                        CostObj.Cheque_No = Convert.ToString(CostDs.Tables[0].Rows[0]["Cheque_No"]);
                        CostObj.Cheque_Remark = Convert.ToString(CostDs.Tables[0].Rows[0]["Cheque_Remark"]);
                        CostObj.SurveyerID = CostDs.Tables[0].Rows[0]["SchSurveyorID"] == DBNull.Value ? (int?)null : Convert.ToInt32(CostDs.Tables[0].Rows[0]["SchSurveyorID"]);
                        CostObj.ShowSendToMobile = Convert.ToInt16(CostDs.Tables[0].Rows[0]["ShowSendToMobile"]);
                        CostObj.ISDeliveryDateValid = Convert.ToBoolean(CostDs.Tables[0].Rows[0]["ISDeliveryDateValid"]);

                        //Insurance Details
                        CostObj.insuranceDetail.ContactPerson = CostDs.Tables[0].Rows[0]["InsContactPerson"] == DBNull.Value ? null : Convert.ToString(CostDs.Tables[0].Rows[0]["InsContactPerson"]);
                        CostObj.insuranceDetail.ContactNumber = CostDs.Tables[0].Rows[0]["InsContactNumber"] == DBNull.Value ? null : Convert.ToString(CostDs.Tables[0].Rows[0]["InsContactNumber"]);
                        CostObj.insuranceDetail.EmailID = CostDs.Tables[0].Rows[0]["InsEmailID"] == DBNull.Value ? null : Convert.ToString(CostDs.Tables[0].Rows[0]["InsEmailID"]);
                        CostObj.insuranceDetail.FinancePerson = CostDs.Tables[0].Rows[0]["InsFinancePerson"] == DBNull.Value ? null : Convert.ToString(CostDs.Tables[0].Rows[0]["InsFinancePerson"]);
                        CostObj.insuranceDetail.InsuranceValueAmount = CostDs.Tables[0].Rows[0]["InsuranceValueAmount"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(CostDs.Tables[0].Rows[0]["InsuranceValueAmount"]);
                        CostObj.insuranceDetail.InsuranceValueCurrency = CostDs.Tables[0].Rows[0]["InsuranceValueCurrency"] == DBNull.Value ? (int?)null : Convert.ToInt32(CostDs.Tables[0].Rows[0]["InsuranceValueCurrency"]);
                        CostObj.insuranceDetail.InsuranceBreakdown = CostDs.Tables[0].Rows[0]["InsuranceBreakdown"] == DBNull.Value ? 0 : Convert.ToInt32(CostDs.Tables[0].Rows[0]["InsuranceBreakdown"]);
                        CostObj.insuranceDetail.BreakdownInsurance = CostDs.Tables[0].Rows[0]["BreakdownInsuranceDMS"] == DBNull.Value ? 0 : Convert.ToInt32(CostDs.Tables[0].Rows[0]["BreakdownInsuranceDMS"]);


                        CostObj.IsShowCloseJob = Convert.ToBoolean(CostDs.Tables[0].Rows[0]["IsShowCloseJob"]);
                        CostObj.IsInvPrepared = Convert.ToBoolean(CostDs.Tables[0].Rows[0]["IsInvPrepared"]);
                        CostObj.IsGCCInsurance = Convert.ToBoolean(CostDs.Tables[0].Rows[0]["IsGCCInsurance"]);
                        CostObj.MoveJob.FinancePerson = CostDs.Tables[0].Rows[0]["FinancePerson"] == DBNull.Value ? (int?)null : Convert.ToInt32(CostDs.Tables[0].Rows[0]["FinancePerson"]);
                        CostObj.DefaultGPPercent = CostDs.Tables[0].Rows[0]["DefaultGPPercent"] == DBNull.Value ? 0 : Convert.ToDecimal(CostDs.Tables[0].Rows[0]["DefaultGPPercent"]);
                        CostObj.GPAmount = CostDs.Tables[0].Rows[0]["DefaultGPAmount"] == DBNull.Value ? 0 : Convert.ToDecimal(CostDs.Tables[0].Rows[0]["DefaultGPAmount"]);
                        CostObj.IsGPApproved = CostDs.Tables[0].Rows[0]["IsGPApproved"] == DBNull.Value ? true : Convert.ToBoolean(CostDs.Tables[0].Rows[0]["IsGPApproved"]);
                        CostObj.IsGPSendAppr = CostDs.Tables[0].Rows[0]["IsGPSendAppr"] == DBNull.Value ? true : Convert.ToBoolean(CostDs.Tables[0].Rows[0]["IsGPSendAppr"]);
                        CostObj.IsGPSendSD = CostDs.Tables[0].Rows[0]["IsGPSendSD"] == DBNull.Value ? true : Convert.ToBoolean(CostDs.Tables[0].Rows[0]["IsGPSendSD"]);
                        CostObj.IsGPProcess = CostDs.Tables[0].Rows[0]["IsGPProceed"] == DBNull.Value ? true : Convert.ToBoolean(CostDs.Tables[0].Rows[0]["IsGPProceed"]);
                        CostObj.GPRemark = CostDs.Tables[0].Rows[0]["GPRemark"] == DBNull.Value ? null : Convert.ToString(CostDs.Tables[0].Rows[0]["GPRemark"]);

                        CostObj.OrgWarehouse = CostDs.Tables[0].Rows[0]["OrgWarehouse"] == DBNull.Value ? (int?)null : Convert.ToInt32(CostDs.Tables[0].Rows[0]["OrgWarehouse"]);
                        CostObj.DestWarehouse = CostDs.Tables[0].Rows[0]["DestWarehouse"] == DBNull.Value ? (int?)null : Convert.ToInt32(CostDs.Tables[0].Rows[0]["DestWarehouse"]);
                        CostObj.IsOutSourced = CostDs.Tables[0].Rows[0]["OutSourced"] == DBNull.Value ? false : Convert.ToBoolean(CostDs.Tables[0].Rows[0]["OutSourced"]);
                        CostObj.IsDestWHSave = CostDs.Tables[0].Rows[0]["IsDestWHSave"] == DBNull.Value ? true : Convert.ToBoolean(CostDs.Tables[0].Rows[0]["IsDestWHSave"]);
                        CostObj.IsOrgWHSave = CostDs.Tables[0].Rows[0]["IsOrgWHSave"] == DBNull.Value ? true : Convert.ToBoolean(CostDs.Tables[0].Rows[0]["IsOrgWHSave"]);
                        CostObj.ISGDPRNationalty = CostDs.Tables[0].Rows[0]["ISGDPRNATIONALTY"] == DBNull.Value ? false : Convert.ToBoolean(CostDs.Tables[0].Rows[0]["ISGDPRNATIONALTY"]);
                        CostObj.IsShowSTGUnlock = CostDs.Tables[0].Rows[0]["IsShowSTGUnlock"] == DBNull.Value ? false : Convert.ToBoolean(CostDs.Tables[0].Rows[0]["IsShowSTGUnlock"]);
                        CostObj.IsSTGUnlock = CostDs.Tables[0].Rows[0]["IsSTGUnlock"] == DBNull.Value ? false : Convert.ToBoolean(CostDs.Tables[0].Rows[0]["IsSTGUnlock"]);
                    }
                    #endregion
                    CostObj.IsDTD = false;
                    if (CostDs.Tables.Count > 1 && CostDs.Tables[1] != null && CostDs.Tables[1].Rows.Count > 0)
                    {
                        #region AgentGrid Details and Survey Details
                        CostObj.MoveJob.MoveRateCompList = (from item in CostDs.Tables[1].AsEnumerable()
                                                            select new MoveRateComponent()
                                                            {
                                                                RateComponentID = item["RateCompID"] == DBNull.Value ? 0 : Convert.ToInt32(item["RateCompID"]),
                                                                AgentID = item["AgentID"] == DBNull.Value ? 0 : Convert.ToInt32(item["AgentID"]),
                                                                JobAgentID = item["JobAgentID"] == DBNull.Value ? 0 : Convert.ToInt32(item["JobAgentID"]),
                                                                RateComponentName = Convert.ToString(item["RateComponentName"]),
                                                                AgentName = Convert.ToString(item["AgentName"]),
                                                                JobAgentName = Convert.ToString(item["JobAgentName"]),
                                                                ActJobAgentName = Convert.ToString(item["AgentName"]),
                                                                ExitPortID = item["JobOrgPortID"] == DBNull.Value ? (item["OrgPortID"] == DBNull.Value ? CostDs.Tables[0] != null && CostDs.Tables[0].Rows.Count > 0 && CostDs.Tables[0].Rows[0]["OrgPortID"] == DBNull.Value ? 0 : Convert.ToInt32(CostDs.Tables[0].Rows[0]["OrgPortID"]) : Convert.ToInt32(item["OrgPortID"])) : Convert.ToInt32(item["JobOrgPortID"]),
                                                                EntryPortID = item["JobDestPortID"] == DBNull.Value ? (item["DestPortID"] == DBNull.Value ? CostDs.Tables[0] != null && CostDs.Tables[0].Rows.Count > 0 && CostDs.Tables[0].Rows[0]["DestPortID"] == DBNull.Value ? 0 : Convert.ToInt32(CostDs.Tables[0].Rows[0]["DestPortID"]) : Convert.ToInt32(item["DestPortID"])) : Convert.ToInt32(item["JobDestPortID"]),
                                                                ExitPort = item["JobOrgPortName"] == DBNull.Value ? Convert.ToString(CostDs.Tables[0] != null && CostDs.Tables[0].Rows.Count > 0 ? CostDs.Tables[0].Rows[0]["OrgPort"] : "") : Convert.ToString(item["JobOrgPortName"]),
                                                                EntryPort = item["JobDestPortName"] == DBNull.Value ? Convert.ToString(CostDs.Tables[0] != null && CostDs.Tables[0].Rows.Count > 0 ? CostDs.Tables[0].Rows[0]["DestPort"] : "") : Convert.ToString(item["JobDestPortName"]),
                                                                ActExitPort = item["OrgPortName"] == DBNull.Value ? Convert.ToString(CostDs.Tables[0] != null && CostDs.Tables[0].Rows.Count > 0 ? CostDs.Tables[0].Rows[0]["OrgPort"] : "") : Convert.ToString(item["OrgPortName"]),
                                                                ActEntryPort = item["DestPortName"] == DBNull.Value ? Convert.ToString(CostDs.Tables[0] != null && CostDs.Tables[0].Rows.Count > 0 ? CostDs.Tables[0].Rows[0]["DestPort"] : "") : Convert.ToString(item["DestPortName"])
                                                            }).ToList();
                        if (CostObj.RMCType == "Other Type" && CostDs.Tables[1].Select("RateCompID=1").Count() > 0 && CostDs.Tables[1].Select("RateCompID=2").Count() <= 0)
                        {
                            List<MoveRateComponent> frtRateComp = new List<MoveRateComponent>();
                            frtRateComp = (from item in CostDs.Tables[1].AsEnumerable()
                                           select new MoveRateComponent()
                                           {
                                               RateComponentID = item["RateCompID"] == DBNull.Value ? 0 : Convert.ToInt32(item["RateCompID"]),
                                               AgentID = item["AgentID"] == DBNull.Value ? 0 : Convert.ToInt32(item["AgentID"]),
                                               JobAgentID = item["JobAgentID"] == DBNull.Value ? 0 : Convert.ToInt32(item["JobAgentID"]),
                                               RateComponentName = Convert.ToString(item["RateComponentName"]),
                                               AgentName = Convert.ToString(item["AgentName"]),
                                               JobAgentName = Convert.ToString(item["JobAgentName"]),
                                               ActJobAgentName = Convert.ToString(item["AgentName"]),
                                               ExitPortID = item["JobOrgPortID"] == DBNull.Value ? (item["OrgPortID"] == DBNull.Value ? CostDs.Tables[0].Rows[0]["OrgPortID"] == DBNull.Value ? 0 : Convert.ToInt32(CostDs.Tables[0].Rows[0]["OrgPortID"]) : Convert.ToInt32(item["OrgPortID"])) : Convert.ToInt32(item["JobOrgPortID"]),
                                               EntryPortID = item["JobDestPortID"] == DBNull.Value ? (item["DestPortID"] == DBNull.Value ? CostDs.Tables[0].Rows[0]["DestPortID"] == DBNull.Value ? 0 : Convert.ToInt32(CostDs.Tables[0].Rows[0]["DestPortID"]) : Convert.ToInt32(item["DestPortID"])) : Convert.ToInt32(item["JobDestPortID"]),
                                               ExitPort = item["JobOrgPortName"] == DBNull.Value ? Convert.ToString(CostDs.Tables[0].Rows[0]["OrgPort"]) : Convert.ToString(item["JobOrgPortName"]),
                                               EntryPort = item["JobDestPortName"] == DBNull.Value ? Convert.ToString(CostDs.Tables[0].Rows[0]["DestPort"]) : Convert.ToString(item["JobDestPortName"]),
                                               ActExitPort = item["OrgPortName"] == DBNull.Value ? Convert.ToString(CostDs.Tables[0].Rows[0]["OrgPort"]) : Convert.ToString(item["OrgPortName"]),
                                               ActEntryPort = item["DestPortName"] == DBNull.Value ? Convert.ToString(CostDs.Tables[0].Rows[0]["DestPort"]) : Convert.ToString(item["DestPortName"])
                                           }).ToList();
                            frtRateComp.First().RateComponentID = 2; frtRateComp.First().RateComponentName = "Freight";
                            CostObj.MoveJob.MoveRateCompList.AddRange(frtRateComp);
                        }
                        CostObj.IsDTD = CostObj.MoveJob.MoveRateCompList.Count == 1;

                        //CostObj.MoveJob.MoveRateCompList
                        #endregion

                        #region Survey Details
                        DataRow[] SurveyDataRow = CostDs.Tables[1].Select("RateCompID=1 OR RateCompID=4");
                        DataTable SurveyData = SurveyDataRow.Length > 0 ? SurveyDataRow.CopyToDataTable() : new DataTable();
                        if (SurveyData.Rows.Count > 0 && SurveyData != null)
                        {
                            CostObj.SurveyDetail.OrgAgentID = SurveyData.Rows[0]["JobAgentID"] == DBNull.Value ? (int?)null : Convert.ToInt32(SurveyData.Rows[0]["JobAgentID"]);
                            CostObj.SurveyDetail.ExitPortID = SurveyData.Rows[0]["JobOrgPortID"] == DBNull.Value ? (int?)null : Convert.ToInt32(SurveyData.Rows[0]["JobOrgPortID"]);
                        }

                        SurveyDataRow = CostDs.Tables[1].Select("RateCompID=2 OR RateCompID=4");
                        SurveyData = SurveyDataRow.Length > 0 ? SurveyDataRow.CopyToDataTable() : new DataTable();
                        if (SurveyData.Rows.Count > 0 && SurveyData != null)
                        {
                            CostObj.SurveyDetail.ExitPortID = SurveyData.Rows[0]["JobOrgPortID"] == DBNull.Value ? (int?)null : Convert.ToInt32(SurveyData.Rows[0]["JobOrgPortID"]);
                            CostObj.SurveyDetail.EntryPortID = SurveyData.Rows[0]["JobDestPortID"] == DBNull.Value ? (int?)null : Convert.ToInt32(SurveyData.Rows[0]["JobDestPortID"]);
                            if (CostObj.MoveJob.ModeName.ToUpper() == "ROAD" && CostObj.RMCType == "Brookfield Type")
                            {
                                CostObj.SurveyDetail.FrtAgentID = SurveyData.Rows[0]["JobAgentID"] == DBNull.Value ? (int?)null : Convert.ToInt32(SurveyData.Rows[0]["JobAgentID"]);
                            }

                        }

                        SurveyDataRow = CostDs.Tables[1].Select("RateCompID=3 OR RateCompID=4");
                        SurveyData = SurveyDataRow.Length > 0 ? SurveyDataRow.CopyToDataTable() : new DataTable();
                        if (SurveyData.Rows.Count > 0 && SurveyData != null)
                        {
                            CostObj.SurveyDetail.EntryPortID = SurveyData.Rows[0]["JobDestPortID"] == DBNull.Value ? (int?)null : Convert.ToInt32(SurveyData.Rows[0]["JobDestPortID"]);
                            CostObj.SurveyDetail.DestAgentID = SurveyData.Rows[0]["JobAgentID"] == DBNull.Value ? (int?)null : Convert.ToInt32(SurveyData.Rows[0]["JobAgentID"]);
                        }

                        if (!(CostObj.MoveJob.ModeName.ToUpper() == "ROAD" && CostObj.RMCType == "Brookfield Type"))
                        {
                            CostObj.SurveyDetail.FrtAgentID = CostObj.SurveyDetail.FrtAgentID != null ? CostObj.SurveyDetail.FrtAgentID : CostObj.SurveyDetail.OrgAgentID;
                        }
                        #endregion
                    }

                    #region Survey Report
                    if (CostDs.Tables.Count > 2 && CostDs.Tables[2] != null && CostDs.Tables[2].Rows.Count > 0)
                    {
                        CostObj.SurveyDetail.IsDone = true;

                        CostObj.SurveyReport.SurveyorID = CostDs.Tables[2].Rows[0]["SurveyerID"] == DBNull.Value ? (int?)null : Convert.ToInt32(CostDs.Tables[2].Rows[0]["SurveyerID"]);
                        CostObj.SurveyReport.Surveydate = CostDs.Tables[2].Rows[0]["SurveyDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(CostDs.Tables[2].Rows[0]["SurveyDate"]);
                        CostObj.SurveyReport.SchSurveydate = CostDs.Tables[2].Rows[0]["SchSurveyDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(CostDs.Tables[2].Rows[0]["SchSurveyDate"]);
                        CostObj.IsSOCost = CostDs.Tables[2].Rows[0]["SurveyDate"] == DBNull.Value ? false : true;
                        CostObj.SurveyReport.SurveyDateTime = CostDs.Tables[2].Rows[0]["SurveryTime"] == DBNull.Value ? (TimeSpan?)null : (TimeSpan)CostDs.Tables[2].Rows[0]["SurveryTime"];
                        CostObj.SurveyReport.Packdate = CostDs.Tables[2].Rows[0]["PackDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(CostDs.Tables[2].Rows[0]["PackDate"]); //(DateTime?)CostDs.Tables[2].Rows[0]["PackDate"];
                        CostObj.SurveyReport.Loaddate = CostDs.Tables[2].Rows[0]["LoadDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(CostDs.Tables[2].Rows[0]["LoadDate"]); //(DateTime?)CostDs.Tables[2].Rows[0]["LoadDate"];
                        CostObj.SurveyReport.DensityFact = CostDs.Tables[2].Rows[0]["DensityFact"] == DBNull.Value ? (decimal)0.00 : Convert.ToDecimal(CostDs.Tables[2].Rows[0]["DensityFact"]);
                        CostObj.SurveyReport.VolumeUnitID = CostDs.Tables[2].Rows[0]["VolumeUnitID"] == DBNull.Value ? 0 : Convert.ToInt32(CostDs.Tables[2].Rows[0]["VolumeUnitID"]);
                        CostObj.SurveyReport.WeightUnitID = CostDs.Tables[2].Rows[0]["WeightUnitID"] == DBNull.Value ? 0 : Convert.ToInt32(CostDs.Tables[2].Rows[0]["WeightUnitID"]);
                        CostObj.SurveyReport.TobePackedVol = CostDs.Tables[2].Rows[0]["TobePackedVol"] == DBNull.Value ? (decimal)0.00 : Convert.ToDecimal(CostDs.Tables[2].Rows[0]["TobePackedVol"]);
                        CostObj.SurveyReport.NetVol = CostDs.Tables[2].Rows[0]["NetVol"] == DBNull.Value ? (decimal)0.00 : Convert.ToDecimal(CostDs.Tables[2].Rows[0]["NetVol"]);
                        CostObj.SurveyReport.GrossVol = CostDs.Tables[2].Rows[0]["GrossVol"] == DBNull.Value ? (decimal)0.00 : Convert.ToDecimal(CostDs.Tables[2].Rows[0]["GrossVol"]);
                        CostObj.SurveyReport.NetWt = CostDs.Tables[2].Rows[0]["NetWt"] == DBNull.Value ? (decimal)0.00 : Convert.ToDecimal(CostDs.Tables[2].Rows[0]["NetWt"]);
                        CostObj.SurveyReport.GrossWt = CostDs.Tables[2].Rows[0]["GrossWt"] == DBNull.Value ? (decimal)0.00 : Convert.ToDecimal(CostDs.Tables[2].Rows[0]["GrossWt"]);
                        CostObj.SurveyReport.ACWTWt = CostDs.Tables[2].Rows[0]["ACWTWt"] == DBNull.Value ? (decimal)0.00 : Convert.ToDecimal(CostDs.Tables[2].Rows[0]["ACWTWt"]);
                        CostObj.SurveyReport.ContainerSizeID = CostDs.Tables[2].Rows[0]["ContainerSizeID"] == DBNull.Value ? 0 : Convert.ToInt32(CostDs.Tables[2].Rows[0]["ContainerSizeID"]);
                        CostObj.SurveyReport.RoadKMS = CostDs.Tables[2].Rows[0]["RoadKMS"] == DBNull.Value ? (decimal)0.00 : Convert.ToDecimal(CostDs.Tables[2].Rows[0]["RoadKMS"]);

                        CostObj.SurveyReport.IsCSSenttoApprove = CostDs.Tables[2].Rows[0]["IsSendToApproval"] == DBNull.Value ? false : Convert.ToBoolean(CostDs.Tables[2].Rows[0]["IsSendToApproval"]);
                        CostObj.SurveyReport.CSSenttoApproveUser = CostDs.Tables[2].Rows[0]["ApprovalUserID"] == DBNull.Value ? (Int32?)0 : Convert.ToInt32(CostDs.Tables[2].Rows[0]["ApprovalUserID"]);

                        CostObj.SurveyReport.CSCreatedBY = CostDs.Tables[2].Rows[0]["CSPreparedBy"] == DBNull.Value ? null : Convert.ToString(CostDs.Tables[2].Rows[0]["CSPreparedBy"]);
                        CostObj.SurveyReport.CSCreatedDate = CostDs.Tables[2].Rows[0]["CSPreparedDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(CostDs.Tables[2].Rows[0]["CSPreparedDate"]);
                        CostObj.SurveyReport.CSApprovedBY = CostDs.Tables[2].Rows[0]["CSApprovedBy"] == DBNull.Value ? null : Convert.ToString(CostDs.Tables[2].Rows[0]["CSApprovedBy"]);
                        CostObj.SurveyReport.CSApprovedDate = CostDs.Tables[2].Rows[0]["CSApprovedDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(CostDs.Tables[2].Rows[0]["CSApprovedDate"]);

                        CostObj.SurveyReport.StageRemarks = CostDs.Tables[2].Rows[0]["StageRemark"] == DBNull.Value ? null : Convert.ToString(CostDs.Tables[2].Rows[0]["StageRemark"]);

                        if (CostObj.MoveJob.ModeID != 2)
                        {
                            CostObj.SurveyReport.LCLorFCL = CostDs.Tables[2].Rows[0]["LCLorFCL"] == DBNull.Value ? null : Convert.ToString(CostDs.Tables[2].Rows[0]["LCLorFCL"]);
                            CostObj.SurveyReport.LooseCased = CostDs.Tables[2].Rows[0]["LooseCased"] == DBNull.Value ? null : Convert.ToString(CostDs.Tables[2].Rows[0]["LooseCased"]);
                        }
                        else
                        {
                            CostObj.SurveyReport.LCLorFCL = CostDs.Tables[2].Rows[0]["LCLorFCL"] == DBNull.Value ? "LCL" : Convert.ToString(CostDs.Tables[2].Rows[0]["LCLorFCL"]);
                            CostObj.SurveyReport.LooseCased = CostDs.Tables[2].Rows[0]["LooseCased"] == DBNull.Value ? "Loose" : Convert.ToString(CostDs.Tables[2].Rows[0]["LooseCased"]);
                        }

                    }
                    else
                    {
                        CostObj.SurveyReport.LooseCased = "Loose";
                    }
                    #endregion

                    #region Survey SO
                    if (CostDs.Tables.Count > 3 && CostDs.Tables[3] != null && CostDs.Tables[3].Rows.Count > 0)
                    {
                        CostObj.SurveySOList.SOList = (from item in CostDs.Tables[3].AsEnumerable()
                                                       select new PackingSOList
                                                       {
                                                           SurveyId = item["SurveyId"] == DBNull.Value ? (int?)0 : (Convert.ToInt32(item["SurveyId"])),
                                                           SurveyDetailId = item["SurveyDetailsId"] == DBNull.Value ? (int?)0 : (Convert.ToInt32(item["SurveyDetailsId"])),

                                                           CostHeadID = item["CostHeadID"] == DBNull.Value ? (int?)0 : (Convert.ToInt32(item["CostHeadID"])),

                                                           CostHeadName = Convert.ToString(item["CostHeadName"]),
                                                           Remark = Convert.ToString(item["RemarksOnCostHead"]),

                                                           RateCompId = item["MoveCompId"] == DBNull.Value ? (int)0 : (Convert.ToInt32(item["MoveCompId"])),

                                                           RateCompName = Convert.ToString(item["RateComponentName"]),
                                                           Volume = Convert.ToString(item["WtVolume"]),
                                                           WtUnitID = item["WtUnitID"] == DBNull.Value ? (int)0 : (Convert.ToInt32(item["WtUnitID"])),

                                                           WtUnit = Convert.ToString(item["WeightUnitName"]),
                                                           ExpectedCost = item["ExpectedCost"] == DBNull.Value ? (decimal?)0 : (Convert.ToDecimal(item["ExpectedCost"])),

                                                           Isactive = true,

                                                           /*ConversionRate = item["BaseCurrConversRate"] == DBNull.Value ? 0 : Convert.ToDecimal(item["BaseCurrConversRate"]),
                                                           Amount = item["RateCurrValue"]==DBNull.Value ? 0 :  Convert.ToDecimal(item["RateCurrValue"]),*/
                                                       }).ToList();
                    }
                    #endregion

                    #region Survey Cost
                    if (CostDs.Tables.Count > 4 && CostDs.Tables[4] != null && CostDs.Tables[4].Rows.Count > 0)
                    {
                        CostObj.SurveyCostList.CostListSaved = Convert.ToString(CostDs.Tables[4].Rows[0]["IsSurveyCostSaved"]) == "Y";
                        CostObj.SurveyCostList.CostList = (from item in CostDs.Tables[4].AsEnumerable()
                                                           select new PackingCostList
                                                           {
                                                               RateCompId = item["MoveCompID"] == DBNull.Value ? (int)0 : (Convert.ToInt32(item["MoveCompID"])),
                                                               RateCompName = Convert.ToString(item["RateComponentName"]),

                                                               CostHeadID = item["CostHeadID"] == DBNull.Value ? (int?)0 : (Convert.ToInt32(item["CostHeadID"])),

                                                               CostHeadName = Convert.ToString(item["CostHeadName"]),
                                                               BaseCurrID = item["BaseCurrID"] == DBNull.Value ? (int)0 : (Convert.ToInt32(item["BaseCurrID"])),

                                                               RateCurrID = item["RateCurrID"] == DBNull.Value ? (int)0 : (Convert.ToInt32(item["RateCurrID"])),
                                                               RevRateCurrID = item["RevRateCurrID"] == DBNull.Value ? (int)0 : (Convert.ToInt32(item["RevRateCurrID"])),
                                                               ConversionRate = item["ConversionRate"] == DBNull.Value ? (decimal?)0 : (Convert.ToDecimal(item["ConversionRate"])),
                                                               RevConversionRate = item["RevConversionRate"] == DBNull.Value ? (decimal?)0 : (Convert.ToDecimal(item["RevConversionRate"])),
                                                               CostValue = item["CostValue"] == DBNull.Value ? (decimal?)0 : (Convert.ToDecimal(item["CostValue"])),
                                                               BaseRevenueValue = item["BaseRev"] == DBNull.Value ? (decimal?)0 : (Convert.ToDecimal(item["BaseRev"])),
                                                               RevenueValue = item["Revenuevalue"] == DBNull.Value ? (decimal?)0 : (Convert.ToDecimal(item["Revenuevalue"])),
                                                               Balance = item["Revenuevalue"] == DBNull.Value ? (decimal?)0 : (Convert.ToDecimal(item["Revenuevalue"])),
                                                               BaseCurr = Convert.ToString(item["BaseCurrName"]),
                                                               RateCurr = Convert.ToString(item["RateCurrName"]),
                                                               RevRateCurr = Convert.ToString(item["RevRateCurrName"]),
                                                               Per = item["Per"] == DBNull.Value ? 0 : (Convert.ToInt32(item["Per"])),
                                                               WtVol = item["Wt_Vol_No"] == DBNull.Value ? "0" : (Convert.ToString(item["Wt_Vol_No"])),
                                                               //Per = item["Per"] == DBNull.Value ? 0 : (Convert.ToInt32(item["Per"])),
                                                               Isactive = true,//item["Isactive"] == DBNull.Value ? (bool)false : (Convert.ToBoolean(item["Isactive"])),

                                                               /*ConversionRate = item["BaseCurrConversRate"] == DBNull.Value ? 0 : Convert.ToDecimal(item["BaseCurrConversRate"]),
                                                               Amount = item["RateCurrValue"]==DBNull.Value ? 0 :  Convert.ToDecimal(item["RateCurrValue"]),*/
                                                               Rate = item["Rate"] == DBNull.Value ? 0 : Convert.ToDecimal(item["Rate"]),
                                                               RevRate = item["RevRate"] == DBNull.Value ? 0 : Convert.ToDecimal(item["RevRate"])
                                                               //Rate = Convert.ToDecimal(item["CostValue"]) * Convert.ToDecimal(item["Per"]) / 
                                                               //(Convert.ToDecimal(Convert.ToDecimal(item["ConversionRate"]) * Convert.ToDecimal(item["Wt_Vol_No"])))
                                                           }).ToList();
                        /*CostObj.BaseCurrID = CostObj.SurveyCostList.CostList.First().BaseCurrID;
						CostObj.BaseCurr = CostObj.SurveyCostList.CostList.First().BaseCurr;*/
                        CostObj.SurveyReport.IsApprove = Convert.ToBoolean(CostDs.Tables[2].Rows[0]["IsCostSheetApproved"]);
                        CostObj.SurveyReport.ApproveTitle = CostObj.SurveyReport.IsApprove ? "Approved" : "Pending";
                        
                    }
                    #endregion

                    #region Packing Info
                    if (CostDs.Tables.Count > 5 && CostDs.Tables[5] != null && CostDs.Tables[5].Rows.Count > 0 && CostDs.Tables[5].Rows[0][0].ToString() == "Same as survey details")
                    {
                        orgFlag = false;
                        CostObj.PackingDetail.OrgAgentID = CostObj.SurveyDetail.OrgAgentID;
                        CostObj.PackingDetail.FrtAgentID = CostObj.SurveyDetail.FrtAgentID;
                        CostObj.PackingDetail.DestAgentID = CostObj.SurveyDetail.DestAgentID;
                        CostObj.PackingDetail.ExitPortID = CostObj.SurveyDetail.ExitPortID;
                        CostObj.PackingDetail.EntryPortID = CostObj.SurveyDetail.EntryPortID;

                        /*CostObj.PackingReport.Packdate = CostObj.SurveyReport.Packdate;
                        CostObj.PackingReport.Loaddate = CostObj.SurveyReport.Loaddate;
                        CostObj.PackingReport.DensityFact = CostObj.SurveyReport.DensityFact;
                        CostObj.PackingReport.VolumeUnitID = CostObj.SurveyReport.VolumeUnitID;
                        CostObj.PackingReport.WeightUnitID = CostObj.SurveyReport.WeightUnitID;
                        CostObj.PackingReport.TobePackedVol = CostObj.SurveyReport.TobePackedVol;
                        CostObj.PackingReport.NetVol = CostObj.SurveyReport.NetVol;
                        CostObj.PackingReport.GrossVol = CostObj.SurveyReport.GrossVol;
                        CostObj.PackingReport.NetWt = CostObj.SurveyReport.NetWt;
                        CostObj.PackingReport.GrossWt = CostObj.SurveyReport.GrossWt;
                        CostObj.PackingReport.ACWTWt = CostObj.SurveyReport.ACWTWt;*/

                        CostObj.PackingSOList.SOList = CostObj.SurveySOList.SOList;

                        CostObj.PackingCostList.CostList = CostObj.SurveyCostList.CostList;
                    }
                    else if (CostDs.Tables.Count > 5 && CostDs.Tables[5] != null /*&& CostDs.Tables[5].Rows.Count > 0*/)
                    {

                        orgFlag = true;
                        #region Packing Details
                        if (CostDs.Tables.Count > 5 && CostDs.Tables[5] != null && CostDs.Tables[5].Rows.Count > 0)
                        {
                            CostObj.PackingDetail.IsDone = true;
                            CostObj.PackingDetail.OrgAgentID = CostDs.Tables[5].Rows[0]["OrgAgentID"] == DBNull.Value ? (int?)null : Convert.ToInt32(CostDs.Tables[5].Rows[0]["OrgAgentID"]);
                            CostObj.PackingDetail.OrgAgentName = Convert.ToString(CostDs.Tables[5].Rows[0]["OrgAgentName"]);
                            CostObj.PackingDetail.FrtAgentID = CostDs.Tables[5].Rows[0]["FrtAgentID"] == DBNull.Value ? (int?)null : Convert.ToInt32(CostDs.Tables[5].Rows[0]["FrtAgentID"]);
                            CostObj.PackingDetail.DestAgentID = CostDs.Tables[5].Rows[0]["DestAgentID"] == DBNull.Value ? (int?)null : Convert.ToInt32(CostDs.Tables[5].Rows[0]["DestAgentID"]);
                            CostObj.PackingDetail.DestAgentName = Convert.ToString(CostDs.Tables[5].Rows[0]["DestAgentName"]);
                            CostObj.PackingDetail.ExitPortID = CostDs.Tables[5].Rows[0]["ExitPortID"] == DBNull.Value ? (int?)null : Convert.ToInt32(CostDs.Tables[5].Rows[0]["ExitPortID"]);
                            CostObj.PackingDetail.ExitPortName = Convert.ToString(CostDs.Tables[5].Rows[0]["ExitPortName"]);
                            CostObj.PackingDetail.EntryPortID = CostDs.Tables[5].Rows[0]["EntryPortID"] == DBNull.Value ? (int?)null : Convert.ToInt32(CostDs.Tables[5].Rows[0]["EntryPortID"]);
                            CostObj.PackingDetail.EntryPortName = Convert.ToString(CostDs.Tables[5].Rows[0]["EntryPortName"]);



                            if (CostObj.MoveJob.ModeID != 3)
                            {
                                if (!string.IsNullOrEmpty(Convert.ToString(CostDs.Tables[5].Rows[0]["DestAgentName"])) &&
                                CostObj.MoveJob.MoveRateCompList.Where(x => x.RateComponentID == 3).Count() <= 0)
                                {
                                    CostObj.MoveJob.MoveRateCompList.Add(new MoveRateComponent() { RateComponentID = 3, RateComponentName = "Destination" });
                                }
                                if (!string.IsNullOrEmpty(Convert.ToString(CostDs.Tables[5].Rows[0]["OrgAgentName"])) &&
                                    CostObj.MoveJob.MoveRateCompList.Where(x => x.RateComponentID == 1).Count() <= 0)
                                {

                                    CostObj.MoveJob.MoveRateCompList.Add(new MoveRateComponent() { RateComponentID = 1, RateComponentName = "Origin" });
                                    CostObj.MoveJob.MoveRateCompList.Add(new MoveRateComponent() { RateComponentID = 2, RateComponentName = "Freight" });
                                }

                                CostObj.MoveJob.MoveRateCompList = CostObj.MoveJob.MoveRateCompList.Select(i =>
                                {
                                    if (i.RateComponentID == 1) i.ActJobAgentName = Convert.ToString(CostDs.Tables[5].Rows[0]["OrgAgentName"]);
                                    if (i.RateComponentID == 1) i.ActJobAgentID = Convert.ToString(CostDs.Tables[5].Rows[0]["OrgAgentID"]);
                                    if (i.RateComponentID == 2) i.ActJobAgentName = Convert.ToString(CostDs.Tables[5].Rows[0]["OrgAgentName"]);
                                    if (i.RateComponentID == 2) i.ActJobAgentID = Convert.ToString(CostDs.Tables[5].Rows[0]["OrgAgentID"]);
                                    if (i.RateComponentID == 3) i.ActJobAgentName = Convert.ToString(CostDs.Tables[5].Rows[0]["DestAgentName"]);
                                    if (i.RateComponentID == 3) i.ActJobAgentID = Convert.ToString(CostDs.Tables[5].Rows[0]["DestAgentID"]);
                                    return i;
                                }).ToList();

                            }
                            else
                            {
                                if (CostObj.MoveJob.MoveRateCompList.Count == 1)
                                {
                                    CostObj.MoveJob.MoveRateCompList = CostObj.MoveJob.MoveRateCompList.Select(i =>
                                    {
                                        if (i.RateComponentID == 4) i.ActJobAgentName = Convert.ToString(CostDs.Tables[5].Rows[0]["OrgAgentName"]);
                                        if (i.RateComponentID == 4) i.ActJobAgentID = Convert.ToString(CostDs.Tables[5].Rows[0]["OrgAgentID"]);
                                        //if (i.RateComponentID == 2) i.ActJobAgentName = Convert.ToString(CostDs.Tables[5].Rows[0]["OrgAgentName"]);
                                        //if (i.RateComponentID == 2) i.ActJobAgentID = Convert.ToString(CostDs.Tables[5].Rows[0]["OrgAgentID"]);
                                        //if (i.RateComponentID == 3) i.ActJobAgentName = Convert.ToString(CostDs.Tables[5].Rows[0]["DestAgentName"]);
                                        //if (i.RateComponentID == 3) i.ActJobAgentID = Convert.ToString(CostDs.Tables[5].Rows[0]["DestAgentID"]);
                                        return i;
                                    }).ToList();
                                }
                                else
                                {
                                    if (!string.IsNullOrEmpty(Convert.ToString(CostDs.Tables[5].Rows[0]["DestAgentName"])) &&
                                CostObj.MoveJob.MoveRateCompList.Where(x => x.RateComponentID == 3).Count() <= 0)
                                    {
                                        CostObj.MoveJob.MoveRateCompList.Add(new MoveRateComponent() { RateComponentID = 3, RateComponentName = "Destination" });
                                    }
                                    if (!string.IsNullOrEmpty(Convert.ToString(CostDs.Tables[5].Rows[0]["OrgAgentName"])) &&
                                        CostObj.MoveJob.MoveRateCompList.Where(x => x.RateComponentID == 1).Count() <= 0)
                                    {

                                        CostObj.MoveJob.MoveRateCompList.Add(new MoveRateComponent() { RateComponentID = 1, RateComponentName = "Origin" });
                                        CostObj.MoveJob.MoveRateCompList.Add(new MoveRateComponent() { RateComponentID = 2, RateComponentName = "Freight" });
                                    }

                                    CostObj.MoveJob.MoveRateCompList = CostObj.MoveJob.MoveRateCompList.Select(i =>
                                    {
                                        if (i.RateComponentID == 1) i.ActJobAgentName = Convert.ToString(CostDs.Tables[5].Rows[0]["OrgAgentName"]);
                                        if (i.RateComponentID == 1) i.ActJobAgentID = Convert.ToString(CostDs.Tables[5].Rows[0]["OrgAgentID"]);
                                        if (i.RateComponentID == 2) i.ActJobAgentName = Convert.ToString(CostDs.Tables[5].Rows[0]["FrtAgentName"]);
                                        if (i.RateComponentID == 2) i.ActJobAgentID = Convert.ToString(CostDs.Tables[5].Rows[0]["FrtAgentID"]);
                                        if (i.RateComponentID == 3) i.ActJobAgentName = Convert.ToString(CostDs.Tables[5].Rows[0]["DestAgentName"]);
                                        if (i.RateComponentID == 3) i.ActJobAgentID = Convert.ToString(CostDs.Tables[5].Rows[0]["DestAgentID"]);
                                        return i;
                                    }).ToList();
                                }

                            }


                        }
                        #endregion

                        #region Packing SO
                        if (CostDs.Tables.Count > 6 && CostDs.Tables[6] != null && CostDs.Tables[6].Rows.Count > 0)
                        {
                            CostObj.PackingSOList.SOList = (from item in CostDs.Tables[6].AsEnumerable()
                                                            select new PackingSOList
                                                            {
                                                                CostHeadID = item["CostHeadID"] == DBNull.Value ? (int?)0 : (Convert.ToInt32(item["CostHeadID"])),
                                                                CostHeadName = Convert.ToString(item["CostHeadName"]),
                                                                Remark = Convert.ToString(item["ServOrderRemarks"]),
                                                                RateCompId = Convert.ToInt32(RELOCBS.Common.CommonService.RateComp.Origin),
                                                                Volume = Convert.ToString(item["WtVolume"]),
                                                                WtUnitID = item["WtUnitID"] == DBNull.Value ? (int)0 : (Convert.ToInt32(item["WtUnitID"])),
                                                                WtUnit = Convert.ToString(item["WeightUnitName"]),
                                                                Isactive = true//item["Isactive"] == DBNull.Value ? (bool)false : (Convert.ToBoolean(item["Isactive"])),
                                                            }).ToList();
                        }
                        #endregion

                        #region Packing Report
                        if (CostDs.Tables.Count > 7 && CostDs.Tables[7] != null && CostDs.Tables[7].Rows.Count > 0)
                        {
                            CostObj.PackingReport.Packdate = CostDs.Tables[7].Rows[0]["PackDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(CostDs.Tables[7].Rows[0]["PackDate"]); //(DateTime?)CostDs.Tables[7].Rows[0]["PackDate"];
                            CostObj.PackingReport.ScheduledPackDate = CostDs.Tables[7].Rows[0]["SchPackDate"] == DBNull.Value ? CostObj.SurveyReport.Packdate : Convert.ToDateTime(CostDs.Tables[7].Rows[0]["SchPackDate"]); //(DateTime?)CostDs.Tables[7].Rows[0]["PackDate"];
                            CostObj.PackingReport.Loaddate = CostDs.Tables[7].Rows[0]["LoadDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(CostDs.Tables[7].Rows[0]["LoadDate"]); //(DateTime?)CostDs.Tables[7].Rows[0]["LoadDate"];
                            CostObj.PackingReport.DensityFact = CostDs.Tables[7].Rows[0]["DensityFact"] == DBNull.Value ? (decimal)0.00 : Convert.ToDecimal(CostDs.Tables[7].Rows[0]["DensityFact"]);
                            CostObj.PackingReport.VolumeUnitID = CostDs.Tables[7].Rows[0]["VolumeUnitID"] == DBNull.Value ? 0 : Convert.ToInt32(CostDs.Tables[7].Rows[0]["VolumeUnitID"]);
                            CostObj.PackingReport.WeightUnitID = CostDs.Tables[7].Rows[0]["WeightUnitID"] == DBNull.Value ? 0 : Convert.ToInt32(CostDs.Tables[7].Rows[0]["WeightUnitID"]);
                            CostObj.PackingReport.TobePackedVol = CostDs.Tables[7].Rows[0]["TobePackedVol"] == DBNull.Value ? (decimal)0.00 : Convert.ToDecimal(CostDs.Tables[7].Rows[0]["TobePackedVol"]);
                            CostObj.PackingReport.NetVol = CostDs.Tables[7].Rows[0]["NetVol"] == DBNull.Value ? (decimal)0.00 : Convert.ToDecimal(CostDs.Tables[7].Rows[0]["NetVol"]);
                            CostObj.PackingReport.GrossVol = CostDs.Tables[7].Rows[0]["GrossVol"] == DBNull.Value ? (decimal)0.00 : Convert.ToDecimal(CostDs.Tables[7].Rows[0]["GrossVol"]);
                            CostObj.PackingReport.NetWt = CostDs.Tables[7].Rows[0]["NetWt"] == DBNull.Value ? (decimal)0.00 : Convert.ToDecimal(CostDs.Tables[7].Rows[0]["NetWt"]);
                            CostObj.PackingReport.GrossWt = CostDs.Tables[7].Rows[0]["GrossWt"] == DBNull.Value ? (decimal)0.00 : Convert.ToDecimal(CostDs.Tables[7].Rows[0]["GrossWt"]);
                            CostObj.PackingReport.ACWTWt = CostDs.Tables[7].Rows[0]["ACWTWt"] == DBNull.Value ? (decimal)0.00 : Convert.ToDecimal(CostDs.Tables[7].Rows[0]["ACWTWt"]);
                            CostObj.PackingReport.ContainerSizeID = CostDs.Tables[7].Rows[0]["ContainerSizeID"] == DBNull.Value ? 0 : Convert.ToInt32(CostDs.Tables[7].Rows[0]["ContainerSizeID"]);
                            CostObj.PackingReport.CSCreatedBY = CostDs.Tables[7].Rows[0]["CSPreparedBy"] == DBNull.Value ? null : Convert.ToString(CostDs.Tables[7].Rows[0]["CSPreparedBy"]);
                            CostObj.PackingReport.CSCreatedDate = CostDs.Tables[7].Rows[0]["CSPreparedDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(CostDs.Tables[7].Rows[0]["CSPreparedDate"]);
                            CostObj.PackingReport.CSApprovedBY = CostDs.Tables[7].Rows[0]["CSApprovedBy"] == DBNull.Value ? null : Convert.ToString(CostDs.Tables[7].Rows[0]["CSApprovedBy"]);
                            CostObj.PackingReport.CSApprovedDate = CostDs.Tables[7].Rows[0]["CSApprovedDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(CostDs.Tables[7].Rows[0]["CSApprovedDate"]);
                            CostObj.PackingReport.OrgStgStartDate = CostDs.Tables[7].Rows[0]["OrgStgStartDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(CostDs.Tables[7].Rows[0]["OrgStgStartDate"]);
                            CostObj.PackingReport.OrgStgEndDate = CostDs.Tables[7].Rows[0]["OrgStgEndDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(CostDs.Tables[7].Rows[0]["OrgStgEndDate"]);
                            CostObj.PackingReport.RoadKMS = CostDs.Tables[7].Rows[0]["RoadKMS"] == DBNull.Value ? (decimal)0.00 : Convert.ToDecimal(CostDs.Tables[7].Rows[0]["RoadKMS"]);
                            CostObj.PackingReport.StageRemarks = CostDs.Tables[7].Rows[0]["StageRemark"] == DBNull.Value ? null : Convert.ToString(CostDs.Tables[7].Rows[0]["StageRemark"]);

                            CostObj.PackingReport.IsCSSenttoApprove = CostDs.Tables[7].Rows[0]["IsSendToApproval"] == DBNull.Value ? false : Convert.ToBoolean(CostDs.Tables[7].Rows[0]["IsSendToApproval"]);
                            CostObj.PackingReport.CSSenttoApproveUser = CostDs.Tables[7].Rows[0]["ApprovalUserID"] == DBNull.Value ? (Int32?)0 : Convert.ToInt32(CostDs.Tables[7].Rows[0]["ApprovalUserID"]);



                            if (CostObj.MoveJob.ModeID != 2)
                            {
                                CostObj.PackingReport.LCLorFCL = CostDs.Tables[7].Rows[0]["LCLorFCL"] == DBNull.Value ? null : Convert.ToString(CostDs.Tables[7].Rows[0]["LCLorFCL"]);
                                CostObj.PackingReport.LooseCased = CostDs.Tables[7].Rows[0]["LooseCased"] == DBNull.Value ? null : Convert.ToString(CostDs.Tables[7].Rows[0]["LooseCased"]);
                            }
                            else
                            {
                                CostObj.PackingReport.LCLorFCL = CostDs.Tables[7].Rows[0]["LCLorFCL"] == DBNull.Value ? "LCL" : Convert.ToString(CostDs.Tables[7].Rows[0]["LCLorFCL"]);
                                CostObj.PackingReport.LooseCased = CostDs.Tables[7].Rows[0]["LooseCased"] == DBNull.Value ? "Cased" : Convert.ToString(CostDs.Tables[7].Rows[0]["LooseCased"]);
                            }

                        }
                        else
                        {
                            if (CostObj.MoveJob.ModeID == 2)
                            {
                                CostObj.PackingReport.LCLorFCL = "LCL";
                                CostObj.PackingReport.LooseCased = "Cased";
                            }
                        }
                        #endregion

                        #region Packing Cost   
                        if (CostDs.Tables.Count > 8 && CostDs.Tables[8] != null && CostDs.Tables[8].Rows.Count > 0)
                        {
                            CostObj.PackingCostList.CostListSaved = Convert.ToString(CostDs.Tables[8].Rows[0]["IsPackCostSaved"]) == "Y";
                            CostObj.PackingCostList.CostList = (from item in CostDs.Tables[8].AsEnumerable()
                                                                select new PackingCostList
                                                                {
                                                                    RateCompId = item["CompId"] == DBNull.Value ? (int)0 : (Convert.ToInt32(item["CompId"])),
                                                                    RateCompName = Convert.ToString(item["RateComponentName"]),

                                                                    CostHeadID = item["CostHeadID"] == DBNull.Value ? (int?)0 : (Convert.ToInt32(item["CostHeadID"])),

                                                                    CostHeadName = Convert.ToString(item["CostHeadName"]),
                                                                    BaseCurrID = item["BaseCurrID"] == DBNull.Value ? (int)0 : (Convert.ToInt32(item["BaseCurrID"])),

                                                                    RateCurrID = item["RateCurrID"] == DBNull.Value ? (int)0 : (Convert.ToInt32(item["RateCurrID"])),
                                                                    RevRateCurrID = item["RevRateCurrID"] == DBNull.Value ? (int)0 : (Convert.ToInt32(item["RevRateCurrID"])),
                                                                    ConversionRate = item["ConversionRate"] == DBNull.Value ? (decimal?)0 : (Convert.ToDecimal(item["ConversionRate"])),
                                                                    RevConversionRate = item["RevConversionRate"] == DBNull.Value ? (decimal?)0 : (Convert.ToDecimal(item["RevConversionRate"])),
                                                                    CostValue = item["CostValue"] == DBNull.Value ? (decimal?)0 : (Convert.ToDecimal(item["CostValue"])),
                                                                    BaseRevenueValue = item["BaseRev"] == DBNull.Value ? (decimal?)0 : (Convert.ToDecimal(item["BaseRev"])),
                                                                    RevenueValue = item["Revenuevalue"] == DBNull.Value ? (decimal?)0 : (Convert.ToDecimal(item["Revenuevalue"])),
                                                                    Balance = item["UNBILLED"] == DBNull.Value ? (decimal?)0 : (Convert.ToDecimal(item["UNBILLED"])),
                                                                    BaseCurr = Convert.ToString(item["BaseCurrName"]),

                                                                    RateCurr = Convert.ToString(item["RateCurrName"]),
                                                                    RevRateCurr = Convert.ToString(item["RevRateCurrName"]),
                                                                    Isactive = true,// item["Isactive"] == DBNull.Value ? (bool)false : (Convert.ToBoolean(item["Isactive"])),
                                                                    Per = item["Per"] == DBNull.Value ? (int)0 : (Convert.ToInt32(item["Per"])),
                                                                    ToBill = item["ToBill"] == DBNull.Value ? (bool)false : (Convert.ToBoolean(item["ToBill"])),
                                                                    WtUnitName = item["WtUnitName"] == DBNull.Value ? null : (Convert.ToString(item["WtUnitName"])),
                                                                    Rate = item["Rate"] == DBNull.Value ? (decimal?)0 : (Convert.ToDecimal(item["Rate"])),
                                                                    WtVol = item["Wt_Vol_No"] == DBNull.Value ? null : (Convert.ToString(item["Wt_Vol_No"])),
                                                                    RevRate = item["RevRate"] == DBNull.Value ? 0 : Convert.ToDecimal(item["RevRate"])
                                                                    /*ConversionRate = item["BaseCurrConversRate"] == DBNull.Value ? 0 : Convert.ToDecimal(item["BaseCurrConversRate"]),
                                                                    Amount = item["RateCurrValue"]==DBNull.Value ? 0 :  Convert.ToDecimal(item["RateCurrValue"]),*/
                                                                }).ToList();

                            CostObj.PackingReport.IsApprove = CostDs.Tables[7] != null && CostDs.Tables[7].Rows.Count > 0 && CostDs.Tables[7].Rows[0]["IsCostSheetApproved"] != DBNull.Value ? Convert.ToBoolean(CostDs.Tables[7].Rows[0]["IsCostSheetApproved"]) : (bool)false;
                            CostObj.PackingReport.ApproveTitle = CostObj.PackingReport.IsApprove ? "Approved" : "Pending";
                        }
                        #endregion
                    }

                    #endregion Packing Info

                    #region Freight Info
                    frtFlag = orgFlag ? CostDs.Tables.Count > 9 && CostDs.Tables[9] != null && CostDs.Tables[9].Rows.Count > 0 && CostDs.Tables[9].Rows[0][0].ToString() == "Same as survey details" : CostDs.Tables.Count > 6 && CostDs.Tables[6] != null && CostDs.Tables[6].Rows.Count > 0 && CostDs.Tables[6].Rows[0][0].ToString() == "Same as survey details";
                    int frttable = orgFlag ? 9 : 6;
                    if (frtFlag)
                    {
                        CostObj.FreightDetail.OrgAgentID = CostObj.PackingDetail.OrgAgentID;
                        CostObj.FreightDetail.FrtAgentID = CostObj.PackingDetail.FrtAgentID;
                        CostObj.FreightDetail.DestAgentID = CostObj.PackingDetail.DestAgentID;
                        CostObj.FreightDetail.ExitPortID = CostObj.PackingDetail.ExitPortID;
                        CostObj.FreightDetail.EntryPortID = CostObj.PackingDetail.EntryPortID;

                        /*CostObj.FreightReport.Packdate = CostObj.PackingReport.Packdate;
                        CostObj.FreightReport.Loaddate = CostObj.PackingReport.Loaddate;
                        CostObj.FreightReport.DensityFact = CostObj.PackingReport.DensityFact;
                        CostObj.FreightReport.VolumeUnitID = CostObj.PackingReport.VolumeUnitID;
                        CostObj.FreightReport.WeightUnitID = CostObj.PackingReport.WeightUnitID;
                        CostObj.FreightReport.TobePackedVol = CostObj.PackingReport.TobePackedVol;
                        CostObj.FreightReport.NetVol = CostObj.PackingReport.NetVol;
                        CostObj.FreightReport.GrossVol = CostObj.PackingReport.GrossVol;
                        CostObj.FreightReport.NetWt = CostObj.PackingReport.NetWt;
                        CostObj.FreightReport.GrossWt = CostObj.PackingReport.GrossWt;
                        CostObj.FreightReport.ACWTWt = CostObj.PackingReport.ACWTWt;*/

                        CostObj.FreightSOList.SOList = CostObj.PackingSOList.SOList;

                        CostObj.FreightCostList.CostList = CostObj.PackingCostList.CostList;
                    }
                    else if (!frtFlag && CostDs.Tables.Count > frttable && CostDs.Tables[frttable] != null /*&& CostDs.Tables[frttable].Rows.Count > 0 && CostDs.Tables[frttable].Rows[0][0].ToString() == "Same as survey details"*/)
                    {
                        #region Freight Details
                        if (CostDs.Tables.Count > frttable && CostDs.Tables[frttable] != null && CostDs.Tables[frttable].Rows.Count > 0)
                        {
                            CostObj.FreightDetail.IsDone = true;
                            CostObj.FreightDetail.OrgAgentID = CostDs.Tables[frttable].Rows[0]["OrgAgentID"] == DBNull.Value ? (int?)null : Convert.ToInt32(CostDs.Tables[frttable].Rows[0]["OrgAgentID"]);
                            CostObj.FreightDetail.FrtAgentID = CostDs.Tables[frttable].Rows[0]["FrtAgentID"] == DBNull.Value ? (int?)null : Convert.ToInt32(CostDs.Tables[frttable].Rows[0]["FrtAgentID"]);
                            CostObj.FreightDetail.DestAgentID = CostDs.Tables[frttable].Rows[0]["DestAgentID"] == DBNull.Value ? (int?)null : Convert.ToInt32(CostDs.Tables[frttable].Rows[0]["DestAgentID"]);
                            CostObj.FreightDetail.ExitPortID = CostDs.Tables[frttable].Rows[0]["ExitPortID"] == DBNull.Value ? (int?)null : Convert.ToInt32(CostDs.Tables[frttable].Rows[0]["ExitPortID"]);
                            CostObj.FreightDetail.EntryPortID = CostDs.Tables[frttable].Rows[0]["EntryPortID"] == DBNull.Value ? (int?)null : Convert.ToInt32(CostDs.Tables[frttable].Rows[0]["EntryPortID"]);
                            CostObj.FreightReport.ShipmentCartedOn = CostDs.Tables[frttable].Rows[0]["ShipmentCartedOn"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(CostDs.Tables[frttable].Rows[0]["ShipmentCartedOn"]);
                            CostObj.FreightReport.CustomeClearedOn = CostDs.Tables[frttable].Rows[0]["CustomeClearedOn"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(CostDs.Tables[frttable].Rows[0]["CustomeClearedOn"]);
                            CostObj.FreightReport.SB_GivenOn = CostDs.Tables[frttable].Rows[0]["SB_GivenOn"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(CostDs.Tables[frttable].Rows[0]["SB_GivenOn"]);
                            CostObj.FreightReport.BLSentToAgentOn = CostDs.Tables[frttable].Rows[0]["BLSentToAgentOn"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(CostDs.Tables[frttable].Rows[0]["BLSentToAgentOn"]);
                            CostObj.FreightReport.BLReleaseOn = CostDs.Tables[frttable].Rows[0]["BLReleasedOn"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(CostDs.Tables[frttable].Rows[0]["BLReleasedOn"]);
                            CostObj.FreightReport.SD = CostDs.Tables[frttable].Rows[0]["ShipDespDateSD"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(CostDs.Tables[frttable].Rows[0]["ShipDespDateSD"]);
                            CostObj.FreightReport.OPS = CostDs.Tables[frttable].Rows[0]["ShipDespDateOP"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(CostDs.Tables[frttable].Rows[0]["ShipDespDateOP"]);

                            CostObj.FreightReport.SSAgent = CostDs.Tables[frttable].Rows[0]["SchAgent"] == DBNull.Value ? null : Convert.ToString(CostDs.Tables[frttable].Rows[0]["SchAgent"]);
                            CostObj.TransitAgent = CostDs.Tables[frttable].Rows[0]["TransitAgentID"] == DBNull.Value ? (int?)null : Convert.ToInt32(CostDs.Tables[frttable].Rows[0]["TransitAgentID"]);
                            CostObj.FreightReport.FS_DS = CostDs.Tables[frttable].Rows[0]["FsOrDs"] == DBNull.Value ? null : Convert.ToString(CostDs.Tables[frttable].Rows[0]["FsOrDs"]);
                            CostObj.FreightReport.ISF_Ref = CostDs.Tables[frttable].Rows[0]["ISFNumber"] == DBNull.Value ? null : Convert.ToString(CostDs.Tables[frttable].Rows[0]["ISFNumber"]);
                            CostObj.FreightReport.TruckNo = CostDs.Tables[frttable].Rows[0]["TruckNo"] == DBNull.Value ? null : Convert.ToString(CostDs.Tables[frttable].Rows[0]["TruckNo"]);
                            CostObj.FreightReport.VehicleTypeId = CostDs.Tables[frttable].Rows[0]["VehicleType"] == DBNull.Value ? 0 : Convert.ToInt32(CostDs.Tables[frttable].Rows[0]["VehicleType"]);
                            CostObj.FreightReport.TotalCapacity = CostDs.Tables[frttable].Rows[0]["TotalCapacity"] == DBNull.Value ? 0 : Convert.ToInt32(CostDs.Tables[frttable].Rows[0]["TotalCapacity"]);
                            CostObj.FreightReport.EsordedBy = CostDs.Tables[frttable].Rows[0]["EscortedBy"] == DBNull.Value ? null : Convert.ToString(CostDs.Tables[frttable].Rows[0]["EscortedBy"]);
                            CostObj.FreightReport.LeftOnDate = CostDs.Tables[frttable].Rows[0]["VehLeftOnDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(CostDs.Tables[frttable].Rows[0]["VehLeftOnDate"]);
                            CostObj.FreightReport.Courier = CostDs.Tables[frttable].Rows[0]["Courier"] == DBNull.Value ? 0 : Convert.ToInt32(CostDs.Tables[frttable].Rows[0]["Courier"]);
                            CostObj.FreightReport.AirLine = CostDs.Tables[frttable].Rows[0]["AirLines"] == DBNull.Value ? 0 : Convert.ToInt32(CostDs.Tables[frttable].Rows[0]["AirLines"]);
                            CostObj.FreightReport.ContainerNo = CostDs.Tables[frttable].Rows[0]["ContainerNo"] == DBNull.Value ? null : Convert.ToString(CostDs.Tables[frttable].Rows[0]["ContainerNo"]);
                            CostObj.FreightReport.SealNo = CostDs.Tables[frttable].Rows[0]["SealNo"] == DBNull.Value ? null : Convert.ToString(CostDs.Tables[frttable].Rows[0]["SealNo"]);
                            CostObj.FreightReport.NoOfPacks = CostDs.Tables[frttable].Rows[0]["NoOfPacks"] == DBNull.Value ? 0 : Convert.ToInt32(CostDs.Tables[frttable].Rows[0]["NoOfPacks"]);
                            CostObj.FreightReport.Bill_No = CostDs.Tables[frttable].Rows[0]["BillNo"] == DBNull.Value ? null : Convert.ToString(CostDs.Tables[frttable].Rows[0]["BillNo"]);
                            CostObj.FreightReport.TransitShipment = Convert.ToBoolean(CostDs.Tables[frttable].Rows[0]["TransitShipment"]);
                            CostObj.FreightReport.DeliveryDate = CostDs.Tables[frttable].Rows[0]["SchDeliveryDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(CostDs.Tables[frttable].Rows[0]["SchDeliveryDate"]);

                            CostObj.FreightReport.IsISF = Convert.ToBoolean(CostDs.Tables[frttable].Rows[0]["IsISF"]);
                            CostObj.FreightReport.IsSD = Convert.ToBoolean(CostDs.Tables[frttable].Rows[0]["IsShipDespDateSDs"]);
                            CostObj.FreightReport.IsDirectCarting = Convert.ToBoolean(CostDs.Tables[frttable].Rows[0]["IsDirectCarting"]);
                            CostObj.FreightReport.IsBLSentToAgent = Convert.ToBoolean(CostDs.Tables[frttable].Rows[0]["IsBLSentToAgent"]);
                            CostObj.FreightReport.PortLoad = CostDs.Tables[frttable].Rows[0]["PortLoadID"] == DBNull.Value ? 0 : Convert.ToInt32(CostDs.Tables[frttable].Rows[0]["PortLoadID"]);
                            CostObj.FreightReport.PortDischarge = CostDs.Tables[frttable].Rows[0]["PortDischargeID"] == DBNull.Value ? 0 : Convert.ToInt32(CostDs.Tables[frttable].Rows[0]["PortDischargeID"]);
                            CostObj.FreightReport.LCL_FCL = CostDs.Tables[frttable].Rows[0]["LCLFCL"] == DBNull.Value ? null : Convert.ToString(CostDs.Tables[frttable].Rows[0]["LCLFCL"]);
                            CostObj.FreightReport.ForwardingBr = CostDs.Tables[frttable].Rows[0]["ForwardingBrID"] == DBNull.Value ? (int?)null : Convert.ToInt32(CostDs.Tables[frttable].Rows[0]["ForwardingBrID"]);
                            CostObj.FreightReport.FCL_20 = CostDs.Tables[frttable].Rows[0]["FCL20"] == DBNull.Value ? (int?)null : Convert.ToInt32(CostDs.Tables[frttable].Rows[0]["FCL20"]);
                            CostObj.FreightReport.FCL_40 = CostDs.Tables[frttable].Rows[0]["FCL40"] == DBNull.Value ? (int?)null : Convert.ToInt32(CostDs.Tables[frttable].Rows[0]["FCL40"]);
                            CostObj.FreightReport.FCLHC_40 = CostDs.Tables[frttable].Rows[0]["FCLHC40"] == DBNull.Value ? (int?)null : Convert.ToInt32(CostDs.Tables[frttable].Rows[0]["FCLHC40"]);
                            CostObj.FreightReport.THCCollect = CostDs.Tables[frttable].Rows[0]["THCCollect"] == DBNull.Value ? false : Convert.ToBoolean(CostDs.Tables[frttable].Rows[0]["THCCollect"]);
                            CostObj.FreightReport.THCPrepaid = CostDs.Tables[frttable].Rows[0]["THCPrepaid"] == DBNull.Value ? false : Convert.ToBoolean(CostDs.Tables[frttable].Rows[0]["THCPrepaid"]);
                            CostObj.FreightReport.SSLAgentId = CostDs.Tables[frttable].Rows[0]["SSLAgentID"] == DBNull.Value ? (int?)null : Convert.ToInt32(CostDs.Tables[frttable].Rows[0]["SSLAgentID"]);
                            CostObj.FreightReport.SSLCarrierId = CostDs.Tables[frttable].Rows[0]["CarrierID"] == DBNull.Value ? (int?)null : Convert.ToInt32(CostDs.Tables[frttable].Rows[0]["CarrierID"]);
                            CostObj.FreightReport.SSLAgent = Convert.ToString(CostDs.Tables[frttable].Rows[0]["SSLAgentName"]);
                            CostObj.FreightReport.SSLCarrier = Convert.ToString(CostDs.Tables[frttable].Rows[0]["SSLCarrierName"]);
                        }
                        #endregion
                        frttable++;
                        #region Freight SO
                        if (CostDs.Tables.Count > 6 && CostDs.Tables[frttable] != null && CostDs.Tables[frttable].Rows.Count > 0)
                        {
                            CostObj.FreightSOList.SOList = (from item in CostDs.Tables[frttable].AsEnumerable()
                                                            select new PackingSOList
                                                            {
                                                                CostHeadID = item["CostHeadID"] == DBNull.Value ? (int?)0 : (Convert.ToInt32(item["CostHeadID"])),
                                                                CostHeadName = Convert.ToString(item["CostHeadName"]),
                                                                Remark = Convert.ToString(item["ServOrderRemarks"]),
                                                                RateCompId = Convert.ToInt32(RELOCBS.Common.CommonService.RateComp.Origin.ToString()),
                                                                Volume = Convert.ToString(item["WtVolume"]),
                                                                WtUnitID = item["WtUnitID"] == DBNull.Value ? (int)0 : (Convert.ToInt32(item["WtUnitID"])),
                                                                WtUnit = Convert.ToString(item["WeightUnitName"]),
                                                                Isactive = true// item["Isactive"] == DBNull.Value ? (bool)false : (Convert.ToBoolean(item["Isactive"])),
                                                            }).ToList();
                        }
                        #endregion
                        frttable++;
                        #region Freight Report

                        if (CostDs.Tables.Count > frttable && CostDs.Tables[frttable] != null && CostDs.Tables[frttable].Rows.Count > 0)
                        {
                            /*CostObj.FreightReport.Packdate = CostDs.Tables[frttable].Rows[0]["PackDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(CostDs.Tables[frttable].Rows[0]["PackDate"]); //(DateTime?)CostDs.Tables[frttable].Rows[0]["PackDate"];
                            CostObj.FreightReport.Loaddate = CostDs.Tables[frttable].Rows[0]["LoadDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(CostDs.Tables[frttable].Rows[0]["LoadDate"]); //(DateTime?)CostDs.Tables[frttable].Rows[0]["LoadDate"];
                            CostObj.FreightReport.DensityFact = CostDs.Tables[frttable].Rows[0]["DensityFact"] == DBNull.Value ? (decimal)0.00 : Convert.ToDecimal(CostDs.Tables[frttable].Rows[0]["DensityFact"]);
                            CostObj.FreightReport.VolumeUnitID = CostDs.Tables[frttable].Rows[0]["VolumeUnitID"] == DBNull.Value ? 0 : Convert.ToInt32(CostDs.Tables[frttable].Rows[0]["VolumeUnitID"]);
                            CostObj.FreightReport.WeightUnitID = CostDs.Tables[frttable].Rows[0]["WeightUnitID"] == DBNull.Value ? 0 : Convert.ToInt32(CostDs.Tables[frttable].Rows[0]["WeightUnitID"]);
                            CostObj.FreightReport.TobePackedVol = CostDs.Tables[frttable].Rows[0]["TobePackedVol"] == DBNull.Value ? (decimal)0.00 : Convert.ToDecimal(CostDs.Tables[frttable].Rows[0]["TobePackedVol"]);
                            CostObj.FreightReport.NetVol = CostDs.Tables[frttable].Rows[0]["NetVol"] == DBNull.Value ? (decimal)0.00 : Convert.ToDecimal(CostDs.Tables[frttable].Rows[0]["NetVol"]);
                            CostObj.FreightReport.GrossVol = CostDs.Tables[frttable].Rows[0]["GrossVol"] == DBNull.Value ? (decimal)0.00 : Convert.ToDecimal(CostDs.Tables[frttable].Rows[0]["GrossVol"]);
                            CostObj.FreightReport.NetWt = CostDs.Tables[frttable].Rows[0]["NetWt"] == DBNull.Value ? (decimal)0.00 : Convert.ToDecimal(CostDs.Tables[frttable].Rows[0]["NetWt"]);
                            CostObj.FreightReport.GrossWt = CostDs.Tables[frttable].Rows[0]["GrossWt"] == DBNull.Value ? (decimal)0.00 : Convert.ToDecimal(CostDs.Tables[frttable].Rows[0]["GrossWt"]);
                            CostObj.FreightReport.ACWTWt = CostDs.Tables[frttable].Rows[0]["ACWTWt"] == DBNull.Value ? (decimal)0.00 : Convert.ToDecimal(CostDs.Tables[frttable].Rows[0]["ACWTWt"]);*/

                            if (CostDs.Tables[frttable].Columns.Contains("OrderNo") && CostDs.Tables[frttable].AsEnumerable().Where(m => m.Field<int?>("OrderNo") == null).Count() > 0)
                            {
                                DataColumn column = CostDs.Tables[frttable].Columns["OrderNo"];
                                column.AutoIncrement = true;
                                column.AutoIncrementSeed = 1;
                                column.AutoIncrementStep = 1;
                                column.DataType = typeof(Int32);
                                int index = 1;
                                foreach (DataRow row in CostDs.Tables[frttable].Rows)
                                {
                                    row.SetField(column, index);
                                    index++;
                                }
                            }
                            CostDs.Tables[frttable].Columns.Add(new DataColumn("FromPortId", typeof(Int32)));
                            DataRow previousRow = null;
                            string TransitPortColumn = "TransitPortID";
                            int TransitRowNo = 0;
                            foreach (DataRow dr in CostDs.Tables[frttable].Rows) // Here ldtAlbum is the Datatable
                            {
                                if (TransitRowNo == 0)
                                {
                                    dr["FromPortId"] = CostObj.FreightReport.PortLoad;
                                }
                                else
                                {
                                    dr["FromPortId"] = previousRow[TransitPortColumn];
                                }
                                if ((TransitRowNo + 1) == CostDs.Tables[frttable].Rows.Count)
                                {
                                    dr["TransitPortID"] = CostObj.FreightReport.PortDischarge;
                                }
                                previousRow = dr;
                                TransitRowNo++;
                            }

                            CostObj.FreightReport.TranShipmentList = (from item in CostDs.Tables[frttable].AsEnumerable()
                                                                      select new TranShipment
                                                                      {
                                                                          ScheduleVessel = Convert.ToString(item["SchVessel"]),
                                                                          FromPortId = Convert.ToInt32(item["FromPortId"]),
                                                                          ETD = string.IsNullOrEmpty(Convert.ToString(item["EDD"])) ? (DateTime?)null : Convert.ToDateTime(item["EDD"]),
                                                                          ETA = string.IsNullOrEmpty(Convert.ToString(item["EDA"])) ? (DateTime?)null : Convert.ToDateTime(item["EDA"]),
                                                                          ActArr = string.IsNullOrEmpty(Convert.ToString(item["ActDA"])) ? (DateTime?)null : Convert.ToDateTime(item["ActDA"]),
                                                                          ActDep = string.IsNullOrEmpty(Convert.ToString(item["ActDD"])) ? (DateTime?)null : Convert.ToDateTime(item["ActDD"]),
                                                                          TranshipPortId = string.IsNullOrEmpty(Convert.ToString(item["TransitPortID"])) ? (int?)null : Convert.ToInt32(item["TransitPortID"]),
                                                                          TranshipPortName = Convert.ToString(item["PortName"]),
                                                                          Isactive = string.IsNullOrEmpty(Convert.ToString(item["Isactive"])) ? (bool)false : (Convert.ToBoolean(item["Isactive"])),
                                                                          OrderNo = Convert.ToInt32(item["OrderNo"])
                                                                      }).ToList();
                        }
                        #endregion
                        frttable++;
                        #region freight Cost   
                        if (CostDs.Tables.Count > frttable && CostDs.Tables[frttable] != null && CostDs.Tables[frttable].Rows.Count > 0)
                        {
                            CostObj.FreightCostList.CostList = (from item in CostDs.Tables[frttable].AsEnumerable()
                                                                select new PackingCostList
                                                                {
                                                                    RateCompId = item["CompId"] == DBNull.Value ? (int)0 : (Convert.ToInt32(item["CompId"])),
                                                                    RateCompName = Convert.ToString(item["RateComponentName"]),

                                                                    CostHeadID = item["CostHeadID"] == DBNull.Value ? (int?)0 : (Convert.ToInt32(item["CostHeadID"])),

                                                                    CostHeadName = Convert.ToString(item["CostHeadName"]),
                                                                    BaseCurrID = item["BaseCurrID"] == DBNull.Value ? (int)0 : (Convert.ToInt32(item["BaseCurrID"])),

                                                                    RateCurrID = item["RateCurrID"] == DBNull.Value ? (int)0 : (Convert.ToInt32(item["RateCurrID"])),
                                                                    RevRateCurrID = item["RevRateCurrID"] == DBNull.Value ? (int)0 : (Convert.ToInt32(item["RevRateCurrID"])),
                                                                    RevConversionRate = item["RevConversionRate"] == DBNull.Value ? (decimal?)0 : (Convert.ToDecimal(item["RevConversionRate"])),
                                                                    CostValue = item["CostValue"] == DBNull.Value ? (decimal?)0 : (Convert.ToDecimal(item["CostValue"])),
                                                                    BaseRevenueValue = item["BaseRev"] == DBNull.Value ? (decimal?)0 : (Convert.ToDecimal(item["BaseRev"])),
                                                                    RevenueValue = item["Revenuevalue"] == DBNull.Value ? (decimal?)0 : (Convert.ToDecimal(item["Revenuevalue"])),

                                                                    BaseCurr = Convert.ToString(item["BaseCurrName"]),
                                                                    RateCurr = Convert.ToString(item["RateCurrName"]),
                                                                    RevRateCurr = Convert.ToString(item["RevRateCurrName"]),
                                                                    Balance = item["Revenuevalue"] == DBNull.Value ? (decimal?)0 : (Convert.ToDecimal(item["Revenuevalue"]))

                                                                    ,
                                                                    Isactive = true//item["Isactive"] == DBNull.Value ? (bool)false : (Convert.ToBoolean(item["Isactive"])),

                                                                    /*ConversionRate = item["BaseCurrConversRate"] == DBNull.Value ? 0 : Convert.ToDecimal(item["BaseCurrConversRate"]),
                                                                    Amount = item["RateCurrValue"]==DBNull.Value ? 0 :  Convert.ToDecimal(item["RateCurrValue"]),*/
                                                                }).ToList();
                        }
                        #endregion
                    }

                    #endregion Freight Info

                    #region Destination Info
                    frttable++;

                    int destttable = frttable;
                    destFlag = frtFlag ? CostDs.Tables.Count > destttable && CostDs.Tables[destttable] != null && CostDs.Tables[destttable].Rows.Count > 0 && CostDs.Tables[destttable].Rows[0][0].ToString() == "Same as survey details" : CostDs.Tables.Count > destttable && CostDs.Tables[destttable] != null && CostDs.Tables[destttable].Rows.Count > 0 && CostDs.Tables[destttable].Rows[0][0].ToString() == "Same as survey details";

                    if (destFlag)
                    {
                        CostObj.DeliveryDetail.OrgAgentID = CostObj.PackingDetail.OrgAgentID;
                        CostObj.DeliveryDetail.FrtAgentID = CostObj.PackingDetail.FrtAgentID;
                        CostObj.DeliveryDetail.DestAgentID = CostObj.PackingDetail.DestAgentID;
                        CostObj.DeliveryDetail.ExitPortID = CostObj.PackingDetail.ExitPortID;
                        CostObj.DeliveryDetail.EntryPortID = CostObj.PackingDetail.EntryPortID;

                        /*CostObj.DeliveryReport.Packdate = CostObj.PackingReport.Packdate;
                        CostObj.DeliveryReport.Loaddate = CostObj.PackingReport.Loaddate;
                        CostObj.DeliveryReport.DensityFact = CostObj.PackingReport.DensityFact;
                        CostObj.DeliveryReport.VolumeUnitID = CostObj.PackingReport.VolumeUnitID;
                        CostObj.DeliveryReport.WeightUnitID = CostObj.PackingReport.WeightUnitID;
                        CostObj.DeliveryReport.TobePackedVol = CostObj.PackingReport.TobePackedVol;
                        CostObj.DeliveryReport.NetVol = CostObj.PackingReport.NetVol;
                        CostObj.DeliveryReport.GrossVol = CostObj.PackingReport.GrossVol;
                        CostObj.DeliveryReport.NetWt = CostObj.PackingReport.NetWt;
                        CostObj.DeliveryReport.GrossWt = CostObj.PackingReport.GrossWt;
                        CostObj.DeliveryReport.ACWTWt = CostObj.PackingReport.ACWTWt;*/

                        CostObj.DeliverySOList.SOList = CostObj.PackingSOList.SOList;

                        CostObj.DeliveryCostList.CostList = CostObj.PackingCostList.CostList;
                    }
                    else if (!destFlag && CostDs.Tables.Count > destttable && CostDs.Tables[destttable] != null /*&& CostDs.Tables[destttable].Rows.Count > 0 && CostDs.Tables[destttable].Rows[0][0].ToString() == "Same as survey details"*/)
                    {
                        #region Destination Details
                        if (CostDs.Tables.Count > destttable && CostDs.Tables[destttable] != null && CostDs.Tables[destttable].Rows.Count > 0)
                        {
                            CostObj.DeliveryDetail.IsDone = true;
                            CostObj.DeliveryDetail.OrgAgentID = CostDs.Tables[destttable].Rows[0]["OrgAgentID"] == DBNull.Value ? (int?)null : Convert.ToInt32(CostDs.Tables[destttable].Rows[0]["OrgAgentID"]);
                            CostObj.DeliveryDetail.OrgAgentName = Convert.ToString(CostDs.Tables[destttable].Rows[0]["OrgAgentName"]);
                            CostObj.DeliveryDetail.FrtAgentID = CostDs.Tables[destttable].Rows[0]["FrtAgentID"] == DBNull.Value ? (int?)null : Convert.ToInt32(CostDs.Tables[destttable].Rows[0]["FrtAgentID"]);
                            CostObj.DeliveryDetail.DestAgentID = CostDs.Tables[destttable].Rows[0]["DestAgentID"] == DBNull.Value ? (int?)null : Convert.ToInt32(CostDs.Tables[destttable].Rows[0]["DestAgentID"]);
                            CostObj.DeliveryDetail.DestAgentName = Convert.ToString(CostDs.Tables[destttable].Rows[0]["DestAgentName"]);
                            CostObj.DeliveryDetail.ExitPortID = CostDs.Tables[destttable].Rows[0]["ExitPortID"] == DBNull.Value ? (int?)null : Convert.ToInt32(CostDs.Tables[destttable].Rows[0]["ExitPortID"]);
                            CostObj.DeliveryDetail.ExitPortName = Convert.ToString(CostDs.Tables[destttable].Rows[0]["ExitPortName"]);
                            CostObj.DeliveryDetail.EntryPortID = CostDs.Tables[destttable].Rows[0]["EntryPortID"] == DBNull.Value ? (int?)null : Convert.ToInt32(CostDs.Tables[destttable].Rows[0]["EntryPortID"]);
                            CostObj.DeliveryDetail.EntryPortName = Convert.ToString(CostDs.Tables[destttable].Rows[0]["EntryPortName"]);

                            CostObj.IsDestApprove = Convert.ToBoolean(CostDs.Tables[destttable].Rows[0]["IsCostSheetApproved"]);

                            CostObj.DestApprove = CostObj.IsDestApprove ? "Approved" : "Pending";
                            if (CostObj.MoveJob.ModeID != 3)
                            {
                                if (!string.IsNullOrEmpty(Convert.ToString(CostDs.Tables[destttable].Rows[0]["DestAgentName"])) &&
                                CostObj.MoveJob.MoveRateCompList.Where(x => x.RateComponentID == 3).Count() <= 0)
                                {
                                    CostObj.MoveJob.MoveRateCompList.Add(new MoveRateComponent() { RateComponentID = 3, RateComponentName = "Destination" });
                                }


                                CostObj.MoveJob.MoveRateCompList = CostObj.MoveJob.MoveRateCompList.Select(i =>
                                {
                                    if (i.RateComponentID == 1) i.ActJobAgentName = Convert.ToString(CostDs.Tables[destttable].Rows[0]["OrgAgentName"]);
                                    if (i.RateComponentID == 1) i.ActJobAgentID = Convert.ToString(CostDs.Tables[destttable].Rows[0]["OrgAgentID"]);
                                    if (i.RateComponentID == 2) i.ActJobAgentName = Convert.ToString(CostDs.Tables[destttable].Rows[0]["OrgAgentName"]);
                                    if (i.RateComponentID == 2) i.ActJobAgentID = Convert.ToString(CostDs.Tables[destttable].Rows[0]["OrgAgentID"]);
                                    if (i.RateComponentID == 3) i.ActJobAgentName = Convert.ToString(CostDs.Tables[destttable].Rows[0]["DestAgentName"]);
                                    if (i.RateComponentID == 2) i.ActJobAgentID = Convert.ToString(CostDs.Tables[destttable].Rows[0]["DestAgentID"]);
                                    return i;
                                }).ToList();
                            }
                            else if (CostObj.MoveJob.ModeID == 3 && CostObj.MoveJob.MoveRateCompList.Count() > 1)
                            {
                                if (!string.IsNullOrEmpty(Convert.ToString(CostDs.Tables[destttable].Rows[0]["DestAgentName"])) &&
                                CostObj.MoveJob.MoveRateCompList.Where(x => x.RateComponentID == 3).Count() <= 0)
                                {
                                    CostObj.MoveJob.MoveRateCompList.Add(new MoveRateComponent() { RateComponentID = 3, RateComponentName = "Destination" });
                                }
                                if (!string.IsNullOrEmpty(Convert.ToString(CostDs.Tables[destttable].Rows[0]["OrgAgentName"])) &&
                                    CostObj.MoveJob.MoveRateCompList.Where(x => x.RateComponentID == 1).Count() <= 0)
                                {

                                    CostObj.MoveJob.MoveRateCompList.Add(new MoveRateComponent() { RateComponentID = 1, RateComponentName = "Origin" });
                                    CostObj.MoveJob.MoveRateCompList.Add(new MoveRateComponent() { RateComponentID = 2, RateComponentName = "Freight" });
                                }

                                CostObj.MoveJob.MoveRateCompList = CostObj.MoveJob.MoveRateCompList.Select(i =>
                                {
                                    if (i.RateComponentID == 1) i.ActJobAgentName = Convert.ToString(CostDs.Tables[destttable].Rows[0]["OrgAgentName"]);
                                    if (i.RateComponentID == 1) i.ActJobAgentID = Convert.ToString(CostDs.Tables[destttable].Rows[0]["OrgAgentID"]);
                                    if (i.RateComponentID == 2) i.ActJobAgentName = Convert.ToString(CostDs.Tables[destttable].Rows[0]["FrtAgentName"]);
                                    if (i.RateComponentID == 2) i.ActJobAgentID = Convert.ToString(CostDs.Tables[destttable].Rows[0]["FrtAgentID"]);
                                    if (i.RateComponentID == 3) i.ActJobAgentName = Convert.ToString(CostDs.Tables[destttable].Rows[0]["DestAgentName"]);
                                    if (i.RateComponentID == 3) i.ActJobAgentID = Convert.ToString(CostDs.Tables[destttable].Rows[0]["DestAgentID"]);
                                    return i;
                                }).ToList();
                            }

                        }
                        #endregion
                        destttable++;
                        #region Destination SO
                        if (CostDs.Tables.Count > 6 && CostDs.Tables[destttable] != null && CostDs.Tables[destttable].Rows.Count > 0)
                        {
                            CostObj.DeliverySOList.SOList = (from item in CostDs.Tables[destttable].AsEnumerable()
                                                             select new PackingSOList
                                                             {
                                                                 CostHeadID = item["CostHeadID"] == DBNull.Value ? (int?)0 : (Convert.ToInt32(item["CostHeadID"])),
                                                                 CostHeadName = Convert.ToString(item["CostHeadName"]),
                                                                 Remark = Convert.ToString(item["ServOrderRemarks"]),
                                                                 RateCompId = Convert.ToInt32(RELOCBS.Common.CommonService.RateComp.Origin.ToString()),
                                                                 Volume = Convert.ToString(item["WtVolume"]),
                                                                 WtUnitID = item["WtUnitID"] == DBNull.Value ? (int)0 : (Convert.ToInt32(item["WtUnitID"])),
                                                                 WtUnit = Convert.ToString(item["WeightUnitName"]),
                                                                 Isactive = true//item["Isactive"] == DBNull.Value ? (bool)false : (Convert.ToBoolean(item["Isactive"])),
                                                             }).ToList();
                        }
                        #endregion
                        destttable++;
                        #region Destination Report
                        if (CostDs.Tables.Count > destttable && CostDs.Tables[destttable] != null && CostDs.Tables[destttable].Rows.Count > 0)
                        {
                            //CostObj.DeliveryReport.Packdate = CostDs.Tables[destttable].Rows[0]["PackDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(CostDs.Tables[destttable].Rows[0]["PackDate"]); //(DateTime?)CostDs.Tables[destttable].Rows[0]["PackDate"];
                            //CostObj.DeliveryReport.Loaddate = CostDs.Tables[destttable].Rows[0]["LoadDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(CostDs.Tables[destttable].Rows[0]["LoadDate"]); //(DateTime?)CostDs.Tables[destttable].Rows[0]["LoadDate"];
                            CostObj.DeliveryReport.DeliveryDate = CostDs.Tables[destttable].Rows[0]["Deliverydate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(CostDs.Tables[destttable].Rows[0]["Deliverydate"]); //(DateTime?)CostDs.Tables[destttable].Rows[0]["LoadDate"];
                            CostObj.DeliveryReport.PassportNo = CostDs.Tables[destttable].Rows[0]["PassportNo"] == DBNull.Value ? null : Convert.ToString(CostDs.Tables[destttable].Rows[0]["PassportNo"]);
                            CostObj.DeliveryReport.DensityFact = CostDs.Tables[destttable].Rows[0]["DensityFact"] == DBNull.Value ? (decimal)0.00 : Convert.ToDecimal(CostDs.Tables[destttable].Rows[0]["DensityFact"]);
                            CostObj.DeliveryReport.VolumeUnitID = CostDs.Tables[destttable].Rows[0]["VolumeUnitID"] == DBNull.Value ? 0 : Convert.ToInt32(CostDs.Tables[destttable].Rows[0]["VolumeUnitID"]);
                            CostObj.DeliveryReport.WeightUnitID = CostDs.Tables[destttable].Rows[0]["WeightUnitID"] == DBNull.Value ? 0 : Convert.ToInt32(CostDs.Tables[destttable].Rows[0]["WeightUnitID"]);
                            CostObj.DeliveryReport.TobePackedVol = CostDs.Tables[destttable].Rows[0]["TobePackedVol"] == DBNull.Value ? (decimal)0.00 : Convert.ToDecimal(CostDs.Tables[destttable].Rows[0]["TobePackedVol"]);
                            CostObj.DeliveryReport.NetVol = CostDs.Tables[destttable].Rows[0]["NetVol"] == DBNull.Value ? (decimal)0.00 : Convert.ToDecimal(CostDs.Tables[destttable].Rows[0]["NetVol"]);
                            CostObj.DeliveryReport.GrossVol = CostDs.Tables[destttable].Rows[0]["GrossVol"] == DBNull.Value ? (decimal)0.00 : Convert.ToDecimal(CostDs.Tables[destttable].Rows[0]["GrossVol"]);
                            CostObj.DeliveryReport.NetWt = CostDs.Tables[destttable].Rows[0]["NetWt"] == DBNull.Value ? (decimal)0.00 : Convert.ToDecimal(CostDs.Tables[destttable].Rows[0]["NetWt"]);
                            CostObj.DeliveryReport.GrossWt = CostDs.Tables[destttable].Rows[0]["GrossWt"] == DBNull.Value ? (decimal)0.00 : Convert.ToDecimal(CostDs.Tables[destttable].Rows[0]["GrossWt"]);
                            CostObj.DeliveryReport.ACWTWt = CostDs.Tables[destttable].Rows[0]["ACWTWt"] == DBNull.Value ? (decimal)0.00 : Convert.ToDecimal(CostDs.Tables[destttable].Rows[0]["ACWTWt"]);
                            CostObj.DeliveryReport.ContainerSizeID = CostDs.Tables[destttable].Rows[0]["ContainerSizeID"] == DBNull.Value ? 0 : Convert.ToInt32(CostDs.Tables[destttable].Rows[0]["ContainerSizeID"]);
                            CostObj.DeliveryReport.CSCreatedBY = CostDs.Tables[destttable].Rows[0]["CSPreparedBy"] == DBNull.Value ? null : Convert.ToString(CostDs.Tables[destttable].Rows[0]["CSPreparedBy"]);
                            CostObj.DeliveryReport.CSCreatedDate = CostDs.Tables[destttable].Rows[0]["CSPreparedDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(CostDs.Tables[destttable].Rows[0]["CSPreparedDate"]);
                            CostObj.DeliveryReport.CSApprovedBY = CostDs.Tables[destttable].Rows[0]["CSApprovedBY"] == DBNull.Value ? null : Convert.ToString(CostDs.Tables[destttable].Rows[0]["CSApprovedBY"]);
                            CostObj.DeliveryReport.CSApprovedDate = CostDs.Tables[destttable].Rows[0]["CSApprovedDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(CostDs.Tables[destttable].Rows[0]["CSApprovedDate"]);
                            CostObj.DeliveryReport.ServiceEvaluation = CostDs.Tables[destttable].Rows[0]["IsServiceEvaluation"] == DBNull.Value ? false : Convert.ToBoolean(CostDs.Tables[destttable].Rows[0]["IsServiceEvaluation"]);
                            CostObj.DeliveryReport.ServiceEvaluationScore = CostDs.Tables[destttable].Rows[0]["ServiceEvaluationScore"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(CostDs.Tables[destttable].Rows[0]["ServiceEvaluationScore"]);
                            CostObj.DeliveryReport.ServiceEvaluationRemarks = CostDs.Tables[destttable].Rows[0]["ServiceEvaluationRemark"] == DBNull.Value ? null : Convert.ToString(CostDs.Tables[destttable].Rows[0]["ServiceEvaluationRemark"]);
                            CostObj.DeliveryReport.DestStgStartDate = CostDs.Tables[destttable].Rows[0]["DestStgStartDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(CostDs.Tables[destttable].Rows[0]["DestStgStartDate"]);
                            CostObj.DeliveryReport.DestStgEndDate = CostDs.Tables[destttable].Rows[0]["DestStgEndDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(CostDs.Tables[destttable].Rows[0]["DestStgEndDate"]);
                            CostObj.DeliveryReport.RoadKMS = CostDs.Tables[destttable].Rows[0]["RoadKMS"] == DBNull.Value ? (decimal)0.00 : Convert.ToDecimal(CostDs.Tables[destttable].Rows[0]["RoadKMS"]);
                            CostObj.DeliveryReport.IsClaim = CostDs.Tables[destttable].Rows[0]["IsClaim"] == DBNull.Value ? false : Convert.ToBoolean(CostDs.Tables[destttable].Rows[0]["IsClaim"]);
                            CostObj.DeliveryReport.StageRemarks = CostDs.Tables[destttable].Rows[0]["StageRemark"] == DBNull.Value ? null : Convert.ToString(CostDs.Tables[destttable].Rows[0]["StageRemark"]);

                            CostObj.IsCSSenttoApprove = CostDs.Tables[destttable].Rows[0]["IsSendToApproval"] == DBNull.Value ? false : Convert.ToBoolean(CostDs.Tables[destttable].Rows[0]["IsSendToApproval"]);
                            CostObj.CSSenttoApproveUser = CostDs.Tables[destttable].Rows[0]["ApprovalUserID"] == DBNull.Value ? (Int32?)0 : Convert.ToInt32(CostDs.Tables[destttable].Rows[0]["ApprovalUserID"]);

                            if (CostObj.MoveJob.ModeID != 2)
                            {
                                CostObj.DeliveryReport.LCLorFCL = CostDs.Tables[destttable].Rows[0]["LCLorFCL"] == DBNull.Value ? null : Convert.ToString(CostDs.Tables[destttable].Rows[0]["LCLorFCL"]);
                                CostObj.DeliveryReport.LooseCased = CostDs.Tables[destttable].Rows[0]["LooseCased"] == DBNull.Value ? null : Convert.ToString(CostDs.Tables[destttable].Rows[0]["LooseCased"]);
                            }
                            else
                            {
                                CostObj.DeliveryReport.LCLorFCL = CostDs.Tables[destttable].Rows[0]["LCLorFCL"] == DBNull.Value ? "LCL" : Convert.ToString(CostDs.Tables[destttable].Rows[0]["LCLorFCL"]);
                                CostObj.DeliveryReport.LooseCased = CostDs.Tables[destttable].Rows[0]["LooseCased"] == DBNull.Value ? "Loose" : Convert.ToString(CostDs.Tables[destttable].Rows[0]["LooseCased"]);
                            }


                        }
                        else
                        {
                            CostObj.DeliveryReport.LCLorFCL = "LCL";
                            CostObj.DeliveryReport.LooseCased = "Loose";
                        }
                        #endregion
                        destttable++;
                        #region Destination Cost   
                        if (CostDs.Tables.Count > destttable && CostDs.Tables[destttable] != null && CostDs.Tables[destttable].Rows.Count > 0)
                        {
                            CostObj.DeliveryCostList.CostListSaved = Convert.ToString(CostDs.Tables[destttable].Rows[0]["IsDeliveryCostSaved"]) == "Y";
                            CostObj.DeliveryCostList.CostList = (from item in CostDs.Tables[destttable].AsEnumerable()
                                                                 select new PackingCostList
                                                                 {
                                                                     RateCompId = item["CompId"] == DBNull.Value ? (int)0 : (Convert.ToInt32(item["CompId"])),
                                                                     RateCompName = Convert.ToString(item["RateComponentName"]),

                                                                     CostHeadID = item["CostHeadID"] == DBNull.Value ? (int?)0 : (Convert.ToInt32(item["CostHeadID"])),

                                                                     CostHeadName = Convert.ToString(item["CostHeadName"]),
                                                                     BaseCurrID = item["BaseCurrID"] == DBNull.Value ? (int)0 : (Convert.ToInt32(item["BaseCurrID"])),

                                                                     RateCurrID = item["RateCurrID"] == DBNull.Value ? (int)0 : (Convert.ToInt32(item["RateCurrID"])),
                                                                     RevRateCurrID = item["RevRateCurrID"] == DBNull.Value ? (int)0 : (Convert.ToInt32(item["RevRateCurrID"])),
                                                                     ConversionRate = item["ConversionRate"] == DBNull.Value ? (decimal?)0 : (Convert.ToDecimal(item["ConversionRate"])),
                                                                     RevConversionRate = item["RevConversionRate"] == DBNull.Value ? (decimal?)0 : (Convert.ToDecimal(item["RevConversionRate"])),
                                                                     CostValue = item["CostValue"] == DBNull.Value ? (decimal?)0 : (Convert.ToDecimal(item["CostValue"])),
                                                                     BaseRevenueValue = item["BaseRev"] == DBNull.Value ? (decimal?)0 : (Convert.ToDecimal(item["BaseRev"])),
                                                                     RevenueValue = item["Revenuevalue"] == DBNull.Value ? (decimal?)0 : (Convert.ToDecimal(item["Revenuevalue"])),
                                                                     Balance = item["UnBilled"] == DBNull.Value ? item["Revenuevalue"] == DBNull.Value ? (decimal?)0 : (Convert.ToDecimal(item["Revenuevalue"])) : (Convert.ToDecimal(item["UnBilled"])),
                                                                     BaseCurr = Convert.ToString(item["BaseCurrName"]),
                                                                     RateCurr = Convert.ToString(item["RateCurrName"]),
                                                                     RevRateCurr = Convert.ToString(item["RevRateCurrName"]),
                                                                     Isactive = true,//item["Isactive"] == DBNull.Value ? (bool)false : (Convert.ToBoolean(item["Isactive"])),
                                                                     ToBill = item["ToBill"] == DBNull.Value ? (bool)false : (Convert.ToBoolean(item["ToBill"])),
                                                                     Per = item["Per"] == DBNull.Value ? (int)0 : (Convert.ToInt32(item["Per"])),
                                                                     WtUnitID = item["WtUnitID"] == DBNull.Value ? (int)0 : (Convert.ToInt32(item["WtUnitID"])),
                                                                     WtUnitName = item["WtUnitName"] == DBNull.Value ? null : (Convert.ToString(item["WtUnitName"])),
                                                                     Rate = item["Rate"] == DBNull.Value ? (decimal?)0 : (Convert.ToDecimal(item["Rate"])),
                                                                     WtVol = item["Wt_Vol_No"] == DBNull.Value ? null : (Convert.ToString(item["Wt_Vol_No"])),
                                                                     RevRate = item["RevRate"] == DBNull.Value ? 0 : Convert.ToDecimal(item["RevRate"])
                                                                     //unbill = item["UnBilled"] == DBNull.Value ? null : (Convert.ToString(item["UnBilled"])),
                                                                     /*ConversionRate = item["BaseCurrConversRate"] == DBNull.Value ? 0 : Convert.ToDecimal(item["BaseCurrConversRate"]),
																	 Amount = item["RateCurrValue"]==DBNull.Value ? 0 :  Convert.ToDecimal(item["RateCurrValue"]),*/
                                                                 }).ToList();
                        }
                        #endregion


                    }
                    #endregion Destination Info

                    #region RMCFees
                    destttable++;
                    if (CostDs.Tables.Count > destttable && CostDs.Tables[destttable] != null && CostDs.Tables[destttable].Rows.Count > 0)
                    {
                        CostObj.RMCFees = CostDs.Tables[destttable].AsEnumerable()
                            .Select(dataRow => new RmcFees
                            {
                                BAFlag = Convert.ToChar(dataRow["AddBeforeOrAfteSFR"]),
                                CostHeadId = Convert.ToString(dataRow["AddCostForBillId"]),
                                CostHeadName = Convert.ToString(dataRow["CostHeadName"]),
                                Amount = dataRow["AmtToAdd"] == DBNull.Value ? 0 : Convert.ToDecimal(dataRow["AmtToAdd"]),
                                Percent = dataRow["PercentToAdd"] == DBNull.Value ? 0 : Convert.ToDecimal(dataRow["PercentToAdd"])
                            }).ToList();
                    }
                    #endregion RMCFees

                    #region Job Document

                    if (MoveID != null)
                    {
                        DataSet DocDs = moveMangeDAL.GetEmailNDdocuments(Convert.ToInt32(MoveID));
                        JobDocUpload jobDoc = new JobDocUpload();

                        if (DocDs != null && DocDs.Tables.Count >= 1)
                        {

                            if (DocDs.Tables.Count > 1 && DocDs.Tables[1] != null && DocDs.Tables[1].Rows.Count > 0)
                            {
                                jobDoc.docLists = (from item in DocDs.Tables[1].AsEnumerable()
                                                   select new JobDocument()
                                                   {
                                                       FileID = Convert.ToInt32(item["FileID"]),
                                                       DocTypeID = Convert.ToInt32(item["DocTypeID"]),
                                                       DocNameID = Convert.ToInt32(item["DocNameID"]),
                                                       DocType = Convert.ToString(item["DocTypeName"]),
                                                       DocName = Convert.ToString(item["DocName"]),
                                                       DocDescription = Convert.ToString(item["Description"]),
                                                       FileName = Convert.ToString(item["DocFileName"]),
                                                       UploadBy = Convert.ToString(item["UploadBy"]),
                                                       UploadById = Convert.ToInt32(item["CreatedBy"]),
                                                       UploadDate = Convert.ToDateTime(item["CreatedDate"]),
                                                       InvRefNo = Convert.ToString(item["InvRefNo"]),
                                                       ///FilePath = CommonSubs.EncryptSf(Convert.ToString(item["DocFilePath"]))
                                                   }).ToList();
                            }

                        }

                        CostObj.jobDocUpload = jobDoc;
                    }

                    #endregion

                    #region FollowUpDetails
                    destttable++;
                    if (CostDs.Tables.Count > destttable && CostDs.Tables[destttable] != null && CostDs.Tables[destttable].Rows.Count > 0)
                    {
                        CostObj.FollowUpList = CostDs.Tables[destttable].AsEnumerable()
                            .Select(dataRow => new FollowUpDetails
                            {
                                FollowUpDate = dataRow["FollowUpDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataRow["FollowUpDate"]),
                                FollowUpRemark = Convert.ToString(dataRow["FollowupRemarks"]),
                                CreatedBy = Convert.ToString(dataRow["CreatedBy"]),
                                CreatedDate = dataRow["CreatedDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataRow["CreatedDate"])
                            }).ToList();
                    }
                    #endregion

                    #region CloseJobDetails
                    destttable++;
                    if (CostDs.Tables.Count > destttable && CostDs.Tables[destttable] != null && CostDs.Tables[destttable].Rows.Count > 0)
                    {
                        CostObj.CloseJobRemark = CostDs.Tables[destttable].Rows[0]["JobCloseRemark"] == DBNull.Value ? null : Convert.ToString(CostDs.Tables[destttable].Rows[0]["JobCloseRemark"]);
                        CostObj.CloseJobBy = CostDs.Tables[destttable].Rows[0]["JobCloseBy"] == DBNull.Value ? null : Convert.ToString(CostDs.Tables[destttable].Rows[0]["JobCloseBy"]);
                        CostObj.CloseJobDate = CostDs.Tables[destttable].Rows[0]["JobCloseDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(CostDs.Tables[destttable].Rows[0]["JobCloseDate"]);
                    }
                    #endregion

                    #region InsuranceDetails
                    destttable++;
                    CostObj.Insurance.IsSendForInsurance = false;
                    if (CostDs.Tables.Count > destttable && CostDs.Tables[destttable] != null && CostDs.Tables[destttable].Rows.Count > 0)
                    {
                        CostObj.Insurance.InsPackDate = CostDs.Tables[destttable].Rows[0]["PackDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(CostDs.Tables[destttable].Rows[0]["PackDate"]);
                        CostObj.Insurance.InsuranceValue = CostDs.Tables[destttable].Rows[0]["InsuranceValue"] == DBNull.Value ? 0 : Convert.ToDecimal(CostDs.Tables[destttable].Rows[0]["InsuranceValue"]);
                        CostObj.Insurance.PremiumRate = CostDs.Tables[destttable].Rows[0]["PremiumRate"] == DBNull.Value ? 0 : Convert.ToDecimal(CostDs.Tables[destttable].Rows[0]["PremiumRate"]);
                        CostObj.Insurance.IDVCarValue = CostDs.Tables[destttable].Rows[0]["IDV_value_Car"] == DBNull.Value ? 0 : Convert.ToDecimal(CostDs.Tables[destttable].Rows[0]["IDV_value_Car"]);
                        CostObj.Insurance.VehMakeModel = CostDs.Tables[destttable].Rows[0]["VehMakeModel"] == DBNull.Value ? null : Convert.ToString(CostDs.Tables[destttable].Rows[0]["VehMakeModel"]);
                        CostObj.Insurance.InsuranceCurrID = CostDs.Tables[destttable].Rows[0]["CurrID"] == DBNull.Value ? (int?)null : Convert.ToInt32(CostDs.Tables[destttable].Rows[0]["CurrID"]);
                        CostObj.Insurance.IsSendForInsurance = CostDs.Tables[destttable].Rows[0]["SendForInsurance"] == DBNull.Value ? false : Convert.ToBoolean(CostDs.Tables[destttable].Rows[0]["SendForInsurance"]);
                    }
                    #endregion

                    #region AdvanceCaution
                    destttable++;
                    if (CostDs.Tables.Count > destttable && CostDs.Tables[destttable] != null && CostDs.Tables[destttable].Rows.Count > 0)
                    {
                        CostObj.ShowAdvanceCaution = Convert.ToBoolean(CostDs.Tables[destttable].Rows[0]["ShowAdvanceCaution"]);
                    }
                    #endregion

                    #region TabList
                    destttable++;
                    if (CostDs.Tables.Count > destttable && CostDs.Tables[destttable] != null && CostDs.Tables[destttable].Rows.Count > 0)
                    {
                        //CostObj.ShowAdvanceCaution = Convert.ToBoolean(CostDs.Tables[destttable].Rows[0]["ShowAdvanceCaution"]);
                        for (int i = 0; i < CostDs.Tables[destttable].Columns.Count; i++)
                        {

                            TabList tab = new TabList();
                            tab.TabIndex = Convert.ToInt32(CostDs.Tables[destttable].Rows[0][i].ToString());
                            //if (tab.TabIndex !=0)
                            //{
                            //	
                            //}
                            CostObj.TabList.Add(tab);

                        }
                    }
                    #endregion

                    #region TabList
                    destttable++;
                    if (CostDs.Tables.Count > destttable && CostDs.Tables[destttable] != null && CostDs.Tables[destttable].Rows.Count > 0)
                    {
                        //CostObj.ShowAdvanceCaution = Convert.ToBoolean(CostDs.Tables[destttable].Rows[0]["ShowAdvanceCaution"]);
                        //for (int i = 0; i < CostDs.Tables[destttable].Columns.Count; i++)
                        {

                            //TranShipmentWtVol trnDet = new TranShipmentWtVol();
                            CostObj.FreightReport.TranShipmentWtVolList = CostDs.Tables[destttable].AsEnumerable()
                            .Select(dataRow => new TranShipmentWtVol
                            {
                                BLReleaseOn = dataRow["BLReleaseOn"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataRow["BLReleaseOn"]),
                                Bill_No = Convert.ToString(dataRow["BillNo"]),
                                SealNo = Convert.ToString(dataRow["SealNo"]),
                                ContainerNo = Convert.ToString(dataRow["ContainerNo"]),
                                NoOfPacks = dataRow["NoOfPacks"] == DBNull.Value ? (int)0 : (Convert.ToInt32(dataRow["NoOfPacks"])),
                                WtVol = dataRow["WtVol"] == DBNull.Value ? 0 : Convert.ToDecimal(dataRow["WtVol"]),
                                ContainerTypeId = dataRow["ContainerTypeID"] == DBNull.Value ? (int)0 : (Convert.ToInt32(dataRow["ContainerTypeID"])),
                                AirLineID = dataRow["AirLineID"] == DBNull.Value ? (int)0 : (Convert.ToInt32(dataRow["AirLineID"])),
                                CourierID = dataRow["CourierID"] == DBNull.Value ? (int)0 : (Convert.ToInt32(dataRow["CourierID"])),
                                Courier = Convert.ToString(dataRow["CourierName"]),
                                AirLine = Convert.ToString(dataRow["ShipLineName"]),
                                ContainerType = Convert.ToString(dataRow["ContainerType"]),
                                WtVolUnit = Convert.ToString(dataRow["WeightUnitName"]),
                                LCLorFCL = Convert.ToString(dataRow["FCLLcl"]),
                                WtVolUnitId = dataRow["WtUnitID"] == DBNull.Value ? (int?)null : (Convert.ToInt32(dataRow["WtUnitID"])),
                                VolUnit = Convert.ToString(dataRow["VolUnitName"]),
                                VolUnitId = dataRow["VolUnitID"] == DBNull.Value ? (int?)null : (Convert.ToInt32(dataRow["VolUnitID"])),
                                Vol = dataRow["VolVal"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(dataRow["VolVal"]),
                                Length = dataRow["Length"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(dataRow["Length"]),
                                Breadth = dataRow["Breadth"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(dataRow["Breadth"]),
                                Height = dataRow["Height"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(dataRow["Height"]),
                                Isactive = true,
                                MasterID = string.IsNullOrEmpty(Convert.ToString(dataRow["TransInvMasterID"])) ? 0 : Convert.ToInt64(dataRow["TransInvMasterID"]),
                                MoveID = string.IsNullOrEmpty(Convert.ToString(dataRow["MoveID"])) ? 0 : Convert.ToInt64(dataRow["MoveID"]),
                                JobNo = Convert.ToString(dataRow["JobID"]),
                                Shipper = Convert.ToString(dataRow["ShipperName"]),
                                GrossWt = dataRow["GrossWt"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(dataRow["GrossWt"]),
                                ACWTWt = dataRow["ACWT"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(dataRow["ACWT"]),
                            }).ToList();

                            if (CostObj.FreightReport.TranShipmentWtVolList.First().MasterID > 0)
                            {
                                CostObj.FreightReport.TransInvMasterID = CostObj.FreightReport.TranShipmentWtVolList.First().MasterID;
                            }

                            //if(CostObj.FreightReport.TranShipmentWtVolList!=null &&  CostObj.FreightReport.TranShipmentWtVolList.Count > 0)
                            //{
                            //    var BLobj = CostObj.FreightReport.TranShipmentWtVolList.OrderByDescending(t => t.BLReleaseOn).First(); // CostObj.FreightReport.TranShipmentWtVolList.FirstOrDefault();
                            //    CostObj.FreightReport.Bill_No = BLobj.Bill_No.Trim();
                            //    CostObj.FreightReport.BLReleaseOn = BLobj.BLReleaseOn;
                            //}

                            //if (tab.TabIndex !=0)
                            //{
                            //	CostObj.TabList.Add(tab);
                            //}

                        }
                    }
                    #endregion

                    #region Agent Evaluation

                    if (MoveID != null)
                    {
                        CostObj.vendorEvaluation = moveMangeDAL.GetVendorEvaluation(Convert.ToInt64(MoveID));
                    }

                    #endregion

                    #region Transit Invoice & Job
                    destttable++;
                    if (CostDs.Tables.Count > destttable && CostDs.Tables[destttable] != null && CostDs.Tables[destttable].Rows.Count > 0)
                    {
                        CostObj.FreightReport.transhipInvoiceJobs = CostDs.Tables[destttable].AsEnumerable()
                            .Select(dataRow => new TranshipInvoiceJobs
                            {

                                MoveId = Convert.ToInt64(dataRow["MoveID"]),
                                JobNo = Convert.ToString(dataRow["JobID"]),
                                JobAmt = string.IsNullOrWhiteSpace(Convert.ToString(dataRow["InvoiceAmt"])) ? 0 : Convert.ToDecimal(dataRow["InvoiceAmt"]),
                                Remark = Convert.ToString(dataRow["Remark"]),
                            }).ToList();

                        //CostObj.FreightReport.TransitDistMoveIDList = CostObj.FreightReport.transhipInvoiceJobs.Select(m => m.MoveId).ToArray<Int64>();
                    }

                    destttable++;
                    if (CostDs.Tables.Count > destttable && CostDs.Tables[destttable] != null && CostDs.Tables[destttable].Rows.Count > 0)
                    {
                        CostObj.FreightReport.transhipInvoices = CostDs.Tables[destttable].AsEnumerable()
                            .Select(dataRow => new TranshipInvoice
                            {
                                InvoiceId = dataRow["TransInvDetID"] == DBNull.Value ? (Int64?)null : Convert.ToInt64(dataRow["TransInvDetID"]),
                                InvoiceTypeId = Convert.ToInt32(dataRow["InvTypeID"]),
                                InvoiceType = Convert.ToString(dataRow["InvTypeName"]),
                                InvoiceNo = Convert.ToString(dataRow["InvoiceNo"]),
                                InvoiceDate = Convert.ToDateTime(dataRow["InvoiceDate"]),
                                InvoiceAmt = Convert.ToDecimal(dataRow["InvoiceAmount"]),
                                CurrID = Convert.ToInt32(dataRow["CurrencyID"]),
                                Currancy = Convert.ToString(dataRow["CurrencyAbbrvation"]),
                                Remark = Convert.ToString(dataRow["Remark"]),
                                MasterID = Convert.ToInt64(dataRow["TransInvMasterID"]),
                                InvCredit = Convert.ToString(dataRow["InvCredit"]),
                                InvCreditName = Convert.ToString(dataRow["InvCreditName"]),
                                FirstInvID = dataRow["FirstInvID"] == DBNull.Value ? (Int64?)null : Convert.ToInt64(dataRow["FirstInvID"]),

                            }).ToList();

                        CostObj.FreightReport.TransInvMasterID = CostObj.FreightReport.transhipInvoices.First().MasterID;
                    }
                    #endregion

                    #region Forwrding Details
                    destttable++;
                    if (CostDs.Tables.Count > destttable && CostDs.Tables[destttable] != null && CostDs.Tables[destttable].Rows.Count > 0)
                    {
                        CostObj.ForwardingFlag = CostDs.Tables[destttable].Rows[0]["ButtonParam"] == DBNull.Value ? null : Convert.ToString(CostDs.Tables[destttable].Rows[0]["ButtonParam"]);

                        //CostObj.FreightReport.TransitDistMoveIDList = CostObj.FreightReport.transhipInvoiceJobs.Select(m => m.MoveId).ToArray<Int64>();
                    }
                    #endregion

                    #region GP Approval Details
                    destttable++;
                    if (CostDs.Tables.Count > destttable && CostDs.Tables[destttable] != null && CostDs.Tables[destttable].Rows.Count > 0)
                    {
                        CostObj.GPApprovalDisplayList = (from item in CostDs.Tables[destttable].AsEnumerable()
                                                         select new GPApprovalDisplayList
                                                         {
                                                             Remark = Convert.ToString(item["Remark"]),
                                                             Status = Convert.ToString(item["Status"]),
                                                             CreatedBy = Convert.ToString(item["CreatedBy"]),
                                                             CreatedDate = Convert.ToString(item["CreatedDate"]),
                                                             GPPercent = Convert.ToString(item["GPPercent"]),
                                                             RevAmount = Convert.ToString(item["GPAmount"]),
                                                             BaseCurr = Convert.ToString(item["BaseCurr"]),
                                                             Stage = Convert.ToString(item["Stage"]),
                                                         }).ToList();
                    }
                    #endregion

                    if (!CostObj.IsShowCloseJob)
                    {
                        CostObj.IsShowCloseJob = CostObj.DeliveryCostList.CostList.Sum(x => x.Balance) == 0;
                    }
                    if (!CostObj.IsInvPrepared)
                    {
                        CostObj.IsShowCloseJob = false;
                    }
                }

            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "MoveManageBL", "GetDetailById", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

            return CostObj;
        }


        public GPApproval GetDetailByIdForGPApproval(int SurveyID, int? MoveID, int IsRMCBuss, int LoginID)
        {
            GPApproval CostObj = new GPApproval();
            try
            {
                CostObj.IsDestApprove = false;
                CostObj.DestApprove = CostObj.IsDestApprove ? "Approved" : "Pending";
                bool orgFlag = false, frtFlag = false, destFlag = false;


                DataSet CostDs = moveMangeDAL.GetDetailById(LoginID, SurveyID, MoveID, IsRMCBuss, false, false, false);
                if (CostDs != null && CostDs.Tables.Count >= 1)
                {
                    CostObj.MoveID = MoveID;
                    CostObj.SurveyID = SurveyID;
                    CostObj.IsSOCost = false;

                    #region Job Opening Details
                    if (CostDs.Tables.Count > 0 && CostDs.Tables[0] != null && CostDs.Tables[0].Rows.Count > 0)
                    {
                        CostObj.SurveyID = CostDs.Tables[0].Rows[0]["SurveyID"] == DBNull.Value ? (Int64?)null : Convert.ToInt64(CostDs.Tables[0].Rows[0]["SurveyID"]);
                        CostObj.EnqID = CostDs.Tables[0].Rows[0]["EnqID"] == DBNull.Value ? (Int64?)null : Convert.ToInt64(CostDs.Tables[0].Rows[0]["EnqID"]);
                        CostObj.EnqNo = Convert.ToString(CostDs.Tables[0].Rows[0]["EnqNo"]);
                        CostObj.EnqShpNo = CostDs.Tables[0].Rows[0]["EnqSeqNo"] == DBNull.Value ? (Int64?)null : Convert.ToInt64(CostDs.Tables[0].Rows[0]["EnqSeqNo"]);
                        CostObj.ServiceLineID = CostDs.Tables[0].Rows[0]["ServiceLineID"] == DBNull.Value ? (Int32?)null : Convert.ToInt32(CostDs.Tables[0].Rows[0]["ServiceLineID"]);
                        CostObj.ServiceLine = Convert.ToString(CostDs.Tables[0].Rows[0]["ServiceLine"]);
                        CostObj.EnqDetailID = CostDs.Tables[0].Rows[0]["EnqDetailID"] == DBNull.Value ? (Int64?)null : Convert.ToInt64(CostDs.Tables[0].Rows[0]["EnqDetailID"]);
                        CostObj.MoveJob.WeightUnitID = Convert.ToInt32(CostDs.Tables[0].Rows[0]["WeightUnitID"]);
                        CostObj.MoveJob.WeightUnitFrom = Convert.ToInt64(CostDs.Tables[0].Rows[0]["WeightFrom"]);
                        CostObj.MoveJob.WeightUnitTo = Convert.ToInt64(CostDs.Tables[0].Rows[0]["WeightTo"]);
                        CostObj.MoveJob.FromLocationID = Convert.ToInt32(CostDs.Tables[0].Rows[0]["OrgCityID"]);
                        CostObj.MoveJob.ToLocationID = Convert.ToInt32(CostDs.Tables[0].Rows[0]["DestCityID"]);
                        CostObj.MoveJob.ExitPointID = CostDs.Tables[0].Rows[0]["OrgPortID"] == DBNull.Value ? 0 : Convert.ToInt32(CostDs.Tables[0].Rows[0]["OrgPortID"]);
                        CostObj.MoveJob.EntryPointID = CostDs.Tables[0].Rows[0]["DestPortID"] == DBNull.Value ? 0 : Convert.ToInt32(CostDs.Tables[0].Rows[0]["DestPortID"]);
                        CostObj.MoveJob.ModeID = Convert.ToInt32(CostDs.Tables[0].Rows[0]["ModeID"]);
                        CostObj.MoveJob.RMCID = Convert.ToInt32(CostDs.Tables[0].Rows[0]["RMCID"]);
                        CostObj.MoveJob.RMCName = Convert.ToString(CostDs.Tables[0].Rows[0]["RMCName"]);
                        CostObj.MoveJob.BusinessLineID = Convert.ToInt32(CostDs.Tables[0].Rows[0]["BussinessLineID"]);
                        CostObj.MoveJob.GoodsDescriptionID = Convert.ToInt32(CostDs.Tables[0].Rows[0]["GoodsDescID"]);
                        CostObj.MoveJob.ShipingLineID = CostDs.Tables[0].Rows[0]["ShipinglineID"] == DBNull.Value ? (int?)null : Convert.ToInt32(CostDs.Tables[0].Rows[0]["ShipinglineID"]);
                        CostObj.MoveJob.ShipmentTypeID = CostDs.Tables[0].Rows[0]["ShipmentTypeID"] == DBNull.Value ? 0 : Convert.ToInt32(CostDs.Tables[0].Rows[0]["ShipmentTypeID"]);
                        CostObj.JobNo = Convert.ToString(CostDs.Tables[0].Rows[0]["JobId"]);
                        CostObj.JobDate = DateTime.Now;//Convert.ToDateTime(CostDs.Tables[0].Rows[0]["JobDate"]);
                        CostObj.MoveJob.ModeName = Convert.ToString(CostDs.Tables[0].Rows[0]["ModeName"]);
                        CostObj.MoveJob.TentativeMoveDate = CostDs.Tables[0].Rows[0]["TendativeMoveDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(CostDs.Tables[0].Rows[0]["TendativeMoveDate"]);

                        /////Billing and collection paramters
                        CostObj.MoveJob.BillingOnClientId = CostDs.Tables[0].Rows[0]["billingonClientID"] == DBNull.Value ? (int?)null : Convert.ToInt32(CostDs.Tables[0].Rows[0]["billingonClientID"]);
                        CostObj.MoveJob.ClientId = CostDs.Tables[0].Rows[0]["AgentID"] == DBNull.Value ? (int?)0 : Convert.ToInt32(CostDs.Tables[0].Rows[0]["AgentID"]);
                        CostObj.MoveJob.AccountId = CostDs.Tables[0].Rows[0]["AccountId"] == DBNull.Value ? (int?)null : Convert.ToInt32(CostDs.Tables[0].Rows[0]["AccountId"]);
                        CostObj.MoveJob.Shipper.Title = Convert.ToString(CostDs.Tables[0].Rows[0]["ShipperTitle"]);
                        CostObj.MoveJob.Shipper.ShipperFName = Convert.ToString(CostDs.Tables[0].Rows[0]["ShipperFName"]);
                        CostObj.MoveJob.Shipper.ShipperLName = Convert.ToString(CostDs.Tables[0].Rows[0]["ShipperLName"]);
                        CostObj.MoveJob.Shipper.Address1 = Convert.ToString(CostDs.Tables[0].Rows[0]["Address1"]);
                        CostObj.MoveJob.Shipper.Address2 = Convert.ToString(CostDs.Tables[0].Rows[0]["Address2"]);
                        CostObj.MoveJob.Shipper.Email = Convert.ToString(CostDs.Tables[0].Rows[0]["Email"]);
                        CostObj.MoveJob.Shipper.AddressCityID = CostDs.Tables[0].Rows[0]["AddressCityId"] == DBNull.Value ? (int?)null : Convert.ToInt32(CostDs.Tables[0].Rows[0]["AddressCityId"]);
                        CostObj.MoveJob.Shipper.PIN = Convert.ToString(CostDs.Tables[0].Rows[0]["PIN"]);
                        CostObj.MoveJob.Shipper.Phone1 = Convert.ToString(CostDs.Tables[0].Rows[0]["Phone1"]);
                        CostObj.MoveJob.Shipper.Phone2 = Convert.ToString(CostDs.Tables[0].Rows[0]["Phone2"]);
                        CostObj.MoveJob.Shipper.ShipCategoryID = CostDs.Tables[0].Rows[0]["ShipCategoryID"] == DBNull.Value ? (int?)null : Convert.ToInt32(CostDs.Tables[0].Rows[0]["ShipCategoryID"]);
                        CostObj.MoveJob.Shipper.DOB = CostDs.Tables[0].Rows[0]["ShipperDOB"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(CostDs.Tables[0].Rows[0]["ShipperDOB"]);
                        CostObj.MoveJob.Shipper.Designation = Convert.ToString(CostDs.Tables[0].Rows[0]["ShipperDesig"]);
                        CostObj.MoveJob.Shipper.Nationality = Convert.ToString(CostDs.Tables[0].Rows[0]["ShipperNationality"]);
                        /////Copied the values of Job to Cost head
                        CostObj.MoveCostMst.WeightUnitID = Convert.ToInt32(CostDs.Tables[0].Rows[0]["WeightUnitID"]);
                        CostObj.MoveCostMst.WeightUnitFrom = Convert.ToInt64(CostDs.Tables[0].Rows[0]["WeightFrom"]);
                        CostObj.MoveCostMst.WeightUnitTo = Convert.ToInt64(CostDs.Tables[0].Rows[0]["WeightTo"]);
                        CostObj.MoveCostMst.FromLocationID = Convert.ToInt32(CostDs.Tables[0].Rows[0]["OrgCityID"]);
                        CostObj.MoveCostMst.ToLocationID = Convert.ToInt32(CostDs.Tables[0].Rows[0]["DestCityID"]);
                        CostObj.MoveCostMst.ExitPointID = CostDs.Tables[0].Rows[0]["OrgPortID"] == DBNull.Value ? 0 : Convert.ToInt32(CostDs.Tables[0].Rows[0]["OrgPortID"]);
                        CostObj.MoveCostMst.EntryPointID = CostDs.Tables[0].Rows[0]["DestPortID"] == DBNull.Value ? 0 : Convert.ToInt32(CostDs.Tables[0].Rows[0]["DestPortID"]);
                        CostObj.MoveCostMst.ModeID = Convert.ToInt32(CostDs.Tables[0].Rows[0]["ModeID"]);
                        CostObj.MoveCostMst.RMCID = Convert.ToInt32(CostDs.Tables[0].Rows[0]["RMCID"]);
                        CostObj.MoveCostMst.BusinessLineID = Convert.ToInt32(CostDs.Tables[0].Rows[0]["BussinessLineID"]);
                        CostObj.MoveCostMst.GoodsDescriptionID = Convert.ToInt32(CostDs.Tables[0].Rows[0]["GoodsDescID"]);
                        CostObj.MoveCostMst.ShipingLineID = CostDs.Tables[0].Rows[0]["ShipinglineID"] == DBNull.Value ? (int?)null : Convert.ToInt32(CostDs.Tables[0].Rows[0]["ShipinglineID"]);

                        CostObj.MoveJob.OrgAdd = CostDs.Tables[0].Rows[0]["OrgAdd"] == DBNull.Value ? null : Convert.ToString(CostDs.Tables[0].Rows[0]["OrgAdd"]);
                        CostObj.MoveJob.OrgAdd2 = CostDs.Tables[0].Rows[0]["OrgAdd2"] == DBNull.Value ? null : Convert.ToString(CostDs.Tables[0].Rows[0]["OrgAdd2"]);
                        CostObj.MoveJob.DestAdd = CostDs.Tables[0].Rows[0]["DestAdd"] == DBNull.Value ? null : Convert.ToString(CostDs.Tables[0].Rows[0]["DestAdd"]);
                        CostObj.MoveJob.DestAdd2 = CostDs.Tables[0].Rows[0]["DestAdd2"] == DBNull.Value ? null : Convert.ToString(CostDs.Tables[0].Rows[0]["DestAdd2"]);
                        CostObj.MoveJob.OrgCityID = CostDs.Tables[0].Rows[0]["SOrgCityID"] == DBNull.Value ? (int?)null : Convert.ToInt32(CostDs.Tables[0].Rows[0]["SOrgCityID"]);
                        CostObj.MoveJob.DestCityID = CostDs.Tables[0].Rows[0]["SDestCityID"] == DBNull.Value ? (int?)null : Convert.ToInt32(CostDs.Tables[0].Rows[0]["SDestCityID"]);
                        CostObj.MoveJob.OrgEmail = CostDs.Tables[0].Rows[0]["OrgEmail"] == DBNull.Value ? null : Convert.ToString(CostDs.Tables[0].Rows[0]["OrgEmail"]);
                        CostObj.MoveJob.DestEmail = CostDs.Tables[0].Rows[0]["DestEmail"] == DBNull.Value ? null : Convert.ToString(CostDs.Tables[0].Rows[0]["DestEmail"]);
                        CostObj.MoveJob.OrgPhone = CostDs.Tables[0].Rows[0]["OrgPhone"] == DBNull.Value ? null : Convert.ToString(CostDs.Tables[0].Rows[0]["OrgPhone"]);
                        CostObj.MoveJob.DestPhone = CostDs.Tables[0].Rows[0]["DestPhone"] == DBNull.Value ? null : Convert.ToString(CostDs.Tables[0].Rows[0]["DestPhone"]);
                        CostObj.MoveJob.OrgPin = CostDs.Tables[0].Rows[0]["OrgPin"] == DBNull.Value ? null : Convert.ToString(CostDs.Tables[0].Rows[0]["OrgPin"]);
                        CostObj.MoveJob.DestPin = CostDs.Tables[0].Rows[0]["DestPin"] == DBNull.Value ? null : Convert.ToString(CostDs.Tables[0].Rows[0]["DestPin"]);
                        CostObj.MoveJob.OrgAdd = CostDs.Tables[0].Rows[0]["OrgAdd"] == DBNull.Value ? null : Convert.ToString(CostDs.Tables[0].Rows[0]["OrgAdd"]);
                        CostObj.MoveJob.RMCFileNo = CostDs.Tables[0].Rows[0]["RMCFileNo"] == DBNull.Value ? null : Convert.ToString(CostDs.Tables[0].Rows[0]["RMCFileNo"]);
                        CostObj.MoveJob.WKNo = CostDs.Tables[0].Rows[0]["WorkOrderNo"] == DBNull.Value ? null : Convert.ToString(CostDs.Tables[0].Rows[0]["WorkOrderNo"]);
                        CostObj.MoveJob.MoveCoordinatorID = CostDs.Tables[0].Rows[0]["JobAssignedToID"] == DBNull.Value ? (int?)0 : Convert.ToInt32(CostDs.Tables[0].Rows[0]["JobAssignedToID"]);
                        CostObj.MoveJob.AssistingMoveCoordinatorID = CostDs.Tables[0].Rows[0]["JobAssitedToID"] == DBNull.Value ? (int?)0 : Convert.ToInt32(CostDs.Tables[0].Rows[0]["JobAssitedToID"]);
                        CostObj.UpdatedBatchId = CostDs.Tables[0].Rows[0]["UpdatedBatchID"] == DBNull.Value ? (int?)0 : Convert.ToInt32(CostDs.Tables[0].Rows[0]["UpdatedBatchID"]);
                        CostObj.CombinationID = CostDs.Tables[0].Rows[0]["CombinationID"] == DBNull.Value ? null : Convert.ToString(CostDs.Tables[0].Rows[0]["CombinationID"]);
                        CostObj.MoveJob.ContainerTypeID = CostDs.Tables[0].Rows[0]["ContainerTypeID"] == DBNull.Value ? null : (int?)Convert.ToInt32(CostDs.Tables[0].Rows[0]["ContainerTypeID"]);
                        CostObj.RMCType = CostDs.Tables[0].Rows[0]["RMCType"] == DBNull.Value ? null : Convert.ToString(CostDs.Tables[0].Rows[0]["RMCType"]);
                        CostObj.OldJobNo = CostDs.Tables[0].Rows[0]["OldCbsJobNo"] == DBNull.Value ? null : Convert.ToString(CostDs.Tables[0].Rows[0]["OldCbsJobNo"]);
                        CostObj.HideSurveySave = Convert.ToBoolean(CostDs.Tables[0].Rows[0]["HideSurveySave"]);
                        CostObj.InsurBy = CostDs.Tables[0].Rows[0]["InsurBy"] == DBNull.Value ? 0 : Convert.ToInt32(CostDs.Tables[0].Rows[0]["InsurBy"]);
                        CostObj.HoSdEmpID = CostDs.Tables[0].Rows[0]["HoSdEmpID"] == DBNull.Value ? (int?)0 : Convert.ToInt32(CostDs.Tables[0].Rows[0]["HoSdEmpID"]);
                        CostObj.BrSdEmpID = CostDs.Tables[0].Rows[0]["BrSdEmpID"] == DBNull.Value ? (int?)0 : Convert.ToInt32(CostDs.Tables[0].Rows[0]["BrSdEmpID"]);
                        CostObj.DestBrSdEmpID = CostDs.Tables[0].Rows[0]["DestBrSdEmpID"] == DBNull.Value ? (int?)0 : Convert.ToInt32(CostDs.Tables[0].Rows[0]["DestBrSdEmpID"]);
                        CostObj.Project = CostDs.Tables[0].Rows[0]["Project"] == DBNull.Value ? null : Convert.ToString(CostDs.Tables[0].Rows[0]["Project"]);
                        CostObj.MoveJob.RevenueBr = CostDs.Tables[0].Rows[0]["RevenueBr"] == DBNull.Value ? null : Convert.ToString(CostDs.Tables[0].Rows[0]["RevenueBr"]);
                        CostObj.MoveJob.ShowGetCost = Convert.ToBoolean(CostDs.Tables[0].Rows[0]["ShowGetCost"]);
                        CostObj.MoveJob.FromLocationName = Convert.ToString(CostDs.Tables[0].Rows[0]["OrgCityName"]);
                        CostObj.MoveJob.ToLocationName = Convert.ToString(CostDs.Tables[0].Rows[0]["DestCityName"]);
                        CostObj.JobStatus = Convert.ToString(CostDs.Tables[0].Rows[0]["JobStatus"]);
                        CostObj.JobCancel.CancelRemark = Convert.ToString(CostDs.Tables[0].Rows[0]["CancelRemark"]);
                        CostObj.ClientType = Convert.ToString(CostDs.Tables[0].Rows[0]["ClientType"]);

                        CostObj.Cheque_Amt = Convert.ToString(CostDs.Tables[0].Rows[0]["Cheque_Amt"]);
                        CostObj.Cheque_No = Convert.ToString(CostDs.Tables[0].Rows[0]["Cheque_No"]);
                        CostObj.Cheque_Remark = Convert.ToString(CostDs.Tables[0].Rows[0]["Cheque_Remark"]);
                        CostObj.SurveyerID = CostDs.Tables[0].Rows[0]["SchSurveyorID"] == DBNull.Value ? (int?)null : Convert.ToInt32(CostDs.Tables[0].Rows[0]["SchSurveyorID"]);
                        CostObj.ShowSendToMobile = Convert.ToInt16(CostDs.Tables[0].Rows[0]["ShowSendToMobile"]);
                        CostObj.ISDeliveryDateValid = Convert.ToBoolean(CostDs.Tables[0].Rows[0]["ISDeliveryDateValid"]);

                        //Insurance Details
                        CostObj.insuranceDetail.ContactPerson = CostDs.Tables[0].Rows[0]["InsContactPerson"] == DBNull.Value ? null : Convert.ToString(CostDs.Tables[0].Rows[0]["InsContactPerson"]);
                        CostObj.insuranceDetail.ContactNumber = CostDs.Tables[0].Rows[0]["InsContactNumber"] == DBNull.Value ? null : Convert.ToString(CostDs.Tables[0].Rows[0]["InsContactNumber"]);
                        CostObj.insuranceDetail.EmailID = CostDs.Tables[0].Rows[0]["InsEmailID"] == DBNull.Value ? null : Convert.ToString(CostDs.Tables[0].Rows[0]["InsEmailID"]);
                        CostObj.insuranceDetail.FinancePerson = CostDs.Tables[0].Rows[0]["InsFinancePerson"] == DBNull.Value ? null : Convert.ToString(CostDs.Tables[0].Rows[0]["InsFinancePerson"]);
                        CostObj.insuranceDetail.InsuranceValueAmount = CostDs.Tables[0].Rows[0]["InsuranceValueAmount"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(CostDs.Tables[0].Rows[0]["InsuranceValueAmount"]);
                        CostObj.insuranceDetail.InsuranceValueCurrency = CostDs.Tables[0].Rows[0]["InsuranceValueCurrency"] == DBNull.Value ? (int?)null : Convert.ToInt32(CostDs.Tables[0].Rows[0]["InsuranceValueCurrency"]);
                        CostObj.insuranceDetail.InsuranceBreakdown = CostDs.Tables[0].Rows[0]["InsuranceBreakdown"] == DBNull.Value ? 0 : Convert.ToInt32(CostDs.Tables[0].Rows[0]["InsuranceBreakdown"]);
                        CostObj.insuranceDetail.BreakdownInsurance = CostDs.Tables[0].Rows[0]["BreakdownInsuranceDMS"] == DBNull.Value ? 0 : Convert.ToInt32(CostDs.Tables[0].Rows[0]["BreakdownInsuranceDMS"]);


                        CostObj.IsShowCloseJob = Convert.ToBoolean(CostDs.Tables[0].Rows[0]["IsShowCloseJob"]);
                        CostObj.IsInvPrepared = Convert.ToBoolean(CostDs.Tables[0].Rows[0]["IsInvPrepared"]);
                        CostObj.IsGCCInsurance = Convert.ToBoolean(CostDs.Tables[0].Rows[0]["IsGCCInsurance"]);
                        CostObj.MoveJob.FinancePerson = CostDs.Tables[0].Rows[0]["FinancePerson"] == DBNull.Value ? (int?)null : Convert.ToInt32(CostDs.Tables[0].Rows[0]["FinancePerson"]);
                        CostObj.DefaultGPPercent = CostDs.Tables[0].Rows[0]["DefaultGPPercent"] == DBNull.Value ? 0 : Convert.ToDecimal(CostDs.Tables[0].Rows[0]["DefaultGPPercent"]);
                        CostObj.IsGPApproved = CostDs.Tables[0].Rows[0]["IsGPApproved"] == DBNull.Value ? true : Convert.ToBoolean(CostDs.Tables[0].Rows[0]["IsGPApproved"]);

                    }
                    #endregion
                    CostObj.IsDTD = false;
                    if (CostDs.Tables.Count > 1 && CostDs.Tables[1] != null && CostDs.Tables[1].Rows.Count > 0)
                    {
                        #region AgentGrid Details and Survey Details
                        CostObj.MoveJob.MoveRateCompList = (from item in CostDs.Tables[1].AsEnumerable()
                                                            select new MoveRateComponent()
                                                            {
                                                                RateComponentID = item["RateCompID"] == DBNull.Value ? 0 : Convert.ToInt32(item["RateCompID"]),
                                                                AgentID = item["AgentID"] == DBNull.Value ? 0 : Convert.ToInt32(item["AgentID"]),
                                                                JobAgentID = item["JobAgentID"] == DBNull.Value ? 0 : Convert.ToInt32(item["JobAgentID"]),
                                                                RateComponentName = Convert.ToString(item["RateComponentName"]),
                                                                AgentName = Convert.ToString(item["AgentName"]),
                                                                JobAgentName = Convert.ToString(item["JobAgentName"]),
                                                                ActJobAgentName = Convert.ToString(item["AgentName"]),
                                                                ExitPortID = item["JobOrgPortID"] == DBNull.Value ? (item["OrgPortID"] == DBNull.Value ? CostDs.Tables[0] != null && CostDs.Tables[0].Rows.Count > 0 && CostDs.Tables[0].Rows[0]["OrgPortID"] == DBNull.Value ? 0 : Convert.ToInt32(CostDs.Tables[0].Rows[0]["OrgPortID"]) : Convert.ToInt32(item["OrgPortID"])) : Convert.ToInt32(item["JobOrgPortID"]),
                                                                EntryPortID = item["JobDestPortID"] == DBNull.Value ? (item["DestPortID"] == DBNull.Value ? CostDs.Tables[0] != null && CostDs.Tables[0].Rows.Count > 0 && CostDs.Tables[0].Rows[0]["DestPortID"] == DBNull.Value ? 0 : Convert.ToInt32(CostDs.Tables[0].Rows[0]["DestPortID"]) : Convert.ToInt32(item["DestPortID"])) : Convert.ToInt32(item["JobDestPortID"]),
                                                                ExitPort = item["JobOrgPortName"] == DBNull.Value ? Convert.ToString(CostDs.Tables[0] != null && CostDs.Tables[0].Rows.Count > 0 ? CostDs.Tables[0].Rows[0]["OrgPort"] : "") : Convert.ToString(item["JobOrgPortName"]),
                                                                EntryPort = item["JobDestPortName"] == DBNull.Value ? Convert.ToString(CostDs.Tables[0] != null && CostDs.Tables[0].Rows.Count > 0 ? CostDs.Tables[0].Rows[0]["DestPort"] : "") : Convert.ToString(item["JobDestPortName"]),
                                                                ActExitPort = item["OrgPortName"] == DBNull.Value ? Convert.ToString(CostDs.Tables[0] != null && CostDs.Tables[0].Rows.Count > 0 ? CostDs.Tables[0].Rows[0]["OrgPort"] : "") : Convert.ToString(item["OrgPortName"]),
                                                                ActEntryPort = item["DestPortName"] == DBNull.Value ? Convert.ToString(CostDs.Tables[0] != null && CostDs.Tables[0].Rows.Count > 0 ? CostDs.Tables[0].Rows[0]["DestPort"] : "") : Convert.ToString(item["DestPortName"])
                                                            }).ToList();
                        if (CostObj.RMCType == "Other Type" && CostDs.Tables[1].Select("RateCompID=1").Count() > 0 && CostDs.Tables[1].Select("RateCompID=2").Count() <= 0)
                        {
                            List<MoveRateComponent> frtRateComp = new List<MoveRateComponent>();
                            frtRateComp = (from item in CostDs.Tables[1].AsEnumerable()
                                           select new MoveRateComponent()
                                           {
                                               RateComponentID = item["RateCompID"] == DBNull.Value ? 0 : Convert.ToInt32(item["RateCompID"]),
                                               AgentID = item["AgentID"] == DBNull.Value ? 0 : Convert.ToInt32(item["AgentID"]),
                                               JobAgentID = item["JobAgentID"] == DBNull.Value ? 0 : Convert.ToInt32(item["JobAgentID"]),
                                               RateComponentName = Convert.ToString(item["RateComponentName"]),
                                               AgentName = Convert.ToString(item["AgentName"]),
                                               JobAgentName = Convert.ToString(item["JobAgentName"]),
                                               ActJobAgentName = Convert.ToString(item["AgentName"]),
                                               ExitPortID = item["JobOrgPortID"] == DBNull.Value ? (item["OrgPortID"] == DBNull.Value ? CostDs.Tables[0].Rows[0]["OrgPortID"] == DBNull.Value ? 0 : Convert.ToInt32(CostDs.Tables[0].Rows[0]["OrgPortID"]) : Convert.ToInt32(item["OrgPortID"])) : Convert.ToInt32(item["JobOrgPortID"]),
                                               EntryPortID = item["JobDestPortID"] == DBNull.Value ? (item["DestPortID"] == DBNull.Value ? CostDs.Tables[0].Rows[0]["DestPortID"] == DBNull.Value ? 0 : Convert.ToInt32(CostDs.Tables[0].Rows[0]["DestPortID"]) : Convert.ToInt32(item["DestPortID"])) : Convert.ToInt32(item["JobDestPortID"]),
                                               ExitPort = item["JobOrgPortName"] == DBNull.Value ? Convert.ToString(CostDs.Tables[0].Rows[0]["OrgPort"]) : Convert.ToString(item["JobOrgPortName"]),
                                               EntryPort = item["JobDestPortName"] == DBNull.Value ? Convert.ToString(CostDs.Tables[0].Rows[0]["DestPort"]) : Convert.ToString(item["JobDestPortName"]),
                                               ActExitPort = item["OrgPortName"] == DBNull.Value ? Convert.ToString(CostDs.Tables[0].Rows[0]["OrgPort"]) : Convert.ToString(item["OrgPortName"]),
                                               ActEntryPort = item["DestPortName"] == DBNull.Value ? Convert.ToString(CostDs.Tables[0].Rows[0]["DestPort"]) : Convert.ToString(item["DestPortName"])
                                           }).ToList();
                            frtRateComp.First().RateComponentID = 2; frtRateComp.First().RateComponentName = "Freight";
                            CostObj.MoveJob.MoveRateCompList.AddRange(frtRateComp);
                        }
                        CostObj.IsDTD = CostObj.MoveJob.MoveRateCompList.Count == 1;

                        //CostObj.MoveJob.MoveRateCompList
                        #endregion

                        #region Survey Details
                        DataRow[] SurveyDataRow = CostDs.Tables[1].Select("RateCompID=1 OR RateCompID=4");
                        DataTable SurveyData = SurveyDataRow.Length > 0 ? SurveyDataRow.CopyToDataTable() : new DataTable();
                        if (SurveyData.Rows.Count > 0 && SurveyData != null)
                        {
                            CostObj.SurveyDetail.OrgAgentID = SurveyData.Rows[0]["JobAgentID"] == DBNull.Value ? (int?)null : Convert.ToInt32(SurveyData.Rows[0]["JobAgentID"]);
                            CostObj.SurveyDetail.ExitPortID = SurveyData.Rows[0]["JobOrgPortID"] == DBNull.Value ? (int?)null : Convert.ToInt32(SurveyData.Rows[0]["JobOrgPortID"]);
                        }

                        SurveyDataRow = CostDs.Tables[1].Select("RateCompID=2 OR RateCompID=4");
                        SurveyData = SurveyDataRow.Length > 0 ? SurveyDataRow.CopyToDataTable() : new DataTable();
                        if (SurveyData.Rows.Count > 0 && SurveyData != null)
                        {
                            CostObj.SurveyDetail.ExitPortID = SurveyData.Rows[0]["JobOrgPortID"] == DBNull.Value ? (int?)null : Convert.ToInt32(SurveyData.Rows[0]["JobOrgPortID"]);
                            CostObj.SurveyDetail.EntryPortID = SurveyData.Rows[0]["JobDestPortID"] == DBNull.Value ? (int?)null : Convert.ToInt32(SurveyData.Rows[0]["JobDestPortID"]);
                            if (CostObj.MoveJob.ModeName.ToUpper() == "ROAD" && CostObj.RMCType == "Brookfield Type")
                            {
                                CostObj.SurveyDetail.FrtAgentID = SurveyData.Rows[0]["JobAgentID"] == DBNull.Value ? (int?)null : Convert.ToInt32(SurveyData.Rows[0]["JobAgentID"]);
                            }

                        }

                        SurveyDataRow = CostDs.Tables[1].Select("RateCompID=3 OR RateCompID=4");
                        SurveyData = SurveyDataRow.Length > 0 ? SurveyDataRow.CopyToDataTable() : new DataTable();
                        if (SurveyData.Rows.Count > 0 && SurveyData != null)
                        {
                            CostObj.SurveyDetail.EntryPortID = SurveyData.Rows[0]["JobDestPortID"] == DBNull.Value ? (int?)null : Convert.ToInt32(SurveyData.Rows[0]["JobDestPortID"]);
                            CostObj.SurveyDetail.DestAgentID = SurveyData.Rows[0]["JobAgentID"] == DBNull.Value ? (int?)null : Convert.ToInt32(SurveyData.Rows[0]["JobAgentID"]);
                        }

                        if (!(CostObj.MoveJob.ModeName.ToUpper() == "ROAD" && CostObj.RMCType == "Brookfield Type"))
                        {
                            CostObj.SurveyDetail.FrtAgentID = CostObj.SurveyDetail.FrtAgentID != null ? CostObj.SurveyDetail.FrtAgentID : CostObj.SurveyDetail.OrgAgentID;
                        }
                        #endregion
                    }

                    #region Survey Report
                    if (CostDs.Tables.Count > 2 && CostDs.Tables[2] != null && CostDs.Tables[2].Rows.Count > 0)
                    {
                        CostObj.SurveyDetail.IsDone = true;

                        CostObj.SurveyReport.SurveyorID = CostDs.Tables[2].Rows[0]["SurveyerID"] == DBNull.Value ? (int?)null : Convert.ToInt32(CostDs.Tables[2].Rows[0]["SurveyerID"]);
                        CostObj.SurveyReport.Surveydate = CostDs.Tables[2].Rows[0]["SurveyDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(CostDs.Tables[2].Rows[0]["SurveyDate"]);
                        CostObj.SurveyReport.SchSurveydate = CostDs.Tables[2].Rows[0]["SchSurveyDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(CostDs.Tables[2].Rows[0]["SchSurveyDate"]);
                        CostObj.IsSOCost = CostDs.Tables[2].Rows[0]["SurveyDate"] == DBNull.Value ? false : true;
                        CostObj.SurveyReport.SurveyDateTime = CostDs.Tables[2].Rows[0]["SurveryTime"] == DBNull.Value ? (TimeSpan?)null : (TimeSpan)CostDs.Tables[2].Rows[0]["SurveryTime"];
                        CostObj.SurveyReport.Packdate = CostDs.Tables[2].Rows[0]["PackDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(CostDs.Tables[2].Rows[0]["PackDate"]); //(DateTime?)CostDs.Tables[2].Rows[0]["PackDate"];
                        CostObj.SurveyReport.Loaddate = CostDs.Tables[2].Rows[0]["LoadDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(CostDs.Tables[2].Rows[0]["LoadDate"]); //(DateTime?)CostDs.Tables[2].Rows[0]["LoadDate"];
                        CostObj.SurveyReport.DensityFact = CostDs.Tables[2].Rows[0]["DensityFact"] == DBNull.Value ? (decimal)0.00 : Convert.ToDecimal(CostDs.Tables[2].Rows[0]["DensityFact"]);
                        CostObj.SurveyReport.VolumeUnitID = CostDs.Tables[2].Rows[0]["VolumeUnitID"] == DBNull.Value ? 0 : Convert.ToInt32(CostDs.Tables[2].Rows[0]["VolumeUnitID"]);
                        CostObj.SurveyReport.WeightUnitID = CostDs.Tables[2].Rows[0]["WeightUnitID"] == DBNull.Value ? 0 : Convert.ToInt32(CostDs.Tables[2].Rows[0]["WeightUnitID"]);
                        CostObj.SurveyReport.TobePackedVol = CostDs.Tables[2].Rows[0]["TobePackedVol"] == DBNull.Value ? (decimal)0.00 : Convert.ToDecimal(CostDs.Tables[2].Rows[0]["TobePackedVol"]);
                        CostObj.SurveyReport.NetVol = CostDs.Tables[2].Rows[0]["NetVol"] == DBNull.Value ? (decimal)0.00 : Convert.ToDecimal(CostDs.Tables[2].Rows[0]["NetVol"]);
                        CostObj.SurveyReport.GrossVol = CostDs.Tables[2].Rows[0]["GrossVol"] == DBNull.Value ? (decimal)0.00 : Convert.ToDecimal(CostDs.Tables[2].Rows[0]["GrossVol"]);
                        CostObj.SurveyReport.NetWt = CostDs.Tables[2].Rows[0]["NetWt"] == DBNull.Value ? (decimal)0.00 : Convert.ToDecimal(CostDs.Tables[2].Rows[0]["NetWt"]);
                        CostObj.SurveyReport.GrossWt = CostDs.Tables[2].Rows[0]["GrossWt"] == DBNull.Value ? (decimal)0.00 : Convert.ToDecimal(CostDs.Tables[2].Rows[0]["GrossWt"]);
                        CostObj.SurveyReport.ACWTWt = CostDs.Tables[2].Rows[0]["ACWTWt"] == DBNull.Value ? (decimal)0.00 : Convert.ToDecimal(CostDs.Tables[2].Rows[0]["ACWTWt"]);
                        CostObj.SurveyReport.ContainerSizeID = CostDs.Tables[2].Rows[0]["ContainerSizeID"] == DBNull.Value ? 0 : Convert.ToInt32(CostDs.Tables[2].Rows[0]["ContainerSizeID"]);
                        CostObj.SurveyReport.RoadKMS = CostDs.Tables[2].Rows[0]["RoadKMS"] == DBNull.Value ? (decimal)0.00 : Convert.ToDecimal(CostDs.Tables[2].Rows[0]["RoadKMS"]);

                        CostObj.SurveyReport.IsCSSenttoApprove = CostDs.Tables[2].Rows[0]["IsSendToApproval"] == DBNull.Value ? false : Convert.ToBoolean(CostDs.Tables[2].Rows[0]["IsSendToApproval"]);
                        CostObj.SurveyReport.CSSenttoApproveUser = CostDs.Tables[2].Rows[0]["ApprovalUserID"] == DBNull.Value ? (Int32?)0 : Convert.ToInt32(CostDs.Tables[2].Rows[0]["ApprovalUserID"]);

                        CostObj.SurveyReport.CSCreatedBY = CostDs.Tables[2].Rows[0]["CSPreparedBy"] == DBNull.Value ? null : Convert.ToString(CostDs.Tables[2].Rows[0]["CSPreparedBy"]);
                        CostObj.SurveyReport.CSCreatedDate = CostDs.Tables[2].Rows[0]["CSPreparedDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(CostDs.Tables[2].Rows[0]["CSPreparedDate"]);
                        CostObj.SurveyReport.CSApprovedBY = CostDs.Tables[2].Rows[0]["CSApprovedBy"] == DBNull.Value ? null : Convert.ToString(CostDs.Tables[2].Rows[0]["CSApprovedBy"]);
                        CostObj.SurveyReport.CSApprovedDate = CostDs.Tables[2].Rows[0]["CSApprovedDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(CostDs.Tables[2].Rows[0]["CSApprovedDate"]);

                        if (CostObj.MoveJob.ModeID != 2)
                        {
                            CostObj.SurveyReport.LCLorFCL = CostDs.Tables[2].Rows[0]["LCLorFCL"] == DBNull.Value ? null : Convert.ToString(CostDs.Tables[2].Rows[0]["LCLorFCL"]);
                            CostObj.SurveyReport.LooseCased = CostDs.Tables[2].Rows[0]["LooseCased"] == DBNull.Value ? null : Convert.ToString(CostDs.Tables[2].Rows[0]["LooseCased"]);
                        }
                        else
                        {
                            CostObj.SurveyReport.LCLorFCL = CostDs.Tables[2].Rows[0]["LCLorFCL"] == DBNull.Value ? "LCL" : Convert.ToString(CostDs.Tables[2].Rows[0]["LCLorFCL"]);
                            CostObj.SurveyReport.LooseCased = CostDs.Tables[2].Rows[0]["LooseCased"] == DBNull.Value ? "Loose" : Convert.ToString(CostDs.Tables[2].Rows[0]["LooseCased"]);
                        }

                    }
                    else
                    {
                        CostObj.SurveyReport.LooseCased = "Loose";
                    }
                    #endregion

                    #region Survey SO
                    if (CostDs.Tables.Count > 3 && CostDs.Tables[3] != null && CostDs.Tables[3].Rows.Count > 0)
                    {
                        CostObj.SurveySOList.SOList = (from item in CostDs.Tables[3].AsEnumerable()
                                                       select new PackingSOList
                                                       {
                                                           SurveyId = item["SurveyId"] == DBNull.Value ? (int?)0 : (Convert.ToInt32(item["SurveyId"])),
                                                           SurveyDetailId = item["SurveyDetailsId"] == DBNull.Value ? (int?)0 : (Convert.ToInt32(item["SurveyDetailsId"])),

                                                           CostHeadID = item["CostHeadID"] == DBNull.Value ? (int?)0 : (Convert.ToInt32(item["CostHeadID"])),

                                                           CostHeadName = Convert.ToString(item["CostHeadName"]),
                                                           Remark = Convert.ToString(item["RemarksOnCostHead"]),

                                                           RateCompId = item["MoveCompId"] == DBNull.Value ? (int)0 : (Convert.ToInt32(item["MoveCompId"])),

                                                           RateCompName = Convert.ToString(item["RateComponentName"]),
                                                           Volume = Convert.ToString(item["WtVolume"]),
                                                           WtUnitID = item["WtUnitID"] == DBNull.Value ? (int)0 : (Convert.ToInt32(item["WtUnitID"])),

                                                           WtUnit = Convert.ToString(item["WeightUnitName"]),
                                                           ExpectedCost = item["ExpectedCost"] == DBNull.Value ? (decimal?)0 : (Convert.ToDecimal(item["ExpectedCost"])),

                                                           Isactive = true,

                                                           /*ConversionRate = item["BaseCurrConversRate"] == DBNull.Value ? 0 : Convert.ToDecimal(item["BaseCurrConversRate"]),
                                                           Amount = item["RateCurrValue"]==DBNull.Value ? 0 :  Convert.ToDecimal(item["RateCurrValue"]),*/
                                                       }).ToList();
                    }
                    #endregion

                    #region Survey Cost
                    if (CostDs.Tables.Count > 4 && CostDs.Tables[4] != null && CostDs.Tables[4].Rows.Count > 0)
                    {
                        CostObj.SurveyCostList.CostListSaved = Convert.ToString(CostDs.Tables[4].Rows[0]["IsSurveyCostSaved"]) == "Y";
                        CostObj.SurveyCostList.CostList = (from item in CostDs.Tables[4].AsEnumerable()
                                                           select new PackingCostList
                                                           {
                                                               RateCompId = item["MoveCompID"] == DBNull.Value ? (int)0 : (Convert.ToInt32(item["MoveCompID"])),
                                                               RateCompName = Convert.ToString(item["RateComponentName"]),

                                                               CostHeadID = item["CostHeadID"] == DBNull.Value ? (int?)0 : (Convert.ToInt32(item["CostHeadID"])),

                                                               CostHeadName = Convert.ToString(item["CostHeadName"]),
                                                               BaseCurrID = item["BaseCurrID"] == DBNull.Value ? (int)0 : (Convert.ToInt32(item["BaseCurrID"])),

                                                               RateCurrID = item["RateCurrID"] == DBNull.Value ? (int)0 : (Convert.ToInt32(item["RateCurrID"])),
                                                               RevRateCurrID = item["RevRateCurrID"] == DBNull.Value ? (int)0 : (Convert.ToInt32(item["RevRateCurrID"])),
                                                               ConversionRate = item["ConversionRate"] == DBNull.Value ? (decimal?)0 : (Convert.ToDecimal(item["ConversionRate"])),
                                                               RevConversionRate = item["RevConversionRate"] == DBNull.Value ? (decimal?)0 : (Convert.ToDecimal(item["RevConversionRate"])),
                                                               CostValue = item["CostValue"] == DBNull.Value ? (decimal?)0 : (Convert.ToDecimal(item["CostValue"])),
                                                               BaseRevenueValue = item["BaseRev"] == DBNull.Value ? (decimal?)0 : (Convert.ToDecimal(item["BaseRev"])),
                                                               RevenueValue = item["Revenuevalue"] == DBNull.Value ? (decimal?)0 : (Convert.ToDecimal(item["Revenuevalue"])),
                                                               Balance = item["Revenuevalue"] == DBNull.Value ? (decimal?)0 : (Convert.ToDecimal(item["Revenuevalue"])),
                                                               BaseCurr = Convert.ToString(item["BaseCurrName"]),
                                                               RateCurr = Convert.ToString(item["RateCurrName"]),
                                                               RevRateCurr = Convert.ToString(item["RevRateCurrName"]),

                                                               Isactive = true//item["Isactive"] == DBNull.Value ? (bool)false : (Convert.ToBoolean(item["Isactive"]))

                                                               /*ConversionRate = item["BaseCurrConversRate"] == DBNull.Value ? 0 : Convert.ToDecimal(item["BaseCurrConversRate"]),
                                                               Amount = item["RateCurrValue"]==DBNull.Value ? 0 :  Convert.ToDecimal(item["RateCurrValue"]),*/
                                                           }).ToList();
                        /*CostObj.BaseCurrID = CostObj.SurveyCostList.CostList.First().BaseCurrID;
						CostObj.BaseCurr = CostObj.SurveyCostList.CostList.First().BaseCurr;*/
                        CostObj.SurveyReport.IsApprove = Convert.ToBoolean(CostDs.Tables[2].Rows[0]["IsCostSheetApproved"]);
                        CostObj.SurveyReport.ApproveTitle = CostObj.SurveyReport.IsApprove ? "Approved" : "Pending";
                    }
                    #endregion

                    #region Packing Info
                    if (CostDs.Tables.Count > 5 && CostDs.Tables[5] != null && CostDs.Tables[5].Rows.Count > 0 && CostDs.Tables[5].Rows[0][0].ToString() == "Same as survey details")
                    {
                        orgFlag = false;
                        CostObj.PackingDetail.OrgAgentID = CostObj.SurveyDetail.OrgAgentID;
                        CostObj.PackingDetail.FrtAgentID = CostObj.SurveyDetail.FrtAgentID;
                        CostObj.PackingDetail.DestAgentID = CostObj.SurveyDetail.DestAgentID;
                        CostObj.PackingDetail.ExitPortID = CostObj.SurveyDetail.ExitPortID;
                        CostObj.PackingDetail.EntryPortID = CostObj.SurveyDetail.EntryPortID;

                        /*CostObj.PackingReport.Packdate = CostObj.SurveyReport.Packdate;
                        CostObj.PackingReport.Loaddate = CostObj.SurveyReport.Loaddate;
                        CostObj.PackingReport.DensityFact = CostObj.SurveyReport.DensityFact;
                        CostObj.PackingReport.VolumeUnitID = CostObj.SurveyReport.VolumeUnitID;
                        CostObj.PackingReport.WeightUnitID = CostObj.SurveyReport.WeightUnitID;
                        CostObj.PackingReport.TobePackedVol = CostObj.SurveyReport.TobePackedVol;
                        CostObj.PackingReport.NetVol = CostObj.SurveyReport.NetVol;
                        CostObj.PackingReport.GrossVol = CostObj.SurveyReport.GrossVol;
                        CostObj.PackingReport.NetWt = CostObj.SurveyReport.NetWt;
                        CostObj.PackingReport.GrossWt = CostObj.SurveyReport.GrossWt;
                        CostObj.PackingReport.ACWTWt = CostObj.SurveyReport.ACWTWt;*/

                        CostObj.PackingSOList.SOList = CostObj.SurveySOList.SOList;

                        CostObj.PackingCostList.CostList = CostObj.SurveyCostList.CostList;
                    }
                    else if (CostDs.Tables.Count > 5 && CostDs.Tables[5] != null /*&& CostDs.Tables[5].Rows.Count > 0*/)
                    {

                        orgFlag = true;
                        #region Packing Details
                        if (CostDs.Tables.Count > 5 && CostDs.Tables[5] != null && CostDs.Tables[5].Rows.Count > 0)
                        {
                            CostObj.PackingDetail.IsDone = true;
                            CostObj.PackingDetail.OrgAgentID = CostDs.Tables[5].Rows[0]["OrgAgentID"] == DBNull.Value ? (int?)null : Convert.ToInt32(CostDs.Tables[5].Rows[0]["OrgAgentID"]);
                            CostObj.PackingDetail.OrgAgentName = Convert.ToString(CostDs.Tables[5].Rows[0]["OrgAgentName"]);
                            CostObj.PackingDetail.FrtAgentID = CostDs.Tables[5].Rows[0]["FrtAgentID"] == DBNull.Value ? (int?)null : Convert.ToInt32(CostDs.Tables[5].Rows[0]["FrtAgentID"]);
                            CostObj.PackingDetail.DestAgentID = CostDs.Tables[5].Rows[0]["DestAgentID"] == DBNull.Value ? (int?)null : Convert.ToInt32(CostDs.Tables[5].Rows[0]["DestAgentID"]);
                            CostObj.PackingDetail.DestAgentName = Convert.ToString(CostDs.Tables[5].Rows[0]["DestAgentName"]);
                            CostObj.PackingDetail.ExitPortID = CostDs.Tables[5].Rows[0]["ExitPortID"] == DBNull.Value ? (int?)null : Convert.ToInt32(CostDs.Tables[5].Rows[0]["ExitPortID"]);
                            CostObj.PackingDetail.ExitPortName = Convert.ToString(CostDs.Tables[5].Rows[0]["ExitPortName"]);
                            CostObj.PackingDetail.EntryPortID = CostDs.Tables[5].Rows[0]["EntryPortID"] == DBNull.Value ? (int?)null : Convert.ToInt32(CostDs.Tables[5].Rows[0]["EntryPortID"]);
                            CostObj.PackingDetail.EntryPortName = Convert.ToString(CostDs.Tables[5].Rows[0]["EntryPortName"]);



                            if (CostObj.MoveJob.ModeID != 3)
                            {
                                if (!string.IsNullOrEmpty(Convert.ToString(CostDs.Tables[5].Rows[0]["DestAgentName"])) &&
                                CostObj.MoveJob.MoveRateCompList.Where(x => x.RateComponentID == 3).Count() <= 0)
                                {
                                    CostObj.MoveJob.MoveRateCompList.Add(new MoveRateComponent() { RateComponentID = 3, RateComponentName = "Destination" });
                                }
                                if (!string.IsNullOrEmpty(Convert.ToString(CostDs.Tables[5].Rows[0]["OrgAgentName"])) &&
                                    CostObj.MoveJob.MoveRateCompList.Where(x => x.RateComponentID == 1).Count() <= 0)
                                {

                                    CostObj.MoveJob.MoveRateCompList.Add(new MoveRateComponent() { RateComponentID = 1, RateComponentName = "Origin" });
                                    CostObj.MoveJob.MoveRateCompList.Add(new MoveRateComponent() { RateComponentID = 2, RateComponentName = "Freight" });
                                }

                                CostObj.MoveJob.MoveRateCompList = CostObj.MoveJob.MoveRateCompList.Select(i =>
                                {
                                    if (i.RateComponentID == 1) i.ActJobAgentName = Convert.ToString(CostDs.Tables[5].Rows[0]["OrgAgentName"]);
                                    if (i.RateComponentID == 1) i.ActJobAgentID = Convert.ToString(CostDs.Tables[5].Rows[0]["OrgAgentID"]);
                                    if (i.RateComponentID == 2) i.ActJobAgentName = Convert.ToString(CostDs.Tables[5].Rows[0]["OrgAgentName"]);
                                    if (i.RateComponentID == 2) i.ActJobAgentID = Convert.ToString(CostDs.Tables[5].Rows[0]["OrgAgentID"]);
                                    if (i.RateComponentID == 3) i.ActJobAgentName = Convert.ToString(CostDs.Tables[5].Rows[0]["DestAgentName"]);
                                    if (i.RateComponentID == 3) i.ActJobAgentID = Convert.ToString(CostDs.Tables[5].Rows[0]["DestAgentID"]);
                                    return i;
                                }).ToList();

                            }
                            else
                            {
                                if (CostObj.MoveJob.MoveRateCompList.Count == 1)
                                {
                                    CostObj.MoveJob.MoveRateCompList = CostObj.MoveJob.MoveRateCompList.Select(i =>
                                    {
                                        if (i.RateComponentID == 4) i.ActJobAgentName = Convert.ToString(CostDs.Tables[5].Rows[0]["OrgAgentName"]);
                                        if (i.RateComponentID == 4) i.ActJobAgentID = Convert.ToString(CostDs.Tables[5].Rows[0]["OrgAgentID"]);
                                        //if (i.RateComponentID == 2) i.ActJobAgentName = Convert.ToString(CostDs.Tables[5].Rows[0]["OrgAgentName"]);
                                        //if (i.RateComponentID == 2) i.ActJobAgentID = Convert.ToString(CostDs.Tables[5].Rows[0]["OrgAgentID"]);
                                        //if (i.RateComponentID == 3) i.ActJobAgentName = Convert.ToString(CostDs.Tables[5].Rows[0]["DestAgentName"]);
                                        //if (i.RateComponentID == 3) i.ActJobAgentID = Convert.ToString(CostDs.Tables[5].Rows[0]["DestAgentID"]);
                                        return i;
                                    }).ToList();
                                }
                                else
                                {
                                    if (!string.IsNullOrEmpty(Convert.ToString(CostDs.Tables[5].Rows[0]["DestAgentName"])) &&
                                CostObj.MoveJob.MoveRateCompList.Where(x => x.RateComponentID == 3).Count() <= 0)
                                    {
                                        CostObj.MoveJob.MoveRateCompList.Add(new MoveRateComponent() { RateComponentID = 3, RateComponentName = "Destination" });
                                    }
                                    if (!string.IsNullOrEmpty(Convert.ToString(CostDs.Tables[5].Rows[0]["OrgAgentName"])) &&
                                        CostObj.MoveJob.MoveRateCompList.Where(x => x.RateComponentID == 1).Count() <= 0)
                                    {

                                        CostObj.MoveJob.MoveRateCompList.Add(new MoveRateComponent() { RateComponentID = 1, RateComponentName = "Origin" });
                                        CostObj.MoveJob.MoveRateCompList.Add(new MoveRateComponent() { RateComponentID = 2, RateComponentName = "Freight" });
                                    }

                                    CostObj.MoveJob.MoveRateCompList = CostObj.MoveJob.MoveRateCompList.Select(i =>
                                    {
                                        if (i.RateComponentID == 1) i.ActJobAgentName = Convert.ToString(CostDs.Tables[5].Rows[0]["OrgAgentName"]);
                                        if (i.RateComponentID == 1) i.ActJobAgentID = Convert.ToString(CostDs.Tables[5].Rows[0]["OrgAgentID"]);
                                        if (i.RateComponentID == 2) i.ActJobAgentName = Convert.ToString(CostDs.Tables[5].Rows[0]["FrtAgentName"]);
                                        if (i.RateComponentID == 2) i.ActJobAgentID = Convert.ToString(CostDs.Tables[5].Rows[0]["FrtAgentID"]);
                                        if (i.RateComponentID == 3) i.ActJobAgentName = Convert.ToString(CostDs.Tables[5].Rows[0]["DestAgentName"]);
                                        if (i.RateComponentID == 3) i.ActJobAgentID = Convert.ToString(CostDs.Tables[5].Rows[0]["DestAgentID"]);
                                        return i;
                                    }).ToList();
                                }

                            }


                        }
                        #endregion

                        #region Packing SO
                        if (CostDs.Tables.Count > 6 && CostDs.Tables[6] != null && CostDs.Tables[6].Rows.Count > 0)
                        {
                            CostObj.PackingSOList.SOList = (from item in CostDs.Tables[6].AsEnumerable()
                                                            select new PackingSOList
                                                            {
                                                                CostHeadID = item["CostHeadID"] == DBNull.Value ? (int?)0 : (Convert.ToInt32(item["CostHeadID"])),
                                                                CostHeadName = Convert.ToString(item["CostHeadName"]),
                                                                Remark = Convert.ToString(item["ServOrderRemarks"]),
                                                                RateCompId = Convert.ToInt32(RELOCBS.Common.CommonService.RateComp.Origin),
                                                                Volume = Convert.ToString(item["WtVolume"]),
                                                                WtUnitID = item["WtUnitID"] == DBNull.Value ? (int)0 : (Convert.ToInt32(item["WtUnitID"])),
                                                                WtUnit = Convert.ToString(item["WeightUnitName"]),
                                                                Isactive = true//item["Isactive"] == DBNull.Value ? (bool)false : (Convert.ToBoolean(item["Isactive"])),
                                                            }).ToList();
                        }
                        #endregion

                        #region Packing Report
                        if (CostDs.Tables.Count > 7 && CostDs.Tables[7] != null && CostDs.Tables[7].Rows.Count > 0)
                        {
                            CostObj.PackingReport.Packdate = CostDs.Tables[7].Rows[0]["PackDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(CostDs.Tables[7].Rows[0]["PackDate"]); //(DateTime?)CostDs.Tables[7].Rows[0]["PackDate"];
                            CostObj.PackingReport.ScheduledPackDate = CostDs.Tables[7].Rows[0]["SchPackDate"] == DBNull.Value ? CostObj.SurveyReport.Packdate : Convert.ToDateTime(CostDs.Tables[7].Rows[0]["SchPackDate"]); //(DateTime?)CostDs.Tables[7].Rows[0]["PackDate"];
                            CostObj.PackingReport.Loaddate = CostDs.Tables[7].Rows[0]["LoadDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(CostDs.Tables[7].Rows[0]["LoadDate"]); //(DateTime?)CostDs.Tables[7].Rows[0]["LoadDate"];
                            CostObj.PackingReport.DensityFact = CostDs.Tables[7].Rows[0]["DensityFact"] == DBNull.Value ? (decimal)0.00 : Convert.ToDecimal(CostDs.Tables[7].Rows[0]["DensityFact"]);
                            CostObj.PackingReport.VolumeUnitID = CostDs.Tables[7].Rows[0]["VolumeUnitID"] == DBNull.Value ? 0 : Convert.ToInt32(CostDs.Tables[7].Rows[0]["VolumeUnitID"]);
                            CostObj.PackingReport.WeightUnitID = CostDs.Tables[7].Rows[0]["WeightUnitID"] == DBNull.Value ? 0 : Convert.ToInt32(CostDs.Tables[7].Rows[0]["WeightUnitID"]);
                            CostObj.PackingReport.TobePackedVol = CostDs.Tables[7].Rows[0]["TobePackedVol"] == DBNull.Value ? (decimal)0.00 : Convert.ToDecimal(CostDs.Tables[7].Rows[0]["TobePackedVol"]);
                            CostObj.PackingReport.NetVol = CostDs.Tables[7].Rows[0]["NetVol"] == DBNull.Value ? (decimal)0.00 : Convert.ToDecimal(CostDs.Tables[7].Rows[0]["NetVol"]);
                            CostObj.PackingReport.GrossVol = CostDs.Tables[7].Rows[0]["GrossVol"] == DBNull.Value ? (decimal)0.00 : Convert.ToDecimal(CostDs.Tables[7].Rows[0]["GrossVol"]);
                            CostObj.PackingReport.NetWt = CostDs.Tables[7].Rows[0]["NetWt"] == DBNull.Value ? (decimal)0.00 : Convert.ToDecimal(CostDs.Tables[7].Rows[0]["NetWt"]);
                            CostObj.PackingReport.GrossWt = CostDs.Tables[7].Rows[0]["GrossWt"] == DBNull.Value ? (decimal)0.00 : Convert.ToDecimal(CostDs.Tables[7].Rows[0]["GrossWt"]);
                            CostObj.PackingReport.ACWTWt = CostDs.Tables[7].Rows[0]["ACWTWt"] == DBNull.Value ? (decimal)0.00 : Convert.ToDecimal(CostDs.Tables[7].Rows[0]["ACWTWt"]);
                            CostObj.PackingReport.ContainerSizeID = CostDs.Tables[7].Rows[0]["ContainerSizeID"] == DBNull.Value ? 0 : Convert.ToInt32(CostDs.Tables[7].Rows[0]["ContainerSizeID"]);
                            CostObj.PackingReport.CSCreatedBY = CostDs.Tables[7].Rows[0]["CSPreparedBy"] == DBNull.Value ? null : Convert.ToString(CostDs.Tables[7].Rows[0]["CSPreparedBy"]);
                            CostObj.PackingReport.CSCreatedDate = CostDs.Tables[7].Rows[0]["CSPreparedDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(CostDs.Tables[7].Rows[0]["CSPreparedDate"]);
                            CostObj.PackingReport.CSApprovedBY = CostDs.Tables[7].Rows[0]["CSApprovedBy"] == DBNull.Value ? null : Convert.ToString(CostDs.Tables[7].Rows[0]["CSApprovedBy"]);
                            CostObj.PackingReport.CSApprovedDate = CostDs.Tables[7].Rows[0]["CSApprovedDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(CostDs.Tables[7].Rows[0]["CSApprovedDate"]);
                            CostObj.PackingReport.OrgStgStartDate = CostDs.Tables[7].Rows[0]["OrgStgStartDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(CostDs.Tables[7].Rows[0]["OrgStgStartDate"]);
                            CostObj.PackingReport.OrgStgEndDate = CostDs.Tables[7].Rows[0]["OrgStgEndDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(CostDs.Tables[7].Rows[0]["OrgStgEndDate"]);
                            CostObj.PackingReport.RoadKMS = CostDs.Tables[7].Rows[0]["RoadKMS"] == DBNull.Value ? (decimal)0.00 : Convert.ToDecimal(CostDs.Tables[7].Rows[0]["RoadKMS"]);

                            CostObj.PackingReport.IsCSSenttoApprove = CostDs.Tables[7].Rows[0]["IsSendToApproval"] == DBNull.Value ? false : Convert.ToBoolean(CostDs.Tables[7].Rows[0]["IsSendToApproval"]);
                            CostObj.PackingReport.CSSenttoApproveUser = CostDs.Tables[7].Rows[0]["ApprovalUserID"] == DBNull.Value ? (Int32?)0 : Convert.ToInt32(CostDs.Tables[7].Rows[0]["ApprovalUserID"]);



                            if (CostObj.MoveJob.ModeID != 2)
                            {
                                CostObj.PackingReport.LCLorFCL = CostDs.Tables[7].Rows[0]["LCLorFCL"] == DBNull.Value ? null : Convert.ToString(CostDs.Tables[7].Rows[0]["LCLorFCL"]);
                                CostObj.PackingReport.LooseCased = CostDs.Tables[7].Rows[0]["LooseCased"] == DBNull.Value ? null : Convert.ToString(CostDs.Tables[7].Rows[0]["LooseCased"]);
                            }
                            else
                            {
                                CostObj.PackingReport.LCLorFCL = CostDs.Tables[7].Rows[0]["LCLorFCL"] == DBNull.Value ? "LCL" : Convert.ToString(CostDs.Tables[7].Rows[0]["LCLorFCL"]);
                                CostObj.PackingReport.LooseCased = CostDs.Tables[7].Rows[0]["LooseCased"] == DBNull.Value ? "Cased" : Convert.ToString(CostDs.Tables[7].Rows[0]["LooseCased"]);
                            }

                        }
                        else
                        {
                            if (CostObj.MoveJob.ModeID == 2)
                            {
                                CostObj.PackingReport.LCLorFCL = "LCL";
                                CostObj.PackingReport.LooseCased = "Cased";
                            }
                        }
                        #endregion

                        #region Packing Cost   
                        if (CostDs.Tables.Count > 8 && CostDs.Tables[8] != null && CostDs.Tables[8].Rows.Count > 0)
                        {
                            CostObj.PackingCostList.CostListSaved = Convert.ToString(CostDs.Tables[8].Rows[0]["IsPackCostSaved"]) == "Y";
                            CostObj.PackingCostList.CostList = (from item in CostDs.Tables[8].AsEnumerable()
                                                                select new PackingCostList
                                                                {
                                                                    RateCompId = item["CompId"] == DBNull.Value ? (int)0 : (Convert.ToInt32(item["CompId"])),
                                                                    RateCompName = Convert.ToString(item["RateComponentName"]),

                                                                    CostHeadID = item["CostHeadID"] == DBNull.Value ? (int?)0 : (Convert.ToInt32(item["CostHeadID"])),

                                                                    CostHeadName = Convert.ToString(item["CostHeadName"]),
                                                                    BaseCurrID = item["BaseCurrID"] == DBNull.Value ? (int)0 : (Convert.ToInt32(item["BaseCurrID"])),

                                                                    RateCurrID = item["RateCurrID"] == DBNull.Value ? (int)0 : (Convert.ToInt32(item["RateCurrID"])),
                                                                    RevRateCurrID = item["RevRateCurrID"] == DBNull.Value ? (int)0 : (Convert.ToInt32(item["RevRateCurrID"])),
                                                                    ConversionRate = item["ConversionRate"] == DBNull.Value ? (decimal?)0 : (Convert.ToDecimal(item["ConversionRate"])),
                                                                    RevConversionRate = item["RevConversionRate"] == DBNull.Value ? (decimal?)0 : (Convert.ToDecimal(item["RevConversionRate"])),
                                                                    CostValue = item["CostValue"] == DBNull.Value ? (decimal?)0 : (Convert.ToDecimal(item["CostValue"])),
                                                                    BaseRevenueValue = item["BaseRev"] == DBNull.Value ? (decimal?)0 : (Convert.ToDecimal(item["BaseRev"])),
                                                                    RevenueValue = item["Revenuevalue"] == DBNull.Value ? (decimal?)0 : (Convert.ToDecimal(item["Revenuevalue"])),
                                                                    Balance = item["UNBILLED"] == DBNull.Value ? (decimal?)0 : (Convert.ToDecimal(item["UNBILLED"])),
                                                                    BaseCurr = Convert.ToString(item["BaseCurrName"]),
                                                                    RateCurr = Convert.ToString(item["RateCurrName"]),
                                                                    RevRateCurr = Convert.ToString(item["RevRateCurrName"]),
                                                                    Isactive = true,// item["Isactive"] == DBNull.Value ? (bool)false : (Convert.ToBoolean(item["Isactive"])),
                                                                    Per = item["Per"] == DBNull.Value ? (int)0 : (Convert.ToInt32(item["Per"])),
                                                                    ToBill = item["ToBill"] == DBNull.Value ? (bool)false : (Convert.ToBoolean(item["ToBill"])),
                                                                    WtUnitName = item["WtUnitName"] == DBNull.Value ? null : (Convert.ToString(item["WtUnitName"])),
                                                                    Rate = item["Rate"] == DBNull.Value ? (decimal?)0 : (Convert.ToDecimal(item["Rate"])),
                                                                    WtVol = item["Wt_Vol_No"] == DBNull.Value ? null : (Convert.ToString(item["Wt_Vol_No"])),
                                                                    /*ConversionRate = item["BaseCurrConversRate"] == DBNull.Value ? 0 : Convert.ToDecimal(item["BaseCurrConversRate"]),
                                                                    Amount = item["RateCurrValue"]==DBNull.Value ? 0 :  Convert.ToDecimal(item["RateCurrValue"]),*/
                                                                }).ToList();

                            CostObj.PackingReport.IsApprove = CostDs.Tables[7] != null && CostDs.Tables[7].Rows.Count > 0 && CostDs.Tables[7].Rows[0]["IsCostSheetApproved"] != DBNull.Value ? Convert.ToBoolean(CostDs.Tables[7].Rows[0]["IsCostSheetApproved"]) : (bool)false;
                            CostObj.PackingReport.ApproveTitle = CostObj.PackingReport.IsApprove ? "Approved" : "Pending";
                        }
                        #endregion
                    }

                    #endregion Packing Info

                    #region Freight Info
                    frtFlag = orgFlag ? CostDs.Tables.Count > 9 && CostDs.Tables[9] != null && CostDs.Tables[9].Rows.Count > 0 && CostDs.Tables[9].Rows[0][0].ToString() == "Same as survey details" : CostDs.Tables.Count > 6 && CostDs.Tables[6] != null && CostDs.Tables[6].Rows.Count > 0 && CostDs.Tables[6].Rows[0][0].ToString() == "Same as survey details";
                    int frttable = orgFlag ? 9 : 6;
                    if (frtFlag)
                    {
                        CostObj.FreightDetail.OrgAgentID = CostObj.PackingDetail.OrgAgentID;
                        CostObj.FreightDetail.FrtAgentID = CostObj.PackingDetail.FrtAgentID;
                        CostObj.FreightDetail.DestAgentID = CostObj.PackingDetail.DestAgentID;
                        CostObj.FreightDetail.ExitPortID = CostObj.PackingDetail.ExitPortID;
                        CostObj.FreightDetail.EntryPortID = CostObj.PackingDetail.EntryPortID;

                        /*CostObj.FreightReport.Packdate = CostObj.PackingReport.Packdate;
                        CostObj.FreightReport.Loaddate = CostObj.PackingReport.Loaddate;
                        CostObj.FreightReport.DensityFact = CostObj.PackingReport.DensityFact;
                        CostObj.FreightReport.VolumeUnitID = CostObj.PackingReport.VolumeUnitID;
                        CostObj.FreightReport.WeightUnitID = CostObj.PackingReport.WeightUnitID;
                        CostObj.FreightReport.TobePackedVol = CostObj.PackingReport.TobePackedVol;
                        CostObj.FreightReport.NetVol = CostObj.PackingReport.NetVol;
                        CostObj.FreightReport.GrossVol = CostObj.PackingReport.GrossVol;
                        CostObj.FreightReport.NetWt = CostObj.PackingReport.NetWt;
                        CostObj.FreightReport.GrossWt = CostObj.PackingReport.GrossWt;
                        CostObj.FreightReport.ACWTWt = CostObj.PackingReport.ACWTWt;*/

                        CostObj.FreightSOList.SOList = CostObj.PackingSOList.SOList;

                        CostObj.FreightCostList.CostList = CostObj.PackingCostList.CostList;
                    }
                    else if (!frtFlag && CostDs.Tables.Count > frttable && CostDs.Tables[frttable] != null /*&& CostDs.Tables[frttable].Rows.Count > 0 && CostDs.Tables[frttable].Rows[0][0].ToString() == "Same as survey details"*/)
                    {
                        #region Freight Details
                        if (CostDs.Tables.Count > frttable && CostDs.Tables[frttable] != null && CostDs.Tables[frttable].Rows.Count > 0)
                        {
                            CostObj.FreightDetail.IsDone = true;
                            CostObj.FreightDetail.OrgAgentID = CostDs.Tables[frttable].Rows[0]["OrgAgentID"] == DBNull.Value ? (int?)null : Convert.ToInt32(CostDs.Tables[frttable].Rows[0]["OrgAgentID"]);
                            CostObj.FreightDetail.FrtAgentID = CostDs.Tables[frttable].Rows[0]["FrtAgentID"] == DBNull.Value ? (int?)null : Convert.ToInt32(CostDs.Tables[frttable].Rows[0]["FrtAgentID"]);
                            CostObj.FreightDetail.DestAgentID = CostDs.Tables[frttable].Rows[0]["DestAgentID"] == DBNull.Value ? (int?)null : Convert.ToInt32(CostDs.Tables[frttable].Rows[0]["DestAgentID"]);
                            CostObj.FreightDetail.ExitPortID = CostDs.Tables[frttable].Rows[0]["ExitPortID"] == DBNull.Value ? (int?)null : Convert.ToInt32(CostDs.Tables[frttable].Rows[0]["ExitPortID"]);
                            CostObj.FreightDetail.EntryPortID = CostDs.Tables[frttable].Rows[0]["EntryPortID"] == DBNull.Value ? (int?)null : Convert.ToInt32(CostDs.Tables[frttable].Rows[0]["EntryPortID"]);
                            CostObj.FreightReport.ShipmentCartedOn = CostDs.Tables[frttable].Rows[0]["ShipmentCartedOn"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(CostDs.Tables[frttable].Rows[0]["ShipmentCartedOn"]);
                            CostObj.FreightReport.CustomeClearedOn = CostDs.Tables[frttable].Rows[0]["CustomeClearedOn"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(CostDs.Tables[frttable].Rows[0]["CustomeClearedOn"]);
                            CostObj.FreightReport.SB_GivenOn = CostDs.Tables[frttable].Rows[0]["SB_GivenOn"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(CostDs.Tables[frttable].Rows[0]["SB_GivenOn"]);
                            CostObj.FreightReport.BLSentToAgentOn = CostDs.Tables[frttable].Rows[0]["BLSentToAgentOn"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(CostDs.Tables[frttable].Rows[0]["BLSentToAgentOn"]);
                            CostObj.FreightReport.BLReleaseOn = CostDs.Tables[frttable].Rows[0]["BLReleasedOn"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(CostDs.Tables[frttable].Rows[0]["BLReleasedOn"]);
                            CostObj.FreightReport.SD = CostDs.Tables[frttable].Rows[0]["ShipDespDateSD"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(CostDs.Tables[frttable].Rows[0]["ShipDespDateSD"]);
                            CostObj.FreightReport.OPS = CostDs.Tables[frttable].Rows[0]["ShipDespDateOP"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(CostDs.Tables[frttable].Rows[0]["ShipDespDateOP"]);

                            CostObj.FreightReport.SSAgent = CostDs.Tables[frttable].Rows[0]["SchAgent"] == DBNull.Value ? null : Convert.ToString(CostDs.Tables[frttable].Rows[0]["SchAgent"]);
                            CostObj.TransitAgent = CostDs.Tables[frttable].Rows[0]["TransitAgentID"] == DBNull.Value ? (int?)null : Convert.ToInt32(CostDs.Tables[frttable].Rows[0]["TransitAgentID"]);
                            CostObj.FreightReport.FS_DS = CostDs.Tables[frttable].Rows[0]["FsOrDs"] == DBNull.Value ? null : Convert.ToString(CostDs.Tables[frttable].Rows[0]["FsOrDs"]);
                            CostObj.FreightReport.ISF_Ref = CostDs.Tables[frttable].Rows[0]["ISFNumber"] == DBNull.Value ? null : Convert.ToString(CostDs.Tables[frttable].Rows[0]["ISFNumber"]);
                            CostObj.FreightReport.TruckNo = CostDs.Tables[frttable].Rows[0]["TruckNo"] == DBNull.Value ? null : Convert.ToString(CostDs.Tables[frttable].Rows[0]["TruckNo"]);
                            CostObj.FreightReport.VehicleTypeId = CostDs.Tables[frttable].Rows[0]["VehicleType"] == DBNull.Value ? 0 : Convert.ToInt32(CostDs.Tables[frttable].Rows[0]["VehicleType"]);
                            CostObj.FreightReport.TotalCapacity = CostDs.Tables[frttable].Rows[0]["TotalCapacity"] == DBNull.Value ? 0 : Convert.ToInt32(CostDs.Tables[frttable].Rows[0]["TotalCapacity"]);
                            CostObj.FreightReport.EsordedBy = CostDs.Tables[frttable].Rows[0]["EscortedBy"] == DBNull.Value ? null : Convert.ToString(CostDs.Tables[frttable].Rows[0]["EscortedBy"]);
                            CostObj.FreightReport.LeftOnDate = CostDs.Tables[frttable].Rows[0]["VehLeftOnDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(CostDs.Tables[frttable].Rows[0]["VehLeftOnDate"]);
                            CostObj.FreightReport.Courier = CostDs.Tables[frttable].Rows[0]["Courier"] == DBNull.Value ? 0 : Convert.ToInt32(CostDs.Tables[frttable].Rows[0]["Courier"]);
                            CostObj.FreightReport.AirLine = CostDs.Tables[frttable].Rows[0]["AirLines"] == DBNull.Value ? 0 : Convert.ToInt32(CostDs.Tables[frttable].Rows[0]["AirLines"]);
                            CostObj.FreightReport.ContainerNo = CostDs.Tables[frttable].Rows[0]["ContainerNo"] == DBNull.Value ? null : Convert.ToString(CostDs.Tables[frttable].Rows[0]["ContainerNo"]);
                            CostObj.FreightReport.SealNo = CostDs.Tables[frttable].Rows[0]["SealNo"] == DBNull.Value ? null : Convert.ToString(CostDs.Tables[frttable].Rows[0]["SealNo"]);
                            CostObj.FreightReport.NoOfPacks = CostDs.Tables[frttable].Rows[0]["NoOfPacks"] == DBNull.Value ? 0 : Convert.ToInt32(CostDs.Tables[frttable].Rows[0]["NoOfPacks"]);
                            CostObj.FreightReport.Bill_No = CostDs.Tables[frttable].Rows[0]["BillNo"] == DBNull.Value ? null : Convert.ToString(CostDs.Tables[frttable].Rows[0]["BillNo"]);
                            CostObj.FreightReport.TransitShipment = Convert.ToBoolean(CostDs.Tables[frttable].Rows[0]["TransitShipment"]);
                            CostObj.FreightReport.DeliveryDate = CostDs.Tables[frttable].Rows[0]["SchDeliveryDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(CostDs.Tables[frttable].Rows[0]["SchDeliveryDate"]);

                            CostObj.FreightReport.IsISF = Convert.ToBoolean(CostDs.Tables[frttable].Rows[0]["IsISF"]);
                            CostObj.FreightReport.IsSD = Convert.ToBoolean(CostDs.Tables[frttable].Rows[0]["IsShipDespDateSDs"]);
                            CostObj.FreightReport.IsDirectCarting = Convert.ToBoolean(CostDs.Tables[frttable].Rows[0]["IsDirectCarting"]);
                            CostObj.FreightReport.IsBLSentToAgent = Convert.ToBoolean(CostDs.Tables[frttable].Rows[0]["IsBLSentToAgent"]);
                            CostObj.FreightReport.PortLoad = CostDs.Tables[frttable].Rows[0]["PortLoadID"] == DBNull.Value ? 0 : Convert.ToInt32(CostDs.Tables[frttable].Rows[0]["PortLoadID"]);
                            CostObj.FreightReport.PortDischarge = CostDs.Tables[frttable].Rows[0]["PortDischargeID"] == DBNull.Value ? 0 : Convert.ToInt32(CostDs.Tables[frttable].Rows[0]["PortDischargeID"]);
                            CostObj.FreightReport.LCL_FCL = CostDs.Tables[frttable].Rows[0]["LCLFCL"] == DBNull.Value ? null : Convert.ToString(CostDs.Tables[frttable].Rows[0]["LCLFCL"]);
                            CostObj.FreightReport.ForwardingBr = CostDs.Tables[frttable].Rows[0]["ForwardingBrID"] == DBNull.Value ? (int?)null : Convert.ToInt32(CostDs.Tables[frttable].Rows[0]["ForwardingBrID"]);
                            CostObj.FreightReport.FCL_20 = CostDs.Tables[frttable].Rows[0]["FCL20"] == DBNull.Value ? (int?)null : Convert.ToInt32(CostDs.Tables[frttable].Rows[0]["FCL20"]);
                            CostObj.FreightReport.FCL_40 = CostDs.Tables[frttable].Rows[0]["FCL40"] == DBNull.Value ? (int?)null : Convert.ToInt32(CostDs.Tables[frttable].Rows[0]["FCL40"]);
                            CostObj.FreightReport.FCLHC_40 = CostDs.Tables[frttable].Rows[0]["FCLHC40"] == DBNull.Value ? (int?)null : Convert.ToInt32(CostDs.Tables[frttable].Rows[0]["FCLHC40"]);
                            CostObj.FreightReport.THCCollect = CostDs.Tables[frttable].Rows[0]["THCCollect"] == DBNull.Value ? false : Convert.ToBoolean(CostDs.Tables[frttable].Rows[0]["THCCollect"]);
                            CostObj.FreightReport.THCPrepaid = CostDs.Tables[frttable].Rows[0]["THCPrepaid"] == DBNull.Value ? false : Convert.ToBoolean(CostDs.Tables[frttable].Rows[0]["THCPrepaid"]);
                            CostObj.FreightReport.SSLAgentId = CostDs.Tables[frttable].Rows[0]["SSLAgentID"] == DBNull.Value ? (int?)null : Convert.ToInt32(CostDs.Tables[frttable].Rows[0]["SSLAgentID"]);
                            CostObj.FreightReport.SSLCarrierId = CostDs.Tables[frttable].Rows[0]["CarrierID"] == DBNull.Value ? (int?)null : Convert.ToInt32(CostDs.Tables[frttable].Rows[0]["CarrierID"]);
                            CostObj.FreightReport.SSLAgent = Convert.ToString(CostDs.Tables[frttable].Rows[0]["SSLAgentName"]);
                            CostObj.FreightReport.SSLCarrier = Convert.ToString(CostDs.Tables[frttable].Rows[0]["SSLCarrierName"]);
                        }
                        #endregion
                        frttable++;
                        #region Freight SO
                        if (CostDs.Tables.Count > 6 && CostDs.Tables[frttable] != null && CostDs.Tables[frttable].Rows.Count > 0)
                        {
                            CostObj.FreightSOList.SOList = (from item in CostDs.Tables[frttable].AsEnumerable()
                                                            select new PackingSOList
                                                            {
                                                                CostHeadID = item["CostHeadID"] == DBNull.Value ? (int?)0 : (Convert.ToInt32(item["CostHeadID"])),
                                                                CostHeadName = Convert.ToString(item["CostHeadName"]),
                                                                Remark = Convert.ToString(item["ServOrderRemarks"]),
                                                                RateCompId = Convert.ToInt32(RELOCBS.Common.CommonService.RateComp.Origin.ToString()),
                                                                Volume = Convert.ToString(item["WtVolume"]),
                                                                WtUnitID = item["WtUnitID"] == DBNull.Value ? (int)0 : (Convert.ToInt32(item["WtUnitID"])),
                                                                WtUnit = Convert.ToString(item["WeightUnitName"]),
                                                                Isactive = true// item["Isactive"] == DBNull.Value ? (bool)false : (Convert.ToBoolean(item["Isactive"])),
                                                            }).ToList();
                        }
                        #endregion
                        frttable++;
                        #region Freight Report

                        if (CostDs.Tables.Count > frttable && CostDs.Tables[frttable] != null && CostDs.Tables[frttable].Rows.Count > 0)
                        {
                            /*CostObj.FreightReport.Packdate = CostDs.Tables[frttable].Rows[0]["PackDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(CostDs.Tables[frttable].Rows[0]["PackDate"]); //(DateTime?)CostDs.Tables[frttable].Rows[0]["PackDate"];
                            CostObj.FreightReport.Loaddate = CostDs.Tables[frttable].Rows[0]["LoadDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(CostDs.Tables[frttable].Rows[0]["LoadDate"]); //(DateTime?)CostDs.Tables[frttable].Rows[0]["LoadDate"];
                            CostObj.FreightReport.DensityFact = CostDs.Tables[frttable].Rows[0]["DensityFact"] == DBNull.Value ? (decimal)0.00 : Convert.ToDecimal(CostDs.Tables[frttable].Rows[0]["DensityFact"]);
                            CostObj.FreightReport.VolumeUnitID = CostDs.Tables[frttable].Rows[0]["VolumeUnitID"] == DBNull.Value ? 0 : Convert.ToInt32(CostDs.Tables[frttable].Rows[0]["VolumeUnitID"]);
                            CostObj.FreightReport.WeightUnitID = CostDs.Tables[frttable].Rows[0]["WeightUnitID"] == DBNull.Value ? 0 : Convert.ToInt32(CostDs.Tables[frttable].Rows[0]["WeightUnitID"]);
                            CostObj.FreightReport.TobePackedVol = CostDs.Tables[frttable].Rows[0]["TobePackedVol"] == DBNull.Value ? (decimal)0.00 : Convert.ToDecimal(CostDs.Tables[frttable].Rows[0]["TobePackedVol"]);
                            CostObj.FreightReport.NetVol = CostDs.Tables[frttable].Rows[0]["NetVol"] == DBNull.Value ? (decimal)0.00 : Convert.ToDecimal(CostDs.Tables[frttable].Rows[0]["NetVol"]);
                            CostObj.FreightReport.GrossVol = CostDs.Tables[frttable].Rows[0]["GrossVol"] == DBNull.Value ? (decimal)0.00 : Convert.ToDecimal(CostDs.Tables[frttable].Rows[0]["GrossVol"]);
                            CostObj.FreightReport.NetWt = CostDs.Tables[frttable].Rows[0]["NetWt"] == DBNull.Value ? (decimal)0.00 : Convert.ToDecimal(CostDs.Tables[frttable].Rows[0]["NetWt"]);
                            CostObj.FreightReport.GrossWt = CostDs.Tables[frttable].Rows[0]["GrossWt"] == DBNull.Value ? (decimal)0.00 : Convert.ToDecimal(CostDs.Tables[frttable].Rows[0]["GrossWt"]);
                            CostObj.FreightReport.ACWTWt = CostDs.Tables[frttable].Rows[0]["ACWTWt"] == DBNull.Value ? (decimal)0.00 : Convert.ToDecimal(CostDs.Tables[frttable].Rows[0]["ACWTWt"]);*/

                            if (CostDs.Tables[frttable].Columns.Contains("OrderNo") && CostDs.Tables[frttable].AsEnumerable().Where(m => m.Field<int?>("OrderNo") == null).Count() > 0)
                            {
                                DataColumn column = CostDs.Tables[frttable].Columns["OrderNo"];
                                column.AutoIncrement = true;
                                column.AutoIncrementSeed = 1;
                                column.AutoIncrementStep = 1;
                                column.DataType = typeof(Int32);
                                int index = 1;
                                foreach (DataRow row in CostDs.Tables[frttable].Rows)
                                {
                                    row.SetField(column, index);
                                    index++;
                                }
                            }
                            CostDs.Tables[frttable].Columns.Add(new DataColumn("FromPortId", typeof(Int32)));
                            DataRow previousRow = null;
                            string TransitPortColumn = "TransitPortID";
                            int TransitRowNo = 0;
                            foreach (DataRow dr in CostDs.Tables[frttable].Rows) // Here ldtAlbum is the Datatable
                            {
                                if (TransitRowNo == 0)
                                {
                                    dr["FromPortId"] = CostObj.FreightReport.PortLoad;
                                }
                                else
                                {
                                    dr["FromPortId"] = previousRow[TransitPortColumn];
                                }
                                if ((TransitRowNo + 1) == CostDs.Tables[frttable].Rows.Count)
                                {
                                    dr["TransitPortID"] = CostObj.FreightReport.PortDischarge;
                                }
                                previousRow = dr;
                                TransitRowNo++;
                            }

                            CostObj.FreightReport.TranShipmentList = (from item in CostDs.Tables[frttable].AsEnumerable()
                                                                      select new TranShipment
                                                                      {
                                                                          ScheduleVessel = Convert.ToString(item["SchVessel"]),
                                                                          FromPortId = Convert.ToInt32(item["FromPortId"]),
                                                                          ETD = string.IsNullOrEmpty(Convert.ToString(item["EDD"])) ? (DateTime?)null : Convert.ToDateTime(item["EDD"]),
                                                                          ETA = string.IsNullOrEmpty(Convert.ToString(item["EDA"])) ? (DateTime?)null : Convert.ToDateTime(item["EDA"]),
                                                                          ActArr = string.IsNullOrEmpty(Convert.ToString(item["ActDA"])) ? (DateTime?)null : Convert.ToDateTime(item["ActDA"]),
                                                                          ActDep = string.IsNullOrEmpty(Convert.ToString(item["ActDD"])) ? (DateTime?)null : Convert.ToDateTime(item["ActDD"]),
                                                                          TranshipPortId = string.IsNullOrEmpty(Convert.ToString(item["TransitPortID"])) ? (int?)null : Convert.ToInt32(item["TransitPortID"]),
                                                                          TranshipPortName = Convert.ToString(item["PortName"]),
                                                                          Isactive = string.IsNullOrEmpty(Convert.ToString(item["Isactive"])) ? (bool)false : (Convert.ToBoolean(item["Isactive"])),
                                                                          OrderNo = Convert.ToInt32(item["OrderNo"])
                                                                      }).ToList();
                        }
                        #endregion
                        frttable++;
                        #region freight Cost   
                        if (CostDs.Tables.Count > frttable && CostDs.Tables[frttable] != null && CostDs.Tables[frttable].Rows.Count > 0)
                        {
                            CostObj.FreightCostList.CostList = (from item in CostDs.Tables[frttable].AsEnumerable()
                                                                select new PackingCostList
                                                                {
                                                                    RateCompId = item["CompId"] == DBNull.Value ? (int)0 : (Convert.ToInt32(item["CompId"])),
                                                                    RateCompName = Convert.ToString(item["RateComponentName"]),

                                                                    CostHeadID = item["CostHeadID"] == DBNull.Value ? (int?)0 : (Convert.ToInt32(item["CostHeadID"])),

                                                                    CostHeadName = Convert.ToString(item["CostHeadName"]),
                                                                    BaseCurrID = item["BaseCurrID"] == DBNull.Value ? (int)0 : (Convert.ToInt32(item["BaseCurrID"])),

                                                                    RateCurrID = item["RateCurrID"] == DBNull.Value ? (int)0 : (Convert.ToInt32(item["RateCurrID"])),
                                                                    RevRateCurrID = item["RevRateCurrID"] == DBNull.Value ? (int)0 : (Convert.ToInt32(item["RevRateCurrID"])),
                                                                    RevConversionRate = item["RevConversionRate"] == DBNull.Value ? (decimal?)0 : (Convert.ToDecimal(item["RevConversionRate"])),
                                                                    CostValue = item["CostValue"] == DBNull.Value ? (decimal?)0 : (Convert.ToDecimal(item["CostValue"])),
                                                                    BaseRevenueValue = item["BaseRev"] == DBNull.Value ? (decimal?)0 : (Convert.ToDecimal(item["BaseRev"])),
                                                                    RevenueValue = item["Revenuevalue"] == DBNull.Value ? (decimal?)0 : (Convert.ToDecimal(item["Revenuevalue"])),

                                                                    BaseCurr = Convert.ToString(item["BaseCurrName"]),
                                                                    RateCurr = Convert.ToString(item["RateCurrName"]),
                                                                    RevRateCurr = Convert.ToString(item["RevRateCurrName"]),
                                                                    Balance = item["Revenuevalue"] == DBNull.Value ? (decimal?)0 : (Convert.ToDecimal(item["Revenuevalue"]))

                                                                    ,
                                                                    Isactive = true//item["Isactive"] == DBNull.Value ? (bool)false : (Convert.ToBoolean(item["Isactive"])),

                                                                    /*ConversionRate = item["BaseCurrConversRate"] == DBNull.Value ? 0 : Convert.ToDecimal(item["BaseCurrConversRate"]),
                                                                    Amount = item["RateCurrValue"]==DBNull.Value ? 0 :  Convert.ToDecimal(item["RateCurrValue"]),*/
                                                                }).ToList();
                        }
                        #endregion
                    }

                    #endregion Freight Info

                    #region Destination Info
                    frttable++;

                    int destttable = frttable;
                    destFlag = frtFlag ? CostDs.Tables.Count > destttable && CostDs.Tables[destttable] != null && CostDs.Tables[destttable].Rows.Count > 0 && CostDs.Tables[destttable].Rows[0][0].ToString() == "Same as survey details" : CostDs.Tables.Count > destttable && CostDs.Tables[destttable] != null && CostDs.Tables[destttable].Rows.Count > 0 && CostDs.Tables[destttable].Rows[0][0].ToString() == "Same as survey details";

                    if (destFlag)
                    {
                        CostObj.DeliveryDetail.OrgAgentID = CostObj.PackingDetail.OrgAgentID;
                        CostObj.DeliveryDetail.FrtAgentID = CostObj.PackingDetail.FrtAgentID;
                        CostObj.DeliveryDetail.DestAgentID = CostObj.PackingDetail.DestAgentID;
                        CostObj.DeliveryDetail.ExitPortID = CostObj.PackingDetail.ExitPortID;
                        CostObj.DeliveryDetail.EntryPortID = CostObj.PackingDetail.EntryPortID;

                        /*CostObj.DeliveryReport.Packdate = CostObj.PackingReport.Packdate;
                        CostObj.DeliveryReport.Loaddate = CostObj.PackingReport.Loaddate;
                        CostObj.DeliveryReport.DensityFact = CostObj.PackingReport.DensityFact;
                        CostObj.DeliveryReport.VolumeUnitID = CostObj.PackingReport.VolumeUnitID;
                        CostObj.DeliveryReport.WeightUnitID = CostObj.PackingReport.WeightUnitID;
                        CostObj.DeliveryReport.TobePackedVol = CostObj.PackingReport.TobePackedVol;
                        CostObj.DeliveryReport.NetVol = CostObj.PackingReport.NetVol;
                        CostObj.DeliveryReport.GrossVol = CostObj.PackingReport.GrossVol;
                        CostObj.DeliveryReport.NetWt = CostObj.PackingReport.NetWt;
                        CostObj.DeliveryReport.GrossWt = CostObj.PackingReport.GrossWt;
                        CostObj.DeliveryReport.ACWTWt = CostObj.PackingReport.ACWTWt;*/

                        CostObj.DeliverySOList.SOList = CostObj.PackingSOList.SOList;

                        CostObj.DeliveryCostList.CostList = CostObj.PackingCostList.CostList;
                    }
                    else if (!destFlag && CostDs.Tables.Count > destttable && CostDs.Tables[destttable] != null /*&& CostDs.Tables[destttable].Rows.Count > 0 && CostDs.Tables[destttable].Rows[0][0].ToString() == "Same as survey details"*/)
                    {
                        #region Destination Details
                        if (CostDs.Tables.Count > destttable && CostDs.Tables[destttable] != null && CostDs.Tables[destttable].Rows.Count > 0)
                        {
                            CostObj.DeliveryDetail.IsDone = true;
                            CostObj.DeliveryDetail.OrgAgentID = CostDs.Tables[destttable].Rows[0]["OrgAgentID"] == DBNull.Value ? (int?)null : Convert.ToInt32(CostDs.Tables[destttable].Rows[0]["OrgAgentID"]);
                            CostObj.DeliveryDetail.OrgAgentName = Convert.ToString(CostDs.Tables[destttable].Rows[0]["OrgAgentName"]);
                            CostObj.DeliveryDetail.FrtAgentID = CostDs.Tables[destttable].Rows[0]["FrtAgentID"] == DBNull.Value ? (int?)null : Convert.ToInt32(CostDs.Tables[destttable].Rows[0]["FrtAgentID"]);
                            CostObj.DeliveryDetail.DestAgentID = CostDs.Tables[destttable].Rows[0]["DestAgentID"] == DBNull.Value ? (int?)null : Convert.ToInt32(CostDs.Tables[destttable].Rows[0]["DestAgentID"]);
                            CostObj.DeliveryDetail.DestAgentName = Convert.ToString(CostDs.Tables[destttable].Rows[0]["DestAgentName"]);
                            CostObj.DeliveryDetail.ExitPortID = CostDs.Tables[destttable].Rows[0]["ExitPortID"] == DBNull.Value ? (int?)null : Convert.ToInt32(CostDs.Tables[destttable].Rows[0]["ExitPortID"]);
                            CostObj.DeliveryDetail.ExitPortName = Convert.ToString(CostDs.Tables[destttable].Rows[0]["ExitPortName"]);
                            CostObj.DeliveryDetail.EntryPortID = CostDs.Tables[destttable].Rows[0]["EntryPortID"] == DBNull.Value ? (int?)null : Convert.ToInt32(CostDs.Tables[destttable].Rows[0]["EntryPortID"]);
                            CostObj.DeliveryDetail.EntryPortName = Convert.ToString(CostDs.Tables[destttable].Rows[0]["EntryPortName"]);

                            CostObj.IsDestApprove = Convert.ToBoolean(CostDs.Tables[destttable].Rows[0]["IsCostSheetApproved"]);

                            CostObj.DestApprove = CostObj.IsDestApprove ? "Approved" : "Pending";
                            if (CostObj.MoveJob.ModeID != 3)
                            {
                                if (!string.IsNullOrEmpty(Convert.ToString(CostDs.Tables[destttable].Rows[0]["DestAgentName"])) &&
                                CostObj.MoveJob.MoveRateCompList.Where(x => x.RateComponentID == 3).Count() <= 0)
                                {
                                    CostObj.MoveJob.MoveRateCompList.Add(new MoveRateComponent() { RateComponentID = 3, RateComponentName = "Destination" });
                                }


                                CostObj.MoveJob.MoveRateCompList = CostObj.MoveJob.MoveRateCompList.Select(i =>
                                {
                                    if (i.RateComponentID == 1) i.ActJobAgentName = Convert.ToString(CostDs.Tables[destttable].Rows[0]["OrgAgentName"]);
                                    if (i.RateComponentID == 1) i.ActJobAgentID = Convert.ToString(CostDs.Tables[destttable].Rows[0]["OrgAgentID"]);
                                    if (i.RateComponentID == 2) i.ActJobAgentName = Convert.ToString(CostDs.Tables[destttable].Rows[0]["OrgAgentName"]);
                                    if (i.RateComponentID == 2) i.ActJobAgentID = Convert.ToString(CostDs.Tables[destttable].Rows[0]["OrgAgentID"]);
                                    if (i.RateComponentID == 3) i.ActJobAgentName = Convert.ToString(CostDs.Tables[destttable].Rows[0]["DestAgentName"]);
                                    if (i.RateComponentID == 2) i.ActJobAgentID = Convert.ToString(CostDs.Tables[destttable].Rows[0]["DestAgentID"]);
                                    return i;
                                }).ToList();
                            }
                            else if (CostObj.MoveJob.ModeID == 3 && CostObj.MoveJob.MoveRateCompList.Count() > 1)
                            {
                                if (!string.IsNullOrEmpty(Convert.ToString(CostDs.Tables[destttable].Rows[0]["DestAgentName"])) &&
                                CostObj.MoveJob.MoveRateCompList.Where(x => x.RateComponentID == 3).Count() <= 0)
                                {
                                    CostObj.MoveJob.MoveRateCompList.Add(new MoveRateComponent() { RateComponentID = 3, RateComponentName = "Destination" });
                                }
                                if (!string.IsNullOrEmpty(Convert.ToString(CostDs.Tables[destttable].Rows[0]["OrgAgentName"])) &&
                                    CostObj.MoveJob.MoveRateCompList.Where(x => x.RateComponentID == 1).Count() <= 0)
                                {

                                    CostObj.MoveJob.MoveRateCompList.Add(new MoveRateComponent() { RateComponentID = 1, RateComponentName = "Origin" });
                                    CostObj.MoveJob.MoveRateCompList.Add(new MoveRateComponent() { RateComponentID = 2, RateComponentName = "Freight" });
                                }

                                CostObj.MoveJob.MoveRateCompList = CostObj.MoveJob.MoveRateCompList.Select(i =>
                                {
                                    if (i.RateComponentID == 1) i.ActJobAgentName = Convert.ToString(CostDs.Tables[destttable].Rows[0]["OrgAgentName"]);
                                    if (i.RateComponentID == 1) i.ActJobAgentID = Convert.ToString(CostDs.Tables[destttable].Rows[0]["OrgAgentID"]);
                                    if (i.RateComponentID == 2) i.ActJobAgentName = Convert.ToString(CostDs.Tables[destttable].Rows[0]["FrtAgentName"]);
                                    if (i.RateComponentID == 2) i.ActJobAgentID = Convert.ToString(CostDs.Tables[destttable].Rows[0]["FrtAgentID"]);
                                    if (i.RateComponentID == 3) i.ActJobAgentName = Convert.ToString(CostDs.Tables[destttable].Rows[0]["DestAgentName"]);
                                    if (i.RateComponentID == 3) i.ActJobAgentID = Convert.ToString(CostDs.Tables[destttable].Rows[0]["DestAgentID"]);
                                    return i;
                                }).ToList();
                            }

                        }
                        #endregion
                        destttable++;
                        #region Destination SO
                        if (CostDs.Tables.Count > 6 && CostDs.Tables[destttable] != null && CostDs.Tables[destttable].Rows.Count > 0)
                        {
                            CostObj.DeliverySOList.SOList = (from item in CostDs.Tables[destttable].AsEnumerable()
                                                             select new PackingSOList
                                                             {
                                                                 CostHeadID = item["CostHeadID"] == DBNull.Value ? (int?)0 : (Convert.ToInt32(item["CostHeadID"])),
                                                                 CostHeadName = Convert.ToString(item["CostHeadName"]),
                                                                 Remark = Convert.ToString(item["ServOrderRemarks"]),
                                                                 RateCompId = Convert.ToInt32(RELOCBS.Common.CommonService.RateComp.Origin.ToString()),
                                                                 Volume = Convert.ToString(item["WtVolume"]),
                                                                 WtUnitID = item["WtUnitID"] == DBNull.Value ? (int)0 : (Convert.ToInt32(item["WtUnitID"])),
                                                                 WtUnit = Convert.ToString(item["WeightUnitName"]),
                                                                 Isactive = true//item["Isactive"] == DBNull.Value ? (bool)false : (Convert.ToBoolean(item["Isactive"])),
                                                             }).ToList();
                        }
                        #endregion
                        destttable++;
                        #region Destination Report
                        if (CostDs.Tables.Count > destttable && CostDs.Tables[destttable] != null && CostDs.Tables[destttable].Rows.Count > 0)
                        {
                            //CostObj.DeliveryReport.Packdate = CostDs.Tables[destttable].Rows[0]["PackDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(CostDs.Tables[destttable].Rows[0]["PackDate"]); //(DateTime?)CostDs.Tables[destttable].Rows[0]["PackDate"];
                            //CostObj.DeliveryReport.Loaddate = CostDs.Tables[destttable].Rows[0]["LoadDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(CostDs.Tables[destttable].Rows[0]["LoadDate"]); //(DateTime?)CostDs.Tables[destttable].Rows[0]["LoadDate"];
                            CostObj.DeliveryReport.DeliveryDate = CostDs.Tables[destttable].Rows[0]["Deliverydate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(CostDs.Tables[destttable].Rows[0]["Deliverydate"]); //(DateTime?)CostDs.Tables[destttable].Rows[0]["LoadDate"];
                            CostObj.DeliveryReport.PassportNo = CostDs.Tables[destttable].Rows[0]["PassportNo"] == DBNull.Value ? null : Convert.ToString(CostDs.Tables[destttable].Rows[0]["PassportNo"]);
                            CostObj.DeliveryReport.DensityFact = CostDs.Tables[destttable].Rows[0]["DensityFact"] == DBNull.Value ? (decimal)0.00 : Convert.ToDecimal(CostDs.Tables[destttable].Rows[0]["DensityFact"]);
                            CostObj.DeliveryReport.VolumeUnitID = CostDs.Tables[destttable].Rows[0]["VolumeUnitID"] == DBNull.Value ? 0 : Convert.ToInt32(CostDs.Tables[destttable].Rows[0]["VolumeUnitID"]);
                            CostObj.DeliveryReport.WeightUnitID = CostDs.Tables[destttable].Rows[0]["WeightUnitID"] == DBNull.Value ? 0 : Convert.ToInt32(CostDs.Tables[destttable].Rows[0]["WeightUnitID"]);
                            CostObj.DeliveryReport.TobePackedVol = CostDs.Tables[destttable].Rows[0]["TobePackedVol"] == DBNull.Value ? (decimal)0.00 : Convert.ToDecimal(CostDs.Tables[destttable].Rows[0]["TobePackedVol"]);
                            CostObj.DeliveryReport.NetVol = CostDs.Tables[destttable].Rows[0]["NetVol"] == DBNull.Value ? (decimal)0.00 : Convert.ToDecimal(CostDs.Tables[destttable].Rows[0]["NetVol"]);
                            CostObj.DeliveryReport.GrossVol = CostDs.Tables[destttable].Rows[0]["GrossVol"] == DBNull.Value ? (decimal)0.00 : Convert.ToDecimal(CostDs.Tables[destttable].Rows[0]["GrossVol"]);
                            CostObj.DeliveryReport.NetWt = CostDs.Tables[destttable].Rows[0]["NetWt"] == DBNull.Value ? (decimal)0.00 : Convert.ToDecimal(CostDs.Tables[destttable].Rows[0]["NetWt"]);
                            CostObj.DeliveryReport.GrossWt = CostDs.Tables[destttable].Rows[0]["GrossWt"] == DBNull.Value ? (decimal)0.00 : Convert.ToDecimal(CostDs.Tables[destttable].Rows[0]["GrossWt"]);
                            CostObj.DeliveryReport.ACWTWt = CostDs.Tables[destttable].Rows[0]["ACWTWt"] == DBNull.Value ? (decimal)0.00 : Convert.ToDecimal(CostDs.Tables[destttable].Rows[0]["ACWTWt"]);
                            CostObj.DeliveryReport.ContainerSizeID = CostDs.Tables[destttable].Rows[0]["ContainerSizeID"] == DBNull.Value ? 0 : Convert.ToInt32(CostDs.Tables[destttable].Rows[0]["ContainerSizeID"]);
                            CostObj.DeliveryReport.CSCreatedBY = CostDs.Tables[destttable].Rows[0]["CSPreparedBy"] == DBNull.Value ? null : Convert.ToString(CostDs.Tables[destttable].Rows[0]["CSPreparedBy"]);
                            CostObj.DeliveryReport.CSCreatedDate = CostDs.Tables[destttable].Rows[0]["CSPreparedDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(CostDs.Tables[destttable].Rows[0]["CSPreparedDate"]);
                            CostObj.DeliveryReport.CSApprovedBY = CostDs.Tables[destttable].Rows[0]["CSApprovedBY"] == DBNull.Value ? null : Convert.ToString(CostDs.Tables[destttable].Rows[0]["CSApprovedBY"]);
                            CostObj.DeliveryReport.CSApprovedDate = CostDs.Tables[destttable].Rows[0]["CSApprovedDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(CostDs.Tables[destttable].Rows[0]["CSApprovedDate"]);
                            CostObj.DeliveryReport.ServiceEvaluation = CostDs.Tables[destttable].Rows[0]["IsServiceEvaluation"] == DBNull.Value ? false : Convert.ToBoolean(CostDs.Tables[destttable].Rows[0]["IsServiceEvaluation"]);
                            CostObj.DeliveryReport.ServiceEvaluationScore = CostDs.Tables[destttable].Rows[0]["ServiceEvaluationScore"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(CostDs.Tables[destttable].Rows[0]["ServiceEvaluationScore"]);
                            CostObj.DeliveryReport.ServiceEvaluationRemarks = CostDs.Tables[destttable].Rows[0]["ServiceEvaluationRemark"] == DBNull.Value ? null : Convert.ToString(CostDs.Tables[destttable].Rows[0]["ServiceEvaluationRemark"]);
                            CostObj.DeliveryReport.DestStgStartDate = CostDs.Tables[destttable].Rows[0]["DestStgStartDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(CostDs.Tables[destttable].Rows[0]["DestStgStartDate"]);
                            CostObj.DeliveryReport.DestStgEndDate = CostDs.Tables[destttable].Rows[0]["DestStgEndDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(CostDs.Tables[destttable].Rows[0]["DestStgEndDate"]);
                            CostObj.DeliveryReport.RoadKMS = CostDs.Tables[destttable].Rows[0]["RoadKMS"] == DBNull.Value ? (decimal)0.00 : Convert.ToDecimal(CostDs.Tables[destttable].Rows[0]["RoadKMS"]);
                            CostObj.DeliveryReport.IsClaim = CostDs.Tables[destttable].Rows[0]["IsClaim"] == DBNull.Value ? false : Convert.ToBoolean(CostDs.Tables[destttable].Rows[0]["IsClaim"]);

                            CostObj.IsCSSenttoApprove = CostDs.Tables[destttable].Rows[0]["IsSendToApproval"] == DBNull.Value ? false : Convert.ToBoolean(CostDs.Tables[destttable].Rows[0]["IsSendToApproval"]);
                            CostObj.CSSenttoApproveUser = CostDs.Tables[destttable].Rows[0]["ApprovalUserID"] == DBNull.Value ? (Int32?)0 : Convert.ToInt32(CostDs.Tables[destttable].Rows[0]["ApprovalUserID"]);

                            if (CostObj.MoveJob.ModeID != 2)
                            {
                                CostObj.DeliveryReport.LCLorFCL = CostDs.Tables[destttable].Rows[0]["LCLorFCL"] == DBNull.Value ? null : Convert.ToString(CostDs.Tables[destttable].Rows[0]["LCLorFCL"]);
                                CostObj.DeliveryReport.LooseCased = CostDs.Tables[destttable].Rows[0]["LooseCased"] == DBNull.Value ? null : Convert.ToString(CostDs.Tables[destttable].Rows[0]["LooseCased"]);
                            }
                            else
                            {
                                CostObj.DeliveryReport.LCLorFCL = CostDs.Tables[destttable].Rows[0]["LCLorFCL"] == DBNull.Value ? "LCL" : Convert.ToString(CostDs.Tables[destttable].Rows[0]["LCLorFCL"]);
                                CostObj.DeliveryReport.LooseCased = CostDs.Tables[destttable].Rows[0]["LooseCased"] == DBNull.Value ? "Loose" : Convert.ToString(CostDs.Tables[destttable].Rows[0]["LooseCased"]);
                            }


                        }
                        else
                        {
                            CostObj.DeliveryReport.LCLorFCL = "LCL";
                            CostObj.DeliveryReport.LooseCased = "Loose";
                        }
                        #endregion
                        destttable++;
                        #region Destination Cost   
                        if (CostDs.Tables.Count > destttable && CostDs.Tables[destttable] != null && CostDs.Tables[destttable].Rows.Count > 0)
                        {
                            CostObj.DeliveryCostList.CostListSaved = Convert.ToString(CostDs.Tables[destttable].Rows[0]["IsDeliveryCostSaved"]) == "Y";
                            CostObj.DeliveryCostList.CostList = (from item in CostDs.Tables[destttable].AsEnumerable()
                                                                 select new PackingCostList
                                                                 {
                                                                     RateCompId = item["CompId"] == DBNull.Value ? (int)0 : (Convert.ToInt32(item["CompId"])),
                                                                     RateCompName = Convert.ToString(item["RateComponentName"]),

                                                                     CostHeadID = item["CostHeadID"] == DBNull.Value ? (int?)0 : (Convert.ToInt32(item["CostHeadID"])),

                                                                     CostHeadName = Convert.ToString(item["CostHeadName"]),
                                                                     BaseCurrID = item["BaseCurrID"] == DBNull.Value ? (int)0 : (Convert.ToInt32(item["BaseCurrID"])),

                                                                     RateCurrID = item["RateCurrID"] == DBNull.Value ? (int)0 : (Convert.ToInt32(item["RateCurrID"])),
                                                                     RevRateCurrID = item["RevRateCurrID"] == DBNull.Value ? (int)0 : (Convert.ToInt32(item["RevRateCurrID"])),
                                                                     ConversionRate = item["ConversionRate"] == DBNull.Value ? (decimal?)0 : (Convert.ToDecimal(item["ConversionRate"])),
                                                                     RevConversionRate = item["RevConversionRate"] == DBNull.Value ? (decimal?)0 : (Convert.ToDecimal(item["RevConversionRate"])),
                                                                     CostValue = item["CostValue"] == DBNull.Value ? (decimal?)0 : (Convert.ToDecimal(item["CostValue"])),
                                                                     BaseRevenueValue = item["BaseRev"] == DBNull.Value ? (decimal?)0 : (Convert.ToDecimal(item["BaseRev"])),
                                                                     RevenueValue = item["Revenuevalue"] == DBNull.Value ? (decimal?)0 : (Convert.ToDecimal(item["Revenuevalue"])),
                                                                     Balance = item["UnBilled"] == DBNull.Value ? item["Revenuevalue"] == DBNull.Value ? (decimal?)0 : (Convert.ToDecimal(item["Revenuevalue"])) : (Convert.ToDecimal(item["UnBilled"])),
                                                                     BaseCurr = Convert.ToString(item["BaseCurrName"]),
                                                                     RateCurr = Convert.ToString(item["RateCurrName"]),
                                                                     RevRateCurr = Convert.ToString(item["RevRateCurrName"]),
                                                                     Isactive = true,//item["Isactive"] == DBNull.Value ? (bool)false : (Convert.ToBoolean(item["Isactive"])),
                                                                     ToBill = item["ToBill"] == DBNull.Value ? (bool)false : (Convert.ToBoolean(item["ToBill"])),
                                                                     Per = item["Per"] == DBNull.Value ? (int)0 : (Convert.ToInt32(item["Per"])),
                                                                     WtUnitID = item["WtUnitID"] == DBNull.Value ? (int)0 : (Convert.ToInt32(item["WtUnitID"])),
                                                                     WtUnitName = item["WtUnitName"] == DBNull.Value ? null : (Convert.ToString(item["WtUnitName"])),
                                                                     Rate = item["Rate"] == DBNull.Value ? (decimal?)0 : (Convert.ToDecimal(item["Rate"])),
                                                                     WtVol = item["Wt_Vol_No"] == DBNull.Value ? null : (Convert.ToString(item["Wt_Vol_No"])),
                                                                     //unbill = item["UnBilled"] == DBNull.Value ? null : (Convert.ToString(item["UnBilled"])),
                                                                     /*ConversionRate = item["BaseCurrConversRate"] == DBNull.Value ? 0 : Convert.ToDecimal(item["BaseCurrConversRate"]),
																	 Amount = item["RateCurrValue"]==DBNull.Value ? 0 :  Convert.ToDecimal(item["RateCurrValue"]),*/
                                                                 }).ToList();
                        }
                        #endregion


                    }
                    #endregion Destination Info

                    #region RMCFees
                    destttable++;
                    if (CostDs.Tables.Count > destttable && CostDs.Tables[destttable] != null && CostDs.Tables[destttable].Rows.Count > 0)
                    {
                        CostObj.RMCFees = CostDs.Tables[destttable].AsEnumerable()
                            .Select(dataRow => new RmcFees
                            {
                                BAFlag = Convert.ToChar(dataRow["AddBeforeOrAfteSFR"]),
                                CostHeadId = Convert.ToString(dataRow["AddCostForBillId"]),
                                CostHeadName = Convert.ToString(dataRow["CostHeadName"]),
                                Amount = dataRow["AmtToAdd"] == DBNull.Value ? 0 : Convert.ToDecimal(dataRow["AmtToAdd"]),
                                Percent = dataRow["PercentToAdd"] == DBNull.Value ? 0 : Convert.ToDecimal(dataRow["PercentToAdd"])
                            }).ToList();
                    }
                    #endregion RMCFees

                    #region Job Document

                    //if (MoveID != null)
                    //{
                    //	DataSet DocDs = moveMangeDAL.GetEmailNDdocuments(Convert.ToInt32(MoveID));
                    //	JobDocUpload jobDoc = new JobDocUpload();

                    //	if (DocDs != null && DocDs.Tables.Count >= 1)
                    //	{

                    //		if (DocDs.Tables.Count > 1 && DocDs.Tables[1] != null && DocDs.Tables[1].Rows.Count > 0)
                    //		{
                    //			jobDoc.docLists = (from item in DocDs.Tables[1].AsEnumerable()
                    //							   select new JobDocument()
                    //							   {
                    //								   FileID = Convert.ToInt32(item["FileID"]),
                    //								   DocType = Convert.ToString(item["DocTypeName"]),
                    //								   DocName = Convert.ToString(item["DocName"]),
                    //								   DocDescription = Convert.ToString(item["Description"]),
                    //								   FileName = Convert.ToString(item["DocFileName"]),
                    //								   UploadBy = Convert.ToString(item["UploadBy"]),
                    //								   UploadById = Convert.ToInt32(item["CreatedBy"]),
                    //								   UploadDate = Convert.ToDateTime(item["CreatedDate"]),
                    //								   InvRefNo = Convert.ToString(item["InvRefNo"]),
                    //								   ///FilePath = CommonSubs.EncryptSf(Convert.ToString(item["DocFilePath"]))
                    //							   }).ToList();
                    //		}

                    //	}

                    //	CostObj.jobDocUpload = jobDoc;
                    //}

                    #endregion

                    #region FollowUpDetails
                    destttable++;
                    if (CostDs.Tables.Count > destttable && CostDs.Tables[destttable] != null && CostDs.Tables[destttable].Rows.Count > 0)
                    {
                        CostObj.FollowUpList = CostDs.Tables[destttable].AsEnumerable()
                            .Select(dataRow => new FollowUpDetails
                            {
                                FollowUpDate = dataRow["FollowUpDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataRow["FollowUpDate"]),
                                FollowUpRemark = Convert.ToString(dataRow["FollowupRemarks"]),
                                CreatedBy = Convert.ToString(dataRow["CreatedBy"]),
                                CreatedDate = dataRow["CreatedDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataRow["CreatedDate"])
                            }).ToList();
                    }
                    #endregion

                    #region CloseJobDetails
                    destttable++;
                    if (CostDs.Tables.Count > destttable && CostDs.Tables[destttable] != null && CostDs.Tables[destttable].Rows.Count > 0)
                    {
                        CostObj.CloseJobRemark = CostDs.Tables[destttable].Rows[0]["JobCloseRemark"] == DBNull.Value ? null : Convert.ToString(CostDs.Tables[destttable].Rows[0]["JobCloseRemark"]);
                        CostObj.CloseJobBy = CostDs.Tables[destttable].Rows[0]["JobCloseBy"] == DBNull.Value ? null : Convert.ToString(CostDs.Tables[destttable].Rows[0]["JobCloseBy"]);
                        CostObj.CloseJobDate = CostDs.Tables[destttable].Rows[0]["JobCloseDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(CostDs.Tables[destttable].Rows[0]["JobCloseDate"]);
                    }
                    #endregion

                    #region InsuranceDetails
                    destttable++;
                    CostObj.Insurance.IsSendForInsurance = false;
                    if (CostDs.Tables.Count > destttable && CostDs.Tables[destttable] != null && CostDs.Tables[destttable].Rows.Count > 0)
                    {
                        CostObj.Insurance.InsPackDate = CostDs.Tables[destttable].Rows[0]["PackDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(CostDs.Tables[destttable].Rows[0]["PackDate"]);
                        CostObj.Insurance.InsuranceValue = CostDs.Tables[destttable].Rows[0]["InsuranceValue"] == DBNull.Value ? 0 : Convert.ToDecimal(CostDs.Tables[destttable].Rows[0]["InsuranceValue"]);
                        CostObj.Insurance.PremiumRate = CostDs.Tables[destttable].Rows[0]["PremiumRate"] == DBNull.Value ? 0 : Convert.ToDecimal(CostDs.Tables[destttable].Rows[0]["PremiumRate"]);
                        CostObj.Insurance.IDVCarValue = CostDs.Tables[destttable].Rows[0]["IDV_value_Car"] == DBNull.Value ? 0 : Convert.ToDecimal(CostDs.Tables[destttable].Rows[0]["IDV_value_Car"]);
                        CostObj.Insurance.VehMakeModel = CostDs.Tables[destttable].Rows[0]["VehMakeModel"] == DBNull.Value ? null : Convert.ToString(CostDs.Tables[destttable].Rows[0]["VehMakeModel"]);
                        CostObj.Insurance.InsuranceCurrID = CostDs.Tables[destttable].Rows[0]["CurrID"] == DBNull.Value ? (int?)null : Convert.ToInt32(CostDs.Tables[destttable].Rows[0]["CurrID"]);
                        CostObj.Insurance.IsSendForInsurance = CostDs.Tables[destttable].Rows[0]["SendForInsurance"] == DBNull.Value ? false : Convert.ToBoolean(CostDs.Tables[destttable].Rows[0]["SendForInsurance"]);
                    }
                    #endregion

                    #region AdvanceCaution
                    destttable++;
                    if (CostDs.Tables.Count > destttable && CostDs.Tables[destttable] != null && CostDs.Tables[destttable].Rows.Count > 0)
                    {
                        CostObj.ShowAdvanceCaution = Convert.ToBoolean(CostDs.Tables[destttable].Rows[0]["ShowAdvanceCaution"]);
                    }
                    #endregion

                    #region TabList
                    destttable++;
                    if (CostDs.Tables.Count > destttable && CostDs.Tables[destttable] != null && CostDs.Tables[destttable].Rows.Count > 0)
                    {
                        //CostObj.ShowAdvanceCaution = Convert.ToBoolean(CostDs.Tables[destttable].Rows[0]["ShowAdvanceCaution"]);
                        for (int i = 0; i < CostDs.Tables[destttable].Columns.Count; i++)
                        {

                            TabList tab = new TabList();
                            tab.TabIndex = Convert.ToInt32(CostDs.Tables[destttable].Rows[0][i].ToString());
                            //if (tab.TabIndex !=0)
                            //{
                            //	
                            //}
                            CostObj.TabList.Add(tab);

                        }
                    }
                    #endregion

                    #region TabList
                    destttable++;
                    if (CostDs.Tables.Count > destttable && CostDs.Tables[destttable] != null && CostDs.Tables[destttable].Rows.Count > 0)
                    {
                        //CostObj.ShowAdvanceCaution = Convert.ToBoolean(CostDs.Tables[destttable].Rows[0]["ShowAdvanceCaution"]);
                        //for (int i = 0; i < CostDs.Tables[destttable].Columns.Count; i++)
                        {

                            //TranShipmentWtVol trnDet = new TranShipmentWtVol();
                            CostObj.FreightReport.TranShipmentWtVolList = CostDs.Tables[destttable].AsEnumerable()
                            .Select(dataRow => new TranShipmentWtVol
                            {
                                BLReleaseOn = dataRow["BLReleaseOn"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataRow["BLReleaseOn"]),
                                Bill_No = Convert.ToString(dataRow["BillNo"]),
                                SealNo = Convert.ToString(dataRow["SealNo"]),
                                ContainerNo = Convert.ToString(dataRow["ContainerNo"]),
                                NoOfPacks = dataRow["NoOfPacks"] == DBNull.Value ? (int)0 : (Convert.ToInt32(dataRow["NoOfPacks"])),
                                WtVol = dataRow["WtVol"] == DBNull.Value ? 0 : Convert.ToDecimal(dataRow["WtVol"]),
                                ContainerTypeId = dataRow["ContainerTypeID"] == DBNull.Value ? (int)0 : (Convert.ToInt32(dataRow["ContainerTypeID"])),
                                AirLineID = dataRow["AirLineID"] == DBNull.Value ? (int)0 : (Convert.ToInt32(dataRow["AirLineID"])),
                                CourierID = dataRow["CourierID"] == DBNull.Value ? (int)0 : (Convert.ToInt32(dataRow["CourierID"])),
                                Courier = Convert.ToString(dataRow["CourierName"]),
                                AirLine = Convert.ToString(dataRow["ShipLineName"]),
                                ContainerType = Convert.ToString(dataRow["ContainerType"]),
                                WtVolUnit = Convert.ToString(dataRow["WeightUnitName"]),
                                LCLorFCL = Convert.ToString(dataRow["FCLLcl"]),
                                WtVolUnitId = dataRow["WtUnitID"] == DBNull.Value ? (int?)null : (Convert.ToInt32(dataRow["WtUnitID"])),
                                VolUnit = Convert.ToString(dataRow["VolUnitName"]),
                                VolUnitId = dataRow["VolUnitID"] == DBNull.Value ? (int?)null : (Convert.ToInt32(dataRow["VolUnitID"])),
                                Vol = dataRow["VolVal"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(dataRow["VolVal"]),
                                Length = dataRow["Length"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(dataRow["Length"]),
                                Breadth = dataRow["Breadth"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(dataRow["Breadth"]),
                                Height = dataRow["Height"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(dataRow["Height"]),
                                Isactive = true,
                                MasterID = string.IsNullOrEmpty(Convert.ToString(dataRow["TransInvMasterID"])) ? 0 : Convert.ToInt64(dataRow["TransInvMasterID"]),
                                MoveID = string.IsNullOrEmpty(Convert.ToString(dataRow["MoveID"])) ? 0 : Convert.ToInt64(dataRow["MoveID"]),
                                JobNo = Convert.ToString(dataRow["JobID"]),
                                Shipper = Convert.ToString(dataRow["ShipperName"]),
                                GrossWt = dataRow["GrossWt"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(dataRow["GrossWt"]),
                                ACWTWt = dataRow["ACWT"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(dataRow["ACWT"]),
                            }).ToList();

                            if (CostObj.FreightReport.TranShipmentWtVolList.First().MasterID > 0)
                            {
                                CostObj.FreightReport.TransInvMasterID = CostObj.FreightReport.TranShipmentWtVolList.First().MasterID;
                            }

                            //if(CostObj.FreightReport.TranShipmentWtVolList!=null &&  CostObj.FreightReport.TranShipmentWtVolList.Count > 0)
                            //{
                            //    var BLobj = CostObj.FreightReport.TranShipmentWtVolList.OrderByDescending(t => t.BLReleaseOn).First(); // CostObj.FreightReport.TranShipmentWtVolList.FirstOrDefault();
                            //    CostObj.FreightReport.Bill_No = BLobj.Bill_No.Trim();
                            //    CostObj.FreightReport.BLReleaseOn = BLobj.BLReleaseOn;
                            //}

                            //if (tab.TabIndex !=0)
                            //{
                            //	CostObj.TabList.Add(tab);
                            //}

                        }
                    }
                    #endregion

                    #region Agent Evaluation

                    //if (MoveID != null)
                    //{
                    //	CostObj.vendorEvaluation = moveMangeDAL.GetVendorEvaluation(Convert.ToInt64(MoveID));
                    //}

                    #endregion

                    #region Transit Invoice & Job
                    destttable++;
                    if (CostDs.Tables.Count > destttable && CostDs.Tables[destttable] != null && CostDs.Tables[destttable].Rows.Count > 0)
                    {
                        CostObj.FreightReport.transhipInvoiceJobs = CostDs.Tables[destttable].AsEnumerable()
                            .Select(dataRow => new TranshipInvoiceJobs
                            {

                                MoveId = Convert.ToInt64(dataRow["MoveID"]),
                                JobNo = Convert.ToString(dataRow["JobID"]),
                                JobAmt = string.IsNullOrWhiteSpace(Convert.ToString(dataRow["InvoiceAmt"])) ? 0 : Convert.ToDecimal(dataRow["InvoiceAmt"]),
                                Remark = Convert.ToString(dataRow["Remark"]),
                            }).ToList();

                        //CostObj.FreightReport.TransitDistMoveIDList = CostObj.FreightReport.transhipInvoiceJobs.Select(m => m.MoveId).ToArray<Int64>();
                    }

                    destttable++;
                    if (CostDs.Tables.Count > destttable && CostDs.Tables[destttable] != null && CostDs.Tables[destttable].Rows.Count > 0)
                    {
                        CostObj.FreightReport.transhipInvoices = CostDs.Tables[destttable].AsEnumerable()
                            .Select(dataRow => new TranshipInvoice
                            {
                                InvoiceId = dataRow["TransInvDetID"] == DBNull.Value ? (Int64?)null : Convert.ToInt64(dataRow["TransInvDetID"]),
                                InvoiceTypeId = Convert.ToInt32(dataRow["InvTypeID"]),
                                InvoiceType = Convert.ToString(dataRow["InvTypeName"]),
                                InvoiceNo = Convert.ToString(dataRow["InvoiceNo"]),
                                InvoiceDate = Convert.ToDateTime(dataRow["InvoiceDate"]),
                                InvoiceAmt = Convert.ToDecimal(dataRow["InvoiceAmount"]),
                                CurrID = Convert.ToInt32(dataRow["CurrencyID"]),
                                Currancy = Convert.ToString(dataRow["CurrencyAbbrvation"]),
                                Remark = Convert.ToString(dataRow["Remark"]),
                                MasterID = Convert.ToInt64(dataRow["TransInvMasterID"]),
                                InvCredit = Convert.ToString(dataRow["InvCredit"]),
                                InvCreditName = Convert.ToString(dataRow["InvCreditName"]),
                                FirstInvID = dataRow["FirstInvID"] == DBNull.Value ? (Int64?)null : Convert.ToInt64(dataRow["FirstInvID"]),

                            }).ToList();

                        CostObj.FreightReport.TransInvMasterID = CostObj.FreightReport.transhipInvoices.First().MasterID;
                    }
                    #endregion

                    #region Forwrding Details
                    destttable++;
                    if (CostDs.Tables.Count > destttable && CostDs.Tables[destttable] != null && CostDs.Tables[destttable].Rows.Count > 0)
                    {
                        CostObj.ForwardingFlag = CostDs.Tables[destttable].Rows[0]["ButtonParam"] == DBNull.Value ? null : Convert.ToString(CostDs.Tables[destttable].Rows[0]["ButtonParam"]);

                        //CostObj.FreightReport.TransitDistMoveIDList = CostObj.FreightReport.transhipInvoiceJobs.Select(m => m.MoveId).ToArray<Int64>();
                    }
                    #endregion

                    #region GP Approval Details
                    destttable++;
                    if (CostDs.Tables.Count > destttable && CostDs.Tables[destttable] != null && CostDs.Tables[destttable].Rows.Count > 0)
                    {
                        CostObj.GPApprovalDisplayList = (from item in CostDs.Tables[destttable].AsEnumerable()
                                                         select new GPApprovalDisplayList
                                                         {
                                                             Remark = Convert.ToString(item["Remark"]),
                                                             Status = Convert.ToString(item["Status"]),
                                                             CreatedBy = Convert.ToString(item["CreatedBy"]),
                                                             CreatedDate = Convert.ToString(item["CreatedDate"]),
                                                             GPPercent = Convert.ToString(item["GPPercent"]),
                                                             RevAmount = Convert.ToString(item["GPAmount"]),
                                                         }).ToList();
                    }
                    #endregion

                    if (!CostObj.IsShowCloseJob)
                    {
                        CostObj.IsShowCloseJob = CostObj.DeliveryCostList.CostList.Sum(x => x.Balance) == 0;
                    }
                    if (!CostObj.IsInvPrepared)
                    {
                        CostObj.IsShowCloseJob = false;
                    }
                }

            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "MoveManageBL", "GetDetailById", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

            return CostObj;
        }

        public List<IEnumerable<SelectListItem>> GetAgentGridCombo(int RateCompID, int AgentID, int SurveyID, ref DataTable dt)
        {
            List<IEnumerable<SelectListItem>> list = new List<IEnumerable<SelectListItem>>();

            //dt = new DataTable();
            if (dt.Rows.Count <= 0)
            {
                dt = moveMangeDAL.GetAgentGridCombo(RateCompID, SurveyID);
                if (RateCompID == (int)RELOCBS.Common.CommonService.RateComp.Origin)
                {

                    list.Add((from item in (new DataView(dt)).ToTable(true, new string[] { "AgentID", "AgentName" }).AsEnumerable()
                              select new SelectListItem()
                              {
                                  Value = item["AgentID"].ToString(),
                                  Text = item["AgentName"].ToString()
                              }).AsQueryable().Distinct().ToList());
                    /*list.Add((from item in dt.AsEnumerable()
                              select new SelectListItem()
                              {
                                  Value = item["OrgPortID"].ToString(),
                                  Text = item["OrgPortName"].ToString()
                              }).ToList());*/
                }
                else if (RateCompID == (int)RELOCBS.Common.CommonService.RateComp.Freight)
                {
                    list.Add((from item in (new DataView(dt)).ToTable(true, new string[] { "AgentID", "AgentName" }).AsEnumerable()
                              select new SelectListItem()
                              {
                                  Value = item["AgentID"].ToString(),
                                  Text = item["AgentName"].ToString()
                              }).ToList().Distinct());
                    /* list.Add((from item in dt.AsEnumerable()
                               select new SelectListItem()
                               {
                                   Value = item["OrgPortID"].ToString(),
                                   Text = item["OrgPortName"].ToString()
                               }).ToList());
                     list.Add((from item in dt.AsEnumerable()
                               select new SelectListItem()
                               {
                                   Value = item["DestPortID"].ToString(),
                                   Text = item["DestPortName"].ToString()
                               }).ToList());*/
                }
                else if (RateCompID == (int)RELOCBS.Common.CommonService.RateComp.Destination)
                {
                    list.Add((from item in (new DataView(dt)).ToTable(true, new string[] { "AgentID", "AgentName" }).AsEnumerable()
                              select new SelectListItem()
                              {
                                  Value = item["AgentID"].ToString(),
                                  Text = item["AgentName"].ToString()
                              }).ToList().Distinct());
                    /*list.Add((from item in dt.AsEnumerable()
                              select new SelectListItem()
                              {
                                  Value = item["DestPortID"].ToString(),
                                  Text = item["DestPortName"].ToString()
                              }).ToList());*/

                }

            }
            else
            {
                if (RateCompID == (int)RELOCBS.Common.CommonService.RateComp.Origin)
                {
                    /*list.Add((from item in dt.AsEnumerable()
                              select new SelectListItem()
                              {
                                  Value = item["AgentID"].ToString(),
                                  Text = item["AgentName"].ToString()
                              }).ToList());*/
                    list.Add((from item in (new DataView(dt.Select("AgentID =" + AgentID).CopyToDataTable())).ToTable(true, new string[] { "OrgPortID", "OrgPortName" }).AsEnumerable()
                              select new SelectListItem()
                              {
                                  Value = item["OrgPortID"].ToString(),
                                  Text = item["OrgPortName"].ToString()
                              }).ToList().Distinct());
                }
                else if (RateCompID == (int)RELOCBS.Common.CommonService.RateComp.Freight)
                {
                    /*list.Add((from item in dt.AsEnumerable()
                              select new SelectListItem()
                              {
                                  Value = item["AgentID"].ToString(),
                                  Text = item["AgentName"].ToString()
                              }).ToList());*/
                    list.Add((from item in (new DataView(dt.Select("AgentID =" + AgentID).CopyToDataTable())).ToTable(true, new string[] { "OrgPortID", "OrgPortName" }).AsEnumerable()
                              select new SelectListItem()
                              {
                                  Value = item["OrgPortID"].ToString(),
                                  Text = item["OrgPortName"].ToString()
                              }).ToList().Distinct());
                    list.Add((from item in (new DataView(dt.Select("AgentID =" + AgentID).CopyToDataTable())).ToTable(true, new string[] { "DestPortID", "DestPortName" }).AsEnumerable()
                              select new SelectListItem()
                              {
                                  Value = item["DestPortID"].ToString(),
                                  Text = item["DestPortName"].ToString()
                              }).ToList().Distinct());
                }
                else if (RateCompID == (int)RELOCBS.Common.CommonService.RateComp.Destination)
                {
                    list.Add((from item in (new DataView(dt.Select("AgentID =" + AgentID).CopyToDataTable())).ToTable(true, new string[] { "DestPortID", "DestPortName" }).AsEnumerable()
                              select new SelectListItem()
                              {
                                  Value = item["DestPortID"].ToString(),
                                  Text = item["DestPortName"].ToString()
                              }).ToList().Distinct());
                    /*list.Add((from item in dt.AsEnumerable()
                              select new SelectListItem()
                              {
                                  Value = item["DestPortID"].ToString(),
                                  Text = item["DestPortName"].ToString()
                              }).ToList());*/

                }
            }

            return list;
        }

        public PackingCostList GetCost(int AgentId, int CityId, int RMCId, int GoodsDescId, DateTime JobDate, decimal ConversionRate, int CostHeadId)
        {
            DataTable dt = new DataTable();
            PackingCostList result = new PackingCostList();
            dt = moveMangeDAL.GetCost(AgentId, CityId, RMCId, GoodsDescId, JobDate, ConversionRate, CostHeadId);
            if (dt != null && dt.Rows.Count > 0)
            {
                result.CostValue = dt.Rows[0]["CostVal"] == DBNull.Value ? 0 : Convert.ToDecimal(dt.Rows[0]["CostVal"].ToString());
                result.RevenueValue = dt.Rows[0]["Revenueval"] == DBNull.Value ? 0 : Convert.ToDecimal(dt.Rows[0]["Revenueval"].ToString());
                result.RateCurrID = dt.Rows[0]["CurrencyID"] == DBNull.Value ? 0 : Convert.ToInt32(dt.Rows[0]["CurrencyID"].ToString());
                result.WtUnitID = dt.Rows[0]["WtUnitID"] == DBNull.Value ? 0 : Convert.ToInt32(dt.Rows[0]["WtUnitID"].ToString());
                //WtVol = list[0]["Wt_Vol_No"],
                result.Per = dt.Rows[0]["Per"] == DBNull.Value ? 0 : Convert.ToInt32(dt.Rows[0]["Per"].ToString());

            }
            return result;
        }

        public bool InsertDocument(MoveManageViewModel SaveData, int LoginID, out string result)
        {
            result = String.Empty;

            try
            {
                return moveMangeDAL.InsertDocument(SaveData, LoginID, out result);

            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(LoginID), "MoveManageBL", "InsertDocument", Properties.Resources.UnExpectedErrorAtBL, ex);
            }

        }

        public bool InsertGCCDocument(MoveManageViewModel SaveData, int LoginID, out string result)
        {
            result = String.Empty;

            try
            {
                return moveMangeDAL.InsertGCCDocument(SaveData, LoginID, out result);

            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(LoginID), "MoveManageBL", "InsertGCCDocument", Properties.Resources.UnExpectedErrorAtBL, ex);
            }

        }

        public JobDocument GetDownloadFile(int FileID)
        {
            JobDocument obj = new JobDocument();
            try
            {
                return moveMangeDAL.GetDownloadFile(FileID, UserSession.GetUserSession().LoginID);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "MoveManageBL", "GetDownloadFile", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

            return obj;
        }

        //public EmailDetails 
        public List<EmailList> GetEmailTransactionDetail(int ActivityID, int MailTransactionID)
        {
            DataTable dt = new DataTable();
            List<EmailList> list = new List<EmailList>();

            //dt = new DataTable();
            if (dt.Rows.Count <= 0)
            {
                dt = moveMangeDAL.GetEmailTransactionDetail(ActivityID, MailTransactionID);

                list = (from item in dt.AsEnumerable()
                        select new EmailList()
                        {
                            TransactionID = item["TransactionID"] == DBNull.Value ? 0 : Convert.ToInt32(item["TransactionID"]),
                            ActivityID = item["ActivityID"] == DBNull.Value ? 0 : Convert.ToInt32(item["ActivityID"]),
                            ActivityName = Convert.ToString(item["ActivityName"]),
                            SentOn = Convert.ToDateTime(item["SentOn"])
                        }).ToList();

            }

            return list;
        }

        public bool InsertFollowUpDetials(MoveManageViewModel SaveData, out string result)
        {
            result = String.Empty;
            int LoginID = UserSession.GetUserSession().LoginID;
            try
            {

                return moveMangeDAL.InsertFollowUpDetials(SaveData, LoginID, out result);

            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(LoginID), "MoveManageBL", "InsertPacking", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

        }

        public bool InsertCloseJobDetials(MoveManageViewModel SaveData, out string result)
        {
            result = String.Empty;
            int LoginID = UserSession.GetUserSession().LoginID;
            try
            {

                return moveMangeDAL.InsertCloseJobDetials(SaveData, LoginID, out result);

            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(LoginID), "MoveManageBL", "InsertCloseJobDetials", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

        }


        public bool InsertInsuranceBySD(MoveManageViewModel SaveData, out string result)
        {
            result = String.Empty;
            int LoginID = UserSession.GetUserSession().LoginID;
            try
            {
                return moveMangeDAL.InsertInsuranceBySD(SaveData, LoginID, out result);

            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(LoginID), "MoveManageBL", "InsertPacking", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

        }

        public bool CancelJob(MoveManageViewModel SaveData, out string result)
        {
            result = String.Empty;
            int LoginID = UserSession.GetUserSession().LoginID;
            try
            {
                return moveMangeDAL.CancelJob(SaveData, LoginID, out result);

            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(LoginID), "MoveManageBL", "CancelJob", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

        }

        public bool ApproveAdvanceCaution(MoveManageViewModel SaveData, out string result)
        {
            result = String.Empty;
            int LoginID = UserSession.GetUserSession().LoginID;
            try
            {
                return moveMangeDAL.ApproveAdvanceCaution(SaveData, LoginID, out result);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(LoginID), "MoveManageBL", "ApproveAdvanceCaution", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }
        }

        public bool DeleteDocument(int ID, out string result)
        {
            result = String.Empty;
            int LoginID = UserSession.GetUserSession().LoginID;

            try
            {
                return moveMangeDAL.DeleteDocument(ID, LoginID, out result);

            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(LoginID), "MoveManageBL", "InsertDocument", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

        }

        public JobDocUpload GetDocumentGrid(Int64 ID, string FromType, String DocFromType, int DocTypeID = -1, int DocNameID = -1, string DocDescription = "")
        {
            JobDocUpload jobDoc = new JobDocUpload();
            jobDoc.ID = ID;
            jobDoc.DocFromType = FromType;
            jobDoc.DocTypeID = DocTypeID;
            try
            {
                DataTable DocDs = moveMangeDAL.GetDocumentGrid(ID, DocFromType, DocTypeID, DocNameID, DocDescription);


                if (DocDs != null)
                {

                    jobDoc.docLists = (from item in DocDs.AsEnumerable()
                                       select new JobDocument()
                                       {
                                           FileID = Convert.ToInt32(item["FileID"]),
                                           DocType = Convert.ToString(item["DocTypeName"]),
                                           DocName = Convert.ToString(item["DocName"]),
                                           DocDescription = Convert.ToString(item["Description"]),
                                           FileName = Convert.ToString(item["DocFileName"]),
                                           UploadBy = Convert.ToString(item["UploadBy"]),
                                           UploadById = Convert.ToInt32(item["CreatedBy"]),
                                           UploadDate = Convert.ToDateTime(item["CreatedDate"]),
                                           PolicyNo = Convert.ToString(item["PolicyNo"]),
                                           InvRefNo = Convert.ToString(item["InvRefNo"]),
                                           ///FilePath = CommonSubs.EncryptSf(Convert.ToString(item["DocFilePath"]))
                                       }).ToList();


                }

                return jobDoc;
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "MoveManageBL", "GetDocumentGrid", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

        }

        public bool InsertVendorEvaluation(MoveManageViewModel SaveData, out string result)
        {
            result = String.Empty;
            int LoginID = UserSession.GetUserSession().LoginID;
            try
            {
                return moveMangeDAL.InsertVendorEvaluation(SaveData, LoginID, out result);

            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(LoginID), "MoveManageBL", "InsertVendorEvaluation", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

        }

        public JobLabel GetJobLable(Int64 MoveId)
        {
            int LoginID = UserSession.GetUserSession().LoginID;

            try
            {
                return moveMangeDAL.GetJobLable(MoveId, LoginID);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(LoginID), "MoveManageBL", "GetJobLable", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

        }

        public Tuple<Int64, Int64, Int64, bool> GetJobReportParams(Int64 MoveID, Int64 SurveyID, int ReportID)
        {
            int LoginID = UserSession.GetUserSession().LoginID;

            try
            {
                Int64 RateCompanyRateWtID = -1;
                Int64 RateCompRateBatchID = -1;
                bool IsLumSum = false;
                DataTable data = moveMangeDAL.GetJobReportParams(MoveID, SurveyID, ReportID, LoginID);
                if (data != null && data.Rows.Count > 0)
                {
                    DataRow item = data.Rows[0];

                    RateCompanyRateWtID = Convert.ToInt64(item["RateCompanyRateWtID"]);
                    RateCompRateBatchID = Convert.ToInt64(item["RateCompRateBatchID"]);
                    IsLumSum = Convert.ToBoolean(item["IsLumSum"]);
                }

                return new Tuple<Int64, Int64, Int64, bool>(RateCompanyRateWtID, RateCompRateBatchID, SurveyID, IsLumSum);


            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(LoginID), "MoveManageBL", "GetJobLable", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

        }

        public DataTable GetTrasshipmentVessel(Int64 MoveID)
        {
            int LoginID = UserSession.GetUserSession().LoginID;
            try
            {
                return moveMangeDAL.GetTrasshipmentVessel(MoveID, LoginID);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(LoginID), "MoveManageBL", "GetTrasshipmentVessel", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

        }

        public TranShipmentWtVol getGetJobWtDetail(Int64 MoveId)
        {
            int LoginID = UserSession.GetUserSession().LoginID;
            try
            {
                return moveMangeDAL.getGetJobWtDetail(MoveId, LoginID);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(LoginID), "MoveManageBL", "getGetJobWtDetail", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }


        }

        public EmailConfig GetMailFormat(Int64 ActivityID, Int64? MoveID = null, Int64? TransID = null)
        {
            int LoginID = UserSession.GetUserSession().LoginID;
            try
            {
                DataTable dt = moveMangeDAL.GetMailFormat(ActivityID, LoginID, MoveID, TransID);
                if (true)
                {

                }
                return new EmailConfig()
                {
                    EmailTo = Convert.ToString(dt.Rows[0]["EmailTo"]),
                    EmailBCC = Convert.ToString(dt.Rows[0]["EmailBCC"]),
                    EmailCC = Convert.ToString(dt.Rows[0]["EmailCC"]),
                    Subject = Convert.ToString(dt.Rows[0]["EmailSubject"]),
                    Body = Convert.ToString(dt.Rows[0]["Body"])
                };
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(LoginID), "MoveManageBL", "GetMailFormat", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }


        }

        public List<EmailActiviryHistory> GetMailActivityHistory(Int64? MoveId, Int64? ActivityID = null)
        {
            int LoginID = UserSession.GetUserSession().LoginID;
            try
            {
                DataTable dt = moveMangeDAL.GetMailActivityHistory(LoginID, MoveId, ActivityID);
                if (dt != null)
                {
                    return (from item in dt.AsEnumerable()
                            select new EmailActiviryHistory()
                            {
                                MailTransID = item["MailTransId"] == DBNull.Value ? (Int32?)null : Convert.ToInt32(item["MailTransId"]),
                                ActivityID = Convert.ToInt32(item["ActivityID"]),
                                ActivityName = Convert.ToString(item["ActivityName"]),
                                SentBy = Convert.ToString(item["SentBy"]),
                                SentDate = Convert.ToString(item["SentOn"])
                                ///FilePath = CommonSubs.EncryptSf(Convert.ToString(item["DocFilePath"]))
                            }).ToList();
                }
                else
                {
                    return new List<EmailActiviryHistory>();
                }
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(LoginID), "MoveManageBL", "GetMailActivityHistory", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }


        }

        public bool CheckCreditPrivateClient(Int64 MoveID, bool IsWIS = false)
        {
            int LoginID = UserSession.GetUserSession().LoginID;
            try
            {
                DataTable checkDt = moveMangeDAL.CheckCreditPrivateClient(MoveID, LoginID, IsWIS);

                if (checkDt != null && checkDt.Rows.Count > 0)
                {
                    return true;
                }

                return false;
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(LoginID), "MoveManageBL", "CheckCreditPrivateClient", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

        }

        public bool InsertACODetails(MoveManageViewModel SaveData, out string result)
        {
            result = String.Empty;
            int LoginID = UserSession.GetUserSession().LoginID;
            try
            {
                return moveMangeDAL.InsertACODetails(SaveData, LoginID, out result);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(LoginID), "MoveManageBL", "InsertACODetails", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }
        }

        public ACODetails GetACODetails(Int64 MoveID)
        {
            ACODetails result = new ACODetails();
            int LoginID = UserSession.GetUserSession().LoginID;
            try
            {
                DataTable dt = new DataTable();
                dt = moveMangeDAL.GetACODetails(LoginID, MoveID);
                if (dt != null && dt.Rows.Count > 0)
                {
                    result.ACODetailsId = dt.Rows[0]["ACODetailsId"] == DBNull.Value ? (Int64?)null : Convert.ToInt64(dt.Rows[0]["ACODetailsId"]);
                    result.JobStatusSDId = dt.Rows[0]["JobStatusSDId"] == DBNull.Value ? (Int32?)null : Convert.ToInt32(dt.Rows[0]["JobStatusSDId"]);
                    result.BillingStatusId = dt.Rows[0]["BillingStatusId"] == DBNull.Value ? (Int32?)null : Convert.ToInt32(dt.Rows[0]["BillingStatusId"]);
                    result.Remarks = Convert.ToString(dt.Rows[0]["Remarks"]);
                }
                return result;
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(LoginID), "MoveManageBL", "InsertPacking", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }
        }

        public bool InsertOFSDoc(MoveManageViewModel model, Int64? MoveID, int LoginID, out string result)
        {
            result = String.Empty;

            try
            {
                ComboBL combo = new ComboBL();
                List<System.Web.HttpPostedFileBase> files = new List<System.Web.HttpPostedFileBase>();
                files.Add(model.MoveJob.OFSDocument);
                model.jobDocUpload.file = files.ToArray();
                model.jobDocUpload.ID = Convert.ToInt64(MoveID);
                model.jobDocUpload.DocTypeID = Convert.ToInt32(combo.GetJobDocTypelDropdown(DocFromType: "MoveMan").FirstOrDefault(s => s.Text.Equals("Origin", StringComparison.OrdinalIgnoreCase)).Value);
                model.jobDocUpload.DocNameID = Convert.ToInt32(combo.GetJobDocNamelDropdown(DocTypeID: model.jobDocUpload.DocTypeID).FirstOrDefault(s => s.Text.Equals("OFS", StringComparison.OrdinalIgnoreCase)).Value);
                model.jobDocUpload.DocDescription = "Job OFS Document";
                model.jobDocUpload.DocFromType = "MoveMan";
                model.jobDocUpload.DocNameText = "Job OFS Document";
                model.MoveID = MoveID;
                return moveMangeDAL.InsertDocument(model, LoginID, out result);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(LoginID), "MoveManageBL", "InsertOFSDoc", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }
        }

        public bool InsertRequestDocsData(MoveManageViewModel ViewData, int LoginID, out string result)
        {
            result = string.Empty;
            try
            {
                return moveMangeDAL.InsertRequestDocsData(ViewData, LoginID, out result);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(LoginID), "MoveManageBL", "InsertRequestDocsData", Properties.Resources.UnExpectedErrorAtBL, ex);
            }
        }

        public MoveManageViewModel GetRequestDocsDetails(Int64 MoveID, Int64 RequestDocsGroupID)
        {
            MoveManageViewModel MoveManageObj = new MoveManageViewModel();
            try
            {
                DataSet ds = moveMangeDAL.GetRequestDocsDetails(MoveID, RequestDocsGroupID);

                if (ds != null && ds.Tables.Count > 0)
                {

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        MoveManageObj.JobNo = ds.Tables[0].Rows[0]["JobID"] == DBNull.Value ? null : Convert.ToString(ds.Tables[0].Rows[0]["JobID"]);
                    }

                    MoveManageObj.RequestDocsUploadList = (from item in ds.Tables[0].AsEnumerable()
                                                           select new JobDocUpload()
                                                           {
                                                               JobDocTypeId = item["JobDocTypeId"] == DBNull.Value ? 0 : Convert.ToInt32(item["JobDocTypeId"]),
                                                               DocTypeID = item["DocTypeID"] == DBNull.Value ? 0 : Convert.ToInt32(item["DocTypeID"]),
                                                               DocTypeText = item["DocTypeName"] == DBNull.Value ? null : Convert.ToString(item["DocTypeName"]),
                                                               DocNameID = item["DocNameID"] == DBNull.Value ? 0 : Convert.ToInt32(item["DocNameID"]),
                                                               DocNameText = item["DocName"] == DBNull.Value ? null : Convert.ToString(item["DocName"]),
                                                               DocDescription = item["Description"] == DBNull.Value ? null : Convert.ToString(item["Description"]),
                                                               Remarks = item["Remarks"] == DBNull.Value ? null : Convert.ToString(item["Remarks"]),
                                                               FileName = item["DocFileName"] == DBNull.Value ? null : Convert.ToString(item["DocFileName"]),
                                                           }).ToList();
                }
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(null, "MoveManageBL", "GetRequestDocsDetails", Properties.Resources.UnExpectedErrorAtBL, ex);
            }
            return MoveManageObj;
        }

        public ShipperFeedback GetShipperFeedbackTemplate(int LoginID, Int64? MoveID, int? SFTemplateID, Int64? ShipperFeedbackID, bool IsLoggedInUser)
        {
            ShipperFeedback ShipperFeedback = new ShipperFeedback();
            try
            {
                DataSet SFQuestionsDs = moveMangeDAL.GetShipperFeedbackTemplate(LoginID, MoveID, SFTemplateID, ShipperFeedbackID, IsLoggedInUser);

                if (SFQuestionsDs != null)
                {
                    if (SFQuestionsDs.Tables.Count > 0 && SFQuestionsDs.Tables[0].Rows.Count > 0)
                    {
                        DataRow row = SFQuestionsDs.Tables[0].Rows[0];

                        ShipperFeedback.SFTemplateID = row["SFTemplateID"] == DBNull.Value ? (int?)null : Convert.ToInt32(row["SFTemplateID"]);
                        ShipperFeedback.TemplateName = row["TemplateName"] == DBNull.Value ? null : Convert.ToString(row["TemplateName"]);
                        ShipperFeedback.Description = row["Description"] == DBNull.Value ? null : Convert.ToString(row["Description"]);
                        ShipperFeedback.ShipperFeedbackID = row["ShipperFeedbackID"] == DBNull.Value ? (Int64?)null : Convert.ToInt64(row["ShipperFeedbackID"]);
                        ShipperFeedback.MoveID = row["MoveID"] == DBNull.Value ? (Int64?)null : Convert.ToInt64(row["MoveID"]);
                        ShipperFeedback.JobNo = row["JobID"] == DBNull.Value ? null : Convert.ToString(row["JobID"]);
                        ShipperFeedback.EmailTo = row["EmailTo"] == DBNull.Value ? null : Convert.ToString(row["EmailTo"]);
                        ShipperFeedback.FeedbackLinkSentBy = row["FeedbackLinkSentBy"] == DBNull.Value ? null : Convert.ToString(row["FeedbackLinkSentBy"]);
                        ShipperFeedback.FeedbackLinkSentDate = row["FeedbackLinkSentDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(row["FeedbackLinkSentDate"]);
                        ShipperFeedback.IsFeedbackSubmitted = row["IsFeedbackSubmitted"] == DBNull.Value ? false : Convert.ToBoolean(row["IsFeedbackSubmitted"]);
                        ShipperFeedback.FeedbackSubmittedDate = row["FeedbackSubmittedDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(row["FeedbackSubmittedDate"]);
                    }
                    if (SFQuestionsDs.Tables.Count > 2 && SFQuestionsDs.Tables[1].Rows.Count > 0)
                    {
                        ShipperFeedback.SFQuestions = (from rw in SFQuestionsDs.Tables[1].AsEnumerable()
                                                       select new SFQuestion()
                                                       {
                                                           SFQuestionID = rw["SFQuestionID"] == DBNull.Value ? (int?)null : Convert.ToInt32(rw["SFQuestionID"]),
                                                           QuestionText = rw["QuestionText"] == DBNull.Value ? null : Convert.ToString(rw["QuestionText"]),
                                                           AnswerType = rw["AnswerType"] == DBNull.Value ? null : Convert.ToString(rw["AnswerType"]),
                                                           SrNo = rw["SrNo"] == DBNull.Value ? (int?)null : Convert.ToInt32(rw["SrNo"]),
                                                           IsQuestionChecked = rw["IsQuestionChecked"] == DBNull.Value ? false : Convert.ToBoolean(rw["IsQuestionChecked"]),
                                                           SFAnswerOptionID = rw["SFAnswerOptionID"] == DBNull.Value ? (int?)null : Convert.ToInt32(rw["SFAnswerOptionID"]),
                                                           AnswerText = rw["AnswerText"] == DBNull.Value ? null : Convert.ToString(rw["AnswerText"]),
                                                           SFAnswerOption = GetSFAnswerOption(Convert.ToInt32(rw["SFQuestionID"]), SFQuestionsDs.Tables[2])
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
                throw new BussinessLogicException(Convert.ToString(LoginID), "UserFeedbackBL", "GetFeedbackQuestions", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }
            return ShipperFeedback;
        }

        public bool AddEditShipperFeedback(MoveManageViewModel ViewData, int LoginID, int CompanyID, bool IsSubmitFeedback, string Url, out string result)
        {
            result = string.Empty;
            try
            {
                return moveMangeDAL.AddEditShipperFeedback(ViewData, LoginID, CompanyID, IsSubmitFeedback, Url, out result);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(LoginID), "MoveManageBL", "AddEditShipperFeedback", Properties.Resources.UnExpectedErrorAtBL, ex);
            }
        }

        private List<SFAnswerOption> GetSFAnswerOption(int SFQuestionID, DataTable SFAnswerOptionDt)
        {
            List<SFAnswerOption> SFAnswerOption = new List<SFAnswerOption>();
            if (SFAnswerOptionDt != null && SFAnswerOptionDt.Rows.Count > 0)
            {
                SFAnswerOption = (from rw in SFAnswerOptionDt.AsEnumerable().Where(r => r.Field<Int32>("SFQuestionID") == SFQuestionID).AsEnumerable()
                                  select new SFAnswerOption()
                                  {
                                      SFAnswerOptionID = rw["SFAnswerOptionID"] == DBNull.Value ? (int?)null : Convert.ToInt32(rw["SFAnswerOptionID"]),
                                      AnswerText = rw["AnswerText"] == DBNull.Value ? null : Convert.ToString(rw["AnswerText"]),
                                      ImageUrl = rw["ImageUrl"] == DBNull.Value ? null : Convert.ToString(rw["ImageUrl"]),
                                      SrNo = rw["SrNo"] == DBNull.Value ? (int?)null : Convert.ToInt32(rw["SrNo"]),
                                      IsAnswerChecked = rw["IsAnswerChecked"] == DBNull.Value ? false : Convert.ToBoolean(rw["IsAnswerChecked"]),
                                      Value = rw["SFAnswerOptionID"] == DBNull.Value ? null : Convert.ToString(rw["SFAnswerOptionID"]),
                                      Text = rw["AnswerText"] == DBNull.Value ? null : Convert.ToString(rw["AnswerText"]),
                                  }).ToList();
            }
            return SFAnswerOption;
        }

        public bool InsertAgentInvDocument(AgentInvoice SaveData, int LoginID, out string result)
        {
            result = String.Empty;

            try
            {
                return moveMangeDAL.InsertAgentInvDocument(SaveData, LoginID, out result);

            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(LoginID), "MoveManageBL", "InsertGCCDocument", Properties.Resources.UnExpectedErrorAtBL, ex);
            }

        }

        public AgentInvoice GetAgentInvDocumentGrid(Int64 ID, string FromType, String DocFromType, int DocTypeID = -1, int DocNameID = -1, string DocDescription = "")
        {
            AgentInvoice jobDoc = new AgentInvoice();
            jobDoc.ID = ID;
            jobDoc.DocFromType = FromType;
            jobDoc.DocTypeID = DocTypeID;
            jobDoc.DocNameID = DocNameID;
            jobDoc.DocDescription = DocDescription;
            try
            {
                DataTable DocDs = moveMangeDAL.GetAgentInvDocumentGrid(ID, DocFromType, DocTypeID, DocNameID, DocDescription);
                if (DocDs != null)
                {
                    jobDoc.docLists = (from item in DocDs.AsEnumerable()
                                       select new JobDocument()
                                       {
                                           FileID = Convert.ToInt32(item["FileID"]),
                                           DocType = Convert.ToString(item["DocTypeName"]),
                                           DocName = Convert.ToString(item["DocName"]),
                                           DocDescription = Convert.ToString(item["Description"]),
                                           FileName = Convert.ToString(item["DocFileName"]),
                                           UploadBy = Convert.ToString(item["UploadBy"]),
                                           UploadById = Convert.ToInt32(item["CreatedBy"]),
                                           UploadDate = Convert.ToDateTime(item["CreatedDate"]),
                                           PolicyNo = Convert.ToString(item["PolicyNo"]),
                                           InvRefNo = Convert.ToString(item["InvRefNo"]),
                                           AgentName = Convert.ToString(item["AgentName"]),
                                           Amount = item["Amount"] == DBNull.Value ? 0 : Convert.ToDecimal(item["Amount"]),
                                           ///FilePath = CommonSubs.EncryptSf(Convert.ToString(item["DocFilePath"]))
                                       }).ToList();


                }
                return jobDoc;
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "MoveManageBL", "GetAgentInvDocumentGrid", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

        }

        public bool InsertGPApproval(GPApproval SaveData, Int32 LoginID, out string result)
        {
            result = String.Empty;

            try
            {
                return moveMangeDAL.InsertGPApproval(SaveData, LoginID, out result);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(LoginID), "MoveManageBL", "InsertGPApproval", Properties.Resources.UnExpectedErrorAtBL, ex);
            }

        }

        public bool InsertGPApproval(MoveManageViewModel SaveData, Int32 LoginID, string stage, out string result)
        {
            result = String.Empty;

            try
            {
                return moveMangeDAL.InsertGPApproval(SaveData, LoginID, stage, out result);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(LoginID), "MoveManageBL", "InsertGPApproval", Properties.Resources.UnExpectedErrorAtBL, ex);
            }

        }

        public Tuple<decimal, decimal, int> GetGPAmount(decimal RevAmt, decimal GPPercent, int MoveID, string BaseCurr)
        {
            DataTable DocDs = moveMangeDAL.GetGPAmount(RevAmt, GPPercent, MoveID, BaseCurr);
            decimal GPAmount = 0; decimal GPDefPercent = 0; int GPMasterID = 0;
            if (DocDs != null && DocDs.Rows.Count > 0)
            {
                GPAmount = DocDs.Rows[0]["GPPercent"] == DBNull.Value ? 0 : Convert.ToDecimal(DocDs.Rows[0]["GPAmount"]);
                GPDefPercent = DocDs.Rows[0]["GPPercent"] == DBNull.Value ? 0 : Convert.ToDecimal(DocDs.Rows[0]["GPPercent"]);
                GPMasterID = DocDs.Rows[0]["GPMasterID"] == DBNull.Value ? 0 : Convert.ToInt32(DocDs.Rows[0]["GPMasterID"]);
            }
            return new Tuple<decimal, decimal, int>(GPAmount, GPDefPercent, GPMasterID);
        }

        public Tuple<decimal, decimal, bool, string> GetPrevGPAmount(int MoveID)
        {
            DataTable DocDs = moveMangeDAL.GetPrevGPAmount(MoveID);
            decimal GPAmount = 0; decimal GPDefPercent = 0; bool IsApproved = false; string Basecurr = null;
            if (DocDs != null && DocDs.Rows.Count > 0)
            {
                GPAmount = DocDs.Rows[0]["GPAmount"] == DBNull.Value ? 0 : Convert.ToDecimal(DocDs.Rows[0]["GPAmount"]);
                GPDefPercent = DocDs.Rows[0]["GPPercent"] == DBNull.Value ? 0 : Convert.ToDecimal(DocDs.Rows[0]["GPPercent"]);
                IsApproved = DocDs.Rows[0]["IsApprove"] == DBNull.Value ? false : Convert.ToBoolean(DocDs.Rows[0]["IsApprove"]);
                Basecurr = DocDs.Rows[0]["Basecurr"] == DBNull.Value ? null : Convert.ToString(DocDs.Rows[0]["Basecurr"]);
            }
            return new Tuple<decimal, decimal, bool, string>(GPAmount, GPDefPercent, IsApproved, Basecurr);
        }

        public bool GetGDPRNationality(string Nationality)
        {
            DataTable DocDs = moveMangeDAL.GetGDPRNationality(Nationality);
            bool ISGDPRNationality = false;
            if (DocDs != null && DocDs.Rows.Count > 0)
            {
                ISGDPRNationality = DocDs.Rows[0]["GDPRNationality"] == DBNull.Value ? false : Convert.ToBoolean(DocDs.Rows[0]["GDPRNationality"]);
            }
            return ISGDPRNationality;
        }

        public bool UnlockSTGDate(MoveManageViewModel SaveData, out string result)
        {
            result = String.Empty;
            int LoginID = UserSession.GetUserSession().LoginID;
            try
            {
                return moveMangeDAL.UnlockSTGDate(SaveData, LoginID, out result);

            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(LoginID), "MoveManageBL", "UnlockSTGDate", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

        }
    }
}