using RELOCBS.App_Code;
using RELOCBS.CustomAttributes;
using RELOCBS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RELOCBS.BL.UserFeedback;
using RELOCBS.Extensions;
using RELOCBS.Utility;

namespace RELOCBS.Controllers
{
    [AuthorizeUser]
    public class UserFeedbackController : BaseController
    {

        private string _PageID = "";
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
        

        private UserFeedbackBL _feedbackBL;
        public UserFeedbackBL feedbackBL
        {
            get
            {
                if (this._feedbackBL == null)
                    this._feedbackBL = new UserFeedbackBL();
                return this._feedbackBL;
            }
        }

        // GET: UserFeedback/Create
        public ActionResult Create()
        {
            session.Set<string>("PageSession", "User Feedback");
            UserFeedback model = new UserFeedback();
            model = feedbackBL.GetFeedbackQuestions();
            return View(model);
        }

        // POST: UserFeedback/Create
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Create(UserFeedback model)
        {
            session.Set<string>("PageSession", "User Feedback");
            try
            {
                string message = string.Empty;
                
                if (ModelState.IsValid)
                {
                    RELOCBS.AjaxHelper.AjaxResponse result = new RELOCBS.AjaxHelper.AjaxResponse();
                    result.Success = feedbackBL.SumbitUserFeedback(model, out message);
                    if (!result.Success)
                    {
                        ModelState.AddModelError(string.Empty, message);
                    }
                    else
                    {
                        feedbackBL.UpdateUserFeedbackSession();
                        result.Message = message;
                        result.Result = this.RenderPartialViewToString("Create", model);
                        return Json(result);
                        
                    }
                }
                return Request.IsAjaxRequest()? (ActionResult)PartialView("Create", model) : View(model);
            }
            catch
            {
                return View("Create", model);
            }
        }


    }
}
