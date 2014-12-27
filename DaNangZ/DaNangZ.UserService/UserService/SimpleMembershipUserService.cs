using DaNangZ.CoreLib.Data;
using DaNangZ.CoreLib.Data.Entity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics.CodeAnalysis;
using System.DirectoryServices;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using WebMatrix.WebData;

namespace DaNangZ.UserService.UserService
{
    public class SimpleMembershipUserService<TUserProfile> : IUserService<TUserProfile, int>
        where TUserProfile : class, IUserProfile<int>
    {
        private readonly ConnectionStringSettings _ldapdomain = ConfigurationManager.ConnectionStrings["DefaultConnection"];
        private readonly IUnitOfWorkFactory<UnitOfWork> _unitOfWorkFactory;

        public SimpleMembershipUserService(IUnitOfWorkFactory<UnitOfWork> unitOfWorkFactory)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
        }

        [DirectoryServicesPermission(System.Security.Permissions.SecurityAction.Demand, Unrestricted = true)]
        [SuppressMessage("Microsoft.Reliability", "CA2000:DisposeObjectsBeforeLosingScope", Justification = "Returns disposable object for other methods.")]
        protected virtual DirectoryEntry OpenEntry()
        {
            var entry = new DirectoryEntry(_ldapdomain.ToString());
            var un = DaNangZ.CoreLib.Utilities.Utils.GetSetting("ldap-username", string.Empty);

            if (!string.IsNullOrEmpty(un))
            {
                entry.Username = un;
                entry.Password = DaNangZ.CoreLib.Utilities.Utils.GetSetting("ldap-password", string.Empty); ;
            }

            return entry;
        }

        [DirectoryServicesPermission(System.Security.Permissions.SecurityAction.Demand, Unrestricted = true)]
        public IList<TUserProfile> Search(string term, int limit, bool userOnly)
        {
            if (string.IsNullOrEmpty(term)) return new List<TUserProfile>();

            using (var entry = OpenEntry())
            {
                using (var mySearcher = new DirectorySearcher(entry))
                {

                    const string pattern = "['\"]";
                    var isExactly = true;
                    var resultTermCheck = Regex.Replace(term, pattern, ";##");
                    var resultTerm = Regex.Replace(term, pattern, string.Empty);
                    var isEmail = CheckEmailFormat(resultTerm);


                    if (!string.IsNullOrEmpty(resultTermCheck))
                    {
                        isExactly = resultTermCheck.Contains(";##");
                    }

                    if (isExactly)
                    {
                        if (!isEmail)
                        {
                            mySearcher.Filter = string.Format(userOnly ? "(&(|(sAMAccountName={0})(displayName={0})(objectCategory=User))(!(userAccountControl:1.2.840.113556.1.4.803:=2)))"
                                : "(&(|(sAMAccountName={0})(displayName={0}))(|(objectCategory=User)(objectCategory=Group))(!(userAccountControl:1.2.840.113556.1.4.803:=2)))", resultTerm);
                        }
                        else
                        {
                            mySearcher.Filter = string.Format("(&(mail={0})(|(objectCategory=User)(objectCategory=Group))(!(userAccountControl:1.2.840.113556.1.4.803:=2)))", resultTerm);
                        }
                    }
                    else
                    {
                        if (!isEmail)
                        {
                            mySearcher.Filter = string.Format(userOnly ? "(&(|(sAMAccountName=*{0}*)(displayName=*{0}*))(objectCategory=User)(!(userAccountControl:1.2.840.113556.1.4.803:=2)))"
                                : "(&(|(sAMAccountName=*{0}*)(displayName=*{0}*))(|(objectCategory=User)(objectCategory=Group))(!(userAccountControl:1.2.840.113556.1.4.803:=2)))", term);
                        }
                        else
                        {
                            mySearcher.Filter = string.Format("(&(mail={0})(|(objectCategory=User)(objectCategory=Group))(!(userAccountControl:1.2.840.113556.1.4.803:=2)))", term);
                        }
                    }
                    mySearcher.PropertiesToLoad.Add("sAMAccountName"); // Name
                    mySearcher.PropertiesToLoad.Add("displayName"); // DisplayName                
                    mySearcher.PropertiesToLoad.Add("mail"); // Email 
                    mySearcher.PageSize = limit;
                    mySearcher.Sort = new SortOption("displayName", SortDirection.Ascending);

                    var result = mySearcher.FindAll();
                    var final = (from SearchResult r in result
                                 where
                                    r.Properties.Contains("mail")
                                 let email = Convert.ToString(r.Properties["mail"][0])
                                 where
                                       !string.IsNullOrEmpty(email)
                                 select ToUser(r)).ToList();
                    return final;
                }
            }
        }

