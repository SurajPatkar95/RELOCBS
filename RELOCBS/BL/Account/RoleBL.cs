using RELOCBS.Common.ExceptionHandling;
using RELOCBS.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RELOCBS.DAL.Account;
using System.Data;
using RELOCBS.Entities;
using Newtonsoft.Json;

namespace RELOCBS.BL.Account
{
    public class RoleBL
    {
        private RoleDAL _roleDAL;

        public RoleDAL roleDAL
        {

            get
            {
                if (this._roleDAL == null)
                    this._roleDAL = new RoleDAL();
                return this._roleDAL;
            }
        }

        public bool Insert(Role role, out string result)
        {
            try
            {
                DataTable dt = new DataTable("RoleMenuList");
                if (role.HFGridValue != null)
                {
                    dt = (DataTable)JsonConvert.DeserializeObject(role.HFGridValue, (typeof(DataTable)));
                }
                else
                {
                    dt.Columns.Add("Id", typeof(int));
                    dt.Columns.Add("Name", typeof(string));
                    dt.Columns.Add("View", typeof(bool));
                    dt.Columns.Add("Add", typeof(bool));
                    dt.Columns.Add("Edit", typeof(bool));
                    dt.Columns.Add("Delete", typeof(bool));
                }
                //dt = (DataTable)JsonConvert.DeserializeObject(role.HFGridValue, (typeof(DataTable)));
                dt.TableName = "RoleMenuList";
                return roleDAL.Insert(role, dt, out result);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "RoleBL", "Insert", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }
        }

        public IEnumerable<MenuRole> GetDetailById(int? Roleid)
        {
            IEnumerable<MenuRole> MenuList = new List<MenuRole>();
            try
            {
                DataTable dtRole = roleDAL.GetDetailById(Roleid, UserSession.GetUserSession().LoginID);
                if (dtRole.Rows.Count > 0)
                {
                    MenuList = dtRole.AsEnumerable().
                        Select(dataRow => new MenuRole
                        {
                            MenuId = Convert.ToInt32(dataRow["MenuId"]),
                            MenuName = Convert.ToString(dataRow["MenuName"]),
                            SubMenuName = Convert.ToString(dataRow["SubMenu"]),
                            ParentMenuName = Convert.ToString(dataRow["ParentMenu"]),
                            IsActive = Convert.ToString(dataRow["ACTIVE"]),
                            Allow_Add = Convert.ToBoolean(dataRow["Allow_Add"]),
                            Allow_Edit = Convert.ToBoolean(dataRow["Allow_Edit"]),
                            Allow_Delete = Convert.ToBoolean(dataRow["Allow_Delete"]),
                            Allow_View = Convert.ToBoolean(dataRow["Allow_View"]),
                        }).ToList();

                }
                

                return MenuList;

            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "RoleBL", "GetDetailById", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }
        }

        public IEnumerable<Role> GetRoleDetail(int? Roleid = null)
        {
            IEnumerable<Role> RoleList = new List<Role>();
            try
            {
                DataTable dtRole = roleDAL.GetRoleDetail(Roleid, UserSession.GetUserSession().LoginID);
                if (dtRole.Rows.Count > 0)
                {
                    RoleList = dtRole.AsEnumerable().
                        Select(dataRow => new Role
                        {
                            RoleId = Convert.ToInt32(dataRow["RoleID"]),
                            RoleName = Convert.ToString(dataRow["RoleName"])
                        }).ToList();

                }


                return RoleList;

            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "RoleBL", "GetDetailById", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }
        }
    }
}