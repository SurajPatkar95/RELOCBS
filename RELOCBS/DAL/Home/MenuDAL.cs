using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace RELOCBS.DAL.Home
{
    public class MenuDAL
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

        public DataTable InitializeMenu(string LoginID)
        {
            DataSet dstMenu = CSubs.GetDataSet(string.Format("EXEC Access.GetAssignedMenuItems 'ASSIGNED',{0}; EXEC Access.GetAssignedMenuItems 'ALL',''", CSubs.QSafeValue(LoginID)));
            DataRow drADD;

            DataTable dtMenuTable = new DataTable("MENU");
            dtMenuTable.Columns.Add("MENUID", typeof(Int32));
            dtMenuTable.Columns.Add("MENUNAME", typeof(String));
            dtMenuTable.Columns.Add("MENULINK", typeof(String));
            dtMenuTable.Columns.Add("PARENTMENUID", typeof(Int32));
            dtMenuTable.Columns.Add("MENUORDER", typeof(Int32));
            dtMenuTable.Columns.Add("PARENTORDER", typeof(Int32));
            dtMenuTable.Columns.Add("ALLOW_VIEW", typeof(Int32));
            dtMenuTable.Columns.Add("ALLOW_ADD", typeof(Int32));
            dtMenuTable.Columns.Add("ALLOW_EDIT", typeof(Int32));
            dtMenuTable.Columns.Add("ALLOW_DELETE", typeof(Int32));
            dtMenuTable.Columns.Add("ISBLANK", typeof(bool));
            dtMenuTable.Columns.Add("SHOWINMENU", typeof(bool));
            dtMenuTable.Columns.Add("CONTROLLER", typeof(String));
            dtMenuTable.Columns.Add("IMGPATH", typeof(String));


            foreach (DataRow DR in dstMenu.Tables[0].Rows)
            {
                drADD = dtMenuTable.NewRow();
                drADD["MENUID"] = DR["MENUID"];
                drADD["MENUNAME"] = DR["MENUNAME"];
                drADD["MENULINK"] = DR["MENULINK"];
                drADD["PARENTMENUID"] = DR["PARENTMENUID"];
                drADD["MENUORDER"] = DR["MENUORDER"];
                drADD["PARENTORDER"] = DR["PARENTORDER"];
                drADD["ALLOW_VIEW"] = DR["ALLOW_VIEW"];
                drADD["ALLOW_ADD"] = DR["ALLOW_ADD"];
                drADD["ALLOW_EDIT"] = DR["ALLOW_EDIT"];
                drADD["ALLOW_DELETE"] = DR["ALLOW_DELETE"];
                drADD["ISBLANK"] = DR["ISBLANK"];
                drADD["SHOWINMENU"] = DR["SHOWINMENU"];
                drADD["CONTROLLER"] = DR["CONTROLLER"];
                drADD["IMGPATH"] = DR["IMGPATH"];
                

                dtMenuTable.Rows.Add(drADD);
                dtMenuTable.AcceptChanges();
                if ((Int32)drADD["PARENTMENUID"] != 0)
                    CheckParentExists((Int32)drADD["PARENTMENUID"], dtMenuTable, dstMenu.Tables[1]);
            }

            return dtMenuTable;
            
        }

        private void CheckParentExists(Int32 prmMenuId, DataTable prmAssignedTable, DataTable prmAllTable)
        {
            DataRow[] dr = prmAssignedTable.Select("MENUID=" + prmMenuId);
            DataRow drADD;
            if (dr.Length <= 0)
            {
                DataRow[] drNew = prmAllTable.Select("MENUID=" + prmMenuId);
                drADD = prmAssignedTable.NewRow();
                drADD["MENUID"] = drNew[0]["MENUID"];
                drADD["MENUNAME"] = drNew[0]["MENUNAME"];
                drADD["MENULINK"] = drNew[0]["MENULINK"];
                drADD["PARENTMENUID"] = drNew[0]["PARENTMENUID"];
                drADD["MENUORDER"] = drNew[0]["MENUORDER"];
                drADD["PARENTORDER"] = drNew[0]["PARENTORDER"];
                drADD["ALLOW_VIEW"] = drNew[0]["ALLOW_VIEW"];
                drADD["ALLOW_ADD"] = drNew[0]["ALLOW_ADD"];
                drADD["ALLOW_EDIT"] = drNew[0]["ALLOW_EDIT"];
                drADD["ALLOW_DELETE"] = drNew[0]["ALLOW_DELETE"];
                drADD["ISBLANK"] = drNew[0]["ISBLANK"];
                drADD["SHOWINMENU"] = drNew[0]["SHOWINMENU"];
                drADD["CONTROLLER"] = drNew[0]["CONTROLLER"];
                drADD["IMGPATH"] = drNew[0]["IMGPATH"];

                prmAssignedTable.Rows.Add(drADD);
                prmAssignedTable.AcceptChanges();
                if ((Int32)drADD["PARENTMENUID"] != 0)
                    CheckParentExists((Int32)drADD["PARENTMENUID"], prmAssignedTable, prmAllTable);
            }
        }

    }
}