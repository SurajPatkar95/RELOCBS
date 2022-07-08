using RELOCBS.AjaxHelper;
using RELOCBS.App_Code;
using RELOCBS.BL;
using RELOCBS.BL.TransToFAUpload;
using RELOCBS.CustomAttributes;
using RELOCBS.Entities;
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
    public class TransToFAUploadController : BaseController
    {
        private string _PageID = "27";
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

        private TransToFAUploadBL _transToFAUploadBL;
        public TransToFAUploadBL transToFAUploadBL
        {
            get
            {
                if (this._transToFAUploadBL == null)
                    this._transToFAUploadBL = new TransToFAUploadBL();
                return this._transToFAUploadBL;
            }
        }


        // GET: TransToFAUpload
        public ActionResult Index()
        {
            session.Set<string>("PageSession", "Transfer To FA Upload");
            FillCombo();
            TransToFAUploadVM model = new TransToFAUploadVM();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(TransToFAUploadVM model)
        {
            session.Set<string>("PageSession", "Transfer To FA Upload");
            FillCombo();
            string message = string.Empty;
            //AjaxResponse result = new AjaxResponse();
            if (ModelState.IsValid)
            {

                List<HttpPostedFileBase> Files = new List<HttpPostedFileBase>();
                Files.Add(model.file);
                string FileResult = string.Empty;
                if (!CSubs.ValidateFileForSecurity(Files, out FileResult))
                {

                    ModelState.AddModelError(string.Empty, FileResult);
                    this.AddToastMessage("RELOCBS", FileResult, ToastType.Error);
                    return View(model);
                }
                
                if (model.file != null && model.file.ContentLength > 0)
                {

                    string fileLocation = Server.MapPath("~/uploads/");
                    bool res = transToFAUploadBL.UploadTransFAOther(model, fileLocation, out message);
                    //result.Success = res;
                    //result.Message = message;
                    this.AddToastMessage("RELOCBS", message, !res ? ToastType.Error : ToastType.Success);
                    if (!res)
                    {
                        ModelState.AddModelError(string.Empty, message);
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Upload file is required");
                }

            }

            return View(model);
        }

        [NonAction]
        private void FillCombo()
        {
            ViewData["AppList"] = comboBL.GetTransFA_AppDropdown();
        }

        public ActionResult UploadFormat(int AppID)
        {

            CostUploadFormat format = transToFAUploadBL.GetUploadFormat(AppID);

            if (format.FileID > 0)
            {
                //byte[] fileBytes = RELOCBS.Properties.Resources.OriginCostUploadFormat;
                byte[] fileBytes = (byte[])RELOCBS.Properties.Resources.ResourceManager.GetObject(format.ResourceName, Properties.Resources.Culture);

                string fileName = format.FileName;
                if (fileBytes == null || !fileBytes.Any())
                    return new HttpStatusCodeResult(404);


                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
            }
            else
            {

                return new HttpStatusCodeResult(404);
            }
        }
    }
}
