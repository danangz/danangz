using DaNangZ.BusinessService;
using DaNangZ.DbFirst.Model;
using DaNangZ.UserService.RoleService;
using DaNangZ.UserService.UserService;
using DaNangZ.Web.Common;
using DaNangZ.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebMatrix.WebData;

namespace DaNangZ.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService<UserProfile, int> _userService;
        private IDaNangZService _dnZService = null;
        private readonly IRoleService _roleService;

        public AccountController(IUserService<UserProfile, int> userService, IDaNangZService dnZService, IRoleService roleService)
        {
            _userService = userService;
            _dnZService = dnZService;
            _roleService = roleService;
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            var isLoged = System.Web.HttpContext.Current.User.Identity.IsAuthenticated;
            ViewBag.ReturnUrl = returnUrl;

            if (IsValidUri(returnUrl))
            {
                return View("AccessDenied");
            }

            if (!isLoged)
            {
                return View();
            }

            return View();
        }

        public bool IsValidUri(string uri)
        {
            Uri validatedUri;
            return Uri.TryCreate(uri, UriKind.RelativeOrAbsolute, out validatedUri);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (LoginTroughLocalMembership(model))
                {
                    return RedirectToLocal(returnUrl);
                }
            }
            return View(model);
        }

        private bool LoginTroughLocalMembership(LoginModel model)
        {
            try
            {
                string username = "admin";
                string password = "danangZ@123$";

                if (WebSecurity.GetUserId(username) == -1)
                {
                    WebSecurity.CreateUserAndAccount(username, password, new { UserName = username, DisplayName = username, Email = "admin@danangz.com", ReceiveEmail = true, StatusId = Constant.Active, UpdBy = username, UpdAt = DateTime.Now, InsBy = username, InsAt = DateTime.Now }, false);
                    _roleService.AddUserToRole(username, Constant.Role.SystemAdmin);
                }

                if (WebSecurity.Login(model.UserName, model.Password, persistCookie: model.RememberMe))
                {
                    if (_userService.GetUserByUserName(model.UserName) != null)
                    {
                        FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);
                        return true;
                    }
                    else
                    {
                        ModelState.AddModelError("LoginFail", "The user name is inactive.");
                        return false;
                    }
                }
                else
                {
                    ModelState.AddModelError("LoginFail", "The user Id or password provided is incorrect.");
                    return false;
                }
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError("", "System user and role setting is incorrectly configured.");
            }

            ModelState.AddModelError("LoginFail", "The user Id or password provided is incorrect.");
            return false;
        }

        public ActionResult LogOff()
        {
            WebSecurity.Logout();
            if (Request.IsAjaxRequest())
            {
                return JavaScript("document.location.replace('" + Url.Action("Login", "Account") + "');");
            }

            return RedirectToAction("Login", "Account");
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Dashboard");
        }
    }
}