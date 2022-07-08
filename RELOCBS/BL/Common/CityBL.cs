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
    public class CityBL
    {

        private CityDAL _cityDAL;
        
        public CityDAL cityDAL
        {

            get
            {
                if (this._cityDAL == null)
                    this._cityDAL = new CityDAL();
                return this._cityDAL;
            }
        }

        public bool Insert(CityViewModel city,out string result)
        {
            try
            {
               return cityDAL.Insert(city,out result);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "CityBL", "Insert", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

        }

        public bool Update(CityViewModel city, out string result)
        {
             result=string.Empty;
            try
            {
               return  cityDAL.Update(city,out result);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "CityBL", "Update", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }
        }

        public bool DeleteById(int id,out string result)
        {
            result = string.Empty;
            try
            {
               return cityDAL.DeleteById(id,out result);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "CityBL", "DeleteById", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }
        }

       

        public CityViewModel GetDetailById(int? id)
        {
            CityViewModel CityObj=new CityViewModel();
            try
            {
                 DataTable CityDt = cityDAL.GetDetailById(id,UserSession.GetUserSession().LoginID);
                 if(CityDt!=null && CityDt.Rows.Count>0)
                {

                    CityObj = (from rw in CityDt.AsEnumerable()
                                  select new CityViewModel()
                                  {
                                      CityName = Convert.ToString(rw["CityName"]),
                                      CityCode = Convert.ToString(rw["CityCode"]),
                                      CityID = Convert.ToInt32(rw["CityID"]),
                                      CountryID = Convert.ToInt32(rw["CountryID"]),
                                      CountryName = Convert.ToString(rw["CountryName"]),
                                      StateID = string.IsNullOrWhiteSpace(Convert.ToString(rw["StateID"])) ? (int?)null  : Convert.ToInt32(rw["StateID"]),
                                      StateName = Convert.ToString(rw["StateName"]),
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
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "CityBL", "GetDetailById", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

            return CityObj;

        }

        public IEnumerable<City> GetCityList(int pPageIndex, int pPageSize, string pOrderBy, int pOrder, int? pCityID, int? pisActive, string SearchKey, int LoggedinUserID,out int totalCount)
        {
             totalCount = 0;

            try
            {
                IEnumerable<City> CityList = cityDAL.GetCityList(pPageIndex, pPageSize, pOrderBy, pOrder, pCityID, pisActive, SearchKey, LoggedinUserID,out totalCount);

               return CityList;
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "CityBL", "GetDetailById", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

        }

        public IEnumerable<SelectListItem> GetCityDropdown()
        {
            try
            {
                return cityDAL.GetCityDropdown(Convert.ToString(UserSession.GetUserSession().LoginID));
            }
            catch (DataAccessException ex)
            {
                throw new DataAccessException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "CityBL", "GetCityDropdown", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

        }

        public IEnumerable<SelectListItem> GetCountryByContinent(string ContinentID)
        {
            try
            {
                return cityDAL.GetCountryByContinent(ContinentID, Convert.ToString(UserSession.GetUserSession().LoginID));
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "CityBL", "GetCountryByContinent", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }
            
        }
    }
}