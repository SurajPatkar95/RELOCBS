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
    public class ShippingLineBL
    {
        private ShippingLineDAL _shippingLineDAL;

        public ShippingLineDAL shippingLineDAL
        {

            get
            {
                if (this._shippingLineDAL == null)
                    this._shippingLineDAL = new ShippingLineDAL();
                return this._shippingLineDAL;
            }
        }

        public bool Insert(ShippingLine data, out string result)
        {
            try
            {
                data.CompID = UserSession.GetUserSession().CompanyID;
                return shippingLineDAL.Insert(data, out result);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "ShippingLineBL", "Insert", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

        }

        public bool Update(ShippingLine data, out string result)
        {
            result = string.Empty;
            try
            {
                return shippingLineDAL.Update(data, out result);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "ShippingLineBL", "Update", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }
        }

        public bool DeleteById(int id, out string result)
        {
            result = string.Empty;
            try
            {
                return shippingLineDAL.DeleteById(id, out result);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "ShippingLineBL", "DeleteById", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }
        }

        public ShippingLine GetDetailById(int? id)
        {
            ShippingLine ShippingLineObj = new ShippingLine();
            try
            {
                DataTable ShippingLineDt = shippingLineDAL.GetDetailById(id, UserSession.GetUserSession().LoginID);
                if (ShippingLineDt != null && ShippingLineDt.Rows.Count > 0)
                {

                    ShippingLineObj = (from rw in ShippingLineDt.AsEnumerable()
                                  select new ShippingLine()
                                  {
                                      ShipLineID = Convert.ToInt32(rw["ShipLineID"]),
                                      ShipLineName = Convert.ToString(rw["ShipLineName"]),
                                      ModeID = Convert.ToInt32(rw["ModeID"]),
                                      ModeName = Convert.ToString(rw["TransportModeName"]),
                                      Isactive = Convert.ToBoolean(rw["IsActive"]),
                                      CompID = Convert.ToInt32(rw["CompID"]),
                                      CompanyName = Convert.ToString(rw["CompanyName"]),
                                  }).First();


                    return ShippingLineObj;
                }
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "ShippingLineBL", "GetDetailById", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

            return ShippingLineObj;

        }

        public IEnumerable<ShippingLine> GetShippingLineList(int pPageIndex, int pPageSize, string pOrderBy, int pOrder, int? pModeID, int? pisActive, string SearchKey, out int totalCount)
        {
            totalCount = 0;

            try
            {
                IEnumerable<ShippingLine> ShippingLineList = shippingLineDAL.GetShippingLineList(pPageIndex, pPageSize, pOrderBy, pOrder, pModeID, pisActive, SearchKey, out totalCount);

                return ShippingLineList;
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "ShippingLineBL", "GetDetailById", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

        }
    }
}