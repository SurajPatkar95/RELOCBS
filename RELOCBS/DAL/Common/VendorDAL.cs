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
    public class VendorDAL
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

        public bool Insert(Vendor model, out string result)
        {
            result = string.Empty;

            try
            {

                using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {
                        
                        conn.AddCommand("[Comm].[AddEditVendor]", QueryType.Procedure);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Vendor_ID", SqlDbType.Int, 0, ParameterDirection.InputOutput, -1);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Vendor_RefCode", SqlDbType.NVarChar, 50, ParameterDirection.Input, CSubs.PSafeValue(model.Vendor_RefCode));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Vendor_Name", SqlDbType.NVarChar, 100, ParameterDirection.Input, CSubs.PSafeValue(model.Vendor_Name));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Address", SqlDbType.NVarChar, 500, ParameterDirection.Input, CSubs.PSafeValue(model.Address));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Oper_MailID", SqlDbType.NVarChar, 500, ParameterDirection.Input, CSubs.PSafeValue(model.Oper_MailID));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Finance_MailID", SqlDbType.NVarChar, 500, ParameterDirection.Input, CSubs.PSafeValue(model.Finance_MailID));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CONTACT_PERSON", SqlDbType.NVarChar, 250, ParameterDirection.Input, CSubs.PSafeValue(model.CONTACT_PERSON));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CONTACT_NUMBER", SqlDbType.VarChar, 20, ParameterDirection.Input, CSubs.PSafeValue(model.CONTACT_NUMBER));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CONTACT_FAX_NUMBER", SqlDbType.NVarChar, 250, ParameterDirection.Input, CSubs.PSafeValue(model.CONTACT_FAX_NUMBER));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_PAN_NO", SqlDbType.NVarChar, 50, ParameterDirection.Input, CSubs.PSafeValue(model.PAN_NO));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_GST_NO", SqlDbType.NVarChar, 50, ParameterDirection.Input, CSubs.PSafeValue(model.GST_NO));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IsActive", SqlDbType.Bit, 0, ParameterDirection.Input, model.IsActive);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LOGINID", SqlDbType.Int, 0, ParameterDirection.Input, UserSession.GetUserSession().LoginID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CompID", SqlDbType.Int, 0, ParameterDirection.Input, UserSession.GetUserSession().CompanyID);
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
                throw new DataAccessException(Convert.ToString(UserSession.GetUserSession().LoginID), "VendorDAL", "Insert", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
            //return true;
        }

        public bool Update(Vendor model, out string result)
        {
            result = string.Empty;
            try
            {
                using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {
                        
                        
                        conn.AddCommand("[Comm].[AddEditVendor]", QueryType.Procedure);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Vendor_ID", SqlDbType.Int, 0, ParameterDirection.InputOutput, model.Vendor_ID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Vendor_RefCode", SqlDbType.NVarChar, 50, ParameterDirection.Input, CSubs.PSafeValue(model.Vendor_RefCode));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Vendor_Name", SqlDbType.NVarChar, 100, ParameterDirection.Input, CSubs.PSafeValue(model.Vendor_Name));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Address", SqlDbType.NVarChar, 500, ParameterDirection.Input, CSubs.PSafeValue(model.Address));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Oper_MailID", SqlDbType.NVarChar, 500, ParameterDirection.Input, CSubs.PSafeValue(model.Oper_MailID));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Finance_MailID", SqlDbType.NVarChar, 500, ParameterDirection.Input, CSubs.PSafeValue(model.Finance_MailID));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CONTACT_PERSON", SqlDbType.NVarChar, 250, ParameterDirection.Input, CSubs.PSafeValue(model.CONTACT_PERSON));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CONTACT_NUMBER", SqlDbType.VarChar, 20, ParameterDirection.Input, CSubs.PSafeValue(model.CONTACT_NUMBER));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CONTACT_FAX_NUMBER", SqlDbType.NVarChar, 250, ParameterDirection.Input, CSubs.PSafeValue(model.CONTACT_FAX_NUMBER));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_PAN_NO", SqlDbType.NVarChar, 50, ParameterDirection.Input, CSubs.PSafeValue(model.PAN_NO));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_GST_NO", SqlDbType.NVarChar, 50, ParameterDirection.Input, CSubs.PSafeValue(model.GST_NO));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IsActive", SqlDbType.Bit, 0, ParameterDirection.Input, model.IsActive);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LOGINID", SqlDbType.Int, 0, ParameterDirection.Input, UserSession.GetUserSession().LoginID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CompID", SqlDbType.Int, 0, ParameterDirection.Input, UserSession.GetUserSession().CompanyID);
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
                            throw new ArgumentException(conn.ErrorMessage);

                    }
                    else
                        throw new ArgumentException(conn.ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(UserSession.GetUserSession().LoginID), "VendorDAL", "Update", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
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

                        conn.AddCommand("Comm.VendorDelete", QueryType.Procedure);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_VendorID", SqlDbType.Int, 0, ParameterDirection.Input, id);
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
                throw new DataAccessException(Convert.ToString(UserSession.GetUserSession().LoginID), "VendorDAL", "DeleteById", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

        }

        public DataTable GetDetailById(int? id, int LoginID)
        {
            DataTable CrewDetailDt = new DataTable();

            try
            {
                string query = string.Format("exec [Comm].[GETVendorDetail] @SP_VendorID={0},@SP_LoginID={1}",
                 CSubs.QSafeValue(Convert.ToString(id)), CSubs.QSafeValue(Convert.ToString(LoginID)));
                CrewDetailDt = CSubs.GetDataTable(query);
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoginID), "VendorDAL", "GetDetailById", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

            return CrewDetailDt;

        }

        public IEnumerable<Vendor> GetVendorList(int pPageIndex, int pPageSize, string pOrderBy, int pOrder, int? CountryID, int? CityID, int? pisActive, string SearchKey, out int TotalCount)
        {
            TotalCount = 0;
            try
            {
                string query = string.Format("exec [Comm].[GETVendorForGrid] @SP_PageIndex={0},@SP_PageSize={1},@SP_OrderBy={2},@SP_Order={3},@SP_isActive={4},@SP_SearchString={5},@SP_LoginID={6},@SP_CompanyID={7}",
                CSubs.QSafeValue(Convert.ToString(pPageIndex)),
                CSubs.QSafeValue(Convert.ToString(pPageSize)),
                CSubs.QSafeValue(pOrderBy),
                CSubs.QSafeValue(Convert.ToString(pOrder)),
                CSubs.QSafeValue(Convert.ToString(pisActive)),
                CSubs.QSafeValue(SearchKey),
                Convert.ToString(UserSession.GetUserSession().LoginID),
                Convert.ToString(UserSession.GetUserSession().CompanyID)
                );

                DataTable dataTable = CSubs.GetDataTable(query);
                if (dataTable != null && dataTable.Rows.Count > 0)
                {
                    TotalCount = Convert.ToInt32(Convert.ToString(dataTable.Rows[0]["TotalRows"]));
                }

                var result = (from rw in dataTable.AsEnumerable()
                              select new Vendor()
                              {
                                  Vendor_ID = Convert.ToInt32(rw["Vendor_ID"]),
                                  Vendor_RefCode = Convert.ToString(rw["Vendor_RefCode"]),
                                  Vendor_Name = Convert.ToString(rw["Vendor_Name"]),
                                  Address = Convert.ToString(rw["Address"]),
                                  Oper_MailID = Convert.ToString(rw["Oper_MailID"]),
                                  Finance_MailID = Convert.ToString(rw["Finance_MailID"]),
                                  CONTACT_PERSON = Convert.ToString(rw["CONTACT_PERSON"]),
                                  CONTACT_NUMBER = Convert.ToString(rw["CONTACT_NUMBER"]),
                                  CONTACT_FAX_NUMBER = Convert.ToString(rw["CONTACT_FAX_NUMBER"]),
                                  GST_NO = Convert.ToString(rw["GST_NO"]),
                                  PAN_NO = Convert.ToString(rw["PAN_NO"]),
                                  IsActive = Convert.ToBoolean(rw["IsActive"]),
                                  CompID = Convert.ToInt32(rw["CompID"]),
                              }).ToList();


                return result;
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(UserSession.GetUserSession().LoginID), "VendorDAL", "GetVendorList", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

            //return null;

        }



    }
}