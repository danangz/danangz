using DaNangZ.BusinessService.Models;
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
    public class UserProfileBusiness : IUserProfileBusiness
    {
        private readonly IUnitOfWorkFactory<UnitOfWork> _unitOfWorkFactory;

        public UserProfileBusiness(IUnitOfWorkFactory<UnitOfWork> unitOfWorkFactory)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
        }

        #region Implementation of IPeriodBusiness

        public DNZCollectionModel<UserProfile> GetAll(string sidx, string sord, int pageNo, int pageSize)
        {
            using (UnitOfWork uow = _unitOfWorkFactory.Create())
            {
                var list = uow.Repository<UserProfile>().Where(status => status.StatusId.Equals(Constant.Constant.Active))
               .OrderByDescending(x => x.UserId)
               .Skip(pageSize * (pageNo - 1))
               .Take(pageSize);

                var totalRecords = uow.Repository<UserProfile>().Where(status => status.StatusId.Equals(Constant.Constant.Active)).Count();

                int totalPages = (totalRecords % pageSize) == 0 ? (totalRecords / pageSize) : (totalRecords / pageSize) + 1;

                return new DNZCollectionModel<UserProfile>
                {
                    Data = list.ToList(),
                    TotalPages = totalPages,
                    TotalRecords = totalRecords
                };
            }
        }

        public IList<UserProfile> GetAll()
        {
            using (UnitOfWork uow = _unitOfWorkFactory.Create())
            {
                List<UserProfile> UserProfiles = uow.Repository<UserProfile>().Where(status => status.StatusId.Equals(Constant.Constant.Active))
                                                            .OrderBy(x => x.UserId).ToList();

                return UserProfiles;
            }
        }

        public UserProfile Insert(UserProfile userProfile, string password)
        {
            try
            {
                using (UnitOfWork uow = _unitOfWorkFactory.Create())
                {
                    UserProfile insertedUserProfile = uow.Repository<UserProfile>().Add(userProfile);

                    uow.SaveChanges();

                    webpages_Membership membership = new webpages_Membership() { UserId = insertedUserProfile.UserId, Password = password };
                    webpages_Membership insertedMembership = uow.Repository<webpages_Membership>().Add(membership);

                    uow.SaveChanges();

                    return insertedUserProfile;
                }
            }
            catch (Exception ex)
            {
                throw new DataLayerException(ex.Message);
            }
        }

        public UserProfile Update(UserProfile userProfile)
        {
            try
            {
                using (UnitOfWork uow = _unitOfWorkFactory.Create())
                {
                    UserProfile existingUserProfile = uow.Repository<UserProfile>().FirstOrDefault(o => o.UserId == userProfile.UserId);

                    if (existingUserProfile != null)
                    {
                        existingUserProfile.UpdBy = userProfile.UpdBy;
                        existingUserProfile.UpdAt = userProfile.UpdAt;

                        uow.SaveChanges();
                    }

                    return existingUserProfile;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public UserProfile Details(int id)
        {
            try
            {
                using (UnitOfWork uow = _unitOfWorkFactory.Create())
                {
                    UserProfile userProfileResult =
                        uow.Repository<UserProfile>().FirstOrDefault(o => o.UserId == id);

                    if (userProfileResult != null)
                    {
                        return userProfileResult;
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
                    UserProfile userProfile =
                        uow.Repository<UserProfile>().FirstOrDefault(o => o.UserId == id && o.StatusId.Equals(Constant.Constant.Active));

                    if (userProfile == null)
                    {
                        throw new DataLayerException("UserProfile is not found in the system");
                    }

                    //Delete
                    uow.Repository<UserProfile>().Remove(userProfile);

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
