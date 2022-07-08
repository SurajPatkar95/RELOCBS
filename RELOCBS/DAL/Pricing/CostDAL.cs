using Newtonsoft.Json;
using RELOCBS.App_Code;
using RELOCBS.Common.ExceptionHandling;
using RELOCBS.Entities;
using RELOCBS.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Xml;
using RELOCBS.Extensions;

namespace RELOCBS.DAL.Pricing
{
    public class CostDAL
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

        public bool InsertCost(CostViewModel SaveRate, int LoginID, out string result)
        {
            result = String.Empty;

            try
            {
                using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {


                        System.Xml.Linq.XNode node = JsonConvert.DeserializeXNode(SaveRate.HFVCostList, "CostHeadwiseDetails");
                        string CostHeadXml = node.ToString();

                        System.Xml.Linq.XNode node1 = JsonConvert.DeserializeXNode(SaveRate.HFSubCostList, "SubCostDetails");
                        string CostHeadXml1 = node1.ToString();

                        conn.AddCommand("[Rate].[AddEditAgentsRate]", QueryType.Procedure);
                        //conn.AddParameter(ParameterOF.PROCEDURE, "@Sp_EditDeleteInsert", SqlDbType.VarChar, 1, ParameterDirection.Input, CSubs.PSafeValue("I"));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CompanyID", SqlDbType.BigInt, 0, ParameterDirection.Input, SaveRate.CompanyID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_SurveyID", SqlDbType.BigInt, 0, ParameterDirection.Input, SaveRate.SurveyID > 0 ? SaveRate.SurveyID : (int?)null);
                        //conn.AddParameter(ParameterOF.PROCEDURE, "@SP_EnqueryID", SqlDbType.BigInt, 0, ParameterDirection.Input, SaveRate.EnqID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_OrgCityID", SqlDbType.BigInt, 0, ParameterDirection.Input, SaveRate.FromLocationID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_OrgPortID", SqlDbType.BigInt, 0, ParameterDirection.Input, SaveRate.ExitPointID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_DestPortID", SqlDbType.BigInt, 0, ParameterDirection.Input, (SaveRate.EntryPointID));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_DestCityID", SqlDbType.BigInt, 0, ParameterDirection.Input, (SaveRate.ToLocationID));
                        //conn.AddParameter(ParameterOF.PROCEDURE, "@SP_AgentID", SqlDbType.Int, 0, ParameterDirection.Input, (SaveRate.AgentID));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ModeID", SqlDbType.Int, 0, ParameterDirection.Input, SaveRate.ModeID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@Sp_RMCID", SqlDbType.Int, 0, ParameterDirection.Input, (SaveRate.RMCID));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_BussLineID", SqlDbType.Int, 0, ParameterDirection.Input, (SaveRate.BusinessLineID));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_GoodsDescID", SqlDbType.Int, 0, ParameterDirection.Input, (SaveRate.GoodsDescriptionID));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_RateCurrId", SqlDbType.Int, 0, ParameterDirection.Input, (SaveRate.RateCurrencyID));
                        //conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CompID", SqlDbType.Int, 0, ParameterDirection.Input, (SaveRate.RateComponentID));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_WeightUnitID", SqlDbType.Int, 0, ParameterDirection.Input, (SaveRate.WeightUnitID));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_WeightFm", SqlDbType.Float, 0, ParameterDirection.Input, (SaveRate.WeightUnitFrom));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_WeightTo", SqlDbType.Float, 0, ParameterDirection.Input, (SaveRate.WeightUnitTo));
                        //conn.AddParameter(ParameterOF.PROCEDURE, "@SP_TransTimeFm", SqlDbType.Float, 0, ParameterDirection.Input, (SaveRate.TransitTimeFrom));
                        //conn.AddParameter(ParameterOF.PROCEDURE, "@SP_TransTimeTo", SqlDbType.Float, 0, ParameterDirection.Input, (SaveRate.TransitTimeTo));
                        //conn.AddParameter(ParameterOF.PROCEDURE, "@SP_BaseCurrID", SqlDbType.Int, 0, ParameterDirection.Input, (SaveRate.BaseCurrencyRateID));
                        //conn.AddParameter(ParameterOF.PROCEDURE, "@SP_BaseCurrConversRate", SqlDbType.Int, 0, ParameterDirection.Input, (SaveRate.ConversionRate));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CostHeadWiseInfo", SqlDbType.Xml, -1, ParameterDirection.Input, (CostHeadXml));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_SubCostHeadWiseInfo", SqlDbType.Xml, -1, ParameterDirection.Input, (CostHeadXml1));

                        conn.AddParameter(ParameterOF.PROCEDURE, "@Sp_ShipingLineID", SqlDbType.Int, 0, ParameterDirection.Input, SaveRate.ShipingLineID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@Sp_TransitAgentID", SqlDbType.Int, 0, ParameterDirection.Input, SaveRate.TransitAgentID);
                        /////optional parameters
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_RateCompRateID", SqlDbType.BigInt, 0, ParameterDirection.Input, 0);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_RateCompanyRatewtID", SqlDbType.BigInt, 0, ParameterDirection.Input, SaveRate.RateCompRateWtID);

                        //conn.AddParameter(ParameterOF.PROCEDURE, "@SP_RateCompanyRateID", SqlDbType.BigInt, 0, ParameterDirection.Input, 0);

                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_RateCompRateBatchId", SqlDbType.BigInt, 0, ParameterDirection.Input, SaveRate.RateCompRateBatchId);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Remarks", SqlDbType.VarChar, 500, ParameterDirection.Input, SaveRate.Remarks);
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
                throw new DataAccessException(Convert.ToString(LoginID), "CostDAL", "Insert", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
        }

        public IEnumerable<Cost> GetForGrid(int LoginID, int RateComponetID, int? Page = 1)
        {

            try
            {
                string query = string.Format("exec [Rate].[ForGrid_GetUpdatedAgentRate]  @SP_LoginID={0},@SP_OrgCityID={1},@SP_OrgPortID={2},@SP_DestPortID={3},@SP_DestCityID={4}", Convert.ToString(LoginID), CSubs.QSafeValue(Convert.ToString(RateComponetID)));
                //@SP_AgentID,@SP_Mode ,@Sp_RMCID ,@SP_BussLineID ,@SP_GoodsDescID ,@SP_CompanyID ,

                DataTable dataTable = CSubs.GetDataTable(query);


                var result = (from rw in dataTable.AsEnumerable()
                              select new Cost()
                              {
                                  WeightUnitFrom = Convert.ToInt64(rw["WeightFrom"]),
                                  WeightUnitTo = Convert.ToInt64(rw["WeightTo"]),
                                  RateCurrencyName = Convert.ToString(rw["RateCurr"]),
                                  RateReceived = Convert.ToString(rw["RateReceived"]),
                                  BaseCurrencyRateName = Convert.ToString(rw["BaseCurr"]),
                                  ConversionRate = Convert.ToUInt64(rw["BaseCurrConversRate"]),
                                  FromLocationName = Convert.ToString(rw["FromLoc"]),
                                  ToLocationName = Convert.ToString(rw["ToLoc"]),
                                  RMCName = Convert.ToString(rw["RMCName"]),
                                  AgentName = Convert.ToString(rw["AgentName"]),
                                  BusinessLineName = Convert.ToString(rw["AgentName"]),
                                  GoodsDescriptionName = Convert.ToString(rw["GoodsDescName"]),
                                  WeightUnitName = Convert.ToString(rw["WeightUnitName"])

                              }).ToList();


                return result;

            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoginID), "CostDAL", "GetForGrid", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
        }

        public DataSet GetDetailById(int? SurveyID, int? RateCompRateWtID, int LoginID, int RateCompRateBatchId)
        {
            DataSet Dtobj = new DataSet();

            try
            {
                string query = string.Format("exec [Rate].[RateMasterDetailsForSurvey] @SP_SurveryID={0},@SP_RateCompRateWtID={1},@SP_LoginID={2},@SP_RateCompRateBatchId={3}",
                 CSubs.QSafeValue(Convert.ToString(SurveyID)), CSubs.QSafeValue(Convert.ToString(RateCompRateWtID)), CSubs.QSafeValue(Convert.ToString(LoginID)), CSubs.QSafeValue(Convert.ToString(RateCompRateBatchId)));
                Dtobj = CSubs.GetDataSet(query);
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoginID), "CostDAL", "GetDetailById", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }


            return Dtobj;
        }


        public DataSet GetGridFromCity(int LoginID, int? FromCityID, int? ToCityID, int? ExitPortID = -1, int? EntryPortID = -1)
        {


            try
            {
                string query = string.Format("exec [Rate].[RateMasterDetailsForSurvey] @SP_FromCityID={0},@SP_ToCityID={1},@SP_OrgPortID={2},@SP_DestPortID={3},@SP_LoginID={4}",
                CSubs.QSafeValue(Convert.ToString(FromCityID)), CSubs.QSafeValue(Convert.ToString(ToCityID))
                , CSubs.QSafeValue(Convert.ToString(ExitPortID)), CSubs.QSafeValue(Convert.ToString(EntryPortID))
                , CSubs.QSafeValue(Convert.ToString(LoginID)));

                return CSubs.GetDataSet(query);

            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoginID), "CostDAL", "GetGridFromCity", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
        }

        public DataTable GetCitywiseAccessServiceRate(int cityid, int rmcid, int CompanyID, int LoginID)
        {
            DataTable dataTable = new DataTable();

            try
            {

                #region Test Data
                dataTable.Columns.Add("AdditionalServicesID");
                dataTable.Columns.Add("ServiceName");
                dataTable.Columns.Add("Agnet 1");
                dataTable.Columns.Add("Agnet 2");
                dataTable.Columns.Add("Agnet 3");
                dataTable.Columns.Add("Agnet 4");
                dataTable.Columns.Add("Rate");
                dataTable.Columns.Add("UOMID");
                dataTable.Columns.Add("WriterProfit");
                dataTable.Columns.Add("USDPM");
                dataTable.Columns.Add("CalculatedWBCOST");


                string querys = string.Format("exec [RMC].[GetCityCostForGrid] @SP_CityID={0},@SP_RMCID={1},@SP_CompanyID={2},@SP_LoginID={3}",
                CSubs.QSafeValue(Convert.ToString(cityid)), CSubs.QSafeValue(Convert.ToString(rmcid)),
                 CSubs.QSafeValue(Convert.ToString(CompanyID))
                , CSubs.QSafeValue(Convert.ToString(LoginID)));


                //var data2 = CSubs.GetDataTable(querys).AsEnumerable().Select(x => new
                //{
                //    CostHeadID = x.Field<System.Object>("CostHeadId"),
                //    CostHeadName = x.Field<System.Object>("CostHeadName"),
                //    AgentName = x.Field<System.Object>("AgentName"),
                //    CostVal = x.Field<System.Object>("CostVal")
                //});
                //.ToPivotTable(
                //    item => item.AgentName,
                //    item => item.CostHeadName,
                //    items => items.Any() ? items.Max(x => x.CostVal) : 0);

                var data2 = CSubs.GetDataTable(querys);

                DataTable temp1 = new DataTable();
                DataTable temp = (new DataView(data2)).ToTable(true, "CostHeadId", "CostHeadName");

                temp1 = (new DataView(data2)).ToTable(true, new string[] { "AgentName" });
                DataSet ds = new DataSet();

                foreach (DataRow item in data2.Rows)
                {
                    string col = item["AgentName"].ToString();
                    if (!temp.Columns.Contains(col))
                    {
                        temp.Columns.Add(col);
                    }

                    temp.AsEnumerable().Where(s => Convert.ToString(s["CostHeadId"]).Equals(item["CostHeadId"].ToString())).ToList()
                        .ForEach(D => D.SetField(col, item["CostVal"].ToString()));
                }

                temp.Columns["CostHeadId"].ColumnName = "AdditionalServicesID";
                temp.Columns["CostHeadName"].ColumnName = "ServiceName";

                temp.Columns.Add("Rate");
                temp.Columns.Add("UOMID");
                temp.Columns.Add("WriterProfit");
                temp.Columns.Add("USDPM");
                temp.Columns.Add("CalculatedWBCOST");
                temp.AcceptChanges();
                dataTable = temp;
                //dataTable.Rows.Add("1", "Test 1", 10, 0, 0, 10,8 ,1, 10, "P", 0);
                //dataTable.Rows.Add("2", "Test 2", 1, 10, 0, 10,0,1, 10, "P", 0);
                //dataTable.Rows.Add("3", "Test 3", 1, 10, 0, 10,4, 1, 10, "P", 0);
                //dataTable.Rows.Add("4", "Test 3", 1, 10, 0, 20,0, 1, 10, "P", 0);
                //dataTable.Rows.Add("5", "Test 3", 1, 10, 0, 10,5, 1, 10, "P", 0);
                #endregion



            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoginID), "CostDAL", "GetCitywiseAccessServiceRate", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

            return dataTable;

        }

        public bool Delete(int SurveyID, int RateCompRateWtID, int RateCompRateBatchID, int LoginID, out string message)
        {
            message = string.Empty;
            try
            {
                using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {


                        conn.AddCommand("[Rate].[DeleteEstimate]", QueryType.Procedure);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_SurveyID", SqlDbType.BigInt, 0, ParameterDirection.Input, (SurveyID));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_RateCompRateWtID", SqlDbType.BigInt, 0, ParameterDirection.Input, (RateCompRateWtID));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_RateCompRateBatchID", SqlDbType.BigInt, 0, ParameterDirection.Input, (RateCompRateBatchID));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LOGINID", SqlDbType.Int, 0, ParameterDirection.Input, UserSession.GetUserSession().LoginID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@RETURNSTATUS", SqlDbType.SmallInt, 0, ParameterDirection.ReturnValue);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@Out_Message", SqlDbType.NVarChar, 500, ParameterDirection.Output);
                        conn.ExecuteProcedure(ProcedureReturnType.SingleValue);

                        if (!conn.IsError)
                        {
                            int ReturnStatus = Convert.ToInt32(conn.GetParameterValue(ParameterOF.PROCEDURE, "@RETURNSTATUS"));
                            message = Convert.ToString(conn.GetParameterValue(ParameterOF.PROCEDURE, "@Out_Message"));

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
                throw new DataAccessException(Convert.ToString(LoginID), "QuotingDAL", "Delete", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
        }

        public List<HistoryRate> getHistoryRate(int LoginID, int ComponentID, int? RMCID, int? AgentID, int? FromCity, int? ToCity, int? ExitPort, int? EntryPort, char RevnOrCost, int? ExitPortAir, int? EntryPortAir)
        {

            try
            {
                string query = string.Format("exec [RMC].[GetHistoryRateForGrid]  @SP_LoginID={0},@SP_FromCityID={1},@SP_FromPortID={2},@SP_ToPortID={3},@SP_ToCityID={4},@SP_RMCID={5},@SP_CompanyID={6},@SP_AgentID={7},@SP_ComponentID={8},@SP_RevnOrCost={9},@SP_FromPortID_Air={10},@SP_ToPortID_Air={11}",
                    Convert.ToString(LoginID),
                    CSubs.QSafeValue(Convert.ToString(FromCity)),
                    CSubs.QSafeValue(Convert.ToString(ExitPort)),
                    CSubs.QSafeValue(Convert.ToString(EntryPort)),
                    CSubs.QSafeValue(Convert.ToString(ToCity)),
                    CSubs.QSafeValue(Convert.ToString(RMCID)),
                    CSubs.QSafeValue(Convert.ToString(UserSession.GetUserSession().CompanyID)),
                    CSubs.QSafeValue(Convert.ToString(AgentID)),
                    CSubs.QSafeValue(Convert.ToString(ComponentID)),
                    CSubs.QSafeValue(Convert.ToString(RevnOrCost)),
                    CSubs.QSafeValue(Convert.ToString(ExitPortAir)),
                    CSubs.QSafeValue(Convert.ToString(EntryPortAir))
                    );
                //@SP_AgentID,@SP_Mode ,@Sp_RMCID ,@SP_BussLineID ,@SP_GoodsDescID ,@SP_CompanyID ,

                DataTable dataTable = CSubs.GetDataTable(query);

                if (dataTable != null && dataTable.Rows.Count > 0)
                {
                    var result = (from rw in dataTable.AsEnumerable()
                                  select new HistoryRate()
                                  {
                                      WeightFrom = rw["WeightFrom"] == DBNull.Value ? ((float?)null) : Convert.ToInt64(rw["WeightFrom"]),
                                      WeightTo = rw["WeightTo"] == DBNull.Value ? ((float?)null) : Convert.ToInt64(rw["WeightTo"]),
                                      AgentName = rw["AgentName"] == DBNull.Value ? "" : Convert.ToString(rw["AgentName"]),
                                      EffectiveFrom = rw["EffectiveFrom"] == DBNull.Value ? ((DateTime?)null) : Convert.ToDateTime(rw["EffectiveFrom"]),
                                      EffectiveTo = rw["EffectiveTo"] == DBNull.Value ? ((DateTime?)null) : Convert.ToDateTime(rw["EffectiveTo"]),
                                      CityName_1 = Convert.ToString(rw["FromCity"]),
                                      PortName_1 = Convert.ToString(rw["ExitPort"]),
                                      PortName_2 = Convert.ToString(rw["EntryPort"]),
                                      CityName_2 = Convert.ToString(rw["ToCity"]),
                                      RateItemVal1 = Convert.ToString(rw["RateItemVal1"]),
                                      RateItemVal2 = Convert.ToString(rw["RateItemVal2"]),
                                      RateItemVal3 = rw["RateItemVal3"] == DBNull.Value ? "" : Convert.ToString(rw["RateItemVal3"]),
                                      RateItemVal4 = rw["RateItemVal4"] == DBNull.Value ? "" : Convert.ToString(rw["RateItemVal4"]),
                                      RateItemVal5 = rw["RateItemVal5"] == DBNull.Value ? "" : Convert.ToString(rw["RateItemVal5"]),
                                      RateItemVal6 = rw["RateItemVal6"] == DBNull.Value ? "" : Convert.ToString(rw["RateItemVal6"]),
                                      Isactive = rw["Isactive"] == DBNull.Value ? (Boolean?)null : Convert.ToBoolean(rw["Isactive"]),
                                      OrgRMCAgentEffectDateID = Convert.ToInt64(rw["RMCAgentEffectDateID"]),
                                      ModeName = Convert.ToString(rw["TransportModeName"]),
                                      RateCurr = Convert.ToString(rw["RateCurr"]),
                                      ConvRate = Convert.ToString(rw["ConvRate"]),
                                  }).ToList();


                    foreach (var item in result)
                    {
                        item.rates.Add(new Rate()); //= new List<Rate>().Add(new Rate());
                        #region Not to use this 
                        //query = string.Format("exec [RMC].[GetHistoryRateForGrid]  @SP_LoginID={0},@SP_FromCityID={1},@SP_FromPortID={2},@SP_ToPortID={3},@SP_ToCityID={4},@SP_RMCID={5},@SP_CompanyID={6},@SP_AgentID={7},@SP_ComponentID={8},@SP_RMCAgentEffectDateID={9},@SP_RevnOrCost={10}",
                        // Convert.ToString(LoginID),
                        // CSubs.QSafeValue(Convert.ToString(FromCity)),
                        // CSubs.QSafeValue(Convert.ToString(ExitPort)),
                        // CSubs.QSafeValue(Convert.ToString(EntryPort)),
                        // CSubs.QSafeValue(Convert.ToString(ToCity)),
                        // CSubs.QSafeValue(Convert.ToString(RMCID)),
                        // CSubs.QSafeValue(Convert.ToString(UserSession.GetUserSession().CompanyID)),
                        // CSubs.QSafeValue(Convert.ToString(AgentID)),
                        // CSubs.QSafeValue(Convert.ToString(ComponentID)),
                        // CSubs.QSafeValue(Convert.ToString(item.OrgRMCAgentEffectDateID)),
                        // CSubs.QSafeValue(Convert.ToString(RevnOrCost))

                        // );


                        //   dataTable = CSubs.GetDataTable(query);

                        //   if (dataTable != null && dataTable.Rows.Count > 0)
                        //   {
                        //       item.rates = (from rw in dataTable.AsEnumerable()
                        //                     select new Rate()
                        //                     {
                        //                         WeightFrom = Convert.ToString(rw["WeightFrom"]),
                        //                         WeightTo = Convert.ToString(rw["WeightTo"]),
                        //                         AgentName = Convert.ToString(rw["AgentName"]),
                        //                         EffectiveFrom = Convert.ToString(rw["EffectiveFrom"]),
                        //                         EffectiveTo = Convert.ToString(rw["EffectiveTo"]),
                        //                         //EffectiveTo = rw["EffectiveTo"] == DBNull.Value ? ((DateTime?)null) : Convert.ToString(rw["EffectiveTo"]),
                        //                         //CityName_1 = Convert.ToString(rw["CityName"]),
                        //                         //PortName_1 = Convert.ToString(rw["PortName"]),
                        //                         //PortName_2 = Convert.ToString(rw["PortName1"]),
                        //                         //CityName_2 = Convert.ToString(rw["CityName"]),
                        //                         RateItemVal1 = Convert.ToString(rw["RateItemVal1"]),
                        //                         RateItemVal2 = Convert.ToString(rw["RateItemVal2"]),
                        //                         RateItemVal3 = Convert.ToString(rw["RateItemVal3"]),
                        //                         RateItemVal4 = Convert.ToString(rw["RateItemVal4"]),
                        //                         RateItemVal5 = Convert.ToString(rw["RateItemVal5"]),
                        //                         RateItemVal6 = Convert.ToString(rw["RateItemVal6"]),
                        //                         RateItemVal7 = Convert.ToString(rw["NetVal"]),
                        //                         RateItemVal8 = Convert.ToString(rw["GrossVal"]),
                        //                         Isactive = Convert.ToString(rw["Isactive"]),
                        //                         //OrgRMCAgentEffectDateID = Convert.ToInt64(rw["RMCAgentEffectDateID"])
                        //                     }).ToList();


                        //   }


                        #endregion
                    }

                    return result;
                }

                return new List<HistoryRate>();

            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoginID), "CostDAL", "GetForGrid", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
        }

        public DataSet GetCompareRate(int SurveyID, int LoginID)
        {
            string querys = string.Format("EXEC [Rate].[GetComparableRates] @SP_SurveyID={0},@SP_LoginID={1}", CSubs.QSafeValue(Convert.ToString(SurveyID)),
                 CSubs.QSafeValue(Convert.ToString(LoginID)));

            DataSet data = CSubs.GetDataSet(querys);

            return data;

        }

        public CostUploadFormat GetCostUploadFormat(int LoginID, int RMCID, int ComponentID, string FileType, string CostOrRevenue, string Mode)
        {
            try
            {
                string query = string.Format("exec [Rate].[GetCostUploadFileFormat] @SP_LoginID={0},@SP_RMCID={1},@SP_ComponentID={2},@SP_FileType={3},@SP_CostOrRevenue={4},@sp_Mode={5}",
                    Convert.ToString(LoginID),
                    CSubs.QSafeValue(Convert.ToString(RMCID)),
                    CSubs.QSafeValue(Convert.ToString(ComponentID)),
                    CSubs.QSafeValue(Convert.ToString(FileType)),
                    CSubs.QSafeValue(Convert.ToString(CostOrRevenue)),
                    CSubs.QSafeValue(Convert.ToString(Mode))
                    );
                DataTable dataTable = CSubs.GetDataTable(query);

                if (dataTable != null && dataTable.Rows.Count > 0)
                {
                    var result = (from rw in dataTable.AsEnumerable()
                                  select new CostUploadFormat()
                                  {

                                      FileName = Convert.ToString(rw["FileName"]),
                                      ResourceName = Convert.ToString(rw["ResourceName"]),
                                      FileID = Convert.ToInt32(rw["FileID"])
                                  }).FirstOrDefault();

                    return result;
                }

                return new CostUploadFormat();

            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoginID), "CostDAL", "GetCostUploadFormat", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
        }

        public List<Rate> getSubHistoryRate(int LoginID, int ComponentID, int? RMCID, int? AgentID, int? FromCity, int? ToCity, int? ExitPort, int? EntryPort, char RevnOrCost, Int64 OrgRMCAgentEffectDateID, bool IsJobPage)
        {
            List<Rate> rates = new List<Rate>();
            try
            {
                string query = string.Format("exec [RMC].[GetHistoryRateForGrid]  @SP_LoginID={0},@SP_FromCityID={1},@SP_FromPortID={2},@SP_ToPortID={3},@SP_ToCityID={4},@SP_RMCID={5},@SP_CompanyID={6},@SP_AgentID={7},@SP_ComponentID={8},@SP_RMCAgentEffectDateID={9},@SP_RevnOrCost={10},@SP_IsFromCostSheet={11}",
                  Convert.ToString(LoginID),
                  CSubs.QSafeValue(Convert.ToString(FromCity)),
                  CSubs.QSafeValue(Convert.ToString(ExitPort)),
                  CSubs.QSafeValue(Convert.ToString(EntryPort)),
                  CSubs.QSafeValue(Convert.ToString(ToCity)),
                  CSubs.QSafeValue(Convert.ToString(RMCID)),
                  CSubs.QSafeValue(Convert.ToString(UserSession.GetUserSession().CompanyID)),
                  CSubs.QSafeValue(Convert.ToString(AgentID)),
                  CSubs.QSafeValue(Convert.ToString(ComponentID)),
                  CSubs.QSafeValue(Convert.ToString(OrgRMCAgentEffectDateID)),
                  CSubs.QSafeValue(Convert.ToString(RevnOrCost)),
                  IsJobPage
                  );

                DataTable dataTable = CSubs.GetDataTable(query);

                if (dataTable != null && dataTable.Rows.Count > 0)
                {
                    rates = (from rw in dataTable.AsEnumerable()
                             select new Rate()
                             {
                                 WeightFrom = Convert.ToString(rw["WeightFrom"]),
                                 WeightTo = Convert.ToString(rw["WeightTo"]),
                                 AgentName = Convert.ToString(rw["AgentName"]),
                                 EffectiveFrom = Convert.ToString(rw["EffectiveFrom"]),
                                 EffectiveTo = Convert.ToString(rw["EffectiveTo"]),
                                 //EffectiveTo = rw["EffectiveTo"] == DBNull.Value ? ((DateTime?)null) : Convert.ToString(rw["EffectiveTo"]),
                                 //CityName_1 = Convert.ToString(rw["CityName"]),
                                 //PortName_1 = Convert.ToString(rw["PortName"]),
                                 //PortName_2 = Convert.ToString(rw["PortName1"]),
                                 //CityName_2 = Convert.ToString(rw["CityName"]),
                                 RateItemVal1 = Convert.ToString(rw["RateItemVal1"]),
                                 RateItemVal2 = Convert.ToString(rw["RateItemVal2"]),
                                 RateItemVal3 = Convert.ToString(rw["RateItemVal3"]),
                                 RateItemVal4 = Convert.ToString(rw["RateItemVal4"]),
                                 RateItemVal5 = Convert.ToString(rw["RateItemVal5"]),
                                 RateItemVal6 = Convert.ToString(rw["RateItemVal6"]),
                                 RateItemVal7 = Convert.ToString(rw["NetVal"]),
                                 RateItemVal8 = Convert.ToString(rw["GrossVal"]),
                                 OrgPrice = Convert.ToString(rw["OrgPrice"]),
                                 Isactive = Convert.ToString(rw["Isactive"]),
                                 //OrgRMCAgentEffectDateID = Convert.ToInt64(rw["RMCAgentEffectDateID"])
                             }).ToList();


                }

                return rates;
            }
            catch (Exception)
            {

                throw;
            }

        }

        public DataTable GetCitywiseRevenue(int cityid, int rmcid, int CompanyID, int LoginID)
        {

            DataTable dataTable = new DataTable();

            try
            {

                string querys = string.Format("EXEC [RMC].[GetCityCostForGrid] @SP_CityID={0},@SP_RMCID={1},@SP_CompanyID={2},@SP_LoginID={3},@SP_IsRevenue=1",
                CSubs.QSafeValue(Convert.ToString(cityid)), CSubs.QSafeValue(Convert.ToString(rmcid)),
                 CSubs.QSafeValue(Convert.ToString(CompanyID))
                , CSubs.QSafeValue(Convert.ToString(LoginID)));

                dataTable = CSubs.GetDataTable(querys);

            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoginID), "CostDAL", "GetCitywiseRevenue", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

            return dataTable;
        }

        public List<HistorySplTHC> getBTRTHCHistory(int LoginID, int? Component, int? RMC, int? Agent, int? DestCity, int? OrgContinent)
        {
            try
            {
                string query = string.Format("exec [RMC].[BTR_Spl_THC_History] @SP_LoginID={0},@SP_OrgContinentID={1},@sp_DestCityID={2},@SP_RMCID={3},@SP_CompID={4},@SP_AgentID={5}",
                    Convert.ToString(LoginID),
                    CSubs.QSafeValue(Convert.ToString(OrgContinent)),
                    CSubs.QSafeValue(Convert.ToString(DestCity)),
                    CSubs.QSafeValue(Convert.ToString(RMC)),
                    CSubs.QSafeValue(Convert.ToString(UserSession.GetUserSession().CompanyID)),
                    CSubs.QSafeValue(Convert.ToString(Agent))
                    );
                DataTable dataTable = CSubs.GetDataTable(query);

                if (dataTable != null)
                {
                    var result = (from rw in dataTable.AsEnumerable()
                                  select new HistorySplTHC()
                                  {
                                      AgentName = rw["AgentName"] == DBNull.Value ? "" : Convert.ToString(rw["AgentName"]),
                                      EffectiveFrom = rw["EffectiveFrom"] == DBNull.Value ? ((DateTime?)null) : Convert.ToDateTime(rw["EffectiveFrom"]),
                                      EffectiveTo = rw["EffectiveTo"] == DBNull.Value ? ((DateTime?)null) : Convert.ToDateTime(rw["EffectiveTo"]),
                                      RMCName = Convert.ToString(rw["RMCName"]),
                                      DestCity = Convert.ToString(rw["DestCity"]),
                                      OrgContinentName = Convert.ToString(rw["OrgContinentName"]),
                                      Isactive = rw["Isactive"] == DBNull.Value ? (Boolean?)null : Convert.ToBoolean(rw["Isactive"]),
                                      MastTransId = Convert.ToInt64(rw["MastTransId"]),
                                      SubTHC = new List<SubHistorySplTHC>()
                                  }).ToList();


                    return result;
                }

                return new List<HistorySplTHC>();

            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoginID), "CostDAL", "getBTRTHCHistory", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
        }

        public List<SubHistorySplTHC> getSubBTRTHCHistory(int LoginID, int? Component, int? RMC, int? Agent, int? DestCity, int? OrgContinent, Int64? MastTransID)
        {
            try
            {
                string query = string.Format("exec [RMC].[BTR_Spl_THC_History] @SP_LoginID={0},@SP_OrgContinentID={1},@sp_DestCityID={2},@SP_RMCID={3},@SP_CompID={4},@SP_AgentID={5},@SP_MastTransId={6}",
                    Convert.ToString(LoginID),
                    CSubs.QSafeValue(Convert.ToString(OrgContinent)),
                    CSubs.QSafeValue(Convert.ToString(DestCity)),
                    CSubs.QSafeValue(Convert.ToString(RMC)),
                    CSubs.QSafeValue(Convert.ToString(UserSession.GetUserSession().CompanyID)),
                    CSubs.QSafeValue(Convert.ToString(Agent)),
                    CSubs.QSafeValue(Convert.ToString(MastTransID))
                    );
                DataTable dataTable = CSubs.GetDataTable(query);

                if (dataTable != null)
                {
                    var result = (from rw in dataTable.AsEnumerable()
                                  select new SubHistorySplTHC()
                                  {
                                      Mode = Convert.ToString(rw["Mode"]),
                                      EffectiveFrom = rw["EffectiveFrom"] == DBNull.Value ? ((DateTime?)null) : Convert.ToDateTime(rw["EffectiveFrom"]),
                                      EffectiveTo = rw["EffectiveTo"] == DBNull.Value ? ((DateTime?)null) : Convert.ToDateTime(rw["EffectiveTo"]),
                                      SlabValue = Convert.ToInt32(rw["SlabValue"]),
                                      THCValue = Convert.ToDecimal(rw["THCValue"]),
                                      Isactive = rw["Isactive"] == DBNull.Value ? (Boolean?)null : Convert.ToBoolean(rw["Isactive"]),
                                      MastTransId = Convert.ToInt64(rw["MastTransId"]),
                                      TransId = Convert.ToInt64(rw["TransId"]),
                                  }).ToList();


                    return result;
                }

                return new List<SubHistorySplTHC>();
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoginID), "CostDAL", "getSubBTRTHCHistory", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
        }

        public DataTable getRMCCompareRate(int LoginID, int ComponentID, int? RMCID, int? AgentID, int? FromCity, int? ToCity, int? ExitPort, int? EntryPort, char RevnOrCost, int? ExitPortAir, int? EntryPortAir)
        {

            try
            {
                string query = string.Format("exec [RMC].[GetrateComparisonReport] @SP_LoginID={0},@SP_OrgCityID={1},@SP_OrgPortIDSea={2},@SP_DestPortIDSea={3},@SP_DestCityID={4},@SP_RMCID={5},@SP_CompanyID={6},@SP_AgentID={7},@SP_RateComponentID={8},@SP_CostOrRev={9},@SP_OrgPortIDAir={10},@SP_DestPortIDAir={11}",
                    Convert.ToString(LoginID),
                    CSubs.QSafeValue(Convert.ToString(FromCity)),
                    CSubs.QSafeValue(Convert.ToString(ExitPort)),
                    CSubs.QSafeValue(Convert.ToString(EntryPort)),
                    CSubs.QSafeValue(Convert.ToString(ToCity)),
                    CSubs.QSafeValue(Convert.ToString(RMCID)),
                    CSubs.QSafeValue(Convert.ToString(UserSession.GetUserSession().CompanyID)),
                    CSubs.QSafeValue(Convert.ToString(AgentID)),
                    CSubs.QSafeValue(Convert.ToString(ComponentID)),
                    CSubs.QSafeValue(Convert.ToString(RevnOrCost)),
                    CSubs.QSafeValue(Convert.ToString(ExitPortAir)),
                    CSubs.QSafeValue(Convert.ToString(EntryPortAir))
                    );

                return CSubs.GetDataTable(query);
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoginID), "CostDAL", "getRMCCompareRate", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
        }

        public DataSet GetWHServiceCosts(int LoginID, Int64? SurveyID, Int64? RateCompRateID, Int64? RateCompRateBatchId)
        {
            DataSet WHServiceCostDs = new DataSet();
            try
            {
                string query = string.Format("EXEC [Rate].[GetWHServiceCosts] @SP_LoginID={0},@SP_SurveyID={1},@SP_RateCompRateID={2},@SP_RateCompRateBatchId={3}",
                    CSubs.QSafeValue(Convert.ToString(LoginID)), CSubs.QSafeValue(Convert.ToString(SurveyID)), CSubs.QSafeValue(Convert.ToString(RateCompRateID)), CSubs.QSafeValue(Convert.ToString(RateCompRateBatchId)));
                WHServiceCostDs = CSubs.GetDataSet(query);
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoginID), "CostDAL", "GetWHServiceCosts", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
            return WHServiceCostDs;
        }

        public bool SaveWHServiceCosts(CostViewModel cost, out string result)
        {
            result = string.Empty;
            try
            {
                using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {
                        string WHServiceCostXml = null;
                        if (!string.IsNullOrEmpty(cost.WHServiceCostListHidden))
                        {
                            System.Xml.Linq.XNode node = JsonConvert.DeserializeXNode(cost.WHServiceCostListHidden, "CostHeadwiseDetails");
                            WHServiceCostXml = node.ToString();
                        }

                        conn.AddCommand("[Rate].[AddEditWHServiceCosts]", QueryType.Procedure);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LoginID", SqlDbType.Int, 0, ParameterDirection.Input, UserSession.GetUserSession().LoginID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_SurveyID", SqlDbType.BigInt, 0, ParameterDirection.Input, cost.SurveyID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CostHeadWiseInfo_Service", SqlDbType.Xml, 0, ParameterDirection.Input, WHServiceCostXml);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@RETURNSTATUS", SqlDbType.SmallInt, 0, ParameterDirection.ReturnValue);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_OutMsg", SqlDbType.NVarChar, 500, ParameterDirection.Output);
                        conn.ExecuteProcedure(ProcedureReturnType.SingleValue);

                        if (!conn.IsError)
                        {
                            int ReturnStatus = Convert.ToInt32(conn.GetParameterValue(ParameterOF.PROCEDURE, "@RETURNSTATUS"));
                            result = Convert.ToString(conn.GetParameterValue(ParameterOF.PROCEDURE, "@SP_OutMsg"));

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
                throw new DataAccessException(Convert.ToString(UserSession.GetUserSession().LoginID), "CostDAL", "SaveWHServiceCosts", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
        }
    }
}