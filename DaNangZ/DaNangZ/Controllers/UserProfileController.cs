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
                            Action = string.Format("<input type='button' id='btnEdit' title='Sửa danh mục' onClick='Edit({0})' class='icon-edit'/>&nbsp&nbsp<input type='button' id='btnDel' title='Xóa danh mục' onClick='Delete({0})'  class='icon-delete'/>", user.UserId),
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
    }
}