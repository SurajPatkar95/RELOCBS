using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RELOCBS.DAL.Common;
using RELOCBS.Entities;
using RELOCBS.Utility;
using RELOCBS.Common.ExceptionHandling;
using RELOCBS.DAL.Survey;
using System.Data;
using System.Linq.Dynamic;
using System.Dynamic;

namespace RELOCBS.BL.Survey
{
    public class SurveyBL
    {

        private SurveyDAL _surveyDAL;

        public SurveyDAL surveyDAL
        {

            get
            {
                if (this._surveyDAL == null)
                    this._surveyDAL = new SurveyDAL();
                return this._surveyDAL;
            }
        }

        public bool Insert(SurveyViewModel survey, out string result, DateTime? SchSurveydate = null)
        {
            try
            {
                return surveyDAL.Insert(survey, out result, SchSurveydate);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "surveyBL", "Insert", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

        }

        public bool Completed(SurveyViewModel survey, out string result)
        {
            try
            {
                return surveyDAL.Completed(survey, out result);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "surveyBL", "Insert", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

        }

        public bool Update(SurveyViewModel survey, out string result)
        {
            result = string.Empty;
            try
            {
                return surveyDAL.Update(survey, out result);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "surveyBL", "Update", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }
        }

        public bool DeleteById(int id, out string result)
        {
            result = string.Empty;
            try
            {
                return surveyDAL.DeleteById(id, out result);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "surveyBL", "DeleteById", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }
        }

        public SurveyViewModel GetSurveyCostHeads(int SurveyID)
        {
            SurveyViewModel surveyList;

            try
            {
                surveyList = surveyDAL.GetSurveyCostHeads(SurveyID, UserSession.GetUserSession().LoginID);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "surveyBL", "DeleteById", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

            return surveyList;

        }

        public SurveyViewModel GetDetailById(int? EnqDetailID, int? surveyID = -1)
        {
            SurveyViewModel surveyObj = new SurveyViewModel();
            try
            {

                DataSet surveyDt = surveyDAL.GetDetailById(UserSession.GetUserSession().LoginID, EnqDetailID, surveyID);
                if (surveyDt != null && surveyDt.Tables.Count > 0)
                {

                    surveyObj = (from rw in surveyDt.Tables[0].AsEnumerable()
                                 select new SurveyViewModel()
                                 {
                                     SurveyId = rw["surveyID"] == DBNull.Value ? (Int64?)null : Convert.ToInt64(rw["surveyID"]),
                                     EnqID = rw["EnqID"] == DBNull.Value ? (Int64?)null : Convert.ToInt64(rw["EnqID"]),
                                     EnqNo = rw["EnqNo"] == DBNull.Value ? null : Convert.ToString(rw["EnqNo"]),
                                     EnqDetSequenceID = rw["EnqDetSequenceID"] == DBNull.Value ? (Int32?)null : Convert.ToInt32(rw["EnqDetSequenceID"]),
                                     AccountID = rw["AgentID"] == DBNull.Value ? (Int32?)null : Convert.ToInt32(rw["AgentID"]),
                                     AgentID = rw["AgentID"] == DBNull.Value ? (Int32?)null : Convert.ToInt32(rw["AgentID"]),
                                     AccountMngrID = rw["ChangeAcctMgrID"] == DBNull.Value ? (Int32?)null : Convert.ToInt32(rw["ChangeAcctMgrID"]),
                                     //AccountMngrName = Convert.ToInt32(rw["ChangeAcctMgrID"]),
                                     AvailableRate = rw["AvailableRate"] == DBNull.Value ? (Int64?)null : Convert.ToInt64(rw["AvailableRate"]),
                                     //CompetitorId = rw["CompetitorId"] == DBNull.Value ? (Int32?)null : Convert.ToInt32(rw["CompetitorId"]),
                                     CompititorName = rw["Competitortext"] == DBNull.Value ? null : Convert.ToString(rw["Competitortext"]),
                                     DeliveryDate = rw["ReqDeliveryDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(rw["ReqDeliveryDate"]),
                                     //DepartureDate = Convert.ToDateTime(rw["DepartureDate"]),
                                     EnqDate = rw["EnqDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(rw["EnqDate"]),
                                     EnqRecievedDate = rw["EnqReceivedDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(rw["EnqReceivedDate"]),
                                     EnqRemarks = Convert.ToString(rw["EnqRemarks"]),
                                     //EnqStatusDate = Convert.ToDateTime(rw["EnqStatusDate"]),
                                     //EnqStatusID = Convert.ToInt32(rw["EnqStatusID"]),
                                     //EnqStatusName="",
                                     InsCurrencyID = rw["InsCurrrencyID"] == DBNull.Value ? (Int32?)null : Convert.ToInt32(rw["InsCurrrencyID"]),
                                     InsuredAmount = rw["ApproxInsuAmt"] == DBNull.Value ? (Double?)null : Convert.ToDouble(rw["ApproxInsuAmt"]),
                                     InsuredById = rw["InsurBy"] == DBNull.Value ? (Int32?)null : Convert.ToInt32(rw["InsurBy"]),
                                     IsCompition = rw["Competetion"] == DBNull.Value ? (bool?)null : Convert.ToBoolean(rw["Competetion"]),
                                     //CompititorID=""
                                     LoadDate = rw["LoadDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(rw["LoadDate"]),
                                     MonitoryEntCurrID = rw["MonitoryEntCurrID"] == DBNull.Value ? (Int32?)null : Convert.ToInt32(rw["MonitoryEntCurrID"]),
                                     MonitoryEntitlement = rw["MonitoryEntitlement"] == DBNull.Value ? (Int64?)null : Convert.ToInt64(rw["MonitoryEntitlement"]),
                                     NoOfDays = rw["NoOfDays"] == DBNull.Value ? (Int32?)null : Convert.ToInt32(rw["NoOfDays"]),
                                     PackDate = rw["PackDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(rw["PackDate"]),
                                     QuotationSubmissionDate = rw["QuoteSubmissionDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(rw["QuoteSubmissionDate"]),
                                     RateAvailable = rw["RateAvailable"] == DBNull.Value ? (bool?)null : Convert.ToBoolean(rw["RateAvailable"]),
                                     RateCurrencyID = rw["RateCurrencyID"] == DBNull.Value ? (Int32?)null : Convert.ToInt32(rw["RateCurrencyID"]),
                                     RevenueBrID = rw["RevenueBrId"] == DBNull.Value ? (Int32?)null : Convert.ToInt32(rw["RevenueBrId"]),
                                     RevenueBr = Convert.ToString(rw["RevenueBranch"]),
                                     ShipCategoryID = rw["ShipCategoryID"] == DBNull.Value ? (Int32?)null : Convert.ToInt32(rw["ShipCategoryID"]),
                                     ShipperName = Convert.ToString(rw["ShipperName"]),
                                     SurveyConductedById = rw["SurveyerID"] == DBNull.Value ? (Int32?)null : Convert.ToInt32(rw["SurveyerID"]),
                                     SurveyDate = rw["SurveyDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(rw["SurveyDate"]),
                                     SurveyDateTime = rw["SurveryTime"] == DBNull.Value ? (TimeSpan?)null : Convert.ToDateTime(rw["SurveryTime"]).TimeOfDay,
                                     SurveyRemarks = Convert.ToString(rw["SurveyRemarks"]),
                                     IsCompleted = rw["IsCompleted"] == DBNull.Value ? (int?)null : Convert.ToInt32(rw["IsCompleted"]),
                                     CompletedStatus = Convert.ToString(rw["CompletedStatus"]),
                                     CompletedBy = Convert.ToString(rw["CompletedBy"]),
                                     CompletedDate = rw["CompletedDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(rw["CompletedDate"]),
                                     BussLineName = Convert.ToString(rw["BussLineName"]),




                                     EnquiryDetail = new EnquiryDetail()
                                     {

                                         ContainerTypeID = rw["ContainerTypeID"] == DBNull.Value ? (Int32?)null : Convert.ToInt32(rw["ContainerTypeID"]),
                                         EnqDetailID = Convert.ToInt32(rw["EnqDetailID"]),
                                         EnqID = Convert.ToInt32(rw["EnqID"]),
                                         FromCity = rw["FromCity"] == DBNull.Value ? (Int32?)null : Convert.ToInt32(rw["FromCity"]),
                                         GoodsDescId = rw["GoodsDescId"] == DBNull.Value ? (Int32?)null : Convert.ToInt32(rw["GoodsDescId"]),
                                         HandlingBrId = rw["HandlingBrId"] == DBNull.Value ? (Int32?)null : Convert.ToInt32(rw["HandlingBrId"]),
                                         Mode = Convert.ToInt32(rw["ModeID"]),
                                         Remarks = Convert.ToString(rw["Remarks"]),
                                         SchSurveyDate = rw["SchSurveyDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(rw["SchSurveyDate"]),
                                         SchSurveyorID = rw["SchSurveyorID"] == DBNull.Value ? (Int32?)null : Convert.ToInt32(rw["SchSurveyorID"]),
                                         ServiceLineID = rw["ServiceLineID"] == DBNull.Value ? (Int32?)null : Convert.ToInt32(rw["ServiceLineID"]),
                                         ServiceLine = rw["ServiceLine"] == DBNull.Value ? null : Convert.ToString(rw["ServiceLine"]),
                                         ShipmentTypeID = rw["ShipmentTypeID"] == DBNull.Value ? (Int32?)null : Convert.ToInt32(rw["ShipmentTypeID"]),
                                         TentativeMovedate = rw["TentativeMovedate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(rw["TentativeMovedate"]),
                                         ToCityID = rw["ToCityID"] == DBNull.Value ? (Int32?)null : Convert.ToInt32(rw["ToCityID"]),
                                         VolumeGross = rw["VolumeGross"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(rw["VolumeGross"]),
                                         VolumeNet = rw["VolumeNet"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(rw["VolumeNet"]),
                                         VolumeToPack = rw["VolumeToPack"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(rw["VolumeToPack"]),
                                         WtACWT = rw["WtACWT"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(rw["WtACWT"]),
                                         VolumeUnitID = rw["VolumeUnitID"] == DBNull.Value ? (Int32?)null : Convert.ToInt32(rw["VolumeUnitID"]),
                                         WtGross = rw["WtGross"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(rw["WtGross"]),
                                         WtNet = rw["WtNet"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(rw["WtNet"]),
                                         WtUnitid = rw["WtUnitid"] == DBNull.Value ? (Int32?)null : Convert.ToInt32(rw["WtUnitid"]),
                                         DensiyFactor = rw["DensityFact"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(rw["DensityFact"]),
                                         LCLFCL = Convert.ToString(rw["EntLCLorFCL"]),
                                         LooseCased = Convert.ToString(rw["EntLooseCased"]),
                                     },
                                     shipmentDetail = new ShipmentDetail()
                                     {
                                         SurveyID = rw["surveyID"] == DBNull.Value ? (Int64?)null : Convert.ToInt64(rw["surveyID"]),
                                         SurveyLooseCased = Convert.ToString(rw["SurveyLooseCased"]),
                                         SurveyDensityFact = rw["SurveyDensityFact"] == DBNull.Value ? rw["DensityFact"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(rw["DensityFact"]) : Convert.ToDecimal(rw["SurveyDensityFact"]),
                                         SurveyLCLorFCL = Convert.ToString(rw["SurveyLCLorFCL"]),
                                         Survey_VolumeUnitID = rw["SurveyVolumeUnitID"] == DBNull.Value ? (Int32?)null : Convert.ToInt32(rw["SurveyVolumeUnitID"]),
                                         DestAdd = Convert.ToString(rw["DestAdd"]),
                                         DestAdd2 = Convert.ToString(rw["DestAdd2"]),
                                         DestCityID = rw["DestCityID"] == DBNull.Value ? (Int32?)null : Convert.ToInt32(rw["DestCityID"]),
                                         DestPhone = Convert.ToString(rw["DestPhone"]),
                                         DestPin = Convert.ToString(rw["DestPin"]),
                                         Email = Convert.ToString(rw["DestEmailID"]),
                                         OrgAdd = Convert.ToString(rw["OrgAdd"]),
                                         OrgAdd2 = Convert.ToString(rw["OrgAdd2"]),
                                         OrgCityID = rw["OrgCityID"] == DBNull.Value ? (Int32?)null : Convert.ToInt32(rw["OrgCityID"]),
                                         OrgPhone = Convert.ToString(rw["OrgPhone"]),
                                         OrgEmail = Convert.ToString(rw["OrgEmailID"]),
                                         OrgPin = Convert.ToString(rw["OrgPin"]),
                                         Remarks = "",
                                         Survey_ContainerID = rw["SurveyContainerSizeID"] == DBNull.Value ? (Int32?)null : Convert.ToInt32(rw["SurveyContainerSizeID"]),
                                         Survey_Volume_GrossValue = rw["SurveyGrossVol"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(rw["SurveyGrossVol"]),
                                         Survey_Volume_NetValue = rw["SurveyNetVol"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(rw["SurveyNetVol"]),
                                         Survey_Volume_TobePackedValue = rw["SurveyTobePackedVol"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(rw["SurveyTobePackedVol"]),
                                         Survey_WeightUnitID = rw["SurveyWeightUnitID"] == DBNull.Value ? (Int32?)null : Convert.ToInt32(rw["SurveyWeightUnitID"]),
                                         Survey_Weight_ACWTValue = rw["SurveyACWTWt"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(rw["SurveyACWTWt"]),
                                         Survey_Weight_GrossValue = rw["SurveyGrossWt"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(rw["SurveyGrossWt"]),
                                         Survey_Weight_NetValue = rw["SurveyNetWt"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(rw["SurveyNetWt"]),
                                     }

                                 }).First();


                    if (surveyDt.Tables[1] != null && surveyDt.Tables[1].Rows.Count > 0)
                    {
                        foreach (DataRow item in surveyDt.Tables[1].Rows)
                        {
                            ServiceDetail service = new ServiceDetail();
                            service.SurveyDetailsID = Convert.ToInt32(item["SurveyDetailsID"]);
                            service.CostHeadID = Convert.ToInt32(item["CostHeadID"]);
                            service.CostHeadName = Convert.ToString(item["CostHeadName"]);
                            service.RateCompID = Convert.ToInt32(item["RateCompID"]);
                            service.RateCompName = Convert.ToString(item["RateComponentName"]);
                            service.RemarksOnCostHead = Convert.ToString(item["RemarksOnCostHead"]);
                            surveyObj.ServiceDetailList.Add(service);
                        }
                    }

                    return surveyObj;
                }
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "surveyBL", "GetDetailById", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

            return surveyObj;

        }

        public IEnumerable<SurveyData> GetsurveyList(DateTime? FromDate, DateTime? Todate, string Shipper, string searchType, string search, string sort, string sortdir, int skip, int pageSize, out int totalCount, int? EnqID = -1, int? CompId = -1, int? surveyID = 1)
        {
            totalCount = 0;

            try
            {
                int RMCBuss = 0;
                if (UserSession.GetUserSession().BussinessLine == "NON RMC-BUSINESS")
                {
                    RMCBuss = 0;
                }
                else
                {
                    RMCBuss = 1;
                }

                IQueryable<SurveyData> surveyList = surveyDAL.GetsurveyList(FromDate, Todate, Shipper, searchType, search, UserSession.GetUserSession().LoginID, EnqID, UserSession.GetUserSession().CompanyID, surveyID, RMCBuss);

                totalCount = surveyList.Count();

                if (!string.IsNullOrWhiteSpace(sort))
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
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "surveyBL", "GetsurveyList", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

        }

        public dynamic GetEnquiryDetailAddress(Int64 EnqDetailID, bool IsOrigin = false, bool IsDest = false)
        {

            dynamic addrss = new ExpandoObject();

            try
            {
                DataTable dt = surveyDAL.GetEnquiryDetailAddress(UserSession.GetUserSession().LoginID, EnqDetailID, IsOrigin, IsDest);
                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow rw = dt.Rows[0];
                    addrss.Addrs1 = Convert.ToString(rw["Address1"]);
                    addrss.Addrs2 = Convert.ToString(rw["Address2"]);
                    addrss.CityID = rw["AddressCityID"] == DBNull.Value ? (Int32?)null : Convert.ToInt32(rw["AddressCityID"]);
                    addrss.Pin = Convert.ToString(rw["Pin"]);
                    addrss.Phone = Convert.ToString(rw["Phone1"]);
                    addrss.Email = Convert.ToString(rw["Email"]);
                }

                return addrss;
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "surveyBL", "GetEnquiryDetailAddress", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }
        }

        public SurveyViewModel GetCopyEnqSurvey(Int64 EnqDetailID, Int64 CopyEnqDetailID)
        {
            SurveyViewModel surveyObj = new SurveyViewModel();
            try
            {

                DataSet surveyDt = surveyDAL.GetCopyEnqSurvey(UserSession.GetUserSession().LoginID, EnqDetailID, CopyEnqDetailID);
                if (surveyDt != null && surveyDt.Tables.Count > 0)
                {
                    //////Enq Shippment Detail
                    surveyObj = (from rw in surveyDt.Tables[0].AsEnumerable()
                                 select new SurveyViewModel()
                                 {
                                     SurveyId = rw["surveyID"] == DBNull.Value ? (Int64?)null : Convert.ToInt64(rw["surveyID"]),
                                     EnqID = rw["EnqID"] == DBNull.Value ? (Int64?)null : Convert.ToInt64(rw["EnqID"]),
                                     EnqNo = rw["EnqNo"] == DBNull.Value ? null : Convert.ToString(rw["EnqNo"]),
                                     EnqDetSequenceID = rw["EnqDetSequenceID"] == DBNull.Value ? (Int32?)null : Convert.ToInt32(rw["EnqDetSequenceID"]),
                                     AccountID = rw["AgentID"] == DBNull.Value ? (Int32?)null : Convert.ToInt32(rw["AgentID"]),
                                     AgentID = rw["AgentID"] == DBNull.Value ? (Int32?)null : Convert.ToInt32(rw["AgentID"]),
                                     AccountMngrID = rw["ChangeAcctMgrID"] == DBNull.Value ? (Int32?)null : Convert.ToInt32(rw["ChangeAcctMgrID"]),
                                     EnqDate = rw["EnqDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(rw["EnqDate"]),
                                     EnqRecievedDate = rw["EnqReceivedDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(rw["EnqReceivedDate"]),
                                     EnqRemarks = Convert.ToString(rw["EnqRemarks"]),
                                     RevenueBrID = rw["RevenueBrId"] == DBNull.Value ? (Int32?)null : Convert.ToInt32(rw["RevenueBrId"]),
                                     RevenueBr = Convert.ToString(rw["RevenueBranch"]),
                                     ShipCategoryID = rw["ShipCategoryID"] == DBNull.Value ? (Int32?)null : Convert.ToInt32(rw["ShipCategoryID"]),
                                     ShipperName = Convert.ToString(rw["ShipperName"]),
                                     IsCompleted = rw["IsCompleted"] == DBNull.Value ? (int?)null : Convert.ToInt32(rw["IsCompleted"]),
                                     CompletedStatus = Convert.ToString(rw["CompletedStatus"]),
                                     CompletedBy = Convert.ToString(rw["CompletedBy"]),
                                     CompletedDate = rw["CompletedDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(rw["CompletedDate"]),
                                     BussLineName = Convert.ToString(rw["BussLineName"]),

                                     EnquiryDetail = new EnquiryDetail()
                                     {

                                         ContainerTypeID = rw["ContainerTypeID"] == DBNull.Value ? (Int32?)null : Convert.ToInt32(rw["ContainerTypeID"]),
                                         EnqDetailID = Convert.ToInt32(rw["EnqDetailID"]),
                                         EnqID = Convert.ToInt32(rw["EnqID"]),
                                         FromCity = rw["FromCity"] == DBNull.Value ? (Int32?)null : Convert.ToInt32(rw["FromCity"]),
                                         GoodsDescId = rw["GoodsDescId"] == DBNull.Value ? (Int32?)null : Convert.ToInt32(rw["GoodsDescId"]),
                                         HandlingBrId = rw["HandlingBrId"] == DBNull.Value ? (Int32?)null : Convert.ToInt32(rw["HandlingBrId"]),
                                         Mode = Convert.ToInt32(rw["ModeID"]),
                                         Remarks = Convert.ToString(rw["Remarks"]),
                                         SchSurveyDate = rw["SchSurveyDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(rw["SchSurveyDate"]),
                                         SchSurveyorID = rw["SchSurveyorID"] == DBNull.Value ? (Int32?)null : Convert.ToInt32(rw["SchSurveyorID"]),
                                         ServiceLineID = rw["ServiceLineID"] == DBNull.Value ? (Int32?)null : Convert.ToInt32(rw["ServiceLineID"]),
                                         ShipmentTypeID = rw["ShipmentTypeID"] == DBNull.Value ? (Int32?)null : Convert.ToInt32(rw["ShipmentTypeID"]),
                                         TentativeMovedate = rw["TentativeMovedate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(rw["TentativeMovedate"]),
                                         ToCityID = rw["ToCityID"] == DBNull.Value ? (Int32?)null : Convert.ToInt32(rw["ToCityID"]),
                                         VolumeGross = rw["VolumeGross"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(rw["VolumeGross"]),
                                         VolumeNet = rw["VolumeNet"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(rw["VolumeNet"]),
                                         VolumeToPack = rw["VolumeToPack"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(rw["VolumeToPack"]),
                                         WtACWT = rw["WtACWT"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(rw["WtACWT"]),
                                         VolumeUnitID = rw["VolumeUnitID"] == DBNull.Value ? (Int32?)null : Convert.ToInt32(rw["VolumeUnitID"]),
                                         WtGross = rw["WtGross"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(rw["WtGross"]),
                                         WtNet = rw["WtNet"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(rw["WtNet"]),
                                         WtUnitid = rw["WtUnitid"] == DBNull.Value ? (Int32?)null : Convert.ToInt32(rw["WtUnitid"]),
                                         DensiyFactor = rw["DensityFact"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(rw["DensityFact"]),

                                     },
                                 }).First();
                    //////Enq Shippment Detail

                    //////Survey Details
                    if (surveyDt.Tables.Count > 1 && surveyDt.Tables[1] != null && surveyDt.Tables[1].Rows.Count > 0)
                    {
                        DataRow rw = surveyDt.Tables[1].Rows[0];
                        string CopyAddress = "B";
                        if (Convert.ToInt32(rw["ServiceLineID"]) != surveyObj.EnquiryDetail.ServiceLineID)
                        {
                            CopyAddress = "O";
                        }



                        surveyObj.AvailableRate = rw["AvailableRate"] == DBNull.Value ? (Int64?)null : Convert.ToInt64(rw["AvailableRate"]);
                        // surveyObj.CompetitorId = rw["CompetitorId"] == DBNull.Value ? (Int32?)null : Convert.ToInt32(rw["CompetitorId"]);
                        surveyObj.DeliveryDate = rw["ReqDeliveryDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(rw["ReqDeliveryDate"]);
                        surveyObj.InsCurrencyID = rw["InsCurrrencyID"] == DBNull.Value ? (Int32?)null : Convert.ToInt32(rw["InsCurrrencyID"]);
                        surveyObj.InsuredAmount = rw["ApproxInsuAmt"] == DBNull.Value ? (Double?)null : Convert.ToDouble(rw["ApproxInsuAmt"]);
                        surveyObj.InsuredById = rw["InsurBy"] == DBNull.Value ? (Int32?)null : Convert.ToInt32(rw["InsurBy"]);
                        surveyObj.IsCompition = rw["Competetion"] == DBNull.Value ? (bool?)null : Convert.ToBoolean(rw["Competetion"]);
                        surveyObj.LoadDate = rw["LoadDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(rw["LoadDate"]);
                        surveyObj.MonitoryEntCurrID = rw["MonitoryEntCurrID"] == DBNull.Value ? (Int32?)null : Convert.ToInt32(rw["MonitoryEntCurrID"]);
                        surveyObj.MonitoryEntitlement = rw["MonitoryEntitlement"] == DBNull.Value ? (Int64?)null : Convert.ToInt64(rw["MonitoryEntitlement"]);
                        surveyObj.NoOfDays = rw["NoOfDays"] == DBNull.Value ? (Int32?)null : Convert.ToInt32(rw["NoOfDays"]);
                        surveyObj.PackDate = rw["PackDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(rw["PackDate"]);
                        surveyObj.QuotationSubmissionDate = rw["QuoteSubmissionDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(rw["QuoteSubmissionDate"]);
                        surveyObj.RateAvailable = rw["RateAvailable"] == DBNull.Value ? (bool?)null : Convert.ToBoolean(rw["RateAvailable"]);
                        surveyObj.RateCurrencyID = rw["RateCurrencyID"] == DBNull.Value ? (Int32?)null : Convert.ToInt32(rw["RateCurrencyID"]);
                        surveyObj.SurveyConductedById = rw["SurveyerID"] == DBNull.Value ? (Int32?)null : Convert.ToInt32(rw["SurveyerID"]);
                        surveyObj.SurveyDate = rw["SurveyDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(rw["SurveyDate"]);
                        surveyObj.SurveyDateTime = rw["SurveryTime"] == DBNull.Value ? (TimeSpan?)null : Convert.ToDateTime(rw["SurveryTime"]).TimeOfDay;
                        surveyObj.SurveyRemarks = Convert.ToString(rw["SurveyRemarks"]);

                        #region Survey Shippment
                        surveyObj.shipmentDetail = new ShipmentDetail()
                        {
                            DestAdd = CopyAddress == "B" ? Convert.ToString(rw["DestAdd"]) : string.Empty,
                            DestAdd2 = CopyAddress == "B" ? Convert.ToString(rw["DestAdd2"]) : string.Empty,
                            DestCityID = CopyAddress == "B" ? (string.IsNullOrWhiteSpace(Convert.ToString(rw["DestCityID"])) ? (Int32?)null : Convert.ToInt32(rw["DestCityID"])) : (Int32?)null,
                            DestPhone = CopyAddress == "B" ? Convert.ToString(rw["DestPhone"]) : String.Empty,
                            DestPin = CopyAddress == "B" ? Convert.ToString(rw["DestPin"]) : String.Empty,
                            Email = CopyAddress == "B" ? Convert.ToString(rw["DestEmailID"]) : String.Empty,
                            OrgAdd = Convert.ToString(rw["OrgAdd"]),
                            OrgAdd2 = Convert.ToString(rw["OrgAdd2"]),
                            OrgCityID = string.IsNullOrWhiteSpace(Convert.ToString(rw["OrgCityID"])) ? (Int32?)null : Convert.ToInt32(rw["OrgCityID"]),
                            OrgPhone = Convert.ToString(rw["OrgPhone"]),
                            OrgEmail = Convert.ToString(rw["OrgEmailID"]),
                            OrgPin = Convert.ToString(rw["OrgPin"]),

                            /* //SurveyID = rw["surveyID"] == DBNull.Value ? (Int64?)null : Convert.ToInt64(rw["surveyID"]),
                            SurveyLooseCased = Convert.ToString(rw["SurveyLooseCased"]),
                            SurveyDensityFact = !string.IsNullOrWhiteSpace(Convert.ToString(rw["SurveyDensityFact"]))? Convert.ToDecimal(rw["SurveyDensityFact"]): (decimal?)null,
                            SurveyLCLorFCL = Convert.ToString(rw["SurveyLCLorFCL"]),
                            Survey_VolumeUnitID = string.IsNullOrWhiteSpace(Convert.ToString(rw["SurveyVolumeUnitID"])) ? (Int32?)null : Convert.ToInt32(rw["SurveyVolumeUnitID"]),
                            Remarks = "",
                            Survey_ContainerID = string.IsNullOrWhiteSpace(Convert.ToString(rw["SurveyContainerSizeID"])) ? (Int32?)null : Convert.ToInt32(rw["SurveyContainerSizeID"]),
                            Survey_Volume_GrossValue = string.IsNullOrWhiteSpace(Convert.ToString(rw["SurveyGrossVol"])) ? (decimal?)null : Convert.ToDecimal(rw["SurveyGrossVol"]),
                            Survey_Volume_NetValue = string.IsNullOrWhiteSpace(Convert.ToString(rw["SurveyNetVol"])) ? (decimal?)null : Convert.ToDecimal(rw["SurveyNetVol"]),
                            Survey_Volume_TobePackedValue = string.IsNullOrWhiteSpace(Convert.ToString(rw["SurveyTobePackedVol"])) ? (decimal?)null : Convert.ToDecimal(rw["SurveyTobePackedVol"]),
                            Survey_WeightUnitID = string.IsNullOrWhiteSpace(Convert.ToString(rw["SurveyWeightUnitID"])) ? (Int32?)null : Convert.ToInt32(rw["SurveyWeightUnitID"]),
                            Survey_Weight_ACWTValue = string.IsNullOrWhiteSpace(Convert.ToString(rw["SurveyACWTWt"])) ? (decimal?)null : Convert.ToDecimal(rw["SurveyACWTWt"]),
                            Survey_Weight_GrossValue = string.IsNullOrWhiteSpace(Convert.ToString(rw["SurveyGrossWt"] )) ? (decimal?)null : Convert.ToDecimal(rw["SurveyGrossWt"]),
                            Survey_Weight_NetValue = string.IsNullOrWhiteSpace(Convert.ToString(rw["SurveyNetWt"] )) ? (decimal?)null : Convert.ToDecimal(rw["SurveyNetWt"]),
                            */
                        };

                        #endregion

                    }
                    //////Survey Details


                    if (surveyDt.Tables.Count > 2 && surveyDt.Tables[2] != null && surveyDt.Tables[2].Rows.Count > 0)
                    {
                        foreach (DataRow item in surveyDt.Tables[2].Rows)
                        {
                            ServiceDetail service = new ServiceDetail();
                            service.SurveyDetailsID = Convert.ToInt32(item["SurveyDetailsID"]);
                            service.CostHeadID = Convert.ToInt32(item["CostHeadID"]);
                            service.CostHeadName = Convert.ToString(item["CostHeadName"]);
                            service.RateCompID = Convert.ToInt32(item["RateCompID"]);
                            service.RateCompName = Convert.ToString(item["RateComponentName"]);
                            service.RemarksOnCostHead = Convert.ToString(item["RemarksOnCostHead"]);
                            surveyObj.ServiceDetailList.Add(service);
                        }
                    }

                    return surveyObj;
                }
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "surveyBL", "GetCopyEnqSurvey", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

            return surveyObj;

        }

        public JobDocument GetVoxmeReport(Int64 EnqID, string EnqNo)
        {
            JobDocument document = new JobDocument();
            try
            {
                document = surveyDAL.GetVoxmeReport(UserSession.GetUserSession().LoginID, EnqID, EnqNo);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "surveyBL", "GetVoxmeReport", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

            return document;

        }

        public List<SurveyCommItem> GetCommItemDetails(int LoginID, Int64? surveyID)
        {
            List<SurveyCommItem> SurveyCommItemList = new List<SurveyCommItem>();
            try
            {
                DataSet CommItemDetailsDs = surveyDAL.GetCommItemDetails(LoginID, surveyID);

                if (CommItemDetailsDs != null && CommItemDetailsDs.Tables.Count > 0)
                {
                    if (CommItemDetailsDs.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow item in CommItemDetailsDs.Tables[0].Rows)
                        {
                            SurveyCommItem SurveyCommItem = new SurveyCommItem();
                            SurveyCommItem.SurveyItemDeiID = item["SurveyItemDeiID"] == DBNull.Value ? (Int64?)null : Convert.ToInt32(item["SurveyItemDeiID"]);
                            SurveyCommItem.ItemName = item["ItemName"] == DBNull.Value ? null : Convert.ToString(item["ItemName"]);
                            SurveyCommItem.QuantiyUnitID = item["QuantiyUnit"] == DBNull.Value ? (int?)null : Convert.ToInt32(item["QuantiyUnit"]);
                            SurveyCommItem.QuantiyUnit = item["QtyUnitName"] == DBNull.Value ? null : Convert.ToString(item["QtyUnitName"]);
                            SurveyCommItem.Quantity = item["Quantity"] == DBNull.Value ? (int?)null : Convert.ToInt32(item["Quantity"]);
                            SurveyCommItem.NoOfPack = item["NoOfPack"] == DBNull.Value ? (int?)null : Convert.ToInt32(item["NoOfPack"]);
                            SurveyCommItem.VolUnitID = item["VolUnit"] == DBNull.Value ? (int?)null : Convert.ToInt32(item["VolUnit"]);
                            SurveyCommItem.VolUnit = item["VolUnitName"] == DBNull.Value ? null : Convert.ToString(item["VolUnitName"]);
                            SurveyCommItem.ExpectedVol = item["ExpectedVol"] == DBNull.Value ? (int?)null : Convert.ToInt32(item["ExpectedVol"]);
                            SurveyCommItem.SchDepDate = item["SchDepDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(item["SchDepDate"]);

                            SurveyCommItemList.Add(SurveyCommItem);
                        }
                    }
                }
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "surveyBL", "GetDetailById", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }
            return SurveyCommItemList;
        }

        public bool SaveCommItemDetails(SurveyViewModel survey, out string result)
        {
            try
            {
                return surveyDAL.SaveCommItemDetails(survey, out result);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "surveyBL", "SaveCommItemDetails", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }
        }
    }
}