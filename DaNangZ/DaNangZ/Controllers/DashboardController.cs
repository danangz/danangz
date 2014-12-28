using DaNangZ.BusinessService;
using DaNangZ.DbFirst.Model;
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
    public class DashboardController : Controller
    {
        private IDaNangZService _dnZService = null;

        public DashboardController(IDaNangZService dnZService)
        {
            this._dnZService = dnZService;
        }

        public ActionResult Index()
        {
            UserProfile usp = _dnZService.UserProfile.Details(WebSecurity.GetUserId(Membership.GetUser().UserName));

            Session["DisplayNameSession"] = usp.DisplayName;

            ViewBag.TotalCategory = _dnZService.Dashboard.CountTotalCategory().ToString();
            ViewBag.TotalEntry = _dnZService.Dashboard.CountTotalEntry().ToString();
            ViewBag.TotalEntryCompleted = _dnZService.Dashboard.CountTotalEntryWhereStatus(Constant.StatusIndicator.Completed).ToString();
            ViewBag.TotalEntryInProgress = _dnZService.Dashboard.CountTotalEntryWhereStatus(Constant.StatusIndicator.InProgress).ToString();
            ViewBag.TotalEntryOverdue = _dnZService.Dashboard.CountTotalEntryWhereStatus(Constant.StatusIndicator.Overdue).ToString();

            return View();
        }


    }
}