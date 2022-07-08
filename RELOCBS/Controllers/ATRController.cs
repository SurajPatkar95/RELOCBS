using PagedList;
using RELOCBS.App_Code;
using RELOCBS.BL;
using RELOCBS.BL.ATR;
using RELOCBS.CustomAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RELOCBS.Entities;
using RELOCBS.Extensions;
using RELOCBS.AjaxHelper;

namespace RELOCBS.Controllers
{
    [AuthorizeUser]
    public class ATRController : BaseController
    {

        private string _PageID = "73";
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
        
        private ATRBL _aTRBL;
        public ATRBL aTRBL
        {
            get
            {
                if (this._aTRBL == null)
                    this._aTRBL = new ATRBL();
                return this._aTRBL;
            }
        }


        private CommonSubs _CSubs;

        public CommonSubs CSubs
        {

            get
            {
                if (this._CSubs == null)
                    this._CSubs = new CommonSubs();
                return this._CSubs;
            }
        }

        // GET: ATR
        public ActionResult Index(int page = 1)
        {
            if (!CSubs.CheckPageRights(_PageID, PermissionType.VIEW))
            {
                return new HttpStatusCodeResult(403);
            }

            session.Set<string>("PageSession", "ATR");
            string sort = "ATRPointId";
            string sortdir = "desc";
            int pageSize = settings.GetSettingByKey<int>("pagination_pagesize");
            string OrderBy = "";
            int Order = 0;
            int? DeptId = -1;
            string IssueHeading = string.Empty;
            int? ComplianceStatusId = -1;
            DateTime? IssuedMonth=null;


            string SearchKey = string.Empty;
            if (Request.Form["IssuedMonth"] != null && Request.Form["IssuedMonth"].Trim() != "")
            {
                IssuedMonth = Convert.ToDateTime(Request.Form["IssuedMonth"]);
            }
            if (Request.Form["IssueHeading"] != null && Request.Form["IssueHeading"].Trim() != "")
            {
                IssueHeading = Convert.ToString(Request.Form["IssueHeading"]);
            }

            if (Request.Form["ComplianceStatusId"] != null && Request.Form["ComplianceStatusId"].Trim() != "")
            {
                ComplianceStatusId = Convert.ToInt32(Request.Form["ComplianceStatusId"]);
            }

            if (Request.Form["DeptId"] != null && Request.Form["DeptId"].Trim() != "")
            {
                DeptId = Convert.ToInt32(Request.Form["DeptId"]);
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
            
            var data = aTRBL.GetForGrid(DeptId, IssueHeading, ComplianceStatusId, IssuedMonth, sort, sortdir, skip, pageSize, out totalRecord);
            FillCombo();
            
            ViewBag.TotalRows = totalRecord;
            ViewBag.IssueHeading = IssueHeading;

            var itemsAsIPagedList = new StaticPagedList<Entities.ATRPointGrid>(data, pageSize, skip, totalRecord);
            return Request.IsAjaxRequest() ? (ActionResult)PartialView("_AjaxPartial", itemsAsIPagedList) : View(itemsAsIPagedList);
        }

        private void FillCombo(Int64? id=-1)
        {
            ViewData["RiskList"] = comboBL.GetATRRiskDropdown();
            ViewData["DeptList"] = comboBL.GetATRDepartmentDropdown();
            ViewData["CompStatusList"] = comboBL.GetATRStatusDropdown(IsCompliance:true,ATRPointId:id);
            ViewData["AuditeeStatusList"] = comboBL.GetATRStatusDropdown(IsAuitee: true);
            ViewData["EmpList"] = comboBL.GetATREmployeeDropdown();
            ViewData["CategoryList"] = comboBL.GetATRCategoryDropdown();
        }


        // GET: ATR/Create
        public ActionResult Create(int? id)
        {
            ATRPoint data = new ATRPoint();
            FillCombo(id:id);
            if (id != null)
            {
                data = aTRBL.GetDetailById(Convert.ToInt32(id));
            }
            
            return Request.IsAjaxRequest()? (ActionResult)PartialView("Create", data): View(data);

        }

        // POST: ATR/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ATRPoint model)
        {
            try
            {
                AjaxResponse result = new AjaxResponse();
                string message = string.Empty;
                ViewBag.Flag = "1";
                FillCombo(id: model.ATRPointId);
                if (ModelState.IsValid)
                {
                    result.Success = aTRBL.Insert(model, out message);

                    if (result.Success)
                    {
                        result.Result = this.RenderPartialViewToString("Create", model);
                        return Json(result);
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, message);
                    }
                }
                return Request.IsAjaxRequest() ? (ActionResult)PartialView("Create", model) : View("Create", model);
            }
            catch
            {
                return View("Create", model);
            }
        }

        
        public ActionResult MngtResponse(int id)
        {
            FillCombo();
            ATRPointReponse data = aTRBL.GetMgtResponseDetailById(Convert.ToInt32(id));
            data.submit = "Save";
            return Request.IsAjaxRequest() ? (ActionResult)PartialView("MngtResponse", data) : View(data);

        }

