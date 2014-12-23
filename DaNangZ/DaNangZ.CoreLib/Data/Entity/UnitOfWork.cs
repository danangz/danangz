using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace DaNangZ.CoreLib.Data.Entity
{
    public class UnitOfWork : IUnitOfWork
    {
        readonly Dictionary<Type, IDbRepository> cachedRepositories = new Dictionary<Type, IDbRepository>();
        readonly DbContext dbContext;
        readonly Func<IPrincipal> getCurrentUser;

        public UnitOfWork(DbContext dbContext, Func<IPrincipal> getCurrentUser)
        {
            this.dbContext = dbContext;
            this.getCurrentUser = getCurrentUser;
        }

        public int SaveChanges()
        {
            // modify ins & upd fields
            // Update DateTime

            var changeSet = dbContext.ChangeTracker.Entries();

            if (changeSet != null)
            {
                DateTime currentDateTime = DateTime.Now;
                Type type;
                //TODO get current username
                //string currentUsername = getCurrentUser().Identity.Name;
                string currentUsername = "admin";

                foreach (var entry in changeSet.Where(c => c.State != EntityState.Unchanged))
                {
                    type = entry.Entity.GetType();

                    type.GetProperty(Constant.UpdAt).SetValue(entry.Entity, currentDateTime, null);
                    type.GetProperty(Constant.UpdBy).SetValue(entry.Entity, currentUsername, null);
                    
                    if (entry.State == EntityState.Added)
                    {
                        type.GetProperty(Constant.InsAt).SetValue(entry.Entity, currentDateTime, null);
                        type.GetProperty(Constant.InsBy).SetValue(entry.Entity, currentUsername, null);
                        type.GetProperty(Constant.StatusId).SetValue(entry.Entity, Constant.Active, null);
                    }
                }
            }

            return dbContext.SaveChanges();
        }

        public IDbRepository<TEntity> Repository<TEntity>() where TEntity : class
        {
            var type = typeof(TEntity);
            if (cachedRepositories.ContainsKey(type))
            {
                return cachedRepositories[type] as IDbRepository<TEntity>;
            }
            else
            {
                var repo = new DbRepository<TEntity>(dbContext.Set<TEntity>(), getCurrentUser);
                cachedRepositories[type] = repo;
                return repo;
            }
        }

        public DbContext GetDbContext()
        {
            return dbContext;
        }

        public void Dispose()
        {
            dbContext.Dispose();
        }
    }
}
