using RELOCBS.App_Code;
using RELOCBS.Common.ExceptionHandling;
using RELOCBS.Entities;
using RELOCBS.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace RELOCBS.DAL.Common
{
    public class MaterialDAL
    {
        private CommonSubs _CSubs;

        public CommonSubs CSubs
        {

            get
            {
                if (this._CSubs == null)
                    this._CSubs = new CommonSubs();
                return this._CSubs;
            }
        }


        public bool Insert(Material material, out string result)
        {
            result = string.Empty;

            try
            {

                using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {


                        conn.AddCommand("[Comm].[AddEditMaterial]", QueryType.Procedure);
                        //conn.AddParameter(ParameterOF.PROCEDURE, "@SP_materialID", SqlDbType.Int, 0, ParameterDirection.InputOutput, material.CompID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_MaterialCode", SqlDbType.NVarChar, 20, ParameterDirection.Input, CSubs.PSafeValue(material.MaterialCode));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_MaterialName", SqlDbType.NVarChar, 100, ParameterDirection.Input, CSubs.PSafeValue(material.MaterialName));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_MaterialDescription", SqlDbType.NVarChar, 100, ParameterDirection.Input, CSubs.PSafeValue(material.MaterialDescription));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_MinQty", SqlDbType.Int, 0, ParameterDirection.Input, material.MinQty);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ReOrderQty", SqlDbType.Int, 0, ParameterDirection.Input, material.ReOrderQty);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Measurement", SqlDbType.VarChar, 20, ParameterDirection.Input, CSubs.PSafeValue(material.Measurement));
                        
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Rate", SqlDbType.Float, 0, ParameterDirection.Input, material.Rate);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IsActive", SqlDbType.Bit, 0, ParameterDirection.Input, true);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LOGINID", SqlDbType.Int, 0, ParameterDirection.Input, UserSession.GetUserSession().LoginID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@RETURNSTATUS", SqlDbType.SmallInt, 0, ParameterDirection.ReturnValue);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@Out_Message", SqlDbType.NVarChar, 500, ParameterDirection.Output);
                        conn.ExecuteProcedure(ProcedureReturnType.SingleValue);

                        if (!conn.IsError)
                        {
                            int ReturnStatus = Convert.ToInt32(conn.GetParameterValue(ParameterOF.PROCEDURE, "@RETURNSTATUS"));
                            result = Convert.ToString(conn.GetParameterValue(ParameterOF.PROCEDURE, "@Out_Message"));

                            if (ReturnStatus == 0)
                            {

                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                        else
                            throw new Exception(conn.ErrorMessage);

                    }
                    else
                        throw new Exception(conn.ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(UserSession.GetUserSession().LoginID), "MaterialDAL", "Insert", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
            //return true;
        }

        public bool Update(Material material, out string result)
        {
            result = string.Empty;
            try
            {
                using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {

                        conn.AddCommand("[Comm].[AddEditMaterial]", QueryType.Procedure);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_MaterialID", SqlDbType.Int, 0, ParameterDirection.InputOutput, material.MaterialID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_MaterialCode", SqlDbType.NVarChar, 20, ParameterDirection.Input, CSubs.PSafeValue(material.MaterialCode));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_MaterialName", SqlDbType.NVarChar, 100, ParameterDirection.Input, CSubs.PSafeValue(material.MaterialName));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_MaterialDescription", SqlDbType.NVarChar, 100, ParameterDirection.Input, CSubs.PSafeValue(material.MaterialDescription));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_MinQty", SqlDbType.Int, 0, ParameterDirection.Input, material.MinQty);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ReOrderQty", SqlDbType.Int, 0, ParameterDirection.Input, material.ReOrderQty);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Measurement", SqlDbType.VarChar, 20, ParameterDirection.Input, CSubs.PSafeValue(material.Measurement));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Rate", SqlDbType.Float, 0, ParameterDirection.Input, material.Rate);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IsActive", SqlDbType.Bit, 0, ParameterDirection.Input, material.IsActive);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LOGINID", SqlDbType.Int, 0, ParameterDirection.Input, UserSession.GetUserSession().LoginID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@RETURNSTATUS", SqlDbType.SmallInt, 0, ParameterDirection.ReturnValue);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@Out_Message", SqlDbType.NVarChar, 500, ParameterDirection.Output);
                        conn.ExecuteProcedure(ProcedureReturnType.SingleValue);

                        if (!conn.IsError)
                        {
                            int ReturnStatus = Convert.ToInt32(conn.GetParameterValue(ParameterOF.PROCEDURE, "@RETURNSTATUS"));
                            result = Convert.ToString(conn.GetParameterValue(ParameterOF.PROCEDURE, "@Out_Message"));
                            if (ReturnStatus == 0)
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }

                        }
                        else
                            throw new ArgumentException(conn.ErrorMessage);

                    }
                    else
                        throw new ArgumentException(conn.ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(UserSession.GetUserSession().LoginID), "MaterialDAL", "Update", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

            // return true;
        }

        public bool DeleteById(int id, out string result)
        {
            result = string.Empty;

            try
            {
                using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {

                        conn.AddCommand("Comm.materialDelete", QueryType.Procedure);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_materialID", SqlDbType.Int, 0, ParameterDirection.Input, id);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LOGINID", SqlDbType.Int, 0, ParameterDirection.Input, UserSession.GetUserSession().LoginID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@RETURNSTATUS", SqlDbType.SmallInt, 0, ParameterDirection.ReturnValue);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_MESSAGE", SqlDbType.NVarChar, 500, ParameterDirection.Output);
                        conn.ExecuteProcedure(ProcedureReturnType.SingleValue);

                        if (!conn.IsError)
                        {

                            int ReturnStatus = Convert.ToInt32(conn.GetParameterValue(ParameterOF.PROCEDURE, "@RETURNSTATUS"));
                            result = Convert.ToString(conn.GetParameterValue(ParameterOF.PROCEDURE, "@SP_MESSAGE"));

                            if (ReturnStatus == 0)
                            {

                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                        else
                            throw new Exception(conn.ErrorMessage);

                    }
                    else
                        throw new Exception(conn.ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(UserSession.GetUserSession().LoginID), "MaterialDAL", "DeleteById", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

        }

        public DataTable GetDetailById(int? id, int LoginID)
        {
            DataTable Dt = new DataTable();

            try
            {
                string query = string.Format("exec [Comm].[GETMaterialDetail] @SP_MaterialID={0},@SP_LoginID={1}",
                 CSubs.QSafeValue(Convert.ToString(id)), CSubs.QSafeValue(Convert.ToString(LoginID)));
                Dt = CSubs.GetDataTable(query);
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoginID), "MaterialDAL", "GetDetailById", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

            return Dt;

        }

        public IEnumerable<Material> GetMaterialList(int pPageIndex, int pPageSize, string pOrderBy, int pOrder, int? MaterialID,  int? pisActive, string SearchKey, int LoggedinUserID, out int TotalCount)
        {
            TotalCount = 0;
            try
            {
                string query = string.Format("exec Comm.GETMaterialForGrid @SP_PageIndex={0},@SP_PageSize={1},@SP_OrderBy={2},@SP_Order={3},@SP_MaterialID={4},@SP_isActive={5},@SP_SearchString={6},@SP_LoginID={7}",
                CSubs.QSafeValue(Convert.ToString(pPageIndex)),
                CSubs.QSafeValue(Convert.ToString(pPageSize)),
                CSubs.QSafeValue(pOrderBy),
                CSubs.QSafeValue(Convert.ToString(pOrder)),
                CSubs.QSafeValue(Convert.ToString(MaterialID)),
                CSubs.QSafeValue(Convert.ToString(pisActive)),
                CSubs.QSafeValue(SearchKey),
                Convert.ToString(LoggedinUserID)
                );

                DataTable dataTable = CSubs.GetDataTable(query);
                if (dataTable != null && dataTable.Rows.Count > 0)
                {
                    TotalCount = Convert.ToInt32(Convert.ToString(dataTable.Rows[0]["TotalRows"]));
                }

                var result = (from rw in dataTable.AsEnumerable()
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
                              }).ToList();


                return result;
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoggedinUserID), "MaterialDAL", "GetMaterialList", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

            //return null;

        }
        
    }
}