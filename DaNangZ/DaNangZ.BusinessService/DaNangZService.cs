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
        private readonly IUserProfileBusiness _userProfileBusiness;
        private readonly IDashboardBusiness _dashboardBusiness;

        public DaNangZService(ICategoryBusiness categoryBusiness, IEntryBusiness entryBusiness, IUserProfileBusiness userProfileBusiness,
                                 IDashboardBusiness dashboardBusiness)
        {
            _categoryBusiness = categoryBusiness;
            _entryBusiness = entryBusiness;
            _userProfileBusiness = userProfileBusiness;
            _dashboardBusiness = dashboardBusiness;
        }

        public ICategoryBusiness Category
        {
            get { return _categoryBusiness; }
        }

        public IEntryBusiness Entry
        {
            get { return _entryBusiness; }
        }

        public IUserProfileBusiness UserProfile
        {
            get { return _userProfileBusiness; }
        }

        public IDashboardBusiness Dashboard
        {
            get { return _dashboardBusiness; }
        }
    }
}
