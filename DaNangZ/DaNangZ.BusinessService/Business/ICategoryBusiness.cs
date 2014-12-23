using DaNangZ.BusinessService.Models;
using DaNangZ.DbFirst;
using DaNangZ.DbFirst.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaNangZ.BusinessService.Business
{
    public interface ICategoryBusiness
    {
        DNZCollectionModel<Category> GetAll(string sidx, string sord, int pageNo, int pageSize);
        IList<Category> GetAll();
        Category Insert(Category period);
        Category Update(Category period);
        Category Details(int id);
        bool Delete(int id);
    }
}
