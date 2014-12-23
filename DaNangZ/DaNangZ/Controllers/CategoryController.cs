using DaNangZ.BusinessService;
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
            var cateList = _dnZService.Category.GetAll("", "asc", 1, Constant.PageSize);

            return View(cateList.Data.ToList());
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
                            UpdAt = cate.UpdAt,
                            UpdBy = cate.UpdBy
                        }).ToList()
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}