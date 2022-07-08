using RELOCBS.Common.ExceptionHandling;
using RELOCBS.DAL.Pricing;
using RELOCBS.Entities;
using RELOCBS.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace RELOCBS.BL.Pricing
{
    public class CostBL
    {
        private CostDAL _costDAL;

        public CostDAL costDAL
        {

            get
            {
                if (this._costDAL == null)
                    this._costDAL = new CostDAL();
                return this._costDAL;
            }
        }

        public bool InsertCost(CostViewModel SaveRate, int LoginID, out string result)
        {
            result = String.Empty;

            try
            {
                SaveRate.CompanyID = UserSession.GetUserSession().CompanyID;
                return costDAL.InsertCost(SaveRate, LoginID, out result);

            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(LoginID), "CostBL", "Insert", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

        }

        public IEnumerable<Cost> GetForGrid(int LoginID, int RateComponetID, int? page = 1)
        {
            try
            {
                return costDAL.GetForGrid(LoginID, RateComponetID, page);

            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(LoginID), "CostBL", "Insert", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }


        }

        public void GetRateComponentLable(int RateComponentID, out String FromLoc, out string ToLoc)
        {
            FromLoc = "";
            ToLoc = "";

            if (RateComponentID == 1)
            {
                FromLoc = "Origin City";
                ToLoc = "Origin Port";

            }
            else if (RateComponentID == 2)
            {
                FromLoc = "Origin Port";
                ToLoc = "Destination Port";

            }
            else if (RateComponentID == 3)
            {
                FromLoc = "Destination Port";
                ToLoc = "Destination City";

            }
            else if (RateComponentID == 4)
            {
                FromLoc = "Origin City";
                ToLoc = "Destination City";

            }

        }

        public CostViewModel GetDetailById(int? SurveyID = -1, int? RateCompRateWtID = -1, int RateCompRateBatchId = 0)
        {
            CostViewModel CostObj = new CostViewModel();
            try
            {
                CostObj.BaseCurrencyRateID = UserSession.GetUserSession().BaseCurrID;
                CostObj.RateCurrencyID = UserSession.GetUserSession().BaseCurrID;
                DataSet CostDs = costDAL.GetDetailById(SurveyID, RateCompRateWtID, UserSession.GetUserSession().LoginID, RateCompRateBatchId);

                if (CostDs != null)
                {
                    CostObj.CostHeadList = new List<CostHeadDetail>();
                    if (CostDs.Tables.Count > 0 && CostDs.Tables[0] != null && CostDs.Tables[0].Rows.Count > 0)
                    {
                        CostObj.SurveyID = CostDs.Tables[0].Rows[0]["SurveyID"] == DBNull.Value ? (int?)null : Convert.ToInt32(CostDs.Tables[0].Rows[0]["SurveyID"]);
                        CostObj.EnqID = CostDs.Tables[0].Rows[0]["EnqID"] == DBNull.Value ? (int?)null : Convert.ToInt32(CostDs.Tables[0].Rows[0]["EnqID"]);
                        CostObj.EnqNo = CostDs.Tables[0].Rows[0]["ENQNO"] == DBNull.Value ? null : Convert.ToString(CostDs.Tables[0].Rows[0]["ENQNO"]);
                        CostObj.EnqDetSequenceID = CostDs.Tables[0].Rows[0]["EnqDetSequenceID"] == DBNull.Value ? (int?)null : Convert.ToInt32(CostDs.Tables[0].Rows[0]["EnqDetSequenceID"]);
                        CostObj.EnqDetailID = CostDs.Tables[0].Rows[0]["EnqDetailID"] == DBNull.Value ? (int?)null : Convert.ToInt32(CostDs.Tables[0].Rows[0]["EnqDetailID"]);
                        CostObj.Shipper = Convert.ToString(CostDs.Tables[0].Rows[0]["Shipper"]);
                        CostObj.FromLocationID = Convert.ToInt32(CostDs.Tables[0].Rows[0]["FromCityID"]);
                        CostObj.ToLocationID = Convert.ToInt32(CostDs.Tables[0].Rows[0]["ToCityID"]);
                        CostObj.ModeID = Convert.ToInt32(CostDs.Tables[0].Rows[0]["ModeID"]);
                        CostObj.ModeName = Convert.ToString(CostDs.Tables[0].Rows[0]["Mode"]);
                        CostObj.GoodsDescriptionID = Convert.ToInt32(CostDs.Tables[0].Rows[0]["GoodsDescId"]);
                        CostObj.GoodsDescriptionName = Convert.ToString(CostDs.Tables[0].Rows[0]["GoodsDescName"]);
                        CostObj.BusinessLineID = Convert.ToInt32(CostDs.Tables[0].Rows[0]["BussinessLineID"]);
                        CostObj.BusinessLineName = Convert.ToString(CostDs.Tables[0].Rows[0]["BussLineName"]);
                        CostObj.ServiceLineID = Convert.ToInt32(CostDs.Tables[0].Rows[0]["ServiceLineID"]);
                        CostObj.Project = Convert.ToString(CostDs.Tables[0].Rows[0]["Project"]);
                        CostObj.ServiceLineName = Convert.ToString(CostDs.Tables[0].Rows[0]["ServiceLine"]);
                        CostObj.FromLocationName = Convert.ToString(CostDs.Tables[0].Rows[0]["FromCityName"]);
                        CostObj.ToLocationName = Convert.ToString(CostDs.Tables[0].Rows[0]["ToCityName"]);
                        CostObj.DefaultAgentID = CostDs.Tables[0].Rows[0]["DefaultAgentID"] == DBNull.Value ? (int?)null : Convert.ToInt32(CostDs.Tables[0].Rows[0]["DefaultAgentID"]);

                        CostObj.WeightUnitID = Convert.ToInt32(CostDs.Tables[0].Rows[0]["WeightUnitID"]);
                        CostObj.WeightUnitFrom = Convert.ToInt64(CostDs.Tables[0].Rows[0]["WeightFrom"]);
                        CostObj.WeightUnitTo = Convert.ToInt64(CostDs.Tables[0].Rows[0]["WeightTo"]);
                        CostObj.RMCID = CostDs.Tables[0].Rows[0]["rmcid"] == DBNull.Value ? 0 : Convert.ToInt32(CostDs.Tables[0].Rows[0]["rmcid"]);
                    }

                    if (CostDs.Tables.Count > 1 && CostDs.Tables[1] != null && CostDs.Tables[1].Rows.Count > 0)
                    {
                        CostObj.AgentName = Convert.ToString(CostDs.Tables[1].Rows[0]["AgentName"]);
                        CostObj.BusinessLineName = Convert.ToString(CostDs.Tables[1].Rows[0]["BusinessLineName"]);
                        CostObj.GoodsDescriptionName = Convert.ToString(CostDs.Tables[1].Rows[0]["GoodsDescName"]);
                        CostObj.ServiceLineID = Convert.ToInt32(CostDs.Tables[0].Rows[0]["ServiceLineID"]);
                        //CostObj.RateCompRateWtBatchID = Convert.ToInt32(CostDs.Tables[1].Rows[0]["RateCompRateWtBatchID"]);
                        CostObj.RateCompRateWtID = RateCompRateBatchId > 0 ? 0 : Convert.ToInt32(CostDs.Tables[1].Rows[0]["RateCompanyRateWtID"]);
                        CostObj.WeightUnitID = Convert.ToInt32(CostDs.Tables[1].Rows[0]["WeightUnitID"]);
                        CostObj.WeightUnitFrom = Convert.ToInt64(CostDs.Tables[1].Rows[0]["WeightFrom"]);
                        CostObj.WeightUnitTo = Convert.ToInt64(CostDs.Tables[1].Rows[0]["WeightTo"]);
                        CostObj.FromLocationID = Convert.ToInt32(CostDs.Tables[1].Rows[0]["OrgCityID"]);
                        CostObj.ToLocationID = Convert.ToInt32(CostDs.Tables[1].Rows[0]["DestCityID"]);
                        CostObj.ExitPointID = CostDs.Tables[1].Rows[0]["OrgPortID"] == DBNull.Value ? (int?)0 : Convert.ToInt32(CostDs.Tables[1].Rows[0]["OrgPortID"]);
                        CostObj.EntryPointID = CostDs.Tables[1].Rows[0]["DestPortID"] == DBNull.Value ? (int?)0 : Convert.ToInt32(CostDs.Tables[1].Rows[0]["DestPortID"]);
                        CostObj.ModeID = Convert.ToInt32(CostDs.Tables[1].Rows[0]["ModeID"]);
                        CostObj.RMCID = Convert.ToInt32(CostDs.Tables[1].Rows[0]["RMCID"]);
                        CostObj.BusinessLineID = Convert.ToInt32(CostDs.Tables[1].Rows[0]["BussinessLineID"]);
                        CostObj.GoodsDescriptionID = Convert.ToInt32(CostDs.Tables[1].Rows[0]["GoodsDescID"]);
                        DataRow[] dr = CostDs.Tables[1].Select("RateCompID=" + Convert.ToInt32(RELOCBS.Common.CommonService.RateComp.Freight));
                        if (dr != null && dr.Count() > 0)
                        {
                            CostObj.ShipingLineID = dr.First()["ShipinglineID"] != DBNull.Value ? Convert.ToInt32(dr.First()["ShipinglineID"]) : (int?)null;
                            CostObj.TransitAgentID = dr.First()["TransitAgentID"] != DBNull.Value ? Convert.ToInt32(dr.First()["TransitAgentID"]) : 0;
                        }

                        CostObj.WeightUnitName = Convert.ToString(CostDs.Tables[1].Rows[0]["WeightUnitName"]);
                        CostObj.FromLocationName = Convert.ToString(CostDs.Tables[1].Rows[0]["OriginCityName"]);
                        CostObj.ToLocationName = Convert.ToString(CostDs.Tables[1].Rows[0]["DestinationCityName"]);

                        CostObj.ExitPointName = Convert.ToString(CostDs.Tables[1].Rows[0]["OriginPortName"]);
                        CostObj.EntryPointName = Convert.ToString(CostDs.Tables[1].Rows[0]["DestinationPortName"]);
                        CostObj.ModeName = Convert.ToString(CostDs.Tables[1].Rows[0]["ModeName"]);
                        CostObj.RMCName = Convert.ToString(CostDs.Tables[1].Rows[0]["RMCName"]);
                        CostObj.ShipingLineName = CostDs.Tables[1].Rows[0]["ShipinglineID"] == DBNull.Value ? "" : Convert.ToString(CostDs.Tables[1].Rows[0]["ShipLineName"]);
                        CostObj.BusinessLineName = Convert.ToString(CostDs.Tables[1].Rows[0]["BusinessLineName"]);
                        CostObj.GoodsDescriptionName = Convert.ToString(CostDs.Tables[1].Rows[0]["GoodsDescName"]);

                        CostObj.CostHeadList = (from item in CostDs.Tables[1].AsEnumerable()
                                                select new CostHeadDetail()
                                                {
                                                    CostHeadID = Convert.ToInt32(item["CostHeadID"]),
                                                    BaseCurrencyRateID = Convert.ToInt32(item["BaseCurrID"]),
                                                    RateCurrencyID = Convert.ToInt32(item["RateCurrID"]),
                                                    RateComponentID = Convert.ToInt32(item["RateCompID"]),
                                                    AgentID = Convert.ToInt32(item["AgentID"]),
                                                    CostHeadName = Convert.ToString(item["CostHeadName"]),
                                                    BaseCurrencyRateName = Convert.ToString(item["BaseCurrName"]),
                                                    RateCurrencyName = Convert.ToString(item["RateCurrName"]),
                                                    RateComponentName = Convert.ToString(item["RateComponentName"]),
                                                    AgentName = Convert.ToString(item["AgentName"]),
                                                    Amount = item["RateCurrValue"] == DBNull.Value ? 0 : decimal.Round(Convert.ToDecimal(item["RateCurrValue"]), 2, MidpointRounding.AwayFromZero),
                                                    ConversionRate = Convert.ToDecimal(item["BaseCurrConversRate"]),
                                                    TransitTimeFrom = Convert.ToInt32(item["TransTimeFrom"]),
                                                    TransitTimeTo = Convert.ToInt32(item["TransTimeTo"])
                                                }).ToList();
                    }

                    if (CostDs.Tables.Count > 2 && CostDs.Tables[2] != null && CostDs.Tables[2].Rows.Count > 0)
                    {
                        CostObj.SurveyRateGridDt = (from rw in CostDs.Tables[2].AsEnumerable()
                                                    select new CostEstimateGrid()
                                                    {
                                                        RateCompRateBatchId = Convert.ToInt64(rw["RateCompRateBatchId"]),
                                                        SurveyID = Convert.ToInt64(rw["SurveyID"]),
                                                        ratecompanyratewtid = Convert.ToInt64(rw["ratecompanyratewtid"]),
                                                        Colour = Convert.ToString(rw["Colour"]),
                                                        FromCity = Convert.ToString(rw["FromCity"]),
                                                        ToCity = Convert.ToString(rw["ToCity"]),
                                                        EntryPort = Convert.ToString(rw["EntryPort"]),
                                                        Exitport = Convert.ToString(rw["Exitport"]),
                                                        TotEstimate = Convert.ToInt64(rw["TotEstimate"]),
                                                        WeightFrom = Convert.ToInt64(rw["WeightFrom"]),
                                                        WeightTo = Convert.ToInt64(rw["WeightTo"]),
                                                        WtUnit = Convert.ToString(rw["WeightUnitName"]),
                                                        Remarks = Convert.ToString(rw["Remarks"])
                                                        //WtUnitID = Convert.ToInt32(rw["WeightUnitID"])
                                                    }
                                                ).ToList();
                        if (Convert.ToInt32(RateCompRateBatchId) > 0 && Convert.ToInt32(RateCompRateWtID) > 0)
                        {
                            CostObj.Remarks = CostObj.SurveyRateGridDt.Where(x => x.RateCompRateBatchId == Convert.ToInt32(RateCompRateBatchId) && x.ratecompanyratewtid == Convert.ToInt32(RateCompRateWtID)).FirstOrDefault().Remarks;
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
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "CostBL", "GetDetailById", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

            return CostObj;
        }


        public List<CostEstimateGrid> GetGridFromCity(int LoginID, int? FromCityID, int? ToCityID, int? ExitPortID = -1, int? EntryPortID = -1)
        {
            List<CostEstimateGrid> CostGrid = new List<CostEstimateGrid>();

            try
            {
                var CostDs = costDAL.GetGridFromCity(LoginID, FromCityID, ToCityID, ExitPortID, EntryPortID);

                if (CostDs != null && CostDs.Tables.Count > 0)
                {
                    if (CostDs.Tables.Count > 2 && CostDs.Tables[2] != null && CostDs.Tables[2].Rows.Count > 0)
                    {
                        CostGrid = (from rw in CostDs.Tables[2].AsEnumerable()
                                    select new CostEstimateGrid()
                                    {
                                        SurveyID = Convert.ToInt64(rw["SurveyID"]),
                                        ratecompanyratewtid = Convert.ToInt64(rw["ratecompanyratewtid"]),
                                        Colour = Convert.ToString(rw["Colour"]),
                                        FromCity = Convert.ToString(rw["FromCity"]),
                                        ToCity = Convert.ToString(rw["ToCity"]),
                                        EntryPort = Convert.ToString(rw["EntryPort"]),
                                        Exitport = Convert.ToString(rw["Exitport"]),
                                        TotEstimate = Convert.ToInt64(rw["TotEstimate"]),
                                        WeightFrom = Convert.ToInt64(rw["WeightFrom"]),
                                        WeightTo = Convert.ToInt64(rw["WeightTo"])

                                    }
                                                    ).ToList();
                    }
                }
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(LoginID), "CostBL", "GetGrid", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

            return CostGrid;

        }


        public AccessServiceAgentList GetCitywiseAccessServiceRate(int cityid, int rmcid)
        {
            AccessServiceAgentList model = new AccessServiceAgentList();

            try
            {
                model.ASACityID = Convert.ToInt32(cityid);
                model.ASARMCID = Convert.ToInt32(rmcid);

                if (cityid > 0 && rmcid > 0)
                {

                    model.AgentList = costDAL.GetCitywiseAccessServiceRate(cityid, rmcid, UserSession.GetUserSession().CompanyID, UserSession.GetUserSession().LoginID);

                    if (model.AgentList != null && model.AgentList.Rows.Count > 0)
                    {
                        //AdditionalServicesData AddSvr;
                        //List<AdditionalServicesData> lstAddSvr = new List<AdditionalServicesData>();
                        //for (int i = 0; i < model.CitywiseVendors.Rows.Count; i++)
                        //{
                        //    AddSvr = new AdditionalServicesData();
                        //    AddSvr.AdditionalServicesID = Convert.ToInt32(model.CitywiseVendors.Rows[i]["AdditionalServicesID"]);
                        //    AddSvr.ServiceName = model.CitywiseVendors.Rows[i]["ServiceName"].ToString();
                        //    AddSvr.UOM = model.CitywiseVendors.Rows[i]["UOM"].ToString();
                        //    //AddSvr.WriterProfit= model.CitywiseVendors.Rows[i]["WriterProfit"]!=null?Convert.ToDecimal(model.CitywiseVendors.Rows[i]["WriterProfit"]) : null;
                        //    lstAddSvr.Add(AddSvr);
                        //}

                        CitywiseAdditionalServiceRates cityaddSvrrate;
                        List<CitywiseAdditionalServiceRates> lstcityaddSvrrate = new List<CitywiseAdditionalServiceRates>();
                        for (int i = 0; i < model.AgentList.Rows.Count; i++)
                        {

                            cityaddSvrrate = new CitywiseAdditionalServiceRates();
                            cityaddSvrrate.AdditionalServiceId = Convert.ToInt32(model.AgentList.Rows[i]["AdditionalServicesID"]);
                            cityaddSvrrate.ServiceName = model.AgentList.Rows[i]["ServiceName"].ToString();
                            cityaddSvrrate.WriterProfit = model.AgentList.Rows[i]["WriterProfit"] != DBNull.Value ? Convert.ToDecimal(model.AgentList.Rows[i]["WriterProfit"]) : 0;
                            cityaddSvrrate.Rate = model.AgentList.Rows[i]["Rate"] != DBNull.Value ? Convert.ToDecimal(model.AgentList.Rows[i]["Rate"]) : 0;
                            cityaddSvrrate.MinAmt = model.AgentList.Rows[i]["CalculatedWBCOST"] != DBNull.Value ? Convert.ToDecimal(model.AgentList.Rows[i]["CalculatedWBCOST"]) : 0;
                            cityaddSvrrate.MaxAmt = model.AgentList.Rows[i]["CalculatedWBCOST"] != DBNull.Value ? Convert.ToDecimal(model.AgentList.Rows[i]["CalculatedWBCOST"]) : 0;
                            cityaddSvrrate.USDPM = model.AgentList.Rows[i]["USDPM"] != DBNull.Value ? Convert.ToString(model.AgentList.Rows[i]["USDPM"]) : "P";
                            //cityaddSvrrate.UOMID = model.AgentList.Rows[i]["UOMID"] != DBNull.Value ? Convert.ToInt32(model.AgentList.Rows[i]["UOMID"]) : 0;

                            lstcityaddSvrrate.Add(cityaddSvrrate);
                        }

                        model.CitywiseAdditionalServiceRates = lstcityaddSvrrate;
                        //model.AdditionalServicesData = lstAddSvr;

                        DataTable dt = model.AgentList;
                        if (dt.Columns.Contains("Rate"))
                        {
                            dt.Columns.Remove("Rate");
                        }
                        if (dt.Columns.Contains("UOMID"))
                        {
                            dt.Columns.Remove("UOMID");
                        }
                        if (dt.Columns.Contains("WriterProfit"))
                        {
                            dt.Columns.Remove("WriterProfit");
                        }
                        if (dt.Columns.Contains("USDPM"))
                        {
                            dt.Columns.Remove("USDPM");
                        }
                        if (dt.Columns.Contains("CalculatedWBCOST"))
                        {
                            dt.Columns.Remove("CalculatedWBCOST");
                        }

                        model.AgentList = dt;
                    }
                }
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "CostBL", "GetCitywiseAccessServiceRate", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

            return model;

        }

        public bool Delete(int SurveyID, int RateCompRateWtID, int RateCompRateBatchID, int LoginID, out string message)
        {
            message = String.Empty;

            try
            {
                return costDAL.Delete(SurveyID, RateCompRateWtID, RateCompRateBatchID, LoginID, out message);

            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(LoginID), "CostBL", "Delete", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }


        }

        public IEnumerable<HistoryRate> getHistoryRate(int ComponentID, int RMCID, int? AgentID, int? City1, int? City2, int? Port1, int? Port2, char RevnOrCost, int? ExitPortAir, int? EntryPortAir, string sort, string sortdir, int skip, int pageSize, out int totalCount)
        {
            IQueryable<HistoryRate> rates;
            try
            {
                rates = costDAL.getHistoryRate(UserSession.GetUserSession().LoginID, ComponentID, RMCID, AgentID, City1, City2, Port1, Port2, RevnOrCost, ExitPortAir, EntryPortAir).AsQueryable();
                totalCount = rates.Count();
                if (totalCount > 0)
                {

                    //if (!string.IsNullOrEmpty(sort) && !string.IsNullOrEmpty(sortdir))
                    //{
                    //    rates = rates.OrderBy(sort + " " + sortdir);
                    //}
                    if (pageSize > 1)
                    {
                        rates = rates.Skip((skip * (pageSize - 1))).Take(skip);
                    }
                    else
                    {
                        rates = rates.Take(skip);
                    }
                }
                return rates;
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "CostBL", "getHistoryRate", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }



        }

        public DataSet GetCompareRate(int SurveyID, int LoginID)
        {
            DataSet ds = new DataSet();
            //dt1.Columns.Add("CompareDesc");
            //dt1.Rows.Add("ACWT");
            //dt1.Rows.Add("Exit Port");
            //dt1.Rows.Add("Shipping Line");
            //dt1.Rows.Add("Dest. Agent");
            //dt1.Rows.Add("Entry Port");
            DataSet dsdata = costDAL.GetCompareRate(SurveyID, LoginID);

            if (dsdata != null && dsdata.Tables.Count > 1)
            {
                DataTable data = dsdata.Tables[0].Copy();
                DataTable SubCostdt = dsdata.Tables[1].Copy();

                //DataView view = new DataView(data);
                //DataTable distinctValues = view.ToTable("dtComponent", true, new string[] { "Component" });

                var CostHead = data.AsEnumerable()
                    .Select(r => new
                    {
                        RateCompID = r.Field<int>("RateCompID"),
                        CostHeadID = r.Field<int>("CostHeadID"),
                        Component = r.Field<string>("Component")
                    })
                    .Distinct();

                DataTable distinctValues = new DataTable("dtComponent");
                distinctValues.Columns.Add("RateCompID", typeof(int));
                distinctValues.Columns.Add("CostHeadID", typeof(int));
                distinctValues.Columns.Add("SubCostHeadID", typeof(int));
                distinctValues.Columns.Add("Component", typeof(string));

                foreach (var item in CostHead)
                {
                    distinctValues.Rows.Add(item.RateCompID, item.CostHeadID, 0, "<u><b>" + item.Component + "</b></u>");
                    var subComponent = SubCostdt.AsEnumerable().Where(s => Convert.ToInt32(s["CostHeadID"]) == item.CostHeadID && Convert.ToInt32(s["RateCompID"]) == item.RateCompID)
                        .Select(r => new
                        {
                            RateCompID = r.Field<int>("RateCompID"),
                            CostHeadID = r.Field<int>("CostHeadID"),
                            SubCostHeadID = r.Field<int>("SubCostHeadID"),
                            Component = r.Field<string>("SubCostHeadName")
                        }).Distinct();
                    if (subComponent != null && subComponent.Count() > 0)
                    {
                        foreach (var value in subComponent)
                        {
                            distinctValues.Rows.Add(value.RateCompID, value.CostHeadID, value.SubCostHeadID, value.Component);
                        }
                    }
                }

                string col = string.Empty;
                if (data != null && data.Rows.Count > 0)
                {

                    foreach (DataRow item in data.Rows)
                    {
                        //col = item["RateCompanyRateWtID"].ToString();
                        col = item["RateCompRateBatchID"].ToString();
                        int rateCompID = Convert.ToInt32(item["RateCompID"]);
                        int costHeadID = Convert.ToInt32(item["CostHeadID"]);
                        if (!distinctValues.Columns.Contains(col))
                        {
                            distinctValues.Columns.Add(col);
                        }



                        distinctValues.AsEnumerable().Where(s => Convert.ToInt32(s["RateCompID"]) == Convert.ToInt32(item["RateCompID"]) && Convert.ToInt32(s["CostHeadID"]) == Convert.ToInt32(item["CostHeadID"])
                        && Convert.ToInt32(s["SubCostHeadID"]) == 0).ToList()
                        .ForEach(D => D.SetField(col, "<u><b>" + item["ValueInBaseCurr"].ToString() + "</b></u>"));


                        var items = (from p in distinctValues.AsEnumerable()
                                     join t in SubCostdt.AsEnumerable()
                                     on new { RateCompID = p.Field<int>("RateCompID"), CostHeadID = p.Field<int>("CostHeadID"), SubCostHeadID = p.Field<int>("SubCostHeadID") } equals new { RateCompID = t.Field<int>("RateCompID"), CostHeadID = t.Field<int>("CostHeadID"), SubCostHeadID = t.Field<int>("SubCostHeadID") }
                                     where t.Field<Int64>("RateCompRateBatchID") == Convert.ToInt64(col) && p.Field<int>("CostHeadID") == costHeadID
                                     select new
                                     {
                                         RateCompID = t.Field<int>("RateCompID"),
                                         CostHeadID = t.Field<int>("CostHeadID"),
                                         SubCostHeadID = t.Field<int>("SubCostHeadID"),
                                         SubCostHeadName = t.Field<string>("SubCostHeadName"),
                                         Amount = t.Field<string>("Amt")
                                     }).ToList();

                        if (items.Count > 0)
                        {
                            foreach (var subitem in items)
                            {
                                distinctValues.AsEnumerable()
                                .Where(s => Convert.ToInt32(s["RateCompID"]) == subitem.RateCompID && Convert.ToInt32(s["CostHeadID"]) == subitem.CostHeadID
                                 && Convert.ToInt32(s["SubCostHeadID"]) == subitem.SubCostHeadID).ToList()
                                .ForEach(D => D.SetField(col, subitem.Amount));
                            }
                            //distinctValues.AsEnumerable().ToList().ForEach(x => x.SetField(col,items.FirstOrDefault(y => y.SubCostHeadName.Equals(Convert.ToString(x["Component"]))).Amount));
                        }

                        //if (SubCostdt.AsEnumerable().Where(p => Convert.ToInt32(p["RateCompID"])==rateCompID && Convert.ToInt64(p["RateCompRateBatchID"]) == Convert.ToInt64(col) && Convert.ToInt32(p["CostHeadID"]) == CostHeadID).Count() > 0)
                        //{
                        //    distinctValues.AsEnumerable().ToList()
                        //    .ForEach(x => x.SetField(col,
                        //    Convert.ToString(
                        //    SubCostdt.AsEnumerable().FirstOrDefault(p => Convert.ToString(p["SubCostHeadName"]) == Convert.ToString(x["Component"])
                        //    && Convert.ToInt32(p["RateCompID"]) == rateCompID
                        //    && Convert.ToInt64(p["RateCompRateBatchID"]) == Convert.ToInt64(col) && Convert.ToInt32(p["CostHeadID"]) == CostHeadID).ItemArray[6]
                        //    )
                        //    ));
                        //}
                    }

                    foreach (System.Data.DataColumn column in distinctValues.Columns)
                    {
                        distinctValues.AsEnumerable().Where(s => string.IsNullOrWhiteSpace(Convert.ToString(s[column.ColumnName]))).ToList()
                        .ForEach(D => D.SetField(column.ColumnName, "0"));
                    }

                    DataTable dt1 = new DataTable("dtDescription");
                    if (distinctValues != null && distinctValues.Columns.Count > 1 && distinctValues.Rows.Count > 0)
                    {
                        dt1 = distinctValues.Clone();
                        dt1.Columns.Remove("RateCompID");
                        dt1.Columns.Remove("CostHeadID");
                        dt1.Columns.Remove("SubCostHeadID");
                        dt1.TableName = "dtDescription";
                        dt1.Rows.Add("Org. Agent");
                        dt1.Rows.Add("Goods Description");
                        dt1.Rows.Add("ACWT");
                        dt1.Rows.Add("Exit Port");
                        dt1.Rows.Add("Shipping Line");
                        dt1.Rows.Add("Dest. Agent");
                        dt1.Rows.Add("Entry Port");
                        dt1.Rows.Add("Remarks");

                        foreach (DataRow item in data.Rows)
                        {
                            col = item["RateCompRateBatchID"].ToString();

                            dt1.AsEnumerable().Where(s => Convert.ToString(s["Component"]).Equals("Goods Description")).ToList()
                                .ForEach(D => D.SetField(col, item["GoodsDescName"].ToString()));

                            dt1.AsEnumerable().Where(s => Convert.ToString(s["Component"]).Equals("ACWT")).ToList()
                            .ForEach(D => D.SetField(col, item["ACWT"].ToString()));

                            dt1.AsEnumerable().Where(s => Convert.ToString(s["Component"]).Equals("Exit Port")).ToList()
                            .ForEach(D => D.SetField(col, item["ExitPortName"].ToString()));

                            dt1.AsEnumerable().Where(s => Convert.ToString(s["Component"]).Equals("Entry Port")).ToList()
                            .ForEach(D => D.SetField(col, item["EntryPortName"].ToString()));

                            dt1.AsEnumerable().Where(s => Convert.ToString(s["Component"]).Equals("Remarks")).ToList()
                            .ForEach(D => D.SetField(col, item["Remarks"].ToString()));

                            if (item["RateCompID"].ToString() == "1")
                            {
                                dt1.AsEnumerable().Where(s => Convert.ToString(s["Component"]).Equals("Org. Agent")).ToList()
                                .ForEach(D => D.SetField(col, item["AgentName"].ToString()));
                            }
                            else if (item["RateCompID"].ToString() == "2")
                            {

                                dt1.AsEnumerable().Where(s => Convert.ToString(s["Component"]).Equals("Shipping Line")).ToList()
                                .ForEach(D => D.SetField(col, item["ShippingLine"].ToString()));

                            }
                            else if (item["RateCompID"].ToString() == "3")
                            {

                                dt1.AsEnumerable().Where(s => Convert.ToString(s["Component"]).Equals("Dest. Agent")).ToList()
                                .ForEach(D => D.SetField(col, item["AgentName"].ToString()));

                            }



                        }

                    }

                    foreach (System.Data.DataColumn column in dt1.Columns)
                    {
                        dt1.AsEnumerable().Where(s => string.IsNullOrWhiteSpace(Convert.ToString(s[column.ColumnName]))).ToList()
                        .ForEach(D => D.SetField(column.ColumnName, " "));
                    }

                    int number = 0;
                    foreach (DataColumn column in dt1.Columns)
                    {
                        if (column.ColumnName == "Component")
                        {
                            column.Caption = "Compare Description";
                        }
                        else
                        {
                            column.Caption = "Compare Value " + Convert.ToString(number);
                        }

                        number = number + 1;
                    }
                    number = 0;
                    DataRow row = distinctValues.NewRow();
                    foreach (DataColumn column in distinctValues.Columns)
                    {
                        if (column.ColumnName == "Component")
                        {
                            row[column.ColumnName] = "<u><b>Total</b></u>";
                            column.Caption = "Cost Description";
                        }
                        else if (column.ColumnName != "RateCompID" && column.ColumnName != "CostHeadID" && column.ColumnName != "SubCostHeadID")
                        {
                            number = number + 1;
                            row[column.ColumnName] = "<b><u>" + Convert.ToString(data.AsEnumerable().Where(x => Convert.ToInt64(x["RateCompRateBatchID"]) == Convert.ToInt64(column.ColumnName)).Sum(x => Convert.ToDouble(x["ValueInBaseCurr"]))) + "</b></u>";
                            column.Caption = "Cost Value" + Convert.ToString(number);
                        }


                    }
                    distinctValues.Rows.Add(row);
                    distinctValues.Columns.Remove("RateCompID");
                    distinctValues.Columns.Remove("CostHeadID");
                    distinctValues.Columns.Remove("SubCostHeadID");

                    ds.Tables.AddRange(new DataTable[] { distinctValues, dt1, data });

                }


            }







            return ds;

        }

        public CostUploadFormat GetCostUploadFormat(int RMCID, int ComponentID, string FileType, string CostOrRevenue, string Mode = "ALL")
        {
            try
            {
                return costDAL.GetCostUploadFormat(UserSession.GetUserSession().LoginID, RMCID, ComponentID, FileType, CostOrRevenue, Mode);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "CostBL", "GetCostUploadFormat", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }
        }

        public List<Rate> getSubHistoryRate(int ComponentID, int? RMCID, int? AgentID, int? FromCity, int? ToCity, int? ExitPort, int? EntryPort, char RevnOrCost, Int64 OrgRMCAgentEffectDateID, bool IsJobPage)
        {
            try
            {
                return costDAL.getSubHistoryRate(UserSession.GetUserSession().LoginID, ComponentID, RMCID, AgentID, FromCity, ToCity, ExitPort, EntryPort, RevnOrCost, OrgRMCAgentEffectDateID, IsJobPage);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "CostBL", "getSubHistoryRate", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

        }

        public CityCostRevenue GetCitywiseRevenue(int cityid, int rmcid)
        {
            CityCostRevenue revenue = new CityCostRevenue();
            revenue.CityId = cityid;
            revenue.RMCId = rmcid;

            try
            {
                if (cityid > 0 && rmcid > 0)
                {
                    DataTable dt = costDAL.GetCitywiseRevenue(cityid, rmcid, UserSession.GetUserSession().CompanyID, UserSession.GetUserSession().LoginID);

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        revenue.RMC = Convert.ToString(dt.Rows[0]["RMCName"]);
                        revenue.City = Convert.ToString(dt.Rows[0]["CityName"]);
                        revenue.CityId = Convert.ToInt32(dt.Rows[0]["CityID"]);
                        revenue.EffectiveFrom = dt.Rows[0]["EffectiveFrom"] == DBNull.Value ? System.DateTime.Now : Convert.ToDateTime(dt.Rows[0]["EffectiveFrom"]);
                        revenue.EffectiveTo = System.DateTime.Now;

                        revenue.revenueLists = (from rw in dt.AsEnumerable()
                                                select new CityCostRevenueList()
                                                {
                                                    CostHeadID = Convert.ToInt32(rw["CostHeadID"]),
                                                    CostHead = Convert.ToString(rw["CostHeadName"]),
                                                    Currency = Convert.ToString(rw["CurrencyAbbrvation"]),
                                                    Revenue = Convert.ToDecimal(rw["RevenueVal"]),
                                                    //WtUnitID = Convert.ToInt32(rw["WeightUnitID"])
                                                }
                                                    ).ToList();

                    }

                }
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "CostBL", "GetCitywiseRevenue", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

            return revenue;

        }

        public IEnumerable<HistorySplTHC> getBTRTHCHistory(int? Component, int? RMC, int? Agent, int? DestCity, int? OrgContinent, string sort, string sortdir, int skip, int pageSize, out int totalCount)
        {
            IQueryable<HistorySplTHC> rates;
            try
            {
                rates = costDAL.getBTRTHCHistory(UserSession.GetUserSession().LoginID, Component, RMC, Agent, DestCity, OrgContinent).AsQueryable();
                totalCount = rates.Count();
                if (totalCount > 0)
                {
                    if (pageSize > 1)
                    {
                        rates = rates.Skip((skip * (pageSize - 1))).Take(skip);
                    }
                    else
                    {
                        rates = rates.Take(skip);
                    }
                }
                return rates;
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "CostBL", "getHistoryRate", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }



        }


        public List<SubHistorySplTHC> getSubBTRTHCHistory(int? Component, int? RMC, int? Agent, int? DestCity, int? OrgContinent, Int64? MastTransID)
        {
            try
            {
                return costDAL.getSubBTRTHCHistory(UserSession.GetUserSession().LoginID, Component, RMC, Agent, DestCity, OrgContinent, MastTransID);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "CostBL", "getSubHistoryRate", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

        }


        public DataSet getRMCCompareRate(int ComponentID, int RMCID, int? AgentID, int? City1, int? City2, int? Port1, int? Port2, char RevnOrCost, int? ExitPortAir, int? EntryPortAir)
        {
            DataSet rates = new DataSet();
            try
            {
                DataTable dtRates = costDAL.getRMCCompareRate(UserSession.GetUserSession().LoginID, ComponentID, RMCID, AgentID, City1, City2, Port1, Port2, RevnOrCost, ExitPortAir, EntryPortAir);

                DataTable headerDT = new DataTable("HeaderTbl");
                if (ComponentID == 2 || ComponentID == 1 || ComponentID == 3)
                {

                    string RateCompColumnName = (ComponentID == 2) ? "OrgPortName" : "CityName";
                    headerDT.Columns.AddRange(new[] { new DataColumn(RateCompColumnName, typeof(string)),
                                                  new DataColumn("RateCompID", typeof(int)),
                                                  new DataColumn("Mode", typeof(string)),
                                                  new DataColumn("UniqueId", typeof(int))
                });

                    var CostHead = dtRates.AsEnumerable().Select(r => new
                    {
                        CityName = r.Field<string>(RateCompColumnName),
                        TransportModeName = r.Field<string>("TransportModeName")
                    }).Distinct();
                    int headerCount = 0;
                    foreach (var item in CostHead)
                    {
                        DataRow row = headerDT.NewRow();
                        headerCount = headerCount + 1;
                        row[RateCompColumnName] = item.CityName;
                        row["RateCompID"] = ComponentID;
                        row["Mode"] = item.TransportModeName;
                        row["UniqueId"] = headerCount;
                        headerDT.Rows.Add(row);
                    }
                    //////Header Table
                    rates.Tables.Add(headerDT);
                    foreach (DataRow item in headerDT.Rows)
                    {
                        string UniqueId = Convert.ToString(item["UniqueId"]);
                        DataTable ratesDT = new DataTable("RatesTbl" + UniqueId);
                        ratesDT.Columns.Add("WeightFrom", typeof(Int32));

                        var Weights = dtRates.AsEnumerable().Where(s => Convert.ToString(s[RateCompColumnName]).Equals(Convert.ToString(item[RateCompColumnName]), StringComparison.OrdinalIgnoreCase)

                                                                     && Convert.ToString(s["TransportModeName"]).Equals(Convert.ToString(item["Mode"]), StringComparison.OrdinalIgnoreCase))
                            .Select(r => new
                            {
                                WeightFrom = r.Field<Int32>("WeightFrom")
                            }).Distinct();

                        foreach (var Weight in Weights.ToArray())
                        {
                            DataRow row = ratesDT.NewRow();
                            row["WeightFrom"] = Weight.WeightFrom;
                            ratesDT.Rows.Add(row);
                        }

                        var agentPorts = dtRates.AsEnumerable().Where(s => Convert.ToString(s[RateCompColumnName]).Equals(Convert.ToString(item[RateCompColumnName]), StringComparison.OrdinalIgnoreCase)
                                                                     && Convert.ToString(s["TransportModeName"]).Equals(Convert.ToString(item["Mode"]), StringComparison.OrdinalIgnoreCase))
                            .Select(r => new
                            {
                                AgentPortName = r.Field<string>("PortName") + "/" + r.Field<string>("AgentName"),
                                AgentName = r.Field<string>("AgentName"),
                                PortName = r.Field<string>("PortName")
                            }).Distinct();

                        foreach (var agentPort in agentPorts)
                        {
                            ratesDT.Columns.Add(agentPort.AgentPortName, typeof(float));

                            var AgentPortAmount = dtRates.AsEnumerable().Where(s => Convert.ToString(s[RateCompColumnName]).Equals(Convert.ToString(item[RateCompColumnName]), StringComparison.OrdinalIgnoreCase)
                                                            && Convert.ToString(s["TransportModeName"]).Equals(Convert.ToString(item["Mode"]), StringComparison.OrdinalIgnoreCase)
                                                            && Convert.ToString(s["AgentName"]).Equals(agentPort.AgentName, StringComparison.OrdinalIgnoreCase)
                                                            && Convert.ToString(s["PortName"]).Equals(agentPort.PortName, StringComparison.OrdinalIgnoreCase)
                                                             ).Select(r => new
                                                             {
                                                                 WeightFrom = r.Field<System.Int32?>("WeightFrom"),
                                                                 Amount = r.Field<System.Double?>("Amount")
                                                             }).Distinct();

                            //Type fieldType = ratesDT.Columns["WeightFrom"].DataType;
                            //Type fieldType1 = dtRates.Columns["Amount"].DataType;

                            ratesDT.AsEnumerable().Join(AgentPortAmount,
                             rateAmt => rateAmt.Field<System.Int32>("WeightFrom"),
                             AgentPort => Convert.ToInt32(AgentPort.WeightFrom),
                             (rateAmt, AgentPort) => new
                             {
                                 rateAmt,
                                 AgentPort
                             }).ToList().ForEach(o => o.rateAmt.SetField(agentPort.AgentPortName, o.AgentPort.Amount));

                        }

                        //////Child Tables
                        rates.Tables.Add(ratesDT);
                    }
                }
                else
                {
                    string RateCompColumnName = "CityName";
                    headerDT.Columns.AddRange(new[] { new DataColumn(RateCompColumnName, typeof(string)),
                                                  new DataColumn("DestCityName", typeof(string)),
                                                  new DataColumn("RateCompID", typeof(int)),
                                                  new DataColumn("Mode", typeof(string)),
                                                  new DataColumn("UniqueId", typeof(int))
                    });

                    var CostHead = dtRates.AsEnumerable().Select(r => new
                    {
                        CityName = r.Field<string>(RateCompColumnName),
                        DestCityName = r.Field<string>("DestCityName"),
                        TransportModeName = r.Field<string>("TransportModeName")
                    }).Distinct();
                    int headerCount = 0;
                    foreach (var item in CostHead)
                    {
                        DataRow row = headerDT.NewRow();
                        headerCount = headerCount + 1;
                        row[RateCompColumnName] = item.CityName;
                        row["DestCityName"] = item.DestCityName;
                        row["RateCompID"] = ComponentID;
                        row["Mode"] = item.TransportModeName;
                        row["UniqueId"] = headerCount;
                        headerDT.Rows.Add(row);
                    }
                    //////Header Table
                    rates.Tables.Add(headerDT);
                    foreach (DataRow item in headerDT.Rows)
                    {
                        string UniqueId = Convert.ToString(item["UniqueId"]);
                        DataTable ratesDT = new DataTable("RatesTbl" + UniqueId);
                        ratesDT.Columns.Add("WeightFrom", typeof(Int32));

                        var Weights = dtRates.AsEnumerable().Where(s => Convert.ToString(s[RateCompColumnName]).Equals(Convert.ToString(item[RateCompColumnName]), StringComparison.OrdinalIgnoreCase)
                                                                        && Convert.ToString(s["TransportModeName"]).Equals(Convert.ToString(item["Mode"]), StringComparison.OrdinalIgnoreCase)
                                                                        && Convert.ToString(s["DestCityName"]).Equals(Convert.ToString(item["DestCityName"]), StringComparison.OrdinalIgnoreCase)
                                                                     )
                            .Select(r => new
                            {
                                WeightFrom = r.Field<Int32>("WeightFrom")
                            }).Distinct();

                        foreach (var Weight in Weights.ToArray())
                        {
                            DataRow row = ratesDT.NewRow();
                            row["WeightFrom"] = Weight.WeightFrom;
                            ratesDT.Rows.Add(row);
                        }

                        var agentPorts = dtRates.AsEnumerable().Where(s => Convert.ToString(s[RateCompColumnName]).Equals(Convert.ToString(item[RateCompColumnName]), StringComparison.OrdinalIgnoreCase)
                                                                     && Convert.ToString(s["TransportModeName"]).Equals(Convert.ToString(item["Mode"]), StringComparison.OrdinalIgnoreCase)
                                                                     && Convert.ToString(s["DestCityName"]).Equals(Convert.ToString(item["DestCityName"]), StringComparison.OrdinalIgnoreCase)
                                                                     )
                            .Select(r => new
                            {
                                AgentName = r.Field<string>("AgentName")
                            }).Distinct();

                        foreach (var agentPort in agentPorts)
                        {
                            ratesDT.Columns.Add(agentPort.AgentName, typeof(float));

                            var AgentPortAmount = dtRates.AsEnumerable().Where(s => Convert.ToString(s[RateCompColumnName]).Equals(Convert.ToString(item[RateCompColumnName]), StringComparison.OrdinalIgnoreCase)
                                                            && Convert.ToString(s["TransportModeName"]).Equals(Convert.ToString(item["Mode"]), StringComparison.OrdinalIgnoreCase)
                                                            && Convert.ToString(s["AgentName"]).Equals(agentPort.AgentName, StringComparison.OrdinalIgnoreCase)
                                                            && Convert.ToString(s["DestCityName"]).Equals(Convert.ToString(item["DestCityName"]), StringComparison.OrdinalIgnoreCase)
                                                             ).Select(r => new
                                                             {
                                                                 WeightFrom = r.Field<System.Int32?>("WeightFrom"),
                                                                 Amount = r.Field<System.Double?>("Amount")
                                                             }).Distinct();

                            ratesDT.AsEnumerable().Join(AgentPortAmount,
                             rateAmt => rateAmt.Field<System.Int32>("WeightFrom"),
                             AgentPort => Convert.ToInt32(AgentPort.WeightFrom),
                             (rateAmt, AgentPort) => new
                             {
                                 rateAmt,
                                 AgentPort
                             }).ToList().ForEach(o => o.rateAmt.SetField(agentPort.AgentName, o.AgentPort.Amount));

                        }

                        //////Child Tables
                        rates.Tables.Add(ratesDT);
                    }


                }
                return rates;
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "CostBL", "getCompareRate", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }
        }

        public List<WHServiceCost> GetWHServiceCosts(int LoginID, Int64? SurveyID, Int64? RateCompRateID, Int64? RateCompRateBatchId)
        {
            List<WHServiceCost> WHServiceCostList = new List<WHServiceCost>();
            try
            {
                DataSet WHServiceCostDs = costDAL.GetWHServiceCosts(LoginID, SurveyID, RateCompRateID, RateCompRateBatchId);

                if (WHServiceCostDs != null && WHServiceCostDs.Tables.Count > 0 && WHServiceCostDs.Tables[0].Rows.Count > 0)
                {
                    WHServiceCostList = (from rw in WHServiceCostDs.Tables[0].AsEnumerable()
                                         select new WHServiceCost()
                                         {
                                             Ser_CostHeadRateID = rw["Ser_CostHeadRateID"] == DBNull.Value ? (Int64?)null : Convert.ToInt64(rw["Ser_CostHeadRateID"]),
                                             SurveyID = rw["SurveyID"] == DBNull.Value ? (Int64?)null : Convert.ToInt64(rw["SurveyID"]),
                                             RateCompRateID = RateCompRateID,//rw["RateCompRateWtID"] == DBNull.Value ? (Int64?)null : Convert.ToInt64(rw["RateCompRateWtID"]),
                                             RateCompRateBatchID = RateCompRateBatchId,//rw["RateCompRateIDBatchID"] == DBNull.Value ? (Int64?)null : Convert.ToInt64(rw["RateCompRateIDBatchID"]),
                                             RateCompID = rw["RateCompID"] == DBNull.Value ? (Int64?)null : Convert.ToInt64(rw["RateCompID"]),
                                             RateComp = rw["RateComponentName"] == DBNull.Value ? null : Convert.ToString(rw["RateComponentName"]),
                                             EmpTypeID = rw["EmpTypeID"] == DBNull.Value ? (int?)null : Convert.ToInt32(rw["EmpTypeID"]),
                                             EmpType = rw["EmpTypeName"] == DBNull.Value ? null : Convert.ToString(rw["EmpTypeName"]),
                                             BaseCurrID = rw["BaseCurrID"] == DBNull.Value ? (int?)null : Convert.ToInt32(rw["BaseCurrID"]),
                                             BaseCurr = rw["BaseCurrName"] == DBNull.Value ? null : Convert.ToString(rw["BaseCurrName"]),
                                             BaseCurrConversRate = rw["BaseCurrConversRate"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(rw["BaseCurrConversRate"]),
                                             RateCurrID = rw["RateCurrID"] == DBNull.Value ? (int?)null : Convert.ToInt32(rw["RateCurrID"]),
                                             RateCurr = rw["RateCurrName"] == DBNull.Value ? null : Convert.ToString(rw["RateCurrName"]),
                                             RateCurrValue = rw["RateCurrValue"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(rw["RateCurrValue"]),
                                             WorkHrs = rw["WorkHrs"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(rw["WorkHrs"]),
                                         }).ToList();
                }
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "CostBL", "GetWHServiceCosts", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }
            return WHServiceCostList;
        }

        public bool SaveWHServiceCosts(CostViewModel cost, out string result)
        {
            try
            {
                return costDAL.SaveWHServiceCosts(cost, out result);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "CostBL", "SaveWHServiceCosts", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }
        }
    }
}