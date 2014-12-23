using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaNangZ.CoreLib.Data
{
    public interface IUnitOfWork : IDisposable
    {
        //
        // Summary:
        //     Saves all changes made in this context to the underlying database.
        //
        // Returns:
        //     The number of objects written to the underlying database.
        //
        // Exceptions:
        //   System.InvalidOperationException:
        //     Thrown if the context has been disposed.
        int SaveChanges();
        //
        // Summary:
        //     Returns a DbSet instance for access to entities of the given type in the
        //     context, the ObjectStateManager, and the underlying store.
        //
        // Type parameters:
        //   TEntity:
        //     The type entity for which a set should be returned.
        //
        // Returns:
        //     A set for the given entity type.
        //
        // Remarks:
        //     See the DbSet class for more details.
        IDbRepository<TEntity> Repository<TEntity>() where TEntity : class;
    }
}
