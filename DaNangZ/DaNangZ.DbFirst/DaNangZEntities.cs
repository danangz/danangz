using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DaNangZ.DbFirst.Model
{
    public partial class DaNangZEntities : IDaNangZEntities
    {
        public DaNangZEntities(string connectionString)
            : base("name=" + connectionString)
        {
        }
    }
}
