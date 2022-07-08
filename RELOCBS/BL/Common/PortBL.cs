using RELOCBS.Common.ExceptionHandling;
using RELOCBS.DAL.Common;
using RELOCBS.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using RELOCBS.Entities;

namespace RELOCBS.BL.Common
{
    public class PortBL
    {
        private PortDAL _agentDAL;

        public PortDAL agentDAL
        {

            get
            {
                if (this._agentDAL == null)
                    this._agentDAL = new PortDAL();
                return this._agentDAL;
            }
        }

        public bool Insert(Port port, out string result)
        {
            try
            {
                return agentDAL.Insert(port, out result);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "PortBL", "Insert", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

        }

        public bool Update(Port port, out string result)
        {
            result = string.Empty;
            try
            {
                return agentDAL.Update(port, out result);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "PortBL", "Update", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }
        }

        public bool DeleteById(int id, out string result)
        {
            result = string.Empty;
            try
            {
                return agentDAL.DeleteById(id, out result);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "PortBL", "DeleteById", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }
        }

        public Port GetDetailById(int? id)
        {
            Port CompanyObj = new Port();
            try
            {
                DataTable CompanyDt = agentDAL.GetDetailById(id, UserSession.GetUserSession().LoginID);
                if (CompanyDt != null && CompanyDt.Rows.Count > 0)
                {

                    CompanyObj = (from rw in CompanyDt.AsEnumerable()
                                  select new Port()
                                  {
                                      PortName = Convert.ToString(rw["PortName"]),
                                      PortID = Convert.ToInt32(rw["PortID"]),
                                      CountryID = Convert.ToInt32(rw["CountryID"]),
                                      CityID = string.IsNullOrEmpty(Convert.ToString(rw["CityID"])) ? (int?)null : Convert.ToInt32(rw["CityID"]),
                                      PortCode = Convert.ToString(rw["PortCode"]),
                                      CountryName = Convert.ToString(rw["CountryName"]),
                                      CityName = Convert.ToString(rw["CityName"]),
                                      Isactive = Convert.ToBoolean(rw["isActive"]),
                                      AirorSea = Convert.ToString(rw["AirorSea"]),
                                      ModeID  = Convert.ToInt32(rw["ModeID"]),
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
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "PortBL", "GetDetailById", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

            return CompanyObj;

        }

        public IEnumerable<Port> GetPortList(int pPageIndex, int pPageSize, string pOrderBy, int pOrder, int? pCityID, string SearOrAir,int ModeID, string SearchKey, int LoggedinUserID, out int totalCount)
        {
            totalCount = 0;

            try
            {
                IEnumerable<Port> PortList = agentDAL.GetPortList(pPageIndex, pPageSize, pOrderBy, pOrder, pCityID, ModeID, SearOrAir, 1, SearchKey, LoggedinUserID, out totalCount);

                return PortList;
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "PortBL", "GetDetailById", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

        }
    }
}