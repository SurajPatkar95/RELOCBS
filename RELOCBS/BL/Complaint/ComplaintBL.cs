using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using RELOCBS.Common.ExceptionHandling;
using RELOCBS.DAL.Complaint;
using RELOCBS.Utility;

namespace RELOCBS.BL.Complaint
{
    public class ComplaintBL
    {
        private ComplaintDAL  _complaintDAL;

        public ComplaintDAL complaintDAL
        {

            get
            {
                if (this._complaintDAL == null)
                    this._complaintDAL = new ComplaintDAL();
                return this._complaintDAL;
            }
        }

        public bool Insert(RELOCBS.Entities.Complaints model, out string result)
        {
            try
            {
                return complaintDAL.Insert(model, out result);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "complaintBL", "Insert", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

        }

        public IEnumerable<Entities.ComplaintGrid> GetForGrid(int? classificationId, int? statusId, string shipper, string loggerName, string filterType,string filterValue, string sort, string sortdir, int skip, int pageSize, out int totalCount)
        {
            try
            {
                bool RMCBuss = !UserSession.GetUserSession().BussinessLine.Equals("NON RMC-BUSINESS");
                IQueryable<Entities.ComplaintGrid> grids = complaintDAL.GetForGrid(Convert.ToInt32(UserSession.GetUserSession().EmpID), UserSession.GetUserSession().CompanyID, RMCBuss, classificationId, statusId, shipper, loggerName, filterType, filterValue);

                totalCount = grids.Count();
                if (pageSize > 1)
                {
                    grids = grids.Skip((skip * (pageSize - 1))).Take(skip);
                }
                else
                {
                    grids = grids.Take(skip);
                }

                //AllocationList = AllocationList.OrderBy(sort + " " + sortdir);

                return grids.ToList();

            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "ComplaintBL", "GetForGrid", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }


        }

        public RELOCBS.Entities.Complaints GetDetailById(Int64 Id)
        {

            RELOCBS.Entities.Complaints AObj = new RELOCBS.Entities.Complaints();
            try
            {

                DataSet CostDs = complaintDAL.GetDetailById(Convert.ToInt32(UserSession.GetUserSession().EmpID), Id);

                if (CostDs != null && CostDs.Tables.Count >= 1)
                {

                    if (CostDs.Tables.Count > 0 && CostDs.Tables[0] != null && CostDs.Tables[0].Rows.Count > 0)
                    {

                        AObj.ComplaintId = Convert.ToInt64(CostDs.Tables[0].Rows[0]["ComplaintId"]);
                        AObj.Description = Convert.ToString(CostDs.Tables[0].Rows[0]["Description"]);
                        AObj.Logger_Name = Convert.ToString(CostDs.Tables[0].Rows[0]["Logger_Name"]);
                        AObj.Logger_Email = Convert.ToString(CostDs.Tables[0].Rows[0]["Logger_Email"]);
                        AObj.Logger_Phone = Convert.ToString(CostDs.Tables[0].Rows[0]["Logger_Phone"]);
                        AObj.Logger_Mobile = Convert.ToString(CostDs.Tables[0].Rows[0]["Logger_Mobile"]);
                        AObj.ClassificationId = Convert.ToInt32(CostDs.Tables[0].Rows[0]["ClassificationId"]);
                        AObj.StatusId = Convert.ToInt32(CostDs.Tables[0].Rows[0]["StatusId"]);
                        AObj.SourceId = CostDs.Tables[0].Rows[0]["SourceId"] != DBNull.Value ? Convert.ToInt32(CostDs.Tables[0].Rows[0]["SourceId"]) : (int?)null;
                        AObj.MoveID = CostDs.Tables[0].Rows[0]["MoveID"] != DBNull.Value ? Convert.ToInt64(CostDs.Tables[0].Rows[0]["MoveID"]) : (Int64?)null;
                        AObj.EnqID = CostDs.Tables[0].Rows[0]["EnqID"] != DBNull.Value ? Convert.ToInt32(CostDs.Tables[0].Rows[0]["EnqID"]) : (int?)null;
                        AObj.EnqDetail_ID = CostDs.Tables[0].Rows[0]["EnqDetail_ID"] != DBNull.Value ? Convert.ToInt32(CostDs.Tables[0].Rows[0]["EnqDetail_ID"]) : (int?)null;
                        AObj.LastCreatedBy = Convert.ToString(CostDs.Tables[0].Rows[0]["LastCreatedBy"]);
                        AObj.LastCreatedDate = Convert.ToDateTime(CostDs.Tables[0].Rows[0]["LastCreatedDate"]);
                        AObj.Shipper = Convert.ToString(CostDs.Tables[0].Rows[0]["ShipperName"]);
                        AObj.FromCity = Convert.ToString(CostDs.Tables[0].Rows[0]["FromCity"]);
                        AObj.ToCity = Convert.ToString(CostDs.Tables[0].Rows[0]["ToCity"]);
                        AObj.Mode = Convert.ToString(CostDs.Tables[0].Rows[0]["Mode"]);
                        AObj.EnqNo = Convert.ToString(CostDs.Tables[0].Rows[0]["EnqNo"]);
                        AObj.EnqDetail_No = Convert.ToString(CostDs.Tables[0].Rows[0]["EnqDetSequenceID"]);
                        AObj.JoBNo = Convert.ToString(CostDs.Tables[0].Rows[0]["JobID"]);
                        AObj.Classification = Convert.ToString(CostDs.Tables[0].Rows[0]["ClassificationName"]);
                        AObj.Source = Convert.ToString(CostDs.Tables[0].Rows[0]["SourceName"]);
                    }
                    
                }


            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "ComplaintBL", "GetDetailById", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

            return AObj;


        }

        public RELOCBS.Entities.EnqJobDto GetEnqJobDetailById(Int64 EnqDetailId, Int64 MoveId)
        {
            RELOCBS.Entities.EnqJobDto AObj = new RELOCBS.Entities.EnqJobDto();
            var userVM = UserSession.GetUserSession();
            try
            {
                DataTable CostDt = complaintDAL.GetEnqJobDetailById(Convert.ToInt32(userVM.EmpID), EnqDetailId, MoveId);
                if (CostDt != null && CostDt.Rows.Count > 0)
                {
                    AObj.FromCity = Convert.ToString(CostDt.Rows[0]["FromCity"]);
                    AObj.ToCity = Convert.ToString(CostDt.Rows[0]["ToCity"]);
                    AObj.Mode = Convert.ToString(CostDt.Rows[0]["Mode"]);
                    AObj.Shipper = Convert.ToString(CostDt.Rows[0]["ShipperName"]);
                }
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "ComplaintBL", "GetEnqJobDetailById", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

            return AObj;


        }

        public bool Delete(int id, out string result)
        {
            result = string.Empty;
            try
            {
                return complaintDAL.Delete(id,out result);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "ComplaintBL", "Delete", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

        }
    }
}