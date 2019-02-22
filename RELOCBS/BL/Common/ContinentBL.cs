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
    public class ContinentBL
    {
        private ContinentDAL _continentDAL;

        public ContinentDAL continentDAL
        {

            get
            {
                if (this._continentDAL == null)
                    this._continentDAL = new ContinentDAL();
                return this._continentDAL;
            }
        }

        public bool Insert(Continent continent)
        {
            try
            {
                continentDAL.Insert(continent);
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return true;
        }

        public bool Update(Continent continent)
        {
            try
            {
                continentDAL.Update(continent);
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
                continentDAL.DeleteById(id);
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return result;
        }

        
        public IEnumerable<Continent> GetContinentList(int pPageIndex, int pPageSize, string pOrderBy, int pOrder, int? pCityID, int? pisActive, string SearchKey, int LoggedinUserID, out int totalCount)
        {
            totalCount = 0;

            try
            {
                IEnumerable<Continent> ContinentList = continentDAL.GetContinentList(pPageIndex, pPageSize, pOrderBy, pOrder, pCityID, pisActive, SearchKey, LoggedinUserID, out totalCount);
                

                return ContinentList;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public IEnumerable<SelectListItem> GetContinentDropdown()
        {
            return continentDAL.GetContinentDropdown(Convert.ToString(UserSession.GetUserSession().LoginID));
        }

    }
}