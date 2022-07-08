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
    public class VehicleDAL
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


        public bool Insert(Vehicle vehicle, out string result)
        {
            result = string.Empty;

            try
            {

                using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {

                        conn.AddCommand("[Comm].[AddEditVehicle]", QueryType.Procedure);
                        //conn.AddParameter(ParameterOF.PROCEDURE, "@SP_vehicleID", SqlDbType.Int, 0, ParameterDirection.InputOutput, vehicle.CompID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_VehicleNo", SqlDbType.NVarChar, 100, ParameterDirection.Input, CSubs.PSafeValue(vehicle.VehicleNo));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_VendorID", SqlDbType.Int, 0, ParameterDirection.Input, vehicle.VendorID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Category", SqlDbType.VarChar, 50, ParameterDirection.Input, CSubs.PSafeValue(vehicle.Category));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Capacity", SqlDbType.Float, 0, ParameterDirection.Input, vehicle.Capacity);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_BranchId", SqlDbType.Int, 0, ParameterDirection.Input, vehicle.BranchID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_VehicleType", SqlDbType.VarChar, 2, ParameterDirection.Input, "O");
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Vehicle_Cost", SqlDbType.Float, 0, ParameterDirection.Input, vehicle.Cost);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IsActive", SqlDbType.Bit, 0, ParameterDirection.Input, vehicle.IsActive);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LOGINID", SqlDbType.Int, 0, ParameterDirection.Input, UserSession.GetUserSession().LoginID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CompID", SqlDbType.Int, 0, ParameterDirection.Input, vehicle.CompID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@RETURNSTATUS", SqlDbType.SmallInt, 0, ParameterDirection.ReturnValue);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@Out_Message", SqlDbType.NVarChar, 500, ParameterDirection.Output);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@sp_dimensionid", SqlDbType.Int, 0, ParameterDirection.Input, vehicle.DimensionId);
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
                throw new DataAccessException(Convert.ToString(UserSession.GetUserSession().LoginID), "vehicleDAL", "Insert", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
            //return true;
        }

        public bool Update(Vehicle vehicle, out string result)
        {
            result = string.Empty;
            try
            {
                using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {

                        conn.AddCommand("[Comm].[AddEditVehicle]", QueryType.Procedure);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_VehicleId", SqlDbType.Int, 0, ParameterDirection.InputOutput, vehicle.VehicleID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_VehicleNo", SqlDbType.NVarChar, 100, ParameterDirection.Input, CSubs.PSafeValue(vehicle.VehicleNo));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_VendorID", SqlDbType.Int, 0, ParameterDirection.Input, vehicle.VendorID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Category", SqlDbType.VarChar, 50, ParameterDirection.Input, CSubs.PSafeValue(vehicle.Category));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Capacity", SqlDbType.Float, 0, ParameterDirection.Input, vehicle.Capacity);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_BranchId", SqlDbType.Int, 0, ParameterDirection.Input, vehicle.BranchID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_VehicleType", SqlDbType.VarChar, 2, ParameterDirection.Input, "O");
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Vehicle_Cost", SqlDbType.Float, 0, ParameterDirection.Input, vehicle.Cost);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IsActive", SqlDbType.Bit, 0, ParameterDirection.Input, vehicle.IsActive);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LOGINID", SqlDbType.Int, 0, ParameterDirection.Input, UserSession.GetUserSession().LoginID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CompID", SqlDbType.Int, 0, ParameterDirection.Input, vehicle.CompID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@RETURNSTATUS", SqlDbType.SmallInt, 0, ParameterDirection.ReturnValue);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@Out_Message", SqlDbType.NVarChar, 500, ParameterDirection.Output);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@sp_dimensionid", SqlDbType.Int, 0, ParameterDirection.Input, vehicle.DimensionId);
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
                throw new DataAccessException(Convert.ToString(UserSession.GetUserSession().LoginID), "vehicleDAL", "Update", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
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

                        conn.AddCommand("Comm.vehicleDelete", QueryType.Procedure);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_vehicleID", SqlDbType.Int, 0, ParameterDirection.Input, id);
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
                throw new DataAccessException(Convert.ToString(UserSession.GetUserSession().LoginID), "vehicleDAL", "DeleteById", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

        }

        public DataTable GetDetailById(int? id, int LoginID)
        {
            DataTable vehicleDetailDt = new DataTable();

            try
            {
                string query = string.Format("exec [Comm].[GETVehicleDetail] @SP_vehicleID={0},@SP_LoginID={1}",
                 CSubs.QSafeValue(Convert.ToString(id)), CSubs.QSafeValue(Convert.ToString(LoginID)));
                vehicleDetailDt = CSubs.GetDataTable(query);
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoginID), "vehicleDAL", "GetDetailById", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

            return vehicleDetailDt;

        }

        public IEnumerable<Vehicle> GetvehicleList(int pPageIndex, int pPageSize, string pOrderBy, int pOrder, int? BranchID, int? VendorID, int? pisActive, string SearchKey, out int TotalCount)
        {
            TotalCount = 0;
            try
            {
                string query = string.Format("exec Comm.GETVehicleForGrid @SP_PageIndex={0},@SP_PageSize={1},@SP_OrderBy={2},@SP_Order={3},@SP_BranchID={4},@SP_VendorID={5},@SP_isActive={6},@SP_SearchString={7},@SP_LoginID={8},@SP_CompID={9}",
                 CSubs.QSafeValue(Convert.ToString(pPageIndex)),
                CSubs.QSafeValue(Convert.ToString(pPageSize)),
                CSubs.QSafeValue(pOrderBy),
                CSubs.QSafeValue(Convert.ToString(pOrder)),
                CSubs.QSafeValue(Convert.ToString(BranchID)),
                CSubs.QSafeValue(Convert.ToString(VendorID)),
                 CSubs.QSafeValue(Convert.ToString(pisActive)),
                CSubs.QSafeValue(SearchKey),
                Convert.ToString(UserSession.GetUserSession().LoginID),
                Convert.ToString(UserSession.GetUserSession().CompanyID)
                );

                DataTable dataTable = CSubs.GetDataTable(query);
                if (dataTable != null && dataTable.Rows.Count > 0)
                {
                    TotalCount = Convert.ToInt32(Convert.ToString(dataTable.Rows[0]["TotalRows"]));

                    var result = (from rw in dataTable.AsEnumerable()
                                  select new Vehicle()
                                  {
                                      VehicleID = Convert.ToInt32(rw["Vehicle_Id"]),
                                      VehicleNo = Convert.ToString(rw["Vehicle_No"]),
                                      VehicleType = Convert.ToString(rw["Vehicle_Type"]),
                                      VendorID = Convert.ToInt32(rw["Vendor_ID"]),
                                      VendorName = Convert.ToString(rw["VendorName"]),
                                      Capacity = Convert.ToDouble(rw["Capacity"]),
                                      Cost = Convert.ToDouble(rw["Vehicle_Cost"]),
                                      Category = Convert.ToString(rw["Category"]),
                                      BranchID = Convert.ToInt32(rw["Branch_Id"]),
                                      BranchName = Convert.ToString(rw["BranchName"]),
                                      IsActive = Convert.ToBoolean(rw["IsActive"]),
                                      CompID = Convert.ToInt32(rw["CompID"]),
                                  }).ToList();


                    return result;
                }

                return new List<Vehicle>();
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(UserSession.GetUserSession().LoginID), "vehicleDAL", "GetvehicleList", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

            //return null;

        }
    }
}