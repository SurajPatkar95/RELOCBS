using RELOCBS.Common.ExceptionHandling;
using RELOCBS.DAL.WOSCommon;
using RELOCBS.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Dynamic;

namespace RELOCBS.BL.WOSCommon
{
    public class WOSSubServiceBL
    {
        private WOSSubServiceDAL _WOSSubServiceDAL;
        public WOSSubServiceDAL WOSSubServiceDAL
        {
            get
            {
                if (_WOSSubServiceDAL == null)
                    _WOSSubServiceDAL = new WOSSubServiceDAL();
                return _WOSSubServiceDAL;
            }
        }

        public IEnumerable<Entities.WOSSubService> GetWOSSubServiceList(string Sort, string SortDir, int Skip, int PageSize, out int TotalCount)
        {
            IQueryable<Entities.WOSSubService> WOSSubServiceList;
            int LoginID = UserSession.GetUserSession().LoginID;
            TotalCount = 0;
            try
            {
                DataSet WOSSubServiceDs = WOSSubServiceDAL.GetWOSSubServiceList(LoginID);

                if (WOSSubServiceDs != null && WOSSubServiceDs.Tables.Count > 0 && WOSSubServiceDs.Tables[0].Rows.Count > 0)
                {
                    var result = (from rw in WOSSubServiceDs.Tables[0].AsEnumerable()
                                  select new Entities.WOSSubService()
                                  {
                                      SubServiceMastID = rw["SubServiceMastID"] == DBNull.Value ? 0 : Convert.ToInt32(rw["SubServiceMastID"]),
                                      ServiceName = rw["ServiceName"] == DBNull.Value ? null : Convert.ToString(rw["ServiceName"]),
                                      SubServiceName = rw["SubServiceName"] == DBNull.Value ? null : Convert.ToString(rw["SubServiceName"]),
                                      IsActive = rw["Isactive"] == DBNull.Value ? false : Convert.ToBoolean(rw["Isactive"])
                                  }).ToList();
                    WOSSubServiceList = result.AsQueryable();

                    TotalCount = WOSSubServiceList.Count();
                    WOSSubServiceList = WOSSubServiceList.OrderBy(Sort + " " + SortDir);
                    if (PageSize > 0)
                    {
                        WOSSubServiceList = WOSSubServiceList.Skip(Skip).Take(PageSize);
                    }
                    return WOSSubServiceList.ToList();
                }
                else
                {
                    return new List<Entities.WOSSubService>();
                }
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(LoginID), "WOSSubServiceBL", "GetWOSSubServiceList", Properties.Resources.UnExpectedErrorAtBL, ex);
            }
        }

        public Entities.WOSSubService GetWOSSubServiceById(int? SubServiceMastID)
        {
            Entities.WOSSubService WOSSubServiceObj = new Entities.WOSSubService();
            int LoginID = UserSession.GetUserSession().LoginID;

            try
            {
                DataSet WOSSubServiceDs = WOSSubServiceDAL.GetWOSSubServiceById(LoginID, SubServiceMastID);

                if (WOSSubServiceDs != null)
                {
                    if (WOSSubServiceDs.Tables.Count > 0 && WOSSubServiceDs.Tables[0].Rows.Count > 0)
                    {
                        WOSSubServiceObj = (from rw in WOSSubServiceDs.Tables[0].AsEnumerable()
                                            select new Entities.WOSSubService()
                                            {
                                                SubServiceMastID = rw["SubServiceMastID"] == DBNull.Value ? 0 : Convert.ToInt32(rw["SubServiceMastID"]),
                                                ServiceMastID = rw["ServiceMastID"] == DBNull.Value ? 0 : Convert.ToInt32(rw["ServiceMastID"]),
                                                SubServiceName = rw["SubServiceName"] == DBNull.Value ? null : Convert.ToString(rw["SubServiceName"]),
                                                IsActive = rw["Isactive"] == DBNull.Value ? false : Convert.ToBoolean(rw["Isactive"])
                                            }).First();
                    }
                }
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "WOSSubServiceBL", "GetWOSSubServiceById", Properties.Resources.UnExpectedErrorAtBL, ex);
            }
            return WOSSubServiceObj;
        }

        public bool SaveWOSSubService(Entities.WOSSubService WOSSubServiceObj, out string result)
        {
            try
            {
                return WOSSubServiceDAL.SaveWOSSubService(WOSSubServiceObj, out result);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "WOSSubServiceBL", "SaveWOSSubService", Properties.Resources.UnExpectedErrorAtBL, ex);
            }
        }
    }
}