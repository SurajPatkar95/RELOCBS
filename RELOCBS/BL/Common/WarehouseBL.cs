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
    public class WarehouseBL
    {
        private WarehouseDAL _WarehouseDAL;

        public WarehouseDAL WarehouseDAL
        {

            get
            {
                if (this._WarehouseDAL == null)
                    this._WarehouseDAL = new WarehouseDAL();
                return this._WarehouseDAL;
            }
        }

        public bool Insert(Warehouse Warehouse, out string result)
        {
            try
            {
                ////set the company id as user session id
                Warehouse.CompID = UserSession.GetUserSession().CompanyID;
                return WarehouseDAL.Insert(Warehouse, out result);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "WarehouseBL", "Insert", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

        }

        public bool Update(Warehouse Warehouse, out string result)
        {
            result = string.Empty;
            try
            {
                return WarehouseDAL.Update(Warehouse, out result);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "WarehouseBL", "Update", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }
        }

        public bool DeleteById(int id, out string result)
        {
            result = string.Empty;
            try
            {
                return WarehouseDAL.DeleteById(id, out result);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "WarehouseBL", "DeleteById", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }
        }

        public Warehouse GetDetailById(int? id)
        {
            Warehouse WarehouseObj = new Warehouse();
            try
            {
                DataTable WarehouseDt = WarehouseDAL.GetDetailById(id, UserSession.GetUserSession().LoginID);
                if (WarehouseDt != null && WarehouseDt.Rows.Count > 0)
                {

                    WarehouseObj = (from rw in WarehouseDt.AsEnumerable()
                                  select new Warehouse()
                                  {
                                      WH_ID = Convert.ToInt32(rw["WH_ID"]),
                                      Warehoue_Code = Convert.ToString(rw["Warehoue_Code"]),
                                      Warehoue_Name = Convert.ToString(rw["Warehoue_Name"]),
                                      BranchName = Convert.ToString(rw["BranchName"]),
                                      CityName = Convert.ToString(rw["CityName"]),
                                      City_ID = Convert.ToInt32(rw["City_ID"]),
                                      BranchID = Convert.ToInt32(rw["BranchID"]),
                                      WH_Address = Convert.ToString(rw["WH_Address"]),
                                      Incharge = Convert.ToString(rw["Incharge"]),
                                      Contact_No = Convert.ToString(rw["Contact_No"]),
                                      No_Of_Crews = !string.IsNullOrWhiteSpace(Convert.ToString(rw["No_Of_Crews"])) ? Convert.ToInt32(rw["No_Of_Crews"]) : (int?)null,
                                      PACKAGE_CREW_CAP_PER_DAY = !string.IsNullOrWhiteSpace(Convert.ToString(rw["PACKAGE_CREW_CAP_PER_DAY"])) ? Convert.ToInt32(rw["PACKAGE_CREW_CAP_PER_DAY"]) : (int?)null,
                                      DELIVERY_CREW_CAP_PER_DAY = !string.IsNullOrWhiteSpace(Convert.ToString(rw["DELIVERY_CREW_CAP_PER_DAY"])) ? Convert.ToInt32(rw["DELIVERY_CREW_CAP_PER_DAY"]) : (int?)null,
                                      WARE_LOC = Convert.ToString(rw["WARE_LOC"]),
                                      WH_FAX = Convert.ToString(rw["WH_FAX"]),
                                      WH_EMAIL = Convert.ToString(rw["WH_EMAIL"]),
                                      CompID = Convert.ToInt32(rw["CompID"]),
                                      CompanyName = Convert.ToString(rw["CompanyName"]),
                                      IsActive = Convert.ToBoolean(rw["IsActive"]),
                                      Area = Convert.ToString(rw["Area"]),
                                  }).First();


                    return WarehouseObj;
                }
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "WarehouseBL", "GetDetailById", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

            return WarehouseObj;

        }

        public IEnumerable<Warehouse> GetWarehouseBrachList(int pPageIndex, int pPageSize, string pOrderBy, int pOrder, int? pCountryID, int? pCityID, int? pCompID, int? pisActive, string SearchKey, int LoggedinUserID, out int totalCount)
        {
            totalCount = 0;

            try
            {
                IEnumerable<Warehouse> WarehouseList = WarehouseDAL.GetWarehouseList(pPageIndex, pPageSize, pOrderBy, pOrder, pCountryID, pCityID, pCompID, pisActive, SearchKey, LoggedinUserID, out totalCount);

                return WarehouseList;
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "WarehouseBL", "GetDetailById", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

        }
    }
}