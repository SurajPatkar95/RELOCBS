using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace RELOCBS.Services.Implementation
{
    public class Settings
    {

        RELOCBS.App_Code.CommonSubs _CSubs;

        public RELOCBS.App_Code.CommonSubs CSubs
        {
            get
            {
                if (this._CSubs == null)
                    this._CSubs = new RELOCBS.App_Code.CommonSubs();
                return this._CSubs;
            }
        }

        public T GetSettingByKey<T>(string Key)
        {

            //object value = CSubs.GetValue(string.Format("Exec [Access].[GetConfigKeyValue] @sp_keyname={0}", CSubs.QSafeValue(Key))); 
            object value=null;
            DataTable dtObj = new DataTable();

            dtObj= CSubs.GetDataTable(string.Format("Exec [Access].[GetConfigKeyValue] @sp_keyname={0}", CSubs.QSafeValue(Key)));

            if (dtObj!=null && dtObj.Rows.Count>0)
            {
                value = dtObj.Rows[0][0];
            }

            return (T)Convert.ChangeType(value, typeof(T));
        }

    }
}