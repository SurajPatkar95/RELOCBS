using RELOCBS.App_Code;
using RELOCBS.Common.ExceptionHandling;
using RELOCBS.Entities;
using RELOCBS.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Xml;

namespace RELOCBS.DAL.Pricing
{
    public class GeneralPricingDAL
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

        public bool InsertRate(GeneralPriceViewModel SaveRate, int LoginID, out string result)
        {
            result = String.Empty;

            try
            {
                using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {

                        XmlDocument xml = new XmlDocument();
                        XmlElement root = xml.CreateElement("CostHeadwiseDetails");
                        xml.AppendChild(root);


                        if (SaveRate.ShowConstHeads)
                        {
                            foreach (var item in SaveRate.CostHeadList)
                            {
 
                                XmlElement child = xml.CreateElement("CostHeadwiseDetail");
                                //XmlElement child1 = 
                                child.AppendChild(xml.CreateElement("CostHeadID")).InnerText = Convert.ToString(item.CostHeadID);
                                //XmlElement child2 = (XmlElement)
                                child.AppendChild(xml.CreateElement("NetAmt")).InnerText= Convert.ToString(item.Amount);
                                root.AppendChild(child);
                            }

                        }
                        else
                        {
                            XmlElement child = xml.CreateElement("CostHeadwiseDetail");
                            //XmlElement child2 = xml.CreateElement("child");
                            child.AppendChild(xml.CreateElement("CostHeadID")).InnerText = "0";
                            //XmlElement child2 = (XmlElement)
                            child.AppendChild(xml.CreateElement("NetAmt")).InnerText = Convert.ToString(SaveRate.Rate);
                            //child1.AppendChild(child2);
                            root.AppendChild(child);
                        }

                        string CostHeadXml = xml.OuterXml;

                        conn.AddCommand("[Rate].[AddEditAgentsRate]", QueryType.Procedure);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@Sp_EditDeleteInsert", SqlDbType.VarChar, 1, ParameterDirection.Input,CSubs.PSafeValue("I"));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_RateCompID", SqlDbType.BigInt, 0, ParameterDirection.Input, SaveRate.CompanyID);

                        if (SaveRate.RateComponentID == 1 || SaveRate.RateComponentID == 4)
                        {
                            conn.AddParameter(ParameterOF.PROCEDURE, "@SP_OrgCityID", SqlDbType.BigInt, 0, ParameterDirection.Input, SaveRate.FromLocationID);
                        }

                        if (SaveRate.RateComponentID == 1 || SaveRate.RateComponentID == 2)
                        {
                            conn.AddParameter(ParameterOF.PROCEDURE, "@SP_OrgPortID", SqlDbType.BigInt, 0, ParameterDirection.Input, SaveRate.ToLocationID);
                        }

                        if (SaveRate.RateComponentID == 2 || SaveRate.RateComponentID == 3)
                        {
                            conn.AddParameter(ParameterOF.PROCEDURE, "@SP_DestPortID", SqlDbType.BigInt, 0, ParameterDirection.Input, (SaveRate.ToLocationID));
                        }
                        if (SaveRate.RateComponentID == 3 || SaveRate.RateComponentID == 4)
                        {
                            conn.AddParameter(ParameterOF.PROCEDURE, "@SP_DestCityID", SqlDbType.BigInt, 0, ParameterDirection.Input, (SaveRate.ToLocationID));
                        }
                        
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_AgentID", SqlDbType.Int, 0, ParameterDirection.Input, (SaveRate.AgentID));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Mode", SqlDbType.VarChar, 10, ParameterDirection.Input,  CSubs.PSafeValue(SaveRate.ModeName));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@Sp_RMCID", SqlDbType.Int, 0, ParameterDirection.Input, (SaveRate.RMCID));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_BussLineID", SqlDbType.Int, 0, ParameterDirection.Input, (SaveRate.BusinessLineID));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_GoodsDescID", SqlDbType.Int, 0, ParameterDirection.Input, (SaveRate.GoodsDescriptionID));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_RateCurrId", SqlDbType.Int, 0, ParameterDirection.Input, (SaveRate.RateCurrencyID));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CompID", SqlDbType.Int, 0, ParameterDirection.Input, (SaveRate.RateComponentID));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_WeightUnitID", SqlDbType.Int, 0, ParameterDirection.Input, (SaveRate.WeightUnitID));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_WeightFm", SqlDbType.Float, 0, ParameterDirection.Input, (SaveRate.WeightUnitFrom));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_WeightTo", SqlDbType.Float, 0, ParameterDirection.Input, (SaveRate.WeightUnitTo));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_TransTimeFm", SqlDbType.Float, 0, ParameterDirection.Input, (SaveRate.TransitTimeFrom));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_TransTimeTo", SqlDbType.Float, 0, ParameterDirection.Input, (SaveRate.TransitTimeTo));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_BaseCurrID", SqlDbType.Int, 0, ParameterDirection.Input, (SaveRate.BaseCurrencyRateID));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_BaseCurrConversRate", SqlDbType.Int, 0, ParameterDirection.Input, (SaveRate.ConversionRate));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CostHeadWiseInfo", SqlDbType.Xml, -1, ParameterDirection.Input, (CostHeadXml));
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
                throw new DataAccessException(Convert.ToString(LoginID), "GeneralPricingDAL", "Insert", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
        }

        public IEnumerable<GeneralPrice> GetForGrid(int LoginID,int RateComponetID, int? Page=1)
        {
            
            try
            {
                string query = string.Format("exec [Rate].[ForGrid_GetUpdatedAgentRate]  @SP_LoginID={0},@SP_RateCompID={1},@Out_Message='' ", Convert.ToString(LoginID),CSubs.QSafeValue(Convert.ToString(RateComponetID)));

                DataTable dataTable = CSubs.GetDataTable(query);
                //if (dataTable != null && dataTable.Rows.Count > 0)
                //{
                //    TotalCount = Convert.ToInt32(Convert.ToString(dataTable.Rows[0]["TotalRows"]));
                //}

                var result = (from rw in dataTable.AsEnumerable()
                              select new GeneralPrice()
                              {
                                  WeightUnitFrom = Convert.ToInt64(rw["WeightFrom"]),
                                  WeightUnitTo = Convert.ToInt64(rw["WeightTo"]),
                                  RateCurrencyName = Convert.ToString(rw["RateCurr"]),
                                  RateReceived = Convert.ToString(rw["RateReceived"]),
                                  BaseCurrencyRateName = Convert.ToString(rw["BaseCurr"]),
                                  ConversionRate = Convert.ToUInt64(rw["BaseCurrConversRate"]),
                                  FromLocationName= Convert.ToString(rw["FromLoc"]),
                                  ToLocationName = Convert.ToString(rw["ToLoc"]),
                                  RMCName = Convert.ToString(rw["RMCName"]),
                                  AgentName=Convert.ToString(rw["AgentName"]),
                                  BusinessLineName = Convert.ToString(rw["AgentName"]),
                                  GoodsDescriptionName = Convert.ToString(rw["GoodsDescName"]),
                                  WeightUnitName = Convert.ToString(rw["WeightUnitName"])
                                  
                              }).ToList();


                return result;

            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoginID), "GeneralPricingDAL", "GetForGrid", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
        }

        public DataSet GetDetailById(int RateCompRateWtBatchID, int RateCompRateWtID,int  LoginID)
        {
            DataSet Dtobj = new DataSet();
            
            try
            {
                string query = string.Format("exec [Comm].[GETCityDetail] @SP_RateCompRateWtID={0},@SP_RateCompRateWtBatchID={1},@SP_LoginID={2}",
                 CSubs.QSafeValue(Convert.ToString(RateCompRateWtID)), CSubs.QSafeValue(Convert.ToString(RateCompRateWtBatchID)), CSubs.QSafeValue(Convert.ToString(LoginID)));
                Dtobj = CSubs.GetDataSet(query);
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoginID), "GeneralPricingDAL", "GetDetailById", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }


            return Dtobj;
        }
    }
}