        // POST: ATR/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MngtResponse(ATRPointReponse model)
        {
            try
            {
                AjaxResponse result = new AjaxResponse();
                string message = string.Empty;
                ViewBag.Flag = "1";
                FillCombo();
                if (model.submit.Equals("DeleteDoc", StringComparison.OrdinalIgnoreCase) && model.FileDeleteId > 0)
                {
                    ModelState.Remove("AuditeeStatusId");
                    ModelState.Remove("MgtReponse");
                    ModelState.Remove("CommittedDate");
                }

                if (model.IsHO)
                {
                    //ModelState.Remove("AuditeeStatusId");
                    ModelState.Remove("CommittedDate");
                }

                if (ModelState.IsValid)
                {

                    if(model.submit.Equals("DeleteDoc",StringComparison.OrdinalIgnoreCase) && model.FileDeleteId > 0)
                    {
                        result.Success  = aTRBL.DeleteDocument(Convert.ToInt64(model.FileDeleteId), out message);
                    }
                    else
                    {
                        result.Success = aTRBL.InsertManagementResponse(model, out message);
                    }
                    result.Message = message;
                    if (result.Success)
                    {
                        model = aTRBL.GetMgtResponseDetailById(Convert.ToInt32(model.aTRPoint.ATRPointId));
                        result.Result = this.RenderPartialViewToString("MngtResponse", model);
                        return Json(result);
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, message);
                    }
                }
                return Request.IsAjaxRequest() ? (ActionResult)PartialView("MngtResponse", model) : View("MngtResponse", model);
            }
            catch
            {
                return Request.IsAjaxRequest() ? (ActionResult)PartialView("MngtResponse", model) : View("MngtResponse", model);
            }
        }

        public ActionResult ViewDocument(int id)
        {
            
            ATRPointDoc jobDocument = aTRBL.GetDownloadFile(id);
            if (!string.IsNullOrWhiteSpace(jobDocument.FilePath) && System.IO.File.Exists(jobDocument.FilePath))
            {
                byte[] FileBytes = System.IO.File.ReadAllBytes(jobDocument.FilePath);
                Response.Headers.Add("Content-Disposition", "inline;filename=\"" + jobDocument.FileName + "\"");
                
                return File(FileBytes, MimeMapping.GetMimeMapping(jobDocument.FilePath));
            }

            return new HttpStatusCodeResult(404);

        }


        public ActionResult DocDownload(int id)
        {
            ATRPointDoc jobDocument = aTRBL.GetDownloadFile(id);
            if (!string.IsNullOrWhiteSpace(jobDocument.FilePath) && System.IO.File.Exists(jobDocument.FilePath))
            {
                //var filepath = System.IO.Path.Combine(Server.MapPath("/Files/"), fileName);
                return File(jobDocument.FilePath, MimeMapping.GetMimeMapping(jobDocument.FilePath), jobDocument.FileName);
            }

            return new HttpStatusCodeResult(404);
        }

        // GET: ATR/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ATR/Delete/5
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
    }
}
