using RELOCBS.App_Code;
using RELOCBS.BL;
using RELOCBS.BL.Support;
using RELOCBS.CustomAttributes;
using RELOCBS.Entities;
using RELOCBS.Extensions;
using RELOCBS.Utility;
using System;
using System.Web.Mvc;

namespace RELOCBS.Controllers
{
    [AuthorizeUser]
    public class SupportController : BaseController
    {
        private string _PageID = "90";

        private CommonSubs _CSubs;
        public CommonSubs CSubs
        {
            get
            {
                if (_CSubs == null)
                    _CSubs = new CommonSubs();
                return _CSubs;
            }
        }

        private ComboBL _ComboBL;
        public ComboBL ComboBL
        {
            get
            {
                if (_ComboBL == null)
                    _ComboBL = new ComboBL();
                return _ComboBL;
            }
        }

        private SupportBL _SupportBL;
        public SupportBL SupportBL
        {
            get
            {
                if (_SupportBL == null)
                    _SupportBL = new SupportBL();
                return _SupportBL;
            }
        }

        Support _Support = new Support();
        public SupportController(Support SupportObj)
        {
            _Support = SupportObj;
        }

        public ActionResult Index()
        {
            try
            {
                if (!CSubs.CheckPageRights(_PageID, PermissionType.VIEW))
                {
                    return new HttpStatusCodeResult(403);
                }
                session.Set("PageSession", "Support");

                GetDropDownLists();

                return View("Index", _Support);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult Index(Support SupportObj, string SaveButton)
        {
            try
            {
                string message = string.Empty;
                GetDropDownLists();

                if (ModelState.IsValid)
                {
                    bool result = false;
                    int LoginID = UserSession.GetUserSession().LoginID;

                    if (SaveButton == "Remove Inv Approval")
                    {
                        result = SupportBL.RemoveInvApproval(SupportObj, LoginID, out message);
                    }
                    else if (SaveButton == "Change City In Job")
                    {
                        result = SupportBL.ChangeCityInJob(SupportObj, LoginID, out message);
                    }
                    else if (SaveButton == "Change Ref In Inv")
                    {
                        result = SupportBL.ChangeRefInInv(SupportObj, LoginID, out message);
                    }
                    else if (SaveButton == "Change Rev Br")
                    {
                        result = SupportBL.ChangeRevBr(SupportObj, LoginID, out message);
                    }

                    if (result)
                    {
                        if (string.IsNullOrEmpty(message)) message = "Data saved successfully.";
                        this.AddToastMessage("RELOCBS", message, ToastType.Success);
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(message)) message = "Error occured.";
                        this.AddToastMessage("RELOCBS", message, ToastType.Error);
                        ModelState.AddModelError(string.Empty, "Error occured while saving.");
                    }

                    return RedirectToAction("Index");
                }
                else
                {
                    this.AddToastMessage("RELOCBS", message, ToastType.Error);
                    ModelState.AddModelError(string.Empty, "Error occured while saving.");
                    return View("Index", SupportObj);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void GetDropDownLists()
        {
            try
            {
                ViewData["CompBranchList"] = ComboBL.GetCompanyBranchDropdown(ForPage: "Support");
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}