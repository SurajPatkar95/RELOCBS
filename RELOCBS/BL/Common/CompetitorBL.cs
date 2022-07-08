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
    public class CompetitorBL
    {

        private CompetitorDAL _competitorDAL;

        public CompetitorDAL competitorDAL
        {

            get
            {
                if (this._competitorDAL == null)
                    this._competitorDAL = new CompetitorDAL();
                return this._competitorDAL;
            }
        }

        public bool Insert(CompetitorViewModel comp, out string result)
        {
            try
            {
                return competitorDAL.Insert(comp, out result);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "CompetitorBL", "Insert", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

        }

        public bool Update(CompetitorViewModel comp, out string result)
        {
            result = string.Empty;
            try
            {
                return competitorDAL.Update(comp, out result);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "CompetitorBL", "Update", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }
        }

        public bool DeleteById(int id, out string result)
        {
            result = string.Empty;
            try
            {
                return competitorDAL.DeleteById(id, out result);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "CompetitorBL", "DeleteById", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }
        }

        public CompetitorViewModel GetDetailById(int? id)
        {
            CompetitorViewModel CompanyObj = new CompetitorViewModel();
            try
            {
                DataTable CompanyDt = competitorDAL.GetDetailById(id, UserSession.GetUserSession().LoginID);
                if (CompanyDt != null && CompanyDt.Rows.Count > 0)
                {

                    CompanyObj = (from rw in CompanyDt.AsEnumerable()
                                  select new CompetitorViewModel()
                                  {
                                      CompitID = Convert.ToInt32(rw["CompitID"]),
                                      CompanyName = Convert.ToString(rw["CompanyName"]),
                                      CompitName = Convert.ToString(rw["CompitName"]),
                                      ContactNo = Convert.ToString(rw["ContactNo"]),
                                      ContactPerson = Convert.ToString(rw["ContactPerson"]),
                                      CompId = Convert.ToInt32(rw["CompId"]),
                                      //CityID = Convert.ToInt32(rw["CityID"]),
                                      //CityName = Convert.ToString(rw["CityName"]),
                                      Isactive = Convert.ToBoolean(rw["IsActive"]),
                                      
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

        public IEnumerable<Competitor> GetCompetitorList(int pPageIndex, int pPageSize, string pOrderBy, int pOrder, int? pCountryID, int? pCityID, int? pCompID, int? pisActive, string SearchKey, int LoggedinUserID, out int totalCount)
        {
            totalCount = 0;

            try
            {
                IEnumerable<Competitor> CompanyList = competitorDAL.GetCompetitorList(pPageIndex, pPageSize, pOrderBy, pOrder, pCountryID, pCityID, pCityID, pisActive, SearchKey, LoggedinUserID, out totalCount);

                return CompanyList;
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "CompetitorBL", "GetDetailById", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

        }
        
    }
}