using RELOCBS.App_Code;
using RELOCBS.Common.ExceptionHandling;
using RELOCBS.Entities;
using RELOCBS.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace RELOCBS.DAL.Common
{
    public class CrewDAL
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


        public bool Insert(Crew Crew, out string result)
        {
            result = string.Empty;

            try
            {

                using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {

                        var xEle = new XElement("Crews", from emp in Crew.members
                                                            select new XElement("Crew",
                                                   new XElement("EmpID", emp.EmpID),
                                                   new XElement("CWMID", emp.CWMID)
                                                   
                                               ));

                        string CrewMembersXml= xEle.ToString();

                        conn.AddCommand("[Comm].[AddEditCrew]", QueryType.Procedure);
                        //conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CrewID", SqlDbType.Int, 0, ParameterDirection.InputOutput, Crew.CompID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CrewCode", SqlDbType.NVarChar, 100, ParameterDirection.Input, CSubs.PSafeValue(Crew.CrewCode));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_SupervisorID", SqlDbType.Int, 0, ParameterDirection.Input, Crew.SuperviserID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ServiceLineID", SqlDbType.Int, 0, ParameterDirection.Input, Crew.ServiceLineID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CrewMembersXml", SqlDbType.Xml, -1, ParameterDirection.Input, CrewMembersXml);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IsActive", SqlDbType.Bit, 0, ParameterDirection.Input, true);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CompID", SqlDbType.Int, 0, ParameterDirection.Input, Crew.CompID);
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
                throw new DataAccessException(Convert.ToString(UserSession.GetUserSession().LoginID), "CrewDAL", "Insert", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
            //return true;
        }

        public bool Update(Crew Crew, out string result)
        {
            result = string.Empty;
            try
            {
                using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {

                        var xEle = new XElement("Crews", from emp in Crew.members
                                                         select new XElement("Crew",
                                                new XElement("EmpID", emp.EmpID),
                                                new XElement("CWMID", emp.CWMID)

                                            ));

                        string CrewMembersXml = xEle.ToString();
                        conn.AddCommand("[Comm].[AddEditCrew]", QueryType.Procedure);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CrewID", SqlDbType.Int, 0, ParameterDirection.InputOutput, Crew.CrewID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CrewCode", SqlDbType.NVarChar, 100, ParameterDirection.Input, CSubs.PSafeValue(Crew.CrewCode));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_SupervisorID", SqlDbType.Int, 0, ParameterDirection.Input, Crew.SuperviserID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ServiceLineID", SqlDbType.Int, 0, ParameterDirection.Input, Crew.ServiceLineID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CrewMembersXml", SqlDbType.Xml, -1, ParameterDirection.Input, CrewMembersXml);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CompID", SqlDbType.Int, 0, ParameterDirection.Input, Crew.CompID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IsActive", SqlDbType.Bit, 0, ParameterDirection.Input, Crew.IsActive);
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
                throw new DataAccessException(Convert.ToString(UserSession.GetUserSession().LoginID), "CrewDAL", "Update", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
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

                        conn.AddCommand("Comm.CrewDelete", QueryType.Procedure);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CrewID", SqlDbType.Int, 0, ParameterDirection.Input, id);
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
                throw new DataAccessException(Convert.ToString(UserSession.GetUserSession().LoginID), "CrewDAL", "DeleteById", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

        }

        public DataSet GetDetailById(int? id, int LoginID)
        {
            DataSet CrewDetailDt = new DataSet();

            try
            {
                string query = string.Format("exec [Comm].[GETCrewDetail] @SP_CrewID={0},@SP_LoginID={1}",
                 CSubs.QSafeValue(Convert.ToString(id)), CSubs.QSafeValue(Convert.ToString(LoginID)));
                CrewDetailDt = CSubs.GetDataSet(query);
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoginID), "CrewDAL", "GetDetailById", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

            return CrewDetailDt;

        }

        public IEnumerable<Crew> GetCrewList(int pPageIndex, int pPageSize, string pOrderBy, int pOrder, int? CountryID, int? CityID, int? pisActive, string SearchKey, out int TotalCount)
        {
            TotalCount = 0;
            try
            {
                string query = string.Format("exec Comm.GETCrewForGrid @SP_PageIndex={0},@SP_PageSize={1},@SP_OrderBy={2},@SP_Order={3},@SP_SupervisorID={4},@SP_CityID={5},@SP_isActive={6},@SP_SearchString={7},@SP_LoginID={8},@SP_CompID={9}",
                 CSubs.QSafeValue(Convert.ToString(pPageIndex)),
                CSubs.QSafeValue(Convert.ToString(pPageSize)),
                CSubs.QSafeValue(pOrderBy),
                CSubs.QSafeValue(Convert.ToString(pOrder)),
                CSubs.QSafeValue(Convert.ToString(CountryID)),
                CSubs.QSafeValue(Convert.ToString(CityID)),
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
                              select new Crew()
                              {
                                  CrewID = Convert.ToInt32(rw["CrewID"]),
                                  CrewCode = Convert.ToString(rw["CrewCode"]),
                                  SuperviserID = Convert.ToInt32(rw["SuperviserID"]),
                                  Superviser = Convert.ToString(rw["Superviser"]),
                                  IsActive = Convert.ToBoolean(rw["IsActive"])
                              }).ToList();


                return result;
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(UserSession.GetUserSession().CompanyID), "CrewDAL", "GetCrewList", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

            //return null;

        }


    }
}