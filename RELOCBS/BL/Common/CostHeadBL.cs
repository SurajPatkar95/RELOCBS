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
    public class CostHeadBL
    {
        private CostHeadDAL _costHeadDAL;

        public CostHeadDAL costHeadDAL
        {

            get
            {
                if (this._costHeadDAL == null)
                    this._costHeadDAL = new CostHeadDAL();
                return this._costHeadDAL;
            }
        }

        public bool Insert(CostHeadMaster comp, out string result)
        {
            try
            {
                return costHeadDAL.Insert(comp, out result);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "CostHeadBL", "Insert", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

        }

        public bool Update(CostHeadMaster comp, out string result)
        {
            result = string.Empty;
            try
            {
                return costHeadDAL.Update(comp, out result);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "CostHeadBL", "Update", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }
        }

        public bool DeleteById(int id, out string result)
        {
            result = string.Empty;
            try
            {
                return costHeadDAL.DeleteById(id, out result);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "CostHeadBL", "DeleteById", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }
        }

        public CostHeadMaster GetDetailById(int? id)
        {
            CostHeadMaster CompanyObj = new CostHeadMaster();
            try
            {
                DataTable CompanyDt = costHeadDAL.GetDetailById(id, UserSession.GetUserSession().LoginID);
                if (CompanyDt != null && CompanyDt.Rows.Count > 0)
                {

                    CompanyObj = (from rw in CompanyDt.AsEnumerable()
                                  select new CostHeadMaster()
                                  {
                                      CostHeadID = Convert.ToInt32(rw["CostHeadID"]),
                                      CostHeadName = Convert.ToString(rw["CostHeadName"]),
                                      SSCCode = Convert.ToString(rw["SSCCode"]),
                                      ItemCode = Convert.ToString(rw["ItemCode"]),
                                      FinanceCode = Convert.ToString(rw["FinanceCode"]),
                                      InvDescription = Convert.ToString(rw["InvDescription"]),
                                      HasSubCostHead = Convert.ToBoolean(rw["HasSubCostHead"]),
                                      IsGSTApplicable = Convert.ToBoolean(rw["IsGSTApplicable"]),
                                      IsActive = Convert.ToBoolean(rw["IsActive"]),
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

        public IEnumerable<CostHeadMaster> GetList(int pPageIndex, int pPageSize, string pOrderBy, int pOrder, string SearchKey, int LoggedinUserID, out int totalCount)
        {
            totalCount = 0;

            try
            {
                IEnumerable<CostHeadMaster> CompanyList = costHeadDAL.GetList(pPageIndex, pPageSize, pOrderBy, pOrder, SearchKey, LoggedinUserID, out totalCount);

                return CompanyList;
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "CostHeadBL", "GetList", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

        }


    }
}