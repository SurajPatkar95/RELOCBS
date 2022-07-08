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
    public class CrewBL
    {
        private CrewDAL _CrewDAL;

        public CrewDAL CrewDAL
        {

            get
            {
                if (this._CrewDAL == null)
                    this._CrewDAL = new CrewDAL();
                return this._CrewDAL;
            }
        }

        public bool Insert(Crew Crew, out string result)
        {
            try
            {
                Crew.CompID = UserSession.GetUserSession().CompanyID;
                return CrewDAL.Insert(Crew, out result);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "CrewBL", "Insert", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

        }

        public bool Update(Crew Crew, out string result)
        {
            result = string.Empty;
            try
            {
                return CrewDAL.Update(Crew, out result);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "CrewBL", "Update", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }
        }

        public bool DeleteById(int id, out string result)
        {
            result = string.Empty;
            try
            {
                return CrewDAL.DeleteById(id, out result);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "CrewBL", "DeleteById", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }
        }

        public Crew GetDetailById(int? id)
        {
            Crew CrewObj = new Crew();
            try
            {
                DataSet CrewDs = CrewDAL.GetDetailById(id, UserSession.GetUserSession().LoginID);
                if (CrewDs != null && CrewDs.Tables.Count > 1)
                {

                    CrewObj = (from rw in CrewDs.Tables[0].AsEnumerable()
                                  select new Crew()
                                  {
                                      CrewCode = Convert.ToString(rw["CrewCode"]),
                                      SuperviserID = Convert.ToInt32(rw["SuperviserID"]),
                                      ServiceLineID=Convert.ToInt32(rw["ServiceLineID"]),
                                      CrewID = Convert.ToInt32(rw["CrewID"]),
                                      IsActive = Convert.ToBoolean(rw["Isactive"]),
                                      CompID = Convert.ToInt32(rw["CompID"]),
                                  }).First();

                    CrewObj.members = (from rw in CrewDs.Tables[1].AsEnumerable()
                                       select new CrewMember()
                                       {
                                           CWMID = Convert.ToInt32(rw["CrewMemberID"]),
                                           EmpID = Convert.ToInt32(rw["EmpID"]),
                                           CardEmpCode = Convert.ToString(rw["CardEmpCode"]),
                                           EmpName = Convert.ToString(rw["EmpName"]),
                                           IsActive = Convert.ToBoolean(rw["Isactive"]),
                                           
                                       }).ToList();

                    return CrewObj;
                }
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "CrewBL", "GetDetailById", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

            return CrewObj;

        }

        public IEnumerable<Crew> GetCrewList(int pPageIndex, int pPageSize, string pOrderBy, int pOrder, int? pCountryID, int? pCityID, int? pisActive, string SearchKey, out int totalCount)
        {
            totalCount = 0;

            try
            {
                IEnumerable<Crew> CrewList = CrewDAL.GetCrewList(pPageIndex, pPageSize, pOrderBy, pOrder, pCountryID, pCityID, pisActive, SearchKey, out totalCount);

                return CrewList;
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "CrewBL", "GetDetailById", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

        }

    }
}