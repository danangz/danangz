using DaNangZ.CoreLib.Data;
using DaNangZ.UserService.RoleService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaNangZ.BusinessService.Data
{
    public class DaNangZDatabaseSeeder : IDatabaseSeeder<IUnitOfWork>
    {
        readonly IRoleService roleService;
        readonly Action<string, string, object> ensureLocalUserRegistered;

        public DaNangZDatabaseSeeder(IRoleService roleService, Action<string, string, object> ensureLocalUserRegistered)
        {
            this.roleService = roleService;
            this.ensureLocalUserRegistered = ensureLocalUserRegistered;
        }

        public void Seed(IUnitOfWork uow)
        {
            SeedUserAndRole(uow);
        }

        void SeedUserAndRole(IUnitOfWork uow)
        {
            //if (!roleService.IsRoleExists("SystemAdmin"))
            //    roleService.AddRole("SystemAdmin");

            //if (!roleService.IsRoleExists("ForcalPerson"))
            //    roleService.AddRole("ForcalPerson");
        }
    }
}
