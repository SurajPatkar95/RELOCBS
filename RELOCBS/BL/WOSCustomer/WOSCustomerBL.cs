using RELOCBS.Common.ExceptionHandling;
using RELOCBS.DAL.WOSCustomer;
using RELOCBS.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Dynamic;

namespace RELOCBS.BL.WOSCustomer
{
    public class WOSCustomerBL
    {
        private WOSCustomerDAL _WOSCustomerDAL;
        public WOSCustomerDAL WOSCustomerDAL
        {
            get
            {
                if (_WOSCustomerDAL == null)
                    _WOSCustomerDAL = new WOSCustomerDAL();
                return _WOSCustomerDAL;
            }
        }

        public IEnumerable<Entities.WOSCustomer> GetCustomerServiceMapping(string Sort, string SortDir, int Skip, int PageSize, string CustomerName, bool? IsRMC, int? RMCID, int? ClientID,
            int? AccountID, DateTime? EffectiveFrom, out int TotalCount)
        {
            IQueryable<Entities.WOSCustomer> CustomerServiceMapList = null;
            TotalCount = 0;
            int LoginID = UserSession.GetUserSession().LoginID;

            try
            {
                DataSet CustomerServiceMapDs = WOSCustomerDAL.GetCustomerServiceMapping(LoginID, CustomerName, IsRMC, RMCID, ClientID, AccountID, EffectiveFrom);

                if (CustomerServiceMapDs != null && CustomerServiceMapDs.Tables.Count > 0 && CustomerServiceMapDs.Tables[0].Rows.Count > 0)
                {
                    var result = (from rw in CustomerServiceMapDs.Tables[0].AsEnumerable()
                                  select new Entities.WOSCustomer()
                                  {
                                      CustServMapMasterID = rw["CustServMapMasterID"] == DBNull.Value ? 0 : Convert.ToInt32(rw["CustServMapMasterID"]),
                                      EffectiveFrom = rw["EffectiveFrom"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(rw["EffectiveFrom"]),
                                      EffectiveTo = rw["EffectiveTo"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(rw["EffectiveTo"]),
                                      Client = rw["Client"] == DBNull.Value ? null : Convert.ToString(rw["Client"]),
                                      Account = rw["Account"] == DBNull.Value ? null : Convert.ToString(rw["Account"]),
                                      OriginCountry = rw["OrgCountry"] == DBNull.Value ? null : Convert.ToString(rw["OrgCountry"]),
                                      DestinationCountry = rw["DestCountry"] == DBNull.Value ? null : Convert.ToString(rw["DestCountry"]),
                                      CostCurrency = rw["CostCurr"] == DBNull.Value ? null : Convert.ToString(rw["CostCurr"]),
                                      RevenueCurrency = rw["RevCurr"] == DBNull.Value ? null : Convert.ToString(rw["RevCurr"]),
                                      IsActive = rw["Isactive"] == DBNull.Value ? false : Convert.ToBoolean(rw["Isactive"])
                                  }).ToList();
                    CustomerServiceMapList = result.AsQueryable();

                    TotalCount = CustomerServiceMapList.Count();
                    CustomerServiceMapList = CustomerServiceMapList.OrderBy(Sort + " " + SortDir);
                    if (PageSize > 0)
                    {
                        CustomerServiceMapList = CustomerServiceMapList.Skip(Skip).Take(PageSize);
                    }
                    return CustomerServiceMapList.ToList();
                }
                else
                {
                    return new List<Entities.WOSCustomer>();
                }
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "WOSCustomerBL", "GetCustomerServiceMapping", Properties.Resources.UnExpectedErrorAtBL, ex);
            }
        }

        public Entities.WOSCustomer GetClientServiceMapingDetailsById(Int64 CustServMapMasterID)
        {
            Entities.WOSCustomer CustomerServiceMapObj = new Entities.WOSCustomer();
            int LoginID = UserSession.GetUserSession().LoginID;

            try
            {
                DataSet CustomerServiceMapDs = WOSCustomerDAL.GetClientServiceMapingDetailsById(LoginID, CustServMapMasterID);

                if (CustomerServiceMapDs != null)
                {
                    if (CustomerServiceMapDs.Tables.Count > 0 && CustomerServiceMapDs.Tables[0].Rows.Count > 0)
                    {
                        CustomerServiceMapObj = (from rw in CustomerServiceMapDs.Tables[0].AsEnumerable()
                                                 select new Entities.WOSCustomer()
                                                 {
                                                     CustServMapMasterID = rw["CustServMapMasterID"] == DBNull.Value ? 0 : Convert.ToInt32(rw["CustServMapMasterID"]),
                                                     EffectiveFrom = rw["EffectiveFrom"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(rw["EffectiveFrom"]),
                                                     EffectiveTo = rw["EffectiveTo"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(rw["EffectiveTo"]),
                                                     IsRMC = rw["ISRMC"] == DBNull.Value ? false : Convert.ToBoolean(rw["ISRMC"]),
                                                     ClientID = rw["ClientID"] == DBNull.Value ? (Int64?)null : Convert.ToInt64(rw["ClientID"]),
                                                     Client = rw["Client"] == DBNull.Value ? null : Convert.ToString(rw["Client"]),
                                                     AccountID = rw["AccountID"] == DBNull.Value ? (Int64?)null : Convert.ToInt64(rw["AccountID"]),
                                                     Account = rw["Account"] == DBNull.Value ? null : Convert.ToString(rw["Account"]),
                                                     OriginCountryID = rw["OrgCountryID"] == DBNull.Value ? (int?)null : Convert.ToInt32(rw["OrgCountryID"]),
                                                     OriginCountry = rw["OrgCountry"] == DBNull.Value ? null : Convert.ToString(rw["OrgCountry"]),
                                                     DestinationCountryID = rw["DestCountryID"] == DBNull.Value ? (int?)null : Convert.ToInt32(rw["DestCountryID"]),
                                                     DestinationCountry = rw["DestCountry"] == DBNull.Value ? null : Convert.ToString(rw["DestCountry"]),
                                                     RevenueCurrencyID = rw["RevenueCurrID"] == DBNull.Value ? (int?)null : Convert.ToInt32(rw["RevenueCurrID"]),
                                                     RevenueCurrency = rw["RevenueCurr"] == DBNull.Value ? null : Convert.ToString(rw["RevenueCurr"]),
                                                     CostCurrencyID = rw["CostCurrID"] == DBNull.Value ? (int?)null : Convert.ToInt32(rw["CostCurrID"]),
                                                     CostCurrency = rw["CostCurr"] == DBNull.Value ? null : Convert.ToString(rw["CostCurr"])
                                                 }).First();
                    }
                    if (CustomerServiceMapDs.Tables.Count > 1 && CustomerServiceMapDs.Tables[1].Rows.Count > 0)
                    {
                        CustomerServiceMapObj.WOSSubServiceList = CustomerServiceMapDs.Tables[1].AsEnumerable().
                            Select(rw => new Entities.WOSSubService
                            {
                                SubServiceMastID = rw["SubServiceMastID"] == DBNull.Value ? 0 : Convert.ToInt32(rw["SubServiceMastID"]),
                                ServiceMastID = rw["ServiceMastID"] == DBNull.Value ? 0 : Convert.ToInt32(rw["ServiceMastID"]),
                                ServiceName = rw["ServiceName"] == DBNull.Value ? null : Convert.ToString(rw["ServiceName"]),
                                SubServiceName = rw["SubServiceName"] == DBNull.Value ? null : Convert.ToString(rw["SubServiceName"]),
                                MastCostAmount = rw["CostAmt"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(rw["CostAmt"]),
                                MastRevenueAmount = rw["RevenueAmt"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(rw["RevenueAmt"]),
                                IsChecked = rw["IsChecked"] == DBNull.Value ? false : Convert.ToBoolean(rw["IsChecked"]),
                                SrNo = rw["DispOrder"] == DBNull.Value ? 0 : Convert.ToInt32(rw["DispOrder"])
                            }).ToList();
                    }
                }
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "WOSCustomerBL", "GetClientServiceMapingDetailsById", Properties.Resources.UnExpectedErrorAtBL, ex);
            }
            return CustomerServiceMapObj;
        }

        public bool SaveCustomerServiceMap(Entities.WOSCustomer WOSCustomer, out string result)
        {
            int LoginID = UserSession.GetUserSession().LoginID;
            try
            {
                return WOSCustomerDAL.SaveCustomerServiceMap(WOSCustomer, LoginID, out result);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(LoginID), "WOSCustomerBL", "SaveCustomerServiceMap", Properties.Resources.UnExpectedErrorAtBL, ex);
            }
        }
    }
}