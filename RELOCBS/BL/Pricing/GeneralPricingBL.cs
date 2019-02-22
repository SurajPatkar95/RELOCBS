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
    public class GeneralPricingBL
    {

        private GeneralPricingDAL _generalPricingDAL;

        public GeneralPricingDAL  generalPricingDAL
        {

            get
            {
                if (this._generalPricingDAL == null)
                    this._generalPricingDAL = new GeneralPricingDAL();
                return this._generalPricingDAL;
            }
        }

        public bool InsertRate(GeneralPriceViewModel SaveRate, int LoginID,out string result)
        {
            result = String.Empty;

            try
            {
                return generalPricingDAL.InsertRate(SaveRate, LoginID,out result);
                
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(LoginID), "GeneralPricingBL", "Insert", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }
            
        }

        public IEnumerable<GeneralPrice> GetForGrid(int LoginID,int RateComponetID, int? page = 1)
        {
            try
            {
                return generalPricingDAL.GetForGrid( LoginID, RateComponetID, page);

            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(LoginID), "GeneralPricingBL", "Insert", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }


        }

        public void GetRateComponentLable(int RateComponentID,out String FromLoc,out string ToLoc)
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

        public GeneralPriceViewModel GetDetailById(int RateCompRateWtBatchID,int RateCompRateWtID)
        {
            GeneralPriceViewModel CostObj = new GeneralPriceViewModel();
            try
            {
                DataSet CostDs = generalPricingDAL.GetDetailById(RateCompRateWtBatchID, RateCompRateWtID, UserSession.GetUserSession().LoginID);
                if (CostDs != null && CostDs.Tables.Count > 0)
                {

                    CostObj.CostHeadList = new List<CostHead>();

                    CostObj = (from rw in CostDs.Tables[1].AsEnumerable()
                               select new GeneralPriceViewModel()
                               {
                                   WeightUnitFrom = Convert.ToInt64(rw["WeightFrom"]),
                                   WeightUnitTo = Convert.ToInt64(rw["WeightTo"]),
                                   RateCurrencyName = Convert.ToString(rw["RateCurr"]),
                                   BaseCurrencyRateName = Convert.ToString(rw["BaseCurr"]),
                                   ConversionRate = Convert.ToUInt64(rw["BaseCurrConversRate"]),
                                   FromLocationName = Convert.ToString(rw["FromLoc"]),
                                   ToLocationName = Convert.ToString(rw["ToLoc"]),
                                   RMCName = Convert.ToString(rw["RMCName"]),
                                   AgentName = Convert.ToString(rw["AgentName"]),
                                   BusinessLineName = Convert.ToString(rw["AgentName"]),
                                   GoodsDescriptionName = Convert.ToString(rw["GoodsDescName"]),
                                   WeightUnitName = Convert.ToString(rw["WeightUnitName"])

                               }).First();

                    foreach (DataRow item in CostDs.Tables[0].Rows)
                    {

                        CostHead costHead = new CostHead();

                        costHead.CostHeadID = Convert.ToInt32(item["CostHeadID"]);
                        costHead.CostHeadName = Convert.ToString(item["CostHeadName"]);
                        costHead.Amount = Convert.ToInt64(item["RateCurrValue"]);
                        CostObj.CostHeadList.Add(costHead);
                    }

                    CostObj.RateCompRateWtBatchID = RateCompRateWtBatchID;
                    CostObj.RateCompRateWtID = RateCompRateWtID;
                    
                }
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "CityBL", "GetDetailById", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

            return CostObj;
        }
    }
}