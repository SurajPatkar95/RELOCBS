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
    public class CostHeadCommisionableForRMCBL
    {
        private CostHeadCommisionableForRMCDAL _forRMCDAL;

        public CostHeadCommisionableForRMCDAL forRMCDAL
        {
            get
            {
                if (this._forRMCDAL == null)
                    this._forRMCDAL = new CostHeadCommisionableForRMCDAL();
                return this._forRMCDAL;
            }
        }

        public bool Insert(CostHeadCommisionableForRMC model, out string result)
        {
            try
            {
                forRMCDAL.Insert(model, out result);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "CostHeadCommisionableForRMCBL", "Update", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

            return true;
        }

        public bool Update(CostHeadCommisionableForRMC model, out string result)
        {
            try
            {
                forRMCDAL.Update(model, out result);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "CostHeadCommisionableForRMCBL", "Update", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

            return true;
        }

        public bool DeleteById(int id, out string result)
        {
            result = string.Empty;
            try
            {
               return forRMCDAL.DeleteById(id, out result);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "CostHeadCommisionableForRMCBL", "DeleteById", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }
            
        }

        public CostHeadCommisionableForRMC GetDetailById(int? id)
        {
            CostHeadCommisionableForRMC Obj = new CostHeadCommisionableForRMC();
            try
            {
                DataTable Dt = forRMCDAL.GetDetailById(id, UserSession.GetUserSession().LoginID);
                if (Dt != null && Dt.Rows.Count > 0)
                {

                    Obj = (from rw in Dt.AsEnumerable()
                                  select new CostHeadCommisionableForRMC()
                                  {
                                      RMCCommiCostID = Convert.ToInt32(rw["RMCCommiCostID"]),
                                      CostHeadID = Convert.ToInt32(rw["CostHeadID"]),
                                      RMCID = Convert.ToInt32(rw["continentID"]),
                                      Isactive = Convert.ToBoolean(rw["isActive"])
                                  }).First();


                    return Obj;
                }
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "CostHeadCommisionableForRMCBL", "GetDetailById", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

            return Obj;

        }

        public IEnumerable<CostHeadCommisionableForRMC> GetGridList(int pPageIndex, int pPageSize, string pOrderBy, int pOrder, int? pisActive, string SearchKey, int LoggedinUserID, out int totalCount)
        {
            totalCount = 0;

            try
            {
                IEnumerable<CostHeadCommisionableForRMC> list = forRMCDAL.GetGridList(pPageIndex, pPageSize, pOrderBy, pOrder,  pisActive, SearchKey, LoggedinUserID, out totalCount);

                return list;
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "CostHeadCommisionableForRMCBL", "GetGridList", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

        }


    }
}