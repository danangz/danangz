using Autofac;
using Autofac.Integration.Mvc;
using DaNangZ.BusinessService;
using DaNangZ.CoreLib.Data;
using DaNangZ.CoreLib.Data.Entity;
using DaNangZ.DbFirst;
using DaNangZ.DbFirst.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;

namespace DaNangZ.Web.App_Start
{
    public class IocConfig
    {
        public static void Config()
        {
            var builder = new ContainerBuilder();
            builder.RegisterControllers(Assembly.GetExecutingAssembly());
            builder.RegisterControllers(typeof(MvcApplication).Assembly);

            builder.Register<Func<IPrincipal>>(c => () => HttpContext.Current.User);

            builder.RegisterType(typeof(UnitOfWorkFactory<UnitOfWork, DaNangZEntities>)).As(
                typeof(IUnitOfWorkFactory<UnitOfWork>))
                .SingleInstance().WithParameter("connectionStringName", "DaNangZEntities");

            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                .Where(t => t.Name.EndsWith("Business"))
                .AsImplementedInterfaces();

            builder.RegisterAssemblyTypes(typeof(IDaNangZService).Assembly).AsImplementedInterfaces();

            //builder.RegisterAssemblyTypes(typeof(IRoleService).Assembly)
            //   .Where(t => t.Name.EndsWith("Service"))
            //   .AsImplementedInterfaces();

            //builder.RegisterType(typeof(SimpleMembershipUserService<UserProfile>))
            //       .As(typeof(IUserService<UserProfile, int>)).SingleInstance().PreserveExistingDefaults();
            //builder.RegisterAssemblyTypes(typeof(IUserService<IUserProfile<int>, int>).Assembly).AsImplementedInterfaces();
            //builder.RegisterAssemblyTypes(typeof(IEmailService).Assembly).AsImplementedInterfaces();

            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}