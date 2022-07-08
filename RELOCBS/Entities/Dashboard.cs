using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace RELOCBS.Entities
{
    public class Dashboard
    {
        public List<DataPoint> dataPoint { get; set; }
        public List<DataPoint> dataPoint2 { get; set; }
        public List<DataPoint> dataPoint3 { get; set; }
        public List<DataPoint> dataPoint_Pie { get; set; }
        public UserDetails UserDetails { get; set; } = new UserDetails();
        public List<JobDetail> JobDetail { get; set; }
    }

    public class ScheduleSurveyDashboard
    {
            [Required(ErrorMessage = "Month required")]
            public DateTime ForMonthDate { get; set; }
            [Required(ErrorMessage = "Branch required")]
            public int? BranchId { get; set; }
            public DataSet data { get; set; }
    }

    public class SurveyorEvent
    {
        
        public string EnqNo { get; set; }
        public string Shipper { get; set; }
        public string SchTime { get; set; }
        public string Surveyor { get; set; }

    }

    public class CrewUtilizationDashboard
    {
        //[DataType(DataType.Date)]
        [Required(ErrorMessage = "From Month required")]
        //[DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime FromMonthDate { get; set; }

        //[DataType(DataType.Date)]
        [Required(ErrorMessage = "Branch required")]
        //[DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime ToMonthDate { get; set; }
        public DataSet data { get; set; }
    }


    [DataContract]
    public class DataPoint
    {
        //public DataPoint(string x, string y)
        //{
        //    this.x = x;
        //    this.y = y;
        //}

        //Explicitly setting the name to be used while serializing to JSON.
        [DataMember(Name = "label")]
        public string x = null;


        public int StatusID;
        public int indexLabel { get; set; }
        //Explicitly setting the name to be used while serializing to JSON.
        [DataMember(Name = "y")]
        public Nullable<int> y = null;
    }

    public class UserDetails
    {
        public int UserID { get; set; }
    }

    public class JobDetail
    {
        public int? MoveID { get; set; }
        
        public int? TransactionID { get; set; }
        public int? JobStatusID { get; set; }
        public int? UserID { get; set; }
        public string JobNo { get; set; }
        public string ServiceLine { get; set; }
        public string Controller { get; set; }
        public string JobDate { get; set; }
        public string CurrStage { get; set; }
        public string ShipperName { get; set; }
        public string Mode { get; set; }
        public string Client { get; set; }
        public string Origin { get; set; }
        public string OrgCountry { get; set; }
        public string Destination { get; set; }
        public string DestCountry { get; set; }
        public string SD { get; set; }
        public string FollowupDate { get; set; }
        public string NextFollowupDate { get; set; }
        public string NextFollowupReason { get; set; }
        public string OrderNo { get; set; }
        public string[] Reason { get; set; }
        public string Remark { get; set; }
        public bool CancelFollowUP { get; set; }
        public List<FollowUpDetails> FollowUpList { get; set; } = new List<FollowUpDetails>();
    }

    

}