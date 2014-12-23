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

        public DaNangZService(ICategoryBusiness categoryBusiness)
        {
            _categoryBusiness = categoryBusiness;
        }

        public ICategoryBusiness Category
        {
            get { return _categoryBusiness; }
        }
    }
}
