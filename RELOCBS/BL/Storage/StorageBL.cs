using RELOCBS.Common.ExceptionHandling;
using RELOCBS.DAL.Storage;
using RELOCBS.Entities;
using RELOCBS.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Linq.Dynamic;
using System.Data;

namespace RELOCBS.BL.Storage
{
    public class StorageBL
    {

        private StorageDAL _storageDAL;

        public StorageDAL storageDAL
        {

            get
            {
                if (this._storageDAL == null)
                    this._storageDAL = new StorageDAL();
                return this._storageDAL;
            }
        }

        public IEnumerable<Entities.JobStorageGrid> GetStorageGrid(DateTime? FromDate, DateTime? Todate, bool IsJobDate, bool IsStrgDate, Int64 MoveId, string Shipper, string sort, string sortdir, int skip, int pageSize, out int totalCount)
        {
            totalCount = 0;

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
                IQueryable<Entities.JobStorageGrid> storage = storageDAL.GetStorageGrid(FromDate, Todate, IsJobDate, IsStrgDate, MoveId, Shipper, RMCBuss, UserSession.GetUserSession().CompanyID);
                if (storage != null)
                {
                    totalCount = storage.Count();

                    if (pageSize > 1)
                    {
                        storage = storage.Skip((skip * (pageSize - 1))).Take(skip);
                    }
                    else
                    {
                        storage = storage.Take(skip);
                    }

                    storage = storage.OrderBy(sort + " " + sortdir);

                    return storage.ToList();
                }
                else
                {
                    return new List<Entities.JobStorageGrid>();
                }
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "StorageBL", "GetStorageGrid", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

        }

