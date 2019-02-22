using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RELOCBS.Utility
{
	public static class EnumUtility
	{

        public enum PageAction
        {
            VIEW = 1,
            ADD = 2,
            EDIT = 3,
            DELETE = 4,
            Queue = 101,
            Assigned = 102,
            Completed = 103,
            Cancel = 104,
            Approved = 105,
            Hold = 106,
            Task = 1001,
            Phone = 1002,
            eMail = 1003,
            Visit = 1004,
            Meeting = 1005,
            Gotopricing = 1101,
            SendApproval = 1102,
            InvoiceList = 1103,
            ViewTimeline = 1104,
            OADAChange = 1105,
            MoveAssign = 1106,
            ManageInvoiceDraft = 1107,
            ManageInvoiceAudit = 1108,
            ManageInvoiceApprove = 1109,
            ManageInvoiceFinal = 1110,
            RegCodes = 1111,
            UOMValues = 1112,
            BankDetails = 1113,
            RMCCoordinator = 1114,
            AgentContactPerson = 1115,
            HistoryRates = 1116,
            UploadRates = 1117,
            CityRates = 1118,
            UOMCategory = 1119,
            MoveDetails = 1120,
            MoveCoordinator = 1121,
            PricingView = 1122,
            MoveView = 1123,
            BillingView = 1124,
            NONE = 99
        }

    }
}