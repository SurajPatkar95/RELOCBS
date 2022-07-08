using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RELOCBS.Common.ExceptionHandling;
using RELOCBS.DAL.GMMS;
using RELOCBS.Entities;
using RELOCBS.Utility;
using System.Data;
using System.Linq.Dynamic;

namespace RELOCBS.BL.GMMS
{
    public class LeadBL
    {
        private LeadDAL _leadDAL;

        public LeadDAL leadDAL
        {

            get
            {
                if (this._leadDAL == null)
                    this._leadDAL = new LeadDAL();
                return this._leadDAL;
            }
        }
        public bool Insert(LeadViewModel lead, out string result)
        {
            try
            {
                return leadDAL.InsertLead(lead,UserSession.GetUserSession().LoginID, out result);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "LeadBL", "InsertLead", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

        }

        public bool InsertJob(LeadViewModel lead, out string result, out Int64 MoveId)
        {
            try
            {
                return leadDAL.InsertJob(lead, UserSession.GetUserSession().LoginID, out result, out MoveId);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "LeadBL", "InsertJob", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

        }

        public IEnumerable<Lead> GetForGrid(int? RMCID,int? FromCityID,int? ToCityID, bool isRoad ,int? UpdatedBatchId, string sort, string sortdir, int skip, int pageSize, out int totalCount)
        {
            try
            {
                IQueryable<Lead> LeadList = leadDAL.GetForGrid(UserSession.GetUserSession().LoginID, RMCID, FromCityID, ToCityID,UpdatedBatchId, isRoad);
                totalCount = LeadList.Count();
                if (UpdatedBatchId == null)
                {
                    
                    if (!string.IsNullOrEmpty(sort) && !string.IsNullOrEmpty(sortdir))
                    {
                        LeadList = LeadList.OrderBy(sort + " " + sortdir);
                    }
                    if (pageSize > 1)
                    {
                        LeadList = LeadList.Skip(skip).Take(pageSize);
                    }
                }  
                
                return LeadList.ToList();
                
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "LeadBL", "GetForGrid", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }
        }

        public Entities.GMMSRateUpload GetDetailById(int LeadID)
        {
            Entities.GMMSRateUpload LeadObj = new Entities.GMMSRateUpload();
            try
            {

                DataTable LeadDt = leadDAL.GetDetailById(UserSession.GetUserSession().LoginID,LeadID);

                
                if (LeadDt != null  && LeadDt.Rows.Count > 0)
                {

                    LeadObj = (from rw in LeadDt.AsEnumerable()
                                 select new Entities.GMMSRateUpload()
                                 {
                                     LeadID = Convert.ToInt32(rw["LeadID"]),
                                     RMCID = Convert.ToInt32(rw["ForRMCID"]),
                                     //FromCityID =  Convert.ToInt32(rw["OrgCityID"]),
                                     //ToCityID =  Convert.ToInt32(rw["DestCityID"])
                                 }).First();

                    
                    return LeadObj;
                }
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "LeadBL", "GetDetailById", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

            return LeadObj;

        }


		public string GetRMCType(int? RMCID)
		{
			string LeadObj = null;
			try
			{

				DataTable LeadDt = leadDAL.GetRMCType(RMCID, UserSession.GetUserSession().LoginID);
				

				if (LeadDt != null && LeadDt.Rows.Count > 0)
				{

					LeadObj = Convert.ToString(LeadDt.Rows[0][0]);
					

					return LeadObj;
				}
			}
			catch (DataAccessException ex)
			{
				throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
			}
			catch (Exception ex)
			{
				throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "LeadBL", "GetDetailById", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
			}

			return LeadObj;

		}
	}

}