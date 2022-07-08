using RELOCBS.App_Code;
using RELOCBS.Common.ExceptionHandling;
using RELOCBS.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using RELOCBS.Entities;

namespace RELOCBS.DAL.WH_Assessment
{
    public class WHAssessQuestionsDAL
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


        public IQueryable<Entities.WHAssessmentQuestionGrid> GetGrid(int? CategoryId, string Parameter, bool? RMCBuss = false)
        {
            int LoggedinUserID = UserSession.GetUserSession().LoginID;
            try
            {

                IQueryable<Entities.WHAssessmentQuestionGrid> data = null;

                CategoryId = CategoryId == null ? -1 : CategoryId;
                
                using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {
                        conn.AddCommand("[Warehouse].[GetWHAssessQuestionsGrid]", QueryType.Procedure);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CategoryId", SqlDbType.Int, 0, ParameterDirection.Input, CategoryId);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Parameter", SqlDbType.NVarChar, -1, ParameterDirection.Input, CSubs.PSafeValue(Parameter));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CompId", SqlDbType.Int, 0, ParameterDirection.Input, UserSession.GetUserSession().CompanyID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IsRMCBuss", SqlDbType.Bit, 1, ParameterDirection.Input, RMCBuss);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LoginID", SqlDbType.Int, 0, ParameterDirection.Input, UserSession.GetUserSession().LoginID);
                        DataTable dt = (DataTable)conn.ExecuteProcedure(ProcedureReturnType.DataTable);

                        if (dt != null)
                        {
                            var result = (from item in dt.AsEnumerable()
                                          select new Entities.WHAssessmentQuestionGrid()
                                          {
                                              QuestionId = Convert.ToInt32(item["QuestionId"]) ,
                                              QuestionOrder = Convert.ToInt16(item["QuestionOrder"]),
                                              Parameter = Convert.ToString(item["Parameter"]),
                                              Desired = Convert.ToString(item["Desired"]),
                                              CategoryName = Convert.ToString(item["CategoryName"]),
                                              ResponsibilityName = Convert.ToString(item["ResponsibilityName"]),
                                              PriorityName = Convert.ToString(item["PriorityName"]),
                                              Score = Convert.ToInt32(item["Score"]),
                                              EffectiveFrom = Convert.ToDateTime(item["EffectiveFrom"]),
                                              IsActive = Convert.ToBoolean(item["IsActive"])
                                          }).ToList();
                            data = result.AsQueryable<Entities.WHAssessmentQuestionGrid>();
                        }
                    }
                    else
                        throw new Exception(conn.ErrorMessage);
                }
                return data;
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoggedinUserID), "WHAssessQuestionsDAL", "GetGrid", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

            //return null;


        }

        public DataTable GetDetail(int LoginID, Int64 TransId)
        {
            DataTable Dtobj = new DataTable();

            try
            {
                string query = string.Format("[Warehouse].[GetWHAssessQuestionDetail] @SP_QuestionId={0},@SP_LoginId={1}",
                CSubs.QSafeValue(Convert.ToString(TransId)), CSubs.QSafeValue(Convert.ToString(LoginID)));
                Dtobj = CSubs.GetDataTable(query);

            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoginID), "WHAssessQuestionsDAL", "GetDetail", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

            return Dtobj;
        }

        public DataTable GetWarehouseDetail(int LoginID, Int64 WarehouseId)
        {
            DataTable Dtobj = new DataTable();

            try
            {
                string query = string.Format("[Comm].[GetWarehouseDetail] @SP_WarehouseID={0},@SP_LoginID={1}",
                CSubs.QSafeValue(Convert.ToString(WarehouseId)), CSubs.QSafeValue(Convert.ToString(LoginID)));

                Dtobj = CSubs.GetDataTable(query);

            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoginID), "WHAssessQuestionsDAL", "GetWarehouseDetail", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

            return Dtobj;
        }

        public bool Insert(WHAssessmentQuestions model, int LoginID, out string result)
        {
            try
            {
                bool RMCBuss = false;
                if (UserSession.GetUserSession().BussinessLine == "NON RMC-BUSINESS")
                {
                    RMCBuss = false;
                }
                else
                {
                    RMCBuss = true;
                }
                
                using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {

                        conn.AddCommand("[Warehouse].[AddEditWHAssessQuestion]", QueryType.Procedure);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_QuestionId", SqlDbType.Int, 0, ParameterDirection.InputOutput, model.QuestionId);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_QuestionOrder", SqlDbType.SmallInt, 0, ParameterDirection.Input, model.QuestionOrder);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Parameter", SqlDbType.VarChar, -1, ParameterDirection.Input, CSubs.PSafeValue(model.Parameter));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Desired", SqlDbType.VarChar, -1, ParameterDirection.Input, CSubs.PSafeValue(model.Desired));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CategoryId", SqlDbType.Int, 0, ParameterDirection.Input, model.CategoryId);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_PriorityId", SqlDbType.Float, 0, ParameterDirection.Input, model.PriorityId);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ResponsibilityId", SqlDbType.Int, 0, ParameterDirection.Input, model.ResponsibilityId);

                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Score", SqlDbType.VarChar, -1, ParameterDirection.Input, model.Score);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IsActive", SqlDbType.VarChar, -1, ParameterDirection.Input, model.IsActive);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_EffectiveFrom", SqlDbType.DateTime, 0, ParameterDirection.Input, model.EffectiveFrom);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_INACTIVEDATE", SqlDbType.DateTime, 0, ParameterDirection.Input, model.EffectiveTo);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IsRMC", SqlDbType.Int, 0, ParameterDirection.Input, RMCBuss);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@sp_CompID", SqlDbType.Int, 0, ParameterDirection.Input, UserSession.GetUserSession().CompanyID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LoginID", SqlDbType.Int, 0, ParameterDirection.Input, UserSession.GetUserSession().LoginID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@RETURNSTATUS", SqlDbType.SmallInt, 0, ParameterDirection.ReturnValue);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_OutMsg", SqlDbType.NVarChar, -1, ParameterDirection.Output);
                        conn.ExecuteProcedure(ProcedureReturnType.SingleValue);

                        if (!conn.IsError)
                        {
                            int ReturnStatus = Convert.ToInt32(conn.GetParameterValue(ParameterOF.PROCEDURE, "@RETURNSTATUS"));
                            result = Convert.ToString(conn.GetParameterValue(ParameterOF.PROCEDURE, "@SP_OutMsg"));

                            if (ReturnStatus == 0)
                            {
                                model.QuestionId = Convert.ToInt32(conn.GetParameterValue(ParameterOF.PROCEDURE, "@SP_QuestionId"));
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
                throw new DataAccessException(Convert.ToString(LoginID), "WHAssessQuestionsDAL", "Insert", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
        }

        public bool Delete(Int64 ID, int LoginID, out string message)
        {
            message = string.Empty;
            try
            {
                using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {
                        conn.AddCommand("[Warehouse].[DeleteWHAssessQuestion]", QueryType.Procedure);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_QuestionId", SqlDbType.BigInt, 0, ParameterDirection.Input, (ID));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LoginID", SqlDbType.Int, 0, ParameterDirection.Input, UserSession.GetUserSession().LoginID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@RETURNSTATUS", SqlDbType.SmallInt, 0, ParameterDirection.ReturnValue);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_MESSAGE", SqlDbType.NVarChar, -1, ParameterDirection.Output);
                        conn.ExecuteProcedure(ProcedureReturnType.SingleValue);

                        if (!conn.IsError)
                        {
                            int ReturnStatus = Convert.ToInt32(conn.GetParameterValue(ParameterOF.PROCEDURE, "@RETURNSTATUS"));
                            message = Convert.ToString(conn.GetParameterValue(ParameterOF.PROCEDURE, "@SP_MESSAGE"));

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
                throw new DataAccessException(Convert.ToString(LoginID), "WHAssessQuestionsDAL", "Delete", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

        }
    }
}