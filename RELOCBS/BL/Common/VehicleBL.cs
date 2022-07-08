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
    public class VehicleBL
    {

        private VehicleDAL _vehicleDAL;

        public VehicleDAL vehicleDAL
        {

            get
            {
                if (this._vehicleDAL == null)
                   this._vehicleDAL = new VehicleDAL();
                return this._vehicleDAL;
            }
        }

        public bool Insert(Vehicle vehicle, out string result)
        {
            try
            {
                vehicle.CompID = UserSession.GetUserSession().CompanyID;
                return vehicleDAL.Insert(vehicle, out result);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "vehicleBL", "Insert", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

        }

        public bool Update(Vehicle vehicle, out string result)
        {
            result = string.Empty;
            try
            {
                return vehicleDAL.Update(vehicle, out result);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "vehicleBL", "Update", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }
        }

        public bool DeleteById(int id, out string result)
        {
            result = string.Empty;
            try
            {
                return vehicleDAL.DeleteById(id, out result);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "vehicleBL", "DeleteById", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }
        }

        public Vehicle GetDetailById(int? id)
        {
            Vehicle CompanyObj = new Vehicle();
            try
            {
                DataTable CompanyDt = vehicleDAL.GetDetailById(id, UserSession.GetUserSession().LoginID);
                if (CompanyDt != null && CompanyDt.Rows.Count > 0)
                {

                    CompanyObj = (from rw in CompanyDt.AsEnumerable()
                                  select new Vehicle()
                                  {
                                      VehicleID = Convert.ToInt32(rw["Vehicle_Id"]),
                                      VehicleNo = Convert.ToString(rw["Vehicle_No"]),
                                      VehicleType = Convert.ToString(rw["Vehicle_Type"]),
                                      VendorID = Convert.ToInt32(rw["Vendor_ID"]),
                                      Capacity = Convert.ToDouble(rw["Capacity"]),
                                      Cost = Convert.ToDouble(rw["Vehicle_Cost"]),
                                      BranchID = Convert.ToInt32(rw["Branch_ID"]),
                                      Category = Convert.ToString(rw["Category"]),
                                      IsActive = Convert.ToBoolean(rw["Is_Active"]),
                                      CompID = Convert.ToInt32(rw["CompID"]),
                                      DimensionId = !string.IsNullOrWhiteSpace(Convert.ToString(rw["DimensionID"])) ? Convert.ToInt32(rw["DimensionID"]) : (int?)null,
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
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "vehicleBL", "GetDetailById", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

            return CompanyObj;

        }

        public IEnumerable<Vehicle> GetVehicleList(int pPageIndex, int pPageSize, string pOrderBy, int pOrder, int? pCountryID, int? pCityID, int? pisActive, string SearchKey, out int totalCount)
        {
            totalCount = 0;

            try
            {
                IEnumerable<Vehicle> VehicleList = vehicleDAL.GetvehicleList(pPageIndex, pPageSize, pOrderBy, pOrder, pCountryID, pCityID, pisActive, SearchKey, out totalCount);

                return VehicleList;
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "VehicleBL", "GetVehicleList", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

        }


    }
}