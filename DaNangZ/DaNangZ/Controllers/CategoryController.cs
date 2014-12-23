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
    public class CategoryController : Controller
    {
        private IDaNangZService _dnZService = null;

        public CategoryController(IDaNangZService dnZService)
        {
            this._dnZService = dnZService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetAll(string sidx, string sord, int page)
        {
            var cateList = _dnZService.Category.GetAll(sidx, sord, page, Constant.PageSize);

            var result = new
            {
                total = cateList.TotalPages,
                page = page,
                records = cateList.TotalRecords,
                rows = (from cate in cateList.Data
                        select new
                        {
                            Id = cate.Id,
                            CategoryName = cate.CategoryName,
                            InsAt = cate.InsAt,
                            InsBy = cate.InsBy,
                            UpdAt = cate.UpdAt,
                            UpdBy = cate.UpdBy,
                            Action = string.Format("<input type='button' id='btnEdit' title='Sửa danh mục' onClick='Edit({0}, \"{1}\")' class='icon-edit'/>&nbsp&nbsp<input type='button' id='btnDel' title='Xóa danh mục' onClick='Delete({0})'  class='icon-delete'/>", cate.Id, cate.CategoryName),
                        }).ToList()
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddCategory(string categoryName)
        {
            Category cat = new Category() { CategoryName = categoryName };
            var data = _dnZService.Category.Insert(cat);
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult EditCategory(int categoryId, string categoryNameOld, string categoryNameNew)
        {
            Category cat = new Category() { Id= categoryId, CategoryName = categoryNameNew };
            var data = _dnZService.Category.Update(cat);
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult DeleteCategory(int categoryId)
        {
            var cate = _dnZService.Category.Details(categoryId);

            if (cate != null)
            {
                var data = _dnZService.Category.Delete(categoryId);
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}