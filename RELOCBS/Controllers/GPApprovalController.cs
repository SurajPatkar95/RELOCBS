using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RELOCBS.AjaxHelper;
using RELOCBS.BL;
using RELOCBS.BL.MoveMange;
using RELOCBS.Entities;
using RELOCBS.Extensions;
using RELOCBS.Utility;

namespace RELOCBS.Controllers
{
	public class GPApprovalController : Controller
	{
		private MoveManageBL _moveManageBL;
		public MoveManageBL moveManageBL
		{
			get
			{
				if (this._moveManageBL == null)
					this._moveManageBL = new MoveManageBL();
				return this._moveManageBL;
			}
		}

		private CommanBL _commanBL;
		public CommanBL commanBL
		{
			get
			{
				if (this._commanBL == null)
					this._commanBL = new CommanBL();
				return this._commanBL;
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
		// GET: GPApproval
		//[Route("{EMPID}/Mvctest/{MoveID:int}")]  
		public ActionResult Index(int EMPID, int MoveID, bool IsApproval,bool RMCBuss,int key)//Type = {"1", "0" }
		{
			GPApproval obj = new GPApproval();
			obj.MoveID = MoveID;
			
			int LoginID = commanBL.GetLoginByEmployeeDropdown(EMPID);
			TempData["LoginID"] = LoginID;
			
			obj = moveManageBL.GetDetailByIdForGPApproval(0, (int?)obj.MoveID, 0, LoginID);
			obj.IsApporvalFlag = IsApproval;
			obj.GPPercent = obj.DefaultGPPercent;
			List<SelectListItem> GPApprovalList = comboBL.GetGPApprovalUserList(IsRMCBuss: RMCBuss, MoveId: Convert.ToString(MoveID), IsSendForApproval: false,LoginID:LoginID).ToList();
			ViewData["GPApprovalUserList"] = GPApprovalList;
			obj.dsCostSheet = moveManageBL.GetCostSheet(obj.MoveID, LoginID);
			obj.GPMasterID = key;
			return View(obj);
		}

		public ActionResult SaveApproval(GPApproval objData,string btnGPApproval)
		{
			int LoginID = Convert.ToInt32(TempData["LoginID"]);
			TempData.Keep("LoginID");
			bool res = false;
			string message = string.Empty;
			AjaxResponse result = new AjaxResponse();
			if (ModelState.IsValid)
			{
				objData.IsGPSendtoApproval = btnGPApproval == "Send for approval";
				objData.IsGPSendtoSD = btnGPApproval == "Send to SD";
				objData.IsGPApprove = btnGPApproval == "Approve";

				res = moveManageBL.InsertGPApproval(objData, LoginID, out message);
				if (!res)
				{
					result.Success = false;
					ModelState.AddModelError(string.Empty, "Unable to Send data.");
					result.Message = message;
					this.AddToastMessage("RELOCBS", message, ToastType.Error);
					//return Json(result);
				}
				else
				{
					result.Success = true;
					result.Message = message;
					this.AddToastMessage("RELOCBS", message, ToastType.Success);
				}
				return View("_SearchForm");
			}
			else
			{
				return Request.IsAjaxRequest() ? (ActionResult)PartialView("Index", objData)
				  : View("Index", objData);
			}

			//return View(objData);
		}
	}
}