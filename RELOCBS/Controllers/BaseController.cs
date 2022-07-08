using RELOCBS.Entities;
using RELOCBS.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RELOCBS.Services.Implementation;
using RELOCBS.Utility;
using RELOCBS.Models;
using RELOCBS.CustomAttributes;
using RELOCBS.Extensions;

namespace RELOCBS.Controllers
{
    /// <summary>
    /// This will be base controller for all controllers.
    /// like HomeController:BaseController
    /// </summary>
    [CheckSessionTimeOut]
    public class BaseController : Controller
	{

		RELOCBS.Services.Implementation.Settings _settings;

		public RELOCBS.Services.Implementation.Settings settings
		{
			get
			{
				if (this._settings == null)
					this._settings = new RELOCBS.Services.Implementation.Settings();
				return this._settings;
			}
		}

		private CustomSessionStore _session;
		public CustomSessionStore session
		{
			get
			{
				if (this._session == null)
					this._session = new CustomSessionStore();
				return this._session;
			}
		}

		private LoggerService _logger;

		public ILoggerService Logger
		{
			get
			{
				if (this._logger == null)
					this._logger = new LoggerService();
				return this._logger;
			}
		}


		protected BaseController()
		{

			//ViewBag.UserMenu = UserSession.GetUserMenu();
			ViewBag.SiteTitle = settings.GetSettingByKey<string>("site_name");
			ViewBag.SiteTitleShort = settings.GetSettingByKey<string>("site_name_short");
			ViewBag.Copyright = settings.GetSettingByKey<string>("site_copyright");
			ViewBag.SiteNameSubtitle = settings.GetSettingByKey<string>("site_name_subtitle");
			ViewBag.LoginId = (UserSession.GetUserSession() == null) ? 0 : UserSession.GetUserSession().LoginID;
			// session.Set<string>("PageSession", );
		}



		//public ICommonServices Services
		//{
		//    get;
		//    set;
		//}

		/// <summary>
		/// Pushes an info message to the notification queue
		/// </summary>
		/// <param name="message">Message</param>
		/// <param name="durable">A value indicating whether the message should be persisted for the next request</param>
		protected virtual void NotifyInfo(string message, bool durable = true)
		{
			//Services.Notifier.Information(message, durable);
		}

		/// <summary>
		/// Pushes a warning message to the notification queue
		/// </summary>
		/// <param name="message">Message</param>
		/// <param name="durable">A value indicating whether the message should be persisted for the next request</param>
		protected virtual void NotifyWarning(string message, bool durable = true)
		{
			//Services.Notifier.Warning(message, durable);
		}

		/// <summary>
		/// Pushes a success message to the notification queue
		/// </summary>
		/// <param name="message">Message</param>
		/// <param name="durable">A value indicating whether the message should be persisted for the next request</param>
		protected virtual void NotifySuccess(string message, bool durable = true)
		{
			//Services.Notifier.Success(message, durable);
		}

		/// <summary>
		/// Pushes an error message to the notification queue
		/// </summary>
		/// <param name="message">Message</param>
		/// <param name="durable">A value indicating whether the message should be persisted for the next request</param>
		protected virtual void NotifyError(string message, bool durable = true)
		{
			//Services.Notifier.Error(message, durable);
		}

		/// <summary>
		/// Pushes an error message to the notification queue
		/// </summary>
		/// <param name="exception">Exception</param>
		/// <param name="durable">A value indicating whether a message should be persisted for the next request</param>
		/// <param name="logException">A value indicating whether the exception should be logged</param>
		protected virtual void NotifyError(Exception exception, bool durable = true, bool logException = true)
		{
			if (logException)
			{
				LogException(exception);
			}

			// Services.Notifier.Error(HttpUtility.HtmlEncode(exception.ToAllMessages()), durable);
		}

		/// <summary>
		/// Pushes an error message to the notification queue that the access to a resource has been denied
		/// </summary>
		/// <param name="durable">A value indicating whether a message should be persisted for the next request</param>
		/// <param name="log">A value indicating whether the message should be logged</param>
		protected virtual void NotifyAccessDenied(bool durable = true, bool log = true)
		{
			var message = "Admin.AccessDenied.Description";

			if (log)
			{
				Logger.Error(message, null, Convert.ToString(UserSession.GetUserSession().LoginID));
			}

			//Services.Notifier.Error(message, durable);
		}

		protected virtual ActionResult RedirectToReferrer()
		{
			if (Request.UrlReferrer != null && !string.IsNullOrWhiteSpace(Convert.ToString(Request.UrlReferrer)))
			{
				return Redirect(Request.UrlReferrer.ToString());
			}

			return RedirectToRoute("HomePage");
		}

		protected virtual ActionResult RedirectToHomePageWithError(string reason, bool durable = true)
		{
			string message = ""; //("Common.RequestProcessingFailed", this.RouteData.Values["controller"], this.RouteData.Values["action"], reason.NaIfEmpty());

			//Services.Notifier.Error(message, durable);

			return RedirectToRoute("HomePage");
		}


