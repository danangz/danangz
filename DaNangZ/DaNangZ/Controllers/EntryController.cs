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
    [Authorize(Roles = "System Admin")]
    public class EntryController : Controller
    {
        private IDaNangZService _dnZService = null;

        public EntryController(IDaNangZService dnZService)
        {
            this._dnZService = dnZService;
        }

        [Authorize(Roles = "System Admin")]
        public ActionResult Index()
        {
            var cateList = _dnZService.Category.GetAll().ToList();

            List<SelectListItem> categories = new List<SelectListItem>();

            foreach (Category c in cateList)
            {
                categories.Add(new SelectListItem { Text = c.CategoryName, Value = c.Id.ToString() });
            }

            ViewBag.CategoryList = categories;

            List<SelectListItem> activedList = new List<SelectListItem>();

            activedList.Add(new SelectListItem { Text = "Đang đăng", Value = Constant.StatusIndicator.Completed });
            activedList.Add(new SelectListItem { Text = "Chưa được đăng", Value = Constant.StatusIndicator.InProgress });
            activedList.Add(new SelectListItem { Text = "Đã được đăng", Value = Constant.StatusIndicator.Overdue });

            ViewBag.ActivedList = activedList;

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
                            Action = string.Format("<input type='button' id='btnView' title='Xem bài viết' onClick='View({0})' class='icon-view'/>&nbsp&nbsp<input type='button' id='btnEdit' title='Sửa bài viết' onClick='Edit({0})' class='icon-edit'/>&nbsp&nbsp<input type='button' id='btnDel' title='Xóa bài viết' onClick='Delete({0})'  class='icon-delete'/>", ent.Id),
                        }).ToList()
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ViewEntry(string entryId)
        {
            Entry ent = _dnZService.Entry.Details(Convert.ToInt32(entryId));

            var result = new
            {
                Id = ent.Id,
                AvatarLink = ent.AvatarLink,
                Actived = ent.Actived,
                EntrySubject = ent.EntrySubject,
                Summarize = ent.Summarize,
                Content = ent.EntryContent,
                InsAt = ent.InsAt
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

        public ActionResult AddEntryPage()
        {
            var cateList = _dnZService.Category.GetAll().ToList();

            List<SelectListItem> categories = new List<SelectListItem>();

            foreach (Category c in cateList)
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

            return View("AddEntry");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public JsonResult AddEntry(string categoryId, string avatarLink, string actived, string orderNumber, string entrySubject, string entryContent, string summarize)
        {
            if (string.IsNullOrEmpty(categoryId))
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
            if (actived.Equals("true"))
            {
                actived = Constant.StatusIndicator.Completed;
            }
            else
            {
                actived = Constant.StatusIndicator.InProgress;
            }
            if (string.IsNullOrEmpty(orderNumber))
            {
                orderNumber = "0";
            }

            Entry entry = new Entry()
            {
                CategoryId = Convert.ToInt32(categoryId),
                AvatarLink = avatarLink,
                OrderInHome = Convert.ToInt32(orderNumber),
                EntrySubject = entrySubject,
                EntryContent = entryContent,
                Summarize = summarize,
                Actived = actived
            };
            var data = _dnZService.Entry.Insert(entry);
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult DeleteEntry(int entryId)
        {
            var entry = _dnZService.Entry.Details(entryId);

            if (entry != null)
            {
                var data = _dnZService.Entry.Delete(entryId);
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult ApproveEntry(int entryId)
        {
            bool result = _dnZService.Entry.ApproveEntry(new Entry() { Id = entryId });

            if (result)
            {
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
