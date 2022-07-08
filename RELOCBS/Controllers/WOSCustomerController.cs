using PagedList;
using RELOCBS.App_Code;
using RELOCBS.BL;
using RELOCBS.BL.WOSCustomer;
using RELOCBS.Common;
using RELOCBS.CustomAttributes;
using RELOCBS.Entities;
using RELOCBS.Extensions;
using RELOCBS.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace RELOCBS.Controllers
{
    [AuthorizeUser]
    public class WOSCustomerController : BaseController
    {
        private string _PageID = "78";

        private CommonSubs _CSubs;
        public CommonSubs CSubs
        {
            get
            {
                if (_CSubs == null)
                    _CSubs = new CommonSubs();
                return _CSubs;
            }
        }

        private ComboBL _comboBL;
        public ComboBL ComboBL
        {
            get
            {
                if (_comboBL == null)
                    _comboBL = new ComboBL();
                return _comboBL;
            }
        }

        private CommanBL _comBL;
        public CommanBL ComBL
        {
            get
            {
                if (_comBL == null)
                    _comBL = new CommanBL();
                return _comBL;
            }
        }

        private WOSCustomerBL _WOSCustomerBL;
        public WOSCustomerBL WOSCustomerBL
        {
            get
            {
                if (_WOSCustomerBL == null)
                    _WOSCustomerBL = new WOSCustomerBL();
                return _WOSCustomerBL;
            }
        }

        WOSCustomer _WOSCustomer = new WOSCustomer();
        public WOSCustomerController(WOSCustomer WOSCustomer)
        {
            _WOSCustomer = WOSCustomer;
        }

        public ActionResult Index(int Page = 1)
        {
            try
            {
                if (!CSubs.CheckPageRights(_PageID, PermissionType.VIEW))
                {
                    return new HttpStatusCodeResult(403);
                }

                session.Set("PageSession", "WOS Customer Service Map");

                int? CustServMapMasterID = null;
                string CustomerName = string.Empty;
                string Sort = "CustServMapMasterID";
                string SortDir = "asc";
                int PageSize = settings.GetSettingByKey<int>("pagination_pagesize");
                int Order = 0;

                bool? IsRMC = null; int? RMCID = null; int? ClientID = null; int? AccountID = null; DateTime? EffectiveFrom = null;

                if (Request.Form["CustServMapMasterID"] != null && Request.Form["CustServMapMasterID"].Trim() != "")
                    CustServMapMasterID = Convert.ToInt32(Request.Form["CustServMapMasterID"]);

                if (Request.Form["CustomerName"] != null && Request.Form["CustomerName"].Trim() != "")
                    CustomerName = Convert.ToString(Request.Form["CustomerName"]);

                if (Request.Params["grid-column"] != null && Request.Params["grid-column"].Trim() != "")
                    Sort = Request.Params["grid-column"].Trim().ToString();

                if (Request.Params["grid-dir"] != null && Request.Params["grid-dir"].Trim() != "")
                {
                    Order = Convert.ToInt32(Request.Params["grid-dir"].Trim().ToString());
                    if (Order == 1)
                        SortDir = "asc";
                    else
                        SortDir = "desc";
                }

                int TotalRecord = 0;
                if (Page < 1) Page = 1;
                int Skip = (Page * PageSize) - PageSize;

                var CustomerServiceMapList = WOSCustomerBL.GetCustomerServiceMapping(Sort, SortDir, Skip, PageSize, CustomerName, IsRMC, RMCID, ClientID, AccountID, EffectiveFrom, out TotalRecord);

                var ItemsAsIPagedList = new StaticPagedList<WOSCustomer>(CustomerServiceMapList, Page, PageSize, TotalRecord);

                return Request.IsAjaxRequest() ? (ActionResult)PartialView("_GridPartial", ItemsAsIPagedList) : View(ItemsAsIPagedList);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpGet]
        public ActionResult Create(string Key)
        {
            try
            {
                session.Set("PageSession", "WOS Customer Service Map");

                var list = CommonService.GetQueryString(Key);
                Int64 CustServMapMasterID = 0;
                if (list.ContainsKey("CustServMapMasterID"))
                    CustServMapMasterID = Convert.ToInt32(list["CustServMapMasterID"]);

                GetDropDownLists();

                _WOSCustomer = WOSCustomerBL.GetClientServiceMapingDetailsById(CustServMapMasterID);

                return View(_WOSCustomer);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult Create(WOSCustomer WOSCustomerObj)
        {
            try
            {
                string message = string.Empty;
                GetDropDownLists();

                if (ModelState.IsValid)
                {
                    if (WOSCustomerObj.WOSSubServiceList.Count(r => r.IsChecked) > 0)
                    {
                        bool result = WOSCustomerBL.SaveCustomerServiceMap(WOSCustomerObj, out message);
                        if (result)
                        {
                            this.AddToastMessage("RELOCBS", message, ToastType.Success);
                            return RedirectToAction("Create", new { Key = CommonService.GetCrypt("CustServMapMasterID=" + WOSCustomerObj.CustServMapMasterID.ToString(), 1) });
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, message);
                            this.AddToastMessage("RELOCBS", message, ToastType.Error);
                        }
                    }
                    else
                    {
                        message = "Please select at least one serive.";
                        ModelState.AddModelError(string.Empty, message);
                        this.AddToastMessage("RELOCBS", message, ToastType.Error);
                    }
                }
                else
                {
                    message = "Error occured.";
                    this.AddToastMessage("RELOCBS", message, ToastType.Error);
                }
                return Request.IsAjaxRequest() ? (ActionResult)PartialView("Create", WOSCustomerObj) : View(WOSCustomerObj);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void GetDropDownLists()
        {
            try
            {
                List<SelectListItem> BusinessList = new List<SelectListItem>
                {
                    new SelectListItem() { Value = "false", Text = "NON RMC-BUSINESS" },
                    new SelectListItem() { Value = "true", Text = "RMC-BUSINESS" }
                };
                ViewData["BusinessList"] = BusinessList;
                ViewData["RMCList"] = ComboBL.GetRMCDropdown();
                ViewData["CountryList"] = ComboBL.GETCountryDropdown();
                ViewData["CurrencyList"] = ComboBL.GetCurrencyDropdown();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}