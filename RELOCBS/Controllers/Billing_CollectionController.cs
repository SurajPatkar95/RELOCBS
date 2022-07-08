using RELOCBS.BL;
using RELOCBS.BL.Billing_Collection;
using RELOCBS.BL.Enquiry;
using RELOCBS.Common;
using RELOCBS.CustomAttributes;
using RELOCBS.Entities;
using RELOCBS.Extensions;
using RELOCBS.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RELOCBS.Controllers
{
	[AuthorizeUser]
	public class Billing_CollectionController : BaseController
    {
        private string _PageID = "7";
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
        Billing_Collection _bnc = new Billing_Collection();
        ComboBL combo = new ComboBL();
        public Billing_Collection bnc
        {
            get
            {
                if (this._bnc == null)
                    this._bnc = new Billing_Collection();
                return this._bnc;
            }
        }

        private ClientDetailBL _clientDetailBL;
        public ClientDetailBL ClientDetailBL
        {
            get
            {
                if (this._clientDetailBL == null)
                    this._clientDetailBL = new ClientDetailBL();
                return this._clientDetailBL;
            }
        }
        private BillingCollectionBL _bncBL;
        public BillingCollectionBL bncBL
        {
            get
            {
                if (this._bncBL == null)
                    this._bncBL = new BillingCollectionBL();
                return this._bncBL;
            }
        }

        // GET: Billing_Collection
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {

            return View(new Billing_Collection());
        }
        [HttpGet]
        public ActionResult Create(int EnquirydetId)
        {
            
            session.Set<string>("PageSession", "Billing & Collection");
            List<SelectListItem> enquiryList = new List<SelectListItem>();
            _bnc = bncBL.GetDetailById(EnquirydetId, out enquiryList);
            ViewData["enquiryList"] = enquiryList;
            if (_bnc.chgAccountMgr <= 0 || _bnc.chgAccountMgr == null)
            {
                _bnc.AccountMgr = GetClientDetails(_bnc.AccountId, 'A').AccountMgr;
            }
			_bnc.AccountGSTNO= GetClientDetails(_bnc.AccountId, 'A').AccountGSTNO;
			_bnc.ClientGSTNO= GetClientDetails(_bnc.ClientId, 'c').ClientGSTNO;
			GetDropDownList(_bnc.ClientType);
			//_bnc.EnqDetailID = EnquirydetId;
			return View(_bnc);
        }
        [HttpPost]
        public ActionResult Create(Billing_Collection bnc, string SurveyId)
        {
            GetDropDownList(bnc.ClientType);
            return View(_bnc);
        }

        [HttpGet]
        public ActionResult Save(Billing_Collection bnc, int i = 0)
        {
            GetDropDownList(bnc.ClientType);
            return View("Create", bnc);
        }

        [HttpPost]
        public ActionResult Save(Billing_Collection bnc)
        {

            GetDropDownList(bnc.ClientType);
            string message = string.Empty;
            List<SelectListItem> enquiryList = new List<SelectListItem>();
            ViewData["enquiryList"] = enquiryList;
			if (bnc.Advance && (bnc.Amount <=0 || bnc.Amount == null))
			{
				ModelState.AddModelError("Amount", "Advance Amount is mandatory.");
			} 
			if (!bnc.Advance && string.IsNullOrEmpty(bnc.PaymentPostDelivery) && string.IsNullOrEmpty(bnc.PaymentPreDelivery))
			{
				ModelState.AddModelError("PaymentPostDelivery", "Select atleast one payment term.");
			}
            if ((!string.IsNullOrEmpty(bnc.PaymentPostDelivery) && (bnc.NoDays == null ? 0 : bnc.NoDays) <= 0) /*|| string.IsNullOrEmpty(bnc.PaymentPreDelivery)*/)
            {
                ModelState.AddModelError("NoDays", "No of days is mandatory.");
            }
            if (ModelState.IsValid)
            {
				//if (bnc.BillingOn == "Client")
				//{
				//	bnc.BillingOnClientId = bnc.ClientId;
				//}
				//else if(bnc.BillingOn == "Corporate")
				//{
				//	bnc.BillingOnClientId = bnc.AccountId;
				//}
                bool res = false;
                res = bncBL.Insert(bnc, out message);
                if (!res)
                {
                    ModelState.AddModelError(string.Empty, "Unable to save Enquiry data.");
                    this.AddToastMessage("RELOCBS", message, ToastType.Error);
                }
                else
                {
                    this.AddToastMessage("RELOCBS", message, ToastType.Success);
                }
                return RedirectToAction("Create", new { EnquirydetId = bnc.EnqDetailID });
            }
            else
            {
                return View("Create", bnc);
            }
        }


        private void GetDropDownList(string ClientType)
        {
			bool RMCBuss = !(UserSession.GetUserSession().BussinessLine == "NON RMC-BUSINESS");
			ViewData["AgentList"] = combo.GetAgentDropdown(CORA:"A");
			
			ViewData["CientList"] = combo.GetAgentDropdown(CORA: RMCBuss ? "R" : ClientType); 
			//ViewData["CientList"] = combo.GetAgentDropdown(CORA: ClientType);
			ViewData["AccountList"] = combo.GetAgentDropdown(CORA: "C");
			ViewData["ShipperTypeList"] = combo.GetShipperCategoryDropdown();
            ViewData["OriCityList"] = combo.OriginCityDropdown();
            ViewData["AcctMgrList"] = combo.GetEmployeeDropdown();
        }

        public JsonResult GetaJAXClientDetails(int ClientId, char Mode)
        {
            return Json(new { AccountMgr = GetClientDetails(ClientId, Mode) }, JsonRequestBehavior.AllowGet);
        }

        private ClientDetails GetClientDetails(int ClientId, char Mode)
        {
			bool RMCBuss = !(UserSession.GetUserSession().BussinessLine == "NON RMC-BUSINESS");
			return ClientDetailBL.GetClientDetail(ClientId,Mode);
			//return "";
        }

        public ActionResult GetReport(int EnqDetailID)
        {
            //EncryptedQueryString query = new EncryptedQueryString();
            //query["PageID"] = _PageID;
            //query["ReportID"] = "10007";
            //query["EnqDetailID"] = Convert.ToString(EnqDetailID).Trim();

            //return this.RedirectSameDomain("/Reports/ReportViewer.aspx?args=" + query.ToString());
            List<SelectListItem> listItems = new List<SelectListItem>();
            Billing_Collection model = bncBL.GetDetailById(EnqDetailID,out listItems);
            return View("_BNC_Print", model);

            //return Redirect("/Reports/ReportViewer.aspx");
        }
    }
}