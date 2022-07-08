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
    public class AgentGroupBL
    {
        private AgentGroupDAL _agentDAL;

        public AgentGroupDAL agentDAL
        {

            get
            {
                if (this._agentDAL == null)
                    this._agentDAL = new AgentGroupDAL();
                return this._agentDAL;
            }
        }

        public bool Insert(AgentGroup agent, out string result)
        {
            try
            {
                return agentDAL.Insert(agent, out result);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "AgentGroupBL", "Insert", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

        }

        public bool Update(AgentGroup agent, out string result)
        {
            result = string.Empty;
            try
            {
                return agentDAL.Update(agent, out result);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "AgentGroupBL", "Update", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
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
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "AgentGroupBL", "DeleteById", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }
        }

        public AgentGroup GetDetailById(int? id)
        {
            AgentGroup CompanyObj = new AgentGroup();
            try
            {
                DataTable CompanyDt = agentDAL.GetDetailById(id, UserSession.GetUserSession().LoginID);
                if (CompanyDt != null && CompanyDt.Rows.Count > 0)
                {

                    CompanyObj = (from rw in CompanyDt.AsEnumerable()
                                  select new AgentGroup()
                                  {
                                      AgentGroupID = Convert.ToInt32(rw["AgentNameGrpID"]),
                                      AgentGroupName = Convert.ToString(rw["AgentNameGrp"]),
                                      ShortAgentGroupName = Convert.ToString(rw["ShortAgentName"]),
                                      AgentOrCorp = Convert.ToString(rw["AgentOrCorp"]),
                                      CompID = Convert.ToInt32(rw["CompID"]),
                                      CompanyName = Convert.ToString(rw["CompanyName"]),
                                      Address1 = Convert.ToString(rw["Address1"]),
                                      Address2 = Convert.ToString(rw["Address2"]),
                                      PinCode = Convert.ToString(rw["PinCode"]),
                                      ContactPerson = Convert.ToString(rw["ContactPerson"]),
                                      ContactPhone = Convert.ToString(rw["ContactPhone"]),
                                      CityID = Convert.ToInt32(rw["CityID"]),
                                      CityName = Convert.ToString(rw["CityName"]),
                                      Isactive = Convert.ToBoolean(rw["IsActive"])
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
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "AgentGroupBL", "GetDetailById", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

            return CompanyObj;

        }

        public IEnumerable<AgentGroup> GetAgentList(int pPageIndex, int pPageSize, string pOrderBy, int pOrder, int? pCountryID, int? pCityID, string CorA, string SearchKey, int LoggedinUserID, out int totalCount)
        {
            totalCount = 0;

            try
            {
                IEnumerable<AgentGroup> AgentList = agentDAL.GetAgentList(pPageIndex, pPageSize, pOrderBy, pOrder, pCountryID, pCityID, CorA, SearchKey, LoggedinUserID, out totalCount);

                return AgentList;
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "AgentGroupBL", "GetDetailById", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

        }

    }
}