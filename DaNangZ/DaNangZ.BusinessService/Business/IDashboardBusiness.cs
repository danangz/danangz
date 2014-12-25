using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaNangZ.BusinessService.Business
{
    public interface IDashboardBusiness
    {
        int CountTotalCategory();

        int CountTotalEntry();

        int CountTotalEntryWhereStatus(string actived);
    }
}
