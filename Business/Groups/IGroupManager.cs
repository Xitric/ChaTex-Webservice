using Business.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Groups
{
    public interface IGroupManager
    {
        /// <summary>
        /// Create a group with the specified name. The callerId is the id of the group administrator.
        /// </summary>
        /// <param name="callerId">the id of the creater of the group and thereby group administrator</param>
        /// <param name="groupName">The specified name given to the group</param>
        /// <param name="allowEmployeeSticky"></param>
        /// <param name="allowEmployeeAcknowledgeable"></param>
        /// <param name="allowEmployeeBookmark"></param>
        /// <returns></returns>
        int? CreateGroup(int callerID, string groupName, bool allowEmployeeSticky = false, bool allowEmployeeAcknowledgeable = false, bool allowEmployeeBookmark = false);

        void DeleteGroup(int groupId, int callerId);

        /// <summary>
        /// Add users to a specific group. the callerId is the id of the group administrator
        /// </summary>
        /// <param name="groupId">the Id of the specific group</param>
        /// <param name="userIds">A list of Users to be added to the Group</param>
        /// <param name="callerId">The id of the user who triggered this method</param>
        void AddUsersToGroup(int groupId, IEnumerable<int> userIds, int callerId);

        /// <summary>
        /// Remove Users from a specific group. the CallerId must be a group administrator.
        /// </summary>
        /// <param name="groupId">the Id of the specific group</param>
        /// <param name="userIds">A list of Users to be removed to the Group</param>
        /// <param name="callerId">The id of the user who triggered this method</param>
        void RemoveUsersFromGroup(int groupId, IEnumerable<int> userIds, int callerId);

        /// <summary>
        /// Add useres with a specific role to a specific group. the callerId must be a group administrator
        /// </summary>
        /// <param name="groupId">The Id of the specific group</param>
        /// <param name="roleIds">A collection of the users with a spcific role </param>
        /// <param name="callerId">The id of the user who triggered this method</param>
        void AddRolesToGroup(int groupId, IEnumerable<int> roleIds, int callerId);

        /// <summary>
        /// Removes useres with a specific role to a specific group. the callerId must be a group administrator
        /// </summary>
        /// <param name="groupId">The Id of the specific group</param>
        /// <param name="roleIds">A collection of the users with a spcific role </param>
        /// <param name="callerId">The id of the user who triggered this method</param>
        void RemoveRolesFromGroup(int groupId, IEnumerable<int> roleIds, int callerId);

        /// <summary>
        /// Sets useres with a specific id to be administrators of a specific group. the callerid must be a group administrator.
        /// </summary>
        /// <param name="groupId">The Id of the specific group</param>
        /// <param name="userId">the Id of the user to become a adminitrator</param>
        /// <param name="callerId">The id of the user who triggered this method</param>
        /// <param name="isAdministrator">A boolean to determin whether the user is an administrator</param>
        void SetUserAdministratorOnGroup(int groupId, int userId, int callerId, bool isAdministrator);

        IEnumerable<UserModel> GetAllGroupUsers(int groupId, int callerId);

        IEnumerable<UserModel> GetAllGroupAdmins(int groupId, int callerId);

        IEnumerable<GroupModel> GetGroupsForUser(int userId);

        void UpdateGroup(int groupId, string groupName, int callerId);

        IEnumerable<UserModel> GetAllDirectGroupUsers(int groupId, int callerId);

        IEnumerable<RoleModel> GetAllGroupRoles(int groupId);
    }
}
