using RELOCBS.BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RELOCBS.Entities;
using RELOCBS.BL.TransClearTax;
using System.Data;
using RELOCBS.Utility;
using RELOCBS.App_Code;
using RELOCBS.Extensions;
using RELOCBS.BL.Billing;

namespace RELOCBS.Controllers
{
	public class TransClearTaxController : BaseController
	{
		// GET: TransClearTax
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

		private TransClearTaxBL _transClearTaxBL;
		public TransClearTaxBL transClearTaxBL
		{
			get
			{
				if (this._transClearTaxBL == null)
					this._transClearTaxBL = new TransClearTaxBL();
				return this._transClearTaxBL;

			}
		}

		public ActionResult Index()
		{
			FillCombo();
			session.Set<string>("PageSession", "Transfer To Clear Tax");
			TransClearTax trans = new TransClearTax();
			return View(trans);
		}

		[HttpPost]
		public ActionResult Index(TransClearTax trans)
		{
			FillCombo();
			trans = transClearTaxBL.getTransClearTax(trans);

			return View(trans);
		}

		public void FillCombo()
		{
			ViewData["ApplicationList"] = comboBL.GetTransFA_AppDropdown();
		}

		public JsonResult DownloadToExcel(string InvNo, int AppID)
		{
			string AppName = comboBL.GetTransFA_AppDropdown().Where(x => x.Value == Convert.ToString(AppID)).FirstOrDefault().Text;

			//InvNo = "FC19201001317";
			//AppName = "oldcbs";
			Dictionary<string, string> exptoExlParameters = new Dictionary<string, string>();
			string errormsg = string.Empty;
			string htmlstring = string.Empty;
			int Colcount = 0;
			try
			{
				DataTable dtgridData = new DataTable();
				exptoExlParameters.Add("@SP_InvoiceNumber", Convert.ToString(InvNo));
				exptoExlParameters.Add("@SP_LoginID", Convert.ToString(UserSession.GetUserSession().LoginID));
				exptoExlParameters.Add("@SP_CompID", Convert.ToString(UserSession.GetUserSession().CompanyID));
				exptoExlParameters.Add("@SP_AppName", Convert.ToString(AppName));
				string param = string.Join(",", exptoExlParameters.Select(x => x.Key + "=" + CSubs.QSafeValue(x.Value)).ToArray()).TrimEnd(',');
				string query = string.Format("EXEC {0} {1}", "[E_Invoice].[EInvoiceSchemaOtherApp]", param);
				dtgridData = CSubs.GetDataTable(query);
				Colcount = dtgridData.Columns.Count;
				htmlstring = "<tr>";
				foreach (DataColumn col in dtgridData.Columns)
				{
					htmlstring += "<th bgcolor='#DCDCDC'>" + col.ColumnName + "</th>";
				}
				htmlstring += "</tr>";
				//htmlstring += "<tr>";
				foreach (DataRow row in dtgridData.Rows)
				{
					if (string.IsNullOrEmpty(Convert.ToString(row[0])))
						htmlstring += "<tr>";
					else
						htmlstring += "<tr>";


					foreach (DataColumn col in dtgridData.Columns)
					{
						if (string.IsNullOrEmpty(Convert.ToString(row[0])))
							htmlstring += "<td bgcolor='#B0C4DE' style=\"font-weight:bold\">" + row[col.ColumnName].ToString() + "</td>";
						else
							htmlstring += "<td>" + row[col.ColumnName].ToString() + "</td>";

					}
					htmlstring += "</tr>";


				}

				//htmlstring += "<tr>";
				//string SearchKey = string.Empty;
				//if (Request.Form["SearchKey"] != null && Request.Form["SearchKey"].Trim() != "")
				//{
				//	param.Add("@SP_SearchString", Request.Form["SearchKey"]);
				//}


				//CommonService.GenerateExcel(this.Response, "CostSheet", "[MoveMan].[GetCostSheet]", param);

			}
			catch (Exception ex)
			{
				this.AddToastMessage("RELOCBS", "UnExpected Error occured", ToastType.Error);
			}

			return Json(new { htmlstring = htmlstring, errormsg = errormsg, ColCount = Colcount }, JsonRequestBehavior.AllowGet);
			//return View();
		}

		public JsonResult GenerateEInvoice(string InvNo, string AppID)
		{
			string AppName = comboBL.GetTransFA_AppDropdown().Where(x => x.Value == Convert.ToString(AppID)).FirstOrDefault().Text;
			string result = string.Empty;
			result = transClearTaxBL.GenerateEInvoice(InvNo, AppName);
			return Json(new { result = result }, JsonRequestBehavior.AllowGet);
		}

		public JsonResult SaveImage(string InvNo, string AppID)
		{
			string msg = "";
			try
			{
				string AppName = comboBL.GetTransFA_AppDropdown().Where(x => x.Value == Convert.ToString(AppID)).FirstOrDefault().Text;
				msg = transClearTaxBL.GetQRCode(InvNo, AppName);
			}
			catch (Exception e)
			{
				msg = "Error in saving image";
				//this.AddToastMessage("RELOCBS", "Error in saving image", ToastType.Error);
			}
			return Json(new { error = msg }, JsonRequestBehavior.AllowGet);
		}

		public JsonResult GetPOSRule(string InvNo, string AppID)
		{
			string msg = "";
			GSTLogic GSTLogic = new GSTLogic();
			try
			{
				string AppName = comboBL.GetTransFA_AppDropdown().Where(x => x.Value == Convert.ToString(AppID)).FirstOrDefault().Text;
				GSTLogic = transClearTaxBL.GetPOSRule(InvNo, AppName);
				//this.AddToastMessage("RELOCBS", "Image successfully saved", ToastType.Success);

			}
			catch (Exception e)
			{
				msg = "Error in fetching";
				//this.AddToastMessage("RELOCBS", msg, ToastType.Error);
			}
			return Json(new { GSTLogic = GSTLogic, error = msg }, JsonRequestBehavior.AllowGet);
		}

		public ActionResult BillFormatPrint_OthApp(string key)
		{
			Billing objmodel = new Billing();
			BillingBL billingBL = new BillingBL();
			objmodel = billingBL.GetDetailById(0, 0, 0, 'I', 3, key, true);

			objmodel.BillType = (char)'I';
			return View(objmodel);
			try
			{
				// TODO: Add delete logic here

				return RedirectToAction("Index");
			}
			catch
			{
				return View();
			}
		}
	}
}