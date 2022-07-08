
using PagedList;
using RELOCBS.App_Code;
using RELOCBS.BL;
using RELOCBS.BL.FundTransfer;
using RELOCBS.Common;
using RELOCBS.Controllers;
using RELOCBS.CustomAttributes;
using RELOCBS.Extensions;
using RELOCBS.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RELOCBS.Entities
{
    [AuthorizeUser]
    public class FundTransferController : BaseController
    {
        string _PageID;

        private CommonSubs _cSubs;
		public CommonSubs CSubs
		{
			get
			{
				if (this._cSubs == null)
					this._cSubs = new CommonSubs();
				return this._cSubs;
			}
		}
		private FundTransferBL _fundTranferBL;
		public FundTransferBL fundTranferBL
		{
			get
			{
				if (this._fundTranferBL == null)
					this._fundTranferBL = new FundTransferBL();
				return this._fundTranferBL;
			}
		}
		
		private ComboBL _comboBL;
		public ComboBL comboBL
		{
			get
			{
				if (this._comboBL == null)
					this._comboBL = new ComboBL();
				return this._comboBL;
			}
		}
		ComboBL combo = new ComboBL();

		// GET: FundTransfer
		public ActionResult Index(FundTranfer fa, int page = 1)
		{
			//if (!CSubs.CheckPageRights(_PageID, PermissionType.VIEW))
			//{
			//	return new HttpStatusCodeResult(403);
			//}

			session.Set<string>("PageSession", "Transfer To FA");
			string sort = "BillNo";
			string sortdir = "desc";
			string search = "";
			string BillNo = "";
			int pageSize = settings.GetSettingByKey<int>("pagination_pagesize");
			string OrderBy = "";
			int Order = 0;
			DateTime? Fromdate = null;//System.DateTime.Now.Date.AddDays(-2);
			DateTime? Todate = null;//System.DateTime.Now;
			string Shipper = "";
			FillCombo();
			//fa.FromDate = fa.FromDate
			//string SearchKey = string.Empty;
			//if (Request.Form["FromDate"] != null && Request.Form["FromDate"].Trim() != "")
			//{
			//	Fromdate = Convert.ToDateTime(Request.Form["FromDate"]);
			//}
			//if (Request.Form["BillNo"] != null && Request.Form["BillNo"].Trim() != "")
			//{
			//	BillNo = Convert.ToString(Request.Form["BillNo"]);
			//}
			//if (Request.Form["ToDate"] != null && Request.Form["ToDate"].Trim() != "")
			//{
			//	Todate = Convert.ToDateTime(Request.Form["ToDate"]);
			//}

			//if (Request.Params["grid-column"] != null && Request.Params["grid-column"].Trim() != "")
			//{
			//	sort = Request.Params["grid-column"].Trim().ToString();
			//}
			//if (Request.Params["grid-dir"] != null && Request.Params["grid-dir"].Trim() != "")
			//{
			//	Order = Convert.ToInt32(Request.Params["grid-dir"].Trim().ToString());

			//	if (Order == 1)
			//	{
			//		sortdir = "asc";
			//	}
			//	else
			//	{
			//		sortdir = "desc";
			//	}
			//}
			//if (Request.Params["search"] != null && Request.Params["search"].Trim() != "")
			//{
			//	search = Request.Params["search"].Trim().ToString();
			//}
			//int totalRecord = 0;
			//if (page < 1) page = 1;
			//int skip = (page * pageSize) - pageSize;
			ViewBag.htmlString = Convert.ToString(TempData["htmltable"]);
			System.Data.DataTable dt = new System.Data.DataTable();
			var data = fundTranferBL.GetFDGridDetails(fa, sort, sortdir, pageSize, out dt);
			//FundTranfer objfa = new FundTranfer();
			fa.InvGrid = data.ToList() ;
			TempData["FAList"] = dt;
			return View(fa);
					/*Request.IsAjaxRequest()
					? (ActionResult)PartialView("_AjaxPartial", itemsAsIPagedList)
					: View(itemsAsIPagedList);*/
			
		}

		public ActionResult Export(FundTranfer FAList)
		{
			string htmltable = null;
			try
			{
				if (TempData["FAList"] != null)
				{
					
					System.Data.DataTable dt = ((System.Data.DataTable)TempData["FAList"]);
					System.Data.DataTable dtFAList = dt.Clone();
					FAList.InvGrid = FAList.InvGrid.Where(x => x.IsExport).ToList();
					string msg = string.Empty;
					bool res = false;
					if (FAList.SearchFor == "Exported")
					{
						res = true;
						msg = "Data Exported Successfully.";
					}
					else
					{
						res = fundTranferBL.InsertTransferFA(FAList, out msg);
					}
					
					if (res)
					{
						foreach (var item in FAList.InvGrid)
						{
							
							DataRow row = dt.Select("[InvOrCredit] ='" + item.InvOrCredit + "' and [CBSRefID]='" + item.CBSRefID + "'").First();
							row.SetField("Paid to /Received From (2nd Leg)", item.AccountCode);
							string Code = item.AccountCode+ "-" +Convert.ToString(row["Customer Code"]).Split(new char[]{'-'})[1];
							row.SetField("Customer Code", Code);
							Code = item.AccountCode + "-" + Convert.ToString(row["Place of Supply"]).Split(new char[] {'-'})[1];
							row.SetField("Place of Supply", Code);
							dtFAList.Rows.Add(row.ItemArray);
							
						}
						dtFAList.Columns.Remove("InvOrCredit");
						dtFAList.Columns.Remove("CBSRefID");
						dtFAList.Columns.Remove("BillTo");

						
						TempData["htmltable"] = MakeHtmlTable(dtFAList);

						//RELOCBS.Common.CommonService.DataTabletoExcel("Transfer To FA", dtFAList);
						this.AddToastMessage("RELOCBS", msg, ToastType.Success);
					}
					else
					{
						this.AddToastMessage("RELOCBS", msg, ToastType.Error);
					}
				}
			}
			catch
			{
				this.AddToastMessage("RELOCBS", "Error in saving.", ToastType.Error);
				
			}
			
			return RedirectToAction("Index", "FundTransfer");
		}

		private void FillCombo(string ForPage="")
		{
			List<SelectListItem> list = new List<SelectListItem>();
			bool RMCBuss = !(UserSession.GetUserSession().BussinessLine == "NON RMC-BUSINESS");
			list.Add(new SelectListItem() { Value = "Exported", Text = "Exported" });
			list.Add(new SelectListItem() { Value = "Finalized", Text = "Finalized" });
			ViewData["SearchFor"] = list;
			
			ViewData["RevenueBranchList"] = comboBL.GetCompanyBranchDropdown(CompanyID:UserSession.GetUserSession().CompanyID,IsRMCBuss: RMCBuss,IsRev:true,ForPage: ForPage);
			ViewData["ServiceLineList"] = comboBL.GetServiceLineDropdown(RMCBuss:RMCBuss);
		}


        public ActionResult Dubai(FundTranfer model, int page = 1)
        {
            _PageID = "62";

            if (!CSubs.CheckPageRights(_PageID, PermissionType.VIEW))
            {
                return new HttpStatusCodeResult(403);
            }

            session.Set<string>("PageSession", "Transfer To FA GCC");
            //DateTime Fromdate = model.FromDate;
            //DateTime Todate = model.ToDate;
            FillCombo(ForPage: "TransFA_GCC");
            ViewBag.htmlString = Convert.ToString(TempData["htmltable"]);
            string message;
            var data = fundTranferBL.GetTransferToFADubai(model, out message);
            TempData["FAList"] = data;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DubaiExport(FundTranfer model, string SearchForText)
        {
            _PageID = "62";
            if (!CSubs.CheckPageRights(_PageID, PermissionType.VIEW))
            {
                return new HttpStatusCodeResult(403);
            }
            session.Set<string>("PageSession", "Transfer To FA Dubai");
            FillCombo(ForPage: "TransFA_Duabi");

            //if (ModelState.IsValid)
            //{
            ///Check if the Exported or Finalized is selected
            if (!string.IsNullOrWhiteSpace(SearchForText))
            {
                
                string message = string.Empty;

                if (TempData["FAList"] != null)
                {
                    DataTable data = (DataTable)TempData["FAList"];
                    //System.Data.DataTable dtFAList = data.Clone();
                    model.SearchFor = SearchForText;
                    DataTable result = fundTranferBL.GetDubaiTAExport(model, data, out message);

                    if (result!=null && result.Rows.Count > 0)
                    {
                        TempData["htmltable"] = MakeHtmlTable(result);
                        TempData["FAList"] = null;
                        this.AddToastMessage("RELOCBS", message, ToastType.Success);
                    }
                    else
                    {
                        this.AddToastMessage("RELOCBS", message, ToastType.Error);
                    }
                }
                else
                {
                    this.AddToastMessage("RELOCBS", "No data to Export", ToastType.Error);
                }
            }
            else
            {
                this.AddToastMessage("RELOCBS", "Status is required", ToastType.Error);
            }
            //}

            return RedirectToAction("Dubai");
        }

        private string MakeHtmlTable(DataTable data)
		{
			string[] table = new string[data.Rows.Count];
			long counter = 1;
			string htmlstring = "<tr>";
			foreach (DataColumn col in data.Columns)
			{
				htmlstring += "<th bgcolor=\"#DCDCDC\">" + col.ColumnName + "</th>";
			}
			htmlstring += "</tr>";
			foreach (DataRow row in data.Rows)
			{
				table[counter - 1] = "<tr><td>" + String.Join("</td><td>", (from o in row.ItemArray select o.ToString().Trim()).ToArray()) + "</td></tr>";

				counter += 1;
			}

			return "<table border=\"2px\">" + htmlstring + String.Join("", table) + "</table>";
		}
        
    }
}