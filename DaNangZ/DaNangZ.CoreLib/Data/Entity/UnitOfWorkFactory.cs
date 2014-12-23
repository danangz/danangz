using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace DaNangZ.CoreLib.Data.Entity
{
    public class UnitOfWorkFactory<TUnitOfWork, TDbContext> : IUnitOfWorkFactory<TUnitOfWork>
        where TUnitOfWork : UnitOfWork, IUnitOfWork
        where TDbContext : DbContext
    {
        readonly string connectionStringName;
        readonly Func<IPrincipal> getCurrentUser;

        public UnitOfWorkFactory(string connectionStringName, Func<IPrincipal> getCurrentUser)
        {
            this.connectionStringName = connectionStringName;
            this.getCurrentUser = getCurrentUser;
        }

        public void CreateDatabase(bool dropIfExist, bool seedData)
        {
            throw new NotImplementedException();
        }

        public void DeleteDatabase()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Create an instance of unit of work. Override this function if the derived TUnitOfWork or TDbContext have different constructor signature.
        /// </summary>
        /// <returns></returns>
        public virtual TUnitOfWork Create()
        {
            var ctx = Activator.CreateInstance(typeof(TDbContext), connectionStringName) as TDbContext;
            return Activator.CreateInstance(typeof(TUnitOfWork), ctx, getCurrentUser) as TUnitOfWork;
        }
    }
}
