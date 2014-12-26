using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;

namespace DaNangZ.UserService.RoleService
{
    public class RoleService : IRoleService
    {
        public bool IsEnabled()
        {
            return Roles.Enabled;
        }

        public bool IsRoleExists(string roleName)
        {
            return Roles.RoleExists(roleName);
        }

        public IEnumerable<string> GetAllRoles()
        {
            return Array.AsReadOnly(Roles.GetAllRoles());
        }

        public void AddRole(string roleName)
        {
            Roles.CreateRole(roleName);
        }

        public bool DeleteRole(string roleName)
        {
            return Roles.DeleteRole(roleName);
        }

        public bool IsUserInRole(string username, string roleName)
        {
            return Roles.IsUserInRole(username, roleName);
        }

        public IEnumerable<string> GetUsersInRole(string roleName)
        {
            return Array.AsReadOnly(Roles.GetUsersInRole(roleName));
        }
  
        public IList<string> GetUsersInRole(string roleName, int page, int pageSize, out int total)
        {
            var list = Roles.GetUsersInRole(roleName).ToList();
            total = list.Count;
            return list.Skip(pageSize * (page - 1)).Take(pageSize).ToList();
        }
 
        public IEnumerable<string> GetRolesForUser(string username)
        {
            return Array.AsReadOnly(Roles.GetRolesForUser(username));
        }

        public IEnumerable<string> FindUsersInRole(string nameToMatch, string roleName, bool findByUsernameOrEmailOrName = false)
        {
            // TODO: Roles - How to improve performance if too many users here?
            // TODO: Roles - Implement findByUsernameOrEmailOrName
            if (findByUsernameOrEmailOrName) throw new NotImplementedException();
            return Array.AsReadOnly(Roles.FindUsersInRole(nameToMatch, roleName));
        }

        public void AddUserToRole(string username, string roleName)
        {
            Roles.AddUserToRole(username, roleName);
        }

        public void AddUsersToRoles(IEnumerable<string> usernames, IEnumerable<string> roleNames)
        {
            Roles.AddUsersToRoles(usernames.ToArray(), roleNames.ToArray());
        }

        public void RemoveUserFromRole(string username, string roleName)
        {
            Roles.RemoveUserFromRole(username, roleName);
        }

        public void RemoveUsersFromRoles(IEnumerable<string> usernames, IEnumerable<string> roleNames)
        {
            Roles.RemoveUsersFromRoles(usernames.ToArray(), roleNames.ToArray());
        }
    }
}
