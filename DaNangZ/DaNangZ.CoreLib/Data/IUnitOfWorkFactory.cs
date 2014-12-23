using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaNangZ.CoreLib.Data
{
    public interface IUnitOfWorkFactory<out TUnitOfWork> where TUnitOfWork : IUnitOfWork
    {
        /// <summary>
        /// Create the database based on the schema defined in the POCO class and optionally seed some data
        /// </summary>
        /// <param name="dropIfExist"></param>
        /// <param name="seedData"></param>
        void CreateDatabase(bool dropIfExist, bool seedData);

        void DeleteDatabase();

        TUnitOfWork Create();
    }
}
