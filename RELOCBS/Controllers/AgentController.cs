using PagedList;
using RELOCBS.AjaxHelper;
using RELOCBS.App_Code;
using RELOCBS.BL;
using RELOCBS.BL.Common;
using RELOCBS.Common;
using RELOCBS.CustomAttributes;
using RELOCBS.Entities;
using RELOCBS.Extensions;
using RELOCBS.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace RELOCBS.Controllers
{
    [AuthorizeUser]
    public class AgentController : BaseController
    {
        private string _PageID = "14";

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

        private AgentBL _agentBL;
        public AgentBL agentBL
        {
            get
            {
                if (this._agentBL == null)
                    this._agentBL = new AgentBL();
                return this._agentBL;

            }
        }

        // GET: Agent
        public ActionResult Index(int? page,string Type)
        {
            if (Type=="C")
            {
                _PageID = "36";
            }

            if (!CSubs.CheckPageRights(_PageID, PermissionType.VIEW))
            {
                return new HttpStatusCodeResult(403);
            }
            string Title = Type == "A" ? "Agent" : "Corporate";
            session.Set<string>("PageSession", Title);
            //ViewData["CityList"] = comboBL.GetCityDropdown();
            var pageIndex = (page ?? 1);
            int pageSize = settings.GetSettingByKey<int>("pagination_pagesize");
            int totalCount = 10;
            string OrderBy = "";
            int Order = 0;
            int? CityID = null;
            string SearchKey = string.Empty;
            if (Request.Form["SearchKey"] != null && Request.Form["SearchKey"].Trim() != "")
            {
                SearchKey = Request.Form["SearchKey"];
            }
            if (Request.Params["grid-column"] != null && Request.Params["grid-column"].Trim() != "")
            {
                OrderBy = Request.Params["grid-column"].Trim().ToString();
            }
            if (Request.Params["grid-dir"] != null && Request.Params["grid-column"].Trim() != "")
            {
                Order = Convert.ToInt32(Request.Params["grid-dir"].Trim().ToString());
            }
            if (Request.Form["SearchCityID"] != null && Request.Form["SearchCityID"].Trim() != "")
            {
                CityID = Convert.ToInt32(Request.Form["SearchCityID"]);
            }
            var items = agentBL.GetAgentList(pageIndex, pageSize, OrderBy, Order, null, CityID, Type, SearchKey, UserSession.GetUserSession().LoginID, out totalCount);
            if (totalCount == 0 && pageIndex > 1)
            {
                pageIndex = 1;
                items = agentBL.GetAgentList(pageIndex, pageSize, OrderBy, Order, null, CityID, Type, SearchKey, UserSession.GetUserSession().LoginID, out totalCount);
            }

            var itemsAsIPagedList = new StaticPagedList<Agent>(items, pageIndex, pageSize, totalCount);
            ViewData["Type"] = Type;
            return Request.IsAjaxRequest()
                ? (ActionResult)PartialView("_AjaxPartial", itemsAsIPagedList)
                : View(itemsAsIPagedList);
        }

        // GET: Agent/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Agent/Create
        public ActionResult Create(string Type)
        {
            AgentViewModel model = new AgentViewModel();
            model.Isactive = true;
            model.AgentOrCorp = Type;
            ViewData["Type"] = model.AgentOrCorp;
            FillCombo(CorA: model.AgentOrCorp);
            return Request.IsAjaxRequest()
                ? (ActionResult)PartialView("Create", model)
                : View(model);
        }

        // POST: Agent/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AgentViewModel data)
        {
            try
            {
                FillCombo(CorA: data.AgentOrCorp);
                RELOCBS.AjaxHelper.AjaxResponse result = new AjaxResponse();
                ViewData["Type"] = data.AgentOrCorp;
                if (ModelState.IsValid)
                {
                    string Message;
                    result.Success = agentBL.Insert(data, out Message);
                    if (result.Success)
                    {
                        result.Message = Message;
                        result.Result = this.RenderPartialViewToString("Create", data);
                        return Json(result);
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, Message);
                    }
                }
                return Request.IsAjaxRequest()
                              ? (ActionResult)PartialView("Create", data)
                              : View(data);


            }
            catch
            {
                return View();
            }
        }

        // GET: Agent/Edit/5
        public ActionResult Edit(int id)
        {
            if (id <= 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AgentViewModel data = agentBL.GetDetailById(id);
            FillCombo(CorA: data.AgentOrCorp);
            ViewData["Type"] = data.AgentOrCorp;
            if (data == null)
            {
                return HttpNotFound();
            }
            return Request.IsAjaxRequest()
                ? (ActionResult)PartialView("Edit", data)
                : View(data);
            
        }

        // POST: Agent/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(AgentViewModel data)
        {
            try
            {
                FillCombo(CorA: data.AgentOrCorp);
                ViewData["Type"] = data.AgentOrCorp;
                AjaxResponse result = new AjaxResponse();
                string message = string.Empty;
                if (ModelState.IsValid)
                {
                    result.Success = agentBL.Update(data, out message);

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
                return Request.IsAjaxRequest()
                  ? (ActionResult)PartialView("Edit", data)
                  : View(data);
            }
            catch
            {
                return View();
            }
        }

		public ActionResult BankDetails(int id)
		{
			if (id <= 0)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			DynamicBankDetails data = new DynamicBankDetails();//agentBL.GetDetailById(id);
			BankList bankdtl = new BankList() { Header = "suraj", Value = "suraj" };
			data.bankList.Add(bankdtl);
			//FillCombo(CorA: data.AgentOrCorp);
			//ViewData["Type"] = data.AgentOrCorp;
			//if (data == null)
			//{
			//	return HttpNotFound();
			//}
			return Request.IsAjaxRequest()
				? (ActionResult)PartialView("BankDetails", data)
				: View(data);

		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult BankDetails(DynamicBankDetails data)
		{
			try
			{
				//FillCombo(CorA: data.AgentOrCorp);
				//ViewData["Type"] = data.AgentOrCorp;
				AjaxResponse result = new AjaxResponse();
				string message = string.Empty;
				if (ModelState.IsValid)
				{
					//result.Success = agentBL.Update(data, out message);

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
				return Request.IsAjaxRequest()
				  ? (ActionResult)PartialView("BankDetail", data)
				  : View(data);
			}
			catch
			{
				return View();
			}
		}


		// GET: Agent/Delete/5
		public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Agent/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
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

        private void FillCombo(string CorA)
        {
            ViewData["City"] = new List<SelectListItem>(); //comboBL.GetCityDropdown(ContinentID: -1, CountryID: -1);
            ViewData["Country"] = comboBL.GETCountryDropdown(ContinentID: -1);
            ViewData["BusinessLine"] = comboBL.GetBusinessLineDropdown();
            ViewData["Company"] = comboBL.GetCompanyDropdown();
            ViewData["AgentGroup"] = comboBL.GetAgentGroupDropdown(CorA: CorA);
			ViewData["BankDet"] = comboBL.GetBankDetailDropdown();
		}

        public JsonResult GetAutoPopulateList()
        {
            var lst = comboBL.GetAgentDropdown().Select(i => new { i.Value, i.Text }).ToList();
            return Json(lst, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ExportToExcel()
        {
            Dictionary<string, string> param = new Dictionary<string, string>();
            try
            {

                string SearchKey = string.Empty;
                if (Request.Form["SearchKey"] != null && Request.Form["SearchKey"].Trim() != "")
                {
                    param.Add("@SP_SearchString", Request.Form["SearchKey"]);
                }

                if (Request.Form["CityID"] != null && Request.Form["CityID"].Trim() != "")
                {
                    param.Add("@SP_CityID", Request.Form["CityID"]);
                }

                param.Add("@SP_LoginID", Convert.ToString(UserSession.GetUserSession().LoginID));

                CommonService.GenerateExcel(this.Response, "Agent", "[Comm].[GETAgentForGrid_ExpToExl]", param);
            }
            catch (Exception ex)
            {
                this.AddToastMessage("RELOCBS", "UnExpected Error occured", ToastType.Error);
            }
            return View();
        }


        public ActionResult FinanceDetails(int id)
        {
            if (id <= 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AgentViewModel data = agentBL.GetDetailById(id);
            //FillCombo(CorA: data.AgentOrCorp);
            //ViewData["Type"] = data.AgentOrCorp;
            if (data == null)
            {
                return HttpNotFound();
            }
            return Request.IsAjaxRequest()
                ? (ActionResult)PartialView("FinanceDetails", data)
                : View(data);

        }

        [HttpPost]
        public ActionResult FinanceDetails(AgentViewModel data)
        {
            try
            {
                //FillCombo(CorA: data.AgentOrCorp);
                //ViewData["Type"] = data.AgentOrCorp;
                AjaxResponse result = new AjaxResponse();
                //string message = string.Empty;
                if (ModelState.IsValid)
                {
                    //result.Success = agentBL.Update(data, out message);

                    //if (result.Success)
                    //{
                    //    result.Result = this.RenderPartialViewToString("Create", data);
                    //    return Json(result);
                    //}
                    //else
                    //{
                    //    ModelState.AddModelError(string.Empty, message);
                    //}
                }
                return Request.IsAjaxRequest()
                  ? (ActionResult)PartialView("Edit", data)
                  : View(data);
            }
            catch
            {
                return View();
            }
        }
    }
}
