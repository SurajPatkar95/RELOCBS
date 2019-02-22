using RELOCBS.DAL.Common;
using RELOCBS.Entities;
using RELOCBS.Utility;
using System;
using System.Collections.Generic;
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

        public bool Insert(CountryViewModel country)
        {
            try
            {
                countryDAL.Insert(country);
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return true;
        }

        public bool Update(CountryViewModel country)
        {
            try
            {
                countryDAL.Update(country);
            }
            catch (Exception ex)
            {

                throw ex;
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
            catch (Exception ex)
            {

                throw ex;
            }

            return result;
        }

        public IEnumerable<Country> GetCountryList(int pPageIndex, int pPageSize, string pOrderBy, int pOrder, int? pCountryID, int? pisActive, string SearchKey, int LoggedinUserID, out int totalCount)
        {
            totalCount = 0;

            try
            {
                IEnumerable<Country> CountryList = countryDAL.GetCountryList(pPageIndex, pPageSize, pOrderBy, pOrder, pCountryID, pisActive, SearchKey, LoggedinUserID,out totalCount);

                return CountryList;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public IEnumerable<SelectListItem> GetCountryDropdown()
        {
            return countryDAL.GetCountryDropdown(Convert.ToString(UserSession.GetUserSession().LoginID));
        }

    }
}