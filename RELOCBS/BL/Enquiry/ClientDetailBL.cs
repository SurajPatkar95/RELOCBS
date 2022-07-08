using RELOCBS.Common.ExceptionHandling;
using RELOCBS.DAL.Enquiry;
using RELOCBS.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace RELOCBS.BL.Enquiry
{
    public class ClientDetailBL
    {
        private ClientDetailDAL _clientDetailDAL;

        public ClientDetailDAL clientDetailDAL
        {
            get
            {
                if (this._clientDetailDAL == null)
                    this._clientDetailDAL = new ClientDetailDAL();
                return this._clientDetailDAL;
            }
        }

        public Entities.ClientDetails GetClientDetail(int ClientId, char Mode)// Mode('A'=Client, 'C'=Corporate)
        {
            try
            {
                Entities.ClientDetails _clientDet = new Entities.ClientDetails();
                DataTable dt = clientDetailDAL.GetClientDetail(ClientId,Mode);
                if (dt != null && dt.Rows.Count > 0)
                {
					if (Mode.Equals('A'))
					{
						_clientDet.BussLnID = dt.Rows[0]["BussLineID"] == DBNull.Value ? null : Convert.ToString(dt.Rows[0]["BussLineID"]);
						_clientDet.BussLnName = dt.Rows[0]["BussLineName"] == DBNull.Value ? null : Convert.ToString(dt.Rows[0]["BussLineName"]);
						_clientDet.ContactPerson = dt.Rows[0]["ContactPerson"] == DBNull.Value ? null : Convert.ToString(dt.Rows[0]["ContactPerson"]);
						_clientDet.ClientGSTNO = dt.Rows[0]["ClientGSTNO"] == DBNull.Value ? null : Convert.ToString(dt.Rows[0]["ClientGSTNO"]);
					}
					else if (Mode.Equals('C'))
					{
						_clientDet.AccountMgr = dt.Rows[0]["AccMGRID"] == DBNull.Value ? null : Convert.ToString(dt.Rows[0]["AccMGRID"]);

						_clientDet.AccountGSTNO = dt.Rows[0]["AccountGSTNO"] == DBNull.Value ? null : Convert.ToString(dt.Rows[0]["AccountGSTNO"]);
					}
                    
                }
                return _clientDet;
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "ClientDetailBL", "GetClientDetail", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
        }
    }
}