using RELOCBS.Common.ExceptionHandling;
using RELOCBS.Entities;
using RELOCBS.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RELOCBS.DAL.Common;
using System.Data;

namespace RELOCBS.BL.Common
{
    public class MaterialBL
    {
        private MaterialDAL _materialDAL;

        public MaterialDAL materialDAL
        {

            get
            {
                if (this._materialDAL == null)
                    this._materialDAL = new MaterialDAL();
                return this._materialDAL;
            }
        }

        public bool Insert(Material  material, out string result)
        {
            try
            {
                return materialDAL.Insert(material, out result);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "MaterialBL", "Insert", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

        }

        public bool Update(Material material, out string result)
        {
            result = string.Empty;
            try
            {
                return materialDAL.Update(material, out result);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "MaterialBL", "Update", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }
        }

        public bool DeleteById(int id, out string result)
        {
            result = string.Empty;
            try
            {
                return materialDAL.DeleteById(id, out result);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "MaterialBL", "DeleteById", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }
        }

        public Material GetDetailById(int? id)
        {
            Material MaterialObj = new Material();
            try
            {
                DataTable dt = materialDAL.GetDetailById(id, UserSession.GetUserSession().LoginID);
                if (dt != null && dt.Rows.Count > 0)
                {

                    MaterialObj = (from rw in dt.AsEnumerable()
                               select new Material()
                               {
                                   MaterialID = Convert.ToInt32(rw["materialID"]),
                                   MaterialCode = Convert.ToString(rw["MaterialCode"]),
                                   MaterialName = Convert.ToString(rw["MaterialName"]),
                                   MaterialDescription = Convert.ToString(rw["MaterialDescription"]),
                                   MinQty = Convert.ToInt32(rw["MinQty"]),
                                   ReOrderQty = Convert.ToInt32(rw["ReOrderQty"]),
                                   Measurement = Convert.ToString(rw["Measurement"]),
                                   Rate = Convert.ToDouble(rw["Rate"]),
                                   IsActive = Convert.ToBoolean(rw["IsActive"])
                               }).First();

                    
                    return MaterialObj;
                }
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "MaterialBL", "GetDetailById", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

            return MaterialObj;

        }

        public IEnumerable<Material> GetMaterialList(int pPageIndex, int pPageSize, string pOrderBy, int pOrder, int? pMaterialID,  int? pisActive, string SearchKey, int LoggedinUserID, out int totalCount)
        {
            totalCount = 0;

            try
            {
                IEnumerable<Material> MaterialList = materialDAL.GetMaterialList(pPageIndex, pPageSize, pOrderBy, pOrder, pMaterialID, pisActive, SearchKey, LoggedinUserID, out totalCount);

                return MaterialList;
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "MaterialBL", "GetDetailById", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

        }


    }
}