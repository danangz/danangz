using DaNangZ.BusinessService;
using DaNangZ.DbFirst.Model;
using DaNangZ.UserService.RoleService;
using DaNangZ.UserService.UserService;
using DaNangZ.Web.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebMatrix.WebData;

namespace DaNangZ.Web.Controllers
{
    [Authorize(Roles = "System Admin")]
    public class UserProfileController : Controller
    {
        private readonly IUserService<UserProfile, int> _simpleMembershipService;
        private readonly IRoleService _roleService;
        private IDaNangZService _dnZService = null;

        public UserProfileController(IUserService<UserProfile, int> userService, IRoleService roleService, IDaNangZService dnZService)
        {
            _simpleMembershipService = userService;
            _roleService = roleService;
            _dnZService = dnZService;
        }

        [Authorize(Roles = "System Admin")]
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetAll(string sidx, string sord, int page)
        {
            var userList = _dnZService.UserProfile.GetAll(sidx, sord, page, Constant.PageSize);

            var result = new
            {
                total = userList.TotalPages,
                page = page,
                records = userList.TotalRecords,
                rows = (from user in userList.Data
                        select new
                        {
                            Id = user.UserId,
                            UserName = user.UserName,
                            DisplayName = user.DisplayName,
                            Email = user.Email,
                            Role = _roleService.GetRolesForUser(user.UserName),
                            InsAt = user.InsAt,
                            InsBy = user.InsBy,
                            Action = string.Format("<input type='button' id='btnEdit' title='Sửa tài khoản' onClick='Edit({0})' class='icon-edit'/>&nbsp&nbsp<input type='button' id='btnDel' title='Xóa tài khoản' onClick='Delete({0})'  class='icon-delete'/>", user.UserId),
                        }).ToList()
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddUserProfile(string userName, string displayName, string password, string email, string systemAdmin, string entryAdmin, string pushAdmin, string viewAdmin)
        {
            WebSecurity.CreateUserAndAccount(userName, password, new { UserName = userName, DisplayName = displayName, Email = email, ReceiveEmail = true, StatusId = "StatusActive", UpdBy = Membership.GetUser().UserName, UpdAt = DateTime.Now, InsBy = Membership.GetUser().UserName, InsAt = DateTime.Now }, false);
            if (systemAdmin.Equals("true")) { _roleService.AddUserToRole(userName, Constant.Role.SystemAdmin); }
            if (entryAdmin.Equals("true")) { _roleService.AddUserToRole(userName, Constant.Role.EntryAdmin); }
            if (pushAdmin.Equals("true")) { _roleService.AddUserToRole(userName, Constant.Role.PushAdmin); }
            if (viewAdmin.Equals("true")) { _roleService.AddUserToRole(userName, Constant.Role.ViewAdmin); }

            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetUserProfileToEdit(string userProfileIdEdit)
        {
            UserProfile user = _dnZService.UserProfile.Details(Convert.ToInt32(userProfileIdEdit));

            var result = new
            {
                Id = user.UserId,
                UserName = user.UserName,
                DisplayName = user.DisplayName,
                Email = user.Email,
                Role = _roleService.GetRolesForUser(user.UserName),
                InsAt = user.InsAt,
                InsBy = user.InsBy,
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult EditUserProfile(string userProfileIdEdit, string userName, string displayName, string password, string email, string systemAdmin, string entryAdmin, string pushAdmin, string viewAdmin)
        {
            UserProfile usp = new UserProfile() { UserId = Convert.ToInt32(userProfileIdEdit), UserName = userName, DisplayName = displayName, Email = email };
            var data = _dnZService.UserProfile.Update(usp);
            if (_roleService.IsUserInRole(userName, Constant.Role.SystemAdmin)) { _roleService.RemoveUserFromRole(userName, Constant.Role.SystemAdmin); }
            if (_roleService.IsUserInRole(userName, Constant.Role.EntryAdmin)) { _roleService.RemoveUserFromRole(userName, Constant.Role.EntryAdmin); }
            if (_roleService.IsUserInRole(userName, Constant.Role.PushAdmin)) { _roleService.RemoveUserFromRole(userName, Constant.Role.PushAdmin); }
            if (_roleService.IsUserInRole(userName, Constant.Role.ViewAdmin)) { _roleService.RemoveUserFromRole(userName, Constant.Role.ViewAdmin); }
            if (systemAdmin.Equals("true")) { _roleService.AddUserToRole(userName, Constant.Role.SystemAdmin); }
            if (entryAdmin.Equals("true")) { _roleService.AddUserToRole(userName, Constant.Role.EntryAdmin); }
            if (pushAdmin.Equals("true")) { _roleService.AddUserToRole(userName, Constant.Role.PushAdmin); }
            if (viewAdmin.Equals("true")) { _roleService.AddUserToRole(userName, Constant.Role.ViewAdmin); }

            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult DeleteUserProfile(string userProfileId)
        {
            var user = _dnZService.UserProfile.Details(Convert.ToInt32(userProfileId));

            if (_roleService.IsUserInRole(user.UserName, Constant.Role.SystemAdmin)) { _roleService.RemoveUserFromRole(user.UserName, Constant.Role.SystemAdmin); }
            if (_roleService.IsUserInRole(user.UserName, Constant.Role.EntryAdmin)) { _roleService.RemoveUserFromRole(user.UserName, Constant.Role.EntryAdmin); }
            if (_roleService.IsUserInRole(user.UserName, Constant.Role.PushAdmin)) { _roleService.RemoveUserFromRole(user.UserName, Constant.Role.PushAdmin); }
            if (_roleService.IsUserInRole(user.UserName, Constant.Role.ViewAdmin)) { _roleService.RemoveUserFromRole(user.UserName, Constant.Role.ViewAdmin); }

            if (user != null)
            {
                var data = _dnZService.UserProfile.Delete(Convert.ToInt32(userProfileId));
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}