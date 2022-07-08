using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using PagedList;
using RELOCBS.AjaxHelper;
using RELOCBS.App_Code;
using RELOCBS.BL;
using RELOCBS.BL.VehicleKmsTrack;
using RELOCBS.BL.WH_Assessment;
using RELOCBS.CustomAttributes;
using RELOCBS.Entities;
using RELOCBS.Extensions;
using RELOCBS.Utility;

namespace RELOCBS.Controllers
{
    [AuthorizeUser]
    public class WHRiskAssessmentController : BaseController
    {
        private string _PageID = "70";
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

        private WHRiskAssessmentBL _assessmentBL;
        public WHRiskAssessmentBL assessmentBL
        {
            get
            {
                if (this._assessmentBL == null)
                    this._assessmentBL = new WHRiskAssessmentBL();
                return this._assessmentBL;
            }
        }

        // GET: VehicleKmsTrack
        public ActionResult Index(int page = 1)
        {
            if (!CSubs.CheckPageRights(_PageID, PermissionType.VIEW))
            {
                return new HttpStatusCodeResult(403);
            }

            session.Set<string>("PageSession", "Warehouse Checklist");
            string sort = "";
            string sortdir = "";
            string search = "";
            int WarehouseId = -1;

            int pageSize = settings.GetSettingByKey<int>("pagination_pagesize");
            string OrderBy = "";
            int Order = 0;
            DateTime? Fromdate = null;//System.DateTime.Now.Date.AddDays(-2);
            DateTime? Todate = null;//System.DateTime.Now;

            string SearchKey = string.Empty;
            if (Request.Form["FromDate"] != null && Request.Form["FromDate"].Trim() != "")
            {
                Fromdate = Convert.ToDateTime(Request.Form["FromDate"]);
            }

            if (Request.Form["ToDate"] != null && Request.Form["ToDate"].Trim() != "")
            {
                Todate = Convert.ToDateTime(Request.Form["ToDate"]);
            }

            if (Request.Form["WarehouseId"] != null && Request.Form["WarehouseId"].Trim() != "")
            {
                WarehouseId = Convert.ToInt32(Request.Form["WarehouseId"]);
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
            var data = assessmentBL.GetGrid(Fromdate, Todate, sort, sortdir, skip, pageSize, out totalRecord, WarehouseId);
            FillCombo();
            ViewBag.TotalRows = totalRecord;
            ViewBag.search = search;

            var itemsAsIPagedList = new StaticPagedList<Entities.WHAssessmentGrid>(data, pageSize, skip, totalRecord);
            return Request.IsAjaxRequest() ? (ActionResult)PartialView("_AjaxPartial", itemsAsIPagedList) : View(itemsAsIPagedList);
        }

        private void FillCombo(bool IsCreate = false)
        {
            //bool RMCBuss = !(UserSession.GetUserSession().BussinessLine == "NON RMC-BUSINESS");
            int CompID = UserSession.GetUserSession().CompanyID;
            if (IsCreate)
            {
                ViewData["EmployeeList"] = comboBL.GetEmployeeDropdown(CompanyID: CompID);
                ViewData["WHAssessmentStatusList"] = comboBL.GetWHAssessmentStatusDropdown(); ///new List<SelectListItem>();
                //ViewData["ServiceLineList"] = comboBL.GetServiceLineDropdown(RMCBuss: RMCBuss);
                ViewData["CategoryList"] = comboBL.GetWHAssessmentCategoryDropdown();
                ViewData["ResponsibilityList"] = comboBL.GetWHAssessmentResponsibilityDropdown();
                ViewData["PriorityList"] = comboBL.GetWHAssessmentPriorityDropdown();
            }
            ViewData["WarehouseList"] = comboBL.GetWarehouseDropdown();
            
        }

        // GET: WHRiskAssessment/Create
        public ActionResult Create(Int64? id)
        {
            session.Set<string>("PageSession", "Warehouse Checklist");
            FillCombo(true);
            var model = new WHAssessmentViewModel();
            model = assessmentBL.GetDetail(Convert.ToInt64(id));
            return View(model);
        }


        // POST: WHRiskAssessment/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(WHAssessmentViewModel model)
        {
            try
            {
                session.Set<string>("PageSession", "Warehouse Checklist");
                FillCombo(true);
                string message = string.Empty;
                if (ModelState.IsValid)
                {
                    bool res = false;
                    if (!string.IsNullOrWhiteSpace(model.HFVQuestions))
                    {
                        List<AssessmentQuestions> questions = JsonConvert.DeserializeObject<List<AssessmentQuestions>>(model.HFVQuestions);
                        model.otherQuestions = questions;
                    }
                    
                    res = assessmentBL.Insert(model, out message);
                    if (!res)
                    {
                        ModelState.AddModelError(string.Empty, "Unable to save data.");
                        this.AddToastMessage("RELOCBS", message, ToastType.Error);
                        //return Json(result);
                        return View("Create", model);
                    }
                    else
                    {
                        this.AddToastMessage("RELOCBS", message, ToastType.Success);
                        return RedirectToAction("Create", new { id = model.TransId });
                        //return Json(result);
                    }
                }
                else
                {
                    return View("Create", model);
                }
            }
            catch
            {
                return View("Create", model);
            }
        }

        
        // POST: WHRiskAssessment/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            try
            {

                AjaxResponse result = new AjaxResponse();
                string Message = string.Empty;

                if (ModelState.IsValid)
                {
                    result.Success = assessmentBL.Delete(id, out Message);

                    if (result.Success)
                    {
                        result.Result = Message;
                    }
                    else
                    {
                        ModelState.AddModelError(String.Empty, Message);
                    }

                }
                else
                {
                    result.Success = false;
                    result.Result = "Delete failed. Please try again.";
                }

                return Json(result);
            }
            catch
            {
                return View();
            }
        }


