using System;

namespace RELOCBS.Entities
{
    public class Support
    {

        public RemoveInvApproval RemoveInvApproval { get; set; }

        public ChangeCityInJob ChangeCityInJob { get; set; }

        public ChangeRefInInv ChangeRefInInv { get; set; }

        public ChangeRevBr ChangeRevBr { get; set; }
    }

    public class RemoveInvApproval
    {
        public string InvNo { get; set; }
    }

    public class ChangeCityInJob
    {
        public string JobNo { get; set; }
        public string OrgOrDest { get; set; }
        public Int64? NewCityID { get; set; }
    }

    public class ChangeRefInInv
    {
        public string InvNo { get; set; }
        public string NewRefNo { get; set; }
    }

    public class ChangeRevBr
    {
        public string JobNo { get; set; }
        public int? RevBrID { get; set; }
    }
}