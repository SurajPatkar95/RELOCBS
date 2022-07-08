using RELOCBS.App_Code;
using RELOCBS.Common.ExceptionHandling;
using RELOCBS.Entities;
using RELOCBS.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml.Linq;

namespace RELOCBS.DAL.Storage
{
    public class StorageDAL
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

        public IQueryable<Entities.JobStorageGrid> GetStorageGrid(DateTime? FromDate, DateTime? Todate, bool IsJobDate, bool IsStorageDate, Int64 MoveId, string Shipper, bool RMCBuss, int CompanyID)
        {
            int LoggedinUserID = UserSession.GetUserSession().LoginID;
            IQueryable<Entities.JobStorageGrid> List = new List<Entities.JobStorageGrid>().AsQueryable();

            try
            {

                using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {
                        conn.AddCommand("[Strg].[GetStrgJobDetailGrid]", QueryType.Procedure);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IsJob", SqlDbType.Bit, 1, ParameterDirection.Input, IsJobDate);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IsStrg", SqlDbType.Bit, 1, ParameterDirection.Input, IsStorageDate);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_FromDate", SqlDbType.DateTime, 0, ParameterDirection.Input, FromDate);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ToDate", SqlDbType.DateTime, 0, ParameterDirection.Input, Todate);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LoginID", SqlDbType.Int, 0, ParameterDirection.Input, UserSession.GetUserSession().LoginID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_MoveID", SqlDbType.Int, 0, ParameterDirection.Input, MoveId);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Shipper", SqlDbType.VarChar, 100, ParameterDirection.Input, CSubs.PSafeValue(Shipper));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CompID", SqlDbType.Int, 0, ParameterDirection.Input, CompanyID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IsRMCBuss", SqlDbType.Bit, 1, ParameterDirection.Input, RMCBuss);
                        DataTable dt = (DataTable)conn.ExecuteProcedure(ProcedureReturnType.DataTable);

