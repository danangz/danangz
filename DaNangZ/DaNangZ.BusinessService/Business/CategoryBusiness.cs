using DaNangZ.BusinessService.Models;
using DaNangZ.CoreLib.Data;
using DaNangZ.CoreLib.Data.Entity;
using DaNangZ.DbFirst;
using DaNangZ.DbFirst.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaNangZ.BusinessService.Business
{
    public class CategoryBusiness : ICategoryBusiness
    {
        private readonly IUnitOfWorkFactory<UnitOfWork> _unitOfWorkFactory;

        public CategoryBusiness(IUnitOfWorkFactory<UnitOfWork> unitOfWorkFactory)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
        }

        #region Implementation of IPeriodBusiness

        public DNZCollectionModel<Category> GetAll(string sidx, string sord, int pageNo, int pageSize)
        {
            using (UnitOfWork uow = _unitOfWorkFactory.Create())
            {
                IList<Category> list = null;

                if (sidx.Equals("CategoryName"))
                {
                    if (sord.Equals("asc"))
                    {
                        list = uow.Repository<Category>().Where(status => status.StatusId.Equals(Constant.Constant.Active))
                                    .OrderBy(x => x.CategoryName)
                                    .Skip(pageSize * (pageNo - 1))
                                    .Take(pageSize)
                                    .ToList();
                    }
                    else
                    {
                        list = uow.Repository<Category>().Where(status => status.StatusId.Equals(Constant.Constant.Active))
                                     .OrderByDescending(x => x.CategoryName)
                                     .Skip(pageSize * (pageNo - 1))
                                     .Take(pageSize)
                                     .ToList();
                    }
                }
                else if (sidx.Equals("UpdAt"))
                {
                    if (sord.Equals("asc"))
                    {
                        list = uow.Repository<Category>().Where(status => status.StatusId.Equals(Constant.Constant.Active))
                                     .OrderBy(x => x.UpdAt)
                                     .Skip(pageSize * (pageNo - 1))
                                     .Take(pageSize)
                                     .ToList();
                    }
                    else
                    {
                        list = uow.Repository<Category>().Where(status => status.StatusId.Equals(Constant.Constant.Active))
                                     .OrderByDescending(x => x.UpdAt)
                                     .Skip(pageSize * (pageNo - 1))
                                     .Take(pageSize)
                                     .ToList();
                    }
                }
                else if (sidx.Equals("UpdBy"))
                {
                    if (sord.Equals("asc"))
                    {
                        list = uow.Repository<Category>().Where(status => status.StatusId.Equals(Constant.Constant.Active))
                                     .OrderBy(x => x.UpdBy)
                                     .Skip(pageSize * (pageNo - 1))
                                     .Take(pageSize)
                                     .ToList();
                    }
                    else
                    {
                        list = uow.Repository<Category>().Where(status => status.StatusId.Equals(Constant.Constant.Active))
                                     .OrderByDescending(x => x.UpdBy)
                                     .Skip(pageSize * (pageNo - 1))
                                     .Take(pageSize)
                                     .ToList();
                    }
                }
                else
                {
                    list = uow.Repository<Category>().Where(status => status.StatusId.Equals(Constant.Constant.Active))
                   .OrderBy(x => x.CategoryName)
                   .Skip(pageSize * (pageNo - 1))
                   .Take(pageSize)
                   .ToList();
                }

                var totalRecords = uow.Repository<Category>().Where(status => status.StatusId.Equals(Constant.Constant.Active)).Count();

                int totalPages = (totalRecords % pageSize) == 0 ? (totalRecords / pageSize) : (totalRecords / pageSize) + 1;

                return new DNZCollectionModel<Category>
                {
                    Data = list,
                    TotalPages = totalPages,
                    TotalRecords = totalRecords
                };
            }
        }

        public IList<Category> GetAll()
        {
            using (UnitOfWork uow = _unitOfWorkFactory.Create())
            {
                List<Category> Categorys = uow.Repository<Category>().Where(status => status.StatusId.Equals(Constant.Constant.Active))
                                                            .OrderBy(x => x.CategoryName).ToList();

                return Categorys;
            }
        }

        public Category Insert(Category Category)
        {
            try
            {
                using (UnitOfWork uow = _unitOfWorkFactory.Create())
                {
                    Category insertedCategory = uow.Repository<Category>().Add(Category);

                    uow.SaveChanges();

                    return insertedCategory;
                }
            }
            catch (Exception ex)
            {
                throw new DataLayerException(ex.Message);
            }
        }

        public Category Update(Category Category)
        {
            try
            {
                using (UnitOfWork uow = _unitOfWorkFactory.Create())
                {
                    Category existingCategory = uow.Repository<Category>().FirstOrDefault(o => o.Id == Category.Id);

                    if (existingCategory != null)
                    {
                        existingCategory.CategoryName = Category.CategoryName;
                        existingCategory.UpdBy = Category.UpdBy;
                        existingCategory.UpdAt = Category.UpdAt;

                        uow.SaveChanges();
                    }

                    return existingCategory;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Category Details(int id)
        {
            try
            {
                using (UnitOfWork uow = _unitOfWorkFactory.Create())
                {
                    Category CategoryResult =
                        uow.Repository<Category>().FirstOrDefault(o => o.Id == id);

                    if (CategoryResult != null)
                    {
                        return CategoryResult;
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
                    Category Category =
                        uow.Repository<Category>().FirstOrDefault(o => o.Id == id && o.StatusId.Equals(Constant.Constant.Active));

                    if (Category == null)
                    {
                        throw new DataLayerException("Category is not found in the system");
                    }

                    //Delete
                    uow.Repository<Category>().Remove(Category);

                    uow.SaveChanges();

                    return true;
                }
            }
            catch (Exception e)
            {
                throw new DataLayerException(e.Message);
            }
        }

        #endregion
    }
}
