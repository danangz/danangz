using DaNangZ.BusinessService.Models;
using DaNangZ.DbFirst.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaNangZ.BusinessService.Business
{
    public interface IEntryBusiness
    {
        DNZCollectionModel<Entry> GetAll(string sidx, string sord, int pageNo, int pageSize);
        IList<Entry> GetAll();
        Entry Insert(Entry period);
        Entry Update(Entry period);
        Entry Details(int id);
        bool Delete(int id);
        bool ApproveEntry(Entry entry);
    }
}
