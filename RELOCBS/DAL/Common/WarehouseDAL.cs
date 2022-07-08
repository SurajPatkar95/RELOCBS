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
    public class WarehouseDAL
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


        public bool Insert(Warehouse model, out string result)
        {
            result = string.Empty;

            try
            {

                using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {
                        conn.AddCommand("[Comm].[AddEditWarehouse]", QueryType.Procedure);
                        //conn.AddParameter(ParameterOF.PROCEDURE, "@SP_WarehouseID", SqlDbType.Int, 0, ParameterDirection.InputOutput, model.WH_ID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_WarehouseName", SqlDbType.VarChar, 100, ParameterDirection.Input, CSubs.PSafeValue(model.Warehoue_Name));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Warehoue_Code", SqlDbType.VarChar, 50, ParameterDirection.Input, CSubs.PSafeValue(model.Warehoue_Code));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Address", SqlDbType.VarChar, 250, ParameterDirection.Input, CSubs.PSafeValue(model.WH_Address));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_City_ID", SqlDbType.Int, 0, ParameterDirection.Input, model.City_ID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_BranchID", SqlDbType.Int, 0, ParameterDirection.Input, model.BranchID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Incharge", SqlDbType.VarChar, 100, ParameterDirection.Input, CSubs.PSafeValue(model.Incharge));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Contact_No", SqlDbType.VarChar, 100, ParameterDirection.Input, CSubs.PSafeValue(model.Contact_No));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_No_Of_Crews", SqlDbType.Int, 0, ParameterDirection.Input, model.No_Of_Crews);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_PACKAGE_CREW_CAP_PER_DAY", SqlDbType.Int, 0, ParameterDirection.Input, model.PACKAGE_CREW_CAP_PER_DAY);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_DELIVERY_CREW_CAP_PER_DAY", SqlDbType.Int, 0, ParameterDirection.Input, model.DELIVERY_CREW_CAP_PER_DAY);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_WARE_LOC", SqlDbType.VarChar, 50, ParameterDirection.Input, CSubs.PSafeValue(model.WARE_LOC));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_WH_FAX", SqlDbType.VarChar, 50, ParameterDirection.Input, CSubs.PSafeValue(model.WH_FAX));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_WH_EMAIL", SqlDbType.VarChar, 50, ParameterDirection.Input, CSubs.PSafeValue(model.WH_EMAIL));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IsActive", SqlDbType.Bit, 0, ParameterDirection.Input, model.IsActive);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Area", SqlDbType.NVarChar, -1, ParameterDirection.Input, CSubs.PSafeValue(model.Area));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CompID", SqlDbType.Int, 0, ParameterDirection.Input, model.CompID);
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
                throw new DataAccessException(Convert.ToString(UserSession.GetUserSession().LoginID), "WarehouseDAL", "Insert", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
            //return true;
        }

        public bool Update(Warehouse model, out string result)
        {
            result = string.Empty;
            try
            {
                using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {

                        conn.AddCommand("[Comm].[AddEditWarehouse]", QueryType.Procedure);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_WarehouseID", SqlDbType.Int, 0, ParameterDirection.InputOutput, model.WH_ID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_WarehouseName", SqlDbType.VarChar, 100, ParameterDirection.Input, CSubs.PSafeValue(model.Warehoue_Name));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Warehoue_Code", SqlDbType.VarChar, 50, ParameterDirection.Input, CSubs.PSafeValue(model.Warehoue_Code));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Address", SqlDbType.VarChar, 250, ParameterDirection.Input, CSubs.PSafeValue(model.WH_Address));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_City_ID", SqlDbType.Int, 0, ParameterDirection.Input, model.City_ID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_BranchID", SqlDbType.Int, 0, ParameterDirection.Input, model.BranchID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Incharge", SqlDbType.VarChar, 100, ParameterDirection.Input, CSubs.PSafeValue(model.Incharge));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Contact_No", SqlDbType.VarChar, 100, ParameterDirection.Input, CSubs.PSafeValue(model.Contact_No));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_No_Of_Crews", SqlDbType.Int, 0, ParameterDirection.Input, model.No_Of_Crews);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_PACKAGE_CREW_CAP_PER_DAY", SqlDbType.Int, 0, ParameterDirection.Input, model.PACKAGE_CREW_CAP_PER_DAY);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_DELIVERY_CREW_CAP_PER_DAY", SqlDbType.Int, 0, ParameterDirection.Input, model.DELIVERY_CREW_CAP_PER_DAY);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_WARE_LOC", SqlDbType.VarChar, 50, ParameterDirection.Input, CSubs.PSafeValue(model.WARE_LOC));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_WH_FAX", SqlDbType.VarChar, 50, ParameterDirection.Input, CSubs.PSafeValue(model.WH_FAX));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_WH_EMAIL", SqlDbType.VarChar, 50, ParameterDirection.Input, CSubs.PSafeValue(model.WH_EMAIL));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IsActive", SqlDbType.Bit, 0, ParameterDirection.Input, model.IsActive);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CompID", SqlDbType.Int, 0, ParameterDirection.Input, model.CompID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Area", SqlDbType.NVarChar, -1, ParameterDirection.Input, CSubs.PSafeValue(model.Area));
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
                throw new DataAccessException(Convert.ToString(UserSession.GetUserSession().LoginID), "WarehouseDAL", "Update", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
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

                        conn.AddCommand("Comm.WarehouseDelete", QueryType.Procedure);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CompBranchID", SqlDbType.Int, 0, ParameterDirection.Input, id);
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
                throw new DataAccessException(Convert.ToString(UserSession.GetUserSession().LoginID), "WarehouseDAL", "DeleteById", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

        }

        public DataTable GetDetailById(int? id, int LoginID)
        {
            DataTable CompanyDetailDt = new DataTable();

            try
            {
                string query = string.Format("exec [Comm].[GETWarehouseDetail] @SP_WarehouseID={0},@SP_LoginID={1}",
                 CSubs.QSafeValue(Convert.ToString(id)), CSubs.QSafeValue(Convert.ToString(LoginID)));
                CompanyDetailDt = CSubs.GetDataTable(query);
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoginID), "WarehouseDAL", "GetDetailById", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

            return CompanyDetailDt;

        }

        public IEnumerable<Warehouse> GetWarehouseList(int pPageIndex, int pPageSize, string pOrderBy, int pOrder, int? CountryID, int? CityID, int? CompID, int? pisActive, string SearchKey, int LoggedinUserID, out int TotalCount)
        {
            TotalCount = 0;
            try
            {
                string query = string.Format("exec Comm.GETWarehouseForGrid @SP_PageIndex={0},@SP_PageSize={1},@SP_OrderBy={2},@SP_Order={3},@SP_CountryID={4},@SP_CityID={5},@SP_CompID={6},@SP_isActive={7},@SP_SearchString={8},@SP_LoginID={9}",
                 CSubs.QSafeValue(Convert.ToString(pPageIndex)),
                CSubs.QSafeValue(Convert.ToString(pPageSize)),
                CSubs.QSafeValue(pOrderBy),
                CSubs.QSafeValue(Convert.ToString(pOrder)),
                CSubs.QSafeValue(Convert.ToString(CountryID)),
                CSubs.QSafeValue(Convert.ToString(CityID)),
                CSubs.QSafeValue(Convert.ToString(CompID)),
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
                                  No_Of_Crews = !string.IsNullOrWhiteSpace(Convert.ToString( rw["No_Of_Crews"])) ? Convert.ToInt32(rw["No_Of_Crews"]) : (int?)null,
                                  PACKAGE_CREW_CAP_PER_DAY = !string.IsNullOrWhiteSpace(Convert.ToString(rw["PACKAGE_CREW_CAP_PER_DAY"])) ? Convert.ToInt32(rw["PACKAGE_CREW_CAP_PER_DAY"]) : (int?)null,
                                  DELIVERY_CREW_CAP_PER_DAY = !string.IsNullOrWhiteSpace(Convert.ToString(rw["DELIVERY_CREW_CAP_PER_DAY"])) ? Convert.ToInt32(rw["DELIVERY_CREW_CAP_PER_DAY"]) : (int?)null,
                                  WARE_LOC = Convert.ToString(rw["WARE_LOC"]),
                                  WH_FAX = Convert.ToString(rw["WH_FAX"]),
                                  WH_EMAIL = Convert.ToString(rw["WH_EMAIL"]),
                                  IsActive = Convert.ToBoolean(rw["IsActive"]),
                                  CompID = Convert.ToInt32(rw["CompID"]),
                                  Area = Convert.ToString(rw["Area"])
                              }).ToList();


                return result;
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoggedinUserID), "WarehouseDAL", "GetWarehouseList", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

            //return null;

        }

    }
}