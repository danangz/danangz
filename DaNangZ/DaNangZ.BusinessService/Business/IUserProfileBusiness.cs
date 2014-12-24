using DaNangZ.BusinessService.Models;
using DaNangZ.DbFirst.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaNangZ.BusinessService.Business
{
    public interface IUserProfileBusiness
    {
        DNZCollectionModel<UserProfile> GetAll(string sidx, string sord, int pageNo, int pageSize);
        IList<UserProfile> GetAll();
        UserProfile Insert(UserProfile period);
        UserProfile Update(UserProfile period);
        UserProfile Details(int id);
        bool Delete(int id);
    }
}
