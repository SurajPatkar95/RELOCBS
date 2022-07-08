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
    public class CompanyBranchBL
    {
        private CompanyBranchDAL _companyBranchDAL;

        public CompanyBranchDAL companyBranchDAL
        {

            get
            {
                if (this._companyBranchDAL == null)
                    this._companyBranchDAL = new CompanyBranchDAL();
                return this._companyBranchDAL;
            }
        }

        public bool Insert(CompanyBranchViewModel company, out string result)
        {
            try
            {
                return companyBranchDAL.Insert(company, out result);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "CompanyBranchBL", "Insert", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

        }

        public bool Update(CompanyBranchViewModel company, out string result)
        {
            result = string.Empty;
            try
            {
                return companyBranchDAL.Update(company, out result);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "CompanyBranchBL", "Update", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }
        }

        public bool DeleteById(int id, out string result)
        {
            result = string.Empty;
            try
            {
                return companyBranchDAL.DeleteById(id, out result);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "CompanyBranchBL", "DeleteById", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }
        }

        public CompanyBranchViewModel GetDetailById(int? id)
        {
            CompanyBranchViewModel CompanyObj = new CompanyBranchViewModel();
            try
            {
                DataTable CompanyDt = companyBranchDAL.GetDetailById(id, UserSession.GetUserSession().LoginID);
                if (CompanyDt != null && CompanyDt.Rows.Count > 0)
                {

                    CompanyObj = (from rw in CompanyDt.AsEnumerable()
                                  select new CompanyBranchViewModel()
                                  {
                                      CompBranchID = Convert.ToInt32(rw["CompBranchID"]),
                                      CompanyName = Convert.ToString(rw["CompanyName"]),
                                      CompBranchName = Convert.ToString(rw["CompBranchName"]),
                                      ContactNo = Convert.ToString(rw["ContactNo"]),
                                      ContactPerson = Convert.ToString(rw["ContactPerson"]),
                                      CompId = Convert.ToInt32(rw["CompId"]),
                                      CityID = Convert.ToInt32(rw["CityID"]),
                                      CityName = Convert.ToString(rw["CityName"]),
                                      Isactive = Convert.ToBoolean(rw["IsActive"]),
                                      IsRevenueBr = rw["IsRevenueBr"] == DBNull.Value ? false : Convert.ToBoolean(rw["IsRevenueBr"])
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

        public IEnumerable<CompanyBranch> GetCompanyBrachList(int pPageIndex, int pPageSize, string pOrderBy, int pOrder, int? pCountryID, int? pCityID, int? pCompID, int? pisActive, string SearchKey, int LoggedinUserID, out int totalCount)
        {
            totalCount = 0;

            try
            {
                IEnumerable<CompanyBranch> CompanyList = companyBranchDAL.GetCompanyBrachList(pPageIndex, pPageSize, pOrderBy, pOrder, pCountryID, pCityID, pCityID, pisActive, SearchKey, LoggedinUserID, out totalCount);

                return CompanyList;
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "CompanyBranchBL", "GetDetailById", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

        }
    }
}