		/// <summary>
		/// On exception
		/// </summary>
		/// <param name="filterContext">Filter context</param>
		protected override void OnException(ExceptionContext filterContext)
		{
			if (filterContext.Exception != null)
			{
				string actionName = (string)filterContext.RouteData.Values["Action"];
				string ControllerName = (string)filterContext.RouteData.Values["Controller"];

				LogException(filterContext.Exception, ControllerName, actionName);

                if (filterContext.HttpContext.Response.StatusCode==440)
                {
                    if (filterContext.HttpContext.Request.IsAjaxRequest())
                    {
                        filterContext.ExceptionHandled = true;
                        filterContext.HttpContext.Response.StatusCode = 440;
                        filterContext.Result = new JsonResult { Data = "Session Timeout. Redirecting...", ContentType = "application/json", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                        filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
                    }
                    else
                    {
                        filterContext.Result = new RedirectToRouteResult(
                            new System.Web.Routing.RouteValueDictionary
                            {
                            { "Controller", "Account" },
                            { "Action", "Login" }
                            });
                    }
                    return;
                }
			}

			base.OnException(filterContext);
		}

		/// <summary>
		/// Log exception
		/// </summary>
		/// <param name="exc">Exception</param>
		private void LogException(Exception exc, string ControllerName = "", string ActionName = "")
		{
			int user = UserSession.GetUserSession() != null ? UserSession.GetUserSession().LoginID : 0;
			Logger.Error(exc.Message, exc, ActionName, ControllerName, Convert.ToString(user));
		}

		/// <summary>
		/// Log Activity
		/// </summary>
		/// <param name="PageUrl"></param>
		/// <param name="ActivityShortName"></param>
		/// <param name="Activity"></param>
		protected void LogActivity(string PageUrl = "", string ActivityShortName = "", string Activity = "")
		{
			int user = UserSession.GetUserSession().LoginID;
			string IPAddress = Request.ServerVariables["HTTP_X_FORWARDED_FOR"] ?? Request.ServerVariables["REMOTE_ADDR"];
			Logger.InsertLog(Convert.ToString(user), IPAddress, PageUrl, ActivityShortName, Activity);
		}

		protected override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			//if (this.Services.WorkContext.CurrentUser != null && this.Services.WorkContext.CurrentUser.UserDetails == null && !(filterContext.ActionDescriptor.ControllerDescriptor.ControllerName == "Account" && filterContext.ActionDescriptor.ActionName == "Profile"))
			//{
			//  filterContext.Result = RedirectToAction("Profile", "Account");
			//}
		}

		public JsonResult GetCityList(string term, string Value)
		{
			List<SelectListItem> CountryList = new List<SelectListItem>();
			if (string.IsNullOrEmpty(Value) && !string.IsNullOrWhiteSpace(term))
			{
				CountryList = (List<SelectListItem>)new BL.ComboBL().GetCityDropdown().Where(m => m.Text.ToLower().Contains(term.ToLower())).ToList();
			}
			else
			{
				CountryList = (List<SelectListItem>)new BL.ComboBL().GetCityDropdown(SPTYPE: "SINGLE", CityID: Convert.ToInt32(Value)).ToList();
			}
			return Json(new { CountryList = CountryList }, JsonRequestBehavior.AllowGet);
		}

		public JsonResult GetAgentList(string term, string Value,string AgentType)
		{
			List<SelectListItem> AgentList = new List<SelectListItem>();
			if (string.IsNullOrEmpty(Value))
			{
				AgentList = (List<SelectListItem>)new BL.ComboBL().GetAgentDropdown(CORA: AgentType, searchstring: term);//.Where(m => m.Text.ToLower().Contains(term.ToLower())).ToList();
			}
			else
			{
				AgentList = (List<SelectListItem>)new BL.ComboBL().GetAgentDropdown(SPTYPE: "SINGLE", CORA: AgentType, AgentId: Convert.ToInt32(Value)).ToList();
			}
			return Json(new { CountryList = AgentList }, JsonRequestBehavior.AllowGet);
		}


		public ActionResult GetSubCostDetails(int CostHeadID, int RateCompID, int SurveyID, int RateCompRateID, int RateCompRateBatchID, int ListLength, int IsEdit)
		{
			//objsub.Except(,;
			//ViewBag.CostHeadID = CostHeadID;
			string strdiv = string.Empty;
			strdiv = GetSubCostHeadList(CostHeadID, RateCompID, SurveyID, RateCompRateID, RateCompRateBatchID, ListLength, IsEdit);
			return Json(new { SubCostHeadList = strdiv ,IsEdit = IsEdit}, JsonRequestBehavior.AllowGet);
			//return (ActionResult)PartialView("_SubCostList", strdiv);
		}

