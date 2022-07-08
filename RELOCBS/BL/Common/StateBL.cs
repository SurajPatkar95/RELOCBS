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
    public class StateBL
    {

        private StateDAL _stateDAL;

        public StateDAL stateDAL
        {

            get
            {
                if (this._stateDAL == null)
                    this._stateDAL = new StateDAL();
                return this._stateDAL;
            }
        }

        public bool Insert(StateViewModel model, out string result)
        {
            try
            {
                return stateDAL.Insert(model, out result);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "StateBL", "Insert", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

        }

        public bool Update(StateViewModel model, out string result)
        {
            result = string.Empty;
            try
            {
                return stateDAL.Update(model, out result);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "StateBL", "Update", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }
        }

        public bool DeleteById(int id, out string result)
        {
            result = string.Empty;
            try
            {
                return stateDAL.DeleteById(id, out result);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "StateBL", "DeleteById", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }
        }



        public StateViewModel GetDetailById(int? id)
        {
            StateViewModel CityObj = new StateViewModel();
            try
            {
                DataTable CityDt = stateDAL.GetDetailById(id, UserSession.GetUserSession().LoginID);
                if (CityDt != null && CityDt.Rows.Count > 0)
                {

                    CityObj = (from rw in CityDt.AsEnumerable()
                               select new StateViewModel()
                               {
                                   StateName = Convert.ToString(rw["StateName"]),
                                   StateID = Convert.ToInt32(rw["StateID"]),
                                   CountryID = Convert.ToInt32(rw["CountryID"]),
                                   CountryName = Convert.ToString(rw["CountryName"]),
                                   isActive = Convert.ToBoolean(rw["isActive"])
                               }).First();


                    return CityObj;
                }
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "StateBL", "GetDetailById", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

            return CityObj;

        }

        public IEnumerable<State> GetStateList(int pPageIndex, int pPageSize, string pOrderBy, int pOrder, int? pStateID, int? pisActive, string SearchKey, int LoggedinUserID, out int totalCount)
        {
            totalCount = 0;

            try
            {
                IEnumerable<State> list = stateDAL.GetStateList(pPageIndex, pPageSize, pOrderBy, pOrder, pStateID, pisActive, SearchKey, LoggedinUserID, out totalCount);

                return list;
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "StateBL", "GetStateList", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

        }

    }
}