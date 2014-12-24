using DaNangZ.BusinessService.Business;
using DaNangZ.CoreLib.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaNangZ.BusinessService
{
    public interface IDaNangZService : IService
    {
        ICategoryBusiness Category { get; }
        IEntryBusiness Entry { get; }
    }
}
