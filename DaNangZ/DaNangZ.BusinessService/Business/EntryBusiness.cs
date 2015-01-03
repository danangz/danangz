using DaNangZ.BusinessService.Models;
using DaNangZ.CoreLib.Data;
using DaNangZ.CoreLib.Data.Entity;
using DaNangZ.DbFirst.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace DaNangZ.BusinessService.Business
{
    public class EntryBusiness : IEntryBusiness
    {
        private readonly IUnitOfWorkFactory<UnitOfWork> _unitOfWorkFactory;

        public EntryBusiness(IUnitOfWorkFactory<UnitOfWork> unitOfWorkFactory)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
        }

        #region Implementation of IPeriodBusiness

        public DNZCollectionModel<Entry> GetAll(string sidx, string sord, int pageNo, int pageSize)
        {
            using (UnitOfWork uow = _unitOfWorkFactory.Create())
            {
                var list = uow.Repository<Entry>().Where(status => status.StatusId.Equals(Constant.Constant.Active))
               .OrderByDescending(x => x.Id)
               .Skip(pageSize * (pageNo - 1))
               .Take(pageSize);

                var totalRecords = uow.Repository<Entry>().Where(status => status.StatusId.Equals(Constant.Constant.Active)).Count();

                int totalPages = (totalRecords % pageSize) == 0 ? (totalRecords / pageSize) : (totalRecords / pageSize) + 1;

                return new DNZCollectionModel<Entry>
                {
                    Data = list.Include(x => x.Category).ToList(),
                    TotalPages = totalPages,
                    TotalRecords = totalRecords
                };
            }
        }

        public IList<Entry> GetAll()
        {
            using (UnitOfWork uow = _unitOfWorkFactory.Create())
            {
                List<Entry> Entrys = uow.Repository<Entry>().Where(status => status.StatusId.Equals(Constant.Constant.Active))
                                                            .OrderBy(x => x.Id).ToList();

                return Entrys;
            }
        }

        public Entry Insert(Entry entry)
        {
            try
            {
                using (UnitOfWork uow = _unitOfWorkFactory.Create())
                {
                    Entry insertedEntry = uow.Repository<Entry>().Add(entry);

                    uow.SaveChanges();

                    return insertedEntry;
                }
            }
            catch (Exception ex)
            {
                throw new DataLayerException(ex.Message);
            }
        }

        public Entry Update(Entry entry)
        {
            try
            {
                using (UnitOfWork uow = _unitOfWorkFactory.Create())
                {
                    Entry existingEntry = uow.Repository<Entry>().FirstOrDefault(o => o.Id == entry.Id);

                    if (existingEntry != null)
                    {
                        existingEntry.UpdBy = entry.UpdBy;
                        existingEntry.UpdAt = entry.UpdAt;

                        uow.SaveChanges();
                    }

                    return existingEntry;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Entry Details(int id)
        {
            try
            {
                using (UnitOfWork uow = _unitOfWorkFactory.Create())
                {
                    Entry entryResult =
                        uow.Repository<Entry>().FirstOrDefault(o => o.Id == id);

                    if (entryResult != null)
                    {
                        return entryResult;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new DataLayerException(ex.Message);
            }

            return null;
        }

        public bool Delete(int id)
        {
            try
            {
                using (UnitOfWork uow = _unitOfWorkFactory.Create())
                {
                    //Get existing Period
                    Entry entry =
                        uow.Repository<Entry>().FirstOrDefault(o => o.Id == id && o.StatusId.Equals(Constant.Constant.Active));

                    if (entry == null)
                    {
                        throw new DataLayerException("Entry is not found in the system");
                    }

                    //Delete
                    uow.Repository<Entry>().Remove(entry);

                    uow.SaveChanges();

                    return true;
                }
            }
            catch (Exception e)
            {
                throw new DataLayerException(e.Message);
            }
        }

        public bool ApproveEntry(Entry entry)
        {
            try
            {
                using (UnitOfWork uow = _unitOfWorkFactory.Create())
                {
                    Entry existingEntry = uow.Repository<Entry>().FirstOrDefault(o => o.Id == entry.Id);

                    if (existingEntry != null)
                    {
                        existingEntry.Actived = Constant.Constant.StatusIndicator.Completed;
                        existingEntry.UpdBy = entry.UpdBy;
                        existingEntry.UpdAt = entry.UpdAt;

                        uow.SaveChanges();
                    }

                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion
    }
}
