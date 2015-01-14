using DaNangZ.CoreLib.Data.Entity;
using DaNangZ.UserService.RoleService;
using DaNangZ.Web.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMatrix.WebData;

namespace DaNangZ.Web.App_Start
{
    public class DbInitializer
    {
        public static void Initialize()
        {
            WebMatrix.WebData.WebSecurity.InitializeDatabaseConnection("DefaultConnection", "UserProfile", "UserId", "UserName", autoCreateTables: true);

            var roleService = DependencyResolver.Current.GetService<IRoleService>();
            var uowFactory = DependencyResolver.Current.GetService<DaNangZ.CoreLib.Data.IUnitOfWorkFactory<UnitOfWork>>();

            string username = "admin";
            string password = "danangZ@123$";

            if (roleService.GetAllRoles().ToList().Count == 0)
            {
                roleService.AddRole(Constant.Role.SystemAdmin);
                roleService.AddRole(Constant.Role.EntryAdmin);
                roleService.AddRole(Constant.Role.PushAdmin);
                roleService.AddRole(Constant.Role.ViewAdmin);
            }

            if (!WebSecurity.UserExists(username))
            {
                WebSecurity.CreateUserAndAccount(username, password, new { UserName = username, DisplayName = username, Email = "admin@danangz.com", ReceiveEmail = true, StatusId = Constant.Active, UpdBy = username, UpdAt = DateTime.Now, InsBy = "system", InsAt = DateTime.Now }, false);
                roleService.AddUserToRole(username, Constant.Role.SystemAdmin);
            }
        }
    }
}