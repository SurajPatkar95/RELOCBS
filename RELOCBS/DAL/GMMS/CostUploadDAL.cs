using Newtonsoft.Json;
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

namespace RELOCBS.DAL.GMMS
{
    public class CostUploadDAL
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

        public bool UploadRate(GMMSRateUpload R, int LoginID, out string result)
        {
            result = String.Empty;

            try
            {

                int ResultCount = 0;
                using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString(), true, System.Data.IsolationLevel.ReadCommitted))
                {
                    if (conn.Connect())
                    {
                        try
                        {
                            
                            DataTable dt = (DataTable)JsonConvert.DeserializeObject(R.CostRateList, (typeof(DataTable)));

                            /// To remove Amount Column for specific RMC
                            dt = new BL.GMMS.CostUploadBL().RemoveAmountColumnForRMC(dt, R.RMCID);

                            if (dt != null && dt.Rows.Count > 0)
                            {
                                

                                conn.AddCommand("[RMC].[AddEditRMCCost]", QueryType.Procedure);
                                conn.AddParameter(ParameterOF.PROCEDURE, "@SP_EditorAddNew", SqlDbType.VarChar, 1, ParameterDirection.Input, CSubs.PSafeValue("A"));
                                conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ComponentID", SqlDbType.Int, 0, ParameterDirection.Input, R.RateComponentID);
                                conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CompanyID", SqlDbType.Int, 0, ParameterDirection.Input, UserSession.GetUserSession().CompanyID);
                                conn.AddParameter(ParameterOF.PROCEDURE, "@SP_AgentID", SqlDbType.Int, 0, ParameterDirection.Input, R.AgentID);
                                conn.AddParameter(ParameterOF.PROCEDURE, "@SP_GoodsDescID", SqlDbType.Int, 0, ParameterDirection.Input, R.GoodsDescriptionID);
                                conn.AddParameter(ParameterOF.PROCEDURE, "@SP_RMCID", SqlDbType.Int, 0, ParameterDirection.Input, R.RMCID);
                                if ((R.RateComponentID == 1 || R.RateComponentID == 3) && dt.Columns.Count>4 && !dt.Columns[4].ColumnName.Trim().ToUpper().EndsWith("THC"))
                                {
                                    conn.AddParameter(ParameterOF.PROCEDURE, "@SP_TableColType", SqlDbType.VarChar, 10, ParameterDirection.Input, CSubs.PSafeValue("BTR"));
                                }
                                else
                                {
                                    conn.AddParameter(ParameterOF.PROCEDURE, "@SP_TableColType", SqlDbType.VarChar, 10, ParameterDirection.Input, CSubs.PSafeValue("WRITER"));
                                }

                                conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LOGINID", SqlDbType.Int, 0, ParameterDirection.Input, LoginID);
                                conn.AddParameter(ParameterOF.PROCEDURE, "@RETURNSTATUS", SqlDbType.SmallInt, 0, ParameterDirection.ReturnValue);
                                conn.AddParameter(ParameterOF.PROCEDURE, "@Out_Message", SqlDbType.NVarChar, 500, ParameterDirection.Output);

                                conn.AddParameter(ParameterOF.PROCEDURE, "@SP_StartPointID", SqlDbType.Int, 0, ParameterDirection.Input);
                                conn.AddParameter(ParameterOF.PROCEDURE, "@SP_EndPointID", SqlDbType.Int, 0, ParameterDirection.Input);
                                conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ModeID", SqlDbType.Int, 0, ParameterDirection.Input);
                                conn.AddParameter(ParameterOF.PROCEDURE, "@SP_TransitTimeFrom", SqlDbType.Int, 0, ParameterDirection.Input);
                                conn.AddParameter(ParameterOF.PROCEDURE, "@SP_TransitTimeTo", SqlDbType.Int, 0, ParameterDirection.Input);
                                conn.AddParameter(ParameterOF.PROCEDURE, "@Sp_RMCCostDataTableType", SqlDbType.Structured, 0, ParameterDirection.Input);
                                conn.AddParameter(ParameterOF.PROCEDURE, "@SP_THCValues", SqlDbType.Xml, 0, ParameterDirection.Input,new BL.GMMS.CostUploadBL().GetTHCSlabXml(R.SpecialTHCList));

                                DataTable Import = new DataTable("RMCCostDataTableType");
                                Import.Columns.Add("WeightFrom", typeof(int));
                                Import.Columns.Add("WeightTo", typeof(int));
                                Import.Columns.Add("RateItemVal1", typeof(float));
                                Import.Columns.Add("RateItemVal2", typeof(float));
                                Import.Columns.Add("RateItemVal3", typeof(float));
                                Import.Columns.Add("RateItemVal4", typeof(float));
                                Import.Columns.Add("RateItemVal5", typeof(float));
                                Import.Columns.Add("RateItemVal6", typeof(float));
                                Import.Columns.Add("RateItemVal7", typeof(float));
                                Import.Columns.Add("RateItemVal8", typeof(float));



                                foreach (TransportModeList item in R.ServiceModeList)
                                {
                                    int StartPoint = 0, Endpoint = 0;
                                    Import.Rows.Clear();
                                    DataRow[] rows = dt.Select(string.Format("[ Mode ]={0}", CSubs.QSafeValue(item.ModeName.ToLower())));
                                    //Import.Rows.Add(rows);

                                    foreach (var rowObj in rows)
                                    {
                                        var row = Import.NewRow();
                                        switch (rowObj.Table.Columns.Count)
                                        {
                                            case 4:
                                                Import.Rows.Add(rowObj[0], rowObj[1], rowObj[3], 0, 0, 0, 0, 0, 0, 0);
                                                break;
                                            case 5:
                                                Import.Rows.Add(rowObj[0], rowObj[1], rowObj[3], rowObj[4], 0, 0, 0, 0, 0, 0);
                                                break;
                                            case 6:
                                                Import.Rows.Add(rowObj[0], rowObj[1], rowObj[3], rowObj[4], rowObj[5], 0, 0, 0, 0, 0);
                                                break;
                                            case 7:
                                                Import.Rows.Add(rowObj[0], rowObj[1], rowObj[3], rowObj[4], rowObj[5], rowObj[6], 0, 0, 0, 0);
                                                break;
                                            case 8:
                                                Import.Rows.Add(rowObj[0], rowObj[1], rowObj[3], rowObj[4], rowObj[5], rowObj[6], rowObj[7], 0, 0, 0);
                                                break;
                                            case 9:
                                                Import.Rows.Add(rowObj[0], rowObj[1], rowObj[3], rowObj[4], rowObj[5], rowObj[6], rowObj[7], rowObj[8], 0, 0);
                                                break;
                                            case 10:
                                                Import.Rows.Add(rowObj[0], rowObj[1], rowObj[3], rowObj[4], rowObj[5], rowObj[6], rowObj[7], rowObj[8], rowObj[9], 0);
                                                break;
                                            case 11:
                                                Import.Rows.Add(rowObj[0], rowObj[1], rowObj[3], rowObj[4], rowObj[5], rowObj[6], rowObj[7], rowObj[8], rowObj[9], rowObj[10]);
                                                break;
                                            default:
                                                break;
                                        }

                                    }

                                    if (R.RateComponentID == 1)
                                    {

                                        if (item.FromCityID != null && item.FromCityID > 0 && item.ExitPortID != null && item.ExitPortID > 0)
                                        {
                                            StartPoint = (int)item.FromCityID; Endpoint = (int)item.ExitPortID;
                                        }
                                    }
                                    else if (R.RateComponentID == 2)
                                    {
                                        if (item.ExitPortID != null && item.ExitPortID > 0 && item.EntryPortID != null && item.EntryPortID > 0)
                                        {
                                            StartPoint = (int)item.ExitPortID; Endpoint = (int)item.EntryPortID;
                                        }

                                    }
                                    else if (R.RateComponentID == 3)
                                    {
                                        if (item.EntryPortID != null && item.EntryPortID > 0 && item.ToCityID != null && item.ToCityID > 0)
                                        {
                                            StartPoint = (int)item.EntryPortID;
                                            Endpoint = (int)item.ToCityID;
                                        }
                                    }
                                    else if (R.RateComponentID == 4)
                                    {
                                        if (item.FromCityID != null && item.FromCityID > 0 && item.ToCityID != null && item.ToCityID > 0)
                                        {
                                            StartPoint = (int)item.FromCityID; Endpoint = (int)item.ToCityID;
                                        }
                                    }

                                    ////if start point, end point and import datable having the data then only insert record
                                    if (StartPoint > 0 && Endpoint > 0 && Import != null && Import.Rows.Count > 0)
                                    {
                                        conn.SetParameterValue(ParameterOF.PROCEDURE, "@SP_StartPointID", StartPoint);
                                        conn.SetParameterValue(ParameterOF.PROCEDURE, "@SP_EndPointID", Endpoint);
                                        conn.SetParameterValue(ParameterOF.PROCEDURE, "@SP_ModeID", item.ModeID);
                                        conn.SetParameterValue(ParameterOF.PROCEDURE, "@SP_TransitTimeFrom", item.TransitTimeFrom);
                                        conn.SetParameterValue(ParameterOF.PROCEDURE, "@SP_TransitTimeTo", item.TransitTimeTo);
                                        conn.SetParameterValue(ParameterOF.PROCEDURE, "@Sp_RMCCostDataTableType", Import);
                                        conn.ExecuteProcedure(ProcedureReturnType.SingleValue);
                                        if (!conn.IsError)
                                        {
                                            int ReturnStatus = Convert.ToInt32(conn.GetParameterValue(ParameterOF.PROCEDURE, "@RETURNSTATUS"));
                                            result = Convert.ToString(conn.GetParameterValue(ParameterOF.PROCEDURE, "@Out_Message"));

                                            if (ReturnStatus == 0)
                                            {
                                                ResultCount = ResultCount + 1;
                                                //return true;
                                            }
                                            else
                                            {
                                                conn.RollbackTransaction();
                                                return false;
                                            }
                                        }
                                        else
                                        {
                                            conn.RollbackTransaction();
                                            result = conn.ErrorMessage;
                                            throw new Exception(conn.ErrorMessage);
                                        }
                                    }
                                    else if (StartPoint > 0 && Endpoint > 0 && (Import == null || Import.Rows.Count <= 0))
                                    {
                                        conn.RollbackTransaction();
                                        result = "No records to upload";
                                        return false;
                                        //throw new Exception(result);
                                    }

                                }

                                if (ResultCount > 0)
                                {
                                    conn.CommitTransaction();
                                    result = "Cost uploaded successfully";
                                }
                                else
                                {
                                    conn.RollbackTransaction();
                                    result = "unable to upload cost";
                                    return false;
                                }
                                
                            }
                            else
                                throw new Exception("No records to Upload");



                        }
                        catch (Exception)
                        {
                            conn.RollbackTransaction();
                            throw;
                        }
                        /////catchblock

                    }
                    else
                        throw new Exception(conn.ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoginID), "CostUploadDAL", "Insert", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

            return true;
        }