        public virtual bool IsAuthenticated()
        {
            return WebSecurity.IsAuthenticated;
        }

        public virtual int GetCurrentUserId()
        {
            return WebSecurity.CurrentUserId;
        }

        public virtual TUserProfile GetCurrentUser()
        {
            var uow = _unitOfWorkFactory.Create();

            var user = uow.Repository<TUserProfile>().FirstOrDefault();

            return user;
        }

        public virtual TUserProfile GetUserById(int id)
        {
            var user =
                _unitOfWorkFactory.Create().Repository<TUserProfile>()
                            .FirstOrDefault(x => x.UserId.Equals(id) && x.StatusId.Equals(Constant.Active));
            return user;
        }

        public virtual string GetNameById(int id)
        {
            var user = GetUserById(id);
            if (user == null) throw new UserAndRoleServiceException(string.Format("User with id '{0}' is not found.", id));
            return user.DisplayName;
        }

        public virtual string GetUsernameById(int id)
        {
            var user = GetUserById(id);
            if (user == null) throw new UserAndRoleServiceException(string.Format("User with id '{0}' is not found.", id));
            return user.UserName;
        }

        public virtual int GetUserIdByUsername(string username)
        {
            return WebSecurity.GetUserId(username);
        }

        public virtual void AddUser(TUserProfile userProfile)
        {
            userProfile.ReceiveEmail = true;

            using (var uow = _unitOfWorkFactory.Create())
            {
                TUserProfile userInserted = uow.Repository<TUserProfile>().Add(userProfile);

                uow.SaveChanges();
            }
        }

        public virtual TUserProfile UpdateUser(TUserProfile userProfile)
        {
            using (var uow = _unitOfWorkFactory.Create())
            {
                var existing = uow.Repository<TUserProfile>().FirstOrDefault(i => i.UserId == userProfile.UserId);

                if (existing != null)
                {
                    existing.UserName = userProfile.UserName;
                    existing.DisplayName = userProfile.DisplayName;
                    existing.Email = userProfile.Email;
                    uow.SaveChanges();
                }
            }

            return userProfile;
        }

        public virtual void RemoveUser(int userId)
        {
            throw new NotImplementedException();
        }

        public IList<string> GetUserNameByComponentId(int componentId)
        {
            throw new NotImplementedException();
        }

        public IList<string> GetEmails(List<string> username)
        {
            using (UnitOfWork unitOfWork = _unitOfWorkFactory.Create())
            {
                var users = unitOfWork.Repository<TUserProfile>().Where(x => username.Contains(x.UserName)).ToList();
                return users.Select(userProfile => userProfile.Email).ToList();
            }
        }

        public IList<TUserProfile> GetUsers(int pageNo, int pageSize, out int totalRecords)
        {
            try
            {
                using (UnitOfWork uow = _unitOfWorkFactory.Create())
                {
                    var list = _unitOfWorkFactory.Create().Repository<TUserProfile>()
                        .Where(i => i.StatusId.Equals(Constant.Active))
                        .OrderByDescending(x => x.DisplayName)
                        .Skip(pageSize * (pageNo - 1))
                        .Take(pageSize)
                        .ToList();

                    totalRecords = uow.Repository<TUserProfile>().Count(i => i.StatusId.Equals(Constant.Active));

                    return list;
                }
            }
            catch (Exception ex)
            {
                throw new DataLayerException(ex.Message, ex);
            }
        }

        public IList<TUserProfile> GetAllUsersInMembership(string userName)
        {
            try
            {
                using (UnitOfWork uow = _unitOfWorkFactory.Create())
                {
                    var list = _unitOfWorkFactory.Create().Repository<TUserProfile>()
                        .Where(i => i.StatusId.Equals(Constant.Active) && (i.DisplayName.Contains(userName) || i.Email.Contains(userName) || i.UserName.Contains(userName)))
                        .OrderByDescending(x => x.DisplayName)
                        .ToList();

                    return list;
                }
            }
            catch (Exception ex)
            {
                throw new DataLayerException(ex.Message, ex);
            }
        }

        public TUserProfile GetUserByUserName(string userName)
        {
            var user =
                _unitOfWorkFactory.Create().Repository<TUserProfile>()
                            .FirstOrDefault(x => x.UserName.Equals(userName) && x.StatusId == Constant.Active);
            return user;
        }

