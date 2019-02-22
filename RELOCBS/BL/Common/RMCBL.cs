using RELOCBS.Common.ExceptionHandling;
using RELOCBS.DAL.Common;
using RELOCBS.Entities;
using RELOCBS.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace RELOCBS.BL.Common
{
    public class RMCBL
    {
        private RMCDAL _RMCDAL;

        public RMCDAL rmcDAL
        {
            get
            {
                if (_RMCDAL == null)
                {
                    _RMCDAL = new RMCDAL();
                }

                return _RMCDAL;
            }

        }

        public bool Insert(RMCViewModel RMC, out string result)
        {
            try
            {
                return rmcDAL.Insert(RMC, out result);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "RMCBL", "Insert", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

        }

        public bool Update(RMCViewModel RMC, out string result)
        {
            result = string.Empty;
            try
            {
                return rmcDAL.Update(RMC, out result);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "RMCBL", "Update", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }
        }

        public bool DeleteById(int id, out string result)
        {
            result = string.Empty;
            try
            {
                return rmcDAL.DeleteById(id, out result);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "RMCBL", "DeleteById", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }
        }

        public RMCViewModel GetDetailById(int? id)
        {
            RMCViewModel RMCObj = new RMCViewModel();
            try
            {
                DataTable RMCDt = rmcDAL.GetDetailById(id, UserSession.GetUserSession().LoginID);
                if (RMCDt != null && RMCDt.Rows.Count > 0)
                {

                    RMCObj = (from rw in RMCDt.AsEnumerable()
                               select new RMCViewModel()
                               {
                                   RMCName = Convert.ToString(rw["RMCName"]),
                                   ShortRMCName = Convert.ToString(rw["ShortRMCName"]),
                                   CityName = Convert.ToString(rw["CityName"]),
                                   CountryName = Convert.ToString(rw["CountryName"]),
                                   RMCID = Convert.ToInt32(rw["RMCID"]),
                                   CityID = Convert.ToInt32(rw["CityID"]),
                                   CountryID = Convert.ToInt32(rw["CountryID"]),
                                   isActive = Convert.ToBoolean(rw["isActive"])
                               }).First();


                    return RMCObj;
                }
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "RMCBL", "GetDetailById", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

            return RMCObj;

        }

        public IEnumerable<RELOCBS.Entities.RMC> GetRMCList(int pPageIndex, int pPageSize, string pOrderBy, int pOrder, int? RateTypeGrpID, int? CountryID, int? CityID, int? pisActive, string SearchKey, int LoggedinUserID, out int totalCount)
        {
            totalCount = 0;

            try
            {
                IEnumerable<RELOCBS.Entities.RMC> rmcList = rmcDAL.GetRMCList(pPageIndex, pPageSize, pOrderBy, pOrder, RateTypeGrpID, CountryID, CityID, pisActive, SearchKey, LoggedinUserID, out totalCount);

                return rmcList;
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "RMCBL", "GetDetailById", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

        }

    }
}