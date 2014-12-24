using DaNangZ.BusinessService;
using DaNangZ.DbFirst.Model;
using DaNangZ.Web.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DaNangZ.Web.Controllers
{
    public class EntryController : Controller
    {
        private IDaNangZService _dnZService = null;

        public EntryController(IDaNangZService dnZService)
        {
            this._dnZService = dnZService;
        }

        public ActionResult Index()
        {
            var cateList = _dnZService.Category.GetAll().ToList();

            List<SelectListItem> categories = new List<SelectListItem>();

            foreach(Category c in cateList)
            {
                categories.Add(new SelectListItem { Text = c.CategoryName, Value = c.Id.ToString() });
            }

            ViewBag.CategoryList = categories;

            List<SelectListItem> orderNumber = new List<SelectListItem>();

            for (int i = 1; i <= 10; i++)
            {
                orderNumber.Add(new SelectListItem { Text = i.ToString(), Value = i.ToString() });
            }

            ViewBag.OrderNumber = orderNumber;

            return View();
        }

        public JsonResult GetAll(string sidx, string sord, int page)
        {
            var entList = _dnZService.Entry.GetAll(sidx, sord, page, Constant.PageSize);

            var result = new
            {
                total = entList.TotalPages,
                page = page,
                records = entList.TotalRecords,
                rows = (from ent in entList.Data
                        select new
                        {
                            Id = ent.Id,
                            CategoryId = ent.Category.CategoryName,
                            EntrySubject = ent.EntrySubject,
                            Summarize = ent.Summarize,
                            Actived = GetIndicator(ent.Actived),
                            InsAt = ent.InsAt,
                            InsBy = ent.InsBy,
                            UpdAt = ent.UpdAt,
                            UpdBy = ent.UpdBy,
                            Action = string.Format("<input type='button' id='btnEdit' title='Sửa danh mục' onClick='Edit({0})' class='icon-edit'/>&nbsp&nbsp<input type='button' id='btnDel' title='Xóa danh mục' onClick='Delete({0})'  class='icon-delete'/>", ent.Id),
                        }).ToList()
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        private string GetIndicator(string status)
        {
            if (status.Equals(Constant.StatusIndicator.Completed))
            {
                return string.Format("<img src='../Images/icon/green.png' alt='Completed' height='16' width='16'>");
            }
            else if (status.Equals(Constant.StatusIndicator.InProgress))
            {
                return string.Format("<img src='../Images/icon/blue.png' alt='In Progress' height='16' width='16'>");
            }
            else if (status.Equals(Constant.StatusIndicator.Overdue))
            {
                return string.Format("<img src='../Images/icon/orange.png' alt='Overdue' height='16' width='16'>");
            }
            else
            {
                return string.Empty;
            }
        }
    }
}