        [DirectoryServicesPermission(System.Security.Permissions.SecurityAction.Demand, Unrestricted = true)]
        public TUserProfile SearchExactAd(string user)
        {
            if (string.IsNullOrEmpty(user)) return null;
            if (user.Contains("\\"))
            {
                user = user.Remove(0, user.LastIndexOf("\\", StringComparison.Ordinal) + 1);
            }

            using (var entry = OpenEntry())
            {
                using (var mySearcher = new DirectorySearcher(entry))
                {
                    var u = Activator.CreateInstance<TUserProfile>();
                    if (!CheckEmailFormat(user)) //Not email
                    {
                        mySearcher.Filter = string.Format("(&(sAMAccountName={0})(|(objectCategory=User)(objectCategory=Group))(!(userAccountControl:1.2.840.113556.1.4.803:=2)))", user);

                        mySearcher.PropertiesToLoad.Add("givenName"); // Name
                        mySearcher.PropertiesToLoad.Add("sAMAccountName"); // Name
                        mySearcher.PropertiesToLoad.Add("displayName"); // DisplayName                
                        mySearcher.PropertiesToLoad.Add("mail"); // Email 
                        mySearcher.PropertiesToLoad.Add("objectGUID"); // GUID
                        mySearcher.PropertiesToLoad.Add("objectSid"); // GUID
                        mySearcher.PropertiesToLoad.Add("objectCategory"); // Group or User
                        mySearcher.PropertiesToLoad.Add("memberOf");
                        mySearcher.PropertiesToLoad.Add("distinguishedName");
                        mySearcher.PropertiesToLoad.Add("telephoneNumber");
                        mySearcher.PropertiesToLoad.Add("mobile");
                        mySearcher.PropertiesToLoad.Add("company");
                        var r = mySearcher.FindOne();

                        u = r == null ? null : ToUser(r);

                    }
                    else
                    {
                        mySearcher.Filter = string.Format("(&(mail={0})(|(objectCategory=User)(objectCategory=Group))(!(userAccountControl:1.2.840.113556.1.4.803:=2)))", user);

                        mySearcher.PropertiesToLoad.Add("givenName"); // Name
                        mySearcher.PropertiesToLoad.Add("sAMAccountName"); // Name
                        mySearcher.PropertiesToLoad.Add("displayName"); // DisplayName                
                        mySearcher.PropertiesToLoad.Add("mail"); // Email 
                        mySearcher.PropertiesToLoad.Add("objectGUID"); // GUID
                        mySearcher.PropertiesToLoad.Add("objectSid"); // GUID
                        mySearcher.PropertiesToLoad.Add("objectCategory"); // Group or User
                        mySearcher.PropertiesToLoad.Add("memberOf");
                        mySearcher.PropertiesToLoad.Add("distinguishedName");
                        mySearcher.PropertiesToLoad.Add("telephoneNumber");
                        mySearcher.PropertiesToLoad.Add("mobile");
                        var r = mySearcher.FindOne();

                        if (r != null)
                        {
                            if (r.Properties.Contains("mail"))
                            {
                                var email = Convert.ToString(r.Properties["mail"][0]);
                                if (!string.IsNullOrEmpty(email))
                                {
                                    u = ToUser(r);
                                }
                            }
                            else
                            {
                                u = ToUser(r);
                            }
                        }
                    }

                    return u;
                }
            }
        }

        public TUserProfile GetByAccountName(string username)
        {
            if (string.IsNullOrEmpty(username)) return null;

            var accounts = username.Split(new[] { "\\" }, StringSplitOptions.RemoveEmptyEntries);

            return SearchExactAd(accounts.Length > 1 ? accounts[1] : accounts[0]);
        }

        private bool CheckEmailFormat(string inputValue)
        {
            const string pattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|" + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)" + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";
            var regex = new Regex(pattern, RegexOptions.IgnoreCase);
            var match = regex.Match(inputValue);
            return match.Success;
        }

        [DirectoryServicesPermission(System.Security.Permissions.SecurityAction.Demand, Unrestricted = true)]
        private TUserProfile ToUser(SearchResult r)
        {
            var user = Activator.CreateInstance<TUserProfile>();

            if (r.Properties["mail"].Count > 0)
            {
                user.Email = r.Properties["mail"][0].ToString();
            }
            if (r.Properties["displayName"].Count > 0)
            {
                user.DisplayName = r.Properties["displayName"][0].ToString();
            }
            if (r.Properties["sAMAccountName"].Count > 0)
            {
                user.UserName = r.Properties["sAMAccountName"][0].ToString().ToLower();
            }

            return user;
        }
    }
}
