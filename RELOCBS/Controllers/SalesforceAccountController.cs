using PagedList;
using RELOCBS.AjaxHelper;
using RELOCBS.App_Code;
using RELOCBS.BL;
using RELOCBS.BL.SalesforceAccount;
using RELOCBS.Common;
using RELOCBS.CustomAttributes;
using RELOCBS.Entities;
using RELOCBS.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace RELOCBS.Controllers
{
    [AuthorizeUser]
    public class SalesforceAccountController : BaseController
    {
        private string _PageID = "66";

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

        private SFAccountBL _accountBL;
        public SFAccountBL accountBL
        {
            get
            {
                if (this._accountBL == null)
                    this._accountBL = new SFAccountBL();
                return this._accountBL;

            }
        }

        // GET: SalesforceAccount
        public ActionResult Index(int page=1)
        {
            if (!CSubs.CheckPageRights(_PageID, PermissionType.VIEW))
            {
                return new HttpStatusCodeResult(403);
            }
            
            session.Set<string>("PageSession", "SF Account To Agent/Corporate");
            int pageSize = settings.GetSettingByKey<int>("pagination_pagesize");
            int totalCount = 10;
            string sort = "";
            string sortdir = "";
            string OrderBy = "";
            int Order = 0;
            DateTime? Fromdate =System.DateTime.Now.Date.AddMonths(-1);
            DateTime? Todate = System.DateTime.Now;
            string search= string.Empty, searchType = string.Empty;
            if (Request.Form["FromDate"] != null && Request.Form["FromDate"].Trim() != "")
            {
                Fromdate = Convert.ToDateTime(Request.Form["FromDate"]);
            }

            if (Request.Form["ToDate"] != null && Request.Form["ToDate"].Trim() != "")
            {
                Todate = Convert.ToDateTime(Request.Form["ToDate"]);
            }

            if (Request.Form["search"] != null && Request.Form["search"].Trim() != "")
            {
                search = Convert.ToString(Request.Form["search"]);
            }

            if (Request.Form["SearchType"] != null && Request.Form["SearchType"].Trim() != "")
            {
                searchType = Convert.ToString(Request.Form["SearchType"]);
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
            int totalRecord = 0;
            if (page < 1) page = 1;
            //int skip = (pageNo * pageSize) - pageSize;
            int skip = pageSize;
            pageSize = page;
            var items = accountBL.GetGrid(Fromdate, Todate,searchType,search, sort, sortdir, skip, pageSize, out totalRecord);
            var itemsAsIPagedList = new StaticPagedList<Entities.SFAccount>(items, pageSize, skip, totalRecord);
            return Request.IsAjaxRequest() ? (ActionResult)PartialView("_AjaxPartial", itemsAsIPagedList) : View(itemsAsIPagedList);
        }

        // GET: SalesforceAccount/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        private void FillCombo(string CorA,int CityID)
        {
            ViewData["City"] = comboBL.GetCityDropdown(ContinentID: -1, CountryID: -1,SPTYPE:"SINGLE",CityID: CityID);//new List<SelectListItem>(); //
            ViewData["Country"] = comboBL.GETCountryDropdown(ContinentID: -1);
            ViewData["BusinessLine"] = comboBL.GetBusinessLineDropdown();
            ViewData["Company"] = comboBL.GetCompanyDropdown();
            ViewData["AgentGroup"] = comboBL.GetAgentGroupDropdown(CorA: CorA);
        }

        // GET: SalesforceAccount/Create
        public ActionResult Create(string id,int TempID)
        {
            if (string.IsNullOrWhiteSpace(id) || TempID<=0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SFAccount data = accountBL.GetDetail(id, TempID);
            int CityID = data.CityId != null ? Convert.ToInt32(data.CityId): 0;
            FillCombo(CorA:data.AgentOrCorp,CityID:CityID);
            ViewData["Type"] = data.AgentOrCorp;
            if (data == null)
            {
                return HttpNotFound();
            }
            return Request.IsAjaxRequest()
                ? (ActionResult)PartialView("Create", data)
                : View(data);
        }

        // POST: SalesforceAccount/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SFAccount data)
        {
            try
            {
                int CityID = data.CityId != null ? Convert.ToInt32(data.CityId) : 0;
                FillCombo(CorA: data.AgentOrCorp,CityID: CityID);
                ViewData["Type"] = data.AgentOrCorp;
                AjaxResponse result = new AjaxResponse();
                string message = string.Empty;
                if (ModelState.IsValid)
                {
                    result.Success = accountBL.Insert(data, out message);

                    if (result.Success)
                    {
                        result.Result = this.RenderPartialViewToString("Create", data);
                        return Json(result);
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, message);
                    }
                }
                return Request.IsAjaxRequest()? (ActionResult)PartialView("Create", data): View(data);
            }
            catch
            {
                return View(data);
            }
        }
        
    }
}
