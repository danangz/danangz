using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaNangZ.UserService.UserService
{
    public interface IUserService<TUserProfile, TUserId> where TUserId : struct
    {
        /// <summary>
        /// Gets the authentication status of the current user.
        /// </summary>
        /// <returns>true if the current user is authenticated; otherwise, false. The default is false.</returns>
        bool IsAuthenticated();

        /// <summary>
        /// Search all users by name , email.
        /// </summary>
        /// <returns>list of user profile with searched name or email</returns>
        IList<TUserProfile> Search(string term, int limit, bool userOnly = true);

        /// <summary>
        /// Get the user profile based from AD on the specified user's information
        /// </summary>
        /// <returns>The user profile.</returns>
        TUserProfile SearchExactAd(string user);

        /// <summary>
        /// Get the user profile based from AD on the specified username
        /// </summary>
        /// <returns>The user profile.</returns>
        TUserProfile GetByAccountName(string username);

        /// <summary>
        /// Gets the ID for the current user.
        /// </summary>
        /// <returns>The ID for the current user.</returns>
        TUserId GetCurrentUserId();

        /// <summary>
        /// Gets the profile for the current user.
        /// </summary>
        /// <returns>The profile for the current user.</returns>
        TUserProfile GetCurrentUser();

        /// <summary>
        /// Get the user profile based on the specified user id.
        /// </summary>
        /// <param name="id">The user id</param>
        /// <returns>The user profile</returns>
        TUserProfile GetUserById(TUserId id);

        /// <summary>
        /// Get the name of the user based on the user id.
        /// </summary>
        /// <param name="id">User id</param>
        /// <returns>The Name of user from IUserProfile</returns>
        string GetNameById(TUserId id);

        /// <summary>
        /// Get the username of the user based on the user id.
        /// </summary>
        /// <param name="id">User id</param>
        /// <returns>The username of user from IUserProfile</returns>
        string GetUsernameById(TUserId id);

        /// <summary>
        /// Get the user id based on the username.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns>The user id.</returns>
        TUserId GetUserIdByUsername(string username);

        /// <summary>
        /// Add new user profile. Creating user profile is required for any IRoleService to function correctly. Useful after using IFormUserService.Register method.
        /// </summary>
        /// <param name="userProfile">The user profile information</param>
        void AddUser(TUserProfile userProfile);

        /// <summary>
        /// Update the user profile
        /// </summary>
        /// <param name="userProfile"></param>
        /// <returns></returns>
        TUserProfile UpdateUser(TUserProfile userProfile);

        /// <summary>
        /// Remove user profile. Make sure the user does not have any role associated.
        /// </summary>
        /// <param name="userId">The user id.</param>
        void RemoveUser(TUserId userId);

        IList<string> GetUserNameByComponentId(int componentId);

        IList<string> GetEmails(List<string> username);

        IList<TUserProfile> GetUsers(int pageNo, int pageSize, out int totalRecords);

        IList<TUserProfile> GetAllUsersInMembership(string userName);

        TUserProfile GetUserByUserName(string userName);
    }
}
