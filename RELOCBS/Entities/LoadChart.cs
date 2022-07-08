using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RELOCBS.Entities
{
    public class LoadCharts
    {
        public Int64 LoadChartID { get; set; }

        public int BranchID { get; set; }

        public string BranchName { get; set; }

        public int ModeID { get; set; }

        public string TLCID { get; set; }

        public DateTime LoadChartDate { get; set; } = System.DateTime.Now;

        public string TruckNo { get; set; }

        public string SealNo { get; set; }

        public string Transporter { get; set; }

        public int TransporterID { get; set; }

        public int VehicleTypeID { get; set; }

        public string VehicleType { get; set; }

        public string VehicleSize { get; set; }

        public string VehicleCapacity { get; set; }

        public string EscortedBy { get; set; }

        public int? EscortedByID { get; set; }

        public string EscortedByBranch { get; set; }

        public int? EscortedByBranchID { get; set; }

        public DateTime? LeftOnDate { get; set; }

        public string LoadedAtBranchID { get; set; }

        public string LoadedToBranchID { get; set; }

        public string LoadedViaBranchID { get; set; }

        public List<LoadChartBranchAccess> BranchAccess { get; set; } = new List<LoadChartBranchAccess>();

        public string HFVBranchAccess { get; set; }

        public List<LoadChartShipment> loadChartShipments { get; set; } = new List<LoadChartShipment>();

        public string HFVloadChartShipments { get; set; }
        
        public bool IsOutSideVehicle { get; set; }

        public bool IsDirectDelivery { get; set; }

        public bool IsTallyChartPrepared { get; set; }

        public bool IsTallyChartSentToLoc { get; set; }

        public decimal CostForVehicle { get; set; }

        public string Remarks { get; set; }

        public string Notes { get; set; }

        public string Common { get; set; }
    }

    public class LoadChartBranchAccess
    {
        public Int64 BranchAccessID { get; set; }

        public string BranchAccessTypeID { get; set; }

        public string BranchAccessType { get; set; }

        public int BranchID { get; set; }

        public string Branch { get; set; }

    }

    public class LoadChartShipment
    {
        public Int64 ShipmentID { get; set; }

        public int ServiceLineID { get; set; }

        public string ServiceLine { get; set; }

        public string AgentName { get; set; }

        public string AccountName { get; set; }

        public Int64 MoveID { get; set; }

        public string JobNo { get; set; }

        public string Shipper { get; set; }

        public string NoOfPacsDetails { get; set; }

        public decimal Vol { get; set; }

        public decimal Revenue { get; set; }

        public decimal ApproxCost { get; set; }

        public decimal Savings { get; set; }

        public string LoadAt { get; set; }

        public string LoadedBySupervisor { get; set; }

        public string Mode { get; set; }

        public int? LoadedBySupervisorID { get; set; }

    }

    public class LoadChartsGrid
    {

        public Int64 LoadChartID { get; set; }

        public string Branch { get; set; }

        public string Mode { get; set; }

        public string TLCID { get; set; }

        public DateTime? LoadChartDate { get; set; }

        public string TruckNo { get; set; }

        public string SealNo { get; set; }

        public string Transporter { get; set; }

        public string EscartByEMP { get; set; }
        public string EscartBranch { get; set; }


    }
}