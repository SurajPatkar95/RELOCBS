using RELOCBS.App_Code;
using RELOCBS.Common.ExceptionHandling;
using RELOCBS.Entities;
using RELOCBS.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace RELOCBS.DAL.Enquiry
{
    public class ClientDetailDAL
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

        public DataTable GetClientDetail(int ClientId,char Mode)
        {
            try
            {
				string query = Mode.Equals('A') ? "EXEC [Comm].[GetController]" : "EXEC [Comm].[GETAcctDetailsForAgent]";
				
                query = query + string.Format("@SP_AgentID={0}",
                 CSubs.QSafeValue(Convert.ToString(ClientId)));
                return CSubs.GetDataTable(query);
                
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(UserSession.GetUserSession().LoginID), "ClientDetailDAL", "GetClientDetail", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
        }
    }
}