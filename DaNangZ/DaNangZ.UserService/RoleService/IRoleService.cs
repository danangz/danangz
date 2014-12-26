using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaNangZ.UserService.RoleService
{
    public interface IRoleService
    {
        bool IsEnabled();

        bool IsRoleExists(string roleName);

        IEnumerable<string> GetAllRoles();
        
        void AddRole(string roleName);
        
        bool DeleteRole(string roleName);

        bool IsUserInRole(string username, string roleName);
        
        IEnumerable<string> GetUsersInRole(string roleName);
        
        IList<string> GetUsersInRole(string roleName, int page, int pageSize, out int total);
        
        IEnumerable<string> GetRolesForUser(string username);
        
        IEnumerable<string> FindUsersInRole(string nameToMatch, string roleName, bool findByUsernameOrEmailOrName = false);
        
        void AddUserToRole(string username, string roleName);
        
        void AddUsersToRoles(IEnumerable<string> usernames, IEnumerable<string> roleNames);
        
        void RemoveUserFromRole(string username, string roleName);
        
        void RemoveUsersFromRoles(IEnumerable<string> usernames, IEnumerable<string> roleNames);
    }
}
