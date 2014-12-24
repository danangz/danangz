using DaNangZ.BusinessService;
using DaNangZ.Web.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DaNangZ.Web.Controllers
{
    public class UserProfileController : Controller
    {
        private IDaNangZService _dnZService = null;

        public UserProfileController(IDaNangZService dnZService)
        {
            this._dnZService = dnZService;
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
                            InsAt = user.InsAt,
                            InsBy = user.InsBy,
                            Action = string.Format("<input type='button' id='btnEdit' title='Sửa danh mục' onClick='Edit({0})' class='icon-edit'/>&nbsp&nbsp<input type='button' id='btnDel' title='Xóa danh mục' onClick='Delete({0})'  class='icon-delete'/>", user.UserId),
                        }).ToList()
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}