        public ActionResult GetWarehoueArea(int id)
        {
            return Json(assessmentBL.GetWarehouseArea(id));
        }


        private string FileUploadFillCombo(string Type, string DocName = "")
        {
            String FromType = Type == "ENQDETAIL" ? "ENQUIRY" : Type;

            if (!string.IsNullOrWhiteSpace(DocName))
            {
                DocName = DocName.ToUpper();
            }
            else
            {
                DocName = "";
            }

            List<SelectListItem> items = comboBL.GetWHDocTypeDropdown(DocFromType: FromType).ToList();
            string value = "-1";

            if (items.Count > 0)
            {
                if (items.Count == 1 || Type.ToUpper() == "ENQUIRY")
                {
                    items.First().Selected = true;
                    value = items.First().Value;

                }
                else
                {
                    items.Last().Selected = true;
                    value = items.Last().Value;
                    Type = "ENQUIRY";
                }

            }

            ViewData["DocTypeList"] = items;

            List<SelectListItem> DocItems = comboBL.GetWHDocNameDropdown(DocTypeID: Convert.ToInt32(value)).ToList();
            if (!string.IsNullOrWhiteSpace(DocName))
            {
                DocItems = DocItems.Where<SelectListItem>(m => m.Text.ToUpper() == DocName).ToList();
                DocItems.Last().Selected = true;
            }
            ViewData["DocNameList"] = DocItems;

            return value;

        }

        // GET: DMS/Create
        //[OutputCache(Duration = 0)]
        public ActionResult CreateDoc(int id, string Type, string DocName)
        {

            string DocType = FileUploadFillCombo(Type.ToUpper(), DocName);

            int DocTypeID = -1;

            if (!string.IsNullOrWhiteSpace(DocType))
            {
                DocTypeID = Convert.ToInt32(DocType);
            }

            string FromType = Type;

            JobDocUpload Model = assessmentBL.GetDocumentGrid(id, Type, FromType, DocTypeID);
            Model.DocNameText = DocName;
            return Request.IsAjaxRequest()
                ? (ActionResult)PartialView("CreateDoc", Model)
                : View(Model);
        }

        // POST: DMS/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateDoc(JobDocUpload Model, int id, string Type)
        {
            try
            {
                string DocType = FileUploadFillCombo(Type.ToUpper(), Model.DocNameText);

                if (ModelState.IsValid)
                {
                    string message = string.Empty;
                    bool res = false;
                    AjaxResponse result = new AjaxResponse();
                    MoveManageViewModel JobModel = new MoveManageViewModel();
                    JobModel.jobDocUpload = Model;

                    res = assessmentBL.InsertDocument(JobModel, UserSession.GetUserSession().LoginID, out message);

                    if (res)
                    {

                        string FromType = Type;
                        switch (Type.ToUpper())
                        {
                            case "ENQDETAIL":

                                FromType = "Enquiry";

                                break;

                            default:
                                break;
                        }

                        Model = assessmentBL.GetDocumentGrid(id, Type, FromType, Model.DocTypeID);
                        //Model.DocName = DocName;
                        result.Result = this.RenderPartialViewToString("CreateDoc", Model);
                        return Json(result);

                        //result.Success = false;
                        //ModelState.AddModelError(string.Empty, message);
                        //result.Message = message;

                        //this.AddToastMessage("RELOCBS", message, ToastType.Error);

                    }
                    else
                    {
                        //result.Success = true;
                        //result.Message = message;
                        ModelState.AddModelError(string.Empty, message);
                    }


                }

                return Request.IsAjaxRequest()
                  ? (ActionResult)PartialView("CreateDoc", Model)
                  : View(Model);
            }
            catch
            {
                return Request.IsAjaxRequest()
                  ? (ActionResult)PartialView("CreateDoc", Model)
                  : View(Model);
            }
        }