        public JobStorage GetStorageDetails(Int64 MoveID, Int64? Storage_ID = 0)
        {
            JobStorage model = new JobStorage();

            try
            {
                model.jobDetail.MoveID = MoveID;
                model.StorageID = Storage_ID;

                DataSet data = storageDAL.GetStorageDetails(UserSession.GetUserSession().LoginID, MoveID, Storage_ID);

                if (data != null && data.Tables.Count > 0)
                {
                    if (data.Tables.Count > 0 && data.Tables[0] != null && data.Tables[0].Rows.Count > 0)
                    {
                        model = (from item in data.Tables[0].AsEnumerable()
                                 select new JobStorage()
                                 {
                                     jobDetail = new StorageJobDetail()
                                     {
                                         MoveID = Convert.ToInt64(item["MoveID"]),
                                         Controller = Convert.ToString(item["Controller"]),
                                         JobCommodity = Convert.ToString(item["Commodity"]),
                                         QuotationID = Convert.ToString(item["QuotationID"]),
                                         ShipperName = Convert.ToString(item["ShipperName"]),
                                         Client = Convert.ToString(item["ClientName"]),
                                         Corporate = Convert.ToString(item["Corporate"]),
                                         JobNo = Convert.ToString(item["JobID"]),
                                         //JobDate = Convert.ToDateTime(item["JobNo"]),
                                         ServiceLine = Convert.ToString(item["ServiceLine"]),
                                         ShipperAddress = Convert.ToString(item["ShipperAddress"]),


                                     },
                                     SD_BranchID = !string.IsNullOrWhiteSpace(Convert.ToString(item["BrSdEmpID"])) ? Convert.ToInt32(item["BrSdEmpID"]) : 0,
                                     SD_HOID = !string.IsNullOrWhiteSpace(Convert.ToString(item["HoSdEmpID"])) ? Convert.ToInt32(item["HoSdEmpID"]) : 0,
                                     InsuredByID = !string.IsNullOrWhiteSpace(Convert.ToString(item["InsurBy"])) ? Convert.ToInt32(item["InsurBy"]) : 0,
                                     CurrID = !string.IsNullOrWhiteSpace(Convert.ToString(item["BaseCurrID"])) ? Convert.ToInt32(item["BaseCurrID"]) : 0,
                                     StorageCommodityID = !string.IsNullOrWhiteSpace(Convert.ToString(item["CommodityID"])) ? Convert.ToInt32(item["CommodityID"]) : 0,
                                     ApprovalStatus = Convert.ToBoolean(item["ApproveStatus"]),
                                     ApprovedBY = Convert.ToString(item["ApproveBy"]),
                                     ApprovedDate = !string.IsNullOrWhiteSpace(Convert.ToString(item["ApproveDate"])) ? Convert.ToDateTime(item["ApproveDate"]) : (DateTime?)null,
                                     BtnApproveLable = Convert.ToBoolean(item["ApproveStatus"]) ? "Approved" : "Pending",
                                     BaseCurrName = Convert.ToString(item["BaseCurrName"])

                                 }).FirstOrDefault();

                        if (data.Tables.Count>1 && data.Tables[1]!=null && data.Tables[1].Rows.Count>0)
                        {

                            DataRow item = data.Tables[1].Rows[0];
                            model.StorageID = !string.IsNullOrWhiteSpace(Convert.ToString(item["StrgJobMasterID"])) ? Convert.ToInt64(item["StrgJobMasterID"]) : 0;
                            model.StorageCommodityID = !string.IsNullOrWhiteSpace(Convert.ToString(item["CommodityID"])) ? Convert.ToInt32(item["CommodityID"]) : 0;
                            model.WarehouseID = !string.IsNullOrWhiteSpace(Convert.ToString(item["WarehouseID"])) ? Convert.ToInt32(item["WarehouseID"]) : 0;
                            model.CurrID = !string.IsNullOrWhiteSpace(Convert.ToString(item["CurrID"])) ? Convert.ToInt32(item["CurrID"]) : 0;
                            model.BillStartDate = !string.IsNullOrWhiteSpace(Convert.ToString(item["BillStartDate"])) ? Convert.ToDateTime(item["BillStartDate"]) : (DateTime?)null;
                            model.StorageEntryDate = !string.IsNullOrWhiteSpace(Convert.ToString(item["StrgEntryDate"])) ? Convert.ToDateTime(item["StrgEntryDate"]) : (DateTime?)null;
                            model.StorageExitDate = !string.IsNullOrWhiteSpace(Convert.ToString(item["StrgExitDate"])) ? Convert.ToDateTime(item["StrgExitDate"]) : (DateTime?)null;
                            model.DocRecDate = !string.IsNullOrWhiteSpace(Convert.ToString(item["DocRecdDate"])) ? Convert.ToDateTime(item["DocRecdDate"]) : (DateTime?)null;
                            model.FileCloseDate = !string.IsNullOrWhiteSpace(Convert.ToString(item["FileCloseDate"])) ? Convert.ToDateTime(item["FileCloseDate"]) : (DateTime?)null;
                            model.IsInsured = !string.IsNullOrWhiteSpace(Convert.ToString(item["IsInsured"])) ? Convert.ToBoolean(item["IsInsured"]) : false;
                            model.InsuredByID = !string.IsNullOrWhiteSpace(Convert.ToString(item["InsuredBy"])) ? Convert.ToInt32(item["InsuredBy"]) : 0;
                            model.SD_BranchID = !string.IsNullOrWhiteSpace(Convert.ToString(item["SDFromBr"])) ? Convert.ToInt32(item["SDFromBr"]) : 0;
                            model.SD_HOID = !string.IsNullOrWhiteSpace(Convert.ToString(item["SDFromHO"])) ? Convert.ToInt32(item["SDFromHO"]) : 0;
                            model.PackDate = !string.IsNullOrWhiteSpace(Convert.ToString(item["PackDate"])) ? Convert.ToDateTime(item["PackDate"]) : (DateTime?)null;
							model.LoadDate = !string.IsNullOrWhiteSpace(Convert.ToString(item["LoadDate"])) ? Convert.ToDateTime(item["LoadDate"]) : (DateTime?)null;
							model.StrgStateID = !string.IsNullOrWhiteSpace(Convert.ToString(item["PackStateID"])) ? Convert.ToInt32(item["PackStateID"]) : 0;
                            model.StorageDetails = Convert.ToString(item["StorageDetails"]);
                            model.CreatedBY = Convert.ToString(item["prepareBy"]);
                            model.CreatedDate = !string.IsNullOrWhiteSpace(Convert.ToString(item["prepareDate"])) ? Convert.ToDateTime(item["prepareDate"]) : (DateTime?)null;
                            model.StrgCityID = !string.IsNullOrWhiteSpace(Convert.ToString(item["PackCityID"])) ? Convert.ToInt32(item["PackCityID"]) : 0; 
                        }

                        if (data.Tables.Count > 2 && data.Tables[2] != null && data.Tables[2].Rows.Count > 0)
                        {
                            model.StorageList = (from item in data.Tables[2].AsEnumerable()
                                                    select new StorageDetails()
                                                    {
                                                        StorageDetailID = !string.IsNullOrWhiteSpace(Convert.ToString(item["StrgVolDetailID"])) ? Convert.ToInt64(item["StrgVolDetailID"]) : 0,
                                                        IsBilled = !string.IsNullOrWhiteSpace(Convert.ToString(item["isBilled"])) ? Convert.ToInt32(item["isBilled"]) : 0,
                                                        VolumeUnitID = !string.IsNullOrWhiteSpace(Convert.ToString(item["WtUnitID"])) ? Convert.ToInt32(item["WtUnitID"]) : 0,
                                                        VolumeUnit = Convert.ToString(item["WeightUnitName"]),
                                                        VolumeCFT = !string.IsNullOrWhiteSpace(Convert.ToString(item["WtVol"])) ? Convert.ToDecimal(item["WtVol"]) : 0,
                                                        VolumeDate = !string.IsNullOrWhiteSpace(Convert.ToString(item["DateFrom"])) ? Convert.ToDateTime(item["DateFrom"]) : (DateTime?)null,
                                                        VolumeRemark =Convert.ToString(item["Remarks"]),
                                                        InsuranceValue= !string.IsNullOrWhiteSpace(Convert.ToString(item["InsuredValue"])) ? Convert.ToDecimal(item["InsuredValue"]) : 0,
                                                        InsurancePercent = !string.IsNullOrWhiteSpace(Convert.ToString(item["PremPercent"])) ? Convert.ToDecimal(item["PremPercent"]) : 0,
                                                        InsuranceCycleID = !string.IsNullOrWhiteSpace(Convert.ToString(item["PeriodFreqID"])) ? Convert.ToInt32(item["PeriodFreqID"]) : 0,
                                                        InsuranceCycle = Convert.ToString(item["PeriodFreq"]),
                                                        InsuranceDate = !string.IsNullOrWhiteSpace(Convert.ToString(item["InsuredDateFrom"])) ? Convert.ToDateTime(item["InsuredDateFrom"]) : (DateTime?)null,
                                                    }).ToList();

                            model.rateStorageList = model.StorageList;
                        }

                        if (data.Tables.Count > 3 && data.Tables[3] != null && data.Tables[3].Rows.Count > 0)
                        {
                            DataRow item = data.Tables[3].Rows[0];
                            model.AsOnDate= !string.IsNullOrWhiteSpace(Convert.ToString(item["DateFrom"])) ? Convert.ToDateTime(item["DateFrom"]) : (DateTime?)null;
                            model.Strg_Inc_percent = !string.IsNullOrWhiteSpace(Convert.ToString(item["PercentInc"])) ? Convert.ToDecimal(item["PercentInc"]) : 0;
                            model.Months = !string.IsNullOrWhiteSpace(Convert.ToString(item["PeriodFreqID"])) ? Convert.ToInt32(item["PeriodFreqID"]) : 0;
                        }

                        if (data.Tables.Count > 4 && data.Tables[4] != null && data.Tables[4].Rows.Count > 0)
                        {
                            model.Approver = true;
                        }    
                    }

                }
                
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "StorageBL", "GetInsuranceDetails", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

            return model;
        }

        public bool Insert(JobStorage model, out string result)
        {
            try
            {
                return storageDAL.Insert(model, UserSession.GetUserSession().LoginID, out result);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "StorageBL", "Inset", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

        }

        public bool InsertRate(JobStorage model, out string result)
        {
            try
            {
                return storageDAL.InsertRate(model, UserSession.GetUserSession().LoginID, out result);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "StorageBL", "InsertRate", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

        }

        public JobStorage GetRateInsDetails(Int64 MoveID, Int64 StorageID, Int64 StorageDetailID)
        {
            JobStorage obj = new JobStorage();

            try
            {
                DataSet data = storageDAL.GetRateInsDetails(UserSession.GetUserSession().LoginID, MoveID, StorageID, StorageDetailID);
                if (data != null && data.Tables.Count > 0)
                {
                    if (data.Tables.Count > 0 && data.Tables[0] != null && data.Tables[0].Rows.Count > 0)
                    {
                        DataRow item = data.Tables[0].Rows[0];
                        obj.Strg_Inc_percent = !string.IsNullOrWhiteSpace(Convert.ToString(item["PercentInc"])) ? Convert.ToDecimal(item["PercentInc"]) : 0;
                        obj.AsOnDate = !string.IsNullOrWhiteSpace(Convert.ToString(item["AsOnDate"])) ? Convert.ToDateTime(item["AsOnDate"]) : (DateTime?)null;
                        obj.Months = !string.IsNullOrWhiteSpace(Convert.ToString(item["PeriodFreqID"])) ? Convert.ToInt32(item["PeriodFreqID"]) : 0;

                    }


                    if (data.Tables.Count > 1 && data.Tables[1] != null && data.Tables[1].Rows.Count > 0)
                    {

                        DataRow item = data.Tables[1].Rows[0];
                        obj.RateStorageDetailID = Convert.ToInt64(item["StrgVolDetailID"]);
                        obj.RateVolumeCFT = Convert.ToDecimal(item["WtVol"]);
                        obj.RateVolumeUnitID = Convert.ToInt32(item["WtUnitID"]);
                        obj.VolumeRemark = Convert.ToString(item["Remarks"]);
                        obj.InsuranceCycleID = !string.IsNullOrWhiteSpace(Convert.ToString(item["InsPeriodFreqID"])) ? Convert.ToInt32(item["InsPeriodFreqID"]) : 0;
                        obj.InsuranceValue = !string.IsNullOrWhiteSpace(Convert.ToString(item["InsuredValue"])) ? Convert.ToDecimal(item["InsuredValue"]) : 0;
                        obj.InsurancePercent = !string.IsNullOrWhiteSpace(Convert.ToString(item["InsPremPercent"])) ? Convert.ToDecimal(item["InsPremPercent"]) : 0; 
                        obj.IsRateVolumeBilled = !string.IsNullOrWhiteSpace(Convert.ToString(item["InsPremPercent"])) ? Convert.ToInt16(item["isBilled"]) : 0;
                        obj.VolumeDate = !string.IsNullOrWhiteSpace(Convert.ToString(item["WtDateFrom"])) ? Convert.ToDateTime(item["WtDateFrom"]) : (DateTime?)null;
                        obj.RateFromDate = obj.VolumeDate;
                        obj.InsuranceDate = obj.VolumeDate;
                    }

                    if (data.Tables.Count > 2 && data.Tables[2] != null && data.Tables[2].Rows.Count > 0)
                    {
                        //obj.RateFromDate = !string.IsNullOrWhiteSpace(Convert.ToString(data.Tables[2].Rows[0]["DateFrom"])) ? Convert.ToDateTime(data.Tables[2].Rows[0]["DateFrom"]) : (DateTime?)null;
                        obj.ratesList = (from item in data.Tables[2].AsEnumerable()
                                         select new StorageRateDetails()
                                         {
                                             CostHeadid = Convert.ToInt32(item["CostHeadID"]),
                                             CostHead = Convert.ToString(item["StrgCostHeadName"]),
                                             IsBilled = Convert.ToInt32(item["isBilled"]),
                                             Rate = Convert.ToDecimal(item["TotAmtQuoted"]),
                                             RatePeriod = Convert.ToString(item["FreqName"]),
                                             RatePeriodID = Convert.ToInt32(item["PeriodFreqID"]),
                                             VolumeCFT = Convert.ToDecimal(item["WtVol"]),
                                             RatePerUnit = Convert.ToDecimal(item["RatePerMonthPerCFT"]),
                                             VolumeUnit = Convert.ToString(item["WeightUnitName"]),
                                             //JobDate = Convert.ToDateTime(item["JobNo"]),
                                             VolumeUnitID = Convert.ToInt32(item["WtUnitID"]),
                                         }

                                 ).ToList();
                    }
                }
                
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "StorageBL", "GetInsuranceDetails", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

            return obj;
        }

        public StorageRate GetStorageRates(Int64 StorageID, Int64 WeightID)
        {
            StorageRate obj = new StorageRate();

            try
            {
                DataSet data = storageDAL.GetStorageRates(UserSession.GetUserSession().LoginID, StorageID, WeightID);
                if (data != null && data.Tables.Count > 0)
                {

                    if (data.Tables.Count > 0 && data.Tables[0] != null && data.Tables[0].Rows.Count > 0)
                    {

                        DataRow item = data.Tables[0].Rows[0];
                        obj.RateVolumeCFT = Convert.ToDecimal(item["RateVolumeCFT"]);
                        obj.RateVolumeUnitID = Convert.ToInt32(item["RateVolumeUnitID"]);
                        obj.Strg_Inc_percent = Convert.ToDecimal(item["Strg_Inc_percent"]);
                        obj.AsOnDate = Convert.ToDateTime(item["AsOnDate"]);
                        obj.Months = Convert.ToInt32(item["Months"]);
                    }

                    if (data.Tables.Count > 1 && data.Tables[1] != null && data.Tables[1].Rows.Count > 0)
                    {
                        obj.ratesList = (from item in data.Tables[1].AsEnumerable()
                                         select new StorageRateDetails()
                                         {
                                             CostHeadid = Convert.ToInt32(item["CostHeadid"]),
                                             CostHead = Convert.ToString(item["CostHead"]),
                                             IsBilled = Convert.ToInt32(item["IsBilled"]),
                                             Rate = Convert.ToDecimal(item["Rate"]),
                                             RatePeriod = Convert.ToString(item["RatePeriod"]),
                                             RatePeriodID = Convert.ToInt32(item["RatePeriodID"]),
                                             VolumeCFT = Convert.ToDecimal(item["VolumeCFT"]),
                                             RatePerUnit = Convert.ToDecimal(item["RatePerUnit"]),
                                             VolumeUnit = Convert.ToString(item["VolumeUnit"]),
                                             //JobDate = Convert.ToDateTime(item["JobNo"]),
                                             VolumeUnitID = Convert.ToInt32(item["VolumeUnitID"]),
                                         }

                                 ).ToList();
                    }
                }
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "StorageBL", "GetStorageRates", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

            return obj;
        }

        public Int64 GetStorageIDForJob(Int64 MoveID)
        {
            try
            {
                DataTable data = storageDAL.GetStorageIDForJob(UserSession.GetUserSession().LoginID, MoveID);
                if (data != null && data.Rows.Count > 0)
                {
                    return Convert.ToInt64(data.Rows[0]["StrgJobMasterID"]);

                }

                return 0;
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "StorageBL", "GetStorageIDForJob", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

        }

        public bool ApproveStorage(JobStorageApproveDTO model,  int LoginID, out string result)
        {
            result = String.Empty;

            try
            {
                return storageDAL.ApproveStorage(model,LoginID, out result);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(LoginID), "StorageBL", "ApproveStorage", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

        }
    }
}