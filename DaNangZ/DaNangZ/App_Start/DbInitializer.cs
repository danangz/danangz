using DaNangZ.BusinessService.Data;
using DaNangZ.CoreLib.Data;
using DaNangZ.CoreLib.Data.Entity;
using DaNangZ.UserService.RoleService;
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
            var uowFactory = DependencyResolver.Current.GetService<IUnitOfWorkFactory<UnitOfWork>>();
            var dbSeeder = new DaNangZDatabaseSeeder(roleService, ensureLocalUserRegistered);
            dbSeeder.Seed(uowFactory.Create());
        }

        static Action<string, string, object> ensureLocalUserRegistered = (username, password, userProfile) =>
        {
            if (!WebSecurity.UserExists(username))
                WebSecurity.CreateUserAndAccount(username, password, userProfile);
        };
    }
}