		public JsonResult IsSubCostHead(int CostHeadID)
		{
			//List<Entities.SubCosthead> objsub = new List<Entities.SubCosthead>();
			bool IsSubCost = new BL.CommanBL().IsSubCostHead(CostHeadID);
			return Json(new { IsSubCost = IsSubCost }, JsonRequestBehavior.AllowGet);
			//return (ActionResult)PartialView("_SubCostList", objsub);
		}

		public JsonResult RemoveSubCostDetails(int CostHeadID,int RateCompID)
		{
			List<Entities.SubCosthead> objsub = new List<Entities.SubCosthead>();
			objsub = (List<Entities.SubCosthead>)TempData["SubCostList"];
			objsub = objsub.Where(x => x.CostHeadID != CostHeadID && x.RateCompID != RateCompID).ToList();
			TempData["SubCostList"] = objsub;
			return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
		}

		public string GetSubCostHeadList(int CostHeadID, int RateCompID, int SurveyID, int RateCompRateID, int RateCompRateBatchID, int ListLength, int IsEdit, int MoveID = 0)
		{
			string strdiv = string.Empty;
			try
			{
				List<Entities.SubCosthead> objsub = new List<Entities.SubCosthead>();
				List<Entities.SubCosthead> addsub = new List<Entities.SubCosthead>();
				//objsub = new BL.CommanBL().GetSubCostDetails(CostHeadID);
				if (ListLength > 0)
				{
					objsub = (List<Entities.SubCosthead>)TempData["SubCostList"];
				}
				else
				{
					objsub = null;
				}
				addsub = new BL.CommanBL().GetSubCostDetails(CostHeadID, SurveyID, RateCompRateID, RateCompRateBatchID, RateCompID,MoveID);
				if (objsub != null)
				{
					int count = objsub.Where(p2 => (addsub.All(p1 => p1.RateCompID == p2.RateCompID && p1.CostHeadID == p2.CostHeadID))).ToList().Count;
					//objsub.Where(l2 => !(addsub)
					//				.Any(l1 => l1.CostHeadID == l2.CostHeadID && l1.RateCompID == l2.RateCompID)).ToList();
					if (count <= 0)
					{
						objsub.AddRange(addsub);
					}
					else
					{
						addsub.Clear();
					}
				}
				else
				{
					objsub = addsub;
				}
				
				foreach (var item in addsub)
				{
					//string valueclass = IsEdit > 0 ? "onblur=\"CalculateAmount(this,'1');\"" : "onblur=\"CalculateAmount(this,'0');\"";
					string valueclass = "";
					string valuedisable = IsEdit > 0 ? "onblur=\"SetAmount(this);\"" : "readonly";
					string divclass = "row hide SubCost SubCost_" + item.RateCompID + item.CostHeadID;
					string currDropdown = new RELOCBS.BL.ComboBL().HtmlCurrencyList(item.RateCurrID);
					//string divSubcost = "<div class=\"col-sm-8\">" + item.CostHeadName + "</div><div class=\"col-sm-4\">" + item.Value + "</div>";
					strdiv += string.Format("<div class=\"{0}\" style=\"padding:5px;\">" +
						"<input type=\"hidden\" class=\"col-sm-4 input-sm hfPopRateCompID \" value=\"{2}\" /><input type=\"hidden\" class=\"col-sm-4 input-sm hfPopCostHeadID\" value=\"{3}\" /><input type=\"hidden\" class=\"col-sm-4 input-sm hfPopSubCostHeadID\" value=\"{5}\" />" +
						"<div class=\"col-sm-5\">" + item.CostHeadName + "</div><input type=\"text\" class=\"col-sm-2 input-sm txtrate\" {4} " + valuedisable + " value=\"{8}\"></input><select class=\"col-sm-1 input-sm ddlcurr m-r-sm\" " + valuedisable + " value=\"{6}\">" + currDropdown+"</select>" +
						"<input type=\"text\" class=\"col-sm-1 input-sm txtconvrate  m-r-sm\" {4} " + valuedisable + " value=\"{7}\"></input><input type=\"text\" class=\"col-sm-2 input-sm txtvalue\" {4} readonly value=\"{1}\"></input></div>",
						divclass, item.Value, item.RateCompID, item.CostHeadID, valueclass, item.SubCostID,item.RateCurrID,item.ConvRate,item.RateValue);
				}
				TempData["SubCostList"] = objsub;
			}
			catch (Exception ex)
			{
			}
			return strdiv;
		}

		public JsonResult GetConvRate(int FromCurrID,int ToCurrID, string FromPage = null)
		{
			//List<Entities.SubCosthead> objsub = new List<Entities.SubCosthead>();
			decimal ConvRate = new BL.CommanBL().GetConvRate(FromCurrID,ToCurrID, FromPage);
			return Json(new { ConvRate = ConvRate }, JsonRequestBehavior.AllowGet);
			//return (ActionResult)PartialView("_SubCostList", objsub);
		}
	}
}
