using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RELOCBS.Entities;
using RELOCBS.DAL.Common;
using RELOCBS.Utility;
using RELOCBS.Common.ExceptionHandling;
using System.Data;

namespace RELOCBS.BL.Common
{
    public class LaneBL
    {
        private LaneDAL _laneDAL;

        public LaneDAL laneDAL
        {
            get
            {
                if (_laneDAL == null)
                {
                    _laneDAL = new LaneDAL();
                }

                return _laneDAL;
            }

        }

        public bool Insert(LaneViewModel lane, out string result)
        {
            try
            {
                return laneDAL.Insert(lane, out result);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "LaneBL", "Insert", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

        }

        public bool Update(LaneViewModel lane, out string result)
        {
            result = string.Empty;
            try
            {
                return laneDAL.Update(lane, out result);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "LaneBL", "Update", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }
        }

        public bool DeleteById(int id, out string result)
        {
            result = string.Empty;
            try
            {
                return laneDAL.DeleteById(id, out result);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "LaneBL", "DeleteById", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }
        }

        public LaneViewModel GetDetailById(int? id)
        {
            LaneViewModel laneObj = new LaneViewModel();
            try
            {
                DataTable laneDt =  laneDAL.GetDetailById(id, UserSession.GetUserSession().LoginID);
                if (laneDt != null && laneDt.Rows.Count > 0)
                {

                    laneObj = (from rw in laneDt.AsEnumerable()
                               select new LaneViewModel()
                               {
                                   LaneName = Convert.ToString(rw["LaneName"]),
                                   OriginCity = Convert.ToString(rw["OriginCityName"]),
                                   DestinationCity = Convert.ToString(rw["DestinationCityName"]),
                                   LaneId = Convert.ToInt32(rw["LaneId"]),
                                   OriginCityID= Convert.ToInt32(rw["OriginCityID"]),
                                   DestinationCityID= Convert.ToInt32(rw["DestinationCityID"]),
                                   isActive = Convert.ToBoolean(rw["isActive"])
                               }).First();


                    return laneObj;
                }
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "LaneBL", "GetDetailById", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

            return laneObj;

        }

        public IEnumerable<RELOCBS.Entities.Lane> GetLaneList(int pPageIndex, int pPageSize, string pOrderBy, int pOrder, int? OriginCountryID, int? DestinationCountryID, int? pisActive, string SearchKey, int LoggedinUserID, out int totalCount)
        {
            totalCount = 0;

            try
            {
                IEnumerable<RELOCBS.Entities.Lane> LaneList = laneDAL.GetLaneList(pPageIndex, pPageSize, pOrderBy, pOrder, OriginCountryID, DestinationCountryID, pisActive, SearchKey, LoggedinUserID, out totalCount);

                return LaneList;
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "LaneBL", "GetDetailById", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

        }
    }
}