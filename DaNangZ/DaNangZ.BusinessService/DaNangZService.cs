using DaNangZ.BusinessService.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaNangZ.BusinessService
{
    public class DaNangZService : IDaNangZService
    {
        private readonly ICategoryBusiness _categoryBusiness;
        private readonly IEntryBusiness _entryBusiness;

        public DaNangZService(ICategoryBusiness categoryBusiness, IEntryBusiness entryBusiness)
        {
            _categoryBusiness = categoryBusiness;
            _entryBusiness = entryBusiness;
        }

        public ICategoryBusiness Category
        {
            get { return _categoryBusiness; }
        }

        public IEntryBusiness Entry
        {
            get { return _entryBusiness; }
        }
    }
}
