using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaNangZ.CoreLib.Data
{
    public interface IDatabaseSeeder<TUnitOfWork>
       where TUnitOfWork : IUnitOfWork
    {
        void Seed(TUnitOfWork uow);
    }
}
