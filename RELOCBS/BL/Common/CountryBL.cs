using RELOCBS.Common.ExceptionHandling;
using RELOCBS.DAL.Common;
using RELOCBS.Entities;
using RELOCBS.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RELOCBS.BL.Common
{
    public class CountryBL
    {
        private CountryDAL _countryBAL;

        public CountryDAL countryDAL
        {
            get
            {
                if (this._countryBAL == null)
                    this._countryBAL = new CountryDAL();
                return this._countryBAL;
            }
        }

        public bool Insert(CountryViewModel country, out string result)
        {
            try
            {
                countryDAL.Insert(country,out result);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "CountryBL", "Update", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

            return true;
        }

        public bool Update(CountryViewModel country, out string result)
        {
            try
            {
                countryDAL.Update(country,out result);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "CountryBL", "Update", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

            return true;
        }

        public string DeleteById(int id)
        {
            string result = string.Empty;
            try
            {
                countryDAL.DeleteById(id);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "CountryBL", "DeleteById", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

            return result;
        }

        public CountryViewModel GetDetailById(int? id)
        {
            CountryViewModel CountryObj = new CountryViewModel();
            try
            {
                DataTable CountryDt = countryDAL.GetDetailById(id, UserSession.GetUserSession().LoginID);
                if (CountryDt != null && CountryDt.Rows.Count > 0)
                {

                    CountryObj = (from rw in CountryDt.AsEnumerable()
                                       select new CountryViewModel()
                                       {
                                           CountryID = Convert.ToInt32(rw["CountryID"]),
                                           CountryCode = Convert.ToString(rw["CountryCode"]),
                                           CountryName = Convert.ToString(rw["CountryName"]),
                                           ContinentID = rw["ContinentID"]==DBNull.Value ? 0 : Convert.ToInt32(rw["ContinentID"]),
                                           isActive = Convert.ToBoolean(rw["Isactive"])

                                       }).First();


                    return CountryObj;
                }
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "CountryBL", "GetDetailById", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

            return CountryObj;

        }

        public IEnumerable<Country> GetCountryList(int pPageIndex, int pPageSize, string pOrderBy, int pOrder, int? pCountryID, int? pisActive, string SearchKey, int LoggedinUserID, out int totalCount)
        {
            totalCount = 0;

            try
            {
                IEnumerable<Country> CountryList = countryDAL.GetCountryList(pPageIndex, pPageSize, pOrderBy, pOrder, pCountryID, pisActive, SearchKey, LoggedinUserID,out totalCount);

                return CountryList;
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "CountryBL", "GetCountryList", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

        }

        public IEnumerable<SelectListItem> GetCountryDropdown()
        {
            return countryDAL.GetCountryDropdown(Convert.ToString(UserSession.GetUserSession().LoginID));
        }

    }
}