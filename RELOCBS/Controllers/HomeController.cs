using RELOCBS.CustomAttributes;
using RELOCBS.Entities;
using RELOCBS.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace RELOCBS.Controllers
{
    [OutputCache(Location = OutputCacheLocation.None)]
    [CheckSessionTimeOut]
    public class HomeController : BaseController
    {
        //
        // GET: /Home/
        [AuthorizeUser]
        public ActionResult Index()
        {
            session.Set<string>("PageSession", "");
            
            return RedirectToActionPermanent("Index", "Dashboard",new { UserID = UserSession.GetUserSession().EmpID });
            //return View();
        }

        public ActionResult PanelLead(DashboardFilter model = null)
        {
            
            if (model == null)
            {
                model = new DashboardFilter();
            }
            //model.LoggedinUserID = _commonServices.WorkContext.CurrentUser.UserID;
            model.ForPanel = "lead";

            try
            {
                //ViewData["DashLead"] = _spService.GetDashboardData(model).Tables[0];
            }
            catch (Exception ex)
            {
                ViewData["DashLead"] = null;
            }
            return Request.IsAjaxRequest() ? (ActionResult)PartialView("_PanelLead", model) : View("_PanelLead", model);
        }

        public ActionResult PanelPricing(DashboardFilter model = null)
        {
            
            if (model == null)
            {
                model = new DashboardFilter();
            }

            //model.LoggedinUserID = _commonServices.WorkContext.CurrentUser.UserID;
            model.ForPanel = "pricing";

            try
            {
                //ViewData["Pricing"] = _spService.GetDashboardData(model).Tables[0];
            }
            catch (Exception ex)
            {
                ViewData["Pricing"] = null;
            }

            return Request.IsAjaxRequest() ? (ActionResult)PartialView("_PanelPricing", model) : View("_PanelPricing", model);

        }

        public ActionResult PanelSurvey(DashboardFilter model = null)
        {
            
            if (model == null)
            {
                model = new DashboardFilter();
            }

            //model.LoggedinUserID = _commonServices.WorkContext.CurrentUser.UserID;
            model.ForPanel = "Survey";

            try
            {
                //ViewData["SurveyData"] = _spService.GetDashboardData(model).Tables[0];
            }
            catch (Exception ex)
            {
                ViewData["SurveyData"] = null;
            }

            return Request.IsAjaxRequest() ? (ActionResult)PartialView("_PanelSurvey", model) : View("_PanelSurvey", model);

        }

		public ActionResult PanelFollowUp(DashboardFilter model = null)
		{
			if (model == null)
			{
				model = new DashboardFilter();
			}

			//model.LoggedinUserID = _commonServices.WorkContext.CurrentUser.UserID;
			model.ForPanel = "FollowUp";

			try
			{
				//ViewData["SurveyData"] = _spService.GetDashboardData(model).Tables[0];
			}
			catch (Exception ex)
			{
				ViewData["FollowUpData"] = null;
			}

			return Request.IsAjaxRequest() ? (ActionResult)PartialView("_PanelFollowUp", model) : View("_PanelFollowUp", model);

		}

		public ActionResult PanelMoveManagement(DashboardFilter model = null)
        {

            if (model == null)
            {
                model = new DashboardFilter();
            }

            //model.LoggedinUserID = _commonServices.WorkContext.CurrentUser.UserID;
            model.ForPanel = "MoveManagement";

            try
            {
                //ViewData["MoveManagementData"] = _spService.GetDashboardData(model).Tables[0];
                //ViewData["ShipmentMode"] = _spService.BindDropdown("ShipmentMode", "", "");
            }
            catch (Exception ex)
            {
                ViewData["MoveManagementData"] = null;
            }

            return Request.IsAjaxRequest() ? (ActionResult)PartialView("_PanelMoveManagement", model) : View("_PanelMoveManagement", model);

        }

        public ActionResult PanelMoveManagementRevenueLoad(DashboardFilter model = null)
        {

            if (model == null)
            {
                model = new DashboardFilter();
            }

            //model.LoggedinUserID = _commonServices.WorkContext.CurrentUser.UserID;
            model.ForPanel = "MoveManagementRevenueLoad";

            try
            {
                //ViewData["MoveManagementRevenueLoadData"] = _spService.GetDashboardData(model).Tables[0];
            }
            catch (Exception ex)
            {
                //ViewData["MoveManagementRevenueLoadData"] = null;
            }
            return Request.IsAjaxRequest() ? (ActionResult)PartialView("_PanelMoveManagementRevenueLoad", model) : View("_PanelMoveManagementRevenueLoad", model);
        }

        public ActionResult PanelMoveManagementProfitability(DashboardFilter model = null)
        {
            

            if (model == null)
            {
                model = new DashboardFilter();
            }
            //model.LoggedinUserID = _commonServices.WorkContext.CurrentUser.UserID;
            model.ForPanel = "MoveManagementProfitability";
            try
            {
                var Profitability = new DataSet(); //_spService.GetDashboardData(model);
                if (Profitability.Tables.Count == 0)
                {
                    DataTable dt = new DataTable();
                    dt.Columns.Add("RMC");
                    dt.Columns.Add("Moves");
                    dt.Columns.Add("Revenue");
                    dt.Columns.Add("Cost");
                    dt.Columns.Add("Profit_Loss");
                    dt.Columns.Add("Profit_LossPer");
                    ViewData["MoveManagementProfitabilityData"] = dt;
                }
                else
                {
                    ViewData["MoveManagementProfitabilityData"] = Profitability.Tables[0];
                }

            }
            catch (Exception ex)
            {
                ViewData["MoveManagementProfitabilityData"] = null;
            }

            return Request.IsAjaxRequest() ? (ActionResult)PartialView("_PanelMoveManagementProfitability", model) : View("_PanelMoveManagementProfitability", model);
        }

        public ActionResult PanelBilling(DashboardFilter model = null)
        {
            if (model == null)
            {
                model = new DashboardFilter();
            }
            //model.LoggedinUserID = _commonServices.WorkContext.CurrentUser.UserID;
            model.ForPanel = "PanelBilling";
            try
            {
                var BillingData = new DataSet();//_spService.GetDashboardData(model);
                if (BillingData.Tables.Count == 0)
                {
                    DataTable dt = new DataTable();
                    dt.Columns.Add("RMC");
                    dt.Columns.Add("Draft");
                    dt.Columns.Add("Audit");
                    dt.Columns.Add("Approved");
                    dt.Columns.Add("Total_Billing");
                    ViewData["PanelBillingData"] = dt;
                }
                else
                {
                    ViewData["PanelBillingData"] = BillingData.Tables[0];
                }
            }
            catch (Exception ex)
            {
                ViewData["PanelBillingData"] = null;
            }

            return Request.IsAjaxRequest() ? (ActionResult)PartialView("_PanelBilling", model) : View("_PanelBilling", model);
        }

    }
}
