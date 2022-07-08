using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RELOCBS.Entities
{
    public class VehicleKms
    {

        public Int64? VehicleKmsID { get; set; }
        public int BranchID { get; set; }
        public int VehicleID { get; set; }
        public string VehicleNo { get; set; }
        public DateTime OdometerDate { get; set; } = System.DateTime.Now;

        public float StartOdometer { get; set; }
        public float EndOdometer { get; set; }
        public float TotalOdometer { get; set; }
        public string Remarks { get; set; }

        public List<VehicleKmsJobs> kmsJobs { get; set; } = new List<VehicleKmsJobs>();
        public List<VehicleKmsTravelLocation> travelLocations { get; set; } = new List<VehicleKmsTravelLocation>();
        public string HFVkmsJobs {get;set;}
        public string HFVLocations { get; set; }
    }

    public class VehicleKmsJobs
    {
        public Int64 VehicleKmsJobID { get; set; }

        public int ServiceLineID { get; set; }

        public string ServiceLine { get; set; }

        public string AgentName { get; set; }

        public string AccountName { get; set; }

        public Int64 MoveID { get; set; }

        public string JobNo { get; set; }

        public string Shipper { get; set; }

        public string NoOfPacsDetails { get; set; }

        public string Remark { get; set; }

    }

    public class VehicleKmsTravelLocation
    {
        public int VehicleKmsTravelID { get; set; }

        public String FromLocation { get; set; }

        public String ToLocation { get; set; }

        public string Remark { get; set; }

    }

    public class VehicleKmsGrid
    {

        public Int64 VehicleKmsID { get; set; }
        public string VehicleNo { get; set; }

        public string BranchName { get; set; }
        public DateTime OdometerDate { get; set; }

        public float StartOdometer { get; set; }
        public float EndOdometer { get; set; }

        public float TotalOdometer { get; set; }

        public string Remarks { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }


    }
}