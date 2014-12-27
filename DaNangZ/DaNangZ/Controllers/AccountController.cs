using DaNangZ.DbFirst.Model;
using DaNangZ.UserService.UserService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DaNangZ.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService<UserProfile, int> _userService;

        public AccountController(IUserService<UserProfile, int> userService)
        {
            _userService = userService;
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
    }
}