using DaNangZ.CoreLib.Data;
using DaNangZ.CoreLib.Data.Entity;
using DaNangZ.DbFirst.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaNangZ.BusinessService.Business
{
    public class DashboardBusiness : IDashboardBusiness
    {
        private readonly IUnitOfWorkFactory<UnitOfWork> _unitOfWorkFactory;

        public DashboardBusiness(IUnitOfWorkFactory<UnitOfWork> unitOfWorkFactory)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
        }

        public int CountTotalCategory()
        {
            using (UnitOfWork uow = _unitOfWorkFactory.Create())
            {
                return uow.Repository<Category>().Where(status => status.StatusId.Equals(Constant.Constant.Active)).Count();
            }
        }

        public int CountTotalEntry()
        {
            using (UnitOfWork uow = _unitOfWorkFactory.Create())
            {
                return uow.Repository<Entry>().Where(status => status.StatusId.Equals(Constant.Constant.Active)).Count();
            }
        }

        public int CountTotalEntryWhereStatus(string actived)
        {
            using (UnitOfWork uow = _unitOfWorkFactory.Create())
            {
                return uow.Repository<Entry>().Where(status => status.StatusId.Equals(Constant.Constant.Active) && status.Actived.Equals(actived)).Count();
            }
        }
    }
}
