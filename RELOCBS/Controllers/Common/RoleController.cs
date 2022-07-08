using RELOCBS.App_Code;
using RELOCBS.BL.Account;
using RELOCBS.CustomAttributes;
using RELOCBS.Entities;
using RELOCBS.Extensions;
using RELOCBS.Models;
using RELOCBS.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RELOCBS.Controllers.Common
{
    [AuthorizeUser]
    public class RoleController : BaseController
    {

        private string _PageID = "35";

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

        private RoleBL _roleBL;

        public RoleBL roleBL
        {

            get
            {
                if (this._roleBL == null)
                    this._roleBL = new RoleBL();
                return this._roleBL;
            }
        }

        // GET: Role
        public ActionResult Index()
        {
            if (!CSubs.CheckPageRights(_PageID, PermissionType.VIEW))
            {
                return new HttpStatusCodeResult(403);
            }
            session.Set<string>("PageSession", "Role");

            IEnumerable<Role> list = roleBL.GetRoleDetail();
            ViewBag.RoleList = list.AsEnumerable();
            return View(new Role());
        }

        public ActionResult RoleCreation(int Role)
        {
            IEnumerable<MenuRole> menulist = roleBL.GetDetailById(Role);
            return PartialView("RoleGrid", menulist);
        }

        public ActionResult Save(Role role)
        {
            string message = string.Empty;
            bool res = false;
            res = roleBL.Insert(role, out message);
            
            if (!res)
            {
                ModelState.AddModelError(string.Empty, "Unable to save Enquiry data.");
                this.AddToastMessage("RELOCBS", message, ToastType.Error);
                //return Json(result);
            }
            else
            {
                this.AddToastMessage("RELOCBS", message, ToastType.Success);
                //return Json(result);
            }
            return RedirectToAction("Index");
        }
    }
}