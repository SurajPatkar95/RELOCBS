using PagedList;
using RELOCBS.AjaxHelper;
using RELOCBS.App_Code;
using RELOCBS.BL;
using RELOCBS.BL.VendorContract;
using RELOCBS.CustomAttributes;
using RELOCBS.Extensions;
using RELOCBS.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RELOCBS.Controllers
{
    [AuthorizeUser]
    public class VendorContractController : BaseController
    {
        private string _PageID = "85";

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

        private VendorContractBL _BL;
        public VendorContractBL BL
        {
            get
            {
                if (this._BL == null)
                    this._BL = new VendorContractBL();
                return this._BL;
            }
        }

        private void FillCombo()
        {
            ViewData["BranchList"] = comboBL.GetCompanyBranchDropdown();
            ViewData["EmployeeList"] = comboBL.GetEmployeeDropdown();
            ViewData["ServiceCategoryList"] = comboBL.GetVendorContractServiceCategoryDropdown();
            ViewData["BusinessUnitList"] = comboBL.GetVendorContractBuinessUnitDropdown();
            ViewData["MSMEList"] = RELOCBS.Common.CommonService.MSME;
            ViewData["VCStatusList"] = comboBL.GetVCStatusList();
        }


        // GET: VendorContract
        public ActionResult Index(int page = 1)
        {
            if (!CSubs.CheckPageRights(_PageID, PermissionType.VIEW))
            {
                return new HttpStatusCodeResult(403);
            }

            session.Set<string>("PageSession", "Vendor Contract");
            string sort = "";
            string sortdir = "";
            int pageSize = settings.GetSettingByKey<int>("pagination_pagesize");
            int Order = 0;
            string VendorName = null;
            string MasterCode = string.Empty;
            string SubCode = string.Empty;
            if (Request.Form["MasterCode"] != null && Request.Form["MasterCode"].Trim() != "")
            {
                MasterCode = Convert.ToString(Request.Form["MasterCode"]);
            }

            if (Request.Form["VendorName"] != null && Request.Form["VendorName"].Trim() != "")
            {
                VendorName = Convert.ToString(Request.Form["VendorName"]);
            }
            if (Request.Form["SubCode"] != null && Request.Form["SubCode"].Trim() != "")
            {
                SubCode = Convert.ToString(Request.Form["SubCode"]);
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
            var data = BL.GetGrid(VendorName,MasterCode,SubCode, sort, sortdir, skip, pageSize, out totalRecord);
            FillCombo();
            ViewBag.TotalRows = totalRecord;

            var itemsAsIPagedList = new StaticPagedList<Entities.VendorContractGrid>(data, pageSize, skip, totalRecord);
            return Request.IsAjaxRequest() ? (ActionResult)PartialView("_AjaxPartial", itemsAsIPagedList) : View(itemsAsIPagedList);
        }


        // GET: VendorContract/Create
        public ActionResult Create(int? id)
        {
            session.Set<string>("PageSession", "Vendor Contract Creation");
            FillCombo();
            Entities.VendorContractModel data = new Entities.VendorContractModel();
            if (id != null && id > 0)
            {
                data = BL.GetDetail(Convert.ToInt32(id));
            }

            return Request.IsAjaxRequest() ? (ActionResult)PartialView("Create", data) : View(data);
        }

        // POST: VendorContract/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Entities.VendorContractModel model)
        {
            try
            {
                session.Set<string>("PageSession", "Vendor Contract Creation");
                FillCombo();
                string message = string.Empty;
                if (ModelState.IsValid)
                {
                    bool res = false;
                    

                    res = BL.Insert(model, out message);
                    if (!res)
                    {
                        ModelState.AddModelError(string.Empty, "Unable to save data.");
                        this.AddToastMessage("RELOCBS", message, ToastType.Error);
                        
                        return View("Create", model);
                    }
                    else
                    {
                        this.AddToastMessage("RELOCBS", message, ToastType.Success);
                        return RedirectToAction("Create", new { id = model.VContractId });
                        
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
        

        // POST: VendorContract/Delete/5
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
                    result.Success = BL.Delete(id, out Message);

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

        private int FillDocCombo()
        {

            List<SelectListItem> items = comboBL.GetVendorContractDocTypeDropdown(DocFromType: "VendorContract").ToList();
            int value = -1;

            if (items.Count > 0)
            {
                items.Last().Selected = true;
                value = Convert.ToInt32(items.Last().Value);
            }
            ViewData["DocTypeList"] = items;
            ViewData["DocNameList"] = comboBL.GetVendorContractDocumentDropdown(DocNameID: value);
            

            return value;
        }

        public ActionResult CreateDocument(int id, string Type)
        {

            
            int DocTypeID = FillDocCombo();

            string FromType = Type;

            Entities.VendorContractDocUpload Model = BL.GetDocumentGrid(id, Type, FromType, DocTypeID);
            return Request.IsAjaxRequest()? (ActionResult)PartialView("CreateDoc", Model): View("CreateDoc", Model);
        }

        // POST: DMS/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateDocument(Entities.VendorContractDocUpload Model, int id, string Type)
        {
            try
            {
                FillDocCombo();
                if (ModelState.IsValid)
                {
                    string message = string.Empty;
                    bool res = false;
                    AjaxResponse result = new AjaxResponse();

                    res = BL.InsertDocument(Model, UserSession.GetUserSession().LoginID, out message);

                    if (res)
                    {

                        string FromType = Type;

                        Model = BL.GetDocumentGrid(id, Type, FromType, Model.DocTypeID);
                        
                        result.Result = this.RenderPartialViewToString("CreateDoc", Model);
                        return Json(result);
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, message);
                    }
                }

                return Request.IsAjaxRequest()? (ActionResult)PartialView("CreateDoc", Model): View("CreateDoc",Model);
            }
            catch
            {
                return Request.IsAjaxRequest()? (ActionResult)PartialView("CreateDoc", Model): View("CreateDoc",Model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DocDelete(int FileID, Entities.VendorContractDocUpload Model)
        {
            try
            {
                FillDocCombo();
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

                    res = BL.DeleteDocument(FileID, out message);

                    if (res)
                    {

                        string FromType = Model.DocFromType;

                        Model = BL.GetDocumentGrid(Model.ID, Model.DocFromType, FromType, Model.DocTypeID);
                        Model.DocNameText = DocName;
                        result.Result = this.RenderPartialViewToString("CreateDoc", Model);
                        return Json(result);

                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, message);
                    }


                }

                return Request.IsAjaxRequest()? (ActionResult)PartialView("CreateDoc", Model): View("CreateDoc",Model);
            }
            catch
            {
                return Request.IsAjaxRequest()? (ActionResult)PartialView("CreateDoc", Model): View("CreateDoc", Model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ViewDocument(int id)
        {
            Entities.VendorContractDocuments jobDocument = BL.GetDownloadFile(id);
            if (!string.IsNullOrWhiteSpace(jobDocument.FilePath) && System.IO.File.Exists(jobDocument.FilePath))
            {
                byte[] FileBytes = System.IO.File.ReadAllBytes(jobDocument.FilePath);
                Response.Headers.Add("Content-Disposition", "inline;filename=\"" + jobDocument.FileName + "\"");
                return File(FileBytes, MimeMapping.GetMimeMapping(jobDocument.FilePath));
            }

            return new HttpStatusCodeResult(404);

        }


        public JsonResult GetDocName(int DocType)
        {
            var lstItem = comboBL.GetVendorContractDocumentDropdown(DocTypeID: DocType, Allitems: false).Select(i => new { i.Value, i.Text }).ToList();
            return Json(lstItem, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DocDownload(int id)
        {
            Entities.VendorContractDocuments jobDocument = BL.GetDownloadFile(id);
            if (!string.IsNullOrWhiteSpace(jobDocument.FilePath) && System.IO.File.Exists(jobDocument.FilePath))
            {
                return File(jobDocument.FilePath, MimeMapping.GetMimeMapping(jobDocument.FilePath), jobDocument.FileName);
            }

            return new HttpStatusCodeResult(404);
        }

        public ActionResult GetDocumentList(Int64 id, Int16? DocTypeID, int? DocNameID, string DocDescription = "")
        {
            FillDocCombo();
            Entities.VendorContractDocUpload DocModel;
            int DTypeID = DocTypeID == null ? -1 : Convert.ToInt16(DocTypeID);
            int DNameID = DocNameID == null ? -1 : Convert.ToInt32(DocNameID);
            DocModel = BL.GetDocumentGrid(id, "VendorContract", "VendorContract", DTypeID, DNameID, DocDescription);
            AjaxResponse result = new AjaxResponse();
            result.Result = this.RenderPartialViewToString("CreateDoc", DocModel);
            return Json(result);
        }
    }
}
