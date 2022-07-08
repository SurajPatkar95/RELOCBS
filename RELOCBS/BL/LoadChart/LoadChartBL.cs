using RELOCBS.Common.ExceptionHandling;
using RELOCBS.DAL.LoadChart;
using RELOCBS.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Linq.Dynamic;
using RELOCBS.Entities;
using System.Data;

namespace RELOCBS.BL.LoadChart
{
    public class LoadChartBL
    {

        private LoadChartDAL _loadChartDAL;

        public LoadChartDAL loadChartDAL
        {

            get
            {
                if (this._loadChartDAL == null)
                    this._loadChartDAL = new LoadChartDAL();
                return this._loadChartDAL;
            }
        }


        public IEnumerable<Entities.LoadChartsGrid> GetGrid(DateTime? FromDate, DateTime? Todate, string sort, string sortdir, int skip, int pageSize, out int totalCount, string TCLId = null, string TransporterId = null,string Shipper=null, Int64? JobNo=null)
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
                IQueryable<Entities.LoadChartsGrid> List = loadChartDAL.GetGrid(FromDate, Todate, RMCBuss, TCLId, TransporterId, Shipper , JobNo);
                if (List != null)
                {
                    totalCount = List.Count();

                    if (pageSize > 1)
                    {
                        List = List.Skip((skip * (pageSize - 1))).Take(skip);
                    }
                    else
                    {
                        List = List.Take(skip);
                    }

                    if (!string.IsNullOrEmpty(sort))
                    {
                        List = List.OrderBy(sort + " " + sortdir);
                    }
                    return List.ToList();
                }
                else
                {
                    return new List<Entities.LoadChartsGrid>();
                }
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "LoadChartBL", "GetJobReportList", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

        }