        // POST: DMS/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DocDelete(int FileID, JobDocUpload Model)
        {
            try
            {
                string DocType = FileUploadFillCombo(Model.DocFromType.ToUpper(), Model.DocNameText);
                string DocName = Model.DocNameText;

                /////Remove the model Error in delete
                foreach (var modelValue in ModelState.Values)
                {
                    modelValue.Errors.Clear();
                }
                ////if (ModelState.IsValid)
                {
                    string message = string.Empty;
                    bool res = false;
                    AjaxResponse result = new AjaxResponse();

                    res = assessmentBL.DeleteDocument(FileID, out message);

                    if (res)
                    {

                        string FromType = Model.DocFromType;
                        
                        Model = assessmentBL.GetDocumentGrid(Model.ID, Model.DocFromType, FromType, Model.DocTypeID);
                        Model.DocNameText = DocName;
                        result.Result = this.RenderPartialViewToString("CreateDoc", Model);
                        return Json(result);

                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, message);
                    }


                }

                return Request.IsAjaxRequest()
                  ? (ActionResult)PartialView("CreateDoc", Model)
                  : View(Model);
            }
            catch
            {
                return Request.IsAjaxRequest()
                  ? (ActionResult)PartialView("CreateDoc", Model)
                  : View(Model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ViewDocument(int id)
        {
            //string path = Server.MapPath("~/Content/JViewFiles/");
            //if (!Directory.Exists(path))
            //{
            //    Directory.CreateDirectory(path);
            //}
            JobDocument jobDocument = assessmentBL.GetDownloadFile(id);
            if (!string.IsNullOrWhiteSpace(jobDocument.FilePath) && System.IO.File.Exists(jobDocument.FilePath))
            {
                //var filepath = System.IO.Path.Combine(Server.MapPath("/Files/"), fileName);

                byte[] FileBytes = System.IO.File.ReadAllBytes(jobDocument.FilePath);
                Response.Headers.Add("Content-Disposition", "inline;filename=\"" + jobDocument.FileName + "\"");
                //HttpContext.Response.AppendHeader("Content-Disposition", "inline;filename=" + jobDocument.FileName + "");
                return File(FileBytes, MimeMapping.GetMimeMapping(jobDocument.FilePath));

                ////Using the FileStreamResult 
                //var fileStream = new FileStream(jobDocument.FilePath, FileMode.Open, FileAccess.Read);
                //var fsResult = new FileStreamResult(fileStream, MimeMapping.GetMimeMapping(jobDocument.FileName));
                ////fsResult.FileDownloadName = jobDocument.FileName;
                //return fsResult;
            }

            return new HttpStatusCodeResult(404);

        }


        public ActionResult DocDownload(int id)
        {
            JobDocument jobDocument = assessmentBL.GetDownloadFile(id);
            if (!string.IsNullOrWhiteSpace(jobDocument.FilePath) && System.IO.File.Exists(jobDocument.FilePath))
            {
                //var filepath = System.IO.Path.Combine(Server.MapPath("/Files/"), fileName);
                return File(jobDocument.FilePath, MimeMapping.GetMimeMapping(jobDocument.FilePath), jobDocument.FileName);
            }

            return new HttpStatusCodeResult(404);
        }
        
        public ActionResult GetReport(int id)
        {
            var jobreportmodel = assessmentBL.GetReport(id);
            return View("Report", jobreportmodel);
        }

        [HttpPost]
        [ValidateInput(false)]
        public FileResult Export(string GridHtml)
        {
            return File(Encoding.ASCII.GetBytes(GridHtml), "application/vnd.ms-excel", "WarehouseChecklist.xls");
        }

    }
}
