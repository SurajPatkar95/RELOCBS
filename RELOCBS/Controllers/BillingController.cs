using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
//using RELOCBS.Entities;
using RELOCBS.BL.Billing;
using RELOCBS.Entities;
using RELOCBS.Common;
using RELOCBS.BL;
using RELOCBS.Utility;
using RELOCBS.Extensions;
using RELOCBS.App_Code;
using PagedList;
using RELOCBS.CustomAttributes;
using System.Data;

namespace RELOCBS.Controllers
{
	[AuthorizeUser]
	public class BillingController : BaseController
	{
		private bool _IsStatement = false;
		private string _PageID = "25";

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

		private BillingBL _billingBL;
		public BillingBL billingBL
		{
			get
			{
				if (this._billingBL == null)
					this._billingBL = new BillingBL();
				return this._billingBL;
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

		// GET: Billing
		//public ActionResult Index()
		//      {
		//          return View();
		//      }

		public ActionResult Index(int page = 1)
		{
			if (!CSubs.CheckPageRights(_PageID, PermissionType.VIEW))
			{
				return new HttpStatusCodeResult(403);
			}
			session.Set<string>("PageSession", "Billing");
			string sort = "";
			string sortdir = "";
			string search = "";
			string searchType = "";
			//string sort = "InvID";
			//string sortdir = "asc";
			//string search = "";

			int pageSize = settings.GetSettingByKey<int>("pagination_pagesize");
			string OrderBy = "";
			int Order = 0;
			DateTime? Fromdate = null;
			DateTime? Todate = null;
			int? InvoiceID = null;
			string Shipper = null;
			string Status = null;
			string SearchKey = string.Empty;
			bool RMCBuss = !UserSession.GetUserSession().BussinessLine.Equals("NON RMC-BUSINESS");
			if (Request.Form["FromDate"] != null && Request.Form["FromDate"].Trim() != "")
			{
				Fromdate = Convert.ToDateTime(Request.Form["FromDate"]);
			}

			if (Request.Form["ToDate"] != null && Request.Form["ToDate"].Trim() != "")
			{
				Todate = Convert.ToDateTime(Request.Form["ToDate"]);
			}

			if (Request.Params["grid-column"] != null && Request.Params["grid-column"].Trim() != "")
			{
				sort = Request.Params["grid-column"].Trim().ToString();
			}
			if (Request.Params["grid-dir"] != null && Request.Params["grid-dir"].Trim() != "")
			{
				Order = Convert.ToInt32(Request.Params["grid-dir"].Trim().ToString());

				if (Order == 1)
				{
					sortdir = "asc";
				}
				else
				{
					sortdir = "desc";
				}

			}
			if (Request.Form["InvoiceID"] != null && Request.Form["InvoiceID"].Trim() != "")
			{
				InvoiceID = Convert.ToInt32(Request.Form["InvoiceID"]);
			}
			if (Request.Form["search"] != null && Request.Form["search"].Trim() != "")
			{
				search = Convert.ToString(Request.Form["search"]);
			}

			if (Request.Form["SearchType"] != null && Request.Form["SearchType"].Trim() != "")
			{
				searchType = Convert.ToString(Request.Form["SearchType"]);
			}
			if (Request.Params["Shipper"] != null && Request.Params["Shipper"].Trim() != "")
			{
				Shipper = Request.Params["Shipper"].Trim().ToString();
			}
			if (Request.Params["Status"] != null && Request.Params["Status"].Trim() != "")
			{
				Status = Request.Params["Status"].Trim().ToString();
			}
			int totalRecord = 0;
			if (page < 1) page = 1;
			int skip = (page * pageSize) - pageSize;

			var data = billingBL.GetForGrid(Fromdate, Todate, search, searchType, InvoiceID, Shipper, Status, sort, sortdir, skip, pageSize, 'I', out totalRecord, out RMCBuss);

			ViewBag.TotalRows = totalRecord;
			ViewBag.RMCBuss = RMCBuss;
			ViewBag.search = search;
			var itemsAsIPagedList = new StaticPagedList<Billing>(data, page, pageSize, totalRecord);

			//var CNdata = 
			if (itemsAsIPagedList.Count>0)
			{
				ViewBag.CreditNoteGrid = searchType == "CreditNoteNo" && itemsAsIPagedList.ToList().First().IsShowCreditNote ? GetCreditNoteList(Convert.ToInt32(itemsAsIPagedList.ToList().First().BillID)) : new List<Billing>();
			}
			else
			{
				ViewBag.CreditNoteGrid = new List<Billing>();
			}
			//ViewBag.CreditNoteGrid = new StaticPagedList<Billing>(data, page, pageSize, totalRecord);
			//return (ActionResult)PartialView("_AjaxPartial", itemsAsIPagedList);
			return Request.IsAjaxRequest()
				? (ActionResult)PartialView("_AjaxPartial", itemsAsIPagedList)
				: View(itemsAsIPagedList);
		}

		public ActionResult GetCreditNote(int InvoiceID)
		{
			List<Billing> list = GetCreditNoteList(InvoiceID);
			ViewBag.CreditNoteGrid = list;
			return PartialView("_AjaxCreditNotePartial", list);
		}

		public List<Billing> GetCreditNoteList(int InvoiceID)
		{
			int totalRecord = 0;
			bool RMCBuss = !UserSession.GetUserSession().BussinessLine.Equals("NON RMC-BUSINESS");
			List<Billing> list = new List<Billing>();
			list = billingBL.GetForGrid(null, null, null, null, InvoiceID, null, null, null, null, 0, 0, 'C', out totalRecord, out RMCBuss).ToList();
			return list;
		}

		// GET: Billing/Details/5
		public ActionResult Details(int id)
		{
			return View();
		}


        /// <summary>
        ///GET: Billing/Create // type= "AI" (Alter iNVOICE) AND "NI" (New Invoice) AND "NC" (New CreditNote) AND "AC" (Alter CreditNote)  
        /// StrgKey Param  used as StrogeInvID to identify Bill as storage Bill
        /// </summary>
        public ActionResult Create(int? key, int PageIndex, string type,int? StrgKey=null)
		{
			Billing ObjInv = new Billing();
			int? InvoiceID = 0; int? MoveID = 0; int? CreditNoteID = 0;
			char Type = 'I';
			if (type.Equals("AI"))
			{
				session.Set<string>("PageSession", "Billing");
				InvoiceID = key;
			}
			else if (type.Equals("NI"))
			{
				session.Set<string>("PageSession", "Billing");
				MoveID = key;
			}
			else if (type.Equals("AC"))
			{
				session.Set<string>("PageSession", "Credit Note");
				Type = 'C';
				CreditNoteID = key;
			}
			else if (type.Equals("NC"))
			{
				session.Set<string>("PageSession", "Credit Note");
				Type = 'C';
				InvoiceID = key;
			}

           
            
			ObjInv = billingBL.GetDetailById(MoveID, InvoiceID, CreditNoteID, Type, PageIndex, StrgInvID : StrgKey);
			ObjInv.BillType = Type;
            ObjInv.StrgInvID = StrgKey;
			ViewBag.Comp = UserSession.GetUserSession().CompanyID==2 ? "BTR" : "" ;
            int BillToAgentID = 0;
            if (Type == 'I')
            {
                if (ObjInv.RMCID > 0 && ObjInv.BillToID == "Client")
                {
                    BillToAgentID = ObjInv.RMCID;
                }
                else if (ObjInv.BillToID == "Client")
                {
                    BillToAgentID = ObjInv.BillToClientID;
                }
                else if (ObjInv.BillToID == "Corporate")
                {
                    BillToAgentID = ObjInv.BillToAccountID;
                }
            }
            
            
            
            FillCombo(ObjInv.MoveID, BillToAgentID, ObjInv.RMCID, UserSession.GetUserSession().BussinessLine[0], ObjInv.BillItems.Count);
			ObjInv = SetDefaultValues(ObjInv);
			ViewBag.ShowStatement = Convert.ToBoolean(TempData["_IsStatement"]);
			return View(ObjInv);
		}

		// POST: Billing/Create
		[HttpPost]
		public ActionResult Create(Billing billing, string SubmitInvoice)
		{
			try
			{
				bool CNValidate = true;
                int? StrgInvID = billing.StrgInvID;
				FillCombo(billing.MoveID, billing.RMCID > 0 ? billing.RMCID : billing.BillToClientID, billing.RMCID, UserSession.GetUserSession().BussinessLine[0], billing.BillItems != null ? billing.BillItems.Count : 0);
				
				if (billing.OrgCountry == billing.DestCountry && billing.OrgCountry == "INDIA" && billing.Mode == "Road"
					&& SubmitInvoice.Equals("Send to Finance") && billing.BillType == 'I')
				{
					CNValidate = !string.IsNullOrEmpty(billing.VehicleNo) && !string.IsNullOrEmpty(Convert.ToString(billing.NoofPkgs));
				}
				if (ModelState.IsValid && CNValidate)
				{
					bool Isauditamount = true;
					bool SaveTopOnly = false;
					string status = SubmitInvoice;
					string outMsg = "";
					if (billing.InvoiceStatus == "Approved" || billing.InvoiceStatus == "Finalized" || billing.InvoiceStatus == "Send To Finance")
					{
						Isauditamount = false;
					}
					if ((string.IsNullOrEmpty(billing.InvoiceStatus) || billing.InvoiceStatus == "Draft") && (SubmitInvoice.Equals("Save Invoice")))
					{
						status = "Draft";
					}
					if (SubmitInvoice.Equals("Approve"))
					{
						status = "Approved";
						Isauditamount = false;
					}
					if (SubmitInvoice.Equals("Final Approve"))
					{
						status = "Finalized";
						Isauditamount = false;
					}

					if (SubmitInvoice.Equals("Send to Finance") || SubmitInvoice.Equals("Send to SD"))
					{
						Isauditamount = false;
					}
					if (billing.BillType == 'C')
					{
						Isauditamount = false;
					}
					if (SubmitInvoice.Equals("Get Tax"))
					{
						SaveTopOnly = true;
						status = "Draft";
					}
					bool success = false;
					if (SubmitInvoice == "Statement of Charges")
					{
						success = billingBL.SaveStatement(billing);
					}
					else
					{
						success = billingBL.SaveInvoice(billing, status, Isauditamount, SaveTopOnly, out outMsg);
					}

					if (success)
					{

						//return PartialView("Statement_Print", billing);


						if (!SaveTopOnly)
						{
							this.AddToastMessage("RELOCBS", outMsg, ToastType.Success);
						}

						if (SubmitInvoice.Equals("Send to Finance"))
						{
							return RedirectToActionPermanent("Index", "MoveManage");
						}
						else
						{

							int? id = billing.BillType == 'C' ? billing.CreditNoteID : billing.BillID;
							TempData["_IsStatement"] = SubmitInvoice == "Statement of Charges";
							return RedirectToActionPermanent("Create", new { key = id, PageIndex = 3, type = (billing.BillType == 'C' ? "AC" : "AI"),StrgKey = StrgInvID });
						}

					}
					else
					{
						this.AddToastMessage("RELOCBS", "Error in saving.", ToastType.Error);
					}

				}
				else
				{
					char Type = billing.BillType;
					billing = billingBL.GetDetailById(billing.MoveID, billing.BillID, billing.CreditNoteID, billing.BillType, 3);
					billing.BillType = Type;
                    billing.StrgInvID = StrgInvID;
					FillCombo(billing.MoveID, billing.RMCID > 0 ? billing.RMCID : billing.BillToClientID, billing.RMCID, UserSession.GetUserSession().BussinessLine[0], billing.BillItems.Count);
					billing = SetDefaultValues(billing);
					if (!CNValidate)
					{
						ModelState.AddModelError("", "Vehicle No and No. of pkgs is mandatory.");
					}
					else
					{
						this.AddToastMessage("RELOCBS", "Error in saving.", ToastType.Error);
					}

				}
				// TODO: Add insert logic here
				ViewBag.Comp = UserSession.GetUserSession().CompanyID == 2 ? "BTR" : "";
				return View(billing);
			}
			catch
			{
				return View();
			}
		}

		private void FillCombo(int MoveID, int RMCID = 0,int CRRMCID = 0,char BussLine = 'R',int? count = 0)
		{
			ViewData["CityList"] = comboBL.GetCityDropdown(ContinentID: -1, CountryID: -1);
			ViewData["RateCurrency"] = comboBL.GetRateCurrencyDropdown();
			ViewData["Sequence"] = comboBL.SequenceDropDown(Count: count);
			ViewData["StorageState"] = comboBL.GetStorageStateDropdown(MoveID);
			ViewData["PaymentTerm"] = comboBL.GetPaymentTermList();
			ViewData["BTRServiceList"] = comboBL.GetBTRServiceList();
            ViewData["BillingEntityList"] = comboBL.GetBillingEntityList(RMCID, BillType: 'I',BussLine:BussLine);
            ViewData["CreditNoteEntityList"] = comboBL.GetBillingEntityList(CRRMCID, BillType: 'C');
            ViewData["BillCategoryList"] = comboBL.GetBillCategory();
            //ViewData["RMCList"] = comboBL.GetRMCDropdown();
            //ViewData["ShipperTypeList"] = comboBL.GetShipperTypeDropdown();
            //ViewData["ShipperCategoryList"] = comboBL.GetShipperCategoryDropdown();
            //ViewData["ShippingLineList"] = comboBL.GetShippingLineDropdown("1");
            //ViewData["ModeList"] = comboBL.GetModeDropdown();
            //ViewData["RevenueBranchList"] = comboBL.GetCompanyBranchDropdown();
            //ViewData["AccountList"] = comboBL.GetAgentDropdown(CORA: "C");
            //ViewData["MoveCoordinatorList"] = comboBL.GetEmployeeDropdown();
            //ViewData["UnitList"] = comboBL.GetMeasurementUnitDropdown('A');
        }

		private Billing SetDefaultValues(Billing ObjInv)
		{
			//Billing bill = new Billing();
			if ((ObjInv.SLShortName == "DMMS" || ObjInv.SLShortName == "MSTG") && ObjInv.BillType == 'I')
			{
				ObjInv.RateCurrancyID = (int?)Convert.ToInt32(comboBL.GetRateCurrencyDropdown().Where(a => a.Value == "2").FirstOrDefault().Value);
				ObjInv.ConvRate = 1;
			}
			return ObjInv;
		}

		// GET: Billing/Edit/5
		public ActionResult Edit(int id)
		{
			return View();
		}

		// POST: Billing/Edit/5
		[HttpPost]
		public ActionResult Edit(int id, FormCollection collection)
		{
			try
			{
				// TODO: Add update logic here

				return RedirectToAction("Index");
			}
			catch
			{
				return View();
			}
		}

		// GET: Billing/Delete/5
		public ActionResult Delete(int id)
		{
			return View();
		}

		// POST: Billing/Delete/5
		[HttpPost]
		public ActionResult Delete(int id, FormCollection collection)
		{
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

		public JsonResult GetAddressDetials(int Client_AccountID, Int64 MoveID, string BillTo, char OrgorDest, int BillingEntityID,char BillType = 'I')
		{
			AddressList address = new AddressList();
			address = billingBL.GetAddressDetials(Client_AccountID, MoveID, BillTo, OrgorDest,BillingEntityID,BillType);
			return Json(new { result = address }, JsonRequestBehavior.AllowGet);
		}

		public JsonResult GetCityDropdownList(string City)
		{
			int CityID = string.IsNullOrEmpty(City) ? 0 : Convert.ToInt32(City);
			List<SelectListItem> CityList = comboBL.GetCityDropdown(SPTYPE: "SINGLE", ContinentID: -1, CountryID: -1, CityID: CityID).ToList();
			return Json(new { CityList = CityList }, JsonRequestBehavior.AllowGet);
		}

		public ActionResult BillFormat_Print(int? key, char? BillType, bool IsStatement = false, bool IsConsignment = false,int? StrgKey=null)
		{
			Billing objmodel = new Billing();
			int? BillID = 0; int? creditnoteID = 0;
			if (BillType == 'I')
			{
				BillID = key;
			}
			else
			{
				creditnoteID = key;
			}
			objmodel = billingBL.GetDetailById(0, BillID, creditnoteID, (char)BillType, 3, StrgInvID:StrgKey);

			objmodel.BillType = (char)BillType;
			if (IsStatement)
			{
				return PartialView("Statement_Print", objmodel);
			}
			if (IsConsignment)
			{
				return PartialView("ConsignmentNote_Print", objmodel);
			}
			else
			{
				return View(objmodel);
			}
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

		public ActionResult BTRBillFormat_Print(int? key, char? BillType, bool IsStatement = false, bool IsConsignment = false, int? StrgKey = null)
		{
			Billing objmodel = new Billing();
			int? BillID = 0; int? creditnoteID = 0;
			if (BillType == 'I')
			{
				BillID = key;
			}
			else
			{
				creditnoteID = key;
			}

			objmodel = billingBL.GetDetailById(0, BillID, creditnoteID, (char)BillType, 3, StrgInvID: StrgKey);

			objmodel.BillType = (char)BillType;
			if (IsStatement)
			{
				return PartialView("Statement_Print", objmodel);
			}
			if (IsConsignment)
			{
				return PartialView("ConsignmentNote_Print", objmodel);
			}
			else
			{
				return View(objmodel);
				//return View();
			}
			/*try
			{
				// TODO: Add delete logic here

				return RedirectToAction("Index");
			}
			catch
			{
				return View();
			}*/
		}
        public ActionResult ArabicBillFormat_Print(int? key, char? BillType, bool IsStatement = false, bool IsConsignment = false, int? StrgKey = null)
        {
            Billing objmodel = new Billing();
            int? BillID = 0; int? creditnoteID = 0;
            if (BillType == 'I')
            {
                BillID = key;
            }
            else
            {
                creditnoteID = key;
            }

            objmodel = billingBL.GetDetailById(0, BillID, creditnoteID, (char)BillType, 3, StrgInvID: StrgKey);

            objmodel.BillType = (char)BillType;
            if (IsStatement)
            {
                return PartialView("Statement_Print", objmodel);
            }
            if (IsConsignment)
            {
                return PartialView("ConsignmentNote_Print", objmodel);
            }
            else
            {
                return View(objmodel);
                //return View();
            }
            /*try
			{
				// TODO: Add delete logic here

				return RedirectToAction("Index");
			}
			catch
			{
				return View();
			}*/
        }

        public ActionResult CancelInvoice(int key)
		{
			try
			{
				string result = string.Empty;
				bool res = billingBL.CancelInvoice(key, out result);
				if (res)
				{
					this.AddToastMessage("RELOCBS", result, ToastType.Success);
				}
				else
				{
					this.AddToastMessage("RELOCBS", result, ToastType.Error);
				}
			}
			catch
			{
				this.AddToastMessage("RELOCBS", "Error in saving.", ToastType.Error);
			}


			return RedirectToAction("Index");

		}


		public JsonResult GenerateEInvoice(string InvNo)
		{
			string result = string.Empty;
			result = billingBL.GenerateEInvoice(InvNo);
			return Json(new { result = result }, JsonRequestBehavior.AllowGet);
		}


		public JsonResult DownloadToExcel(string InvNo)
		{
			string AppName = "NewCBS";

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
				string query = string.Format("EXEC {0} {1}", "[E_Invoice].[EInvoiceSchema]", param);
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

        public JsonResult GetEntityDropDownList (int RMCID)
        {

            //int CityID = string.IsNullOrEmpty(CityID) ? 0 : Convert.ToInt32(City);
            List<SelectListItem> EntityList =  comboBL.GetBillingEntityList(RMCID, BillType: 'I', BussLine: UserSession.GetUserSession().BussinessLine[0]).ToList();//comboBL.GetCityDropdown(SPTYPE: "SINGLE", ContinentID: -1, CountryID: -1, CityID: CityID).ToList();
            return Json(new { EntityList = EntityList }, JsonRequestBehavior.AllowGet);
        }

    }
}