        public LoadCharts GetDetail(Int64 LoadChartID)
        {
            LoadCharts model = new LoadCharts();

            try
            {
                DataSet data = loadChartDAL.GetDetail(UserSession.GetUserSession().LoginID, LoadChartID);

                if (data != null && data.Tables.Count > 0)
                {
                    if (data.Tables.Count > 0 && data.Tables[0] != null && data.Tables[0].Rows.Count > 0)
                    {
                        model = (from item in data.Tables[0].AsEnumerable()
                               select new LoadCharts()
                               {
                                   LoadChartID = !string.IsNullOrWhiteSpace(Convert.ToString(item["LoadChartMasterID"])) ? Convert.ToInt64(item["LoadChartMasterID"]) : -1,
                                   TLCID = Convert.ToString(item["LoadChartMasterID"]),
                                   ModeID = Convert.ToInt32(item["ModeID"]),
                                   BranchID = Convert.ToInt32(item["BranchID"]),
                                   TransporterID = Convert.ToInt32(item["TransporterID"]),
                                   //MoveId = Convert.ToInt64(item["MoveID"]),
                                   TruckNo = Convert.ToString(item["VehicleNumber"]),
                                   SealNo = Convert.ToString(item["SealNo"]),
                                   VehicleTypeID = Convert.ToInt32(item["VechTypeID"]),
                                   //VehicleSize = Convert.ToString(item["VechSize"]),
                                   //VehicleCapacity = Convert.ToString(item["VechCapacity"]),
                                   LeftOnDate = string.IsNullOrWhiteSpace(Convert.ToString(item["StartDate"]))? (DateTime?)null : Convert.ToDateTime(item["StartDate"]),
                                   LoadedAtBranchID = Convert.ToString(item["LoadedAt"]),
                                   LoadedViaBranchID = Convert.ToString(item["Via"]),
                                   LoadedToBranchID = Convert.ToString(item["LoadedTo"]),
                                   IsOutSideVehicle  =!string.IsNullOrWhiteSpace(Convert.ToString(item["OutSideVeh"])) ? Convert.ToBoolean(item["OutSideVeh"]) : false,
                                   IsDirectDelivery = !string.IsNullOrWhiteSpace(Convert.ToString(item["DirectDelivery"])) ? Convert.ToBoolean(item["DirectDelivery"]) : false,
                                   IsTallyChartPrepared = !string.IsNullOrWhiteSpace(Convert.ToString(item["TallyChartPrepared"])) ? Convert.ToBoolean(item["TallyChartPrepared"]) : false,
                                   IsTallyChartSentToLoc = !string.IsNullOrWhiteSpace(Convert.ToString(item["TallyChartSentToLoc"])) ? Convert.ToBoolean(item["TallyChartSentToLoc"]) : false,
                                   CostForVehicle = !string.IsNullOrWhiteSpace(Convert.ToString(item["CostForVeh"])) ? Convert.ToDecimal(item["CostForVeh"]) : 0,
                                   EscortedByID = !string.IsNullOrWhiteSpace(Convert.ToString(item["EscartByEMPID"])) ? Convert.ToInt32(item["EscartByEMPID"]) : 0,
                                   EscortedByBranchID = !string.IsNullOrWhiteSpace(Convert.ToString(item["EscartBranchID"])) ? Convert.ToInt32(item["EscartBranchID"]) : 0,
                                   EscortedBy = Convert.ToString(item["EscortedBy"]),
                                   EscortedByBranch = Convert.ToString(item["EscortedByBranch"]),
                                   Remarks =Convert.ToString(item["Remarks"]),
                                   VehicleType = Convert.ToString(item["VehTypeName"]),
                                   Transporter = Convert.ToString(item["TransporterName"]),
                               }).FirstOrDefault();

                    }

                    ////Shipment Details
                    if (data.Tables.Count > 1 && data.Tables[1] != null && data.Tables[1].Rows.Count > 0)
                    {
                        model.loadChartShipments = (from item in data.Tables[1].AsEnumerable()
                                        select new LoadChartShipment()
                                        {
                                            ShipmentID = Convert.ToInt64(item["LoadChartDetID"]),
                                            MoveID = Convert.ToInt64(item["MoveID"]),
                                            JobNo = Convert.ToString(item["JobID"]),
                                            AccountName = Convert.ToString(item["AccountName"]),
                                            Shipper = Convert.ToString(item["ShipperName"]),
                                            Vol = Convert.ToDecimal(item["Vol"]),
                                            Revenue = Convert.ToDecimal(item["RevAmt"]),
                                            ApproxCost = Convert.ToDecimal(item["CostAmt"]),
                                            LoadAt = Convert.ToString(item["LoadedAt"]),
                                            NoOfPacsDetails =Convert.ToString(item["NoOfPacsDetails"]),
                                            Mode = Convert.ToString(item["Mode"]),
                                            LoadedBySupervisorID = !string.IsNullOrWhiteSpace(Convert.ToString(item["LoadedBySupervisorID"])) ? Convert.ToInt32(item["LoadedBySupervisorID"]) : 0,
                                            LoadedBySupervisor = Convert.ToString(item["LoadedBySupervisor"]),
                                        }).ToList();
                    }

                    ////Branch Access details
                    if (data.Tables.Count > 2 && data.Tables[2] != null && data.Tables[2].Rows.Count > 0)
                    {
                        model.BranchAccess = (from item in data.Tables[2].AsEnumerable()
                                              select new LoadChartBranchAccess()
                                              {
                                                  BranchID = Convert.ToInt32(item["BranchID"]),
                                                  BranchAccessTypeID = Convert.ToString(item["OrgDestVia"]),
                                                  BranchAccessType = Convert.ToString(item["OrgDestVia"]),
                                              }).ToList();
                    }
                    
                }
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "LoadChartBL", "GetDetails", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

            return model;
        }


        public LoadChartShipment GetJobDetail(Int64 MoveID)
        {
            LoadChartShipment model = new LoadChartShipment();

            try
            {
                DataTable data = loadChartDAL.GetJobDetail(UserSession.GetUserSession().LoginID, MoveID);

                if (data!=null && data.Rows.Count>0)
                {

                    model = (from item in data.AsEnumerable()
                             select new LoadChartShipment()
                             {
                                 MoveID = MoveID,
                                 JobNo = Convert.ToString(item["JobID"]),
                                 AccountName = Convert.ToString(item["AccountName"]),
                                 Shipper = Convert.ToString(item["ShipperName"]),
                             }).FirstOrDefault();

                }
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "LoadChartBL", "GetJobDetail", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

            return model;

        }

        public bool Insert(LoadCharts model, out string result)
        {
            try
            {
                return loadChartDAL.Insert(model, UserSession.GetUserSession().LoginID, out result);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "LoadChartBL", "Insert", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

        }



    }
}