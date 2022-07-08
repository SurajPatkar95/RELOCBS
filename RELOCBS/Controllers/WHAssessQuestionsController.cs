using PagedList;
using RELOCBS.App_Code;
using RELOCBS.BL;
using RELOCBS.BL.WH_Assessment;
using RELOCBS.CustomAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RELOCBS.Entities;
using RELOCBS.Common;
using RELOCBS.Utility;
using RELOCBS.Extensions;

namespace RELOCBS.Controllers
{
    [AuthorizeUser]
    public class WHAssessQuestionsController : BaseController
    {
        private string _PageID = "72";

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

        private WHAssessQuestionsBL _questionBL;
        public WHAssessQuestionsBL questionBL
        {
            get
            {
                if (this._questionBL == null)
                    this._questionBL = new WHAssessQuestionsBL();
                return this._questionBL;

            }
        }

        private void FillCombo()
        {  
            ViewData["Category"] = comboBL.GetWHAssessmentCategoryDropdown();
            ViewData["Resposibility"] = comboBL.GetWHAssessmentResponsibilityDropdown();
            ViewData["Priority"] = comboBL.GetWHAssessmentPriorityDropdown();
        }


        // GET: WHAssessQuestions
        public ActionResult Index(int page=1)
        {
            if (!CSubs.CheckPageRights(_PageID, PermissionType.VIEW))
            {
                return new HttpStatusCodeResult(403);
            }

            session.Set<string>("PageSession", "Warehouse Checklist Paramater");
            string sort = "";
            string sortdir = "";
            
            

            int pageSize = settings.GetSettingByKey<int>("pagination_pagesize");
            string OrderBy = "";
            int Order = 0;
            int? CategoryId=null;
            string SearchKey = string.Empty;
            if (Request.Form["SearchKey"] != null && Request.Form["SearchKey"].Trim() != "")
            {
                SearchKey = Convert.ToString(Request.Form["SearchKey"]);
            }

            if (Request.Form["CategoryId"] != null && Request.Form["CategoryId"].Trim() != "")
            {
                CategoryId = Convert.ToInt32(Request.Form["CategoryId"]);
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
            int skip = pageSize;
            pageSize = page;
            var data = questionBL.GetGrid( CategoryId, SearchKey, sort, sortdir, skip, pageSize, out totalRecord);
            FillCombo();
            ViewBag.TotalRows = totalRecord;
            ViewBag.search = SearchKey;

            var itemsAsIPagedList = new StaticPagedList<Entities.WHAssessmentQuestionGrid>(data, pageSize, skip, totalRecord);
            return Request.IsAjaxRequest() ? (ActionResult)PartialView("_AjaxPartial", itemsAsIPagedList) : View(itemsAsIPagedList);
        }


        // GET: Warehouse/Create
        public ActionResult Create(int? id)
        {
            FillCombo();
            WHAssessmentQuestions data = null;
            if (id!=null && id > 0)
            {
                data = questionBL.GetDetail(Convert.ToInt32(id));
            }
            
            return Request.IsAjaxRequest()? (ActionResult)PartialView("Create", data): View(data);
        }

        // POST: Warehouse/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(WHAssessmentQuestions data)
        {
            try
            {
                FillCombo();
                RELOCBS.AjaxHelper.AjaxResponse result = new RELOCBS.AjaxHelper.AjaxResponse();
                if (ModelState.IsValid)
                {
                    string Message;
                    result.Success = questionBL.Insert(data, out Message);
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

        
        // GET: WHAssessQuestions/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: WHAssessQuestions/Delete/5
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

        public JsonResult GetAutoPopulateList()
        {
            var lst = comboBL.GetCompanyBranchDropdown().Select(i => new { i.Value, i.Text }).ToList();
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

                param.Add("@SP_LoginID", Convert.ToString(UserSession.GetUserSession().LoginID));

                CommonService.GenerateExcel(this.Response, "Warehouse", "[Comm].[GETWarehouseForGrid_ExpToExl]", param);
            }
            catch (Exception ex)
            {
                this.AddToastMessage("RELOCBS", "UnExpected Error occured", ToastType.Error);
            }
            return View();
        }

    }
}
