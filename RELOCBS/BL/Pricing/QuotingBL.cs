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
    public class QuotingBL
    {

        private QuotingDAL _quotingDAL;

        public QuotingDAL quotingDAL
        {

            get
            {
                if (this._quotingDAL == null)
                    this._quotingDAL = new QuotingDAL();
                return this._quotingDAL;
            }
        }

        public bool InsertQuoting(QuotingViewModel SaveRate, int LoginID, out string result)
        {
            result = String.Empty;

            try
            {
                return quotingDAL.InsertQuoting(SaveRate, LoginID, out result);

            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(LoginID), "QuotingBL", "Insert", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

        }

        public IEnumerable<Quoting> GetForGrid(int LoginID, int RateComponetID, int? page = 1)
        {
            try
            {
                return quotingDAL.GetForGrid(LoginID, RateComponetID, page);

            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(LoginID), "QuotingBL", "Insert", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }


        }
        
        public QuotingViewModel GetDetailById(int? SurveyID = -1, int? RateCompRateWtID = -1, int RateCompRateBatchId = -1)
        {
            QuotingViewModel CostObj = new QuotingViewModel();
            try
            {
                DataSet CostDs = quotingDAL.GetDetailById(SurveyID, RateCompRateWtID, UserSession.GetUserSession().LoginID, RateCompRateBatchId);
                if (CostDs != null && CostDs.Tables.Count >=1)
                {

                    CostObj.CostHeadList = new List<QuotingCostHeadDetail>();
                    CostObj.QuoteApprove = "N";
                    if (CostDs.Tables.Count > 0 && CostDs.Tables[0] != null && CostDs.Tables[0].Rows.Count > 0)
                    {

                        CostObj.SurveyID = Convert.ToInt32(CostDs.Tables[0].Rows[0]["SurveyID"]);
                        CostObj.EnqID = Convert.ToInt32(CostDs.Tables[0].Rows[0]["EnqID"]);
						CostObj.EnqNo = Convert.ToString(CostDs.Tables[0].Rows[0]["EnqNo"]);
						CostObj.EnqDetailID = Convert.ToInt32(CostDs.Tables[0].Rows[0]["EnqDetailID"]);
                        CostObj.RateCompRateWtBatchID = Convert.ToInt32(CostDs.Tables[0].Rows[0]["RateCompRateBatchID"]);
                        CostObj.RateCompRateWtID = Convert.ToInt32(CostDs.Tables[0].Rows[0]["RateCompanyRateWtID"]);
                        CostObj.WeightUnitID = Convert.ToInt32(CostDs.Tables[0].Rows[0]["WeightUnitID"]);
                        CostObj.WeightUnitFrom = Convert.ToInt64(CostDs.Tables[0].Rows[0]["WeightFrom"]);
                        CostObj.WeightUnitTo = Convert.ToInt64(CostDs.Tables[0].Rows[0]["WeightTo"]);
                        CostObj.FromLocationID = Convert.ToInt32(CostDs.Tables[0].Rows[0]["OrgCityID"]);
                        CostObj.ToLocationID = Convert.ToInt32(CostDs.Tables[0].Rows[0]["DestCityID"]);
                        CostObj.ExitPointID = Convert.ToInt32(CostDs.Tables[0].Rows[0]["OrgPortID"]);
                        CostObj.EntryPointID = Convert.ToInt32(CostDs.Tables[0].Rows[0]["DestPortID"]);
                        CostObj.ModeID = Convert.ToInt32(CostDs.Tables[0].Rows[0]["ModeID"]);
                        CostObj.RMCID = Convert.ToInt32(CostDs.Tables[0].Rows[0]["RMCID"]);
                        CostObj.BusinessLineID = Convert.ToInt32(CostDs.Tables[0].Rows[0]["BussinessLineID"]);
                        CostObj.GoodsDescriptionID = Convert.ToInt32(CostDs.Tables[0].Rows[0]["GoodsDescID"]);
                        CostObj.ShipingLineID= CostDs.Tables[0].Rows[0]["ShipinglineID"]==DBNull.Value ? (int?)null : Convert.ToInt32(CostDs.Tables[0].Rows[0]["ShipinglineID"]);

                        CostObj.WeightUnitName = Convert.ToString(CostDs.Tables[0].Rows[0]["WeightUnitName"]);
                        CostObj.FromLocationName = Convert.ToString(CostDs.Tables[0].Rows[0]["OriginCityName"]);
                        CostObj.ToLocationName = Convert.ToString(CostDs.Tables[0].Rows[0]["DestinationCityName"]);

                        CostObj.ExitPointName = Convert.ToString(CostDs.Tables[0].Rows[0]["OriginPortName"]);
                        CostObj.EntryPointName = Convert.ToString(CostDs.Tables[0].Rows[0]["DestinationPortName"]);

                        CostObj.ModeName = Convert.ToString(CostDs.Tables[0].Rows[0]["ModeName"]);
                        CostObj.RMCName = Convert.ToString(CostDs.Tables[0].Rows[0]["RMCName"]);
                        CostObj.ShipingLineName = CostDs.Tables[0].Rows[0]["ShipinglineID"] == DBNull.Value ? "" : Convert.ToString(CostDs.Tables[0].Rows[0]["ShipLineName"]);
                        CostObj.BusinessLineName = Convert.ToString(CostDs.Tables[0].Rows[0]["BusinessLineName"]);
                        CostObj.GoodsDescriptionName = Convert.ToString(CostDs.Tables[0].Rows[0]["GoodsDescName"]);
                        CostObj.ServiceLineName = Convert.ToString(CostDs.Tables[0].Rows[0]["ServiceLine"]);
                        CostObj.ServiceLineID = Convert.ToInt32(CostDs.Tables[0].Rows[0]["ServiceLineID"]);
						
						CostObj.EstimatedBy = Convert.ToString(CostDs.Tables[0].Rows[0]["EstimatedBy"]);
						CostObj.EstimatedDate = Convert.ToString(CostDs.Tables[0].Rows[0]["EstimatedDate"]);
						CostObj.ApprovedBy = Convert.ToString(CostDs.Tables[0].Rows[0]["ApprovedBY"]);
						CostObj.ApprovedDate= Convert.ToString(CostDs.Tables[0].Rows[0]["Approvedate"]);
                        CostObj.EnqDetSequenceID = CostDs.Tables[0].Rows[0]["EnqDetSequenceID"] == DBNull.Value ? (Int32?)null : Convert.ToInt32(CostDs.Tables[0].Rows[0]["EnqDetSequenceID"]);

						CostObj.CostHeadList = (from item in CostDs.Tables[0].AsEnumerable()
                                                select new QuotingCostHeadDetail()
                                                {
													OrderSeq = item["OrderSeq"] == DBNull.Value ? 0 : Convert.ToInt32(item["OrderSeq"]),
													RateComponentRateID = Convert.ToInt32(item["RateCompRateID"]),
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
                                                    Amount = Convert.ToInt64(item["QuoteConvValue"]),
                                                    BaseAmount = Convert.ToInt64(item["QuoteCurrValue"]),
                                                    QuotePercent = Convert.ToDecimal(item["QuoteCurrPercent"]),
                                                    QuotePercentAmount = Convert.ToDecimal(item["QuoteCurrPercentAmt"]),
                                                    TotalAmount = Convert.ToDecimal(item["QuoteCurrTotalAmt"]),
                                                    ConversionRate = Convert.ToInt64(item["BaseCurrConversRate"]),
                                                    TransitTimeFrom = Convert.ToInt32(item["TransTimeFrom"]),
                                                    TransitTimeTo = Convert.ToInt32(item["TransTimeTo"])
                                                }).ToList();
                    }

                    if (CostDs.Tables.Count > 2 && CostDs.Tables[2] != null && CostDs.Tables[2].Rows.Count > 0)
                    {
                        CostObj.QuoteNo = Convert.ToString(CostDs.Tables[2].Rows[0]["QuoteNumber"]);
                        CostObj.QuoteApprove = Convert.ToString(CostDs.Tables[2].Rows[0]["IsQuoteApprove"])=="0" ?"N":"Y" ;
						CostObj.QuoteSentApprove = CostDs.Tables[2].Rows[0]["IsSendToApproval"] == DBNull.Value ? false : Convert.ToBoolean(CostDs.Tables[2].Rows[0]["IsSendToApproval"]); 

					}

                    if (CostDs.Tables.Count > 1 && CostDs.Tables[1] != null && CostDs.Tables[1].Rows.Count > 0)
                    {
                        CostObj.SurveyRateGridDt = (from rw in CostDs.Tables[1].AsEnumerable()
                                                    select new QuotingGrid()
                                                    {
                                                        SurveyID = Convert.ToInt64(rw["SurveyID"]),
                                                        EnqDetailID = Convert.ToInt64(rw["EnqDetailID"]),
                                                        ratecompanyratewtid = Convert.ToInt64(rw["ratecompanyratewtid"]),
                                                        Colour = Convert.ToString(rw["Colour"]),
                                                        FromCity = Convert.ToString(rw["FromCity"]),
                                                        ToCity = Convert.ToString(rw["ToCity"]),
                                                        EntryPort = Convert.ToString(rw["EntryPort"]),
                                                        Exitport = Convert.ToString(rw["Exitport"]),
                                                        TotEstimate = Convert.ToInt64(rw["TotEstimate"]),
                                                        WeightFrom = Convert.ToInt64(rw["WeightFrom"]),
                                                        WeightTo = Convert.ToInt64(rw["WeightTo"]),
                                                        UseForJob = rw["UseForJob"]==DBNull.Value ? (bool?)null : Convert.ToBoolean(rw["UseForJob"]),
                                                        UsedForJob = rw["UsedForJob"] == DBNull.Value ? (bool?)null : Convert.ToBoolean(rw["UsedForJob"]),
                                                        ratecompratebatchid = Convert.ToInt64(rw["RateCompRateBatchID"])
                                                        ,TotQuote= Convert.ToInt32(rw["TotQuote"])
                                                        ,QuoteNo= CostObj.QuoteNo
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
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "QuotingBL", "GetDetailById", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

            return CostObj;
        }

        public bool UpdateUseForJobStatus(int SurveyID,int RateCompRateWtID, int LoginID, int RateCompRateBatchID,out string message)
        {
            message = String.Empty;

            try
            {
                return quotingDAL.UpdateUseForJobStatus(SurveyID, RateCompRateWtID, LoginID, RateCompRateBatchID,out message);

            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(LoginID), "QuotingBL", "UpdateUseForJobStatus", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }
        }

        public bool Delete(int SurveyID,int RateCompRateWtID, int RateCompRateBatchID, int LoginID, out string message)
        {
            message = String.Empty;

            try
            {
                return quotingDAL.Delete(SurveyID, RateCompRateWtID, RateCompRateBatchID, LoginID, out message);

            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(LoginID), "QuotingBL", "Delete", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }


        }

        public bool InsertQuotingPrint(QuotePrint print, int LoginID, out string result)
        {
            result = String.Empty;

            try
            {
                return quotingDAL.InsertQuotingPrint(print, LoginID, out result);

            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(LoginID), "QuotingBL", "InsertQuotingPrint", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

        }

		public QuotePrint GetQuotingPrintDetail(int? SurveyID = -1, int? RateCompRateWtID = -1, int? RateCompRateWtBatchID = -1, bool? IsLumsum = false)
		{
            QuotePrint CostObj = new QuotePrint();
            try
            {
				DataSet CostDs = quotingDAL.GetQuotingPrintDetail(SurveyID, RateCompRateWtID, RateCompRateWtBatchID, IsLumsum, UserSession.GetUserSession().CompanyID, UserSession.GetUserSession().LoginID);
				CostObj.SurveyID = SurveyID;
                CostObj.RateCompRateWtID = Convert.ToInt32(RateCompRateWtID);
                if (CostDs != null && CostDs.Tables.Count >= 1)
                {
                    if (CostDs.Tables.Count > 0 && CostDs.Tables[0] != null && CostDs.Tables[0].Rows.Count > 0)
                    {
                        CostObj.QuoteSendToID = string.IsNullOrEmpty(Convert.ToString(CostDs.Tables[0].Rows[0]["QuoteSendToID"])) ? 0 : Convert.ToInt64(Convert.ToString(CostDs.Tables[0].Rows[0]["QuoteSendToID"]));
                        CostObj.QuoteTo = Convert.ToString(CostDs.Tables[0].Rows[0]["QouteTo"]);
                        CostObj.ClientID = Convert.ToInt32(Convert.ToString(CostDs.Tables[0].Rows[0]["ClientID"]));
                        CostObj.AccountID = Convert.ToInt32(Convert.ToString(CostDs.Tables[0].Rows[0]["AccountID"]));

                        CostObj.AcctAddrs1 = Convert.ToString(CostDs.Tables[0].Rows[0]["AcctAddrs1"]);
                        CostObj.AcctAddrs2 = Convert.ToString(CostDs.Tables[0].Rows[0]["AcctAddrs2"]);
                        CostObj.AcctAddrs3 = Convert.ToString(CostDs.Tables[0].Rows[0]["AcctAddrs3"]);
                        CostObj.AcctCityID = string.IsNullOrEmpty(Convert.ToString(CostDs.Tables[0].Rows[0]["AcctCityID"])) ? 0 : Convert.ToInt32(Convert.ToString(CostDs.Tables[0].Rows[0]["AcctCityID"]));
                        CostObj.AcctZip = Convert.ToString(CostDs.Tables[0].Rows[0]["AcctZip"]);
                        CostObj.ClientAddrs1 = Convert.ToString(CostDs.Tables[0].Rows[0]["ClientAddrs1"]);
                        CostObj.ClientAddrs2 = Convert.ToString(CostDs.Tables[0].Rows[0]["ClientAddrs2"]);
                        CostObj.ClientAddrs3 = Convert.ToString(CostDs.Tables[0].Rows[0]["ClientAddrs3"]);
						CostObj.ClientCityID = string.IsNullOrEmpty(Convert.ToString(CostDs.Tables[0].Rows[0]["ClientCityID"])) ? 0 : Convert.ToInt32(Convert.ToString(CostDs.Tables[0].Rows[0]["ClientCityID"]));
                        CostObj.ClientZip = Convert.ToString(CostDs.Tables[0].Rows[0]["ClientZip"]);
                        CostObj.ShipperAddrs1 = Convert.ToString(CostDs.Tables[0].Rows[0]["ShipperAddrs1"]);
                        CostObj.ShipperAddrs2 = Convert.ToString(CostDs.Tables[0].Rows[0]["ShipperAddrs2"]);
                        
                        CostObj.ShipperCityID = string.IsNullOrEmpty(Convert.ToString(CostDs.Tables[0].Rows[0]["ShipperCityID"])) ? 0 : Convert.ToInt32(Convert.ToString(CostDs.Tables[0].Rows[0]["ShipperCityID"]));
                        CostObj.ShipperZip = Convert.ToString(CostDs.Tables[0].Rows[0]["shipperPin"]);

                        
                        if (!string.IsNullOrWhiteSpace(CostObj.QuoteTo) && CostObj.QuoteTo.ToUpper()=="CLIENT")
                        {  
                           CostObj.ClientID = Convert.ToInt32(Convert.ToString(CostDs.Tables[0].Rows[0]["AccountClientID"]));
                        }
                        else if (!string.IsNullOrWhiteSpace(CostObj.QuoteTo) && CostObj.QuoteTo.ToUpper() == "CORPORATE")
                        {
                           CostObj.AccountID = Convert.ToInt32(Convert.ToString(CostDs.Tables[0].Rows[0]["AccountClientID"]));
                        }

                        CostObj.Shipper_Title = Convert.ToString(CostDs.Tables[0].Rows[0]["QtTitle"]);
                        CostObj.ShipperFName = Convert.ToString(CostDs.Tables[0].Rows[0]["FName"]);
                        CostObj.ShipperLName = Convert.ToString(CostDs.Tables[0].Rows[0]["LName"]);
                        CostObj.Address1= Convert.ToString(CostDs.Tables[0].Rows[0]["QtAddress1"]);
                        CostObj.Address2 = Convert.ToString(CostDs.Tables[0].Rows[0]["QtAddress2"]);
                        CostObj.Address3 = Convert.ToString(CostDs.Tables[0].Rows[0]["QtAddress3"]);
                        CostObj.Zip = Convert.ToString(CostDs.Tables[0].Rows[0]["ZIp_Pin"]);
                        CostObj.City = !string.IsNullOrWhiteSpace(Convert.ToString(CostDs.Tables[0].Rows[0]["Cityid"]))? Convert.ToInt32(Convert.ToString(CostDs.Tables[0].Rows[0]["Cityid"])) : (int?)null;

                        CostObj.QuoteIntro = Convert.ToString(CostDs.Tables[0].Rows[0]["QtIntroduction"]);
                        CostObj.Attention = Convert.ToString(CostDs.Tables[0].Rows[0]["QtAttentionTitle"]);
                        CostObj.AttentionName = Convert.ToString(CostDs.Tables[0].Rows[0]["QtAttention"]);
                        CostObj.Insurance = Convert.ToString(CostDs.Tables[0].Rows[0]["Insurance"]);
                        CostObj.Remarks = Convert.ToString(CostDs.Tables[0].Rows[0]["Remarks"]);
                        CostObj.Subject = Convert.ToString(CostDs.Tables[0].Rows[0]["QtSubject"]);
                        CostObj.SentBy = !string.IsNullOrWhiteSpace(Convert.ToString(CostDs.Tables[0].Rows[0]["SentByEmpID"])) ? Convert.ToInt32(Convert.ToString(CostDs.Tables[0].Rows[0]["SentByEmpID"])):0;
						CostObj.QuotedExchange_rate = !string.IsNullOrWhiteSpace(Convert.ToString(CostDs.Tables[0].Rows[0]["Qt_ExchangeRate"])) ? Convert.ToDouble(CostDs.Tables[0].Rows[0]["Qt_ExchangeRate"]) : 0;
						CostObj.Quoted_Curr = !string.IsNullOrWhiteSpace(Convert.ToString(CostDs.Tables[0].Rows[0]["Qt_Curr"])) ? Convert.ToInt32(CostDs.Tables[0].Rows[0]["Qt_Curr"]) : 0;
						CostObj.Estimated_BaseCurr = !string.IsNullOrWhiteSpace(Convert.ToString(CostDs.Tables[0].Rows[0]["BaseCurrID"])) ? Convert.ToInt32(CostDs.Tables[0].Rows[0]["BaseCurrID"]) : 0;
					}

                    if (CostDs.Tables.Count > 0 && CostDs.Tables[1] != null && CostDs.Tables[1].Rows.Count > 0) {

                        CostObj.CostHeadList =
                            (from rw in CostDs.Tables[1].AsEnumerable()
                             select new CostHeadDetail()
                             {
                                 RateCompRateID = string.IsNullOrEmpty(Convert.ToString(rw["RateCompRateID"])) ? (Int64?)null :  Convert.ToInt64(rw["RateCompRateID"]),
                                 CostHeadID = Convert.ToInt32(rw["CostHeadID"]),
                                 RateComponentID = Convert.ToInt32(rw["CompID"]),
                                 CostHeadName = Convert.ToString(rw["CostHeadName"]),
                                 CostHeadDescription = Convert.ToString(rw["InfoForDisplay"]),
                                 Checked = (!string.IsNullOrEmpty(Convert.ToString(rw["IsSaved"])) && Convert.ToString(rw["IsSaved"]).ToUpper() == "YES") ? true : false,
                                 Amount = Convert.ToInt64(rw["QuoteCurrValue"]),
                             }).ToList();
                    }

                    if (CostDs.Tables.Count > 1 && CostDs.Tables[2] != null && CostDs.Tables[2].Rows.Count > 0) {

                        CostObj.QuoteTermsList =
                            (from rw in CostDs.Tables[2].AsEnumerable().Where(a => Convert.ToString(a["TermsInclusiveExclusive"]).ToUpper().Equals("T"))
                             select new QuoteTerm()
                             {
                                 TermID = Convert.ToInt32(rw["SurveyID"]),
								 TermName = Convert.ToString(rw["MasterToDisplay"]),
								 TermDescription = Convert.ToString(rw["MsgToDisplay"]),
                                 Type = Convert.ToString(rw["TermsInclusiveExclusive"]),
                                 Checked = (!string.IsNullOrEmpty(Convert.ToString(rw["IsSaved"])) && Convert.ToString(rw["IsSaved"]).ToUpper() == "YES") ? true : false

                             }).ToList();

                        CostObj.InclusionList =
                            (from rw in CostDs.Tables[2].AsEnumerable().Where(a => Convert.ToString(a["TermsInclusiveExclusive"]).ToUpper().Equals("I"))
                             select new QuoteInclusionExclusion()
                             {
                                 CostHeadID = Convert.ToInt32(rw["SurveyID"]),
								 CostHeadName = Convert.ToString(rw["MasterToDisplay"]),
								 CostHeadDescription = Convert.ToString(rw["MsgToDisplay"]),
                                 Type = Convert.ToString(rw["TermsInclusiveExclusive"]),
                                 Checked = (!string.IsNullOrEmpty(Convert.ToString(rw["IsSaved"])) && Convert.ToString(rw["IsSaved"]).ToUpper() == "YES") ? true : false

                             }).ToList();

                        CostObj.ExclusionList =
                            (from rw in CostDs.Tables[2].AsEnumerable().Where(a => Convert.ToString(a["TermsInclusiveExclusive"]).ToUpper().Equals("E"))
                             select new QuoteInclusionExclusion()
                             {
                                 CostHeadID = Convert.ToInt32(rw["SurveyID"]),
								 CostHeadName = Convert.ToString(rw["MasterToDisplay"]),
								 CostHeadDescription = Convert.ToString(rw["MsgToDisplay"]),
                                 Type = Convert.ToString(rw["TermsInclusiveExclusive"]),
                                 Checked = (!string.IsNullOrEmpty(Convert.ToString(rw["IsSaved"])) && Convert.ToString(rw["IsSaved"]).ToUpper() == "YES") ? true : false

                             }).ToList();

                    }

                    if (CostDs.Tables.Count > 3 && CostDs.Tables[3] != null && CostDs.Tables[3].Rows.Count > 0)
                    {
                        CostObj.QuoteNo = Convert.ToString(CostDs.Tables[3].Rows[0]["QuoteNumber"]);
                        CostObj.QuoteApprove = "Y";
                    }
                }
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "QuotingBL", "GetQuotingPrint", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

            return CostObj;

        }

        public DataSet GetCompareQuote(int SurveyID, int LoginID)
        {
            DataSet ds = new DataSet();
            //dt1.Columns.Add("CompareDesc");
            //dt1.Rows.Add("ACWT");
            //dt1.Rows.Add("Exit Port");
            //dt1.Rows.Add("Shipping Line");
            //dt1.Rows.Add("Dest. Agent");
            //dt1.Rows.Add("Entry Port");
            DataTable data = quotingDAL.GetCompareQuote(SurveyID, LoginID);
            DataView view = new DataView(data);
            DataTable distinctValues = view.ToTable("dtComponent", true, new string[] { "Component" });
            string col = string.Empty;
            if (data != null && data.Rows.Count > 0)
            {
                foreach (DataRow item in data.Rows)
                {
                    col = item["RateCompanyRateWtID"].ToString();
                    if (!distinctValues.Columns.Contains(col))
                    {
                        distinctValues.Columns.Add(col);
                    }

                    distinctValues.AsEnumerable().Where(s => Convert.ToString(s["Component"]).Equals(item["Component"].ToString())).ToList()
                    .ForEach(D => D.SetField(col, item["ValueInBaseCurr"].ToString()));
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
                    dt1.TableName = "dtDescription";
                    dt1.Rows.Add("Goods Description");
                    dt1.Rows.Add("ACWT");
                    dt1.Rows.Add("Exit Port");
                    dt1.Rows.Add("Shipping Line");
                    dt1.Rows.Add("Org. Agent");
                    dt1.Rows.Add("Dest. Agent");
                    dt1.Rows.Add("Entry Port");

                    foreach (DataRow item in data.Rows)
                    {
                        col = item["RateCompanyRateWtID"].ToString();

                        if (item["RateCompID"].ToString() == "1")
                        {
                            dt1.AsEnumerable().Where(s => Convert.ToString(s["Component"]).Equals("Goods Description")).ToList()
                            .ForEach(D => D.SetField(col, item["GoodsDescName"].ToString()));

                            dt1.AsEnumerable().Where(s => Convert.ToString(s["Component"]).Equals("ACWT")).ToList()
                            .ForEach(D => D.SetField(col, item["ACWT"].ToString()));

                            dt1.AsEnumerable().Where(s => Convert.ToString(s["Component"]).Equals("Exit Port")).ToList()
                            .ForEach(D => D.SetField(col, item["ExitPortName"].ToString()));

                            dt1.AsEnumerable().Where(s => Convert.ToString(s["Component"]).Equals("Entry Port")).ToList()
                            .ForEach(D => D.SetField(col, item["EntryPortName"].ToString()));

                            dt1.AsEnumerable().Where(s => Convert.ToString(s["Component"]).Equals("Org. Agent")).ToList()
                            .ForEach(D => D.SetField(col, item["AgentName"].ToString()));
                        }
                        else if (item["RateCompID"].ToString() == "2")
                        {

                            dt1.AsEnumerable().Where(s => Convert.ToString(s["Component"]).Equals("Shipping Line")).ToList()
                            .ForEach(D => D.SetField(col, item["ShippingLine"].ToString()));

                        }
                        else
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
                        row[column.ColumnName] = "Total";
                        column.Caption = "Cost Description";
                    }
                    else
                    {
                        row[column.ColumnName] = distinctValues.AsEnumerable().Sum(x => Convert.ToInt32(x[column.ColumnName]));
                        column.Caption = "Cost Value" + Convert.ToString(number);
                    }

                    number = number + 1;
                }
                distinctValues.Rows.Add(row);


                ds.Tables.AddRange(new DataTable[] { distinctValues, dt1, data });

            }

            return ds;

        }

        public bool ApproveQuote(int SurveyID,bool IsRemoveApproval, bool QuoteSentApprove, int? QuoteSenttoApproveUser, int LoginID, int RateCompRateBatchID,out string message)
        {
            message = String.Empty;

            try
            {
                return quotingDAL.ApproveQuote(SurveyID, IsRemoveApproval, QuoteSentApprove, QuoteSenttoApproveUser, LoginID, RateCompRateBatchID, out message);

            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(LoginID), "QuotingBL", "ApproveQuote", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }


        }

        public  QouteHtmlPrint GetQuotingPrint(int? SurveyID, int? RateCompRateWtID, int? RateCompRateWtBatchID, bool? IsLumsum,Int16 IsEmail)
        {
            var User = UserSession.GetUserSession();
            QouteHtmlPrint print = new QouteHtmlPrint();
            try
            {
                DataSet printDs = quotingDAL.GetQuotingPrint(SurveyID, RateCompRateWtID, RateCompRateWtBatchID, IsLumsum, User.CompanyID, User.LoginID, IsEmail);
                if (printDs.Tables.Count > 0 && printDs.Tables[0] != null && printDs.Tables[0].Rows.Count > 0)
                {

                    print = (from rw in printDs.Tables[0].AsEnumerable()
                             select new QouteHtmlPrint()
                             {
                                 SurveyID = Convert.ToInt32(rw["SurveyID"]),
                                 RateCompRateWtID = Convert.ToInt32(rw["RateCompRateWtID"]),
                                 QouteTo = Convert.ToString(rw["QouteTo"]),
                                 QtIntroduction = Convert.ToString(rw["QtIntroduction"]),
                                 QtTitle = Convert.ToString(rw["QtTitle"]),
                                 FName = Convert.ToString(rw["FName"]),
                                 LName = Convert.ToString(rw["LName"]),
                                 QtAttentionTitle = Convert.ToString(rw["QtAttentionTitle"]),
                                 QtAttention = Convert.ToString(rw["QtAttention"]),
                                 Insurance = Convert.ToString(rw["Insurance"]),
                                 Remarks = Convert.ToString(rw["Remarks"]),
                                 QtAddress1 = Convert.ToString(rw["QtAddress1"]),
                                 QtAddress2 = Convert.ToString(rw["QtAddress2"]),
                                 QtAddress3 = Convert.ToString(rw["QtAddress3"]),
                                 Cityid = Convert.ToString(rw["Cityid"]),
                                 ZIp_Pin = Convert.ToString(rw["ZIp_Pin"]),
                                 QtCityName = Convert.ToString(rw["QtCityName"]),
                                 SentBy = Convert.ToString(rw["SentBy"]),
                                 SentByDesign = Convert.ToString(rw["SentByDesign"]),
                                 QtSubject = Convert.ToString(rw["QtSubject"]),
                                 Line1 = Convert.ToString(rw["Line1"]),
                                 Line2 = Convert.ToString(rw["Line2"]),
                                 Line3 = Convert.ToString(rw["Line3"]),
                                 QuoteNo = Convert.ToString(rw["QuoteNo"]),
                                 Annotation = Convert.ToString(rw["Annotation"]),
                                 IsEmail = Convert.ToInt32(rw["IsEmail"]),

                             }).FirstOrDefault();


                    if (printDs.Tables.Count>1 && printDs.Tables[1]!=null)
                    {
                        print.QouteCostHeads = (from rw in printDs.Tables[1].AsEnumerable()
                                                select new QouteCostHeadPrint()
                                                {
                                                    CostHeadID = Convert.ToInt32(rw["CostHeadID"]),
                                                    CostHeadName = Convert.ToString(rw["CostHeadName"]),
                                                    InfoForDisplay = Convert.ToString(rw["InfoForDisplay"]),
                                                    ConvCurrValue = Convert.ToString(rw["ConvCurrValue"]),
                                                    QuoteCurrValue = Convert.ToString(rw["QuoteCurrValue"]),
                                                    IsSaved = Convert.ToString(rw["IsSaved"]),
                                                }).ToList();

                    }

                    if (printDs.Tables.Count > 2 && printDs.Tables[2] != null)
                    {
                        print.qouteTerms = (from rw in printDs.Tables[2].AsEnumerable()
                                                select new QouteTermsPrint()
                                                {
                                                    //Amount = Convert.ToString(rw["Amount"]),
                                                    MasterToDisplay = Convert.ToString(rw["MasterToDisplay"]),
                                                    MsgToDisplay = Convert.ToString(rw["MsgToDisplay"]),
                                                    TermsInclusiveExclusive = Convert.ToString(rw["TermsInclusiveExclusive"]),
                                                    IsSaved = Convert.ToString(rw["IsSaved"]),
                                                }).ToList();

                    }

                    if (printDs.Tables.Count > 3 && printDs.Tables[3] != null)
                    {
                        print.exclusive = (from rw in printDs.Tables[3].AsEnumerable()
                                            select new QouteInclusiveExclusivePrint()
                                            {
                                                MasterToDisplay = Convert.ToString(rw["MasterToDisplay"]),
                                                MsgToDisplay = Convert.ToString(rw["MsgToDisplay"]),
                                                TermsInclusiveExclusive = Convert.ToString(rw["TermsInclusiveExclusive"]),
                                                IsSaved = Convert.ToString(rw["IsSaved"]),
                                                
                                            }).FirstOrDefault();

                    }


                    if (printDs.Tables.Count > 4 && printDs.Tables[4] != null)
                    {
                        print.inclusive = (from rw in printDs.Tables[4].AsEnumerable()
                                           select new QouteInclusiveExclusivePrint()
                                           {
                                               MasterToDisplay = Convert.ToString(rw["MasterToDisplay"]),
                                               MsgToDisplay = Convert.ToString(rw["MsgToDisplay"]),
                                               TermsInclusiveExclusive = Convert.ToString(rw["TermsInclusiveExclusive"]),
                                               IsSaved = Convert.ToString(rw["IsSaved"]),

                                           }).FirstOrDefault();

                    }


                }
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(User.LoginID), "QuotingBL", "GetQuotingPrint", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

            return print;

        }
        
    }
}