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
    public class CurrencyConversionDAL
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


        public bool Insert(DataTable exceldt, out string result)
        {
            result = string.Empty;

            try
            {

                exceldt.TableName = "UT_CurrConversionRates";

                using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {

                        conn.AddCommand("[Comm].[Insert_CurrConversionRates]", QueryType.Procedure);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CurrConversionRates", SqlDbType.Structured, 0, ParameterDirection.Input, exceldt);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LOGINID", SqlDbType.Int, 0, ParameterDirection.Input, UserSession.GetUserSession().LoginID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@RETURNSTATUS", SqlDbType.SmallInt, 0, ParameterDirection.ReturnValue);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_MESSAGE", SqlDbType.NVarChar, -1, ParameterDirection.Output);
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
                throw new DataAccessException(Convert.ToString(UserSession.GetUserSession().LoginID), "CurrencyConversionDAL", "Insert", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
            //return true;
        }

        public IEnumerable<CurrencyConversion> GetList(int pPageIndex, int pPageSize, string pOrderBy, int pOrder, int? CountryID, int? CityID, int? pisActive, string SearchKey, int LoggedinUserID, out int TotalCount)
        {
            TotalCount = 0;
            try
            {
                string query = string.Format("exec Comm.GETCurrConversionForGrid @SP_PageIndex={0},@SP_PageSize={1},@SP_OrderBy={2},@SP_Order={3},@SP_isActive={4},@SP_SearchString={5},@SP_LoginID={6}",
                    CSubs.QSafeValue(Convert.ToString(pPageIndex)),
                    CSubs.QSafeValue(Convert.ToString(pPageSize)),
                    CSubs.QSafeValue(pOrderBy),
                    CSubs.QSafeValue(Convert.ToString(pOrder)),
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
                              select new CurrencyConversion()
                              {
                                  CurrConvID = Convert.ToInt64(rw["CurrConvID"]),
                                  FIN_PERIOD = Convert.ToString(rw["FIN_PERIOD"]),
                                  Currency_Code = Convert.ToString(rw["Currency_Code"]),
                                  From_Curr_ID = Convert.ToInt32(rw["From_Curr_ID"]),
                                  Currency_Code_To = Convert.ToString(rw["Currency_Code_To"]),
                                  To_Curr_ID = Convert.ToInt32(rw["To_Curr_ID"]),
                                  From_Date = Convert.ToDateTime(rw["From_Date"]),
                                  To_Date = Convert.ToDateTime(rw["To_Date"]),
                                  ConversionRate = Convert.ToDecimal(rw["ConversionRate"]),
                                  Multiply_Divide = Convert.ToString(rw["Multiply_Divide"]),
                                  IsActive = Convert.ToBoolean(rw["IsActive"]),
                                  CreatedDate = Convert.ToDateTime(rw["CreatedDate"]),
                                  CreatedBy = Convert.ToString(rw["CreatedBy"]),
                              }).ToList();


                return result;
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoggedinUserID), "CurrencyConversionDAL", "GetList", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

            //return null;

        }
    }
}