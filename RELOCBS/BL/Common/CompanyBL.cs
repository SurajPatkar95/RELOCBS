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
    public class CompanyBL
    {
        private  CompanyDAL  _companyDAL;

        public CompanyDAL companyDAL
        {

            get
            {
                if (this._companyDAL == null)
                    this._companyDAL = new CompanyDAL();
                return this._companyDAL;
            }
        }

        public bool Insert(CompanyViewModel company, out string result)
        {
            try
            {
                return companyDAL.Insert(company, out result);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "companyBL", "Insert", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

        }

        public bool Update(CompanyViewModel company, out string result)
        {
            result = string.Empty;
            try
            {
                return companyDAL.Update(company, out result);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "companyBL", "Update", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }
        }

        public bool DeleteById(int id, out string result)
        {
            result = string.Empty;
            try
            {
                return companyDAL.DeleteById(id, out result);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "companyBL", "DeleteById", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }
        }
        
        public CompanyViewModel GetDetailById(int? id)
        {
            CompanyViewModel CompanyObj = new CompanyViewModel();
            try
            {
                DataTable CompanyDt = companyDAL.GetDetailById(id, UserSession.GetUserSession().LoginID);
                if (CompanyDt != null && CompanyDt.Rows.Count > 0)
                {

                    CompanyObj = (from rw in CompanyDt.AsEnumerable()
                               select new CompanyViewModel()
                               {
                                   CompanyName = Convert.ToString(rw["CityName"]),
                                   ShortCompanyName = Convert.ToString(rw["CityCode"]),
                                   CompID=Convert.ToInt32(0),
                                   CityID = Convert.ToInt32(rw["CityID"]),
                                   CityName = Convert.ToString(rw["CountryName"]),
                                   IsActive = Convert.ToBoolean(rw["isActive"])
                               }).First();


                    return CompanyObj;
                }
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "CompanyBL", "GetDetailById", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

            return CompanyObj;

        }

        public IEnumerable<Company> GetCompanyList(int pPageIndex, int pPageSize, string pOrderBy, int pOrder,int? pRateTypeGrpID, int? pCountryID, int? pCityID, int? pisActive, string SearchKey, int LoggedinUserID, out int totalCount)
        {
            totalCount = 0;

            try
            {
                IEnumerable<Company> CompanyList = companyDAL.GetCompanyList(pPageIndex, pPageSize, pOrderBy, pOrder, pRateTypeGrpID, pCountryID, pCityID, pisActive, SearchKey, LoggedinUserID, out totalCount);

                return CompanyList;
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "CompanyBL", "GetDetailById", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

        }

        
    }
}