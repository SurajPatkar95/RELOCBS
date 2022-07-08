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
    public class WOSServiceBL
    {
        private WOSServiceDAL _WOSServiceDAL;
        public WOSServiceDAL WOSServiceDAL
        {
            get
            {
                if (_WOSServiceDAL == null)
                    _WOSServiceDAL = new WOSServiceDAL();
                return _WOSServiceDAL;
            }
        }

        public IEnumerable<Entities.WOSService> GetWOSServiceList(string Sort, string SortDir, int Skip, int PageSize, out int TotalCount)
        {
            IQueryable<Entities.WOSService> WOSServiceList;
            int LoginID = UserSession.GetUserSession().LoginID;
            TotalCount = 0;
            try
            {
                DataSet WOSServiceDs = WOSServiceDAL.GetWOSServiceList(LoginID);

                if (WOSServiceDs != null && WOSServiceDs.Tables.Count > 0 && WOSServiceDs.Tables[0].Rows.Count > 0)
                {
                    var result = (from rw in WOSServiceDs.Tables[0].AsEnumerable()
                                  select new Entities.WOSService()
                                  {
                                      ServiceMastID = rw["ServiceMastID"] == DBNull.Value ? 0 : Convert.ToInt32(rw["ServiceMastID"]),
                                      ServiceName = rw["ServiceName"] == DBNull.Value ? null : Convert.ToString(rw["ServiceName"]),
                                      IsActive = rw["Isactive"] == DBNull.Value ? false : Convert.ToBoolean(rw["Isactive"])
                                  }).ToList();
                    WOSServiceList = result.AsQueryable();

                    TotalCount = WOSServiceList.Count();
                    WOSServiceList = WOSServiceList.OrderBy(Sort + " " + SortDir);
                    if (PageSize > 0)
                    {
                        WOSServiceList = WOSServiceList.Skip(Skip).Take(PageSize);
                    }
                    return WOSServiceList.ToList();
                }
                else
                {
                    return new List<Entities.WOSService>();
                }
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(LoginID), "WOSServiceBL", "GetWOSServiceList", Properties.Resources.UnExpectedErrorAtBL, ex);
            }
        }

        public Entities.WOSService GetWOSServiceById(int? ServiceMastID)
        {
            Entities.WOSService WOSServiceObj = new Entities.WOSService();
            int LoginID = UserSession.GetUserSession().LoginID;

            try
            {
                DataSet WOSServiceDs = WOSServiceDAL.GetWOSServiceById(LoginID, ServiceMastID);

                if (WOSServiceDs != null)
                {
                    if (WOSServiceDs.Tables.Count > 0 && WOSServiceDs.Tables[0].Rows.Count > 0)
                    {
                        WOSServiceObj = (from rw in WOSServiceDs.Tables[0].AsEnumerable()
                                         select new Entities.WOSService()
                                         {
                                             ServiceMastID = rw["ServiceMastID"] == DBNull.Value ? 0 : Convert.ToInt32(rw["ServiceMastID"]),
                                             ServiceName = rw["ServiceName"] == DBNull.Value ? null : Convert.ToString(rw["ServiceName"]),
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
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "WOSServiceBL", "GetWOSServiceById", Properties.Resources.UnExpectedErrorAtBL, ex);
            }
            return WOSServiceObj;
        }

        public bool SaveWOSService(Entities.WOSService WOSServiceObj, out string result)
        {
            try
            {
                return WOSServiceDAL.SaveWOSService(WOSServiceObj, out result);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "WOSServiceBL", "SaveWOSService", Properties.Resources.UnExpectedErrorAtBL, ex);
            }
        }
    }
}