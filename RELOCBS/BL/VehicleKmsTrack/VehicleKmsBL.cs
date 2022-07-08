using RELOCBS.Common.ExceptionHandling;
using RELOCBS.DAL;
using RELOCBS.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RELOCBS.Entities;
using System.Data;
using System.Linq.Dynamic;

namespace RELOCBS.BL.VehicleKmsTrack
{
    public class VehicleKmsBL
    {
        private VehicleKmsDAL _vehicleKmsDAL;

        public VehicleKmsDAL vehicleKmsDAL
        {

            get
            {
                if (this._vehicleKmsDAL == null)
                    this._vehicleKmsDAL = new VehicleKmsDAL();
                return this._vehicleKmsDAL;
            }
        }


        public IEnumerable<VehicleKmsGrid> GetGrid(DateTime? FromDate, DateTime? Todate, string sort, string sortdir, int skip, int pageSize, out int totalCount,int? BranchID=null, int? VehicleNo = null, string Shipper = null, string JobNo = null)
        {
            totalCount = 0;

            try
            {
                bool RMCBuss = false;
                if (UserSession.GetUserSession().BussinessLine == "NON RMC-BUSINESS")
                {
                    RMCBuss = false;
                }
                else
                {
                    RMCBuss = true;
                }
                IQueryable<Entities.VehicleKmsGrid> List = vehicleKmsDAL.GetGrid(FromDate, Todate, RMCBuss, BranchID, VehicleNo, Shipper, JobNo);
                if (List != null)
                {
                    totalCount = List.Count();

                    if (pageSize > 1)
                    {
                        List = List.Skip((skip * (pageSize - 1))).Take(skip);
                    }
                    else
                    {
                        List = List.Take(skip);
                    }

                    if (!string.IsNullOrEmpty(sort))
                    {
                        List = List.OrderBy(sort + " " + sortdir);
                    }
                    return List.ToList();
                }
                else
                {
                    return new List<Entities.VehicleKmsGrid>();
                }
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "VehicleKmsBL", "GetGrid", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

        }

        public VehicleKms GetDetail(Int64 VehicleKmsID)
        {
            VehicleKms model = new VehicleKms();

            try
            {
                DataSet data = vehicleKmsDAL.GetDetail(UserSession.GetUserSession().LoginID, VehicleKmsID);

                if (data != null && data.Tables.Count > 0)
                {
                    if (data.Tables.Count > 0 && data.Tables[0] != null && data.Tables[0].Rows.Count > 0)
                    {
                        model = (from item in data.Tables[0].AsEnumerable()
                                 select new VehicleKms()
                                 {
                                     VehicleKmsID = !string.IsNullOrWhiteSpace(Convert.ToString(item["VehicleKmsId"])) ? Convert.ToInt64(item["VehicleKmsId"]) : -1,
                                     VehicleID = Convert.ToInt32(item["VehicleId"]),
                                     BranchID = Convert.ToInt32(item["BranchId"]),
                                     StartOdometer = Convert.ToInt64(item["StartOdometer"]),
                                     EndOdometer = Convert.ToInt64(item["EndOdometer"]),
                                     OdometerDate = Convert.ToDateTime(item["OdometerDate"]),
                                     Remarks = Convert.ToString(item["Remarks"])
                                 }).FirstOrDefault();

                    }

                    ////Job Details
                    if (data.Tables.Count > 1 && data.Tables[1] != null && data.Tables[1].Rows.Count > 0)
                    {
                        model.kmsJobs = (from item in data.Tables[1].AsEnumerable()
                                                    select new VehicleKmsJobs()
                                                    {
                                                        VehicleKmsJobID = Convert.ToInt64(item["VehicleKmsJobsId"]),
                                                        MoveID = Convert.ToInt64(item["MoveID"]),
                                                        JobNo = Convert.ToString(item["JobID"]),
                                                        AccountName = Convert.ToString(item["AccountName"]),
                                                        Shipper = Convert.ToString(item["ShipperName"]),
                                                        Remark = Convert.ToString(item["Remark"]),
                                                        // NoOfPacsDetails = Convert.ToString(item["NoOfPacsDetails"]),
                                                        // ServiceLine = Convert.ToString(item["NoOfPacsDetails"]),
                                                        // AgentName = Convert.ToString(item["NoOfPacsDetails"]),
                                                    }).ToList();
                    }

                    ////From-To Traveled Locations
                    if (data.Tables.Count > 2 && data.Tables[2] != null && data.Tables[2].Rows.Count > 0)
                    {
                        model.travelLocations = (from item in data.Tables[2].AsEnumerable()
                                              select new VehicleKmsTravelLocation()
                                              {
                                                  VehicleKmsTravelID = Convert.ToInt32(item["Travel_Id"]),
                                                  FromLocation = Convert.ToString(item["FromLocation"]),
                                                  ToLocation = Convert.ToString(item["ToLocation"]),
                                                  Remark = Convert.ToString(item["Remark"]),
                                              }).ToList();
                    }

                }
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "VehicleKmsBL", "GetDetails", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

            return model;
        }


        public VehicleKmsJobs GetJobDetail(Int64 MoveID)
        {
            VehicleKmsJobs model = new VehicleKmsJobs();

            try
            {
                DataTable data = vehicleKmsDAL.GetJobDetail(UserSession.GetUserSession().LoginID, MoveID);

                if (data != null && data.Rows.Count > 0)
                {

                    model = (from item in data.AsEnumerable()
                             select new VehicleKmsJobs()
                             {
                                 MoveID = MoveID,
                                 JobNo = Convert.ToString(item["JobID"]),
                                 AccountName = Convert.ToString(item["AccountName"]),
                                 Shipper = Convert.ToString(item["ShipperName"]),
                             }).FirstOrDefault();

                }
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "VehicleKmsBL", "GetJobDetail", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

            return model;

        }

        public bool Insert(VehicleKms model, out string result)
        {
            try
            {
                return vehicleKmsDAL.Insert(model, UserSession.GetUserSession().LoginID, out result);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "VehicleKmsBL", "Insert", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

        }

        public bool Delete(Int64 ID, out string message)
        {
            try
            {
                return vehicleKmsDAL.Delete(ID, UserSession.GetUserSession().LoginID, out message);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "VehicleKmsBL", "Delete", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

        }
    }
}