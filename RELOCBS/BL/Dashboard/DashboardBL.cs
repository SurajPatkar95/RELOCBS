using RELOCBS.Common.ExceptionHandling;
using RELOCBS.DAL.Dashboard;
using RELOCBS.Entities;
using RELOCBS.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace RELOCBS.BL.Dashboard
{
    public class DashboardBL
    {

        private DashboardDAL _DAL;

        public DashboardDAL DAL
        {

            get
            {
                if (this._DAL == null)
                    this._DAL = new DashboardDAL();
                return this._DAL;
            }
        }

        public DataSet GetSurveyDashboard(ScheduleSurveyDashboard model)
        {
            try
            {
                bool IsRmcBuss = UserSession.GetUserSession().BussinessLine != "NON RMC-BUSINESS";
                return DAL.GetSurveyDashboard(UserSession.GetUserSession().LoginID, UserSession.GetUserSession().CompanyID, IsRmcBuss, model);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "DashboardBL", "GetSurveyDashboard", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }
        }


        public IEnumerable<SurveyorEvent> GetSurveyorEvents(int id,DateTime fromDate)
        {
            try
            {
                return DAL.GetSurveyorEvents(UserSession.GetUserSession().LoginID, UserSession.GetUserSession().CompanyID, id, fromDate);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "DashboardBL", "GetSurveyDashboard", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }
        }

        public DataSet GetCrewUtilization(CrewUtilizationDashboard model)
        {
            try
            {
                bool IsRmcBuss = UserSession.GetUserSession().BussinessLine != "NON RMC-BUSINESS";
                return DAL.GetCrewUtilization(UserSession.GetUserSession().LoginID, UserSession.GetUserSession().CompanyID, IsRmcBuss, model);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "DashboardBL", "GetCrewUtilization", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }
        }

        public Entities.Dashboard GetDataPointList(int id = 10063)
        {
            try
            {
                DataSet dt = DAL.GetDataPointList(id);
                Entities.Dashboard Obj = new Entities.Dashboard();
                Obj.dataPoint = (from item in dt.Tables[0].AsEnumerable()
                                 select new Entities.DataPoint
                                 {
                                     x = Convert.ToString(item["JobStatusID"]) + "-" +Convert.ToString(item["JobStagename"]),
                                     y = item["NoOfJob"] == DBNull.Value ? 0 : Convert.ToInt32(item["NoOfJob"]),
                                     indexLabel = item["JobStatusID"] == DBNull.Value ? 0 : Convert.ToInt32(item["JobStatusID"])
                                 }).ToList();

                Obj.dataPoint2 = (from item in dt.Tables[1].AsEnumerable()
                                     select new Entities.DataPoint
                                     {
                                         x = Convert.ToString(item["RecID"]) + "-" + Convert.ToString(item["DateInfo"]),
                                         y = item["JobNos"] == DBNull.Value ? 0 : Convert.ToInt32(item["JobNos"]),
                                         indexLabel = item["RecID"] == DBNull.Value ? 0 : Convert.ToInt32(item["RecID"])
                                     }).ToList();
                
                Obj.dataPoint3 = (from item in dt.Tables[2].AsEnumerable()
                                 select new Entities.DataPoint
                                 {
                                     x = Convert.ToString(item["ID"]) + "-" + Convert.ToString(item["InvMonth"]),
                                     y = item["invVal"] == DBNull.Value ? 0 : Convert.ToInt32(item["invVal"]),
                                     indexLabel = item["ID"] == DBNull.Value ? 0 : Convert.ToInt32(item["ID"])
                                 }).ToList();

                Obj.dataPoint_Pie = (from item in dt.Tables[3].AsEnumerable()
                                     select new Entities.DataPoint
                                     {
                                         x = Convert.ToString(item["Text"]) + "-" + Convert.ToString(item["Text"]),
                                         y = item["Value"] == DBNull.Value ? 0 : Convert.ToInt32(item["Value"])
                                         //StatusID = item["JobStatusID"] == DBNull.Value ? 0 : Convert.ToInt32(item["JobStatusID"])
                                     }).ToList();

                return Obj;
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "DashboardBL", "GetSurveyDashboard", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }
        }

        public List<JobDetail> GetFollowUpDetails(string label, string UserID,string flag)
        {
            try
            {
                //return 1;
                DataSet dt = DAL.GetFollowUpDetails(UserID, label, flag);
                Entities.Dashboard Obj = new Entities.Dashboard();
                Obj.JobDetail = (from item in dt.Tables[0].AsEnumerable()
                                 select new Entities.JobDetail
                                 {
                                     MoveID = Convert.ToInt32(item["MoveID"]),
                                     ServiceLine = Convert.ToString(item["Service Line"]),
                                     JobNo = Convert.ToString(item["Job No"]),
                                     Controller = Convert.ToString(item["Controller"]),
                                     JobDate = Convert.ToString(item["Job Date"]),
                                     CurrStage = Convert.ToString(item["Current Stage"]),
                                     ShipperName = Convert.ToString(item["Shipper Name"]),
                                     Mode = Convert.ToString(item["Mode"]),
                                     Client = Convert.ToString(item["Client"]),
                                     Origin = Convert.ToString(item["Origin"]),
                                     OrgCountry = Convert.ToString(item["Origin_Country"]),
                                     Destination = Convert.ToString(item["Destination"]),
                                     DestCountry = Convert.ToString(item["Destination_Country"]),
                                     SD = Convert.ToString(item["SD"]),
                                     FollowupDate = Convert.ToString(item["Followup Dt"]),
                                     JobStatusID = (int?)Convert.ToInt32(label.Split('-')[0]),
                                     UserID = (int?)Convert.ToInt32(UserID),
                                     //NextFollowupDate = Convert.ToString(item["NextFollowupDate"]),
                                     //NextFollowupReason = Convert.ToString(item["NextFollowupReason"]),
                                     OrderNo = Convert.ToString(item["Order No"]),
                                     //Reason = Convert.ToString(item["Drop Down Remarks"])
                                     //FollowUpList = new FollowUpDetails
                                     //{
                                     //    FollowUpRemark = Convert.ToString(item["LastFollowUpRemark"]),
                                     //    FollowUpDate = item["LastFollowUpDate"] == DBNull.Value ? (DateTime?)null: Convert.ToDateTime(item["LastFollowUpDate"]),
                                     //    CreatedBy = Convert.ToString(item["LastCreatedBy"]),
                                     //}
                                 }).ToList();

                foreach (var item in Obj.JobDetail)
                {
                    
                    if (dt.Tables[1].Select("MoveID=" + item.MoveID).Count() > 0)
                    {
                        item.FollowUpList = (from item1 in dt.Tables[1].Select("MoveID=" + item.MoveID).AsEnumerable()
                                             select new Entities.FollowUpDetails
                                             {
                                                 FollowUpDate = (DateTime?)Convert.ToDateTime(item1["FollowUpDate"]),
                                                 FollowUpRemark = Convert.ToString(item1["FollowUpRemarks"]),
                                                 CreatedBy = Convert.ToString(item1["CreatedBy"]),
                                                 CreatedDate = (DateTime?)Convert.ToDateTime(item1["CreatedDate"])

                                             }).ToList();
                    }
                }

                //return Obj.dataPoint;
                return Obj.JobDetail;
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "DashboardBL", "GetCrewUtilization", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }
        }
        
        public bool SaveFollowup(Entities.JobDetail model, out string result)
        {
            try
            {
                //var ModeXML = model.ModeList != null ? new XElement("Modes", model.ModeList.Select(x => new XElement("ModeIDs", new XElement("ModeID", x)))) : new XElement("Modes");

                //string result = string.Empty;
                int loginid = UserSession.GetUserSession().LoginID;
                return DAL.SaveFollowup(model, loginid, out result);
                //return true;
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "DashboardBL", "SaveFollowup", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }
        }
    }
}