                        if (dt != null)
                        {
                            var result = (from item in dt.AsEnumerable()
                                          select new Entities.JobStorageGrid()
                                          {
                                              MoveID = Convert.ToInt64(item["MoveID"]),
                                              StorageID = !string.IsNullOrWhiteSpace(Convert.ToString(item["StrgJobMasterID"])) ? Convert.ToInt64(item["StrgJobMasterID"]) : -1,
                                              JobNo = Convert.ToString(item["JobID"]),
                                              //InsuranceNo = Convert.ToString(item["InsNumber"]),
                                              JobDate = Convert.ToDateTime(item["JobOpenedDate"]),
                                              StorageEntryDate = !string.IsNullOrWhiteSpace(Convert.ToString(item["StrgEntryDate"])) ? Convert.ToDateTime(item["StrgEntryDate"]) :(DateTime?)null,
                                              StorageExitDate = !string.IsNullOrWhiteSpace(Convert.ToString(item["StrgExitDate"])) ? Convert.ToDateTime(item["StrgExitDate"]) : (DateTime?)null,
                                              ServiceLine = Convert.ToString(item["ServiceLine"]),
                                              //OrgCity = Convert.ToString(item["FromCity"]),
                                              //DestCity = Convert.ToString(item["ToCity"]),
                                              Controller = Convert.ToString(item["Controller"]),
                                              JobCommodity = Convert.ToString(item["Commodity"]),
                                              QuotationID = Convert.ToString(item["QuotationID"]),
                                              ShipperName = Convert.ToString(item["ShipperName"]),
                                              Client = Convert.ToString(item["ClientName"]),
                                              Corporate = Convert.ToString(item["Corporate"]),
                                              InsuredBy = Convert.ToString(item["InsuredBy"]),
                                              Warehouse = Convert.ToString(item["Warehoue_Name"]),
                                              BillStartDate = !string.IsNullOrWhiteSpace(Convert.ToString(item["BillStartDate"])) ? Convert.ToDateTime(item["BillStartDate"]) : (DateTime?)null,
                                          }).AsQueryable();
                            List = result;
                        }
                    }
                    else
                        throw new Exception(conn.ErrorMessage);
                }


                return List;
            }
            catch (Exception ex)
            {

                throw new DataAccessException(Convert.ToString(LoggedinUserID), "StorageDAL", "GetStorageGrid", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }


        }

        public DataSet GetStorageDetails(int LoginID, Int64 MoveID, Int64? Storage_ID)
        {
            DataSet Ds = new DataSet();

            try
            {
                string query = string.Format("[Strg].[GetStrgJobDetailMaster] @SP_MoveID={0},@SP_StrgJobMasterID={1} ,@SP_LoginID={2}",
                CSubs.QSafeValue(Convert.ToString(MoveID)),
                CSubs.QSafeValue(Convert.ToString(Storage_ID))
                ,CSubs.QSafeValue(Convert.ToString(LoginID))
                );

                Ds = CSubs.GetDataSet(query);

                return Ds;
            }
            catch (Exception ex)
            {

                throw new DataAccessException(Convert.ToString(LoginID), "StorageDAL", "GetStorageDetails", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

        }

        public bool Insert(JobStorage model, int LoginID, out string result)
        {
            try
            {
                using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {

                        conn.AddCommand("[Strg].[AddEditStrgJobDetailMaster]", QueryType.Procedure);
                        //conn.AddParameter(ParameterOF.PROCEDURE, "@SP_StrgJobMasterID", SqlDbType.BigInt, 0, ParameterDirection.InputOutput, model.StorageID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_StrgJobMasterID", SqlDbType.BigInt, 0, ParameterDirection.InputOutput, model.StorageID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_MoveID", SqlDbType.BigInt, 0, ParameterDirection.Input, model.jobDetail.MoveID);
                        //conn.AddParameter(ParameterOF.PROCEDURE, "@SP_InsCompID", SqlDbType.BigInt, 0, ParameterDirection.InputOutput, model.StorageDetailID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CommodityID", SqlDbType.Int, 0, ParameterDirection.Input, model.StorageCommodityID);

                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_PackDate", SqlDbType.DateTime, 0, ParameterDirection.Input, model.PackDate);
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Loaddate", SqlDbType.DateTime, 0, ParameterDirection.Input, model.LoadDate);
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_PackStateID", SqlDbType.Int, 0, ParameterDirection.Input, model.StrgStateID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_StorageDetails", SqlDbType.VarChar, 500, ParameterDirection.Input, CSubs.PSafeValue(model.StorageDetails));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_WarehouseID", SqlDbType.Int, 50, ParameterDirection.Input, model.WarehouseID );
                        //conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CurrID", SqlDbType.Int, 0, ParameterDirection.Input, model.CurrID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_SDFromBr", SqlDbType.Int, 0, ParameterDirection.Input, model.SD_BranchID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_SDFromHO", SqlDbType.Int, 0, ParameterDirection.Input, model.SD_HOID);

                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_StrgEntryDate", SqlDbType.DateTime, 0, ParameterDirection.Input, model.StorageEntryDate);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_StrgExitDate", SqlDbType.DateTime, 0, ParameterDirection.Input, model.StorageExitDate);
                        

                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_BillStartDate", SqlDbType.DateTime, 0, ParameterDirection.Input, model.BillStartDate);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_FileCloseDate", SqlDbType.DateTime, 0, ParameterDirection.Input, model.FileCloseDate);

                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IsInsured", SqlDbType.Bit, 0, ParameterDirection.Input, model.IsInsured);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_InsuredBy", SqlDbType.Int, 0, ParameterDirection.Input, model.InsuredByID);

                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_StrgVolDetailID", SqlDbType.BigInt, 0, ParameterDirection.Input, model.StorageDetailID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_WtUnitID", SqlDbType.Int, 0, ParameterDirection.Input, model.VolumeUnitID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_VolAsOnTheDate", SqlDbType.Float, 0, ParameterDirection.Input, model.VolumeCFT);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_DateOfVolEntry", SqlDbType.DateTime, 0, ParameterDirection.Input, model.VolumeDate);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_VolRemarks", SqlDbType.VarChar, 500, ParameterDirection.Input, CSubs.PSafeValue(model.VolumeRemark));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CurrID", SqlDbType.Int, 0, ParameterDirection.Input, model.CurrID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LoginID", SqlDbType.Int, 0, ParameterDirection.Input, UserSession.GetUserSession().LoginID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_PackCityID", SqlDbType.Int, 0, ParameterDirection.Input, model.StrgCityID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@RETURNSTATUS", SqlDbType.SmallInt, 0, ParameterDirection.ReturnValue);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@Out_Message", SqlDbType.NVarChar, -1, ParameterDirection.Output);
                        conn.ExecuteProcedure(ProcedureReturnType.SingleValue);

                        if (!conn.IsError)
                        {
                            int ReturnStatus = Convert.ToInt32(conn.GetParameterValue(ParameterOF.PROCEDURE, "@RETURNSTATUS"));
                            result = Convert.ToString(conn.GetParameterValue(ParameterOF.PROCEDURE, "@Out_Message"));

                            if (ReturnStatus == 0)
                            {
                                model.StorageID = Convert.ToInt64(conn.GetParameterValue(ParameterOF.PROCEDURE, "@SP_StrgJobMasterID"));
                                //model.StorageDetailID = Convert.ToInt64(conn.GetParameterValue(ParameterOF.PROCEDURE, "@SP_InsPremiunForJobID"));

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
                throw new DataAccessException(Convert.ToString(LoginID), "StorageDAL", "Insert", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

        }

        public bool InsertRate(JobStorage model, int LoginID, out string result)
        {
            try
            {
                using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {

                        String RateDetailXml=String.Empty;
                        if (model.ratesList != null && model.ratesList.Count > 0)
                        {
                            RateDetailXml = new XElement("CostHeadwiseDetails", from emp in model.ratesList
                                                                     select new XElement("CostHeadwiseDetail",
                                                         new XElement("CostHeadID", emp.CostHeadid),
                                                         new XElement("Vol", emp.VolumeCFT),
                                                         new XElement("RatePerCFT", String.Format("{0:0.####}", emp.RatePerUnit)),
                                                         new XElement("TotAmtQuoted", String.Format("{0:0.####}", emp.Rate)),
                                                         new XElement("PeriodFreqID", emp.RatePeriodID)

                                                     )).ToString();

                            //if (!string.IsNullOrWhiteSpace(RateDetailXml))
                            //{
                            //    RateDetailXml = Regex.Replace(RateDetailXml, @"\t|\n|\r", ""); //InstIDxml.Replace("\r\n", "");
                            //}
                        }

                        conn.AddCommand("[Strg].[AddEditStrgRates]", QueryType.Procedure);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_StrgJobMasterID", SqlDbType.BigInt, 0, ParameterDirection.InputOutput, model.StorageID);
                        //conn.AddParameter(ParameterOF.PROCEDURE, "@SP_MoveID", SqlDbType.BigInt, 0, ParameterDirection.Input, model.jobDetail.MoveID);
                        //conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CommodityID", SqlDbType.Int, 0, ParameterDirection.Input, model.StorageCommodityID);

                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_DateFrom", SqlDbType.DateTime, 0, ParameterDirection.Input, model.AsOnDate);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_PercentInc", SqlDbType.Float, 0, ParameterDirection.Input, model.Strg_Inc_percent);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_BillFrqID", SqlDbType.Int, 50, ParameterDirection.Input, model.Months);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_StrgVolDetailID", SqlDbType.BigInt, 0, ParameterDirection.Input, model.RateStorageDetailID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_InsuredAmt", SqlDbType.Float, 0, ParameterDirection.Input, model.InsuranceValue);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_PremPercent", SqlDbType.Float, 0, ParameterDirection.Input, model.InsurancePercent);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_InsBillFreqID", SqlDbType.Int, 0, ParameterDirection.Input, model.InsuranceCycleID);
                        
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_RateDetails", SqlDbType.Xml, 0, ParameterDirection.Input, RateDetailXml);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_RateDateFrom", SqlDbType.DateTime, 0, ParameterDirection.Input, model.RateFromDate);
                        
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LoginID", SqlDbType.Int, 0, ParameterDirection.Input, UserSession.GetUserSession().LoginID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@RETURNSTATUS", SqlDbType.SmallInt, 0, ParameterDirection.ReturnValue);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@Out_Message", SqlDbType.NVarChar, -1, ParameterDirection.Output);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_InsDateFrom", SqlDbType.DateTime, 0, ParameterDirection.Input, model.InsuranceDate);

                        conn.ExecuteProcedure(ProcedureReturnType.SingleValue);

                        if (!conn.IsError)
                        {
                            int ReturnStatus = Convert.ToInt32(conn.GetParameterValue(ParameterOF.PROCEDURE, "@RETURNSTATUS"));
                            result = Convert.ToString(conn.GetParameterValue(ParameterOF.PROCEDURE, "@Out_Message"));

                            if (ReturnStatus == 0)
                            {
                                model.StorageID = Convert.ToInt64(conn.GetParameterValue(ParameterOF.PROCEDURE, "@SP_StrgJobMasterID"));
                                //model.StorageDetailID = Convert.ToInt64(conn.GetParameterValue(ParameterOF.PROCEDURE, "@SP_InsPremiunForJobID"));

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
                throw new DataAccessException(Convert.ToString(LoginID), "StorageDAL", "InsertRate", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

        }

        public DataSet GetRateInsDetails(int LoginID,Int64 MoveID,Int64 StorageID, Int64 StorageDetailID)
        {
            DataSet Ds = new DataSet();
            try
            {
                string query = string.Format("[Strg].[GetStrgRateDetails] @SP_StrgJobMasterID={0},@SP_MoveID={1},@SP_LoginID={2},@SP_StrgVolDetailID={3}",
                CSubs.QSafeValue(Convert.ToString(StorageID)),
                CSubs.QSafeValue(Convert.ToString(MoveID)),
                CSubs.QSafeValue(Convert.ToString(LoginID)),
                CSubs.QSafeValue(Convert.ToString(StorageDetailID))
                );

                Ds = CSubs.GetDataSet(query);
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoginID), "StorageDAL", "GetStorageRatesDetail", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

            return Ds;

        }

        public DataSet GetStorageRates(int LoginID, Int64 MoveID, Int64? Storage_ID)
        {
            DataSet Ds = new DataSet();

            try
            {
                string query = string.Format("[Strg].[GetStorageDetails] @SP_MoveID={0},@SP_StorageID={1},@SP_LoginID={2}",
                CSubs.QSafeValue(Convert.ToString(MoveID)),
                CSubs.QSafeValue(Convert.ToString(Storage_ID)),
                CSubs.QSafeValue(Convert.ToString(LoginID)));

                Ds = CSubs.GetDataSet(query);
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoginID), "StorageDAL", "GetStorageRates", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

            return Ds;
        }

        public DataTable GetStorageIDForJob(int LoginID,Int64 MoveID)
        {
            try
            {
                string query = string.Format("[Strg].[CheckStrgForJob] @SP_MoveID={0},@SP_LoginID={1}",
                CSubs.QSafeValue(Convert.ToString(MoveID)),
                CSubs.QSafeValue(Convert.ToString(LoginID)));

                return CSubs.GetDataTable(query);
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoginID), "StorageDAL", "GetStorageIDForJob", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

        }

        public bool ApproveStorage(JobStorageApproveDTO model,  int LoginID, out string result)
        {
            result = String.Empty;

            try
            {
                using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {
                        conn.AddCommand("[Strg].[ApproveStrgJob]", QueryType.Procedure);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_MoveID", SqlDbType.BigInt, 0, ParameterDirection.Input, model.MoveID <= 0 ? -1 : model.MoveID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IsRemoveApproval", SqlDbType.Bit, 0, ParameterDirection.Input, model.IsApproved);
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
                return true;

            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoginID), "StorageDAL", "ApproveStorage", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

        }
    }
}