        public bool UploadPricing(GMMSRateUpload R, int LoginID, out string result)
        {
            result = String.Empty;

            try
            {

                int ResultCount = 0;
                using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString(), true, System.Data.IsolationLevel.ReadCommitted))
                {
                    if (conn.Connect())
                    {
                        try
                        {

                            DataTable dt = (DataTable)JsonConvert.DeserializeObject(R.CostRateList, (typeof(DataTable)));
                            conn.AddCommand("[RMC_BGR].[AddEditRMCPrice]", QueryType.Procedure);
                            conn.AddParameter(ParameterOF.PROCEDURE, "@SP_EditorAddNew", SqlDbType.VarChar, 1, ParameterDirection.Input, CSubs.PSafeValue("A"));
                            conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ComponentID", SqlDbType.Int, 0, ParameterDirection.Input, R.RateComponentID);
                            conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CompanyID", SqlDbType.Int, 0, ParameterDirection.Input, UserSession.GetUserSession().CompanyID);
							//conn.AddParameter(ParameterOF.PROCEDURE, "@SP_AgentID", SqlDbType.Int, 0, ParameterDirection.Input, R.AgentID);
							conn.AddParameter(ParameterOF.PROCEDURE, "@SP_AgentID", SqlDbType.Int, 0, ParameterDirection.Input, 0);
							conn.AddParameter(ParameterOF.PROCEDURE, "@SP_GoodsDescID", SqlDbType.Int, 0, ParameterDirection.Input, R.GoodsDescriptionID);
                            conn.AddParameter(ParameterOF.PROCEDURE, "@SP_RMCID", SqlDbType.Int, 0, ParameterDirection.Input, R.RMCID);
                            conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LOGINID", SqlDbType.Int, 0, ParameterDirection.Input, LoginID);
                            conn.AddParameter(ParameterOF.PROCEDURE, "@RETURNSTATUS", SqlDbType.SmallInt, 0, ParameterDirection.ReturnValue);
                            conn.AddParameter(ParameterOF.PROCEDURE, "@Out_Message", SqlDbType.NVarChar, 500, ParameterDirection.Output);

                            conn.AddParameter(ParameterOF.PROCEDURE, "@SP_StartPointID", SqlDbType.Int, 0, ParameterDirection.Input);
                            conn.AddParameter(ParameterOF.PROCEDURE, "@SP_EndPointID", SqlDbType.Int, 0, ParameterDirection.Input);
                            conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ModeID", SqlDbType.Int, 0, ParameterDirection.Input);
                            conn.AddParameter(ParameterOF.PROCEDURE, "@SP_TransitTimeFrom", SqlDbType.Int, 0, ParameterDirection.Input);
                            conn.AddParameter(ParameterOF.PROCEDURE, "@SP_TransitTimeTo", SqlDbType.Int, 0, ParameterDirection.Input);
                            conn.AddParameter(ParameterOF.PROCEDURE, "@Sp_RMCCostDataTableType", SqlDbType.Structured, 0, ParameterDirection.Input);

                            DataTable Import = new DataTable("RMCCostDataTableType");
                            Import.Columns.Add("WeightFrom", typeof(int));
                            Import.Columns.Add("WeightTo", typeof(int));
                            Import.Columns.Add("RateItemVal1", typeof(float));
                            Import.Columns.Add("RateItemVal2", typeof(float));
                            Import.Columns.Add("RateItemVal3", typeof(float));
                            Import.Columns.Add("RateItemVal4", typeof(float));
                            Import.Columns.Add("RateItemVal5", typeof(float));
                            Import.Columns.Add("RateItemVal6", typeof(float));
                            Import.Columns.Add("RateItemVal7", typeof(float));
                            Import.Columns.Add("RateItemVal8", typeof(float));

                            foreach (TransportModeList item in R.ServiceModeList)
                            {
                                int StartPoint = 0, Endpoint = 0;
                                Import.Rows.Clear();
                                DataRow[] rows = dt.Select(string.Format("[ Mode ]={0}", CSubs.QSafeValue(item.ModeName.ToLower())));
                                //Import.Rows.Add(rows);

                                foreach (var rowObj in rows)
                                {
                                    var row = Import.NewRow();
                                    switch (rowObj.Table.Columns.Count)
                                    {
                                        case 4:
                                            Import.Rows.Add(rowObj[0], rowObj[1], rowObj[3], 0, 0, 0, 0, 0, 0, 0);
                                            break;
                                        case 5:
                                            Import.Rows.Add(rowObj[0], rowObj[1], rowObj[3], rowObj[4], 0, 0, 0, 0, 0, 0);
                                            break;
                                        case 6:
                                            Import.Rows.Add(rowObj[0], rowObj[1], rowObj[3], rowObj[4], rowObj[5], 0, 0, 0, 0, 0);
                                            break;
                                        case 7:
                                            Import.Rows.Add(rowObj[0], rowObj[1], rowObj[3], rowObj[4], rowObj[5], rowObj[6], 0, 0, 0, 0);
                                            break;
                                        case 8:
                                            Import.Rows.Add(rowObj[0], rowObj[1], rowObj[3], rowObj[4], rowObj[5], rowObj[6], rowObj[7], 0, 0, 0);
                                            break;
                                        case 9:
                                            Import.Rows.Add(rowObj[0], rowObj[1], rowObj[3], rowObj[4], rowObj[5], rowObj[6], rowObj[7], rowObj[8], 0, 0);
                                            break;
                                        case 10:
                                            Import.Rows.Add(rowObj[0], rowObj[1], rowObj[3], rowObj[4], rowObj[5], rowObj[6], rowObj[7], rowObj[8], rowObj[9], 0);
                                            break;
                                        case 11:
                                            Import.Rows.Add(rowObj[0], rowObj[1], rowObj[3], rowObj[4], rowObj[5], rowObj[6], rowObj[7], rowObj[8], rowObj[9], rowObj[10]);
                                            break;
                                        default:
                                            break;
                                    }

                                }

                                if (R.RateComponentID == 1)
                                {

                                    if (item.FromCityID != null && item.FromCityID > 0 && item.ExitPortID != null && item.ExitPortID > 0)
                                    {
                                        StartPoint = (int)item.FromCityID; Endpoint = (int)item.ExitPortID;
                                    }
                                }
                                else if (R.RateComponentID == 2)
                                {
                                    if (item.ExitPortID != null && item.ExitPortID > 0 && item.EntryPortID != null && item.EntryPortID > 0)
                                    {
                                        StartPoint = (int)item.ExitPortID; Endpoint = (int)item.EntryPortID;
                                    }

                                }
                                else if (R.RateComponentID == 3)
                                {
                                    if (item.EntryPortID != null && item.EntryPortID > 0 && item.ToCityID != null && item.ToCityID > 0)
                                    {
                                        StartPoint = (int)item.EntryPortID;
                                        Endpoint = (int)item.ToCityID;
                                    }
                                }
                                else if (R.RateComponentID == 4)
                                {
                                    if (item.FromCityID != null && item.FromCityID > 0 && item.ToCityID != null && item.ToCityID > 0)
                                    {
                                        StartPoint = (int)item.FromCityID; Endpoint = (int)item.ToCityID;
                                    }
                                }

                                ////if start point, end point and import datable having the data then only insert record
                                if (StartPoint > 0 && Endpoint > 0 && Import != null && Import.Rows.Count > 0)
                                {
                                    conn.SetParameterValue(ParameterOF.PROCEDURE, "@SP_StartPointID", StartPoint);
                                    conn.SetParameterValue(ParameterOF.PROCEDURE, "@SP_EndPointID", Endpoint);
                                    conn.SetParameterValue(ParameterOF.PROCEDURE, "@SP_ModeID", item.ModeID);
                                    conn.SetParameterValue(ParameterOF.PROCEDURE, "@SP_TransitTimeFrom", item.TransitTimeFrom);
                                    conn.SetParameterValue(ParameterOF.PROCEDURE, "@SP_TransitTimeTo", item.TransitTimeTo);
                                    conn.SetParameterValue(ParameterOF.PROCEDURE, "@Sp_RMCCostDataTableType", Import);
                                    conn.ExecuteProcedure(ProcedureReturnType.SingleValue);
                                    if (!conn.IsError)
                                    {
                                        int ReturnStatus = Convert.ToInt32(conn.GetParameterValue(ParameterOF.PROCEDURE, "@RETURNSTATUS"));
                                        result = Convert.ToString(conn.GetParameterValue(ParameterOF.PROCEDURE, "@Out_Message"));

                                        if (ReturnStatus == 0)
                                        {
                                            ResultCount = ResultCount + 1;
                                            //return true;
                                        }
                                        else
                                        {
                                            conn.RollbackTransaction();
                                            return false;
                                        }
                                    }
                                    else
                                    {
                                        conn.RollbackTransaction();
                                        result = conn.ErrorMessage;
                                        throw new Exception(conn.ErrorMessage);
                                    }
                                }
                                else if (StartPoint > 0 && Endpoint > 0 && (Import == null || Import.Rows.Count <= 0))
                                {
                                    conn.RollbackTransaction();
                                    result = "No records to upload";
                                    return false;
                                    //throw new Exception(result);
                                }

                            }

                            if (ResultCount > 0)
                            {
                                conn.CommitTransaction();
                                result = "Cost uploaded successfully";
                            }
                            else
                            {
                                conn.RollbackTransaction();
                                result = "unable to upload cost";
                                return false;
                            }



                        }
                        catch (Exception)
                        {
                            conn.RollbackTransaction();
                            throw;
                        }
                        /////catchblock

                    }
                    else
                        throw new Exception(conn.ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoginID), "CostUploadDAL", "Insert", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

            return true;
        }

        
        public bool CityRateAdd(AccessorialServicesViewModel data,int LoginID, out string result)
        {
            result = String.Empty;
            try
            {
                using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {
                        conn.AddCommand("[RMC].[AddEditCityCost]", QueryType.Procedure);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_EditorAddNew", SqlDbType.VarChar, 1, ParameterDirection.Input, CSubs.PSafeValue("A"));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_RMCID", SqlDbType.Int,0, ParameterDirection.Input, data.RMCID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CompanyID", SqlDbType.Int, 0, ParameterDirection.Input, UserSession.GetUserSession().CompanyID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_GoodsDescID", SqlDbType.Int, 0, ParameterDirection.Input, data.GoodsDescriptionID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CityID", SqlDbType.Int, 0, ParameterDirection.Input, data.CityID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_AgentID", SqlDbType.Int, 0, ParameterDirection.Input, data.AgentID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CostHeadID", SqlDbType.Int, 0, ParameterDirection.Input, data.CostHeadID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CurrID", SqlDbType.Int, 0, ParameterDirection.Input, data.BaseCurrencyRateID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CostVal", SqlDbType.Float, 0, ParameterDirection.Input, data.Amount);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_EffectiveFrom", SqlDbType.Date, 0, ParameterDirection.Input, data.EffectiveFromDate);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LoginID", SqlDbType.Int, 0, ParameterDirection.Input, LoginID);
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
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "CostUploadBL", "UploadRate", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

            return true;

        }

        public bool CityRevenueAdd(AccessServiceAgentList data,int LoginID,out string result)
        {
            result = string.Empty;

            try
            {

                using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {
                        
                        var xEle = new XElement("RevDetails",
                        from emp in data.CitywiseAdditionalServiceRates
                        select new XElement("RevDetail",
                                     new XElement("CostHeadID", emp.AdditionalServiceId),
                                       new XElement("Reven", emp.Rate)
                                       
                                   ));

                        conn.AddCommand("[RMC].[AddEditCityRevenue]", QueryType.Procedure);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_EditorAddNew", SqlDbType.VarChar, 1, ParameterDirection.Input, CSubs.PSafeValue("A"));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_RMCID", SqlDbType.Int, 0, ParameterDirection.Input, data.ASARMCID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CompanyID", SqlDbType.Int, 0, ParameterDirection.Input, UserSession.GetUserSession().CompanyID);
                        //conn.AddParameter(ParameterOF.PROCEDURE, "@SP_GoodsDescID", SqlDbType.Int, 0, ParameterDirection.Input, data.ASAGoodDescriptionID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CityID", SqlDbType.Int, 0, ParameterDirection.Input, data.ASACityID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CostheadRevenDet", SqlDbType.Xml, -1, ParameterDirection.Input, xEle.ToString());
                        //conn.AddParameter(ParameterOF.PROCEDURE, "@SP_currID", SqlDbType.Int, 0, ParameterDirection.Input, data.currID);
                        //conn.AddParameter(ParameterOF.PROCEDURE, "@SP_EffectiveFrom", SqlDbType.Date, 0, ParameterDirection.Input, data.EffectiveFromDate);

                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LoginID", SqlDbType.Int, 0, ParameterDirection.Input, LoginID);
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
                throw new DataAccessException(Convert.ToString(LoginID), "CostUploadDAL", "CityRevenueAdd", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

            return true;
            
        }

        public bool CityCostUpload(CityCostUpload data, int LoginID, out string result)
        {
            result = string.Empty;

            try
            {

                int ResultCount = 0;
                using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString(), true, System.Data.IsolationLevel.ReadCommitted))
                {
                    if (conn.Connect())
                    {
                        try
                        {
                            
                            conn.AddCommand("[RMC].[AddEditCityCost]", QueryType.Procedure);
                            conn.AddParameter(ParameterOF.PROCEDURE, "@SP_EditorAddNew", SqlDbType.VarChar, 1, ParameterDirection.Input, CSubs.QSafeValue("A"));
                            conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CompanyID", SqlDbType.Int, 0, ParameterDirection.Input, UserSession.GetUserSession().CompanyID);
                            conn.AddParameter(ParameterOF.PROCEDURE, "@SP_AgentID", SqlDbType.Int, 0, ParameterDirection.Input, data.AgentID);
                            conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CityID", SqlDbType.Int, 0, ParameterDirection.Input, data.CityID);
                            conn.AddParameter(ParameterOF.PROCEDURE, "@SP_GoodsDescID", SqlDbType.Int, 0, ParameterDirection.Input, data.GoodsDescriptionID);
                            conn.AddParameter(ParameterOF.PROCEDURE, "@SP_RMCID", SqlDbType.Int, 0, ParameterDirection.Input, data.RMCID);
                            conn.AddParameter(ParameterOF.PROCEDURE, "@SP_EffectiveFrom", SqlDbType.Date, 0, ParameterDirection.Input, data.EffectiveFromDate);
                            conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CurrID", SqlDbType.Int, 0, ParameterDirection.Input, data.BaseCurrencyRateID);
                            conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LOGINID", SqlDbType.Int, 0, ParameterDirection.Input, LoginID);
                            conn.AddParameter(ParameterOF.PROCEDURE, "@RETURNSTATUS", SqlDbType.SmallInt, 0, ParameterDirection.ReturnValue);
                            conn.AddParameter(ParameterOF.PROCEDURE, "@Out_Message", SqlDbType.NVarChar, 500, ParameterDirection.Output);
                            
                            conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CostHeadID", SqlDbType.Int, 0, ParameterDirection.Input);
                            conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CostVal", SqlDbType.Float, 0, ParameterDirection.Input);
                            //conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ServiceInc", SqlDbType.Bit, 0, ParameterDirection.Input);

                            foreach (CityCostHead item in data.CityCostHeadList)
                            {
                                ////if start point, end point and import datable having the data then only insert record
                                if (item.CostHeadID > 0 && item.Amount > 0)
                                {
                                    conn.SetParameterValue(ParameterOF.PROCEDURE, "@SP_CostHeadID", item.CostHeadID);
                                    conn.SetParameterValue(ParameterOF.PROCEDURE, "@SP_CostVal", item.Amount);
                                    //conn.SetParameterValue(ParameterOF.PROCEDURE, "@SP_ServiceInc", item.ServiceIncluded);

                                    conn.ExecuteProcedure(ProcedureReturnType.SingleValue);
                                    if (!conn.IsError)
                                    {
                                        int ReturnStatus = Convert.ToInt32(conn.GetParameterValue(ParameterOF.PROCEDURE, "@RETURNSTATUS"));
                                        result = Convert.ToString(conn.GetParameterValue(ParameterOF.PROCEDURE, "@Out_Message"));

                                        if (ReturnStatus == 0)
                                        {
                                            ResultCount = ResultCount + 1;
                                            //return true;
                                        }
                                        else
                                        {
                                            conn.RollbackTransaction();
                                            return false;
                                        }
                                    }
                                    else
                                    {
                                        conn.RollbackTransaction();
                                        result = conn.ErrorMessage;
                                        throw new Exception(conn.ErrorMessage);
                                    }
                                }
                                else 
                                {
                                    conn.RollbackTransaction();
                                    result = "No records to upload";
                                    return false;
                                    //throw new Exception(result);
                                }

                            }

                            if (ResultCount > 0)
                            {
                                conn.CommitTransaction();
                                result = "City Rate uploaded successfully";
                            }
                            else
                            {
                                conn.RollbackTransaction();
                                result = "unable to upload City Rate";
                                return false;
                            }



                        }
                        catch (Exception)
                        {
                            conn.RollbackTransaction();
                            throw;
                        }
                        /////catchblock

                    }
                    else
                        throw new Exception(conn.ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoginID), "CostUploadDAL", "Insert", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

            return true;

        }

        public DataTable GetDataForUpload(int LoginID)
        {
            DataTable dataTble = new DataTable();
            try
            {
                string query = string.Format("EXEC [Comm].[ForCombo_TransportMode] @SP_Type='COSTFORMAT',@SP_LoginID={0}", Convert.ToString(LoginID));
                dataTble = CSubs.GetDataTable(query);
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoginID), "CostUploadDAL", "GetDataForUpload", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

            return dataTble;

        }

        public bool GetRMCForRateAmountCalculation(int RMCID, int LoginID)
        {

            bool result = false;
            try
            {
                string query = string.Format("EXEC [Rate].[GetRMCForRateAmountCalculation] @SP_RMCID={0},@SP_LoginID={1}"
                    ,CSubs.QSafeValue(Convert.ToString(RMCID)), CSubs.QSafeValue(Convert.ToString(LoginID)));
                DataTable dt = CSubs.GetDataTable(query);
                if (dt!=null && dt.Rows.Count > 0) {
                    result = true;
                }
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoginID), "CostUploadDAL", "GetDataForUpload", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

            return result;

        }
        
        public DataTable GetTHCWeightSlab(int LoginID)
        {
            DataTable dataTble = new DataTable();
            try
            {
                string query = string.Format("EXEC RMC.BTR_GetTHCSlabsFromMaster");
                dataTble = CSubs.GetDataTable(query);
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoginID), "CostUploadDAL", "GetTHCWeightSlab", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

            return dataTble;
        }
    }

}